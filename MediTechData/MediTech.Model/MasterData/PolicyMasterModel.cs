using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class PolicyMasterModel
    {
        public int PolicyMasterUID { get; set; }
        public string Code { get; set; }
        public string PolicyName { get; set; }
        public string Description { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
        public byte[] TIMESTAMP { get; set; }
        public int? AGTYPUID { get; set; }
        public string AgreementType { get; set; }
    }
}
