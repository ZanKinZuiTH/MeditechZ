using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model.Report
{
    public class PatientWellnessModel
    {
        public PatientVisitModel PatientInfomation { get; set; }
        public List<PatientResultComponentModel> MobileResult { get; set; }
        public List<ResultRadiologyModel> Radiology { get; set; }
        public List<PatientResultComponentModel> LabCompare { get; set; }
        public List<CheckupGroupResultModel> GroupResult { get; set; }

    }
}
