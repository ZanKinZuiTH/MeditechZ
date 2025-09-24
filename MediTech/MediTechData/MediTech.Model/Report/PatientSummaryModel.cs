using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model.Report
{
    public class PatientSummaryModel
    {
        public int Female { get; set; }
        public int Male { get; set; }
        public int Unknown { get; set; }
        public int Child { get; set; }
        public int Adut { get; set; }
        public int age0_4 { get; set; }
        public int age5_9 { get; set; }
        public int age10_14 { get; set; }
        public int age15_19 { get; set; }
        public int age20_24 { get; set; }
        public int age25_29 { get; set; }
        public int age30_34 { get; set; }
        public int age35_39 { get; set; }
        public int age40_44 { get; set; }
        public int age45_49 { get; set; }
        public int age50_54 { get; set; }
        public int age55_59 { get; set; }
        public int age60_64 { get; set; }
        public int age65_69 { get; set; }
        public int age70 { get; set; }
        public int Thai { get; set; }
        public int Foreign { get; set; }
        public int NoInputNation { get; set; }
        public int ThaiOld { get; set; }
        public int ThaiNew { get; set; }
        public int ForeignOld { get; set; }
        public int ForeignNew { get; set; }
        public int NoInputNationNew { get; set; }
        public int NoInputNationOld { get; set; }
        public int Counsult { get; set; }
        public int Repeat { get; set; }
        public string Year { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; }
    }
}
