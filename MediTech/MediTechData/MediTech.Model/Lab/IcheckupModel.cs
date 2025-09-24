using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class IcheckupModel
    {
        public List<RequestDetailSpecimenModel> LabData { get; set; }
        public PatientVisitModel PatientData { get; set; }


    }

}