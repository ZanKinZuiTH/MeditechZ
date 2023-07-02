using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model.Report
{
    public class PatientSummaryDataModel
    {
        public string HN { get; set; }
        public string PatientName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Age { get; set; }
        public string Province { get; set; }
        public string Amphur { get; set; }
        public string District { get; set; }
        public string MobilePhone { get; set; }
        public string SecondPhone { get; set; }

        public string VisitID { get; set; }
        public DateTime StartDateTime { get; set; }

        private string _StartDate;

        public string StartDate
        {
            get { return StartDateTime.ToString("dd/MM/yyyy"); }
            set { _StartDate = value; }
        }

        private string _StartTime;

        public string StartTime
        {
            get { return StartDateTime.ToString("HH:mm:ss"); }
            set { _StartTime = value; }
        }
        public string VisitStatus { get; set; }
        public string VisitType { get; set; }
        public double? NetAmountVisit { get; set; }
        public string Doctor { get; set; }
        public string ICD10 { get; set; }
        public string PayorName { get; set; }

        public string Allergy { get; set; }
        public DateTime? AppointmentDttm { get; set; }
        public string PayorAgreement { get; set; }
        public string OrderSet { get; set; }

        public string ItemName { get; set; }
        public double? Amount { get; set; }
        public double? ItemMutiplier { get; set; }
        public double? Discount { get; set; }
        public double? NetAmount { get; set; }
        public double? ItemCost { get; set; }

        public string NoteCashier { get; set; }
        public string CancelBillReason { get; set; }
        public string TypeOrder { get; set; }
        public string OrderComments { get; set; }

    }
}
