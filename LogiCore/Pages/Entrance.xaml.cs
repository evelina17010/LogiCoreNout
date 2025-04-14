using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using LogiCore.Connection;
using LogiCore.Function;
using LogiCore.Pages;

namespace LC.Pages
{
    /// <summary>
    /// Логика взаимодействия для Entrance.xaml
    /// </summary>
    public partial class Entrance : Page
    {
        public static User user;

        public Entrance()
        {
            InitializeComponent();
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new Registration());
        }

        private void TextBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            Reg1.Foreground = new SolidColorBrush(Colors.Pink);
        }

        private void TextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            Reg1.Foreground = new SolidColorBrush(Colors.Red);
        }

        private void btnVhod_Click(object sender, RoutedEventArgs e)
        {
            string email = Convert.ToString(txbEmail.Text.Trim());
            string password = Convert.ToString(txbPassword.Text.Trim());          
        user= Authorisation.AuthorisationSotr(email,password);
            if (user != null)
            {
                if (user.Role_id == 1)
                {
                    NavigationService.Navigate(new Admin(user));

                }
                else if (user.Role_id == 2)
                {
                    NavigationService.Navigate(new Client(user));

                }
                else if (user.Role_id == 4)
                {
                    NavigationService.Navigate(new Manager(user));

                }
                else if (user.Role_id == 3)
                {
                    NavigationService.Navigate(new Drivers(user));

                }
            }
            else MessageBox.Show("Логин или пароль неверный", "error", MessageBoxButton.OK, MessageBoxImage.Error);       
        }

        private void TextBlock_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new Main());
        }
    }
}
