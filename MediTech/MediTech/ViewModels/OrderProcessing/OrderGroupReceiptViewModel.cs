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
        private DateTime _StartDate;
        public OrderSetModel orderSet { get; set; }

        private GroupReceiptDetailModel _OrderGroupReceipt;
        public GroupReceiptDetailModel OrderGroupReceipt
        {
            get { return _OrderGroupReceipt; }
            set { Set(ref _OrderGroupReceipt, value); }
        }

        public DateTime StartDate
        {
            get { return _StartDate; }
            set { Set(ref _StartDate, value); }
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

        private double  _Quantity;
        public double Quantity
        {
            get { return _Quantity; }
            set
            {
                Set(ref _Quantity, value);
                
                ReCash = SumPrice(Quantity, Int64.Parse(Price), Discount);
                UnitPrice = ReCash;
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

                ReCash = SumPrice(Quantity, Int64.Parse(Price), Discount);
                UnitPrice = ReCash;
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

                ReCash = SumPrice(Quantity, Int64.Parse(Price), Discount);
                UnitPrice = ReCash;
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

        public void BindingFromOrderset()
        {
            List<GroupReceiptModel> orderPrice = DataService.OrderProcessing.GetOrderPriceByUID(orderSet.OrderSetUID);
            double? ordersetPrice = orderPrice.Sum(item => item.PriceUnit);
            DateTime now = DateTime.Now;
            //TypeOrder = BillableItem.BillingServiceMetaData;
            OrderName = orderSet.Name;
            OrderCode = "Code : " + orderSet.Code;


            Price = ordersetPrice.ToString();
            UnitPrice = ReCash;

            StartDate = now.Date;
        }

        public void BindingFromBillableItem()
        {
            DateTime now = DateTime.Now;
            Unit = Unit;
            OrderName = BillableItem.ItemName;
            OrderCode = "Code : " + BillableItem.Code;
            Price = BillableItem.Price.ToString();
            UnitPrice = ReCash;
            StartDate = now.Date;
        }


        private void Add()
        {
            try
            {
                if(Quantity == 0)
                {
                    WarningDialog("กรุณาใส่จำนวน");
                    return;
                }
                if (OrderGroupReceipt == null)
                {
                    OrderGroupReceipt = new GroupReceiptDetailModel();
                    OrderGroupReceipt.ItemName = OrderName;
                    OrderGroupReceipt.Quantity = Quantity;
                    OrderGroupReceipt.UnitItem = Unit;
                    OrderGroupReceipt.PriceUnit = Int64.Parse(Price);
                    OrderGroupReceipt.Discount = Discount;
                    OrderGroupReceipt.TotalPrice = UnitPrice;
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

        public double SumPrice(double cash, double total, double discount)
        {
            double result = 0;
            if (cash != 0 && total != 0)
            {
                result = cash * total;
            }
            result = result - discount;

            return result;
        }

        #endregion
    }
}
