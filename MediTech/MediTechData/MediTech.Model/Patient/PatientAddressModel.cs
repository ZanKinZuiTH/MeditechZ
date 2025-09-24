using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class PatientAddressModel
    {
        public long PatientAddressUID { get; set; }
        public long PatientUID { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        public string Line4 { get; set; }
        public Nullable<int> ADTYPUID { get; set; }
        public string AddressType { get; set; }
        public Nullable<int> DistrictUID { get; set; }
        public string DistrictName { get; set; }
        public Nullable<int> AmphurUID { get; set; }
        public string AmphurName { get; set; }
        public Nullable<int> ProvinceUID { get; set; }
        public string ProvinceName { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public int MUser { get; set; }
        public int CUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public System.DateTime CWhen { get; set; }
        public string StatusFlag { get; set; }
    }
}
