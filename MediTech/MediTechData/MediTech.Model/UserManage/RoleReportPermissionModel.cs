using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class RoleReportPermissionModel : ReportsModel
    {
        public int RoleUID { get; set; }
        public int ReportPermissionUID { get; set; }
    }
}
