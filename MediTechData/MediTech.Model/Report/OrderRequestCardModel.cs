using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model.Report
{
    public class OrderRequestCardModel
    {

        public long No { get; set; }

        public Nullable<int> CareProviderUID { get; set; }
        public string Gender { get; set; }
        public string PatientID { get; set; }
        public string PatientName { get; set; }
        public string Age { get; set; }
        public long PatientVisitUID { get; set; }
        public string VisitID { get; set; }
        public DateTime? VisitDate { get; set; }
        public string strVisitData { get; set; }
        public string Doctor { get; set; }
        public string DrugAllergy { get; set; }
        public List<DetailOrderDetailCard> OrderDetail { get; set; }
        public DateTime? DOB { get; set; }
        public string Staff { get; set; }



    }

    public class DetailOrderDetailCard
    {
        public string ItemName { get; set; }
        public string CheckOrder { get; set; }
        public double Quantity { get; set; }
        public string Comment { get; set; }

    }


}
