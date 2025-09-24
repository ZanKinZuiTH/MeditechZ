using DevExpress.Xpf.Core;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MediTech.Views
{
    /// <summary>
    /// Interaction logic for EMRView.xaml
    /// </summary>
    public partial class EMRView : UserControl
    {
        #region PatientBannerVisibilitySource Dependency Property

        public Visibility PatientBannerVisibility
        {
            get { return (Visibility)GetValue(PatientBannerVisibilityProperty); }
            set { SetValue(PatientBannerVisibilityProperty, value); }
        }

        public static readonly DependencyProperty PatientBannerVisibilityProperty = DependencyProperty.Register("PatientBannerVisibility", typeof(Visibility)
    , typeof(EMRView), new UIPropertyMetadata(Visibility.Collapsed, OnPatientBannerVisibilityChanged));

        private static void OnPatientBannerVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EMRView actb = d as EMRView;
            if (actb == null) return;
            actb.OnItemsSourceChanged(e.NewValue);

        }

        protected void OnItemsSourceChanged(object source)
        {
            this.PatientBanner.Visibility = (Visibility)source;
            this.layoutVisit.Visibility = (Visibility)source;
        }

        #endregion

        #region PatientVisitSource Dependency Property

        public PatientVisitModel PatientVisit
        {
            get { return (PatientVisitModel)GetValue(PatientVisitProperty); }
            set { SetValue(PatientVisitProperty, value); }
        }

        public static readonly DependencyProperty PatientVisitProperty = DependencyProperty.Register("PatientVisit", typeof(PatientVisitModel)
    , typeof(EMRView), new UIPropertyMetadata(null, OnPatientVisitChanged));

        private static void OnPatientVisitChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EMRView actb = d as EMRView;
            if (actb == null) return;
            actb.OnItemsSourceChanged(e.NewValue as PatientVisitModel);

        }

        protected void OnItemsSourceChanged(PatientVisitModel source)
        {
            if (this.DataContext is EMRViewViewModel)
            {
                (this.DataContext as EMRViewViewModel).SelectedPatientVisit = source;
            }
        }

        #endregion
        public EMRView()
        {
            InitializeComponent();
            documentFrame.Navigating += DocumentFrame_Navigating;
            if (this.DataContext is EMRViewViewModel)
            {
                (this.DataContext as EMRViewViewModel).PatientBannerVisibilityChanged += EMRView_PatientBannerVisibilityChanged;
                (this.DataContext as EMRViewViewModel).View = this;
            }
        }

        public void SetPatientVisit(PatientVisitModel source)
        {
            if (this.DataContext is EMRViewViewModel)
            {
                (this.DataContext as EMRViewViewModel).SelectedPatientVisit = source;
            }
        }

        private void DocumentFrame_Navigating(object sender, DevExpress.Xpf.WindowsUI.Navigation.NavigatingEventArgs e)
        {
            if (e.Source is UserControl)
            {
                var dataContext = (MediTechViewModelBase)(e.Source as UserControl).DataContext;
                dataContext.View = e.Source;
            }
        }

        private void EMRView_PatientBannerVisibilityChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Visibility bannerVisibilty = (Visibility)(sender);
            OnItemsSourceChanged(bannerVisibilty);
        }


        public void DefaultPage()
        {
            if ((this as EMRView).Parent is DXWindow)
            {
                layoutVisit.Visibility = Visibility.Visible;
            }
            (this.DataContext as EMRViewViewModel).DefaultPage();
        }
    }
}
