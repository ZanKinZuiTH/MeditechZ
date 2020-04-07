using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class PatientBillModel
    {
        public long PatientBillUID { get; set; }
        public long PatientUID { get; set; }
        public long PatientVisitUID { get; set; }
        public string PatientID { get; set; }
        public string PatientName { get; set; }
        public string PatientAddress { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public DateTime DOBDttm { get; set; }
        public string SecondPhone { get; set; }
        public string MobilePhone { get; set; }
        public string VisitID { get; set; }
        public DateTime VisitDttm { get; set; }
        public string VisitType { get; set; }
        public Nullable<System.DateTime> BillGeneratedDttm { get; set; }
        public Nullable<double> TotalAmount { get; set; }
        public Nullable<double> DiscountAmount { get; set; }
        public Nullable<double> NetAmount { get; set; }
        public string BillNumber { get; set; }
        public string IsRefund { get; set; }
        public double PaidAmount { get; set; }
        public Nullable<double> ChangeAmount { get; set; }
        public Nullable<long> PatientVisitPayorUID { get; set; }
        public Nullable<int> PayorDetailUID { get; set; }
        public Nullable<int> PayorAgreementUID { get; set; }
        public string PayorName { get; set; }
        public string PayorTINNo { get; set; }
        public string PayorAgreement { get; set; }
        public string BillType { get; set; }
        public string CancelReason { get; set; }
        public Nullable<System.DateTime> CancelledDttm { get; set; }
        public bool IsCancel { get; set; }
        public string Comments { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
        public int OwnerOrganisationUID { get; set; }
        public string OrganisationTaxID { get; set; }
        public string OrganisationCode { get; set; }
        public string OrganisationName { get; set; }
        public string OrganisationAddress { get; set; }
        public string OperationBy { get; set; }
        public List<PatientBilledItemModel> PatientBilledItems {get; set;}
    }
}
