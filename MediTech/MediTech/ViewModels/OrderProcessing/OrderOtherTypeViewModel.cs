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
    public class OrderOtherTypeViewModel : MediTechViewModelBase
    {
        #region Properties

        private GroupReceiptDetailModel _OrderGroupReceipt;
        public GroupReceiptDetailModel OrderGroupReceipt
        {
            get { return _OrderGroupReceipt; }
            set { Set(ref _OrderGroupReceipt, value); }
        }


        public List<LookupReferenceValueModel> TaxChoice { get; set; }

        private LookupReferenceValueModel _TaxSelect;
        public LookupReferenceValueModel TaxSelect
        {
            get { return _TaxSelect; }
            set
            {
                Set(ref _TaxSelect, value);
                Calculate();
                    //ReCash = SumPrice(Quantity, Int64.Parse(Price), Discount);
                    //UnitPrice = ReCash;

            }
        }

        private double _Quantity;
        public double Quantity
        {
            get { return _Quantity; }
            set
            {
                Set(ref _Quantity, value);
                if(Price != null)
                {
                    Calculate();
                    //ReCash = SumPrice(Quantity, Int64.Parse(Price), Discount);
                    //UnitPrice = ReCash;
                }
                else
                {
                    WarningDialog("กรุณาใส่ราคาต่อหน่วย");
                    return;
                }
            }
        }

        private double _ReCash;
        public double ReCash
        {
            get { return _ReCash; }
            set { Set(ref _ReCash, value); }
        }

        private string _ItemName;
        public string ItemName
        {
            get { return _ItemName; }
            set { Set(ref _ItemName, value); }
        }

        private string _TypeOrder;
        public string TypeOrder
        {
            get { return _TypeOrder; }
            set { Set(ref _TypeOrder, value); }
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

        //private string _Tax;
        //public string Tax
        //{
        //    get { return _Tax; }
        //    set { Set(ref _Tax, value); }
        //}

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

        private GroupReceiptDetailModel _model;
        public GroupReceiptDetailModel model
        {
            get { return _model; }
            set { _model = value; }
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

        public OrderOtherTypeViewModel()
        {
            
            TaxChoice = new List<LookupReferenceValueModel>{
                new LookupReferenceValueModel { Key = 0, Display = "7%" ,NumericValue = 7},
                new LookupReferenceValueModel { Key = 1, Display = "ยกเว้นภาษี",NumericValue = 0 }
            };

            TaxSelect = TaxChoice.FirstOrDefault(p => p.Key == 0);
        }

        private void Add()
        {
            try
            {
                if(ItemName == null)
                {
                    WarningDialog("กรุณาใส่รายการ");
                    return;
                }
                if (Price == null)
                {
                    WarningDialog("กรุณาใส่รายการ");
                    return;
                }
                if (Quantity == 0)
                {
                    WarningDialog("กรุณาใส่จำนวน");
                    return;
                }
                if(TaxSelect == null)
                {
                    WarningDialog("กรุณาเลือกประเภทภาษี");
                    return;
                }
                if (OrderGroupReceipt == null)
                {
                    OrderGroupReceipt = new GroupReceiptDetailModel();
                    OrderGroupReceipt.ItemName = ItemName;
                    OrderGroupReceipt.Quantity = Quantity;
                    OrderGroupReceipt.UnitItem = Unit;
                    OrderGroupReceipt.PriceUnit = Int64.Parse(Price);
                    OrderGroupReceipt.Discount = Discount;
                    OrderGroupReceipt.TotalPrice = UnitPrice;
                    OrderGroupReceipt.PTaxPercentage = TaxSelect != null ? TaxSelect.NumericValue : 0;
                    OrderGroupReceipt.TypeOrder = "OrtherType";

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


        public void AssignModel(GroupReceiptDetailModel modelData)
        {
            model = modelData;
            AssingModelToProperties();
        }

        public void AssingModelToProperties()
        {
            ItemName = model.ItemName;
            Price = model.PriceUnit.ToString();
            Discount = model.Discount.Value;
            Quantity = model.Quantity.Value;
            Unit = model.UnitItem;
            //UnitPrice = model.TotalPrice.Value;
            //TaxSelect.Display = model.Tax;
            //TaxChoice = new List<LookupReferenceValueModel>{
            //    new LookupReferenceValueModel {Display = model.Tax} };
            TaxSelect = TaxChoice.FirstOrDefault(p => p.NumericValue == model.PTaxPercentage);
        }

        public void Cancel()
        {
            CloseViewDialog(ActionDialog.Cancel);
        }


        public double SumPrice(double quantity, double total, double  discount)
        {
            double result = 0;
            
            double tax = 0;
            if (quantity != 0 && total != 0)
            {
                result = quantity * total;
            }

            if(TaxSelect != null)
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
            if(Quantity != 0 && Price != null)
            {
                ReCash = SumPrice(Quantity, Int64.Parse(Price), Discount);
                UnitPrice = ReCash;
            }
        }

        #endregion
    }
}
