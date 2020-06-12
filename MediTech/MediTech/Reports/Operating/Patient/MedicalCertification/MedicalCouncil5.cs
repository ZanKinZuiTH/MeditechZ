using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.DataService;

namespace MediTech.Reports.Operating.Patient
{
    public partial class MedicalCouncil5 : DevExpress.XtraReports.UI.XtraReport
    {
        public MedicalCouncil5()
        {
            InitializeComponent();
            this.BeforePrint += MedicalCouncil5_BeforePrint;
           
        }

        private void MedicalCouncil5_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
          
            long PatientVisitUID = long.Parse(this.Parameters["PatientVisitUID"].Value.ToString());
            var dataSource = (new ReportsService()).PrintConfinedSpaceCertificate(PatientVisitUID);



            this.DataSource = dataSource;
        }
    }
}
