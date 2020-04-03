using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class ManageBilledItemSaleViewModel : MediTechViewModelBase
    {
        #region Properites

        #region PatientSearch

        private string _SearchPatientCriteria;

        public string SearchPatientCriteria
        {
            get { return _SearchPatientCriteria; }
            set
            {
                Set(ref _SearchPatientCriteria, value);
                PatientsSearchSource = null;
            }
        }


        private List<PatientInformationModel> _PatientsSearchSource;

        public List<PatientInformationModel> PatientsSearchSource
        {
            get { return _PatientsSearchSource; }
            set { Set(ref _PatientsSearchSource, value); }
        }

        private PatientInformationModel _SelectedPateintSearch;

        public PatientInformationModel SelectedPateintSearch
        {
            get { return _SelectedPateintSearch; }
            set
            {
                _SelectedPateintSearch = value;
                if (_SelectedPateintSearch != null)
                {

                }
            }
        }

        #endregion

        public List<HealthOrganisationModel> Organisations { get; set; }
        private HealthOrganisationModel _SelectOrganisation;

        public HealthOrganisationModel SelectOrganisation
        {
            get { return _SelectOrganisation; }
            set
            {
                Set(ref _SelectOrganisation, value);
                if (_SelectOrganisation != null)
                {
                    Stores = DataService.Inventory.GetStoreByOrganisationUID(_SelectOrganisation.HealthOrganisationUID);
                }
                else
                {
                    Stores = null;
                }
            }
        }


        private List<StoreModel> _Stores;

        public List<StoreModel> Stores
        {
            get { return _Stores; }
            set { Set(ref _Stores, value); }
        }
        private StoreModel _SelectStore;

        public StoreModel SelectStore
        {
            get { return _SelectStore; }
            set {
                Set(ref _SelectStore, value);
                if (SelectStore != null)
                {
                    ItemMasters = DataService.Inventory.GetItemMasterQtyByStore(SelectStore.StoreUID);
                }
            }
        }

        private List<LookupReferenceValueModel> _Consumers;

        public List<LookupReferenceValueModel> Consumers
        {
            get { return _Consumers; }
            set { Set(ref _Consumers, value); }
        }

        private LookupReferenceValueModel _SelectCunsumer;

        public LookupReferenceValueModel SelectCunsumer
        {
            get { return _SelectCunsumer; }
            set { Set(ref _SelectCunsumer, value); }
        }

        private List<CareproviderModel> _SalesReps;

        public List<CareproviderModel> SalesReps
        {
            get { return _SalesReps; }
            set { Set(ref _SalesReps, value); }
        }

        private List<CareproviderModel> _Doctors;

        public List<CareproviderModel> Doctors
        {
            get { return _Doctors; }
            set { Set(ref _Doctors, value); }
        }

        private string _SalesRepName;

        public string SalesRepName
        {
            get { return _SalesRepName; }
            set { Set(ref _SalesRepName, value); }
        }

        private string _Referredname;

        public string Referredname
        {
            get { return _Referredname; }
            set { Set(ref _Referredname, value); }
        }

        private string _Comments;

        public string Comments
        {
            get { return _Comments; }
            set { Set(ref _Comments, value); }
        }


        private bool _IsPrint;

        public bool IsPrint
        {
            get { return _IsPrint; }
            set { Set(ref _IsPrint, value); }
        }

        private double _TotalPrice;
        public double TotalPrice
        {
            get { return _TotalPrice; }
            set
            {
                Set(ref _TotalPrice, value);
                //TotalAmount = CalTotalAmount(TotalPrice, DiscountAmount);
            }
        }

        private double _DiscountAmount;

        public double DiscountAmount
        {
            get { return _DiscountAmount; }
            set
            {
                Set(ref _DiscountAmount, value);
                //TotalAmount = CalTotalAmount(TotalPrice, DiscountAmount);
            }
        }

        private double _TotalAmount;

        public double TotalAmount
        {
            get { return _TotalAmount; }
            set { Set(ref _TotalAmount, value); }
        }


        private double _Payment;
        public double Payment
        {
            get { return _Payment; }
            set
            {
                Set(ref _Payment, value);
                ReCash = CalReCash(Payment, TotalAmount);

            }
        }

        private double _ReCash;
        public double ReCash
        {
            get { return _ReCash; }
            set { Set(ref _ReCash, value); }
        }

        private List<ItemMasterModel> _ItemMasters;

        public List<ItemMasterModel> ItemMasters
        {
            get { return _ItemMasters; }
            set { Set(ref _ItemMasters, value); }
        }

        private ObservableCollection<ItemMasterList> _SaleItemList;

        public ObservableCollection<ItemMasterList> SaleItemList
        {
            get { return _SaleItemList; }
            set
            {
                Set(ref _SaleItemList, value);
            }
        }

        private ObservableCollection<ItemMasterList> _SelectSaleItem;

        public ObservableCollection<ItemMasterList> SelectSaleItem
        {
            get { return _SelectSaleItem; }
            set
            {
                Set(ref _SelectSaleItem, value);
            }
        }

        #endregion

        #region Command

        private RelayCommand _PatientSearchCommand;

        public RelayCommand PatientSearchCommand
        {
            get { return _PatientSearchCommand ?? (_PatientSearchCommand = new RelayCommand(PatientSearch)); }
        }

        #endregion

        #region Method

        public ManageBilledItemSaleViewModel()
        {
            Organisations = GetHealthOrganisationIsRoleStock();
            Consumers = DataService.Technical.GetReferenceValueMany("SALTO");
            var careprovider = DataService.UserManage.GetCareproviderAll();
            SalesReps = careprovider.Where(p => p.IsDoctor == false).ToList();
            Doctors = careprovider.Where(p => p.IsDoctor == true).ToList();
            if (Organisations != null)
            {
                SelectOrganisation = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == AppUtil.Current.OwnerOrganisationUID);
            }

            SalesRepName = AppUtil.Current.UserName;
        }

        public void PatientSearch()
        {
            string patientID = string.Empty;
            string firstName = string.Empty; ;
            string lastName = string.Empty;
            if (SearchPatientCriteria.Length >= 3)
            {
                string[] patientName = SearchPatientCriteria.Split(' ');
                if (patientName.Length >= 2)
                {
                    firstName = patientName[0];
                    lastName = patientName[1];
                }
                else
                {
                    int num = 0;
                    foreach (var ch in SearchPatientCriteria)
                    {
                        if (ShareLibrary.CheckValidate.IsNumber(ch.ToString()))
                        {
                            num++;
                        }
                    }
                    if (num >= 5)
                    {
                        patientID = SearchPatientCriteria;
                    }
                    else if (num <= 2)
                    {
                        firstName = SearchPatientCriteria;
                        lastName = "empty";
                    }

                }
                List<PatientInformationModel> searchResult = DataService.PatientIdentity.SearchPatient(patientID, firstName, "", lastName, "", null, null, "", null);
                PatientsSearchSource = searchResult;
            }
            else
            {
                PatientsSearchSource = null;
            }

        }

        public double CalReCash(double cash, double total)
        {
            double result = 0;
            if (cash != 0 && total != 0)
            {
                result = cash - total;
            }

            return result;
        }

        #endregion
    }
}
