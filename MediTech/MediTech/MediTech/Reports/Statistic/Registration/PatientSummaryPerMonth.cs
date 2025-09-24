using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.DataService;
using MediTech.Model.Report;
using System.Collections.Generic;
using System.Linq;

namespace MediTech.Reports.Statistic.Registration
{
    public partial class PatientSummaryPerMonth : DevExpress.XtraReports.UI.XtraReport
    {
        public PatientSummaryPerMonth()
        {
            InitializeComponent();
            this.BeforePrint += VisitSummary_BeforePrint;
        }

        private void VisitSummary_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string organisationList = this.Parameters["OrganisationList"].Value.ToString() ;        
            int year = Convert.ToInt32(this.Parameters["Year"].Value);
            string monthLists = this.Parameters["MonthLists"].Value.ToString();
            List<PatientSummaryModel> dataStatistic = (new ReportsService()).PatientSummaryPerMonth(year, monthLists, organisationList);
            if (dataStatistic != null && dataStatistic.Count > 0)
            {
                this.DataSource = dataStatistic;
                //PatientSummaryModel data = dataStatistic.FirstOrDefault();
                //cellMale.Text = data.Male.ToString();
                //cellFemale.Text = data.Female.ToString();
                //cellUnknown.Text = data.Unknown.ToString();
                //cellAdult.Text = data.Adut.ToString();
                //cellChild.Text = data.Child.ToString();
                //cellThai.Text = data.Thai.ToString();
                //cellThaiNew.Text = data.ThaiNew.ToString();
                //cellThaiOld.Text = data.ThaiOld.ToString();
                //cellforeign.Text = data.Foreign.ToString();
                //cellForeignNew.Text = data.ForeignNew.ToString();
                //cellForeignOld.Text = data.ForeignOld.ToString();
                //cellNoNation.Text = data.NoInputNation.ToString();
                //CellNoNationNew.Text = data.NoInputNationNew.ToString();
                //CellNoNationOld.Text = data.NoInputNationOld.ToString();
                //cell0_4.Text = data.age0_4.ToString();
                //cell5_9.Text = data.age5_9.ToString();
                //cell10_14.Text = data.age10_14.ToString();
                //cell15_19.Text = data.age15_19.ToString();
                //cell20_24.Text = data.age20_24.ToString();
                //cell25_29.Text = data.age25_29.ToString();
                //cell30_34.Text = data.age30_34.ToString();
                //cell35_39.Text = data.age35_39.ToString();
                //cell40_44.Text = data.age40_44.ToString();
                //cell45_49.Text = data.age45_49.ToString();
                //cell50_54.Text = data.age50_54.ToString();
                //cell55_59.Text = data.age55_59.ToString();
                //cell60_64.Text = data.age60_64.ToString();
                //cell65_69.Text = data.age65_69.ToString();
                //cell70.Text = data.age70.ToString();
                //cellConsult.Text = data.Counsult.ToString();
                //cellRepeat.Text = data.Repeat.ToString();
            }

        }
    }
}
