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
using LogiCore.Pages;

namespace LogiCore.Add
{
    /// <summary>
    /// Логика взаимодействия для AddPayment.xaml
    /// </summary>
    public partial class AddPayment : Window
    {
        public static List<Order> orders { get; set; }
        public Payment Payment { get; set; }
        private bool isEditMode = false;
        public AddPayment()
        {
            InitializeComponent();
            InitializeWindow(null);
        }
        public AddPayment(Payment paymentToEdit)
        {
            InitializeComponent();
            InitializeWindow(paymentToEdit);
            isEditMode = true;
        }

        private void InitializeWindow(Payment payment)
        {
            orders = DB.log.Order.ToList();
            DataContext = this;

            if (payment != null)
            {
                Payment = payment;
                Ordercb.SelectedItem = DB.log.Order.FirstOrDefault(o => o.Order_id == payment.Order_id);
                txbSumm.Text = payment.Amount.ToString();
                Statuscb.SelectedValue = payment.Status_payment;
            }
            else
            {
                Payment = new Payment
                {
                    Transaction_date = DateTime.Now,
                    Status_payment = "Ожидается"
                };
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Ordercb.SelectedItem == null ||
             txbSumm.Text == null ||
            Statuscb.SelectedItem == null)
                {
                    MessageBox.Show("Заполните все обязательные поля", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error); return;
                }
                if (!isEditMode)
                {
                    Payment = new Payment();
                }
                Payment.Order_id = ((Order)Ordercb.SelectedItem).Order_id;
                Payment.Amount = decimal.Parse(txbSumm.Text);
                Payment.Status_payment = ((ComboBoxItem)Statuscb.SelectedItem).Content.ToString();
                Payment.Transaction_date = DateTime.Now;

                if (!isEditMode)
                {
                    DB.log.Payment.Add(Payment);
                }

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
            DialogResult = false;
            Close();
        }
    }
}
