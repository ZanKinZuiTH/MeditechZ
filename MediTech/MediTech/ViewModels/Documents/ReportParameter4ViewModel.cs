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
    public class ReportParameter4ViewModel : MediTechViewModelBase
    {
        #region Properites

        public ReportsModel ReportTemplate { get; set; }

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
            set { Set(ref _SelectOrganisation, value); }
        }


        private DateTime? _Date;

        public DateTime? Date
        {
            get { return _Date; }
            set { Set(ref _Date, value); }
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

        public ReportParameter4ViewModel()
        {
            Date = DateTime.Now;
            Organisations = GetHealthOrganisationRole();
            if (Organisations != null)
            {
                SelectOrganisation = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == AppUtil.Current.OwnerOrganisationUID);
            }
        }

        void Print()
        {
            var myReport = Activator.CreateInstance(Type.GetType(ReportTemplate.NamespaceName));
            XtraReport report = (XtraReport)myReport;

            foreach (var item in report.Parameters)
            {
                switch (item.Name)
                {
                    case "Date":
                        item.Value = Date;
                        break;
                    case "OrganisationName":
                        item.Value = (SelectOrganisation != null && SelectOrganisation.Name != "All") ? !string.IsNullOrEmpty(SelectOrganisation.Description) ? SelectOrganisation.Description : SelectOrganisation.Name : null;
                        break;
                    case "OrganisationAddress":
                        item.Value = SelectOrganisation != null ? SelectOrganisation.AddressFull : null;
                        break;
                    case "OrganisationUID":
                        item.Value = (SelectOrganisation != null && SelectOrganisation.Name != "All") ? SelectOrganisation.HealthOrganisationUID : (object)null;
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
