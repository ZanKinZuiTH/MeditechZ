using MediTech.DataService;
using MediTech.Model;
using MediTech.ViewModels;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for PatientBanner.xaml
    /// </summary>
    public partial class PatientBanner : UserControl
    {
        #region ItemsSource Dependency Property
        public PatientVisitModel PatientVisit
        {
            get { return (PatientVisitModel)GetValue(PatientVisitProperty); }
            set { SetValue(PatientVisitProperty, value); }
        }

        public static readonly DependencyProperty PatientVisitProperty = DependencyProperty.Register("PatientVisit", typeof(PatientVisitModel)
            , typeof(PatientBanner), new UIPropertyMetadata(null, OnPatientVisitChanged));




        private static void OnPatientVisitChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PatientBanner actb = d as PatientBanner;
            if (actb == null) return;
            actb.OnItemsSourceChanged(e.NewValue as PatientVisitModel);

        }

        protected void OnItemsSourceChanged(PatientVisitModel source)
        {
            if (source != null)
            {
                SetPatientBanner(source.PatientUID, source.PatientVisitUID);
            }
            else
            {
                ClearData();
            }
        }

        #endregion

        PatientIdentityService patentData;
        PatientBannerModel patientVisit;
        public PatientBanner()
        {
            InitializeComponent();
            //imaAllergy.MouseLeftButtonDown += imaAllergy_MouseLeftButtonDown;
            patentData = new PatientIdentityService();
        }

        //void imaAllergy_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    PatientAllergy pageview = new PatientAllergy();
        //    var dataContext = (pageview.DataContext as PatientAllergyViewModel);
        //    PatientVisitModel visitModel = new PatientVisitModel();
        //    visitModel.PatientUID = patientVisit.PatientUID;
        //    visitModel.PatientVisitUID = patientVisit.PatientVisitUID;
        //    dataContext.AssingPatientVisit(visitModel);
        //    PatientAllergyViewModel result = (PatientAllergyViewModel)dataContext.LaunchViewDialog(pageview, "LIARGY", false);
        //    if (result != null && result.ResultDialog == ActionDialog.Save)
        //    {
        //        dataContext.SaveSuccessDialog();
        //    }
        //}

        public void ClearData()
        {
            txtHN.Text = "";
            txtAge.Text = "";
            txtDateOfBirth.Text = "";
            txtPateintName.Text = "";
            txtGender.Text = "";
            txtBloodGroup.Text = "";
            txtCareProvider.Text = "";
            txtAddress.Text = "";
            txtWgt.Text = "";
            txtHgt.Text = "";
            txtBMI.Text = "";
            txtMobile.Text = "";
            txtPhone.Text = "";
            imgAllergy.Visibility = System.Windows.Visibility.Collapsed;
            imgVIP.Visibility = Visibility.Collapsed;
            Uri uri = new Uri(@"pack://application:,,,/MediTech;component/Resources/Images/Other/Blue-Pictures-icon.png");
            BitmapImage image = new BitmapImage(uri);
            patientImage.Source = image;
            patientImageTootip.Source = image;
            txtVisitID.Text = "";
            txtPastVisits.Text = "";
        }

        public void SetPatientBanner(long patientUID, long patientVisitUID)
        {
            patientVisit = patentData.GetPatientDataForBanner(patientUID, patientVisitUID);
            if (patientVisit != null)
            {
                txtHN.Text = patientVisit.PatientID;
                txtAge.Text = patientVisit.AgeString;
                txtPateintName.Text = patientVisit.PatientName;
                txtDateOfBirth.Text = patientVisit.BirthDttmString;
                txtGender.Text = patientVisit.Gender;
                txtBloodGroup.Text = patientVisit.BloodGroup;
                txtCareProvider.Text = patientVisit.CareproviderName;
                txtAddress.Text = patientVisit.PatientAddress;
                txtWgt.Text = patientVisit.Weight.ToString() ?? "";
                txtHgt.Text = patientVisit.Height.ToString() ?? "";
                txtBMI.Text = patientVisit.BMI.ToString() ?? "";
                txtMobile.Text = patientVisit.MobilePhone;
                txtPhone.Text = patientVisit.SecondPhone;
                txtVisitID.Text = patientVisit.VisitID?.ToString();
                txtPastVisits.Text = (patientVisit.VisitCount?.ToString() != "0" && patientVisit.VisitCount?.ToString() != "") ? "Past Vists : " + patientVisit.VisitCount.ToString() ?? "" : "";

                if (patientVisit.IsAllergy)
                {
                    imgAllergy.Visibility = System.Windows.Visibility.Visible;
                    dtgAllergy.ItemsSource = patentData.GetPatientAllergyByPatientUID(patientUID);
                }
                else
                {
                    imgAllergy.Visibility = System.Windows.Visibility.Collapsed;
                    dtgAllergy.ItemsSource = null;
                }

                if (patientVisit.IsVIP)
                {
                    imgVIP.Visibility = System.Windows.Visibility.Visible;
                    txtVIPType.Text = patientVisit.VIPType;
                    txtVIPActiveFrom.Text = patientVisit.VIPActiveFrom?.ToString("dd/MM/yyyy");
                    txtVIPActiveTo.Text = patientVisit.VIPActiveTo?.ToString("dd/MM/yyyy");
                }
                else
                {
                    imgVIP.Visibility = System.Windows.Visibility.Collapsed;
                }

                if (patientVisit.PatientImage != null)
                {
                    BitmapImage image = new BitmapImage();
                    MemoryStream mem = new MemoryStream(patientVisit.PatientImage);
                    image.BeginInit();
                    image.StreamSource = mem;
                    image.EndInit();
                    patientImage.Source = image;
                    patientImageTootip.Source = image;
                }
                //else
                //{
                //    Uri uri = new Uri(@"pack://application:,,,/MediTech;component/Resources/Images/Other/Blue-Pictures-icon.png");
                //    BitmapImage image = new BitmapImage(uri);
                //    patientImage.Source = image;
                //    patientImageTootip.Source = image;
                //}
            }
            else
            {
                ClearData();
            }
        }
    }
}
