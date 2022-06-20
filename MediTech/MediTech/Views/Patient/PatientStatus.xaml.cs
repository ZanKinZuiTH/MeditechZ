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
    public partial class PatientStatus : Window
    {

        public ActionDialog ResultDialog = ActionDialog.Cancel;

        PatientVisitModel model;
        PatientStatusType type;
        public PatientStatus(PatientVisitModel modelData, PatientStatusType type)
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

            cmbStatus.ItemsSource = (new TechnicalService()).GetReferenceValueMany("VISTS").Where(p => p.ValueCode == "CHKOUT" || p.ValueCode == "SNDDOC" || p.ValueCode == "ARRVD").ToList();
            if (cmbStatus.ItemsSource != null)
            {
                if (type == PatientStatusType.SendToDoctor)
                {
                    txtTitle.Text = "Send To Doctor";
                    cmbStatus.SelectedItem = ((List<LookupReferenceValueModel>)cmbStatus.ItemsSource).FirstOrDefault(p => p.ValueCode == "SNDDOC");
                    cmbDoctor.ItemsSource = (new UserManageService()).GetCareproviderDoctor();
                }
                else if (type == PatientStatusType.MedicalDischarge)
                {
                    txtTitle.Text = "Medical Discharge";
                    cmbStatus.SelectedItem = ((List<LookupReferenceValueModel>)cmbStatus.ItemsSource).FirstOrDefault(p => p.ValueCode == "CHKOUT");
                }
                else if (type == PatientStatusType.Arrive)
                {
                    txtTitle.Text = "Arrived";
                    cmbStatus.SelectedItem = ((List<LookupReferenceValueModel>)cmbStatus.ItemsSource).FirstOrDefault(p => p.ValueCode == "ARRVD");
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

                if (type == PatientStatusType.SendToDoctor)
                {
                    if (cmbDoctor.EditValue == null)
                    {
                        return;
                    }

                    CareProviderUID = (int)cmbDoctor.EditValue;
                }
                else if(type == PatientStatusType.MedicalDischarge)
                {
                    CareProviderUID = model.CareProviderUID;
                }


                (new PatientIdentityService()).ChangeVisitStatus(model.PatientVisitUID, (int)cmbStatus.EditValue, CareProviderUID, arriveTime, AppUtil.Current.UserID);
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
