using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using MediTech.Model;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using MediTech.DataService;
using MediTech.Views;
using System.Windows.Forms;
using ShareLibrary;

namespace MediTech.ViewModels
{
    public class PatientOrderEntryViewModel : MediTechViewModelBase
    {
        int FINDIS = 421;
        int BLINP = 423;
        int CANCEL = 410;

        #region Properites

        private bool _IsBilling;

        public bool IsBilling
        {
            get { return _IsBilling; }
            set { _IsBilling = value; }
        }


        private int _SelectTabIndex;

        public int SelectTabIndex
        {
            get { return _SelectTabIndex; }
            set { Set(ref _SelectTabIndex, value); }
        }


        private string _SearchOrderCriteria;

        public string SearchOrderCriteria
        {
            get { return _SearchOrderCriteria; }
            set
            {
                Set(ref _SearchOrderCriteria, value);
                if (!string.IsNullOrEmpty(_SearchOrderCriteria) && _SearchOrderCriteria.Length >= 3)
                {
                    int ownerOrganisationUID = 0;
                    ownerOrganisationUID = AppUtil.Current.OwnerOrganisationUID;
                    OrderItems = DataService.OrderProcessing.SearchOrderItem(_SearchOrderCriteria, ownerOrganisationUID);

                }
                else
                {
                    OrderItems = null;
                }
            }
        }

        private DateTime _StartDate;

        public DateTime StartDate
        {
            get { return _StartDate; }
            set { Set(ref _StartDate, value); }
        }

        private DateTime _StartTime;
        public DateTime StartTime
        {
            get { return _StartTime; }
            set { Set(ref _StartTime, value); }
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
            set { Set(ref _SelectLocation, value); }
        }


        private PatientVisitModel _PatientVisit;
        public PatientVisitModel PatientVisit
        {
            get { return _PatientVisit; }
            set { Set(ref _PatientVisit, value); }
        }

        private List<SearchOrderItem> _OrderItems;

        public List<SearchOrderItem> OrderItems
        {
            get { return _OrderItems; }
            set { Set(ref _OrderItems, value); }
        }

        private SearchOrderItem _SelectOrderItem;

        public SearchOrderItem SelectOrderItem
        {
            get { return _SelectOrderItem; }
            set
            {
                _SelectOrderItem = value;
                if (_SelectOrderItem != null)
                {
                    ApplyOrderItem(_SelectOrderItem);
                }
            }
        }


        private List<CareproviderModel> _Careproviders;

        public List<CareproviderModel> Careproviders
        {
            get { return _Careproviders; }
            set { Set(ref _Careproviders, value); }
        }

        private CareproviderModel _SelectCareprovider;

        public CareproviderModel SelectCareprovider
        {
            get { return _SelectCareprovider; }
            set { Set(ref _SelectCareprovider, value); }
        }


        private List<PatientOrderAlertModel> _PatientOrderAlerts;

        public List<PatientOrderAlertModel> PatientOrderAlerts
        {
            get { return _PatientOrderAlerts; }
            set { Set(ref _PatientOrderAlerts, value); }
        }


        private ObservableCollection<PatientOrderDetailModel> _PatientOrders;

        public ObservableCollection<PatientOrderDetailModel> PatientOrders
        {
            get { return _PatientOrders ?? (_PatientOrders = new ObservableCollection<PatientOrderDetailModel>()); }
            set
            {
                Set(ref _PatientOrders, value);
                IsEnableOrderFrom = true;
                if (_PatientOrders != null || _PatientOrders.Count > 0)
                {
                    IsEnableOrderFrom = false;
                }
            }
        }

        private PatientOrderDetailModel _SelectPatientOrder;

        public PatientOrderDetailModel SelectPatientOrder
        {
            get { return _SelectPatientOrder; }
            set { _SelectPatientOrder = value; }
        }

        private ObservableCollection<PatientOrderDetailModel> _ExistingOrders;

        public ObservableCollection<PatientOrderDetailModel> ExistingOrders
        {
            get { return _ExistingOrders ?? (_ExistingOrders = new ObservableCollection<PatientOrderDetailModel>()); }
            set { Set(ref _ExistingOrders, value); }
        }

        private ObservableCollection<PatientOrderDetailModel> _SelectExistingOrder;

