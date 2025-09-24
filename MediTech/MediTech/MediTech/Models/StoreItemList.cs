using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Models
{
    public class StoreItemList : PatientOrderDetailModel
    {
        public ObservableCollection<PatientOrderDetailModel> StoreStockItem { get; set; }
    }
}
