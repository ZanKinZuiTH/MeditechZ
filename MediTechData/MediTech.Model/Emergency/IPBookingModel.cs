using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class IPBookingModel: PatientVisitModel
    {
        public long IPBookingUID { get; set; }
        public long PatientUID { get; set; }
        public long? PatientVisitUID { get; set; }
        public int? LocationUID { get; set; }
        public int? SpecialityUID { get; set; }
        public int CareproviderUID { set; get; }
        public int? BedUID { get; set; }
        public DateTime AdmissionDttm { get; set; }
        public DateTime ExpectedDischargeDttm { get; set; }
        public int? ExpectedLengthofStay { get; set; }
        public DateTime BookedDttm { get; set; }
        public int BKSTSUID { get; set; }
        public int VISTYUID { get; set; }
        public int BKTYPUID { get; set; }
        public int? BDCATUID { get; set; }
        public string ReferredBy { get; set; }
        public int? ReferredByUID { get; set;  }
        public int? RequestedByLocationUID { get; set; }
        public int? RequestedByUID { get; set; }
        public int? CANRSUID { get; set; }
        public int? CancelledBy { get; set; }
        public DateTime? CancelledDttm { get; set; }
        public string Comments { get; set; }
        public int OwnerOrganisationUID { get; set; }
        public int CUser { get; set; }
        public DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
        public byte[] Timestamp { get; set; }
        public string BedName { get; set; }
        public string BedBookingRequest { get; set; }
        public string BookingStatus { get; set; }
        public string LocationName { get; set; }
        public string RequestedLocationName { get; set; }
        public string SpecialityName { get; set; }
        public string Ward { get; set; }
    }
}
