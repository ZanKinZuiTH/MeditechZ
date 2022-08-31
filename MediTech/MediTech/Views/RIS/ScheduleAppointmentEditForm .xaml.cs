using DevExpress.Xpf.Scheduler;
using DevExpress.Xpf.Scheduler.UI;
using DevExpress.XtraScheduler;
using MediTech.Model;
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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MediTech.Views
{
    /// <summary>
    /// Interaction logic for RadiologistAppointmentWindow.xaml
    /// </summary>
    public partial class ScheduleAppointmentEditForm : System.Windows.Controls.UserControl
    {
        MediTech.DataService.MediTechDataService mediTechDataService = new MediTech.DataService.MediTechDataService();
        SchedulerControl control;
        Appointment appointment;
        MyAppointmentFormController controller;

        ScheduleRadiologistModel model;

        public ScheduleAppointmentEditForm(SchedulerControl control, Appointment appointment)
        {
            InitializeComponent();
            this.Loaded += ScheduleAppointmentEditForm_Loaded;
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;
            this.control = control;
            this.appointment = appointment;
            this.controller = new MyAppointmentFormController(Control, Appointment);

        }

        public SchedulerControl Control { get { return control; } }
        public Appointment Appointment { get { return appointment; } }
        public MyAppointmentFormController Controller { get { return controller; } }
        public ScheduleRadiologistModel Model { get { return model; } }
        protected internal bool IsNewAppointment { get { return controller != null ? controller.IsNewAppointment : true; } }

        private void ScheduleAppointmentEditForm_Loaded(object sender, RoutedEventArgs e)
        {
            var radiologist = mediTechDataService.UserManage.GetCareproviderDoctor();
            var imageType = mediTechDataService.Technical.GetReferenceValueList("RIMTYP");
            lkeRadiologist.ItemsSource = radiologist.Where(p => p.IsRadiologist).ToList();
            cmbImageType.ItemsSource = imageType;
            dteStartDate.EditValue = appointment.Start;
            UpdateContainerCaption(Controller.Subject);
        }
        //protected override void OnVisualParentChanged(DependencyObject oldParent)
        //{
        //    base.OnVisualParentChanged(oldParent);
        //    if (oldParent == null)
        //    {
        //        UpdateContainerCaption(Controller.Subject);
        //    }
        //}

        void UpdateContainerCaption(string subject)
        {
            if (Controller.IsNewAppointment)
            {
                SchedulerFormBehavior.SetTitle(this, "New appointment");
                btnCancel.Visibility = Visibility.Collapsed;
            }
            else
            {
                var scheduleList = Control.Storage.AppointmentStorage.DataSource as ObservableCollection<ScheduleRadiologistModel>;
                if (scheduleList != null)
                {
                    model = scheduleList.FirstOrDefault(p => p.ScheduleRadiologistUID == int.Parse(Appointment.Id.ToString()));
                }
                SchedulerFormBehavior.SetTitle(this, "Edit - [" + Appointment.Subject + "]");
                lkeRadiologist.SelectedItem = (lkeRadiologist.ItemsSource as List<CareproviderModel>).FirstOrDefault(p => p.CareproviderUID == model.CareproviderUID);
                cmbImageType.SelectedItem = (cmbImageType.ItemsSource as List<LookupReferenceValueModel>).FirstOrDefault(p => p.Key == model.RIMTYPUID);
                if (model.StartWorkTime.ToString("HH:mm") == "19:00")
                {
                    rad1.IsChecked = true;
                }
                else
                {
                    rad2.IsChecked = true;
                }
                btnCancel.Visibility = Visibility.Visible;
            }
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lkeRadiologist.SelectedItem == null)
                {
                    System.Windows.Forms.MessageBox.Show("กรุณาเลือกรังสีแพทย์", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if ((rad1.IsChecked ?? false) == false && (rad2.IsChecked ?? false) == false)
                {
                    System.Windows.Forms.MessageBox.Show("กรุณาเลือกเวลา", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (model == null)
                {
                    model = new ScheduleRadiologistModel();
                }
                model.CareproviderUID = (lkeRadiologist.SelectedItem as CareproviderModel).CareproviderUID;
                model.CareproviderName = (lkeRadiologist.SelectedItem as CareproviderModel).FullName;
                if (cmbImageType.SelectedItem != null)
                {
                    model.RIMTYPUID = (cmbImageType.SelectedItem as LookupReferenceValueModel).Key;
                    model.ImageType = (cmbImageType.SelectedItem as LookupReferenceValueModel).Display;
                }
                if (rad1.IsChecked ?? false)
                {
                    model.StartWorkTime = Convert.ToDateTime(dteStartDate.DateTime.Date.ToString("dd/MM/yyyy ").ToString() + "19:00:00");
                    model.EndWorkTime = Convert.ToDateTime(dteStartDate.DateTime.AddDays(1).Date.ToString("dd/MM/yyyy ").ToString() + "00:59:00");
                }
                else
                {
                    model.StartWorkTime = Convert.ToDateTime(dteStartDate.DateTime.AddDays(1).Date.ToString("dd/MM/yyyy ").ToString() + "01:00:00");
                    model.EndWorkTime = Convert.ToDateTime(dteStartDate.DateTime.AddDays(1).Date.ToString("dd/MM/yyyy ").ToString() + "07:00:00");
                }

                model.AllDay = false;
                model.Status = 2;
                model.Label = 1;
                model.EventType = 0;

                int scheduleRadiologistUID = mediTechDataService.Radiology.AddOrUpdateScheDuleRadiologist(model, AppUtil.Current.UserID);
                //System.Windows.Forms.MessageBox.Show("บันทึกข้อมูลเรียบร้อย", "Sucess", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (model.ScheduleRadiologistUID == 0)
                    model.ScheduleRadiologistUID = scheduleRadiologistUID;

                model.StartWorkTime = model.StartWorkTime.ToString("HH:mm") == "01:00" ? model.StartWorkTime.AddDays(-1) : model.StartWorkTime;
                model.EndWorkTime = model.EndWorkTime.ToString("HH:mm") == "07:00" ? model.EndWorkTime.AddDays(-1) : model.EndWorkTime.ToString("HH:mm") == "00:59" ? model.EndWorkTime.AddHours(-1) : model.EndWorkTime;

                if (Control.Storage.AppointmentStorage.DataSource != null)
                {
                    var scheduleList = Control.Storage.AppointmentStorage.DataSource as ObservableCollection<ScheduleRadiologistModel>;
                    if (scheduleList != null)
                    {
                        var selectSchedule = scheduleList.FirstOrDefault(p => p.ScheduleRadiologistUID == model.ScheduleRadiologistUID);

                        if (selectSchedule != null)
                        {
                            selectSchedule.ScheduleRadiologistUID = model.ScheduleRadiologistUID;
                            selectSchedule.CareproviderUID = model.CareproviderUID;
                            selectSchedule.CareproviderName = model.CareproviderName;
                            selectSchedule.ImageType = model.ImageType;
                            selectSchedule.RIMTYPUID = model.RIMTYPUID;
                            selectSchedule.StartWorkTime = model.StartWorkTime;
                            selectSchedule.EndWorkTime = model.EndWorkTime;
                            selectSchedule.AllDay = false;
                            selectSchedule.Status = 2;
                            selectSchedule.Label = 1;
                            selectSchedule.EventType = 0;
                        }
                        else
                        {
                            scheduleList.Add(model);
                        }

                    }

                }

                Control.Storage.RefreshData();
                SchedulerFormBehavior.Close(this, false);
            }
            catch (Exception ex)
            {

                System.Windows.Forms.MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (appointment.Id != null)
                {
                    DialogResult result = System.Windows.Forms.MessageBox.Show("คุณต้องการยกเลิกตารางนี้ ใช่หรือไม่", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        int scheduleRadiologistUID = Convert.ToInt32(appointment.Id);
                        mediTechDataService.Radiology.DeleteScheDuleRadiologist(scheduleRadiologistUID, AppUtil.Current.UserID);
                        var scheduleList = Control.Storage.AppointmentStorage.DataSource as ObservableCollection<ScheduleRadiologistModel>;
                        if (scheduleList != null)
                        {
                            var selectSchedule = scheduleList.FirstOrDefault(p => p.ScheduleRadiologistUID == model.ScheduleRadiologistUID);
                            scheduleList.Remove(selectSchedule);
                        }
                        Control.Storage.RefreshData();
                    }

                }
                SchedulerFormBehavior.Close(this, false);
            }
            catch (Exception ex)
            {

                System.Windows.Forms.MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }

    public class MyAppointmentFormController : AppointmentFormController
    {

        public string CustomName { get { return (string)EditedAppointmentCopy.CustomFields["CustomName"]; } set { EditedAppointmentCopy.CustomFields["CustomName"] = value; } }
        public string CustomStatus { get { return (string)EditedAppointmentCopy.CustomFields["CustomStatus"]; } set { EditedAppointmentCopy.CustomFields["CustomStatus"] = value; } }

        string SourceCustomName { get { return (string)SourceAppointment.CustomFields["CustomName"]; } set { SourceAppointment.CustomFields["CustomName"] = value; } }
        string SourceCustomStatus { get { return (string)SourceAppointment.CustomFields["CustomStatus"]; } set { SourceAppointment.CustomFields["CustomStatus"] = value; } }

        public MyAppointmentFormController(SchedulerControl control, Appointment apt)
            : base(control, apt)
        {
        }

        public override bool IsAppointmentChanged()
        {
            if (base.IsAppointmentChanged())
                return true;
            return SourceCustomName != CustomName ||
                SourceCustomStatus != CustomStatus;
        }

        protected override void ApplyCustomFieldsValues()
        {
            SourceCustomName = CustomName;
            SourceCustomStatus = CustomStatus;
        }
    }
}
