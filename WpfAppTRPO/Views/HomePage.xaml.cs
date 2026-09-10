using System;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using WpfAppTRPO.Helpers;
using WpfAppTRPO.Models;

namespace WpfAppTRPO.Views
{
    public partial class HomePage : Page
    {
        private Employee _employee;
        internal HomePage(Employee employee)
        {
            try
            {
                InitializeComponent();
                _employee = employee;
                textBlockName.Text = $"{employee.Surname} {employee.Name} {employee.MiddleName}";
                textBlockRole.Content = employee.Role;
                if (_employee.Role != "администратор")
                {
                    buttonEmployees.Visibility = Visibility.Hidden;
                }

                DataTable dt = DatabaseHelper.GetLog();
                int max = dt.Rows.Count-1;
                while (dt.Rows.Count != 5)
                {
                    dt.Rows.RemoveAt(max);
                    max--;
                }
                listViewLog.ItemsSource = dt.DefaultView;
                //imageEmployeePhoto.Source = new BitmapImage(new Uri(employee.Photo));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Ошибка при загрузке страницы HomaPage.",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }

        private void ImageEmployeePhoto_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            imageEmployeePhoto.Source = new BitmapImage(new Uri("C:\\Users\\Елизавета\\Documents\\KURSACH\\WpfAppTRPO\\WpfAppTRPO\\pics"));
        }
        private void ButtonCatalog_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new CatalogPage());
        }

        private void ButtonJournal_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new JournalPage(_employee.Id));
        }

        private void ButtonReaders_Click(object sender, RoutedEventArgs e)
        { NavigationService?.Navigate(new ReadersPage()); }
        private void ButtonEmployees_Click(object sender, RoutedEventArgs e)
        { NavigationService?.Navigate(new EmployeesPage()); }

    }
}
