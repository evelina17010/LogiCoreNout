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

namespace LogiCore.Add
{
    /// <summary>
    /// Логика взаимодействия для AddTruck.xaml
    /// </summary>
    public partial class AddTruck : Window
    {
        public Truck Truck { get; private set; }
        public ObservableCollection<User> Drivers { get; set; }
        private bool isEditMode = false;

        public AddTruck()
        {
            InitializeComponent();
            InitializeWindow(null);
        }

        public AddTruck(Truck truckToEdit)
        {
            InitializeComponent();
            InitializeWindow(truckToEdit);
            isEditMode = true;
        }

        private void InitializeWindow(Truck truck)
        {
            Drivers = new ObservableCollection<User>(DB.log.User.Where(u => u.Role_id == 4).ToList());
            DataContext = this;

            if (truck != null)
            {
                Truck = truck;
                txtModel.Text = truck.Model;
                txtRegNumber.Text = truck.Registration_number;
                txtCapacity.Text = truck.Capacity.ToString();
                txtDimensions.Text = truck.Dimensions;
                txtLocation.Text = truck.Location;
                chkStatus.IsChecked = truck.Status_truck;
                cmbDriver.SelectedItem = Drivers.FirstOrDefault(d => d.User_id == truck.Driver_id );
            }
            else
            {
                Truck = new Truck();
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtModel.Text == null || txtRegNumber.Text == null ||
                  txtCapacity.Text == null)
                {
                    MessageBox.Show("Заполните все обязательные поля", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (!decimal.TryParse(txtCapacity.Text, out decimal capacity))
                {
                    MessageBox.Show("Грузоподъемность должна быть числовым значением", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!isEditMode)
                {
                    Truck = new Truck();
                }

                Truck.Model = txtModel.Text.Trim();
                Truck.Registration_number = txtRegNumber.Text.Trim();
                Truck.Capacity = capacity;
                Truck.Dimensions = txtDimensions.Text.Trim();
                Truck.Location = txtLocation.Text.Trim();
                Truck.Status_truck = chkStatus.IsChecked;
                Truck.Driver_id = ((User)cmbDriver.SelectedItem).User_id;

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Отмена добавления");
            Close();
        }
    }
}