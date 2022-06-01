using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class InsuranceCompanyModel
    {
        public int InsuranceCompanyUID { get; set; }
        public string Code { get; set; }
        public string CompanyName { get; set; }
        public string Description { get; set; }
        public int? CMPTPUID { get; set; }
        public DateTime? ActiveFrom { get; set; }
        public DateTime? ActiveTo { get; set; }
        public string Comments { get; set; }
        public int? OldPayorDetailUID { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
        public byte[] TIMESTAMP { get; set; }
    }
}
