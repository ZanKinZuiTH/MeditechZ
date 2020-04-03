using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using MediTech.DataService;
using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ShareLibrary;
using System.Windows.Media.Imaging;
using System.IO;
using MediTech.Helpers;
using MediTech.Views;
using System.Drawing;

namespace MediTech.ViewModels
{
    public class CreateVisitViewModel : MediTechViewModelBase
    {
        #region Properties

        

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
                if (SelectOrganisation != null)
                {
                    if (SelectOrganisation.HealthOrganisationUID == 5)
                    {
                        SelectedVisitType = VisitTypeSource.FirstOrDefault(p => p.ValueCode == "REFTU");
                    }
                    else
                    {
                        SelectedVisitType = VisitTypeSource.FirstOrDefault(p => p.ValueCode == "DCPAT");
                    }
                }
            }
        }

        public List<LookupReferenceValueModel> VisitTypeSource { get; set; }
        private LookupReferenceValueModel _SelectedVisitType;

        public LookupReferenceValueModel SelectedVisitType
        {
            get { return _SelectedVisitType; }
            set { Set(ref _SelectedVisitType, value); }
        }

        private DateTime _StartDate;

        public DateTime StartDate
        {
            get { return _StartDate; }
            set { Set(ref _StartDate, value); }
        }

        private DateTime _StartTime;
        public DateTime StartTime
        {
            get { return _StartTime; }
            set { Set(ref _StartTime, value); }
        }

        public List<LookupReferenceValueModel> PrioritySource { get; set; }
        private LookupReferenceValueModel _SelectedPriority;

        public LookupReferenceValueModel SelectedPriority
        {
            get { return _SelectedPriority; }
            set { _SelectedPriority = value; }
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
                    PayorAgreementSource = DataService.MasterData.GetAgreementByPayorDetailUID(_SelectedPayorDetail.PayorDetailUID);
                    if (PayorAgreementSource != null)
                    {
                        SelectedPayorAgreement = PayorAgreementSource.FirstOrDefault();
                    }
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

        private Visibility _CancelVisitVisibility;

        public Visibility CancelVisitVisibility
        {
            get { return _CancelVisitVisibility; }
            set { Set(ref _CancelVisitVisibility, value); }
        }

        private Visibility _CancelVisibility;

        public Visibility CancelVisibility
        {
            get { return _CancelVisibility; }
            set { Set(ref _CancelVisibility, value); }
        }

        private bool _IsUpdateVisit = false;

        public bool IsUpdateVisit
        {
            get { return _IsUpdateVisit; }
            set { Set(ref _IsUpdateVisit, value); }
        }

        #endregion

        #region Command

        private RelayCommand _SaveCommand;

        public RelayCommand SaveCommand
        {
            get { return _SaveCommand ?? (_SaveCommand = new RelayCommand(SavePatientVisit)); }
        }

        #endregion

        #region Method

        public override void OnLoaded()
        {
            if (IsUpdateVisit)
            {
                CancelVisibility = Visibility.Visible;
                CancelVisibility = Visibility.Visible;
            }
            else
            {
                CancelVisibility = Visibility.Collapsed;
                CancelVisibility = Visibility.Collapsed;
            }
        }

        public CreateVisitViewModel()
        {
            DateTime now = DateTime.Now;

            List<LookupReferenceValueModel> dataLookupSource = DataService.Technical.GetReferenceValueList("VISTY,RQPRT");
            Organisations = GetHealthOrganisationRoleMedical();
            VisitTypeSource = dataLookupSource.Where(p => p.DomainCode == "VISTY").ToList();
            PrioritySource = dataLookupSource.Where(P => P.DomainCode == "RQPRT").ToList();
            PayorDetailSource = DataService.MasterData.GetPayorDetail();
            CareproviderSource = DataService.UserManage.GetCareproviderDoctor();
            SelectedPriority = PrioritySource.FirstOrDefault(p => p.Key == 440);

            StartDate = now.Date;
            StartTime = now;
        }

        


        void SavePatientVisit()
        {
            if (ValidateVisitData())
            {
                return;
            }
            var parent = ((System.Windows.Controls.UserControl)this.View).Parent;
            if (parent == null)
            {

            }
            else if (parent is System.Windows.Window)
            {
                CloseViewDialog(ActionDialog.Save);
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
