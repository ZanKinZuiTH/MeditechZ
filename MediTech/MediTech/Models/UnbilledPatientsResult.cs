using MediTech.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MediTech.Models
{
    public class UnbilledPatientsResult : PatientVisitModel
    {


        private Visibility _UnlockVisibility;

        public Visibility UnlockVisibility
        {
            get { return _UnlockVisibility; }
            set { _UnlockVisibility = value; OnPropertyRaised("UnlockVisibility"); }
        }


        private Visibility _LockVisibility;

        public Visibility LockVisibility
        {
            get { return _LockVisibility; }
            set { _LockVisibility = value; OnPropertyRaised("LockVisibility"); }
        }

    }
}