        public ObservableCollection<PatientOrderDetailModel> SelectExistingOrder
        {
            get
            {
                return _SelectExistingOrder
                    ?? (_SelectExistingOrder = new ObservableCollection<PatientOrderDetailModel>());
            }
            set { Set(ref _SelectExistingOrder, value); }
        }


        private List<PatientVisitModel> _PatientVisitsList;

        public List<PatientVisitModel> PatientVisitsList
        {
            get { return _PatientVisitsList; }
            set { _PatientVisitsList = value; }
        }

        private List<LookupItemModel> _LookupVisit;

        public List<LookupItemModel> LookupVisit
        {
            get { return _LookupVisit; }
            set { Set(ref _LookupVisit, value); }
        }

        private LookupItemModel _SelectLookupVisit;

        public LookupItemModel SelectLookupVisit
        {
            get { return _SelectLookupVisit; }
            set
            {
                Set(ref _SelectLookupVisit, value);
                if (_SelectLookupVisit != null)
                {
                    var selectVisit = PatientVisitsList.FirstOrDefault(p => p.PatientVisitUID == _SelectLookupVisit.Key2);
                    if (selectVisit != null)
                    {
                        DateExitingFrom = selectVisit.StartDttm;
                        DateExitingTo = selectVisit.EndDttm;

                        SearchExistingOrder();

                        if (selectVisit.VISTSUID != CANCEL && selectVisit.VISTSUID != FINDIS && selectVisit.VISTSUID != BLINP
                            || (selectVisit.VISTSUID == BLINP && IsBilling))
                        {
                            EnabledCancelOrder = true;
                        }
                        else
                        {
                            EnabledCancelOrder = false;
                        }
                    }
                }
            }
        }

        private DateTime? _DateExitingFrom;

        public DateTime? DateExitingFrom
        {
            get { return _DateExitingFrom; }
            set { Set(ref _DateExitingFrom, value); }
        }

        private DateTime? _DateExitingTo;

        public DateTime? DateExitingTo
        {
            get { return _DateExitingTo; }
            set { Set(ref _DateExitingTo, value); }
        }

        private double? _TotalExistingAmount;

        public double? TotalExistingAmount
        {
            get { return _TotalExistingAmount; }
            set { Set(ref _TotalExistingAmount, value); }
        }

        private bool _EnableEnterOrder = true;

        public bool EnableEnterOrder
        {
            get { return _EnableEnterOrder; ; }
            set { Set(ref _EnableEnterOrder, value); }
        }

        private bool _EnabledCancelOrder;

        public bool EnabledCancelOrder
        {
            get { return _EnabledCancelOrder; ; }
            set { Set(ref _EnabledCancelOrder, value); }
        }

        private bool _IsEnableOrderFrom = true;

        public bool IsEnableOrderFrom
        {
            get { return _IsEnableOrderFrom; }
            set { Set(ref _IsEnableOrderFrom, value); }
        }



        #endregion

        #region Command

        private RelayCommand _DeleteCommand;

        public RelayCommand DeleteCommand
        {
            get
            {
                return _DeleteCommand
                    ?? (_DeleteCommand = new RelayCommand(ExecuteDeleteRow));
            }
        }

        private RelayCommand _EditCommand;

        public RelayCommand EditCommand
        {
            get
            {
                return _EditCommand
                    ?? (_EditCommand = new RelayCommand(EditOrder));
            }
        }

