using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class InstancesCopyToModel
    {
        public string SOPInstanceUID { get; set; }
        public int? InstanceNumber { get; set; }
        public int? NumberOfFrames { get; set; }
        public string FileSizeDisplay { get; set; }

        public byte[] DicomFiles { get; set; }
    }
}
