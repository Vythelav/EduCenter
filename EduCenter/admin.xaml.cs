using EduCenter;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace EduCenter
{
    /// <summary>    
    /// Логика взаимодействия для admin.xaml    
    /// </summary>    
    public partial class admin : Window
    {
        private List<User> users;
        private string connectionString = "Server=510EC15;Database=EduCenter;Trusted_Connection=True;";

        public admin()
        {
            InitializeComponent();
            LoadUsers();
        }

        private void LoadUsers()
        {
            users = GetUsersFromDatabase();
            UsersListBox.ItemsSource = users;
        }

        private List<User> GetUsersFromDatabase()
        {
            List<User> userList = new List<User>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString)) 
                {
                    connection.Open();
                    string query = "SELECT Id, Username, Password, IsBlocked FROM Users";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                User user = new User
                                {
                                    Id = (int)reader["Id"],
                                    Username = reader["Username"].ToString(),
                                    Password = reader["Password"].ToString(),
                                    IsBlocked = (bool)reader["IsBlocked"]
                                };
                                userList.Add(user);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex) 
            {
                MessageBox.Show($"Ошибка при работе с базой данных: {ex.Message}");
            }
            catch (Exception ex) 
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}");
            }

            return userList;
        }

        private void BlockButton_Click(object sender, RoutedEventArgs e)
        {
            if (UsersListBox.SelectedItem is User selectedUser)
            {
                selectedUser.IsBlocked = true;
                UpdateUserStatusInDatabase(selectedUser);   
                MessageBox.Show($"Пользователь {selectedUser.Username} заблокирован.");
                LoadUsers(); 
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите пользователя для блокировки.");
            }
        }

        private void UnblockButton_Click(object sender, RoutedEventArgs e)
        {
            if (UsersListBox.SelectedItem is User selectedUser)
            {
                selectedUser.IsBlocked = false;
                UpdateUserStatusInDatabase(selectedUser); 
                MessageBox.Show($"Пользователь {selectedUser.Username} разблокирован.");
                LoadUsers();  
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите пользователя для разблокировки.");
            }
        }

        private User GetUserFromDatabase(string username)
        {
            User user = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString)) 
                {
                    connection.Open();
                    string query = "SELECT Id, Username, Password, IsBlocked FROM Users WHERE Username = @Username";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                user = new User
                                {
                                    Id = (int)reader["Id"],
                                    Username = reader["Username"].ToString(),
                                    Password = reader["Password"].ToString(),
                                    IsBlocked = (bool)reader["IsBlocked"]
                                };
                            }
                        }
                    }
                }
            }
            catch (SqlException ex) 
            {
                MessageBox.Show($"Ошибка при работе с базой данных: {ex.Message}");
            }
            catch (Exception ex) 
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}");
            }

            return user;
        }

        public bool Login(string username, string password)
        {
            User user = GetUserFromDatabase(username);    

            if (user == null)
            {
                MessageBox.Show("Пользователь не найден.");
                return false;
            }

            if (user.IsBlocked)
            {
                MessageBox.Show("Ваша учетная запись заблокирована. Обратитесь к администратору.");
                return false;
            }

            if (user.Password == password) 
            {
                return true; 
            }

            MessageBox.Show("Неверный пароль."); 
            return false; 
        }

        private void UpdateUserStatusInDatabase(User user)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE Users SET IsBlocked = @IsBlocked WHERE Id = @Id";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IsBlocked", user.IsBlocked);
                        command.Parameters.AddWithValue("@Id", user.Id);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Ошибка при обновлении статуса пользователя: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}");
            }
        }
    }
}
