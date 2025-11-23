using System.Collections.Generic;
using BookManagementSystem.Domain.Abstractions;

namespace BookManagementSystem.Domain.Entities;

public class Genre : IDomainObject
{
    public int ID { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<Book> Books { get; set; } = new List<Book>();
}
