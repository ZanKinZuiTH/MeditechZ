using GalaSoft.MvvmLight.Command;
using MediTech.Interface;
using MediTech.Model;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MediTech.ViewModels
{
    public class EMRViewViewModel : MediTechViewModelBase, IPatientVisitViewModel
    {
        public event PropertyChangedEventHandler PatientBannerVisibilityChanged;
        protected void OnPatientBannerVisibilityChanged()
        {
            PropertyChangedEventHandler handler = PatientBannerVisibilityChanged;
            if (handler != null)
            {
                handler(PatientBannerVisibility, new PropertyChangedEventArgs("PatientBannerVisibility"));
            }
        }

        #region Properties

        private ObservableCollection<PatientVisitModel> _PatientVisitLists;

        public ObservableCollection<PatientVisitModel> PatientVisitLists
        {
            get { return _PatientVisitLists; }
            set { Set(ref _PatientVisitLists, value); }
        }

        private PatientVisitModel _SelectedPatientVisit;

        public PatientVisitModel SelectedPatientVisit
        {
            get { return _SelectedPatientVisit; }
            set
            {
                Set(ref _SelectedPatientVisit, value);
                (this.View as EMRView).summeryView.SetPatientVisit(_SelectedPatientVisit);
            }
        }

        private Visibility _PatientBannerVisibility;

        public Visibility PatientBannerVisibility
        {
            get { return _PatientBannerVisibility; }
            set { Set(ref _PatientBannerVisibility, value); OnPatientBannerVisibilityChanged(); }
        }


        private ObservableCollection<PageViewModuleModel> _PageViewModule;

        public ObservableCollection<PageViewModuleModel> PageViewModule
        {
            get { return _PageViewModule; }
            set { Set(ref _PageViewModule, value); }
        }

        private PageViewModel _SelectedPage;

        public PageViewModel SelectedPage
        {
            get { return _SelectedPage; }
            set
            {
                Set(ref _SelectedPage, value);
                if (_SelectedPage != null)
                {
                    ChangeView(_SelectedPage);
                }
            }
        }

        #endregion

        #region Command


        private RelayCommand _OpenVitalSignsChartCommand;

        public RelayCommand OpenVitalSignsChartCommand
        {
            get { return _OpenVitalSignsChartCommand ?? (_OpenVitalSignsChartCommand = new RelayCommand(OpenVitalSignsChart)); }
        }

        private RelayCommand _OpenScannedDocumentCommand;

        public RelayCommand OpenScannedDocumentCommand
        {
            get { return _OpenScannedDocumentCommand ?? (_OpenScannedDocumentCommand = new RelayCommand(OpenScannedDocument)); }
        }
        #endregion

        #region Method



        public EMRViewViewModel()
        {
            PageViewModule = new ObservableCollection<PageViewModuleModel>((new List<PageViewModuleModel>() {
                 new PageViewModuleModel() { ModuleName = "Demographic"
                , PageViews = new ObservableCollection<PageViewModel>() {
                    new PageViewModel() { Name = "EMR View",NamespaceName= "MediTech.Views", ClassName= "SummeryView" } } }
                ,new PageViewModuleModel() { ModuleName = "Diagnosis"
                , PageViews = new ObservableCollection<PageViewModel>() {
                    new PageViewModel() { Name = "Diagnosis",NamespaceName= "MediTech.Views", ClassName= "PatientDiagnosis" } } }
            , new PageViewModuleModel() { ModuleName = "Referrals"
                ,PageViews = new ObservableCollection<PageViewModel>(){
                    new PageViewModel() {Name = "Admission Request", NamespaceName = "MediTech.Views", ClassName = "AdmissionRequest"  }
                    ,new PageViewModel() { Name = "Consult", NamespaceName = "MediTech.Views", ClassName = "SendConsult" }
                    ,new PageViewModel() {Name = "IPD Consult", NamespaceName = "MediTech.Views", ClassName = "IPDConsult"  } } }
            }));



        }

        public void AssignPatientVisit(PatientVisitModel patVisitData)
        {
            var dataVisitList = DataService.PatientIdentity.GetPatientVisitByPatientUID(patVisitData.PatientUID);
            foreach (var item in dataVisitList)
            {
                item.Comments = item.StartDttm.Value.ToString("dd MMM yyyy HH:mm") + " / " + item.VisitID + " / " + item.OwnerOrganisation;
            }
            PatientVisitLists = new ObservableCollection<PatientVisitModel>(dataVisitList.OrderByDescending(p => p.StartDttm).ToList());
            PatientVisitLists.Add(new PatientVisitModel { PatientUID = patVisitData.PatientUID, PatientVisitUID = 0, Comments = "All" });
            PatientBannerVisibility = Visibility.Visible;
            SelectedPatientVisit = PatientVisitLists.FirstOrDefault(p => p.PatientVisitUID == patVisitData.PatientVisitUID);

        }


        public void DefaultPage()
        {
            (this.View as EMRView).documentFrame.Navigate((this.View as EMRView).summeryView);
            SelectedPage = null;
        }
        public void ChangeView(PageViewModel pageView)
        {
            object viewSource;
            if (pageView.Name == "EMR View")
            {
                viewSource = (this.View as EMRView).summeryView;
            }
            else
            {
                viewSource = Activator.CreateInstance(Type.GetType(pageView.NamespaceName + "." + pageView.ClassName));
            }

            if (viewSource is UserControl)
            {
                ((UserControl)viewSource).Tag = this.View;
                var viewModel = ((UserControl)viewSource).DataContext;
                if (viewModel is IPatientVisitViewModel)
                {
                    if (SelectedPatientVisit == null || (SelectedPatientVisit.PatientVisitUID == 0 && pageView.Name != "EMR View"))
                    {
                        WarningDialog("กรุณาเลือก Visit");
                        return;
                    }
                    (viewModel as IPatientVisitViewModel).AssignPatientVisit(SelectedPatientVisit);
                }
            }
            (this.View as EMRView).documentFrame.Navigate(viewSource);
        }

        void OpenVitalSignsChart()
        {
            VitalSignsChart pageView = new VitalSignsChart();
            (pageView.DataContext as VitalSignsChartViewModel).PatientUID = SelectedPatientVisit.PatientUID;
            LaunchViewDialogNonPermiss(pageView, false, false);
        }

        void OpenScannedDocument()
        {
            ScannedDocument pageview = new ScannedDocument();
            (pageview.DataContext as ScannedDocumentViewModel).AssingPatientVisit(SelectedPatientVisit);
            ScannedDocumentViewModel result = (ScannedDocumentViewModel)LaunchViewDialog(pageview, "PATSCD", false);
        }

        #endregion
    }
}
