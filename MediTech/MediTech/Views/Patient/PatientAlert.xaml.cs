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
    /// Interaction logic for PatientAlert.xaml
    /// </summary>
    public partial class PatientAlert : UserControl
    {
        #region PatientBannerVisibilitySource Dependency Property

        public Visibility PatientBannerVisibility
        {
            get { return (Visibility)GetValue(PatientBannerVisibilityProperty); }
            set { SetValue(PatientBannerVisibilityProperty, value); }
        }

        public static readonly DependencyProperty PatientBannerVisibilityProperty = DependencyProperty.Register("PatientBannerVisibility", typeof(Visibility)
    , typeof(PatientAlert), new UIPropertyMetadata(Visibility.Collapsed, OnPatientBannerVisibilityChanged));

        private static void OnPatientBannerVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PatientAlert actb = d as PatientAlert;
            if (actb == null) return;
            actb.OnItemsBannerSourceChanged(e.NewValue);

        }

        protected void OnItemsBannerSourceChanged(object source)
        {
            this.PatientBanner.Visibility = (Visibility)source;
        }

        #endregion

        #region PatientSearchVisibilitySource Dependency Property

        public Visibility PatientSearchVisibility
        {
            get { return (Visibility)GetValue(PatientSearchVisibilityProperty); }
            set { SetValue(PatientBannerVisibilityProperty, value); }
        }

        public static readonly DependencyProperty PatientSearchVisibilityProperty = DependencyProperty.Register("PatientSearchVisibility", typeof(Visibility)
    , typeof(PatientAlert), new UIPropertyMetadata(Visibility.Collapsed, OnPatientSearchVisibilityChanged));

        private static void OnPatientSearchVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PatientAlert actb = d as PatientAlert;
            if (actb == null) return;
            actb.OnItemsSearchSourceChanged(e.NewValue);

        }

        protected void OnItemsSearchSourceChanged(object source)
        {
            this.PatientSearch.Visibility = (Visibility)source;
        }

        #endregion
        public PatientAlert()
        {
            InitializeComponent();
            (this.DataContext as PatientAlertViewModel).PatientSearchVisibilityChanged += PatientAlert_PatientSearchVisibilityChanged ;
            (this.DataContext as PatientAlertViewModel).PatientBannerVisibilityChanged += PatientAlert_PatientBannerVisibilityChanged;
            (this.DataContext as PatientAlertViewModel).UpdateEvent += PatientAlert_UpdateEvent;
        }

        private void PatientAlert_UpdateEvent(object sender, EventArgs e)
        {
            grdListAlert.RefreshData();
        }

        private void PatientAlert_PatientSearchVisibilityChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Visibility bannerVisibilty = (Visibility)(sender);
            OnItemsSearchSourceChanged(bannerVisibilty);
        }

        private void PatientAlert_PatientBannerVisibilityChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Visibility bannerVisibilty = (Visibility)(sender);
            OnItemsBannerSourceChanged(bannerVisibilty);
        }
    }
}
