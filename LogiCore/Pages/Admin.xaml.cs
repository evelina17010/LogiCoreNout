using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
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
using System.Xml.Linq;
using LC.Pages;
using LogiCore.Connection;

namespace LogiCore.Pages
{
    /// <summary>
    /// Логика взаимодействия для Admin.xaml
    /// </summary>
    public partial class Admin : Page
    {
        private User _user;
        public static ObservableCollection<User> users { get; set; }
        public Admin(User admin_user)
        {
            InitializeComponent();
            _user = admin_user;
            DataContext = _user;
            UsersMenuItem_Click(null, null);
        }
        private void UsersMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new UsersPage());
        }
        private void DriversMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new DriversPage());
        }
        private void CargoMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new CargoPage());
        }
        private void OrdersMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new OrderPage());
        }
        private void PaymentsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new PaymentPage());
        }
        private void RoutesMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new RoutePage());
        }
        private void TariffsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new TariffsPage());
        }
        private void TrucksMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new TrucksPage());
        }

        private void btnexit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Main());
        }
    }
}
