using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class PatientEmergencyModel : PatientInformationModel
    {
        public int No { get; set; }
        public string Status { get; set; }
        public string Severe { get; set; }
    }
}
