using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogiCore.Connection;

namespace LogiCore.Function
{
   public class Authorisation
    {
    
        public static ObservableCollection<User> users { get; set; }
        public static User AuthorisationSotr(string email, string password)
        {
            users = new ObservableCollection<User>(DB.log.User.ToList());
            var userExists = users.Where(users => users.Email == email && users.Password == password).FirstOrDefault();
            if (userExists != null)
            {
                return userExists;
            }
            else
            {
                return userExists;
            }
        }   
}
}
