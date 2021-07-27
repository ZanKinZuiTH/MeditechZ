using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Model.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class OrderGroupReceiptViewModel : MediTechViewModelBase
    {
        #region Properties
        public OrderSetModel OrderSet { get; set; }

        private GroupReceiptDetailModel _OrderGroupReceipt;
        public GroupReceiptDetailModel OrderGroupReceipt
        {
            get { return _OrderGroupReceipt; }
            set { Set(ref _OrderGroupReceipt, value); }
        }

        //private DateTime _StartDate;
        //public DateTime StartDate
        //{
        //    get { return _StartDate; }
        //    set { Set(ref _StartDate, value); }
        //}

        public List<LookupReferenceValueModel> TaxChoice { get; set; }

        private LookupReferenceValueModel _TaxSelect;
        public LookupReferenceValueModel TaxSelect
        {
            get { return _TaxSelect; }
            set
            {
                Set(ref _TaxSelect, value);
                Calculate();
                //if (TaxSelect != null)
                //{
                //    ReCash = SumPrice(Quantity, Int64.Parse(Price), Discount);
                //    UnitPrice = ReCash;
                //}
            }
        }

        public int? OwnerOrgansitaion { get; set; }

        public BillableItemModel BillableItem { get; set; }

        private List<StockModel> _Stores;
        public List<StockModel> Stores
        {
            get { return _Stores; }
            set { Set(ref _Stores, value); }
        }

        //private double _Quantity;
        //public double Quantity
        //{
        //    get { return _Quantity; }
        //    set { Set(ref _Quantity, value); }
        //}

        private GroupReceiptDetailModel _model;
        public GroupReceiptDetailModel model
        {
            get { return _model; }
            set { _model = value; }
        }

        private double _Quantity;
        public double Quantity
        {
            get { return _Quantity; }
            set
            {
                Set(ref _Quantity, value);

                Calculate();
                //ReCash = SumPrice(Quantity, Int64.Parse(Price), Discount);
                //UnitPrice = ReCash;
            }
        }

        private double _ReCash;
        public double ReCash
        {
            get { return _ReCash; }
            set { Set(ref _ReCash, value); }
        }

        private string _TypeOrder;
        public string TypeOrder
        {
            get { return _TypeOrder; }
            set { Set(ref _TypeOrder, value); }
        }

        private string _OrderName;
        public string OrderName
        {
            get { return _OrderName; }
            set { Set(ref _OrderName, value); }
        }

        private string _OrderCode;
        public string OrderCode
        {
            get { return _OrderCode; }
            set { Set(ref _OrderCode, value); }
        }

        private double _Discount;
        public double Discount
        {
            get { return _Discount; }
            set
            {
                Set(ref _Discount, value);

                Calculate();
                //ReCash = SumPrice(Quantity, Int64.Parse(Price), Discount);
                //UnitPrice = ReCash;
            }
        }

        private string _Tax;
        public string Tax
        {
            get { return _Tax; }
            set { Set(ref _Tax, value); }
        }

        private string _Price;
        public string Price
        {
            get { return _Price; }
            set
            {
                Set(ref _Price, value);
                Calculate();
                //ReCash = SumPrice(Quantity, Int64.Parse(Price), Discount);
                //UnitPrice = ReCash;
            }
        }

        private string _Unit;
        public string Unit
        {
            get { return _Unit; }
            set { Set(ref _Unit, value); }
        }

        private double _UnitPrice;
        public double UnitPrice
        {
            get { return _UnitPrice; }
            set { Set(ref _UnitPrice, value); }
        }

        #endregion

        #region Command
        private RelayCommand _AddCommand;
        public RelayCommand AddCommand
        {
            get { return _AddCommand ?? (_AddCommand = new RelayCommand(Add)); }
        }


        private RelayCommand _CancelCommand;
        public RelayCommand CancelCommand
        {
            get { return _CancelCommand ?? (_CancelCommand = new RelayCommand(Cancel)); }
        }
        #endregion

        #region Method

        public OrderGroupReceiptViewModel()
        {

            TaxChoice = new List<LookupReferenceValueModel>{
                new LookupReferenceValueModel { Key = 0, Display = "7%" ,NumericValue = 7},
                new LookupReferenceValueModel { Key = 1, Display = "ยกเว้นภาษี",NumericValue = 0 }
            };

            TaxSelect = TaxChoice.FirstOrDefault(p => p.Key == 1);
        }

        public void BindingFromOrderset()
        {
            OrderSetModel orderSet = DataService.MasterData.GetOrderSetByUID(OrderSet.OrderSetUID);
            double? ordersetPrice = orderSet.OrderSetBillableItems.Sum(item => item.NetPrice);
            DateTime now = DateTime.Now;
            OrderName = OrderSet.Name;
            OrderCode = "Code : " + OrderSet.Code;


            Price = ordersetPrice.ToString();
            UnitPrice = ReCash;

            //StartDate = now.Date;
        }

        public void BindingFromBillableItem()
        {
            DateTime now = DateTime.Now;
            Unit = Unit;
            OrderName = BillableItem.ItemName;
            OrderCode = "Code : " + BillableItem.Code;
            Price = BillableItem.Price.ToString();
            UnitPrice = ReCash;
            //StartDate = now.Date;
        }

        public void AssignModel(GroupReceiptDetailModel modelData)
        {
            model = modelData;
            AssingModelToProperties();
        }

        public void AssingModelToProperties()
        {
            OrderName = model.ItemName;
            Price = model.PriceUnit.ToString();
            Discount = model.Discount.Value;
            Quantity = model.Quantity.Value;
            Unit = model.UnitItem;
            //UnitPrice = model.TotalPrice.Value;
            //TaxSelect.Display = model.Tax;
            //TaxChoice = new List<LookupReferenceValueModel>{
            //    new LookupReferenceValueModel {Display = model.Tax} };
            TaxSelect = TaxChoice.FirstOrDefault(p => p.NumericValue == model.PTaxPercentage);
            //StartDate = model.

        }

        private void Add()
        {
            try
            {
                if (Quantity == 0)
                {
                    WarningDialog("กรุณาใส่จำนวน");
                    return;
                }
                if (OrderGroupReceipt == null)
                {
                    OrderGroupReceipt = new GroupReceiptDetailModel();
                    OrderGroupReceipt.ItemName = OrderName;
                    if (BillableItem != null)
                        OrderGroupReceipt.BillableItemUID = BillableItem.BillableItemUID;
                    if(OrderSet != null)
                        OrderGroupReceipt.OrderSetUID = OrderSet.OrderSetUID;
                    OrderGroupReceipt.Quantity = Quantity;
                    OrderGroupReceipt.UnitItem = Unit;
                    OrderGroupReceipt.PriceUnit = Int64.Parse(Price);
                    OrderGroupReceipt.Discount = Discount;
                    OrderGroupReceipt.TotalPrice = UnitPrice;
                    OrderGroupReceipt.PTaxPercentage = TaxSelect != null ? TaxSelect.NumericValue : 0;
                    //OrderGroupReceipt.ItemCode = orderSet.Code;
                    OrderGroupReceipt.TypeOrder = TypeOrder;

                    if (model != null)
                    {
                        OrderGroupReceipt.No = model.No;
                    }
                }

                CloseViewDialog(ActionDialog.Save);
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }

        public void Cancel()
        {
            CloseViewDialog(ActionDialog.Cancel);
        }

        public double SumPrice(double quantity, double total, double discount)
        {
            double result = 0;
            double tax = 0;
            if (quantity != 0 && total != 0)
            {
                result = quantity * total;
            }

            if (TaxSelect != null)
            {
                if (TaxSelect.Display == "7%")
                {
                    tax = (total * 0.07) * quantity;
                }
            }

            result = (result + tax) - discount;

            return result;
        }

        public void Calculate()
        {
            if (Quantity != 0 && Price != null)
            {
                ReCash = SumPrice(Quantity, Double.Parse(Price), Discount);
                UnitPrice = ReCash;
            }
        }

        #endregion
    }
}
