using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace MediTech.Reports.Operating.Patient.RiskBook
{
    public partial class PatientName : DevExpress.XtraReports.UI.XtraReport
    {
        public PatientName()
        {
            InitializeComponent();
        }

        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }
    }
}
