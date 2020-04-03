using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model.Report
{
    public class RadiologyRDUReviewModel
    {
        public string PatientID { get; set; }
        public string PatientName { get; set; }
        public string RDUStaff { get; set; }
        public string ResultStatus { get; set; }
    }
}
