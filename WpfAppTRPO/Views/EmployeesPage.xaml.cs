using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfAppTRPO.Helpers;

namespace WpfAppTRPO.Views
{
    public partial class EmployeesPage : Page
    {
        public EmployeesPage()
        {
            InitializeComponent();
            LoadEmployees();
        }

        private void LoadEmployees()
        {
            dataGridEmployees.ItemsSource = DatabaseHelper.GetAllEmployees().DefaultView;
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            Window editWindow = new EmployeeEditAdd("Добавление сотрудника");
            editWindow.ShowDialog();
            LoadEmployees();
        }

        private void DgEmp_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dataGridEmployees.SelectedItem == null) return;
            DataRowView selectedRow = (DataRowView)dataGridEmployees.SelectedItem;
            int empId = (int)selectedRow["Id"];
            Window editWindow = new EmployeeEditAdd("Редактирование", DatabaseHelper.FindEmployeeById(empId));
            editWindow.ShowDialog();
            LoadEmployees();
        }
        private void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        { LoadEmployees(); }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridEmployees.SelectedItem == null)
            {
                MessageBox.Show("Выберите сотрудника для удаления.", "Удаление", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            DataRowView selectedRow = (DataRowView)dataGridEmployees.SelectedItem;
            int empId = (int)selectedRow["Id"];
            if (MessageBox.Show("Удалить выбранного сотрудника?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                bool deleted = DatabaseHelper.DeleteEmployee(empId);
                if (deleted)
                {
                    LoadEmployees();
                    MessageBox.Show("Сотрудник удален.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Не удалось удалить сотрудника.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
