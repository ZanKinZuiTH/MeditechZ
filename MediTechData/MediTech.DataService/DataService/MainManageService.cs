using MediTech.Model;
using ShareLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.DataService
{
    public class MainManageService
    {
        #region DesktopApp
        public List<PageViewModuleModel> GetPageViewModule()
        {
            string requestApi = "Api/Main/GetPageViewModule";
            List<PageViewModuleModel> listPageViewModule = MeditechApiHelper.Get<List<PageViewModuleModel>>(requestApi);

            return listPageViewModule;
        }

        public List<PageViewModel> GetPageView(int roleUID,string mode)
        {
            string requestApi = string.Format("Api/Main/GetPageView?roleUID={0}&mode={1}", roleUID,mode);
            List<PageViewModel> listPageView = MeditechApiHelper.Get<List<PageViewModel>>(requestApi);

            return listPageView;
        }

        #endregion
    }
}
