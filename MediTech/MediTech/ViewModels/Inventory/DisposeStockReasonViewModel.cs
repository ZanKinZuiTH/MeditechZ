using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediTech.Model;
namespace MediTech.ViewModels
{
    public class DisposeStockReasonViewModel : MediTechViewModelBase
    {
        #region Properties

        private List<LookupReferenceValueModel> _DisposeReason;

        public List<LookupReferenceValueModel> DisposeReason
        {
            get { return _DisposeReason; }
            set { Set(ref _DisposeReason , value); }
        }

        private LookupReferenceValueModel _SelectDisposeReason;

        public LookupReferenceValueModel SelectDisposeReason
        {
            get { return _SelectDisposeReason; }
            set { Set(ref _SelectDisposeReason, value); }
        }

        private List<StockModel> _StockForDispose;

        public List<StockModel> StockForDispose
        {
            get { return _StockForDispose; }
            set { Set(ref _StockForDispose, value); }
        }

        private string _Comments;

        public string Comments
        {
            get { return _Comments; }
            set { Set(ref _Comments, value); }
        }


        #endregion

        #region Command

        private RelayCommand _SaveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return _SaveCommand ?? (_SaveCommand = new RelayCommand(Save));
            }
        }

        private RelayCommand _CancelCommand;
        public RelayCommand CancelCommand
        {
            get
            {
                return _CancelCommand ?? (_CancelCommand = new RelayCommand(Cancel));
            }
        }

        #endregion

        #region Method

        public DisposeStockReasonViewModel(List<StockModel> selectStockDispose)
        {
            StockForDispose = selectStockDispose;
            DisposeReason = DataService.Technical.GetReferenceValueMany("DSPRSN");
        }
        void Save()
        {
            try
            {
                if (SelectDisposeReason == null)
                {
                    WarningDialog("กรุณาเลือกเหตุผล");
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
