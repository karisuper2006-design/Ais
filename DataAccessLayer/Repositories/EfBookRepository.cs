using System;
using System.Collections.Generic;
using System.Linq;
using BookManagementSystem.DataAccessLayer.Contexts;
using BookManagementSystem.Domain.Entities;
using BookManagementSystem.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BookManagementSystem.DataAccessLayer.Repositories;

public class EfBookRepository : IBookRepository
{
    private readonly BookDbContext _context;

    /// <summary>
    /// Инициализирует новый экземпляр репозитория Entity Framework.
    /// </summary>
    /// <param name="context">Контекст базы данных.</param>
    public EfBookRepository(BookDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Добавляет новую книгу в базу данных через EF Core.
    /// </summary>
    /// <param name="entity">Книга для добавления.</param>
    /// <returns>Добавленная книга.</returns>
    public Book Add(Book entity)
    {
        // Разрешаем дубликаты жанров перед добавлением
        entity.Genres = ResolveGenres(entity.Genres.Select(g => g.Name));
        _context.Books.Add(entity);
        _context.SaveChanges();
        return entity;
    }

    /// <summary>
    /// Удаляет книгу по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор книги.</param>
    /// <returns>True, если книга была удалена.</returns>
    public bool Delete(int id)
    {
        var book = _context.Books.Find(id);
        if (book is null)
        {
            return false;
        }

        _context.Books.Remove(book);
        _context.SaveChanges();
        return true;
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

        return _context.Books
            .AsNoTracking()
            .Include(b => b.Genres)
            .ToList()
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

        return _context.Books
            .AsNoTracking()
            .Include(b => b.Genres)
            .ToList()
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
        return _context.Books
            .AsNoTracking()
            .Include(b => b.Genres)
            .OrderBy(b => b.ID)
            .ToList();
    }

    /// <summary>
    /// Получает книгу по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор книги.</param>
    /// <returns>Книга или null, если не найдена.</returns>
    public Book? GetById(int id)
    {
        return _context.Books
            .AsNoTracking()
            .Include(b => b.Genres)
            .FirstOrDefault(b => b.ID == id);
    }

    /// <summary>
    /// Обновляет данные книги.
    /// </summary>
    /// <param name="entity">Книга с обновленными данными.</param>
    /// <returns>True, если обновление прошло успешно.</returns>
    public bool Update(Book entity)
    {
        var existing = _context.Books.Find(entity.ID);
        if (existing is null)
        {
            return false;
        }

        existing.Title = entity.Title;
        existing.Author = entity.Author;
        existing.Year = entity.Year;
        UpdateGenres(existing, entity);

        _context.SaveChanges();
        return true;
    }

    /// <summary>
    /// Обновляет список жанров книги, синхронизируя его с базой данных.
    /// </summary>
    private void UpdateGenres(Book target, Book source)
    {
        _context.Entry(target).Collection(b => b.Genres).Load();
        target.Genres.Clear();
        var genres = ResolveGenres(source.Genres.Select(g => g.Name));
        foreach (var genre in genres)
        {
            target.Genres.Add(genre);
        }
    }

    /// <summary>
    /// Находит существующие жанры в базе или создает новые.
    /// </summary>
    /// <param name="genres">Список названий жанров.</param>
    /// <returns>Список сущностей жанров.</returns>
    private List<Genre> ResolveGenres(IEnumerable<string> genres)
    {
        var normalized = genres
            .Where(name => !string.IsNullOrWhiteSpace(name))
            .Select(name => name.Trim())
            .Distinct(StringComparer.CurrentCultureIgnoreCase)
            .ToList();

        if (!normalized.Any())
        {
            return new List<Genre>();
        }

        var allGenres = _context.Genres.ToList();
        var result = new List<Genre>();

        foreach (var name in normalized)
        {
            var genre = allGenres
                .FirstOrDefault(g => string.Equals(g.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (genre is null)
            {
                genre = new Genre { Name = name };
                allGenres.Add(genre);
                _context.Genres.Add(genre);
            }

            result.Add(genre);
        }

        return result;
    }

    /// <summary>
    /// Получает список всех уникальных жанров.
    /// </summary>
    /// <returns>Список названий жанров.</returns>
    public IReadOnlyCollection<string> GetAllGenres()
    {
        var names = _context.Genres
            .AsNoTracking()
            .Select(g => g.Name)
            .ToList();

        return names
            .Where(name => !string.IsNullOrWhiteSpace(name))
            .Select(name => name.Trim())
            .Distinct(StringComparer.CurrentCultureIgnoreCase)
            .OrderBy(name => name, StringComparer.CurrentCultureIgnoreCase)
            .ToList();
    }
}
