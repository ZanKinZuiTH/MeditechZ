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

        private HealthOrganisationModel _SelectOrganisation;

        public HealthOrganisationModel SelectOrganisation
        {
            get { return _SelectOrganisation; }
            set { Set(ref _SelectOrganisation, value); }
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
            if (Organisations != null)
            {
                SelectOrganisation = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == AppUtil.Current.OwnerOrganisationUID);
            }
        }

        void Search()
        {
            int? OwnerOrganisationUID = SelectOrganisation != null ? SelectOrganisation.HealthOrganisationUID : (int?)null;
            PatientSummaryDatas = DataService.Reports.PatientSummaryData(DateFrom, DateTo, OwnerOrganisationUID);
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


        private string ShowSaveFileDialog(string title, string filter)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            string name = System.Windows.Forms.Application.ProductName;
            int n = name.LastIndexOf(".") + 1;
            if (n > 0) name = name.Substring(n, name.Length - n);
            dlg.Title = "Export To " + title;
            dlg.FileName = name;
            dlg.Filter = filter;
            if (dlg.ShowDialog() == DialogResult.OK) return dlg.FileName;
            return "";
        }

        private void OpenFile(string fileName)
        {
            if (System.Windows.Forms.MessageBox.Show("Do you want to open this file?", "Export To...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    process.StartInfo.FileName = fileName;
                    process.StartInfo.Verb = "Open";
                    process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                    process.Start();
                }
                catch
                {
                    System.Windows.Forms.MessageBox.Show("Cannot find an application on your system suitable for openning the file with exported data.", System.Windows.Forms.Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        #endregion
    }
}
