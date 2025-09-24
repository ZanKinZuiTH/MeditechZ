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
    public class ReportParameter3ViewModel : MediTechViewModelBase
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

        #endregion

        #region Command

        private RelayCommand _PrintCommand;
        public RelayCommand PrintCommand
        {
            get
            {
                return  _PrintCommand
                    ?? ( _PrintCommand = new RelayCommand(Print));
            }
        }

        private RelayCommand _CancelCommand;
        public RelayCommand CancelCommand
        {
            get
            {
                return  _CancelCommand
                    ?? ( _CancelCommand = new RelayCommand(Cancel));
            }
        }
        #endregion

        #region Method

        public ReportParameter3ViewModel()
        {
            DateFrom = DateTime.Now;
            DateTo = DateTime.Now;
            Organisations = GetHealthOrganisationRole();
            var SelectOrganisation = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == AppUtil.Current.OwnerOrganisationUID);
            SelectOrganisations.Add(SelectOrganisation.HealthOrganisationUID);
        }

        void Print()
        {
            var myReport = Activator.CreateInstance(Type.GetType(ReportTemplate.NamespaceName));
            XtraReport report = (XtraReport)myReport; string healthOrganisationList = "";
            if (SelectOrganisations == null || SelectOrganisations.Count == 0)
            {
                WarningDialog("กรุณาเลือกสถานประกอบการ");
                return;
            }
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
