using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MediTech.ViewModels
{
    public class ListPolicyMasterViewModel : MediTechViewModelBase
    {
        #region Properties
        private List<PolicyMasterModel> _PolicyMasterSource;
        public List<PolicyMasterModel> PolicyMasterSource
        {
            get { return _PolicyMasterSource; }
            set { Set(ref _PolicyMasterSource, value); }
        }

        private PolicyMasterModel _SelectPolicyMaster;
        public PolicyMasterModel SelectPolicyMaster
        {
            get { return _SelectPolicyMaster; }
            set { Set(ref _SelectPolicyMaster, value); }
        }

        private string _ItemCode;
        public string ItemCode
        {
            get { return _ItemCode; }
            set { Set(ref _ItemCode, value); }
        }

        private string _ItemName;
        public string ItemName
        {
            get { return _ItemName; }
            set { Set(ref _ItemName, value); }
        }
        #endregion

        #region Command
        private RelayCommand _AddCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return _AddCommand ?? (_AddCommand = new RelayCommand(Add));
            }
        }

        private RelayCommand _SearchCommand;
        public RelayCommand SearchCommand
        {
            get
            {
                return _SearchCommand ?? (_SearchCommand = new RelayCommand(Search));
            }
        }

        private RelayCommand _ModifyCommand;
        public RelayCommand ModifyCommand
        {
            get
            {
                return _ModifyCommand ?? (_ModifyCommand = new RelayCommand(Modify));
            }
        }

        private RelayCommand _DeleteCommand;
        public RelayCommand DeleteCommand
        {
            get
            {
                return _DeleteCommand ?? (_DeleteCommand = new RelayCommand(Delete));
            }
        }
        #endregion

        #region Method

        public ListPolicyMasterViewModel()
        {
            PolicyMasterSource = DataService.Billing.GetPolicyMasterAll();
        }

        private void Search()
        {
            PolicyMasterSource = DataService.Billing.SearchPolicyMaster(ItemCode, ItemName);
        }

        private void Add()
        {
                ManagePolicyMaster pageview = new ManagePolicyMaster();
                ManagePolicyMasterViewModel result = (ManagePolicyMasterViewModel)LaunchViewDialog(pageview, "MNPCMT", false,true);
                if (result != null && result.ResultDialog == ActionDialog.Save)
                {
                    SaveSuccessDialog();
                    PolicyMasterSource = DataService.Billing.GetPolicyMasterAll();
                }
        }

        private void Modify()
        {
            if (SelectPolicyMaster != null)
            {
                ManagePolicyMaster pageview = new ManagePolicyMaster();
                (pageview.DataContext as ManagePolicyMasterViewModel).AssingModel(SelectPolicyMaster.PolicyMasterUID);
                ManagePolicyMasterViewModel result = (ManagePolicyMasterViewModel)LaunchViewDialog(pageview, "MNPCMT", false, true);
                if (result != null && result.ResultDialog == ActionDialog.Save)
                {
                    SaveSuccessDialog();
                    PolicyMasterSource = DataService.Billing.GetPolicyMasterAll();
                }
            }
        }

        private void Delete()
        {
            if (SelectPolicyMaster != null)
            {
                try
                {
                    MessageBoxResult result = DeleteDialog();
                    if (result == MessageBoxResult.Yes)
                    {
                        DataService.Billing.DeletePolicyMaster(SelectPolicyMaster.PolicyMasterUID, AppUtil.Current.UserID);
                        DeleteSuccessDialog();
                        PolicyMasterSource.Remove(SelectPolicyMaster);
                        PolicyMasterSource = DataService.Billing.GetPolicyMasterAll();
                    }
                }
                catch (Exception er)
                {

                    ErrorDialog(er.Message);
                }
            }
        }

        #endregion
    }
}
