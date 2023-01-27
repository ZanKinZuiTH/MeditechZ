using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Helpers
{
    public static class ConstantData
    {
        public static List<ReportsModel> GetPatientReports()
        {
            List<ReportsModel>  ListPatientReports = new List<ReportsModel>();
            ListPatientReports.Add(new ReportsModel { Name = "OPD Card", NamespaceName = "MediTech.Reports.Operating.Patient.OPDCard" });
            ListPatientReports.Add(new ReportsModel { Name = "OPD กายภาพ", NamespaceName = "MediTech.Reports.Operating.Patient.RehabPhysical" });
            ListPatientReports.Add(new ReportsModel { Name = "Visit Slip", NamespaceName = "MediTech.Reports.Operating.Patient.PatientVisitSlip" });
            ListPatientReports.Add(new ReportsModel { Name = "ใบ Request Order", NamespaceName = "MediTech.Reports.Operating.Patient.OrderRequest" });
            ListPatientReports.Add(new ReportsModel { Name = "ใบรับรองแพทย์", NamespaceName = "MediTech.Reports.Operating.Patient.MedicalCertificate" });
            ListPatientReports.Add(new ReportsModel { Name = "ใบรับรองแพทย์ 5 โรค", NamespaceName = "MediTech.Reports.Operating.Patient.MedicalCouncil5" });
            ListPatientReports.Add(new ReportsModel { Name = "ใบรับรองแพทย์ 5 โรค (ภาษาอังกฤษ)", NamespaceName = "MediTech.Reports.Operating.Patient.MedicalCertificateEng2" });
            ListPatientReports.Add(new ReportsModel { Name = "ใบรับรองแพทย์ (ภาษาอังกฤษ)", NamespaceName = "MediTech.Reports.Operating.Patient.MedicalCertificateEng" });
            ListPatientReports.Add(new ReportsModel { Name = "ใบรับรองแพทย์ 2 ส่วน", NamespaceName = "MediTech.Reports.Operating.Patient.MedicalCertificate2Parts" });
            ListPatientReports.Add(new ReportsModel { Name = "ใบรับรองแพทย์ 2 ส่วน (ภาษาอังกฤษ)", NamespaceName = "MediTech.Reports.Operating.Patient.MedicalCertificateEng2Parts" });
            ListPatientReports.Add(new ReportsModel { Name = "ใบรับรองรังสีแพทย์", NamespaceName = "MediTech.Reports.Operating.Patient.RadilogyCertificate" });
            ListPatientReports.Add(new ReportsModel { Name = "ใบรับรองแพทย์ที่อับอากาศ", NamespaceName = "MediTech.Reports.Operating.Patient.ConfinedSpaceCertificate1" });
            ListPatientReports.Add(new ReportsModel { Name = "ใบรับรองแพทย์ทำงานบนที่สูง", NamespaceName = "MediTech.Reports.Operating.Patient.WorkingHeightCertificate1" });
            ListPatientReports.Add(new ReportsModel { Name = "ใบรับรองแพทย์ทำงานบนที่สูง (ภาษาอังกฤษ)", NamespaceName = "MediTech.Reports.Operating.Patient.WorkingHeightCertificateEng1" });
            //ListPatientReports.Add(new ReportsModel { Name = "ใบรับรองแพทย์โควิด", NamespaceName = "MediTech.Reports.Operating.Patient.CovidRapidTestCertification" });
            ListPatientReports.Add(new ReportsModel { Name = "ใบรับรองแพทย์โควิด", NamespaceName = "MediTech.Reports.Operating.Patient.CertificateCovid" });
            ListPatientReports.Add(new ReportsModel { Name = "ใบรับรองแพทย์โควิดนอกสถานที่", NamespaceName = "MediTech.Reports.Operating.Patient.CovidRapidTestCertification2" });
            ListPatientReports.Add(new ReportsModel { Name = "ใบรับรองแพทย์กายภาพ", NamespaceName = "MediTech.Reports.Operating.Patient.MedicalCertificate" });
            ListPatientReports.Add(new ReportsModel { Name = "ใบรับรองแพทย์ สณ.11", NamespaceName = "MediTech.Reports.Operating.Patient.MedicalCouncil10" });
            ListPatientReports.Add(new ReportsModel { Name = "ใบรับรองแพทย์ 5 โรค (Mobile)", NamespaceName = "MediTech.Reports.Operating.Patient.MedicalCouncil5" });
            ListPatientReports.Add(new ReportsModel { Name = "ใบรับรองแพทย์ Fittnes", NamespaceName = "MediTech.Reports.Operating.Patient.MedicalFitness" });
            ListPatientReports.Add(new ReportsModel { Name = "ปริ้น Sticker", NamespaceName = "MediTech.Reports.Operating.Patient.PatientSticker" });
            ListPatientReports.Add(new ReportsModel { Name = "ปริ้น Sticker Large", NamespaceName = "MediTech.Reports.Operating.Patient.PatientLargSticker" });
            ListPatientReports.Add(new ReportsModel { Name = "ใบรับรองแพทย์ต่างด้าว", NamespaceName = "MediTech.Reports.Operating.Patient.Alien" });
            ListPatientReports.Add(new ReportsModel { Name = "รายงานตรวจPapSmear", NamespaceName = "MediTech.Reports.Operating.Checkup.Papsmear" });
           

            return ListPatientReports;
        }
    }
}
