using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Windows.Controls;
using DevExpress.XtraPrinting;
using MediTech.DataService;
using MediTech.Model.Report;
using MediTech.Reports.Operating.Patient.MedicalCertification;

namespace MediTech.Reports.Operating.Patient
{
    public partial class ConfinedSpaceCertificate1 : DevExpress.XtraReports.UI.XtraReport
    {
        ConfinedSpaceCertificate2 page2 = new ConfinedSpaceCertificate2();
        MedicalCertificateModel model = new MedicalCertificateModel();


        public ConfinedSpaceCertificate1()
        {
            InitializeComponent();
            BeforePrint += ConfinedSpaceCertificate1_BeforePrint;
            AfterPrint += Page2_AfterPrint;
        }

        private void ConfinedSpaceCertificate1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            long PatientVisitUID = long.Parse(this.Parameters["PatientVisitUID"].Value.ToString());
            model = (new ReportsService()).PrintConfinedSpaceCertificate(PatientVisitUID);
            
            if(model != null)
            {
                this.lbPatientName.Text = model.PatientName;
                this.lbIdCard.Text = model.IDCard;

                page2.lbDoctor.Text = model.Doctor;
                page2.lbDoctorNo.Text = model.DoctorLicenseNo;
                page2.lbPatientName.Text = model.PatientName;
                page2.lbDateNow.Text = DateTime.Now.ToString("dd'/'MM'/'yyyy");
                page2.lbDate.Text = model.strVisitData?.ToString("dd'/'MM'/'yyyy");
                page2.lbWeight.Text = model.Weight.ToString();
                page2.lbHeight.Text = model.Height.ToString();
                page2.lbBMI.Text = model.BMI.ToString();
                page2.lbBP.Text = model.BPSys.ToString() + "/" + model.BPDio.ToString();
                page2.lbPuls.Text = model.Pulse.ToString();
            } 
        }

        private void Page2_AfterPrint(object sender, EventArgs e)
        {

            page2.CreateDocument();
            this.Pages.AddRange(page2.Pages);
        }

    }
}
