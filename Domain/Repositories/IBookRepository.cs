using System.Collections.Generic;
using BookManagementSystem.Domain.Entities;

namespace BookManagementSystem.Domain.Repositories;

/// <summary>
/// Интерфейс репозитория книг. Расширяет базовый репозиторий специфичными методами.
/// </summary>
public interface IBookRepository : IRepository<Book>
{
    /// <summary>
    /// Ищет книги по автору.
    /// </summary>
    /// <param name="author">Имя автора или его часть.</param>
    /// <returns>Коллекция найденных книг.</returns>
    IEnumerable<Book> FindByAuthor(string author);

    /// <summary>
    /// Ищет книги по названию.
    /// </summary>
    /// <param name="title">Название книги или его часть.</param>
    /// <returns>Коллекция найденных книг.</returns>
    IEnumerable<Book> FindByTitle(string title);

    /// <summary>
    /// Получает список всех уникальных жанров, используемых в книгах.
    /// </summary>
    /// <returns>Коллекция названий жанров.</returns>
    IReadOnlyCollection<string> GetAllGenres();
}
