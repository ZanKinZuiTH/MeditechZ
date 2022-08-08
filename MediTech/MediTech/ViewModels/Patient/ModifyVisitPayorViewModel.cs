using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class ModifyVisitPayorViewModel : MediTechViewModelBase
    {
        #region Properties

        private List<LookupReferenceValueModel> _PayorTypes;

        public List<LookupReferenceValueModel> PayorTypes
        {
            get { return _PayorTypes; }
            set
            {
                Set(ref _PayorTypes, value);
            }
        }


        private LookupReferenceValueModel _SelectedPayorType;

        public LookupReferenceValueModel SelectedPayorType
        {
            get { return _SelectedPayorType; }
            set
            {
                Set(ref _SelectedPayorType, value);
            }
        }

        private List<InsuranceCompanyModel> _InsuranceCompanys;

        public List<InsuranceCompanyModel> InsuranceCompanys
        {
            get { return _InsuranceCompanys; }
            set { Set(ref _InsuranceCompanys, value); }
        }

        private InsuranceCompanyModel _SelectInsuranceCompany;

        public InsuranceCompanyModel SelectInsuranceCompany
        {
            get { return _SelectInsuranceCompany; }
            set
            {
                Set(ref _SelectInsuranceCompany, value);
                if (_SelectInsuranceCompany != null)
                {
                    InsurancePlans = DataService.Billing.GetInsurancePlans(_SelectInsuranceCompany.InsuranceCompanyUID);
                }
            }
        }

        private List<InsurancePlanModel> _InsurancePlans;

        public List<InsurancePlanModel> InsurancePlans
        {
            get { return _InsurancePlans; }
            set { Set(ref _InsurancePlans, value); }
        }

        private InsurancePlanModel _SelectInsurancePlan;

        public InsurancePlanModel SelectInsurancePlan
        {
            get { return _SelectInsurancePlan; }
            set
            {
                Set(ref _SelectInsurancePlan, value);
                if (_SelectInsurancePlan != null)
                {
                    OPDCoverPerDay = _SelectInsurancePlan.OPDCoverPerDay;
                    FixedCopayAmount = _SelectInsurancePlan.FixedCopayAmount;
                    ClaimPercentage = _SelectInsurancePlan.ClaimPercentage;
                }
            }
        }

        private string _Comments;

        public string Comments
        {
            get { return _Comments; }
            set { Set(ref _Comments, value); }
        }

        private double? _OPDCoverPerDay;

        public double? OPDCoverPerDay
        {
            get { return _OPDCoverPerDay; }
            set { Set(ref _OPDCoverPerDay, value); }
        }

        private double? _ClaimPercentage;

        public double? ClaimPercentage
        {
            get { return _ClaimPercentage; }
            set { Set(ref _ClaimPercentage, value); }
        }

        private double? _FixedCopayAmount;

        public double? FixedCopayAmount
        {
            get { return _FixedCopayAmount; }
            set { Set(ref _FixedCopayAmount, value); }
        }


        private DateTime? _ActiveForm;

        public DateTime? ActiveFrom
        {
            get { return _ActiveForm; }
            set { Set(ref _ActiveForm, value); }
        }

        private DateTime? _ActiveTo;

        public DateTime? ActiveTo
        {
            get { return _ActiveTo; }
            set { Set(ref _ActiveTo, value); }
        }

        private List<PatientVisitPayorModel> _deletedVisitPayorList;

        private ObservableCollection<PatientVisitPayorModel> _PatientVisitPayorList;

        public ObservableCollection<PatientVisitPayorModel> PatientVisitPayorList
        {
            get { return _PatientVisitPayorList; }
            set { Set(ref _PatientVisitPayorList, value); }
        }

        private PatientVisitPayorModel _SelectedPatientVisitPayor;

        public PatientVisitPayorModel SelectedPatientVisitPayor
        {
            get { return _SelectedPatientVisitPayor; }
            set
            {
                Set(ref _SelectedPatientVisitPayor, value);
                if (_SelectedPatientVisitPayor != null)
                {
                    SelectedPayorType = PayorTypes.FirstOrDefault(p => p.Key == _SelectedPatientVisitPayor.PAYRTPUID);
                    SelectInsuranceCompany = InsuranceCompanys.FirstOrDefault(p => p.InsuranceCompanyUID == _SelectedPatientVisitPayor.InsuranceCompanyUID);
                    SelectInsurancePlan = InsurancePlans.FirstOrDefault(p => p.PayorAgreementUID == _SelectedPatientVisitPayor.PayorAgreementUID);
                    OPDCoverPerDay = _SelectedPatientVisitPayor.EligibleAmount;
                    ClaimPercentage = _SelectedPatientVisitPayor.ClaimPercentage;
                    FixedCopayAmount = _SelectedPatientVisitPayor.FixedCopayAmount;
                    ActiveFrom = _SelectedPatientVisitPayor.ActiveFrom;
                    ActiveTo = _SelectedPatientVisitPayor.ActiveTo;
                }

            }
        }

        private List<PatientInsuranceDetailModel> _PatientInsuranceDetails;

        public List<PatientInsuranceDetailModel> PatientInsuranceDetails
        {
            get { return _PatientInsuranceDetails; }
            set { Set(ref _PatientInsuranceDetails, value); }
        }

        private PatientInsuranceDetailModel _SelectPatientInsuranceDetail;

        public PatientInsuranceDetailModel SelectPatientInsuranceDetail
        {
            get { return _SelectPatientInsuranceDetail; }
            set
            {
                _SelectPatientInsuranceDetail = value;
            }
        }

        private PatientVisitModel _PatientVisit;

        public PatientVisitModel PatientVisit
        {
            get { return _PatientVisit; }
            set { _PatientVisit = value; }
        }


        #endregion

        #region Command

        private RelayCommand _AddVisitPayorCommand;

        public RelayCommand AddVisitPayorCommand
        {
            get { return _AddVisitPayorCommand ?? (_AddVisitPayorCommand = new RelayCommand(AddVisitPayor)); }
        }

        private RelayCommand _UpdateVisitPayorCommand;

        public RelayCommand UpdateVisitPayorCommand
        {
            get { return _UpdateVisitPayorCommand ?? (_UpdateVisitPayorCommand = new RelayCommand(UpdateVisitPayor)); }
        }

        private RelayCommand _DeleteVisitPayorCommand;

        public RelayCommand DeleteVisitPayorCommand
        {
            get { return _DeleteVisitPayorCommand ?? (_DeleteVisitPayorCommand = new RelayCommand(DeleteVisitPayor)); }
        }

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

        private RelayCommand<DevExpress.Xpf.Grid.RowDoubleClickEventArgs> _RowDoubleClickCommand;

        /// <summary>
        /// Gets the RowDoubleClickCommand.
        /// </summary>
        public RelayCommand<DevExpress.Xpf.Grid.RowDoubleClickEventArgs> RowDoubleClickCommand
        {
            get
            {
                return _RowDoubleClickCommand
                    ?? (_RowDoubleClickCommand = new RelayCommand<DevExpress.Xpf.Grid.RowDoubleClickEventArgs>(RowDoubleClick));
            }
        }

        #endregion

        #region Method

        public override void OnLoaded()
        {
            base.OnLoaded();
            LoadPatientVisitPayors();
            PayorTypes = DataService.Technical.GetReferenceValueMany("PAYRTP");
            InsuranceCompanys = DataService.Billing.GetInsuranceCompanies();
            if (PatientVisitPayorList != null && PatientVisitPayorList.Count() > 0 && PayorTypes != null)
                SelectedPayorType = (from p in PayorTypes where (!(from q in PatientVisitPayorList select q.PAYRTPUID).Contains(p.Key)) select p).FirstOrDefault();
            else
                SelectedPayorType = PayorTypes.FirstOrDefault(p => p.ValueCode == "PRIMARY");

            ActiveFrom = PatientVisit.StartDttm;
        }

        void LoadPatientVisitPayors()
        {
            var patientInsuranceDetails = DataService.PatientIdentity.GetPatientInsuranceDetail(PatientVisit.PatientUID);
            var patientVisitPayors = DataService.PatientIdentity.GetPatientVisitPayorByVisitUID(PatientVisit.PatientVisitUID);
            PatientInsuranceDetails = patientInsuranceDetails;
            PatientVisitPayorList = new ObservableCollection<PatientVisitPayorModel>(patientVisitPayors);
            _deletedVisitPayorList = new List<PatientVisitPayorModel>();
        }

        public void AssingPatientVisit(PatientVisitModel visitModel)
        {
            PatientVisit = visitModel;
        }

        void AddVisitPayor()
        {
            if (PatientVisitPayorList == null)
                PatientVisitPayorList = new ObservableCollection<PatientVisitPayorModel>();

            if (SelectInsuranceCompany == null || SelectInsuranceCompany.InsuranceCompanyUID == 0)
            {
                WarningDialog("กรุณาเลือก Payor");
                return;
            }
            if (SelectInsurancePlan == null || SelectInsurancePlan.PayorAgreementUID == 0)
            {
                WarningDialog("กรุณาเลือก Agreement");
                return;
            }

            if (ClaimPercentage != null && ClaimPercentage > 100)
            {
                WarningDialog("ClaimPercentage ไม่ถูกต้อง");
                return;
            }
            if (PatientVisitPayorList != null && PatientVisitPayorList.Count() > 0)
            {
                if (PatientVisitPayorList.Where(i => i.PAYRTPUID == SelectedPayorType.Key) != null && PatientVisitPayorList.Where(i => i.PAYRTPUID == SelectedPayorType.Key).Count() > 0)
                {
                    WarningDialog("PayorType ซ้ำ กรุณาตรวจสอบ");
                    return;
                }
            }

            if (CheckDataPresentInList(new PatientVisitPayorModel { PayorAgreementUID = SelectInsurancePlan.PayorAgreementUID, PayorDetailUID = SelectInsurancePlan.PayorDetailUID }))
            {
                WarningDialog("Payor ซ้ำ กรุณาตรวจสอบ");
                return;
            }

            PatientVisitPayorModel newPatientVisitPayor = new PatientVisitPayorModel();
            newPatientVisitPayor.PatientUID = PatientVisit.PatientUID;
            newPatientVisitPayor.PatientVisitUID = PatientVisit.PatientVisitUID;
            newPatientVisitPayor.PAYRTPUID = SelectedPayorType.Key;
            newPatientVisitPayor.PayorType = SelectedPayorType.Display;
            newPatientVisitPayor.InsuranceCompanyUID = SelectInsuranceCompany.InsuranceCompanyUID;
            newPatientVisitPayor.InsuranceName = SelectInsuranceCompany.CompanyName;
            newPatientVisitPayor.PayorAgreementUID = SelectInsurancePlan.PayorAgreementUID;
            newPatientVisitPayor.AgreementName = SelectInsurancePlan.PayorAgreementName;
            newPatientVisitPayor.PayorDetailUID = SelectInsurancePlan.PayorDetailUID;
            newPatientVisitPayor.PayorName = GetPayorDetailName(SelectInsurancePlan.PayorDetailUID);
            var agreement = GetAgreement(newPatientVisitPayor.PayorAgreementUID);
            if (agreement != null)
            {
                newPatientVisitPayor.PolicyMasterUID = agreement.PolicyMasterUID;
                newPatientVisitPayor.PolicyName = agreement.PolicyName;
            }
            newPatientVisitPayor.Comment = Comments;
            newPatientVisitPayor.ClaimPercentage = ClaimPercentage;
            newPatientVisitPayor.FixedCopayAmount = FixedCopayAmount;
            newPatientVisitPayor.EligibleAmount = OPDCoverPerDay;
            newPatientVisitPayor.ActiveFrom = ActiveFrom;
            newPatientVisitPayor.ActiveTo = ActiveTo;
            newPatientVisitPayor.StatusFlag = "A";
            newPatientVisitPayor.CUser = AppUtil.Current.UserID;
            newPatientVisitPayor.MUser = AppUtil.Current.UserID;
            newPatientVisitPayor.OwnerOrganisationUID = AppUtil.Current.OwnerOrganisationUID;
            newPatientVisitPayor.CreatedBy = AppUtil.Current.UserName;
            PatientVisitPayorList.Add(newPatientVisitPayor);
            ClearControl();
        }
        void UpdateVisitPayor()
        {
            if (SelectedPatientVisitPayor != null)
            {
                if (SelectInsuranceCompany == null || SelectInsuranceCompany.InsuranceCompanyUID == 0)
                {
                    WarningDialog("กรุณาเลือก Payor");
                    return;
                }
                if (SelectInsurancePlan == null || SelectInsurancePlan.PayorAgreementUID == 0)
                {
                    WarningDialog("กรุณาเลือก Agreement");
                    return;
                }

                if (ClaimPercentage != null && ClaimPercentage > 100)
                {
                    WarningDialog("ClaimPercentage ไม่ถูกต้อง");
                    return;
                }
                if (PatientVisitPayorList != null && PatientVisitPayorList.Count() > 0)
                {
                    if (PatientVisitPayorList.Where(i => i.PAYRTPUID == SelectedPayorType.Key && !i.Equals(SelectedPatientVisitPayor)) != null
                        && PatientVisitPayorList.Where(i => i.PAYRTPUID == SelectedPayorType.Key && !i.Equals(SelectedPatientVisitPayor)).Count() > 0)
                    {
                        WarningDialog("Rank ซ้ำ กรุณาตรวจสอบ");
                        return;
                    }
                }

                if (PatientVisitPayorList != null && PatientVisitPayorList.Count > 0)
                {
                    foreach (PatientVisitPayorModel payor in PatientVisitPayorList)
                    {
                        if (payor.PayorDetailUID == SelectInsurancePlan.PayorDetailUID
                            && payor.PayorAgreementUID == SelectInsurancePlan.PayorAgreementUID
                            && !payor.Equals(SelectedPatientVisitPayor)
                            )
                        {
                            WarningDialog("Payor ซ้ำ กรุณาตรวจสอบ");
                            return;
                        }
                    }
                }

                SelectedPatientVisitPayor.PAYRTPUID = SelectedPayorType.Key;
                SelectedPatientVisitPayor.PayorType = SelectedPayorType.Display;
                SelectedPatientVisitPayor.InsuranceCompanyUID = SelectInsuranceCompany.InsuranceCompanyUID;
                SelectedPatientVisitPayor.InsuranceName = SelectInsuranceCompany.CompanyName;
                SelectedPatientVisitPayor.PayorAgreementUID = SelectInsurancePlan.PayorAgreementUID;
                SelectedPatientVisitPayor.AgreementName = SelectInsurancePlan.PayorAgreementName;
                SelectedPatientVisitPayor.PayorDetailUID = SelectInsurancePlan.PayorDetailUID;
                SelectedPatientVisitPayor.PayorName = GetPayorDetailName(SelectInsurancePlan.PayorDetailUID);
                var agreement = GetAgreement(SelectedPatientVisitPayor.PayorAgreementUID);
                if (agreement != null)
                {
                    SelectedPatientVisitPayor.PolicyMasterUID = agreement.PolicyMasterUID;
                    SelectedPatientVisitPayor.PolicyName = agreement.PolicyName;
                }
                SelectedPatientVisitPayor.Comment = Comments;
                SelectedPatientVisitPayor.ClaimPercentage = ClaimPercentage;
                SelectedPatientVisitPayor.FixedCopayAmount = FixedCopayAmount;
                SelectedPatientVisitPayor.EligibleAmount = OPDCoverPerDay;
                SelectedPatientVisitPayor.ActiveFrom = ActiveFrom;
                SelectedPatientVisitPayor.ActiveTo = ActiveTo;
                SelectedPatientVisitPayor.StatusFlag = "A";
                SelectedPatientVisitPayor.CUser = AppUtil.Current.UserID;
                SelectedPatientVisitPayor.MUser = AppUtil.Current.UserID;
                SelectedPatientVisitPayor.OwnerOrganisationUID = AppUtil.Current.OwnerOrganisationUID;
                SelectedPatientVisitPayor.CreatedBy = AppUtil.Current.UserName;
                ClearControl();
                SelectedPatientVisitPayor = null;
            }
        }

        void DeleteVisitPayor()
        {
            if (SelectedPatientVisitPayor != null)
            {
                SelectedPatientVisitPayor.StatusFlag = "D";
                _deletedVisitPayorList.Add(SelectedPatientVisitPayor);
                PatientVisitPayorList.Remove(SelectedPatientVisitPayor);
                ClearControl();
            }
        }

        void ClearControl()
        {
            if (PatientVisitPayorList != null && PatientVisitPayorList.Count() > 0 && PayorTypes != null)
            {
                SelectedPayorType = (from p in PayorTypes where (!(from q in PatientVisitPayorList select q.PAYRTPUID).Contains(p.Key)) select p).FirstOrDefault();
                PatientVisitPayorList = new ObservableCollection<PatientVisitPayorModel>( PatientVisitPayorList.OrderBy(p => Convert.ToInt32(p.PayorType)));
            }
            else
            {
                SelectedPayorType = PayorTypes.FirstOrDefault(p => p.ValueCode == "PRIMARY");
            }

            SelectInsuranceCompany = null;
            SelectInsurancePlan = null;
            Comments = null;
            OPDCoverPerDay = null;
            FixedCopayAmount = null;
            ClaimPercentage = null;
            ActiveFrom = PatientVisit.StartDttm;
            ActiveTo = null;
            InsurancePlans = new List<InsurancePlanModel>();

            (this.View as ModifyVisitPayor).grdVisitPayor.RefreshData();
        }

        private void RowDoubleClick(DevExpress.Xpf.Grid.RowDoubleClickEventArgs e)
        {
            try
            {
                if (SelectPatientInsuranceDetail != null)
                {
                    SelectInsuranceCompany = InsuranceCompanys.FirstOrDefault(p => p.InsuranceCompanyUID == SelectPatientInsuranceDetail.InsuranceCompanyUID);
                    SelectInsurancePlan = InsurancePlans.FirstOrDefault(p => p.PayorAgreementUID == SelectPatientInsuranceDetail.PayorAgreementUID);
                    OPDCoverPerDay = SelectPatientInsuranceDetail.EligibleAmount;
                    ClaimPercentage = SelectPatientInsuranceDetail.ClaimPercentage;
                    FixedCopayAmount = SelectPatientInsuranceDetail.FixedCopayAmount;
                    ActiveFrom = SelectPatientInsuranceDetail.StartDttm;
                    ActiveTo = SelectPatientInsuranceDetail.EndDttm;
                }
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }

        }

        string GetInsuranceComapnyName(int insuranceComapnyUID)
        {
            string name = string.Empty;

            var insuranceCompany = DataService.Billing.GetInsuranceCompanyByUID(insuranceComapnyUID);
            if (insuranceCompany != null)
            {
                name = insuranceCompany.CompanyName;
            }

            return name;
        }

        private bool CheckDataPresentInList(PatientVisitPayorModel inputPayor)
        {
            bool ret = false;
            if (PatientVisitPayorList != null && PatientVisitPayorList.Count > 0)
            {
                foreach (PatientVisitPayorModel payor in PatientVisitPayorList)
                {
                    if (payor.PayorDetailUID == inputPayor.PayorDetailUID
                        && payor.PayorAgreementUID == inputPayor.PayorAgreementUID)
                    {
                        ret = true;
                        break;
                    }
                }
            }
            return ret;
        }

        string GetPayorDetailName(int payorDetailUID)
        {
            string name = string.Empty;

            var payorDetail = DataService.Billing.GetPayorDetailByUID(payorDetailUID);
            if (payorDetail != null)
            {
                name = payorDetail.PayorName;
            }

            return name;
        }

        PayorAgreementModel GetAgreement(int agreeementUID)
        {

            var payorAgreement = DataService.Billing.GetPayorAgreementByUID(agreeementUID);
            return payorAgreement;

        }


        private void Save()
        {
            try
            {
                if (PatientVisitPayorList == null || PatientVisitPayorList.Count() <= 0)
                {
                    WarningDialog("กรุณาใส่ข้อมูล Payor");
                }

                if (PatientVisitPayorList != null || PatientVisitPayorList.Count() >= 0)
                {
                    if (PatientVisitPayorList.Count(p => p.PayorType == "1") <= 0)
                    {
                        WarningDialog("กรุณาใส่ข้อมูล Payor Rank 1");
                        return;
                    }
                }

                var PateintVisitPayorDatas = PatientVisitPayorList.ToList();
                if (_deletedVisitPayorList != null)
                    PateintVisitPayorDatas.AddRange(_deletedVisitPayorList);
                DataService.PatientIdentity.ManagePatientVisitPayor(PateintVisitPayorDatas, AppUtil.Current.UserID);
                DataService.PatientIdentity.ManagePatientInsuranceDetail(PateintVisitPayorDatas, AppUtil.Current.UserID);
                CloseViewDialog(ActionDialog.Save);
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }

        }

        private void Cancel()
        {
            CloseViewDialog(ActionDialog.Cancel);
        }
        #endregion
    }
}
