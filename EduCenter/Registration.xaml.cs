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
        private string connectionString = "Server=510EC15;Database=EduCenter;Trusted_Connection=True;";

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
                    string query = "INSERT INTO Users (FullName, Username, Password, Post, IsBlocked) VALUES (@FullName, @Username, @Password, @Post, @IsBlocked)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FullName", user.FullName);
                        command.Parameters.AddWithValue("@Username", user.Username);
                        command.Parameters.AddWithValue("@Password", user.Password);
                        command.Parameters.AddWithValue("@Post", user.Post.Replace("System.Windows.Controls.ComboBoxItem:", ""));
                        command.Parameters.AddWithValue("@IsBlocked", 1);

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
            string password = PasswordBox.Text; 
            string post = GenderComboBox.SelectedItem.ToString(); 

            User newUser = new User
            {
                FullName = fullName,
                Username = login,
                Password = password,
                Post = post,
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<char> characters = new List<char>();
            characters.AddRange("abcdefghijklmnopqrstuvwxyz".ToCharArray());
            characters.AddRange("ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray());
            characters.AddRange("0123456789".ToCharArray());
            characters.AddRange("!\"No ; % :?".ToCharArray());

           
            Random random = new Random();
            int length = random.Next(12, 17);

           
            string password = "";
            for (int i = 0; i < length; i++)
            {
                
                int index = random.Next(characters.Count);
                password += characters[index];
            }


            PasswordBox.Text = password;
        }
    }
    }

