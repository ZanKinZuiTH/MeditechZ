using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class SeriesModel
    {
        public string StudyInstanceUID { get; set; }
        public string SeriesInstanceUID { get; set; }
        public string DisplayInstanceUID { get; set; }
        public int SeriesNumber { get; set; }
        public System.Nullable<DateTime> SeriesDate { get; set; }
        public string SeriesTime { get; set; }
        public string SeriesDescription { get; set; }
        public string Modality { get; set; }
        public string BodypartExamined { get; set; }
        public int NumberOfSeriesRelatedInstances { get; set; }
        public string SpecificCharacterSet { get; set; }
        public System.Nullable<DateTime> Edited { get; set; }
        public List<InstancesModel> Instances { get; set; }
        public byte[] ImageDisplay { get; set; }
        public List<ImageSerires> ImageSeries { get; set; }
    }

    public class ImageSerires
    {
        public byte[] ImageSources { get; set; }
    }
}
