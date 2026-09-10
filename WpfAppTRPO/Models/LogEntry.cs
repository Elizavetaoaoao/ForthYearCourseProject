using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppTRPO.Models
{
    public class LogEntry
    {
        public int ID { get; set; }
        public int BookCopyId { get; set; }
        public int ReaderId { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public int LibrarianId { get; set; }
        // для отображения
        public string BookTitle { get; set; }        
        public string ReaderFullName { get; set; }
        public string LibrarianName { get; set; }
    }
}
