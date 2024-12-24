using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;

namespace EduCenter
{
    public partial class MyCurses : Window
    {
        private string connectionString = "Server=LAPTOP-V0AGQKUF\\SLAUUUIK;Database=EduCenter;Trusted_Connection=True;";
        private int currentUserId;

        public MyCurses(int userId)
        {
            InitializeComponent();
            currentUserId = userId;
            LoadUserCourses();
        }

        private void LoadUserCourses()
        {
            List<Course> userCourses = new List<Course>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT C.Id, C.CourseName FROM UserCourse UC JOIN Curses C ON UC.CourseId = C.Id WHERE UC.UserId = @UserId", connection);
                command.Parameters.AddWithValue("@UserId", currentUserId);

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var course = new Course
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1)
                    };
                    userCourses.Add(course);
                }
            }

            UserCoursesListBox.ItemsSource = userCourses;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
