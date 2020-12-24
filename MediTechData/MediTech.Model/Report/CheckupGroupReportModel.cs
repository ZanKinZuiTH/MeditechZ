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

        public string BPSys { get; set; }

        public string BPDio { get; set; }
        public string Pulse { get; set; }
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

        public string PAR21 { get; set; } //Appearance
        public string D0080 { get; set; } //Color
        public string D0001 { get; set; } //Sp.Gr.
        public string E0080 { get; set; } //ph
        public string E0085 { get; set; } //Protein
        public string E0090 { get; set; } //Glucose
        public string E0047 { get; set; } //Ketne
        public string E0154 { get; set; } //Nitrites
        public string E0151 { get; set; } //Bilirubin
        public string E0150 { get; set; } //Urobilinogen
        public string E0153 { get; set; } //Leukocyte
        public string E0152 { get; set; } //Blood
        public string E0140 { get; set; } //Erythrocytes
        public string D0250 { get; set; }//Wbc ua
        public string D0260 { get; set; }//Rbc ua
        public string D0270 { get; set; }//EpithelialCells
        public string E0155 { get; set; }//Bacteria
        public string PAR124 { get; set; }//Toluene in urine
        public string PAR116 { get; set; }//Methanol
        public string PAR115 { get; set; } //Benzene
        public string PAR194 { get; set; } //Aluminium in blood

        public string PEXAM1 { get; set; } 
        public string PEXAM2 { get; set; }
        public string PEXAM3 { get; set; }
        public string PEXAM4 { get; set; }
        public string PEXAM5 { get; set; }
        public string PEXAM6 { get; set;}
        public string PEXAM7 { get; set; }
        public string PEXAM8 { get; set; }
        public string PEXAM9 { get; set; }
        public string PEXAM10 { get; set; }
        public string PEXAM11 { get; set; }
        public string PEXAM12 { get; set; }
        public string PEXAM13 { get; set; }
        public string PEXAM14 { get; set; }
        
        public string MUCS1 { get; set; }
        public string MUCS2 { get; set; }

        public string PAR117 { get; set; } //actone

        public string PAR195 { get; set; } //Styrene
        public string PAR50 { get; set; } //sgot
        public string PAR51 { get; set; } //sgpt

        public string C0320 { get; set; } //uric

        public string C0180 { get; set; } //fbs
        public string PAR7 { get; set; } //HbA1c

        public string PAR118 { get; set; } //Hexane

        public string PAR71 { get; set; } //WBC stool
        public string PAR72 { get; set; } //RBC stool
        public string PAR70 { get; set; } //Appearance stool
        public string PAR73 { get; set; } //ova 
        public string PAR69 { get; set; } //stool color

        public string PAR187 { get; set; } //psa
        public string PAR41 { get; set; } //ca125

        public string PAR114 { get; set; } //ca19-9
        public string PAR189 { get; set; } //stool culture
        public string PAR39 { get; set; } // afp
        public string PAR186 { get; set; } // afcon
        public string PAR40 { get; set; } //cea
        public string PAR185 { get; set; } //cea text
        public string PAR35 { get; set; } //HBs Ag
        public string PAR34 { get; set; } //COI of HBs Ag
        public string PAR42 { get; set; } //HBs Ab 

        public string PAR121 { get; set; } //COI of HBs Ab 

        public string SPIRO3 { get; set; } //FVC (%Pred.)
        public string SPIRO6 { get; set; } //FEV1 (%Pred.)
        public string SPIRO9 { get; set; } //FEV1/FVC % (%Pred.)

        public string PAR190 { get; set; } //Anti hva 
        public string PAR191 { get; set; } // coi Anti hav
        public string PAR192 { get; set; } //Anti Hav เชื้อ
        public string Conclusion { get; set; }
        public string ResultStatus { get; set; }
        public string Radiologist { get; set; }

    }
}
