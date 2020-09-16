using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MediTech.Models;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using MediTech.Views;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data;
using DevExpress.Xpf.Grid;

namespace MediTech.ViewModels
{
    public class ImportLabResultViewModel : MediTechViewModelBase
    {
        #region Properties

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

        public bool IsEditDate { get; set; }

        public List<LookupItemModel> DateTypes { get; set; }
        private LookupItemModel _SelectDateType;

        public LookupItemModel SelectDateType
        {
            get { return _SelectDateType; }
            set
            {
                Set(ref _SelectDateType, value);

                if (SelectDateType != null)
                {
                    if (SelectDateType.Key == 1)
                    {
                        IsEditDate = false;
                        DateFrom = DateTime.Now.AddDays(-30);
                        DateTo = DateTime.Now;
                        IsEditDate = true;
                    }
                    else if (SelectDateType.Key == 2)
                    {
                        IsEditDate = false;
                        DateFrom = DateTime.Now.AddDays(-45);
                        DateTo = DateTime.Now;
                        IsEditDate = true;
                    }
                    else if (SelectDateType.Key == 3)
                    {
                        IsEditDate = false;
                        DateFrom = DateTime.Now.AddDays(-90);
                        DateTo = DateTime.Now;
                        IsEditDate = true;
                    }
                }
            }
        }

        private DateTime? _DateFrom;

        public DateTime? DateFrom
        {
            get { return _DateFrom; }
            set
            {
                Set(ref _DateFrom, value);
                if (IsEditDate)
                {
                    SelectDateType = null;
                }

            }
        }


        private DateTime? _DateTo;

        public DateTime? DateTo
        {
            get { return _DateTo; }
            set
            {
                Set(ref _DateTo, value);
                if (IsEditDate)
                {
                    SelectDateType = null;
                }
            }
        }

        private List<LookupReferenceValueModel> _RequestStatus;

        public List<LookupReferenceValueModel> RequestStatus
        {
            get { return _RequestStatus; }
            set { Set(ref _RequestStatus, value); }
        }


        private LookupReferenceValueModel _SelectRequestStatus;

