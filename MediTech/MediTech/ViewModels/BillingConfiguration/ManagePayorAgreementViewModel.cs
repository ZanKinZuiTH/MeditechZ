using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class ManagePayorAgreementViewModel : MediTechViewModelBase
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

        private List<LookupReferenceValueModel> _BillType;
        public List<LookupReferenceValueModel> BillType
        {
            get { return _BillType; }
            set { Set(ref _BillType, value); }
        }

        private LookupReferenceValueModel _SelectBillType;
        public LookupReferenceValueModel SelectBillType
        {
            get { return _SelectBillType; }
            set { Set(ref _SelectBillType, value); }
        }

        private List<LookupReferenceValueModel> _PayorBillType;
        public List<LookupReferenceValueModel> PayorBillType
        {
            get { return _PayorBillType; }
            set { Set(ref _PayorBillType, value); }
        }

        private LookupReferenceValueModel _SelectPayorBillType;
        public LookupReferenceValueModel SelectPayorBillType
        {
            get { return _SelectPayorBillType; }
            set { Set(ref _SelectPayorBillType, value); }
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

        private string _CoverPerDay;
        public string CoverPerDay
        {
            get { return _CoverPerDay; }
            set { Set(ref _CoverPerDay, value); }
        }

        private string _ClaimPercentage;
        public string ClaimPercentage
        {
            get { return _ClaimPercentage; }
            set { Set(ref _ClaimPercentage, value); }
        }

        private List<LookupReferenceValueModel> _PrimaryTariff;
        public List<LookupReferenceValueModel> PrimaryTariff
        {
            get { return _PrimaryTariff; }
            set { Set(ref _PrimaryTariff, value); }
        }

        private LookupReferenceValueModel _SelectPrimaryTariff;
        public LookupReferenceValueModel SelectPrimaryTariff
        {
            get { return _SelectPrimaryTariff; }
            set { Set(ref _SelectPrimaryTariff, value); }
        }

        private List<LookupReferenceValueModel> _SecondaryTariff;
        public List<LookupReferenceValueModel> SecondaryTariff
        {
            get { return _SecondaryTariff; }
            set { Set(ref _SecondaryTariff, value); }
        }

        private LookupReferenceValueModel _SelectSecondaryTariff;
        public LookupReferenceValueModel SelectSecondaryTariff
        {
            get { return _SelectSecondaryTariff; }
            set { Set(ref _SelectSecondaryTariff, value); }
        }

        private List<LookupReferenceValueModel> _TertiaryTariff;
        public List<LookupReferenceValueModel> TertiaryTariff
        {
            get { return _TertiaryTariff; }
            set { Set(ref _TertiaryTariff, value); }
        }

        private LookupReferenceValueModel _SelectTertiaryTariff;
        public LookupReferenceValueModel SelectTertiaryTariff
        {
            get { return _SelectTertiaryTariff; }
            set { Set(ref _SelectTertiaryTariff, value); }
        }

        private List<PolicyMasterModel> _PolicyMaster;
        public List<PolicyMasterModel> PolicyMaster
        {
            get { return _PolicyMaster; }
            set { Set(ref _PolicyMaster, value); }
        }

        private PolicyMasterModel _SelectPolicyMaster;
        public PolicyMasterModel SelectPolicyMaster
        {
            get { return _SelectPolicyMaster; }
            set { Set(ref _SelectPolicyMaster, value); 
                if(SelectPolicyMaster != null)
                {
                    AgreementType = SelectPolicyMaster.AgreementType;
                }
                else
                {
                    AgreementType = null;
                }
            }
        }

        private string _AgreementType;
        public string AgreementType
        {
            get { return _AgreementType; }
            set { Set(ref _AgreementType, value); }
        }

        private int _InsuranceCompanyID;
        public int InsuranceCompanyID
        {
            get { return _InsuranceCompanyID; }
            set
            {
                Set(ref _InsuranceCompanyID, value);
            }
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
        PayorAgreementModel payorAgreement;

        public ManagePayorAgreementViewModel()
        {
            var refValue = DataService.Technical.GetReferenceValueList("PAYTRM,PYRACAT,PBTYP,PBLCT,BLTYP");
            //ProvinceSource = DataService.Technical.GetProvince();
            CreditTerm = refValue.Where(p => p.DomainCode == "PAYTRM").ToList();
            //PayorCategory = refValue.Where(p => p.DomainCode == "PYRACAT").ToList();
            PayorBillType = refValue.Where(p => p.DomainCode == "PBTYP").ToList();
            BillType = refValue.Where(p => p.DomainCode == "BLTYP").ToList();

            PrimaryTariff = refValue.Where(p => p.DomainCode == "PBLCT").ToList();
            SecondaryTariff = refValue.Where(p => p.DomainCode == "PBLCT").ToList();
            TertiaryTariff = refValue.Where(p => p.DomainCode == "PBLCT").ToList();

            PolicyMaster = DataService.MasterData.GetPolicyMasterAll();

        }
        private void Save()
        {
            if (String.IsNullOrEmpty(Name))
            {
                WarningDialog("กรุณาระบุ Name");
                return;
            }
            if (String.IsNullOrEmpty(Code))
            {
                WarningDialog("กรุณาระบุ Code");
                return;
            }

            AssignProprotiesToModel();
            DataService.MasterData.ManagePayorAgreement(payorAgreement, AppUtil.Current.UserID);
            CloseViewDialog(ActionDialog.Save);
        }

        public void AssignModel(int? insuranceCompanyUID, PayorAgreementModel agreementModel = null)
        {
            InsuranceCompanyID = insuranceCompanyUID ?? 0;
            if (agreementModel != null)
            {
                payorAgreement = agreementModel;
                AssignModelToProperties(agreementModel);
            }
        }

        private void AssignModelToProperties(PayorAgreementModel model)
        {
            Name = model.Name;
            Code = model.Code;
            
            if (model.BLTYPUID != null)
                SelectBillType = BillType.FirstOrDefault(p => p.Key == model.BLTYPUID);
            if (model.PBTYPUID != null)
                SelectPayorBillType = PayorBillType.FirstOrDefault(p => p.Key == model.PBTYPUID);
            if (model.CRDTRMUID != null)
                SelectCreditTerm = CreditTerm.FirstOrDefault(p => p.Key == model.CRDTRMUID);
            if (model.PrimaryPBLCTUID != null)
                SelectPrimaryTariff = PrimaryTariff.FirstOrDefault(p => p.Key == model.PrimaryPBLCTUID);
            if (model.SecondaryPBLCTUID != null)
                SelectSecondaryTariff = SecondaryTariff.FirstOrDefault(p => p.Key == model.SecondaryPBLCTUID);
            if (model.TertiaryPBLCTUID != null)
                SelectTertiaryTariff = TertiaryTariff.FirstOrDefault(p => p.Key == model.TertiaryPBLCTUID);
            //if (model.AGTYPUID != null)
            //    SelectPolicyMaster = PolicyMaster.FirstOrDefault(p => p.PolicyMasterUID == model.AGTYPUID);
            ClaimPercentage = model.ClaimPercentage.ToString();
            CoverPerDay = model.OPDCoverPerDay.ToString();

            ActiveFrom = model.ActiveFrom;
            ActiveTo = model.ActiveTo;
        }


        private void AssignProprotiesToModel()
        {
            if (payorAgreement == null)
            {
                payorAgreement = new PayorAgreementModel();
            }

            payorAgreement.InsuranceCompanyUID = InsuranceCompanyID;
            payorAgreement.Name = Name;
            payorAgreement.Code = Code;
            //payorAgreement.Description = 
            payorAgreement.BLTYPUID = SelectBillType != null ? SelectBillType.Key : (int?)null;
            payorAgreement.CRDTRMUID = SelectCreditTerm != null ? SelectCreditTerm.Key : (int?)null;
            payorAgreement.PBTYPUID = SelectPayorBillType != null ? SelectPayorBillType.Key : (int?)null;

            payorAgreement.ClaimPercentage = !String.IsNullOrEmpty(ClaimPercentage) ? double.Parse(ClaimPercentage) : (double?)null;
            payorAgreement.OPDCoverPerDay = !String.IsNullOrEmpty(CoverPerDay) ? double.Parse(CoverPerDay) : (double?)null;

            payorAgreement.PrimaryPBLCTUID = SelectPrimaryTariff != null ? SelectPrimaryTariff.Key : (int?)null;
            payorAgreement.SecondaryPBLCTUID = SelectSecondaryTariff != null ? SelectSecondaryTariff.Key : (int?)null;
            payorAgreement.TertiaryPBLCTUID = SelectTertiaryTariff != null ? SelectTertiaryTariff.Key : (int?)null;
            payorAgreement.AGTYPUID = SelectPolicyMaster != null ? SelectPolicyMaster.AGTYPUID : (int?)null;
        }

        
        private void Cancel()
        {
            CloseViewDialog(ActionDialog.Cancel);
        }
        #endregion
    }
}
