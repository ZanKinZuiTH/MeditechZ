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
    public class CancelOrderViewModel : MediTechViewModelBase
    {

        #region Properties

        public string CancelReason { get; set; }

        private List<PatientOrderDetailModel> _ListOrderCancel;

        public List<PatientOrderDetailModel> ListOrderCancel
        {
            get { return _ListOrderCancel; }
            set { Set(ref _ListOrderCancel, value); }
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

        public void Save()
        {
            try
            {
                List<long> patientOrderDetailUIDs = ListOrderCancel.Select(p => p.PatientOrderDetailUID).ToList();
                DataService.OrderProcessing.CancelOrders(patientOrderDetailUIDs, CancelReason, AppUtil.Current.UserID);
                SaveSuccessDialog();
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
