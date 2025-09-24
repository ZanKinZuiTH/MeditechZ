using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MediTech.DataService;
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
    public class ManageStoreUOMConversionViewModel : MediTechViewModelBase
    {
        #region Properties
        public List<LookupReferenceValueModel> StoreUOMs { get; set; }

        private LookupReferenceValueModel _SelectBaseUOM;

        public LookupReferenceValueModel SelectBaseUOM
        {
            get { return _SelectBaseUOM; }
            set { Set(ref _SelectBaseUOM ,value); }
        }
        private LookupReferenceValueModel _SelectConverionsUOM;

        public LookupReferenceValueModel SelectConverionsUOM
        {
            get { return _SelectConverionsUOM; }
            set { Set(ref _SelectConverionsUOM, value); }
        }

        private double _ConversionValue;

        public double ConversionValue
        {
            get { return _ConversionValue; }
            set { Set(ref _ConversionValue, value); }
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
        StoreUOMConversionModel model;
        public ManageStoreUOMConversionViewModel()
        {
            StoreUOMs = DataService.Technical.GetReferenceValueMany("IMUOM");
        }

        private void Save()
        {
            try
            {

                if (SelectBaseUOM == null)
                {
                    WarningDialog("กรุณาระบุ หน่วยหลัก");
                    return;
                }

                if (SelectConverionsUOM == null)
                {
                    WarningDialog("กรุณาระบุ หน่อวยแปลง");
                    return;
                }

                if (ConversionValue == 0)
                {
                    WarningDialog("กรุณาระบุ ค่า");
                    return;
                }

                AssingPropertiesToModel();
                DataService.Inventory.ManageStoreUOMConversion(model, AppUtil.Current.UserID);
                SaveSuccessDialog();
                ListStoreUOMConversion listPage = new ListStoreUOMConversion();
                ChangeViewPermission(listPage);
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }
        private void Cancel()
        {
            ListStoreUOMConversion listPage = new ListStoreUOMConversion();
            ChangeViewPermission(listPage);
        }
        public void AssingModel(StoreUOMConversionModel model)
        {
            this.model = model;
            AssingModelToProperties();
        }

        void AssingModelToProperties()
        {
            SelectBaseUOM = StoreUOMs != null ? StoreUOMs.FirstOrDefault(p => p.Key == model.BaseUOMUID) : null;
            SelectConverionsUOM = StoreUOMs != null ? StoreUOMs.FirstOrDefault(p => p.Key == model.ConversionUOMUID) : null;
            ConversionValue = model.ConversionValue ?? 0;
        }

        void AssingPropertiesToModel()
        {
            if (model == null)
            {
                model = new StoreUOMConversionModel();
            }
            model.BaseUOMUID = SelectBaseUOM.Key.Value;
            model.ConversionUOMUID = SelectConverionsUOM.Key.Value;
            model.ConversionValue = ConversionValue;
        }


        #endregion

    }
}
