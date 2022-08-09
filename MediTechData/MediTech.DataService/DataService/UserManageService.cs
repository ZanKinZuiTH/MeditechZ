using MediTech.Model;
using ShareLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.DataService
{
    public class UserManageService
    {


        public List<CareproviderModel> GetUserByLogin(string userName, string password)
        {
            string requestApi = string.Format("Api/UserManage/GetUserByLogin?userName={0}&password={1}", userName, password);
            List<CareproviderModel> data = MeditechApiHelper.Get<List<CareproviderModel>>(requestApi);
            return data;
        }

        public List<CareproviderModel> CheckUserByLogin(LoginModel loginModel)
        {
            string requestApi = string.Format("Api/UserManage/CheckUserByLogin");
            List<CareproviderModel> data = MeditechApiHelper.Post<LoginModel, List<CareproviderModel>>(requestApi, loginModel);
            return data;
        }
        public void StampAppLastAccess(int loginUID)
        {
            string requestApi = string.Format("Api/UserManage/StampAppLastAccess?loginUID={0}", loginUID);
            MeditechApiHelper.Put(requestApi);
        }

        public void StampWebLastAccess(int loginUID)
        {
            string requestApi = string.Format("Api/UserManage/StampWebLastAccess?loginUID={0}", loginUID);
            MeditechApiHelper.Put(requestApi);
        }

        public List<CareproviderModel> GetCareproviderAll()
        {

            List<CareproviderModel> data = MeditechApiHelper.Get<List<CareproviderModel>>("Api/UserManage/GetCareproviderAll");
            return data;
        }

        public CareproviderModel GetCareproviderByUID(int careproviderUID)
        {
            string requestApi = string.Format("Api/UserManage/GetCareproviderByUID?careproviderUID={0}", careproviderUID);
            CareproviderModel data = MeditechApiHelper.Get<CareproviderModel>(requestApi);

            return data;
        }


        public CareproviderModel GetCareproviderByCode(string careproviderCode)
        {
            string requestApi = string.Format("Api/UserManage/GetCareproviderByCode?careproviderCode={0}", careproviderCode);
            CareproviderModel data = MeditechApiHelper.Get<CareproviderModel>(requestApi);

            return data;
        }

        public List<RoleProfileModel> GetRoleProfileByLoginUID(int loginUID, int organisationUID)
        {
            string requestApi = string.Format("Api/UserManage/GetRoleProfileByLoginUID?loginUID={0}&organisationUID={1}", loginUID, organisationUID);
            List<RoleProfileModel> data = MeditechApiHelper.Get<List<RoleProfileModel>>(requestApi);

            return data;
        }

        public bool ManageCareProvider(CareproviderModel careproviderData, int userID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/UserManage/ManageCareProvider?userID={0}", userID);
                MeditechApiHelper.Post<CareproviderModel>(requestApi, careproviderData);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }

            return flag;
        }


        public bool ChangePassword(LoginModel loginModel)
        {
            bool flag = false;
            try
            {
                MeditechApiHelper.Post<LoginModel>("Api/UserManage/ChangePassword", loginModel);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }
            return flag;
        }

        public bool DeleteCareProvider(int? careproviderUID, int? loginUID, int userID)
        {
            bool flag = false;

            try
            {
                string requestApi = string.Format("Api/UserManage/DeleteCareProvider?careproviderUID={0}&userID={2}", careproviderUID, loginUID, userID);
                MeditechApiHelper.Delete(requestApi);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }

            return flag;

        }
        public List<CareproviderModel> GetCareProviderByType(int type)
        {
            string requestApi = string.Format("Api/UserManage/GetCareProviderByType?type={0}", type);
            List<CareproviderModel> data = MeditechApiHelper.Get<List<CareproviderModel>>(requestApi);
            return data;
        }

        public List<CareproviderModel> GetCareproviderDoctor()
        {
            string requestApi = "Api/UserManage/GetCareProviderDoctor";
            List<CareproviderModel> data = MeditechApiHelper.Get<List<CareproviderModel>>(requestApi);
            return data;
        }


        public List<CareproviderOrganisationModel> GetCareProviderOrganisation()
        {
            string requestApi = "Api/UserManage/GetCareProviderOrganisation";
            List<CareproviderOrganisationModel> data = MeditechApiHelper.Get<List<CareproviderOrganisationModel>>(requestApi);
            return data;
        }

        public List<CareproviderOrganisationModel> GetCareProviderOrganisation(int organisationUID)
        {
            string requestApi = string.Format("Api/UserManage/GetCareProviderOrganisation?organisationUID={0}", organisationUID);
            List<CareproviderOrganisationModel> data = MeditechApiHelper.Get<List<CareproviderOrganisationModel>>(requestApi);
            return data;
        }

        public List<CareproviderOrganisationModel> GetCareProviderOrganisationByUser(int careproviderUID)
        {
            string requestApi = string.Format("Api/UserManage/GetCareProviderOrganisationByUser?careproviderUID={0}", careproviderUID);
            List<CareproviderOrganisationModel> data = MeditechApiHelper.Get<List<CareproviderOrganisationModel>>(requestApi);
            return data;
        }

        public bool ManageCareProviderOrganisation(CareproviderOrganisationModel careOrgnModel, int userID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/UserManage/ManageCareProviderOrganisation?userID={0}", userID);
                MeditechApiHelper.Post<CareproviderOrganisationModel>(requestApi, careOrgnModel);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }

            return flag;
        }

        public bool DeleteCareproviderOrganisation(int uid, int userID)
        {
            bool flag = false;

            try
            {
                string requestApi = string.Format("Api/UserManage/DeleteCareproviderOrganisation?uid={0}&userID={1}", uid, userID);
                MeditechApiHelper.Delete(requestApi);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }

            return flag;

        }

        public List<CareproviderLocationModel> GetCareProviderLocation(int locationUID)
        {
            string requestApi = string.Format("Api/UserManage/GetCareProviderLocation?locationUID={0}", locationUID);
            List<CareproviderLocationModel> data = MeditechApiHelper.Get<List<CareproviderLocationModel>>(requestApi);
            return data;
        }

        public List<CareproviderLocationModel> GetCareProviderLocationByUser(int careproviderUID, int organisationUID)
        {
            string requestApi = string.Format("Api/UserManage/GetCareProviderLocationByUser?careproviderUID={0}&organisationUID={1}", careproviderUID, organisationUID);
            List<CareproviderLocationModel> data = MeditechApiHelper.Get<List<CareproviderLocationModel>>(requestApi);
            return data;
        }

        public bool ManageCareProviderLocation(CareproviderLocationModel careLocModel, int userID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/UserManage/ManageCareProviderLocation?userID={0}", userID);
                MeditechApiHelper.Post<CareproviderLocationModel>(requestApi, careLocModel);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }

            return flag;
        }

        public bool DeleteCareproviderLocation(int uid, int userID)
        {
            bool flag = false;

            try
            {
                string requestApi = string.Format("Api/UserManage/DeleteCareproviderLocation?uid={0}&userID={1}", uid, userID);
                MeditechApiHelper.Delete(requestApi);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }

            return flag;

        }

    }
}
