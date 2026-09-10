using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Windows.Threading;
using WpfAppTRPO.Models;

namespace WpfAppTRPO.Views
{
    public partial class MainWindow : Window
    {
        private Employee _employee;
        private DispatcherTimer _timer;
        internal MainWindow(Employee employee)
        {
            InitializeComponent();
            _employee = employee;
            MainFrame.Navigate(new HomePage(_employee));
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += (s, e) => textBoxDateTime.Text = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
            _timer.Start();
            textBoxDateTime.Text = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void buttonMain_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new HomePage(_employee));
        }
        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.CanGoBack)
                MainFrame.GoBack();
        }
    }
}
