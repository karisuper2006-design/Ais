using System.Collections.Generic;
using System.Linq;
using BookManagementSystem.Domain.Abstractions;

namespace BookManagementSystem.Domain.Entities;

public class Book : IDomainObject
{
    /// <summary>
    /// Уникальный идентификатор книги.
    /// </summary>
    public int ID { get; set; }

    /// <summary>
    /// Название книги.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Автор книги.
    /// </summary>
    public string Author { get; set; } = string.Empty;

    /// <summary>
    /// Год издания книги.
    /// </summary>
    public int Year { get; set; }

    /// <summary>
    /// Список жанров, к которым относится книга.
    /// </summary>
    public List<Genre> Genres { get; set; } = new();

    /// <summary>
    /// Строковое представление списка жанров для отображения.
    /// Возвращает список названий жанров через запятую или "Без жанра", если список пуст.
    /// </summary>
    public string GenresDisplay => Genres.Any()
        ? string.Join(", ", Genres.Select(g => g.Name))
        : "Без жанра";

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public Book()
    {
    }

    /// <summary>
    /// Конструктор с параметрами для создания новой книги.
    /// </summary>
    /// <param name="title">Название книги.</param>
    /// <param name="author">Автор книги.</param>
    /// <param name="year">Год издания.</param>
    /// <param name="genres">Список названий жанров.</param>
    public Book(string title, string author, int year, IEnumerable<string> genres)
    {
        Title = title;
        Author = author;
        Year = year;
        Genres = genres
            .Where(g => !string.IsNullOrWhiteSpace(g))
            .Select(name => new Genre { Name = name.Trim() })
            .ToList();
    }

    /// <summary>
    /// Возвращает строковое представление объекта книги.
    /// </summary>
    /// <returns>Строка с информацией о книге.</returns>
    public override string ToString()
    {
        return $"ID: {ID}, Название: {Title}, Автор: {Author}, Год: {Year}, Жанры: {GenresDisplay}";
    }
}
