using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model.PACS
{
    [Serializable]
    public class DataInbound
    {
        public string PatientID { get; set; }
        public string VisitID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Title { get; set; }
        public DateTime BirthDate { get; set; }
        public string RequestNote { get; set; }
        public string RequestItemCode { get; set; }
        public string RequestItemName { get; set; }
        public double NetAmount { get; set; }
        public long PatientOrderDetailUID { get; set; }
        public string NetworkPartnerID { get; set; }
        public string HealthOrganisationCode { get; set; }
        public string DocumentName { get; set; }
        public byte[] DocumentContent { get; set; }
        public byte[] DicomFile { get; set; }
    }
}
