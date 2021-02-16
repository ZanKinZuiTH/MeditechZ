using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.DataService;
using System.Data;
using System.Linq;
using MediTech.Model.Report;
using System.Collections.Generic;
using DevExpress.XtraCharts;

namespace MediTech.Reports.Statistic.Patient
{
    public partial class PatientProblemStatistic : DevExpress.XtraReports.UI.XtraReport
    {
        public PatientProblemStatistic()
        {
            InitializeComponent();
            this.BeforePrint += PatientProblemStatistic_BeforePrint;
        }

        private void PatientProblemStatistic_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string organisationList = this.Parameters["OrganisationList"].Value.ToString();
            int? vistyuid = this.Parameters["VISTYUID"].Value.ToString() != "" ? Convert.ToInt32(this.Parameters["VISTYUID"].Value) : 0;
            DateTime dateFrom = Convert.ToDateTime(this.Parameters["DateFrom"].Value);
            DateTime dateTo = Convert.ToDateTime(this.Parameters["DateTo"].Value);
            List<ProblemStatisticModel> dataStatistic = (new ReportsService()).PatientProblemStaistic(dateFrom, dateTo, vistyuid != 0 ? vistyuid : (int?)null, organisationList);
            this.DataSource = dataStatistic;
          
        }
    }
}
