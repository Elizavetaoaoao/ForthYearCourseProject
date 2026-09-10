using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using WpfAppTRPO.Helpers;

namespace WpfAppTRPO.Views
{
    public partial class LogReturnBook : Window
    {
        private int librarian;
        private DataTable readers;
        private DataTable books;
        public LogReturnBook(int lib)
        {
            InitializeComponent();
            librarian = lib;
            LoadData();
        }
        private void LoadData()
        {
            readers = DatabaseHelper.GetAllReaders();
            comboBoxReaders.ItemsSource = readers.DefaultView;
            comboBoxReaders.SelectedIndex = 0;
            comboBoxBookCopies_SelectionChanged(null, null);
            books = DatabaseHelper.GetReaderBooks(Convert.ToInt32(comboBoxReaders.SelectedValue));
            comboBoxBooks.ItemsSource = books.DefaultView;
            comboBoxBooks.SelectedIndex = 0;
            //textBlockDueDate.Text = books.Rows[0][3].ToString();
            //status.Text = (Convert.ToDateTime(books.Rows[0][3]) < DateTime.Now) ? "прострочен" : "можно вернуть";
        }
        private void ButtonGive_Click(object sender, RoutedEventArgs e)
        {
            int copyId = (int)comboBoxBooks.SelectedValue;
            int readerId = (int)comboBoxReaders.SelectedValue;
            if (DatabaseHelper.ReturnBook(copyId))
            {
                Close();
            }
            else
            {
                MessageBox.Show("Не удалось сохранить возврат. Проверьте данные и попробуйте снова.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void comboBoxBookCopies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            comboBoxBooks.ItemsSource = DatabaseHelper.GetReaderBooks(Convert.ToInt32(comboBoxReaders.SelectedValue)).DefaultView;
        }
    }
}
