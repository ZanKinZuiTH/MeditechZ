using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.DataService;

namespace MediTech.Reports.Statistic.Inventory
{
    public partial class StockOnHand : DevExpress.XtraReports.UI.XtraReport
    {
        public StockOnHand()
        {
            InitializeComponent();
            this.BeforePrint += StockOnHand_BeforePrint;
        }

        void StockOnHand_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string organisationList = this.Parameters["OrganisationList"].Value.ToString();
            this.DataSource = (new ReportsService()).StockOnHand(organisationList);
        }

    }
}
