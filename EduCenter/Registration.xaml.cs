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
using System.Windows.Shapes;

namespace EduCenter
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        private string connectionString = "Server=LAPTOP-V0AGQKUF\\SLAUUUIK;Database=EduCenter;Trusted_Connection=True;";

        public Registration()
        {
            InitializeComponent();
        }
        private bool RegisterUser(User user)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Users (FullName, Username, Password, Gender, DateOfBirth) VALUES (@FullName, @Username, @Password, @Gender, @DateOfBirth)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FullName", user.FullName);
                        command.Parameters.AddWithValue("@Username", user.Username);
                        command.Parameters.AddWithValue("@Password", user.Password);
                        command.Parameters.AddWithValue("@Gender", user.Gender);
                        command.Parameters.AddWithValue("@DateOfBirth", user.DateOfBirth);

                        int result = command.ExecuteNonQuery();
                        return result > 0; 
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string fullName = FullNameTextBox.Text; 
            string login = LoginTextBox.Text; 
            string password = PasswordBox.Password; 
            string gender = GenderComboBox.SelectedItem.ToString(); 
            DateTime dateOfBirth = DatePicker.SelectedDate ?? DateTime.Now; 

            User newUser = new User
            {
                FullName = fullName,
                Username = login,
                Password = password,
                Gender = gender,
                DateOfBirth = dateOfBirth
            };

            if (RegisterUser(newUser))
            {
                MessageBox.Show("Регистрация прошла успешно!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close(); 
            }
            else
            {
                MessageBox.Show("Не удалось зарегистрировать пользователя. Попробуйте еще раз.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }
}
