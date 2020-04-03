using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model.Report
{
    public class PatientResultRadiology
    {
        public string RequestItemName { get; set; }
        public string Modality { get; set; }
        public long ResultUID { get; set; }
        public long PatientUID { get; set; }
        public long PatientVisitUID { get; set; }
        public long RequestUID { get; set; }
        public long RequestDetailUID { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PatientName { get; set; }
        public DateTime? DOBDttm { get; set; }
        public string MobilePhone { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public string HN { get; set; }
        public string OtherID{ get; set; }
        public string EN { get; set; }
        public string Address { get; set; }
        public string Location { get; set; }
        public DateTime RequestedDttm { get; set; }
        public DateTime? ResultEnteredDttm { get; set; }
        public string ResultStatus { get; set; }
        public string Doctor { get; set; }
        public string PayorName { get; set; }
        public string ThaiResult { get; set; }
        public string ResultValue { get; set; }
        public string ResultHtml { get; set; }
        public string OgranastionAddress { get; set; }
        public string No { get; set; }
        public string EmployeeID { get; set; }
        public string Company { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
        public DateTime? CheckupDttm { get; set; }

    }
}
