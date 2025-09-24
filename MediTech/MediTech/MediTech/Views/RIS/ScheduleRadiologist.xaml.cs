using DevExpress.Xpf.Core;
using DevExpress.Xpf.Scheduler;
using DevExpress.XtraScheduler;
using MediTech.Model;
using MediTech.ViewModels;
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

namespace MediTech.Views
{
    /// <summary>
    /// Interaction logic for ScheduleRadiologist.xaml
    /// </summary>
    public partial class ScheduleRadiologist : UserControl
    {
        public ScheduleRadiologist()
        {
            InitializeComponent();
            this.scheControl.PopupMenuShowing += SchedulerControl_PopupMenuShowing;
            this.scheControl.EditAppointmentFormShowing += ScheControl_EditAppointmentFormShowing;
            this.scheControl.VisibleIntervalChanged += ScheControl_VisibleIntervalChanged;
            this.scheControl.AppointmentDrop += ScheControl_AppointmentDrop;
            this.Loaded += ScheduleRadiologist_Loaded;
        }

        private void ScheControl_AppointmentDrop(object sender, AppointmentDragEventArgs e)
        {
            string moveEventMsg = "คุณต้องการย้ายตารางเวลารังสีแพทย์ {0} จากวันที่ {1} เวลา {2} ไปวันที่ {3} เวลา {4} หรือไม่?";
            DateTime srcStart = e.SourceAppointment.Start;
            DateTime newStart = e.EditedAppointment.Start;

            string msg = String.Format(moveEventMsg, e.SourceAppointment.Subject, srcStart.ToShortDateString(), srcStart.ToShortTimeString(), newStart.ToShortDateString(), newStart.ToShortTimeString());

            if (DXMessageBox.Show(msg, "Calendar", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                e.Allow = false;
            }

            var scheduleList = scheControl.Storage.AppointmentStorage.DataSource as ObservableCollection<ScheduleRadiologistModel>;
            if (scheduleList != null)
            {
                var model = scheduleList.FirstOrDefault(p => p.ScheduleRadiologistUID == int.Parse(e.SourceAppointment.Id.ToString()));

                if (srcStart.ToString("HH:mm") == "19:00")
                {
                    model.StartWorkTime = Convert.ToDateTime(newStart.Date.ToString("dd/MM/yyyy ").ToString() + "19:00:00");
                    model.EndWorkTime = Convert.ToDateTime(newStart.AddDays(1).Date.ToString("dd/MM/yyyy ").ToString() + "00:59:00");
                }
                else if (srcStart.ToString("HH:mm") == "01:00")
                {
                    model.StartWorkTime = Convert.ToDateTime(newStart.AddDays(1).Date.ToString("dd/MM/yyyy ").ToString() + "01:00:00");
                    model.EndWorkTime = Convert.ToDateTime(newStart.AddDays(1).Date.ToString("dd/MM/yyyy ").ToString() + "07:00:00");
                }
                (new DataService.MediTechDataService()).Radiology.AddOrUpdateScheDuleRadiologist(model, AppUtil.Current.UserID);
            }
            scheControl.Focus();
        }

        private void ScheduleRadiologist_Loaded(object sender, RoutedEventArgs e)
        {
            ScheControl_VisibleIntervalChanged(null, null);
        }

        private void ScheControl_VisibleIntervalChanged(object sender, EventArgs e)
        {
            TimeIntervalCollection timeInterval = scheControl.MonthView.GetVisibleIntervals();
            DateTime start = timeInterval.Start;
            DateTime end = timeInterval.End;
            (this.DataContext as ScheduleRadiologistViewModel).RenderScheduleRadiologist(start, end);
            scheControl.Storage.RefreshData();
        }

        private void ScheControl_EditAppointmentFormShowing(object sender, DevExpress.Xpf.Scheduler.EditAppointmentFormEventArgs e)
        {
            SchedulerControl control = sender as SchedulerControl;
            if (Object.Equals(control, null))
                return;
            e.Form = new ScheduleAppointmentEditForm(sender as SchedulerControl, e.Appointment);
            e.AllowResize = false;
        }

        private void SchedulerControl_PopupMenuShowing(object sender, DevExpress.Xpf.Scheduler.SchedulerMenuEventArgs e)
        {
            e.Handled = true;
        }
    }
}
