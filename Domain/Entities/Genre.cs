using System.Collections.Generic;
using BookManagementSystem.Domain.Abstractions;

namespace BookManagementSystem.Domain.Entities;

public class Genre : IDomainObject
{
    /// <summary>
    /// Уникальный идентификатор жанра.
    /// </summary>
    public int ID { get; set; }

    /// <summary>
    /// Название жанра.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Коллекция книг, относящихся к данному жанру.
    /// </summary>
    public ICollection<Book> Books { get; set; } = new List<Book>();
}
