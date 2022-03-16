using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using MediTech.DataService;
using MediTech.Model;
using System.Collections.Generic;

namespace MediTech.Reports.Statistic.Ecount
{
    public partial class EcountGetPayorDetail : DevExpress.XtraReports.UI.XtraReport
    {
        public EcountGetPayorDetail()
        {
            InitializeComponent();
            BeforePrint += EcountGetPayorDetail_BeforePrint;
        }

        private void EcountGetPayorDetail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

            DateTime dateFrom = Convert.ToDateTime(this.Parameters["Date"].Value);
            DateTime dateTo = Convert.ToDateTime(this.Parameters["Date"].Value);
            List<PayorDetailModel> data = (new ReportsService()).GetPayordetailByDate(dateFrom, dateTo);
            this.DataSource = data;
        }
    }
}
