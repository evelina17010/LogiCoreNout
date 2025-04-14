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
using LC.Pages;
using LogiCore.Connection;
using LogiCore.Function;

namespace LogiCore.Pages
{
    /// <summary>
    /// Логика взаимодействия для ClientPay.xaml
    /// </summary>
    public partial class ClientPay : Page
    {
        public static ObservableCollection<Payment> Payments { get; set; }
        private int _userId;
        public ClientPay(int userid)
        {
            InitializeComponent();
            _userId = userid;
            Payments = new ObservableCollection<Payment>();
            this.DataContext = this;
            LoadPayments();
        }
        private void LoadPayments()
        {
            try
            {
                Payments = new ObservableCollection<Payment>(ClientFun.GetPayments(_userId));
                ListViewPayments.ItemsSource = null;  
                ListViewPayments.ItemsSource = Payments;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки платежей: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }        
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadPayments();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка обновления: {ex.Message}");
            }     
    }
    }
    }

