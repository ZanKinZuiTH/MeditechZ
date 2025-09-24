using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediTech.Model;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using MediTech.DataService;
using MediTech.Views;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using MediTech.Models;
using System.Data.OleDb;
using System.Data;
using System.Globalization;

namespace MediTech.ViewModels
{
  public class EcountTranferViewModel : MediTechViewModelBase
    {


        #region Properties


        private string _FileLocation;

        public string FileLocation
        {
            get { return _FileLocation; }
            set { Set(ref _FileLocation, value); }
        }



        private DateTime? _IssueDate;

        public DateTime? IssueDate
        {
            get { return _IssueDate; }
            set { Set(ref _IssueDate, value); }
        }

        private string _RequestNo;

        public string RequestNo
        {
            get { return _RequestNo; }
            set { Set(ref _RequestNo, value); }
        }

        public int? ItemRequestUID { get; set; }

        public List<HealthOrganisationModel> OrganisationsFrom { get; set; }
        private HealthOrganisationModel _SelectOrganisationFrom;

        public HealthOrganisationModel SelectOrganisationFrom
        {
            get { return _SelectOrganisationFrom; }
            set
            {
                Set(ref _SelectOrganisationFrom, value);
                if (_SelectOrganisationFrom != null)
                {
                    StoresFrom = DataService.Inventory.GetStoreByOrganisationUID(SelectOrganisationFrom.HealthOrganisationUID);
                    OrganisationsTo = HealthOrganisations.Where(p => p.HealthOrganisationUID != SelectOrganisationFrom.HealthOrganisationUID).ToList();
                }
                else
                {
                    StoresFrom = null;
                }
            }
        }

        private List<StoreModel> _StoresFrom;

        public List<StoreModel> StoresFrom
        {
            get { return _StoresFrom; }
            set { Set(ref _StoresFrom, value); }
        }




        private List<ItemMasterModel> _ItemMasters;

        public List<ItemMasterModel> ItemMasters
        {
            get { return _ItemMasters; }
            set { Set(ref _ItemMasters, value); }
        }

        private StoreModel _SelectStoreFrom;
        public StoreModel SelectStoreFrom
        {
            get { return _SelectStoreFrom; }
            set
            {
                Set(ref _SelectStoreFrom, value);
                if (SelectStoreFrom != null)
                {
                    ItemMasters = DataService.Inventory.GetItemMasterForIssue(SelectOrganisationFrom.HealthOrganisationUID, SelectStoreFrom.StoreUID);
                }
            }
        }

        private List<HealthOrganisationModel> _OrganisationsTo;

        public List<HealthOrganisationModel> OrganisationsTo
        {
            get { return _OrganisationsTo; }
            set { Set(ref _OrganisationsTo, value); }
        }
        private HealthOrganisationModel _SelectOrganisationTo;

        public HealthOrganisationModel SelectOrganisationTo
        {
            get { return _SelectOrganisationTo; }
            set
            {
                Set(ref _SelectOrganisationTo, value);
                if (_SelectOrganisationTo != null)
                {
                    StoresTo = DataService.Inventory.GetStoreByOrganisationUID(SelectOrganisationTo.HealthOrganisationUID);
                }
                else
                {
                    StoresTo = null;
                }
            }
        }

        private List<StoreModel> _StoresTo;

        public List<StoreModel> StoresTo
        {
            get { return _StoresTo; }
            set { Set(ref _StoresTo, value); }
        }

        private StoreModel _SelectStoreTo;

        public StoreModel SelectStoreTo
        {
            get { return _SelectStoreTo; }
            set { Set(ref _SelectStoreTo, value); }
        }

        private ObservableCollection<ItemMasterList> _ItemIssueDetail;

        public ObservableCollection<ItemMasterList> ItemIssueDetail
        {
            get { return _ItemIssueDetail; }
            set
            {
                Set(ref _ItemIssueDetail, value);
            }
        }

        private ItemMasterList _SelectItemIssueDetail;

        public ItemMasterList SelectItemIssueDetail
        {
            get { return _SelectItemIssueDetail; }
            set
            {
                Set(ref _SelectItemIssueDetail, value);
            }
        }

        private Visibility _VisibilitySearchRequest = Visibility.Visible;

        public Visibility VisibilitySearchRequest
        {
            get { return _VisibilitySearchRequest; }
            set { Set(ref _VisibilitySearchRequest, value); }
        }

        private double _OtherChages;

        public double OtherChages
        {
            get { return _OtherChages; }
            set { Set(ref _OtherChages, value); }
        }



