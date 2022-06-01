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
    /// Interaction logic for SendToDepartment.xaml
    /// </summary>
    public partial class SendToDepartment : Window
    {
        public ActionDialog ResultDialog = ActionDialog.Cancel;
        //BedStatusModel model;
        //public SendToDepartment(BedStatusModel modelData)
        //{
        //    InitializeComponent();
        //    this.Loaded += SendToDepartments_Loaded;
        //    btnCancel.Click += btnCancel_Click;
        //    this.model = modelData;
        //}

        void SendToDepartments_Loaded(object sender, RoutedEventArgs e)
        {
            var EncounterType = new List<LookupReferenceValueModel>();
            EncounterType.Add(new LookupReferenceValueModel { Key = 1, DomainCode = "ENCTTY", ValueCode = "ERDPM", Display = "Emergency" });
            EncounterType.Add(new LookupReferenceValueModel { Key = 2, DomainCode = "ENCTTY", ValueCode = "OPDDPM", Display = "OPD" });
            EncounterType.Add(new LookupReferenceValueModel { Key = 3, DomainCode = "ENCTTY", ValueCode = "IPDDPM", Display = "IPD" });
            cmbDepartment.ItemsSource = EncounterType;
            timeEditor.EditValue = DateTime.Now;
            //txtPatientName.Text = model.PatientName;
        }

        void btnSave_Click(object sender, RoutedEventArgs e)
        {

        }

        void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ResultDialog = ActionDialog.Cancel;
            this.Close();
        }
    }
}
