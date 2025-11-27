using System.Collections.Generic;
using BookManagementSystem.Domain.Abstractions;

namespace BookManagementSystem.Domain.Repositories;

/// <summary>
/// Обобщенный интерфейс репозитория для выполнения CRUD операций.
/// </summary>
/// <typeparam name="T">Тип доменного объекта, реализующего IDomainObject.</typeparam>
public interface IRepository<T>
    where T : IDomainObject
{
    /// <summary>
    /// Получает все сущности из репозитория.
    /// </summary>
    /// <returns>Коллекция всех сущностей.</returns>
    IEnumerable<T> GetAll();

    /// <summary>
    /// Получает сущность по её идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор сущности.</param>
    /// <returns>Сущность или null, если не найдена.</returns>
    T? GetById(int id);

    /// <summary>
    /// Добавляет новую сущность в репозиторий.
    /// </summary>
    /// <param name="entity">Сущность для добавления.</param>
    /// <returns>Добавленная сущность (возможно с обновленным ID).</returns>
    T Add(T entity);

    /// <summary>
    /// Обновляет существующую сущность в репозитории.
    /// </summary>
    /// <param name="entity">Сущность с обновленными данными.</param>
    /// <returns>True, если обновление прошло успешно; иначе False.</returns>
    bool Update(T entity);

    /// <summary>
    /// Удаляет сущность из репозитория по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор удаляемой сущности.</param>
    /// <returns>True, если удаление прошло успешно; иначе False.</returns>
    bool Delete(int id);
}