        private RelayCommand _SaveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return _SaveCommand ?? (_SaveCommand = new RelayCommand(SaveOrder));
            }
        }

        private RelayCommand _ClearOrderCommand;
        public RelayCommand ClearOrderCommand
        {
            get
            {
                return _ClearOrderCommand ?? (_ClearOrderCommand = new RelayCommand(ClearOrder));
            }
        }

        private RelayCommand _LoadedExistingCommand;
        public RelayCommand LoadedExistingCommand
        {
            get
            {
                return _LoadedExistingCommand ?? (_LoadedExistingCommand = new RelayCommand(LoadedExisting));
            }
        }

        private RelayCommand _SearchExistingCommand;
        public RelayCommand SearchExistingCommand
        {
            get
            {
                return _SearchExistingCommand ?? (_SearchExistingCommand = new RelayCommand(SearchExistingOrder));
            }
        }

        private RelayCommand _CancelOrderCommand;
        public RelayCommand CancelOrderCommand
        {
            get
            {
                return _CancelOrderCommand ?? (_CancelOrderCommand = new RelayCommand(CancelOrder));
            }
        }

        private RelayCommand _ReviewOrderCommand;
        public RelayCommand ReviewOrderCommand
        {
            get
            {
                return _ReviewOrderCommand ?? (_ReviewOrderCommand = new RelayCommand(ReviewOrder));
            }
        }

        private RelayCommand _OffMedicineCommand;
        public RelayCommand OffMedicineCommand
        {
            get
            {
                return _OffMedicineCommand ?? (_OffMedicineCommand = new RelayCommand(OffMedicine));
            }
        }

        #endregion

        #region Variable

        bool IsOneceLoad = false;

        List<LookupReferenceValueModel> OrderTypes;

        #endregion

        #region Method


        public PatientOrderEntryViewModel()
        {
            var refVale = DataService.Technical.GetReferenceValueList("PRSTYP");
            OrderTypes = refVale.Where(p => p.DomainCode == "PRSTYP").ToList();
        }

        public override void OnLoaded()
        {
            Careproviders = DataService.UserManage.GetCareproviderAll();
            SelectCareprovider = Careproviders.FirstOrDefault(p => p.CareproviderUID == AppUtil.Current.UserID);
            var locationData = GetLocatioinRole(AppUtil.Current.OwnerOrganisationUID);
            Locations = locationData.Where(p => p.IsCanOrder == "Y").ToList();
            SelectLocation = Locations.FirstOrDefault(p => p.LocationUID == AppUtil.Current.LocationUID);
            //SelectLocation = Locations.FirstOrDefault(p => p.LocationUID == PatientVisit.LocationUID);

            DateTime now = DateTime.Now;
            StartDate = now.Date;
            StartTime = now;

            var patientVisit = DataService.PatientIdentity.GetPatientVisitByUID(PatientVisit.PatientVisitUID);

            if ((patientVisit.VISTSUID == FINDIS || patientVisit.VISTSUID == BLINP || patientVisit.VISTSUID == CANCEL) && IsBilling == false)
            {
                EnableEnterOrder = false;
            }

            (this.View as PatientOrderEntry).txtOrder.Focus();
        }

        public void LoadedExisting()
        {
            if (!IsOneceLoad)
            {
                PatientVisitsList = DataService.PatientIdentity.GetPatientVisitByPatientUID(PatientVisit.PatientUID).OrderByDescending(p => p.StartDttm).ToList();
                if (PatientVisitsList != null)
                {
                    LookupVisit = PatientVisitsList.Select(p => new LookupItemModel
                    {
                        Key2 = p.PatientVisitUID,
                        Display = p.VisitID + " : " + p.StartDttm.Value.ToString("dd/MM/yyyy") + " - " + (p.EndDttm.HasValue ? p.EndDttm.Value.ToString("dd/MM/yyyy") : "Today")
                    }).ToList();

                    SelectLookupVisit = LookupVisit.FirstOrDefault(p => p.Key2 == PatientVisit.PatientVisitUID);
                }
                IsOneceLoad = true;
            }
        }

        public void CancelOrder()
        {
            try
            {
                if (SelectExistingOrder != null)
                {
                    List<PatientOrderDetailModel> existingOrderNotCancel = SelectExistingOrder.Where(p => p.ORDSTUID != 2848 && p.PaymentStatus == "Un Billed").ToList();
                    if (existingOrderNotCancel != null && existingOrderNotCancel.Count > 0)
                    {
                        CancelOrder cancelOrderView = new CancelOrder(existingOrderNotCancel);
                        CancelOrderViewModel result = (CancelOrderViewModel)LaunchViewDialog(cancelOrderView, "CANORD", true);
                        if (result != null && result.ResultDialog == ActionDialog.Save)
                        {
                            SearchExistingOrder();
                            ResultDialog = ActionDialog.Save;
                        }
                    }

                }
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }

        }

        public void ReviewOrder()
        {
            try
            {
                if (SelectExistingOrder != null)
                {
                    if (SelectLocation == null)
                    {
                        WarningDialog("กรุณาเลือก Order จาก");
                        SelectTabIndex = 0;
                        return;
                    }

                    foreach (var item in SelectExistingOrder)
                    {
                        PopUpOrder(item.BillableItemUID);
                    }
                    SelectExistingOrder.Clear();
                    SelectTabIndex = 0;
                }
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }

        public void OffMedicine()
        {
            try
            {
                if (SelectExistingOrder != null)
                {
                    List<PatientOrderDetailModel> closedOrderList = SelectExistingOrder.Where(p => p.ORDSTUID != 2848 && p.EndDttm == null).ToList();
                    if (closedOrderList != null && closedOrderList.Count > 0)
                    {
                        CloseOrderPopUp closeOrderPopUp = new CloseOrderPopUp(closedOrderList);
                        CloseOrderPopUpViewModel result = (CloseOrderPopUpViewModel)LaunchViewDialogNonPermiss(closeOrderPopUp, true);
                        if (result != null && result.ResultDialog == ActionDialog.Save)
                        {

                            SearchExistingOrder();
                            ResultDialog = ActionDialog.Save;
                        }
                    }

                }
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }
        void SearchExistingOrder()
        {
            ExistingOrders = new ObservableCollection<PatientOrderDetailModel>(DataService.OrderProcessing.GetOrderAllByVisitUID(SelectLookupVisit.Key2.Value, DateExitingFrom, DateExitingTo));

            TotalExistingAmount = ExistingOrders.Where(p => p.ORDSTUID != 2848).Sum(p => p.NetAmount);
        }

        private void ExecuteDeleteRow()
        {
            if (SelectPatientOrder != null)
            {
                if (SelectPatientOrder.IsStandingOrder == "Y")
                {
                    foreach (var orders in PatientOrders.Where(p => p.StandingPatientOrder != null))
                    {
                        if (orders.StandingPatientOrder.IdentityGen == SelectPatientOrder.IdentityGen)
                        {
                            orders.StandingPatientOrder = null;
                        }
                    }
                }
                PatientOrders.Remove(SelectPatientOrder);
            }
        }

        private void EditOrder()
        {
            if (SelectPatientOrder != null)
            {
                PopUpOrderEdit(SelectPatientOrder);
            }
        }
        public void SaveOrder()
        {
            try
            {
                if (SelectLocation == null)
                {
                    WarningDialog("กรุณาเลือก Order จากแผนกไหน");
                    return;
                }
                if (PatientOrders != null && PatientOrders.Count > 0)
                {
                    int userUID = AppUtil.Current.UserID;
                    //if (SelectHealthOrganisation != null)
                    //{
                    //    owerOrganisationUID = SelectHealthOrganisation.HealthOrganisationUID;
                    //}
                    //else
                    //{
                    //    owerOrganisationUID = PatientVisit.OwnerOrganisationUID ?? userUID;
                    //}
                    int locationUID = SelectLocation.LocationUID;
                    int ownerorganisationUID = AppUtil.Current.OwnerOrganisationUID;
                    var createOrderList = PatientOrders.ToList();
                    if (createOrderList.Count(p => p.IsContinuous == "Y") > 0)
                    {
                        foreach (var orderDetail in createOrderList.ToList())
                        {
                            if (orderDetail.IsStandingOrder == "Y" && PatientOrders.Count(p => p.StandingPatientOrder != null && p.StandingPatientOrder.IdentityGen == orderDetail.IdentityGen) > 0)
                            {
                                createOrderList.Remove(orderDetail);
                            }
                        }
                    }

                    DataService.OrderProcessing.CreateOrder(PatientVisit.PatientUID, PatientVisit.PatientVisitUID, userUID, locationUID, ownerorganisationUID, createOrderList);
                    PatientOrderEntry view = (PatientOrderEntry)this.View;
                    SaveSuccessDialog();
                    CloseViewDialog(ActionDialog.Save);
                }
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }

        public void ClearOrder()
        {
            if (PatientOrders != null && PatientOrders.Count > 0)
            {
                MessageBoxResult result = QuestionDialog("คุณต้องการ Clear Order ทั้งหมดใช้หรือไม่ ?");
                if (result == MessageBoxResult.Yes)
                {
                    PatientOrders = null;
                }
            }
        }

        void ApplyOrderItem(SearchOrderItem orderItem)
        {
            try
            {

                if (orderItem.TypeOrder == "OrderSet")
                {
                    OrderSetModel orderSet = DataService.MasterData.GetOrderSetByUID(orderItem.BillableItemUID);
                    int ownerUID = AppUtil.Current.OwnerOrganisationUID;
                    if (orderSet.OrderSetBillableItems != null)
                    {
                        var OrderSetBillItmActive = orderSet.OrderSetBillableItems
                            .Where(p => (p.ActiveFrom == null || p.ActiveFrom.Value.Date <= DateTime.Now.Date)
                            && (p.ActiveTo == null || p.ActiveTo.Value.Date >= DateTime.Now.Date));
                        foreach (var item in OrderSetBillItmActive)
                        {

                            PatientOrderDetailModel newOrder = new PatientOrderDetailModel();
                            BillableItemModel billItem = DataService.MasterData.GetBillableItemByUID(item.BillableItemUID);
                            if (billItem == null)
                            {
                                WarningDialog("รายการ " + item.OrderCatalogName + " ถูกลบออกจากรายการสำหรับขายแล้ว โปรดตรวจสอบ");
                                continue;
                            }
                            var orderAlready = PatientOrders.FirstOrDefault(p => p.BillableItemUID == billItem.BillableItemUID);
                            if (orderAlready != null)
                            {
                                WarningDialog("รายการ " + billItem.ItemName + " นี้มีอยู่แล้ว โปรดตรวจสอบ");
                                if (billItem.BillingServiceMetaData == "Lab Test" || billItem.BillingServiceMetaData == "Radiology"
                                    || billItem.BillingServiceMetaData == "Mobile Checkup")
                                    continue;
                            }

                            List<PatientOrderAlertModel> listOrderAlert = DataService.OrderProcessing.CriteriaOrderAlert(PatientVisit.PatientUID, billItem);
                            if (listOrderAlert != null && listOrderAlert.Count > 0)
                            {
                                WarningDialog("รายการ " + billItem.ItemName + " นี้มีแจ้งเตือนการคีย์");
                                OrderAlertViewModel viewModel = (OrderAlertViewModel)ShowModalDialogUsingViewModel(new OrderAlert(), new OrderAlertViewModel(listOrderAlert), true);
                                if (viewModel.ResultDialog != ActionDialog.Save)
                                {
                                    continue;
                                }
                                PatientOrderAlerts = viewModel.OrderAlerts;
                            }

                            //var billItemPrice = GetBillableItemPrice(billItem.BillableItemDetails, ownerUID ?? 0);

                            //if (billItemPrice == null)
                            //{
                            //    WarningDialog("รายการ " + billItem.ItemName + " นี้ยังไม่ได้กำหนดราคาสำหรับขาย โปรดตรวจสอบ");
                            //    continue;
                            //}

                            if (billItem.BillingServiceMetaData == "Drug"
                                || billItem.BillingServiceMetaData == "Medical Supplies"
                                || billItem.BillingServiceMetaData == "Supply")
                            {
                                ItemMasterModel itemMaster = DataService.Inventory.GetItemMasterByUID(billItem.ItemUID.Value);
                                List<StockModel> stores = new List<StockModel>();

                                if (itemMaster == null)
                                {
                                    WarningDialog("ไม่มี " + billItem.ItemName + " ในคลัง โปรดตรวจสอบ");
                                    continue;
                                }

                                stores = DataService.Inventory.GetStockRemainForDispensedByItemMasterUID(itemMaster.ItemMasterUID, ownerUID);

                                if (stores == null || stores.Count <= 0)
                                {
                                    WarningDialog("ไม่มี " + billItem.ItemName + " ในคลัง โปรดตรวจสอบ");
                                    continue;
                                }
                                else
                                {
                                    bool CanDispense = false;
                                    foreach (var store in stores)
                                    {
                                        if (item.Quantity > store.Quantity)
                                        {
                                            CanDispense = true;
                                        }
                                    }
                                    if (CanDispense == false)
                                    {
                                        if (itemMaster.CanDispenseWithOutStock != "Y")
                                        {
                                            WarningDialog("มี " + billItem.ItemName + " ในคลังไม่พอสำหรับจ่ายยา โปรดตรวจสอบ");
                                            continue;

                                        }
                                        else if (itemMaster.CanDispenseWithOutStock == "Y")
                                        {
                                            MessageBoxResult result = QuestionDialog("มี" + billItem.ItemName + "ในคลังไม่พอ คุณต้องการดำเนินการต่อหรือไม่ ?");
                                            if (result == MessageBoxResult.No || result == MessageBoxResult.Cancel)
                                            {
                                                continue;
                                            }
                                        }
                                    }
                                }

                                if (item.Quantity <= 0)
                                {
                                    WarningDialog("ไม่อนุญาติให้คีย์ + " + billItem.ItemName + " จำนวน < 0");
                                    continue;
                                }

                                if (itemMaster.MinSalesQty != null && item.Quantity < itemMaster.MinSalesQty)
                                {
                                    WarningDialog("คีย์จำนวน " + billItem.ItemName + " ที่ใช้น้อยกว่าจำนวนขั้นต่ำที่คีย์ได้ โปรดตรวจสอบ");
                                    continue;
                                }


                                newOrder.IsStock = itemMaster.IsStock;
                                newOrder.StoreUID = stores.FirstOrDefault(p => p.Quantity > item.Quantity) != null ? stores.FirstOrDefault(p => p.Quantity > item.Quantity).StoreUID : (int?)null;
                                newOrder.DFORMUID = itemMaster.FORMMUID;
                                newOrder.PDSTSUID = itemMaster.PDSTSUID;
                                newOrder.QNUOMUID = itemMaster.BaseUOM;

                            }

                            newOrder.OrderSetUID = item.OrderSetUID;
                            newOrder.OrderSetBillableItemUID = item.OrderSetBillableItemUID;
                            newOrder.BillableItemUID = billItem.BillableItemUID;
                            newOrder.ItemName = billItem.ItemName;
                            newOrder.BSMDDUID = billItem.BSMDDUID;
                            newOrder.ItemUID = billItem.ItemUID;
                            newOrder.ItemCode = billItem.Code;
                            newOrder.BillingService = billItem.BillingServiceMetaData;
                            newOrder.UnitPrice = item.Price;

                            newOrder.PRSTYPUID = OrderTypes.FirstOrDefault(p => p.ValueCode == "ROMED").Key;
                            newOrder.OrderType = OrderTypes.FirstOrDefault(p => p.ValueCode == "ROMED").Display;

                            newOrder.DisplayPrice = item.Price;

                            newOrder.FRQNCUID = item.FRQNCUID;
                            newOrder.Quantity = item.Quantity;
                            newOrder.Dosage = item.DoseQty;
                            newOrder.Comments = item.ProcessingNotes;
                            newOrder.IsPriceOverwrite = "N";
                            newOrder.StartDttm = DateTime.Now;
                            newOrder.EndDttm = newOrder.StartDttm;

                            newOrder.NetAmount = ((item.Price) * item.Quantity);
                            newOrder.DoctorFeePer = item.DoctorFee;
                            newOrder.DoctorFee = (item.DoctorFee / 100) * newOrder.NetAmount;

                            if (item.DoctorFee != null && item.DoctorFee != 0)
                            {
                                if (item.CareproviderUID != null)
                                {
                                    newOrder.CareproviderUID = item.CareproviderUID;
                                    newOrder.CareproviderName = item.CareproviderName;
                                }
                                else
                                {
                                    newOrder.CareproviderUID = PatientVisit.CareProviderUID;
                                    newOrder.CareproviderName = PatientVisit.CareProviderName;
                                }
                            }
                            newOrder.OwnerOrganisationUID = ownerUID;

                            if (PatientOrderAlerts != null && PatientOrderAlerts.Count() > 0)
                                newOrder.PatientOrderAlert = PatientOrderAlerts;

                            PatientOrders.Add(newOrder);
                            OnUpdateEvent();
                        }
                    }
                }
                else
                {
                    PopUpOrder(orderItem.BillableItemUID);
                }
            }
            catch (Exception ex)
            {
                ErrorDialog(ex.Message);
            }

        }
        void PopUpOrder(int billableItemUID)
        {
            try
            {
                DateTime startDttm = StartDate.Add(StartTime.TimeOfDay);
                BillableItemModel billItem = DataService.MasterData.GetBillableItemByUID(billableItemUID);
                var orderAlready = PatientOrders.FirstOrDefault(p => p.BillableItemUID == billableItemUID);
                if (orderAlready != null)
                {
                    WarningDialog("รายการ " + orderAlready.ItemName + " นี้มีอยู่แล้ว โปรดตรวจสอบ");
                    if (billItem.BillingServiceMetaData == "Lab Test" || billItem.BillingServiceMetaData == "Radiology"
                        || billItem.BillingServiceMetaData == "Mobile Checkup")
                        return;
                }


                int ownerUID = AppUtil.Current.OwnerOrganisationUID;

                var billItemPrice = GetBillableItemPrice(billItem.BillableItemDetails, ownerUID);

                if (billItemPrice == null)
                {
                    WarningDialog("รายการ " + billItem.ItemName + " นี้ยังไม่ได้กำหนดราคาสำหรับขาย โปรดตรวจสอบ");
                    return;
                }

                billItem.Price = billItemPrice.Price;
                billItem.CURNCUID = billItemPrice.CURNCUID;

                if (billItem != null)
                {
                    var listOrderAlert = DataService.OrderProcessing.CriteriaOrderAlert(PatientVisit.PatientUID, billItem);
                    if (listOrderAlert != null && listOrderAlert.Count > 0)
                    {
                        OrderAlert viewOrderAlert = new OrderAlert();
                        OrderAlertViewModel viewModel = (OrderAlertViewModel)ShowModalDialogUsingViewModel(viewOrderAlert, new OrderAlertViewModel(listOrderAlert), true);
                        if (viewModel.ResultDialog != ActionDialog.Save)
                        {
                            return;
                        }
                        PatientOrderAlerts = viewModel.OrderAlerts;
                    }


                    switch (billItem.BillingServiceMetaData)
                    {
                        case "Lab Test":
                        case "Radiology":
                        case "Mobile Checkup":
                        case "Order Item":
                            {
                                OrderWithOutStockItem ordRe = new OrderWithOutStockItem(billItem, ownerUID, startDttm: startDttm);
                                OrderWithOutStockItemViewModel resultRe = (OrderWithOutStockItemViewModel)LaunchViewDialog(ordRe, "ORDLAB", true);
                                if (resultRe != null && resultRe.ResultDialog == ActionDialog.Save)
                                {
                                    if (PatientOrderAlerts != null && PatientOrderAlerts.Count() > 0)
                                        resultRe.PatientOrderDetail.PatientOrderAlert = PatientOrderAlerts;

                                    PatientOrders.Add(resultRe.PatientOrderDetail);
                                    OnUpdateEvent();
                                }
                                break;
                            }
                        case "Medical Supplies":
                        case "Supply":
                            OrderMedicalItem ordMed = new OrderMedicalItem(billItem, ownerUID, startDttm: startDttm);
                            OrderMedicalItemViewModel resultMed = (OrderMedicalItemViewModel)LaunchViewDialog(ordMed, "ORDMED", true);
                            if (resultMed != null && resultMed.ResultDialog == ActionDialog.Save)
                            {
                                if (PatientOrderAlerts != null && PatientOrderAlerts.Count() > 0)
                                    resultMed.PatientOrderDetail.PatientOrderAlert = PatientOrderAlerts;

                                PatientOrders.Add(resultMed.PatientOrderDetail);
                                OnUpdateEvent();
                            }
                            break;
                        case "Drug":
                            OrderDrugItem ordDrug = new OrderDrugItem(billItem, ownerUID, PatientVisit.ENTYPUID ?? 0, startDttm: startDttm);
                            OrderDrugItemViewModel resultDrug = (OrderDrugItemViewModel)LaunchViewDialog(ordDrug, "ORDDRG", true);
                            if (resultDrug != null && resultDrug.ResultDialog == ActionDialog.Save)
                            {
                                if (PatientOrderAlerts != null && PatientOrderAlerts.Count() > 0)
                                    resultDrug.PatientOrderDetail.PatientOrderAlert = PatientOrderAlerts;

                                if (resultDrug.PatientOrderDetail.IsStandingOrder == "Y")
                                {
                                    var orderNoContinuous = (PatientOrderDetailModel)resultDrug.PatientOrderDetail.CloneObject();
                                    orderNoContinuous.IsContinuous = "N";
                                    orderNoContinuous.StartDttm = DateTime.Now;
                                    orderNoContinuous.EndDttm = StartDate.Date.AddSeconds(86400);
                                    orderNoContinuous.IdentityGen = Guid.NewGuid().ToString();
                                    resultDrug.PatientOrderDetail.StandingPatientOrder = orderNoContinuous;
                                    PatientOrders.Add(resultDrug.PatientOrderDetail);
                                    PatientOrders.Add(resultDrug.PatientOrderDetail.StandingPatientOrder);

                                }
                                else
                                {
                                    PatientOrders.Add(resultDrug.PatientOrderDetail);
                                }

                                OnUpdateEvent();
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorDialog(ex.Message);
            }


        }

        void PopUpOrderEdit(PatientOrderDetailModel selectPatientOrder)
        {
            switch (selectPatientOrder.BillingService)
            {
                case "Lab Test":
                case "Radiology":
                case "Mobile Checkup":
                case "Order Item":
                    {
                        OrderWithOutStockItem ordRe = new OrderWithOutStockItem(selectPatientOrder);
                        OrderWithOutStockItemViewModel resultRe = (OrderWithOutStockItemViewModel)LaunchViewDialog(ordRe, "ORDLAB", true);
                        if (resultRe != null && resultRe.ResultDialog == ActionDialog.Save)
                        {
                            selectPatientOrder = resultRe.PatientOrderDetail;
                            OnUpdateEvent();
                        }
                        break;
                    }
                case "Medical Supplies":
                case "Supply":
                    OrderMedicalItem ordMed = new OrderMedicalItem(selectPatientOrder);
                    OrderMedicalItemViewModel resultMed = (OrderMedicalItemViewModel)LaunchViewDialog(ordMed, "ORDMED", true);
                    if (resultMed != null && resultMed.ResultDialog == ActionDialog.Save)
                    {
                        selectPatientOrder = resultMed.PatientOrderDetail;
                        OnUpdateEvent();
                    }
                    break;
                case "Drug":
                    OrderDrugItem ordDrug = new OrderDrugItem(selectPatientOrder, PatientVisit.ENTYPUID ?? 0);
                    OrderDrugItemViewModel resultDrug = (OrderDrugItemViewModel)LaunchViewDialog(ordDrug, "ORDDRG", true);
                    if (resultDrug != null && resultDrug.ResultDialog == ActionDialog.Save)
                    {
                        if (resultDrug.PatientOrderDetail.IsStandingOrder == "Y")
                        {
                            var orderNoContinuous = (PatientOrderDetailModel)resultDrug.PatientOrderDetail.CloneObject();
                            orderNoContinuous.IsContinuous = "N";
                            orderNoContinuous.StartDttm = DateTime.Now;
                            orderNoContinuous.EndDttm = StartDate.Date.AddSeconds(86400);
                            orderNoContinuous.IdentityGen = Guid.NewGuid().ToString();
                            resultDrug.PatientOrderDetail.StandingPatientOrder = orderNoContinuous;
                            selectPatientOrder = resultDrug.PatientOrderDetail;
                            PatientOrders.Add(resultDrug.PatientOrderDetail.StandingPatientOrder);

                        }
                        else
                        {
                            selectPatientOrder = resultDrug.PatientOrderDetail;
                        }



                        OnUpdateEvent();
                    }
                    break;
            }
            OnUpdateEvent();
        }
        public void AssingPatientVisit(PatientVisitModel visitModel, bool isbilling = false)
        {
            PatientVisit = visitModel;
            IsBilling = isbilling;
        }

        public BillableItemDetailModel GetBillableItemPrice(List<BillableItemDetailModel> billItmDetail, int ownerOrganisationUID)
        {
            BillableItemDetailModel selectBillItemDetail = null;

            if (billItmDetail.Count(p => p.OwnerOrganisationUID == ownerOrganisationUID) > 0)
            {
                selectBillItemDetail = billItmDetail
                    .FirstOrDefault(p => p.StatusFlag == "A" && p.OwnerOrganisationUID == ownerOrganisationUID
                    && (p.ActiveFrom == null || (p.ActiveFrom.Date <= DateTime.Now.Date))
                    && (p.ActiveTo == null || (p.ActiveTo.HasValue && p.ActiveTo.Value.Date >= DateTime.Now.Date))
                    );
            }

            return selectBillItemDetail;
        }
        #endregion
    }
}
