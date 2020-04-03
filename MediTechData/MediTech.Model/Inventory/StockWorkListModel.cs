using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class StockWorkListModel
    {
        public int DocumentUID { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNumber { get; set; }
        public DateTime DocumentDate { get; set; }
        public string RasiedOn { get; set; }
        public string RasiedBy { get; set; }
        public string Status { get; set; }
        public int StatusUID { get; set; }
    }
}
