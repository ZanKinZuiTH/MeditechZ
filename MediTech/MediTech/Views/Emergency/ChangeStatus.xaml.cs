using MediTech.DataService;
using MediTech.Model;
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
    public partial class ChangeStatus : Window
    {

        public ActionDialog ResultDialog = ActionDialog.Cancel;

        BedStatusModel model;
        string type;
        public ChangeStatus(BedStatusModel modelData, string type)
        {
            InitializeComponent();
            this.Loaded += PatientStatus_Loaded;
            btnSave.Click += btnSave_Click;
            btnCancel.Click += btnCancel_Click;
            cmbStatus.EditValueChanged += cmbStatus_EditValueChanged;
            this.model = modelData;
            this.type = type;
        }

        void cmbStatus_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (cmbStatus.EditValue != null)
            {
                //if ((int)cmbStatus.EditValue == 419)
                //{
                //    lytDoctor.Visibility = System.Windows.Visibility.Visible;
                //}
                //else
                //{
                //    lytDoctor.Visibility = System.Windows.Visibility.Collapsed;
                //}
            }
        }


        void PatientStatus_Loaded(object sender, RoutedEventArgs e)
        {

            string SevereType = model.SevereCode;
                cmbStatus.ItemsSource = (new List<LookupReferenceValueModel> {
                new LookupReferenceValueModel { Key = 1, DomainCode = "TRIAGE", ValueCode = "TRIAGE1", Display = "สีแดง(emergency/immediate)" },
                new LookupReferenceValueModel { Key = 2, DomainCode = "TRIAGE", ValueCode = "TRIAGE2", Display = "สีเหลือง(urgent)" },
                new LookupReferenceValueModel { Key = 3, DomainCode = "TRIAGE", ValueCode = "TRIAGE3", Display = "สีเขียว(delayed)" },
                new LookupReferenceValueModel { Key = 4, DomainCode = "TRIAGE", ValueCode = "TRIAGE4", Display = "สีฟ้า(expectant)" },
                new LookupReferenceValueModel { Key = 5, DomainCode = "TRIAGE", ValueCode = "TRIAGE5", Display = "สีดำ(dead)" }
            });

            if (cmbStatus.ItemsSource != null)
            {
                //if (type == PatientStatusType.SendToDoctor)
                //{
                //    txtTitle.Text = "Send To Doctor";
                //    cmbStatus.SelectedItem = ((List<LookupReferenceValueModel>)cmbStatus.ItemsSource).FirstOrDefault(p => p.ValueCode == "SNDDOC");
                //    //cmbDoctor.ItemsSource = (new UserManageService()).GetCareproviderDoctor();
                //}
                //else if (type == PatientStatusType.MedicalDischarge)
                //{
                //    txtTitle.Text = "Medical Discharge";
                //    cmbStatus.SelectedItem = ((List<LookupReferenceValueModel>)cmbStatus.ItemsSource).FirstOrDefault(p => p.ValueCode == "CHKOUT");
                //}

                if (SevereType == "TRIAGE1")
                {
                    cmbStatus.SelectedItem = ((List<LookupReferenceValueModel>)cmbStatus.ItemsSource).FirstOrDefault(p => p.ValueCode == "TRIAGE1");
                    //cmbDoctor.ItemsSource = (new UserManageService()).GetCareproviderDoctor();
                }
                else if (SevereType == "TRIAGE2")
                {
                    cmbStatus.SelectedItem = ((List<LookupReferenceValueModel>)cmbStatus.ItemsSource).FirstOrDefault(p => p.ValueCode == "TRIAGE2");
                }
                else if (SevereType == "TRIAGE3")
                {
                    cmbStatus.SelectedItem = ((List<LookupReferenceValueModel>)cmbStatus.ItemsSource).FirstOrDefault(p => p.ValueCode == "TRIAGE3");
                }
                else if (SevereType == "TRIAGE4")
                {
                    cmbStatus.SelectedItem = ((List<LookupReferenceValueModel>)cmbStatus.ItemsSource).FirstOrDefault(p => p.ValueCode == "TRIAGE4");
                }
                else if (SevereType == "TRIAGE5")
                {

                    cmbStatus.SelectedItem = ((List<LookupReferenceValueModel>)cmbStatus.ItemsSource).FirstOrDefault(p => p.ValueCode == "TRIAGE5");
                }

            }

            timeEditor.EditValue = DateTime.Now;
            txtPatientName.Text = model.PatientName;
            //cmbDoctor.EditValue = model.CareProviderUID;
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

                //if (type == PatientStatusType.SendToDoctor)
                //{
                //    if (cmbDoctor.EditValue == null)
                //    {
                //        return;
                //    }

                //    CareProviderUID = (int)cmbDoctor.EditValue;
                //}
                //else if(type == PatientStatusType.MedicalDischarge)
                //{
                //    CareProviderUID = model.CareProviderUID;
                //}


                //(new PatientIdentityService()).ChangeVisitStatus(model.PatientVisitUID, (int)cmbStatus.EditValue, CareProviderUID, arriveTime, AppUtil.Current.UserID);
                //ResultDialog = ActionDialog.Save;
                this.Close();
            }
            catch (Exception er)
            {

                MessageBox.Show(er.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
