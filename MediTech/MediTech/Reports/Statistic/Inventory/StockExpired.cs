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
            int? organisationUID = this.Parameters["OrganisationUID"].Value.ToString() != "" ? Convert.ToInt32(this.Parameters["OrganisationUID"].Value) : 0;
            this.DataSource = (new ReportsService()).StockExpiredReport(organisationUID);
        }

    }
}
