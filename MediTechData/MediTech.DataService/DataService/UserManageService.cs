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

        public List<RoleProfileModel> GetRoleProfileByLoginUID(int loginUID)
        {
            string requestApi = string.Format("Api/UserManage/GetRoleProfileByLoginUID?loginUID={0}", loginUID);
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

        #region Blife
        public bool BLIFEVerifyPatientIdentity(long patientUID, string natinalID, int userID)
        {
            bool flag = false;

            try
            {
                string requestApi = string.Format("Api/UserManage/BLIFEVerifyPatientIdentity?patientUID={0}&natinalID={1}&userID={1}", patientUID,natinalID, userID);
                MeditechApiHelper.Put(requestApi);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }

            return flag;

        }
        #endregion

    }
}
