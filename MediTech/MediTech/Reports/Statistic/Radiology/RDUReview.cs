using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.Model.Report;
using MediTech.DataService;
using System.Collections.Generic;

namespace MediTech.Reports.Statistic.Radiology
{
    public partial class RDUReview : DevExpress.XtraReports.UI.XtraReport
    {
        public RDUReview()
        {
            InitializeComponent();
            this.BeforePrint += RadiologyRDUReview_BeforePrint;
        }

        private void RadiologyRDUReview_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int organisationUID = this.Parameters["OrganisationUID"].Value.ToString() != "" ? Convert.ToInt32(this.Parameters["OrganisationUID"].Value) : 0;
            DateTime dateFrom = Convert.ToDateTime(this.Parameters["DateFrom"].Value);
            DateTime dateTo = Convert.ToDateTime(this.Parameters["DateTo"].Value);
            List<RadiologyRDUReviewModel> dataStatistic = (new ReportsService()).GetRadiologyRDUReview(dateFrom, dateTo, organisationUID != 0 ? organisationUID : (int?)null);
            this.DataSource = dataStatistic;
        }
    }
}
