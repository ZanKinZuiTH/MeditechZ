using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class PatientResultCheckupModel : PatientResultComponentModel
    {
        public string Conclusion { get; set; }
        public string CheckupResultStatus { get; set; }
        public string Radiologist { get; set; }
    }
}
