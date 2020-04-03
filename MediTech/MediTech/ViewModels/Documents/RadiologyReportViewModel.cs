using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class RadiologyReportViewModel : ReportViewModelBase
    {
        public RadiologyReportViewModel()
        {
            ListReports = DataService.RoleManage.GetReportGroupPermission(AppUtil.Current.RoleUID, "RadiologyReport");
        }
    }
}
