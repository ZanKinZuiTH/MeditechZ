using DevExpress.XtraReports.UI;
using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Reports.Operating.Cashier;
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

        private List<GroupReceiptPatientBillModel> _GroupReceiptPatientBill;

        public List<GroupReceiptPatientBillModel> GroupReceiptPatientBill
        {
            get { return _GroupReceiptPatientBill ?? (_GroupReceiptPatientBill = new List<GroupReceiptPatientBillModel>()); }
            set
            {
                _GroupReceiptPatientBill = value;
                if (_GroupReceiptPatientBill != null && _GroupReceiptPatientBill.Count > 0)
                {
                    VisibilityTextInvocice = Visibility.Visible;
                }
                else
                {
                    VisibilityTextInvocice = Visibility.Hidden;
                }
            }
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

        public DateTime? BillDate { get; set; }

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

        private double _Amount;
        public double Amount
        {
            get { return _Amount; }
            set { Set(ref _Amount, value); }
        }

        private double _Discount;
        public double Discount
        {
            get { return _Discount; }
            set { Set(ref _Discount, value); }
        }


        private double _NetAmount;
        public double NetAmount
        {
            get { return _NetAmount; }
            set { Set(ref _NetAmount, value); }
        }

        private double _NoTaxAmount;
        public double NoTaxAmount
        {
            get { return _NoTaxAmount; }
            set { Set(ref _NoTaxAmount, value); }
        }

        private double _BfTaxAmount;
        public double BfTaxAmount
        {
            get { return _BfTaxAmount; }
            set { Set(ref _BfTaxAmount, value); }
        }

        private double _TaxAmount;
        public double TaxAmount
        {
            get { return _TaxAmount; }
            set { Set(ref _TaxAmount, value); }
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

        private string _ReceiptNo;

        public string ReceiptNo
        {
            get { return _ReceiptNo; }
            set
            {
                Set(ref _ReceiptNo, value);
                VisibilityReceiptNo = string.IsNullOrEmpty(ReceiptNo) ? Visibility.Hidden : Visibility.Visible;
            }
        }

        private Visibility _VisibilityReceiptNo = Visibility.Hidden;

        public Visibility VisibilityReceiptNo
        {
            get { return _VisibilityReceiptNo; }
            set { Set(ref _VisibilityReceiptNo, value); }
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

        private Visibility _VisibilityTextInvocice = Visibility.Hidden;

        public Visibility VisibilityTextInvocice
        {
            get { return _VisibilityTextInvocice; }
            set { Set(ref _VisibilityTextInvocice, value); }
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

        private bool _IsPrintReceipt = true;

        public bool IsPrintReceipt
        {
            get { return _IsPrintReceipt; }
            set { Set(ref _IsPrintReceipt, value); }
        }

        private List<LookupReferenceValueModel> _BillingCategory;

        public List<LookupReferenceValueModel> BillingCategory
        {
            get { return _BillingCategory; }
            set { _BillingCategory = value; }
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
            PayorDetails = DataService.Billing.GetPayorDetail();
            var refVale = DataService.Technical.GetReferenceValueList("PBLCT");
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
                new LookupReferenceValueModel { Key = 0, Display = "7%" ,NumericValue = 7},
                new LookupReferenceValueModel { Key = 1, Display = "ยกเว้นภาษี",NumericValue = 0 }
            };

            BillingCategory = refVale.Where(p => p.DomainCode == "PBLCT").ToList();
        }

        public void ChangeValue(DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            CalculateNetAmount();
        }
        void CalculateNetAmount()
        {
            Amount = 0;
            Discount = 0;
            NetAmount = 0;
            NoTaxAmount = 0;
            BfTaxAmount = 0;
            TaxAmount = 0;
            foreach (var item in OrderGroupReceipt)
            {
                Amount += (item.PriceUnit * item.Quantity) ?? 0;
                Discount += item.Discount ?? 0;

                if (item.PTaxPercentage == 0)
                {
                    NoTaxAmount += ((item.PriceUnit * item.Quantity) ?? 0) - (item.Discount ?? 0);
                }
                else if (item.PTaxPercentage == 7)
                {
                    double amountTax = ((item.PriceUnit * item.Quantity) ?? 0) - (item.Discount ?? 0);
                    BfTaxAmount += amountTax - (amountTax * 7 / 107);
                    TaxAmount += (amountTax * 7 / 107);
                }
                item.TotalPrice = ((item.PriceUnit * item.Quantity) ?? 0) - (item.Discount ?? 0);
            }
            NetAmount = Amount - Discount;

        }

        private void DeleteItem()
        {
            if (SelectOrderGroupReceipt != null)
            {
                MessageBoxResult result = QuestionDialog("คุณต้องการลบ Order ลบใช่หรือไม่ ?");
                if (result == MessageBoxResult.Yes)
                {
                    OrderGroupReceipt.Remove(SelectOrderGroupReceipt);
                    CalculateNetAmount();
                }
            }
        }

        private void EditItem()
        {
            if (SelectOrderGroupReceipt != null)
            {
                if (SelectOrderGroupReceipt.TypeOrder == "OrtherType")
                {
                    OrderOtherType receipt = new OrderOtherType();
                    (receipt.DataContext as OrderOtherTypeViewModel).AssignModel(SelectOrderGroupReceipt);
                    OrderOtherTypeViewModel result = (OrderOtherTypeViewModel)LaunchViewDialog(receipt, "ORDOTT", true);

                    if (result != null)
                    {
                        var item = OrderGroupReceipt.Where(p => p.No == result.OrderGroupReceipt.No);
                        OrderGroupReceipt.Remove(item.FirstOrDefault());

                        OrderGroupReceipt.Add(result.OrderGroupReceipt);
                    }

                    CalculateNetAmount();
                }
                else
                {
                    OrderGroupReceipt order = new OrderGroupReceipt();
                    (order.DataContext as OrderGroupReceiptViewModel).AssignModel(SelectOrderGroupReceipt);
                    OrderGroupReceiptViewModel result = (OrderGroupReceiptViewModel)LaunchViewDialog(order, "ORGRPT", true);
                    if (result != null)
                    {
                        var item = OrderGroupReceipt.Where(p => p.No == result.OrderGroupReceipt.No);
                        OrderGroupReceipt.Remove(item.FirstOrDefault());

                        OrderGroupReceipt.Add(result.OrderGroupReceipt);
                    }

                    //OrderGroupReceipt.Remove.Where(p => p.No == model.FirstOrDefault().No).ToList().Add(result.OrderGroupReceipt);

                    //foreach (var test in OrderGroupReceipt.Where(p => p.No == model.FirstOrDefault().No))
                    //{
                    //    //test.Quantity = model.FirstOrDefault().Quantity;
                    //    //test.PriceUnit = model.FirstOrDefault().PriceUnit;
                    //    //test.Discount = model.FirstOrDefault().Discount;
                    //}

                    //if (OrderGroupReceipt != null && OrderGroupReceipt.Count > 0)
                    //{
                    //    int i = 1;
                    //    OrderGroupReceipt.ToList().ForEach(p => p.No = i++);
                    //}

                    CalculateNetAmount();
                    OnUpdateEvent();
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
                    int? groupReceiptUID = DataService.Purchaseing.ManageGroupReceipt(model, AppUtil.Current.UserID);
                    SaveSuccessDialog();

                    if (IsPrintReceipt)
                    {

                        GroupReceipt rpt = new GroupReceipt();
                        rpt.Parameters["GroupReceiptUID"].Value = groupReceiptUID;
                        //rpt.DataSource = SelectGroupReceipt;
                        ReportPrintTool printTool = new ReportPrintTool(rpt);
                        rpt.RequestParameters = false;
                        rpt.ShowPrintMarginsWarning = false;
                        printTool.ShowPreviewDialog();
                    }
                    ListGroupReceipt listgroupReceipt = new ListGroupReceipt();
                    ChangeViewPermission(listgroupReceipt);
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
            if (SelectPayorDetail != null)
            {
                Address = SelectPayorDetail.Address1;
                TaxNumber = SelectPayorDetail.GovernmentNo;
            }
        }

        public void AddPayor()
        {
            bool pageWindow = true;
            ManagePayorDetail order = new ManagePayorDetail(pageWindow);
            ManagePayorDetailViewModel result = (ManagePayorDetailViewModel)LaunchViewDialog(order, "PAYMN", false);

            PayorDetails = DataService.Billing.GetPayorDetail();

        }

        private void SearchPatientBill()
        {
            SearchPatientBill order = new SearchPatientBill();
            if (GroupReceiptPatientBill != null)
                (order.DataContext as SearchPatientBillViewModel).PatientBillGroup = new ObservableCollection<GroupReceiptPatientBillModel>(GroupReceiptPatientBill);
            SearchPatientBillViewModel result = (SearchPatientBillViewModel)LaunchViewDialog(order, "SRRCPTB", false);
            if (result != null && result.ResultDialog == ActionDialog.Save)
            {
                GroupReceiptPatientBill = result.PatientBillGroup.ToList();

                if (model == null || model.GroupReceiptDetails.Count <= 0)
                {
                    List<PatientBilledItemModel> patientBilledOrderDetail = new List<PatientBilledItemModel>();
                    foreach (var patientBill in GroupReceiptPatientBill)
                    {
                        var patientBillOrder = DataService.Billing.GetPatientBilledOrderDetail(patientBill.PatientBillUID);
                        patientBilledOrderDetail.AddRange(patientBillOrder);
                    }

                    var orerSetGroupBill = patientBilledOrderDetail.Where(p => p.OrderSetUID != null)
                        .GroupBy(p => new { p.PatientBillUID, p.OrderSetUID })
                        .Select(s => new
                        {
                            OrderSetUID = s.FirstOrDefault().OrderSetUID,
                            Quantity = s.Select(z => new { z.PatientBillUID, z.OrderSetUID }).Distinct().Count()
                        });

                    var orerSetCollect = orerSetGroupBill.Where(p => p.OrderSetUID != null)
                      .GroupBy(p => new { p.OrderSetUID })
                      .Select(s => new
                      {
                          OrderSetUID = s.FirstOrDefault().OrderSetUID,
                          Quantity = s.Sum(p => p.Quantity)
                      });


                    var billableItemCollect = patientBilledOrderDetail.Where(p => p.OrderSetUID == null)
                        .GroupBy(p => new { p.BillableItemUID })
                        .Select(s => new
                        {
                            BillableItemName = s.FirstOrDefault().ItemName,
                            BillableItemUID = s.FirstOrDefault().BillableItemUID,
                            PriceUnit = s.FirstOrDefault().Amount,
                            Unit = s.FirstOrDefault().Unit,
                            Discount = s.Sum(p => p.Discount),
                            Quantity = s.Sum(p => p.ItemMultiplier),
                            NetAmount = s.Sum(p => p.NetAmount)
                        });

                    OrderGroupReceipt = new ObservableCollection<GroupReceiptDetailModel>();


                    foreach (var item in orerSetCollect)
                    {
                        var OrderSet = DataService.MasterData.GetOrderSetByUID(item.OrderSetUID.Value);
                        GroupReceiptDetailModel newOrderReceipt = new GroupReceiptDetailModel();
                        newOrderReceipt.OrderSetUID = OrderSet.OrderSetUID;
                        newOrderReceipt.ItemName = OrderSet.Name;
                        newOrderReceipt.Quantity = item.Quantity;
                        newOrderReceipt.UnitItem = "ชุด";
                        newOrderReceipt.PriceUnit = OrderSet.OrderSetBillableItems.Sum(p => p.NetPrice);
                        newOrderReceipt.Discount = 0;
                        newOrderReceipt.TotalPrice = (newOrderReceipt.PriceUnit * newOrderReceipt.Quantity);
                        newOrderReceipt.PTaxPercentage = 0;
                        OrderGroupReceipt.Add(newOrderReceipt);
                    }

                    foreach (var item in billableItemCollect)
                    {
                        GroupReceiptDetailModel newOrderReceipt = new GroupReceiptDetailModel();
                        newOrderReceipt.BillableItemUID = item.BillableItemUID;
                        newOrderReceipt.ItemName = item.BillableItemName;
                        newOrderReceipt.Quantity = item.Quantity;
                        newOrderReceipt.UnitItem = item.Unit;
                        newOrderReceipt.PriceUnit = item.PriceUnit;
                        newOrderReceipt.Discount = item.Discount;
                        newOrderReceipt.TotalPrice = (newOrderReceipt.PriceUnit * newOrderReceipt.Quantity) - item.Discount;
                        newOrderReceipt.PTaxPercentage = 0;
                        OrderGroupReceipt.Add(newOrderReceipt);
                    }


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
                        OrderGroupReceipt order = new OrderGroupReceipt(orderSet, ownerUID, orderItem.TypeOrder);
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
                    PopUpOrder(orderItem.BillableItemUID, orderItem.TypeOrder);
                }
            }
            catch (Exception ex)
            {
                ErrorDialog(ex.Message);
            }

        }

        void PopUpOrder(int billableItemUID, string typeOrder)
        {
            try
            {
                BillableItemModel billItem = DataService.MasterData.GetBillableItemByUID(billableItemUID);
                int? ownerUID = SelectOrganisation != null ? SelectOrganisation.HealthOrganisationUID : (int?)null;
                int? PBLCTUID = BillingCategory.FirstOrDefault(p => p.ValueCode == "OPDTRF")?.Key;

                var billItemPrice = GetBillableItemPrice(billItem.BillableItemDetails, PBLCTUID, ownerUID ?? 0);
                if (billItemPrice == null)
                {
                    WarningDialog("รายการ " + billItem.ItemName + " นี้ยังไม่ได้กำหนดราคาสำหรับขาย โปรดตรวจสอบ");
                    return;
                }

                billItem.Price = billItemPrice.Price;
                billItem.CURNCUID = billItemPrice.CURNCUID;

                if (billItem != null)
                {
                    OrderGroupReceipt order = new OrderGroupReceipt(billItem, ownerUID, typeOrder);
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

        public void AssignModel(GroupReceiptModel modelData)
        {
            model = modelData;
            AssingModelToProperties();
        }

        public void AssingModelToProperties()
        {
            SelectOrganisation = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == model.OwnerOrganisation);
            Seller = model.Seller;
            BillDate = model.StartDttm;
            SelectPayorDetail = PayorDetails.FirstOrDefault(p => p.PayorDetailUID == model.PayorDetailUID);
            Address = model.PayerAddress;
            TaxNumber = model.TINNo;
            ReceiptNo = model.ReceiptNo;
            foreach (var item in model.GroupReceiptDetails)
            {
                GroupReceiptDetailModel newItems = new GroupReceiptDetailModel();
                newItems.GroupReceiptDetailUID = item.GroupReceiptDetailUID;
                newItems.GroupReceiptUID = item.GroupReceiptUID;
                newItems.ItemName = item.ItemName;
                newItems.BillableItemUID = item.BillableItemUID;
                newItems.OrderSetUID = item.OrderSetUID;
                newItems.PriceUnit = item.PriceUnit;
                newItems.Quantity = item.Quantity;
                newItems.UnitItem = item.UnitItem;
                newItems.TotalPrice = item.TotalPrice;
                newItems.Discount = item.Discount;
                newItems.PTaxPercentage = item.PTaxPercentage;
                OrderGroupReceipt.Add(newItems);
            }
            if (model.GroupReceiptPatientBills.Count > 0)
            {
                foreach (var item in model.GroupReceiptPatientBills)
                {
                    GroupReceiptPatientBill.Add(item);
                }
                VisibilityTextInvocice = Visibility.Visible;
            }


            CalculateNetAmount();
            OnUpdateEvent();
        }

        public void AssingPropertiesToModel()
        {
            if (model == null)
            {
                model = new GroupReceiptModel();
            }
            model.GroupReceiptDetails = new List<GroupReceiptDetailModel>();
            model.GroupReceiptPatientBills = new List<GroupReceiptPatientBillModel>();
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
                model.Amount = Amount;
                model.Discount = Discount;
                model.NetAmount = NetAmount;
                model.TaxAmount = TaxAmount;
                model.NoTaxAmount = NoTaxAmount;
                model.BfTaxAmount = BfTaxAmount;

                model.GroupReceiptDetails.AddRange(OrderGroupReceipt);

                model.GroupReceiptPatientBills.AddRange(GroupReceiptPatientBill);
            }
        }
        #endregion
    }
}
