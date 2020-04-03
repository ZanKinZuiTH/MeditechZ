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
    public class ReportParameter5ViewModel : MediTechViewModelBase
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


        private DateTime? _Year;

        public DateTime? Year
        {
            get { return _Year; }
            set { Set(ref _Year, value); }
        }

        private List<LookupReferenceValueModel> _Months;

        public List<LookupReferenceValueModel> Months
        {
            get { return _Months; }
            set { Set(ref _Months, value); }
        }


        private List<object> _SelectMonths;

        public List<object> SelectMonths
        {
            get { return _SelectMonths ?? (_SelectMonths = new List<object>()); }
            set { Set(ref _SelectMonths, value); }
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

        public ReportParameter5ViewModel()
        {
            Year = DateTime.Now;
            Organisations = GetHealthOrganisationRole();
            if (Organisations != null)
            {
                SelectOrganisation = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == AppUtil.Current.OwnerOrganisationUID);
            }

            Months = new List<LookupReferenceValueModel>()
            {
                new LookupReferenceValueModel(){ Key = 1,Display = "มกราคม"},
                new LookupReferenceValueModel(){ Key = 2,Display = "กุมภาพันธ์"},
                new LookupReferenceValueModel(){ Key = 3,Display = "มีนาคม"},
                new LookupReferenceValueModel(){ Key = 4,Display = "เมษายน"},
                new LookupReferenceValueModel(){ Key = 5,Display = "พฤษพาคม"},
                new LookupReferenceValueModel(){ Key = 6,Display = "มิถุนายา"},
                new LookupReferenceValueModel(){ Key = 7,Display = "กรกฎาคม"},
                new LookupReferenceValueModel(){ Key = 8,Display = "สิงหาคม"},
                new LookupReferenceValueModel(){ Key = 9,Display = "กันยายน"},
                new LookupReferenceValueModel(){ Key = 10,Display = "ตุลาคม"},
                new LookupReferenceValueModel(){ Key = 11,Display = "พฤศจิกายน"},
                new LookupReferenceValueModel(){ Key = 12,Display = "ธันวาคม"},
            };


        }

        void Print()
        {
            string months = "";
            if (SelectMonths != null)
            {
                foreach (object item in SelectMonths)
                {
                    if (item.ToString() != "0")
                    {
                        if (months == "")
                        {
                            months = item.ToString();
                        }
                        else
                        {
                            months += "," + item.ToString();
                        }
                    }

                }
            }

            if (string.IsNullOrEmpty(months))
            {
                WarningDialog("กรุณาเลือกเดือน");
                return;
            }
            var myReport = Activator.CreateInstance(Type.GetType(ReportTemplate.NamespaceName));
            XtraReport report = (XtraReport)myReport;

            foreach (var item in report.Parameters)
            {
                switch (item.Name)
                {
                    case "Year":
                        item.Value = Year != null ? Year.Value.Year : DateTime.Now.Year;
                        break;
                    case "MonthLists":
                        item.Value = months;
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
