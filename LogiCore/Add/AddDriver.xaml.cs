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
    /// Логика взаимодействия для AddDriver.xaml
    /// </summary>
    public partial class AddDriver : Window
    {
        public Driver Driver { get;  set; }
        private bool isEditMode = false;
        public AddDriver()
        {
            InitializeComponent();
            InitializeWindow(null);
        }
        public AddDriver(Driver driverToEdit)
        {
            InitializeComponent();
            InitializeWindow(driverToEdit);
            isEditMode = true;
        }

        private void InitializeWindow(Driver driver)
        {
            if (driver != null)
            {
                Driver = driver;
                txbName.Text = driver.User.Full_name;
                txbEmail.Text = driver.User.Email;
                txbNumber.Text = driver.User.Phone;
                txtLicenseNumber.Text = driver.License_number;
                cmbStatus.Text = driver.Status_driver;
            }
            else
            {
                Driver = new Driver
                {
                    User = new User
                    {
                        Role_id = 4, 
                        Is_blocked = false,
                        Created_at = DateTime.Now,
                        Password = "123456" 
                    }
                };
            }
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txbName.Text==null ||txtLicenseNumber.Text==null ||
                  cmbStatus.Text==null)
                {
                    MessageBox.Show("Заполните все обязательные поля!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!isEditMode)
                {
                    Driver = new Driver
                    {
                        User = new User()
                    };
                }
                Driver.User.Full_name = txbName.Text.Trim();
                Driver.User.Email = txbEmail.Text.Trim();
                Driver.User.Phone = txbNumber.Text.Trim();
                Driver.License_number = txtLicenseNumber.Text.Trim();
                Driver.Status_driver = cmbStatus.Text;
                Driver.User.Password = "123456";
                Driver.User.Role_id = 4; 

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
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
    

