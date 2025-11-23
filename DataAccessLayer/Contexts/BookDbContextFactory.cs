using BookManagementSystem.DataAccessLayer.Seeding;
using Microsoft.EntityFrameworkCore;

namespace BookManagementSystem.DataAccessLayer.Contexts;

public static class BookDbContextFactory
{
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
