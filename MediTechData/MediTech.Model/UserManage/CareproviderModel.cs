using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class CareproviderModel
    {
        public int CareproviderUID { get; set; }
        public string Code { get; set; }
        public Nullable<int> TITLEUID { get; set; }
        public string TitleDesc { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public Nullable<int> SEXXXUID { get; set; }
        public string SexDesc { get; set; }
        public string EnglishName { get; set; }
        public string ImgPath { get; set; }
        public string LicenseNo { get; set; }
        public Nullable<System.DateTime> LicenseIssueDttm { get; set; }
        public Nullable<System.DateTime> LicenseExpiryDttm { get; set; }
        public Nullable<System.DateTime> DOBDttm { get; set; }
        public bool IsDoctor { get; set; }
        public bool IsRadiologist { get; set; }
        public bool IsAdminRadiologist { get; set; }
        public bool IsAdminRadread { get; set; }
        public bool IsRDUStaff { get; set; }
        public string Tel { get; set; }
        public string Email { get; set; }
        public string LineID { get; set; }
        public Nullable<System.DateTime> ActiveFrom { get; set; }
        public Nullable<System.DateTime> ActiveTo { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
        public LoginModel loginModel { get; set; }
        public int VISTYUID { get; set; }
        public int? CPTYPUID { get; set; }
        public string Qualification { get; set; }
    }
}
