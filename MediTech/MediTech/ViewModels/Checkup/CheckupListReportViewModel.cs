using DevExpress.XtraReports.UI;
using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Reports.Operating.Checkup.RiskBook;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Printing;
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

        private string _SelectPrinter;
        public string SelectPrinter
        {
            get { return _SelectPrinter; }
            set { Set(ref _SelectPrinter, value); }
        }

        private List<string> _PrinterLists;
        public List<string> PrinterLists
        {
            get { return _PrinterLists; }
            set { Set(ref _PrinterLists, value); }
        }

        private List<ReportsModel> _ReportsList;

        public List<ReportsModel> ReportsList
        {
            get { return _ReportsList; }
            set { Set(ref _ReportsList, value); }
        }

        private ReportsModel _SelectReport;

        public ReportsModel SelectReport
        {
            get { return _SelectReport; }
            set { Set(ref _SelectReport, value); }
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

        private RelayCommand _PreviewCommand;
        public RelayCommand PreviewCommand
        {
            get { return _PreviewCommand ?? (_PreviewCommand = new RelayCommand(Preview)); }
        }

        private RelayCommand _PrintAutoCommand;
        public RelayCommand PrintAutoCommand
        {
            get { return _PrintAutoCommand ?? (_PrintAutoCommand = new RelayCommand(PrintAuto)); }
        }

        #endregion

        #region Method
        public CheckupListReportViewModel()
        {
            DateFrom = DateTime.Now;
            DateTo = DateTime.Now;
            PayorDetails = DataService.MasterData.GetPayorDetail();
            PrinterLists = new List<string>();
            PrintDocument printDoc = new PrintDocument();
            foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                PrinterLists.Add(printer);
            }
            if (printDoc.PrinterSettings.IsDefaultPrinter)
            {
                SelectPrinter = printDoc.PrinterSettings.PrinterName;
            }

            ReportsList = new List<ReportsModel>();
            ReportsList.Add(new ReportsModel { Name = "สมุดตรวจสุขภาพรายบุคคล", NamespaceName = "MediTech.Reports.Operating.Checkup.CheckupBookReport.CheckupPage1" });
            ReportsList.Add(new ReportsModel { Name = "เล่มความเสี่ยง", NamespaceName = "MediTech.Reports.Operating.Checkup.RiskBook.RiskBook1" });
            ReportsList.Add(new ReportsModel { Name = "ผลตรวจสมรรถภาพการได้ยินเบื้องต้น", NamespaceName = "MediTech.Reports.Operating.Checkup.AudiogramGraph" });
            ReportsList.Add(new ReportsModel { Name = "ใบรับรองแพทย์สำหรับทำงานที่อับอากาศ", NamespaceName = "MediTech.Reports.Operating.Patient.ConfinedSpaceCertificate1" });

            SelectReport = ReportsList[0];
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

            if (PatientCheckupResult != null && PatientCheckupResult.Count > 0)
            {
                int i = 1;
                PatientCheckupResult.ForEach(p => p.RowHandle = i++);
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
                    var myReport = Activator.CreateInstance(Type.GetType(SelectReport.NamespaceName));
                    XtraReport rpt = (XtraReport)myReport;
                    var patientResultLabList = SelectPatientCheckupResult.OrderBy(p => p.RowHandle);
                    foreach (var item in patientResultLabList.ToList())
                    {
                        rpt.Parameters["PatientUID"].Value = item.PatientUID;
                        rpt.Parameters["PatientVisitUID"].Value = item.PatientVisitUID;

                        if (SelectReport.Name == "สมุดตรวจสุขภาพรายบุคคล" || SelectReport.Name == "เล่มความเสี่ยง")
                            rpt.Parameters["PayorDetailUID"].Value = item.PayorDetailUID;

                        ReportPrintTool printTool = new ReportPrintTool(rpt);
                        rpt.RequestParameters = false;
                        rpt.ShowPrintMarginsWarning = false;
                        printTool.Print(SelectPrinter);

                        SelectPatientCheckupResult.Remove(item);
                    }
                }
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }

        void Preview()
        {
            if (SelectPatientCheckupResult != null)
            {
                var myReport = Activator.CreateInstance(Type.GetType(SelectReport.NamespaceName));
                XtraReport rpt = (XtraReport)myReport;
                var patientResultLabList = SelectPatientCheckupResult.OrderBy(p => p.RowHandle);
                foreach (var item in patientResultLabList.ToList())
                {
                    rpt.Parameters["PatientUID"].Value = item.PatientUID;
                    rpt.Parameters["PatientVisitUID"].Value = item.PatientVisitUID;

                    if (SelectReport.Name == "สมุดตรวจสุขภาพรายบุคคล" || SelectReport.Name == "เล่มความเสี่ยง")
                        rpt.Parameters["PayorDetailUID"].Value = item.PayorDetailUID;

                    ReportPrintTool printTool = new ReportPrintTool(rpt);
                    rpt.RequestParameters = false;
                    rpt.ShowPrintMarginsWarning = false;
                    printTool.ShowPreviewDialog();

                    SelectPatientCheckupResult.Remove(item);
                }
            }
        }
        #endregion
    }
}
