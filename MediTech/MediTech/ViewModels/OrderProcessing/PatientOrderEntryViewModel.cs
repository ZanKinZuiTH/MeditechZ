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

        private string _OrderSumDesc = "Order All = 0 list, Out Package = 0 list Price = 0";

        public string OrderSumDesc
        {
            get { return _OrderSumDesc; }
            set { Set(ref _OrderSumDesc, value); }
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

        private string _TotalExistingAmount;

        public string TotalExistingAmount
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


        private string _SearchPackageCriteria;

        public string SearchPackageCriteria
        {
            get { return _SearchPackageCriteria; }
            set
            {
                Set(ref _SearchPackageCriteria, value);
                if (!string.IsNullOrEmpty(_SearchPackageCriteria) && _SearchPackageCriteria.Length >= 3)
                {

                    BillPackages = DataService.OrderProcessing.SearchBillPackage(_SearchPackageCriteria
                        , SelectCategoryPackage != null ? SelectCategoryPackage.OrderCategoryUID : (int?)null
                        , SelectSubCategoryPackage != null ? SelectSubCategoryPackage.OrderSubCategoryUID : (int?)null);

                }
                else
                {
                    BillPackages = null;
                }
            }
        }

        private List<BillPackageModel> _BillPackages;

        public List<BillPackageModel> BillPackages
        {
            get { return _BillPackages; }
            set { Set(ref _BillPackages, value); }
        }

        private BillPackageModel _SelectBillPackage;

        public BillPackageModel SelectBillPackage
        {
            get { return _SelectBillPackage; }
            set { Set(ref _SelectBillPackage, value); }
        }


        private List<LookupReferenceValueModel> _BillingCategory;

        public List<LookupReferenceValueModel> BillingCategory
        {
            get { return _BillingCategory; }
            set { _BillingCategory = value; }
        }


        private List<OrderCategoryModel> _CategoryPackages;
        public List<OrderCategoryModel> CategoryPackages
        {
            get { return _CategoryPackages; }
            set { Set(ref _CategoryPackages, value); }
        }

        private OrderCategoryModel _SelectCategoryPackage;
        public OrderCategoryModel SelectCategoryPackage
        {
            get { return _SelectCategoryPackage; }
            set
            {
                Set(ref _SelectCategoryPackage, value);
                if (_SelectCategoryPackage != null)
                {
                    SubCategoryPackages = DataService.MasterData.GetOrderSubCategoryByUID(_SelectCategoryPackage.OrderCategoryUID);
                }
            }
        }

        private List<OrderSubCategoryModel> _SubCategoryPackages;
        public List<OrderSubCategoryModel> SubCategoryPackages
        {
            get { return _SubCategoryPackages; }
            set
            {
                Set(ref _SubCategoryPackages, value);

            }
        }

        private OrderSubCategoryModel _SelectSubCategoryPackage;
        public OrderSubCategoryModel SelectSubCategoryPackage
        {
            get { return _SelectSubCategoryPackage; }
            set { Set(ref _SelectSubCategoryPackage, value); }
        }

        private DateTime _StartPackageDate;

        public DateTime StartPackageDate
        {
            get { return _StartPackageDate; }
            set { Set(ref _StartPackageDate, value); }
        }

        private Visibility _IsVisibilityUsedPackage = Visibility.Collapsed;

        public Visibility IsVisibilityUsedPackage
        {
            get { return _IsVisibilityUsedPackage; }
            set { Set(ref _IsVisibilityUsedPackage, value); }
        }

        private string _UsedPackageCount;

        public string UsedPackageCount
        {
            get { return _UsedPackageCount; }
            set { Set(ref _UsedPackageCount, value); }
        }


        private List<PatientPackageModel> _UsedPackages;

        public List<PatientPackageModel> UsedPackages
        {
            get { return _UsedPackages; }
            set
            {
                Set(ref _UsedPackages, value);
                if (_UsedPackages != null && _UsedPackages.Count > 0)
                {
                    IsVisibilityUsedPackage = Visibility.Visible;
                    UsedPackagedDescription = string.Format("Package {0} List. Prices = {1:#,#.00} ", _UsedPackages.Count(), Math.Round(_UsedPackages.Sum(p => p.TotalAmount) ?? 0, 2));
                    UsedPackageCount = _UsedPackages.Count().ToString();
                }
                else
                {
                    IsVisibilityUsedPackage = Visibility.Collapsed;
                    UsedPackagedDescription = "";
                    UsedPackageCount = "";
                }
                }
            }

        private PatientPackageModel _SelectUsedPackage;

        public PatientPackageModel SelectUsedPackage
        {
            get { return _SelectUsedPackage; }
            set
            {
                Set(ref _SelectUsedPackage, value);
                if (SelectUsedPackage != null)
                {
                    AdjustOrderDetailForPackage();
                }
                else
                {
                    OrderPackageBy = "";
                    UsedPackagePrice = "";
                    BillPackageItems = null;
                    AdjustablePackageItems = null;
                }
            }
        }

        private string _OrderPackageBy;

        public string OrderPackageBy
        {
            get { return _OrderPackageBy; }
            set { Set(ref _OrderPackageBy, value); }
        }

        private string _UsedPackagePrice;

        public string UsedPackagePrice
        {
            get { return _UsedPackagePrice; }
            set { Set(ref _UsedPackagePrice, value); }
        }


        private ObservableCollection<PatientPackageItemModel> _BillPackageItems;

        public ObservableCollection<PatientPackageItemModel> BillPackageItems
        {
            get { return _BillPackageItems; }
            set { Set(ref _BillPackageItems, value); }
        }

        private PatientPackageItemModel _SelectBillPackageItem;

        public PatientPackageItemModel SelectBillPackageItem
        {
            get { return _SelectBillPackageItem; }
            set { Set(ref _SelectBillPackageItem, value); }
        }

        private List<AdjustablePackageItemModel> _AdjustablePackageItems;

        public List<AdjustablePackageItemModel> AdjustablePackageItems
        {
            get { return _AdjustablePackageItems; }
            set { Set(ref _AdjustablePackageItems, value); }
        }

        private string _UsedPackagedDescription;

        public string UsedPackagedDescription
        {
            get { return _UsedPackagedDescription; }
            set { Set(ref _UsedPackagedDescription, value); }
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


        private RelayCommand _LoadedPackangeCommand;
        public RelayCommand LoadedPackangeCommand
        {
            get
            {
                return _LoadedPackangeCommand ?? (_LoadedPackangeCommand = new RelayCommand(LoadedPackange));
            }
        }

        private RelayCommand _AddPackageCommand;
        public RelayCommand AddPackageCommand
        {
            get
            {
                return _AddPackageCommand ?? (_AddPackageCommand = new RelayCommand(AddPackage));
            }
        }

        private RelayCommand _DeletePackageCommand;
        public RelayCommand DeletePackageCommand
        {
            get
            {
                return _DeletePackageCommand ?? (_DeletePackageCommand = new RelayCommand(DeletePackage));
            }
        }

        private RelayCommand _SelectAllPackageCommand;
        public RelayCommand SelectAllPackageCommand
        {
            get
            {
                return _SelectAllPackageCommand ?? (_SelectAllPackageCommand = new RelayCommand(SelectAllPackage));
            }
        }


        private RelayCommand _OrderPackageCommand;
        public RelayCommand OrderPackageCommand
        {
            get
            {
                return _OrderPackageCommand ?? (_OrderPackageCommand = new RelayCommand(OrderPackage));
            }
        }


        private RelayCommand _UnlinkPackageCommand;
        public RelayCommand UnlinkPackageCommand
        {
            get
            {
                return _UnlinkPackageCommand ?? (_UnlinkPackageCommand = new RelayCommand(UnlinkPackage));
            }
        }

        private RelayCommand _EditPackageItemCommand;
        public RelayCommand EditPackageItemCommand
        {
            get
            {
                return _EditPackageItemCommand ?? (_EditPackageItemCommand = new RelayCommand(EditPackageItem));
            }
        }

        #endregion

        #region Variable

        bool IsExisitsOrderOneceLoad = false;

        bool IsPackageOneceLoad = false;

        List<LookupReferenceValueModel> OrderTypes;

        List<LookupReferenceValueModel> Priorities;
        #endregion

        #region Method


        public PatientOrderEntryViewModel()
        {
            var refVale = DataService.Technical.GetReferenceValueList("PRSTYP,PBLCT,RQPRT");
            OrderTypes = refVale.Where(p => p.DomainCode == "PRSTYP").ToList();
            Priorities = refVale.Where(p => p.DomainCode == "RQPRT").ToList();
            BillingCategory = refVale.Where(p => p.DomainCode == "PBLCT").ToList();

            PatientOrders.CollectionChanged += PatientOrders_CollectionChanged;
        }

        private void PatientOrders_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            IsEnableOrderFrom = true;
            if (_PatientOrders != null || _PatientOrders.Count > 0)
            {
                IsEnableOrderFrom = false;
            }
            OrderSumDesc = String.Format("Order All = {0} list, Out Package = {1} list Price = {2:#,#.00}", PatientOrders.Count()
                , PatientOrders.Count(p => p.BillPackageUID == null), PatientOrders.Where(p => p.BillPackageUID == null).Sum(p => p.NetAmount));
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

            UsedPackages = DataService.OrderProcessing.GetPatientPackageByVisitUID(PatientVisit.PatientVisitUID);
            SelectUsedPackage = _UsedPackages?.FirstOrDefault();
            (this.View as PatientOrderEntry).txtOrder.Focus();
        }

        public void LoadedExisting()
        {
            if (!IsExisitsOrderOneceLoad)
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
                IsExisitsOrderOneceLoad = true;
            }
        }

        public void CancelOrder()
        {
            try
            {
                if (SelectExistingOrder != null)
                {
                    if (SelectExistingOrder.Count(p => p.ORDSTUID == 2861) > 0)
                    {
                        WarningDialog("รายที่เลือก มีสถานะ Dispensed กรุณาไปทำการยกเลิกที่หน้า ใบสั่งยา");
                    }
                    List<PatientOrderDetailModel> existingOrderNotCancel = SelectExistingOrder.Where(p => p.ORDSTUID != 2848
                    && p.ORDSTUID != 2861 && p.PaymentStatus != "Ignore" && p.PaymentStatus != "Billed").ToList();

                    if (existingOrderNotCancel != null && existingOrderNotCancel.Count > 0)
                    {
                        CancelOrder cancelOrderView = new CancelOrder(existingOrderNotCancel);
                        CancelOrderViewModel result = (CancelOrderViewModel)LaunchViewDialog(cancelOrderView, "CANORD", true);
                        if (result != null && result.ResultDialog == ActionDialog.Save)
                        {
                            SearchExistingOrder();
                            AdjustOrderDetailForPackage();
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

            double totalPackage = UsedPackages.Sum(p => p.TotalAmount) ?? 0;
            double totalNotPackage = ExistingOrders.Where(p => p.ORDSTUID != 2848 && (p.BillPackageUID ?? 0) == 0).Sum(p => p.NetAmount) ?? 0;
            double totalAmount = totalPackage + totalNotPackage;
            TotalExistingAmount = string.Format("Total in Pakcage = {0:#,#.00},Total Not Package = {1:#,#.00},Total amount = {2:#,#.00}", totalPackage, totalNotPackage, totalAmount);
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


        public void LoadedPackange()
        {
            if (!IsPackageOneceLoad)
            {
                StartPackageDate = DateTime.Now;
                CategoryPackages = DataService.MasterData.GetOrderCategory();
                IsPackageOneceLoad = true;
            }

        }

        public void AddPackage()
        {
            try
            {
                if (SelectBillPackage != null)
                {
                    if (UsedPackages != null && UsedPackages.FirstOrDefault(p => p.BillPackageUID == SelectBillPackage.BillPackageUID) != null)
                    {
                        var result = QuestionDialog("Package นี้การมีคีย์ใช้แล้ว คุณต้องการดำเนินการต่อ หรือไม่ ?");
                        if (result != MessageBoxResult.Yes)
                        {
                            return;
                        }
                    }
                    PatientPackageModel newPatPackage = new PatientPackageModel();
                    newPatPackage.PatientUID = PatientVisit.PatientUID;
                    newPatPackage.PatientVisitUID = PatientVisit.PatientVisitUID;
                    newPatPackage.PackageName = SelectBillPackage.PackageName;
                    newPatPackage.BillPackageUID = SelectBillPackage.BillPackageUID;
                    newPatPackage.ActiveFrom = SelectBillPackage.ActiveFrom;
                    newPatPackage.ActiveTo = SelectBillPackage.ActiveTo;
                    newPatPackage.PackageCreatedByUID = AppUtil.Current.UserID;
                    newPatPackage.IsConsiderItem = "N";
                    newPatPackage.TotalAmount = SelectBillPackage.TotalAmount;
                    newPatPackage.Qty = 1;
                    newPatPackage.OwnerOrganisationUID = PatientVisit.OwnerOrganisationUID ?? 0;
                    newPatPackage.CUser = AppUtil.Current.UserID;
                    newPatPackage.MUser = AppUtil.Current.UserID;
                    DataService.OrderProcessing.AddPatientPackage(newPatPackage);


                    UsedPackages = DataService.OrderProcessing.GetPatientPackageByVisitUID(PatientVisit.PatientVisitUID);

                    SelectUsedPackage = _UsedPackages.OrderByDescending(p => p.PatientPackageUID).FirstOrDefault();

                    SearchPackageCriteria = String.Empty;
                    SelectBillPackage = null;
                }
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }

        }

        public void DeletePackage()
        {
            try
            {
                
                if (SelectUsedPackage != null)
                {
                    if(SelectUsedPackage.BillPackageDetails.Count(p => p.UsedQuantity > 0) > 0)
                    {
                        WarningDialog("ไม่สามรถลบ Package ได้ ต้องทำการ Unlink Order ทั้งหมดก่อน");
                        return;
                    }
                    var result = QuestionDialog(String.Format("ต้องการรบ Package {0} หรือไม่", SelectUsedPackage.PackageName));
                    if (result == MessageBoxResult.Yes)
                    {
                        DataService.OrderProcessing.DeletePatientPackage(SelectUsedPackage.PatientPackageUID, AppUtil.Current.UserID);

                        UsedPackages = DataService.OrderProcessing.GetPatientPackageByVisitUID(PatientVisit.PatientVisitUID);

                        SelectUsedPackage = _UsedPackages.OrderByDescending(p => p.PatientPackageUID).FirstOrDefault();
                    }
                }
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }

        public void SelectAllPackage()
        {
            if (BillPackageItems.Count(p => p.IsSelected) != BillPackageItems.Count())
            {
                foreach (var item in BillPackageItems)
                {
                    item.IsSelected = true;
                }
            }
            else
            {
                foreach (var item in BillPackageItems)
                {
                    item.IsSelected = false;
                }
            }

            OnUpdateEvent();
        }

        private void EditPackageItem()
        {
            if (SelectBillPackageItem != null)
            {
                AdjustOrderDetailForPackageViewModel viewModel = new AdjustOrderDetailForPackageViewModel(SelectUsedPackage.PackageName, OrderPackageBy, SelectUsedPackage.PatientPackageUID, SelectBillPackageItem.BillableItemUID,PatientVisit.PatientVisitUID);
                AdjustOrderDetailForPackage view = new AdjustOrderDetailForPackage();
                view.DataContext = viewModel;
                var result = (AdjustOrderDetailForPackageViewModel)this.LaunchViewDialogNonPermiss(view,false);
                if (result != null && result.ResultDialog == ActionDialog.Save)
                {
                    long patientPackageUID = SelectUsedPackage.PatientPackageUID;
                    UsedPackages = DataService.OrderProcessing.GetPatientPackageByVisitUID(PatientVisit.PatientVisitUID);
                    SelectUsedPackage = _UsedPackages?.FirstOrDefault(p => p.PatientPackageUID == patientPackageUID);
                }
            }
        }

        public void OrderPackage()
        {

            foreach (var item in BillPackageItems)
            {
                if (item.IsSelected && PatientOrders.Count(p => p.BillableItemUID == item.BillableItemUID && p.PatientPackageUID == item.PatientPackageUID) <= 0)
                {
                    int ownerUID = AppUtil.Current.OwnerOrganisationUID;
                    PatientOrderDetailModel newOrder = new PatientOrderDetailModel();
                    BillableItemModel billItem = DataService.MasterData.GetBillableItemByUID(item.BillableItemUID);
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
                                if (store.Quantity >= item.ItemMultiplier)
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

                        if (item.ItemMultiplier <= 0)
                        {
                            WarningDialog("ไม่อนุญาติให้คีย์ + " + billItem.ItemName + " จำนวน < 0");
                            continue;
                        }

                        if (itemMaster.MinSalesQty != null && item.ItemMultiplier < itemMaster.MinSalesQty)
                        {
                            WarningDialog("คีย์จำนวน " + billItem.ItemName + " ที่ใช้น้อยกว่าจำนวนขั้นต่ำที่คีย์ได้ โปรดตรวจสอบ");
                            continue;
                        }


                        newOrder.IsStock = itemMaster.IsStock;
                        newOrder.StoreUID = stores.Count(p => p.Quantity >= item.ItemMultiplier) > 0 ? stores.Where(p => p.Quantity >= item.ItemMultiplier)?.FirstOrDefault().StoreUID : (int?)null;
                        newOrder.DFORMUID = itemMaster.FORMMUID;
                        newOrder.ROUTEUID = itemMaster.ROUTEUID;
                        newOrder.PDSTSUID = itemMaster.PDSTSUID;
                        newOrder.QNUOMUID = itemMaster.BaseUOM;
                        newOrder.Dosage = itemMaster.DoseQuantity;
                    }

                    newOrder.BillPackageUID = item.BillPackageUID;
                    newOrder.PatientPackageUID = item.PatientPackageUID;
                    newOrder.PatientPackageItemUID = item.PatientPackageItemUID;
                    newOrder.Package_OrderSet_Name = SelectUsedPackage.PackageName;
                    newOrder.BillableItemUID = billItem.BillableItemUID;
                    newOrder.ItemName = billItem.ItemName;
                    newOrder.BSMDDUID = billItem.BSMDDUID;
                    newOrder.ItemUID = billItem.ItemUID;
                    newOrder.ItemCode = billItem.Code;
                    newOrder.BillingService = billItem.BillingServiceMetaData;
                    newOrder.UnitPrice = item.Amount;
                    newOrder.OriginalUnitPrice = item.Amount;
                    newOrder.OrderCatagoryUID = billItem.OrderCategoryUID;
                    newOrder.OrderSubCategoryUID = billItem.OrderSubCategoryUID;

                    newOrder.ORDPRUID = Priorities.FirstOrDefault(p => p.ValueCode == "NORML").Key;
                    newOrder.PRSTYPUID = OrderTypes.FirstOrDefault(p => p.ValueCode == "ROMED").Key;
                    newOrder.OrderType = OrderTypes.FirstOrDefault(p => p.ValueCode == "ROMED").Display;

                    newOrder.DisplayPrice = item.Amount;

                    newOrder.Quantity = item.ItemMultiplier;

                    newOrder.IsPriceOverwrite = "N";
                    newOrder.StartDttm = DateTime.Now;
                    newOrder.EndDttm = newOrder.StartDttm?.AddDays(1);

                    newOrder.NetAmount = ((item.Amount) * item.ItemMultiplier);
                    newOrder.DoctorFeePer = billItem.DoctorFee;
                    newOrder.DoctorFee = (billItem.DoctorFee / 100) * newOrder.NetAmount;

                    newOrder.OwnerOrganisationUID = ownerUID;

                    if (PatientOrderAlerts != null && PatientOrderAlerts.Count() > 0)
                        newOrder.PatientOrderAlert = PatientOrderAlerts;

                    PatientOrders.Add(newOrder);
                    OnUpdateEvent();
                }
            }
            SelectTabIndex = 0;


        }

        public void UnlinkPackage()
        {
            try
            {
                var result = QuestionDialog("คุณต้องการ ยกรายการทั้งหมดออกจาก Package ใช้หรือไม่?");
                if (result == MessageBoxResult.Yes)
                {
                    DataService.OrderProcessing.AdjustOrderDetailForPackage(PatientVisit.PatientUID, PatientVisit.PatientVisitUID, SelectUsedPackage.PatientPackageUID, 0, null);
                    AdjustOrderDetailForPackage();
                }
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }

        void AdjustOrderDetailForPackage()
        {
            if (SelectUsedPackage != null)
            {
                BillPackageItems = new ObservableCollection<PatientPackageItemModel>(SelectUsedPackage.BillPackageDetails);
                AdjustablePackageItems = DataService.OrderProcessing.GetAdjustablePackageItems(PatientVisit.PatientUID, PatientVisit.PatientVisitUID, SelectUsedPackage.PatientPackageUID);
                OrderPackageBy = string.Format("    -   {0}   -   {1}", SelectUsedPackage.PackageCreatedByName, SelectUsedPackage.PackageCreatedDttm.ToString("dd-MM-yyyy HH:MM tt"));
                UsedPackagePrice = String.Format("{0:#,#.00}", SelectUsedPackage.TotalAmount?.ToString());

                foreach (var item in BillPackageItems)
                {
                    double? outPackageQuantity = null;
                    double? UsedQuantity = null;
                    if (AdjustablePackageItems != null && AdjustablePackageItems.Count(p => p.BillableItemUID == item.BillableItemUID) > 0)
                    {
                        outPackageQuantity = AdjustablePackageItems.Where(p => p.BillableItemUID == item.BillableItemUID && p.BillPackageUID == 0).Sum(p => p.Quantity);
                        UsedQuantity = AdjustablePackageItems.Where(p => p.BillableItemUID == item.BillableItemUID && p.BillPackageUID == item.PatientPackageUID).Sum(p => p.Quantity);
                    }
                    item.OutPackageQuantity = outPackageQuantity;
                    item.UsedQuantity = UsedQuantity;
                    item.Lest_Over_Quantity = item.ItemMultiplier - item.UsedQuantity ?? 0;
                }
                OnUpdateEvent();
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

                            List<PatientOrderAlertModel> listOrderAlert = DataService.OrderProcessing.CriteriaOrderAlert(PatientVisit.PatientUID, PatientVisit.PatientVisitUID, billItem);
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
                                        if (store.Quantity >= item.Quantity)
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
                                newOrder.StoreUID = stores.Count(p => p.Quantity >= item.Quantity) > 0 ? stores.Where(p => p.Quantity >= item.Quantity)?.FirstOrDefault().StoreUID : (int?)null;
                                newOrder.DFORMUID = itemMaster.FORMMUID;
                                newOrder.ROUTEUID = itemMaster.ROUTEUID;
                                newOrder.PDSTSUID = itemMaster.PDSTSUID;
                                newOrder.QNUOMUID = itemMaster.BaseUOM;


                            }

                            newOrder.OrderSetUID = item.OrderSetUID;
                            newOrder.OrderSetBillableItemUID = item.OrderSetBillableItemUID;
                            newOrder.BillableItemUID = billItem.BillableItemUID;
                            newOrder.Package_OrderSet_Name = orderSet.Name;
                            newOrder.ItemName = billItem.ItemName;
                            newOrder.BSMDDUID = billItem.BSMDDUID;
                            newOrder.ItemUID = billItem.ItemUID;
                            newOrder.ItemCode = billItem.Code;
                            newOrder.BillingService = billItem.BillingServiceMetaData;
                            newOrder.UnitPrice = item.Price;
                            newOrder.OriginalUnitPrice = item.Price;
                            newOrder.OrderCatagoryUID = billItem.OrderCategoryUID;
                            newOrder.OrderSubCategoryUID = billItem.OrderSubCategoryUID;

                            newOrder.ORDPRUID = Priorities.FirstOrDefault(p => p.ValueCode == "NORML").Key;
                            newOrder.PRSTYPUID = OrderTypes.FirstOrDefault(p => p.ValueCode == "ROMED").Key;
                            newOrder.OrderType = OrderTypes.FirstOrDefault(p => p.ValueCode == "ROMED").Display;

                            newOrder.DisplayPrice = item.Price;

                            newOrder.FRQNCUID = item.FRQNCUID;
                            newOrder.Quantity = item.Quantity;
                            newOrder.Dosage = item.DoseQty;
                            newOrder.Comments = item.ProcessingNotes;
                            newOrder.IsPriceOverwrite = "N";
                            newOrder.StartDttm = DateTime.Now;
                            newOrder.EndDttm = newOrder.StartDttm?.AddDays(1);

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

                var patientVisitPayors = DataService.PatientIdentity.GetPatientVisitPayorByVisitUID(PatientVisit.PatientVisitUID);

                var firstVisitPayors = patientVisitPayors.FirstOrDefault();

                int? PBLCTUID = BillingCategory.FirstOrDefault(p => p.ValueCode == "OPDTRF")?.Key;

                if (firstVisitPayors != null)
                {
                    PBLCTUID = firstVisitPayors.PrimaryPBLCTUID;
                }

                var billItemPrice = GetBillableItemPrice(billItem.BillableItemDetails, PBLCTUID, ownerUID);

                if (billItemPrice == null)
                {
                    WarningDialog("รายการ " + billItem.ItemName + " นี้ยังไม่ได้กำหนดราคาสำหรับขาย โปรดตรวจสอบ");
                    return;
                }

                billItem.Price = billItemPrice.Price;
                billItem.CURNCUID = billItemPrice.CURNCUID;

                if (billItem != null)
                {
                    var listOrderAlert = DataService.OrderProcessing.CriteriaOrderAlert(PatientVisit.PatientUID, PatientVisit.PatientVisitUID, billItem);
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
            OrderSumDesc = String.Format("Order All = {0} list, Out Package = {1} list Price = {2:#,#.00}", PatientOrders.Count()
    , PatientOrders.Count(p => p.BillPackageUID == null), PatientOrders.Where(p => p.BillPackageUID == null).Sum(p => p.NetAmount));
        }
        public void AssingPatientVisit(PatientVisitModel visitModel, bool isbilling = false)
        {
            PatientVisit = visitModel;
            IsBilling = isbilling;
        }

        public BillableItemDetailModel GetBillableItemPrice(List<BillableItemDetailModel> billItmDetail, int? PBLCTUID, int ownerOrganisationUID)
        {
            BillableItemDetailModel selectBillItemDetail = null;

            if (billItmDetail.Count(p => p.OwnerOrganisationUID == ownerOrganisationUID) > 0)
            {
                selectBillItemDetail = billItmDetail
                    .FirstOrDefault(p => p.StatusFlag == "A"
                    && p.PBLCTUID == PBLCTUID
                    && p.OwnerOrganisationUID == ownerOrganisationUID
                    && (p.ActiveFrom == null || (p.ActiveFrom.Date <= DateTime.Now.Date))
                    && (p.ActiveTo == null || (p.ActiveTo.HasValue && p.ActiveTo.Value.Date >= DateTime.Now.Date))
                    );
            }

            return selectBillItemDetail;
        }
        #endregion
    }
}
