using BookManagementSystem.BusinessLogicLayer.Services;
using BookManagementSystem.DataAccessLayer.Configuration;
using BookManagementSystem.DataAccessLayer.Contexts;
using BookManagementSystem.DataAccessLayer.Repositories;
using BookManagementSystem.Domain.Entities;
using Ninject;
using BookManagementSystem.BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp
{
    /// <summary>
    /// Главная форма приложения управления библиотекой.
    /// </summary>
    public partial class Form1 : Form
    {
        private readonly BookService _service;

        private List<Book> _allBooks = new();
        private List<string> _currentGenres = new();
        private bool _suppressAuthorSuggestionUpdates;
        private bool _suppressTitleSuggestionUpdates;
        private bool _suppressSelectionChange;
        private bool _suppressGenreFilterUpdates;
        private ComboBox? _genreFilterComboBox;
        private readonly OutsideClickMessageFilter _outsideClickFilter;

        private const string AllGenresOption = "Все жанры";

        /// <summary>
        /// Инициализирует новый экземпляр главной формы.
        /// </summary>
        /// <param name="useDapper">Флаг использования Dapper для доступа к данным.</param>
        public Form1(bool useDapper = false)
        {
            InitializeComponent();
            _outsideClickFilter = new OutsideClickMessageFilter(this);
            Application.AddMessageFilter(_outsideClickFilter);
            _genreFilterComboBox = comboBoxGenreFilter;
            ConfigureGrid();
            UpdateGenresDisplay();

            IKernel ninjectKernel = new StandardKernel(new SimpleConfigModule(useDapper));
            _service = ninjectKernel.Get<BookService>();

            Activated += Form1_Activated;
            LoadBooks();
        }

        /// <summary>
        /// Освобождает ресурсы при закрытии формы.
        /// </summary>
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            Application.RemoveMessageFilter(_outsideClickFilter);
            Application.RemoveMessageFilter(_outsideClickFilter);
        }

        /// <summary>
        /// Фильтр сообщений для обработки кликов вне элементов управления (для скрытия подсказок).
        /// </summary>
        private sealed class OutsideClickMessageFilter : IMessageFilter
        {
            private readonly Form1 _owner;

            public OutsideClickMessageFilter(Form1 owner)
            {
                _owner = owner;
            }

            public bool PreFilterMessage(ref Message m)
            {
                const int WM_LBUTTONDOWN = 0x0201;
                const int WM_RBUTTONDOWN = 0x0204;
                const int WM_MBUTTONDOWN = 0x0207;

                if (m.Msg == WM_LBUTTONDOWN || m.Msg == WM_RBUTTONDOWN || m.Msg == WM_MBUTTONDOWN)
                {
                    var clickedControl = Control.FromHandle(m.HWnd);

                    if (_owner.listBoxTitleSuggestions.Visible &&
                        !IsWithinAllowedControls(clickedControl, _owner.listBoxTitleSuggestions, _owner.textBoxSearchTitle))
                    {
                        _owner.HideTitleSuggestions();
                    }

                    if (_owner.listBoxAuthorSuggestions.Visible &&
                        !IsWithinAllowedControls(clickedControl, _owner.listBoxAuthorSuggestions, _owner.textBoxSearchAuthor))
                    {
                        _owner.HideAuthorSuggestions();
                    }
                }

                return false;
            }

            private static bool IsWithinAllowedControls(Control? control, params Control?[] roots)
            {
                if (control is null)
                {
                    return false;
                }

                foreach (var root in roots)
                {
                    if (IsDescendant(control, root))
                    {
                        return true;
                    }
                }

                return false;
            }

            private static bool IsDescendant(Control? candidate, Control? root)
            {
                while (candidate is not null)
                {
                    if (candidate == root)
                    {
                        return true;
                    }

                    candidate = candidate.Parent;
                }

                return false;
            }
        }

        private void Form1_Activated(object? sender, EventArgs e)
        {
            LoadBooks();
        }

        /// <summary>
        /// Настраивает столбцы таблицы книг.
        /// </summary>
        private void ConfigureGrid()
        {
            dataGridViewBooks.AutoGenerateColumns = false;
            dataGridViewBooks.AllowUserToAddRows = false;
            dataGridViewBooks.AllowUserToDeleteRows = false;
            dataGridViewBooks.MultiSelect = false;
            dataGridViewBooks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewBooks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dataGridViewBooks.Columns.Clear();
            dataGridViewBooks.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(Book.ID),
                HeaderText = "ID",
                FillWeight = 15,
                ReadOnly = true
            });
            dataGridViewBooks.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(Book.Title),
                HeaderText = "Название",
                FillWeight = 35,
                ReadOnly = true
            });
            dataGridViewBooks.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(Book.Author),
                HeaderText = "Автор",
                FillWeight = 30,
                ReadOnly = true
            });
            dataGridViewBooks.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(Book.Year),
                HeaderText = "Год",
                FillWeight = 10,
                ReadOnly = true
            });
            dataGridViewBooks.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(Book.GenresDisplay),
                HeaderText = "Жанры",
                FillWeight = 30,
                ReadOnly = true
            });
        }

        /// <summary>
        /// Загружает список книг из сервиса и обновляет отображение.
        /// </summary>
        private void LoadBooks()
        {
            _allBooks = _service
                .GetAllBooks()
                .OrderBy(b => b.ID)
                .ToList();

            DisplayBooks(_allBooks);
            RefreshGenreFilterOptions();
        }

        /// <summary>
        /// Отображает переданную коллекцию книг в таблице.
        /// </summary>
        private void DisplayBooks(IReadOnlyCollection<Book> books)
        {
            dataGridViewBooks.DataSource = null;
            dataGridViewBooks.DataSource = books.ToList();
            dataGridViewBooks.ClearSelection();
        }

        /// <summary>
        /// Обработчик кнопки добавления книги.
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            var title = textBoxTitle.Text.Trim();
            var author = textBoxAuthor.Text.Trim();
            var genres = _currentGenres.ToList();

            if (!int.TryParse(textBoxYear.Text, out var year))
            {
                MessageBox.Show("Год должен быть числом.");
                return;
            }

            if (string.IsNullOrWhiteSpace(title) ||
                string.IsNullOrWhiteSpace(author) ||
                !genres.Any())
            {
                MessageBox.Show("Все поля обязательны.");
                return;
            }

            _service.CreateBook(title, author, year, genres);
            LoadBooks();
            ClearFields();
            MessageBox.Show("Книга добавлена.");
        }

        /// <summary>
        /// Обработчик кнопки удаления книги.
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            var id = ReadSelectedBookId();
            if (id is null)
            {
                MessageBox.Show("Выберите книгу для удаления.");
                return;
            }

            if (_service.DeleteBook(id.Value))
            {
                LoadBooks();
                ClearFields();
                MessageBox.Show("Книга удалена.");
            }
            else
            {
                MessageBox.Show("Не удалось удалить книгу.");
            }
        }

        /// <summary>
        /// Обработчик кнопки сохранения изменений (обновления книги).
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textBoxId.Text, out var id))
            {
                MessageBox.Show("Выберите запись для сохранения.");
                return;
            }

            var title = textBoxTitle.Text.Trim();
            var author = textBoxAuthor.Text.Trim();
            var genres = _currentGenres.ToList();

            if (!int.TryParse(textBoxYear.Text, out var year))
            {
                MessageBox.Show("Год должен быть числом.");
                return;
            }

            if (string.IsNullOrWhiteSpace(title) ||
                string.IsNullOrWhiteSpace(author) ||
                !genres.Any())
            {
                MessageBox.Show("Все поля обязательны.");
                return;
            }

            if (_service.UpdateBook(id, title, author, year, genres))
            {
                LoadBooks();
                ClearFields();
                MessageBox.Show("Книга обновлена.");
            }
            else
            {
                MessageBox.Show("Не удалось обновить запись.");
            }
        }

        /// <summary>
        /// Обработчик кнопки группировки книг по жанрам.
        /// </summary>
        private void btnGroup_Click(object sender, EventArgs e)
        {
            if (!_allBooks.Any())
            {
                MessageBox.Show("Книги отсутствуют.");
                return;
            }

            var sorted = _allBooks
                .OrderBy(b => GetPrimaryGenre(b), StringComparer.CurrentCultureIgnoreCase)
                .ThenBy(b => b.Title, StringComparer.CurrentCultureIgnoreCase)
                .ToList();

            DisplayBooks(sorted);
        }

        /// <summary>
        /// Обработчик кнопки обновления списка книг.
        /// </summary>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadBooks();
            ClearFields();
        }

        /// <summary>
        /// Обработчик кнопки редактирования жанров.
        /// </summary>
        private void btnEditGenres_Click(object sender, EventArgs e)
        {
            var available = _service.GetAllGenres();
            using var dialog = new GenreDialog(_currentGenres, available);
            _suppressSelectionChange = true;
            try
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    _currentGenres = dialog.Genres
                        .Select(name => name.Trim())
                        .Where(name => !string.IsNullOrWhiteSpace(name))
                        .Distinct(StringComparer.CurrentCultureIgnoreCase)
                        .ToList();
                    UpdateGenresDisplay();
                }
            }
            finally
            {
                _suppressSelectionChange = false;
            }
        }

        /// <summary>
        /// Обработчик изменения выбранного фильтра жанров.
        /// </summary>
        private void comboBoxGenreFilter_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (_suppressGenreFilterUpdates)
            {
                return;
            }

            ApplyCombinedFilters();
        }

        /// <summary>
        /// Обработчик изменения выбранной строки в таблице книг.
        /// </summary>
        private void dataGridViewBooks_SelectionChanged(object sender, EventArgs e)
        {
            if (_suppressSelectionChange)
            {
                return;
            }

            if (dataGridViewBooks.CurrentRow?.DataBoundItem is Book book)
            {
                PopulateFormFields(book);
            }
        }

        /// <summary>
        /// Заполняет поля формы данными выбранной книги.
        /// </summary>
        private void PopulateFormFields(Book book)
        {
            textBoxId.Text = book.ID.ToString();
            textBoxTitle.Text = book.Title;
            textBoxAuthor.Text = book.Author;
            textBoxYear.Text = book.Year.ToString();
            _currentGenres = book.Genres
                .Select(g => g.Name)
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .Select(name => name.Trim())
                .Distinct(StringComparer.CurrentCultureIgnoreCase)
                .ToList();
            UpdateGenresDisplay();
        }

        /// <summary>
        /// Обработчик изменения текста в поле поиска автора.
        /// </summary>
        private void textBoxSearchAuthor_TextChanged(object sender, EventArgs e)
        {
            if (_suppressAuthorSuggestionUpdates)
            {
                return;
            }

            UpdateAuthorSuggestions(textBoxSearchAuthor.Text);
            ApplyCombinedFilters();
        }

        /// <summary>
        /// Обработчик выбора автора из списка подсказок.
        /// </summary>
        private void listBoxAuthorSuggestions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxAuthorSuggestions.SelectedItem is not string author)
            {
                return;
            }

            _suppressAuthorSuggestionUpdates = true;
            textBoxSearchAuthor.Text = author;
            textBoxSearchAuthor.SelectionStart = author.Length;
            _suppressAuthorSuggestionUpdates = false;

            HideAuthorSuggestions();
            ApplyCombinedFilters();
        }

        /// <summary>
        /// Обработчик изменения текста в поле поиска названия.
        /// </summary>
        private void textBoxSearchTitle_TextChanged(object sender, EventArgs e)
        {
            if (_suppressTitleSuggestionUpdates)
            {
                return;
            }

            UpdateTitleSuggestions(textBoxSearchTitle.Text);
            ApplyCombinedFilters();
        }

        /// <summary>
        /// Обработчик выбора названия из списка подсказок.
        /// </summary>
        private void listBoxTitleSuggestions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxTitleSuggestions.SelectedItem is not string title)
            {
                return;
            }

            _suppressTitleSuggestionUpdates = true;
            textBoxSearchTitle.Text = title;
            textBoxSearchTitle.SelectionStart = title.Length;
            _suppressTitleSuggestionUpdates = false;

            HideTitleSuggestions();
            ApplyCombinedFilters();
        }

        /// <summary>
        /// Фильтрует книги по автору.
        /// </summary>
        private IEnumerable<Book> FilterBooksByAuthor(string? authorFragment, IEnumerable<Book>? source = null)
        {
            source ??= _allBooks;
            if (string.IsNullOrWhiteSpace(authorFragment))
            {
                return source;
            }

            var needle = authorFragment.Trim();
            return source.Where(b =>
                !string.IsNullOrWhiteSpace(b.Author) &&
                b.Author.Contains(needle, StringComparison.CurrentCultureIgnoreCase));
        }

        /// <summary>
        /// Фильтрует книги по названию.
        /// </summary>
        private IEnumerable<Book> FilterBooksByTitle(string? titleFragment, IEnumerable<Book>? source = null)
        {
            source ??= _allBooks;
            if (string.IsNullOrWhiteSpace(titleFragment))
            {
                return source;
            }

            var needle = titleFragment.Trim();
            return source.Where(b =>
                !string.IsNullOrWhiteSpace(b.Title) &&
                b.Title.Contains(needle, StringComparison.CurrentCultureIgnoreCase));
        }

        /// <summary>
        /// Фильтрует книги по выбранному жанру.
        /// </summary>
        private IEnumerable<Book> FilterBooksByGenre(IEnumerable<Book> source)
        {
            var selectedGenre = GetSelectedGenreFilter();
            if (string.IsNullOrWhiteSpace(selectedGenre))
            {
                return source;
            }

            return source.Where(b =>
                b.Genres.Any(g => string.Equals(g.Name, selectedGenre, StringComparison.CurrentCultureIgnoreCase)));
        }

        /// <summary>
        /// Применяет все фильтры (автор, название, жанр) и обновляет список.
        /// </summary>
        private void ApplyCombinedFilters()
        {
            var books = FilterBooksByAuthor(textBoxSearchAuthor.Text);
            books = FilterBooksByTitle(textBoxSearchTitle.Text, books);
            books = FilterBooksByGenre(books);
            DisplayBooks(books.ToList());
        }

        /// <summary>
        /// Получает текущий выбранный фильтр жанра.
        /// </summary>
        private string? GetSelectedGenreFilter()
        {
            if (_genreFilterComboBox is null ||
                _genreFilterComboBox.SelectedItem is not string selected ||
                selected == AllGenresOption)
            {
                return null;
            }

            return selected;
        }

        /// <summary>
        /// Очищает поля ввода формы.
        /// </summary>
        private void ClearFields()
        {
            textBoxId.Clear();
            textBoxTitle.Clear();
            textBoxAuthor.Clear();
            textBoxYear.Clear();
            _currentGenres.Clear();
            UpdateGenresDisplay();
            textBoxSearchAuthor.Clear();
            textBoxSearchTitle.Clear();
            if (_genreFilterComboBox is not null && _genreFilterComboBox.Items.Count > 0)
            {
                _suppressGenreFilterUpdates = true;
                _genreFilterComboBox.SelectedIndex = 0;
                _suppressGenreFilterUpdates = false;
            }
            HideAuthorSuggestions();
            HideTitleSuggestions();
            dataGridViewBooks.ClearSelection();
        }

        /// <summary>
        /// Обновляет отображение списка выбранных жанров.
        /// </summary>
        private void UpdateGenresDisplay()
        {
            labelGenresValue.Text = _currentGenres.Any()
                ? string.Join(", ", _currentGenres)
                : "Жанры не выбраны";
        }

        /// <summary>
        /// Обновляет список жанров в фильтре.
        /// </summary>
        private void RefreshGenreFilterOptions()
        {
            if (_genreFilterComboBox is null)
            {
                return;
            }

            var selected = GetSelectedGenreFilter();
            var genres = _service.GetAllGenres();

            _suppressGenreFilterUpdates = true;
            _genreFilterComboBox.Items.Clear();
            _genreFilterComboBox.Items.Add(AllGenresOption);
            foreach (var genre in genres)
            {
                _genreFilterComboBox.Items.Add(genre);
            }

            if (!string.IsNullOrWhiteSpace(selected) && _genreFilterComboBox.Items.Contains(selected))
            {
                _genreFilterComboBox.SelectedItem = selected;
            }
            else
            {
                _genreFilterComboBox.SelectedIndex = 0;
            }

            _suppressGenreFilterUpdates = false;
        }

        /// <summary>
        /// Получает ID выбранной книги из таблицы или поля ввода.
        /// </summary>
        private int? ReadSelectedBookId()
        {
            if (dataGridViewBooks.CurrentRow?.DataBoundItem is Book book)
            {
                return book.ID;
            }

            if (int.TryParse(textBoxId.Text, out var id))
            {
                return id;
            }

            return null;
        }

        /// <summary>
        /// Обновляет список подсказок авторов.
        /// </summary>
        private void UpdateAuthorSuggestions(string? query)
        {
            var normalized = query?.Trim() ?? string.Empty;

            IEnumerable<string> authors = _allBooks
                .Select(b => b.Author)
                .Where(name => !string.IsNullOrWhiteSpace(name));

            if (!string.IsNullOrEmpty(normalized))
            {
                authors = authors.Where(name =>
                    name.Contains(normalized, StringComparison.CurrentCultureIgnoreCase));
            }

            var suggestions = authors
                .Distinct(StringComparer.CurrentCultureIgnoreCase)
                .OrderBy(name => name, StringComparer.CurrentCultureIgnoreCase)
                .ToList();

            if (!suggestions.Any())
            {
                HideAuthorSuggestions();
                return;
            }

            listBoxAuthorSuggestions.BeginUpdate();
            listBoxAuthorSuggestions.Items.Clear();
            listBoxAuthorSuggestions.Items.AddRange(suggestions.Cast<object>().ToArray());
            listBoxAuthorSuggestions.EndUpdate();
            listBoxAuthorSuggestions.Visible = true;
        }

        /// <summary>
        /// Скрывает список подсказок авторов.
        /// </summary>
        private void HideAuthorSuggestions()
        {
            listBoxAuthorSuggestions.Visible = false;
            listBoxAuthorSuggestions.Items.Clear();
        }

        /// <summary>
        /// Обновляет список подсказок названий.
        /// </summary>
        private void UpdateTitleSuggestions(string? query)
        {
            var normalized = query?.Trim() ?? string.Empty;

            IEnumerable<string> titles = _allBooks
                .Select(b => b.Title)
                .Where(name => !string.IsNullOrWhiteSpace(name));

            if (!string.IsNullOrEmpty(normalized))
            {
                titles = titles.Where(name =>
                    name.Contains(normalized, StringComparison.CurrentCultureIgnoreCase));
            }

            var suggestions = titles
                .Distinct(StringComparer.CurrentCultureIgnoreCase)
                .OrderBy(name => name, StringComparer.CurrentCultureIgnoreCase)
                .ToList();

            if (!suggestions.Any())
            {
                HideTitleSuggestions();
                return;
            }

            listBoxTitleSuggestions.BeginUpdate();
            listBoxTitleSuggestions.Items.Clear();
            listBoxTitleSuggestions.Items.AddRange(suggestions.Cast<object>().ToArray());
            listBoxTitleSuggestions.EndUpdate();
            listBoxTitleSuggestions.Visible = true;
        }

        /// <summary>
        /// Скрывает список подсказок названий.
        /// </summary>
        private void HideTitleSuggestions()
        {
            listBoxTitleSuggestions.Visible = false;
            listBoxTitleSuggestions.Items.Clear();
        }

        /// <summary>
        /// Получает основной жанр книги (первый из списка) или "Без жанра".
        /// </summary>
        private static string GetPrimaryGenre(Book book)
        {
            return book.Genres
                .Select(g => g.Name)
                .FirstOrDefault(name => !string.IsNullOrWhiteSpace(name))
                ?? "Без жанра";
        }
    }
}
