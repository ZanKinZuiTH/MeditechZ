using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.DataService;

namespace MediTech.Reports.Statistic.Cashier
{
	public partial class DoctorFeeReport2 : DevExpress.XtraReports.UI.XtraReport
	{	
		public DoctorFeeReport2()
		{
			InitializeComponent();
            this.BeforePrint += DoctorFeeReport_BeforePrint;
		}

        private void DoctorFeeReport_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int? careproviderUID = this.Parameters["CareproviderUID"].Value.ToString() != "0" ? Convert.ToInt32(this.Parameters["CareproviderUID"].Value) : (int?)null;
            DateTime dateFrom = Convert.ToDateTime(this.Parameters["DateFrom"].Value);
            DateTime dateTo = Convert.ToDateTime(this.Parameters["DateTo"].Value);
            var data = (new ReportsService()).GetDoctorfeeReport2(dateFrom, dateTo, careproviderUID);
            this.DataSource = data;
        }
    }
}
