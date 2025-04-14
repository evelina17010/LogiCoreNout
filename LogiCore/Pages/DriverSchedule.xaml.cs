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
using LogiCore.Connection;

namespace LogiCore.Pages
{
    public class Schedule
    {
        public DateTime Date { get; set; }
        public Route Route { get; set; }
        public Order Order { get; set; }
        public string Status { get; set; }
        public string TimeSlot { get; set; }
    }
    public partial class DriverSchedule : Page
    {
        private User _Driver;
        public ObservableCollection<Schedule> Schedules { get; set; }
        public DriverSchedule(User driver)
        {
            InitializeComponent();
            _Driver = driver;
            InitializeDatePickers();
        }

        private void InitializeDatePickers()
        {
            DatePickerFrom.SelectedDate = DateTime.Today;
            DatePickerTo.SelectedDate = DateTime.Today.AddDays(7);
        }
        private void LoadSchedule(DateTime fromDate, DateTime toDate)
        {
            try
            {
                var schedule = DB.log.Order
                    .Where(o => o.Truck.Driver.User_id == _Driver.User_id &&
                           o.Scheduled_pickup >= fromDate &&
                           o.Scheduled_pickup <= toDate)
                    .AsEnumerable() 
                    .Select(o => new Schedule
                    {
                        Date = o.Scheduled_pickup.Value,
                        Route = o.Route,
                        Order = o,
                        Status = o.Status_order,
                        TimeSlot = $"{o.Scheduled_pickup:HH:mm}"
                    })
                    .OrderBy(s => s.Date)
                    .ToList();

                Schedules = new ObservableCollection<Schedule>(schedule);
                ScheduleGrid.ItemsSource = Schedules;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }


        private void ShowSchedule_Click(object sender, RoutedEventArgs e)
        {
            if (DatePickerFrom.SelectedDate == null || DatePickerTo.SelectedDate == null)
            {
                MessageBox.Show("Выберите период дат");
                return;
            }
            LoadSchedule(DatePickerFrom.SelectedDate.Value, DatePickerTo.SelectedDate.Value);
        }

        private void RefreshSchedule_Click(object sender, RoutedEventArgs e)
        {
            if (DatePickerFrom.SelectedDate != null && DatePickerTo.SelectedDate != null)
            {
                LoadSchedule(DatePickerFrom.SelectedDate.Value, DatePickerTo.SelectedDate.Value);
            }
        }
    }
}
