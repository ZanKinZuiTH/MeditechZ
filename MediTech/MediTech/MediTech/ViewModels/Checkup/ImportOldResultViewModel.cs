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
using System.Windows.Data;
using DevExpress.Data.Extensions;
using System.Text.RegularExpressions;
using DevExpress.XtraRichEdit.API.Native;
using DevExpress.Xpf.WindowsUI.Navigation;
using DevExpress.XtraRichEdit.Import.Html;
using DevExpress.DataAccess.DataFederation;

namespace MediTech.ViewModels
{
    public class ImportOldResultViewModel : MediTechViewModelBase
    {
        #region Properties

        private int _SelectTabIndex;
        public int SelectTabIndex
        {
            get { return _SelectTabIndex; }
            set
            {
                Set(ref _SelectTabIndex, value);
                if (SelectTabIndex == 1)
                {
                    ImportOldResult view = (ImportOldResult)this.View;
                    view.gcRiskHistory.ItemsSource = null;
                    tempDt = new DataTable();
                    ColumnsRiskItems = new ObservableCollection<Column>();
                    ColumnsRiskItems.Add(new Column() { Header = "NO", FieldName = "NO", VisibleIndex = 0 });
                    ColumnsRiskItems.Add(new Column() { Header = "HN", FieldName = "HN", VisibleIndex = 1 });
                    ColumnsRiskItems.Add(new Column() { Header = "EmployeeID", FieldName = "EmployeeID", VisibleIndex = 2 });
                    ColumnsRiskItems.Add(new Column() { Header = "Title", FieldName = "Title", VisibleIndex = 3 });
                    ColumnsRiskItems.Add(new Column() { Header = "FirstName", FieldName = "FirstName", VisibleIndex = 4 });
                    ColumnsRiskItems.Add(new Column() { Header = "LastName", FieldName = "LastName", VisibleIndex = 5 });

                    ColumnsRiskItems.Add(new Column() { Header = "เคยมีประวัติเป็นโรคปอดหรือไม่", FieldName = "เคยมีประวัติเป็นโรคปอดหรือไม่", VisibleIndex = 6 });
                    ColumnsRiskItems.Add(new Column() { Header = "เคยมีประวัติเป็นโรคลมชักหรือไม่", FieldName = "เคยมีประวัติเป็นโรคลมชักหรือไม่", VisibleIndex = 7 });
                    ColumnsRiskItems.Add(new Column() { Header = "เคยมีอาการเป็นลม หมดสติ หรือวูบหรือไม่", FieldName = "เคยมีอาการเป็นลม หมดสติ หรือวูบหรือไม่", VisibleIndex = 8 });
                    ColumnsRiskItems.Add(new Column() { Header = "เคยมีประวัติเป็นโรคหัวใจหรือความดันสูงหรือไม่", FieldName = "เคยมีประวัติเป็นโรคหัวใจหรือความดันสูงหรือไม่", VisibleIndex = 9 });
                    ColumnsRiskItems.Add(new Column() { Header = "เคยมีประวัติเป็นโรคโลหิตจางหรือไม่", FieldName = "เคยมีประวัติเป็นโรคโลหิตจางหรือไม่", VisibleIndex = 10 });
                    ColumnsRiskItems.Add(new Column() { Header = "โรคอื่นๆ", FieldName = "โรคอื่นๆ", VisibleIndex = 20 });


                    ColumnsRiskItems.Add(new Column() { Header = "ท่านมีโรคประจำตัวหรือโรคเรื้อรังหรือไม่", FieldName = "ท่านมีโรคประจำตัวหรือโรคเรื้อรังหรือไม่", VisibleIndex = 11 });
                    ColumnsRiskItems.Add(new Column() { Header = "ท่านเคยได้รับการผ่าตัดหรือไม่", FieldName = "ท่านเคยได้รับการผ่าตัดหรือไม่", VisibleIndex = 12 });
                    ColumnsRiskItems.Add(new Column() { Header = "ท่านเคยได้รับภูมิคุ้มกันกรณีเกิดโรคระบาดหรือโรคติดต่อหรือไม่", FieldName = "ท่านเคยได้รับภูมิคุ้มกันกรณีเกิดโรคระบาดหรือโรคติดต่อหรือไม่", VisibleIndex = 13 });
                    ColumnsRiskItems.Add(new Column() { Header = "ประวัติการเจ็บป่วยของสมาชิกในครอบครัว", FieldName = "ประวัติการเจ็บป่วยของสมาชิกในครอบครัว", VisibleIndex = 14 });
                    ColumnsRiskItems.Add(new Column() { Header = "ปัจจุบันมียาที่ต้องรับประทานเป็นประจำหรือไม่", FieldName = "ปัจจุบันมียาที่ต้องรับประทานเป็นประจำหรือไม่", VisibleIndex = 15 });
                    ColumnsRiskItems.Add(new Column() { Header = "มีประวัติแพ้ยาหรือไม่", FieldName = "มีประวัติแพ้ยาหรือไม่", VisibleIndex = 16 });
                    ColumnsRiskItems.Add(new Column() { Header = "เคยสูบบุหรี่บ้างหรือไม่", FieldName = "เคยสูบบุหรี่บ้างหรือไม่", VisibleIndex = 17 });
                    ColumnsRiskItems.Add(new Column() { Header = "ดื่มสุรา เบียร์ หรือเครื่องดื่มที่มีแอลกอฮอล์บ้างหรือไม่", FieldName = "ดื่มสุรา เบียร์ หรือเครื่องดื่มที่มีแอลกอฮอล์บ้างหรือไม่", VisibleIndex = 18 });
                    ColumnsRiskItems.Add(new Column() { Header = "เคยเสพยาเสพติดใด ๆ บ้างหรือไม่", FieldName = "เคยเสพยาเสพติดใด ๆ บ้างหรือไม่", VisibleIndex = 19 });
  
                    tempDt.Columns.Add("NO");
                    tempDt.Columns.Add("HN");
                    tempDt.Columns.Add("EmployeeID");
                    tempDt.Columns.Add("Title");
                    tempDt.Columns.Add("FirstName");
                    tempDt.Columns.Add("LastName");
                }

                if (SelectTabIndex == 2)
                {
                    ImportOldResult view = (ImportOldResult)this.View;
                    view.gcInjuryHistory.ItemsSource = null;
                    tempDt = new DataTable();
                    ColumnsInjuryItems = new ObservableCollection<Column>();
                    ColumnsInjuryItems.Add(new Column() { Header = "NO", FieldName = "NO", VisibleIndex = 0 });
                    ColumnsInjuryItems.Add(new Column() { Header = "HN", FieldName = "HN", VisibleIndex = 1 });
                    ColumnsInjuryItems.Add(new Column() { Header = "EmployeeID", FieldName = "EmployeeID", VisibleIndex = 2 });
                    ColumnsInjuryItems.Add(new Column() { Header = "Title", FieldName = "Title", VisibleIndex = 3 });
                    ColumnsInjuryItems.Add(new Column() { Header = "FirstName", FieldName = "FirstName", VisibleIndex = 4 });
                    ColumnsInjuryItems.Add(new Column() { Header = "LastName", FieldName = "LastName", VisibleIndex = 5 });

                    ColumnsInjuryItems.Add(new Column() { Header = "ส่วนของร่างกายที่บาดเจ็บ", FieldName = "ส่วนของร่างกายที่บาดเจ็บ", VisibleIndex = 6 });
                    ColumnsInjuryItems.Add(new Column() { Header = "สาเหตุของการบาดเจ็บหรือการเจ็บป่วย", FieldName = "สาเหตุของการบาดเจ็บหรือการเจ็บป่วย", VisibleIndex = 7 });
                    ColumnsInjuryItems.Add(new Column() { Header = "ระดับความรุนแรง", FieldName = "ระดับความรุนแรง", VisibleIndex = 8 });
                   
                }
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
                Location = null;
                if (SelectOrganisation != null)
                {
                    var loct = DataService.MasterData.GetLocationByOrganisationUID(SelectOrganisation.HealthOrganisationUID);
                    Location = loct.Where(p => p.IsRegistrationAllowed == "Y").ToList();
                }
            }
        }

