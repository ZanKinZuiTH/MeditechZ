using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class PatientTrackingViewModel : MediTechViewModelBase
    {
        #region Properties

        private List<PatientServiceEventModel> _PatientTracking;
        public List<PatientServiceEventModel> PatientTracking
        {
            get { return _PatientTracking; }
            set { Set(ref _PatientTracking, value); }
        }

        private PatientVisitModel _SelectPatientVisit;
        public PatientVisitModel SelectPatientVisit
        {
            get { return _SelectPatientVisit; }
            set { Set(ref _SelectPatientVisit, value); }
        }

        private PatientVisitModel _PatientVisit;
        public PatientVisitModel PatientVisit
        {
            get { return _PatientVisit; }
            set { Set(ref _PatientVisit, value); 
                if(PatientVisit != null)
                {
                    PatientTracking = DataService.PatientIdentity.GetPatientServiceEventByUID(PatientVisit.PatientVisitUID);
                }
            }
        }

        #endregion

        #region Command
        #endregion

        #region Medthod
        public void AssingModel(PatientVisitModel model)
        {
            PatientVisit = model;
            SelectPatientVisit = model;
        }

        #endregion
    }
}
