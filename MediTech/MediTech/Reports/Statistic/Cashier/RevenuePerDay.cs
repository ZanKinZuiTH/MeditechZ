using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace MediTech.Reports.Statistic.Cashier
{
    public partial class RevenuePerDay : DevExpress.XtraReports.UI.XtraReport
    {
        public RevenuePerDay()
        {
            InitializeComponent();
            this.BeforePrint += RevenuePerDay_BeforePrint;
        }

        private void RevenuePerDay_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string organisationList = this.Parameters["OrganisationList"].Value.ToString();
            DateTime date = Convert.ToDateTime(this.Parameters["Date"].Value);
        }
    }
}
