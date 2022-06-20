using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class GenerateBillViewModel : MediTechViewModelBase
    {
        #region Properties

        private PatientVisitModel _SelectPateintVisit;

        public PatientVisitModel SelectPateintVisit
        {
            get { return _SelectPateintVisit; }
            set { Set(ref _SelectPateintVisit, value); }
        }


        #endregion

        #region Command

        #endregion

        #region Method

        #endregion
    }
}
