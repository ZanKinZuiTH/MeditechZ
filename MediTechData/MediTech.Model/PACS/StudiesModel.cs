using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class StudiesModel
    {
        public string StudyInstanceUID { get; set; }
        public string PatientID { get; set; }
        public string PatientName { get; set; }
        public string OtherID { get; set; }
        public string PatientComments { get; set; }
        public string PatientBirthDate { get; set; }
        public string PatientSex { get; set; }
        public string StudyDescription { get; set; }
        public string StudyID { get; set; }
        public DateTime StudyDate { get; set; }
        public string StudyTime { get; set; }
        public string AccessionNumber { get; set; }
        public string InstitutionName { get; set; }
        public string ModalitiesInStudy { get; set; }
        public string BodyPartsInStudy { get; set; }
        public int NumberOfStudyRelatedSeries { get; set; }
        public int NumberOfStudyRelatedInstances { get; set; }
        public string SpecificCharacterSet { get; set; }
        public string PatientAge { get; set; }
        public DateTime Edited { get; set; }
        public int ReportStatus { get; set; }

        public List<SeriesModel> Series { get; set; }
    }
}
