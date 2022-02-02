using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.DataService;
using MediTech.Model.Report;
using System.Collections.Generic;

namespace MediTech.Reports.Statistic.Cashier
{
    public partial class StockToEcount : DevExpress.XtraReports.UI.XtraReport
    {
        public StockToEcount()
        {
            InitializeComponent();
            this.BeforePrint += StockToEcount_BeforePrint;
         
        }

  

        private void StockToEcount_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string organisationList = this.Parameters["OrganisationList"].Value.ToString();
            DateTime dateFrom = Convert.ToDateTime(this.Parameters["DateFrom"].Value);
            DateTime dateTo = Convert.ToDateTime(this.Parameters["DateTo"].Value);
            int? vistyuid = this.Parameters["VISTYUID"].Value.ToString() != "0" ? Convert.ToInt32(this.Parameters["VISTYUID"].Value) : (int?)null;
            //xrPivotGrid1.DataSource = (new ReportsService()).GetStockToEcount(dateFrom, dateTo, vistyuid, organisationList);
            List<EcountExportModel>  data  = (new ReportsService()).GetStockToEcount(dateFrom, dateTo, vistyuid, organisationList);
            this.DataSource = data;
        }

      
    }
}
