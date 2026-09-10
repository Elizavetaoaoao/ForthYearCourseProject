using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfAppTRPO.Helpers;

namespace WpfAppTRPO.Views
{
    public partial class JournalPage : Page
    {
        private int librarianID;
        public JournalPage(int librarianID)
        {
            InitializeComponent();
            LoadData();
            this.librarianID = librarianID;
        }
        private void LoadData()
        {
            dataGridLog.ItemsSource = DatabaseHelper.GetLog().DefaultView;
        }

        private void ButtonGiveBook_Click(object sender, RoutedEventArgs e)
        {
            Window editWindow = new LogGiveBook(librarianID);
            editWindow.ShowDialog();
            LoadData();
        }

        private void ButtonReturn_Click(object sender, RoutedEventArgs e)
        {
            Window editWindow = new LogReturnBook(librarianID);
            editWindow.ShowDialog();
            LoadData();
        }

        private void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
        private void DgEmp_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dataGridLog.SelectedItem == null) return;
            DataRowView selectedRow = (DataRowView)dataGridLog.SelectedItem;
            int empId = (int)selectedRow["Id"];
            Window editWindow = new EmployeeEditAdd("Редактирование", DatabaseHelper.FindEmployeeById(empId));
            editWindow.ShowDialog();
            LoadData();
        }
    }
}
