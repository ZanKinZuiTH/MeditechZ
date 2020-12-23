using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.Model;

namespace MediTech.Reports.Statistic.Checkup
{
    public partial class CheckupSummary : DevExpress.XtraReports.UI.XtraReport
    {
        MediTech.DataService.MediTechDataService dbService = new DataService.MediTechDataService();

        CheckupSummaryChart fChartSummary = new CheckupSummaryChart();
        public CheckupSummary()
        {
            InitializeComponent();
            this.AfterPrint += CheckupSummary_AfterPrint;
            this.BeforePrint += CheckupSummary_BeforePrint;
        }

        private void CheckupSummary_AfterPrint(object sender, EventArgs e)
        {
            string title = this.Parameters["Title"].Value.ToString();
            string year = this.Parameters["Year"].Value.ToString();
            fChartSummary.Parameters["Header"].Value = "กราฟแสดงผู้ที่มีผลการตรวจปกติ และผลผิดปกติประจำปี " + year
                + Environment.NewLine + title;
            fChartSummary.checkupChart.DataSource = this.DataSource;
            fChartSummary.CreateDocument();
            this.Pages.AddRange(fChartSummary.Pages);

        }

        private void CheckupSummary_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

            int checkupJobUID = Convert.ToInt32(this.Parameters["CheckupJobUID"].Value);
            string companyName = this.Parameters["CompanyName"].Value.ToString();
            string GPRSTUIDs = this.Parameters["GPRSTUIDs"].Value.ToString();
            string Year = this.Parameters["Year"].Value.ToString();
            string title = this.Parameters["Title"].Value.ToString();
            lbTitle.Text = title + Environment.NewLine + " โปรแกรมตรวจสุขภาพประจำปี " + Year;
            CheckupBranchModel branchModel = new CheckupBranchModel();
            branchModel.CheckupJobUID = checkupJobUID;
            branchModel.GPRSTUIDs = GPRSTUIDs;
            branchModel.BranchName = companyName;
            var dataSummary = dbService.Reports.CheckupSummary(branchModel);
            this.DataSource = dataSummary;
        }
    }
}
