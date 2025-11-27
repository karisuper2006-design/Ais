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

    /// <summary>
    /// Инициализирует новый экземпляр репозитория Dapper.
    /// </summary>
    /// <param name="connectionString">Строка подключения к базе данных.</param>
    public DapperBookRepository(string connectionString)
    {
        _connectionString = connectionString;
        // Убеждаемся, что база данных создана перед началом работы
        DbInitializer.EnsureCreated(connectionString);
    }

    /// <summary>
    /// Добавляет новую книгу в базу данных.
    /// </summary>
    /// <param name="entity">Книга для добавления.</param>
    /// <returns>Добавленная книга с присвоенным ID.</returns>
    public Book Add(Book entity)
    {
        const string sql = """
            INSERT INTO Books (Title, Author, Year)
            VALUES (@Title, @Author, @Year);
            SELECT last_insert_rowid();
            """;

        using var connection = CreateConnection();
        using var transaction = connection.BeginTransaction();

        // Вставка книги и получение ID
        var id = connection.ExecuteScalar<long>(sql, entity, transaction);
        entity.ID = (int)id;

        // Сохранение жанров
        ReplaceGenres(connection, transaction, entity);
        transaction.Commit();

        return entity;
    }

    /// <summary>
    /// Удаляет книгу по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор книги.</param>
    /// <returns>True, если книга была удалена.</returns>
    public bool Delete(int id)
    {
        const string sql = "DELETE FROM Books WHERE ID = @ID";
        using var connection = CreateConnection();
        var affected = connection.Execute(sql, new { ID = id });
        return affected > 0;
    }

    /// <summary>
    /// Ищет книги по автору.
    /// </summary>
    /// <param name="author">Имя автора (частичное совпадение).</param>
    /// <returns>Список найденных книг.</returns>
    public IEnumerable<Book> FindByAuthor(string author)
    {
        if (string.IsNullOrWhiteSpace(author))
        {
            return Enumerable.Empty<Book>();
        }

        var needle = author.Trim();
        var comparison = StringComparison.CurrentCultureIgnoreCase;

        // Фильтрация на стороне клиента, так как SQLite LIKE может быть чувствителен к регистру (зависит от настроек)
        // Для простоты загружаем все и фильтруем в памяти
        return GetAll()
            .Where(b => !string.IsNullOrWhiteSpace(b.Author) &&
                        b.Author.Contains(needle, comparison))
            .OrderBy(b => b.Title)
            .ToList();
    }

    /// <summary>
    /// Ищет книги по названию.
    /// </summary>
    /// <param name="title">Название книги (частичное совпадение).</param>
    /// <returns>Список найденных книг.</returns>
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

    /// <summary>
    /// Получает все книги из базы данных.
    /// </summary>
    /// <returns>Список всех книг.</returns>
    public IEnumerable<Book> GetAll()
    {
        const string sql = "SELECT ID, Title, Author, Year FROM Books ORDER BY ID";
        using var connection = CreateConnection();
        var books = connection.Query<Book>(sql).ToList();
        AttachGenres(connection, books);
        return books;
    }

    /// <summary>
    /// Получает книгу по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор книги.</param>
    /// <returns>Книга или null, если не найдена.</returns>
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

    /// <summary>
    /// Обновляет данные книги.
    /// </summary>
    /// <param name="entity">Книга с обновленными данными.</param>
    /// <returns>True, если обновление прошло успешно.</returns>
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

    /// <summary>
    /// Получает список всех уникальных жанров.
    /// </summary>
    /// <returns>Список названий жанров.</returns>
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

    /// <summary>
    /// Заменяет жанры книги в базе данных (удаляет старые связи и добавляет новые).
    /// </summary>
    private static void ReplaceGenres(SqliteConnection connection, SqliteTransaction transaction, Book entity)
    {
        const string deleteSql = "DELETE FROM BookGenres WHERE BookId = @BookId";
        const string insertSql = "INSERT INTO BookGenres (BookId, GenreId) VALUES (@BookId, @GenreId)";

        // Удаляем существующие связи
        connection.Execute(deleteSql, new { BookId = entity.ID }, transaction);

        var names = ExtractGenreNames(entity).ToList();
        if (!names.Any())
        {
            return;
        }

        // Получаем или создаем ID жанров
        var genreIds = EnsureGenreIds(connection, transaction, names);
        foreach (var name in names)
        {
            if (!genreIds.TryGetValue(name, out var genreId))
            {
                continue;
            }

            // Создаем новые связи
            connection.Execute(insertSql, new { BookId = entity.ID, GenreId = genreId }, transaction);
        }
    }

    /// <summary>
    /// Загружает и привязывает жанры к списку книг.
    /// </summary>
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

    /// <summary>
    /// Обеспечивает существование жанров в базе данных и возвращает их ID.
    /// </summary>
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
