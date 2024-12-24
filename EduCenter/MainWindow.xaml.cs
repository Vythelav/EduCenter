using System;
using System.Collections.Generic;
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

namespace EduCenter 
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string connectionString = "Server=LAPTOP-V0AGQKUF\\SLAUUUIK;Database=EduCenter;Trusted_Connection=True;";

        public MainWindow()
        {
            InitializeComponent();
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = LoginTextBox.Text;
            string password = PasswordBox.Password;

            if (username.Equals("admin") && password.Equals("admin"))
            {
                admin admin = new admin();
                admin.Show();
                this.Close();
            }
            else
            {
                User user = ValidateUser(username, password);
                if (user != null)
                {
                    Kurs kursWindow = new Kurs(user);
                    kursWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }



        private User ValidateUser(string username, string password)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT FullName, Username FROM Users WHERE Username = @username AND Password = @password";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@password", password);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new User
                                {
                                    FullName = reader["FullName"].ToString(),
                                    Username = reader["Username"].ToString()
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return null;
        }


        private void Registr_Click(object sender, RoutedEventArgs e)
        {
            Registration registration = new Registration();
            registration.Show();
            Close();

        }

       
    }
}

    

