using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class PrescriptionItemModel : System.ComponentModel.INotifyPropertyChanged
    {
        public long PrescriptionItemUID { get; set; }
        public long PrescriptionUID { get; set; }
        public string PrestionItemStatus { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public Nullable<int> ROUTEUID { get; set; }
        public Nullable<int> FRQNCUID { get; set; }
        public Nullable<int> DFORMUID { get; set; }
        public string DrugForm { get; set; }
        public string ClinicalComments { get; set; }
        public string Comments { get; set; }
        public Nullable<System.DateTime> StartDttm { get; set; }
        public Nullable<int> DrugDuration { get; set; }
        public Nullable<double> Quantity { get; set; }
        public Nullable<double> Dosage { get; set; }
        public string QuantityUnit { get; set; }
        public string Frequency { get; set; }
        public string InstructionRoute { get; set; }
        public string InstructionText { get; set; }
        public string LocalInstructionText { get; set; }
        public Nullable<int> ORDSTUID { get; set; }
        public Nullable<int> PDSTSUID { get; set; }
        public Nullable<int> IMUOMUID { get; set; }
        public Nullable<int> StoreUID { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public Nullable<int> ItemMasterUID { get; set; }
        public int BillableItemUID { get; set; }
        public string BillingService { get; set; }
        public string DrugType { get; set; }
        public long PatientOrderDetailUID { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
        public string StoreName { get; set; }
        public Nullable<int> OwnerOrganisationUID { get; set; }
        public int No { get; set; }
        public int? StockUID { get; set; }
        public bool IsWithoutStock { get; set; }
        public ObservableCollection<PatientOrderDetailModel> StoreStockItem { get; set; }


        private double? _BalQty;

        public double? BalQty
        {
            get { return _BalQty; }
            set
            {
                _BalQty = value;
                NotifyPropertyChanged("BalQty");

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
