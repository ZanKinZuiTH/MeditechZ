using DevExpress.XtraReports.UI;
using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Reports.Operating.Patient.CheckupBook;
using MediTech.Reports.Operating.Patient.CheckupBookReport;
using MediTech.Reports.Operating.Patient.RiskBook;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediTech.ViewModels
{
    public class CheckupReportViewModel : MediTechViewModelBase
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
                    Search();
                    SearchPatientCriteria = string.Empty;
                }
            }
        }

        #endregion

        private string _FileLocation;

        public string FileLocation
        {
            get { return _FileLocation; }
            set { Set(ref _FileLocation, value); }
        }

        private int _TotalRecord;

        public int TotalRecord
        {
            get { return _TotalRecord; }
            set { Set(ref _TotalRecord, value); }
        }


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

        private List<PayorDetailModel> _PayorDetails;

        public List<PayorDetailModel> PayorDetails
        {
            get { return _PayorDetails; }
            set { Set(ref _PayorDetails, value); }
        }

        private PayorDetailModel _SelectPayorDetail;

        public PayorDetailModel SelectPayorDetail
        {
            get { return _SelectPayorDetail; }
            set { Set(ref _SelectPayorDetail, value); }
        }


        private List<PatientResultLabModel> _PatientResultLabList;

        public List<PatientResultLabModel> PatientResultLabList
        {
            get { return _PatientResultLabList; }
            set { Set(ref _PatientResultLabList, value); }
        }

        private ObservableCollection<PatientResultLabModel> _SelectPatientResultLabList;

        public ObservableCollection<PatientResultLabModel> SelectPatientResultLabList
        {
            get { return _SelectPatientResultLabList ?? (_SelectPatientResultLabList = new ObservableCollection<PatientResultLabModel>()); }
            set { Set(ref _SelectPatientResultLabList, value); }
        }

        private List<PatientResultLabModel> _PivotPatientLabData;

        public List<PatientResultLabModel> PivotPatientLabData
        {
            get { return _PivotPatientLabData; }
            set { Set(ref _PivotPatientLabData, value); }
        }

        #endregion

        #region Command

        private RelayCommand _SearchCommand;

        public RelayCommand SearchCommand
        {
            get
            {
                return _SearchCommand
                    ?? (_SearchCommand = new RelayCommand(Search));
            }
        }


        private RelayCommand _PreviewBookCommand;

        public RelayCommand PreviewBookCommand
        {
            get { return _PreviewBookCommand ?? (_PreviewBookCommand = new RelayCommand(PreviewBook)); }
        }

        private RelayCommand _RiskBookCommand;
        public RelayCommand RiskBookCommand
        {
            get { return _RiskBookCommand ?? (_RiskBookCommand = new RelayCommand(RiskBook)); }
        }

        private RelayCommand _PrintRiskAutoCommand;
        public RelayCommand PrintRiskAutoCommand
        {
            get { return _PrintRiskAutoCommand ?? (_PrintRiskAutoCommand = new RelayCommand(PrintRiskbook)); }
        }

        private RelayCommand _PreviewBookCheckopCommand;
        public RelayCommand PreviewBookCheckopCommand
        {
            get { return _PreviewBookCheckopCommand ?? (_PreviewBookCheckopCommand = new RelayCommand(PreviewBookCheckup)); }
        }


        private RelayCommand _ExportPivotGridToExcelCommand;

        public RelayCommand ExportPivotGridToExcelCommand
        {
            get { return _ExportPivotGridToExcelCommand ?? (_ExportPivotGridToExcelCommand = new RelayCommand(ExportPivotGridToExcel)); }
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
        #endregion

        #region Method

        public CheckupReportViewModel()
        {
            DateFrom = DateTime.Now;
            DateTo = DateTime.Now;
            PayorDetails = DataService.MasterData.GetPayorDetail();
        }


        void PreviewBook()
        {
            if (SelectPatientResultLabList != null)
            {
                var patientResultLabList = SelectPatientResultLabList.OrderBy(p => p.No);
                foreach (var item in patientResultLabList.ToList())
                {
                    CheckupBook1 rpt = new CheckupBook1();
                    rpt.Parameters["PatientUID"].Value = item.PatientUID;
                    rpt.Parameters["PatientVisitUID"].Value = item.PatientVisitUID;
                    rpt.Parameters["PayorDetailUID"].Value = item.PayorDetailUID;
                    ReportPrintTool printTool = new ReportPrintTool(rpt);
                    //rpt.PrintingSystem.StartPrint += PrintingSystem_StartPrint;
                    rpt.RequestParameters = false;
                    rpt.ShowPrintMarginsWarning = false;
                    printTool.ShowPreviewDialog();

                    SelectPatientResultLabList.Remove(item);
                }

            }

        }

        void RiskBook()
        {
            if (SelectPatientResultLabList != null)
            {
                var patientResultLabList = SelectPatientResultLabList.OrderBy(p => p.No);
                foreach (var item in patientResultLabList.ToList())
                {
                    RiskBook1 rpt = new RiskBook1();
                    rpt.Parameters["PatientUID"].Value = item.PatientUID;
                    rpt.Parameters["PatientVisitUID"].Value = item.PatientVisitUID;
                    rpt.Parameters["PayorDetailUID"].Value = item.PayorDetailUID;
                    ReportPrintTool printTool = new ReportPrintTool(rpt);
                    //rpt.PrintingSystem.StartPrint += PrintingSystem_StartPrint;
                    rpt.RequestParameters = false;
                    rpt.ShowPrintMarginsWarning = false;
                    printTool.ShowPreviewDialog();

                    SelectPatientResultLabList.Remove(item);
                }

            }

        }

        void PrintRiskbook()
        {
            if (SelectPatientResultLabList != null)
            {
                var patientResultLabList = SelectPatientResultLabList.OrderBy(p => p.No);
                foreach (var item in patientResultLabList.ToList())
                {
                    RiskBook1 rpt = new RiskBook1();
                    rpt.Parameters["PatientUID"].Value = item.PatientUID;
                    rpt.Parameters["PatientVisitUID"].Value = item.PatientVisitUID;
                    rpt.Parameters["PayorDetailUID"].Value = item.PayorDetailUID;
                    ReportPrintTool printTool = new ReportPrintTool(rpt);
                    rpt.RequestParameters = false;
                    rpt.ShowPrintMarginsWarning = false;
                    printTool.Print();
                }
            }
        }

        void PreviewBookCheckup()
        {
            if (SelectPatientResultLabList != null)
            {
                var patientResultLabList = SelectPatientResultLabList.OrderBy(p => p.No);
                foreach (var item in patientResultLabList.ToList())
                {
                    CheckupPage1 rpt = new CheckupPage1();
                    rpt.Parameters["PatientUID"].Value = item.PatientUID;
                    rpt.Parameters["PatientVisitUID"].Value = item.PatientVisitUID;
                    rpt.Parameters["PayorDetailUID"].Value = item.PayorDetailUID;
                    ReportPrintTool printTool = new ReportPrintTool(rpt);
                    //rpt.PrintingSystem.StartPrint += PrintingSystem_StartPrint;
                    rpt.RequestParameters = false;
                    rpt.ShowPrintMarginsWarning = false;
                    printTool.ShowPreviewDialog();

                    SelectPatientResultLabList.Remove(item);
                }
            }
        }

        private void PrintingSystem_StartPrint(object sender, DevExpress.XtraPrinting.PrintDocumentEventArgs e)
        {
            if (!e.PrintDocument.PrinterSettings.CanDuplex)
            {
                throw new Exception("Cannot print in duplex mode");
            }
            e.PrintDocument.PrinterSettings.Duplex = System.Drawing.Printing.Duplex.Vertical;
        }


        void ExportPivotGridToExcel()
        {
            if (PivotPatientLabData != null)
            {
                string fileName = ShowSaveFileDialog("Microsoft Excel Document", "Microsoft Excel|*.xlsx");
                if (fileName != "")
                {
                    CheckupReport view = (CheckupReport)this.View;
                    view.pivotData.ExportToXlsx(fileName);
                    OpenFile(fileName);
                }

            }
            (this.View as CheckupReport).pivotData.ExportToXlsx(@"C:\Users\asus\Desktop\text.xlsx");
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

            var dataCheckupValue = DataService.Lab.SearchResultLabList(DateFrom, DateTo, patientUID, payorDetailUID);

            if (dataCheckupValue != null)
            {
                PatientResultLabList = dataCheckupValue
                    .GroupBy(g => new {  g.PatientUID, g.PatientID, g.FirstName, g.LastName, g.StartDttm, g.TITLEUID, g.Title, g.SEXXXUID, g.Gender, g.Age })
                    .Select(p => new PatientResultLabModel
                    {
                        PatientVisitUID = p.FirstOrDefault().PatientVisitUID,
                        PatientUID = p.FirstOrDefault().PatientUID,
                        PatientID = p.FirstOrDefault().PatientID,
                        FirstName = p.FirstOrDefault().FirstName,
                        LastName = p.FirstOrDefault().LastName,
                        PayorDetailUID = p.FirstOrDefault().PayorDetailUID,
                        StartDttm = p.FirstOrDefault().StartDttm,
                        TITLEUID = p.FirstOrDefault().TITLEUID,
                        Title = p.FirstOrDefault().Title,
                        SEXXXUID = p.FirstOrDefault().SEXXXUID,
                        Gender = p.FirstOrDefault().Gender,
                        Age = p.FirstOrDefault().Age,
                        EmployeeID = p.FirstOrDefault().EmployeeID,
                        CompanyName = p.FirstOrDefault().CompanyName,
                        Department = p.FirstOrDefault().Department,
                        Position = p.FirstOrDefault().Position
                    }).ToList();

                PivotPatientLabData = dataCheckupValue;

                if (PatientResultLabList != null && PatientResultLabList.Count > 0)
                {
                    int i = 1;
                    PatientResultLabList.ForEach(p => p.No = i++);
                }

                //(this.View as CheckupReport).pivotData.BestFit();
            }
            else
            {
                PatientResultLabList = null;
                PivotPatientLabData = null;
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
                List<PatientInformationModel> searchResult = DataService.PatientIdentity.SearchPatient(patientID, firstName, "", lastName, "", null, null, "", null);
                PatientsSearchSource = searchResult;
            }
            else
            {
                PatientsSearchSource = null;
            }

        }

        private string ShowSaveFileDialog(string title, string filter)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            string name = System.Windows.Forms.Application.ProductName;
            int n = name.LastIndexOf(".") + 1;
            if (n > 0) name = name.Substring(n, name.Length - n);
            dlg.Title = "Export To " + title;
            dlg.FileName = name;
            dlg.Filter = filter;
            if (dlg.ShowDialog() == DialogResult.OK) return dlg.FileName;
            return "";
        }

        private void OpenFile(string fileName)
        {
            if (System.Windows.Forms.MessageBox.Show("Do you want to open this file?", "Export To...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    process.StartInfo.FileName = fileName;
                    process.StartInfo.Verb = "Open";
                    process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                    process.Start();
                }
                catch
                {
                    System.Windows.Forms.MessageBox.Show("Cannot find an application on your system suitable for openning the file with exported data.", System.Windows.Forms.Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        #endregion
    }
}
