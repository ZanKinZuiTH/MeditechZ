using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.DataService;

namespace MediTech.Reports.Statistic.Inventory
{
    public partial class StockAdjustmentOut : DevExpress.XtraReports.UI.XtraReport
    {
        public StockAdjustmentOut()
        {
            InitializeComponent();
            this.BeforePrint += StockAdjustmentOut_BeforePrint;
        }
        private void StockAdjustmentOut_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int organisationUID = this.Parameters["OrganisationUID"].Value.ToString() != "" ? Convert.ToInt32(this.Parameters["OrganisationUID"].Value) : 0;
            DateTime dateFrom = Convert.ToDateTime(this.Parameters["DateFrom"].Value);
            DateTime dateTo = Convert.ToDateTime(this.Parameters["DateTo"].Value);
            this.DataSource = (new ReportsService()).StockAdjustmentOut(dateFrom, dateTo, organisationUID);

        }
    }
}
