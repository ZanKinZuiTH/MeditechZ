using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.DataService;

namespace MediTech.Reports.Statistic.Inventory
{
    public partial class StockBalancePerMounth : DevExpress.XtraReports.UI.XtraReport
    {
        public StockBalancePerMounth()
        {
            InitializeComponent();
            this.BeforePrint += StockBalancePerMounth_BeforePrint;
        }

        private void StockBalancePerMounth_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string organisationList = this.Parameters["OrganisationList"].Value.ToString() ;
            int year = Convert.ToInt32(this.Parameters["Year"].Value);
            string monthLists = this.Parameters["MonthLists"].Value.ToString();
            this.DataSource = (new ReportsService()).StockBalancePerMounth(year, monthLists, organisationList);
        }
    }
}
