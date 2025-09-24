using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class RoleModel
    {
        public int RoleUID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }

        public List<RoleViewPermissionModel> RoleView { get; set; }

        public List<RoleReportPermissionModel> RoleReport { get; set; }
    }

    public class RoleViewPermissionModel
    {
        public int RoleViewPermissionUID { get; set; }
        public int RoleUID { get; set; }
        public int PageViewPermissionUID { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
    }
}
