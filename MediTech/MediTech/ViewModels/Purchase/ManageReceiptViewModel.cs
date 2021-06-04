using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Model.Report;
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
    public class ManageReceiptViewModel : MediTechViewModelBase
    {
        #region Propoties
        public List<HealthOrganisationModel> _Organisations;
        public List<HealthOrganisationModel> Organisations
        {
            get { return _Organisations; }
            set { Set(ref _Organisations, value); }
        }

        private ObservableCollection<GroupReceiptDetailModel> _OrderGroupReceipt;
        public ObservableCollection<GroupReceiptDetailModel> OrderGroupReceipt
        {
            get { return _OrderGroupReceipt ?? (_OrderGroupReceipt = new ObservableCollection<GroupReceiptDetailModel>()); }
            set { Set(ref _OrderGroupReceipt, value); }
        }
        
        private GroupReceiptDetailModel _SelectOrderGroupReceipt;
        public GroupReceiptDetailModel SelectOrderGroupReceipt
        {
            get { return _SelectOrderGroupReceipt; }
            set
            {
                Set(ref _SelectOrderGroupReceipt, value);
            }
        }



        private HealthOrganisationModel _SelectOrganisation;

        public HealthOrganisationModel SelectOrganisation
        {
            get { return _SelectOrganisation; }
            set
            {
                Set(ref _SelectOrganisation, value);
                if (SelectOrganisation == null)
                {
                    EnableSearchItem = false;
                }
                else
                {
                    EnableSearchItem = true;
                }
            }
        }

        private ObservableCollection<GroupReceiptModel> _GroupReceiptOrders;

        public ObservableCollection<GroupReceiptModel> GroupReceiptOrders
        {
            get { return _GroupReceiptOrders ?? (_GroupReceiptOrders = new ObservableCollection<GroupReceiptModel>()); }
            set { Set(ref _GroupReceiptOrders, value); }
        }

        private GroupReceiptModel _model;

        public GroupReceiptModel model
        {
            get { return _model; }
            set { _model = value; }
        }

        public DateTime ? BillDate { get; set; }

        private List<PayorDetailModel> _PayorDetails;
        public List<PayorDetailModel> PayorDetails
        {
            get { return _PayorDetails; }
            set { Set(ref _PayorDetails, value); }
        }

        //private string _ReceiptNumber;
        //public string ReceiptNumber
        //{
        //    get { return _ReceiptNumber; }
        //    set { Set(ref _ReceiptNumber, value); }
        //}

        private double ? _SumTotalPrice;
        public double ? SumTotalPrice
        {
            get { return _SumTotalPrice; }
            set { Set(ref _SumTotalPrice, value); }
        }

        private double ? _TaxSum;
        public double ? TaxSum
        {
            get { return _TaxSum; }
            set { Set(ref _TaxSum, value); }
        }

        private double? _NetPrice;
        public double? NetPrice
        {
            get { return _NetPrice; }
            set { Set(ref _NetPrice, value); }
        }

        private double? _Discount;
        public double? Discount
        {
            get { return _Discount; }
            set { Set(ref _Discount, value); }
        }

        private string _Address;
        public string Address
        {
            get { return _Address; }
            set { Set(ref _Address, value); }
        }

        private string _TaxNumber;
        public string TaxNumber
        {
            get { return _TaxNumber; }
            set { Set(ref _TaxNumber, value); }
        }

        private string _Seller;
        public string Seller
        {
            get { return _Seller; }
            set { Set(ref _Seller, value); }
        }

        private PayorDetailModel _SelectPayorDetail;
        public PayorDetailModel SelectPayorDetail
        {
            get { return _SelectPayorDetail; }
            set
            {
                Set(ref _SelectPayorDetail, value);
                if (_SelectPayorDetail != null)
                {
                    AddressCustomer();
                }
            }
        }

        private PatientVisitModel _PatientVisit;
        public PatientVisitModel PatientVisit
        {
            get { return _PatientVisit; }
            set { Set(ref _PatientVisit, value); }
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
                    if (SelectOrganisation != null)
                    {
                        ownerOrganisationUID = SelectOrganisation.HealthOrganisationUID;
                    }
                    OrderItems = DataService.OrderProcessing.SearchOrderItem(_SearchOrderCriteria, ownerOrganisationUID);

                }
                else
                {
                    OrderItems = null;
                }
            }
        }

        private List<SearchOrderItem> _OrderItems;
        public List<SearchOrderItem> OrderItems
        {
            get { return _OrderItems; }
            set { Set(ref _OrderItems, value); }
        }

        private bool _EnableSearchItem = false;
        public bool EnableSearchItem
        {
            get { return _EnableSearchItem; ; }
            set { Set(ref _EnableSearchItem, value); }
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

        private Visibility _VisibilitySearchRequest = Visibility.Visible;

        public Visibility VisibilitySearchRequest
        {
            get { return _VisibilitySearchRequest; }
            set { Set(ref _VisibilitySearchRequest, value); }
        }

        public List<LookupReferenceValueModel> _TaxChoice;
        public List<LookupReferenceValueModel> TaxChoice
        {
            get { return _TaxChoice; }
            set
            {
                Set(ref _TaxChoice, value);
                
            }
        }

        #endregion

        #region Command
        private RelayCommand _SaveReceiptCommand;
        public RelayCommand SaveReceiptCommand
        {
            get
            {
                return _SaveReceiptCommand ?? (_SaveReceiptCommand = new RelayCommand(Save));
            }
        }

        private RelayCommand _CancelReceiptCommand;
        public RelayCommand CancelReceiptCommand
        {
            get
            {
                return _CancelReceiptCommand ?? (_CancelReceiptCommand = new RelayCommand(Cancel));
            }
        }

        private RelayCommand _DeleteItemCommand;
        public RelayCommand DeleteItemCommand
        {
            get
            {
                return _DeleteItemCommand ?? (_DeleteItemCommand = new RelayCommand(DeleteItem));
            }
        }

        private RelayCommand _EditItemCommand;
        public RelayCommand EditItemCommand
        {
            get
            {
                return _EditItemCommand ?? (_EditItemCommand = new RelayCommand(EditItem));
            }
        }

        private RelayCommand<DevExpress.Xpf.Grid.CellValueChangedEventArgs> _ChangeValueCommand;
        public RelayCommand<DevExpress.Xpf.Grid.CellValueChangedEventArgs> ChangeValueCommand
        {
            get { return _ChangeValueCommand ?? (_ChangeValueCommand = new RelayCommand<DevExpress.Xpf.Grid.CellValueChangedEventArgs>(ChangeValue)); }
        }

        private RelayCommand _AddPayorCommand;
        public RelayCommand AddPayorCommand
        {
            get
            {
                return _AddPayorCommand ?? (_AddPayorCommand = new RelayCommand(AddPayor));
            }
        }

        private RelayCommand _AddItemCommand;
        public RelayCommand AddItemCommand
        {
            get
            {
                return _AddItemCommand ?? (_AddItemCommand = new RelayCommand(AddItem));
            }
        }

        private RelayCommand _SearchPatientBillCommand;

        public RelayCommand SearchPatientBillCommand
        {
            get { return _SearchPatientBillCommand ?? (_SearchPatientBillCommand = new RelayCommand(SearchPatientBill)); }
        }

        #endregion

        #region Method
        public ManageReceiptViewModel()
        {
            DateTime now = DateTime.Now;
            Organisations = GetHealthOrganisationIsRoleStock();
            PayorDetails = DataService.MasterData.GetPayorDetail();
            BillDate = now.Date;
            SelectOrganisation = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == AppUtil.Current.OwnerOrganisationUID);

            if (SelectOrganisation == null)
            {
                int ownerOrganisationUID = 0;
                ownerOrganisationUID = SelectOrganisation.HealthOrganisationUID;
                SelectOrganisation = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == AppUtil.Current.OwnerOrganisationUID);
                OrderItems = DataService.OrderProcessing.SearchOrderItem(_SearchOrderCriteria, ownerOrganisationUID);
            }

            TaxChoice = new List<LookupReferenceValueModel>{
                new LookupReferenceValueModel { Key = 0, Display = "7%" },
                new LookupReferenceValueModel { Key = 1, Display = "ยกเลิกภาษี" }
            };

           
        }

        public void ChangeValue(DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
         {
            CalculateNetAmount();
        }
        void CalculateNetAmount()
        {
            double ? NetAmount = 0;
            double ? discout = 0;
            double? sumprice = 0;
            double ? totalprice;
            double ? tax = 0;
            string taxtype = "";
            foreach (var item in OrderGroupReceipt)
            {
                totalprice = 0;
                double ? unitdiscout = 0;

                if (item.Tax == "7%")
                {
                    tax = (item.PriceUnit * 0.07) * item.Quantity;
                    //taxtype = item.Tax;
                }
                if (item.Discount != null)
                {
                    unitdiscout = item.Discount;
                }

                totalprice += item.Quantity * item.PriceUnit;
                discout += unitdiscout;
                NetAmount += (totalprice + tax) - unitdiscout;
                item.TotalPrice = (totalprice + tax) - unitdiscout;
                sumprice += totalprice;
            }
            Discount = discout;
            SumTotalPrice = sumprice;
            TaxSum = tax;
            NetPrice = NetAmount;
        }

        private void DeleteItem()
        {
            if (SelectOrderGroupReceipt != null)
            {
                MessageBoxResult result = QuestionDialog("คุณต้องการลบ Order ลบใช่หรือไม่ ?");
                if (result == MessageBoxResult.Yes)
                {
                    //OrderGroupReceipt = null;

                    DataService.Purchaseing.DeleteGroupReceiptDetail(SelectOrderGroupReceipt.GroupReceiptDetailUID, AppUtil.Current.UserID);
                    var item = OrderGroupReceipt.Single(p => p.GroupReceiptDetailUID == SelectOrderGroupReceipt.GroupReceiptDetailUID);
                    OrderGroupReceipt.Remove(item);

                    DeleteSuccessDialog();
                }
            }
        }

        private void EditItem()
        {
            if (SelectOrderGroupReceipt != null)
            {
                if(SelectOrderGroupReceipt.TypeOrder == "OrtherType")
                {
                    OrderOtherType receipt = new OrderOtherType();
                    (receipt.DataContext as OrderOtherTypeViewModel).AssignModel(SelectOrderGroupReceipt);
                    OrderOtherTypeViewModel result = (OrderOtherTypeViewModel)LaunchViewDialog(receipt, "ORDOTT", true);

                }
                else
                {
                    //ManageReceipt receipt = new ManageReceipt();
                    //var data = DataService.Purchaseing.GetGroupReceiptByUID(SelectGroupReceipt.GroupReceiptUID);
                    //(receipt.DataContext as ManageReceiptViewModel).AssignModel(data);
                    //ChangeViewPermission(receipt);
                }
            }
        }

        private void Save()
        {
            if (OrderGroupReceipt != null)
            {
                try
                {
                    if (SelectPayorDetail == null)
                    {
                        WarningDialog("กรุณาใส่ชื่อลูกค้า");
                        return;
                    }
                    AssingPropertiesToModel();
                    DataService.Purchaseing.ManageGroupReceipt(model, AppUtil.Current.UserID);
                    SaveSuccessDialog();

                    ListGroupReceipt groupReceipt = new ListGroupReceipt();
                    ChangeViewPermission(groupReceipt);
                }
                catch (Exception ex)
                {
                    ErrorDialog(ex.Message);
                }
            }
        }
        private void Cancel()
        {
            ListGroupReceipt groupReceipt = new ListGroupReceipt();
            ChangeViewPermission(groupReceipt);
        }

        private void AddressCustomer()
        {
            if(SelectPayorDetail != null)
            {
                Address = SelectPayorDetail.Address1;
                TaxNumber = SelectPayorDetail.TINNo;
            }
        }

        public void AddPayor()
        {
            bool pageWindow = true;
            ManagePayorDetail order = new ManagePayorDetail(pageWindow);
            ManagePayorDetailViewModel result = (ManagePayorDetailViewModel)LaunchViewDialog(order, "PAYMN", false);
            
            PayorDetails = DataService.MasterData.GetPayorDetail();
            
        }

        private void SearchPatientBill()
        {
            SearchPatientBill order = new SearchPatientBill();
            SearchPatientBillViewModel result = (SearchPatientBillViewModel)LaunchViewDialog(order, "SRRCPTB", false);

        }

        public void AddItem()
        {
            OrderOtherType order = new OrderOtherType();
            OrderOtherTypeViewModel resultitem = (OrderOtherTypeViewModel)LaunchViewDialog(order, "ORDOTT", true);
            if (resultitem != null && resultitem.ResultDialog == ActionDialog.Save)
            {
                OrderGroupReceipt.Add(resultitem.OrderGroupReceipt);

                if (OrderGroupReceipt != null && OrderGroupReceipt.Count > 0)
                {
                    int i = 1;
                    OrderGroupReceipt.ToList().ForEach(p => p.No = i++);
                }
                CalculateNetAmount();
                OnUpdateEvent();
            }
        }

        void ApplyOrderItem(SearchOrderItem orderItem)
        {
            try
            {
                if (SelectOrganisation == null)
                {
                    WarningDialog("กรุณาเลือก Order จาก");
                    return;
                }

                if (orderItem.TypeOrder == "OrderSet")
                {
                    OrderSetModel orderSet = DataService.MasterData.GetOrderSetByUID(orderItem.BillableItemUID);
                    int? ownerUID = SelectOrganisation != null ? SelectOrganisation.HealthOrganisationUID : (int?)null;
                    
                    if (orderSet.OrderSetBillableItems != null)
                    {
                        OrderGroupReceipt order = new OrderGroupReceipt(orderSet, ownerUID);
                        OrderGroupReceiptViewModel resultMed = (OrderGroupReceiptViewModel)LaunchViewDialog(order, "ORGRPT", true);
                        if (resultMed != null && resultMed.ResultDialog == ActionDialog.Save)
                        {
                            OrderGroupReceipt.Add(resultMed.OrderGroupReceipt);

                            if (OrderGroupReceipt != null && OrderGroupReceipt.Count > 0)
                            {
                                int i = 1;
                                OrderGroupReceipt.ToList().ForEach(p => p.No = i++);
                            }
                            CalculateNetAmount();
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
                BillableItemModel billItem = DataService.MasterData.GetBillableItemByUID(billableItemUID);
                int? ownerUID = SelectOrganisation != null ? SelectOrganisation.HealthOrganisationUID : (int?)null;
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
                    OrderGroupReceipt order = new OrderGroupReceipt(billItem, ownerUID);
                    OrderGroupReceiptViewModel result = (OrderGroupReceiptViewModel)LaunchViewDialog(order, "ORGRPT", true);

                    if (result != null && result.ResultDialog == ActionDialog.Save)
                    {
                        OrderGroupReceipt.Add(result.OrderGroupReceipt);

                        if (OrderGroupReceipt != null && OrderGroupReceipt.Count > 0)
                        {
                            int i = 1;
                            OrderGroupReceipt.ToList().ForEach(p => p.No = i++);
                        }
                        CalculateNetAmount();
                        OnUpdateEvent();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorDialog(ex.Message);
            }
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

        public void AssignModel(GroupReceiptModel modelData)
        {
            model = modelData;
            AssingModelToProperties();
        }

        public void AssingModelToProperties()
        {
            SelectOrganisation = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == model.OwnerOrganisation);
            //ReceiptNumber = model.ReceiptNo;
            Seller = model.Seller;
            BillDate = model.StartDttm;
            SelectPayorDetail = PayorDetails.FirstOrDefault(p => p.PayorDetailUID == model.PayorDetailUID);

            //model.GroupReceiptUID = model.GroupReceiptUID;
            AddressCustomer();
            
            foreach (var item in model.GroupReceiptDetailModel)
            {
                GroupReceiptDetailModel newItems = new GroupReceiptDetailModel();
                newItems.GroupReceiptDetailUID = item.GroupReceiptDetailUID;
                newItems.GroupReceiptUID = item.GroupReceiptUID;
                newItems.ItemName = item.ItemName;
                newItems.ItemCode = item.ItemCode;
                newItems.PriceUnit = item.PriceUnit;
                newItems.Quantity = item.Quantity;
                newItems.UnitItem = item.UnitItem;
                newItems.TotalPrice = item.TotalPrice;
                newItems.Discount = item.Discount;
                newItems.Tax = item.Tax;
                OrderGroupReceipt.Add(newItems);
            }
        }

        public void AssingPropertiesToModel()
        {
            if (model == null)
            {
                model = new GroupReceiptModel();
            }
            model.GroupReceiptDetailModel = new List<GroupReceiptDetailModel>();
            if (GroupReceiptOrders != null)
            {
                double? allPrice = OrderGroupReceipt.Sum(item => item.TotalPrice);
                //model.ReceiptNo = ReceiptNumber;
                model.OwnerOrganisation = SelectOrganisation.HealthOrganisationUID;
                model.PayorName = SelectPayorDetail.PayorName;
                model.StartDttm = BillDate;
                model.Seller = Seller;
                model.PayorDetailUID = SelectPayorDetail.PayorDetailUID;
                model.PayerAddress = Address;
                model.PriceUnit = allPrice;

                model.GroupReceiptDetailModel.AddRange(OrderGroupReceipt);
            }
        }
        #endregion
    }
}
