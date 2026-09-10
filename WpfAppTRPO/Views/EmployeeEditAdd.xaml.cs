using System.Windows;
using System.Windows.Input;
using WpfAppTRPO.Helpers;
using WpfAppTRPO.Models;

namespace WpfAppTRPO.Views
{
    public partial class EmployeeEditAdd : Window
    {
        Employee employee;
        public EmployeeEditAdd(string Title)
        {
            InitializeComponent();
            this.Title = Title;
            FillComboBox();
            EmpAddInterface();
        }
        internal EmployeeEditAdd(string Title, Employee emp)
        {
            InitializeComponent();
            this.Title = Title;
            employee = emp;
            FillComboBox();
            EmpEditInterface();
        }
        private void FillComboBox()
        {
            comboBoxRoles.Items.Clear();
            comboBoxRoles.ItemsSource = new object[] { "администратор", "библиотекарь" };
        }
        private void EmpAddInterface()
        {
            buttonSave.Click -= ButtonSaveUpdate_Click;
            buttonSave.Click += ButtonSaveAdd_Click;
        }
        private void EmpEditInterface()
        {
            buttonSave.Click += ButtonSaveUpdate_Click;
            if (employee == null)
            {
                MessageBox.Show("Сотрудник не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                DialogResult = false;
                Close();
                return;
            }
            textBoxName.Text = employee.Name.ToString();
            textBoxMiddle.Text = employee.MiddleName.ToString();
            textBoxSurname.Text = employee.Surname.ToString();
            textBoxLogin.Text = employee.Login.ToString();
            textBoxPassword.Text = employee.Password.ToString();
            comboBoxRoles.SelectedIndex = (employee.Role == "библиотекарь") ? 1 : 0;
        }

        private void ButtonSaveUpdate_Click(object sender, RoutedEventArgs e)
        {
            string validation = ValidateFields();
            if (validation != string.Empty)
            {
                MessageBox.Show(validation, "Ошибка введенных данных", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            employee.Name = textBoxName.Text.Trim();
            employee.MiddleName = textBoxMiddle.Text.Trim();
            employee.Surname = textBoxSurname.Text.Trim();
            employee.FullName = $"{employee.Surname} {employee.Name[0]}.{employee.MiddleName[0]}.";
            employee.Login = textBoxLogin.Text.Trim();
            employee.Password = textBoxPassword.Text.Trim();
            employee.Role = comboBoxRoles.SelectedItem.ToString();

            if (DatabaseHelper.UpdateEmployee(employee))
            {
                Close();
            }
            else
            {
                MessageBox.Show("Не удалось сохранить сотрудника. Проверьте данные и попробуйте снова.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ButtonSaveAdd_Click(object sender, RoutedEventArgs e)
        {
            string validation = ValidateFields();
            if (validation != string.Empty)
            {
                MessageBox.Show(validation, "Ошибка введенных данных", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            employee = new Employee();
            employee.Name = textBoxName.Text.Trim();
            employee.MiddleName = textBoxMiddle.Text.Trim();
            employee.Surname = textBoxSurname.Text.Trim();
            employee.FullName = $"{employee.Surname} {employee.Name[0]}.{employee.MiddleName[0]}.";
            employee.Login = textBoxLogin.Text.Trim();
                employee.Password = textBoxPassword.Text.Trim();
            employee.Role = comboBoxRoles.SelectedItem.ToString();
            employee.Photo = "";
            
            if (DatabaseHelper.EmployeeExists(employee))
            {
                MessageBox.Show("Такой сотрудник уже существует.", "Дубликат", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (DatabaseHelper.AddEmployee(employee))
            {
                Close();
            }
            else
            {
                MessageBox.Show("Не удалось добавить сотрудника. Проверьте данные и попробуйте снова.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private string ValidateFields()
        {
            string message = string.Empty;
            if (string.IsNullOrWhiteSpace(textBoxName.Text))
            {
                message = "Введите имя сотрудника.";
            }
            if (string.IsNullOrWhiteSpace(textBoxSurname.Text))
            {
                message = "Введите фамилию сотрудника.";
            }
            if (string.IsNullOrWhiteSpace(textBoxLogin.Text))
            {
                message = "Введите логин сотрудника.";
            }
            if (string.IsNullOrWhiteSpace(textBoxPassword.Text))
            {
                message = "Введите пароль сотрудника.";
            }
            return message;
        }
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }
    }
}
