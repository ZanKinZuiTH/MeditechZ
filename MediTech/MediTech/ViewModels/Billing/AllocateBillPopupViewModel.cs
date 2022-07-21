using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class AllocateBillPopupViewModel : MediTechViewModelBase
    {

        #region Variables
        double _amount;
        double _netAmount;
        double _discountPercent;
        public AllocatedPatBillableItemsResultModel[] _results = null;
        double _amountSplitPercentDecimal;
        double _itemAmountSplit;
        bool _isNetAmountEdit;
        bool _isAmountEdit;
        //
        bool _isSubGroupTotalEdit;
        bool _isSubGroupNetEdit;
        bool _isGroupTotalEdit;
        bool _isGroupNetEdit;
        //
        #endregion

        #region Properties

        #region PatientUID
        private long _PatientUID;
        public long PatientUID
        {
            get
            {
                return _PatientUID;
            }
            set
            {
                if (_PatientUID != value)
                {
                    Set(ref _PatientUID, value);
                }
            }
        }
        #endregion
        //
        #region PatientVisitUID
        private long _PatientVisitUID;
        public long PatientVisitUID
        {
            get
            {
                return _PatientVisitUID;
            }
            set
            {
                if (_PatientVisitUID != value)
                {
                    Set(ref _PatientVisitUID, value);
                }
            }
        }
        #endregion

        private AllocatedPatBillableItemsResultModel _SelectAllocateItem;

        public AllocatedPatBillableItemsResultModel SelectAllocateItem
        {
            get { return _SelectAllocateItem; }
            set { Set(ref _SelectAllocateItem, value); }
        }


        private List<PatientVisitPayorModel> _PatientVisitPayors;

        public List<PatientVisitPayorModel> PatientVisitPayors
        {
            get { return _PatientVisitPayors; }
            set { Set(ref _PatientVisitPayors, value); }
        }



        private PatientVisitPayorModel _SelectedModifyPatientVisitPayor;
        public PatientVisitPayorModel SelectedModifyPatientVisitPayor
        {
            get
            {
                return _SelectedModifyPatientVisitPayor;
            }
            set
            {
                Set(ref _SelectedModifyPatientVisitPayor, value);
            }
        }


        private bool _CanKeepDiscount;
        public bool CanKeepDiscount
        {
            get
            {
                return _CanKeepDiscount;
            }
            set
            {
                Set(ref _CanKeepDiscount, value);
            }
        }

        private bool _IsItemSplit;
        public bool IsItemSplit
        {
            get
            {
                return _IsItemSplit;
            }
            set
            {
                Set(ref _IsItemSplit, value);
            }
        }

        #region SubAccountItem
        private double _DisplayAmount;
        public double DisplayAmount
        {
            get
            {
                return _DisplayAmount;
            }
            set
            {
                Set(ref _DisplayAmount, value);
            }
        }

        private double _Discount;
        public double Discount
        {
            get
            {
                return _Discount;
            }
            set
            {
                Set(ref _Discount, value);
            }
        }

        private double _DisplayNetAmount;
        public double DisplayNetAmount
        {
            get
            {
                return _DisplayNetAmount;
            }
            set
            {
                Set(ref _DisplayNetAmount, value);
            }
        }

        private string _AmountString;
        public string AmountString
        {
            get
            {
                return _AmountString;
            }
            set
            {
                if (_AmountString != value)
                {
                    Set(ref _AmountString, value);
                    if ((!_isAmountEdit && !_isNetAmountEdit))
                    {
                        _isNetAmountEdit = false;
                        _isAmountEdit = true;
                        if (AmountString != null && !AmountString.Contains('%') && double.TryParse(AmountString, out _amount))
                        {
                            _amountSplitPercentDecimal = (_amount / DisplayAmount);
                            Calculate(false);
                        }
                        else if (AmountString != null && AmountString.Contains('%'))
                        {
                            double percentAmount;
                            double.TryParse(AmountString.Trim('%'), out percentAmount);
                            _amountSplitPercentDecimal = percentAmount / 100;
                            Calculate(false);
                        }
                        _isNetAmountEdit = false;
                        _isAmountEdit = false;
                    }
                }
            }
        }


        private double _convertedDiscount;
        public double ConvertedDiscount
        {
            get
            {
                return _convertedDiscount;
            }
            set
            {
                if (_convertedDiscount != value)
                {
                    Set(ref _convertedDiscount, value);
                }
            }
        }


        private string _netAmountString;
        public string NetAmountString
        {
            get
            {
                return _netAmountString;
            }
            set
            {
                if (_netAmountString != value)
                {
                    Set(ref _netAmountString, value);
                    if ((!_isNetAmountEdit && !_isAmountEdit))
                    {
                        _isNetAmountEdit = true;
                        _isAmountEdit = false;
                        if (NetAmountString != null && !NetAmountString.Contains('%') && double.TryParse(NetAmountString, out _netAmount))
                        {
                            _amountSplitPercentDecimal = (_netAmount / DisplayNetAmount);
                            Calculate(true);
                        }
                        else if (NetAmountString != null && NetAmountString.Contains('%'))
                        {
                            double percentAmount;
                            double.TryParse(NetAmountString.Trim('%'), out percentAmount);
                            _amountSplitPercentDecimal = percentAmount / 100;
                            Calculate(true);
                        }
                        _isNetAmountEdit = false;
                        _isAmountEdit = false;
                    }
                }
            }
        }


        private double _itemAmount;
        public double ItemAmount
        {
            get
            {
                return _itemAmount;
            }
            set
            {
                if (_itemAmount != value)
                {
                    Set(ref _itemAmount, value);
                }
            }
        }


        private double _convertedNetAmount;
        public double ConvertedNetAmount
        {
            get
            {
                return _convertedNetAmount;
            }
            set
            {
                if (_convertedNetAmount != value)
                {
                    Set(ref _convertedNetAmount, value);
                }
            }
        }


        #endregion

        #region SubGroup

        private double _subGroupTotal;
        public double SubGroupTotal
        {
            get
            {
                return _subGroupTotal;
            }
            set
            {
                if (_subGroupTotal != value)
                {
                    Set(ref _subGroupTotal, value);
                }
            }
        }


        private double _subGroupDiscount;
        public double SubGroupDiscount
        {
            get
            {
                return _subGroupDiscount;
            }
            set
            {
                if (_subGroupDiscount != value)
                {
                    Set(ref _subGroupDiscount, value);
                }
            }
        }

        private double _subGroupCovered;
        public double SubGroupCovered
        {
            get
            {
                return _subGroupCovered;
            }
            set
            {
                if (_subGroupCovered != value)
                {
                    Set(ref _subGroupCovered, value);
                }
            }
        }


        private string _subGroupTotalSplitAmount;
        public string SubGroupTotalSplitAmount
        {
            get
            {
                return _subGroupTotalSplitAmount;
            }
            set
            {
                if (_subGroupTotalSplitAmount != value)
                {
                    Set(ref _subGroupTotalSplitAmount, value);
                    if (!_isSubGroupNetEdit)
                    {
                        _isSubGroupTotalEdit = true;
                        if (SubGroupTotalSplitAmount != null && !SubGroupTotalSplitAmount.Contains('%') && double.TryParse(SubGroupTotalSplitAmount, out _amount))
                        {
                            _amountSplitPercentDecimal = (_amount / SubGroupTotal);
                            //Calculate(false);
                        }
                        else if (SubGroupTotalSplitAmount != null && SubGroupTotalSplitAmount.Contains('%'))
                        {
                            double percentAmount;
                            double.TryParse(SubGroupTotalSplitAmount.Trim('%'), out percentAmount);
                            _amountSplitPercentDecimal = percentAmount / 100;
                            //Calculate(false);
                        }
                        SubGroupDisplayDiscount = Math.Round(SubGroupDiscount * _amountSplitPercentDecimal, 2);
                        SubGroupSplitAmount = Math.Round(SubGroupCovered * _amountSplitPercentDecimal, 2).ToString();
                        _isSubGroupTotalEdit = false;
                    }
                }
            }
        }

        private double _subGroupDisplayDiscount;
        public double SubGroupDisplayDiscount
        {
            get
            {
                return _subGroupDisplayDiscount;
            }
            set
            {
                if (_subGroupDisplayDiscount != value)
                {
                    Set(ref _subGroupDisplayDiscount, value);
                }
            }
        }

        private string _SubGroupSplitAmount;
        public string SubGroupSplitAmount
        {
            get
            {
                return _SubGroupSplitAmount;
            }
            set
            {
                if (_SubGroupSplitAmount != value)
                {
                    Set(ref _SubGroupSplitAmount, value);
                    if (!_isSubGroupTotalEdit)
                    {
                        _isSubGroupNetEdit = true;
                        if (SubGroupSplitAmount != null && !SubGroupSplitAmount.Contains('%') && double.TryParse(SubGroupSplitAmount, out _amount))
                        {
                            _amountSplitPercentDecimal = (_amount / SubGroupCovered);
                            //Calculate(false);
                        }
                        else if (SubGroupSplitAmount != null && SubGroupSplitAmount.Contains('%'))
                        {
                            double percentAmount;
                            double.TryParse(SubGroupSplitAmount.Trim('%'), out percentAmount);
                            _amountSplitPercentDecimal = percentAmount / 100;
                            //Calculate(false);
                        }
                        SubGroupDisplayDiscount = Math.Round(SubGroupDiscount * _amountSplitPercentDecimal, 2);
                        SubGroupTotalSplitAmount = Math.Round(SubGroupTotal * _amountSplitPercentDecimal, 2).ToString();
                        _isSubGroupNetEdit = false;
                    }
                }
            }
        }
        #endregion




        private double _groupTotal;
        public double GroupTotal
        {
            get
            {
                return _groupTotal;
            }
            set
            {
                if (_groupTotal != value)
                {
                    Set(ref _groupTotal, value);
                }
            }
        }


        private double _groupDiscount;
        public double GroupDiscount
        {
            get
            {
                return _groupDiscount;
            }
            set
            {
                if (_groupDiscount != value)
                {
                    Set(ref _groupDiscount, value);
                }
            }
        }

        private double _groupCovered;
        public double GroupCovered
        {
            get
            {
                return _groupCovered;
            }
            set
            {
                if (_groupCovered != value)
                {
                    Set(ref _groupCovered, value);
                }
            }
        }

        private string _groupTotalSplitAmount;
        public string GroupTotalSplitAmount
        {
            get
            {
                return _groupTotalSplitAmount;
            }
            set
            {
                if (_groupTotalSplitAmount != value)
                {
                    Set(ref _groupTotalSplitAmount, value);
                    if (!_isGroupNetEdit)
                    {
                        _isGroupTotalEdit = true;
                        if (GroupTotalSplitAmount != null && !GroupTotalSplitAmount.Contains('%') && double.TryParse(GroupTotalSplitAmount, out _amount))
                        {
                            _amountSplitPercentDecimal = (_amount / GroupTotal);
                            //Calculate(false);
                        }
                        else if (GroupTotalSplitAmount != null && GroupTotalSplitAmount.Contains('%'))
                        {
                            double percentAmount;
                            double.TryParse(GroupTotalSplitAmount.Trim('%'), out percentAmount);
                            _amountSplitPercentDecimal = percentAmount / 100;
                            //Calculate(false);
                        }
                        GroupDisplayDiscount = Math.Round(GroupDiscount * _amountSplitPercentDecimal, 2);
                        GroupSplitAmount = Math.Round(GroupCovered * _amountSplitPercentDecimal, 2).ToString();
                        _isGroupTotalEdit = false;
                    }
                }
            }
        }

        private double _groupDisplayDiscount;
        public double GroupDisplayDiscount
        {
            get
            {
                return _groupDisplayDiscount;
            }
            set
            {
                if (_groupDisplayDiscount != value)
                {
                    Set(ref _groupDisplayDiscount, value);
                }
            }
        }


        private string _GroupSplitAmount;
        public string GroupSplitAmount
        {
            get
            {
                return _GroupSplitAmount;
            }
            set
            {
                if (_GroupSplitAmount != value)
                {
                    Set(ref _GroupSplitAmount, value);
                    if (!_isGroupTotalEdit)
                    {
                        _isGroupNetEdit = true;
                        if (GroupSplitAmount != null && !GroupSplitAmount.Contains('%') && double.TryParse(GroupSplitAmount, out _amount))
                        {
                            _amountSplitPercentDecimal = (_amount / GroupCovered);
                            //Calculate(false);
                        }
                        else if (GroupSplitAmount != null && GroupSplitAmount.Contains('%'))
                        {
                            double percentAmount;
                            double.TryParse(GroupSplitAmount.Trim('%'), out percentAmount);
                            _amountSplitPercentDecimal = percentAmount / 100;
                            //Calculate(false);
                        }
                        GroupDisplayDiscount = Math.Round(GroupDiscount * _amountSplitPercentDecimal, 2);
                        GroupTotalSplitAmount = Math.Round(GroupTotal * _amountSplitPercentDecimal, 2).ToString();
                        _isGroupNetEdit = false;
                    }
                }
            }
        }

        private bool _isNotPackage;
        public bool IsNotPackage
        {
            get
            {
                return _isNotPackage;
            }
            set
            {
                if (_isNotPackage != value)
                {
                    Set(ref _isNotPackage, value);
                }
            }
        }

        private DateTime? _fromDttm;
        public DateTime? FromDttm
        {
            get
            {
                return _fromDttm;
            }
            set
            {
                if (_fromDttm != value)
                {
                    Set(ref _fromDttm, value);
                }
            }
        }

        //
        private DateTime? _toDttm;
        public DateTime? ToDttm
        {
            get
            {
                return _toDttm;
            }
            set
            {
                if (_toDttm != value)
                {
                    Set(ref _toDttm, value);
                }
            }
        }

        private double _quantity;
        public double Quantity
        {
            get
            {
                return _quantity;
            }
            set
            {
                if (_quantity != value)
                {
                    Set(ref _quantity, value);
                }
            }
        }

        private long _splitPayorUID;
        public long SplitPayorUID
        {
            get
            {
                return _splitPayorUID;
            }
            set
            {
                if (_splitPayorUID != value)
                {
                    Set(ref _splitPayorUID, value);
                }
            }
        }


        #endregion

        #region Command
        private RelayCommand _SaveCommand;

        public RelayCommand SaveCommand
        {
            get { return _SaveCommand ?? (_SaveCommand = new RelayCommand(Save)); }
        }

        private RelayCommand _CancelCommand;

        public RelayCommand CancelCommand
        {
            get { return _CancelCommand ?? (_CancelCommand = new RelayCommand(Cancel)); }
        }

        #endregion

        #region Method

        public void AssignAllocatedBillableItem(AllocatedPatBillableItemsResultModel allocatedItem, List<AllocatedPatBillableItemsResultModel> subGroupItems,
            List<AllocatedPatBillableItemsResultModel> GroupItems, List<AllocatedPatBillableItemsResultModel> BillableItems, List<PatientVisitPayorModel> PatientVisitPayors,
            long patientUID, long patintVisitUID, DateTime? fromDate, DateTime? toDate
            )
        {


            this.PatientVisitPayors = PatientVisitPayors;

            SelectAllocateItem = allocatedItem;
            SelectedModifyPatientVisitPayor = new PatientVisitPayorModel();
            PatientUID = patientUID;
            PatientVisitUID = patintVisitUID;
            //
            if (SelectAllocateItem.Amount.HasValue)
            {
                CalculateNetAmount(BillableItems);

                SubGroupCovered = SelectAllocateItem.SubGroupCovered ?? 0;
                SubGroupTotal = SelectAllocateItem.SubGroupTotal ?? 0;
                SubGroupDiscount = SelectAllocateItem.SubGroupDiscount ?? 0;
                GroupCovered = SelectAllocateItem.GroupCovered ?? 0;
                GroupTotal = SelectAllocateItem.GroupTotal ?? 0;
                GroupDiscount = SelectAllocateItem.GroupDiscount ?? 0;
                if (Discount > 0)
                    _discountPercent = (Discount * (100 / (ItemAmount * Quantity)));

                if (SelectAllocateItem.PackageName == null || SelectAllocateItem.PackageName.Length == 0)
                    IsNotPackage = true;
                FromDttm = fromDate;
                ToDttm = toDate;
            }
        }

        void Save()
        {
            try
            {
                String process;
                if (SelectAllocateItem.PatientVisitPayorUID == SelectedModifyPatientVisitPayor.PatientVisitPayorUID)
                {

                    ErrorDialog((AllocateBillPopup)this.View, "CannotAllocateTo");
                    return;
                }
                //
                if (IsItemSplit && SplitPayorUID == 0)
                {
                    ErrorDialog((AllocateBillPopup)this.View, "ChooseAPayor");
                    return;
                }

                if (IsNotPackage)
                    process = "N";
                else
                    process = "P";

                if (((AllocateBillPopup)View).ControlTab.SelectedIndex == 0)
                {
                    if (IsItemSplit)
                    {
                        new AllocateSplitItemModel() {allocatedPatBillableITemUID = SelectAllocateItem.BillableItemUID,amount=_itemAmountSplit,discount = ConvertedDiscount,netAmount = ConvertedNetAmount,userUID = AppUtil.Current.UserID,groupUID = null,subGroupUID = null,currentVisitPayorUID = SelectAllocateItem.PatientVisitPayorUID ?? 0,isSplit = process,fromDate = FromDttm,toDate = ToDttm ,canKeepDiscount = CanKeepDiscount ? "Y" : "N",discountDecimal = _amountSplitPercentDecimal ,amountDecimal = _amountSplitPercentDecimal };
                    }
                    else
                    {

                        DataService.Billing.AllocatePatientBillableItem(new AllocatePatientBillableItemModel() { patientUID = PatientUID, patientVisitUID = PatientVisitUID, isAutoAllocate = process, patientVisitPayorUID = SelectedModifyPatientVisitPayor.PatientVisitUID
                            , subGroupUID = SelectAllocateItem.SubAccountUID, payorAgreementUID = SelectedModifyPatientVisitPayor.PayorAgreementUID, allocatedVisitPayorUID = SelectAllocateItem.PatientVisitPayorUID ?? 0
                            , patientBillableItemUID = SelectAllocateItem.BillableItemUID, groupUID = SelectAllocateItem.GroupUID, canKeepDiscount = CanKeepDiscount ? "Y" : "N", startDate = FromDttm, endDate = ToDttm });
                    }
                }
                CloseViewDialog(ActionDialog.Save);
            }
            catch (Exception ex)
            {

                ErrorDialog((AllocateBillPopup)this.View, ex.Message);
            }
        }

        void Cancel()
        {
            CloseViewDialog(ActionDialog.Cancel);
        }

        private void CalculateNetAmount(List<AllocatedPatBillableItemsResultModel> BillableItems)
        {
            if (BillableItems != null && BillableItems != null
                && BillableItems.Count() > 0)
            {
                foreach (AllocatedPatBillableItemsResultModel oItem in BillableItems)
                {
                    //ItemAmount = ItemAmount SelectedItem.Amount ?? 0;
                    DisplayAmount = DisplayAmount + oItem.TotalAmount ?? 0;
                    Discount = Discount + oItem.Discount ?? 0;
                    DisplayNetAmount = DisplayNetAmount + oItem.NetAmount ?? 0;
                }
            }
        }

        private void Calculate(bool isNetAmount)
        {
            double totalSplitAmount;
            if (_discountPercent > 0)
            {
                ConvertedDiscount = (Discount * _amountSplitPercentDecimal);
            }
            _itemAmountSplit = (ItemAmount * _amountSplitPercentDecimal);
            //ConvertedNetAmount = (_itemAmountSplit) - ConvertedDiscount;
            if (isNetAmount)
            {
                totalSplitAmount = (DisplayAmount * _amountSplitPercentDecimal);
                AmountString = Math.Round(totalSplitAmount, 2).ToString();
            }
            else
            {
                ConvertedNetAmount = (DisplayNetAmount * _amountSplitPercentDecimal);
                NetAmountString = Math.Round(ConvertedNetAmount, 2).ToString();
            }
        }

        #endregion
    }
}
