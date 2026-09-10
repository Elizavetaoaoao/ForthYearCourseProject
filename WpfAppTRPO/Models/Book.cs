using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppTRPO.Models
{
    public class Book: INotifyPropertyChanged
    {
        private int _id;
        private int _libCode;
        private string _title;
        private int _author;
        private string _authorName;
        private string _publisher;
        private int _publicationYear;
        private string _publicationPlace;
        private int _copies;
        private int _availableCopies;
        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(); }
        }

        public int LibCode
        {
            get => _libCode;
            set { _libCode = value; OnPropertyChanged(); }
        }

        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(); }
        }
        public int Author
        {
            get => _author;
            set { _author = value; OnPropertyChanged(); }
        }
        public string AuthorName
        {
            get => _authorName;
            set { _authorName = value; OnPropertyChanged(); }
        }

        public string Publisher
        {
            get => _publisher;
            set { _publisher = value; OnPropertyChanged(); }
        }

        public int PublicationYear
        {
            get => _publicationYear;
            set { _publicationYear = value; OnPropertyChanged(); }
        }

        public string PublicationPlace
        {
            get => _publicationPlace;
            set { _publicationPlace = value; OnPropertyChanged(); }
        }
        public int Copies
        {
            get => _copies;
            set { _copies = value; OnPropertyChanged(); }
        }

        public int AvailableCopies
        {
            get => _availableCopies;
            set { _availableCopies = value; OnPropertyChanged(); }
        }
        

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
