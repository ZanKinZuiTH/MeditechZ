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
    public class ManageItemReceiveViewModel : MediTechViewModelBase
    {
        #region Properties

        public List<LocationModel> LocationData { get; set; }
        public List<LocationModel> LocationIssueData { get; set; }

        private DateTime? _ReceiveDate;

        public DateTime? ReceiveDate
        {
            get { return _ReceiveDate; }
            set { Set(ref _ReceiveDate, value); }
        }

        private string _IssueNo;

        public string IssueNo
        {
            get { return _IssueNo; }
            set { Set(ref _IssueNo, value); }
        }

        public int? ItemIssueUID { get; set; }

        public List<HealthOrganisationModel> Organisations { get; set; }
        private HealthOrganisationModel _SelectOrganisation;

        public HealthOrganisationModel SelectOrganisation
        {
            get { return _SelectOrganisation; }
            set
            {
                Set(ref _SelectOrganisation, value);
                if (_SelectOrganisation != null)
                {
                    LocationData = GetLocatioinRole(_SelectOrganisation.HealthOrganisationUID);
                    Locations = LocationData;

                    if (SelectLocationIssue != null)
                        Locations = LocationData?.Where(p => p.LocationUID != SelectLocationIssue.LocationUID).ToList();
                }
            }
        }

        private List<LocationModel> _Locations;

        public List<LocationModel> Locations
        {
            get { return _Locations; }
            set { Set(ref _Locations, value); }
        }

        private LocationModel _SelectLocation;

        public LocationModel SelectLocation
        {
            get { return _SelectLocation; }
            set
            {
                Set(ref _SelectLocation, value);
                Stores = null;
                if (_SelectLocation != null)
                {
                    Stores = DataService.Inventory.GetStoreByLocationUID(_SelectLocation.LocationUID);
                    LocationIssue = LocationIssueData?.Where(p => p.LocationUID != _SelectLocation.LocationUID).ToList();
                }
            }
        }

        private List<StoreModel> _Stores;

        public List<StoreModel> Stores
        {
            get { return _Stores; }
            set { Set(ref _Stores, value); }
        }

        private StoreModel _SelectStore;

        public StoreModel SelectStore
        {
            get { return _SelectStore; }
            set
            {
                Set(ref _SelectStore, value);
                if (SelectStore != null)
                {
                    ItemMasters = DataService.Inventory.GetItemMasterQtyByStore(SelectStore.StoreUID);
                }
            }
        }




        private List<HealthOrganisationModel> _OrganisationIssues;

        public List<HealthOrganisationModel> OrganisationIssues
        {
            get { return _OrganisationIssues; }
            set { Set(ref _OrganisationIssues, value); }
        }
        private HealthOrganisationModel _SelectOrganisationIssue;

        public HealthOrganisationModel SelectOrganisationIssue
        {
            get { return _SelectOrganisationIssue; }
            set
            {
                Set(ref _SelectOrganisationIssue, value);
                if (_SelectOrganisationIssue != null)
                {
                    LocationIssueData = GetLocatioinRole(_SelectOrganisationIssue.HealthOrganisationUID);
                    LocationIssue = LocationIssueData;

                    if (SelectLocation != null)
                        LocationIssue = LocationIssueData?.Where(p => p.LocationUID != SelectLocation.LocationUID).ToList();
                }
            }
        }



        private List<LocationModel> _LocationIssue;

        public List<LocationModel> LocationIssue
        {
            get { return _LocationIssue; }
            set { Set(ref _LocationIssue, value); }
        }

        private LocationModel _SelectLocationIssue;

        public LocationModel SelectLocationIssue
        {
            get { return _SelectLocationIssue; }
            set
            {
                Set(ref _SelectLocationIssue, value);
                StoresIssues = null;
                if (_SelectLocationIssue != null)
                {
                    StoresIssues = DataService.Inventory.GetStoreByLocationUID(_SelectLocationIssue.LocationUID);
                    Locations = LocationData?.Where(p => p.LocationUID != SelectLocationIssue.LocationUID).ToList();
                }
            }
        }

        private List<StoreModel> _StoresIssues;

        public List<StoreModel> StoresIssues
        {
            get { return _StoresIssues; }
            set { Set(ref _StoresIssues, value); }
        }

        private StoreModel _SelectStoreIssue;

        public StoreModel SelectStoreIssue
        {
            get { return _SelectStoreIssue; }
            set
            {
                Set(ref _SelectStoreIssue, value);
            }
        }


        private List<ItemMasterModel> _ItemMasters;

        public List<ItemMasterModel> ItemMasters
        {
            get { return _ItemMasters; }
            set { Set(ref _ItemMasters, value); }
        }



        private ObservableCollection<ItemMasterList> _ItemReceiveDetail;

        public ObservableCollection<ItemMasterList> ItemReceiveDetail
        {
            get { return _ItemReceiveDetail; }
            set
            {
                Set(ref _ItemReceiveDetail, value);
            }
        }

        private ItemMasterList _SelectItemReceiveDetail;

        public ItemMasterList SelectItemReceiveDetail
        {
            get { return _SelectItemReceiveDetail; }
            set
            {
                Set(ref _SelectItemReceiveDetail, value);
            }
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

        #endregion

        #region Command


        private RelayCommand _SearchRequestCommand;

        public RelayCommand SearchRequestCommand
        {
            get { return _SearchRequestCommand ?? (_SearchRequestCommand = new RelayCommand(SearchIssuePopUp)); }
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

        #region Method

        ItemReceiveModel model;

        #region Varible

        List<HealthOrganisationModel> HealthOrganisations;

        #endregion

        public ManageItemReceiveViewModel()
        {
            ItemReceiveDetail = new ObservableCollection<ItemMasterList>();

            HealthOrganisations = GetHealthOrganisationIsStock();
            Organisations = GetHealthOrganisationIsRoleStock();
            OrganisationIssues = HealthOrganisations;
            ReceiveDate = DateTime.Now;

            if (Organisations != null)
            {
                SelectOrganisation = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == AppUtil.Current.OwnerOrganisationUID);
            }
        }


        private void Save()
        {
            try
            {
                if (SelectOrganisation == null)
                {
                    WarningDialog("กรุณาระบุ สถานประกอบการที่รับของ");
                    return;
                }

                if (SelectLocation == null)
                {
                    WarningDialog("กรุณาระบุ แผนก/สถานที่รับของ");
                    return;
                }

                if (SelectStore == null)
                {
                    WarningDialog("กรุณาระบุ Store รับ");
                    return;
                }
                if (ReceiveDate == null)
                {
                    WarningDialog("กรุณาใส่วันที่ สั่งออก");
                    return;
                }

                AssignPropertiesToModel();
                DataService.Inventory.ManageItemReceive(model, AppUtil.Current.UserID);
                SaveSuccessDialog();

                ListItemReceive view = new ListItemReceive();
                ChangeView_CloseViewDialog(view, ActionDialog.Save);
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }

        private void Cancel()
        {
            ListItemReceive view = new ListItemReceive();
            ChangeView_CloseViewDialog(view, ActionDialog.Cancel);
        }

        private void SearchIssuePopUp()
        {

            int? organTo = SelectOrganisation != null ? SelectOrganisation.HealthOrganisationUID : (int?)null;
            int? organFrom = SelectOrganisationIssue != null ? SelectOrganisationIssue.HealthOrganisationUID : (int?)null;
            SearchIssue view = new SearchIssue(organFrom, organTo);
            SearchIssueViewModel result = (SearchIssueViewModel)LaunchViewDialog(view, "SEAISS", false);
            if (result != null && result.ResultDialog == ActionDialog.Save)
            {
                if (result.ItemIssueDetails != null)
                {
                    ItemReceiveDetail = new ObservableCollection<ItemMasterList>();
                    SelectOrganisation = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == result.SelectItemIssues.RequestedByOrganisationUID);
                    SelectLocation = Locations.FirstOrDefault(p => p.LocationUID == result.SelectItemIssues.RequestedByLocationUID);
                    SelectStore = Stores.FirstOrDefault(p => p.StoreUID == result.SelectItemIssues.RequestedByStoreUID);
                    SelectOrganisationIssue = OrganisationIssues.FirstOrDefault(p => p.HealthOrganisationUID == result.SelectItemIssues.OrganisationUID);
                    SelectLocationIssue = LocationIssue.FirstOrDefault(p => p.LocationUID == result.SelectItemIssues.LocationUID);
                    SelectStoreIssue = StoresIssues.FirstOrDefault(p => p.StoreUID == result.SelectItemIssues.StoreUID);
                    NetAmount = result.SelectItemIssues.NetAmount;
                    OtherChages = result.SelectItemIssues.OtherCharges;

                    foreach (var item in result.ItemIssueDetails)
                    {
                        ItemMasterList newRow = new ItemMasterList();
                        //newRow.ItemListUID = item.ItemIssueDetailUID;
                        //newRow.ItemMasterUID = item.ItemMasterUID;
                        //newRow.ItemCode = item.ItemCode;
                        //newRow.ItemName = item.ItemName;
                        //newRow.Quantity = item.Quantity;
                        //newRow.IMUOMUID = item.IMUOMUID;
                        //newRow.BatchID = item.BatchID;
                        //newRow.ExpiryDttm = item.ExpiryDttm;

                        //if (ItemMasters != null)
                        //{
                        //    ItemMasterModel currentStock = ItemMasters
                        //    .Where(p => p.ItemMasterUID == item.ItemMasterUID)
                        //    .OrderBy(p => p.ExpiryDttm).FirstOrDefault();

                        //    if (currentStock != null)
                        //    {
                        //        newRow.StockQuantity = currentStock.StockQty;
                        //    }
                        //    else
                        //    {
                        //        newRow.StockQuantity = 0;
                        //    }
                        //}


                        newRow.SelectItemMaster = ItemMasters
           .Where(p => p.ItemMasterUID == item.ItemMasterUID).FirstOrDefault();
                        newRow.StockUID = item.StockUID;
                        newRow.Quantity = item.Quantity;
                        newRow.ItemCost = item.ItemCost;
                        newRow.BatchID = item.BatchID;
                        newRow.ExpiryDttm = item.ExpiryDttm;
                        newRow.ItemCost = item.ItemCost;
                        newRow.UnitPrice = item.UnitPrice;
                        newRow.NetAmount = item.NetAmount;
                        ItemReceiveDetail.Add(newRow);
                        if (result.SelectItemIssues != null)
                        {
                            IssueNo = result.SelectItemIssues.ItemIssueID;
                            ItemIssueUID = result.SelectItemIssues.ItemIssueUID;
                        }

                    }
                }
            }


        }

        public void AssignModel(ItemReceiveModel model)
        {
            this.model = model;
            AssignModelToProperties();
        }

        public void AssignPropertiesToModel()
        {
            if (model == null)
            {
                model = new ItemReceiveModel();
            }
            model.ReceiveBy = AppUtil.Current.UserID;
            model.ReceivedDttm = ReceiveDate.Value;
            model.OrganisationUID = SelectOrganisation.HealthOrganisationUID;
            model.LocationUID = SelectLocation.LocationUID;
            model.StoreUID = SelectStore.StoreUID;
            model.IssuedByOrganisationUID = SelectOrganisationIssue.HealthOrganisationUID;
            model.IssuedByLocationUID = SelectLocationIssue.LocationUID;
            model.IssuedByStoreUID = SelectStoreIssue.StoreUID;
            model.RCSTSUID = 2930;
            model.ItemReceiveDetail = new List<ItemReceiveDetailModel>();
            model.ItemIssueUID = ItemIssueUID;
            model.ItemIssueID = IssueNo;
            model.NetAmount = NetAmount;
            model.OtherCharges = OtherChages;
            foreach (var item in ItemReceiveDetail)
            {
                ItemReceiveDetailModel newRow = new ItemReceiveDetailModel();
                //newRow.ItemRecieveDetailUID = item.ItemMasterUID;
                newRow.ItemMasterUID = item.ItemMasterUID;
                newRow.ItemCode = item.ItemCode;
                newRow.ItemName = item.ItemName;
                newRow.ItemCost = item.ItemCost ?? 0;
                newRow.NetAmount = item.NetAmount;
                newRow.UnitPrice = item.UnitPrice ?? 0;
                //newRow.IssuedByStockUID = item.StockUID;
                newRow.BatchID = item.BatchID;
                newRow.Quantity = item.Quantity ?? 0;
                newRow.IMUOMUID = item.IMUOMUID;
                newRow.ExpiryDttm = item.ExpiryDttm;
                newRow.IssuedByStockUID = item.StockUID;
                model.ItemReceiveDetail.Add(newRow);
            }
        }

        public void AssignModelToProperties()
        {
            ReceiveDate = model.ReceivedDttm;
            ItemIssueUID = model.ItemIssueUID;
            IssueNo = model.ItemIssueID;
            SelectOrganisation = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == model.OrganisationUID);
            SelectLocation = Locations.FirstOrDefault(p => p.LocationUID == model.LocationUID);
            SelectStore = Stores.FirstOrDefault(p => p.StoreUID == model.StoreUID);
            SelectOrganisationIssue = OrganisationIssues.FirstOrDefault(p => p.HealthOrganisationUID == model.IssuedByOrganisationUID);
            SelectLocationIssue = LocationIssue.FirstOrDefault(p => p.LocationUID == model.IssuedByLocationUID);
            SelectStoreIssue = StoresIssues.FirstOrDefault(p => p.StoreUID == model.IssuedByStoreUID);
            NetAmount = model.NetAmount;
            OtherChages = model.OtherCharges;
            foreach (var item in model.ItemReceiveDetail)
            {
                ItemMasterList newRow = new ItemMasterList();
                newRow.ItemListUID = item.ItemRecieveDetailUID;
                newRow.ItemMasterUID = item.ItemMasterUID;
                newRow.ItemCode = item.ItemCode;
                newRow.ItemName = item.ItemName;
                newRow.StockUID = item.IssuedByStockUID;
                newRow.BatchID = item.BatchID;
                newRow.Quantity = item.Quantity;
                newRow.ItemCost = item.ItemCost;
                newRow.UnitPrice = item.UnitPrice;
                newRow.NetAmount = item.NetAmount;
                newRow.BatchQuantity = ItemMasters.FirstOrDefault(p => p.StockUID == item.IssuedByStockUID).BatchQty;
                newRow.IMUOMUID = item.IMUOMUID;
                newRow.ExpiryDttm = item.ExpiryDttm;
                newRow.StockUID = item.IssuedByStockUID;
                ItemReceiveDetail.Add(newRow);
            }
        }

        public void AssingPropertiesFromIssue(int itemIssueUID)
        {
            var itemIssue = DataService.Inventory.GetItemIssueByUID(itemIssueUID);
            if (itemIssue != null)
            {
                SelectOrganisation = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == itemIssue.RequestedByOrganisationUID);
                SelectLocation = Locations.FirstOrDefault(p => p.LocationUID == itemIssue.RequestedByLocationUID);
                SelectStore = Stores.FirstOrDefault(p => p.StoreUID == itemIssue.RequestedByStoreUID);
                SelectOrganisationIssue = OrganisationIssues.FirstOrDefault(p => p.HealthOrganisationUID == itemIssue.OrganisationUID);
                SelectLocationIssue = LocationIssue.FirstOrDefault(p => p.LocationUID == itemIssue.LocationUID);
                SelectStoreIssue = StoresIssues.FirstOrDefault(p => p.StoreUID == itemIssue.StoreUID);
                NetAmount = itemIssue.NetAmount;
                OtherChages = itemIssue.OtherCharges;
                foreach (var item in itemIssue.ItemIssueDetail)
                {
                    ItemMasterList newRow = new ItemMasterList();
                    //newRow.ItemListUID = item.ItemIssueDetailUID;
                    //newRow.ItemMasterUID = item.ItemMasterUID;
                    //newRow.ItemCode = item.ItemCode;
                    //newRow.ItemName = item.ItemName;
                    //newRow.Quantity = item.Quantity;
                    //newRow.IMUOMUID = item.IMUOMUID;
                    //newRow.BatchID = item.BatchID;
                    //newRow.ExpiryDttm = item.ExpiryDttm;

                    //if (ItemMasters != null)
                    //{
                    //    ItemMasterModel currentStock = ItemMasters
                    //    .Where(p => p.ItemMasterUID == item.ItemMasterUID)
                    //    .OrderBy(p => p.ExpiryDttm).FirstOrDefault();

                    //    if (currentStock != null)
                    //    {
                    //        newRow.StockQuantity = currentStock.StockQty;
                    //    }
                    //    else
                    //    {
                    //        newRow.StockQuantity = 0;
                    //    }
                    //}


                    newRow.SelectItemMaster = ItemMasters.Where(p => p.ItemMasterUID == item.ItemMasterUID).FirstOrDefault();
                    newRow.StockUID = item.StockUID;
                    newRow.ItemCost = item.ItemCost;
                    newRow.UnitPrice = item.UnitPrice;
                    newRow.NetAmount = item.NetAmount;
                    newRow.Quantity = item.Quantity;
                    newRow.BatchID = item.BatchID;
                    newRow.ExpiryDttm = item.ExpiryDttm;
                    ItemReceiveDetail.Add(newRow);
                    if (itemIssue != null)
                    {
                        IssueNo = itemIssue.ItemIssueID;
                        ItemIssueUID = itemIssue.ItemIssueUID;
                    }

                }
            }
        }
        #endregion
    }
}
