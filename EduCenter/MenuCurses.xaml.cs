using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;

namespace EduCenter
{
    public partial class MenuCurses : Window
    {
        private string connectionString = "Server=LAPTOP-V0AGQKUF\\SLAUUUIK;Database=EduCenter;Trusted_Connection=True;";
        private List<Course> courses;

        public MenuCurses()
        {
            InitializeComponent();
            LoadCourses();
        }

        private void LoadCourses()
        {
            courses = new List<Course>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT Id, CourseName FROM Curses", connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var course = new Course
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1)
                    };
                    courses.Add(course);
                }
            }

            CoursesListBox.ItemsSource = courses;
        }
        

        private void DetailsButton_Click(object sender, RoutedEventArgs e)
        {
            if (CoursesListBox.SelectedItem is Course selectedCourse)
            {
                ShowCourseDetails(selectedCourse.Id);
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите курс.");
            }
        }
        private int currentUserId = 1; 

        private void EnrollButton_Click(object sender, RoutedEventArgs e)
        {
            if (CoursesListBox.SelectedItem is Course selectedCourse)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("INSERT INTO UserCourse (UserId, CourseId) VALUES (@UserId, @CourseId)", connection);
                    command.Parameters.AddWithValue("@UserId", currentUserId);
                    command.Parameters.AddWithValue("@CourseId", selectedCourse.Id);

                    try
                    {
                        command.ExecuteNonQuery();
                        MessageBox.Show("Вы успешно записаны на курс!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при записи на курс: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите курс для записи.");
            }
        }

        private void ShowCourseDetails(int courseId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT Description, Instructor, Price FROM Curses WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", courseId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string description = reader.GetString(0);
                        string instructor = reader.GetString(1);
                        decimal price = reader.GetDecimal(2);

                        MessageBox.Show($"Курс: {courses.Find(c => c.Id == courseId).Name}\nОписание: {description}\nИнструктор: {instructor}\nЦена: {price} руб.");
                    }
                    else
                    {
                        MessageBox.Show("Курс не найден.");
                    }
                }
            }
        }
        private void MyCoursesButton_Click(object sender, RoutedEventArgs e)
        {
            MyCurses myCursesWindow = new MyCurses(currentUserId);
            myCursesWindow.Show();
        }

        
    }
}

    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

