using GalaSoft.MvvmLight.Command;
using MediTech.DataService;
using MediTech.Model;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class ManageDrugGenericViewModel : MediTechViewModelBase
    {

        #region Properties


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
        DrugGenericModel model;

        public ManageDrugGenericViewModel()
        {
            ActiveFrom = DateTime.Now.Date;
        }
        private void Save()
        {
            try
            {
                if (string.IsNullOrEmpty(Name))
                {
                    WarningDialog("กรุณาใส่ ชื่อ");
                    return;
                }
                AssingPropertiesToModel();
                DataService.Pharmacy.ManageDrugGeneric(model, AppUtil.Current.UserID);
                SaveSuccessDialog();
                ListDrugGeneric pageList = new ListDrugGeneric();
                ChangeViewPermission(pageList);
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
           
        }

        private void Cancel()
        {
            ListDrugGeneric pageList = new ListDrugGeneric();
            ChangeViewPermission(pageList);
        }


        public void AssingModel(DrugGenericModel modelData)
        {
            model = modelData;
            AssingModelToProperites();
        }

        private void AssingModelToProperites()
        {
            Name = model.Name;
            Description = model.Description;
            ActiveFrom = model.ActiveFrom;
            ActiveTo = model.ActiveTo;
        }

        private void AssingPropertiesToModel()
        {
            if (model == null)
            {
                model = new DrugGenericModel();
            }
            model.Name = Name;
            model.Description = Description;
            model.ActiveFrom = ActiveFrom;
            model.ActiveTo = ActiveTo;

        }

        #endregion

    }
}
