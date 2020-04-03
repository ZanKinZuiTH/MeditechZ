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

namespace MediTech.Reports.Statistic.Registration
{
    public partial class PatientSumByAreaPerMonth : DevExpress.XtraReports.UI.XtraReport
    {
        public PatientSumByAreaPerMonth()
        {
            InitializeComponent();
            this.BeforePrint += PatientProblemStatistic_BeforePrint;
        }

        private void PatientProblemStatistic_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int organisationUID = this.Parameters["OrganisationUID"].Value.ToString() != "" ? Convert.ToInt32(this.Parameters["OrganisationUID"].Value) : 0;
            int? vistyuid = this.Parameters["VISTYUID"].Value.ToString() != "" ? Convert.ToInt32(this.Parameters["VISTYUID"].Value) : 0;
            DateTime dateFrom = Convert.ToDateTime(this.Parameters["DateFrom"].Value);
            DateTime dateTo = Convert.ToDateTime(this.Parameters["DateTo"].Value);
            List<PatientSumByAreaModel> dataStatistic = (new ReportsService()).PatientSumByAreaPerMonth(dateFrom, dateTo, vistyuid != 0 ? vistyuid : (int?)null, organisationUID != 0 ? organisationUID : (int?)null);
            this.DataSource = dataStatistic;
          
        }
    }
}
