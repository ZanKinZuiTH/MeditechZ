using MediTech.Interface;
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
    /// Interaction logic for DoctorRoom.xaml
    /// </summary>
    public partial class SummeryView : UserControl
    {
        #region PatientVisitSource Dependency Property

        public PatientVisitModel PatientVisit
        {
            get { return (PatientVisitModel)GetValue(PatientVisitProperty); }
            set { SetValue(PatientVisitProperty, value); }
        }

        public static readonly DependencyProperty PatientVisitProperty = DependencyProperty.Register("PatientVisit", typeof(PatientVisitModel)
    , typeof(SummeryView), new UIPropertyMetadata(null, OnPatientVisitChanged));

        private static void OnPatientVisitChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SummeryView actb = d as SummeryView;
            if (actb == null) return;
            actb.OnItemsSourceChanged(e.NewValue as PatientVisitModel);

        }

        protected void OnItemsSourceChanged(PatientVisitModel source)
        {
            if (this.DataContext is SummeryViewViewModel)
            {
                (this.DataContext as SummeryViewViewModel).AssignPatientVisit(source);
            }
        }

        #endregion
        public SummeryView()
        {
            InitializeComponent();
        }

        public void SetPatientVisit(PatientVisitModel source)
        {
            if (this.DataContext is SummeryViewViewModel)
            {
                (this.DataContext as SummeryViewViewModel).AssignPatientVisit(source);
            }
        }

        public void RefershData()
        {
            if (this.DataContext is SummeryViewViewModel)
            {
                (this.DataContext as SummeryViewViewModel).LoadLabResult();
                (this.DataContext as SummeryViewViewModel).LoadRaiologyResult();
            }
        }

        //private void listLabResultGroupLabNumber_Loaded(object sender, RoutedEventArgs e)
        //{
        //    if (this.DataContext is SummeryViewViewModel)
        //    {
        //        (this.DataContext as SummeryViewViewModel).LoadResultGroupLabNumber();
        //    }
        //}

    }


}
