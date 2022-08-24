using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class ItemServiceModel
    {
        public DateTime? ActiveFrom { get; set; }
        public DateTime? ActiveTo { get; set; }
        public string Code { get; set; }
        public string  Manufacturer { get; set; }
        public int ItemUID { get; set; }
        public string Name { get; set; }
    }
}
