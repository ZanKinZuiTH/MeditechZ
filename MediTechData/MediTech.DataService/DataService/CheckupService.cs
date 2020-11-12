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

        public List<ResultComponentModel> GetResultItemByRequestDetailUID(long requestDetailUID)
        {
            string requestApi = string.Format("Api/Checkup/GetResultItemByRequestDetailUID?requestDetailUID={0}", requestDetailUID);
            List<ResultComponentModel> listData = MeditechApiHelper.Get<List<ResultComponentModel>>(requestApi);

            return listData;
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

        public List<RequestListModel> SearchCheckupExamList(DateTime? requestDateFrom, DateTime? requestDateTo, long? patientUID, int? payorDetailUID, int? checkupJobUID, int? PRTGPUID)
        {
            string requestApi = string.Format("Api/Checkup/SearchCheckupExamList?requestDateFrom={0:MM/dd/yyyy}&requestDateTo={1:MM/dd/yyyy}&patientUID={2}&payorDetailUID={3}&checkupJobUID={4}&PRTGPUID={5}", requestDateFrom, requestDateTo, patientUID, payorDetailUID, checkupJobUID, PRTGPUID);
            List<RequestListModel> data = MeditechApiHelper.Get<List<RequestListModel>>(requestApi);
            return data;
        }

        public void SavePhysicalExamination(RequestDetailItemModel requestDetails, int userID)
        {
            try
            {
                string requestApi = string.Format("Api/Checkup/SavePhysicalExamination?userID={0}", userID);
                MeditechApiHelper.Post<RequestDetailItemModel>(requestApi, requestDetails);
            }
            catch (Exception)
            {

                throw;
            }

        }


    }
}
