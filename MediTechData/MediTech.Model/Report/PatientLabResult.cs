using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model.Report
{
    public class PatientLabResult
    {

        public long ResultUID { get; set; }
        public long PatientUID { get; set; }
        public long PatientVisitUID { get; set; }
        public long RequestUID { get; set; }
        public long ResultComponentUID { get; set; }
        public long RequestDetailUID { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PatientName { get; set; }
        public DateTime? DOBDttm { get; set; }
        public string MobilePhone { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public string PatientID { get; set; }
        public string VisitID { get; set; }
        public string RequestBy { get; set; }
        public string ReviewedBy { get; set; }
        public DateTime RequestedDttm { get; set; }
        public DateTime? ResultEnteredDttm { get; set; }
        public string RequestNumber { get; set; }
        public string RequestItemName { get; set; }
        public string PrintGroup { get; set; }
        public string ResultItemName { get; set; }
        public int PrintOrder { get; set; }
        public string ResultValue { get; set; }
        public string ResultValueType { get; set; }
        public string Abnormal { get; set; }
        public string Unit { get; set; }
        public string ReferenceRange { get; set; }
        public string OrganisationName { get; set; }
        public string OrganisationCode { get; set; }
        public string OrganisationAddress { get; set; }
        public string IsConfidential { get; set; }
        public string ResultEnteredBy { get; set; }
        public string ResultQCBy { get; set; }
    }
}
