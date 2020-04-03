using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediTech.ViewModels
{
    public class ModifyVisitViewModel : MediTechViewModelBase
    {
        #region Properties
        private PatientVisitModel _SelectPatientVisit;

        public PatientVisitModel SelectPatientVisit
        {
            get { return _SelectPatientVisit; }
            set { Set(ref _SelectPatientVisit, value); }
        }

        public List<PayorDetailModel> PayorDetailSource { get; set; }
        private PayorDetailModel _SelectedPayorDetail;

        public PayorDetailModel SelectedPayorDetail
        {
            get { return _SelectedPayorDetail; }
            set
            {
                Set(ref _SelectedPayorDetail, value);
                if (_SelectedPayorDetail != null)
                {
                    PayorAgreementSource = DataService.MasterData.GetAgreementByPayorDetailUID(SelectedPayorDetail.PayorDetailUID);
                }
            }
        }

        private List<PayorAgreementModel> _PayorAgreementSource;

        public List<PayorAgreementModel> PayorAgreementSource
        {
            get { return _PayorAgreementSource; }
            set { Set(ref _PayorAgreementSource, value); }
        }

        private PayorAgreementModel _SelectedPayorAgreement;

        public PayorAgreementModel SelectedPayorAgreement
        {
            get { return _SelectedPayorAgreement; }
            set { Set(ref _SelectedPayorAgreement, value); }
        }

        public List<LookupReferenceValueModel> PrioritySource { get; set; }
        private LookupReferenceValueModel _SelectedPriority;

        public LookupReferenceValueModel SelectedPriority
        {
            get { return _SelectedPriority; }
            set { Set(ref _SelectedPriority, value); }
        }

        private DateTime? _StartDate;

        public DateTime? StartDate
        {
            get { return _StartDate; }
            set { Set(ref _StartDate, value); }
        }

        private DateTime? _StartTime;
        public DateTime? StartTime
        {
            get { return _StartTime; }
            set { Set(ref _StartTime, value); }
        }

        private List<HealthOrganisationModel> _Organisations;

        public List<HealthOrganisationModel> Organisations
        {
            get { return _Organisations; }
            set { Set(ref _Organisations, value); }
        }

        private HealthOrganisationModel _SelectOrganisation;

        public HealthOrganisationModel SelectOrganisation
        {
            get { return _SelectOrganisation; }
            set
            {
                Set(ref _SelectOrganisation, value);
            }
        }

        public List<LookupReferenceValueModel> VisitTypeSource { get; set; }
        private LookupReferenceValueModel _SelectedVisitType;
        public LookupReferenceValueModel SelectedVisitType
        {
            get { return _SelectedVisitType; }
            set { Set(ref _SelectedVisitType, value); }
        }

        public List<CareproviderModel> CareproviderSource { get; set; }
        private CareproviderModel _SelectedCareprovider;

        public CareproviderModel SelectedCareprovider
        {
            get { return _SelectedCareprovider; }
            set { Set(ref _SelectedCareprovider, value); }
        }

        private string _CommentDoctor;

        public string CommentDoctor
        {
            get { return _CommentDoctor; }
            set { Set(ref _CommentDoctor, value); }
        }

        private bool _IsEnableFinancial;

        public bool IsEnableFinancial
        {
            get { return _IsEnableFinancial; }
            set { Set(ref _IsEnableFinancial, value); }
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

        private RelayCommand _CancelVisitCommand;

        public RelayCommand CancelVisitCommand
        {
            get { return _CancelVisitCommand ?? (_CancelVisitCommand = new RelayCommand(CancelVisit)); }
        }

        #endregion

        #region Method

        public ModifyVisitViewModel()
        {
            List<LookupReferenceValueModel> dataLookupSource = DataService.Technical.GetReferenceValueList("VISTY,RQPRT");
            VisitTypeSource = dataLookupSource.Where(p => p.DomainCode == "VISTY").OrderBy(p => p.DisplayOrder).ToList();
            PrioritySource = dataLookupSource.Where(P => P.DomainCode == "RQPRT").OrderBy(p => p.DisplayOrder).ToList();
            Organisations = GetHealthOrganisationRoleMedical();
            PayorDetailSource = DataService.MasterData.GetPayorDetail();
        }
        private void Save()
        {
            try
            {
                if (SelectPatientVisit != null)
                {
                    if (ValidateVisitData())
                    {
                        return;
                    }

                    PatientVisitModel visitInfo = new PatientVisitModel();
                    visitInfo.PatientUID = SelectPatientVisit.PatientUID;
                    visitInfo.PatientVisitUID = SelectPatientVisit.PatientVisitUID;
                    visitInfo.StartDttm = DateTime.Parse(StartDate.Value.ToString("dd/MM/yyyy") + " " + StartTime.Value.ToString("HH:mm"));
                    visitInfo.VISTYUID = SelectedVisitType.Key;
                    visitInfo.PRITYUID = SelectedPriority.Key;
                    visitInfo.PayorDetailUID = SelectedPayorDetail.PayorDetailUID;
                    visitInfo.PayorAgreementUID = SelectedPayorAgreement.PayorAgreementUID;
                    visitInfo.Comments = CommentDoctor;
                    visitInfo.OwnerOrganisationUID = SelectOrganisation.HealthOrganisationUID;
                    if (SelectedCareprovider != null)
                        visitInfo.CareProviderUID = SelectedCareprovider.CareproviderUID;

                    DataService.PatientIdentity.ModifyPatientVisit(visitInfo, AppUtil.Current.UserID);
                    CloseViewDialog(ActionDialog.Save);
                }

            }
            catch (Exception ex)
            {

                ErrorDialog(ex.Message);
            }

        }

        private void CancelVisit()
        {
            try
            {
                if (SelectPatientVisit != null)
                {
                    DialogResult diagResult = QuestionDialog("คุณต้องการยกเลิกการละเบียนของผู้ป่วยคนนี้ใช้หรือไม่ ?");
                    if (diagResult == DialogResult.Yes)
                    {
                        DataService.PatientIdentity.CancelVisit(SelectPatientVisit.PatientVisitUID, AppUtil.Current.UserID);
                    }
                    CloseViewDialog(ActionDialog.Save);
                }
            }
            catch (Exception ex)
            {

                ErrorDialog(ex.Message);
            }
        }

        private void Cancel()
        {
            CloseViewDialog(ActionDialog.Cancel);
        }
        public void AssingPatientVisit(PatientVisitModel visitModel)
        {
            SelectPatientVisit = visitModel;
            SelectOrganisation = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == SelectPatientVisit.OwnerOrganisationUID);
            SelectedVisitType = VisitTypeSource.FirstOrDefault(p => p.Key == SelectPatientVisit.VISTYUID);
            StartDate = SelectPatientVisit.StartDttm != null ? SelectPatientVisit.StartDttm.Value : (DateTime?)null;
            StartTime = SelectPatientVisit.StartDttm != null ? SelectPatientVisit.StartDttm.Value : (DateTime?)null;
            SelectedPayorDetail = PayorDetailSource.FirstOrDefault(p => p.PayorDetailUID == SelectPatientVisit.PayorDetailUID);
            if (SelectedPayorDetail != null)
            {
                PayorAgreementSource = DataService.MasterData.GetAgreementByPayorDetailUID(SelectedPayorDetail.PayorDetailUID);
                SelectedPayorAgreement = PayorAgreementSource.FirstOrDefault(p => p.PayorAgreementUID == SelectPatientVisit.PayorAgreementUID);
            }
            SelectedPriority = PrioritySource.FirstOrDefault(p => p.Key == SelectPatientVisit.PRITYUID);
            CareproviderSource = DataService.UserManage.GetCareproviderDoctor();
            SelectedCareprovider = CareproviderSource.FirstOrDefault(p => p.CareproviderUID == SelectPatientVisit.CareProviderUID);
            CommentDoctor = SelectPatientVisit.Comments;

            if (SelectPatientVisit.VisitStatus == "Financial Discharge")
            {
                IsEnableFinancial = false;
            }
            else
            {
                IsEnableFinancial = true;
            }

        }

        public bool ValidateVisitData()
        {
            if (SelectOrganisation == null)
            {
                WarningDialog("กรุณาเลือก สถานที่");
                return true;
            }
            if (SelectedVisitType == null)
            {
                WarningDialog("กรุณาเลือก ประเภท Visit");
                return true;
            }

            if (SelectedPayorDetail == null)
            {
                WarningDialog("กรุณาเลือก Payor");
                return true;
            }

            if (SelectedPayorAgreement == null)
            {
                WarningDialog("กรุณาเลือก Agreemnet");
                return true;
            }

            if (SelectedPriority == null)
            {
                WarningDialog("กรุณาเลือก ความสำคัญ");
                return true;
            }

            return false;
        }

        #endregion
    }
}
