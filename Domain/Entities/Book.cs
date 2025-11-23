using System.Collections.Generic;
using System.Linq;
using BookManagementSystem.Domain.Abstractions;

namespace BookManagementSystem.Domain.Entities;

public class Book : IDomainObject
{
    public int ID { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int Year { get; set; }
    public List<Genre> Genres { get; set; } = new();

    public string GenresDisplay => Genres.Any()
        ? string.Join(", ", Genres.Select(g => g.Name))
        : "Без жанра";

    public Book()
    {
    }

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

    public override string ToString()
    {
        return $"ID: {ID}, Название: {Title}, Автор: {Author}, Год: {Year}, Жанры: {GenresDisplay}";
    }
}
