using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class SearchPatientBillViewModel : MediTechViewModelBase
    {
        #region Propoties
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public string RequestNo { get; set; }

        public List<HealthOrganisationModel> Organisations { get; set; }

        private HealthOrganisationModel _SelectOrganisation;
        public HealthOrganisationModel SelectOrganisation
        {
            get { return _SelectOrganisation; }
            set
            {
                Set(ref _SelectOrganisation, value);
            }
        }

        private List<PayorDetailModel> _PayorDetails;
        public List<PayorDetailModel> PayorDetails
        {
            get { return _PayorDetails; }
            set { Set(ref _PayorDetails, value); }
        }

        private PayorDetailModel _SelectPayorDetail;
        public PayorDetailModel SelectPayorDetail
        {
            get { return _SelectPayorDetail; }
            set{Set(ref _SelectPayorDetail, value); }
        }

        private List<PatientBillModel> _PatientBillSource;
        public List<PatientBillModel> PatientBillSource
        {
            get { return _PatientBillSource; }
            set { Set(ref _PatientBillSource, value); }
        }

        private ObservableCollection<PatientBillModel> _SelectPatientBill;
        public ObservableCollection<PatientBillModel> SelectPatientBill
        {
            get
            {
                return _SelectPatientBill
                    ?? (_SelectPatientBill = new ObservableCollection<PatientBillModel>());
            }
            set { _SelectPatientBill = value; }
        }

        private ObservableCollection<PatientBillModel> _PatientBillGroup;
        public ObservableCollection<PatientBillModel> PatientBillGroup
        {
            get { return _PatientBillGroup; }
            set { Set(ref _PatientBillGroup, value); }
        }

        private PatientBillModel _SelectPatientBillGroup;
        public PatientBillModel SelectPatientBillGroup
        {
            get { return _SelectPatientBillGroup; }
            set { _SelectPatientBillGroup = value; }
        }

        private string _BillNumber;
        public string BillNumber
        {
            get { return _BillNumber; }
            set { Set(ref _BillNumber, value); }
        }
        private double ? _UnitPrice;
        public double ? UnitPrice
        {
            get { return _UnitPrice; }
            set { Set(ref _UnitPrice, value); }
        }

        private double _Quantity;
        public double Quantity
        {
            get { return _Quantity; }
            set { Set(ref _Quantity, value); }
        }

        private double ? _ReCash;
        public double ? ReCash
        {
            get { return _ReCash; }
            set { Set(ref _ReCash, value); }
        }

        private string _Price;
        public string Price
        {
            get { return _Price; }
            set { Set(ref _Price, value); }
        }

        private double ? _Discount;
        public double ? Discount
        {
            get { return _Discount; }
            set { Set(ref _Discount, value); }
        }

        private double ? _TaxSum;
        public double ? TaxSum
        {
            get { return _TaxSum; }
            set { Set(ref _TaxSum, value); }
        }

        private double ? _NetPrice;
        public double ? NetPrice
        {
            get { return _NetPrice; }
            set { Set(ref _NetPrice, value); }
        }

        public List<LookupReferenceValueModel> TaxChoice { get; set; }

        private LookupReferenceValueModel _TaxSelect;
        public LookupReferenceValueModel TaxSelect
        {
            get { return _TaxSelect; }
            set
            {
                Set(ref _TaxSelect, value);
                if (TaxSelect != null)
                {
                    if(PatientBillGroup != null)
                    {
                        Quantity = PatientBillGroup.Count;
                        double? billprice = PatientBillGroup[0].TotalAmount;
                        double? billdiscount = PatientBillGroup[0].DiscountAmount;

                        ReCash = SumPrice(Quantity, billprice, billdiscount);
                        NetPrice = ReCash;
                    }
                }
            }
        }

        #endregion

        #region Command

        private RelayCommand _SearchCommand;
        public RelayCommand SearchCommand
        {
            get { return _SearchCommand ?? (_SearchCommand = new RelayCommand(SearchPatientBill)); }
        }

        private RelayCommand _AddCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return _AddCommand
                    ?? (_AddCommand = new RelayCommand(AddGroupResult));
            }
        }

        private RelayCommand _RemoveCommand;
        public RelayCommand RemoveCommand
        {
            get
            {
                return _RemoveCommand
                    ?? (_RemoveCommand = new RelayCommand(RemoveGroupResult));
            }
        }
        #endregion

        #region Method

        public SearchPatientBillViewModel()
        {
            DateFrom = DateTime.Now;
            DateTo = DateTime.Now;
            PayorDetails = DataService.MasterData.GetPayorDetail();
            Organisations = GetHealthOrganisationRoleMedical();
            SelectOrganisation = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == AppUtil.Current.OwnerOrganisationUID);

            TaxChoice = new List<LookupReferenceValueModel>{
                new LookupReferenceValueModel { Key = 0, Display = "7%" },
                new LookupReferenceValueModel { Key = 1, Display = "ยกเลิกภาษี" }
            };
        }

        public void SearchPatientBill()
        {
            long? patientUID = null;

            int? ownerOrganisationUID = (SelectOrganisation != null && SelectOrganisation.HealthOrganisationUID != 0) ? SelectOrganisation.HealthOrganisationUID : (int?)null;
            PatientBillSource = DataService.Billing.SearchPatientBill(DateFrom, DateTo, patientUID, BillNumber, ownerOrganisationUID);
        }

        public void AddGroupResult()
        {
            double ? billprice = 0;
            double ? billdiscount = 0;
            double ? billNet = 0;
            TaxSum = 0;
            if(SelectPatientBill != null)
            {
                foreach (var item in SelectPatientBill)
                {
                    Quantity = SelectPatientBill.Count;
                    

                    break;
                }

                billprice = SelectPatientBill[0].TotalAmount;
                billdiscount = SelectPatientBill[0].DiscountAmount;
                billNet = SelectPatientBill[0].NetAmount;

                ReCash = SumPrice(Quantity, billprice, billdiscount);
                NetPrice = ReCash;

                PatientBillGroup = SelectPatientBill;
            }
        }

        public void RemoveGroupResult()
        {
            if (SelectPatientBillGroup != null)
            {
                ObservableCollection<PatientBillModel> billRemove = new ObservableCollection<PatientBillModel>();
                billRemove = PatientBillGroup;
                ObservableCollection<PatientBillModel> test = new ObservableCollection<PatientBillModel>();

                PatientBillGroup = null;

                foreach(var item in billRemove)
                {
                    if(SelectPatientBillGroup.BillNumber != item.BillNumber)
                    {
                        test.Add(item);
                    }
                }
                PatientBillGroup = test;

                Quantity = PatientBillGroup.Count;
                double ? billprice = PatientBillGroup[0].TotalAmount;
                double?  billdiscount = PatientBillGroup[0].DiscountAmount;

                ReCash = SumPrice(Quantity, billprice, billdiscount);
                NetPrice = ReCash;
            }
        }

        public double ? SumPrice(double ? quantity, double ? total, double ? discount)
        {
            double ? result = 0;
            double ? tax = 0;
            if (quantity != 0 && total != 0 && discount != null)
            {
                UnitPrice = total * quantity;
                Discount = discount * quantity;
                result = (total - discount) * quantity;

            }

            if (TaxSelect != null)
            {
                if (TaxSelect.Display == "7%")
                {
                    tax = (total * 0.07) * quantity;
                    TaxSum = tax;
                }
            }

            result = result + tax;

            return result;
        }


        #endregion
    }
}
