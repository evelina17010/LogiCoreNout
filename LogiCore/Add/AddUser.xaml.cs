using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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
    /// Логика взаимодействия для AddUser.xaml
    /// </summary>
    public partial class AddUser : Window
    {
        public User User { get; private set; }
        public ObservableCollection<Role> Roles { get; set; }
        private bool isEditMode = false;

        public AddUser()
        {
            InitializeComponent();
            InitializeWindow(null);
        }
        public AddUser(User userToEdit)
        {
            InitializeComponent();
            InitializeWindow(userToEdit);
            isEditMode = true;
        }

        private void InitializeWindow(User user)
        {
            Roles = new ObservableCollection<Role>(DB.log.Role.ToList());
            DataContext = this;

            if (user != null)
            {
                User = user;
                txbName.Text = user.Full_name;
                txbEmail.Text = user.Email;
                txbNumber.Text = user.Phone;
                txbPassword.Text = user.Password;
                CmbRole.SelectedItem = Roles.FirstOrDefault(r => r.Role_id == user.Role_id);
            }
            else
            {
                User = new User
                {
                    Created_at = DateTime.Now,
                    Is_blocked = false
                };
            }
        }
        private void btnReg_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                if (txbName.Text==null ||txbEmail.Text ==null||
                   txbNumber.Text==null ||txbPassword.Text== null ||
                    CmbRole.SelectedItem == null)
                {
                    MessageBox.Show("Заполните все обязательные поля!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (!isEditMode)
                {
                    User = new User();
                }
                User.Full_name = txbName.Text.Trim();
                User.Phone = txbNumber.Text.Trim();
                User.Email = txbEmail.Text.Trim();
               User.Password = txbPassword.Text.Trim();
                User.Role_id = ((Role)CmbRole.SelectedItem).Role_id;
                User.Is_blocked = false;
                User.Created_at = DateTime.Now;
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
