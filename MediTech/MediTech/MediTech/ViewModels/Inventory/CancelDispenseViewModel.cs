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
    public class CancelDispenseViewModel : MediTechViewModelBase
    {
        #region Properties

        private PatientVisitModel _PatientVisit;

        public PatientVisitModel PatientVisit
        {
            get { return _PatientVisit; }
            set { Set(ref _PatientVisit, value); }
        }


        private String _Comments;

        public String Comments
        {
            get { return _Comments; }
            set { Set(ref _Comments, value); }
        }

        private ObservableCollection<PrescriptionItemModel> _PrescriptionItems;

        public ObservableCollection<PrescriptionItemModel> PrescriptionItems
        {
            get
            {
                return _PrescriptionItems
                    ?? (_PrescriptionItems = new ObservableCollection<PrescriptionItemModel>());
            }

            set { Set(ref _PrescriptionItems, value); }
        }


        private ObservableCollection<PrescriptionItemModel> _SelectPrescriptionItems;

        public ObservableCollection<PrescriptionItemModel> SelectPrescriptionItems
        {
            get
            {
                return _SelectPrescriptionItems
                    ?? (_SelectPrescriptionItems = new ObservableCollection<PrescriptionItemModel>());
            }

            set { Set(ref _SelectPrescriptionItems, value); }
        }

        #endregion

        #region Command


        private RelayCommand _SaveCommand;

        public RelayCommand SaveCommand
        {
            get { return _SaveCommand ?? (_SaveCommand = new RelayCommand(Save)); }
        }

        private RelayCommand _CancelCommand;

        public RelayCommand CancelCommand
        {
            get { return _CancelCommand ?? (_CancelCommand = new RelayCommand(Cancel)); }
        }

        #endregion

        #region Method

        public override void OnLoaded()
        {
            base.OnLoaded();
            foreach (var item in PrescriptionItems)
            {
                var storeUsed = DataService.Pharmacy.GetDrugStoreDispense(item.ItemMasterUID ?? 0, item.Quantity ?? 0, item.IMUOMUID ?? 0, item.StoreUID ?? 0);

                item.BalQty = storeUsed.Sum(p => p.BalQty);
            }
        }

        void Save()
        {
            try
            {
                if (SelectPrescriptionItems != null)
                {
                    foreach (var item in SelectPrescriptionItems)
                    {
                        item.Comments = Comments;
                        DataService.Pharmacy.CancelDispensed(item, AppUtil.Current.UserID);
                    }
                }
                ChangeView_CloseViewDialog(new Prescription(), ActionDialog.Cancel);
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }

        }

        void Cancel()
        {
            ChangeView_CloseViewDialog(new Prescription(), ActionDialog.Cancel);
        }

        public void AssignModel(IEnumerable<PrescriptionItemModel> prescriptionItemsModel, PatientVisitModel visitModel)
        {
            PrescriptionItems = new ObservableCollection<PrescriptionItemModel>(prescriptionItemsModel.Where(p => p.PrestionItemStatus == "Dispensed"));
            PatientVisit = visitModel;
        }
        #endregion
    }
}
