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
using LogiCore.Connection;
using LogiCore.Function;

namespace LC.Pages
{
    /// <summary>
    /// Логика взаимодействия для ClientHistory.xaml
    /// </summary>
    public partial class ClientHistory : Page
    {
        public  ObservableCollection<Order> orders { get; set; }
        private int _userid;
        public ClientHistory(int userid)
        {
            InitializeComponent();
            if (userid <= 0)
            {
                MessageBox.Show("Некорректный ID пользователя", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            _userid = userid;
            orders = new ObservableCollection<Order>(DB.log.Order.Where(i=>i.Cargo.Client_id==_userid).ToList());
            lvOrders.ItemsSource = orders;
            Load();
            this.DataContext = this;

        }
        private void Load(string filter = "Все")
        {
            try
            {
                if (DB.log == null)
            {
                return;
            }
            if (DB.log.Order == null)
            {
                return;
            }                          
                var query = DB.log.Order.Where(o => o != null && o.Cargo != null && o.Cargo.Client_id == _userid);
                if (filter == "В обработке")
                {
                    query = query.Where(o => o.Status_order == "В обработке");
                }
                else if (filter == "Завершен")
                {
                    query = query.Where(o => o.Status_order == "Завершен");
                }
                else if (filter == "Отменен")
                {
                    query = query.Where(o => o.Status_order == "Отменен");
                }
                else if (filter == "Новый")
                {
                    query = query.Where(o => o.Status_order == "Новый");
                }
                orders.Clear();
                foreach (var order in query.OrderByDescending(o => o.Order_date))
                {
                    orders.Add(order);
                }
              
            }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка загрузки заказов: {ex.Message}", "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
}
private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
           Load();
        }

        private void cbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbFilter.SelectedIndex == 0) Load("Все");
            else if (cbFilter.SelectedIndex == 1) Load("В обработке");
            else if (cbFilter.SelectedIndex == 2) Load("Завершен");
            else if (cbFilter.SelectedIndex == 3) Load("Отменен");
            else if (cbFilter.SelectedIndex == 4) Load("Новый");
        }
    }
}
