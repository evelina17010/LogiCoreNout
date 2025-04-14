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
    /// Логика взаимодействия для OrderPage.xaml
    /// </summary>
    public partial class OrderPage : Page
    {
        public ObservableCollection<Order> orders { get; set; }
        private Order selectedOrder;
        public OrderPage()
        {
            InitializeComponent();
            LoadOrder();
        }
        private void LoadOrder()
        {
            orders = new ObservableCollection<Order>(DB.log.Order.ToList());
            OrdersGrid.ItemsSource = orders;
        }
        private void OrdersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedOrder = OrdersGrid.SelectedItem as Order;
        }
        private void AddOrder_Click(object sender, RoutedEventArgs e)
        {
            var addOrder = new AddOrder();

            if (addOrder.ShowDialog() == true)
            {
                try
                {
                    DB.log.Order.Add(addOrder.Order);
                    DB.log.SaveChanges();
                    LoadOrder();

                    MessageBox.Show("Заказ успешно добавлен", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void EditOrder_Click(object sender, RoutedEventArgs e)
        {
            if (selectedOrder == null)
            {
                MessageBox.Show("Выберите заказ для редактирования", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var addOrder = new AddOrder(selectedOrder);

            if (addOrder.ShowDialog() == true)
            {
                try
                {
                    DB.log.SaveChanges();
                    LoadOrder();

                    MessageBox.Show("Изменения сохранены успешно", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении изменений: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void DeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            if (selectedOrder == null)
            {
                MessageBox.Show("Выберите заказ для удаления", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            var result = MessageBox.Show($"Удалить заказ №{selectedOrder.Order_id}?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    DB.log.Order.Remove(selectedOrder);
                    DB.log.SaveChanges();
                    LoadOrder();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }       

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadOrder();
        }

        private void OrderSearchTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            string search =OrderSearchTb.Text.Trim();
            if (search == "")
               OrdersGrid.ItemsSource = orders.ToList();
            else
              OrdersGrid.ItemsSource = orders.Where(i => i.Order_id.ToString() == search).ToList();
        }
    }
}
