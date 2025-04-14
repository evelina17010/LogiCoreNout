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
using System.Windows.Shapes;
using LogiCore.Connection;

namespace LogiCore.Add
{
    /// <summary>
    /// Логика взаимодействия для AssignTruck.xaml
    /// </summary>
    public partial class AssignTruck : Window
    {
        public Order CurrentOrder { get; set; }
        private ObservableCollection<Truck> _allTrucks; 
        public AssignTruck(Order order)
        {
            InitializeComponent();
            CurrentOrder = order;
            DataContext = CurrentOrder;
            _allTrucks = new ObservableCollection<Truck>(DB.log.Truck.ToList());
            TrucksDataGrid.ItemsSource = _allTrucks;
        }     
        private void FilterTrucks_Click(object sender, RoutedEventArgs e)
        {
            var filtered = _allTrucks.AsQueryable();

            if (OnlyAvailableCheckBox.IsChecked == true)
            {
                filtered = filtered.Where(t => t.Status_truck == true);
            }

            if (int.TryParse(MinCapacityTextBox.Text, out int minCapacity))
            {
                filtered = filtered.Where(t => t.Capacity >= minCapacity);
            }

            TrucksDataGrid.ItemsSource = new ObservableCollection<Truck>(filtered.ToList());
        }
        private void Assign_Click(object sender, RoutedEventArgs e)
        {
            if (TrucksDataGrid.SelectedItem is Truck selectedTruck)
            {
                CurrentOrder.Truck = selectedTruck;
                CurrentOrder.Truck_id = selectedTruck.Truck_id;
                selectedTruck.Status_truck = false;
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Выберите грузовик для назначения", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
