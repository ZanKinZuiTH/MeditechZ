using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MediTech.DataService;
using MediTech.Helpers;
using MediTech.Model;
using MediTech.Model.Report;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace MediTech.ViewModels
{
    public class TranslateXrayViewModel : MediTechViewModelBase
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

        private PayorDetailModel _SelectedPayorDetail;

        public PayorDetailModel SelectedPayorDetail
        {
            get { return _SelectedPayorDetail; }
            set { Set(ref _SelectedPayorDetail, value); }
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

        private RelayCommand _ShowFieldCommand;

        public RelayCommand ShowFieldCommand
        {
            get
            {
                return _ShowFieldCommand
                    ?? (_ShowFieldCommand = new RelayCommand(ShowFiled));
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


        private RelayCommand _TranslateCommand;

        public RelayCommand TranslateCommand
        {
            get
            {
                return _TranslateCommand
                    ?? (_TranslateCommand = new RelayCommand(Translate));
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

        bool showColumnSelector = true;
        public TranslateXrayViewModel()
        {
            DateFrom = DateTime.Now;
            DateTo = DateTime.Now;

            ResultStatus = DataService.Technical.GetReferenceValueMany("RABSTS");
            SelectResultStatus = ResultStatus.FirstOrDefault();

            OrderItems = new List<LookupItemModel>();
            OrderItems.Add(new LookupItemModel { Key = 1, Display = "UltraSound Abdomen" });
            OrderItems.Add(new LookupItemModel { Key = 2, Display = "UltraSound Breast" });
            OrderItems.Add(new LookupItemModel { Key = 3, Display = "Chest PA" });

            SelectOrderItem = OrderItems.FirstOrDefault();

            PayorDetails = DataService.MasterData.GetPayorDetail();
        }

        public override void OnLoaded()
        {
            ShowColumnSelector(showColumnSelector);
        }

        private void Search()
        {
            string itemName = string.Empty;
            PatientXrays = null;
            SelectPatientXrays = null;
            int? resultStatusUID;
            int? payorDetailUID;
            long? patientUID = null;
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
            payorDetailUID = SelectedPayorDetail != null ? SelectedPayorDetail.PayorDetailUID : (int?)null;
            var dataList = DataService.Radiology.SearchResultRadiologyForTranslate(DateFrom, DateTo, patientUID, itemName, resultStatusUID, payorDetailUID);
            if (dataList != null)
                PatientXrays = new ObservableCollection<PatientResultRadiology>(dataList);
            else
                PatientXrays = null;
            List<XrayTranslateMappingModel> dtResultMapping = DataService.Radiology.GetXrayTranslateMapping();
            if (PatientXrays != null && PatientXrays.Count > 0)
            {
                foreach (var item in PatientXrays)
                {

                    List<string> listNoMapResult = new List<string>();
                    string thairesult = TranslateResult.TranslateResultXray(item.ResultValue, item.ResultStatus,item.RequestItemName, ",", dtResultMapping, ref listNoMapResult);

                    item.ThaiResult = thairesult;
                    if (item.ThaiResult == string.Empty)
                    {
                        SelectPatientXrays.Add(item);
                    }
                }
                OnUpdateEvent();
            }
            else
            {
                InformationDialog("ไม่พบข้อมูล");
            }
        }

        private void ShowFiled()
        {
            this.showColumnSelector = !this.showColumnSelector;
            ShowColumnSelector(showColumnSelector);
        }

        private void ExportData()
        {
            if (PatientXrays != null)
            {
                string fileName = ShowSaveFileDialog("Microsoft Excel Document", "Microsoft Excel|*.xlsx");
                if (fileName != "")
                {
                    TranslateXray view = (TranslateXray)this.View;
                    view.gvPatientXray.ExportToXlsx(fileName);
                    OpenFile(fileName);
                }

            }
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

        private void ShowColumnSelector(bool showFrom)
        {
            TranslateXray view = (TranslateXray)this.View;
            if (showFrom)
            {
                view.btnShowColumn.Content = "Show Column";
                view.gvPatientXray.ShowColumnChooser();
            }
            else
            {
                view.btnShowColumn.Content = "Hide Column";
                view.gvPatientXray.HideColumnChooser();
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
        private void Translate()
        {
            int upperlimit = 0;
            int pgBarCounter = 0;
            try
            {
                TranslateXray view = (TranslateXray)this.View;
                foreach (var currentData in SelectPatientXrays)
                {
                    upperlimit++;
                }
                view.SetProgressBarLimits(0, upperlimit);

                List<XrayTranslateMappingModel> dtResultMapping = DataService.Radiology.GetXrayTranslateMapping();
                foreach (var item in SelectPatientXrays.ToList())
                {
                    string thaiResult = string.Empty;
                    List<string> wordnomap = new List<string>();
                    thaiResult = TranslateResult.TranslateResultXray(item.ResultValue, item.ResultStatus,item.RequestItemName, ",", dtResultMapping, ref wordnomap);

                    if (wordnomap.Count == 0 && thaiResult == "")
                    {
                        ErrorDialog(String.Format("ไม่สามารถแปลงผลของ {0} โปรดแจ้ง Admin", item.PatientName + " (" + item.HN + ")"));
                    }

                    if (wordnomap.Count > 0)
                    {
                        TranslateXrayPopup tranPopup = new TranslateXrayPopup(wordnomap);
                        PatientVisitModel patVisit = DataService.PatientIdentity.GetPatientVisitByUID(item.PatientVisitUID);
                        tranPopup.patBanner.PatientVisit = patVisit;
                        tranPopup.txtOrderName.Text = item.RequestItemName;
                        tranPopup.txtResultStatus.Text = item.ResultStatus;
                        tranPopup.patientResultRadioloty = item;
                        tranPopup.ShowDialog();
                        if (tranPopup.IsCancel)
                        {
                            break;
                        }
                        if (tranPopup.IsUpdateResult)
                        {
                            item.ResultHtml = tranPopup.patientResultRadioloty.ResultHtml;
                            item.ResultValue = tranPopup.patientResultRadioloty.ResultValue;
                            item.ResultStatus = tranPopup.patientResultRadioloty.ResultStatus;
                            item.Doctor = tranPopup.patientResultRadioloty.Doctor;
                        }

                        dtResultMapping = DataService.Radiology.GetXrayTranslateMapping();
                        thaiResult = TranslateResult.TranslateResultXray(item.ResultValue, item.ResultStatus,item.RequestItemName,",", dtResultMapping, ref wordnomap);
                        item.ThaiResult = thaiResult;

                        MessageBoxResult diagResult = MessageBoxResult.None;
                        if (item.ResultStatus.ToLower() == "abnormal" && thaiResult == "ปกติ")
                        {
                            diagResult = QuestionDialog(String.Format("ผู้ป่วย {0} ลงสถานะเป็น Abnormal แต่คำแปลบอกว่า \"ปกติ\" ต้องการตรวจสอบผลใหม่หรือไม่", item.PatientName + " (" + item.HN + ")"));
                        }
                        else if (item.ResultStatus.ToLower() == "normal" && (thaiResult != "" && thaiResult != "ปกติ"))
                        {
                            diagResult = QuestionDialog(String.Format("ผู้ป่วย {0} ลงสถานะเป็น Normal แต่คำแปลบอกว่า \"ไม่ปกติ\" ต้องการตรวจสอบผลใหม่หรือไม่", item.PatientName + " (" + item.HN + ")"));
                        }

                        if (diagResult == MessageBoxResult.Yes)
                        {
                            ReviewRISResult review = new ReviewRISResult();
                            ReviewRISResultViewModel viewModel = (review.DataContext as ReviewRISResultViewModel);
                            viewModel.AssignModel(item.PatientUID, item.PatientVisitUID, item.RequestUID, item.RequestDetailUID);
                            ReviewRISResultViewModel reviewViewModel = (ReviewRISResultViewModel)viewModel.LaunchViewDialog(review, "RESTREV", false, true);
                            if (reviewViewModel != null && reviewViewModel.ResultDialog == ActionDialog.Save)
                            {
                                ResultRadiologyModel dataResultLast = DataService.Radiology.GetResultRadiologyByResultUID(item.ResultUID);
                                item.ResultValue = dataResultLast.PlainText;
                                item.ResultStatus = dataResultLast.ResultStatus;
                                item.ResultHtml = dataResultLast.Value;
                                item.Doctor = dataResultLast.Radiologist;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(item.ThaiResult))
                    {
                        SelectPatientXrays.Remove(item);
                        OnUpdateEvent();
                    }

                    pgBarCounter = pgBarCounter + 1;
                    view.SetProgressBarValue(pgBarCounter);
                }
                view.SetProgressBarValue(upperlimit);
                //InformationDialog("แปลงผลเสร็จสิ้น");


            }
            catch (Exception ex)
            {

                ErrorDialog(ex.Message);
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

        private void ImportFile()
        {
            OleDbConnection conn;
            OleDbCommand cmd;
            System.Data.DataTable dt;
            DataTable ImportData = new DataTable();
            DataSet objDataset1;
            string connectionString = string.Empty;
            int pgBarCounter = 0;
            TranslateXray view = (TranslateXray)this.View;
            try
            {
                if (FileLocation.Trim() != string.Empty)
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
                        if (dt.AsEnumerable().Where(p => p["Table_name"].ToString().ToUpper() == "ลงคอม$").Count() <= 0)
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
                                OleDbCommand objCmdSelect = new OleDbCommand("SELECT * FROM [ลงคอม$] Where ([Order NO] <> '' OR [Order NO] IS NOT NULL)", conn);
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

                    int upperlimit = ImportData.Rows.Count;
                    view.SetProgressBarLimits(0, upperlimit);

                    string itemName = string.Empty;
                    int? resultStatusUID;
                    int? payorDetailUID;
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
                    resultStatusUID = SelectResultStatus != null ? SelectResultStatus.Key : (int?)null;
                    payorDetailUID = SelectedPayorDetail != null ? SelectedPayorDetail.PayorDetailUID : (int?)null;
                    List<XrayTranslateMappingModel> dtResultMapping = DataService.Radiology.GetXrayTranslateMapping();
                    foreach (DataRow item in ImportData.Rows)
                    {

                        PatientResultRadiology dtResult = DataService.Radiology.SearchPatientResultRadiologyForTranslate(DateFrom, DateTo, item["HN"].ToString().Trim(), itemName, resultStatusUID, payorDetailUID);

                        if (dtResult != null && dtResult.ResultEnteredDttm.ToString() != "")
                        {
                            List<string> listNoMapResult = new List<string>();
                            string thaiResult = TranslateResult.TranslateResultXray(dtResult.ResultValue, dtResult.ResultStatus,dtResult.RequestItemName, ",", dtResultMapping, ref listNoMapResult);
                            dtResult.ThaiResult = thaiResult;

                            PatientXrays.Add(dtResult);
                            if (dtResult.ThaiResult == string.Empty)
                            {
                                SelectPatientXrays.Add(dtResult);
                            }
                        }
                        else
                        {
                            PatientInformationModel patient = DataService.PatientIdentity.GetPatientByHN(item["HN"].ToString());
                            PatientResultRadiology patRs = new PatientResultRadiology();
                            if (patient != null)
                            {
                                patRs.HN = patient.PatientID;
                                patRs.PatientName = patient.PatientName;
                            }
                            else
                            {
                                patRs.HN = item["HN"].ToString();
                                patRs.PatientName = item["PreName"].ToString() + " " + item["FirstName"].ToString() + " " + item["LastName"].ToString();
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
