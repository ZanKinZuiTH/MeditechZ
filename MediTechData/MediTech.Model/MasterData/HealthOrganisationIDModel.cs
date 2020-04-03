using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class HealthOrganisationIDModel
    {
        public int HealthOrganisationIDUID { get; set; }
        public int HealthOrganisationUID { get; set; }
        public string IDFormat { get; set; }
        public Nullable<int> IDLength { get; set; }
        public Nullable<int> NumberValue { get; set; }
        public Nullable<System.DateTime> LastRenumberDttm { get; set; }
        public Nullable<int> BLTYPUID { get; set; }
        public string BillType { get; set; }
        public Nullable<System.DateTime> ActiveFrom { get; set; }
        public Nullable<System.DateTime> ActiveTo { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
    }
}
