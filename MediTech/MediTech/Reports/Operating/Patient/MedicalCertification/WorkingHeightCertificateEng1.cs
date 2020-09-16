using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using MediTech.Reports.Operating.Patient.MedicalCertification;
using MediTech.Model.Report;
using MediTech.DataService;

namespace MediTech.Reports.Operating.Patient
{
    public partial class WorkingHeightCertificateEng1 : DevExpress.XtraReports.UI.XtraReport
    {
        WorkingHeightCertificateEng2 page2 = new WorkingHeightCertificateEng2();
        MedicalCertificateModel model = new MedicalCertificateModel();
        public WorkingHeightCertificateEng1()
        {
            InitializeComponent();
            BeforePrint += WorkingHeightCertificateEng1_BeforePrint;
            AfterPrint += Page2_AfterPrint;
        }
        private void WorkingHeightCertificateEng1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            long PatientVisitUID = long.Parse(this.Parameters["PatientVisitUID"].Value.ToString());
            model = (new ReportsService()).PrintConfinedSpaceCertificate(PatientVisitUID);
            
            if(model != null)
            {
                this.lbIdCard.Text = model.IDCard.ToString();

                page2.lbDoctor.Text = model.DoctorEngName;
                page2.lbDoctorNo.Text = model.DoctorLicenseNo;
                page2.lbDateNow.Text = DateTime.Now.ToString("dd'/'MM'/'yyyy");
                page2.lbDate.Text = model.strVisitData?.ToString("dd'/'MM'/'yyyy");
                page2.lbWeight.Text = model.Weight.ToString();
                page2.lbHeight.Text = model.Height.ToString();
                page2.lbBMI.Text = model.BMI.ToString();
                page2.lbBP.Text = model.BPSys.ToString() + "/" + model.BPDio.ToString();
                page2.lbPulse.Text = model.Pulse.ToString();
            }
            
        }
        private void Page2_AfterPrint(object sender, EventArgs e)
        {
            page2.CreateDocument();
            this.Pages.AddRange(page2.Pages);
        }
    }
}