        private ObservableCollection<ItemIssueModel> _IssuMapItems;

        public ObservableCollection<ItemIssueModel> IssuMapItems
        {
            get { return _IssuMapItems; }
            set
            {
                Set(ref _IssuMapItems, value);
            }
        }




        #endregion

        #region Command

        private RelayCommand _ImportCommand;

        public RelayCommand ImportCommand
        {
            get
            {
                return _ImportCommand
                    ?? (_ImportCommand = new RelayCommand(ImportFile));
            }
        }

        private RelayCommand _ChooseCommand;

        public RelayCommand ChooseCommand
        {
            get
            {
                return _ChooseCommand
                    ?? (_ChooseCommand = new RelayCommand(ChooseFile));
            }
        }


        private RelayCommand _SearchRequestCommand;

        public RelayCommand SearchRequestCommand
        {
            get { return _SearchRequestCommand ?? (_SearchRequestCommand = new RelayCommand(SearchRequestPopUp)); }
        }



        private RelayCommand _SaveCommand;

        public RelayCommand SaveCommand
        {
            get { return _SaveCommand ?? (_SaveCommand = new RelayCommand(Save)); }
        }

        private RelayCommand _CancelCommand;

        public RelayCommand CancelCommand
        {
            get { return _CancelCommand ?? (_CancelCommand = new RelayCommand(Cancel)); }
        }



        #endregion

        #region Varible

        List<HealthOrganisationModel> HealthOrganisations;

        #endregion


        #region Method
        public ItemIssueModel model;

        public EcountTranferViewModel()
        {
            ItemIssueDetail = new ObservableCollection<ItemMasterList>();
            HealthOrganisations = GetHealthOrganisationIsStock();
            OrganisationsFrom = GetHealthOrganisationIsRoleStock();
            OrganisationsTo = HealthOrganisations;
            IssueDate = DateTime.Now;

            if (OrganisationsFrom != null)
            {
                SelectOrganisationFrom = OrganisationsFrom.FirstOrDefault(p => p.HealthOrganisationUID == AppUtil.Current.OwnerOrganisationUID);
            }
        }

        private void ImportFile()
        {
            OleDbConnection conn;
            DataSet objDataset1;
            OleDbCommand cmd;
            DataTable dt;
            DataTable ImportData = new DataTable();
            string connectionString = string.Empty;
            int pgBarCounter = 0;
            // TotalRecord = 0;
            EcountTranfer view = (EcountTranfer)this.View;
            try
            {
                if (FileLocation.Trim() != string.Empty)
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
                                OleDbCommand objCmdSelect = new OleDbCommand("SELECT * FROM [" + FileName + "] Where ([รหัสสินค้า] <> '' OR [ลำดับ] IS NOT NULL)", conn);
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


                    foreach (DataRow drow in ImportData.Rows)
                    {
                        //ItemMasterModel itemModel = GetItemByCode(drow["รหัสสินค้า"].ToString().Trim());
                        var itemcodefile = drow["รหัสสินค้า"].ToString().Trim();
                        var serialcodefiel = drow["หมายเลข Serial/Lot"].ToString().Trim();
                      //  List<EcountMassFileModel> mapItem = new List<EcountMassFileModel>();
                       // EcountMassFileModel modelmap = mapItem.FirstOrDefault();
                        ItemMasterList newRow = new ItemMasterList();
                        ItemMasterModel curentItem = GetCuerryItemInStockWithSerial(itemcodefile,serialcodefiel) != null ? GetCuerryItemInStockWithSerial(itemcodefile, serialcodefiel) : null;
                        //mapItem = DataService.Inventory.GetEcountMassFile(SelectStoreFrom.StoreUID, itemModel.ItemMasterUID, drow["หมายเลข Serial/Lot"].ToString().Trim(), null);
                        //string test = drow["หมายเลข Serial/Lot"].ToString().Trim();
                        DateTime checkupDttm;
                        if (DateTime.TryParse(drow["วันหมดอายุ"].ToString().Trim(), new CultureInfo("th-TH"), System.Globalization.DateTimeStyles.None, out checkupDttm))
                            newRow.ExpiryDttm = checkupDttm;
                        if (curentItem == null)
                        {
                            WarningDialog("ไม่มี  Item ในคลัง");
                            continue;
                        }
                        newRow.ItemMasterUID = curentItem.ItemMasterUID;
                        newRow.ItemName = curentItem.Name;
                        newRow.ItemCode = curentItem.Code;
                        newRow.ItemName = curentItem.Name;
                        newRow.Quantity = drow["จำนวน"].ToString().Trim() == "" ? 0 : double.Parse(drow["จำนวน"].ToString().Trim());
                        newRow.BatchQuantity = curentItem.BatchQty;
                        newRow.BatchID = curentItem.BatchID;
                        newRow.IMUOMUID = curentItem.IMUOMUID;
                        newRow.StockUID = curentItem.StockUID;
                        //newRow.SerialNumber = drow["หมายเลข Serial/Lot"].ToString().Trim();
                        newRow.SerialNumber = curentItem.SerialNumber;
                        newRow.ShowBatchQuantity = newRow.BatchQuantity - newRow.Quantity;
                        newRow.ItemCost = curentItem.ItemCost;
                      
                        // newRow.NetAmount = curentItem.NetAmount;
                        ItemIssueDetail.Add(newRow);
                    }
                    // model.NetAmount = model.ItemIssueDetail.Sum(p => p.NetAmount);

                }
            }

