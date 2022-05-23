using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class RoleProfileModel
    {
        public int RoleProfileUID { get; set; }
        public int RoleUID { get; set; }
        public string RoleName { get; set; }
        public int LoginUID { get; set; }
        public int LocationUID { get; set; }
        public string LocationName { get; set; }
        public int HealthOrganisationUID { get; set; }
        public string HealthOrganisationName { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
    }
}
