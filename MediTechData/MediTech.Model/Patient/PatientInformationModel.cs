using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class PatientInformationModel
    {
        public long PatientUID { get; set; }
        public string Title { get; set; }
        public System.Nullable<int> TITLEUID { get; set; }
        public string FirstName { get; set; }
        public string MiddelName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public string PatientName { get; set; }
        public string Gender { get; set; }
        public System.Nullable<int> SEXXXUID { get; set; }
        public System.Nullable<int> SPOKLUID { get; set; }
        public System.Nullable<int> NATNLUID { get; set; }
        public System.Nullable<int> BLOODUID { get; set; }
        public string BloodGroup { get; set; }
        public System.Nullable<int> MARRYUID { get; set; }

        public System.Nullable<int> OCCUPUID { get; set; }
        public bool IsVIP { get; set; }
        public int? VIPTPUID { get; set; }
        public string VIPType { get; set; }
        public DateTime? VIPActiveFrom { get; set; }
        public DateTime? VIPActiveTo { get; set; }
        public string PatientID { get; set; }
        public string EmployeeID { get; set; }
        public string PatientOtherID { get; set; }
        public string IDPassport { get; set; }
        public bool DOBComputed { get; set; }
        public System.Nullable<int> RELGNUID { get; set; }
        public DateTime? LastVisitDttm { get; set; }
        public DateTime? BirthDttm { get; set; }
        public DateTime? RegisterDate { get; set; }
        public string BirthDttmString
        {
            get
            {
                return BirthDttm != null ? BirthDttm.Value.ToString("dd/MM/yyyy") : "";
            }
        }
        public string AgeString { get; set; }
        public string Age { get; set; }
        public string NationalID { get; set; }
        public string Email { get; set; }
        public string IDLine { get; set; }
        public System.Nullable<long> PatientAddressUID { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        public System.Nullable<int> ProvinceUID { get; set; }
        public System.Nullable<int> AmphurUID { get; set; }
        public System.Nullable<int> DistrictUID { get; set; }
        public string ZipCode { get; set; }
        public string MobilePhone { get; set; }
        public string SecondPhone { get; set; }
        public byte[] PatientImage { get; set; }

        public string PatientAddress { get; set; }
        public double? Weight { get; set; }
        public double? Height { get; set; }
        public double? BMI { get; set; }
        public double? BPSys { get; set; }
        public double? BPDio { get; set; }
        public double? Pulse { get; set; }

        public double? WaistCircumference { get; set; }

        public int UserUID { get; set; }

        public int? OwnerOrganisationUID { get; set; }

        public string Department { get; set; }
        public string Position { get; set; }
        public string IsIdentityOnBLIFE { get; set; }
    }
}
