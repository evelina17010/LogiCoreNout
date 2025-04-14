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

namespace LogiCore.Add
{
    /// <summary>
    /// Логика взаимодействия для AddRoute.xaml
    /// </summary>
    public partial class AddRoute : Window
    {
        public Route Route { get; set; }
        private bool isEditMode = false;

        public AddRoute()
        {
            InitializeComponent();
            InitializeWindow(null);
        }
        public AddRoute(Route routeToEdit)
        {
            InitializeComponent();
            InitializeWindow(routeToEdit);
            isEditMode = true;
        }

        private void InitializeWindow(Route route)
        {
            if (route != null)
            {
                Route = route;
                txtStartPoint.Text = route.Start_point;
                txtEndPoint.Text = route.End_point;
                txtDistance.Text = route.Distance.ToString();
                txtEstimatedTime.Text = route.Estimated_time.ToString();
            }
            else
            {
                Route = new Route();
            }

            DataContext = this;
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtStartPoint.Text == null ||
                   txtEndPoint.Text == null ||
                  txtDistance.Text == null ||
                   txtEstimatedTime.Text == null)
                {
                    MessageBox.Show("Заполните все обязательные поля!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (!decimal.TryParse(txtDistance.Text, out decimal distance))
                {
                    MessageBox.Show("Введите корректное значение расстояния!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (!decimal.TryParse(txtEstimatedTime.Text, out decimal estimatedTime))
                {
                    MessageBox.Show("Введите корректное значение времени!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (!isEditMode)
                {
                    Route = new Route();
                }

                Route.Start_point = txtStartPoint.Text.Trim();
                Route.End_point = txtEndPoint.Text.Trim();
                Route.Distance = distance;
                Route.Estimated_time = estimatedTime;
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