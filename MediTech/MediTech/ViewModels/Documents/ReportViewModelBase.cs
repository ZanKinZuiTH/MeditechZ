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

namespace MediTech.ViewModels
{
    public class ReportViewModelBase : MediTechViewModelBase
    {
        #region Properties
        private List<RoleReportPermissionModel> _ListReports;

        public List<RoleReportPermissionModel> ListReports
        {
            get { return _ListReports; }
            set { Set(ref _ListReports, value); }
        }

        private RoleReportPermissionModel _SelectReport;

        public RoleReportPermissionModel SelectReport
        {
            get { return _SelectReport; }
            set { Set(ref _SelectReport, value); }
        }


        #endregion

        #region Command
        private RelayCommand _ChooseReportCommand;

        /// <summary>
        /// Gets the ChooseReportCommand.
        /// </summary>
        public RelayCommand ChooseReportCommand
        {
            get
            {
                return _ChooseReportCommand
                    ?? (_ChooseReportCommand = new RelayCommand(ChooseReport));
            }
        }
        #endregion

        #region Method


        void ChooseReport()
        {
            if (SelectReport != null)
            {
                if (SelectReport.ViewCode == "PARAM1")
                {
                    ChangeView(new ReportParameter1(SelectReport), SelectReport.Name, this.View);
                }
                else if (SelectReport.ViewCode == "PARAM2")
                {
                    ChangeView(new ReportParameter2(SelectReport), SelectReport.Name, this.View);
                }
                else if (SelectReport.ViewCode == "PARAM3")
                {
                    ChangeView(new ReportParameter3(SelectReport), SelectReport.Name, this.View);
                }
                else if (SelectReport.ViewCode == "PARAM4")
                {
                    ChangeView(new ReportParameter4(SelectReport), SelectReport.Name, this.View);
                }
                else if (SelectReport.ViewCode == "PARAM5")
                {
                    ChangeView(new ReportParameter5(SelectReport), SelectReport.Name, this.View);
                }
                else if (SelectReport.ViewCode == "PARAM6")
                {
                    ReportParameter3 rptPara = new ReportParameter3(SelectReport);
                    rptPara.dteFrom.Mask = "MMM yyyy";
                    rptPara.dteFrom.MaskUseAsDisplayFormat = true;
                    rptPara.dteFrom.StyleSettings = new DevExpress.Xpf.Editors.DateEditPickerStyleSettings();
                    rptPara.dteTo.Mask = "MMM yyyy";
                    rptPara.dteTo.MaskUseAsDisplayFormat = true;
                    rptPara.dteTo.StyleSettings = new DevExpress.Xpf.Editors.DateEditPickerStyleSettings();
                    ChangeView(rptPara, SelectReport.Name, this.View);
                }
                else if (SelectReport.ViewCode == "PARAM7")
                {
                    ChangeView(new ReportParameter7(SelectReport), SelectReport.Name, this.View);
                }
                else if(SelectReport.ViewCode == "NONPARAM")
                {
                    var myReport = Activator.CreateInstance(Type.GetType(SelectReport.NamespaceName));
                    XtraReport report = (XtraReport)myReport;
                    ReportPrintTool printTool = new ReportPrintTool(report);
                    report.ShowPrintMarginsWarning = false;
                    printTool.ShowPreviewDialog();
                }
                else if(SelectReport.ViewCode == "PATRP")
                {
                    ChangeView(new PatientSummeryReport(), SelectReport.Name, this.View);
                }
                else if(SelectReport.ViewCode == "CHKJRP")
                {
                    ChangeView(new CheckupJobSummeryReport(), SelectReport.Name, this.View);
                }
                else if (SelectReport.ViewCode == "PARAM112")
                {
                    ChangeView(new ReportParameter8(SelectReport), SelectReport.Name, this.View);
                }
                else if (SelectReport.ViewCode == "PARAM113")
                {
                    ChangeView(new ReportParameter8(SelectReport), SelectReport.Name, this.View);
                }
                else if (SelectReport.ViewCode == "PARAM114")
                {
                    ChangeView(new ReportParameter4(SelectReport), SelectReport.Name, this.View);
                }
            }
        }

        #endregion
    }
}
