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

namespace MediTech.ViewModels
{
    public class ListPurchaseOrderViewModel : MediTechViewModelBase
    {

        #region Properties

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
                    Stores = DataService.Inventory.GetStoreByOrganisationUID(_SelectOrganisation.HealthOrganisationUID);
                }
                else
                {
                    Stores = null;
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
            set { Set(ref _SelectStore, value); }
        }

        public List<VendorDetailModel> Vendors { get; set; }
        private VendorDetailModel _SelectVendor;

        public VendorDetailModel SelectVendor
        {
            get { return _SelectVendor; }
            set { Set(ref _SelectVendor, value); }
        }

        public List<LookupReferenceValueModel> POStatus { get; set; }
        private LookupReferenceValueModel _SelectPOStatus;

        public LookupReferenceValueModel SelectPOStatus
        {
            get { return _SelectPOStatus; }
            set { Set(ref _SelectPOStatus, value); }
        }

        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public string PONumber { get; set; }

        private List<PurchaseOrderModel> _PurchaseOrders;

        public List<PurchaseOrderModel> PurchaseOrders
        {
            get { return _PurchaseOrders; }
            set { Set(ref _PurchaseOrders, value); }
        }

        private PurchaseOrderModel _SelectPurchaseOrder;

        public PurchaseOrderModel SelectPurchaseOrder
        {
            get { return _SelectPurchaseOrder; }
            set
            {
                Set(ref _SelectPurchaseOrder, value);
                if (_SelectPurchaseOrder != null)
                {
                    PurcaseOrderItemList = DataService.Purchaseing.GetPurchaseOrderItemListByPurchaseOrderUID(SelectPurchaseOrder.PurchaseOrderUID);
                    if (SelectPurchaseOrder.POStatus == "ดำเนินการแล้ว" 
                        || SelectPurchaseOrder.POStatus == "ยกเลิก")
                    {
                        IsEnableAP = false;
                        IsEnableCancelPO = false;
                        IsEnableEditPO = false;
                        IsEnableCreateGRN = false;
                    }
                    else
                    {
                        IsEnableAP = true; 
                        IsEnableCancelPO = true;
                        IsEnableEditPO = true;
                        IsEnableCreateGRN = true;
                    }
                }

            }
        }


        private List<PurchaseOrderItemListModel> _PurcaseOrderItemList;

        public List<PurchaseOrderItemListModel> PurcaseOrderItemList
        {
            get { return _PurcaseOrderItemList; }
            set { Set(ref _PurcaseOrderItemList, value); }
        }

        private PurchaseOrderItemListModel _SelectPurchaseOrderItemList;

        public PurchaseOrderItemListModel SelectPurchaseOrderItemList
        {
            get { return _SelectPurchaseOrderItemList; }
            set { Set(ref _SelectPurchaseOrderItemList, value); }
        }

        private bool _IsEnableAP = false;

        public bool IsEnableAP
        {
            get { return _IsEnableAP; }
            set { Set(ref _IsEnableAP, value); }
        }

        private bool _IsEnableEditPO = false;

        public bool IsEnableEditPO
        {
            get { return _IsEnableEditPO; }
            set { Set(ref _IsEnableEditPO, value); }
        }

        private bool _IsEnableCancelPO = false;

        public bool IsEnableCancelPO
        {
            get { return _IsEnableCancelPO; }
            set { Set(ref _IsEnableCancelPO, value); }
        }

        private bool _IsEnableCreateGRN = false;

        public bool IsEnableCreateGRN
        {
            get { return _IsEnableCreateGRN; }
            set { Set(ref _IsEnableCreateGRN, value); }
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

        private RelayCommand _ClearCommand;

        public RelayCommand ClearCommand
        {
            get
            {
                return _ClearCommand
                    ?? (_ClearCommand = new RelayCommand(Clear));
            }

        }




        private RelayCommand _ApporveCommand;

        public RelayCommand ApporveCommand
        {
            get
            {
                return _ApporveCommand
                    ?? (_ApporveCommand = new RelayCommand(ApporvePO));
            }

        }

        private RelayCommand _PrintCommand;

        public RelayCommand PrintCommand
        {
            get
            {
                return _PrintCommand
                    ?? (_PrintCommand = new RelayCommand(Print));
            }

        }



        private RelayCommand _AddCommand;

        public RelayCommand AddCommand
        {
            get
            {
                return _AddCommand
                    ?? (_AddCommand = new RelayCommand(Add));
            }

        }

        private RelayCommand _EditCommand;

        public RelayCommand EditCommand
        {
            get
            {
                return _EditCommand
                    ?? (_EditCommand = new RelayCommand(Edit));
            }

        }

        private RelayCommand _DeleteCommand;

        public RelayCommand DeleteCommand
        {
            get
            {
                return _DeleteCommand
                    ?? (_DeleteCommand = new RelayCommand(CancelPO));
            }

        }

        private RelayCommand _CreateGRNCommand;

        public RelayCommand CreateGRNCommand
        {
            get
            {
                return _CreateGRNCommand
                    ?? (_CreateGRNCommand = new RelayCommand(CreateGRN));
            }
        }

        #endregion

        #region Method

        public ListPurchaseOrderViewModel()
        {
            DateFrom = DateTime.Now.Date;
            Vendors = DataService.Purchaseing.GetVendorDetail();
            Vendors = Vendors.Where(p => p.MNFTPUID == 2937).ToList();
            Organisations = GetHealthOrganisationIsRoleStock();
            POStatus = DataService.Technical.GetReferenceValueMany("POSTS");


            if (Organisations != null)
            {
                SelectOrganisation = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == AppUtil.Current.OwnerOrganisationUID);
            }
        }


