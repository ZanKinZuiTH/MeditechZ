using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model.Report
{
    public class CheckupGroupReportModel
    {
        public int No { get; set; }
        public string EmployeeID { get; set; }
        public string PatientID { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public string BMIValue { get; set; }
        public string AUDIO1 { get; set; }
        public string AUDIO2 { get; set; }
        public string AUDIO3 { get; set; }
        public string AUDIO4 { get; set; }
        public string AUDIO5 { get; set; }
        public string AUDIO6 { get; set; }
        public string AUDIO7 { get; set; }
        public string AUDIO8 { get; set; }
        public string AUDIO9 { get; set; }
        public string AUDIO10 { get; set; }
        public string AUDIO11 { get; set; }
        public string AUDIO12 { get; set; }
        public string AUDIO13 { get; set; }
        public string AUDIO14 { get; set; }

        public string AUDIO15 { get; set; }
        public string AUDIO16 { get; set; }

        public string TIMUS19 { get; set; }

        public string TIMUS20 { get; set; }

        public string TIMUS21 { get; set; }
        public string TIMUS22 { get; set; }
        public string TIMUS23 { get; set; }
        public string TIMUS24 { get; set; }

        public string C0130 { get; set; }

        public string C0140 { get; set; }
        public string C0159 { get; set; }
        public string C0150 { get; set; }

        public string C0070 { get; set; }
        public string PAR27 { get; set; }

        public string A0001 { get; set; } //hb
        public string A0020 { get; set; } //Hct
        public string A0030 { get; set; } //MCH
        public string A0035 { get; set; } //MCHC
        public string A0025 { get; set; } //MCV
        public string PAR1 { get; set; } //RDW
        public string A0428 { get; set; } //RBC
        public string PAR13 { get; set; } //RBC Morphology

        public string A0006 { get; set; } //wbc
        public string A0040 { get; set; } //Neutrophi
        public string A0060 { get; set; } // Monocyte
        public string A0050 { get; set; } // Lymphocyt 
        public string A0070 { get; set; } // Eosinophil 
        public string A0080 { get; set; } // Basophil  
        public string A0010 { get; set; } 
        public string Conclusion { get; set; }
        public string ResultStatus { get; set; }


    }
}
