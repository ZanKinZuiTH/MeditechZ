using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class HealthOrganisationModel
    {
        public int HealthOrganisationUID { get; set; }
        public string Code { get; set; }
        public string IsStock { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string MobileNo { get; set; }
        public string FaxNo { get; set; }
        public string Email { get; set; }
        public int HOTYPUID { get; set; }
        public string HealthOrganisationType { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string AddressFull { get; set; }
        public Nullable<int> ProvinceUID { get; set; }
        public string Province { get; set; }
        public Nullable<int> AmphurUID { get; set; }
        public string Amphur { get; set; }
        public Nullable<int> DistrictUID { get; set; }
        public string District { get; set; }
        public string ZipCode { get; set; }
        public Nullable<bool> IsGenerateBillNumber { get; set; }
        public string IDFormat { get; set; }
        public Nullable<int> IDLength { get; set; }
        public Nullable<int> NumberValue { get; set; }
        public Nullable<System.DateTime> LastRenumberDttm { get; set; }
        public Nullable<System.DateTime> ActiveFrom { get; set; }
        public Nullable<System.DateTime> ActiveTo { get; set; }
        public string TINNo { get; set; }
        public string LicenseNo { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
        public byte[] LogoImage { get; set; }
        public List<HealthOrganisationIDModel> HealthOrganisationIDs { get; set; }
    }
}
