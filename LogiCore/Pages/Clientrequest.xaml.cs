using LogiCore.Connection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Policy;
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


namespace LC.Pages
{
    /// <summary>
    /// Логика взаимодействия для Clientrequest.xaml
    /// </summary>
    public partial class Clientrequest : Page
    {
        private int _client_id;
        public List<Tariff> Tariffs { get; set; }
        public Clientrequest(int client_id)
        {
            InitializeComponent();
            _client_id = client_id;
            Tariffs = DB.log.Tariff.ToList();
            cmbTariffs.ItemsSource = Tariffs;
            this.DataContext = this;

        }

        private void ButtonCargo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!decimal.TryParse(txbweight.Text, out decimal Weight) || Weight <= 0)
                {
                    MessageBox.Show("Введите корректный вес (положительное число)!");
                    return;
                }
                if (!decimal.TryParse(txbvolume.Text, out decimal Volume) || Volume <= 0)
                {
                    MessageBox.Show("Введите корректный объем (положительное число)!");
                    return;
                }
                if (string.IsNullOrEmpty(txbdescription.Text) || string.IsNullOrEmpty(txbFrom.Text) || string.IsNullOrEmpty(txbTo.Text) || string.IsNullOrEmpty(txbweight.Text) ||
                (string.IsNullOrEmpty(txbvolume.Text) || dpcargo.SelectedDate == null))
                {
                    MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    int Client_id = _client_id;
                    string description = txbdescription.Text;
                    string from = txbFrom.Text;
                    string to = txbTo.Text;
                    double weight = (double)Weight;
                    double volume = (double)Volume;
                    DateTime? dateTime = dpcargo.SelectedDate;
                    DateTime cargodate = dateTime.Value;
                    decimal distance = GetDistance(txbFrom.Text, txbTo.Text);
                    Tariff selectedTariff = (Tariff)cmbTariffs.SelectedItem;
                    if (selectedTariff == null)
                    {
                        MessageBox.Show("Выберите тариф");
                        return;
                    }
                    if (txbdescription != null && txbFrom != null && txbTo != null && txbweight != null && txbvolume != null && dpcargo != null)
                    {
                        LogiCore.Function.ClientFun.CreateOrder(Client_id, description, from, to, weight, volume, cargodate);
                        System.Windows.MessageBox.Show("Заказ принят");
                        txbdescription.Text = string.Empty;
                        txbFrom.Text = string.Empty;
                        txbTo.Text = string.Empty;
                        txbweight.Text = string.Empty;
                        txbvolume.Text = string.Empty;
                        dpcargo.SelectedDate = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
        private void CalculateCost_Click(object sender, RoutedEventArgs e)
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

                if (string.IsNullOrEmpty(txbFrom.Text) || string.IsNullOrEmpty(txbTo.Text))
                {
                    MessageBox.Show("Введите пункты отправления и назначения");
                    return;
                }
                Tariff selectedTariff = (Tariff)cmbTariffs.SelectedItem;
                if (selectedTariff == null)
                {
                    MessageBox.Show("Выберите тариф");
                    return;
                }
                decimal distance = GetDistance(txbFrom.Text, txbTo.Text);
                decimal cost = (distance * selectedTariff.Cost_km) +
                              (weight * selectedTariff.Cost_kg) +
                              (volume * selectedTariff.Cost_m3) +
                              selectedTariff.AdditionalCost;

                decimal minCost = 500;
                cost = cost < minCost ? minCost : cost;

                tbCost.Text = $"Примерная стоимость: {cost:N2} руб.\n";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка расчета стоимости: {ex.Message}");
            }
        }

        public static decimal GetDistance(string cityFrom, string cityTo)
        {       
               var route = DB.log.Route
                    .FirstOrDefault(r =>
                        (r.Start_point == cityFrom && r.End_point == cityTo) ||
                        (r.Start_point == cityTo && r.End_point == cityFrom));
                return route?.Distance ?? 100;            
        }
    }
}
