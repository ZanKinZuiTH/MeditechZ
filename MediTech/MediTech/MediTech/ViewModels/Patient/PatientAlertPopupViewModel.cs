using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class PatientAlertPopupViewModel : MediTechViewModelBase
    {
        #region Properties

        private List<PatientAlertModel> _PatientAlertLists;

        public List<PatientAlertModel> PatientAlertLists
        {
            get { return _PatientAlertLists; }
            set { Set(ref _PatientAlertLists, value); }
        }


        #endregion

        #region Command

        private RelayCommand _CloseCommand;

        public RelayCommand CloseCommand
        {
            get { return _CloseCommand ?? (_CloseCommand = new RelayCommand(Close)); }
        }

        #endregion

        #region Method

        void Close()
        {
            this.CloseViewDialog(ActionDialog.Cancel);
        }
        public void AssignModel(List<PatientAlertModel> patientAlerts)
        {
            PatientAlertLists = patientAlerts;
        }

        #endregion
    }
}
