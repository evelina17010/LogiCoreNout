using System;
using System.Collections.Generic;
using System.Data;
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
using System.Collections.ObjectModel;
using LogiCore.Connection;
using LogiCore.Function;

namespace LC.Pages
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Page
    {
        public Registration()
        {
            InitializeComponent();
        }

        private void btnReg_Click(object sender, RoutedEventArgs e)
        {
            string fio = txbName.Text;
            string phone= txbNumber.Text;
            string email= txbEmail.Text;
            string password= txbPassword.Text;           
            int role_id = 2;
            if (txbName != null && txbNumber != null && txbEmail!= null && txbNumber != null && txbPassword!=null )
            {
                LogiCore.Function.Registration.RegistrationUser(fio, phone, email, password, role_id);
                System.Windows.MessageBox.Show("Регистрация прошла успешна");
                NavigationService.Navigate(new Entrance());
              
            }
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