        private List<LocationModel> _Location;

        public List<LocationModel> Location
        {
            get { return _Location; }
            set { Set(ref _Location, value); }
        }

        private LocationModel _SelectLocation;

        public LocationModel SelectLocation
        {
            get { return _SelectLocation; }
            set { Set(ref _SelectLocation, value); }
        }

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

        private List<string> _YearSource;
        public List<string> YearSource
        {
            get { return _YearSource; }
            set { Set(ref _YearSource, value); }
        }

        private string _SelectYearSource;
        public string SelectYearSource
        {
            get { return _SelectYearSource; }
            set { Set(ref _SelectYearSource, value); }
        }

        private List<InsuranceCompanyModel> _InsuranceCompany;
        public List<InsuranceCompanyModel> InsuranceCompany
        {
            get { return _InsuranceCompany; }
            set { Set(ref _InsuranceCompany, value); }
        }

        private InsuranceCompanyModel _SelectInsuranceCompany;
        public InsuranceCompanyModel SelectInsuranceCompany
        {
            get { return _SelectInsuranceCompany; }
            set { Set(ref _SelectInsuranceCompany, value); 
            if(SelectInsuranceCompany != null)
                {
                    PayorDetail = DataService.Billing.GetPayorDetailByInsuranceCompanyUID(SelectInsuranceCompany.InsuranceCompanyUID);
                    SelectPayorDetail = PayorDetail.FirstOrDefault();
                }
            }
        }

        private List<PayorDetailModel> _PayorDetail;

