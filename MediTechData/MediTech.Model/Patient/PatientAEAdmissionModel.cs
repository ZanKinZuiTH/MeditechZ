using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class PatientAEAdmissionModel
    {
        public long Uid { get; set; }
        public long PatientUid { get; set; }
        public long PatientVisitUid { get; set; }
        public int ARRMDUID { get; set; }
        public int? RELTNUID { get; set; }
        public int? ESCTPUID { get; set; }
        public string VehicleNumber { get; set; }
        public int? EMGTPUID { get; set; }
        public int? EMGCDUID { get; set; }
        public int? ProblemUID { get; set; }
        public string InjuryReason { get; set; }
        public string EmergencyExamDetail { get; set; }
        public string IsDead { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        public string Line4 { get; set; }
        public int ADTYPUID { get; set; }
        public int? DistrictUID { get; set; }
        public int? AmphurUID { get; set; }
        public int? ProvinceUID { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string Comments { get; set; }
        public DateTime? EventOccuredDttm { get; set; }
        public int? LocationUid { get; set; }
        public int? IdentifyingUid { get; set; }
        public string IdentifyingType { get; set; }
        public int Cuser { get; set; }
        public DateTime Cwhen { get; set; }
        public int Muser { get; set; }
        public DateTime Mwhen { get; set; }
        public string StatusFlag { get; set; }
        public byte[] Timestamp { get; set; }
        public int? BedUID { get; set; }
        public int? CareproviderUID { get; set; }
        public System.Nullable<int> TITLEUID { get; set; }
        public string FirstName { get; set; }
        public string MiddelName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public string PatientID { get; set; }
        public System.Nullable<int> SEXXXUID { get; set; }
        public System.Nullable<int> SPOKLUID { get; set; }
        public System.Nullable<int> NATNLUID { get; set; }
        public System.Nullable<int> BLOODUID { get; set; }
        public DateTime? BirthDttm { get; set; }
        public bool DOBComputed { get; set; }
    }
}
