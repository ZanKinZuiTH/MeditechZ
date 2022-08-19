using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class CreateDispenseReturnViewModel :MediTechViewModelBase
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
                if (SelectedPateintSearch != null)
                {
                    SearchPatientVisit();
                    //SearchPatientCriteria = string.Empty;
                }
            }
        }

        #endregion

        private List<PrescriptionItemModel> _PrescriptionItem;
        public List<PrescriptionItemModel> PrescriptionItem
        {
            get { return _PrescriptionItem; }
            set { Set(ref _PrescriptionItem, value); }
        }

        private PrescriptionItemModel _SelectPrescriptionItem;
        public PrescriptionItemModel SelectPrescriptionItem
        {
            get { return _SelectPrescriptionItem; }
            set { Set(ref _SelectPrescriptionItem, value); }
        }

        private List<PatientVisitModel> _PatientVisitLists;

        public List<PatientVisitModel> PatientVisitLists
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

            }
        }

        private DateTime _SelectDispenseDate;

        public DateTime SelectDispenseDate
        {
            get { return _SelectDispenseDate; }
            set { Set(ref _SelectDispenseDate, value); }
        }


        private List<StoreModel> _Stores;
        public List<StoreModel> Stores
        {
            get { return _Stores; }
            set { Set(ref _Stores, value); }
        }

        private StoreModel _SelectStore;
        public StoreModel SelectStore
        {
            get { return _SelectStore; }
            set { Set(ref _SelectStore, value); }
        }

        private string _PrescriptionNumber;

        public string PrescriptionNumber
        {
            get { return _PrescriptionNumber; }
            set { Set(ref _PrescriptionNumber, value); }
        }


        private string _ItemName;

        public string ItemName
        {
            get { return _ItemName; }
            set { Set(ref _ItemName, value); }
        }


        private string _Comments;

        public string Comments
        {
            get { return _Comments; }
            set { Set(ref _Comments, value); }
        }

        #endregion

        #region Command

        private RelayCommand _PatientSearchCommand;

        /// <summary>
        /// Gets the PatientSearchCommand.
        /// </summary>
        public RelayCommand PatientSearchCommand
        {
            get
            {
                return _PatientSearchCommand
                    ?? (_PatientSearchCommand = new RelayCommand(PatientSearch));
            }
        }


        private RelayCommand _SearchCommand;
        public RelayCommand SearchCommand
        {
            get { return _SearchCommand ?? (_SearchCommand = new RelayCommand(Search)); }
        }

        private RelayCommand _ClearCommand;
        public RelayCommand ClearCommand
        {
            get { return _ClearCommand ?? (_ClearCommand = new RelayCommand(Clear)); }
        }

        private RelayCommand _SaveCommand;
        public RelayCommand SaveCommand
        {
            get { return _SaveCommand ?? (_SaveCommand = new RelayCommand(Save)); }
        }

        private RelayCommand _CloseCommand;
        public RelayCommand CloseCommand
        {
            get { return _CloseCommand ?? (_CloseCommand = new RelayCommand(Close)); }
        }
        #endregion

        #region Method
        public CreateDispenseReturnViewModel()
        {


        }

        private void Search()
        {

        }

        private void Save()
        {

        }

        private void Close()
        {
            DispenseReturns dispense = new DispenseReturns();
            ChangeViewPermission(dispense);
        }


        private void Clear()
        {

        }

        void SearchPatientVisit()
        {
            long patientUID = 0;

            if (!string.IsNullOrEmpty(SearchPatientCriteria))
            {
                if (SelectedPateintSearch != null)
                {
                    patientUID = SelectedPateintSearch.PatientUID;
                }
            }

            PatientVisitLists = DataService.PatientIdentity.GetPatientVisitDispensed(patientUID);
            if (PatientVisitLists != null)
            {
                Stores = DataService.Inventory.GetStoreDispensedByVisitUID(PatientVisitLists.FirstOrDefault().PatientVisitUID);
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
                List<PatientInformationModel> searchResult = DataService.PatientIdentity.SearchPatient(patientID, firstName, "", lastName, "", null, null, "", null, "", "");
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
