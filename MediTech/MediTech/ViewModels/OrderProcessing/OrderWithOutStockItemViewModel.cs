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
    public class OrderWithOutStockItemViewModel : MediTechViewModelBase
    {
        #region Properties

        public int OwnerOrgansitaionUID { get; set; }

        private BillableItemModel _BillableItem;

        public BillableItemModel BillableItem
        {
            get { return _BillableItem; }
            set
            {
                _BillableItem = value;
            }
        }

        private PatientOrderDetailModel _PatientOrderDetail;

        public PatientOrderDetailModel PatientOrderDetail
        {
            get { return _PatientOrderDetail; }
            set
            {
                _PatientOrderDetail = value;
            }
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


        private string _UnitPrice;

        public string UnitPrice
        {
            get { return _UnitPrice; }
            set { Set(ref _UnitPrice, value); }
        }

        private double? _Quantity;

        public double? Quantity
        {
            get { return _Quantity; }
            set { Set(ref _Quantity, value); }
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


        private double? _OverwritePrice;

        public double? OverwritePrice
        {
            get { return _OverwritePrice; }
            set { Set(ref _OverwritePrice, value); }
        }

        private string _Notes;

        public string Notes
        {
            get { return _Notes; }
            set { Set(ref _Notes, value); }
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


        public void BindingFromBillableItem()
        {
            DateTime now = DateTime.Now;

            TypeOrder = BillableItem.BillingServiceMetaData;
            OrderName = BillableItem.ItemName;
            OrderCode = "Code : " + BillableItem.Code;
            UnitPrice = BillableItem.Price.ToString("#,#.00");
            Quantity = 1;
            StartDate = now.Date;
            StartTime = now;
        }

        public void BindingFromPatientOrderDetail()
        {

            TypeOrder = PatientOrderDetail.BillingService;
            OrderName = PatientOrderDetail.ItemName;
            OrderCode = "Code : " + PatientOrderDetail.ItemCode;
            UnitPrice = PatientOrderDetail.UnitPrice.Value.ToString("#,#.00");
            Quantity = PatientOrderDetail.Quantity ?? 1;

            StartDate = PatientOrderDetail.StartDttm.Value.Date;
            StartTime = PatientOrderDetail.StartDttm.Value;
            Notes = PatientOrderDetail.Comments;
            OverwritePrice = PatientOrderDetail.OverwritePrice;
        }

        public void Add()
        {
            try
            {
                if (Quantity <= 0)
                {
                    WarningDialog("ไม่อนุญาติให้คีย์ จำนวน < 0");
                    return;
                }

                if (PatientOrderDetail == null)
                {
                    PatientOrderDetail = new PatientOrderDetailModel();
                    PatientOrderDetail.BillableItemUID = BillableItem.BillableItemUID;
                    PatientOrderDetail.BSMDDUID = BillableItem.BSMDDUID;
                    PatientOrderDetail.ItemUID = BillableItem.ItemUID;
                    PatientOrderDetail.ItemCode = BillableItem.Code;
                    PatientOrderDetail.ItemName = BillableItem.ItemName;
                    PatientOrderDetail.BillingService = BillableItem.BillingServiceMetaData;
                    PatientOrderDetail.UnitPrice = BillableItem.Price;
                    PatientOrderDetail.DoctorFee = (BillableItem.DoctorFee / 100) * BillableItem.Price;
                }

                PatientOrderDetail.Comments = Notes;

                PatientOrderDetail.Quantity = Quantity;

                PatientOrderDetail.StartDttm = StartDate.Add(StartTime.TimeOfDay);

                if (OverwritePrice != null)
                {
                    PatientOrderDetail.OverwritePrice = OverwritePrice;
                    PatientOrderDetail.IsPriceOverwrite = "Y";
                    //PatientOrderDetail.NetAmount = OverwritePrice + (PatientOrderDetail.DoctorFee ?? 0);
                    PatientOrderDetail.NetAmount = (OverwritePrice * PatientOrderDetail.Quantity);
                    PatientOrderDetail.DisplayPrice = PatientOrderDetail.OverwritePrice;
                }
                else
                {
                    PatientOrderDetail.OverwritePrice = OverwritePrice;
                    PatientOrderDetail.IsPriceOverwrite = "N";
                    //PatientOrderDetail.NetAmount = (PatientOrderDetail.UnitPrice ?? 0) + (PatientOrderDetail.DoctorFee ?? 0);
                    PatientOrderDetail.NetAmount = ((PatientOrderDetail.UnitPrice ?? 0) * PatientOrderDetail.Quantity);
                    PatientOrderDetail.DisplayPrice = PatientOrderDetail.UnitPrice;
                }

                if (PatientOrderDetail.OwnerOrganisationUID == 0)
                {
                    PatientOrderDetail.OwnerOrganisationUID = OwnerOrgansitaionUID;
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

        #endregion

    }
}
