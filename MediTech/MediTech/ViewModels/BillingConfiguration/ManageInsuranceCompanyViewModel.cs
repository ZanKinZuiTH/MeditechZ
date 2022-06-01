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
    public class ManageInsuranceCompanyViewModel : MediTechViewModelBase
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

        private List<LookupReferenceValueModel> _CreditTerm;
        public List<LookupReferenceValueModel> CreditTerm
        {
            get { return _CreditTerm; }
            set { Set(ref _CreditTerm, value); }
        }

        private LookupReferenceValueModel _SelectCreditTerm;
        public LookupReferenceValueModel SelectCreditTerm
        {
            get { return _SelectCreditTerm; }
            set { Set(ref _SelectCreditTerm, value); }
        }

        private DateTime? _ActiveFrom;
        public DateTime? ActiveFrom
        {
            get { return _ActiveFrom; }
            set { Set(ref _ActiveFrom, value); }
        }

        private DateTime? _ActiveTo;
        public DateTime? ActiveTo
        {
            get { return _ActiveTo; }
            set { Set(ref _ActiveTo, value); }
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
        public InsuranceCompanyModel incoModel;

        public ManageInsuranceCompanyViewModel()
        {
            CreditTerm = DataService.Technical.GetReferenceValueMany("CMPTP");
        }

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
            DataService.MasterData.ManageInsuranceCompany(incoModel, AppUtil.Current.UserID);
            CloseViewDialog(ActionDialog.Save);

        }
        public void AssingModel(int InsuranceCompanyUID)
        {
            incoModel = DataService.MasterData.GetInsuranceCompanyByUID(InsuranceCompanyUID);
            AssingModelToProperties(incoModel); 
        }

        private void AssingPropertiesToModel()
        {
            if (incoModel == null)
            {
                incoModel = new InsuranceCompanyModel();
            }

            incoModel.Code = Code;
            incoModel.CompanyName = Name;
            incoModel.Description = Description;
            incoModel.ActiveFrom = ActiveFrom;
            incoModel.ActiveTo = ActiveTo;
            incoModel.CMPTPUID = SelectCreditTerm != null ? SelectCreditTerm.Key : (int?)null;
        }

        private void AssingModelToProperties(InsuranceCompanyModel model)
        {
            Code = model.Code;
            Name = model.CompanyName;
            Description = model.Description;
            ActiveFrom = model.ActiveFrom;
            ActiveTo = model.ActiveTo;
            if (model.CMPTPUID != null)
                SelectCreditTerm = CreditTerm.FirstOrDefault(p => p.Key == model.CMPTPUID);
        }

        private void Cancel()
        {
            CloseViewDialog(ActionDialog.Cancel);
        }

        #endregion

    }
}
