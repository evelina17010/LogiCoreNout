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
    /// Логика взаимодействия для TrucksPage.xaml
    /// </summary>
    public partial class TrucksPage : Page
    {
        public ObservableCollection<Truck> trucks { get; set; }
        private Truck selectedTruck;
        public TrucksPage()
        {
            InitializeComponent();
            LoadTruck();
        }
        private void LoadTruck()
        {
            trucks = new ObservableCollection<Truck>(DB.log.Truck.ToList());
            TruckGrid.ItemsSource = trucks;
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
                    LoadTruck();

                    MessageBox.Show("Грузовик успешно добавлен", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void EditTruck_Click(object sender, RoutedEventArgs e)
        {
            if (selectedTruck == null)
            {
                MessageBox.Show("Выберите грузовик для редактирования", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var addTruck = new AddTruck(selectedTruck);

            if (addTruck.ShowDialog() == true)
            {
                try
                {
                    DB.log.SaveChanges();
                    LoadTruck();

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

        private void DeleteTruck_Click(object sender, RoutedEventArgs e)
        {
            if (selectedTruck == null)
            {
                MessageBox.Show("Выберите грузовик для удаления", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            var result = MessageBox.Show($"Удалить грузовик {selectedTruck.Model} ({selectedTruck.Registration_number})?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    DB.log.Truck.Remove(selectedTruck);
                    DB.log.SaveChanges();
                    LoadTruck();
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
            LoadTruck();
        }

        private void TruckGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedTruck = TruckGrid.SelectedItem as Truck;
        }

        private void TruckSearchTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            string search = TruckSearchTb.Text.Trim();
            if (search == "")
                TruckGrid.ItemsSource = trucks.ToList();
            else
                TruckGrid.ItemsSource = trucks.Where(i => i.Truck_id.ToString() == search).ToList();
        }
    }
}
