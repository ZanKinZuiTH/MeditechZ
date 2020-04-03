using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class InstancesModel
    {
        public string SOPInstanceUID { get; set; }
        public string SeriesInstanceUID { get; set; }
        public string StudyInstanceUID { get; set; }
        public string SOPClassUID { get; set; }
        public string TransferSyntaxUID { get; set; }
        public string PatientID { get; set; }
        public string PatientName { get; set; }
        public string PatientComments { get; set; }
        public string PatientBirthDate { get; set; }
        public string PatientSex { get; set; }
        public string StudyDescription { get; set; }
        public string StudyID { get; set; }
        public string StudyDate { get; set; }
        public string StudyTime { get; set; }
        public string AccessionNumber { get; set; }
        public string InstitutionName { get; set; }
        public int SeriesNumber { get; set; }
        public DateTime? SeriesDate { get; set; }
        public string SeriesTime { get; set; }
        public string SeriesDescription { get; set; }
        public string Modality { get; set; }
        public string BodypartExamined { get; set; }
        public int? InstanceNumber { get; set; }
        public int? NumberOfFrames { get; set; }
        public string PixelSpacing { get; set; }
        public string PixelAspectRatio { get; set; }
        public string SliceThickness { get; set; }
        public string SpacingBetweenSlices { get; set; }
        public string SliceLocation { get; set; }
        public string PatientOrientation { get; set; }
        public string ImagePositionPatient { get; set; }
        public string ImageOrientationPatient { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public int? RescaleIntercept { get; set; }
        public int? RescaleSlope { get; set; }
        public string RescaleType { get; set; }
        public string WindowWidth { get; set; }
        public string WindowCenter { get; set; }
        public int? SamplesPerPixel { get; set; }
        public int? PlanarConfiguration { get; set; }
        public int? BitsAllocated { get; set; }
        public int? BitsStored { get; set; }
        public int? HighBit { get; set; }
        public int? PixelRepresentation { get; set; }
        public int? PixelData { get; set; }
        public string Sender { get; set; }
        public long FileSize { get; set; }
        public string FileSizeDisplay { get; set; }
        public DateTime Created { get; set; }
        public string SpecificCharacterSet { get; set; }
        public string ImageType { get; set; }
        public string PhotoMatricInterpretation { get; set; }
        public string PatientAge { get; set; }
        public string LossyImageCompression { get; set; }
        public int? Method { get; set; }
    }
}
