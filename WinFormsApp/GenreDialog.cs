using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WinFormsApp
{
    public partial class GenreDialog : Form
    {
        private readonly HashSet<string> _selected;

        public IReadOnlyList<string> Genres => checkedListGenres.CheckedItems
            .Cast<string>()
            .ToList();

        public GenreDialog(IEnumerable<string>? selectedGenres, IEnumerable<string>? availableGenres = null)
        {
            InitializeComponent();

            _selected = selectedGenres?
                .Where(g => !string.IsNullOrWhiteSpace(g))
                .Select(g => g.Trim())
                .ToHashSet(StringComparer.CurrentCultureIgnoreCase)
                ?? new HashSet<string>(StringComparer.CurrentCultureIgnoreCase);

            var items = new HashSet<string>(_selected, StringComparer.CurrentCultureIgnoreCase);
            if (availableGenres is not null)
            {
                foreach (var genre in availableGenres)
                {
                    if (!string.IsNullOrWhiteSpace(genre))
                    {
                        items.Add(genre.Trim());
                    }
                }
            }

            var ordered = items.OrderBy(g => g, StringComparer.CurrentCultureIgnoreCase).ToList();
            checkedListGenres.Items.AddRange(ordered.Cast<object>().ToArray());

            for (var i = 0; i < checkedListGenres.Items.Count; i++)
            {
                if (_selected.Contains(checkedListGenres.Items[i].ToString() ?? string.Empty))
                {
                    checkedListGenres.SetItemChecked(i, true);
                }
            }

            checkedListGenres.DoubleClick += (_, _) => ToggleChecked();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var value = textBoxNewGenre.Text.Trim();
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            if (!ItemExists(value))
            {
                checkedListGenres.Items.Add(value);
                var index = checkedListGenres.Items.Count - 1;
                checkedListGenres.SetItemChecked(index, true);
            }
            else
            {
                SetChecked(value, isChecked: true);
            }

            textBoxNewGenre.Clear();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (checkedListGenres.CheckedItems.Count == 0)
            {
                MessageBox.Show("Выберите хотя бы один жанр.", "Жанры", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.None;
                return;
            }

            DialogResult = DialogResult.OK;
        }

        private void ToggleChecked()
        {
            if (checkedListGenres.SelectedItem is string genre)
            {
                var current = checkedListGenres.GetItemChecked(checkedListGenres.SelectedIndex);
                checkedListGenres.SetItemChecked(checkedListGenres.SelectedIndex, !current);
            }
        }

        private bool ItemExists(string value)
        {
            for (var i = 0; i < checkedListGenres.Items.Count; i++)
            {
                var existing = checkedListGenres.Items[i]?.ToString();
                if (string.Equals(existing, value, StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private void SetChecked(string value, bool isChecked)
        {
            for (var i = 0; i < checkedListGenres.Items.Count; i++)
            {
                var existing = checkedListGenres.Items[i]?.ToString();
                if (string.Equals(existing, value, StringComparison.CurrentCultureIgnoreCase))
                {
                    checkedListGenres.SetItemChecked(i, isChecked);
                    break;
                }
            }
        }
    }
}
