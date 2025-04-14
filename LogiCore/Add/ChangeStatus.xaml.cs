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
    /// Логика взаимодействия для ChangeStatus.xaml
    /// </summary>
    public partial class ChangeStatus : Window
    {
        public ObservableCollection<Order> orders { get; set; }
        public List<string> StatusOptions { get; } = new List<string>
        {
            "Новый",
            "В обработке",
            "В пути",
            "Доставлен",
            "Отменен"
        };
        public string SelectedStatus { get; set; }
        public string CurrentStatus { get; }
        public ChangeStatus(string currentStatus)
        {
            InitializeComponent();
            CurrentStatus = currentStatus;
            SelectedStatus = currentStatus; 
            orders = new ObservableCollection<Order>();
            DataContext = this;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
