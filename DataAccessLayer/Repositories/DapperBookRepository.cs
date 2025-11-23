using System;
using System.Collections.Generic;
using System.Linq;
using BookManagementSystem.DataAccessLayer.Seeding;
using BookManagementSystem.Domain.Entities;
using BookManagementSystem.Domain.Repositories;
using Dapper;
using Microsoft.Data.Sqlite;

namespace BookManagementSystem.DataAccessLayer.Repositories;

public class DapperBookRepository : IBookRepository
{
    private readonly string _connectionString;

    public DapperBookRepository(string connectionString)
    {
        _connectionString = connectionString;
        DbInitializer.EnsureCreated(connectionString);
    }

    public Book Add(Book entity)
    {
        const string sql = """
            INSERT INTO Books (Title, Author, Year)
            VALUES (@Title, @Author, @Year);
            SELECT last_insert_rowid();
            """;

        using var connection = CreateConnection();
        using var transaction = connection.BeginTransaction();

        var id = connection.ExecuteScalar<long>(sql, entity, transaction);
        entity.ID = (int)id;

        ReplaceGenres(connection, transaction, entity);
        transaction.Commit();

        return entity;
    }

    public bool Delete(int id)
    {
        const string sql = "DELETE FROM Books WHERE ID = @ID";
        using var connection = CreateConnection();
        var affected = connection.Execute(sql, new { ID = id });
        return affected > 0;
    }

    public IEnumerable<Book> FindByAuthor(string author)
    {
        if (string.IsNullOrWhiteSpace(author))
        {
            return Enumerable.Empty<Book>();
        }

        var needle = author.Trim();
        var comparison = StringComparison.CurrentCultureIgnoreCase;

        return GetAll()
            .Where(b => !string.IsNullOrWhiteSpace(b.Author) &&
                        b.Author.Contains(needle, comparison))
            .OrderBy(b => b.Title)
            .ToList();
    }

    public IEnumerable<Book> FindByTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return Enumerable.Empty<Book>();
        }

        var needle = title.Trim();
        var comparison = StringComparison.CurrentCultureIgnoreCase;

        return GetAll()
            .Where(b => !string.IsNullOrWhiteSpace(b.Title) &&
                        b.Title.Contains(needle, comparison))
            .OrderBy(b => b.Title)
            .ToList();
    }

    public IEnumerable<Book> GetAll()
    {
        const string sql = "SELECT ID, Title, Author, Year FROM Books ORDER BY ID";
        using var connection = CreateConnection();
        var books = connection.Query<Book>(sql).ToList();
        AttachGenres(connection, books);
        return books;
    }

    public Book? GetById(int id)
    {
        const string sql = "SELECT ID, Title, Author, Year FROM Books WHERE ID = @ID";
        using var connection = CreateConnection();
        var book = connection.QuerySingleOrDefault<Book>(sql, new { ID = id });
        if (book is null)
        {
            return null;
        }

        AttachGenres(connection, new[] { book });
        return book;
    }

    public bool Update(Book entity)
    {
        const string sql = """
            UPDATE Books
            SET Title = @Title,
                Author = @Author,
                Year = @Year
            WHERE ID = @ID
            """;

        using var connection = CreateConnection();
        using var transaction = connection.BeginTransaction();

        var affected = connection.Execute(sql, entity, transaction);
        if (affected == 0)
        {
            transaction.Rollback();
            return false;
        }

        ReplaceGenres(connection, transaction, entity);
        transaction.Commit();
        return true;
    }

    public IReadOnlyCollection<string> GetAllGenres()
    {
        const string sql = "SELECT Name FROM Genres ORDER BY Name";
        using var connection = CreateConnection();
        return connection.Query<string>(sql)
            .Where(name => !string.IsNullOrWhiteSpace(name))
            .Select(name => name.Trim())
            .Distinct(StringComparer.CurrentCultureIgnoreCase)
            .ToList();
    }

    private SqliteConnection CreateConnection()
    {
        var connection = new SqliteConnection(_connectionString);
        connection.Open();
        return connection;
    }

    private static IEnumerable<string> ExtractGenreNames(Book entity)
    {
        return entity.Genres
            .Where(g => !string.IsNullOrWhiteSpace(g.Name))
            .Select(g => g.Name.Trim());
    }

    private static void ReplaceGenres(SqliteConnection connection, SqliteTransaction transaction, Book entity)
    {
        const string deleteSql = "DELETE FROM BookGenres WHERE BookId = @BookId";
        const string insertSql = "INSERT INTO BookGenres (BookId, GenreId) VALUES (@BookId, @GenreId)";

        connection.Execute(deleteSql, new { BookId = entity.ID }, transaction);

        var names = ExtractGenreNames(entity).ToList();
        if (!names.Any())
        {
            return;
        }

        var genreIds = EnsureGenreIds(connection, transaction, names);
        foreach (var name in names)
        {
            if (!genreIds.TryGetValue(name, out var genreId))
            {
                continue;
            }

            connection.Execute(insertSql, new { BookId = entity.ID, GenreId = genreId }, transaction);
        }
    }

    private static void AttachGenres(SqliteConnection connection, IEnumerable<Book> books)
    {
        var list = books.ToList();
        if (!list.Any())
        {
            return;
        }

        const string sql = """
            SELECT g.ID, g.Name, bg.BookId
            FROM BookGenres bg
            INNER JOIN Genres g ON g.ID = bg.GenreId
            WHERE bg.BookId IN @Ids
            ORDER BY g.Name
            """;

        var genres = connection.Query<(int Id, string Name, int BookId)>(sql, new { Ids = list.Select(b => b.ID).ToArray() }).ToList();
        var lookup = genres.GroupBy(g => g.BookId)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => new Genre { ID = x.Id, Name = x.Name }).ToList());

        foreach (var book in list)
        {
            if (lookup.TryGetValue(book.ID, out var bookGenres))
            {
                book.Genres = bookGenres;
            }
            else
            {
                book.Genres = new List<Genre>();
            }
        }
    }

    private static Dictionary<string, int> EnsureGenreIds(SqliteConnection connection, SqliteTransaction transaction, IEnumerable<string> names)
    {
        var normalized = names
            .Where(name => !string.IsNullOrWhiteSpace(name))
            .Select(name => name.Trim())
            .Distinct(StringComparer.CurrentCultureIgnoreCase)
            .ToList();

        if (!normalized.Any())
        {
            return new Dictionary<string, int>(StringComparer.CurrentCultureIgnoreCase);
        }

        const string insertSql = "INSERT INTO Genres (Name) VALUES (@Name) ON CONFLICT(Name) DO NOTHING;";
        foreach (var name in normalized)
        {
            connection.Execute(insertSql, new { Name = name }, transaction);
        }

        const string selectSql = "SELECT ID, Name FROM Genres WHERE Name IN @Names;";
        return connection.Query<(int Id, string Name)>(selectSql, new { Names = normalized }, transaction)
            .ToDictionary(x => x.Name, x => x.Id, StringComparer.CurrentCultureIgnoreCase);
    }

}
