using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.DataService;

namespace MediTech.Reports.Operating.Patient
{
    public partial class MedicalCertificateEng : DevExpress.XtraReports.UI.XtraReport
    {
        public MedicalCertificateEng()
        {
            InitializeComponent();
            this.BeforePrint += MedicalCertificateEng_BeforePrint;
        }

        private void MedicalCertificateEng_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

            long PatientVisitUID = long.Parse(this.Parameters["PatientVisitUID"].Value.ToString());
            var dataSource = (new ReportsService()).PrintMedicalCertificate(PatientVisitUID);

            this.DataSource = dataSource;
        }
    }
}
