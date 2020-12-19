using DevExpress.XtraReports.UI;
using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Reports.Operating.Patient.RiskBook;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class CheckupListReportViewModel : MediTechViewModelBase
    {
        #region Propoties

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
                    Search();
                    SearchPatientCriteria = string.Empty;
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

        private PayorDetailModel _SelectPayorDetail;
        public PayorDetailModel SelectPayorDetail
        {
            get { return _SelectPayorDetail; }
            set
            {
                Set(ref _SelectPayorDetail, value);
                if (_SelectPayorDetail != null)
                {
                    CheckupJobContactList = DataService.Checkup.GetCheckupJobContactByPayorDetailUID(_SelectPayorDetail.PayorDetailUID);
                }
            }
        }



        private List<PatientVisitModel> _PatientCheckupResult;
        public List<PatientVisitModel> PatientCheckupResult
        {
            get { return _PatientCheckupResult; }
            set { Set(ref _PatientCheckupResult, value); }
        }

        private ObservableCollection<PatientVisitModel> _SelectPatientCheckupResult;
        public ObservableCollection<PatientVisitModel> SelectPatientCheckupResult
        {
            get { return _SelectPatientCheckupResult ?? (_SelectPatientCheckupResult = new ObservableCollection<PatientVisitModel>()); }
            set { Set(ref _SelectPatientCheckupResult, value); }
        }
   

        private List<PayorDetailModel> _PayorDetails;
        public List<PayorDetailModel> PayorDetails
        {
            get { return _PayorDetails; }
            set { Set(ref _PayorDetails, value); }
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

        private ObservableCollection<PatientResultComponentModel> _SelectPatientResultLabList;
        public ObservableCollection<PatientResultComponentModel> SelectPatientResultLabList
        {
            get { return _SelectPatientResultLabList ?? (_SelectPatientResultLabList = new ObservableCollection<PatientResultComponentModel>()); }
            set { Set(ref _SelectPatientResultLabList, value); }
        }

        private LookupItemModel _SelectPrinter;
        public LookupItemModel SelectPrinter
        {
            get { return _SelectPrinter; }
            set { Set(ref _SelectPrinter, value); }
        }

        private List<LookupItemModel> _PrinterLists;
        public List<LookupItemModel> PrinterLists
        {
            get { return _PrinterLists; }
            set { Set(ref _PrinterLists, value); }
        }
        #endregion

        #region command
        private RelayCommand _SearchCommand;
        public RelayCommand SearchCommand
        {
            get
            {
                return _SearchCommand
                    ?? (_SearchCommand = new RelayCommand(Search));
            }
        }

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

        private RelayCommand _RiskBookCommand;
        public RelayCommand RiskBookCommand
        {
            get { return _RiskBookCommand ?? (_RiskBookCommand = new RelayCommand(RiskBook)); }
        }

        private RelayCommand _PrintAutoCommand;
        public RelayCommand PrintAutoCommand
        {
            get { return _PrintAutoCommand ?? (_PrintAutoCommand = new RelayCommand(PrintAuto)); }
        }

        private RelayCommand _PrintRiskAutoCommand;
        public RelayCommand PrintRiskAutoCommand
        {
            get { return _PrintRiskAutoCommand ?? (_PrintRiskAutoCommand = new RelayCommand(PrintRiskbookAuto)); }
        }

        private RelayCommand _PreviewBookCheckupCommand;
        public RelayCommand PreviewBookCheckupCommand
        {
            get { return _PreviewBookCheckupCommand ?? (_PreviewBookCheckupCommand = new RelayCommand(PreviewBookCheckup)); }
        }

        #endregion


        #region Method
        public CheckupListReportViewModel()
        {
            DateFrom = DateTime.Now;
            DateTo = DateTime.Now;
            PayorDetails = DataService.MasterData.GetPayorDetail();
            PrinterLists = new List<LookupItemModel>();
            int i = 1;
            foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                LookupItemModel lookupData = new LookupItemModel();
                lookupData.Key = i;
                lookupData.Display = printer;
                PrinterLists.Add(lookupData);
                i++;
            }
            SelectPrinter = PrinterLists.FirstOrDefault(p => p.Display.ToLower().Contains("Sticker"));
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
                List<PatientInformationModel> searchResult = DataService.PatientIdentity.SearchPatient(patientID, firstName, "", lastName, "", null, null, "", null);
                PatientsSearchSource = searchResult;
            }
            else
            {
                PatientsSearchSource = null;
            }

        }
        void Search()
        {
            long? patientUID = null;
            int? payorDetailUID = SelectPayorDetail != null ? SelectPayorDetail.PayorDetailUID : (int?)null;
            if (!string.IsNullOrEmpty(SearchPatientCriteria))
            {
                if (SelectedPateintSearch != null)
                {
                    patientUID = SelectedPateintSearch.PatientUID;
                }
            }

            int? chekcupJobContactUID = SelectCheckupJobContact != null ? SelectCheckupJobContact.CheckupJobContactUID : (int?)null;
            PatientCheckupResult = DataService.Checkup.SearchPatientCheckup(DateFrom, DateTo, patientUID, payorDetailUID, chekcupJobContactUID);

        }

        void RiskBook()
        {
            if (SelectPatientCheckupResult != null)
            {
                //var patientResultLabList = SelectPatientCheckupResult.OrderBy(p => p.No);
                //foreach (var item in patientResultLabList.ToList())
                //{
                //    RiskBook1 rpt = new RiskBook1();
                //    rpt.Parameters["PatientUID"].Value = item.PatientUID;
                //    rpt.Parameters["PatientVisitUID"].Value = item.PatientVisitUID;
                //    rpt.Parameters["PayorDetailUID"].Value = item.PayorDetailUID;
                //    ReportPrintTool printTool = new ReportPrintTool(rpt);
                //    //rpt.PrintingSystem.StartPrint += PrintingSystem_StartPrint;
                //    rpt.RequestParameters = false;
                //    rpt.ShowPrintMarginsWarning = false;
                //    printTool.ShowPreviewDialog();

                //    SelectPatientCheckupResult.Remove(item);
                //}

            }
        }

        void PrintRiskbookAuto()
        {
            if (SelectPrinter == null)
            {
                WarningDialog("กรุณาเลือก Printer");
                return;
            }
            try
            {
                if (SelectPatientCheckupResult != null)
                {
                    //var patientResult = SelectPatientCheckupResult.OrderBy(p => p.No);
                    //foreach (var item in patientResult.ToList())
                    //{
                    //    RiskBook1 rpt = new RiskBook1();
                    //    rpt.Parameters["PatientUID"].Value = item.PatientUID;
                    //    rpt.Parameters["PatientVisitUID"].Value = item.PatientVisitUID;
                    //    rpt.Parameters["PayorDetailUID"].Value = item.PayorDetailUID;
                    //    ReportPrintTool printTool = new ReportPrintTool(rpt);
                    //    rpt.RequestParameters = false;
                    //    rpt.ShowPrintMarginsWarning = false;
                    //    printTool.Print();

                    //    SelectPatientCheckupResult.Remove(item);
                    //}
                }
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }
        void PreviewBookCheckup()
        {
            if (SelectPatientCheckupResult != null)
            {
                //var patientResultLabList = SelectPatientCheckupResult.OrderBy(p => p.No);
                //foreach (var item in patientResultLabList.ToList())
                //{
                //    Reports.Operating.Patient.CheckupBookReport.CheckupPage1 rpt = new Reports.Operating.Patient.CheckupBookReport.CheckupPage1();
                //    rpt.Parameters["PatientUID"].Value = item.PatientUID;
                //    rpt.Parameters["PatientVisitUID"].Value = item.PatientVisitUID;
                //    rpt.Parameters["PayorDetailUID"].Value = item.PayorDetailUID;
                //    ReportPrintTool printTool = new ReportPrintTool(rpt);
                //    //rpt.PrintingSystem.StartPrint += PrintingSystem_StartPrint;
                //    rpt.RequestParameters = false;
                //    rpt.ShowPrintMarginsWarning = false;
                //    printTool.ShowPreviewDialog();

                //    SelectPatientCheckupResult.Remove(item);
                //}
            }

        }

        void PrintAuto()
        {
            if (SelectPrinter == null)
            {
                WarningDialog("กรุณาเลือก Printer");
                return;
            }
            try
            {
                if (SelectPatientCheckupResult != null)
                {
                    //var patientResultLabList = SelectPatientCheckupResult.OrderBy(p => p.No);
                    //foreach (var item in patientResultLabList.ToList())
                    //{
                    //    Reports.Operating.Patient.CheckupBookReport.CheckupPage1 rpt = new Reports.Operating.Patient.CheckupBookReport.CheckupPage1();
                    //    rpt.Parameters["PatientUID"].Value = item.PatientUID;
                    //    rpt.Parameters["PatientVisitUID"].Value = item.PatientVisitUID;
                    //    rpt.Parameters["PayorDetailUID"].Value = item.PayorDetailUID;
                    //    ReportPrintTool printTool = new ReportPrintTool(rpt);
                    //    rpt.RequestParameters = false;
                    //    rpt.ShowPrintMarginsWarning = false;
                    //    printTool.Print();

                    //    SelectPatientCheckupResult.Remove(item);
                    //}
                }
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }
            #endregion
     }
}
