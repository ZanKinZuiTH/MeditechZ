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

namespace MediTech.ViewModels
{
    public class ManageItemIssueViewModel : MediTechViewModelBase
    {

        #region Properties

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

        private double _NetAmount;

        public double NetAmount
        {
            get { return _NetAmount; }
            set { Set(ref _NetAmount, value); }
        }

        private double _OtherChages;

        public double OtherChages
        {
            get { return _OtherChages; }
            set { Set(ref _OtherChages, value); }
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
                ItemMasters = null;
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


        #endregion

        #region Command


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


        private RelayCommand<DevExpress.Xpf.Grid.CellValueChangedEventArgs> _CellChangeValueCommand;
        public RelayCommand<DevExpress.Xpf.Grid.CellValueChangedEventArgs> CellChangeValueCommand
        {
            get { return _CellChangeValueCommand ?? (_CellChangeValueCommand = new RelayCommand<DevExpress.Xpf.Grid.CellValueChangedEventArgs>(CellChangeValue)); }
        }
        #endregion

        #region Varible

        List<HealthOrganisationModel> HealthOrganisations;

        #endregion


        #region Method
        public ItemIssueModel model;

        public ManageItemIssueViewModel()
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

        private void Save()
        {
            try
            {
                if (SelectOrganisationFrom == null)
                {
                    WarningDialog("กรุณาระบุ สถานประกอบการที่ส่งออก");
                    return;
                }

                if (SelectOrganisationTo == null)
                {
                    WarningDialog("กรุณาระบุ Store ทีส่งออก");
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
                    WarningDialog("กรุณาใส่วันที่ ส่งออก");
                    return;
                }

                if (ItemIssueDetail == null || ItemIssueDetail.Count <= 0)
                {
                    WarningDialog("กรุณาใส่รายการของที่ ส่งออก");
                    return;
                }

                AssignPropertiesToModel();
                DataService.Inventory.ManageItemIssue(model, AppUtil.Current.UserID);
                SaveSuccessDialog();

                ListItemIssue view = new ListItemIssue();
                ChangeView_CloseViewDialog(view, ActionDialog.Save);
            }
            catch (Exception er)
            {
                ErrorDialog(er.Message);
            }
        }

        private void Cancel()
        {
            ListItemIssue view = new ListItemIssue();
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

                    CellChangeValue(null);
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
            model.NetAmount = NetAmount;
            model.OtherCharges = OtherChages;
            foreach (var item in ItemIssueDetail)
            {
                ItemIssueDetailModel newRow = new ItemIssueDetailModel();
                //newRow.ItemIssueDetailUID = item.ItemListUID;
                newRow.ItemMasterUID = item.ItemMasterUID;
                newRow.ItemCode = item.ItemCode;
                newRow.ItemName = item.ItemName;
                newRow.BatchID = item.BatchID;
                newRow.UnitPrice = item.ItemCost ?? 0;
                newRow.NetAmount = item.NetCost;
                newRow.ItemCost = item.ItemCost ?? 0;
                newRow.Quantity = item.Quantity ?? 0;
                newRow.IMUOMUID = item.IMUOMUID;
                newRow.ExpiryDttm = item.ExpiryDttm;
                newRow.StockUID = item.StockUID;
                model.ItemIssueDetail.Add(newRow);
            }
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
            NetAmount = model.NetAmount;
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
                newRow.Quantity = item.Quantity;
                newRow.BatchQuantity = ItemMasters.FirstOrDefault(p => p.StockUID == item.StockUID).BatchQty;
                newRow.IMUOMUID = item.IMUOMUID;
                newRow.ExpiryDttm = item.ExpiryDttm;
                newRow.StockUID = item.StockUID;
                newRow.NetAmount = item.NetAmount;
                newRow.UnitPrice = item.ItemCost;
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
                    //newRow.ItemListUID = item.ItemRequestDetailUID;
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

        public void CellChangeValue(DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            NetAmount = 0;
            foreach (var item in ItemIssueDetail)
            {
                NetAmount += item.NetCost;
            }
        }

        #endregion
    }
}
