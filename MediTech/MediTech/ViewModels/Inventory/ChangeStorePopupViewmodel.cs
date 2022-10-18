using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels.Inventory
{
    public class ChangeStorePopupViewmodel : MediTechViewModelBase
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

        private PrescriptionModel prescriptionModel = new PrescriptionModel();
        
        #endregion

        #region Command

        private RelayCommand _SaveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return _SaveCommand ?? (_SaveCommand = new RelayCommand(Save));
            }
        }

        private RelayCommand _CancelCommand;
        public RelayCommand CancelCommand
        {
            get
            {
                return _CancelCommand ?? (_CancelCommand = new RelayCommand(Cancel));
            }
        }

        #endregion

        #region Method
        public ChangeStorePopupViewmodel()
        {
            Stores = DataService.Inventory.GetStoreByOrganisationUID(AppUtil.Current.OwnerOrganisationUID);
        }

        private void Save()
        {
            if(SelectStore != null)
            {
                if(SelectStore.StoreUID == prescriptionModel.OrderToLocationStoreUID)
                {
                    WarningDialog("คลังที่เลือกเป็นคลังเดิม กรุณาเลือกคลังใหม่");
                    return;
                }

                foreach (var item in prescriptionModel.PrescriptionItems)
                {
                    item.StoreUID = SelectStore.StoreUID;
                }
                prescriptionModel.OrderToLocationStoreUID = SelectStore.StoreUID;

                DataService.Pharmacy.UpdatePrescriptionStore(prescriptionModel, AppUtil.Current.UserID);
                CloseViewDialog(ActionDialog.Save);
            }
        }

        public void AssingModel(PrescriptionModel model)
        {
            var stuid = model.PrescriptionItems.FirstOrDefault().StoreUID;
            prescriptionModel = model;
            SelectStore = Stores.FirstOrDefault(p => p.StoreUID == stuid);
        }

        private void Cancel()
        {
            CloseViewDialog(ActionDialog.Cancel);
        }
        #endregion
    }
}
