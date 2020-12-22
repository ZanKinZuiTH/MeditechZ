using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace MediTech.Reports.Statistic.Checkup
{
    public partial class CheckupSummary : DevExpress.XtraReports.UI.XtraReport
    {
        MediTech.DataService.MediTechDataService dbService = new DataService.MediTechDataService();

        CheckupSummaryChart page2 = new CheckupSummaryChart();
        public CheckupSummary()
        {
            InitializeComponent();
            this.AfterPrint += CheckupSummary_AfterPrint;
            this.BeforePrint += CheckupSummary_BeforePrint;
        }

        private void CheckupSummary_AfterPrint(object sender, EventArgs e)
        {
            page2.Parameters["Header"].Value = "กราฟแสดงผู้ที่มีผลการตรวจปกติ และผลผิดปกติประจำปี " + this.Parameters["Year"].Value.ToString()
                + Environment.NewLine
                + this.Parameters["CompanyName"].Value.ToString();
            page2.CreateDocument();
            this.Pages.AddRange(page2.Pages);
        }

        private void CheckupSummary_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int checkupJobUID = Convert.ToInt32(this.Parameters["CheckupJobUID"].Value);
            string companyName = this.Parameters["CompanyName"].Value.ToString();
            string GPRSTUIDs = this.Parameters["GPRSTUIDs"].Value.ToString();
            var dataSummary = dbService.Reports.CheckupSummary(checkupJobUID, GPRSTUIDs, companyName);
            this.DataSource = dataSummary;
        }
    }
}
