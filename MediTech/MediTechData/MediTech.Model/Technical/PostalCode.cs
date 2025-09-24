using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class PostalCode
    {
        public string ZipCode { get; set; }
        public int DistrictUID { get; set; }
        public int AmphurUID { get; set; }
        public int ProvinceUID { get; set; }
        public string District { get; set; }
        public string Amphur { get; set; }
        public string Province { get; set; }
    }
}
