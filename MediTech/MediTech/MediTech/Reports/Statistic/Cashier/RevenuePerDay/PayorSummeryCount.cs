using DevExpress.XtraReports.UI;
using MediTech.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace MediTech.Reports.Statistic.Cashier
{
    public partial class PayorSummeryCount : DevExpress.XtraReports.UI.XtraReport
    {
        public PayorSummeryCount()
        {
            InitializeComponent();
        }

        private void PayorSummeryCount_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            lblTitle.Visible = true;
            if (this.DataSource == null)
            {
                lblTitle.Visible = false;
            }

            if (this.DataSource != null)
            {
                if (this.DataSource is List<PatientVisitModel>)
                {
                    if ((this.DataSource as List<PatientVisitModel>).Count <= 0)
                    {
                        lblTitle.Visible = false;
                    }
                }
            }
        }
    }
}
