using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MediTech.DataService;
using MediTech.Model;
using MediTech.Models;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
namespace MediTech.ViewModels
{
    public class ManagePurchaseOrderViewModel : MediTechViewModelBase
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

        private DateTime _RequestedDttm;

        public DateTime RequestedDttm
        {
            get { return _RequestedDttm; }
            set { Set(ref _RequestedDttm, value); }
        }

        private DateTime? _RequiredDttm;

        public DateTime? RequiredDttm
        {
            get { return _RequiredDttm; }
            set { Set(ref _RequiredDttm, value); }
        }


        private string _Comments;

        public string Comments
        {
            get { return _Comments; }
            set { Set(ref _Comments, value); }
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
            set { Set(ref _OtherChages, value); CalculateNetAmount(); }
        }

        private double _Discount;

        public double Discount
        {
            get { return _Discount; }
            set { Set(ref _Discount, value); CalculateNetAmount(); }
        }

        private ObservableCollection<ItemMasterList> _PurchaseOrderItem;

        public ObservableCollection<ItemMasterList> PurchaseOrderItem
        {
            get { return _PurchaseOrderItem; }
            set
            {
                Set(ref _PurchaseOrderItem, value);
            }
        }

        private ItemMasterList _SelectPurchaseOrderItem;

        public ItemMasterList SelectPurchaseOrderItem
        {
            get { return _SelectPurchaseOrderItem; }
            set
            {
                Set(ref _SelectPurchaseOrderItem, value);
            }
        }





        public List<LookupReferenceValueModel> PaymentTypes { get; set; }
        private LookupReferenceValueModel _SelectPaymentType;

        public LookupReferenceValueModel SelectPaymentType
        {
            get { return _SelectPaymentType; }
            set
            {
                Set(ref _SelectPaymentType, value);
                if (_SelectPaymentType != null)
                {
                    if (_SelectPaymentType.ValueCode == "DEBITD" || _SelectPaymentType.ValueCode == "CARDD")
                    {
                        IsExpiryPayment = Visibility.Visible;
                    }
                    else
                    {
                        IsExpiryPayment = Visibility.Hidden;
                    }

                }
            }
        }

        private Visibility _IsExpiryPayment;

        public Visibility IsExpiryPayment
        {
            get { return _IsExpiryPayment; }
            set { Set(ref _IsExpiryPayment, value); }
        }


        private double _PaymentAmount;

        public double PaymentAmount
        {
            get { return _PaymentAmount; }
            set { Set(ref _PaymentAmount, value); }
        }

        public List<LookupReferenceValueModel> UnitCash { get; set; }
        private LookupReferenceValueModel _SelectUnitCash;

        public LookupReferenceValueModel SelectUnitCash
        {
            get { return _SelectUnitCash; }
            set { Set(ref _SelectUnitCash, value); }
        }

        private DateTime _PaidDttm;

        public DateTime PaidDttm
        {
            get { return _PaidDttm; }
            set { Set(ref _PaidDttm, value); }
        }

        private DateTime? _ExpiryDttm;

        public DateTime? ExpiryDttm
        {
            get { return _ExpiryDttm; }
            set { Set(ref _ExpiryDttm, value); }
        }

        private ObservableCollection<PurchaseOrderPaymentModel> _PurchaseOrderPayments;

        public ObservableCollection<PurchaseOrderPaymentModel> PurchaseOrderPayments
        {
            get { return _PurchaseOrderPayments; }
            set
            {
                Set(ref _PurchaseOrderPayments, value);

            }
        }

        private PurchaseOrderPaymentModel _SelectPurchaseOrderPayment;

        public PurchaseOrderPaymentModel SelectPurchaseOrderPayment
        {
            get { return _SelectPurchaseOrderPayment; }
            set
            {
                Set(ref _SelectPurchaseOrderPayment, value);
            }
        }

        private List<ItemMasterModel> _ItemMasters;

        public List<ItemMasterModel> ItemMasters
        {
            get { return _ItemMasters; }
            set { Set(ref _ItemMasters, value); }
        }

        private ItemMasterModel _SelectItemMaster;

