using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для Kurs.xaml
    /// </summary>
    public partial class Kurs : Window
    {
        private User CurrentUser { get; set; }
        private int currentUserId = 1;
        public Kurs(User user)
        {
            InitializeComponent();
            CurrentUser = user;
            LoadUserData();
        }

        private void LoadUserData()
        {
            if (CurrentUser != null)
            {
                UserNameLabel.Content = CurrentUser.FullName; 
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MyCurses myCurses = new MyCurses(currentUserId);
            myCurses.Show();
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MenuCurses menuCurses = new MenuCurses();
            menuCurses.Show();
            Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }
    }

}
