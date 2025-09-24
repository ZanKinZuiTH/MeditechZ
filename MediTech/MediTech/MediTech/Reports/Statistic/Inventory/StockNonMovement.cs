using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.DataService;

namespace MediTech.Reports.Statistic.Inventory
{
    public partial class StockNonMovement : DevExpress.XtraReports.UI.XtraReport
    {
        public StockNonMovement()
        {
            InitializeComponent();
            this.BeforePrint += StockNonMovement_BeforePrint;
        }

        void StockNonMovement_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string organisationList = this.Parameters["OrganisationList"].Value.ToString();
            DateTime dateFrom = Convert.ToDateTime(this.Parameters["DateFrom"].Value);
            DateTime dateTo = Convert.ToDateTime(this.Parameters["DateTo"].Value);
            this.DataSource = (new ReportsService()).StockNonMovement(dateFrom, dateTo, organisationList);
        }

    }
}
