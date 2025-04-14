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
    /// Логика взаимодействия для RoutePage.xaml
    /// </summary>
    public partial class RoutePage : Page
    {
        public ObservableCollection<Route> routes { get; set; }
        private Route selectedRoute;
        public RoutePage()
        {
            InitializeComponent();
            LoadRoute();
        }
        private void LoadRoute()
        {
            routes = new ObservableCollection<Route>(DB.log.Route.ToList());
            RouteGrid.ItemsSource = routes;
        }

        private void AddRoure_Click(object sender, RoutedEventArgs e)
        {
            var addRoute = new AddRoute();

            if (addRoute.ShowDialog() == true)
            {
                try
                {
                    DB.log.Route.Add(addRoute.Route);
                    DB.log.SaveChanges();
                    LoadRoute();

                    MessageBox.Show("Маршрут успешно добавлен", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void EditRoure_Click(object sender, RoutedEventArgs e)
        {
            if (selectedRoute == null)
            {
                MessageBox.Show("Выберите маршрут для редактирования", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var addRoute = new AddRoute(selectedRoute);

            if (addRoute.ShowDialog() == true)
            {
                try
                {
                    DB.log.SaveChanges();
                    LoadRoute();

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

        private void DeleteRoure_Click(object sender, RoutedEventArgs e)
        {
            if (selectedRoute == null)
            {
                MessageBox.Show("Выберите маршрут для удаления", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            var result = MessageBox.Show($"Удалить маршрут {selectedRoute.Start_point} - {selectedRoute.End_point}?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    DB.log.Route.Remove(selectedRoute);
                    DB.log.SaveChanges();

                    LoadRoute();
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
            LoadRoute();
        }

        private void RoureGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedRoute = RouteGrid.SelectedItem as Route;
        }

        private void RoureSearchTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            string search = RoureSearchTb.Text.Trim();
            if (search == "")
                RouteGrid.ItemsSource = routes.ToList();
            else
                RouteGrid.ItemsSource =routes.Where(i => i.Route_id.ToString() == search).ToList();
        }
    }
}