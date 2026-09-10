using System;
using System.Data;
using System.Windows;
using System.Windows.Input;
using WpfAppTRPO.Helpers;
using WpfAppTRPO.Models;

namespace WpfAppTRPO.Views
{
    public partial class BookEditAdd : Window
    {
        private Book book;
        private DataTable authors;
        internal BookEditAdd(string Title, Book book)
        {
            InitializeComponent();
            LoadAuthors();
            this.Title = Title;
            this.book = book;
            BookEditInterface();
        }
        internal BookEditAdd(string Title)
        {
            InitializeComponent();
            LoadAuthors();
            this.Title = Title;
            BookAddInterface();
        }
        private void LoadAuthors()
        {
            authors = DatabaseHelper.GetAuthors();
            comboBoxAuthors.ItemsSource = authors.DefaultView;
            comboBoxAuthors.SelectedValuePath = "ID";
            comboBoxAuthors.DisplayMemberPath = "NameInitials";
            comboBoxAuthors.SelectedIndex = 0;
        }
        private void BookEditInterface()
        {
            buttonSave.Click += ButtonSaveUpdate_Click;
            if (book==null)
            {
                MessageBox.Show("Книга не найдена.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                DialogResult = false;
                Close();
                return;
            }
            textBoxLibCode.Text = book.LibCode.ToString();
            textBoxTitle.Text = book.Title.ToString();
            int authorId = Convert.ToInt32(book.Author);
            foreach (DataRowView authorRow in comboBoxAuthors.Items)
            {
                if (Convert.ToInt32(authorRow["ID"]) == authorId)
                {
                    comboBoxAuthors.SelectedItem = authorRow;
                    break;
                }
            }
            textBoxPublisher.Text= book.Publisher.ToString();
            textBoxPublicationPlace.Text= book.PublicationPlace.ToString();
            textBoxPublicationYear.Text= book.PublicationYear.ToString();
            textBoxCopies.Text= book.Copies.ToString();
        }
        private void BookAddInterface()
        {
            buttonSave.Click -= ButtonSaveUpdate_Click;
            buttonSave.Click+= ButtonSaveAdd_Click;
        }
        private void ButtonSaveUpdate_Click(object sender, RoutedEventArgs e)
        {
            string validation = ValidateFields();
            if (validation != string.Empty)
            {
                MessageBox.Show(validation, "Ошибка введенных данных", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            book.LibCode = Convert.ToInt32(textBoxLibCode.Text);
            book.Title = textBoxTitle.Text.Trim();
            book.Author = (int)((DataRowView)comboBoxAuthors.SelectedItem)["Id"];
            book.Publisher = textBoxPublisher.Text.Trim();
            book.PublicationPlace = textBoxPublicationPlace.Text.Trim();
            book.PublicationYear = Convert.ToInt32(textBoxPublicationYear.Text);
            book.Copies = Convert.ToInt32(textBoxCopies.Text);
            if (DatabaseHelper.UpdateBook(book))
            {
                Close();
            }
            else
            {
                MessageBox.Show("Не удалось сохранить книгу. Проверьте данные и попробуйте снова.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
            book = new Book
            {
                LibCode = Convert.ToInt32(textBoxLibCode.Text),
                Title = textBoxTitle.Text.Trim(),
                Author = (int)((DataRowView)comboBoxAuthors.SelectedItem)["Id"],
                Publisher = textBoxPublisher.Text.Trim(),
                PublicationPlace = textBoxPublicationPlace.Text.Trim(),
                PublicationYear = Convert.ToInt32(textBoxPublicationYear.Text),
                Copies = Convert.ToInt32(textBoxCopies.Text)
            };      
            if (DatabaseHelper.BookExists(book))
            {
                MessageBox.Show("Книга с таким названием, автором, издательством, годом и городом уже существует.", "Дубликат", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (DatabaseHelper.AddBook(book))
            {
                Close();
            }
            else
            {
                MessageBox.Show("Не удалось сохранить книгу. Проверьте данные и попробуйте снова.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private string ValidateFields()
        {
            string message=string.Empty;
            if (string.IsNullOrWhiteSpace(textBoxLibCode.Text))
            {
                message = "Введите шифр книги";
            }
            if (string.IsNullOrWhiteSpace(textBoxTitle.Text))
            {
                message = "Введите название книги";
            }
            if (comboBoxAuthors.SelectedItem == null)
            {
                message = "Выберите автора";
            }
            if (string.IsNullOrWhiteSpace(textBoxCopies.Text) || !int.TryParse(textBoxCopies.Text, out int total))
            {
                message = "Введите корректное количество копий";
            }
            return message;
        }
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