        public LookupReferenceValueModel SelectRequestStatus
        {
            get { return _SelectRequestStatus; }
            set { Set(ref _SelectRequestStatus, value); }
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
            set { Set(ref _SelectOrganisation, value); }
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

        private List<RequestItemModel> _RequestItems;

        public List<RequestItemModel> RequestItems
        {
            get { return _RequestItems; }
            set { _RequestItems = value; }
        }

        private RequestItemModel _SelectedRequestItem;

        public RequestItemModel SelectedRequestItem
        {
            get { return _SelectedRequestItem; }
            set
            {
                _SelectedRequestItem = value;
                var resultItems = DataService.MasterData.GetRequestResultLinkByRequestItemUID(SelectedRequestItem.RequestItemUID);
                if (resultItems != null)
                {
                    ColumnsResultItems = new ObservableCollection<Column>();
                    ColumnsResultItems.Add(new Column() { Header = "HN", FieldName = "HN" });
                    foreach (var item in resultItems.OrderBy(p => p.PrintOrder))
                    {
                        string parameterName = item.ResultItemName +
                            (!string.IsNullOrEmpty(item.Unit) ? " ( " + item.Unit + " )" : "");
                        ColumnsResultItems.Add(new Column()
                        {
                            Header = parameterName,
                            FieldName = parameterName,
                            Tag = item.ResultItemUID
                        });
                    }
                }

            }
        }

        private ObservableCollection<Column> _ColumnsResultItems;

        public ObservableCollection<Column> ColumnsResultItems
        {
            get { return _ColumnsResultItems; }
            set { Set(ref _ColumnsResultItems, value); }
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

        private RelayCommand _ExportCommand;

        public RelayCommand ExportCommand
        {
            get
            {
                return _ExportCommand
                    ?? (_ExportCommand = new RelayCommand(Export));
            }
        }

        #endregion

        #region Method

        public ImportLabResultViewModel()
        {
            RequestItems = DataService.MasterData.GetRequestItemByCategory("Lab");
            RequestItems = RequestItems?.Where(p => p.Code == "LAB111" || p.Code == "LAB311").ToList();
            Organisations = GetHealthOrganisationRoleMedical();
            PayorDetails = DataService.MasterData.GetPayorDetail();
            DateTypes = new List<LookupItemModel>();
            DateTypes.Add(new LookupItemModel { Key = 1, Display = "30 วัน" });
            DateTypes.Add(new LookupItemModel { Key = 2, Display = "45 วัน" });
            DateTypes.Add(new LookupItemModel { Key = 3, Display = "90 วัน" });
            SelectDateType = DateTypes.FirstOrDefault();
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
            if (SelectedRequestItem == null)
            {
                WarningDialog("กรุณาเลือกรายการ Lab");
                return;
            }
            OleDbConnection conn;
            OleDbCommand cmd;
            System.Data.DataTable dt;
            DataTable ImportData = new DataTable();
            DataSet objDataset1;
            string connectionString = string.Empty;
            int pgBarCounter = 0;
            ImportLabResult view = (ImportLabResult)this.View;
            try
            {
                if (!string.IsNullOrEmpty(FileLocation))
                {
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
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            for (int row = 0; row < dt.Rows.Count;)
                            {
                                string FileName = Convert.ToString(dt.Rows[row]["Table_Name"]);
                                cmd = conn.CreateCommand();
                                OleDbCommand objCmdSelect = new OleDbCommand("SELECT * FROM [" + FileName + "]  Where ([HN] <> '' OR [HN] <> '0' OR [HN] IS NOT NULL)", conn);
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
                    DataTable tempDt = ImportData.Clone();
                    tempDt.Clear();
                    view.gcTestParameter.ItemsSource = tempDt.DefaultView;
                    int upperlimit = ImportData.AsEnumerable().Count(p => !string.IsNullOrEmpty(p["HN"].ToString()) && p["HN"].ToString() != "0");
                    view.SetProgressBarLimits(0, upperlimit);
                    foreach (DataRow item in ImportData.Rows)
                    {
                        string patientID = item["HN"].ToString();
                        if (string.IsNullOrEmpty(patientID) || patientID == "0")
                        {
                            continue;
                        }
                        view.gvTestParameter.AddNewRow();
                        int newRowHandle = DataControlBase.NewItemRowHandle;
                        foreach (var column in ImportData.Columns)
                        {
                            view.gcTestParameter.SetCellValue(newRowHandle, column.ToString(), item[column.ToString()].ToString());

                            //Urine Analysis
                            if (SelectedRequestItem.Code == "LAB311")
                            {
                                if (column.ToString() == "Leukocyte")
                                {
                                    string rbcValue = "";
                                    switch (item[column.ToString()].ToString())
                                    {
                                        case "Negative":
                                            rbcValue = "0-2";
                                            break;
                                        case "Trace":
                                            rbcValue = "2-10";
                                            break;
                                        case "+1":
                                        case "1+":
                                            rbcValue = "10-25";
                                            break;
                                        case "+2":
                                        case "2+":
                                            rbcValue = "25-80";
                                            break;
                                        case "+3":
                                        case "3+":
                                            rbcValue = "> 200";
                                            break;
                                    }
                                    var rbcColumn = (from c in ImportData.Columns.Cast<DataColumn>()
                                                     where c.ColumnName.ToLower().Contains("rbc")
                                                     select c.ColumnName).FirstOrDefault();
                                    view.gcTestParameter.SetCellValue(newRowHandle, rbcColumn, rbcValue);
                                }
                                else if (column.ToString() == "Blood")
                                {
                                    string wbcValue = "";
                                    switch (item[column.ToString()].ToString())
                                    {
                                        case "Negative":
                                            wbcValue = "0-5";
                                            break;
                                        case "Trace":
                                            wbcValue = "5-15";
                                            break;
                                        case "+1":
                                        case "1+":
                                            wbcValue = "15-70";
                                            break;
                                        case "+2":
                                        case "2+":
                                            wbcValue = "70-125";
                                            break;
                                        case "+3":
                                        case "3+":
                                            wbcValue = "> 500";
                                            break;
                                    }
                                    var wbcColumn = (from c in ImportData.Columns.Cast<DataColumn>()
                                                     where c.ColumnName.ToLower().Contains("wbc")
                                                     select c.ColumnName).FirstOrDefault();
                                    view.gcTestParameter.SetCellValue(newRowHandle, wbcColumn, wbcValue);
                                }
                            }


                        }
                        pgBarCounter = pgBarCounter + 1;
                        TotalRecord = pgBarCounter;
                        view.SetProgressBarValue(pgBarCounter);
                    }
                }
            }
            catch (Exception er)
            {

                System.Windows.Forms.MessageBox.Show(er.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Export()
        {
            string fileName = ShowSaveFileDialog("Microsoft Excel Document", "Microsoft Excel|*.xlsx");
            if (fileName != "")
            {
                ImportLabResult view = (ImportLabResult)this.View;
                view.gvTestParameter.ExportToXlsx(fileName);
                OpenFile(fileName);
            }
        }

        private string ShowSaveFileDialog(string title, string filter)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            string name = SelectedRequestItem != null ? SelectedRequestItem.ItemName.Replace(":", "") + "_Result" : System.Windows.Forms.Application.ProductName;
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
