using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MediTech.DataService;
using MediTech.Model;
using MediTech.Models;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MediTech.ViewModels
{
    public class ManageDrugFrequencyViewModel : MediTechViewModelBase
    {

        #region Properties

        private string _Code;
        public string Code
        {
            get { return _Code; }
            set { Set(ref _Code, value); }
        }

        private string _FrequencyText;

        public string FrequencyText
        {
            get { return _FrequencyText; }
            set { Set(ref _FrequencyText, value); }
        }

        private string _LocalFrequencyText;

        public string LocalFrequencyText
        {
            get { return _LocalFrequencyText; }
            set { Set(ref _LocalFrequencyText, value); }
        }

        private int? _AmountPerTimes;

        public int? AmountPerTimes
        {
            get { return _AmountPerTimes; }
            set { Set(ref _AmountPerTimes, value); }
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

        FrequencyDefinitionModel model;

        public ManageDrugFrequencyViewModel()
        {
            

        }
        private void Save()
        {
            try
            {
                if (string.IsNullOrEmpty(Code))
                {
                    WarningDialog("กรุณาใส่ รหัส");
                    return;
                }

                if (string.IsNullOrEmpty(FrequencyText))
                {
                    WarningDialog("กรุณาใส่ ภาษาอังกฤษ");
                    return;
                }

                if (string.IsNullOrEmpty(LocalFrequencyText))
                {
                    WarningDialog("กรุณาใส่ ภาษาไทย");
                    return;
                }

                if (AmountPerTimes == null)
                {
                    WarningDialog("กรุณาใส่ จำนวนครั้งต่อเวลา");
                    return;
                }

                AssingPropertiesToModel();
                DataService.Pharmacy.ManageDrugFrequency(model, AppUtil.Current.UserID);
                SaveSuccessDialog();
                ListDrugFrequency listDrugFrequency = new ListDrugFrequency();
                ChangeViewPermission(listDrugFrequency);
            }
            catch (Exception er)
            {
                ErrorDialog(er.Message);
            }


        }

        private void Cancel()
        {
            ListDrugFrequency listDrugFrequency = new ListDrugFrequency();
            ChangeViewPermission(listDrugFrequency);
        }
        public void AssingModel(FrequencyDefinitionModel modelData)
        {
            this.model = modelData;
            AssingModelToPropeties();
        }

        void AssingModelToPropeties()
        {
            Code = model.Code;
            FrequencyText = model.Name;
            LocalFrequencyText = model.Comments;
            AmountPerTimes = model.AmountPerTimes;
            ActiveFrom = model.ActiveFrom;
            ActiveTo = model.ActiveTo;
        }

        void AssingPropertiesToModel()
        {
            if (model == null)
            {
                model = new FrequencyDefinitionModel();
            }
            model.Code = Code;
            model.Name = FrequencyText;
            model.Comments = LocalFrequencyText;
            model.AmountPerTimes = AmountPerTimes;
            model.ActiveFrom = ActiveFrom;
            model.ActiveTo = ActiveTo;
        }

        #endregion


    }
}
