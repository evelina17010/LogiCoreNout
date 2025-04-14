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

namespace LogiCore.Pages
{
    /// <summary>
    /// Логика взаимодействия для DriverOrders.xaml
    /// </summary>
    public partial class DriverOrders : Page
    {
        private User _user;
        public ObservableCollection<Order> Orders { get; set; }
        public DriverOrders(User user)
        {
            InitializeComponent();
            _user = user;
            DataContext = _user;
            LoadOrders();
        }

        private void LoadOrders()
        {
            try
            {
                var driverId = DB.log.Driver
                                .Where(d => d.User_id == _user.User_id)
                                .Select(d => d.Driver_id)
                                .FirstOrDefault();

                if (driverId == 0)
                {
                    MessageBox.Show("Водитель не найден");
                    return;
                }
                var truckIds = DB.log.Truck
                                .Where(t => t.Driver_id == driverId)
                                .Select(t => t.Truck_id)
                                .ToList();

                var ordersList = DB.log.Order
                                .Where(o => truckIds.Contains(o.Truck_id))
                                .OrderByDescending(o => o.Order_date)
                                .ToList();

                Orders = new ObservableCollection<Order>(ordersList);
                OrdersGrid.ItemsSource = Orders;

                Console.WriteLine($"Загружено заказов: {Orders.Count}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки заказов: {ex.Message}");
                Orders = new ObservableCollection<Order>();
                OrdersGrid.ItemsSource = Orders;
            }
        }
        private void RefreshOrders_Click(object sender, RoutedEventArgs e)
        {
            LoadOrders();
        }

        private void CompleteOrder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedOrder = OrdersGrid.SelectedItem as Order;
                if (selectedOrder == null)
                {
                    MessageBox.Show("Выберите заказ");
                    return;
                }
                if (selectedOrder.Status_order == "Доставлен")
                {
                    MessageBox.Show("Этот заказ уже доставлен");
                    return;
                }
                if (MessageBox.Show("Отметить заказ как доставленный?", "Подтверждение",
                    MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                {
                    return;
                }
                selectedOrder.Status_order = "Доставлен";
                DB.log.SaveChanges();
                LoadOrders();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
        private void StatusFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StatusFilter.SelectedItem == null) return;

            string selectedStatus = (StatusFilter.SelectedItem as ComboBoxItem).Content.ToString();

            if (selectedStatus == "Все заказы")
            {
                OrdersGrid.ItemsSource = Orders;
            }
            else
            {
                OrdersGrid.ItemsSource = Orders.Where(o => o.Status_order == selectedStatus).ToList();
            }
        }

        private void AcceptOrder_Click(object sender, RoutedEventArgs e)
        {
            if (!(OrdersGrid.SelectedItem is Order selectedOrder))
            {
                MessageBox.Show("Выберите заказ");
                return;
            }

            if (selectedOrder.Status_order != "Новый")
            {
                MessageBox.Show("Можно принимать только новые заказы");
                return;
            }

            if (MessageBox.Show($"Принять заказ №{selectedOrder.Order_id}?", "Подтверждение",
                MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            {
                return;
            }
            selectedOrder.Status_order = "Принят";
            try
            {
                DB.log.SaveChanges();
                LoadOrders();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}");
                selectedOrder.Status_order = "Новый";
            }
        }
    }
    }
    
