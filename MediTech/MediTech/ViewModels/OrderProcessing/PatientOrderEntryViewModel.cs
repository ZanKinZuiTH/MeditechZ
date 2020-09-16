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

namespace MediTech.ViewModels
{
    public class PatientOrderEntryViewModel : MediTechViewModelBase
    {

        #region Properites

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
                    if (SelectHealthOrganisation != null)
                    {
                        ownerOrganisationUID = SelectHealthOrganisation.HealthOrganisationUID;
                    }
                    OrderItems = DataService.OrderProcessing.SearchOrderItem(_SearchOrderCriteria, ownerOrganisationUID);

                }
                else
                {
                    OrderItems = null;
                }
            }
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

        private List<HealthOrganisationModel> _HealthOrganisations;

        public List<HealthOrganisationModel> HealthOrganisations
        {
            get { return _HealthOrganisations; }
            set { Set(ref _HealthOrganisations, value); }
        }

        private HealthOrganisationModel _SelectHealthOrganisation;

        public HealthOrganisationModel SelectHealthOrganisation
        {
            get { return _SelectHealthOrganisation; }
            set
            {
                Set(ref _SelectHealthOrganisation, value);
                if (SelectHealthOrganisation == null)
                {
                    EnableSearchItem = false;
                }
                else
                {
                    EnableSearchItem = true;
                }
            }
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
            set { Set(ref _PatientOrders, value); }
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

                        if (selectVisit.VISTSUID != 410 && selectVisit.VISTSUID != 421)
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

        private bool _EnableSearchItem = false;

        public bool EnableSearchItem
        {
            get { return _EnableSearchItem; ; }
            set { Set(ref _EnableSearchItem, value); }
        }

        private bool _EnabledCancelOrder;

        public bool EnabledCancelOrder
        {
            get { return _EnabledCancelOrder; ; }
            set { Set(ref _EnabledCancelOrder, value); }
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

        #endregion

        #region Variable

        bool IsOneceLoad = false;

        #endregion

        #region Method

        public override void OnLoaded()
        {
            Careproviders = DataService.UserManage.GetCareproviderAll();
            SelectCareprovider = Careproviders.FirstOrDefault(p => p.CareproviderUID == AppUtil.Current.UserID);

            HealthOrganisations = GetHealthOrganisationRoleMedical();
            SelectHealthOrganisation = HealthOrganisations.FirstOrDefault(p => p.HealthOrganisationUID == PatientVisit.OwnerOrganisationUID);

            if (SelectHealthOrganisation == null)
            {
                SelectHealthOrganisation = HealthOrganisations.FirstOrDefault(p => p.HealthOrganisationUID == AppUtil.Current.OwnerOrganisationUID);
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
                        Display = p.VisitID + " : " + p.StartDttm.Value.ToString("dd/MM/yyyy") + " - " + (p.EndDttm.HasValue ? p.EndDttm.Value.ToString("dd/MM/yyyy") : "[N/A]")
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
                    List<PatientOrderDetailModel> existingOrderNotCancel = SelectExistingOrder.Where(p => p.ORDSTUID != 2848).ToList();
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
                    if (SelectHealthOrganisation == null)
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
        void SearchExistingOrder()
        {
            ExistingOrders = new ObservableCollection<PatientOrderDetailModel>(DataService.OrderProcessing.GetOrderAllByVisitUID(SelectLookupVisit.Key2, DateExitingFrom, DateExitingTo));

            TotalExistingAmount = ExistingOrders.Where(p => p.ORDSTUID != 2848).Sum(p => p.NetAmount);
        }

        private void ExecuteDeleteRow()
        {
            if (SelectPatientOrder != null)
            {
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
                if (PatientOrders != null && PatientOrders.Count > 0)
                {
                    int userUID = AppUtil.Current.UserID;
                    int owerOrganisationUID = 0;
                    //if (SelectHealthOrganisation != null)
                    //{
                    //    owerOrganisationUID = SelectHealthOrganisation.HealthOrganisationUID;
                    //}
                    //else
                    //{
                    //    owerOrganisationUID = PatientVisit.OwnerOrganisationUID ?? userUID;
                    //}
                    owerOrganisationUID = PatientVisit.OwnerOrganisationUID ?? userUID;
                    string orderNumber = DataService.OrderProcessing.CreateOrder(PatientVisit.PatientUID, PatientVisit.PatientVisitUID, userUID, owerOrganisationUID, PatientOrders.ToList());
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
                DialogResult result = QuestionDialog("คุณต้องการ Clear Order ทั้งหมดใช้หรือไม่ ?");
                if (result == DialogResult.Yes)
                {
                    PatientOrders = null;
                }
            }


        }

        void ApplyOrderItem(SearchOrderItem orderItem)
        {
            try
            {
                if (SelectHealthOrganisation == null)
                {
                    WarningDialog("กรุณาเลือก Order จาก");
                    return;
                }

                if (orderItem.TypeOrder == "OrderSet")
                {
                    OrderSetModel orderSet = DataService.MasterData.GetOrderSetByUID(orderItem.BillableItemUID);
                    int? ownerUID = SelectHealthOrganisation != null ? SelectHealthOrganisation.HealthOrganisationUID : (int?)null;
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

                                stores = DataService.Inventory.GetStockRemainByItemMasterUID(itemMaster.ItemMasterUID, ownerUID ?? 0);

                                if (stores == null || stores.Count <= 0)
                                {
                                    WarningDialog("ไม่มี " + billItem.ItemName + " ในคลัง โปรดตรวจสอบ");
                                    continue;
                                }
                                else
                                {
                                    if (item.Quantity > stores.FirstOrDefault().Quantity)
                                    {
                                        if (itemMaster.CanDispenseWithOutStock != "Y")
                                        {
                                            WarningDialog("มี " + billItem.ItemName + " ในคลังไม่พอสำหรับจ่ายยา โปรดตรวจสอบ");
                                            continue;

                                        }
                                        else if (itemMaster.CanDispenseWithOutStock == "Y")
                                        {
                                            DialogResult result = QuestionDialog("มี" + billItem.ItemName + "ในคลังไม่พอ คุณต้องการดำเนินการต่อหรือไม่ ?");
                                            if (result == DialogResult.No || result == DialogResult.Cancel)
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
                                newOrder.StoreUID = stores.FirstOrDefault().StoreUID;
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
                            newOrder.DoctorFee = (item.DoctorFee / 100) * item.Price;
                            newOrder.DisplayPrice = item.Price;

                            newOrder.FRQNCUID = item.FRQNCUID;
                            newOrder.Quantity = item.Quantity;
                            newOrder.Dosage = item.DoseQty;
                            newOrder.Comments = item.ProcessingNotes;
                            newOrder.IsPriceOverwrite = "N";
                            newOrder.StartDttm = DateTime.Now;

                            newOrder.NetAmount = ((item.Price) * item.Quantity);

                            newOrder.OwnerOrganisationUID = ownerUID ?? 0;

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
                var orderAlready = PatientOrders.FirstOrDefault(p => p.BillableItemUID == billableItemUID);
                if (orderAlready != null)
                {
                    WarningDialog("รายการ " + orderAlready.ItemName + " นี้มีอยู่แล้ว โปรดตรวจสอบ");
                    return;
                }
                BillableItemModel billItem = DataService.MasterData.GetBillableItemByUID(billableItemUID);

                int? ownerUID = SelectHealthOrganisation != null ? SelectHealthOrganisation.HealthOrganisationUID : (int?)null;

                var billItemPrice = GetBillableItemPrice(billItem.BillableItemDetails, ownerUID ?? 0);

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
                        case "Order Item":
                            {
                                OrderWithOutStockItem ordRe = new OrderWithOutStockItem(billItem);
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
                            OrderMedicalItem ordMed = new OrderMedicalItem(billItem, ownerUID);
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
                            OrderDrugItem ordDrug = new OrderDrugItem(billItem, ownerUID);
                            OrderDrugItemViewModel resultDrug = (OrderDrugItemViewModel)LaunchViewDialog(ordDrug, "ORDDRG", true);
                            if (resultDrug != null && resultDrug.ResultDialog == ActionDialog.Save)
                            {
                                if (PatientOrderAlerts != null && PatientOrderAlerts.Count() > 0)
                                    resultDrug.PatientOrderDetail.PatientOrderAlert = PatientOrderAlerts;

                                PatientOrders.Add(resultDrug.PatientOrderDetail);
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
                    OrderDrugItem ordDrug = new OrderDrugItem(selectPatientOrder);
                    OrderDrugItemViewModel resultDrug = (OrderDrugItemViewModel)LaunchViewDialog(ordDrug, "ORDDRG", true);
                    if (resultDrug != null && resultDrug.ResultDialog == ActionDialog.Save)
                    {
                        selectPatientOrder = resultDrug.PatientOrderDetail;
                        OnUpdateEvent();
                    }
                    break;
            }
            OnUpdateEvent();
        }
        public void AssingPatientVisit(PatientVisitModel visitModel)
        {
            PatientVisit = visitModel;
        }

        public BillableItemDetailModel GetBillableItemPrice(List<BillableItemDetailModel> billItmDetail, int ownerOrganisationUID)
        {
            BillableItemDetailModel selectBillItemDetail = null;

            if (billItmDetail.Count(p => p.OwnerOrganisationUID == ownerOrganisationUID) > 0)
            {
                selectBillItemDetail = billItmDetail
                    .FirstOrDefault(p => p.StatusFlag == "A" && p.OwnerOrganisationUID == ownerOrganisationUID
                    && (p.ActiveFrom == null || (p.ActiveFrom.HasValue && p.ActiveFrom.Value.Date <= DateTime.Now.Date))
                    && (p.ActiveTo == null || (p.ActiveTo.HasValue && p.ActiveTo.Value.Date >= DateTime.Now.Date))
                    );
            }
            else
            {
                selectBillItemDetail = billItmDetail
    .FirstOrDefault(p => p.StatusFlag == "A" && p.OwnerOrganisationUID == 0
    && (p.ActiveFrom == null || (p.ActiveFrom.HasValue && p.ActiveFrom.Value.Date <= DateTime.Now.Date))
    && (p.ActiveTo == null || (p.ActiveTo.HasValue && p.ActiveTo.Value.Date >= DateTime.Now.Date))
    );
            }

            return selectBillItemDetail;
        }
        #endregion
    }
}
