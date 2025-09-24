using MediTech.Model;
using ShareLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.DataService
{
    public class RoleManageService
    {

        public List<LookupItemModel> GetLookUpRoleAll()
        {
            List<LookupItemModel> data = MeditechApiHelper.Get<List<LookupItemModel>>("Api/RoleManage/GetLookUpRoleAll");
            return data;
        }



        public List<RoleModel> GetRole()
        {
            List<RoleModel> data = MeditechApiHelper.Get<List<RoleModel>>("Api/RoleManage/GetRole");
            return data;
        }


        public RoleModel GetRoleByUID(int roleUID)
        {
            string requestApi = string.Format("Api/RoleManage/GetRoleByUID?roleUID={0}", roleUID);
            RoleModel data = MeditechApiHelper.Get<RoleModel>(requestApi);
            return data;
        }


        public bool ManageRole(RoleModel roleData, int userID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/RoleManage/ManageRole?userID={0}", userID);
                MeditechApiHelper.Post<RoleModel>(requestApi, roleData);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }

            return flag;
        }
        public bool DeleteRole(int roleUID, int userID)
        {
            bool flag = false;
            try
            {

                string requestApi = string.Format("Api/RoleManage/DeleteRole?roleUID={0}&userID={1}", roleUID, userID);
                MeditechApiHelper.Delete(requestApi);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }

            return flag;
        }

        #region Pageview
        public List<PageViewModel> GetPageViewByPermission(string permission)
        {
            string requestApi = string.Format("Api/RoleManage/GetPageViewByPermission?permission={0}", permission);
            List<PageViewModel> data = MeditechApiHelper.Get<List<PageViewModel>>(requestApi);

            return data;
        }

        public List<SecurityPermissionModel> GetPageViewPermission(int roleUID)
        {
            string requestApi = string.Format("Api/RoleManage/GetPageViewPermission?roleUID={0}", roleUID);
            List<SecurityPermissionModel> data = MeditechApiHelper.Get<List<SecurityPermissionModel>>(requestApi);

            return data;
        }

        #endregion

        #region RoleReportTemplate

        public List<RoleReportPermissionModel> GetReportTemplateByPermission(string permission)
        {
            string requestApi = string.Format("Api/RoleManage/GetReportTemplateByPermission?permission={0}", permission);
            List<RoleReportPermissionModel> data = MeditechApiHelper.Get<List<RoleReportPermissionModel>>(requestApi);
            return data;
        }

        public List<RoleReportPermissionModel> GetReportTemplatePermission(int roleUID)
        {
            string requestApi = string.Format("Api/RoleManage/GetReportTemplatePermission?roleUID={0}", roleUID);
            List<RoleReportPermissionModel> data = MeditechApiHelper.Get<List<RoleReportPermissionModel>>(requestApi);
            return data;
        }

        public List<RoleReportPermissionModel> GetReportGroupPermission(int roleUID,string reportGroup)
        {
            string requestApi = string.Format("Api/RoleManage/GetReportGroupPermission?roleUID={0}&reportGroup={1}", roleUID, reportGroup);
            List<RoleReportPermissionModel> data = MeditechApiHelper.Get<List<RoleReportPermissionModel>>(requestApi);
            return data;
        }

        #endregion
    }
}
