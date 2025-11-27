using BookManagementSystem.DataAccessLayer.Seeding;
using Microsoft.EntityFrameworkCore;

namespace BookManagementSystem.DataAccessLayer.Contexts;

/// <summary>
/// Фабрика для создания экземпляров контекста базы данных.
/// </summary>
public static class BookDbContextFactory
{
    /// <summary>
    /// Создает новый экземпляр BookDbContext.
    /// </summary>
    /// <param name="connectionString">Строка подключения к базе данных.</param>
    /// <param name="ensureDatabase">Если true, проверяет существование базы данных и создает её при необходимости.</param>
    /// <returns>Настроенный экземпляр BookDbContext.</returns>
    public static BookDbContext Create(string connectionString, bool ensureDatabase = true)
    {
        var options = new DbContextOptionsBuilder<BookDbContext>()
            .UseSqlite(connectionString)
            .Options;

        var context = new BookDbContext(options);

        if (ensureDatabase)
        {
            DbInitializer.EnsureCreated(context);
        }

        return context;
    }
}
