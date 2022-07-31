using DevExpress.Xpf.Core;
using MediTech.DataService;
using MediTech.Model;
using MediTech.ViewModels;
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

namespace MediTech.Views
{
    /// <summary>
    /// Interaction logic for SendToDoctor.xaml
    /// </summary>
    public partial class PatientStatus : DXWindow
    {

        public ActionDialog ResultDialog = ActionDialog.Cancel;

        PatientVisitModel model;
        int? admissionUID;
        int? ENSTAUID;
        PatientStatusType type;
        public PatientStatus(PatientVisitModel modelData, PatientStatusType type, int? admissionEventUID)
        {
            InitializeComponent();
            this.Loaded += PatientStatus_Loaded;
            btnSave.Click += btnSave_Click;
            btnCancel.Click += btnCancel_Click;
            cmbStatus.EditValueChanged += cmbStatus_EditValueChanged;
            this.model = modelData;
            this.type = type;
            this.admissionUID = admissionEventUID;
        }

        void cmbStatus_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (cmbStatus.EditValue != null)
            {
                if ((int)cmbStatus.EditValue == 419)
                {
                    lytDoctor.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    lytDoctor.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }


        void PatientStatus_Loaded(object sender, RoutedEventArgs e)
        {
            var org = (new MediTechViewModelBase()).GetLocatioinRole(AppUtil.Current.OwnerOrganisationUID);
            cmbLocations.ItemsSource = org.Where(p => p.IsRegistrationAllowed == "Y").ToList();
            cmbStatus.ItemsSource = (new TechnicalService()).GetReferenceValueMany("VISTS").Where(p => p.ValueCode == "REGST" || p.ValueCode == "ARRVD"
            || p.ValueCode == "ARRWI" || p.ValueCode == "SNDDOC" || p.ValueCode == "AWRES" || p.ValueCode == "RFLSTS").ToList();
            
            if (type == PatientStatusType.WardArrived)
            {
                txtTitle.Text = "Arrived";
                lytDoctor.Visibility = System.Windows.Visibility.Collapsed;
                lytStatus.Visibility = System.Windows.Visibility.Collapsed;
                lytDepatment.Visibility = System.Windows.Visibility.Collapsed;

                var status = (new TechnicalService()).GetReferenceValueByCode("ENSTA", "ADMIT");
                var location = ((List<LocationModel>)cmbLocations.ItemsSource).FirstOrDefault(p => p.LocationUID == AppUtil.Current.LocationUID);

                this.ENSTAUID = status.Key;

                cmbWardStatus.Text = status.Display;
                cmbWard.Text = model.LocationName;
                cmbBed.Text = model.BedName;
                cmbWardDoctor.Text = model.CareProviderName;
                cmbWardLocation.Text = location.Name;
            }

            if (cmbStatus.ItemsSource != null && type != PatientStatusType.WardArrived)
            {
                GroupWard.Visibility = System.Windows.Visibility.Collapsed;
                GroupWardStatus.Visibility = System.Windows.Visibility.Collapsed;
                GroupWardDoctor.Visibility = System.Windows.Visibility.Collapsed;
                GroupWardLocation.Visibility = System.Windows.Visibility.Collapsed;

                if (type == PatientStatusType.SendToDoctor)
                {
                    txtTitle.Text = "Send To Doctor";
                    cmbStatus.SelectedItem = ((List<LookupReferenceValueModel>)cmbStatus.ItemsSource).FirstOrDefault(p => p.ValueCode == "SNDDOC");
                    cmbDoctor.ItemsSource = (new UserManageService()).GetCareproviderDoctor();
                    cmbLocations.SelectedItem = ((List<LocationModel>)cmbLocations.ItemsSource).FirstOrDefault(p => p.LocationUID == AppUtil.Current.LocationUID);

                }
                else if (type == PatientStatusType.MedicalDischarge)
                {
                    cmbStatus.ItemsSource = (new TechnicalService()).GetReferenceValueMany("VISTS").Where(p => p.ValueCode == "CHKOUT" || p.ValueCode == "SNDDOC" || p.ValueCode == "ARRVD").ToList();
                    txtTitle.Text = "Medical Discharge";
                    cmbLocations.SelectedItem = ((List<LocationModel>)cmbLocations.ItemsSource).FirstOrDefault(p => p.LocationUID == AppUtil.Current.LocationUID);

                    if (model.VISTSUID != 418)
                    {
                        cmbStatus.SelectedItem = ((List<LookupReferenceValueModel>)cmbStatus.ItemsSource).FirstOrDefault(p => p.ValueCode == "CHKOUT");
                        cmbStatus.IsEnabled = false;
                    }
                    else
                    {
                        cmbStatus.SelectedItem = ((List<LookupReferenceValueModel>)cmbStatus.ItemsSource).FirstOrDefault(p => p.ValueCode == "ARRVD");
                        cmbStatus.IsEnabled = false;
                    }
                }
                else if (type == PatientStatusType.Arrive)
                {
                    txtTitle.Text = "Change Status";
                    cmbDoctor.ItemsSource = (new UserManageService()).GetCareproviderDoctor();
                    cmbStatus.SelectedItem = ((List<LookupReferenceValueModel>)cmbStatus.ItemsSource).FirstOrDefault(p => p.Key == model.VISTSUID);
                    if (model.LocationUID != null && model.LocationUID != 0)
                    {
                        cmbLocations.SelectedItem = ((List<LocationModel>)cmbLocations.ItemsSource).FirstOrDefault(p => p.LocationUID == model.LocationUID);
                    }

                    if (model.VISTSUID == 419)
                    {
                        lytDoctor.Visibility = System.Windows.Visibility.Visible;
                        cmbDoctor.SelectedItem = ((List<CareproviderModel>)cmbDoctor.ItemsSource).FirstOrDefault(p => p.CareproviderUID == model.CareProviderUID);
                    }
                    else
                    {
                        lytDoctor.Visibility = System.Windows.Visibility.Collapsed;
                    }
                }
            }

            timeEditor.EditValue = DateTime.Now;
            txtPatientName.Text = model.PatientName;
            cmbDoctor.EditValue = model.CareProviderUID;
        }

        void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ResultDialog = ActionDialog.Cancel;
            this.Close();
        }

