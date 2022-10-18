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

        private Visibility _CareproviderVisibility = Visibility.Collapsed;

        public Visibility CareproviderVisibility
        {
            get { return _CareproviderVisibility; }
            set { Set(ref _CareproviderVisibility, value); }
        }


        private Visibility _PriorityVisibility = Visibility.Collapsed;

        public Visibility PriorityVisibility
        {
            get { return _PriorityVisibility; }
            set { Set(ref _PriorityVisibility, value); }
        }

        private string _TypeOrder;

        public string TypeOrder
        {
            get { return _TypeOrder; }
            set { Set(ref _TypeOrder, value); }
        }


        private List<LookupReferenceValueModel> _OrderTypes;

        public List<LookupReferenceValueModel> OrderTypes
        {
            get { return _OrderTypes; }
            set { Set(ref _OrderTypes, value); }
        }

        private List<LookupReferenceValueModel> _Priorities;

        public List<LookupReferenceValueModel> Priorities
        {
            get { return _Priorities; }
            set { Set(ref _Priorities, value); }
        }

        private LookupReferenceValueModel _SelectPriority;

        public LookupReferenceValueModel SelectPriority
        {
            get { return _SelectPriority; }
            set { Set(ref _SelectPriority, value); }
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

        public OrderWithOutStockItemViewModel()
        {
            Careproviders = DataService.UserManage.GetCareproviderDoctor();
            if (AppUtil.Current.IsDoctor == true)
            {
                SelectCareprovider = Careproviders.FirstOrDefault(p => p.CareproviderUID == AppUtil.Current.UserID);
            }


            var refVale = DataService.Technical.GetReferenceValueList("PRSTYP,RQPRT");
            OrderTypes = refVale.Where(p => p.DomainCode == "PRSTYP").ToList();
            Priorities = refVale.Where(p => p.DomainCode == "RQPRT").ToList();

            SelectPriority = Priorities.FirstOrDefault(p => p.ValueCode == "NORML");
        }
        public void BindingFromBillableItem()
        {
            DateTime now = DateTime.Now;

            TypeOrder = BillableItem.BillingServiceMetaData;
            OrderName = BillableItem.ItemName;
            OrderCode = "Code : " + BillableItem.Code;
            UnitPrice = BillableItem.Price.ToString("#,#.00");
            Quantity = 1;

            if (BillableItem.DoctorFee != null && BillableItem.DoctorFee != 0 && BillableItem.BSMDDUID != 2841)
            {
                CareproviderVisibility = Visibility.Visible;
            }
            else
            {
                CareproviderVisibility = Visibility.Collapsed;
            }

            if (BillableItem.BSMDDUID == 2813 || BillableItem.BSMDDUID == 2841)
            {
                PriorityVisibility = Visibility.Visible;
            }
            else
            {
                PriorityVisibility = Visibility.Collapsed;
            }
        }

        public void BindingFromPatientOrderDetail()
        {
            TypeOrder = PatientOrderDetail.BillingService;
            OrderName = PatientOrderDetail.ItemName;
            OrderCode = "Code : " + PatientOrderDetail.ItemCode;
            UnitPrice = PatientOrderDetail.OriginalUnitPrice.Value.ToString("#,#.00");
            Quantity = PatientOrderDetail.Quantity ?? 1;

            StartDate = PatientOrderDetail.StartDttm.Value.Date;
            StartTime = PatientOrderDetail.StartDttm.Value;
            Notes = PatientOrderDetail.Comments;
            OverwritePrice = PatientOrderDetail.OverwritePrice;
            SelectCareprovider = Careproviders != null ? Careproviders.FirstOrDefault(p => p.CareproviderUID == PatientOrderDetail.CareproviderUID) : null;
            SelectPriority = Priorities != null ? Priorities.FirstOrDefault(p => p.Key == PatientOrderDetail.ORDPRUID) : null;
            if (PatientOrderDetail.DoctorFee != null && PatientOrderDetail.DoctorFee != 0 && PatientOrderDetail.BSMDDUID != 2841)
            {
                CareproviderVisibility = Visibility.Visible;
            }
            else
            {
                CareproviderVisibility = Visibility.Hidden;
            }

            if (PatientOrderDetail.BSMDDUID == 2813 || PatientOrderDetail.BSMDDUID == 2841)
            {
                PriorityVisibility = Visibility.Visible;
            }
            else
            {
                PriorityVisibility = Visibility.Hidden;
            }
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
                
                if(CareproviderVisibility == Visibility.Visible)
                {
                    if (SelectCareprovider == null)
                    {
                        WarningDialog("กรุณาเลือก แพทย์");
                        return;
                    }
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
                    PatientOrderDetail.OriginalUnitPrice = BillableItem.Price;
                    PatientOrderDetail.DoctorFeePer = BillableItem.DoctorFee;
                    PatientOrderDetail.OrderCatagoryUID = BillableItem.OrderCategoryUID;
                    PatientOrderDetail.OrderSubCategoryUID = BillableItem.OrderSubCategoryUID;
                }

                if (CareproviderVisibility == Visibility.Visible)
                {
                    PatientOrderDetail.CareproviderUID = SelectCareprovider != null ? SelectCareprovider.CareproviderUID : (int?)null;
                    PatientOrderDetail.CareproviderName = SelectCareprovider != null ? SelectCareprovider.FullName : null;
                }
                PatientOrderDetail.Comments = Notes;

                PatientOrderDetail.Quantity = Quantity;

                PatientOrderDetail.StartDttm = StartDate.Add(StartTime.TimeOfDay);
                PatientOrderDetail.EndDttm = StartDate.AddDays(1);

                if (SelectPriority != null)
                {
                    PatientOrderDetail.ORDPRUID = SelectPriority.Key;
                }

                PatientOrderDetail.PRSTYPUID = OrderTypes.FirstOrDefault(p => p.ValueCode == "ROMED").Key;
                PatientOrderDetail.OrderType = OrderTypes.FirstOrDefault(p => p.ValueCode == "ROMED").Display;

          

                if (OverwritePrice != null)
                {
                    PatientOrderDetail.UnitPrice = OverwritePrice;
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
                    PatientOrderDetail.UnitPrice = PatientOrderDetail.OriginalUnitPrice;
                    PatientOrderDetail.DisplayPrice = PatientOrderDetail.UnitPrice;
                    //PatientOrderDetail.NetAmount = (PatientOrderDetail.UnitPrice ?? 0) + (PatientOrderDetail.DoctorFee ?? 0);
                    PatientOrderDetail.NetAmount = ((PatientOrderDetail.UnitPrice ?? 0) * PatientOrderDetail.Quantity);
                }

                if (BillableItem == null)
                {
                    BillableItem = DataService.MasterData.GetBillableItemByUID(PatientOrderDetail.BillableItemUID);
                }
                PatientOrderDetail.DoctorFee = (PatientOrderDetail.DoctorFeePer / 100) * PatientOrderDetail.NetAmount;

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
