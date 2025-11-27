using BookManagementSystem.BusinessLogicLayer.Services;
using BookManagementSystem.BusinessLogicLayer;
using Ninject;
using BookManagementSystem.DataAccessLayer.Configuration;
using BookManagementSystem.DataAccessLayer.Contexts;
using BookManagementSystem.DataAccessLayer.Repositories;
using BookManagementSystem.Domain.Entities;
using BookManagementSystem.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookManagementSystem.PresentationLayer;

internal static class Program
{
    /// <summary>
    /// Перечисление доступных поставщиков данных.
    /// </summary>
    private enum DataProvider
    {
        EntityFramework = 1,
        Dapper = 2
    }

    /// <summary>
    /// Точка входа в консольное приложение.
    /// </summary>
    private static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        // Настройка IoC контейнера
        // По умолчанию используем EF Core, но можно было бы спросить пользователя
        // В текущей реализации выбор провайдера захардкожен в SimpleConfigModule или выбирается через AskForProvider (если бы он использовался)
        // Здесь мы просто создаем ядро с конфигурацией по умолчанию
        IKernel ninjectKernel = new StandardKernel(new SimpleConfigModule());
        var service = ninjectKernel.Get<BookService>();

        RunMenu(service);
    }

    /// <summary>
    /// Запрашивает у пользователя выбор поставщика данных.
    /// (В текущей версии Main не используется, но метод оставлен для возможного расширения).
    /// </summary>
    /// <returns>Выбранный DataProvider.</returns>
    private static DataProvider AskForProvider()
    {
        Console.WriteLine("Выберите поставщика данных:");
        Console.WriteLine("1. Entity Framework Core");
        Console.WriteLine("2. Dapper");

        while (true)
        {
            var input = Console.ReadLine();
            if (input == "1")
            {
                return DataProvider.EntityFramework;
            }

            if (input == "2")
            {
                return DataProvider.Dapper;
            }

            Console.Write("Неверный выбор. Повторите попытку: ");
        }
    }

    /// <summary>
    /// Запускает главный цикл меню приложения.
    /// </summary>
    /// <param name="service">Сервис для работы с книгами.</param>
    private static void RunMenu(BookService service)
    {
        while (true)
        {
            PrintMenu();
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ShowBooks(service.GetAllBooks());
                    break;
                case "2":
                    CreateBook(service);
                    break;
                case "3":
                    DeleteBook(service);
                    break;
                case "4":
                    UpdateBook(service);
                    break;
                case "5":
                    FindByAuthor(service);
                    break;
                case "6":
                    GroupByGenre(service);
                    break;
                case "7":
                    FindByTitle(service);
                    break;
                case "8":
                    return;
                default:
                    Console.WriteLine("Команда не распознана.");
                    break;
            }
        }
    }

    /// <summary>
    /// Выводит пункты меню в консоль.
    /// </summary>
    private static void PrintMenu()
    {
        Console.WriteLine("\n--- Управление библиотекой ---");
        Console.WriteLine("1. Показать книги");
        Console.WriteLine("2. Добавить книгу");
        Console.WriteLine("3. Удалить книгу");
        Console.WriteLine("4. Обновить книгу");
        Console.WriteLine("5. Поиск по автору");
        Console.WriteLine("6. Группировка по жанрам");
        Console.WriteLine("7. Поиск по названию");
        Console.WriteLine("8. Выход");
        Console.Write("Выбор: ");
    }

    /// <summary>
    /// Выводит список книг в консоль.
    /// </summary>
    /// <param name="books">Коллекция книг для отображения.</param>
    private static void ShowBooks(IEnumerable<Book> books)
    {
        var list = books.ToList();
        if (!list.Any())
        {
            Console.WriteLine("Книги не найдены.");
            return;
        }

        foreach (var book in list)
        {
            Console.WriteLine(book);
        }
    }

    /// <summary>
    /// Запрашивает данные и создает новую книгу.
    /// </summary>
    private static void CreateBook(BookService service)
    {
        var title = ReadRequiredString("Название: ");
        var author = ReadRequiredString("Автор: ");
        var year = ReadInt("Год: ");
        var genres = ReadGenresList("Жанры (через запятую): ");

        var book = service.CreateBook(title, author, year, genres);
        Console.WriteLine($"Книга добавлена с ID {book.ID}.");
    }

    /// <summary>
    /// Запрашивает ID и удаляет книгу.
    /// </summary>
    private static void DeleteBook(BookService service)
    {
        var id = ReadInt("ID книги для удаления: ");
        if (service.DeleteBook(id))
        {
            Console.WriteLine("Книга удалена.");
        }
        else
        {
            Console.WriteLine("Книга не найдена.");
        }
    }

    /// <summary>
    /// Запрашивает данные и обновляет существующую книгу.
    /// </summary>
    private static void UpdateBook(BookService service)
    {
        var id = ReadInt("ID книги для обновления: ");
        var title = ReadRequiredString("Новое название: ");
        var author = ReadRequiredString("Новый автор: ");
        var year = ReadInt("Новый год: ");
        var genres = ReadGenresList("Новые жанры (через запятую): ");

        if (service.UpdateBook(id, title, author, year, genres))
        {
            Console.WriteLine("Книга обновлена.");
        }
        else
        {
            Console.WriteLine("Книга не найдена.");
        }
    }

    /// <summary>
    /// Запрашивает автора и ищет книги.
    /// </summary>
    private static void FindByAuthor(BookService service)
    {
        var author = ReadRequiredString("Автор: ");
        ShowBooks(service.FindBooksByAuthor(author));
    }

    /// <summary>
    /// Запрашивает название и ищет книги.
    /// </summary>
    private static void FindByTitle(BookService service)
    {
        var title = ReadRequiredString("Часть названия: ");
        ShowBooks(service.FindBooksByTitle(title));
    }

    /// <summary>
    /// Группирует книги по жанрам и выводит результат.
    /// </summary>
    private static void GroupByGenre(BookService service)
    {
        var groups = service.GroupBooksByGenre();
        if (!groups.Any())
        {
            Console.WriteLine("Книги отсутствуют.");
            return;
        }

        foreach (var (genre, books) in groups)
        {
            Console.WriteLine($"\nЖанр: {genre}");
            foreach (var book in books)
            {
                Console.WriteLine($"  {book}");
            }
        }
    }

    /// <summary>
    /// Считывает непустую строку из консоли.
    /// </summary>
    private static string ReadRequiredString(string prompt)
    {
        Console.Write(prompt);
        while (true)
        {
            var value = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(value))
            {
                return value.Trim();
            }

            Console.Write("Поле не может быть пустым. Повторите ввод: ");
        }
    }

    /// <summary>
    /// Считывает целое число из консоли.
    /// </summary>
    private static int ReadInt(string prompt)
    {
        Console.Write(prompt);
        while (true)
        {
            var input = Console.ReadLine();
            if (int.TryParse(input, out var number))
            {
                return number;
            }

            Console.Write("Некорректное число. Повторите ввод: ");
        }
    }

    /// <summary>
    /// Считывает список жанров, разделенных запятыми.
    /// </summary>
    private static IReadOnlyCollection<string> ReadGenresList(string prompt)
    {
        while (true)
        {
            var raw = ReadRequiredString(prompt);
            var genres = raw
                .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(g => g.Trim())
                .Where(g => !string.IsNullOrWhiteSpace(g))
                .Distinct(StringComparer.CurrentCultureIgnoreCase)
                .ToList();

            if (genres.Any())
            {
                return genres;
            }

            Console.WriteLine("Укажите хотя бы один жанр.");
        }
    }
}
