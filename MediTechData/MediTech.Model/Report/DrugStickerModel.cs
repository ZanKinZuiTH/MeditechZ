using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model.Report
{
    public class DrugStickerModel
    {
        public string PrescriptionNumber { get; set; }
        public DateTime PrescribedDttm { get; set; }
        public string PatientName { get; set; }
        public DateTime DateofBirth { get; set; }
        public string DrugName { get; set; }
        public string DrugLable { get; set; }
        public string DoctorName { get; set; }
        public string FORMM { get; set; }
        public double? NumericValue { get; set; }
        public string FrequencyDefinition { get; set; }
        public string InstructionText { get; set; }
        public string LocalInstructionText { get; set; }
        public string PatientInstruction { get; set; }
        public double? Dosage { get; set; }
        public string ItemUnit { get; set; }
        public string OrganisationCode { get; set; }
        public string OrganisationName { get; set; }
        public string LicenseNo { get; set; }
        public string OrganisationAddress { get; set; }
        public double? Quantity { get; set; }
        public string QuantityLabel { get; set; }
    }
}
