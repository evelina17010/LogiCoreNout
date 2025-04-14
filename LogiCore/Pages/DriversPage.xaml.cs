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
    /// Логика взаимодействия для DriversPage.xaml
    /// </summary>
    public partial class DriversPage : Page
    {
        private Driver selectedDriver;
        public ObservableCollection<Driver> drivers { get; set; }
        public DriversPage()
        {
            InitializeComponent();
            LoadDrivers();
        }
        private void LoadDrivers()
        {
            drivers = new ObservableCollection<Driver>(DB.log.Driver.Include("User").ToList());
            DriversGrid.ItemsSource =drivers;

        }     
       
        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadDrivers();
        }

        private void DriversGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedDriver = DriversGrid.SelectedItem as Driver;
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

                    MessageBox.Show("Водитель успешно добавлен", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void EditDriver_Click(object sender, RoutedEventArgs e)
        {
            if (selectedDriver == null)
            {
                MessageBox.Show("Выберите водителя для редактирования", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var addDriver = new AddDriver(selectedDriver);

            if (addDriver.ShowDialog() == true)
            {
                try
                {
                    DB.log.SaveChanges();
                    LoadDrivers();

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

        private void DeleteDriver_Click(object sender, RoutedEventArgs e)
        {
            if (selectedDriver == null)
            {
                MessageBox.Show("Выберите водителя для удаления", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            var result = MessageBox.Show($"Удалить водителя {selectedDriver.User.Full_name}?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    DB.log.Driver.Remove(selectedDriver);
                    DB.log.SaveChanges();
                    LoadDrivers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void DriverSearchTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            string search = DriverSearchTb.Text.Trim();
            if (search == "")
                DriversGrid.ItemsSource = drivers.ToList();
            else
                DriversGrid.ItemsSource = drivers.Where(i => i.Driver_id.ToString() == search).ToList();
        }
    }
}

