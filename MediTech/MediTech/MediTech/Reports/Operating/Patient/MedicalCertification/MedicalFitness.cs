using DevExpress.XtraReports.UI;
using MediTech.DataService;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace MediTech.Reports.Operating.Patient
{
    public partial class MedicalFitness : DevExpress.XtraReports.UI.XtraReport
    {
        public MedicalFitness()
        {
            InitializeComponent();
            this.BeforePrint += MedicalFitness_BeforePrint;
        }

        private void MedicalFitness_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
          // int OrganisationUID = int.Parse(this.Parameters["OrganisationUID"].Value.ToString());
            long PatientVisitUID = long.Parse(this.Parameters["PatientVisitUID"].Value.ToString());
            var dataSource = (new ReportsService()).PrintMedicalCertificate(PatientVisitUID);
            //int logoType = Convert.ToInt32(this.Parameters["LogoType"].Value.ToString());
            this.DataSource = dataSource;
        }
    }
}
