using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.DataService;

namespace MediTech.Reports.Statistic.Cashier
{
	public partial class UCSAndForeignerFeeReport : DevExpress.XtraReports.UI.XtraReport
	{	
		public UCSAndForeignerFeeReport()
		{
			InitializeComponent();
            this.BeforePrint += UCSAndForeignerFeeReport_BeforePrint;
		}

        private void UCSAndForeignerFeeReport_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DateTime dateFrom = Convert.ToDateTime(this.Parameters["DateFrom"].Value);
            DateTime dateTo = Convert.ToDateTime(this.Parameters["DateTo"].Value);
            var data = (new ReportsService()).GetUCSAndForeignerFee(dateFrom, dateTo);
            this.DataSource = data;
        }
    }
}
