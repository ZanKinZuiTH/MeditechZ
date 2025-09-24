using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class SeriesCopyToModel
    {
        public string PatientID { get; set; }
        public string PatientName { get; set; }
        public string OtherID { get; set; }
        public string StudyID { get; set; }
        public DateTime StudyDate { get; set; }
        public string SeriesInstanceUID { get; set; }
        public int SeriesNumber { get; set; }
        public System.Nullable<DateTime> SeriesDate { get; set; }
        public string SeriesTime { get; set; }
        public string SeriesDescription { get; set; }
        public string Modality { get; set; }
        public List<InstancesCopyToModel> Instances { get; set; }
        public int No { get; set; }
    }
}
