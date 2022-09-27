using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class ManageInsurancePlanViewModel : MediTechViewModelBase
    {
        #region Properties

        private int _InsuranceCompanyID;
        public int InsuranceCompanyID
        {
            get { return _InsuranceCompanyID; }
            set
            {
                Set(ref _InsuranceCompanyID, value);
                if (InsuranceCompanyID != 0)
                {
                    AgreementSource = DataService.Billing.SearchPayorAgreementByINCO("", InsuranceCompanyID);
                    PayorOfficeSource = DataService.Billing.SearchPayorDetailByINCO("", InsuranceCompanyID);

                    if (PayorOfficeSource == null)
                    {
                        WarningDialog("ไม่มีรายการ Payor Office กรุณาสร้าง Payor Office");
                        return;
                    }

                    if (AgreementSource == null)
                    {
                        WarningDialog("ไม่มีรายการ Agreement กรุณาสร้าง Agreement");
                        return;
                    }
                }
            }
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

        private List<PayorDetailModel> _PayorOfficeSource;
        public List<PayorDetailModel> PayorOfficeSource
        {
            get { return _PayorOfficeSource; }
            set { Set(ref _PayorOfficeSource, value); }
        }

        private PayorDetailModel _SelectPayorOffice;
        public PayorDetailModel SelectPayorOffice
        {
            get { return _SelectPayorOffice; }
            set { Set(ref _SelectPayorOffice, value); }
        }

        private List<PayorAgreementModel> _AgreementSource;
        public List<PayorAgreementModel> AgreementSource
        {
            get { return _AgreementSource; }
            set { Set(ref _AgreementSource, value); }
        }

        private PayorAgreementModel _SelectAgreement;
        public PayorAgreementModel SelectAgreement
        {
            get { return _SelectAgreement; }
            set
            {
                Set(ref _SelectAgreement, value);
                if (SelectAgreement != null)
                {
                    SelectPolicy = null;
                    if (SelectAgreement.PolicyMasterUID != null)
                    {
                        SelectPolicy = DataService.Billing.GetPolicyMasterByUID(SelectAgreement.PolicyMasterUID ?? 0);
                    }

                }
            }
        }

        private PolicyMasterModel _SelectPolicy;

        public PolicyMasterModel SelectPolicy
        {
            get { return _SelectPolicy; }
            set { Set(ref _SelectPolicy, value); }
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

        #region Methoh
        InsurancePlanModel InsurancePlan;
        public ManageInsurancePlanViewModel()
        {
            //AgreementSource = DataService.MasterData.SearchPayorAgreementByINCO("", InsuranceCompanyID);
            //PayorOfficeSource = DataService.MasterData.SearchPayorDetailByINCO("", InsuranceCompanyID);
            //PolicySource = DataService.Billing.GetPolicyMasterAll();
        }

        private void Save()
        {
            if (SelectAgreement == null)
            {
                WarningDialog("กรุณาเลือก Agreement");
                return;
            }
            if (SelectPolicy == null)
            {
                WarningDialog("ยังไม่ได้ทำการผูก Policy กับ Agreement กรุณาตรวจสอบ");
                return;
            }

            AssignPropertiesToModel();
            DataService.Billing.ManageInsurancePlan(InsurancePlan, AppUtil.Current.UserID);
            CloseViewDialog(ActionDialog.Save);
        }

        private void Cancel()
        {
            CloseViewDialog(ActionDialog.Cancel);
        }

        public void AssignModel(int? insuranceCompanyUID, InsurancePlanModel model = null)
        {
            InsuranceCompanyID = insuranceCompanyUID ?? 0;
            if (model != null)
            {
                InsurancePlan = model;
                AssignModelToProperties(model);
            }
        }

        private void AssignPropertiesToModel()
        {
            if (InsurancePlan == null)
            {
                InsurancePlan = new InsurancePlanModel();
            }

            InsurancePlan.PayorAgreementUID = SelectAgreement != null ? SelectAgreement.PayorAgreementUID : 0;
            InsurancePlan.PayorDetailUID = SelectPayorOffice != null ? SelectPayorOffice.PayorDetailUID : 0;
            InsurancePlan.PolicyMasterUID = SelectPolicy != null ? SelectPolicy.PolicyMasterUID : 0;
            InsurancePlan.OwnerOrganisationUID = AppUtil.Current.OwnerOrganisationUID;
            InsurancePlan.ActiveFrom = ActiveFrom;
            InsurancePlan.ActiveTo = ActiveTo;
            InsurancePlan.InsuranceCompanyUID = InsuranceCompanyID;
        }

        private void AssignModelToProperties(InsurancePlanModel model)
        {
            SelectAgreement = AgreementSource.FirstOrDefault(p => p.PayorAgreementUID == model.PayorAgreementUID);
            if (model.PayorDetailUID != 0)
                SelectPayorOffice = PayorOfficeSource.FirstOrDefault(p => p.PayorDetailUID == model.PayorDetailUID);
            ActiveFrom = model.ActiveFrom;
            ActiveTo = model.ActiveTo;
        }

        #endregion
    }
}
