using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class MergeBillRecipetPopupViewModel : MediTechViewModelBase
    {

        #region Properties


        private List<PatientVisitPayorModel> _PatientVisitPayors;

        public List<PatientVisitPayorModel> PatientVisitPayors
        {
            get { return _PatientVisitPayors; }
            set { Set(ref _PatientVisitPayors, value); }
        }

        private List<AllocatedPatBillableItemsResultModel> _BillableItems;

        public List<AllocatedPatBillableItemsResultModel> BillableItems
        {
            get { return _BillableItems; }
            set { Set(ref _BillableItems, value); }
        }


        private PatientVisitPayorModel _SelectedSourceVisitPayor;
        public PatientVisitPayorModel SelectedSourceVisitPayor
        {
            get
            {
                return _SelectedSourceVisitPayor;
            }
            set
            {

                if (_SelectedSourceVisitPayor != value)
                {
                    Set(ref _SelectedSourceVisitPayor, value);
                    SourcePayorAmount = BillableItems.Where(f => (f.PatientVisitPayorUID ?? 0) == _SelectedSourceVisitPayor.PatientVisitPayorUID).Sum(p => p.Amount)?.ToString() ?? "0";
                }
            }
        }

        private PatientVisitPayorModel _SelectedDestinationVisitPayor;
        public PatientVisitPayorModel SelectedDestinationVisitPayor
        {
            get
            {
                return _SelectedDestinationVisitPayor;
            }
            set
            {
                if (_SelectedDestinationVisitPayor != value)
                {
                    Set(ref _SelectedDestinationVisitPayor, value);
                    DestinationPayorAmount = BillableItems.Where(f => (f.PatientVisitPayorUID ?? 0) == _SelectedDestinationVisitPayor.PatientVisitPayorUID).Sum(p => p.Amount)?.ToString() ?? "0";
                }
            }
        }

        private string _SourcePayorAmount;

        public string SourcePayorAmount
        {
            get { return _SourcePayorAmount; }
            set
            {
                Set(ref _SourcePayorAmount, value);
            }
        }

        private string _DestinationPayorAmount;

        public string DestinationPayorAmount
        {
            get { return _DestinationPayorAmount; }
            set
            {
                Set(ref _DestinationPayorAmount, value);
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

        public void AssignMergeRecipet(long patientUID, long patintVisitUID,AllocatedPatBillableItemsResultModel allocatedItem,
            List<AllocatedPatBillableItemsResultModel> BillableItems, List<PatientVisitPayorModel> PatientVisitPayors, DateTime? fromDate, DateTime? toDate
    )
        {

            this.PatientVisitPayors = PatientVisitPayors;
            this.BillableItems = BillableItems;
            SelectedSourceVisitPayor = this.PatientVisitPayors.FirstOrDefault(p => p.PatientVisitPayorUID == allocatedItem.PatientVisitPayorUID);
            SelectedDestinationVisitPayor = this.PatientVisitPayors.FirstOrDefault(p => p.PatientVisitPayorUID == allocatedItem.PatientVisitPayorUID);
            FromDttm = fromDate;
            ToDttm = toDate;

        }
        void Save()
        {
            try
            {
                if ((SelectedDestinationVisitPayor.PatientVisitPayorUID == SelectedDestinationVisitPayor.PatientVisitPayorUID))
                {

                    WarningDialog("Cannot add dupicate payor");
                    return;
                }
                CloseViewDialog(ActionDialog.Save);
            }
            catch (Exception ex)
            {

                ErrorDialog(ex.Message);
            }
        }

        void Cancel()
        {
            CloseViewDialog(ActionDialog.Cancel);
        }

        #endregion
    }
}
