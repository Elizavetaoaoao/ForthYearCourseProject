using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfAppTRPO.Helpers;

namespace WpfAppTRPO.Views
{
    public partial class CatalogPage : Page
    {
        internal CatalogPage()
        {
            InitializeComponent();
            LoadBooks();
        }
        private void LoadBooks()
        {
            dataGridBooks.ItemsSource = DatabaseHelper.GetAllBooks().DefaultView;
        }
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            Window editWindow = new BookEditAdd("Добавление книги");
            editWindow.ShowDialog();
            LoadBooks();
        }
        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridBooks.SelectedItem == null)
            {
                MessageBox.Show("Выберите книгу для удаления.", "Удаление", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            DataRowView selectedRow = (DataRowView)dataGridBooks.SelectedItem;
            int bookId = (int)selectedRow["Id"];
            if (MessageBox.Show("Удалить выбранную книгу?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                bool deleted = DatabaseHelper.DeleteBook(bookId);
                if (deleted)
                {
                    LoadBooks();
                    MessageBox.Show("Книга удалена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Не удалось удалить книгу.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadBooks();
        }
        private void DgBooks_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dataGridBooks.SelectedItem == null) return;
            DataRowView selectedRow = (DataRowView)dataGridBooks.SelectedItem;
            int bookId = (int)selectedRow["Id"];
            Window editWindow = new BookEditAdd("Редактирование",DatabaseHelper.FindBookById(bookId));
            editWindow.ShowDialog();
            LoadBooks();
        }
    }
}
