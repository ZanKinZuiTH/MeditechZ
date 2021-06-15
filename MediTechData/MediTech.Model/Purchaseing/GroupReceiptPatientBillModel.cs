using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class GroupReceiptPatientBillModel : PatientBillModel
    {
        public int GroupReceiptPatientBillUID { get; set; }
        public int GroupReceiptUID { get; set; }
    }
}