        public List<PayorDetailModel> PayorDetail
        {
            get { return _PayorDetail; }
            set { Set(ref _PayorDetail, value); }
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

        private ObservableCollection<Column> _ColumnsResultItems;

        public ObservableCollection<Column> ColumnsResultItems
        {
            get { return _ColumnsResultItems; }
            set { Set(ref _ColumnsResultItems, value);  }
        }

        private ObservableCollection<Column> _ColumnsRiskItems;

        public ObservableCollection<Column> ColumnsRiskItems
        {
            get { return _ColumnsRiskItems; }
            set { Set(ref _ColumnsRiskItems, value);
            }
        }
        
        private ObservableCollection<Column> _ColumnsInjuryItems;

        public ObservableCollection<Column> ColumnsInjuryItems
        {
            get { return _ColumnsInjuryItems; }
            set
            {
                Set(ref _ColumnsInjuryItems, value);
            }
        }

        private RequestItemModel _SelectedRequestItem;

        public RequestItemModel SelectedRequestItem
        {
            get { return _SelectedRequestItem; }
            set
            {
                _SelectedRequestItem = value;
                if (SelectedRequestItem.RequestResultLinks != null)
                {
                    ImportOldResult view = (ImportOldResult)this.View;
                    view.gcTestParameter.ItemsSource = null;
                    tempDt = new DataTable();
                    ColumnsResultItems = new ObservableCollection<Column>();
                    ColumnsResultItems.Add(new Column() { Header = "NO", FieldName = "NO", VisibleIndex = 0 });
                    ColumnsResultItems.Add(new Column() { Header = "HN", FieldName = "HN", VisibleIndex = 1 });
                    ColumnsResultItems.Add(new Column() { Header = "EmployeeID", FieldName = "EmployeeID", VisibleIndex = 2 });
                    ColumnsResultItems.Add(new Column() { Header = "Title", FieldName = "Title", VisibleIndex = 3 });
                    ColumnsResultItems.Add(new Column() { Header = "FirstName", FieldName = "FirstName", VisibleIndex = 4 });
                    ColumnsResultItems.Add(new Column() { Header = "LastName", FieldName = "LastName", VisibleIndex = 5});
                    tempDt.Columns.Add("NO");
                    tempDt.Columns.Add("HN");
                    tempDt.Columns.Add("EmployeeID");
                    tempDt.Columns.Add("Title");
                    tempDt.Columns.Add("FirstName");
                    tempDt.Columns.Add("LastName");
                    tempDt.Columns.Add("PatientUID");
                    tempDt.Columns.Add("PatientID");
                    tempDt.Columns.Add("IsSave");
                    //tempDt.Columns.Add("PatientVisitUID");
                    //tempDt.Columns.Add("RequestUID");
                    //tempDt.Columns.Add("RequestDetailUID");
                    //tempDt.Columns.Add("SEXXXUID");

                    int visibleIndex = 6;
                    if (SelectedRequestItem.Code == "LAB111")
                    {
                        foreach (var item in SelectedRequestItem.RequestResultLinks)
                        {
                            if (item.ResultItemName.ToLower().StartsWith("wbc"))
                            {
                                item.PrintOrder = 1;
                            }
                            else if (item.ResultItemName.ToLower().StartsWith("bas"))
                            {
                                item.PrintOrder = 2;
                            }
                            else if (item.ResultItemName.ToLower().StartsWith("neu"))
                            {
                                item.PrintOrder = 3;
                            }
                            else if (item.ResultItemName.ToLower().StartsWith("eos"))
                            {
                                item.PrintOrder = 4;
                            }
                            else if (item.ResultItemName.ToLower().StartsWith("lym"))
                            {
                                item.PrintOrder = 5;
                            }
                            else if (item.ResultItemName.ToLower().StartsWith("mon"))
                            {
                                item.PrintOrder = 6;
                            }
                            else if (item.ResultItemName.ToLower() == "rbc")
                            {
                                item.PrintOrder = 7;
                            }
                            else if (item.ResultItemName.ToLower().StartsWith("hb"))
                            {
                                item.PrintOrder = 8;
                            }
                            else if (item.ResultItemName.ToLower().StartsWith("mcv"))
                            {
                                item.PrintOrder = 9;
                            }
                            else if (item.ResultItemName.ToLower() == "mch")
                            {
                                item.PrintOrder = 10;
                            }
                            else if (item.ResultItemName.ToLower() == "mchc")
                            {
                                item.PrintOrder = 11;
                            }
                            else if (item.ResultItemName.ToLower().StartsWith("rdw"))
                            {
                                item.PrintOrder = 12;
                            }
                            else if (item.ResultItemName.ToLower().StartsWith("hct"))
                            {
                                item.PrintOrder = 13;
                            }
                            else if (item.ResultItemName.ToLower() == "platelets count")
                            {
                                item.PrintOrder = 14;
                            }
                            else if (item.ResultItemName.ToLower().StartsWith("mpv"))
                            {
                                item.PrintOrder = 15;
                            }
                            else
                            {
                                item.PrintOrder = 999;
                            }
                        }
                    }
                    else if (SelectedRequestItem.Code == "LAB311")
                    {
                        foreach (var item in SelectedRequestItem.RequestResultLinks)
                        {
                            if (item.ResultItemName.ToLower() == "leukocyte")
                            {
                                item.PrintOrder = 1;
                            }
                            else if (item.ResultItemName.ToLower() == "ketone")
                            {
                                item.PrintOrder = 2;
                            }
                            else if (item.ResultItemName.ToLower() == "nitrite")
                            {
                                item.PrintOrder = 3;
                            }
                            else if (item.ResultItemName.ToLower() == "urobilinogen")
                            {
                                item.PrintOrder = 4;
                            }
                            else if (item.ResultItemName.ToLower() == "bilirubin")
                            {
                                item.PrintOrder = 5;
                            }
                            else if (item.ResultItemName.ToLower() == "protein")
                            {
                                item.PrintOrder = 6;
                            }
                            else if (item.ResultItemName.ToLower() == "glucose")
                            {
                                item.PrintOrder = 7;
                            }
                            else if (item.ResultItemName.ToLower() == "specific gravity")
                            {
                                item.PrintOrder = 8;
                            }
                            else if (item.ResultItemName.ToLower() == "blood")
                            {
                                item.PrintOrder = 9;
                            }
                            else if (item.ResultItemName.ToLower() == "ph")
                            {
                                item.PrintOrder = 10;
                            }
                            else if (item.ResultItemName.ToLower() == "color")
                            {
                                item.PrintOrder = 11;
                            }
                            else if (item.ResultItemName.ToLower() == "appearance")
                            {
                                item.PrintOrder = 12;
                            }
                            else if (item.ResultItemName.ToLower() == "rbc in urinalysis")
                            {
                                item.PrintOrder = 13;
                            }
                            else if (item.ResultItemName.ToLower() == "wbc in urine analysis")
                            {
                                item.PrintOrder = 14;
                            }
                            else if (item.ResultItemName.ToLower() == "epithelial cells")
                            {
                                item.PrintOrder = 15;
                            }
                            else
                            {
                                item.PrintOrder = 999;
                            }
                        }
                    }

                    foreach (var item in SelectedRequestItem.RequestResultLinks.OrderBy(p => p.PrintOrder))
                    {
                        string parameterName = item.ResultItemName +
                            (!string.IsNullOrEmpty(item.Unit) ? " ( " + item.Unit + " )" : "");
                        ColumnsResultItems.Add(new Column()
                        {
                            Header = parameterName,
                            FieldName = parameterName,
                            VisibleIndex = visibleIndex++,
                            Tag = item
                        });

                        tempDt.Columns.Add(parameterName);
                    }

                }
            }
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

        private RelayCommand _ImportRiskCommand;
        public RelayCommand ImportRiskCommand
        {
            get
            {
                return _ImportRiskCommand
                    ?? (_ImportRiskCommand = new RelayCommand(ImportRiskFile));
            }
        }

        private RelayCommand _ImportInjuryCommand;
        public RelayCommand ImportInjuryCommand
        {
            get
            {
                return _ImportInjuryCommand
                    ?? (_ImportInjuryCommand = new RelayCommand(ImportInjuryFile));
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

        private RelayCommand _SaveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return _SaveCommand
                    ?? (_SaveCommand = new RelayCommand(SaveLabResult));
            }
        }

        private RelayCommand _ExportInjuryCommand;
        public RelayCommand ExportInjuryCommand
        {
            get
            {
                return _ExportInjuryCommand
                    ?? (_ExportInjuryCommand = new RelayCommand(ExportInjury));
            }
        }

        private RelayCommand _ExportRiskCommand;
        public RelayCommand ExportRiskCommand
        {
            get
            {
                return _ExportRiskCommand
                    ?? (_ExportRiskCommand = new RelayCommand(ExportRisk));
            }
        }

        private RelayCommand _SaveRiskCommand;
        public RelayCommand SaveRiskCommand
        {
            get
            {
                return _SaveRiskCommand
                    ?? (_SaveRiskCommand = new RelayCommand(SaveRiskResult));
            }
        }

        private RelayCommand _SaveInjuryCommand;
        public RelayCommand SaveInjuryCommand
        {
            get
            {
                return _SaveInjuryCommand
                    ?? (_SaveInjuryCommand = new RelayCommand(SaveInjuryResult));
            }
        }

        #endregion


        #region method
        DataTable tempDt;
        public ImportOldResultViewModel()
        {
            var year = DateTime.Now.Year;
            YearSource = new List<string>();

            if (year != 0)
            {
                for (int i = 1; i <= 10; i++)
                {
                    int item = year-i;
                    YearSource.Add(item.ToString());
                }
            }
            

            RequestItems = DataService.MasterData.GetRequestItemByCategory("Lab", true);
            RequestItems = RequestItems?
                .Where(p => p.RequestResultLinks.Count() > 0)
                .Where(p => p.RequestResultLinks.FirstOrDefault(s => s.ResultValueType == "Image") == null).OrderBy(p => p.ItemName).ToList();
            
            Organisations = GetHealthOrganisationRole();
            SelectOrganisation = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == AppUtil.Current.OwnerOrganisationUID);

            if (SelectOrganisation != null)
            {
                var loct = DataService.MasterData.GetLocationByOrganisationUID(SelectOrganisation.HealthOrganisationUID);
                Location = loct.Where(p => p.IsRegistrationAllowed == "Y").ToList();
            }

            SelectLocation = Location.FirstOrDefault(p => p.LocationUID == AppUtil.Current.LocationUID);
            InsuranceCompany = DataService.Billing.GetInsuranceCompanyAll();
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
            if (string.IsNullOrEmpty(FileLocation))
            {
                WarningDialog("กรุณาเลือกไฟล์ที่จะนำเข้า");
                return;
            }
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
            string hn = string.Empty;
            ImportOldResult view = (ImportOldResult)this.View;
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
                                OleDbCommand objCmdSelect = new OleDbCommand("SELECT * FROM [" + FileName + "]   Where ( ([NO] <> '' OR [NO] <> '0' OR [NO] IS NOT NULL)) ", conn);
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
                    tempDt.Clear();
                    int upperlimit = ImportData.AsEnumerable().Count(p => !string.IsNullOrEmpty(p["NO"].ToString()) && p["NO"].ToString() != "NO");
                    view.SetProgressBarLimits(0, upperlimit);
                    if (upperlimit > 0)
                    {
                        //if (!ColumnsResultItems.Any(p => p.Header == "HN"))
                        //{
                        //    //ColumnsResultItems.Add(new Column() { Header = "PatientName", FieldName = "PatientName", VisibleIndex = 1 });
                        //    //ColumnsResultItems.Add(new Column() { Header = "Gender", FieldName = "Gender", VisibleIndex = 2 });
                        //    //ColumnsResultItems.Add(new Column() { Header = "LabNumber", FieldName = "LabNumber", VisibleIndex = 3 });
                        //    //ColumnsResultItems.Add(new Column() { Header = "OrderStatus", FieldName = "OrderStatus", VisibleIndex = 4 });
                        //    //ColumnsResultItems.Add(new Column() { Header = "NO", FieldName = "NO", VisibleIndex = 1 });
                        //    //ColumnsResultItems.Add(new Column() { Header = "HN", FieldName = "HN", VisibleIndex = 2 });
                        //    //ColumnsResultItems.Add(new Column() { Header = "HN", FieldName = "HN", VisibleIndex = 1 });
                        //    //ColumnsResultItems.Add(new Column() { Header = "PatientVisitUID", FieldName = "PatientVisitUID", Visible = false });
                        //    //ColumnsResultItems.Add(new Column() { Header = "RequestUID", FieldName = "RequestUID", Visible = false });
                        //    //ColumnsResultItems.Add(new Column() { Header = "RequestDetailUID", FieldName = "RequestDetailUID", Visible = false });
                        //    //ColumnsResultItems.Add(new Column() { Header = "SEXXXUID", FieldName = "SEXXXUID", Visible = false });
                        //}
                        ColumnsResultItems.Add(new Column() { Header = "PatientID", FieldName = "PatientID", VisibleIndex = 1 });
                        view.gcTestParameter.ItemsSource = tempDt.DefaultView;

                        foreach (DataRow item in ImportData.Rows)
                        {
                            PatientInformationModel patient = new PatientInformationModel();
                            string patientID = item["HN"].ToString();
                            string firstname = item["FirstName"].ToString();
                            string lastname = item["LastName"].ToString();
                            string employeeId = item["EmployeeID"].ToString();
                            hn = patientID;
                            bool isPatientDuplicate = false;

                            view.gvTestParameter.AddNewRow();
                            int newRowHandle = DataControlBase.NewItemRowHandle;

                            if (!string.IsNullOrEmpty(patientID))
                            {
                                patient = DataService.PatientIdentity.GetPatientByHN(patientID);
                            }
                            else if (!string.IsNullOrEmpty(firstname) && !string.IsNullOrEmpty(lastname))
                            {
                                patient = null;
                                var data = DataService.PatientIdentity.GetPatientByName(firstname, lastname);
                                if(data != null && data.Count > 1 )
                                {
                                    isPatientDuplicate = true;
                                }
                                else if (data != null && data.Count == 1)
                                {
                                    patient = data.FirstOrDefault();
                                }
                                else 
                                {
                                    if(!string.IsNullOrEmpty(employeeId))
                                    {
                                        patient = DataService.PatientIdentity.GetPatientByEmployeeID(employeeId);
                                    }
                                }
                            }
                            else
                            {
                                continue;
                            }

                            if (isPatientDuplicate == true)
                            {
                                view.gcTestParameter.SetCellValue(newRowHandle, "PatientID", "ผู้ป่วยมี HN มากกว่า 1 HN");
                            }

                            if (patient != null && isPatientDuplicate == false)
                            {
                                
                                view.gcTestParameter.SetCellValue(newRowHandle, "PatientID", patient.PatientID);
                                view.gcTestParameter.SetCellValue(newRowHandle, "PatientUID", patient.PatientUID); 
                                view.gcTestParameter.SetCellValue(newRowHandle, "IsSave", "Falid");
                                //view.gcTestParameter.SetCellValue(newRowHandle, "EmployeeID", patient.EmployeeID);
                                //view.gcTestParameter.SetCellValue(newRowHandle, "FirstName", patient.FirstName);
                                //view.gcTestParameter.SetCellValue(newRowHandle, "LastName", patient.LastName);

                            }
                            else if(patient == null && isPatientDuplicate == false)
                            {
                                view.gcTestParameter.SetCellValue(newRowHandle, "PatientID", "ไม่พบข้อมูล");
                            } 
                            
                            foreach (var column in ImportData.Columns)
                            {
                                string columnName = column.ToString();


                                //Complete blood Count
                                if (SelectedRequestItem.Code == "LAB111")
                                {


                                    if (columnName == "WBC ( cells/ul )")
                                    {
                                        double wbc_cells_ul = double.Parse(item[columnName].ToString());
                                        if (wbc_cells_ul < 1000)
                                        {
                                            wbc_cells_ul = wbc_cells_ul * 1000;
                                        }
                                        view.gcTestParameter.SetCellValue(newRowHandle, columnName, wbc_cells_ul);
                                        continue;
                                    }

                                    if (columnName == "Platelets Count ( cells/mcl )")
                                    {
                                        if(!String.IsNullOrEmpty(item[columnName].ToString()))
                                        {
                                        double pla_cells_ul = double.Parse(item[columnName].ToString());
                                        if (pla_cells_ul < 100000)
                                        {
                                            pla_cells_ul = pla_cells_ul * 1000;
                                        }
                                        view.gcTestParameter.SetCellValue(newRowHandle, columnName, pla_cells_ul);
                                        continue;
                                        }
                                    }

                                }
                                view.gcTestParameter.SetCellValue(newRowHandle, columnName.Replace("#", "."), item[columnName].ToString());
                                
                            }

                            CollectionViewSource.GetDefaultView(view.gcTestParameter.ItemsSource).Refresh();
                            pgBarCounter = pgBarCounter + 1;
                            TotalRecord = pgBarCounter;
                            view.SetProgressBarValue(pgBarCounter);
                           
                        }
                    }
                }
            }
            catch (Exception er)
            {

                System.Windows.Forms.MessageBox.Show("error :  " + er.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ImportRiskFile()
        {
            if (string.IsNullOrEmpty(FileLocation))
            {
                WarningDialog("กรุณาเลือกไฟล์ที่จะนำเข้า");
                return;
            }

            OleDbConnection conn;
            OleDbCommand cmd;
            System.Data.DataTable dt;
            DataTable ImportData = new DataTable();
            DataSet objDataset1;
            string connectionString = string.Empty;
            int pgBarCounter = 0;
            string hn = string.Empty;
            ImportOldResult view = (ImportOldResult)this.View;

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
                                OleDbCommand objCmdSelect = new OleDbCommand("SELECT * FROM [" + FileName + "]   Where ( ([NO] <> '' OR [NO] <> '0' OR [NO] IS NOT NULL)) ", conn);
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
                    int upperlimit = ImportData.AsEnumerable().Count(p => !string.IsNullOrEmpty(p["HN"].ToString()) && p["HN"].ToString() != "HN");
                    view.SetProgressBarLimits(0, upperlimit);
                    if (upperlimit > 0)
                    {
                        ColumnsRiskItems.Add(new Column() { Header = "PatientUID", FieldName = "PatientUID", Visible = false });
                        ColumnsRiskItems.Add(new Column() { Header = "PatientID", FieldName = "PatientID", Visible = false });
                        ColumnsRiskItems.Add(new Column() { Header = "IsSave", FieldName = "IsSave", Visible = false });
                        tempDt.Columns.Add("PatientUID");
                        tempDt.Columns.Add("PatientID");
                        tempDt.Columns.Add("IsSave");

                        view.gcRiskHistory.ItemsSource = tempDt.DefaultView;

                        foreach (DataRow item in ImportData.Rows)
                        {
                            PatientInformationModel patient = new PatientInformationModel();
                            string patientID = item["HN"].ToString();
                            string firstname = item["FirstName"].ToString();
                            string lastname = item["LastName"].ToString();
                            string employeeId = item["EmployeeID"].ToString();
                            bool isPatientDuplicate = false;

                            view.gvRiskHistory.AddNewRow();
                            int newRowHandle = DataControlBase.NewItemRowHandle;

                            if (!string.IsNullOrEmpty(patientID))
                            {
                                patient = DataService.PatientIdentity.GetPatientByHN(patientID);

                            }
                            else if (!string.IsNullOrEmpty(firstname) && !string.IsNullOrEmpty(lastname))
                            {
                                patient = null;
                                var data = DataService.PatientIdentity.GetPatientByName(firstname, lastname);
                                if (data != null && data.Count > 1)
                                {
                                    isPatientDuplicate = true;
                                }
                                else if (data != null && data.Count == 1)
                                {
                                    patient = data.FirstOrDefault();
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(employeeId))
                                    {
                                        patient = DataService.PatientIdentity.GetPatientByEmployeeID(employeeId);
                                    }
                                }
                            }
                            else
                            {
                                continue;
                            }

                            if (isPatientDuplicate == true)
                            {
                                view.gcRiskHistory.SetCellValue(newRowHandle, "PatientID", "ผู้ป่วยมี HN มากกว่า 1 HN");
                            }

                            if (patient != null && isPatientDuplicate == false)
                            {
                                view.gcRiskHistory.SetCellValue(newRowHandle, "PatientUID", patient.PatientUID);
                                view.gcRiskHistory.SetCellValue(newRowHandle, "IsSave", "Falid");
                            }
                            else if (patient == null && isPatientDuplicate == false)
                            {
                                view.gcRiskHistory.SetCellValue(newRowHandle, "PatientID", "ไม่พบข้อมูล");
                            }

                            foreach (var column in ImportData.Columns)
                            {
                                string columnName = column.ToString();
                                view.gcRiskHistory.SetCellValue(newRowHandle, columnName, item[columnName].ToString());
                            }

                            CollectionViewSource.GetDefaultView(view.gcRiskHistory.ItemsSource).Refresh();
                            pgBarCounter = pgBarCounter + 1;
                            TotalRecord = pgBarCounter;
                            view.SetProgressBarValue(pgBarCounter);
                        }
                    }
                }
            }
            catch (Exception er)
            {
                System.Windows.Forms.MessageBox.Show("error :  " + er.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ImportInjuryFile()
        {
            if (string.IsNullOrEmpty(FileLocation))
            {
                WarningDialog("กรุณาเลือกไฟล์ที่จะนำเข้า");
                return;
            }

            OleDbConnection conn;
            OleDbCommand cmd;
            System.Data.DataTable dt;
            DataTable ImportData = new DataTable();
            DataSet objDataset1;
            string connectionString = string.Empty;
            int pgBarCounter = 0;
            string hn = string.Empty;
            ImportOldResult view = (ImportOldResult)this.View;

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
                                OleDbCommand objCmdSelect = new OleDbCommand("SELECT * FROM [" + FileName + "]   Where ( ([NO] <> '' OR [NO] <> '0' OR [NO] IS NOT NULL)) ", conn);
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
                    int upperlimit = ImportData.AsEnumerable().Count(p => !string.IsNullOrEmpty(p["HN"].ToString()) && p["HN"].ToString() != "HN");
                    view.SetProgressBarLimits(0, upperlimit);
                    if (upperlimit > 0)
                    {
                        ColumnsInjuryItems.Add(new Column() { Header = "PatientUID", FieldName = "PatientUID", Visible = false });
                        ColumnsInjuryItems.Add(new Column() { Header = "PatientID", FieldName = "PatientID", Visible = false });
                        ColumnsInjuryItems.Add(new Column() { Header = "IsSave", FieldName = "IsSave", Visible = false });
                        tempDt.Columns.Add("PatientUID");
                        tempDt.Columns.Add("PatientID");
                        tempDt.Columns.Add("IsSave");

                        view.gcInjuryHistory.ItemsSource = tempDt.DefaultView;

                        foreach (DataRow item in ImportData.Rows)
                        {
                            PatientInformationModel patient = new PatientInformationModel();
                            string patientID = item["HN"].ToString();
                            string firstname = item["FirstName"].ToString();
                            string lastname = item["LastName"].ToString();
                            string employeeId = item["EmployeeID"].ToString();
                            bool isPatientDuplicate = false;

                            view.gvInjuryHistory.AddNewRow();
                            int newRowHandle = DataControlBase.NewItemRowHandle;

                            if (!string.IsNullOrEmpty(patientID))
                            {
                                patient = DataService.PatientIdentity.GetPatientByHN(patientID);

                            }
                            else if (!string.IsNullOrEmpty(firstname) && !string.IsNullOrEmpty(lastname))
                            {
                                patient = null;
                                var data = DataService.PatientIdentity.GetPatientByName(firstname, lastname);
                                if (data != null && data.Count > 1)
                                {
                                    isPatientDuplicate = true;
                                }
                                else if (data != null && data.Count == 1)
                                {
                                    patient = data.FirstOrDefault();
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(employeeId))
                                    {
                                        patient = DataService.PatientIdentity.GetPatientByEmployeeID(employeeId);
                                    }
                                }
                            }
                            else
                            {
                                continue;
                            }

                            if (isPatientDuplicate == true)
                            {
                                view.gcInjuryHistory.SetCellValue(newRowHandle, "PatientID", "ผู้ป่วยมี HN มากกว่า 1 HN");
                            }

                            if (patient != null && isPatientDuplicate == false)
                            {
                                view.gcInjuryHistory.SetCellValue(newRowHandle, "PatientUID", patient.PatientUID);
                                view.gcInjuryHistory.SetCellValue(newRowHandle, "IsSave", "Falid");
                            }
                            else if (patient == null && isPatientDuplicate == false)
                            {
                                view.gcInjuryHistory.SetCellValue(newRowHandle, "PatientID", "ไม่พบข้อมูล");
                            }

                            foreach (var column in ImportData.Columns)
                            {
                                string columnName = column.ToString();
                                view.gcInjuryHistory.SetCellValue(newRowHandle, columnName, item[columnName].ToString());
                            }

                            CollectionViewSource.GetDefaultView(view.gcInjuryHistory.ItemsSource).Refresh();
                            pgBarCounter = pgBarCounter + 1;
                            TotalRecord = pgBarCounter;
                            view.SetProgressBarValue(pgBarCounter);
                        }
                    }
                }
            }
            catch (Exception er)
            {
                System.Windows.Forms.MessageBox.Show("error :  " + er.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Export()
        {
            string fileName = ShowSaveFileDialog("Microsoft Excel Document", "Microsoft Excel|*.xlsx");
            if (fileName != "")
            {
                ImportOldResult view = (ImportOldResult)this.View;
                view.gvTestParameter.ExportToXlsx(fileName);
                OpenFile(fileName);
            }
        }

        private void ExportRisk()
        {
            string fileName = ShowSaveFileDialog("Microsoft Excel Document", "Microsoft Excel|*.xlsx");
            if (fileName != "")
            {
                ImportOldResult view = (ImportOldResult)this.View;
                view.gvRiskHistory.ExportToXlsx(fileName);
                OpenFile(fileName);
            }
        }

        private void ExportInjury()
        {
            string fileName = ShowSaveFileDialog("Microsoft Excel Document", "Microsoft Excel|*.xlsx");
            if (fileName != "")
            {
                ImportOldResult view = (ImportOldResult)this.View;
                view.gvInjuryHistory.ExportToXlsx(fileName);
                OpenFile(fileName);
            }
        }

        private void SaveLabResult()
        {

            if (SelectInsuranceCompany == null)
            {
                WarningDialog("กรุณาเลือกรายการ Payor");
                return;
            }
            if (SelectPayorDetail == null)
            {
                WarningDialog("กรุณาเลือกบริษัท");
                return;
            }
            if (SelectYearSource == null)
            {
                WarningDialog("กรุณาเลือกรายการปี");
                return;
            }
            try
            {
                int OrganisationsUID = SelectOrganisation.HealthOrganisationUID;
                int? LocationUID = SelectLocation != null ? SelectLocation.LocationUID: (int?)null;
                string codeLab = SelectedRequestItem.Code;
                int year = Convert.ToInt32(SelectYearSource);
                int payorDetailUID = SelectPayorDetail.PayorDetailUID;
                DateTime resultDate = new DateTime(year,01, 01);

                var payorAgreements = DataService.Billing.GetAgreementByInsuranceUID(SelectInsuranceCompany.InsuranceCompanyUID);
                int payorAgreementsUID = payorAgreements.Where(p => p.Name.Contains("เงินสด")).Select(p => p.PayorAgreementUID).FirstOrDefault();
                
                if(payorAgreementsUID == 0)
                {
                    payorAgreementsUID = payorAgreements.Where(p => p.Name.Contains("วางบิล")).Select(p => p.PayorAgreementUID).FirstOrDefault();
                }

                ImportOldResult view = (ImportOldResult)this.View;
                if (view.gcTestParameter.ItemsSource == null)
                {
                    return;
                }
                DataView ResultlabDataView = (DataView)view.gcTestParameter.ItemsSource;
                int upperlimit = ResultlabDataView.ToTable().AsEnumerable().Count(p => p["PatientID"].ToString() != "ผู้ป่วยมี HN มากกว่า 1 HN" || p["PatientID"].ToString() != "ไม่พบข้อมูล");
               
                int pgBarCounter = 0;

                foreach (DataRowView rowView in ResultlabDataView)
                {
                    string hn =  rowView.Row["PatientID"].ToString();
                   
                    if (hn != "ผู้ป่วยมี HN มากกว่า 1 HN" && hn != "ไม่พบข้อมูล")
                    {
                        //ResultComponentModel resultComponent = new ResultComponentModel();
                        // resultComponent.ResultValue = rowView.Row[]

                        DataRow rowData = rowView.Row;
                        RequestDetailItemModel labResult = new RequestDetailItemModel();
                        labResult.PatientUID = long.Parse(rowData["PatientUID"].ToString());

                        //labResult.RequestUID = long.Parse(rowData["RequestUID"].ToString());
                        //labResult.RequestDetailUID = long.Parse(rowData["RequestDetailUID"].ToString());
                        //labResult.SEXXXUID = int.Parse(rowData["SEXXXUID"].ToString());

                        labResult.RequestItemCode = SelectedRequestItem.Code;
                        labResult.RequestItemName = SelectedRequestItem.ItemName;
                        labResult.RequestItemUID = SelectedRequestItem.RequestItemUID;
                        labResult.ResultComponents = new ObservableCollection<ResultComponentModel>();

                        foreach (var columnParameter in view.gcTestParameter.Columns)
                        {
                            string resultValue = rowData[columnParameter.FieldName].ToString()?.Trim();
                            if (columnParameter.Tag != null)
                            {
                                //string resultValue = rowData[columnParameter.FieldName.Replace(".", "#")].ToString()?.Trim();
                               
                                if (!string.IsNullOrEmpty(resultValue))
                                {
                                    RequestResultLinkModel resultItem = (RequestResultLinkModel)columnParameter.Tag;
                                    ResultComponentModel resultComponent = new ResultComponentModel();
                                    resultComponent.ResultItemUID = resultItem.ResultItemUID;
                                    resultComponent.ResultItemName = resultItem.ResultItemName;
                                    resultComponent.ResultItemCode = resultItem.ResultItemCode;
                                    resultComponent.ResultValueType = resultItem.ResultValueType;
                                    resultComponent.RVTYPUID = resultItem.RVTYPUID.Value;
                                    resultComponent.RSUOMUID = resultItem.RSUOMUID;
                                    resultComponent.ResultValue = resultValue;
                                    resultComponent.Comments = "Migrate Lab Result";

                                    labResult.ResultComponents.Add(resultComponent);
                                }
                            }

                        }

                        DataService.Checkup.SaveOldLabResult(labResult, labResult.PatientUID, payorDetailUID, resultDate, codeLab, AppUtil.Current.UserID, OrganisationsUID, payorAgreementsUID, LocationUID);
                        rowView.Row["IsSave"] = "Success";
                    }

                    pgBarCounter = pgBarCounter + 1;
                    TotalRecord = pgBarCounter;
                    view.SetProgressBarValue(pgBarCounter);
                }
                SaveSuccessDialog();
            }
            catch (Exception er)
            {

                System.Windows.Forms.MessageBox.Show("error : " + er.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveRiskResult()
        {
            ImportOldResult view = (ImportOldResult)this.View;
            if (view.gcRiskHistory.ItemsSource == null)
            {
                WarningDialog("กรุณานำเข้าข้อมูล");
                return;
            }
            try
            {
                DataView ResultlabDataView = (DataView)view.gcRiskHistory.ItemsSource;
                int upperlimit = ResultlabDataView.ToTable().AsEnumerable().Count(p => p["HN"].ToString() != "ผู้ป่วยมี HN มากกว่า 1 HN" || p["PatientID"].ToString() != "ไม่พบข้อมูล");

                int pgBarCounter = 0;

                foreach (DataRowView rowView in ResultlabDataView)
                {
                    string hn = rowView.Row["HN"].ToString();
                    if (hn != "ผู้ป่วยมี HN มากกว่า 1 HN" && hn != "ไม่พบข้อมูล")
                    {
                        DataRow rowData = rowView.Row;
                        PatientMedicalHistoryModel riskData = new PatientMedicalHistoryModel();
                        riskData.PatientUID = long.Parse(rowData["PatientUID"].ToString());
                        riskData.LungDisease = rowData["เคยมีประวัติเป็นโรคปอดหรือไม่"].ToString();
                        riskData.Epilepsy = rowData["เคยมีประวัติเป็นโรคลมชักหรือไม่"].ToString();
                        riskData.Unconscious = rowData["เคยมีอาการเป็นลม หมดสติ หรือวูบหรือไม่"].ToString();
                        riskData.Hypertension = rowData["เคยมีประวัติเป็นโรคหัวใจหรือความดันสูงหรือไม่"].ToString();
                        riskData.Anemia = rowData["เคยมีประวัติเป็นโรคโลหิตจางหรือไม่"].ToString();
                        riskData.ChronicDisease = rowData["ท่านมีโรคประจำตัวหรือโรคเรื้อรังหรือไม่"].ToString();
                        riskData.SurgicalDetail = rowData["ท่านเคยได้รับการผ่าตัดหรือไม่"].ToString();
                        riskData.ImmunizationDetail = rowData["ท่านเคยได้รับภูมิคุ้มกันกรณีเกิดโรคระบาดหรือโรคติดต่อหรือไม่"].ToString();
                        riskData.Familyhistory = rowData["ประวัติการเจ็บป่วยของสมาชิกในครอบครัว"].ToString();
                        riskData.LongTemMedication = rowData["ปัจจุบันมียาที่ต้องรับประทานเป็นประจำหรือไม่"].ToString();
                        riskData.AllergyDescription = rowData["มีประวัติแพ้ยาหรือไม่"].ToString();
                        riskData.Narcotic = rowData["เคยเสพยาเสพติดใด ๆ บ้างหรือไม่"].ToString();

                        var smokeData = rowData["เคยสูบบุหรี่บ้างหรือไม่"].ToString();

                        if (smokeData.Contains("วัน"))
                        {
                            var SmokeDayRange = Regex.Matches(smokeData, @"\d+[_-]+\d+");
                            var matchesSmoke = Regex.Matches(smokeData, @"\d+");
                            if (SmokeDayRange.Count > 0)
                            {
                                riskData.Smoke = SmokeDayRange[0].Value.ToString();
                            }
                            else
                            {
                                riskData.Smoke = matchesSmoke[0].Value.ToString();
                            }
                        }
                        else if (smokeData.Contains("ปี") || smokeData.Contains("เดือน"))
                        {
                            riskData.Smoke = "เคยแต่เลิกแล้ว";
                            if (smokeData.Contains("ปี"))
                            {
                                int begin = smokeData.IndexOf("ปี") - 4;
                                int end = smokeData.LastIndexOf("ปี");
                                string resultYear = smokeData.Substring(begin, end - begin + 2);

                                var SmokeYearRange = Regex.Matches(resultYear, @"\d+[_-]+\d+");
                                var matchesYearSmoke = Regex.Matches(resultYear, @"\d+");

                                if (SmokeYearRange != null || matchesYearSmoke != null)
                                {

                                    if (SmokeYearRange.Count > 0)
                                    {
                                        riskData.SmokePeriodYear = SmokeYearRange[0].Value.ToString();
                                    }
                                    else
                                    {
                                        riskData.SmokePeriodYear = matchesYearSmoke[0].Value.ToString();
                                    }
                                }
                            }

                            if (smokeData.Contains("เดือน"))
                            {
                                int begin = smokeData.IndexOf("เดือน") - 4;
                                int end = smokeData.LastIndexOf("เดือน");
                                string resultMonth = smokeData.Substring(begin, end - begin + 5);

                                var SmokeMonthRange = Regex.Matches(resultMonth, @"\d+[_-]+\d+");
                                var matchesMonthSmoke = Regex.Matches(resultMonth, @"\d+");

                                if (SmokeMonthRange != null || matchesMonthSmoke != null)
                                {
                                    if (SmokeMonthRange.Count > 0)
                                    {
                                        riskData.SmokePeriodMonth = SmokeMonthRange[0].Value.ToString();
                                    }
                                    else
                                    {
                                        riskData.SmokePeriodMonth = matchesMonthSmoke[0].Value.ToString();
                                    }
                                }
                            }

                        }
                        else if(smokeData.Contains("ก่อนเลิก"))
                        {
                            riskData.Smoke = "เคยแต่เลิกแล้ว";
                            var SmokeDayRange = Regex.Matches(smokeData, @"\d+[_-]+\d+");
                            var matchesSmoke = Regex.Matches(smokeData, @"\d+");
                            if (SmokeDayRange.Count > 0)
                            {
                                riskData.BFQuitSmoke = SmokeDayRange[0].Value.ToString();
                            }
                            else
                            {
                                riskData.BFQuitSmoke = matchesSmoke[0].Value.ToString();
                            }
                        }
                        else
                        { 
                            riskData.Smoke = rowData["เคยสูบบุหรี่บ้างหรือไม่"].ToString();
                        }

                        var alcoholData = rowData["ดื่มสุรา เบียร์ หรือเครื่องดื่มที่มีแอลกอฮอล์บ้างหรือไม่"].ToString();

                        if (alcoholData.Contains("ปี") || alcoholData.Contains("เดือน"))
                        {
                            riskData.Alcohol = "เคยแต่เลิกแล้ว";

                            if (alcoholData.Contains("ปี"))
                            {
                                int begin = alcoholData.IndexOf("ปี") - 4;
                                int end = alcoholData.LastIndexOf("ปี");
                                string resultYear = alcoholData.Substring(begin, end - begin + 2);
                            
                                var AlcohoDayRange = Regex.Matches(resultYear, @"\d+[_-]+\d+");
                                var matchesAlcoho = Regex.Matches(resultYear, @"\d+");
                                if (AlcohoDayRange.Count > 0)
                                {
                                    riskData.AlcohoPeriodYear = AlcohoDayRange[0].Value.ToString();
                                }
                                else
                                {
                                    riskData.AlcohoPeriodYear = matchesAlcoho[0].Value.ToString();
                                }
                            }

                            if (alcoholData.Contains("เดือน"))
                            {
                                int begin = alcoholData.IndexOf("เดือน") - 4;
                                int end = alcoholData.LastIndexOf("เดือน");
                                string resultMonth = alcoholData.Substring(begin, end - begin + 5);

                                var AlcohoMonthRange = Regex.Matches(resultMonth, @"\d+[_-]+\d+");
                                var matchesAlcoho = Regex.Matches(resultMonth, @"\d+");

                                if (AlcohoMonthRange.Count > 0)
                                {
                                    riskData.AlcohoPeriodMonth = AlcohoMonthRange[0].Value.ToString();
                                }
                                else
                                {
                                    riskData.AlcohoPeriodMonth = matchesAlcoho[0].Value.ToString();
                                }
                            }
                        }
                        else
                        {
                            riskData.Alcohol = rowData["ดื่มสุรา เบียร์ หรือเครื่องดื่มที่มีแอลกอฮอล์บ้างหรือไม่"].ToString();
                        }

                        riskData.Comments = "migrate risk history";

                        var pastHistory = rowData["โรคอื่นๆ"].ToString();
                        if(pastHistory != "")
                        {
                            PastMedicalHistoryModel dataPast = new PastMedicalHistoryModel();
                            dataPast.MedicalName = pastHistory;
                            riskData.PastMedicalHistorys = new List<PastMedicalHistoryModel> { dataPast };
                        }


                        DataService.Checkup.SaveRiskHistory(riskData, riskData.PatientUID,AppUtil.Current.UserID);
                        rowView.Row["IsSave"] = "Success";

                     }
                
                    pgBarCounter = pgBarCounter + 1;
                    TotalRecord = pgBarCounter;
                    view.SetProgressBarValue(pgBarCounter);
                }

                SaveSuccessDialog();
            }
            catch (Exception er)
            {

                System.Windows.Forms.MessageBox.Show("error : " + er.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveInjuryResult()
        {
            ImportOldResult view = (ImportOldResult)this.View;
            if (view.gcInjuryHistory.ItemsSource == null)
            {
                WarningDialog("กรุณานำเข้าข้อมูล");
                return;
            }
            try
            {
                var levelDetail = DataService.Technical.GetReferenceValueList("INRYSEV");
                DataView ResultlabDataView = (DataView)view.gcInjuryHistory.ItemsSource;
                int upperlimit = ResultlabDataView.ToTable().AsEnumerable().Count(p => p["HN"].ToString() != "ผู้ป่วยมี HN มากกว่า 1 HN" || p["PatientID"].ToString() != "ไม่พบข้อมูล");

                int pgBarCounter = 0;

                foreach (DataRowView rowView in ResultlabDataView)
                {
                    string hn = rowView.Row["HN"].ToString();
                    if (hn != "ผู้ป่วยมี HN มากกว่า 1 HN" && hn != "ไม่พบข้อมูล")
                    {
                        DataRow rowData = rowView.Row;
                        PatientInjuryModel injury = new PatientInjuryModel();

                        injury.PatientUID = long.Parse(rowData["PatientUID"].ToString());
                        injury.InjuryDetail = rowData["สาเหตุของการบาดเจ็บหรือการเจ็บป่วย"].ToString();
                        injury.BodyLocation = rowData["ส่วนของร่างกายที่บาดเจ็บ"].ToString();
                        injury.Comments = "migrate injury history";

                        if (injury.BodyLocation != "-" && !injury.BodyLocation.Contains("ไม่มี") && !injury.BodyLocation.Contains("ไม่เคย") && !injury.BodyLocation.Contains("ไม่"))
                        {
                            var level = rowData["ระดับความรุนแรง"].ToString();

                            if (level.Contains("ทำงานไม่ได้ชั่วคราว"))
                            {
                                injury.INRYSEVUID = levelDetail.Where(p => p.ValueCode == "INRYSEV5").FirstOrDefault().Key;
                            }
                            else if (level.Contains("หยุดงานเกิน 3 วัน"))
                            {
                                injury.INRYSEVUID = levelDetail.Where(p => p.ValueCode == "INRYSEV3").FirstOrDefault().Key;
                            }
                            else if (level.Contains("หยุดงานไม่เกิน 3 วัน"))
                            {
                                injury.INRYSEVUID = levelDetail.Where(p => p.ValueCode == "INRYSEV4").FirstOrDefault().Key;
                            }
                            else if (level.Contains("ทุพพลภาพ"))
                            {
                                injury.INRYSEVUID = levelDetail.Where(p => p.ValueCode == "INRYSEV1").FirstOrDefault().Key;
                            }
                            else if (level.Contains("สูยเสียอวัยวะบางส่วน"))
                            {
                                injury.INRYSEVUID = levelDetail.Where(p => p.ValueCode == "INRYSEV2").FirstOrDefault().Key;
                            }
                        
                            DataService.Checkup.SaveInjuryHistory(injury, injury.PatientUID, AppUtil.Current.UserID);
                        }

                        rowView.Row["IsSave"] = "Success";
                       
                    }
                    pgBarCounter = pgBarCounter + 1;
                    TotalRecord = pgBarCounter;
                    view.SetProgressBarValue(pgBarCounter);
                }
                SaveSuccessDialog();
            }
            catch (Exception er)
            {

                System.Windows.Forms.MessageBox.Show("error : " + er.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private RequestDetailItemModel GetResultLab(DataRow rowData, List<ResultItemRangeModel> resultItemRange)
        {
            RequestDetailItemModel labResult = new RequestDetailItemModel();
            labResult.PatientUID = long.Parse(rowData["PatientUID"].ToString());
            //labResult.PatientVisitUID = long.Parse(rowData["PatientVisitUID"].ToString());
            //labResult.RequestUID = long.Parse(rowData["RequestUID"].ToString());
            //labResult.RequestDetailUID = long.Parse(rowData["RequestDetailUID"].ToString());
            //labResult.SEXXXUID = int.Parse(rowData["SEXXXUID"].ToString());
            labResult.RequestItemCode = SelectedRequestItem.Code;
            labResult.RequestItemName = SelectedRequestItem.ItemName;

            ImportLabResult view = (ImportLabResult)this.View;
            labResult.ResultComponents = new ObservableCollection<ResultComponentModel>();
            foreach (var columnParameter in view.gcTestParameter.Columns)
            {
                if (columnParameter.Tag != null)
                {
                    //string resultValue = rowData[columnParameter.FieldName.Replace(".", "#")].ToString()?.Trim();
                    string resultValue = rowData[columnParameter.FieldName].ToString()?.Trim();
                    if (!string.IsNullOrEmpty(resultValue))
                    {
                        RequestResultLinkModel resultItem = (RequestResultLinkModel)columnParameter.Tag;
                        ResultComponentModel resultComponent = new ResultComponentModel();
                        resultComponent.ResultItemUID = resultItem.ResultItemUID;
                        resultComponent.ResultItemName = resultItem.ResultItemName;
                        resultComponent.ResultItemCode = resultItem.ResultItemCode;
                        resultComponent.ResultValueType = resultItem.ResultValueType;
                        resultComponent.RVTYPUID = resultItem.RVTYPUID.Value;
                        resultComponent.RSUOMUID = resultItem.RSUOMUID;
                        resultComponent.ResultValue = resultValue;
                        resultComponent.Comments = "From Excel";
                    }
                    //labResult.ResultComponents.Add(resultComponent);
                }
            }

            return labResult;
        }

            #endregion

    }
}
