using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model.Report
{
    public class PatientSumByAreaModel
    {
        public string ProvinceName { get; set; }
        public string AmphurName { get; set; }
        public string DistrictName { get; set; }

        public int OldPatient { get; set; }
        public int NewPatient { get; set; }
        public List<DetailAreaModel> ListMooNo { get; set; }
        public int PatientCount { get; set; }


        public class DetailAreaModel
        {
            public string DistrictName { get; set; }
            public string Moono { get; set; }
            public int OldPatient { get; set; }
            public int NewPatient { get; set; }

            public int PatientCount { get; set; }
        }
    }


}
