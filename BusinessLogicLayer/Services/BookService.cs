using System;
using System.Collections.Generic;
using System.Linq;
using BookManagementSystem.Domain.Entities;
using BookManagementSystem.Domain.Repositories;

namespace BookManagementSystem.BusinessLogicLayer.Services;

/// <summary>
/// Сервис для управления книгами.
/// Содержит бизнес-логику работы с книгами и взаимодействует с репозиторием.
/// </summary>
public class BookService
{
    private readonly IBookRepository _repository;

    /// <summary>
    /// Инициализирует новый экземпляр сервиса книг.
    /// </summary>
    /// <param name="repository">Репозиторий книг.</param>
    public BookService(IBookRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Получает список всех книг.
    /// </summary>
    /// <returns>Коллекция всех книг, отсортированная по ID.</returns>
    public IReadOnlyCollection<Book> GetAllBooks()
    {
        return _repository
            .GetAll()
            .OrderBy(b => b.ID)
            .ToList();
    }

    /// <summary>
    /// Создает новую книгу.
    /// </summary>
    /// <param name="title">Название книги.</param>
    /// <param name="author">Автор книги.</param>
    /// <param name="year">Год издания.</param>
    /// <param name="genres">Список жанров.</param>
    /// <returns>Созданная книга.</returns>
    public Book CreateBook(string title, string author, int year, IEnumerable<string> genres)
    {
        var book = new Book(title, author, year, genres);
        return _repository.Add(book);
    }

    /// <summary>
    /// Удаляет книгу по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор удаляемой книги.</param>
    /// <returns>True, если удаление прошло успешно.</returns>
    public bool DeleteBook(int id)
    {
        return _repository.Delete(id);
    }

    /// <summary>
    /// Получает книгу по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор книги.</param>
    /// <returns>Книга или null, если не найдена.</returns>
    public Book? GetBookById(int id)
    {
        return _repository.GetById(id);
    }

    /// <summary>
    /// Обновляет существующую книгу.
    /// </summary>
    /// <param name="id">Идентификатор книги.</param>
    /// <param name="title">Новое название.</param>
    /// <param name="author">Новый автор.</param>
    /// <param name="year">Новый год издания.</param>
    /// <param name="genres">Новый список жанров.</param>
    /// <returns>True, если обновление прошло успешно.</returns>
    public bool UpdateBook(int id, string title, string author, int year, IEnumerable<string> genres)
    {
        var book = new Book(title, author, year, genres) { ID = id };
        return _repository.Update(book);
    }

    /// <summary>
    /// Ищет книги по автору.
    /// </summary>
    /// <param name="author">Имя автора или его часть.</param>
    /// <returns>Коллекция найденных книг.</returns>
    public IReadOnlyCollection<Book> FindBooksByAuthor(string author)
    {
        if (string.IsNullOrWhiteSpace(author))
        {
            return GetAllBooks();
        }

        return _repository
            .FindByAuthor(author)
            .ToList();
    }

    /// <summary>
    /// Получает предложения для автодополнения имени автора.
    /// </summary>
    /// <param name="authorFragment">Фрагмент имени автора.</param>
    /// <returns>Список уникальных имен авторов, содержащих фрагмент.</returns>
    public IReadOnlyCollection<string> GetAuthorSuggestions(string authorFragment)
    {
        var comparer = StringComparer.CurrentCultureIgnoreCase;
        IEnumerable<string> authors = string.IsNullOrWhiteSpace(authorFragment)
            ? _repository.GetAll().Select(b => b.Author)
            : _repository.FindByAuthor(authorFragment).Select(b => b.Author);

        return authors
            .Where(name => !string.IsNullOrWhiteSpace(name))
            .Distinct(comparer)
            .OrderBy(name => name, comparer)
            .ToList();
    }

    /// <summary>
    /// Ищет книги по названию.
    /// </summary>
    /// <param name="title">Название книги или его часть.</param>
    /// <returns>Коллекция найденных книг.</returns>
    public IReadOnlyCollection<Book> FindBooksByTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return GetAllBooks();
        }

        return _repository
            .FindByTitle(title)
            .ToList();
    }

    /// <summary>
    /// Получает список всех жанров.
    /// </summary>
    /// <returns>Коллекция названий жанров, отсортированная по алфавиту.</returns>
    public IReadOnlyCollection<string> GetAllGenres()
    {
        return _repository
            .GetAllGenres()
            .OrderBy(g => g, StringComparer.CurrentCultureIgnoreCase)
            .ToList();
    }

    /// <summary>
    /// Группирует книги по жанрам.
    /// Книги без жанра попадают в группу "Без жанра".
    /// Книги с несколькими жанрами попадают в каждую соответствующую группу.
    /// </summary>
    /// <returns>Словарь, где ключ - название жанра, а значение - список книг.</returns>
    public IReadOnlyDictionary<string, IReadOnlyCollection<Book>> GroupBooksByGenre()
    {
        var books = _repository.GetAll();

        var items = books.SelectMany(book =>
            book.Genres.Any()
                ? book.Genres.Select(g => (Genre: g.Name, Book: book))
                : new[] { (Genre: "Без жанра", Book: book) });

        return items
            .GroupBy(item => string.IsNullOrWhiteSpace(item.Genre) ? "Без жанра" : item.Genre)
            .ToDictionary(
                g => g.Key,
                g => (IReadOnlyCollection<Book>)g.Select(item => item.Book).Distinct().ToList());
    }
}

