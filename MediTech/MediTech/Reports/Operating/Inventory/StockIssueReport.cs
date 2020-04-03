using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.DataService;
namespace MediTech.Reports.Operating.Inventory
{
    public partial class StockIssueReport : DevExpress.XtraReports.UI.XtraReport
    {
        public StockIssueReport()
        {
            InitializeComponent();
            this.BeforePrint += StockIssueReport_BeforePrint;
        }

        private void StockIssueReport_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string issueID = this.Parameters["IssueID"].Value.ToString();
            var dataSource = (new ReportsService()).StockIssueReport(issueID);
            if (dataSource != null && dataSource.Count > 0)
            {
                int i = 1;

                foreach (var item in dataSource)
                {
                    item.RowNumber = i;
                    i++;
                }
            }
            this.DataSource = dataSource;
        }
    }
}
