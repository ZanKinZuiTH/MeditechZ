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
    public class ProgressNoteViewModel : MediTechViewModelBase
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
        public ProgressNoteModel Model { get; set; }

        private string _Note;

        public string Note
        {
            get { return _Note; }
            set { Set(ref _Note , value); }
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
                if (string.IsNullOrEmpty(Note))
                {
                    WarningDialog("กรุณาใส่ Note");
                    return;
                }
                AssignPropertiesModel();
                DataService.PatientHistory.ManageProgressNote(Model, AppUtil.Current.UserID);
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


        public void AssignPropertiesModel()
        {
            if (Model == null)
            {
                Model = new ProgressNoteModel();
            }


            Model.Note = Note.Trim();
            Model.PatientUID = SelectPatientVisit.PatientUID;
            Model.PatientVisitUID = SelectPatientVisit.PatientVisitUID;
        }
        public void AssignModelToProperties(PatientVisitModel visitModel,ProgressNoteModel progressNote = null)
        {
            SelectPatientVisit = visitModel;
            if (progressNote != null)
            {
                Model = progressNote;
                Note = Model.Note;
            }

        }

        #endregion


    }
}
