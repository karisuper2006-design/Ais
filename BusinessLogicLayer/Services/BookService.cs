using System;
using System.Collections.Generic;
using System.Linq;
using BookManagementSystem.Domain.Entities;
using BookManagementSystem.Domain.Repositories;

namespace BookManagementSystem.BusinessLogicLayer.Services;

public class BookService
{
    private readonly IBookRepository _repository;

    public BookService(IBookRepository repository)
    {
        _repository = repository;
    }

    public IReadOnlyCollection<Book> GetAllBooks()
    {
        return _repository
            .GetAll()
            .OrderBy(b => b.ID)
            .ToList();
    }

    public Book CreateBook(string title, string author, int year, IEnumerable<string> genres)
    {
        var book = new Book(title, author, year, genres);
        return _repository.Add(book);
    }

    public bool DeleteBook(int id)
    {
        return _repository.Delete(id);
    }

    public Book? GetBookById(int id)
    {
        return _repository.GetById(id);
    }

    public bool UpdateBook(int id, string title, string author, int year, IEnumerable<string> genres)
    {
        var book = new Book(title, author, year, genres) { ID = id };
        return _repository.Update(book);
    }

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

    public IReadOnlyCollection<string> GetAllGenres()
    {
        return _repository
            .GetAllGenres()
            .OrderBy(g => g, StringComparer.CurrentCultureIgnoreCase)
            .ToList();
    }

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

