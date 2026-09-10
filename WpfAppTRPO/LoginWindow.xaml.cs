using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using WpfAppTRPO.Models;

namespace WpfAppTRPO.Views
{
    public partial class LoginWindow : Window
    {
        private string WARNING_NULL_TEXT = "Заполните поля логин и пароль";
        private string WARNING_WRONG_DATA = "Неверный логин или пароль";
        public LoginWindow()
        {
            InitializeComponent();
            textBoxLogin.Text = "shukina.vp";
            textBoxPassword.Text = "Ork902a";
        }
        private void ButtonConfirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string login = textBoxLogin.Text;
                string password = textBoxPassword.Text;
                if (!ValidateTextBoxes(login, password)) return;
                Employee emp = DatabaseHelper.Authenticate(login, password);
                if (emp == null)
                {
                    labelWarning.Content = WARNING_WRONG_DATA;
                    return;
                }
                MainWindow main = new MainWindow(emp);
                this.Hide();
                main.ShowDialog();
                this.Show();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateTextBoxes(textBoxLogin.Text,textBoxPassword.Text);
        }
        private bool ValidateTextBoxes(string login, string password) {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                labelWarning.Content = WARNING_NULL_TEXT;
                return false;
            }
            else
            {
                return true;
            }
        }
    }

}
