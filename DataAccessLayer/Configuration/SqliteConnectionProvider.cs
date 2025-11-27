using System;
using System.IO;

namespace BookManagementSystem.DataAccessLayer.Configuration;

/// <summary>
/// Провайдер для получения строки подключения к базе данных SQLite.
/// </summary>
public static class SqliteConnectionProvider
{
    /// <summary>
    /// Возвращает строку подключения по умолчанию.
    /// Создает директорию для базы данных, если она не существует.
    /// </summary>
    /// <returns>Строка подключения к SQLite.</returns>
    public static string GetDefaultConnectionString()
    {
        var baseDirectory = AppContext.BaseDirectory;
        var storageFolder = Path.Combine(baseDirectory, "Data");
        Directory.CreateDirectory(storageFolder);
        var databasePath = Path.Combine(storageFolder, "books_v3.db");
        return $"Data Source={databasePath}";
    }
}
