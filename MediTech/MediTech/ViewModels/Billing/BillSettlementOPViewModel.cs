using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MediTech.ViewModels
{
    public class BillSettlementOPViewModel : MediTechViewModelBase
    {
        #region Properties

        #region PatientSearch

        private string _SearchPatientCriteria;

        public string SearchPatientCriteria
        {
            get { return _SearchPatientCriteria; }
            set
            {
                Set(ref _SearchPatientCriteria, value);
                PatientsSearchSource = null;
            }
        }


        private List<PatientInformationModel> _PatientsSearchSource;

        public List<PatientInformationModel> PatientsSearchSource
        {
            get { return _PatientsSearchSource; }
            set { Set(ref _PatientsSearchSource, value); }
        }

        private PatientInformationModel _SelectedPateintSearch;

        public PatientInformationModel SelectedPateintSearch
        {
            get { return _SelectedPateintSearch; }
            set
            {
                _SelectedPateintSearch = value;
                if (_SelectedPateintSearch != null)
                {
                    PatientVisitModel visitInfoNonClose = DataService.PatientIdentity.GetLatestPatientVisitNonClose(_SelectedPateintSearch.PatientUID);
                    if (visitInfoNonClose != null)
                    {
                        SelectPatientVisit = visitInfoNonClose;
                    }
                }
            }
        }

        #endregion

        private Visibility _IsPatientSearchEnabled = Visibility.Visible;

        public Visibility IsPatientSearchEnabled
        {
            get { return _IsPatientSearchEnabled; }
            set { Set(ref _IsPatientSearchEnabled, value); }
        }

        private PatientVisitModel _SelectPatientVisit;

        public PatientVisitModel SelectPatientVisit
        {
            get { return _SelectPatientVisit; }
            set
            {
                Set(ref _SelectPatientVisit, value);
                if (_SelectPatientVisit != null)
                {
                    DateFrom = _SelectPatientVisit.StartDttm;
                }
            }

        }

        private List<PatientVisitPayorModel> _PatientVisitPayors;

        public List<PatientVisitPayorModel> PatientVisitPayors
        {
            get { return _PatientVisitPayors; }
            set { Set(ref _PatientVisitPayors, value); }
        }


        private PatientVisitPayorModel _SelectPatientVisitPayor;

        public PatientVisitPayorModel SelectPatientVisitPayor
        {
            get { return _SelectPatientVisitPayor; }
            set { Set(ref _SelectPatientVisitPayor, value); }
        }

        private DateTime? _DateFrom;
        public DateTime? DateFrom
        {
            get { return _DateFrom; }
            set
            {
                Set(ref _DateFrom, value);
            }
        }

        private DateTime? _DateTo;
        public DateTime? DateTo
        {
            get { return _DateTo; }
            set
            {
                Set(ref _DateTo, value);
            }
        }

        private bool _ExpandAll;

        public bool ExpandAll
        {
            set
            {
                Set(ref _ExpandAll, value);
            }
        }


        #endregion

        #region Command


        private RelayCommand _PatientSearchCommand;

        public RelayCommand PatientSearchCommand
        {
            get { return _PatientSearchCommand ?? (_PatientSearchCommand = new RelayCommand(PatientSearch)); }
        }

        private RelayCommand _SearchCommand;

        public RelayCommand SearchCommand
        {
            get { return _SearchCommand ?? (_SearchCommand = new RelayCommand(Search)); }
        }

        private RelayCommand _AutoAllocateCommand;

        public RelayCommand AutoAllocateCommand
        {
            get { return _AutoAllocateCommand ?? (_AutoAllocateCommand = new RelayCommand(AutoAllocate)); }
        }

        private RelayCommand _ClearCommand;

        public RelayCommand ClearCommand
        {
            get { return _ClearCommand ?? (_ClearCommand = new RelayCommand(Clear)); }
        }

        private RelayCommand _CreateOrderCommand;

        public RelayCommand CreateOrderCommand
        {
            get { return _CreateOrderCommand ?? (_CreateOrderCommand = new RelayCommand(CreateOrder)); }
        }

        private RelayCommand _ModifyVisitPayorCommand;
        public RelayCommand ModifyVisitPayorCommand
        {
            get { return _ModifyVisitPayorCommand ?? (_ModifyVisitPayorCommand = new RelayCommand(ModifyVisitPayor)); }
        }

        private RelayCommand _GeneratebillCommand;
        public RelayCommand GeneratebillCommand
        {
            get { return _GeneratebillCommand ?? (_GeneratebillCommand = new RelayCommand(Generatebill)); }
        }

        #endregion

        #region Method

        public BillSettlementOPViewModel()
        {
            DateFrom = DateTime.Now;
            DateTo = DateTime.Now;
        }

        public override void OnLoaded()
        {
            base.OnLoaded();
            if (SelectPatientVisit != null)
                GetVisitPayors(SelectPatientVisit.PatientVisitUID);
        }

        private void Search()
        {

        }

        private void AutoAllocate()
        {

        }

        private void Clear()
        {

        }

        private void GetVisitPayors(long patientVisitUID)
        {
            PatientVisitPayors = DataService.PatientIdentity.GetPatientVisitPayorByVisitUID(patientVisitUID);
        }

        public void AssingPatientVisit(PatientVisitModel patientVisit)
        {
            SelectPatientVisit = patientVisit;
            IsPatientSearchEnabled = Visibility.Collapsed;
            GetVisitPayors(SelectPatientVisit.PatientVisitUID);
        }

        private void CreateOrder()
        {
            if (SelectPatientVisit != null)
            {
                PatientOrderEntry pageview = new PatientOrderEntry();
                (pageview.DataContext as PatientOrderEntryViewModel).AssingPatientVisit(SelectPatientVisit);
                PatientOrderEntryViewModel result = (PatientOrderEntryViewModel)LaunchViewDialog(pageview, "ORDITM", false, true);
            }
        }


        private void ModifyVisitPayor()
        {
            if (SelectPatientVisit != null)
            {
                var patientVisit = DataService.PatientIdentity.GetPatientVisitByUID(SelectPatientVisit.PatientVisitUID);
                ModifyVisitPayor pageview = new ModifyVisitPayor();
                (pageview.DataContext as ModifyVisitPayorViewModel).AssingPatientVisit(SelectPatientVisit);
                ModifyVisitPayorViewModel result = (ModifyVisitPayorViewModel)LaunchViewDialog(pageview, "MODPAY", true);
                if (result != null && result.ResultDialog == ActionDialog.Save)
                {
                    SaveSuccessDialog();
                }
            }
        }

        private void Generatebill()
        {
            GenerateBill pageview = new GenerateBill();
            GenerateBillViewModel result = (GenerateBillViewModel)LaunchViewDialogNonPermiss(pageview, true);
            if (result != null && result.ResultDialog == ActionDialog.Save)
            {
                SaveSuccessDialog();
            }
        }
    

        public void PatientSearch()
        {
            string patientID = string.Empty;
            string firstName = string.Empty;
            string lastName = string.Empty;
            if (SearchPatientCriteria.Length >= 3)
            {
                string[] patientName = SearchPatientCriteria.Split(' ');
                if (patientName.Length >= 2)
                {
                    firstName = patientName[0];
                    lastName = patientName[1];
                }
                else
                {
                    int num = 0;
                    foreach (var ch in SearchPatientCriteria)
                    {
                        if (ShareLibrary.CheckValidate.IsNumber(ch.ToString()))
                        {
                            num++;
                        }
                    }
                    if (num >= 5)
                    {
                        patientID = SearchPatientCriteria;
                    }
                    else if (num <= 2)
                    {
                        firstName = SearchPatientCriteria;
                        lastName = "empty";
                    }

                }
                List<PatientInformationModel> searchResult = DataService.PatientIdentity.SearchPatient(patientID, firstName, "", lastName, "", null, null, "", null, "");
                PatientsSearchSource = searchResult;
            }
            else
            {
                PatientsSearchSource = null;
            }

        }

        #endregion
    }
}
