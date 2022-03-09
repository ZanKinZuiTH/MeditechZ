using DevExpress.Xpf.Editors;
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
    public class ReportParameter8ViewModel :MediTechViewModelBase
    {
        #region Properites

        public ReportsModel ReportTemplate { get; set; }

        public List<LookupReferenceValueModel> VisitTypeSource { get; set; }
        private LookupReferenceValueModel _SelectedVisitType;

        public LookupReferenceValueModel SelectedVisitType
        {
            get { return _SelectedVisitType; }
            set { Set(ref _SelectedVisitType, value); }
        }

        private List<HealthOrganisationModel> _Organisations;

        public List<HealthOrganisationModel> Organisations
        {
            get { return _Organisations; }
            set { Set(ref _Organisations, value); }
        }




        private List<HealthOrganisationModel> _OrganisationsTo;

        public List<HealthOrganisationModel> OrganisationsTo
        {
            get { return _OrganisationsTo; }
            set { Set(ref _OrganisationsTo, value); }
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

        public List<HealthOrganisationModel> OrganisationsFrom { get; set; }
        private HealthOrganisationModel _SelectOrganisationFrom;

        public HealthOrganisationModel SelectOrganisationFrom
        {
            get { return _SelectOrganisationFrom; }
            set
            {
                Set(ref _SelectOrganisationFrom, value);
                if (_SelectOrganisationFrom != null)
                {
                    StoresFrom = DataService.Inventory.GetStoreByOrganisationUID(SelectOrganisationFrom.HealthOrganisationUID);
                    OrganisationsTo = HealthOrganisations.Where(p => p.HealthOrganisationUID != SelectOrganisationFrom.HealthOrganisationUID).ToList();
                }
                else
                {
                    StoresFrom = null;
                }
            }
        }

        private List<StoreModel> _StoresFrom;

        public List<StoreModel> StoresFrom
        {
            get { return _StoresFrom; }
            set { Set(ref _StoresFrom, value); }
        }

        private StoreModel _SelectStoreFrom;
        public StoreModel SelectStoreFrom
        {
            get { return _SelectStoreFrom; }
            set
            {
                Set(ref _SelectStoreFrom, value);
              
            }
        }




        #region Varible

        List<HealthOrganisationModel> HealthOrganisations;

        #endregion


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

        public ReportParameter8ViewModel()
        {
            DateFrom = DateTime.Now;
            DateTo = DateTime.Now;
            VisitTypeSource = DataService.Technical.GetReferenceValueMany("VISTY").ToList();
            HealthOrganisations = GetHealthOrganisationIsStock();
            OrganisationsFrom = GetHealthOrganisationIsRoleStock();
            OrganisationsTo = HealthOrganisations;
   

            if (OrganisationsFrom != null)
            {
                SelectOrganisationFrom = OrganisationsFrom.FirstOrDefault(p => p.HealthOrganisationUID == AppUtil.Current.OwnerOrganisationUID);
            }

        }

        void Print()
        {
            if (SelectStoreFrom == null)
            {
                WarningDialog("กรุณาระบุ Store");
                return;
            }
            var myReport = Activator.CreateInstance(Type.GetType(ReportTemplate.NamespaceName));
            XtraReport report = (XtraReport)myReport;

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
                    case "VISTYUID":
                        item.Value = (SelectedVisitType != null) ? SelectedVisitType.Key : (object)null;
                        break;
                    case "OrganisationList":
                        item.Value = SelectOrganisationFrom.HealthOrganisationUID;
                        break;
                    case "StoreFrom":
                        item.Value = SelectStoreFrom.Name;
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
