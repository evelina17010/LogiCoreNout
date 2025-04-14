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
    /// Логика взаимодействия для UsersPage.xaml
    /// </summary>
    public partial class UsersPage : Page
    {
        public ObservableCollection<User> users { get; set; }
        private User selectedUser;
        public UsersPage()
        {
            InitializeComponent();
            LoadUsers();
        }
        private void LoadUsers()
        {
            users = new ObservableCollection<User>(DB.log.User.ToList());
            UsersGrid.ItemsSource = users;
        }
        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            var addUser = new AddUser();

            if (addUser.ShowDialog() == true)
            {
                try
                {
                    DB.log.User.Add(addUser.User);
                    DB.log.SaveChanges();
                    LoadUsers();

                    MessageBox.Show("Пользователь успешно добавлен", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        
        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            if (selectedUser == null)
            {
                MessageBox.Show("Выберите пользователя для редактирования", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var addUser = new AddUser(selectedUser);

            if (addUser.ShowDialog() == true)
            {
                try
                {
                    DB.log.SaveChanges();
                    LoadUsers();

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
        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (UsersGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите пользователя для удаления");
                return;
            }
            var selectedUser = (User)UsersGrid.SelectedItem;

            if (MessageBox.Show($"Вы уверены, что хотите удалить пользователя {selectedUser.Full_name}?",
                "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    var driver = DB.log.Driver.FirstOrDefault(d => d.User_id == selectedUser.User_id);
                    if (driver != null)
                    {
                        var truck = DB.log.Truck.FirstOrDefault(t => t.Driver_id == driver.Driver_id);
                        if (truck != null)
                        {
                            MessageBox.Show("Нельзя удалить пользователя, так как он назначен водителем грузовика",
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        DB.log.Driver.Remove(driver);
                    }
                    DB.log.User.Remove(selectedUser);
                    DB.log.SaveChanges();
                    LoadUsers();
                    MessageBox.Show("Пользователь успешно удален", "Успех", MessageBoxButton.OK);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении пользователя: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadUsers();
        }

        private void BlockUser_Click(object sender, RoutedEventArgs e)
        {
            if (UsersGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите пользователя");
                return;
            }
            var selectedUser = (User)UsersGrid.SelectedItem;

            if (selectedUser.Is_blocked == true)
            {
                MessageBox.Show("Пользователь уже заблокирован", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (MessageBox.Show($"Заблокировать пользователя {selectedUser.Full_name}?",
                "Подтверждение блокировки", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    selectedUser.Is_blocked = true;
                    DB.log.SaveChanges();
                    LoadUsers();
                    MessageBox.Show("Пользователь заблокирован", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при блокировке пользователя: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            }

        private void UsersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedUser = UsersGrid.SelectedItem as User;
        }

        private void UserSearchTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            string search = UserSearchTb.Text.Trim();
            if (search == "")
                UsersGrid.ItemsSource = users.ToList();
            else
                UsersGrid.ItemsSource = users.Where(i => i.User_id.ToString() == search).ToList();
        }
    }
}
