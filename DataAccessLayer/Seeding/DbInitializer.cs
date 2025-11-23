using System.Collections.Generic;
using System.Linq;
using BookManagementSystem.DataAccessLayer.Contexts;
using BookManagementSystem.Domain.Entities;

namespace BookManagementSystem.DataAccessLayer.Seeding;

public static class DbInitializer
{
    public static void EnsureCreated(BookDbContext context)
    {
        context.Database.EnsureCreated();

        if (context.Books.Any())
        {
            return;
        }

        var genreCache = new Dictionary<string, Genre>(StringComparer.CurrentCultureIgnoreCase);

        var seedBooks = new[]
        {
            new { Title = "1984", Author = "Джордж Оруэлл", Year = 1949, Genres = new[] { "Антиутопия", "Дистопия" } },
            new { Title = "Преступление и наказание", Author = "Фёдор Достоевский", Year = 1866, Genres = new[] { "Роман" } },
            new { Title = "Властелин колец", Author = "Джон Толкин", Year = 1954, Genres = new[] { "Фэнтези", "Приключения" } },
            new { Title = "Мастер и Маргарита", Author = "Михаил Булгаков", Year = 1966, Genres = new[] { "Мистика", "Роман" } },
            new { Title = "Три товарища", Author = "Эрих Мария Ремарк", Year = 1936, Genres = new[] { "Драма" } }
        }
        .Select(b =>
        {
            var book = new Book(b.Title, b.Author, b.Year, Array.Empty<string>());
            foreach (var name in b.Genres)
            {
                if (!genreCache.TryGetValue(name, out var genre))
                {
                    genre = new Genre { Name = name };
                    genreCache[name] = genre;
                }

                book.Genres.Add(genre);
            }

            return book;
        })
        .ToList();

        context.Books.AddRange(seedBooks);
        context.SaveChanges();
    }

    public static void EnsureCreated(string connectionString)
    {
        using var context = BookDbContextFactory.Create(connectionString, ensureDatabase: false);
        EnsureCreated(context);
    }
}
