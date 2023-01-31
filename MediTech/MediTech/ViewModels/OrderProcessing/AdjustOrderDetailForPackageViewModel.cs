using DevExpress.Xpo.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using MediTech.Model;
using MediTech.Views;
using DevExpress.XtraScheduler.Commands;

namespace MediTech.ViewModels
{
    public class AdjustOrderDetailForPackageViewModel : MediTechViewModelBase
    {

        #region Properties

        public int BillableItemUID { get; set; }
        public long PatientVisitUID { get; set; }

        public long PatientPackageUID { get; set; }

        private string _Packagename;

        public string Packagename
        {
            get { return _Packagename; }
            set { Set(ref _Packagename, value); }
        }

        private string _PackageCreateBy;

        public string PackageCreateBy
        {
            get { return _PackageCreateBy; }
            set { Set(ref _PackageCreateBy, value); }
        }

        private ObservableCollection<LinkPackageModel> _OrderDetails;

        public ObservableCollection<LinkPackageModel> OrderDetails
        {
            get { return _OrderDetails ?? (_OrderDetails = new ObservableCollection<LinkPackageModel>()); }
            set
            {
                Set(ref _OrderDetails, value);

            }
        }

        private LinkPackageModel _SelectPatientOrderDetail;

        public LinkPackageModel SelectPatientOrderDetail
        {
            get { return _SelectPatientOrderDetail; }
            set
            {
                Set(ref _SelectPatientOrderDetail, value);

            }
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

        public AdjustOrderDetailForPackageViewModel(string packagename,string packageCreateBy,long patientPackageUID,int billableItemUID,long patientVisitUID)
        {
            this.Packagename = packagename;
            this.PackageCreateBy = packageCreateBy;
            this.PatientPackageUID = patientPackageUID;
            this.BillableItemUID = billableItemUID;
            this.PatientVisitUID = patientVisitUID;
        }

        public override void OnLoaded()
        {
            var dataOrderDetails = DataService.OrderProcessing.GetLinkPackage(BillableItemUID, PatientVisitUID);
            var OrderDetails = new ObservableCollection<LinkPackageModel>(dataOrderDetails?.Where(P => P.PatientPackageUID == 0 || P.PatientPackageUID == PatientPackageUID));
            foreach (var item in OrderDetails)
            {
                if (item.PatientPackageUID == PatientPackageUID)
                {
                    item.IsSelected = true;
                }
            }
            this.OrderDetails = OrderDetails;
            base.OnLoaded();
        }

        void Save()
        {
            try
            {
                foreach (var item in OrderDetails)
                {
                    DataService.OrderProcessing.UpdateLinkPackage(item.UID, PatientPackageUID, item.IsSelected, AppUtil.Current.UserID);
                }
                this.CloseViewDialog(ActionDialog.Save);
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }

        void Cancel()
        {
            this.CloseViewDialog(ActionDialog.Cancel);
        }
        #endregion
    }
}
