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
using MediTech.ViewModels;
using MediTech.Model;
namespace MediTech.Views
{
    /// <summary>
    /// Interaction logic for SearchPatient.xaml
    /// </summary>
    public partial class SearchEmergencyPatient : UserControl
    {

        #region SelectPatientDependecy
        public static readonly DependencyProperty SelectedPatientDataProperty = DependencyProperty.RegisterAttached("SelectedPatientData", typeof(PatientInformationModel), typeof(SearchEmergencyPatient));


        public PatientInformationModel SelectedPatientData
        {
            get { return (PatientInformationModel)GetValue(SelectedPatientDataProperty); }
            set { SetValue(SelectedPatientDataProperty, value); }
        }



        void SelectedPatientChanged_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            PatientInformationModel selectedPateint = (sender as PatientInformationModel);
            SelectedPatientData = selectedPateint;
        }
        #endregion

        #region SelectBooking

        public static readonly DependencyProperty SelectBookingProperty = DependencyProperty.RegisterAttached("SelectBooking", typeof(BookingModel), typeof(SearchEmergencyPatient));

        public BookingModel SelectBooking
        {
            get { return (BookingModel)GetValue(SelectBookingProperty); }
            set { SetValue(SelectBookingProperty, value); }
        }

        void SearchPatient_SelectedBookingChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            BookingModel selectedBooking = (sender as BookingModel);
            SelectBooking = selectedBooking;
        }

        #endregion

        public SearchEmergencyPatient()
        {
            InitializeComponent();
            if (this.DataContext is SearchEmergencyPatientViewModel)
            {
                (this.DataContext as SearchEmergencyPatientViewModel).SelectedPatientChanged += SelectedPatientChanged_PropertyChanged;
                (this.DataContext as SearchEmergencyPatientViewModel).SelectedBookingChanged += SearchPatient_SelectedBookingChanged;
            }
            //btnSearch.Focus();
            txtFirstName.Focus();
        }

    }
}
