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

        private ObservableCollection<PatientVisitModel> _PatientAllocateLists;
        public ObservableCollection<PatientVisitModel> PatientAllocateLists
        {
            get { return _PatientAllocateLists; }
            set { Set(ref _PatientAllocateLists, value); }
        }

        private PatientVisitModel _SelectPatientAllocate;

        public PatientVisitModel SelectPatientAllocate
        {
            get { return _SelectPatientAllocate; }
            set { Set(ref _SelectPatientAllocate, value); }
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

        private List<InsuranceCompanyModel> _InsuranceCompanyDetails;
        public List<InsuranceCompanyModel> InsuranceCompanyDetails
        {
            get { return _InsuranceCompanyDetails; }
            set { Set(ref _InsuranceCompanyDetails, value); }
        }

        private InsuranceCompanyModel _SelectInsuranceCompanyDetail;
        public InsuranceCompanyModel SelectInsuranceCompanyDetail
        {
            get { return _SelectInsuranceCompanyDetail; }
            set
            {
                Set(ref _SelectInsuranceCompanyDetail, value);
                if (_SelectInsuranceCompanyDetail != null)
                {
                    CheckupJobContactList = DataService.Checkup.GetCheckupJobContactByPayorDetailUID(_SelectInsuranceCompanyDetail.InsuranceCompanyUID);
                    SelectCheckupJobContact = CheckupJobContactList.OrderByDescending(p => p.StartDttm).FirstOrDefault();
                }
            }
        }

        private List<CheckupJobContactModel> _CheckupJobContactList;
        public List<CheckupJobContactModel> CheckupJobContactList
        {
            get { return _CheckupJobContactList; }
            set { Set(ref _CheckupJobContactList, value); }
        }

        private CheckupJobContactModel _SelectCheckupJobContact;
        public CheckupJobContactModel SelectCheckupJobContact
        {
            get { return _SelectCheckupJobContact; }
            set
            {
                Set(ref _SelectCheckupJobContact, value);
                if (_SelectCheckupJobContact != null)
                {
                    DateFrom = _SelectCheckupJobContact.StartDttm;
                    DateTo = _SelectCheckupJobContact.EndDttm;
                }
            }
        }

        private List<string> _PrinterLists;

        public List<string> PrinterLists
        {
            get { return _PrinterLists; }
            set { Set(ref _PrinterLists, value); }
        }

        private string _SelectPrinter;

        public string SelectPrinter
        {
            get { return _SelectPrinter; }
            set { Set(ref _SelectPrinter, value); }
        }
        #endregion

        #region Command

        private RelayCommand _SearchCommand;

        public RelayCommand SearchCommand
        {
            get { return _SearchCommand ?? (_SearchCommand = new RelayCommand(Search)); }
        }

        private RelayCommand _CleanCommand;

        public RelayCommand CleanCommand
        {
            get { return _CleanCommand ?? (_CleanCommand = new RelayCommand(Clean)); }
        }

        #endregion

        #region Method

        public ListQueryBillsMobileViewModel()
        {
            PrinterLists = new List<string>();
            foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                PrinterLists.Add(printer);
            }
        }

        void Search()
        {
            long? patientUID = null;
            int? insuranceCompanyUID = SelectInsuranceCompanyDetail != null ? SelectInsuranceCompanyDetail.InsuranceCompanyUID : (int?)null;
            int? checkupJobUID = SelectInsuranceCompanyDetail != null ? SelectInsuranceCompanyDetail.InsuranceCompanyUID : (int?)null;
            if (SelectedPateintSearch != null && SearchPatientCriteria != "")
            {
                patientUID = SelectedPateintSearch.PatientUID;
            }
            var patientVisitList = DataService.Billing.pSearchPatientCheckupForAllocateBill(patientUID, DateFrom, DateTo, insuranceCompanyUID, checkupJobUID, AppUtil.Current.OwnerOrganisationUID);
            PatientAllocateLists = new ObservableCollection<PatientVisitModel>(patientVisitList.ToList());
        }

        void Clean()
        {
            DateFrom = DateTime.Now;
            DateTo = null;
            SearchPatientCriteria = string.Empty;
            SelectInsuranceCompanyDetail = null;
            SelectCheckupJobContact = null;
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