        public override void OnLoaded()
        {
            Search();
        }

        private void Search()
        {
            PurchaseOrders = null;
            PurcaseOrderItemList = null;
            SelectPurchaseOrder = null;
            SelectPurchaseOrderItemList = null;
            int? organisationUID = SelectOrganisation != null ? SelectOrganisation.HealthOrganisationUID : (int?)null;
            int? storeUID = SelectStore != null ? SelectStore.StoreUID : (int?)null;
            int? vendorDetailUID = SelectVendor != null ? SelectVendor.VendorDetailUID : (int?)null;
            int? poStatusUID = SelectPOStatus != null ? SelectPOStatus.Key : (int?)null;
            PurchaseOrders = DataService.Purchaseing.SearchPurchaseOrder(DateFrom, DateTo, organisationUID, storeUID, vendorDetailUID, poStatusUID, PONumber);
        }

        private void Clear()
        {
            DateFrom = DateTime.Now;
            DateTo = null;
            SelectVendor = null;
            SelectOrganisation = null;
            SelectStore = null;
            PONumber = string.Empty;
            SelectPOStatus = null;
        }

        private void ApporvePO()
        {
            if (SelectPurchaseOrder != null)
            {
                try
                {
                    ApprovePO poApprove = new ApprovePO();
                    ApprovePOViewModel result = (ApprovePOViewModel)LaunchViewDialog(poApprove,"APRPO",true);
                    if (result != null && result.ResultDialog == ActionDialog.Save)
                    {
                        DataService.Purchaseing.UpdatePurchaseOrderStatus(SelectPurchaseOrder.PurchaseOrderUID, result.SelectPOStatus.Key, result.Comments, AppUtil.Current.UserID);
                        SaveSuccessDialog();
                        SelectPurchaseOrder.POSTSUID = result.SelectPOStatus.Key;
                        SelectPurchaseOrder.POStatus = result.SelectPOStatus.Display;
                        OnUpdateEvent();
                    }
                }
                catch (Exception er)
                {

                    ErrorDialog(er.Message);
                }

            }

        }

        private void Print()
        {

        }

        private void Add()
        {
            ManagePurchaseOrder managePage = new ManagePurchaseOrder();
            ChangeViewPermission(managePage);
        }

        private void Edit()
        {
            if (SelectPurchaseOrder != null)
            {
                ManagePurchaseOrder managePage = new ManagePurchaseOrder();
                var poEditData = DataService.Purchaseing.GetPurchaseOrderByUID(SelectPurchaseOrder.PurchaseOrderUID);
                (managePage.DataContext as ManagePurchaseOrderViewModel).AssignModel(poEditData);
                ChangeViewPermission(managePage);
            }

        }

        private void CancelPO()
        {
            if (SelectPurchaseOrder != null)
            {
                try
                {
                    CancelPopup cancelPO = new CancelPopup();
                    CancelPopupViewModel result = (CancelPopupViewModel)LaunchViewDialog(cancelPO,"CANPO",true);
                    if (result != null && result.ResultDialog == ActionDialog.Save)
                    {
                        DataService.Purchaseing.CancelPurchaseOrder(SelectPurchaseOrder.PurchaseOrderUID, result.Comments, AppUtil.Current.UserID);
                        SaveSuccessDialog();
                        SelectPurchaseOrder.POSTSUID = 2909;
                        SelectPurchaseOrder.POStatus = POStatus.FirstOrDefault(p => p.Key == 2909).Display;
                        OnUpdateEvent();
                        SelectPurchaseOrder = null;
                    }
                }
                catch (Exception er)
                {

                    ErrorDialog(er.Message);
                }

            }
        }

        private void CreateGRN()
        {
            if (SelectPurchaseOrder != null)
            {
                    ManageGRN mangePage = new ManageGRN();
                    GRNDetailModel grnDetailModel = new GRNDetailModel();
                    grnDetailModel.RecievedStoreUID = SelectPurchaseOrder.DelieryToStoreUID ?? 0;
                    grnDetailModel.VendorDetailUID = SelectPurchaseOrder.VendorDetailUID;
                    grnDetailModel.RecievedOrganisationUID = SelectPurchaseOrder.HealthOrganisationUID ?? 0;
                    grnDetailModel.Comments = SelectPurchaseOrder.Comments;
                    grnDetailModel.Discount = SelectPurchaseOrder.Discount;
                    grnDetailModel.OtherCharges = SelectPurchaseOrder.OtherCharges;
                    grnDetailModel.PurchaseOrderID = SelectPurchaseOrder.PurchaseOrderID;
                    grnDetailModel.NetAmount = SelectPurchaseOrder.NetAmount;
                    grnDetailModel.GRNItemLists = new List<GRNItemListModel>();

                    var purchaseItemList = DataService.Purchaseing.GetPurchaseOrderItemListByPurchaseOrderUID(SelectPurchaseOrder.PurchaseOrderUID);
                    foreach (var item in purchaseItemList)
                    {
                        GRNItemListModel grnItemList = new GRNItemListModel();
                        grnItemList.ItemMasterUID = item.ItemMasterUID;
                        grnItemList.ItemCode = item.ItemCode;
                        grnItemList.ItemName = item.ItemName;
                        grnItemList.IMUOMUID = item.IMUOMUID;
                        grnItemList.Quantity = item.Quantity;
                        grnItemList.PurchaseCost = item.UnitPrice;
                        grnItemList.NetAmount = item.NetAmount;
                        grnDetailModel.GRNItemLists.Add(grnItemList);
                    }

                    (mangePage.DataContext as ManageGRNViewModel).AssignModel(grnDetailModel,true);
                    ChangeViewPermission(mangePage,this.View);
            }
        }


        #endregion
    }
}
