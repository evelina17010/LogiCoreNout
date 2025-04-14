using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LogiCore.Connection;
using LogiCore.Pages;

namespace LogiCore.Add
{
    /// <summary>
    /// Логика взаимодействия для AddOrder.xaml
    /// </summary>
    public partial class AddOrder : Window
    {
        public Order Order { get;  set; }
        public List<User> Managers { get; set; }
        public List<Cargo> Cargos { get; set; }
        public List<Tariff> Tariffs { get; set; }
        public List<Truck> Trucks { get; set; }
        public List<Route> Routes { get; set; }
        private bool isEditMode = false;
        public AddOrder()
        {
            InitializeComponent();
            InitializeWindow(null);
        }
        public AddOrder(Order orderToEdit)
        {
            InitializeComponent();
            InitializeWindow(orderToEdit);
            isEditMode = true;
        }

        private void InitializeWindow(Order order)
        {
            Managers = DB.log.User.Where(u => u.Role_id == 4).ToList();
            Cargos = DB.log.Cargo.ToList();
            Tariffs = DB.log.Tariff.ToList();
            Trucks = DB.log.Truck.ToList();
            Routes = DB.log.Route.ToList();
            DataContext = this;
            if (order != null)
            {
                Order = order;
                Managercb.SelectedItem = Managers.FirstOrDefault(m => m.User_id == order.Manager_id);
                Cargocb.SelectedItem = Cargos.FirstOrDefault(c => c.Cargo_id == order.Cargo_id);
                Tariffcb.SelectedItem = Tariffs.FirstOrDefault(t => t.Tariff_id == order.Tariff_id);
                Truckcb.SelectedItem = Trucks.FirstOrDefault(t => t.Truck_id == order.Truck_id);
                Routecb.SelectedItem = Routes.FirstOrDefault(r => r.Route_id == order.Route_id);
                Statuscb.SelectedValue = order.Status_order;
            }
            else
            {
                Order = new Order
                {
                    Order_date = DateTime.Now,
                    Status_order = "Новый"
                };
            }
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Managercb.SelectedItem == null ||
               Cargocb.SelectedItem == null ||
              Tariffcb.SelectedItem == null ||
               Truckcb.SelectedItem == null ||
               Routecb.SelectedItem == null )
            {
                MessageBox.Show("Заполните все обязательные поля", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!isEditMode)
            {
                Order = new Order();
            }
                Order.Manager_id = ((User)Managercb.SelectedItem).User_id;
                Order.Cargo_id = ((Cargo)Cargocb.SelectedItem).Cargo_id;
                Order.Tariff_id = ((Tariff)Tariffcb.SelectedItem).Tariff_id;
                Order.Truck_id = ((Truck)Truckcb.SelectedItem).Truck_id;
                Order.Route_id = ((Route)Routecb.SelectedItem).Route_id;
                Order.Status_order = ((ComboBoxItem)Statuscb.SelectedItem).Content.ToString();
                Order.Order_date = DateTime.Now;
                Order.Price = CalculateOrderPrice();

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}\n\n{ex.InnerException?.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private decimal CalculateOrderPrice()
        {         
            var tariff = (Tariff)Tariffcb.SelectedItem;
            var cargo = (Cargo)Cargocb.SelectedItem;
            var route = (Route)Routecb.SelectedItem;

            return (tariff.Cost_km * route.Distance) +
                   (tariff.Cost_kg * cargo.Weight) +
                   (tariff.Cost_m3 * cargo.Volume) +
                   tariff.AdditionalCost;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Отмена добавления");
            DialogResult = false;
            Close();
        }
    }
    }

