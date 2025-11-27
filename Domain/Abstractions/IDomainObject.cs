namespace BookManagementSystem.Domain.Abstractions;

/// <summary>
/// Интерфейс для доменных объектов, имеющих идентификатор.
/// </summary>
public interface IDomainObject
{
    /// <summary>
    /// Уникальный идентификатор объекта.
    /// </summary>
    int ID { get; set; }
}