        void btnSave_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                int? CareProviderUID = null;
                DateTime arriveTime = timeEditor.DateTime;
                int? LocationUID = (int?)cmbLocations.EditValue != null ? (int?)cmbLocations.EditValue : model.LocationUID;
                
                if (type == PatientStatusType.SendToDoctor)
                {
                    if (cmbDoctor.EditValue == null)
                    {
                        return;
                    }
                    CareProviderUID = (int)cmbDoctor.EditValue;
                }
                else if (type == PatientStatusType.MedicalDischarge)
                {
                    CareProviderUID = model.CareProviderUID;
                }
                else if (type == PatientStatusType.Arrive)
                {
                    CareProviderUID = model.CareProviderUID;

                    if ((int)cmbStatus.EditValue == 419)
                    {
                        if (cmbDoctor.EditValue == null)
                        {
                            (new MediTechViewModelBase()).WarningDialog("กรุณาเลือกแพทย์");
                            return;
                        }

                        CareProviderUID = (int)cmbDoctor.EditValue;
                    }
                }

                int VisitTypeUID  = cmbStatus.EditValue != null ? (int)cmbStatus.EditValue : model.VISTSUID ?? 0;

                //if (type == PatientStatusType.WardArrived)
                //    (int)cmbStatus.EditValue = 408;

                (new PatientIdentityService()).ChangeVisitStatus(model.PatientVisitUID, VisitTypeUID, CareProviderUID, LocationUID, arriveTime, AppUtil.Current.UserID,admissionUID, ENSTAUID);
                ResultDialog = ActionDialog.Save;
                this.Close();
            }
            catch (Exception er)
            {

                MessageBox.Show(er.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
