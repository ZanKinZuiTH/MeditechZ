using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class ResultRadiologyModel
    {
        public virtual long ResultUID { get; set; }
        public virtual long ResultRadiologyUID { get; set; }
        public virtual long RequestDetailUID { get; set; }
        public virtual long PatientUID { get; set; }
        public virtual long PatientVisitUID { get; set; }
        public string PatientID { get; set; }
        public string AccessionNumber { get; set; }
        public string Modality { get; set; }
        public string RequestItemName { get; set; }
        public string RequestItemCode { get; set; }
        public string NavigationImage { get; set; }
        public virtual System.Nullable<System.DateTime> ResultEnteredDttm { get; set; }
        public virtual string Comments { get; set; }
        public virtual System.Nullable<int> RadiologistUID { get; set; }
        public string Radiologist { get; set; }
        public virtual System.Nullable<int> ResultedByUID { get; set; }
        public virtual string ResultStatus { get; set; }
        public virtual System.Nullable<int> RABSTSUID { get; set; }
        public virtual System.Nullable<int> ORDSTUID { get; set; }
        public virtual string HasHistory { get; set; }
        public virtual System.Nullable<int> Version { get; set; }
        public virtual string PlainText { get; set; }
        public virtual string Value { get; set; }
        public virtual bool? IsCaseStudy { get; set; }
    }
}
