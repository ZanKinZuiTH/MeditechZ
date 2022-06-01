using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class BedStatusModel
    {
        public int LocationUID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int LOTYPUID { get; set; }
        public string LocationType { get; set; }
        public Nullable<int> ParentLocationUID { get; set; }
        public string IsRegistrationAllowed { get; set; }
        public Nullable<int> LCTSTUID { get; set; }
        public string LocationStatus { get; set; }
        public Nullable<int> EMRZONUID { get; set; }
        public string EmergencyZone { get; set; }
        public string IsCanOrder { get; set; }
        public string IsTemporaryBed { get; set; }
        public string IsEmergency { get; set; }
        public string PhoneNumber { get; set; }
        public Nullable<int> DisplayOrder { get; set; }
        public Nullable<System.DateTime> ActiveFrom { get; set; }
        public Nullable<System.DateTime> ActiveTo { get; set; }
        public Nullable<int> OwnerOrganisationUID { get; set; }
        public string OwnerOrganisation { get; set; }
        public string ParentLocationName { get; set; }
        public int BedUID { get; set; }
        public string BedIsUse { get; set; }
        public bool Isused { get; set; }
        public string EMGTPUID { get; set; }
        public string EMGCDUID { get; set; }
        public string Level { get; set; }
        public DateTime? EmergencyVisitDate { get; set; }
        public DateTime? AdmissionDate { get; set; }
        public DateTime? ExpDischargeDate { get; set; }

        public long PatientUID { get; set; }
        public string PatientName { get; set; }
        public string Gender { get; set; }

        public string PatientID { get; set; }
        public string AgeString { get; set; }
        public string CareProviderName { get; set; }
        public long? PatientVisitUID { get; set; }
        public long? AEAdmissionUID { get; set; }
        public long? AdmissionEventUID { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
    }
}
