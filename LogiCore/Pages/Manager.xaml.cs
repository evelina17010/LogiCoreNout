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
using LC.Pages;
using LogiCore.Add;
using LogiCore.Connection;

namespace LogiCore.Pages
{
    /// <summary>
    /// Логика взаимодействия для Manager.xaml
    /// </summary>
    public partial class Manager : Page
    {
        private User _user;
        public ObservableCollection<Order> Orders { get; set; }
        public ObservableCollection<Truck> Trucks { get; set; }
        public ObservableCollection<Driver>  Drivers { get; set; }
        private Driver selectedDriver;
        private Order selectedOrder;
        private Truck selectedTruck;
        public Manager(User user)
        {
            InitializeComponent();
            _user = user;
           DataContext = this;
            txbName.Text = _user.Full_name;
            LoadDrivers();
            LoadOrders();
            LoadTrucks();
        }
        private void LoadDrivers()
        {
            Drivers = new ObservableCollection<Driver>(DB.log.Driver.ToList());
            DriverGrid.ItemsSource = Drivers;
        }
        private void LoadTrucks()
        {
            Trucks = new ObservableCollection<Truck>(DB.log.Truck.ToList());
            TrucksGrid.ItemsSource = Trucks;
        }
        private void LoadOrders()
        {
            Orders = new ObservableCollection<Order>(DB.log.Order.ToList());
            OrdersGrid.ItemsSource = Orders;
        }
        private void btnexit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Main());
        }
        private void CreateOrder_Click(object sender, RoutedEventArgs e)
        {
            var addOrder = new AddOrder();
            if (addOrder.ShowDialog() == true)
            {
                DB.log.Order.Add(addOrder.Order);
                DB.log.SaveChanges();
                LoadOrders();
            }
        }

        private void ChangeStatus_Click(object sender, RoutedEventArgs e)
        {
            if (selectedOrder == null)
            {
                MessageBox.Show("Выберите заказ для редактирования", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var status = new ChangeStatus(selectedOrder.Status_order);
            if (status.ShowDialog() == true)
            {
                selectedOrder.Status_order = status.SelectedStatus; 
                try
                {
                    DB.log.SaveChanges();
                    LoadOrders();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка сохранения: {ex.Message}");
                }
            }
        }
        private void AssignTruck_Click(object sender, RoutedEventArgs e)
        {
            if (selectedOrder == null)
            {
                MessageBox.Show("Выберите заказ");
                return;
            }

            var assignTruck = new AssignTruck(selectedOrder);
            if (assignTruck.ShowDialog() == true)
            {
                try
                {
                    DB.log.SaveChanges();
                    LoadOrders();
                    MessageBox.Show("Транспорт успешно назначен", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка назначения транспорта: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void AddTruck_Click(object sender, RoutedEventArgs e)
        {
            var addTruck = new AddTruck();
            if (addTruck.ShowDialog() == true)
            {
                try
                {
                    DB.log.Truck.Add(addTruck.Truck);
                    DB.log.SaveChanges();
                    LoadTrucks(); 
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка добавления транспорта: {ex.Message}");
                }
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadTrucks();
        }

        private void OrdersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedOrder = OrdersGrid.SelectedItem as Order;
        }

        private void OrderSearchTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            string search = OrderSearchTb.Text.Trim();
            if (search == "")
                OrdersGrid.ItemsSource = Orders.ToList();
            else
                OrdersGrid.ItemsSource = Orders.Where(i => i.Order_id.ToString() == search).ToList();
        }

        private void RefreshOrders_Click(object sender, RoutedEventArgs e)
        {
            LoadOrders();
        }

        private void EditTruck_Click(object sender, RoutedEventArgs e)
        {
            if (selectedTruck == null)
            {
                MessageBox.Show("Выберите транспорт");
                return;
            }
            var editTruck = new AddTruck(selectedTruck);
            if (editTruck.ShowDialog() == true)
            {
                try
                {
                    DB.log.SaveChanges();
                    LoadTrucks();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка сохранения: {ex.Message}");
                }
            }
        }

        private void DeleteTruck_Click(object sender, RoutedEventArgs e)
        {
            if (selectedTruck == null) return;

            if (MessageBox.Show($"Удалить транспорт {selectedTruck.Model}?",
                "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                DB.log.Truck.Remove(selectedTruck);
                DB.log.SaveChanges();
                LoadTrucks();
            }
        }

        private void RefreshTrucks_Click(object sender, RoutedEventArgs e)
        {
            LoadTrucks();
        }

        private void TrucksGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedTruck = TrucksGrid.SelectedItem as Truck;
        }

        private void StatusFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StatusFilter.SelectedItem == null || Orders == null)
                return;

            var selectedItem = StatusFilter.SelectedItem as ComboBoxItem;
            if (selectedItem == null) return;

            if (selectedItem.Tag.ToString() == "0")
            {
                OrdersGrid.ItemsSource = Orders;
            }
            else
            {
                string selectedStatus = selectedItem.Content.ToString().Trim();
                OrdersGrid.ItemsSource = Orders
                    .Where(o => o.Status_order.Trim() == selectedStatus)
                    .ToList();
            }
        }

        private void DriverGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedDriver =DriverGrid.SelectedItem as Driver;
        }

        private void AddDriver_Click(object sender, RoutedEventArgs e)
        {
            var addDriver = new AddDriver();
            if (addDriver.ShowDialog() == true)
            {
                try
                {
                    DB.log.Driver.Add(addDriver.Driver);
                    DB.log.SaveChanges();
                    LoadDrivers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка добавления транспорта: {ex.Message}");
                }
            }
        }

        private void EditDriver_Click(object sender, RoutedEventArgs e)
        {
            if (selectedDriver == null)
            {
                MessageBox.Show("Выберите водителя");
                return;
            }
            var editDriver = new AddDriver(selectedDriver);
            if (editDriver.ShowDialog() == true)
            {
                try
                {
                    DB.log.SaveChanges();
                    LoadDrivers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка сохранения: {ex.Message}");
                }
            }

        }

        private void RefreshDrivers_Click(object sender, RoutedEventArgs e)
        {
            LoadDrivers();
        }

        private void OrderSearchTb_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }
    }
    }

