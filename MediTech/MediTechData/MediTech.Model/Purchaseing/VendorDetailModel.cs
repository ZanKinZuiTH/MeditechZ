using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class VendorDetailModel
    {
        public int VendorDetailUID { get; set; }
        public string CompanyName { get; set; }
        public string ContactPerson { get; set; }
        public string MobileNo { get; set; }
        public string FaxNo { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string AddressFull { get; set; }
        public int MNFTPUID { get; set; }
        public string TINNo { get; set; }
        public string VendorType { get; set; }
        public Nullable<int> ProvinceUID { get; set; }
        public Nullable<int> AmphurUID { get; set; }
        public Nullable<int> DistrictUID { get; set; }
        public string ZipCode { get; set; }
        public Nullable<System.DateTime> ActiveFrom { get; set; }
        public Nullable<System.DateTime> ActiveTo { get; set; }
        public Nullable<int> CUser { get; set; }
        public Nullable<System.DateTime> CWhen { get; set; }
        public Nullable<int> MUser { get; set; }
        public Nullable<System.DateTime> MWhen { get; set; }
        public string StatusFlag { get; set; }
    }
}
