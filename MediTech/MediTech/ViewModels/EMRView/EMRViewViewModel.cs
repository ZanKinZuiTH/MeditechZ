using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class EMRViewViewModel : MediTechViewModelBase
    {

        #region Properties

        private ObservableCollection<PatientVisitModel> _PatientVisitLists;

        public ObservableCollection<PatientVisitModel> PatientVisitLists
        {
            get { return _PatientVisitLists; }
            set { Set(ref _PatientVisitLists, value); }
        }

        private PatientVisitModel _SelectPatientVisit;

        public PatientVisitModel SelectPatientVisit
        {
            get { return _SelectPatientVisit; }
            set
            {
                Set(ref _SelectPatientVisit, value);
            }
        }

        private PatientVisitModel _PatientBanner;

        public PatientVisitModel PatientBanner
        {
            get { return _PatientBanner; }
            set
            {
                Set(ref _PatientBanner, value);
            }
        }
        #endregion

        #region Command

        #endregion

        #region Method

        public void AssingPatientVisit(PatientVisitModel visitModel)
        {
            PatientBanner = visitModel;
            var dataVisitList = DataService.PatientIdentity.GetPatientVisitByPatientUID(visitModel.PatientUID);
            foreach (var item in dataVisitList)
            {
                item.Comments = item.StartDttm.Value.ToString("dd MMM yyyy HH:mm") + " / " + item.VisitID + " / " + item.OwnerOrganisation;
            }
            PatientVisitLists = new ObservableCollection<PatientVisitModel>(dataVisitList.OrderByDescending(p => p.StartDttm).ToList());
            PatientVisitLists.Add(new PatientVisitModel { PatientUID = visitModel.PatientUID,PatientVisitUID = 0, Comments = "All" });

            SelectPatientVisit = PatientVisitLists.FirstOrDefault(p => p.PatientVisitUID == visitModel.PatientVisitUID);
        }

        #endregion
    }
}
