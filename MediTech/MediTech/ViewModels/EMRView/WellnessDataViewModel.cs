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
    public class WellnessDataViewModel : MediTechViewModelBase
    {
        #region Properties
        private PatientVisitModel _SelectPatientVisit;

        public PatientVisitModel SelectPatientVisit
        {
            get { return _SelectPatientVisit; }
            set
            {
                Set(ref _SelectPatientVisit, value);
            }
        }
        public WellnessDataModel Model { get; set; }

        private string _Diagnosis;

        public string Diagnosis
        {
            get { return _Diagnosis; }
            set { Set(ref _Diagnosis, value); }
        }


        private string _WellnessResult;

        public string WellnessResult
        {
            get { return _WellnessResult; }
            set { Set(ref _WellnessResult, value); }
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

        private void Save()
        {
            try
            {
                if (string.IsNullOrEmpty(Diagnosis) && string.IsNullOrEmpty(WellnessResult))
                {
                    WarningDialog("กรุณาใส่ ข้อมูล");
                    return;
                }
                AssignPropertiesModel();
                DataService.PatientHistory.ManageWellnessData(Model, AppUtil.Current.UserID);
                CloseViewDialog(ActionDialog.Save);
            }
            catch (Exception ex)
            {

                ErrorDialog(ex.Message);
            }

        }

        private void Cancel()
        {
            CloseViewDialog(ActionDialog.Cancel);
        }


        public void AssingPatientVisit(PatientVisitModel visitModel,WellnessDataModel model = null)
        {
            SelectPatientVisit = visitModel;
            this.Model = model;
            AssignModelToProperties();
        }
        private void AssignPropertiesModel()
        {
            if (Model == null)
            {
                Model = new WellnessDataModel();
                Model.PatientUID = SelectPatientVisit.PatientUID;
                Model.PatientVisitUID = SelectPatientVisit.PatientVisitUID;
            }


            Model.WellnessResult = WellnessResult.Trim();
        }

        private void AssignModelToProperties()
        {
            if (Model != null)
            {
                WellnessResult = Model.WellnessResult;
            }

        }
        #endregion
    }
}
