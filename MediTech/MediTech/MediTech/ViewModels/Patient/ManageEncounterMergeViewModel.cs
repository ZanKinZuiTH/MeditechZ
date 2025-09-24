using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MediTech.ViewModels
{
    public class ManageEncounterMergeViewModel : ManagePatientMergeViewModel
    {
        #region Properties

        #endregion

        #region Command


        #endregion

        #region Method

        public ManageEncounterMergeViewModel()
        {
            IsVisibilityPatientMerge = Visibility.Collapsed;
        }
        public override void OnLoaded()
        {
            (this.View as ManagePatientMerge).colSecondarySelect.Visible = true;
        }
        public override void Merge()
        {
            try
            {
                if (SelectedPateintPrimarySearch == null)
                {
                    WarningDialog("กรุณาเลือก Primary Patient");
                    return;
                }
                if (SelectedPateintSecondarySearch == null)
                {
                    WarningDialog("กรุณาเลือก Secondary Patient");
                    return;
                }
                if (VisitSecondaryList != null)
                {
                    if (VisitSecondaryList.Count(p => p.Select) <= 0)
                    {
                        WarningDialog("กรุณาเลือก Visit ที่จะ Merge");
                        return;
                    }
                }
                long majorPatientUID = SelectedPateintPrimarySearch.PatientUID;
                long minorPatientUID = SelectedPateintSecondarySearch.PatientUID;
                string minorVisitUIDS = "";
                foreach (var visit in VisitSecondaryList)
                {
                    if (visit.Select)
                    {
                        minorVisitUIDS += minorVisitUIDS == "" ? visit.PatientVisitUID.ToString() : ","+ visit.PatientVisitUID.ToString();
                    }
                }
                DataService.PatientIdentity.EncounterMergePatient(majorPatientUID, minorPatientUID, minorVisitUIDS, AppUtil.Current.UserID);
                SaveSuccessDialog();
                ChangeViewPermission(new ListPatMerge());
            }
            catch (Exception ex)
            {

                ErrorDialog(ex.Message);
            }
        }
        #endregion
    }
}
