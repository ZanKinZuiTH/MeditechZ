using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.DataService;
namespace MediTech.Reports.Operating.Inventory
{
    public partial class GoodReceiveReport : DevExpress.XtraReports.UI.XtraReport
    {
        public GoodReceiveReport()
        {
            InitializeComponent();
            this.BeforePrint += GoodReceiveReport_BeforePrint;
        }

        private void GoodReceiveReport_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string grnNumber = this.Parameters["GRNNumber"].Value.ToString();
            var dataSource = (new ReportsService()).GoodReceiveReport(grnNumber);
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
