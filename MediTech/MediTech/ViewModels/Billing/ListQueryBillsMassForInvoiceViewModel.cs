using DevExpress.XtraReports.UI;
using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Models;
using MediTech.Reports.Operating.Cashier;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class ListQueryBillsMassForInvoiceViewModel : MediTechViewModelBase
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

        private ObservableCollection<PatientVisitModel> _SelectPatientAllocates;

        public ObservableCollection<PatientVisitModel> SelectPatientAllocates
        {
            get { return _SelectPatientAllocates ?? (_SelectPatientAllocates = new ObservableCollection<PatientVisitModel>()); }
            set { Set(ref _SelectPatientAllocates, value); }
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

        private List<HealthOrganisationModel> _Organisations;

        public List<HealthOrganisationModel> Organisations
        {
            get { return _Organisations; }
            set { Set(ref _Organisations, value); }
        }

        private HealthOrganisationModel _SelectOrganisation;

        public HealthOrganisationModel SelectOrganisation
        {
            get { return _SelectOrganisation; }
            set
            {
                Set(ref _SelectOrganisation, value);
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
                    //DateTo = _SelectCheckupJobContact.EndDttm;
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

        private RelayCommand _CleanCommand;

        public RelayCommand CleanCommand
        {
            get { return _CleanCommand ?? (_CleanCommand = new RelayCommand(Clean)); }
        }


        private RelayCommand _AutoCollocateCommand;
        public RelayCommand AutoCollocateCommand
        {
            get { return _AutoCollocateCommand ?? (_AutoCollocateCommand = new RelayCommand(AutoCollocate)); }
        }

        private RelayCommand _PrintInvoiceCommand;
        public RelayCommand PrintInvoiceCommand
        {
            get { return _PrintInvoiceCommand ?? (_PrintInvoiceCommand = new RelayCommand(PrintInvoice)); }
        }

        #endregion

        #region Method

        public ListQueryBillsMassForInvoiceViewModel()
        {
            DateTime baseDate = DateTime.Today;
            DateFrom = baseDate.AddDays(1 - baseDate.Day);
            DateTo = baseDate;
            InsuranceCompanyDetails = DataService.Billing.GetInsuranceCompanyAll();
            Organisations = GetHealthOrganisationRoleMedical();
            SelectOrganisation = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == AppUtil.Current.OwnerOrganisationUID);

            PrinterLists = new List<string>();
            foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                PrinterLists.Add(printer);
            }
        }

        void AutoCollocate()
        {
            if (SelectPatientAllocates != null && SelectPatientAllocates.Count > 0)
            {
                var patienttUnBillFinalized = SelectPatientAllocates.Where(p => p.IsBillFinalized == "N").ToList();

                ListQueryBillsMassForInvoice view = (ListQueryBillsMassForInvoice)this.View;
                int upperlimit = patienttUnBillFinalized.Count();
                int loopCounter = 0;
                //foreach (var currentData in patienttUnBillFinalized)
                //{
                //    if (currentData.Select == true)
                //    {
                //        upperlimit++;
                //    }
                //}
                view.SetProgressBarLimits(0, upperlimit);

                foreach (var pateintAllocate in patienttUnBillFinalized.ToList())
                {

                    try
                    {
                        AllocatePatientBillableItemModel allocateModel = new AllocatePatientBillableItemModel();
                        allocateModel.PatientUID = pateintAllocate.PatientUID;
                        allocateModel.PatientVisitUID = pateintAllocate.PatientVisitUID;
                        allocateModel.IsAutoAllocate = "Y";
                        allocateModel.PatientVisitPayorUID = pateintAllocate.PatientVisitPayorUID;
                        allocateModel.PayorAgreementUID = pateintAllocate.PayorAgreementUID;
                        allocateModel.UserUID = AppUtil.Current.UserID;
                        allocateModel.StartDate = DateFrom ?? pateintAllocate.StartDttm.Value;
                        allocateModel.EndDate = DateTo ?? DateTime.Now;
                        PatientBillModel patientBill = DataService.Billing.AllocateAndGenerateInvoiceOnly(allocateModel);
                        if (patientBill != null)
                        {
                            pateintAllocate.PatientBillUID = patientBill.PatientBillUID;
                            pateintAllocate.OwnerOrganisationUID = patientBill.OwnerOrganisationUID;
                            pateintAllocate.IsBillFinalized = "Y";
                        }


                    }
                    catch (Exception ex)
                    {

                        WarningDialog(ex.Message);
                    }

                    SelectPatientAllocates.Remove(pateintAllocate);
                    loopCounter = ++loopCounter;
                    view.SetProgressBarValue(loopCounter);

                }

                view.SetProgressBarValue(upperlimit);
                view.grdPatientList.RefreshData();
                view.progressBar1.Value = 0;

                //Search();
            }
        }

        void PrintInvoice()
        {
            if (string.IsNullOrEmpty(SelectPrinter))
            {
                WarningDialog("กรุณาเลือกปริ้นเตอร์");
                return;
            }
            if (SelectPatientAllocates != null && SelectPatientAllocates.Count > 0)
            {
                var patienttUnBillFinalized = SelectPatientAllocates.Where(p => p.IsAllocated == "Y").ToList();

                ListQueryBillsMassForInvoice view = (ListQueryBillsMassForInvoice)this.View;
                int upperlimit = 0;
                int loopCounter = 0;
                foreach (var currentData in patienttUnBillFinalized)
                {
                    if (currentData.Select == true)
                    {
                        upperlimit++;
                    }
                }
                view.SetProgressBarLimits(0, upperlimit);

                foreach (var pateintAllocate in patienttUnBillFinalized)
                {
                    XtraReport report;
                    //var selectOrganisation = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == SelectPatientCloseMed.OwnerOrganisationUID);
                    if (pateintAllocate.VisitType != "Non Medical")
                    {
                        report = new PatientBill();
                    }
                    else
                    {
                        report = new PatientBill2();
                    }

                    report.RequestParameters = false;
                    report.Parameters["OrganisationUID"].Value = pateintAllocate.OwnerOrganisationUID;
                    report.Parameters["PatientBillUID"].Value = pateintAllocate.PatientBillUID;
                    ReportPrintTool printTool = new ReportPrintTool(report);
                    report.ShowPrintMarginsWarning = false;
                    printTool.ShowPreviewDialog();

                    SelectPatientAllocates.Remove(pateintAllocate);
                    loopCounter = ++loopCounter;
                    view.SetProgressBarValue(loopCounter);
                }
                view.SetProgressBarValue(upperlimit);
                view.grdPatientList.RefreshData();
                view.progressBar1.Value = 0;
            }
        }

        void Search()
        {
            long? patientUID = null;
            int? insuranceCompanyUID = SelectInsuranceCompanyDetail != null ? SelectInsuranceCompanyDetail.InsuranceCompanyUID : (int?)null;
            int? checkupJobUID = SelectCheckupJobContact != null ? SelectCheckupJobContact.CheckupJobContactUID : (int?)null;
            int? organisationUID = SelectOrganisation != null ? SelectOrganisation.HealthOrganisationUID : (int?)null;

            if (SelectedPateintSearch != null && SearchPatientCriteria != "")
            {
                patientUID = SelectedPateintSearch.PatientUID;
            }
            var userUID = AppUtil.Current.UserID;
            var patientVisitList = DataService.Billing.pSearchPatientInvoiceForAllocateBill(patientUID, DateFrom, DateTo, insuranceCompanyUID, checkupJobUID, organisationUID, userUID);
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
