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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using LC.Pages;
using LogiCore.Connection;

namespace LogiCore.Pages
{
    /// <summary>
    /// Логика взаимодействия для Drivers.xaml
    /// </summary>
    public partial class Drivers : Page
    {
        private User _user;
        public static List<Route> routes { get; set; }
        public static List<Order> orders { get; set; }
        public static List<Truck> trucks { get; set; }
        public Drivers(User user)
        {
            InitializeComponent();
            _user = user;
            txbName.Text = _user.Full_name;
            MyOrdersMenuItem_Click(null, null);
        }

        private void btnexit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Main());
        }

        private void MyOrdersMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new DriverOrders(_user));
        }

        private void RoutesMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new DriverRoutes(_user));
        }

        private void MyTruckMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new DriverTruck(_user));
        }

        private void ScheduleMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new DriverSchedule(_user));
        }
    }
}
