using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Models;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class ListQueryBillsMobileViewModel : MediTechViewModelBase
    {

        #region Preperites

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

        private ObservableCollection<UnbilledPatientsResult> _UnBilledPatientLists;
        public ObservableCollection<UnbilledPatientsResult> UnBilledPatientLists
        {
            get { return _UnBilledPatientLists; }
            set { Set(ref _UnBilledPatientLists, value); }
        }

        private UnbilledPatientsResult _SelectUnBilledPatient;

        public UnbilledPatientsResult SelectUnBilledPatient
        {
            get { return _SelectUnBilledPatient; }
            set { Set(ref _SelectUnBilledPatient, value); }
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

        private RelayCommand _SearchCommand;

        public RelayCommand SearchCommand
        {
            get { return _SearchCommand ?? (_SearchCommand = new RelayCommand(Search)); }
        }

        #endregion

        #region Method

        void Search()
        {
            long? patientUID = null;
            if (SelectedPateintSearch != null && SearchPatientCriteria != "")
            {
                patientUID = SelectedPateintSearch.PatientUID;
            }
            UnBilledPatientLists = new ObservableCollection<UnbilledPatientsResult>();
            var patientVisitList = DataService.Billing.SearchUnbilledPatients(patientUID, DateFrom, DateTo, AppUtil.Current.OwnerOrganisationUID, "N");
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
                List<PatientInformationModel> searchResult = DataService.PatientIdentity.SearchPatient(patientID, firstName, "", lastName, "", null, null, "", null, null, "");
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
