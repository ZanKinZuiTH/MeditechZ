using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech
{
    public enum ActionDialog
    {
        Save,
        Cancel
    }

    public enum ActionLogin
    {
        Login,
        Cancel,
        Error
    }

    public enum PageRegister
    {
        Search,
        Manage,
        CreateVisit,
    }

    public enum RegionName
    {
        NEW_WINDOW_REGION,
        APP_MAIN_AREA
    }

    public enum PatientStatusType
    {
        Arrive,
        SendToDoctor,
        MedicalDischarge,
        WardArrived
    }
}
