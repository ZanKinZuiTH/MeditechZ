using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.DataService;
using System.Linq;

namespace MediTech.Reports.Statistic.Inventory
{
    public partial class StockDispensed : DevExpress.XtraReports.UI.XtraReport
    {
        public StockDispensed()
        {
            InitializeComponent();
            this.BeforePrint += StockDispensed_BeforePrint;
        }

        void StockDispensed_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int organisationUID = this.Parameters["OrganisationUID"].Value.ToString() != "" ? Convert.ToInt32(this.Parameters["OrganisationUID"].Value) : 0;
            DateTime dateFrom = Convert.ToDateTime(this.Parameters["DateFrom"].Value);
            DateTime dateTo = Convert.ToDateTime(this.Parameters["DateTo"].Value);
            var dataDispense = (new ReportsService()).StockDispensedReport(dateFrom, dateTo, organisationUID != 0 ? organisationUID : (int?)null);
            if(dataDispense != null)
            {
                this.DataSource = dataDispense;
                double cost = Math.Round(dataDispense.Where(p => p.OrderStatus == "Dispensed").Sum(p => p.TotalCost), 2);
                double cancel = Math.Round(dataDispense.Where(p => p.OrderStatus == "Cancelled Dispense").Sum(p => p.TotalCost), 2);
                double price = Math.Round(dataDispense.Where(p => p.OrderStatus == "Dispensed").Sum(p => p.NetPrice), 2);
                xrCancel.Text = cancel.ToString();
                xrCost.Text = cost.ToString();
                xrPrice.Text = price.ToString();
                xrNetProfit.Text = (price - cost).ToString();
            }

        }

    }
}
