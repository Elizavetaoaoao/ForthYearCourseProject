using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WpfAppTRPO.Models;
using WpfAppTRPO.Helpers;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod_BookExists()
        {
            Book book = new Book { 
            Title="1984",
            Author=1,
            Publisher="Книжечка",
            PublicationPlace="Москва",
            PublicationYear=1984};
            bool result_test=DatabaseHelper.BookExists(book);
            Assert.IsFalse(result_test);
        }
        [TestMethod]
        public void TestMethod_DeleteBook() {
            bool result_test = DatabaseHelper.DeleteBook(100);
            Assert.IsFalse(result_test);
        }
        [TestMethod]
        public void TestMethod_FindBook() {
            Book book = DatabaseHelper.FindBookById(3);
            Assert.IsNotNull(book);
        }
    }
}
