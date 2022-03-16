using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.DataService;
using MediTech.Model.Report;
using System.Collections.Generic;

namespace MediTech.Reports.Statistic.Ecount
{
    public partial class EcountSummaryGroupReceipt : DevExpress.XtraReports.UI.XtraReport
    {
        public EcountSummaryGroupReceipt()
        {
            InitializeComponent();
            BeforePrint += EcountSummaryGroupReceipt_BeforePrint1;
        }

        private void EcountSummaryGroupReceipt_BeforePrint1(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string organisationList = this.Parameters["OrganisationList"].Value.ToString();
            DateTime dateFrom = Convert.ToDateTime(this.Parameters["DateFrom"].Value);
            DateTime dateTo = Convert.ToDateTime(this.Parameters["DateTo"].Value);
            int? vistyuid = this.Parameters["VISTYUID"].Value.ToString() != "0" ? Convert.ToInt32(this.Parameters["VISTYUID"].Value) : (int?)null;
          //  string storefromInRow = this.Parameters["StoreFrom"].Value.ToString();
            List<EcountExportModel> data = (new ReportsService()).GetEcountSumGroupReceipt(dateFrom, dateTo, vistyuid, organisationList);
            this.DataSource = data;
        }

       
    }
}
