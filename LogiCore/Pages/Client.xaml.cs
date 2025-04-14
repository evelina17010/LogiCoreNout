using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using LogiCore.Connection;
using LogiCore.Function;
using LogiCore.Pages;

namespace LC.Pages
{
    /// <summary>
    /// Логика взаимодействия для Client.xaml
    /// </summary>
    public partial class Client : Page
    {

        public static ObservableCollection<User> users { get; set; }
        private User _user;

        public Client(User user)
        {
            InitializeComponent();  
            _user = user;
            DataContext = _user;
        }
  

        private void btnhistory_Click(object sender, RoutedEventArgs e)
        {
            frClient.NavigationService.Navigate(new ClientHistory(_user.User_id));
        }

        private void btnexit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Main());
        }

        private void btnrequest_Click(object sender, RoutedEventArgs e)
        {
            frClient.NavigationService.Navigate(new Clientrequest(_user.User_id));
        }
        private void btnPay_Click(object sender, RoutedEventArgs e)
        {
            frClient.NavigationService.Navigate(new ClientPay(_user.User_id));
        }

    }
}
