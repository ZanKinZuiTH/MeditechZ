using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.DataService;

namespace MediTech.Reports.Statistic.Inventory
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
            string organisationList = this.Parameters["OrganisationList"].Value.ToString();
            DateTime dateFrom = Convert.ToDateTime(this.Parameters["DateFrom"].Value);
            DateTime dateTo = Convert.ToDateTime(this.Parameters["DateTo"].Value);
            this.DataSource = (new ReportsService()).StockIssueReport(dateFrom, dateTo, organisationList);
        }
    }
}