        public ItemMasterModel SelectItemMaster
        {
            get { return _SelectItemMaster; }
            set
            {
                Set(ref _SelectItemMaster, value);
            }
        }

        #endregion

        #region Command

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

        private RelayCommand _AddPaymentCommand;
        public RelayCommand AddPaymentCommand
        {
            get { return _AddPaymentCommand ?? (_AddPaymentCommand = new RelayCommand(AddPayment)); }
        }

        private RelayCommand _DeletePaymentCommand;
        public RelayCommand DeletePaymentCommand
        {
            get { return _DeletePaymentCommand ?? (_DeletePaymentCommand = new RelayCommand(DeletePayment)); }
        }

        private RelayCommand<DevExpress.Xpf.Grid.CellValueChangedEventArgs> _ChangeValueCommand;
        public RelayCommand<DevExpress.Xpf.Grid.CellValueChangedEventArgs> ChangeValueCommand
        {
            get { return _ChangeValueCommand ?? (_ChangeValueCommand = new RelayCommand<DevExpress.Xpf.Grid.CellValueChangedEventArgs>(ChangeValue)); }
        }

        #endregion

        #region Method

        public void ChangeValue(DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            CalculateNetAmount();
        }

        void CalculateNetAmount()
        {
            NetAmount = 0;
            foreach (var item in PurchaseOrderItem)
            {
                NetAmount += item.NetAmount;
            }

            NetAmount += OtherChages - Discount;
        }

        PurchaseOrderModel model;

