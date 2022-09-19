using MediTech.Model;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediTech.Model.Report;
using System.Windows.Forms;
using MediTech.Views;
using System.Windows;

namespace MediTech.ViewModels
{
    public class PatientSummeryReportViewModel : MediTechViewModelBase
    {
        #region Properties
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

        private DateTime _DateFrom;

        public DateTime DateFrom
        {
            get { return _DateFrom; }
            set { Set(ref _DateFrom, value); }
        }


        private DateTime _DateTo;

        public DateTime DateTo
        {
            get { return _DateTo; }
            set { Set(ref _DateTo, value); }
        }


        private List<PatientSummaryDataModel> _PatientSummaryDatas;

        public List<PatientSummaryDataModel> PatientSummaryDatas
        {
            get { return _PatientSummaryDatas; }
            set { Set(ref _PatientSummaryDatas, value); }
        }

        private bool _VisibiltyCost;
        public bool VisibiltyCost
        {
            get { return _VisibiltyCost; }
            set { Set(ref _VisibiltyCost, value); }
        }

        #endregion

        #region Command

        private RelayCommand _SearchCommand;
        public RelayCommand SearchCommand
        {
            get
            {
                return _SearchCommand
                    ?? (_SearchCommand = new RelayCommand(Search));
            }
        }

        private RelayCommand _ExportToExcelCommand;
        public RelayCommand ExportToExcelCommand
        {
            get
            {
                return _ExportToExcelCommand
                    ?? (_ExportToExcelCommand = new RelayCommand(ExportToExcel));
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

        public PatientSummeryReportViewModel()
        {
            DateFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTo = DateTime.Now;
            Organisations = GetHealthOrganisationRole();
            var SelectOrganisation = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == AppUtil.Current.OwnerOrganisationUID);
            SelectOrganisations.Add(SelectOrganisation.HealthOrganisationUID);
            IsPermission();
        }

        void Search()
        {

            string healthOrganisationList = "";
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

            PatientSummaryDatas = DataService.Reports.PatientSummaryData(DateFrom, DateTo, healthOrganisationList);
        }

        void ExportToExcel()
        {
            if (PatientSummaryDatas != null)
            {
                string fileName = ShowSaveFileDialog("Microsoft Excel Document", "Microsoft Excel|*.xlsx");
                if (fileName != "")
                {
                    PatientSummeryReport view = (PatientSummeryReport)this.View;
                    view.gvPatientSummary.ExportToXlsx(fileName);
                    OpenFile(fileName);
                }

            }
        }
        void Cancel()
        {
            ChangeViewPermission(this.BackwardView);
        }

        void IsPermission()
        {
            var permission = DataService.RoleManage.GetPageViewPermission(AppUtil.Current.RoleUID);
            var Ispermission = permission.FirstOrDefault(p => p.Type == "PERMISSION" && p.PageViewCode == "ISCSTR");
            if (Ispermission == null)
            {
                VisibiltyCost = false;
            }
            else
            {
                VisibiltyCost = true;
            }
        }

        #endregion
    }
}
