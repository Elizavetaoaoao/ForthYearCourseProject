using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfAppTRPO.Helpers;

namespace WpfAppTRPO.Views
{
    public partial class ReadersPage : Page
    {
        public ReadersPage()
        {
            InitializeComponent();
            LoadReaders();
        }

        private void LoadReaders()
        {
           dataGridReaders.ItemsSource = DatabaseHelper.GetAllReaders().DefaultView;

        }


        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            Window editWindow = new ReaderEditAdd("Новый читатель");
            editWindow.ShowDialog();
            LoadReaders();
        }


        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridReaders.SelectedItem == null)
            {
                MessageBox.Show("Выберите читателя для удаления.", "Удаление", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            DataRowView selectedRow = (DataRowView)dataGridReaders.SelectedItem;
            int readerId = (int)selectedRow["Id"];
            if (MessageBox.Show("Удалить выбранного читателя?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                bool deleted = DatabaseHelper.DeleteReader(readerId);
                if (deleted)
                {
                    LoadReaders();
                    MessageBox.Show("Читатель удален.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Не удалось удалить читателя.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadReaders();
        }
        private void DgReaders_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dataGridReaders.SelectedItem == null) return;
            DataRowView selectedRow = (DataRowView)dataGridReaders.SelectedItem;
            int readerId = (int)selectedRow["Id"];
            Window editWindow = new ReaderEditAdd("Редактирование", DatabaseHelper.FindReaderById(readerId));
            editWindow.ShowDialog();
            LoadReaders();
        }
    }
}
