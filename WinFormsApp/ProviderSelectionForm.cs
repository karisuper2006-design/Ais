using System;
using System.Windows.Forms;
using System.Drawing;

namespace WinFormsApp
{
    public class ProviderSelectionForm : Form
    {
        public bool UseDapper { get; private set; }

        private Button btnEf;
        private Button btnDapper;
        private Label lblInstruction;

        public ProviderSelectionForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Выбор провайдера данных";
            this.Size = new Size(350, 200);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            lblInstruction = new Label();
            lblInstruction.Text = "Выберите способ доступа к данным:";
            lblInstruction.Location = new Point(20, 20);
            lblInstruction.AutoSize = true;
            lblInstruction.Font = new Font(this.Font.FontFamily, 10);

            btnEf = new Button();
            btnEf.Text = "Entity Framework Core";
            btnEf.Location = new Point(20, 60);
            btnEf.Size = new Size(290, 40);
            btnEf.Click += (s, e) => { UseDapper = false; DialogResult = DialogResult.OK; Close(); };

            btnDapper = new Button();
            btnDapper.Text = "Dapper";
            btnDapper.Location = new Point(20, 110);
            btnDapper.Size = new Size(290, 40);
            btnDapper.Click += (s, e) => { UseDapper = true; DialogResult = DialogResult.OK; Close(); };

            this.Controls.Add(lblInstruction);
            this.Controls.Add(btnEf);
            this.Controls.Add(btnDapper);
        }
    }
}
