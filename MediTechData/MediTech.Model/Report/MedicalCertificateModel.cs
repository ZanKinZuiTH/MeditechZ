using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model.Report
{
   public class MedicalCertificateModel
    {
        public long No { get; set; }
        public string PatientID { get; set; }
        public string PatientName { get; set; }
        public string IDCard { get; set; }
        public string PassportID { get; set; }
        public string Gender { get; set; }
        public double? Weight { get; set; }
        public double? Height { get; set; }
        public double? BMI { get; set; }
        public double? Pulse { get; set; }
        public double? BPSys { get; set; }
        public double? BPDio { get; set; }
        public double? Temp { get; set; }
        public DateTime? VitalSignRecordDttm { get; set; }
        public string AgeString { get; set; }
        public string AgeYear { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Doctor { get; set; }
        public string DoctorEngName { get; set; }
        public Nullable<int> CareProviderUID { get; set; }
        public string Comments { get; set; }
        public long PatientVisitUID { get; set; }
        public string VisitID { get; set; }
        public DateTime? VisitDate { get; set; }
        public DateTime? strVisitData { get; set; }
        public string PatientAddress { get; set; }
        public string DoctorLicenseNo { get; set; }
        public string MobilePhone { get; set; }
        public string PatientPayor { get; set; }
        public string PatientEmail { get; set; }
        public List<string> Detail{ get; set; }
        public string CompanyName { get; set; }

    }
}
