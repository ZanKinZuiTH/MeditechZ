using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediTech.Model;
using MediTech.Views;

namespace MediTech.ViewModels
{
    public class ListQueryBillsOPViewModel : MediTechViewModelBase
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
            }
        }


        private List<PatientVisitModel> _PatientVisitList;
        public List<PatientVisitModel> PatientVisitList
        {
            get { return _PatientVisitList; }
            set { Set(ref _PatientVisitList, value); }
        }

        private PatientVisitModel _SelectPatientVisit;

        public PatientVisitModel SelectPatientVisit
        {
            get { return _SelectPatientVisit; }
            set { Set(ref _SelectPatientVisit, value); }
        }

        #endregion

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

        private RelayCommand _AllocateBillCommand;

        public RelayCommand AllocateBillCommand
        {
            get { return _AllocateBillCommand ?? (_AllocateBillCommand = new RelayCommand(AllocateBill)); }
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


        private RelayCommand _CleanCommand;

        public RelayCommand CleanCommand
        {
            get { return _CleanCommand ?? (_CleanCommand = new RelayCommand(Clean)); }
        }

        #endregion

        #region Method

        public ListQueryBillsOPViewModel()
        {
            DateFrom = DateTime.Now;
        }


        void Search()
        {
            long? patientUID = null;

            if (SelectedPateintSearch != null && SearchPatientCriteria != "")
            {
                patientUID = SelectedPateintSearch.PatientUID;
            }
            PatientVisitList = DataService.Billing.SearchUnbilledPatients(patientUID, DateFrom, DateTo, AppUtil.Current.OwnerOrganisationUID, "N");
        }

        void Clean()
        {
            DateFrom = DateTime.Now;
            DateTo = null;
            SearchPatientCriteria = string.Empty;
            PatientVisitList = null;
        }

        private void AllocateBill()
        {
            if (SelectPatientVisit != null)
            {
                BillSettlementOP pageview = new BillSettlementOP();
                (pageview.DataContext as BillSettlementOPViewModel).AssingPatientVisit(SelectPatientVisit);
                BillSettlementOPViewModel result = (BillSettlementOPViewModel)LaunchViewDialog(pageview, "BLSETTLEOP", false, true);
            }
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

        public void PatientSearch()
        {
            string patientID = string.Empty;
            string firstName = string.Empty; ;
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
                List<PatientInformationModel> searchResult = DataService.PatientIdentity.SearchPatient(patientID, firstName, "", lastName, "", null, null, "", null, null);
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
