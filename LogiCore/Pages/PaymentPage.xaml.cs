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
    /// Логика взаимодействия для PaymentPage.xaml
    /// </summary>
    public partial class PaymentPage : Page
    {
        public ObservableCollection<Payment> payments { get; set; }
        private Payment selectedPayment;
        public PaymentPage()
        {
            InitializeComponent();
            LoadPay();
        }
        private void LoadPay()
        {
            payments = new ObservableCollection<Payment>(DB.log.Payment.ToList());
            PayGrid.ItemsSource = payments;
        }
        private void AddPay_Click(object sender, RoutedEventArgs e)
        {
            var addPayment = new AddPayment();

            if (addPayment.ShowDialog() == true)
            {
                try
                {
                    DB.log.Payment.Add(addPayment.Payment);
                    DB.log.SaveChanges();
                    LoadPay();
                    MessageBox.Show("Платеж успешно добавлен", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void EditPay_Click(object sender, RoutedEventArgs e)
        {
            if (selectedPayment == null)
            {
                MessageBox.Show("Выберите платеж для редактирования", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var addPayment = new AddPayment(selectedPayment);

            if (addPayment.ShowDialog() == true)
            {
                try
                {
                    DB.log.SaveChanges();
                    LoadPay();

                    LoadPay();
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

        private void DeletePay_Click(object sender, RoutedEventArgs e)
        {
            if (selectedPayment == null)
            {
                MessageBox.Show("Выберите платеж для удаления", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            var result = MessageBox.Show($"Удалить платеж №{selectedPayment.Payment_id}?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    DB.log.Payment.Remove(selectedPayment);
                    DB.log.SaveChanges();
                    LoadPay();
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
            LoadPay();
        }

        private void PayGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedPayment = PayGrid.SelectedItem as Payment;
        }

        private void PaySearchTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            string search =PaySearchTb.Text.Trim();
            if (search == "")
               PayGrid.ItemsSource = payments.ToList();
            else
                PayGrid.ItemsSource = payments.Where(i => i.Payment_id.ToString() == search).ToList();
        }
    }
}


