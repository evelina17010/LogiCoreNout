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
using LogiCore.Connection;

namespace LogiCore.Pages
{
    /// <summary>
    /// Логика взаимодействия для DriverTruck.xaml
    /// </summary>
    public partial class DriverTruck : Page
    {
        private  User _Driver;
        public DriverTruck(User driver)
        {
            InitializeComponent();
            _Driver = driver;
            LoadTruck();
        }
        private void LoadTruck()
        {
            var truck = DB.log.Truck.FirstOrDefault(t => t.Driver.User_id == _Driver.User_id);

            if (truck == null)
            {
                MessageBox.Show("Грузовик не назначен");
                return;
            }
            DataContext = truck;
        }

        private void RefreshData_Click(object sender, RoutedEventArgs e)
        {
            LoadTruck();
        }
    }
}
