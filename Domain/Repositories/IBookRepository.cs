using System.Collections.Generic;
using BookManagementSystem.Domain.Entities;

namespace BookManagementSystem.Domain.Repositories;

public interface IBookRepository : IRepository<Book>
{
    IEnumerable<Book> FindByAuthor(string author);
    IEnumerable<Book> FindByTitle(string title);
    IReadOnlyCollection<string> GetAllGenres();
}
