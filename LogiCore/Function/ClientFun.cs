using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using LogiCore.Connection;

namespace LogiCore.Function
{
    public class ClientFun
    {

        public static ObservableCollection<User> users { get; set; }
        public static ObservableCollection<Order> orders { get; set; }
        public static ObservableCollection<Payment> payments { get; set; }
        public static ObservableCollection<Cargo> cargos { get; set; }
        public static void CreateOrder(int client_id,string description, string from, string to, double weight, double volume, DateTime cargodate)
        {
            Cargo cargo = new Cargo();
            cargo.Client_id = client_id;
            cargo.Description = description.Trim();
            cargo.Pickup_address = from.Trim();
            cargo.Delivery_address = to.Trim();
            cargo.Weight = (decimal)weight;
            cargo.Volume = (decimal)volume;
            cargo.Date = cargodate;
            DB.log.Cargo.Add(cargo);
          DB.log.SaveChanges();
        }
        public static ObservableCollection<Payment> GetPayments(int userid)
        {
            var payments = DB.log.Payment.Where(p => p.Order.Cargo.Client_id == userid).ToList();
            return new ObservableCollection<Payment>(payments);
        }
        public static void CreateOrder(int client_id, string description, string from, string to, double weight, double volume, DateTime cargodate, DateTime date, decimal cost, double distance, int tariffId)
        {
            Cargo cargo = new Cargo();
            cargo.Client_id = client_id;
            cargo.Description = description.Trim();
            cargo.Pickup_address = from.Trim();
            cargo.Delivery_address = to.Trim();
            cargo.Weight = (decimal)weight;
            cargo.Volume = (decimal)volume;
            cargo.Date = cargodate;
            DB.log.Cargo.Add(cargo);
            DB.log.SaveChanges();
        }
    }
}

