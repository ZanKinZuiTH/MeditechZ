using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class LoginModel
    {
        public string LoginName { get; set; }
        public int LoginUID { get; set; }
        public string Password { get; set; }
        public int RoleUID { get; set; }
        public string RoleName { get; set; }
        public bool IsAdmin { get; set; }
        public string Type { get; set; }
        public System.Nullable<DateTime> LastPasswordModifiredDTTM { get; set; }
        public System.Nullable<DateTime> LastAccessedDttm { get; set; }
        public System.Nullable<DateTime> LastAccessedWebDttm { get; set; }
        public System.Nullable<System.DateTime> ActiveFrom { get; set; }
        public System.Nullable<System.DateTime> ActiveTo { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
        public string OwnerOrganisationName { get; set; }
        public List<RoleProfileModel> RoleProfiles { get; set; }

    }

    public class SecurityPermissionModel
    {
        public int PageViewUID { get; set; }
        public string PageViewName { get; set; }
        public string LocalName { get; set; }
        public string ClassName { get; set; }
        public string NameSpace { get; set; }
        public string ControllerName { get; set; }
        public string ControllerAction { get; set; }
        public int PageViewPermissionUID { get; set; }
        public int RoleUID { get; set; }
        public int PermissionUID { get; set; }
        public string Permission { get; set; }
    }
}