        public ManagePurchaseOrderViewModel()
        {
            PurchaseOrderItem = new ObservableCollection<ItemMasterList>();
            RequestedDttm = DateTime.Now.Date;
            PaidDttm = DateTime.Now.Date;
            //Stores = inventoryData.GetStore();
            Organisations = Organisations = GetHealthOrganisationIsRoleStock();
            Vendors = DataService.Purchaseing.GetVendorDetail();
            Vendors = Vendors.Where(p => p.MNFTPUID == 2937).ToList();
            ItemMasters = DataService.Inventory.GetItemMaster();
            var dataRef = DataService.Technical.GetReferenceValueList("POSTS,PAYMD,CURNC");
            UnitCash = dataRef.Where(p => p.DomainCode == "CURNC").ToList();
            POStatus = dataRef.Where(p => p.DomainCode == "POSTS").ToList();
            PaymentTypes = dataRef.Where(p => p.DomainCode == "PAYMD").ToList();
            SelectPOStatus = POStatus.FirstOrDefault(p => p.ValueCode == "WAIAPP");
            SelectUnitCash = UnitCash.FirstOrDefault();
            SelectPaymentType = PaymentTypes.FirstOrDefault();


            if (Organisations != null)
            {
                SelectOrganisation = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == AppUtil.Current.OwnerOrganisationUID);
            }
        }

        private void Save()
        {

            try
            {
                if (RequestedDttm == null)
                {
                    WarningDialog("กรุณาระบุ วันที่");
                    return;
                }
                if (SelectVendor == null)
                {
                    WarningDialog("กรุณาเลือก ผู้จัดจำหน่าย");
                    return;
                }
                if (PurchaseOrderItem.Count <= 0)
                {
                    WarningDialog("กรุณาสร้างรายการสินค้า");
                    return;
                }

                AssingPropertiesToModel();
                DataService.Purchaseing.ManagePurchaseOrder(model, AppUtil.Current.UserID);
                SaveSuccessDialog();
                ListPurchaseOrder listPage = new ListPurchaseOrder();
                ChangeViewPermission(listPage);
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }

        private void Cancel()
        {
            ListPurchaseOrder listPage = new ListPurchaseOrder();
            ChangeViewPermission(listPage);
        }

        private void AddPayment()
        {
            if (PurchaseOrderPayments == null)
            {
                PurchaseOrderPayments = new ObservableCollection<PurchaseOrderPaymentModel>();
            }
            PurchaseOrderPaymentModel purchasePayment = new PurchaseOrderPaymentModel();
            purchasePayment.CURNCUID = SelectUnitCash.Key;
            purchasePayment.CurrencyType = SelectUnitCash.Display;
            purchasePayment.Amount = PaymentAmount;
            purchasePayment.PAYMDUID = SelectPaymentType.Key;
            purchasePayment.PaymentType = SelectPaymentType.Display;
            purchasePayment.PaidDttm = PaidDttm;
            purchasePayment.ExpiryDttm = ExpiryDttm;
            PurchaseOrderPayments.Add(purchasePayment);
        }

        private void DeletePayment()
        {
            if (SelectPurchaseOrderPayment != null)
            {
                PurchaseOrderPayments.Remove(SelectPurchaseOrderPayment);
                OnUpdateEvent();
            }

        }

        public void AssignModel(PurchaseOrderModel modelData)
        {
            model = modelData;
            AssingModelToProperties();
        }

        public void AssingModelToProperties()
        {
            RequestedDttm = model.RequestedDttm;
            RequiredDttm = model.RequiredDttm;

            if (model.HealthOrganisationUID != null)
                SelectOrganisation = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == model.HealthOrganisationUID);

            if (model.DelieryToStoreUID != null)
                SelectStore = Stores.FirstOrDefault(p => p.StoreUID == model.DelieryToStoreUID);

            SelectVendor = Vendors.FirstOrDefault(p => p.VendorDetailUID == model.VendorDetailUID);

            SelectPOStatus = POStatus.FirstOrDefault(p => p.Key == model.POSTSUID);
            Comments = model.Comments;
            OtherChages = model.OtherCharges;
            Discount = model.Discount;
            NetAmount = model.NetAmount;
            PurchaseOrderItem = new ObservableCollection<ItemMasterList>();
            foreach (var item in model.PurchaseOrderItemList)
            {
                ItemMasterList newItems = new ItemMasterList();
                newItems.ItemListUID = item.PurchaseOrderItemListUID;
                newItems.ItemMasterUID = item.ItemMasterUID;
                newItems.ItemName = item.ItemName;
                newItems.ItemCode = item.ItemCode;
                newItems.IMUOMUID = item.IMUOMUID;
                newItems.Quantity = item.Quantity;
                newItems.UnitPrice = item.UnitPrice;
                newItems.NetAmount = item.NetAmount;
                PurchaseOrderItem.Add(newItems);
            }

            PurchaseOrderPayments = new ObservableCollection<PurchaseOrderPaymentModel>(model.PurchaseOrderPayments);
        }

        public void AssingPropertiesToModel()
        {
            if (model == null)
            {
                model = new PurchaseOrderModel();
            }
            model.RequestedDttm = RequestedDttm;
            model.RequiredDttm = RequiredDttm;
            model.HealthOrganisationUID = SelectOrganisation != null ? SelectOrganisation.HealthOrganisationUID : (int?)null;
            model.DelieryToStoreUID = SelectStore != null ? SelectStore.StoreUID : (int?)null;
            model.VendorDetailUID = SelectVendor.VendorDetailUID;
            model.POSTSUID = SelectPOStatus.Key;
            model.Comments = Comments;
            model.OtherCharges = OtherChages;
            model.Discount = Discount;
            model.NetAmount = NetAmount;

            if (PurchaseOrderPayments != null)
            {
                model.PurchaseOrderPayments = PurchaseOrderPayments.ToList();
            }

            model.PurchaseOrderItemList = new List<PurchaseOrderItemListModel>();
            foreach (var item in PurchaseOrderItem)
            {

                PurchaseOrderItemListModel newRow = new PurchaseOrderItemListModel();
                newRow.PurchaseOrderItemListUID = item.ItemListUID;
                newRow.ItemMasterUID = item.ItemMasterUID;
                newRow.ItemName = item.ItemName;
                newRow.ItemCode = item.ItemCode;
                newRow.Quantity = item.Quantity ?? 0;
                newRow.IMUOMUID = item.IMUOMUID;
                newRow.UnitPrice = item.UnitPrice ?? 0;
                newRow.NetAmount = item.NetAmount;
                newRow.Comments = item.Comments;
                model.PurchaseOrderItemList.Add(newRow);
            }
        }
        #endregion
    }
}
