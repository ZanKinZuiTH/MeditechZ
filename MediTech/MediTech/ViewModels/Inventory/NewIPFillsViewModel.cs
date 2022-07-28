using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class NewIPFillsViewModel : MediTechViewModelBase
    {
        #region Properties
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
            set { Set(ref _SelectStore, value); }
        }

        private List<LocationModel> _ward;
        public List<LocationModel> Ward
        {
            get { return _ward; }
            set { Set(ref _ward, value); }
        }

        private LocationModel _SelectWard;
        public LocationModel SelectWard
        {
            get { return _SelectWard; }
            set { Set(ref _SelectWard, value); }
        }
        
        private int _ForDay;
        public int ForDay
        {
            get { return _ForDay; }
            set { Set(ref _ForDay, value); }
        }

        private ObservableCollection<PatientOrderStandingModel> _PatientOrder;

        public ObservableCollection<PatientOrderStandingModel> PatientOrder
        {
            get { return _PatientOrder; }
            set { Set(ref _PatientOrder, value); }
        }

        private DateTime _ExcludeTime;
        public DateTime ExcludeTime
        {
            get { return _ExcludeTime; }
            set { Set(ref _ExcludeTime, value); }
        }

        #endregion

        #region Command
        private RelayCommand _SearchCommand;
        public RelayCommand SearchCommand
        {
            get { return _SearchCommand ?? (_SearchCommand = new RelayCommand(Search)); }
        }

        private RelayCommand _ClearCommand;

        public RelayCommand ClearCommand
        {
            get { return _ClearCommand ?? (_ClearCommand = new RelayCommand(Clear)); }
        }

        private RelayCommand _DispenseCommand;
        public RelayCommand DispenseCommand
        {
            get { return _DispenseCommand ?? (_DispenseCommand = new RelayCommand(Dispense)); }
        }

        private RelayCommand _CloseCommand;
        public RelayCommand CloseCommand
        {
            get { return _CloseCommand ?? (_CloseCommand = new RelayCommand(Close)); }
        }

        #endregion


        #region Method
        public NewIPFillsViewModel()
        {
            ForDay = 1;
            var locationAll = DataService.InPatientService.GetBedALL();
            Ward = locationAll.Where(w => w.LOTYPUID == 3152).ToList();
            Stores = DataService.Inventory.GetStore();
            SelectStore = Stores.FirstOrDefault();
        }


        private void Search()
        {
            if (SelectWard == null)
            {
                WarningDialog("กรุณาเลือก Ward");
                return;
            }

            if (SelectStore != null)
            {
                int? wardid = SelectWard.LocationUID;
                List<PatientOrderStandingModel> data = DataService.Pharmacy.GetPatientOrderStanding(wardid, SelectStore.StoreUID);
                if (data != null && data.Count != 0)
                {
                    PatientOrder = new ObservableCollection<PatientOrderStandingModel>(data);

                    foreach (var item in PatientOrder)
                    {
                        var storeUsed = DataService.Pharmacy.GetDrugStoreDispense(item.ItemUID ?? 0, item.Quantity ?? 0, item.QNUOMUID ?? 0, item.StoreUID ?? 0);
                        foreach (var stock in storeUsed)
                        {
                            if (stock.Quantity > stock.BalQty)
                            {
                                stock.IsWithoutStock = true;
                            }

                            if (stock.ExpiryDate?.Date <= DateTime.Now.Date)
                            {
                                stock.IsExpired = true;
                            }
                        }
                        item.StockUID = storeUsed.FirstOrDefault() != null ? storeUsed.FirstOrDefault().StockUID : null;
                        item.StoreName = storeUsed.FirstOrDefault().StoreName;
                        item.BalQty = storeUsed.Sum(p => p.BalQty);
                        item.ExpiryDate = storeUsed.FirstOrDefault() != null ? storeUsed.FirstOrDefault().ExpiryDate : null;
                        item.IsWithoutStock = storeUsed.FirstOrDefault(p => p.IsWithoutStock == true) != null ? true : false;
                        item.BatchID = storeUsed.FirstOrDefault().BatchID;
                        item.ORDSTUID = DataService.Technical.GetReferenceValueByCode("ORDST", "DISPE").Key ?? 0;
                        item.ParentUID = Convert.ToInt32(item.PatientOrderUID);
                        item.OwnerOrganisationUID = AppUtil.Current.OwnerOrganisationUID;
                        item.LocationUID = AppUtil.Current.LocationUID;
                        item.ParentOrderDetalUID = item.ParentOrderDetalUID;
                    }
                }
            }
        }
    

        private void Dispense()
        {
            if (PatientOrder != null && PatientOrder.Count != 0)
            {
                IPFillProcessModel iPFillProcess = new IPFillProcessModel();
                iPFillProcess.WardUID = SelectWard.LocationUID;
                iPFillProcess.StoreUID = SelectStore.StoreUID;
                iPFillProcess.OwnerOrganisationUID = AppUtil.Current.OwnerOrganisationUID;
                iPFillProcess.FillForDay = ForDay;
                iPFillProcess.ExcludePriorHour = DateTime.Now.Date;
                iPFillProcess.StandingModels = new List<PatientOrderStandingModel>(PatientOrder);

                DataService.Inventory.DispenseIPFills(iPFillProcess, AppUtil.Current.UserID);
                SaveSuccessDialog();
                Clear();
            }
        }


        private void Clear()
        {
            SelectWard = null;
            PatientOrder = null;

        }

        private void Close()
        {
            IPFills iPFills = new IPFills();
            ChangeViewPermission(iPFills);
        }

        #endregion
    }
}
