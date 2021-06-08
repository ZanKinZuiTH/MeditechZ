using DevExpress.XtraReports.UI;
using MediTech.DataService;
using MediTech.Model.Report;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using DevExpress.Xpf.Core;
using System.Windows;

namespace MediTech.Reports.Statistic.Cashier
{
    public partial class RevenuePerDayNewPage : DevExpress.XtraReports.UI.XtraReport
    {

        public RevenuePerDayNewPage()
        {
            InitializeComponent();
            this.BeforePrint += RevenuePerDay_BeforePrint;
        }



        private void RevenuePerDay_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (this.DataSource != null)
            {
                xrCrossTab1.DataSource = this.DataSource;
                lblReportHeader.Text = "รายรับ " + (this.DataSource as List<RevenuePerDayModel>).FirstOrDefault().HealthOrganisationName;
            }


        }
    }
}
