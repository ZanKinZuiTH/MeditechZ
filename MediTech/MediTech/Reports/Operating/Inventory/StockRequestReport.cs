using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.DataService;
namespace MediTech.Reports.Operating.Inventory
{
    public partial class StockRequestReport : DevExpress.XtraReports.UI.XtraReport
    {
        public StockRequestReport()
        {
            InitializeComponent();
            this.BeforePrint += StockRequestReport_BeforePrint;
        }

        private void StockRequestReport_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string requestID = this.Parameters["RequestID"].Value.ToString();
            var dataSource = (new ReportsService()).StockRequestReport(requestID);
            if (dataSource != null && dataSource.Count > 0)
            {
                int i = 1;

                foreach (var item in dataSource)
                {
                    item.RowNumber = i;
                    i++;
                }
            }
            this.DataSource = dataSource;
        }
    }
}
