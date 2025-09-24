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
            string organisationList = this.Parameters["OrganisationList"].Value.ToString() ;
            DateTime dateFrom = Convert.ToDateTime(this.Parameters["DateFrom"].Value);
            DateTime dateTo = Convert.ToDateTime(this.Parameters["DateTo"].Value);
            List<RadiologyRDUReviewModel> dataStatistic = (new ReportsService()).GetRadiologyRDUReview(dateFrom, dateTo, organisationList);
            this.DataSource = dataStatistic;
        }
    }
}
