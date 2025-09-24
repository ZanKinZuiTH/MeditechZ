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
    public class ImportOccMedResultViewModel : MediTechViewModelBase
    {
        #region Properties
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
                    IsEditDate = false;
                    DateFrom = DateTime.Now.AddDays(-SelectDateType.Key.Value);
                    DateTo = DateTime.Now;
                    IsEditDate = true;
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

        private List<InsuranceCompanyModel> _InsuranceCompanyDetails;

        public List<InsuranceCompanyModel> InsuranceCompanyDetails
        {
            get { return _InsuranceCompanyDetails; }
            set { Set(ref _InsuranceCompanyDetails, value); }
        }

        private InsuranceCompanyModel _SelectInsuranceCompanyDetails;

        public InsuranceCompanyModel SelectInsuranceCompanyDetails
        {
            get { return _SelectInsuranceCompanyDetails; }
            set { Set(ref _SelectInsuranceCompanyDetails, value); }
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
                if (SelectedRequestItem.RequestResultLinks != null)
                {
                    ImportOccMedResult view = (ImportOccMedResult)this.View;
                    view.gcTestParameter.ItemsSource = null;
                    ColumnsResultItems = new ObservableCollection<Column>();
                    ColumnsResultItems.Add(new Column() { Header = "HN", FieldName = "HN", VisibleIndex = 0 });

                    int visibleIndex = 4;
                    foreach (var item in SelectedRequestItem.RequestResultLinks.OrderBy(p => p.PrintOrder))
                    {
                        string parameterName = item.ResultItemName +
                            (!string.IsNullOrEmpty(item.Unit) ? " ( " + item.Unit + " )" : "");
                        ColumnsResultItems.Add(new Column()
                        {
                            Header = parameterName,
                            FieldName = parameterName.Replace(".", "#"),
                            VisibleIndex = visibleIndex++,
                            Tag = item
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


        private RelayCommand _SaveCommand;

        public RelayCommand SaveCommand
        {
            get
            {
                return _SaveCommand
                    ?? (_SaveCommand = new RelayCommand(SaveLabResult));
            }
        }
        #endregion

        #region Method

        public ImportOccMedResultViewModel()
        {
            RequestItems = DataService.MasterData.GetRequestItemByCategory("Mobile Checkup", true);
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

            InsuranceCompanyDetails = DataService.Billing.GetInsuranceCompanyAll();
            DateTypes = new List<LookupItemModel>();
            DateTypes.Add(new LookupItemModel { Key = 30, Display = "30 วัน" });
            DateTypes.Add(new LookupItemModel { Key = 60, Display = "60 วัน" });
            DateTypes.Add(new LookupItemModel { Key = 90, Display = "90 วัน" });
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
            if (string.IsNullOrEmpty(FileLocation))
            {
                WarningDialog("กรุณาเลือกไฟล์ที่จะนำเข้า");
                return;
            }
            if (SelectedRequestItem == null)
            {
                WarningDialog("กรุณาเลือกรายการ");
                return;
            }
            if (DateFrom == null)
            {
                WarningDialog("กรุณาเลือก วันที่");
                return;
            }
           
            if (SelectInsuranceCompanyDetails == null)
            {
                WarningDialog("กรุณาเลือก Payor");
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
            ImportOccMedResult view = (ImportOccMedResult)this.View;
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
                    tempDt.Columns.Add("PatientName");
                    tempDt.Columns.Add("Gender");
                    tempDt.Columns.Add("LabNumber");
                    tempDt.Columns.Add("OrderStatus");
                    tempDt.Columns.Add("PatientUID");
                    tempDt.Columns.Add("PatientVisitUID");
                    tempDt.Columns.Add("RequestUID");
                    tempDt.Columns.Add("RequestDetailUID");
                    tempDt.Columns.Add("SEXXXUID");

                    int upperlimit = ImportData.AsEnumerable().Count(p => !string.IsNullOrEmpty(p["HN"].ToString()) && p["HN"].ToString() != "0");
                    view.SetProgressBarLimits(0, upperlimit);
                    if (upperlimit > 0)
                    {
                        int ownerOrganisationUID = AppUtil.Current.OwnerOrganisationUID;
                        int insuranceCompanyUID = SelectInsuranceCompanyDetails.InsuranceCompanyUID;
                        int requestItemUID = SelectedRequestItem.RequestItemUID;
                        int? locationUID = SelectLocation != null ? SelectLocation.LocationUID : (int?)null;
                        if (!ColumnsResultItems.Any(p => p.Header == "PatientName"))
                        {
                            ColumnsResultItems.Add(new Column() { Header = "PatientName", FieldName = "PatientName", VisibleIndex = 1 });
                            ColumnsResultItems.Add(new Column() { Header = "Gender", FieldName = "Gender", VisibleIndex = 2 });
                            ColumnsResultItems.Add(new Column() { Header = "LabNumber", FieldName = "LabNumber", VisibleIndex = 3 });
                            ColumnsResultItems.Add(new Column() { Header = "OrderStatus", FieldName = "OrderStatus", VisibleIndex = 4 });
                            ColumnsResultItems.Add(new Column() { Header = "PatientUID", FieldName = "PatientUID", Visible = false });
                            ColumnsResultItems.Add(new Column() { Header = "PatientVisitUID", FieldName = "PatientVisitUID", Visible = false });
                            ColumnsResultItems.Add(new Column() { Header = "RequestUID", FieldName = "RequestUID", Visible = false });
                            ColumnsResultItems.Add(new Column() { Header = "RequestDetailUID", FieldName = "RequestDetailUID", Visible = false });
                            ColumnsResultItems.Add(new Column() { Header = "SEXXXUID", FieldName = "SEXXXUID", Visible = false });
                        }
                        view.gcTestParameter.ItemsSource = tempDt.DefaultView;
                        foreach (DataRow item in ImportData.Rows)
                        {
                            string patientID = item["HN"].ToString();
                            hn = patientID;
                            if (string.IsNullOrEmpty(patientID) || patientID == "0")
                            {
                                continue;
                            }

                            var dataPatientRequest = DataService.Lab.GetRequesDetailLabForImport(patientID, ownerOrganisationUID
                                , insuranceCompanyUID, locationUID, requestItemUID, DateFrom, DateTo);
                            view.gvTestParameter.AddNewRow();
                            int newRowHandle = DataControlBase.NewItemRowHandle;

                            if (dataPatientRequest != null)
                            {
                                view.gcTestParameter.SetCellValue(newRowHandle, "PatientName", dataPatientRequest.PatientName);
                                view.gcTestParameter.SetCellValue(newRowHandle, "Gender", dataPatientRequest.Gender);
                                view.gcTestParameter.SetCellValue(newRowHandle, "LabNumber", dataPatientRequest.RequestNumber);
                                view.gcTestParameter.SetCellValue(newRowHandle, "OrderStatus", dataPatientRequest.OrderStatus);
                                view.gcTestParameter.SetCellValue(newRowHandle, "PatientUID", dataPatientRequest.PatientUID);
                                view.gcTestParameter.SetCellValue(newRowHandle, "PatientVisitUID", dataPatientRequest.PatientVisitUID);
                                view.gcTestParameter.SetCellValue(newRowHandle, "RequestUID", dataPatientRequest.RequestUID);
                                view.gcTestParameter.SetCellValue(newRowHandle, "RequestDetailUID", dataPatientRequest.RequestDetailUID);
                                view.gcTestParameter.SetCellValue(newRowHandle, "SEXXXUID", dataPatientRequest.SEXXXUID);
                            }
                            else
                            {
                                view.gcTestParameter.SetCellValue(newRowHandle, "PatientName", "ไม่พบข้อมูล");
                            }
                            foreach (var column in ImportData.Columns)
                            {
                                string columnName = column.ToString();

                                view.gcTestParameter.SetCellValue(newRowHandle, columnName, item[columnName].ToString());

                            }
                            pgBarCounter = pgBarCounter + 1;
                            TotalRecord = pgBarCounter;
                            view.SetProgressBarValue(pgBarCounter);
                        }

                    }

                }
            }
            catch (Exception er)
            {

                System.Windows.Forms.MessageBox.Show("HN : " + hn + " : " + er.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Export()
        {
            string fileName = ShowSaveFileDialog("Microsoft Excel Document", "Microsoft Excel|*.xlsx");
            if (fileName != "")
            {
                ImportOccMedResult view = (ImportOccMedResult)this.View;
                view.gvTestParameter.ExportToXlsx(fileName);
                OpenFile(fileName);
            }
        }

        private void SaveLabResult()
        {
            string hn = string.Empty;
            try
            {
                ImportOccMedResult view = (ImportOccMedResult)this.View;
                if (view.gcTestParameter.ItemsSource == null)
                {
                    return;
                }
                DataView ResultlabDataView = (DataView)view.gcTestParameter.ItemsSource;
                int upperlimit = ResultlabDataView.ToTable().AsEnumerable().Count(p => p["PatientName"].ToString() != "ไม่พบข้อมูล");
                view.SetProgressBarLimits(0, upperlimit);
                int pgBarCounter = 0;
                var resultItemRange = DataService.Lab.GetResultItemRangeByRequestItemUID(SelectedRequestItem.RequestItemUID);
                foreach (DataRowView rowView in ResultlabDataView)
                {
                    if (rowView.Row["PatientName"].ToString() != "ไม่พบข้อมูล")
                    {
                        hn = rowView.Row["HN"].ToString();
                        RequestDetailItemModel labResult = GetResultLab(rowView.Row, resultItemRange);
                        if (labResult.ResultComponents.Count() > 0)
                        {
                            List<RequestDetailItemModel> sendLabResult = new List<RequestDetailItemModel>();
                            sendLabResult.Add(labResult);
                            DataService.Lab.ReviewLabResult(sendLabResult, AppUtil.Current.UserID, AppUtil.Current.UserID);
                        }

                        pgBarCounter = pgBarCounter + 1;
                        TotalRecord = pgBarCounter;
                        view.SetProgressBarValue(pgBarCounter);
                    }
                }
                SaveSuccessDialog();
            }
            catch (Exception er)
            {

                System.Windows.Forms.MessageBox.Show("HN : " + hn + " : " + er.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private RequestDetailItemModel GetResultLab(DataRow rowData, List<ResultItemRangeModel> resultItemRange)
        {

            RequestDetailItemModel labResult = new RequestDetailItemModel();
            labResult.PatientUID = long.Parse(rowData["PatientUID"].ToString());
            labResult.PatientVisitUID = long.Parse(rowData["PatientVisitUID"].ToString());
            labResult.RequestUID = long.Parse(rowData["RequestUID"].ToString());
            labResult.RequestDetailUID = long.Parse(rowData["RequestDetailUID"].ToString());
            labResult.SEXXXUID = int.Parse(rowData["SEXXXUID"].ToString());
            labResult.RequestItemCode = SelectedRequestItem.Code;
            labResult.RequestItemName = SelectedRequestItem.ItemName;

            ImportOccMedResult view = (ImportOccMedResult)this.View;
            labResult.ResultComponents = new ObservableCollection<ResultComponentModel>();
            foreach (var columnParameter in view.gcTestParameter.Columns)
            {
                if (columnParameter.Tag != null)
                {
                    string resultValue = rowData[columnParameter.FieldName.Replace(".", "#")].ToString()?.Trim();
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


                        var itemRange = resultItemRange.FirstOrDefault(p => p.ResultItemUID == resultComponent.ResultItemUID && (p.SEXXXUID == 3));
                        if (itemRange != null)
                        {
                            if (resultComponent.ResultValueType == "Numeric")
                            {
                                resultComponent.Low = itemRange.Low;
                                resultComponent.High = itemRange.High;
                            }
                            else if (resultComponent.ResultValueType == "Free Text Field")
                            {
                                resultComponent.ReferenceRange = itemRange.DisplayValue;
                            }
                        }
                        else
                        {
                            var itemRangeGender = resultItemRange.FirstOrDefault(p => p.ResultItemUID == resultComponent.ResultItemUID && (p.SEXXXUID == labResult.SEXXXUID));
                            if (itemRangeGender != null)
                            {
                                if (resultComponent.ResultValueType == "Numeric")
                                {
                                    resultComponent.Low = itemRangeGender.Low;
                                    resultComponent.High = itemRangeGender.High;
                                }
                                else if (resultComponent.ResultValueType == "Free Text Field")
                                {
                                    resultComponent.ReferenceRange = itemRangeGender.DisplayValue;
                                }
                            }

                        }



                        labResult.ResultComponents.Add(resultComponent);
                    }
                }
            }

            return labResult;
        }


        #endregion
    }
}
