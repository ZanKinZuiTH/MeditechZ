using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class CareproviderLocationModel
    {
        public int CareproviderLocationUID { get; set; }
        public int CareproviderUID { get; set; }
        public int LocationUID { get; set; }
        public int HealthOrganisationUID { get; set; }
        public string CareproviderName { get; set; }
        public string LocationName { get; set; }
        public string HealthOrganisationName { get; set; }
        public DateTime ActiveFrom { get; set; }
        public DateTime? ActiveTo { get; set; }
    }
}
