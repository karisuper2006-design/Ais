using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Показываем форму выбора провайдера перед запуском основного окна
            using var selectionForm = new ProviderSelectionForm();
            if (selectionForm.ShowDialog() == DialogResult.OK)
            {
                Application.Run(new Form1(selectionForm.UseDapper));
            }
        }
    }
}