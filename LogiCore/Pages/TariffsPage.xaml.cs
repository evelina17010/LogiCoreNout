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
    /// Логика взаимодействия для TariffsPage.xaml
    /// </summary>
    public partial class TariffsPage : Page
    {
        public ObservableCollection<Tariff> tariffs { get; set; }
        private Tariff selectedTariff;
        public TariffsPage()
        {
            InitializeComponent();
            LoadTariff();
        }
        private void LoadTariff()
        {
            tariffs = new ObservableCollection<Tariff>(DB.log.Tariff.ToList());
            TariffGrid.ItemsSource = tariffs;
        }

        private void AddTariff_Click(object sender, RoutedEventArgs e)
        {
            var addTariff = new AddTariff();

            if (addTariff.ShowDialog() == true)
            {
                try
                {
                    DB.log.Tariff.Add(addTariff.Tariff);
                    DB.log.SaveChanges();
                    LoadTariff();

                    MessageBox.Show("Тариф успешно добавлен", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void EditTariff_Click(object sender, RoutedEventArgs e)
        {
            if (selectedTariff == null)
            {
                MessageBox.Show("Выберите тариф для редактирования", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var addTariff = new AddTariff(selectedTariff);

            if (addTariff.ShowDialog() == true)
            {
                try
                {
                    DB.log.SaveChanges();
                    LoadTariff();

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

        private void DeleteTariff_Click(object sender, RoutedEventArgs e)
        {
            if (selectedTariff == null)
            {
                MessageBox.Show("Выберите тариф для удаления", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            var result = MessageBox.Show($"Удалить тариф №{selectedTariff.Tariff_id}?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    DB.log.Tariff.Remove(selectedTariff);
                    DB.log.SaveChanges();

                    LoadTariff();
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
            LoadTariff();
        }

        private void TariffGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedTariff = TariffGrid.SelectedItem as Tariff;
        }

        private void TariffSearchTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            string search = TariffSearchTb.Text.Trim();
            if (search == "")
               TariffGrid.ItemsSource = tariffs.ToList();
            else
                TariffGrid.ItemsSource = tariffs.Where(i => i.Tariff_id.ToString() == search).ToList();
        }
    }
}
