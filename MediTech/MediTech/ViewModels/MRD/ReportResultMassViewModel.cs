using DevExpress.XtraReports.UI;
using Dicom;
using Dicom.Imaging;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MediTech.DataService;
using MediTech.Helpers;
using MediTech.Model;
using MediTech.Model.Report;
using MediTech.Reports.Operating.Patient;
using MediTech.Reports.Operating.Radiology;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace MediTech.ViewModels
{
    public class ReportResultMassViewModel : MediTechViewModelBase
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


        private List<LookupItemModel> _OrderItems;

        public List<LookupItemModel> OrderItems
        {
            get { return _OrderItems; }
            set { Set(ref _OrderItems, value); }
        }

        private LookupItemModel _SelectOrderItem;

        public LookupItemModel SelectOrderItem
        {
            get { return _SelectOrderItem; }
            set { Set(ref _SelectOrderItem, value); }
        }

        private bool _IsTranslate;

        public bool IsTranslate
        {
            get { return _IsTranslate; }
            set { Set(ref _IsTranslate, value); }
        }


        private List<LookupItemModel> _Reports;

        public List<LookupItemModel> Reports
        {
            get { return _Reports; }
            set { Set(ref _Reports, value); }
        }

        private LookupItemModel _SelectReport;

        public LookupItemModel SelectReport
        {
            get { return _SelectReport; }
            set
            {
                Set(ref _SelectReport, value);
                if (SelectReport != null)
                {
                    if (SelectReport.Key == 0 || SelectReport.Key == 1)
                    {
                        LogoVisibility = Visibility.Visible;
                    }
                    else
                    {
                        LogoVisibility = Visibility.Collapsed;
                    }
                }
            }
        }

        private List<LookupItemModel> _Logos;

        public List<LookupItemModel> Logos
        {
            get { return _Logos; }
            set { Set(ref _Logos, value); }
        }

        private LookupItemModel _SelectLogo;

        public LookupItemModel SelectLogo
        {
            get { return _SelectLogo; }
            set { Set(ref _SelectLogo, value); }
        }

        private ObservableCollection<PatientResultRadiology> _PatientXrays;

        public ObservableCollection<PatientResultRadiology> PatientXrays
        {
            get { return _PatientXrays; }
            set { Set(ref _PatientXrays, value); }
        }

        private ObservableCollection<PatientResultRadiology> _SelectPatientXrays;

        public ObservableCollection<PatientResultRadiology> SelectPatientXrays
        {
            get
            {
                return _SelectPatientXrays
                    ?? (_SelectPatientXrays = new ObservableCollection<PatientResultRadiology>());
            }

            set { Set(ref _SelectPatientXrays, value); }
        }


        private List<LookupReferenceValueModel> _ResultStatus;

        public List<LookupReferenceValueModel> ResultStatus
        {
            get { return _ResultStatus; }
            set { Set(ref _ResultStatus, value); }
        }

        private LookupItemModel _SelectResultStatus;

        public LookupItemModel SelectResultStatus
        {
            get { return _SelectResultStatus; }
            set { Set(ref _SelectResultStatus, value); }
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


        private int _StickerQuantity;

        public int StickerQuantity
        {
            get { return _StickerQuantity; }
            set { Set(ref _StickerQuantity, value); }
        }


        private Visibility _LogoVisibility = Visibility.Collapsed;

        public Visibility LogoVisibility
        {
            get { return _LogoVisibility; }
            set { Set(ref _LogoVisibility, value); }
        }


        private string _ExportFilePath;

        public string ExportFilePath
        {
            get { return _ExportFilePath; }
            set { Set(ref _ExportFilePath, value); }
        }

        private int _SucessRecord;

        public int SucessRecord
        {
            get { return _SucessRecord; }
            set { Set(ref _SucessRecord, value); }
        }

        private int _NotFoundRecord;

        public int NotFoundRecord
        {
            get { return _NotFoundRecord; }
            set { Set(ref _NotFoundRecord, value); }
        }


        private bool _IsSINE;

        public bool IsSINE
        {
            get { return _IsSINE; }
            set { Set(ref _IsSINE, value); }
        }

        private bool _IsNumberSequence;

        public bool IsNumberSequence
        {
            get { return _IsNumberSequence; }
            set { Set(ref _IsNumberSequence, value); }
        }

        #endregion

        #region Command

        private RelayCommand _ChooseCommand;

        public RelayCommand ChooseCommand
        {
            get
            {
                return _ChooseCommand
                    ?? (_ChooseCommand = new RelayCommand(ChooseFile));
            }
        }


        private RelayCommand _ImportCommand;

        public RelayCommand ImportCommand
        {
            get
            {
                return _ImportCommand
                    ?? (_ImportCommand = new RelayCommand(ImportFile));
            }
        }

        private RelayCommand _SearchCommand;

        public RelayCommand SearchCommand
        {
            get
            {
                return _SearchCommand
                    ?? (_SearchCommand = new RelayCommand(Search));
            }
        }

        private RelayCommand _ExportDataCommand;

        public RelayCommand ExportDataCommand
        {
            get
            {
                return _ExportDataCommand
                    ?? (_ExportDataCommand = new RelayCommand(ExportData));
            }
        }

        private RelayCommand _PrintCommand;

        public RelayCommand PrintCommand
        {
            get
            {
                return _PrintCommand
                    ?? (_PrintCommand = new RelayCommand(PrintResult));
            }
        }




        private RelayCommand _PreviewCommand;

        public RelayCommand PreviewCommand
        {
            get
            {
                return _PreviewCommand
                    ?? (_PreviewCommand = new RelayCommand(PreviewResult));
            }
        }


        private RelayCommand _PrintToPDFCommand;

        public RelayCommand PrintToPDFCommand
        {
            get
            {
                return _PrintToPDFCommand
                    ?? (_PrintToPDFCommand = new RelayCommand(PrintToPDF));
            }
        }

        private RelayCommand _PrintStickerCommand;
        public RelayCommand PrintStickerCommand
        {
            get
            {
                return _PrintStickerCommand
                    ?? (_PrintStickerCommand = new RelayCommand(PrintSticker));
            }
        }

        private RelayCommand _PrintStickerLargeCommand;
        public RelayCommand PrintStickerLargeCommand
        {
            get
            {
                return _PrintStickerLargeCommand
                    ?? (_PrintStickerLargeCommand = new RelayCommand(PrintStickerLarge));
            }
        }

        private RelayCommand _ChooseDicomPathCommand;
        public RelayCommand ChooseDicomPathCommand
        {
            get
            {
                return _ChooseDicomPathCommand
                    ?? (_ChooseDicomPathCommand = new RelayCommand(ChooseDicomPath));
            }
        }

        private RelayCommand _DownloadDicomCommand;
        public RelayCommand DownloadDicomCommand
        {
            get
            {
                return _DownloadDicomCommand
                    ?? (_DownloadDicomCommand = new RelayCommand(DownloadDicom));
            }
        }

        private RelayCommand _DownloadJPEGCommand;
        public RelayCommand DownloadJPEGCommand
        {
            get
            {
                return _DownloadJPEGCommand
                    ?? (_DownloadJPEGCommand = new RelayCommand(DownloadJPEG));
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


        #endregion

        #region Method

        public ReportResultMassViewModel()
        {

            IsTranslate = true;

            DateFrom = DateTime.Now;
            DateTo = DateTime.Now;
            StickerQuantity = 1;

            OrderItems = new List<LookupItemModel>();
            OrderItems.Add(new LookupItemModel { Key = 1, Display = "UltraSound Abdomen" });
            OrderItems.Add(new LookupItemModel { Key = 2, Display = "UltraSound Breast" });
            OrderItems.Add(new LookupItemModel { Key = 3, Display = "Chest PA" });
            //SelectOrderItem = OrderItems.FirstOrDefault();

            Reports = new List<LookupItemModel>();
            Reports.Add(new LookupItemModel { Key = 0, Display = "รายงานภาษาอังกฤษ" });
            Reports.Add(new LookupItemModel { Key = 1, Display = "รายงานสองภาษา" });
            Reports.Add(new LookupItemModel { Key = 2, Display = "รายงานโรงพยาบาลเฉลิมพระเกียรติ หน่วยเชิงรุก" });
            Reports.Add(new LookupItemModel { Key = 3, Display = "รายงานโรงพยาบาลเฉลิมพระเกียรติ หน่วยมลพิษ" });
            SelectReport = Reports.FirstOrDefault();

            Logos = new List<LookupItemModel>();
            Logos.Add(new LookupItemModel { Key = 0, Display = "BRXG" });
            Logos.Add(new LookupItemModel { Key = 1, Display = "โรงพยาบาลพระยุพราช" });
            SelectLogo = Logos.FirstOrDefault();

            ResultStatus = DataService.Technical.GetReferenceValueMany("RABSTS");
            //SelectResultStatus = ResultStatus.FirstOrDefault();

            PayorDetails = DataService.MasterData.GetPayorDetail();
        }


        private void Search()
        {
            string itemName = string.Empty;
            PatientXrays = null;
            SelectPatientXrays = null;
            int? resultStatusUID;
            int? payorDetailUID;
            long? patientUID = null;

            ReportResultMass view = (ReportResultMass)this.View;
            view.RemoveColumnMobile();

            if (SelectOrderItem != null)
            {
                if (SelectOrderItem.Display.Contains("Abdomen"))
                {
                    itemName = "Abdomen";
                }
                else if (SelectOrderItem.Display.Contains("Breast"))
                {
                    itemName = "Breast";
                }
                else if (SelectOrderItem.Display.Contains("Chest"))
                {
                    itemName = "Chest";
                }
            }

            if (!string.IsNullOrEmpty(SearchPatientCriteria))
            {
                if (SelectedPateintSearch != null)
                {
                    patientUID = SelectedPateintSearch.PatientUID;
                }
            }


            resultStatusUID = SelectResultStatus != null ? SelectResultStatus.Key : (int?)null;
            payorDetailUID = SelectPayorDetail != null ? SelectPayorDetail.PayorDetailUID : (int?)null;

            var dataList = DataService.Radiology.SearchResultRadiologyForTranslate(DateFrom, DateTo, patientUID, itemName, resultStatusUID, payorDetailUID);
            if (dataList != null)
                PatientXrays = new ObservableCollection<PatientResultRadiology>(dataList);
            else
                PatientXrays = null;

            List<XrayTranslateMappingModel> dtResultMapping = DataService.Radiology.GetXrayTranslateMapping();
            if (PatientXrays != null && PatientXrays.Count > 0)
            {
                if (IsTranslate)
                {
                    foreach (var item in PatientXrays)
                    {
                        List<string> listNoMapResult = new List<string>();
                        string thairesult = TranslateResult.TranslateResultXray(item.ResultValue, item.ResultStatus, item.RequestItemName, dtResultMapping, ref listNoMapResult);

                        item.ThaiResult = thairesult;
                    }
                    OnUpdateEvent();
                }

            }
            else
            {
                InformationDialog("ไม่พบข้อมูล");
            }
        }
        private void ImportFile()
        {
            OleDbConnection conn;
            OleDbCommand cmd;
            System.Data.DataTable dt;
            DataTable ImportData = new DataTable();
            DataSet objDataset1;
            string connectionString = string.Empty;
            int pgBarCounter = 0;
            ReportResultMass view = (ReportResultMass)this.View;
            try
            {
                if (!string.IsNullOrEmpty(FileLocation))
                {
                    PatientXrays = new ObservableCollection<PatientResultRadiology>();
                    SelectPatientXrays = null;
                    if (FileLocation.Trim().EndsWith(".xls"))
                    {
                        connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + FileLocation.Trim() +
                            "; Extended Properties=\"Excel 8.0; HDR=Yes; IMEX=1\"";
                    }
                    else
                    {
                        connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + FileLocation.Trim() +
                            "; Extended Properties=\"Excel 12.0 Xml; HDR=YES; IMEX=1\"";
                    }

                    using (conn = new OleDbConnection(connectionString))
                    {
                        conn.Open();
                        objDataset1 = new DataSet();
                        dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        if (dt.AsEnumerable().Where(p => p["Table_name"].ToString().ToUpper() == "INBOUND$").Count() <= 0)
                        {
                            WarningDialog("ไม่พบ Sheet ชื่อ ลงคอม กรุณาตรวจสอบ");
                            return;
                        }
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            for (int row = 0; row < dt.Rows.Count;)
                            {
                                string FileName = Convert.ToString(dt.Rows[row]["Table_Name"]);
                                cmd = conn.CreateCommand();
                                OleDbCommand objCmdSelect = new OleDbCommand("SELECT * FROM [INBOUND$] Where ([NO] <> '' OR [NO] IS NOT NULL)", conn);
                                OleDbDataAdapter objAdapter1 = new OleDbDataAdapter();
                                objAdapter1.SelectCommand = objCmdSelect;
                                objAdapter1.Fill(objDataset1);
                                //Break after reading the first sheet
                                break;
                            }
                            ImportData = objDataset1.Tables[0];
                            conn.Close();
                        }
                    }

                    view.AddColumnMobile();

                    int upperlimit = ImportData.Rows.Count;
                    view.SetProgressBarLimits(0, upperlimit);

                    string itemName = string.Empty;
                    if (SelectOrderItem != null)
                    {
                        if (SelectOrderItem.Display.Contains("Abdomen"))
                        {
                            itemName = "Abdomen";
                        }
                        else if (SelectOrderItem.Display.Contains("Breast"))
                        {
                            itemName = "Breast";
                        }
                        else if (SelectOrderItem.Display.Contains("Chest"))
                        {
                            itemName = "Chest";
                        }
                    }


                    int? resultStatusUID = SelectResultStatus != null ? SelectResultStatus.Key : (int?)null;
                    int? payorDetailUID = SelectPayorDetail != null ? SelectPayorDetail.PayorDetailUID : (int?)null;
                    List<XrayTranslateMappingModel> dtResultMapping = DataService.Radiology.GetXrayTranslateMapping();
                    foreach (DataRow item in ImportData.Rows)
                    {

                        PatientResultRadiology dtResult = DataService.Radiology.SearchPatientResultRadiologyForTranslate(DateFrom, DateTo, item["HN"].ToString().Trim(), itemName, resultStatusUID, payorDetailUID);

                        if (dtResult != null && dtResult.ResultEnteredDttm.ToString() != "")
                        {
                            if (IsTranslate)
                            {
                                List<string> listNoMapResult = new List<string>();
                                string thaiResult = TranslateResult.TranslateResultXray(dtResult.ResultValue, dtResult.ResultStatus, dtResult.RequestItemName, dtResultMapping, ref listNoMapResult);
                                dtResult.ThaiResult = thaiResult;
                            }

                            dtResult.OtherID = item["Other ID"].ToString();
                            dtResult.No = item["No"].ToString();
                            dtResult.EmployeeID = item["EmployeeID"].ToString();
                            dtResult.Department = item["Department"].ToString();
                            dtResult.Position = item["Position"].ToString();
                            dtResult.Company = item["Company"].ToString();
                            dtResult.Age = item["Age"].ToString();
                            if (item["CheckupDttm"] != null && !string.IsNullOrEmpty(item["CheckupDttm"].ToString()))
                            {
                                dtResult.CheckupDttm = DateTime.Parse(item["CheckupDttm"].ToString());
                            }
                            PatientXrays.Add(dtResult);
                        }
                        else
                        {
                            PatientInformationModel patient = DataService.PatientIdentity.GetPatientByHN(item["HN"].ToString());
                            PatientResultRadiology patRs = new PatientResultRadiology();
                            if (patient != null)
                            {
                                patRs.HN = patient.PatientID;
                                patRs.PatientName = patient.PatientName;
                                patRs.FirstName = patient.FirstName;
                                patRs.LastName = patient.LastName;
                                patRs.Title = patient.Title;
                                patRs.DOBDttm = patient.BirthDttm;
                            }
                            else
                            {
                                patRs.HN = item["HN"].ToString();
                                patRs.PatientName = item["PreName"].ToString() + " " + item["FirstName"].ToString() + " " + item["LastName"].ToString();
                                patRs.FirstName = item["FirstName"].ToString();
                                patRs.LastName = item["LastName"].ToString();
                                patRs.Title = item["PreName"].ToString();
                            }


                            if (patRs.DOBDttm != null)
                            {
                                string value = ShareLibrary.UtilDate.calAgeFromBirthDate(patRs.DOBDttm.Value);
                                patRs.Age = value;
                            }
                            else
                            {
                                patRs.Age = item["Age"].ToString();
                            }

                            patRs.OtherID = item["Other ID"].ToString();
                            patRs.Gender = item["Sex"].ToString();
                            patRs.No = item["No"].ToString();
                            patRs.ResultValue = "ไม่มีผล";
                            patRs.EmployeeID = item["EmployeeID"].ToString();
                            patRs.Department = item["Department"].ToString();
                            patRs.Position = item["Position"].ToString();
                            patRs.Company = item["Company"].ToString();

                            if (item["CheckupDttm"] != null && !string.IsNullOrEmpty(item["CheckupDttm"].ToString()))
                            {
                                patRs.CheckupDttm = DateTime.Parse(item["CheckupDttm"].ToString());
                            }


                            PatientXrays.Add(patRs);
                        }
                        pgBarCounter = pgBarCounter + 1;
                        TotalRecord = pgBarCounter;
                        view.SetProgressBarValue(pgBarCounter);
                    }
                    view.SetProgressBarValue(upperlimit);
                    OnUpdateEvent();
                }

            }
            catch (Exception er)
            {

                System.Windows.Forms.MessageBox.Show(er.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void ExportData()
        {
            if (PatientXrays != null)
            {
                string fileName = ShowSaveFileDialog("Microsoft Excel Document", "Microsoft Excel|*.xlsx");
                if (fileName != "")
                {
                    ReportResultMass view = (ReportResultMass)this.View;
                    view.gvPatientXray.ExportToXlsx(fileName);
                    OpenFile(fileName);
                }

            }
        }

        private void PrintResult()
        {
            int upperlimit = 0;
            int pgBarCounter = 0;
            try
            {
                if (SelectReport == null)
                {
                    WarningDialog("กรุณาเลือกประเภทรายงานผล");
                }
                if (PatientXrays != null)
                {
                    int countNum = PatientXrays.Count();
                    if (countNum > 0)
                    {
                        ReportResultMass view = (ReportResultMass)this.View;
                        foreach (var currentData in SelectPatientXrays)
                        {
                            upperlimit++;
                        }
                        view.SetProgressBarLimits(0, upperlimit);

                        foreach (var item in SelectPatientXrays.ToList())
                        {
                            if (SelectReport.Key == 0)
                            {
                                ImagingReport rpt = new ImagingReport();
                                rpt.LogoType = SelectLogo != null ? SelectLogo.Display : ""; ;
                                ReportPrintTool printTool = new ReportPrintTool(rpt);


                                rpt.Parameters["ResultUID"].Value = item.ResultUID;
                                rpt.RequestParameters = false;
                                rpt.ShowPrintMarginsWarning = false;
                                printTool.Print();
                            }
                            else if (SelectReport.Key == 1)
                            {
                                ImagingReportThai rpt = new ImagingReportThai();
                                rpt.LogoType = SelectLogo != null ? SelectLogo.Display : "";
                                ReportPrintTool printTool = new ReportPrintTool(rpt);
                                rpt.Parameters["ResultUID"].Value = item.ResultUID;
                                rpt.Parameters["ResultThai"].Value = item.ThaiResult;

                                rpt.RequestParameters = false;
                                rpt.ShowPrintMarginsWarning = false;
                                printTool.Print();
                            }
                            else if (SelectReport.Key == 2 || SelectReport.Key == 3)
                            {
                                ImagingReportV2 rpt = new ImagingReportV2();

                                if (SelectReport.Key == 2)
                                {
                                    rpt.lblTitle.Text = "รายงานผลการตรวจเอกซเรย์ปอด\r\nโครงการตรวจสุขภาพ และเฝ้าระวังสุขภาพเชิงรุก";
                                }
                                else if (SelectReport.Key == 3)
                                {
                                    rpt.lblTitle.Text = "รายงานผลการตรวจเอกซเรย์ปอด\r\nโครงการตรวจสุขภาพ และเฝ้าระวังสุขภาพเชิงมลพิษ";
                                }
                                rpt.lblAge.Text = item.Age;

                                ReportPrintTool printTool = new ReportPrintTool(rpt);
                                rpt.Parameters["ResultUID"].Value = item.ResultUID;
                                if (item.CheckupDttm != null)
                                {
                                    rpt.Parameters["CheckupDate"].Value = item.CheckupDttm;
                                }
                                rpt.Parameters["CheckupLocation"].Value = item.Company;
                                rpt.Parameters["NumberCode"].Value = item.OtherID;

                                rpt.RequestParameters = false;
                                rpt.ShowPrintMarginsWarning = false;
                                printTool.Print();
                            }

                            SelectPatientXrays.Remove(item);
                            pgBarCounter = pgBarCounter + 1;
                            view.SetProgressBarValue(pgBarCounter);

                            OnUpdateEvent();
                        }

                        view.SetProgressBarValue(upperlimit);

                    }
                }
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }

        }

        private void PreviewResult()
        {
            if (SelectReport == null)
            {
                WarningDialog("กรุณาเลือกประเภทรายงานผล");
            }
            if (PatientXrays != null)
            {
                int countNum = PatientXrays.Count();
                if (countNum > 0)
                {
                    foreach (var item in SelectPatientXrays.ToList())
                    {
                        if (SelectReport.Key == 0)
                        {
                            ImagingReport rpt = new ImagingReport();
                            rpt.LogoType = SelectLogo != null ? SelectLogo.Display : "";
                            ReportPrintTool printTool = new ReportPrintTool(rpt);


                            rpt.Parameters["ResultUID"].Value = item.ResultUID;
                            rpt.RequestParameters = false;
                            rpt.ShowPrintMarginsWarning = false;
                            printTool.ShowPreviewDialog();
                        }
                        else if (SelectReport.Key == 1)
                        {
                            ImagingReportThai rpt = new ImagingReportThai();
                            rpt.LogoType = SelectLogo != null ? SelectLogo.Display : "";
                            ReportPrintTool printTool = new ReportPrintTool(rpt);
                            rpt.Parameters["ResultUID"].Value = item.ResultUID;
                            rpt.Parameters["ResultThai"].Value = item.ThaiResult;

                            rpt.RequestParameters = false;
                            rpt.ShowPrintMarginsWarning = false;
                            printTool.ShowPreviewDialog();
                        }
                        else if (SelectReport.Key == 2 || SelectReport.Key == 3)
                        {
                            ImagingReportV2 rpt = new ImagingReportV2();

                            if (SelectReport.Key == 2)
                            {
                                rpt.lblTitle.Text = "รายงานผลการตรวจเอกซเรย์ปอด\r\nโครงการตรวจสุขภาพ และเฝ้าระวังสุขภาพเชิงรุก";
                            }
                            else if (SelectReport.Key == 3)
                            {
                                rpt.lblTitle.Text = "รายงานผลการตรวจเอกซเรย์ปอด\r\nโครงการตรวจสุขภาพ และเฝ้าระวังสุขภาพเชิงมลพิษ";
                            }
                            rpt.lblAge.Text = item.Age;

                            ReportPrintTool printTool = new ReportPrintTool(rpt);
                            rpt.Parameters["ResultUID"].Value = item.ResultUID;
                            if (item.CheckupDttm != null)
                            {
                                rpt.Parameters["CheckupDate"].Value = item.CheckupDttm;
                            }

                            rpt.Parameters["CheckupLocation"].Value = item.Company;

                            rpt.Parameters["NumberCode"].Value = item.OtherID;

                            rpt.RequestParameters = false;
                            rpt.ShowPrintMarginsWarning = false;
                            printTool.ShowPreviewDialog();
                        }

                        SelectPatientXrays.Remove(item);
                    }

                }
            }
        }

        private void PrintSticker()
        {
            int upperlimit = 0;
            int pgBarCounter = 0;
            try
            {

                if (PatientXrays != null && PatientXrays.Count > 0)
                {
                    ReportResultMass view = (ReportResultMass)this.View;
                    foreach (var currentData in SelectPatientXrays)
                    {
                        upperlimit++;
                    }
                    view.SetProgressBarLimits(0, upperlimit);
                    int No = 1;
                    foreach (var item in SelectPatientXrays.ToList())
                    {
                        PatientSticker2 rpt = new PatientSticker2();
                        ReportPrintTool printTool = new ReportPrintTool(rpt);

                        string gender;
                        switch (item.Gender)
                        {
                            case "หญิง (Female)":
                            case "F":
                                gender = "(F)";
                                break;
                            case "ชาย (Male)":
                            case "M":
                                gender = "(M)";
                                break;
                            default:
                                gender = "(N/A)";
                                break;
                        }

                        rpt.Parameters["PatientName"].Value = item.PatientName + " " + gender;

                        if (SelectLogo.Key == 0)
                        {
                            rpt.Parameters["HN"].Value = item.HN;
                        }
                        else if (SelectLogo.Key == 1)
                        {
                            rpt.Parameters["HN"].Value = item.OtherID;
                        }
                        else
                        {
                            rpt.Parameters["HN"].Value = item.HN;
                        }

                        if (view.colNoExcel.Visible)
                        {
                            rpt.Parameters["No"].Value = item.No;
                        }
                        else
                        {
                            rpt.Parameters["No"].Value = No.ToString();
                        }

                        rpt.Parameters["Age"].Value = item.Age;
                        rpt.Parameters["BirthDttm"].Value = item.DOBDttm != null ? item.DOBDttm.Value.ToString("dd/MM/yyyy") : "";
                        rpt.Parameters["Department"].Value = item.Department;
                        rpt.Parameters["EmployeeID"].Value = item.EmployeeID;
                        rpt.Parameters["CompanyName"].Value = item.Company;
                        rpt.RequestParameters = false;
                        rpt.ShowPrintMarginsWarning = false;
                        for (int i = 0; i < StickerQuantity; i++)
                        {
                            printTool.Print();
                        }

                        SelectPatientXrays.Remove(item);

                        pgBarCounter = pgBarCounter + 1;
                        view.SetProgressBarValue(pgBarCounter);

                        OnUpdateEvent();

                        No++;
                    }
                    view.SetProgressBarValue(upperlimit);
                }
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }

        }

        private void PrintStickerLarge()
        {
            int upperlimit = 0;
            int pgBarCounter = 0;
            try
            {

                if (PatientXrays != null && PatientXrays.Count > 0)
                {
                    ReportResultMass view = (ReportResultMass)this.View;
                    foreach (var currentData in SelectPatientXrays)
                    {
                        upperlimit++;
                    }
                    view.SetProgressBarLimits(0, upperlimit);
                    int No = 1;
                    foreach (var item in SelectPatientXrays.ToList())
                    {
                        PatientLargSticker rpt = new PatientLargSticker();
                        ReportPrintTool printTool = new ReportPrintTool(rpt);

                        string gender;
                        switch (item.Gender)
                        {
                            case "หญิง (Female)":
                            case "F":
                                gender = "(F)";
                                break;
                            case "ชาย (Male)":
                            case "M":
                                gender = "(M)";
                                break;
                            default:
                                gender = "(N/A)";
                                break;
                        }

                        rpt.Parameters["PatientName"].Value = item.PatientName + " " + gender;

                        if (SelectLogo.Key == 0)
                        {
                            rpt.Parameters["HN"].Value = item.HN;
                        }
                        else if (SelectLogo.Key == 1)
                        {
                            rpt.Parameters["HN"].Value = item.OtherID;
                        }
                        else
                        {
                            rpt.Parameters["HN"].Value = item.HN;
                        }



                        rpt.Parameters["Age"].Value = item.Age;
                        rpt.Parameters["BirthDate"].Value = item.DOBDttm != null ? item.DOBDttm.Value.ToString("dd/MM/yyyy") : "";
                        rpt.Parameters["OrderName"].Value = item.RequestItemName;
                        rpt.RequestParameters = false;
                        rpt.ShowPrintMarginsWarning = false;
                        for (int i = 0; i < StickerQuantity; i++)
                        {
                            printTool.Print();
                        }

                        SelectPatientXrays.Remove(item);

                        pgBarCounter = pgBarCounter + 1;
                        view.SetProgressBarValue(pgBarCounter);

                        OnUpdateEvent();

                        No++;
                    }
                    view.SetProgressBarValue(upperlimit);
                }
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }
        private void ChooseFile()
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Excel 2007 (*.xlsx)|*.xlsx|Excel 1997 - 2003 (*.xls)|*.xls"; ;
            openDialog.InitialDirectory = @"c:\";
            openDialog.ShowDialog();
            if (openDialog.FileName.Trim() != "")
            {
                try
                {
                    FileLocation = openDialog.FileName.Trim();
                }
                catch (Exception ex)
                {
                    ErrorDialog(ex.Message);
                }
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

        private void PrintToPDF()
        {
            int upperlimit = 0;
            int pgBarCounter = 0;
            try
            {
                if (SelectReport == null)
                {
                    WarningDialog("กรุณาเลือกประเภทรายงานผล");
                }
                if (PatientXrays != null)
                {
                    int countNum = PatientXrays.Count();
                    if (countNum > 0)
                    {
                        FolderBrowserDialog folderDlg = new FolderBrowserDialog();
                        DialogResult result = folderDlg.ShowDialog();
                        if (result == DialogResult.OK)
                        {
                            string path = folderDlg.SelectedPath;

                            ReportResultMass view = (ReportResultMass)this.View;
                            foreach (var currentData in SelectPatientXrays)
                            {
                                upperlimit++;
                            }
                            view.SetProgressBarLimits(0, upperlimit);

                            foreach (var item in SelectPatientXrays.ToList())
                            {
                                string fileName = (string.IsNullOrEmpty(item.OtherID) ? "" : item.OtherID + " ") + item.PatientName + ".pdf";
                                if (SelectReport.Key == 0)
                                {

                                    ImagingReport rpt = new ImagingReport();
                                    rpt.LogoType = SelectLogo != null ? SelectLogo.Display : "";


                                    rpt.Parameters["ResultUID"].Value = item.ResultUID;
                                    rpt.RequestParameters = false;
                                    rpt.ShowPrintMarginsWarning = false;
                                    rpt.ExportToPdf(path + "\\" + fileName);
                                }
                                else if (SelectReport.Key == 1)
                                {
                                    ImagingReportThai rpt = new ImagingReportThai();
                                    rpt.LogoType = SelectLogo != null ? SelectLogo.Display : "";
                                    rpt.Parameters["ResultUID"].Value = item.ResultUID;
                                    rpt.Parameters["ResultThai"].Value = item.ThaiResult;

                                    rpt.RequestParameters = false;
                                    rpt.ShowPrintMarginsWarning = false;

                                    rpt.ExportToPdf(path + "\\" + fileName);
                                }
                                else if (SelectReport.Key == 2 || SelectReport.Key == 3)
                                {
                                    ImagingReportV2 rpt = new ImagingReportV2();
                                    if (SelectReport.Key == 2)
                                    {
                                        rpt.lblTitle.Text = "รายงานผลการตรวจเอกซเรย์ปอด\r\nโครงการตรวจสุขภาพ และเฝ้าระวังสุขภาพเชิงรุก";
                                    }
                                    else if (SelectReport.Key == 3)
                                    {
                                        rpt.lblTitle.Text = "รายงานผลการตรวจเอกซเรย์ปอด\r\nโครงการตรวจสุขภาพ และเฝ้าระวังสุขภาพเชิงมลพิษ";
                                    }
                                    rpt.lblAge.Text = item.Age;

                                    ReportPrintTool printTool = new ReportPrintTool(rpt);
                                    rpt.Parameters["ResultUID"].Value = item.ResultUID;
                                    if (item.CheckupDttm != null)
                                    {
                                        rpt.Parameters["CheckupDate"].Value = item.CheckupDttm;
                                    }
                                    rpt.Parameters["CheckupLocation"].Value = item.Company;
                                    rpt.Parameters["NumberCode"].Value = item.OtherID;

                                    rpt.RequestParameters = false;
                                    rpt.ShowPrintMarginsWarning = false;
                                    rpt.ExportToPdf(path + "\\" + fileName);
                                }
                                SelectPatientXrays.Remove(item);

                                pgBarCounter = pgBarCounter + 1;
                                view.SetProgressBarValue(pgBarCounter);

                                OnUpdateEvent();
                            }
                            view.SetProgressBarValue(upperlimit);
                        }
                    }
                }
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }

        }

        private void ChooseDicomPath()
        {
            var dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    try
                    {
                        ExportFilePath = dialog.SelectedPath;
                    }
                    catch (Exception ex)
                    {
                        ErrorDialog(ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    ErrorDialog(ex.Message);
                }
            }
        }

        private void DownloadDicom()
        {
            try
            {
                if (string.IsNullOrEmpty(ExportFilePath))
                {
                    WarningDialog("กรุณาเลือก ตำแหน่ง Path");
                    return;
                }

                int upperlimit = 0;
                int pgBarCounter = 0;
                SucessRecord = 0;
                NotFoundRecord = 0;
                if (SelectPatientXrays != null && SelectPatientXrays.Count() > 0)
                {
                    ReportResultMass view = (ReportResultMass)this.View;
                    foreach (var currentData in SelectPatientXrays)
                    {
                        upperlimit++;
                    }
                    view.SetProgressBarLimits(0, upperlimit);
                    foreach (var patientInfo in SelectPatientXrays.ToList())
                    {
                        string modlity = patientInfo.Modality;
                        if (modlity == "DX")
                        {
                            modlity = "'DX','CR'";
                        }
                        else if (modlity == "MG")
                        {
                            modlity = "'MG','US'";
                        }
                        else
                        {
                            modlity = "'" + patientInfo.Modality + "'";
                        }

                        List<byte[]> dicomFiles = DataService.PACS.GetDicomFileByPatientID(patientInfo.HN, patientInfo.RequestedDttm, modlity, IsSINE);
                        if (dicomFiles != null && dicomFiles.Count() > 0)
                        {
                            foreach (var file in dicomFiles.ToList())
                            {
                                MemoryStream ms = new MemoryStream(file);
                                var dicomFile = Dicom.DicomFile.Open(ms);
                                string instanceUID = dicomFile.Dataset.GetSingleValueOrDefault<string>(DicomTag.SOPInstanceUID, "");

                                dicomFile.Dataset.NotValidated();
                                foreach (var item in dicomFile.Dataset.ToList())
                                {
                                    string value = "";
                                    dicomFile.Dataset.TryGetString(item.Tag, out value);
                                    if (!String.IsNullOrEmpty(value) && value.EndsWith("\0"))
                                    {
                                        value = value.Replace("\0", "");
                                        dicomFile.Dataset.AddOrUpdate(item.Tag, value);
                                    }
                                }

                                //dicomFile.Dataset.Validate();

                                dicomFile.Dataset.AddOrUpdate(DicomTag.SpecificCharacterSet, Encoding.UTF8, "ISO_IR 192");
                                dicomFile.Dataset.AddOrUpdate(DicomTag.PatientID, Encoding.UTF8, !string.IsNullOrEmpty(patientInfo.OtherID) ? patientInfo.OtherID : patientInfo.HN);
                                if (!IsNumberSequence)
                                {
                                    dicomFile.Dataset.AddOrUpdate(DicomTag.PatientName, Encoding.UTF8, patientInfo.PatientName);
                                }
                                else
                                {
                                    dicomFile.Dataset.AddOrUpdate(DicomTag.PatientName, Encoding.UTF8, patientInfo.No.ToString().PadLeft(4, '0') + " " + patientInfo.PatientName);
                                }
                                string fileName = (!string.IsNullOrEmpty(patientInfo.OtherID) ? patientInfo.OtherID : patientInfo.HN) + "_" + patientInfo.PatientName + "_" + instanceUID;
                                dicomFile.Save(ExportFilePath + "\\" + fileName + ".dcm");
                                dicomFiles.Remove(file);
                                MemoryManagement.FlushMemory();
                            }
                            ++SucessRecord;
                        }
                        else
                        {
                            ++NotFoundRecord;
                        }
                        SelectPatientXrays.Remove(patientInfo);
                        pgBarCounter = pgBarCounter + 1;
                        view.SetProgressBarValue(pgBarCounter);
                    }
                    //OnUpdateEvent();
                    view.SetProgressBarValue(upperlimit);
                }
            }
            catch (Exception ex)
            {

                ErrorDialog(ex.Message);
            }

        }

        private void DownloadJPEG()
        {
            try
            {
                if (string.IsNullOrEmpty(ExportFilePath))
                {
                    WarningDialog("กรุณาเลือก ตำแหน่ง Path");
                    return;
                }

                int upperlimit = 0;
                int pgBarCounter = 0;
                SucessRecord = 0;
                NotFoundRecord = 0;
                if (SelectPatientXrays != null && SelectPatientXrays.Count() > 0)
                {
                    ReportResultMass view = (ReportResultMass)this.View;
                    foreach (var currentData in SelectPatientXrays)
                    {
                        upperlimit++;
                    }
                    view.SetProgressBarLimits(0, upperlimit);

                    foreach (var patientInfo in SelectPatientXrays.ToList())
                    {
                        string modlity = patientInfo.Modality;
                        if (modlity == "DX")
                        {
                            modlity = "'DX','CR'";
                        }
                        else if (modlity == "MG")
                        {
                            modlity = "'MG','US'";
                        }
                        else
                        {
                            modlity = "'" + patientInfo.Modality + "'";
                        }

                        List<byte[]> dicomFiles = DataService.PACS.GetDicomFileByPatientID(patientInfo.HN, patientInfo.RequestedDttm, modlity, IsSINE);
                        if (dicomFiles != null && dicomFiles.Count() > 0)
                        {
                            foreach (var file in dicomFiles)
                            {
                                MemoryStream ms = new MemoryStream(file);
                                var dicomFile = Dicom.DicomFile.Open(ms);
                                string instanceUID = dicomFile.Dataset.GetSingleValueOrDefault<string>(DicomTag.SOPInstanceUID, "");
                                dicomFile.Dataset.AddOrUpdate(DicomTag.SpecificCharacterSet, Encoding.UTF8, "ISO_IR 192");
                                dicomFile.Dataset.AddOrUpdate(DicomTag.PatientID, Encoding.UTF8, !string.IsNullOrEmpty(patientInfo.OtherID) ? patientInfo.OtherID : patientInfo.HN);
                                if (!IsNumberSequence)
                                {
                                    dicomFile.Dataset.AddOrUpdate(DicomTag.PatientName, Encoding.UTF8, patientInfo.PatientName);
                                }
                                else
                                {
                                    dicomFile.Dataset.AddOrUpdate(DicomTag.PatientName, Encoding.UTF8, patientInfo.No.ToString().PadLeft(4, '0') + " " + patientInfo.PatientName);
                                }
                                string fileName = (!string.IsNullOrEmpty(patientInfo.OtherID) ? patientInfo.OtherID : patientInfo.HN) + "_" + patientInfo.PatientName + "_" + instanceUID;
                                var image = new DicomImage(dicomFile.Dataset);
                                image.RenderImage().AsClonedBitmap().Save(ExportFilePath + "\\" + fileName + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                                dicomFiles.Remove(file);
                                MemoryManagement.FlushMemory();
                            }
                            ++SucessRecord;
                        }
                        else
                        {
                            ++NotFoundRecord;
                        }
                        SelectPatientXrays.Remove(patientInfo);
                        pgBarCounter = pgBarCounter + 1;
                        view.SetProgressBarValue(pgBarCounter);
                    }
                    OnUpdateEvent();
                    view.SetProgressBarValue(upperlimit);
                }
            }
            catch (Exception ex)
            {

                ErrorDialog(ex.Message);
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



        #endregion
    }
}
