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
    public class ManageStoreViewModel : MediTechViewModelBase
    {

        #region Properties

        private string _Code;

        public string Code
        {
            get { return _Code; }
            set { Set(ref _Code, value); }
        }

        private string _Name;

        public string Name
        {
            get { return _Name; }
            set { Set(ref _Name, value); }
        }

        private string _Description;

        public string Description
        {
            get { return _Description; }
            set { Set(ref _Description, value); }
        }

        private DateTime? _ActiveFrom;

        public DateTime? ActiveFrom
        {
            get { return _ActiveFrom; }
            set { Set(ref _ActiveFrom, value); }
        }

        private DateTime? _ActiveTo;

        public DateTime? ActiveTo
        {
            get { return _ActiveTo; }
            set { Set(ref _ActiveTo, value); }
        }

        public List<LookupReferenceValueModel> StoreTypes { get; set; }
        private LookupReferenceValueModel _SelectStoreType;

        public LookupReferenceValueModel SelectStoreType
        {
            get { return _SelectStoreType; }
            set { Set(ref _SelectStoreType , value); }
        }


        public List<HealthOrganisationModel> Organisations { get; set; }
        private HealthOrganisationModel _SelectOrganisation;

        public HealthOrganisationModel SelectOrganisation
        {
            get { return _SelectOrganisation; }
            set { Set(ref _SelectOrganisation,value); }
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

        StoreModel model;

        public ManageStoreViewModel()
        {
            StoreTypes = DataService.Technical.GetReferenceValueMany("STDTP");
            Organisations = GetHealthOrganisationIsRoleStock();
        }
        private void Save()
        {
            try
            {
                if (string.IsNullOrEmpty(Name))
                {
                    WarningDialog("กรุณาระบุ ชื่อ");
                    return;
                }

                if (SelectStoreType == null)
                {
                    WarningDialog("กรุณาระบุ ประเภท");
                    return;
                }

                if (SelectOrganisation == null)
                {
                    WarningDialog("กรุณาระบุ สถานประกอบการ");
                    return;
                }

                AssingPropertiesToModel();
                DataService.Inventory.ManageStore(model, AppUtil.Current.UserID);
                SaveSuccessDialog();
                ListStores listPage = new ListStores();
                ChangeViewPermission(listPage);
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }
        private void Cancel()
        {
            ListStores listPage = new ListStores();
            ChangeViewPermission(listPage);
        }
        public void AssingModel(StoreModel model)
        {
            this.model = model;
            AssingModelToProperties();
        }

        void AssingModelToProperties()
        {
            Name = model.Name;
            Description = model.Description;
            SelectStoreType = StoreTypes.FirstOrDefault(p => p.Key == model.STDTPUID);
            ActiveFrom = model.ActiveFrom;
            ActiveTo = model.ActiveTo;
            SelectOrganisation = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == model.OwnerOrganisationUID);
        }

        void AssingPropertiesToModel()
        {
            if (model == null)
            {
                model = new StoreModel();
            }
            model.Name = Name;
            model.Description = Description;
            model.ActiveFrom = ActiveFrom;
            model.ActiveTo = ActiveTo;
            model.STDTPUID = SelectStoreType != null ? SelectStoreType.Key : (int?)null;
            model.OwnerOrganisationUID = SelectOrganisation != null ? SelectOrganisation.HealthOrganisationUID : 0;
        }

        #endregion



    }
}
