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
    public class ListPatMergeViewModel : MediTechViewModelBase
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

                }
            }
        }

        #endregion

        private DateTime? _DateFrom;

        public DateTime? DateFrom
        {
            get { return _DateFrom; }
            set { Set(ref _DateFrom, value); }
        }


        private DateTime? _DateTo;

        public DateTime? DateTo
        {
            get { return _DateTo; }
            set { Set(ref _DateTo, value); }
        }

        private bool _IsMerge;

        public bool IsMerge
        {
            get { return _IsMerge; }
            set { Set(ref _IsMerge, value); }
        }

        private bool _IsUnMerge;

        public bool IsUnMerge
        {
            get { return _IsUnMerge; }
            set { Set(ref _IsUnMerge, value); }
        }

        private List<PatientMergeModel> _ListPatientMerges;

        public List<PatientMergeModel> ListPatientMerges
        {
            get { return _ListPatientMerges; }
            set { Set(ref _ListPatientMerges, value); }
        }

        private PatientMergeModel _SelectPatientMerge;

        public PatientMergeModel SelectPatientMerge
        {
            get { return _SelectPatientMerge; }
            set { Set(ref _SelectPatientMerge, value); }
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

        /// <summary>
        /// Gets the SearchCommand.
        /// </summary>
        public RelayCommand SearchCommand
        {
            get
            {
                return _SearchCommand
                    ?? (_SearchCommand = new RelayCommand(SearchPatientMerge));
            }
        }

        private RelayCommand _ClearCommand;

        /// <summary>
        /// Gets the ClearCommand.
        /// </summary>
        public RelayCommand ClearCommand
        {
            get
            {
                return _ClearCommand
                    ?? (_ClearCommand = new RelayCommand(DefaultControl));
            }
        }

        private RelayCommand _EncounterMergeCommand;

        /// <summary>
        /// Gets the EncounterMergeCommand.
        /// </summary>
        public RelayCommand EncounterMergeCommand
        {
            get
            {
                return _EncounterMergeCommand
                    ?? (_EncounterMergeCommand = new RelayCommand(EncounterMerge));
            }
        }

        private RelayCommand _PatientMergeCommand;

        /// <summary>
        /// Gets the PatientMergeCommand.
        /// </summary>
        public RelayCommand PatientMergeCommand
        {
            get
            {
                return _PatientMergeCommand
                    ?? (_PatientMergeCommand = new RelayCommand(PatientMerge));
            }
        }


        private RelayCommand _UnMergeCommand;

        /// <summary>
        /// Gets the UnMergeCommand.
        /// </summary>
        public RelayCommand UnMergeCommand
        {
            get
            {
                return _UnMergeCommand
                    ?? (_UnMergeCommand = new RelayCommand(UnMerge));
            }
        }


        #endregion

        #region Method

        public ListPatMergeViewModel()
        {
            DateFrom = DateTime.Now;
            DateTo = null;
            SearchPatientMerge();
        }

        void EncounterMerge()
        {
            ManagePatientMerge view = new ManagePatientMerge();
            ChangeViewUsingViewModelPermission(view, new ManageEncounterMergeViewModel());
        }

        void PatientMerge()
        {
            ManagePatientMerge view = new ManagePatientMerge();
            ChangeViewUsingViewModelPermission(view, new ManagePatientMergeViewModel());
        }

        void UnMerge()
        {
            try
            {
                if (SelectPatientMerge != null && SelectPatientMerge.MergeType == "Patient Merge" && SelectPatientMerge.IsUnMerge != "UNMERGE")
                {
                    var diagResult = QuestionDialog("คุณต้องการยกเลิกการ Merge ของคนไข้คนนี้ ใช่หรือไม่ ?");
                    if (diagResult == MessageBoxResult.Yes)
                    {
                        DataService.PatientIdentity.UnMergePatient(SelectPatientMerge.PatientMergeUID, AppUtil.Current.UserID);
                        SaveSuccessDialog();
                        SearchPatientMerge();
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorDialog(ex.Message);
            }

        }
        void SearchPatientMerge()
        {
            long? patientUID = null;
            string unMerge = null;
            if (SearchPatientCriteria != "" && SelectedPateintSearch != null)
            {
                patientUID = SelectedPateintSearch.PatientUID;
            }
            if ((IsMerge == true && IsUnMerge == true) || (IsMerge == false && IsUnMerge == false))
            {
                unMerge = null;
            }
            else if (IsMerge)
            {
                unMerge = "N";
            }
            else if (IsUnMerge)
            {
                unMerge = "Y";
            }

            ListPatientMerges = DataService.PatientIdentity.SearchPatientMerge(DateFrom, DateTo, patientUID, unMerge);
        }
        void DefaultControl()
        {
            DateFrom = DateTime.Now;
            DateTo = null;
            SearchPatientCriteria = "";
            SelectedPateintSearch = null;
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
