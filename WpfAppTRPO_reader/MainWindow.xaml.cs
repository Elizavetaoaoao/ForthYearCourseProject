using System;
using System.Collections.Generic;
using System.Data;
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

namespace WpfAppTRPO_reader
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                dataGridMain.ItemsSource = DatabaseHelper.ExecuteSearchProcedure("").DefaultView;
            }
            catch { }
        }

        private void buttonMain_Click(object sender, RoutedEventArgs e)
        {
            string keyword = textBoxMain.Text.Trim();
            if (string.IsNullOrEmpty(keyword))
            {
                return;
            }
            try
            {
                Cursor = System.Windows.Input.Cursors.Wait;
                DataTable results = DatabaseHelper.ExecuteSearchProcedure(keyword);
                dataGridMain.ItemsSource = results.DefaultView;
            }
            catch (Exception ex) { }
            finally
            {
                Cursor = null;
            }
        }
    }
}