            catch (Exception er)
            {

                System.Windows.Forms.MessageBox.Show(er.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private ItemMasterModel GetCuerryItemInStock(ItemMasterModel item)
        {


            ItemMasterModel curentItem = ItemMasters
                           .Where(p => p.ItemMasterUID == item.ItemMasterUID)
                           .OrderBy(p => p.ExpiryDttm).FirstOrDefault();

           
            return curentItem;
        }


        private ItemMasterModel GetCuerryItemInStockWithSerial(string itemcode , string serialcode)
        {


            ItemMasterModel curentItem = ItemMasters
                           .Where(p => p.Code == itemcode && p.SerialNumber == serialcode)
                           .OrderBy(p => p.ExpiryDttm).FirstOrDefault();


            return curentItem;
        }

        private ItemMasterModel GetItemByCode(string code)
        {
            ItemMasterModel itemModel = DataService.Inventory.GetItemMasterByCode(code);
            return itemModel;
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


        private void Save()
        {
            try
            {
                if (SelectOrganisationFrom == null)
                {
                    WarningDialog("กรุณาระบุ สถานประกอบการที่ย้าย");
                    return;
                }

                if (SelectOrganisationTo == null)
                {
                    WarningDialog("กรุณาระบุ Store ที่ย้าย");
                    return;
                }

                if (SelectStoreFrom == null)
                {
                    WarningDialog("กรุณาระบุ สถานประกอบการที่รับ");
                    return;
                }

                if (SelectStoreTo == null)
                {
                    WarningDialog("กรุณาระบุ Store รับ");
                    return;
                }
                if (IssueDate == null)
                {
                    WarningDialog("กรุณาใส่วันที่ย้าย");
                    return;
                }

                if (ItemIssueDetail == null || ItemIssueDetail.Count <= 0)
                {
                    WarningDialog("กรุณาใส่รายการของที่ย้าย");
                    return;
                }

                AssignPropertiesToModel();
                DataService.Inventory.ManageItemTransferEcount(model, AppUtil.Current.UserID);
                SaveSuccessDialog();

                ListItemTransfer view = new ListItemTransfer();
                ChangeView_CloseViewDialog(view, ActionDialog.Save);
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }

        private void Cancel()
        {
            ListItemTransfer view = new ListItemTransfer();
            ChangeView_CloseViewDialog(view, ActionDialog.Cancel);
        }

        private void SearchRequestPopUp()
        {
            //if (ItemMasters == null)
            //{
            //    WarningDialog("กรุณาเลือก Store ที่จะส่งออก");
            //    return;
            //}
            int? organFrom = SelectOrganisationFrom != null ? SelectOrganisationFrom.HealthOrganisationUID : (int?)null;
            int? organTo = SelectOrganisationTo != null ? SelectOrganisationTo.HealthOrganisationUID : (int?)null;
            SearchRequest view = new SearchRequest(organFrom, organTo);
            SearchRequestViewModel result = (SearchRequestViewModel)LaunchViewDialog(view, "SEAREQ", false);
            if (result != null && result.ResultDialog == ActionDialog.Save)
            {
                if (result.ItemRequestDetails != null)
                {
                    ItemIssueDetail = new ObservableCollection<ItemMasterList>();
                    SelectOrganisationFrom = OrganisationsFrom.FirstOrDefault(p => p.HealthOrganisationUID == result.SelectItemRequest.RequestOnOrganistaionUID);
                    SelectStoreFrom = StoresFrom.FirstOrDefault(p => p.StoreUID == result.SelectItemRequest.RequestOnStoreUID);
                    SelectOrganisationTo = OrganisationsTo.FirstOrDefault(p => p.HealthOrganisationUID == result.SelectItemRequest.OrganisationUID);
                    SelectStoreTo = StoresTo.FirstOrDefault(p => p.StoreUID == result.SelectItemRequest.StoreUID);

                    foreach (var item in result.ItemRequestDetails)
                    {
                        ItemMasterList newRow = new ItemMasterList();
                        newRow.ItemListUID = item.ItemRequestDetailUID;
                        //newRow.ItemMasterUID = item.ItemMasterUID;
                        //newRow.ItemCode = item.ItemCode;
                        //newRow.ItemName = item.ItemName;
                        //
                        //newRow.IMUOMUID = item.IMUOMUID;

                        //ItemMasterModel currentStock = ItemMasters
                        //    .Where(p => p.ItemMasterUID == item.ItemMasterUID)
                        //    .OrderBy(p => p.ExpiryDttm).FirstOrDefault();
                        //newRow.BatchID = currentStock.BatchID;
                        //newRow.BatchQuantity = currentStock.BatchQty;
                        //newRow.ExpiryDttm = currentStock.ExpiryDttm;
                        //newRow.StockUID = currentStock.StockUID;

                        newRow.SelectItemMaster = ItemMasters
                            .Where(p => p.ItemMasterUID == item.ItemMasterUID)
                            .OrderBy(p => p.ExpiryDttm).FirstOrDefault();
                        newRow.IMUOMUID = item.IMUOMUID;
                        if (newRow.SelectItemMaster == null)
                        {
                            WarningDialog("ไม่มี " + item.ItemName + " ในคลัง");
                            continue;
                        }
                        newRow.Quantity = item.Quantity;
                        newRow.ShowBatchQuantity = newRow.BatchQuantity - newRow.Quantity;
                        ItemIssueDetail.Add(newRow);
                        if (result.SelectItemRequest != null)
                        {
                            RequestNo = result.SelectItemRequest.ItemRequestID;
                            ItemRequestUID = result.SelectItemRequest.ItemRequestUID;
                        }

                    }
                }
            }


        }

        public void AssignModel(ItemIssueModel model)
        {
            //this.model = model;
            AssignModelToProperties(model);
            VisibilitySearchRequest = Visibility.Hidden;
        }

        public void AssignPropertiesToModel()
        {
            if (model == null)
            {
                model = new ItemIssueModel();
            }
            model.IssueBy = AppUtil.Current.UserID;
            model.ItemIssueDttm = IssueDate.Value;
            model.OrganisationUID = SelectOrganisationFrom.HealthOrganisationUID;
            model.StoreUID = SelectStoreFrom.StoreUID;
            model.RequestedByOrganisationUID = SelectOrganisationTo.HealthOrganisationUID;
            model.RequestedByStoreUID = SelectStoreTo.StoreUID;
            model.ISUSTUID = 2913;
            model.ItemIssueDetail = new List<ItemIssueDetailModel>();
            model.ItemRequestUID = ItemRequestUID;
            model.ItemRequestID = RequestNo;
            model.OtherCharges = OtherChages;

            foreach (var item in ItemIssueDetail)
            {
                ItemIssueDetailModel newRow = new ItemIssueDetailModel();
                //newRow.ItemIssueDetailUID = item.ItemListUID;
                newRow.ItemMasterUID = item.ItemMasterUID;
                newRow.ItemCode = item.ItemCode;
                newRow.ItemName = item.ItemName;
                newRow.BatchID = item.BatchID;
                newRow.ItemCost = item.ItemCost ?? 0;
                item.UnitPrice = item.ItemCost ?? 0;
                newRow.UnitPrice = item.UnitPrice ?? 0;
                newRow.NetAmount = item.NetAmount;
                newRow.Quantity = item.Quantity ?? 0;
                newRow.IMUOMUID = item.IMUOMUID;
                newRow.ExpiryDttm = item.ExpiryDttm;
                newRow.StockUID = item.StockUID;
                newRow.SerialNumber = item.SerialNumber;
                model.ItemIssueDetail.Add(newRow);
            }

            model.NetAmount = model.ItemIssueDetail.Sum(p => p.NetAmount);
        }

        public DateTime ConventTheiDate(string thaidate)
        {
            System.DateTime convertdate = DateTime.Parse(thaidate);
            thaidate = convertdate.ToString("d MMMM yyyy", new System.Globalization.CultureInfo("th-TH"));

            //DateTime result = thaidate;

            return convertdate;

        }

        public void AssignModelToProperties(ItemIssueModel model)
        {
            IssueDate = model.ItemIssueDttm;
            ItemRequestUID = model.ItemRequestUID;
            RequestNo = model.ItemRequestID;
            SelectOrganisationFrom = OrganisationsFrom.FirstOrDefault(p => p.HealthOrganisationUID == model.OrganisationUID);
            SelectStoreFrom = StoresFrom.FirstOrDefault(p => p.StoreUID == model.StoreUID);
            SelectOrganisationTo = OrganisationsTo.FirstOrDefault(p => p.HealthOrganisationUID == model.RequestedByOrganisationUID);
            SelectStoreTo = StoresTo.FirstOrDefault(p => p.StoreUID == model.RequestedByStoreUID);
            OtherChages = model.OtherCharges;
            foreach (var item in model.ItemIssueDetail)
            {
                ItemMasterList newRow = new ItemMasterList();
                newRow.ItemListUID = item.ItemIssueDetailUID;
                newRow.ItemMasterUID = item.ItemMasterUID;
                newRow.ItemCode = item.ItemCode;
                newRow.ItemName = item.ItemName;
                newRow.BatchID = item.BatchID;
                newRow.ItemCost = item.ItemCost;
                newRow.UnitPrice = item.UnitPrice;
                newRow.NetAmount = item.NetAmount;
                newRow.Quantity = item.Quantity;
                newRow.BatchQuantity = ItemMasters.FirstOrDefault(p => p.StockUID == item.StockUID).BatchQty;
                newRow.IMUOMUID = item.IMUOMUID;
                newRow.ExpiryDttm = item.ExpiryDttm;
                newRow.StockUID = item.StockUID;
                newRow.SerialNumber = item.SerialNumber;
                newRow.ShowBatchQuantity = newRow.BatchQuantity - newRow.Quantity;
                ItemIssueDetail.Add(newRow);
            }
        }

        public void AssingProperitesFromRequest(int itemRequestUID)
        {
            var ItemRequest = DataService.Inventory.GetItemRequestByUID(itemRequestUID);
            if (ItemRequest != null)
            {
                ItemIssueDetail = new ObservableCollection<ItemMasterList>();
                SelectOrganisationFrom = OrganisationsFrom.FirstOrDefault(p => p.HealthOrganisationUID == ItemRequest.RequestOnOrganistaionUID);
                SelectStoreFrom = StoresFrom.FirstOrDefault(p => p.StoreUID == ItemRequest.RequestOnStoreUID);
                SelectOrganisationTo = OrganisationsTo.FirstOrDefault(p => p.HealthOrganisationUID == ItemRequest.OrganisationUID);
                SelectStoreTo = StoresTo.FirstOrDefault(p => p.StoreUID == ItemRequest.StoreUID);

                foreach (var item in ItemRequest.ItemRequestDetail)
                {
                    ItemMasterList newRow = new ItemMasterList();
                    newRow.ItemListUID = item.ItemRequestDetailUID;
                    //newRow.ItemMasterUID = item.ItemMasterUID;
                    //newRow.ItemCode = item.ItemCode;
                    //newRow.ItemName = item.ItemName;
                    //
                    //newRow.IMUOMUID = item.IMUOMUID;

                    //ItemMasterModel currentStock = ItemMasters
                    //    .Where(p => p.ItemMasterUID == item.ItemMasterUID)
                    //    .OrderBy(p => p.ExpiryDttm).FirstOrDefault();
                    //newRow.BatchID = currentStock.BatchID;
                    //newRow.BatchQuantity = currentStock.BatchQty;
                    //newRow.ExpiryDttm = currentStock.ExpiryDttm;
                    //newRow.StockUID = currentStock.StockUID;

                    newRow.SelectItemMaster = ItemMasters
                        .Where(p => p.ItemMasterUID == item.ItemMasterUID)
                        .OrderBy(p => p.ExpiryDttm).FirstOrDefault();
                    newRow.IMUOMUID = item.IMUOMUID;
                    if (newRow.SelectItemMaster == null)
                    {
                        WarningDialog("ไม่มี " + item.ItemName + " ในคลัง");
                        continue;
                    }
                    newRow.Quantity = item.Quantity;
                    newRow.ShowBatchQuantity = newRow.BatchQuantity - newRow.Quantity;
                    ItemIssueDetail.Add(newRow);
                    if (ItemRequest != null)
                    {
                        RequestNo = ItemRequest.ItemRequestID;
                        ItemRequestUID = ItemRequest.ItemRequestUID;
                    }

                }
            }
        }
        #endregion
    }
}
