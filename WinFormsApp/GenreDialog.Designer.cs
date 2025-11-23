namespace WinFormsApp
{
    partial class GenreDialog
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
            checkedListGenres = new CheckedListBox();
            textBoxNewGenre = new TextBox();
            btnAdd = new Button();
            btnOk = new Button();
            btnCancel = new Button();
            SuspendLayout();
            // 
            // checkedListGenres
            // 
            checkedListGenres.BackColor = Color.White;
            checkedListGenres.BorderStyle = BorderStyle.FixedSingle;
            checkedListGenres.CheckOnClick = true;
            checkedListGenres.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            checkedListGenres.FormattingEnabled = true;
            checkedListGenres.Location = new Point(12, 12);
            checkedListGenres.Name = "checkedListGenres";
            checkedListGenres.Size = new Size(360, 180);
            checkedListGenres.TabIndex = 0;
            // 
            // textBoxNewGenre
            // 
            textBoxNewGenre.BackColor = Color.White;
            textBoxNewGenre.BorderStyle = BorderStyle.FixedSingle;
            textBoxNewGenre.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            textBoxNewGenre.Location = new Point(12, 215);
            textBoxNewGenre.Name = "textBoxNewGenre";
            textBoxNewGenre.Size = new Size(260, 27);
            textBoxNewGenre.TabIndex = 1;
            // 
            // btnAdd
            // 
            btnAdd.BackColor = Color.FromArgb(76, 110, 245);
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
            btnAdd.ForeColor = Color.White;
            btnAdd.Location = new Point(278, 213);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(94, 31);
            btnAdd.TabIndex = 2;
            btnAdd.Text = "Добавить";
            btnAdd.UseVisualStyleBackColor = false;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnOk
            // 
            btnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnOk.DialogResult = DialogResult.OK;
            btnOk.BackColor = Color.FromArgb(32, 201, 151);
            btnOk.FlatAppearance.BorderSize = 0;
            btnOk.FlatStyle = FlatStyle.Flat;
            btnOk.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
            btnOk.ForeColor = Color.White;
            btnOk.Location = new Point(180, 268);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(90, 32);
            btnOk.TabIndex = 4;
            btnOk.Text = "ОК";
            btnOk.UseVisualStyleBackColor = false;
            btnOk.Click += btnOk_Click;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.BackColor = Color.FromArgb(237, 241, 250);
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
            btnCancel.ForeColor = Color.FromArgb(30, 32, 54);
            btnCancel.Location = new Point(282, 268);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(90, 32);
            btnCancel.TabIndex = 5;
            btnCancel.Text = "Отмена";
            btnCancel.UseVisualStyleBackColor = false;
            // 
            // GenreDialog
            // 
            AcceptButton = btnOk;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(250, 252, 255);
            CancelButton = btnCancel;
            ClientSize = new Size(384, 312);
            Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            ForeColor = Color.FromArgb(30, 32, 54);
            Controls.Add(btnCancel);
            Controls.Add(btnOk);
            Controls.Add(btnAdd);
            Controls.Add(textBoxNewGenre);
            Controls.Add(checkedListGenres);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "GenreDialog";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Жанры";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox textBoxNewGenre;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckedListBox checkedListGenres;
    }
}
