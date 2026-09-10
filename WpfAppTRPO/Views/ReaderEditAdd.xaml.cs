using System;
using System.Windows;
using System.Windows.Input;
using WpfAppTRPO.Helpers;
using WpfAppTRPO.Models;

namespace WpfAppTRPO.Views
{
    public partial class ReaderEditAdd : Window
    {
        private Reader reader;
        public ReaderEditAdd(string Title)
        {
            InitializeComponent();
            this.Title= Title;
            ReaderAddInterface();
        }
        internal ReaderEditAdd(string Title, Reader reader)
        {
            InitializeComponent();
            this.Title = Title;
            this.reader = reader;
            ReaderEditInterface();
        }
        private void ReaderAddInterface()
        {
            buttonSave.Click -= ButtonSaveUpdate_Click;
            buttonSave.Click += ButtonSaveAdd_Click;
        }

        private void ReaderEditInterface()
        {
            buttonSave.Click += ButtonSaveUpdate_Click;
            if (reader == null)
            {
                MessageBox.Show("Читатель не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                DialogResult = false;
                Close();
                return;
            }
            textBoxName.Text = reader.Name.ToString();
            textBoxMiddle.Text = reader.MiddleName.ToString();
            textBoxSurname.Text = reader.Surname.ToString();
            textBoxPhone.Text = reader.Phone.ToString();
            checkBoxPerpetrator.IsChecked = reader.IsPerpetrator;
        }
        private void ButtonSaveUpdate_Click(object sender, RoutedEventArgs e)
        {
            string validation = ValidateFields();
            if (validation != string.Empty)
            {
                MessageBox.Show(validation, "Ошибка введенных данных", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            reader.Name = textBoxName.Text.Trim();
            reader.MiddleName = textBoxMiddle.Text.Trim();
            reader.Surname = textBoxSurname.Text.Trim();
            reader.FullName = $"{reader.Surname} {reader.Name[0]}.{reader.MiddleName[0]}.";
            reader.Phone = textBoxPhone.Text.Trim();
            reader.IsPerpetrator=Convert.ToBoolean(checkBoxPerpetrator.IsChecked);
            if (DatabaseHelper.UpdateReader(reader))
            {
                Close();
            }
            else
            {
                MessageBox.Show("Не удалось сохранить читателя. Проверьте данные и попробуйте снова.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ButtonSaveAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string validation = ValidateFields();
                if (validation != string.Empty)
                {
                    MessageBox.Show(validation, "Ошибка введенных данных", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                reader = new Reader();
                reader.Name = textBoxName.Text.Trim();
                    reader.MiddleName = textBoxMiddle.Text.Trim();
                reader.Surname = textBoxSurname.Text.Trim();
                reader.FullName = $"{reader.Surname} {reader.Name[0]}.{reader.MiddleName[0]}.";
                reader.Phone = Convert.ToString(textBoxPhone.Text.Trim());
                reader.IsPerpetrator = (checkBoxPerpetrator.IsChecked == true) ? true : false;
                
                if (DatabaseHelper.ReaderExists(reader))
                {
                    MessageBox.Show("Такой читатель уже существует.", "Дубликат", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (DatabaseHelper.AddReader(reader))
                {
                    Close();
                }
                else
                {
                    MessageBox.Show("Не удалось добавить читателя. Проверьте данные и попробуйте снова.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {

            }
        }
        private string ValidateFields()
        {
            string message = string.Empty;
            if (string.IsNullOrWhiteSpace(textBoxName.Text))
            {
                message = "Введите имя читателя.";
            }
            if (string.IsNullOrWhiteSpace(textBoxSurname.Text))
            {
                message = "Введите фамилию читателя.";
            }
            if (string.IsNullOrWhiteSpace(textBoxPhone.Text))
            {
                message = "Введите номер телефона читателя.";
            }
            return message;
        }
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                if (!char.IsDigit(e.Text, 0))
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
