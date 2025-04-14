using System;
using System.Collections.Generic;
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
using System.Xml.Linq;
using System.Collections.ObjectModel;
using LC.Pages;

namespace LogiCore.Add
{
    /// <summary>
    /// Логика взаимодействия для AddCargo.xaml
    /// </summary>
    public partial class AddCargo : Window
    {
        public ObservableCollection<User> users { get; set; }
        public Cargo Cargo { get; private set; }
        private bool isEditMode = false;
        public AddCargo()
        {
            InitializeComponent();
            InitializeWindow(null);
        }
        public AddCargo(Cargo cargoToEdit)
        {
            InitializeComponent();
            InitializeWindow(cargoToEdit);
            isEditMode = true;
        }
        private void InitializeWindow(Cargo cargo)
        {
            users = new ObservableCollection<User>(
                DB.log.User.Where(u => u.Role_id == 2).ToList());

            DataContext = this;

            if (cargo != null)
            {
                Cargo = cargo;
                txbdescription.Text = cargo.Description;
                txbFrom.Text = cargo.Pickup_address;
                txbTo.Text = cargo.Delivery_address;
                txbweight.Text = cargo.Weight.ToString();
                txbvolume.Text = cargo.Volume.ToString();
                dpcargo.SelectedDate = cargo.Date;
                var selectedClient = DB.log.User.FirstOrDefault(c => c.User_id == cargo.Client_id);
                if (selectedClient != null)
                {
                    Sotr.SelectedItem = selectedClient;
                }
            }
            else
            {
                dpcargo.SelectedDate = DateTime.Now;
            }
        }
        private void ButtonCargo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!decimal.TryParse(txbweight.Text, out decimal weight) || weight <= 0)
                {
                    MessageBox.Show("Введите корректный вес (положительное число)!");
                    return;
                }
                if (!decimal.TryParse(txbvolume.Text, out decimal volume) || volume <= 0)
                {
                    MessageBox.Show("Введите корректный объем (положительное число)!");
                    return;
                }
                    if (txbdescription.Text == null || txbFrom.Text == null ||
                       txbTo.Text == null || dpcargo.SelectedDate.Value == null ||
                       Sotr.SelectedItem == null)
                    {
                        MessageBox.Show("Заполните все обязательные поля!", "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    if (!isEditMode)
                    {
                        Cargo = new Cargo();
                    }
                    Cargo.Description = txbdescription.Text;
                    Cargo.Pickup_address = txbFrom.Text;
                    Cargo.Delivery_address = txbTo.Text;
                    Cargo.Weight = weight;
                    Cargo.Volume = volume;
                    Cargo.Date = dpcargo.SelectedDate.Value;
                    Cargo.Client_id = ((User)Sotr.SelectedItem).User_id;

                    DialogResult = true;
                    Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
            

        private void Buttoncancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Отмена добавления");
            DialogResult = false;
            Close();
        }
    }
}
