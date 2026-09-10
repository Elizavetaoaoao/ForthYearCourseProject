using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using WpfAppTRPO.Helpers;

namespace WpfAppTRPO.Views
{
    public partial class LogGiveBook : Window
    {
        private DataTable readers;
        private DataTable books;
        private DataTable freeBooks;
        private DateTime dueDate;
        private int librarian;
        public LogGiveBook(int librarian)
        {
            InitializeComponent();
            LoadData();
            textBlockDueDate.Text = dueDate.ToString();
            this.librarian = librarian;
        }
        private void LoadData()
        {
            readers = DatabaseHelper.GetAllReaders();
            books = DatabaseHelper.GetAllBooks();
            
            comboBoxReaders.ItemsSource = readers.DefaultView;
            comboBoxBooks.ItemsSource = books.DefaultView;
            comboBoxBooks.SelectedIndex = 0;

            freeBooks = DatabaseHelper.GetAvailableBooks(Convert.ToInt32(comboBoxBooks.SelectedValue));
            comboBoxBookCopies.ItemsSource = freeBooks.DefaultView;

            dueDate = DateTime.Now.AddDays(30);
        }

        private void ButtonGive_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int copyId = (int)comboBoxBookCopies.SelectedValue;
                int readerId = (int)comboBoxReaders.SelectedValue;
                if (DatabaseHelper.GiveBook(readerId, copyId, librarian, dueDate))
                {
                    Close();
                }
                else
                {
                    MessageBox.Show("Не удалось выдать книгу. Проверьте данные и попробуйте снова.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch(Exception ex) { }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void comboBoxBookCopies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            freeBooks = DatabaseHelper.GetAvailableBooks(Convert.ToInt32(comboBoxBooks.SelectedValue));
            comboBoxBookCopies.ItemsSource = freeBooks.DefaultView;
        }
    }
}
