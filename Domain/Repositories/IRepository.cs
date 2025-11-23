using System.Collections.Generic;
using BookManagementSystem.Domain.Abstractions;

namespace BookManagementSystem.Domain.Repositories;

public interface IRepository<T>
    where T : IDomainObject
{
    IEnumerable<T> GetAll();
    T? GetById(int id);
    T Add(T entity);
    bool Update(T entity);
    bool Delete(int id);
}
