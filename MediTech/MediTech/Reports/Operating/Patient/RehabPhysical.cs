using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.DataService;

namespace MediTech.Reports.Operating.Patient
{
    public partial class RehabPhysical : DevExpress.XtraReports.UI.XtraReport
    {
        public RehabPhysical()
        {
            InitializeComponent();
            this.BeforePrint += MedicalCertificate_BeforePrint;
        }
        private void MedicalCertificate_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

            int OrganisationUID = int.Parse(this.Parameters["OrganisationUID"].Value.ToString());
            long PatientVisitUID = long.Parse(this.Parameters["PatientVisitUID"].Value.ToString());
            var dataSource = (new ReportsService()).PrintConfinedSpaceCertificate(PatientVisitUID);

            this.DataSource = dataSource;
        }
         
    }
}
