using System;
using System.IO;

namespace BookManagementSystem.DataAccessLayer.Configuration;

public static class SqliteConnectionProvider
{
    public static string GetDefaultConnectionString()
    {
        var baseDirectory = AppContext.BaseDirectory;
        var storageFolder = Path.Combine(baseDirectory, "Data");
        Directory.CreateDirectory(storageFolder);
        var databasePath = Path.Combine(storageFolder, "books_v3.db");
        return $"Data Source={databasePath}";
    }
}
