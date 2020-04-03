using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Model.Report;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class RegistrationReportViewModel : ReportViewModelBase
    {
        #region Method

        public RegistrationReportViewModel()
        {
            ListReports = DataService.RoleManage.GetReportGroupPermission(AppUtil.Current.RoleUID, "RegistrationReport");

        }

        #endregion
    }
}
