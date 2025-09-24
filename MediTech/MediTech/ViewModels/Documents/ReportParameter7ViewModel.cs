using DevExpress.XtraReports.UI;
using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Model.Report;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MediTech.ViewModels
{
    public class ReportParameter7ViewModel : MediTechViewModelBase
    {
        #region Properites

        public ReportsModel ReportTemplate { get; set; }

        private List<HealthOrganisationModel> _Organisations;

        public List<HealthOrganisationModel> Organisations
        {
            get { return _Organisations; }
            set { Set(ref _Organisations, value); }
        }


        private List<object> _SelectOrganisations;

        public List<object> SelectOrganisations
        {
            get { return _SelectOrganisations ?? (_SelectOrganisations = new List<object>()); }
            set { Set(ref _SelectOrganisations, value); }
        }

        public List<LookupReferenceValueModel> DoctorFeeTypeSource { get; set; }
        private LookupReferenceValueModel _SelectedDoctorFeeType;

        public LookupReferenceValueModel SelectedDoctorFeeType
        {
            get { return _SelectedDoctorFeeType; }
            set { Set(ref _SelectedDoctorFeeType, value); }
        }


        private List<CareproviderModel> _Doctors;

        public List<CareproviderModel> Doctors
        {
            get { return _Doctors; }
            set { _Doctors = value; }
        }

        private CareproviderModel _SelectDoctor;

        public CareproviderModel SelectDoctor
        {
            get { return _SelectDoctor; }
            set { Set(ref _SelectDoctor, value); }
        }

        private DateTime? _DateFrom;

        public DateTime? DateFrom
        {
            get { return _DateFrom; }
            set { Set(ref _DateFrom, value); }
        }


        private DateTime? _DateTo;

        public DateTime? DateTo
        {
            get { return _DateTo; }
            set { Set(ref _DateTo, value); }
        }

        private bool _IsEnabledDoctor;

        public bool IsEnabledDoctor
        {
            get { return _IsEnabledDoctor; }
            set { Set(ref _IsEnabledDoctor, value); }
        }

        #endregion

        #region Command

        private RelayCommand _PrintCommand;
        public RelayCommand PrintCommand
        {
            get
            {
                return _PrintCommand
                    ?? (_PrintCommand = new RelayCommand(Print));
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

        public ReportParameter7ViewModel()
        {
            DateFrom = DateTime.Now;
            DateTo = DateTime.Now;
            Organisations = GetHealthOrganisationRole();
            var SelectOrganisation = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == AppUtil.Current.OwnerOrganisationUID);

            foreach ( var organisation in Organisations)
            {
                SelectOrganisations.Add(organisation.HealthOrganisationUID);
            }
        
            
            DoctorFeeTypeSource = new List<LookupReferenceValueModel> {
                new LookupReferenceValueModel(){Display="All",Key=0 }
                ,new LookupReferenceValueModel(){Display="ค่าอ่านฟิล์ม",Key=1 }
                , new LookupReferenceValueModel(){ Display="ค่าอื่นๆ",Key=2} };
            SelectedDoctorFeeType = DoctorFeeTypeSource.FirstOrDefault();

            Doctors = DataService.UserManage.GetCareproviderDoctor();
            if ((AppUtil.Current.IsDoctor ?? false))
            {
                SelectDoctor = Doctors.FirstOrDefault(p => p.CareproviderUID == AppUtil.Current.UserID);
                IsEnabledDoctor = false;
            }
            else
            {
                IsEnabledDoctor = true;
            }

            if (AppUtil.Current.IsAdmin ?? false)
            {
                IsEnabledDoctor = true;
            }
        }

        void Print()
        {
            var myReport = Activator.CreateInstance(Type.GetType(ReportTemplate.NamespaceName));
            XtraReport report = (XtraReport)myReport;
            string healthOrganisationList = "";
            if (SelectOrganisations != null)
            {
                foreach (object item in SelectOrganisations)
                {
                    if (item.ToString() != "0")
                    {
                        if (healthOrganisationList == "")
                        {
                            healthOrganisationList = item.ToString();
                        }
                        else
                        {
                            healthOrganisationList += "," + item.ToString();
                        }
                    }

                }
            }

            foreach (var item in report.Parameters)
            {
                switch (item.Name)
                {
                    case "DateFrom":
                        item.Value = DateFrom;
                        break;
                    case "DateTo":
                        item.Value = DateTo;
                        break;
                    case "CareproviderUID":
                        item.Value = (SelectDoctor != null) ? SelectDoctor.CareproviderUID : (object)null;
                        break;
                    case "DoctorFeeType":
                        item.Value = (SelectedDoctorFeeType != null) ? SelectedDoctorFeeType.Display : (object)null;
                        break;
                    case "OrganisationList":
                        item.Value = healthOrganisationList;
                        break;
                }
            }

            ReportPrintTool printTool = new ReportPrintTool(report);
            report.ShowPrintMarginsWarning = false;
            printTool.ShowPreviewDialog();

        }

        void Cancel()
        {
            ChangeViewPermission(this.BackwardView);
        }

        #endregion

    }
}
