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
            int organisationUID = this.Parameters["OrganisationUID"].Value.ToString() != "" ? Convert.ToInt32(this.Parameters["OrganisationUID"].Value) : 0;
            DateTime dateFrom = Convert.ToDateTime(this.Parameters["DateFrom"].Value);
            DateTime dateTo = Convert.ToDateTime(this.Parameters["DateTo"].Value);
            this.DataSource = (new ReportsService()).StockIssueReport(dateFrom, dateTo, organisationUID != 0 ? organisationUID : (int?)null);
        }
    }
}
