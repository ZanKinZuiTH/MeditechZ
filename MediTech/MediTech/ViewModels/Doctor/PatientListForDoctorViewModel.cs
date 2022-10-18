using DevExpress.DataAccess.Native.ObjectBinding;
using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels.Doctor
{
    public class PatientListForDoctorViewModel : PatientListViewModel
    {
        #region Properties

        private List<PatientVisitModel> _SelectPatientVisits;

        public List<PatientVisitModel> SelectPatientVisits
        {
            get { return _SelectPatientVisits ?? (_SelectPatientVisits = new List<PatientVisitModel>()); }
            set { Set(ref _SelectPatientVisits,value); }
        }


        #endregion

        #region Command

        private RelayCommand _DiagnosisCommand;

        public RelayCommand DiagnosisCommand
        {
            get { return _DiagnosisCommand ?? (_DiagnosisCommand = new RelayCommand(Diagnosis)); }
        }


        #endregion

        #region Method

        void Diagnosis()
        {
            try
            {
                if (SelectPatientVisits != null && SelectPatientVisits.Count() > 0)
                {
                    PatientDiagnosis pageview = new PatientDiagnosis();
                    (pageview.DataContext as PatientDiagnosisViewModel).IsVisitMass = true;
                    PatientDiagnosisViewModel result = (PatientDiagnosisViewModel)LaunchViewDialog(pageview, "PATDIAG", false);

                    if (result != null)
                    {
                        if (result.ResultDialog == ActionDialog.Save)
                        {
                            List<PatientProblemModel> listProblem = new List<PatientProblemModel>();
                            foreach (var patVisit in SelectPatientVisits)
                            {
                                foreach (var patPromblem in result.PatientProblemList)
                                {
                                    PatientProblemModel newProblem = new PatientProblemModel();
                                    newProblem.PatientUID = patVisit.PatientUID;
                                    newProblem.PatientVisitUID = patVisit.PatientVisitUID;
                                    newProblem.ProblemUID = patPromblem.ProblemUID;
                                    newProblem.ProblemCode = patPromblem.ProblemCode;
                                    newProblem.ProblemName = patPromblem.ProblemName;
                                    newProblem.ProblemDescription = patPromblem.ProblemDescription;
                                    newProblem.DIAGTYPUID = patPromblem.DIAGTYPUID;
                                    newProblem.DiagnosisType = patPromblem.DiagnosisType;
                                    newProblem.IsUnderline = patPromblem.IsUnderline;
                                    newProblem.SEVRTUID = patPromblem.SEVRTUID;
                                    newProblem.CERNTUID = patPromblem.CERNTUID;
                                    newProblem.PBMTYUID = patPromblem.PBMTYUID;
                                    newProblem.BDLOCUID = patPromblem.BDLOCUID;
                                    newProblem.Severity = patPromblem.Severity;
                                    newProblem.Certanity = patPromblem.Certanity;
                                    newProblem.ProblemType = patPromblem.ProblemType;
                                    newProblem.BodyLocation = patPromblem.BodyLocation;
                                    newProblem.OnsetDttm = patPromblem.OnsetDttm;
                                    newProblem.ClosureDttm = patPromblem.ClosureDttm;
                                    newProblem.ClosureComments = patPromblem.ClosureComments;
                                    newProblem.RecordedDttm = patPromblem.RecordedDttm;
                                    newProblem.RecordedName = AppUtil.Current.UserName;
                                    listProblem.Add(newProblem);
                                }
                            }
                            if (listProblem.Count() > 0)
                            {
                                DataService.PatientDiagnosis.ManagePatientProblemMass(listProblem, AppUtil.Current.UserID);
                            }
                            SaveSuccessDialog();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                ErrorDialog(ex.Message);
            }

        }

        #endregion
    }
}
