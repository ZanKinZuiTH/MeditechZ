using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.DataService;

namespace MediTech.Reports.Statistic.Inventory
{
    public partial class StockExpiry8month : DevExpress.XtraReports.UI.XtraReport
    {
        public StockExpiry8month()
        {
            InitializeComponent();
            this.BeforePrint += StockExpiry8month_BeforePrint;
        }

        void StockExpiry8month_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string organisationList = this.Parameters["OrganisationList"].Value.ToString();
            this.DataSource = (new ReportsService()).StockExpiryReport(8, organisationList);
        }

    }
}
