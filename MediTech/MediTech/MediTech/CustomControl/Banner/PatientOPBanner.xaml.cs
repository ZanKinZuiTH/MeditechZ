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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MediTech.CustomControl.Banner
{
    /// <summary>
    /// Interaction logic for PatientOPBanner.xaml
    /// </summary>
    public partial class PatientOPBanner : UserControl
    {
        #region ItemsSource Dependency Property
        public PatientVisitModel PatientVisit
        {
            get { return (PatientVisitModel)GetValue(PatientVisitProperty); }
            set { SetValue(PatientVisitProperty, value); }
        }

        public static readonly DependencyProperty PatientVisitProperty = DependencyProperty.Register("PatientVisit", typeof(PatientVisitModel)
            , typeof(PatientOPBanner), new UIPropertyMetadata(null, OnPatientVisitChanged));



        private static void OnPatientVisitChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PatientOPBanner actb = d as PatientOPBanner;
            if (actb == null) return;
            actb.OnItemsSourceChanged(e.NewValue as PatientVisitModel);

        }

        protected void OnItemsSourceChanged(PatientVisitModel source)
        {
            if (source != null)
            {
                if (source.PatientVisitUID != 0)
                {
                    SetPatientBanner(source.PatientUID, source.PatientVisitUID);
                }
                else
                {
                    SetPatientBanner(source.PatientUID, source.CareProviderName);
                }
            }
            else
            {
                ClearData();
            }
        }

        #endregion


        PatientIdentityService patientData;
        public PatientOPBanner()
        {
            InitializeComponent();
            patientData = new PatientIdentityService();
        }

        public void ClearData()
        {
            PatientName.Text = "";
            VisitTextBlock.Content = "";
            VisitIDTextBlock.Content = "";
            AddressValue.Text = "";
            DOBVal.Content = "";
            AgeLabel.Content = "";
            GenderValueText.Content = "";
            DoctorName.Text = "";
            DoctorName.ToolTip = "";
        }

        public void SetPatientBanner(long patientUID, string careprovidername = "")
        {
            var patientInfo = patientData.GetPatientByUID(patientUID);
            if (patientInfo != null)
            {
                int visitCount = patientData.GetPatientVisitByPatientUID(patientUID).Count();

                PatientName.Text = patientInfo.PatientName + " (" + patientInfo.PatientID + ") ";
                VisitTextBlock.Content = visitCount;
                VisitIDTextBlock.Content = "";
                AddressValue.Text = patientInfo.PatientAddress;
                DOBVal.Content = patientInfo.BirthDttmString;
                AgeLabel.Content = patientInfo.AgeString;
                GenderValueText.Content = patientInfo.Gender;
                DoctorName.Text = careprovidername;
                DoctorName.ToolTip = careprovidername;
            }
            else
            {
                ClearData();
            }
        }

        public void SetPatientBanner(long patientUID, long patientVisitUID)
        {
            PatientBannerModel patientVisit = patientData.GetPatientDataForBanner(patientUID, patientVisitUID);
            if (patientVisit != null)
            {
                int visitCount = patientData.GetPatientVisitByPatientUID(patientUID).Count();

                PatientName.Text = patientVisit.PatientName + " (" + patientVisit.PatientID + ") ";
                VisitTextBlock.Content = visitCount;
                VisitIDTextBlock.Content = patientVisit.VisitID;
                AddressValue.Text = patientVisit.PatientAddress;
                DOBVal.Content = patientVisit.BirthDttmString;
                AgeLabel.Content = patientVisit.AgeString;
                GenderValueText.Content = patientVisit.Gender;
                DoctorName.Text = patientVisit.CareproviderName;
                DoctorName.ToolTip = patientVisit.CareproviderName;
            }
            else
            {
                ClearData();
            }
        }
    }
}
