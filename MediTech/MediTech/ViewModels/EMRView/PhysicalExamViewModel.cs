using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using MediTech.Model;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using MediTech.DataService;
using MediTech.Views;
using DevExpress.XtraRichEdit.API.Native;

namespace MediTech.ViewModels
{
    public class PhysicalExamViewModel : MediTechViewModelBase
    {
        #region Properties

        public Document Document { get; set; }

        private PatientVisitModel _SelectPatientVisit;

        public PatientVisitModel SelectPatientVisit
        {
            get { return _SelectPatientVisit; }
            set
            {
                Set(ref _SelectPatientVisit, value);
                //if (SelectPatientVisit != null)
                //{
                //    model = DataService.PatientHistory.GetPhysicalExamByVisit(SelectPatientVisit.PatientVisitUID);
                //    AssignModelToProperties();
                //}

            }
        }

        #endregion

        #region Command
        private RelayCommand _AddToFavoruiteCommand;

        public RelayCommand AddToFavoruiteCommand
        {
            get { return _AddToFavoruiteCommand ?? (_AddToFavoruiteCommand = new RelayCommand(AddToFavoruite)); }
        }


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

        private PhysicalExamModel _PhysicalExamModel;

        public PhysicalExamModel PhysicalExamModel
        {
            get { return _PhysicalExamModel; }
            set
            {
                Set(ref _PhysicalExamModel, value);
            }
        }

        public void AssingPatientVisit(PatientVisitModel visitModel, PhysicalExamModel physicalExamModel = null)
        {
            SelectPatientVisit = visitModel;
            this.PhysicalExamModel = physicalExamModel;
            AssignModelToProperties();
        }
        public void AddToFavoruite()
        {
            try
            {
                string rtfContent = Document.GetRtfText(Document.Range);
                CustomTemplate pageview = new CustomTemplate("PE", rtfContent);
                CustomTemplateViewModel result = (CustomTemplateViewModel)LaunchViewDialogNonPermiss(pageview, true);
                if (result.ResultDialog == ActionDialog.Save)
                {
                    if (result.ActionType == "UseTemplate")
                    {
                        Document.RtfText = result.SelectCCHPIMaster.Description;
                    }
                }
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }


        public void Save()
        {
            try
            {
                if (this.View is PhysicalExam)
                {
                    AssignPropertiesModel();
                    DataService.PatientHistory.ManagePhysicalExam(PhysicalExamModel, AppUtil.Current.UserID);
                    SaveSuccessDialog();
                    CloseViewDialog(ActionDialog.Save);
                }

            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }

        }

        public void Cancel()
        {
            CloseViewDialog(ActionDialog.Cancel);
        }
        public void AssignPropertiesModel()
        {
            if (PhysicalExamModel == null)
            {
                PhysicalExamModel = new PhysicalExamModel();
                PhysicalExamModel.PatientUID = SelectPatientVisit.PatientUID;
                PhysicalExamModel.PatientVisitUID = SelectPatientVisit.PatientVisitUID;
            }

            string rtfContent = Document.GetRtfText(Document.Range);
            string plainText = Document.GetText(Document.Range);

            PhysicalExamModel.PlainText = plainText;
            PhysicalExamModel.Value = rtfContent;

        }
        public void AssignModelToProperties()
        {
            if (PhysicalExamModel != null)
            {
                Document.RtfText = PhysicalExamModel.Value;
            }

        }
        #endregion
    }
}
