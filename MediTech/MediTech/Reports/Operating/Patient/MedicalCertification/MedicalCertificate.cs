using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.DataService;

namespace MediTech.Reports.Operating.Patient
{
    public partial class MedicalCertificate : DevExpress.XtraReports.UI.XtraReport
    {
        public MedicalCertificate()
        {
            InitializeComponent();
            this.BeforePrint += MedicalCertificate_BeforePrint;
        }

        private void MedicalCertificate_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

            long PatientVisitUID = long.Parse(this.Parameters["PatientVisitUID"].Value.ToString());
            var dataSource = (new ReportsService()).PrintMedicalCertificate(PatientVisitUID);

            this.DataSource = dataSource;
        }
    }
}
