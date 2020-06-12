using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.DataService;

namespace MediTech.Reports.Operating.Patient
{
    public partial class MedicalCertificateEng2Parts : DevExpress.XtraReports.UI.XtraReport
    {
        public MedicalCertificateEng2Parts()
        {
            InitializeComponent();
            this.BeforePrint += MedicalCertificateEng2Parts_BeforePrint;
        }

        private void MedicalCertificateEng2Parts_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            long PatientVisitUID = long.Parse(this.Parameters["PatientVisitUID"].Value.ToString());
            var dataSource = (new ReportsService()).PrintConfinedSpaceCertificate(PatientVisitUID);

            this.DataSource = dataSource;
        }
    }
}
