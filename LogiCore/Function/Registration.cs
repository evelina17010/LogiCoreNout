using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using LogiCore.Connection;

namespace LogiCore.Function
{
    public class Registration
    {
        public static ObservableCollection <User> users { get; set; }
        public static void RegistrationUser(string fio, string phone, string email, string password,int role_id)
        {
            User user = new User();
          user.Full_name=fio.Trim();
            user.Phone = phone.Trim();
            user.Email = email.Trim();
            user.Password = password.Trim();
            user.Role_id=role_id;
            user.Is_blocked = false;
            user.Created_at = DateTime.Now;
            DB.log.User.Add(user);
            DB.log.SaveChanges();
        }
    }
}
