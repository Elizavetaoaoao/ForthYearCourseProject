using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;
using WpfAppTRPO.Models;

namespace WpfAppTRPO.Helpers
{
    public static class DatabaseHelper
    {
        private const string CONNECTION = @"Data Source=DESKTOP-1QKCL2P;Initial Catalog=LibraryBase;Integrated Security=True;TrustServerCertificate=True";
        public static Employee Authenticate(string login, string password)
        {
            //password = HashHelper.ComputeSha256Hash(password);
            using (SqlConnection conn = new SqlConnection(CONNECTION))
            {
                using (SqlCommand cmd = new SqlCommand("AuthenticateEmployee", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Login", login);
                    cmd.Parameters.AddWithValue("@Password", password);
                    try
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            return new Employee
                            {
                                Id = reader.GetInt32(0),
                                FullName = reader.GetString(1),
                                Name= reader.GetString(2),
                                MiddleName = reader.GetString(3),
                                Surname = reader.GetString(4),
                                Login = reader.GetString(5),
                                Role = reader.GetString(6),
                                Photo = reader.IsDBNull(7) ? null : reader.GetString(7)
                            };
                        }
                        conn.Close();
                        return null;
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show("Ошибка подключения к БД: " + ex.Message);
                        return null;
                    }
                }
            }
        }
        public static Book FindBookById(int id)
        {
            using (SqlConnection conn = new SqlConnection(CONNECTION))
            {
                using (SqlCommand cmd = new SqlCommand("FindBookById", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);
                    try
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            return new Book
                            {
                                Id = reader.GetInt32(0),
                                LibCode = reader.GetInt32(1),
                                Title = reader.GetString(2),
                                Author = reader.GetInt32(3),
                                Publisher = reader.GetString(4),
                                PublicationPlace = reader.GetString(5),
                                PublicationYear = reader.GetInt32(6),
                                Copies = reader.GetInt32(7),
                                AvailableCopies = reader.GetInt32(8)
                            };
                        }
                        conn.Close();
                        return null;
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show("Ошибка подключения к БД: " + ex.Message);
                        return null;
                    }
                }
            }
        }
        internal static DataTable GetAuthors()
        {
            try
            {
                DataTable authors = new DataTable();
                using (SqlConnection conn = new SqlConnection(CONNECTION))
                {
                    using (SqlCommand cmd = new SqlCommand("GetAllAuthors", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        conn.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(authors);
                        conn.Close();
                    }
                }
                return authors;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка загрузки данных об авторах.", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }
        public static DataTable GetAllBooks()
        {
            try
            {
                DataTable books = new DataTable();
                using (SqlConnection conn = new SqlConnection(CONNECTION))
                using (SqlCommand cmd = new SqlCommand("GetAllBooks", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(books);
                    conn.Close();
                }
                return books;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Ошибка подключения к БД: ",MessageBoxButton.OK,MessageBoxImage.Error);
                return null;
            }
        }
        public static bool BookExists(Book book)
        {
            using (SqlConnection conn = new SqlConnection(CONNECTION))
            {
                using (SqlCommand cmd = new SqlCommand("CheckBookExists", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Title", book.Title);
                    cmd.Parameters.AddWithValue("@AuthorId", book.Author);
                    cmd.Parameters.AddWithValue("@Publisher", (object)book.Publisher ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@PublicationPlace", (object)book.PublicationPlace ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@PublicationYear", (object)book.PublicationYear ?? DBNull.Value);
                    try
                    {
                        conn.Open();
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value) return true;
                        return false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка проверки дубликата", MessageBoxButton.OK,MessageBoxImage.Error);
                        return false;
                    }
                }
            }
        }
        internal static bool AddBook(Book book)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(CONNECTION))
                using (SqlCommand cmd = new SqlCommand("AddBook", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Code", book.LibCode);
                    cmd.Parameters.AddWithValue("@Title", book.Title);
                    cmd.Parameters.AddWithValue("@AuthorId", book.Author);
                    cmd.Parameters.AddWithValue("@Publisher", (object)book.Publisher ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@PublishedPlace", (object)book.PublicationPlace ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@PublishedYear", book.PublicationYear);
                    cmd.Parameters.AddWithValue("@Copies", book.Copies);
                    cmd.Parameters.AddWithValue("@AvailableCopies", book.Copies);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка добавления данных", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
        internal static bool UpdateBook(Book book)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(CONNECTION))
                using (SqlCommand cmd = new SqlCommand("UpdateBook", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", book.Id);
                    cmd.Parameters.AddWithValue("@Code", book.LibCode);
                    cmd.Parameters.AddWithValue("@Title", book.Title);
                    cmd.Parameters.AddWithValue("@Author", book.Author);
                    cmd.Parameters.AddWithValue("@Publisher", book.Publisher);
                    cmd.Parameters.AddWithValue("@PublishedPlace", book.PublicationPlace);
                    cmd.Parameters.AddWithValue("@PublishedYear", book.PublicationYear);
                    cmd.Parameters.AddWithValue("@Copies", book.Copies);
                    cmd.Parameters.AddWithValue("@AvailableCopies", book.AvailableCopies);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка обновления данных", MessageBoxButton.OK,MessageBoxImage.Error);
                return false;
            }
        }
        public static bool DeleteBook(int id)
        {
            string query = "DELETE FROM Books WHERE Id = @id";
            using (SqlConnection conn = new SqlConnection(CONNECTION))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                try
                {
                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка удаления: " + ex.Message);
                    return false;
                }
            }
        }

        internal static bool UpdateReader(Reader reader)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(CONNECTION))
                using (SqlCommand cmd = new SqlCommand("UpdateReader", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", reader.Id);
                    cmd.Parameters.AddWithValue("@FullName", reader.FullName);
                    cmd.Parameters.AddWithValue("@Name", reader.Name);
                    cmd.Parameters.AddWithValue("@MiddleName", reader.MiddleName);
                    cmd.Parameters.AddWithValue("@Surname", reader.Surname);
                    cmd.Parameters.AddWithValue("@Phone", reader.Phone);
                    cmd.Parameters.AddWithValue("@IsPerpetrator", reader.IsPerpetrator);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка обновления данных", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        internal static bool ReaderExists(Reader reader)
        {
            using (SqlConnection conn = new SqlConnection(CONNECTION))
            {
                using (SqlCommand cmd = new SqlCommand("CheckReaderExists", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FullName", reader.FullName);
                    cmd.Parameters.AddWithValue("@Name", reader.Name);
                    cmd.Parameters.AddWithValue("@MiddleName", reader.MiddleName);
                    cmd.Parameters.AddWithValue("@Surname", reader.Surname);
                    cmd.Parameters.AddWithValue("@Phone", reader.Phone);
                    try
                    {
                        conn.Open();
                        object result = cmd.ExecuteScalar();
                        conn.Close();
                        if (result != null && result != DBNull.Value) return true;
                        return false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка проверки дубликата", MessageBoxButton.OK, MessageBoxImage.Error);
                        conn.Close();
                        return false;
                    }
                }
            }
        }

        internal static bool AddReader(Reader reader)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(CONNECTION))
                using (SqlCommand cmd = new SqlCommand("AddReader", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FullName", reader.FullName);
                    cmd.Parameters.AddWithValue("@Name", reader.Name);
                    cmd.Parameters.AddWithValue("@MiddleName", reader.MiddleName);
                    cmd.Parameters.AddWithValue("@Surname", reader.Surname);
                    cmd.Parameters.AddWithValue("@Phone", reader.Phone);
                    cmd.Parameters.AddWithValue("@IsPerpetrator", Convert.ToByte(reader.IsPerpetrator));
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка добавления данных", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        internal static DataTable GetAllReaders()
        {
            try
            {
                DataTable readers = new DataTable();
                using (SqlConnection conn = new SqlConnection(CONNECTION))
                using (SqlCommand cmd = new SqlCommand("GetAllReaders", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(readers);
                    conn.Close();
                }
                return readers;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Ошибка подключения к БД: ", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        internal static bool DeleteReader(int readerId)
        {
            string query = "DELETE FROM Readers WHERE Id = @id";
            using (SqlConnection conn = new SqlConnection(CONNECTION))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", readerId);
                try
                {
                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    conn.Close();
                    return rows > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка удаления: " + ex.Message);
                    conn.Close();
                    return false;
                }
            }
        }

        internal static Reader FindReaderById(int readerId)
        {
            using (SqlConnection conn = new SqlConnection(CONNECTION))
            {
                using (SqlCommand cmd = new SqlCommand("FindReaderById", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", readerId);
                    try
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            return new Reader
                            {
                                Id = reader.GetInt32(0),
                                FullName = reader.GetString(1),
                                Name = reader.GetString(2),
                                MiddleName = reader.GetString(3),
                                Surname = reader.GetString(4),
                                Phone = reader.GetString(5),
                                IsPerpetrator = reader.GetBoolean(6)
                            };
                        }
                        conn.Close();
                        return null;
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show("Ошибка подключения к БД: " + ex.Message);
                        conn.Close();
                        return null;
                    }
                }
            }
        }

        internal static DataTable GetAllEmployees()
        {
            try
            {
                DataTable employees = new DataTable();
                using (SqlConnection conn = new SqlConnection(CONNECTION))
                using (SqlCommand cmd = new SqlCommand("GetAllEmployees", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(employees);
                    conn.Close();
                }
                return employees;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Ошибка подключения к БД: ", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        internal static bool DeleteEmployee(int empId)
        {
            string query = "DELETE FROM Employees WHERE Id = @id";
            using (SqlConnection conn = new SqlConnection(CONNECTION))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", empId);
                try
                {
                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка удаления: " + ex.Message);
                    return false;
                }
            }
        }

        internal static Employee FindEmployeeById(int empId)
        {
            using (SqlConnection conn = new SqlConnection(CONNECTION))
            {
                using (SqlCommand cmd = new SqlCommand("FindEmployeeById", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", empId);
                    try
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            return new Employee
                            {
                                Id = reader.GetInt32(0),
                                FullName = reader.GetString(1),
                                Name = reader.GetString(2),
                                MiddleName = reader.GetString(3),
                                Surname = reader.GetString(4),
                                Login = reader.GetString(5),
                                Password = reader.GetString(6),
                                Role = reader.GetString(7),
                                Photo = reader.GetString(8)
                            };
                        }
                        conn.Close();
                        return null;
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show("Ошибка подключения к БД: " + ex.Message);
                        conn.Close();
                        return null;
                    }
                }
            }
        }

        internal static bool UpdateEmployee(Employee employee)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(CONNECTION))
                using (SqlCommand cmd = new SqlCommand("UpdateEmployee", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", employee.Id);
                    cmd.Parameters.AddWithValue("@FullName", employee.FullName);
                    cmd.Parameters.AddWithValue("@Name", employee.Name);
                    cmd.Parameters.AddWithValue("@MiddleName", employee.MiddleName);
                    cmd.Parameters.AddWithValue("@Surname", employee.Surname);
                    cmd.Parameters.AddWithValue("@Login", employee.Login);
                    cmd.Parameters.AddWithValue("@Password", employee.Password);
                    cmd.Parameters.AddWithValue("@Role", employee.Role);
                    cmd.Parameters.AddWithValue("@Photo", employee.Photo);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка обновления данных", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        internal static bool EmployeeExists(Employee employee)
        {
            using (SqlConnection conn = new SqlConnection(CONNECTION))
            {
                using (SqlCommand cmd = new SqlCommand("CheckEmployeeExists", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FullName", employee.FullName);
                    cmd.Parameters.AddWithValue("@Name", employee.Name);
                    cmd.Parameters.AddWithValue("@MiddleName", employee.MiddleName);
                    cmd.Parameters.AddWithValue("@Surname", employee.Surname);
                    try
                    {
                        conn.Open();
                        object result = cmd.ExecuteScalar();
                        conn.Close();
                        if (result != null && result != DBNull.Value) return true;
                        return false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка проверки дубликата", MessageBoxButton.OK, MessageBoxImage.Error);
                        conn.Close();
                        return false;
                    }
                }
            }
        }
        internal static bool AddEmployee(Employee employee)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(CONNECTION))
                using (SqlCommand cmd = new SqlCommand("AddEmployee", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FullName", employee.FullName);
                    cmd.Parameters.AddWithValue("@Name", employee.Name);
                    cmd.Parameters.AddWithValue("@MiddleName", employee.MiddleName);
                    cmd.Parameters.AddWithValue("@Surname", employee.Surname);
                    cmd.Parameters.AddWithValue("@Login", employee.Login);
                    cmd.Parameters.AddWithValue("@Password", employee.Password);
                    cmd.Parameters.AddWithValue("@Role", employee.Role);
                    cmd.Parameters.AddWithValue("@Photo", employee.Photo);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка добавления данных", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
        internal static DataTable GetLog()
        {
            try
            {
                DataTable log = new DataTable();
                using (SqlConnection conn = new SqlConnection(CONNECTION))
                using (SqlCommand cmd = new SqlCommand("GetLog", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(log);
                    conn.Close();
                }
                return log;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Ошибка подключения к БД: ", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        internal static DataTable GetAvailableBooks(int bookId)
        {
            try
            {
                DataTable availableCopies = new DataTable();
                using (SqlConnection conn = new SqlConnection(CONNECTION))
                using (SqlCommand cmd = new SqlCommand("GetAvailableBooks", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", bookId);
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(availableCopies);
                    conn.Close();
                }
                return availableCopies;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Ошибка подключения к БД: ", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        internal static bool GiveBook(int readerId, int copyId, int librarianID, DateTime dueDate)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(CONNECTION))
                using (SqlCommand cmd = new SqlCommand("GiveBook", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ReaderID", readerId);
                    cmd.Parameters.AddWithValue("@CopyID", copyId);
                    cmd.Parameters.AddWithValue("@LibrarianID", librarianID);
                    cmd.Parameters.AddWithValue("@DateReturn", dueDate.Date);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка добавления данных", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
        public static DataTable ExecuteSearchProcedure(string keyword)
        {
            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                using (SqlCommand command = new SqlCommand("SearchBooks", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Keyword", keyword);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dataTable);
                }
            }
            return dataTable;
        }

        internal static DataTable GetReaderBooks(int readerId)
        {
            try
            {
                DataTable dataTable = new DataTable();

                using (SqlConnection connection = new SqlConnection(CONNECTION))
                {
                    using (SqlCommand command = new SqlCommand("GetReaderBooks", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ReaderId", readerId);

                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        adapter.Fill(dataTable);
                    }
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка добавления данных", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        internal static bool ReturnBook(int copyId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(CONNECTION))
                using (SqlCommand cmd = new SqlCommand("ReturnBook", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CopyID", copyId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка добавления данных", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
        internal static string GetLastAct(int libId)
        {
            string result="";
            try
            {
                using (SqlConnection conn = new SqlConnection(CONNECTION))
                using (SqlCommand cmd = new SqlCommand("GetLastAct", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LibID", libId);
                    var param = new SqlParameter("@stroka", SqlDbType.NVarChar, 100)
                    {
                        Direction = ParameterDirection.Output,
                    };
                    cmd.Parameters.Add(param);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    result = param.Value?.ToString() ?? "нет информации";
                    conn.Close();
                }
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка добавления данных", MessageBoxButton.OK, MessageBoxImage.Error);
                return result;
            }
        }
    }
}

