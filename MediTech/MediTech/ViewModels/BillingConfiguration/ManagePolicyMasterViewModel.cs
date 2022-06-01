using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class ManagePolicyMasterViewModel : MediTechViewModelBase
    {
        #region Properties

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { Set(ref _Name, value); }
        }

        private string _Code;
        public string Code
        {
            get { return _Code; }
            set { Set(ref _Code, value); }
        }

        private string _Description;
        public string Description
        {
            get { return _Description; }
            set { Set(ref _Description, value); }
        }
        #endregion

        #region Command

        private RelayCommand _SaveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return _SaveCommand
                    ?? (_SaveCommand = new RelayCommand(Save));
            }
        }

        private RelayCommand _CancelCommand;
        public RelayCommand CancelCommand
        {
            get
            {
                return _CancelCommand
                    ?? (_CancelCommand = new RelayCommand(Cancel));
            }
        }
        #endregion

        #region Method
        PolicyMasterModel policyMasterModel;

        private void Save()
        {
            if (String.IsNullOrEmpty(Code))
            {
                WarningDialog("กรุณาระบุ Code");
                return;
            }

            if (string.IsNullOrEmpty(Name))
            {
                WarningDialog("กรุณาระบุ Company Name");
                return;
            }

            AssingPropertiesToModel();
            DataService.Billing.ManagePolicyMaster(policyMasterModel, AppUtil.Current.UserID);
            CloseViewDialog(ActionDialog.Save);

        }

        public void AssingModel(int policyMasterUID)
        {
            policyMasterModel = DataService.Billing.GetPolicyMasterByUID(policyMasterUID);
            AssingModelToProperties(policyMasterModel);
        }
        private void AssingPropertiesToModel()
        {
            if (policyMasterModel == null)
            {
                policyMasterModel = new PolicyMasterModel();
            }

            policyMasterModel.Code = Code;
            policyMasterModel.PolicyName = Name;
            policyMasterModel.Description = Description;
        }

        private void AssingModelToProperties(PolicyMasterModel model)
        {
            Code = model.Code;
            Name = model.PolicyName;
            Description = model.Description;
            
        }
        private void Cancel()
        {
            CloseViewDialog(ActionDialog.Cancel);
        }

        #endregion
    }
}
