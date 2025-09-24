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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MediTech.Views
{
    /// <summary>
    /// Interaction logic for ListPatientAllergy.xaml
    /// </summary>
    public partial class PatientAllergy : UserControl
    {
        #region PatientBannerVisibilitySource Dependency Property

        public Visibility PatientBannerVisibility
        {
            get { return (Visibility)GetValue(PatientBannerVisibilityProperty); }
            set { SetValue(PatientBannerVisibilityProperty, value); }
        }

        public static readonly DependencyProperty PatientBannerVisibilityProperty = DependencyProperty.Register("PatientBannerVisibility", typeof(Visibility)
    , typeof(PatientAllergy), new UIPropertyMetadata(Visibility.Collapsed, OnPatientBannerVisibilityChanged));

        private static void OnPatientBannerVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PatientAllergy actb = d as PatientAllergy;
            if (actb == null) return;
            actb.OnItemsSourceChanged(e.NewValue);

        }

        protected void OnItemsSourceChanged(object source)
        {
            this.PatientBanner.Visibility = (Visibility)source;
        }

        #endregion
        public PatientAllergy()
        {
            InitializeComponent();
            if (this.DataContext is PatientAllergyViewModel)
            {
                (this.DataContext as PatientAllergyViewModel).UpdateEvent += PatientAllergyViewModel_UpdateEvent;
                (this.DataContext as PatientAllergyViewModel).PatientBannerVisibilityChanged += PatientAllergy_PatientBannerVisibilityChanged;
            }
        }

        private void PatientAllergy_PatientBannerVisibilityChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Visibility bannerVisibilty = (Visibility)(sender);
            OnItemsSourceChanged(bannerVisibilty);
        }

        void PatientAllergyViewModel_UpdateEvent(object sender, EventArgs e)
        {
            grdPatientAllergy.RefreshData();
        }
    }
}
