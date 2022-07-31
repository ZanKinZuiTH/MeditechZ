using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Interface
{
    public interface IPatientVisitViewModel
    {
        PatientVisitModel SelectedPatientVisit { get; set; }

        void AssignPatientVisit(PatientVisitModel patVisitData);
    }
}
