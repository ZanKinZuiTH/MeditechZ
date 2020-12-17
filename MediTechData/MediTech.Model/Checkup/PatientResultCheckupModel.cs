using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class PatientResultCheckupModel : PatientResultComponentModel
    {
        public string CheckupDescription { get; set; }
        public string CheckupRecommend { get; set; }
        public string CheckupResultStatus { get; set; }
    }
}
