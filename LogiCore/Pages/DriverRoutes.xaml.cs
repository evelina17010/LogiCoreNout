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
using LogiCore.Add;
using LogiCore.Connection;

namespace LogiCore.Pages
{
    /// <summary>
    /// Логика взаимодействия для DriverRoutes.xaml
    /// </summary>
    public partial class DriverRoutes : Page
    {
        private User _Driver;
        public ObservableCollection<Route> Routes { get; set; }
        public DriverRoutes(User driver)
        {
            InitializeComponent();
            _Driver = driver;
            LoadRoutes();
        }
        private void LoadRoutes()
        {
            Routes = new ObservableCollection<Route>(DB.log.Order
          .Where(o => o.Truck.Driver.User_id == _Driver.User_id && o.Route != null)
          .Select(o => o.Route).Distinct().ToList());
            RoutesGrid.ItemsSource = Routes;
        }
        private void RefreshRoutes_Click(object sender, RoutedEventArgs e)
        {
            LoadRoutes();
        }




    }
}
