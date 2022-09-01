using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class PayorDetailModel
    {
        public int PayorDetailUID { get; set; }
        public string Code { get; set; }
        public string PayorName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public Nullable<int> DistrictUID { get; set; }
        public Nullable<int> AmphurUID { get; set; }
        public Nullable<int> ProvinceUID { get; set; }
        public string ZipCode { get; set; }
        public string AddressFull { get; set; }
        public string ContactPersonName { get; set; }
        public string FaxNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Note { get; set; }
        public Nullable<int> CRDTRMUID { get; set; }
        public string PaymentTerms { get; set; }

        public Nullable<int> PYRACATUID { get; set; }
        public string PayorCategory { get; set; }

        public DateTime? ActiveFrom { get; set; }
        public DateTime? ActiveTo { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
        public Nullable<bool> IsGenerateBillNumber { get; set; }
        public string IDFormat { get; set; }
        public Nullable<int> IDLength { get; set; }
        public Nullable<int> NumberValue { get; set; }
        public int? InsuranceCompanyUID { get; set; }
        public int? OldPayorDetailUID { get; set; }
        public string GovernmentNo { get; set; }
        public List<PayorAgreementModel> PayorAgrrements { get; set; }
    }
}
