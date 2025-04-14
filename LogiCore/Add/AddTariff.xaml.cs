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
    /// Логика взаимодействия для AddTariff.xaml
    /// </summary>
    public partial class AddTariff : Window
    {
        public Tariff Tariff { get; set; }
        private bool isEditMode = false;
        public AddTariff()
        {
            InitializeComponent();
            InitializeWindow(null);
        }

        public AddTariff(Tariff tariffToEdit)
        {
            InitializeComponent();
            InitializeWindow(tariffToEdit);
            isEditMode = true;
        }

        private void InitializeWindow(Tariff tariff)
        {
            if (tariff != null)
            {
                Tariff = tariff;
                txtDescription.Text = tariff.Description;
                txtCostKm.Text = tariff.Cost_km.ToString();
                txtCostKg.Text = tariff.Cost_kg.ToString();
                txtCostM3.Text = tariff.Cost_m3.ToString();
                txtAdditionalCost.Text = tariff.AdditionalCost.ToString();
            }
            else
            {
                Tariff = new Tariff
                {
                    Created_time = DateTime.Now
                };
            }

            DataContext = this;
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtDescription.Text == null || txtCostKm.Text == null ||
            txtCostKg.Text == null || txtCostM3.Text == null || txtAdditionalCost.Text == null)
                {
                    MessageBox.Show("Заполните все обязательные поля", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!decimal.TryParse(txtCostKm.Text, out decimal costKm) ||
                    !decimal.TryParse(txtCostKg.Text, out decimal costKg) ||
                    !decimal.TryParse(txtCostM3.Text, out decimal costM3) ||
                    !decimal.TryParse(txtAdditionalCost.Text, out decimal additionalCost))
                {
                    MessageBox.Show("Все стоимостные параметры должны быть числовыми значениями", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (!decimal.TryParse(txtCostKm.Text, out costKm) ||
                    !decimal.TryParse(txtCostKg.Text, out costKg) ||
                    !decimal.TryParse(txtCostM3.Text, out costM3) ||
                    !decimal.TryParse(txtAdditionalCost.Text, out additionalCost))
                {
                    MessageBox.Show("Все стоимостные параметры должны быть числовыми значениями", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!isEditMode)
                {
                    Tariff = new Tariff();
                }

                Tariff.Description = txtDescription.Text.Trim();
                Tariff.Cost_km = costKm;
                Tariff.Cost_kg = costKg;
                Tariff.Cost_m3 = costM3;
                Tariff.AdditionalCost = additionalCost;
                Tariff.Created_time = DateTime.Now;


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