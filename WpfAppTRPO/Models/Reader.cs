using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppTRPO.Models
{
    internal class Reader : INotifyPropertyChanged
    {
        internal int Id { get; set; }
        internal string FullName { get; set; }
        internal string Name { get; set; }
        internal string MiddleName { get; set; }
        internal string Surname { get; set; }
        internal string Phone { get; set; }
        internal bool IsPerpetrator { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
