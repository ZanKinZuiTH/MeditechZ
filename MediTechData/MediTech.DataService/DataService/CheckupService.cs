using MediTech.Model;
using ShareLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.DataService
{
    public class CheckupService
    {
        public List<CheckupJobContactModel> GetCheckupJobContactAll()
        {
            string requestApi = string.Format("Api/Checkup/GetCheckupJobContactAll");
            List<CheckupJobContactModel> data = MeditechApiHelper.Get<List<CheckupJobContactModel>>(requestApi);
            return data;
        }

        public CheckupJobContactModel GetCheckupJobContactByUID(int checkupJobConatctUID)
        {
            string requestApi = string.Format("Api/Checkup/GetCheckupJobContactByUID?checkupJobConatctUID={0}", checkupJobConatctUID);
            CheckupJobContactModel data = MeditechApiHelper.Get<CheckupJobContactModel>(requestApi);
            return data;
        }
        public List<CheckupJobContactModel> GetCheckupJobContactByPayorDetailUID(int payorDetailUID)
        {
            string requestApi = string.Format("Api/Checkup/GetCheckupJobContactByPayorDetailUID?payorDetailUID={0}", payorDetailUID);
            List<CheckupJobContactModel> data = MeditechApiHelper.Get<List<CheckupJobContactModel>>(requestApi);
            return data;
        }

        public bool DeleteCheckupJobContact(int checkupJobContactUID, int userID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/Checkup/DeleteCheckupJobContact?checkupJobContactUID={0}&userID={1}", checkupJobContactUID, userID);
                MeditechApiHelper.Delete(requestApi);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }
            return flag;
        }

        public bool SaveCheckupJobContact(CheckupJobContactModel checkupJobContactModel, int userID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/Checkup/SaveCheckupJobContact?userID={0}", userID);
                MeditechApiHelper.Post<CheckupJobContactModel>(requestApi, checkupJobContactModel);
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
