using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.DataService;

namespace MediTech.Reports.Statistic.Inventory
{
    public partial class StockExpired : DevExpress.XtraReports.UI.XtraReport
    {
        public StockExpired()
        {
            InitializeComponent();
            this.BeforePrint += StockExpiry_BeforePrint;
        }

        void StockExpiry_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string organisationList = this.Parameters["OrganisationList"].Value.ToString();
            this.DataSource = (new ReportsService()).StockExpiredReport(organisationList);
        }

    }
}
