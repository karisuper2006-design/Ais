namespace WinFormsApp
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            dataGridViewBooks = new DataGridView();
            label1 = new Label();
            label2 = new Label();
            textBoxTitle = new TextBox();
            label3 = new Label();
            textBoxAuthor = new TextBox();
            label4 = new Label();
            textBoxYear = new TextBox();
            label5 = new Label();
            labelGenresValue = new Label();
            btnEditGenres = new Button();
            btnAdd = new Button();
            btnDelete = new Button();
            btnSave = new Button();
            btnGroup = new Button();
            btnRefresh = new Button();
            label6 = new Label();
            textBoxSearchAuthor = new TextBox();
            listBoxAuthorSuggestions = new ListBox();
            label7 = new Label();
            textBoxId = new TextBox();
            textBoxSearchTitle = new TextBox();
            label8 = new Label();
            listBoxTitleSuggestions = new ListBox();
            labelGenreFilter = new Label();
            comboBoxGenreFilter = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)dataGridViewBooks).BeginInit();
            SuspendLayout();
            // 
            // dataGridViewBooks
            // 
            dataGridViewBooks.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dataGridViewBooks.BackgroundColor = Color.FromArgb(245, 247, 250);
            dataGridViewBooks.BorderStyle = BorderStyle.None;
            dataGridViewBooks.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridViewBooks.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(76, 110, 245);
            dataGridViewCellStyle3.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            dataGridViewCellStyle3.ForeColor = Color.White;
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(76, 110, 245);
            dataGridViewCellStyle3.SelectionForeColor = Color.White;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            dataGridViewBooks.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dataGridViewBooks.ColumnHeadersHeight = 42;
            dataGridViewBooks.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = Color.White;
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 9.5F);
            dataGridViewCellStyle4.ForeColor = Color.FromArgb(30, 32, 54);
            dataGridViewCellStyle4.SelectionBackColor = Color.FromArgb(228, 232, 255);
            dataGridViewCellStyle4.SelectionForeColor = Color.FromArgb(30, 32, 54);
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
            dataGridViewBooks.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewBooks.EnableHeadersVisualStyles = false;
            dataGridViewBooks.GridColor = Color.FromArgb(228, 232, 255);
            dataGridViewBooks.Location = new Point(11, 43);
            dataGridViewBooks.Margin = new Padding(3, 4, 3, 4);
            dataGridViewBooks.MultiSelect = false;
            dataGridViewBooks.Name = "dataGridViewBooks";
            dataGridViewBooks.ReadOnly = true;
            dataGridViewBooks.RowHeadersVisible = false;
            dataGridViewBooks.RowHeadersWidth = 51;
            dataGridViewBooks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewBooks.Size = new Size(864, 304);
            dataGridViewBooks.TabIndex = 0;
            dataGridViewBooks.SelectionChanged += dataGridViewBooks_SelectionChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            label1.ForeColor = Color.FromArgb(30, 32, 54);
            label1.Location = new Point(11, 11);
            label1.Name = "label1";
            label1.Size = new Size(106, 21);
            label1.TabIndex = 1;
            label1.Text = "Список книг:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10F);
            label2.ForeColor = Color.FromArgb(87, 89, 113);
            label2.Location = new Point(11, 369);
            label2.Name = "label2";
            label2.Size = new Size(72, 19);
            label2.TabIndex = 2;
            label2.Text = "Название:";
            // 
            // textBoxTitle
            // 
            textBoxTitle.BackColor = Color.White;
            textBoxTitle.BorderStyle = BorderStyle.FixedSingle;
            textBoxTitle.Font = new Font("Segoe UI", 10F);
            textBoxTitle.Location = new Point(94, 365);
            textBoxTitle.Margin = new Padding(3, 4, 3, 4);
            textBoxTitle.Name = "textBoxTitle";
            textBoxTitle.Size = new Size(201, 25);
            textBoxTitle.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 10F);
            label3.ForeColor = Color.FromArgb(87, 89, 113);
            label3.Location = new Point(310, 369);
            label3.Name = "label3";
            label3.Size = new Size(50, 19);
            label3.TabIndex = 4;
            label3.Text = "Автор:";
            // 
            // textBoxAuthor
            // 
            textBoxAuthor.BackColor = Color.White;
            textBoxAuthor.BorderStyle = BorderStyle.FixedSingle;
            textBoxAuthor.Font = new Font("Segoe UI", 10F);
            textBoxAuthor.Location = new Point(365, 365);
            textBoxAuthor.Margin = new Padding(3, 4, 3, 4);
            textBoxAuthor.Name = "textBoxAuthor";
            textBoxAuthor.Size = new Size(201, 25);
            textBoxAuthor.TabIndex = 5;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 10F);
            label4.ForeColor = Color.FromArgb(87, 89, 113);
            label4.Location = new Point(581, 369);
            label4.Name = "label4";
            label4.Size = new Size(35, 19);
            label4.TabIndex = 6;
            label4.Text = "Год:";
            // 
            // textBoxYear
            // 
            textBoxYear.BackColor = Color.White;
            textBoxYear.BorderStyle = BorderStyle.FixedSingle;
            textBoxYear.Font = new Font("Segoe UI", 10F);
            textBoxYear.Location = new Point(619, 365);
            textBoxYear.Margin = new Padding(3, 4, 3, 4);
            textBoxYear.Name = "textBoxYear";
            textBoxYear.Size = new Size(100, 25);
            textBoxYear.TabIndex = 7;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 10F);
            label5.ForeColor = Color.FromArgb(87, 89, 113);
            label5.Location = new Point(11, 416);
            label5.Name = "label5";
            label5.Size = new Size(57, 19);
            label5.TabIndex = 8;
            label5.Text = "Жанры:";
            // 
            // labelGenresValue
            // 
            labelGenresValue.BorderStyle = BorderStyle.FixedSingle;
            labelGenresValue.Location = new Point(147, 398);
            labelGenresValue.Margin = new Padding(3, 4, 3, 4);
            labelGenresValue.Name = "labelGenresValue";
            labelGenresValue.Padding = new Padding(8, 6, 8, 6);
            labelGenresValue.Size = new Size(310, 60);
            labelGenresValue.TabIndex = 9;
            labelGenresValue.Text = "Жанры не выбраны";
            labelGenresValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnEditGenres
            // 
            btnEditGenres.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            btnEditGenres.BackColor = Color.FromArgb(87, 115, 255);
            btnEditGenres.FlatAppearance.BorderSize = 0;
            btnEditGenres.FlatStyle = FlatStyle.Flat;
            btnEditGenres.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            btnEditGenres.ForeColor = Color.White;
            btnEditGenres.Location = new Point(515, 406);
            btnEditGenres.Margin = new Padding(3, 4, 3, 4);
            btnEditGenres.Name = "btnEditGenres";
            btnEditGenres.Size = new Size(126, 41);
            btnEditGenres.TabIndex = 10;
            btnEditGenres.Text = "Выбрать жанры";
            btnEditGenres.UseVisualStyleBackColor = false;
            btnEditGenres.Click += btnEditGenres_Click;
            // 
            // btnAdd
            // 
            btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            btnAdd.BackColor = Color.FromArgb(76, 110, 245);
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            btnAdd.ForeColor = Color.White;
            btnAdd.Location = new Point(11, 463);
            btnAdd.Margin = new Padding(3, 4, 3, 4);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(120, 44);
            btnAdd.TabIndex = 11;
            btnAdd.Text = "Добавить";
            btnAdd.UseVisualStyleBackColor = false;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnDelete
            // 
            btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDelete.BackColor = Color.FromArgb(240, 62, 62);
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            btnDelete.ForeColor = Color.White;
            btnDelete.Location = new Point(147, 463);
            btnDelete.Margin = new Padding(3, 4, 3, 4);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(120, 44);
            btnDelete.TabIndex = 12;
            btnDelete.Text = "Удалить";
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSave.BackColor = Color.FromArgb(32, 201, 151);
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(283, 463);
            btnSave.Margin = new Padding(3, 4, 3, 4);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(120, 44);
            btnSave.TabIndex = 13;
            btnSave.Text = "Сохранить";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // btnGroup
            // 
            btnGroup.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnGroup.BackColor = Color.FromArgb(52, 71, 103);
            btnGroup.FlatAppearance.BorderSize = 0;
            btnGroup.FlatStyle = FlatStyle.Flat;
            btnGroup.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            btnGroup.ForeColor = Color.White;
            btnGroup.Location = new Point(421, 463);
            btnGroup.Margin = new Padding(3, 4, 3, 4);
            btnGroup.Name = "btnGroup";
            btnGroup.Size = new Size(200, 44);
            btnGroup.TabIndex = 14;
            btnGroup.Text = "Сортировка по жанрам";
            btnGroup.UseVisualStyleBackColor = false;
            btnGroup.Click += btnGroup_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRefresh.BackColor = Color.FromArgb(104, 213, 191);
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            btnRefresh.ForeColor = Color.FromArgb(36, 37, 66);
            btnRefresh.Location = new Point(635, 463);
            btnRefresh.Margin = new Padding(3, 4, 3, 4);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(140, 44);
            btnRefresh.TabIndex = 15;
            btnRefresh.Text = "Обновить";
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 10F);
            label6.ForeColor = Color.FromArgb(87, 89, 113);
            label6.Location = new Point(11, 537);
            label6.Name = "label6";
            label6.Size = new Size(118, 19);
            label6.TabIndex = 16;
            label6.Text = "Поиск по автору:";
            // 
            // textBoxSearchAuthor
            // 
            textBoxSearchAuthor.BackColor = Color.White;
            textBoxSearchAuthor.BorderStyle = BorderStyle.FixedSingle;
            textBoxSearchAuthor.Font = new Font("Segoe UI", 10F);
            textBoxSearchAuthor.Location = new Point(154, 533);
            textBoxSearchAuthor.Margin = new Padding(3, 4, 3, 4);
            textBoxSearchAuthor.Name = "textBoxSearchAuthor";
            textBoxSearchAuthor.Size = new Size(201, 25);
            textBoxSearchAuthor.TabIndex = 17;
            textBoxSearchAuthor.TextChanged += textBoxSearchAuthor_TextChanged;
            // 
            // listBoxAuthorSuggestions
            // 
            listBoxAuthorSuggestions.BackColor = Color.White;
            listBoxAuthorSuggestions.BorderStyle = BorderStyle.FixedSingle;
            listBoxAuthorSuggestions.Font = new Font("Segoe UI", 9F);
            listBoxAuthorSuggestions.FormattingEnabled = true;
            listBoxAuthorSuggestions.ItemHeight = 15;
            listBoxAuthorSuggestions.Location = new Point(154, 572);
            listBoxAuthorSuggestions.Margin = new Padding(3, 4, 3, 4);
            listBoxAuthorSuggestions.Name = "listBoxAuthorSuggestions";
            listBoxAuthorSuggestions.Size = new Size(201, 92);
            listBoxAuthorSuggestions.TabIndex = 18;
            listBoxAuthorSuggestions.Visible = false;
            listBoxAuthorSuggestions.SelectedIndexChanged += listBoxAuthorSuggestions_SelectedIndexChanged;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 10F);
            label7.ForeColor = Color.FromArgb(87, 89, 113);
            label7.Location = new Point(735, 369);
            label7.Name = "label7";
            label7.Size = new Size(26, 19);
            label7.TabIndex = 19;
            label7.Text = "ID:";
            // 
            // textBoxId
            // 
            textBoxId.BackColor = Color.FromArgb(237, 241, 250);
            textBoxId.BorderStyle = BorderStyle.None;
            textBoxId.Enabled = false;
            textBoxId.Location = new Point(767, 367);
            textBoxId.Margin = new Padding(3, 4, 3, 4);
            textBoxId.Name = "textBoxId";
            textBoxId.Size = new Size(50, 17);
            textBoxId.TabIndex = 20;
            // 
            // textBoxSearchTitle
            // 
            textBoxSearchTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBoxSearchTitle.BackColor = Color.White;
            textBoxSearchTitle.BorderStyle = BorderStyle.FixedSingle;
            textBoxSearchTitle.Font = new Font("Segoe UI", 10F);
            textBoxSearchTitle.Location = new Point(541, 540);
            textBoxSearchTitle.Margin = new Padding(3, 4, 3, 4);
            textBoxSearchTitle.Name = "textBoxSearchTitle";
            textBoxSearchTitle.Size = new Size(114, 25);
            textBoxSearchTitle.TabIndex = 21;
            textBoxSearchTitle.TextChanged += textBoxSearchTitle_TextChanged;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 10F);
            label8.ForeColor = Color.FromArgb(87, 89, 113);
            label8.Location = new Point(383, 540);
            label8.Name = "label8";
            label8.Size = new Size(137, 19);
            label8.TabIndex = 22;
            label8.Text = "Поиск по названию:";
            // 
            // listBoxTitleSuggestions
            // 
            listBoxTitleSuggestions.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            listBoxTitleSuggestions.BackColor = Color.White;
            listBoxTitleSuggestions.BorderStyle = BorderStyle.FixedSingle;
            listBoxTitleSuggestions.Font = new Font("Segoe UI", 9F);
            listBoxTitleSuggestions.FormattingEnabled = true;
            listBoxTitleSuggestions.ItemHeight = 15;
            listBoxTitleSuggestions.Location = new Point(455, 448);
            listBoxTitleSuggestions.Name = "listBoxTitleSuggestions";
            listBoxTitleSuggestions.Size = new Size(265, 77);
            listBoxTitleSuggestions.TabIndex = 24;
            listBoxTitleSuggestions.Visible = false;
            listBoxTitleSuggestions.SelectedIndexChanged += listBoxTitleSuggestions_SelectedIndexChanged;
            // 
            // labelGenreFilter
            // 
            labelGenreFilter.AutoSize = true;
            labelGenreFilter.Font = new Font("Segoe UI", 10F);
            labelGenreFilter.ForeColor = Color.FromArgb(87, 89, 113);
            labelGenreFilter.Location = new Point(383, 580);
            labelGenreFilter.Name = "labelGenreFilter";
            labelGenreFilter.Size = new Size(122, 19);
            labelGenreFilter.TabIndex = 25;
            labelGenreFilter.Text = "Фильтр по жанру:";
            // 
            // comboBoxGenreFilter
            // 
            comboBoxGenreFilter.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            comboBoxGenreFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxGenreFilter.Font = new Font("Segoe UI", 10F);
            comboBoxGenreFilter.FormattingEnabled = true;
            comboBoxGenreFilter.Location = new Point(541, 576);
            comboBoxGenreFilter.Name = "comboBoxGenreFilter";
            comboBoxGenreFilter.Size = new Size(200, 25);
            comboBoxGenreFilter.TabIndex = 26;
            comboBoxGenreFilter.SelectedIndexChanged += comboBoxGenreFilter_SelectedIndexChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(237, 241, 250);
            ClientSize = new Size(900, 707);
            Controls.Add(comboBoxGenreFilter);
            Controls.Add(labelGenreFilter);
            Controls.Add(listBoxTitleSuggestions);
            Controls.Add(label8);
            Controls.Add(textBoxSearchTitle);
            Controls.Add(textBoxId);
            Controls.Add(label7);
            Controls.Add(listBoxAuthorSuggestions);
            Controls.Add(textBoxSearchAuthor);
            Controls.Add(label6);
            Controls.Add(btnRefresh);
            Controls.Add(btnGroup);
            Controls.Add(btnSave);
            Controls.Add(btnDelete);
            Controls.Add(btnAdd);
            Controls.Add(btnEditGenres);
            Controls.Add(labelGenresValue);
            Controls.Add(label5);
            Controls.Add(textBoxYear);
            Controls.Add(label4);
            Controls.Add(textBoxAuthor);
            Controls.Add(label3);
            Controls.Add(textBoxTitle);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(dataGridViewBooks);
            Font = new Font("Segoe UI", 9.5F);
            ForeColor = Color.FromArgb(30, 32, 54);
            Margin = new Padding(3, 4, 3, 4);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Управление книгами";
            ((System.ComponentModel.ISupportInitialize)dataGridViewBooks).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewBooks;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxTitle;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxAuthor;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxYear;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelGenresValue;
        private System.Windows.Forms.Button btnEditGenres;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnGroup;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxSearchAuthor;
        private System.Windows.Forms.ListBox listBoxAuthorSuggestions;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxId;
        private System.Windows.Forms.TextBox textBoxSearchTitle;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ListBox listBoxTitleSuggestions;
        private System.Windows.Forms.Label labelGenreFilter;
        private System.Windows.Forms.ComboBox comboBoxGenreFilter;
    }
}
