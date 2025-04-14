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
    /// Логика взаимодействия для CargoPage.xaml
    /// </summary>
    public partial class CargoPage : Page
    {
        private Cargo selectedCargo;
        public ObservableCollection<Cargo> cargos { get; set; }
        public CargoPage()
        {
            InitializeComponent();
            LoadCargo();
        }
        private void LoadCargo()
        {
            cargos = new ObservableCollection<Cargo>(DB.log.Cargo.ToList());
            CargoGrid.ItemsSource = cargos;
        }
        private void CargoGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedCargo = CargoGrid.SelectedItem as Cargo;
        }
      
        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadCargo();
        }

        private void AddCargo_Click(object sender, RoutedEventArgs e)
        {
            var addCargo = new AddCargo();

            if (addCargo.ShowDialog() == true)
            {
                try
                {
                    DB.log.Cargo.Add(addCargo.Cargo);
                    DB.log.SaveChanges();
                    LoadCargo();

                    MessageBox.Show("Груз успешно добавлен", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void EditCargo_Click(object sender, RoutedEventArgs e)
        {
            if (selectedCargo == null)
            {
                MessageBox.Show("Выберите груз для редактирования", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

             var addCargo = new AddCargo(selectedCargo);

            if (addCargo.ShowDialog() == true)
            {
                try
                {
                    DB.log.SaveChanges();
                    LoadCargo();

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
        

        private void DeleteCargo_Click(object sender, RoutedEventArgs e)
        {
            if (selectedCargo == null)
            {
                MessageBox.Show("Выберите груз для удаления", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            var result = MessageBox.Show($"Удалить груз {selectedCargo.Cargo_id}?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    DB.log.Cargo.Remove(selectedCargo);
                    DB.log.SaveChanges();
                    LoadCargo();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void CargoSearchTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            string search = CargoSearchTb.Text.Trim();
            if (search == "")
               CargoGrid.ItemsSource = cargos.ToList();
            else
                CargoGrid.ItemsSource = cargos.Where(i => i.Cargo_id.ToString() == search).ToList();
        }
    }
}
