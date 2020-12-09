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

        public void SaveOccmedExamination(RequestDetailItemModel requestDetails, int userID)
        {
            try
            {
                string requestApi = string.Format("Api/Checkup/SaveOccmedExamination?userID={0}", userID);
                MeditechApiHelper.Post<RequestDetailItemModel>(requestApi, requestDetails);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public bool CancelOccmedResult(long requestDetailUID, int userUID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/Checkup/CancelOccmedResult?requestDetailUID={0}&userUID={1}", requestDetailUID, userUID);
                MeditechApiHelper.Put(requestApi);
                flag = true;
            }
            catch (Exception)
            {
                throw;
            }
            return flag;
        }

        public List<CheckupRuleModel> GetCheckupRuleByGroup(int GPRSTUID)
        {
            string requestApi = string.Format("Api/Checkup/GetCheckupRuleByGroup?GPRSTUID={0}", GPRSTUID);
            List<CheckupRuleModel> listData = MeditechApiHelper.Get<List<CheckupRuleModel>>(requestApi);

            return listData;
        }

        public void AddCheckupRule(CheckupRuleModel chekcupRuleModel, int userID)
        {
            try
            {
                string requestApi = string.Format("Api/Checkup/AddCheckupRule?userID={0}", userID);
                MeditechApiHelper.Post<CheckupRuleModel>(requestApi, chekcupRuleModel);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool DeleteCheckupRule(int chekcupRuleUID, int userID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/Checkup/DeleteCheckupRule?chekcupRuleUID={0}&userID={1}", chekcupRuleUID, userID);
                MeditechApiHelper.Delete(requestApi);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }
            return flag;
        }

        public List<CheckupRuleItemModel> GetCheckupRuleItemByRuleUID(int chekcupRuleUID)
        {
            string requestApi = string.Format("Api/Checkup/GetCheckupRuleItemByRuleUID?chekcupRuleUID={0}", chekcupRuleUID);
            List<CheckupRuleItemModel> listData = MeditechApiHelper.Get<List<CheckupRuleItemModel>>(requestApi);

            return listData;
        }

        public void AddCheckupRuleItem(CheckupRuleItemModel chekcupRuleItemModel, int userID)
        {
            try
            {
                string requestApi = string.Format("Api/Checkup/AddCheckupRuleItem?userID={0}", userID);
                MeditechApiHelper.Post<CheckupRuleItemModel>(requestApi, chekcupRuleItemModel);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool DeleteCheckupRuleItem(int chekcupRuleItemUID, int userID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/Checkup/DeleteCheckupRuleItem?chekcupRuleItemUID={0}&userID={1}", chekcupRuleItemUID, userID);
                MeditechApiHelper.Delete(requestApi);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }
            return flag;
        }

        public List<CheckupTextMasterModel> GetCheckupTextMaster()
        {
            string requestApi = string.Format("Api/Checkup/GetCheckupTextMaster");
            List<CheckupTextMasterModel> listData = MeditechApiHelper.Get<List<CheckupTextMasterModel>>(requestApi);

            return listData;
        }

        public CheckupTextMasterModel SaveCheckupTextMaster(CheckupTextMasterModel checkupTextMasterModel)
        {
            CheckupTextMasterModel returnData;
            try
            {
                string requestApi = string.Format("Api/Checkup/SaveCheckupTextMaster");
                returnData = MeditechApiHelper.Post<CheckupTextMasterModel, CheckupTextMasterModel>(requestApi, checkupTextMasterModel);
            }
            catch (Exception)
            {

                throw;
            }
            return returnData;
        }

        public bool DeleteCheckupTextMaster(int checkupTextMasterUID, int userUID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/Checkup/DeleteCheckupTextMaster?checkupTextMasterUID={0}&userUID={1}", checkupTextMasterUID, userUID);
                MeditechApiHelper.Delete(requestApi);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }
            return flag;
        }

        public List<CheckupRuleDescriptionModel> GetCheckupRuleDescriptionByRuleUID(int chekcupRuleUID)
        {
            string requestApi = string.Format("Api/Checkup/GetCheckupRuleDescriptionByRuleUID?chekcupRuleUID={0}", chekcupRuleUID);
            List<CheckupRuleDescriptionModel> listData = MeditechApiHelper.Get<List<CheckupRuleDescriptionModel>>(requestApi);

            return listData;
        }

        public List<CheckupRuleRecommendModel> GetCheckupRuleRecommendModelByRuleUID(int chekcupRuleUID)
        {
            string requestApi = string.Format("Api/Checkup/GetCheckupRuleRecommendModelByRuleUID?chekcupRuleUID={0}", chekcupRuleUID);
            List<CheckupRuleRecommendModel> listData = MeditechApiHelper.Get<List<CheckupRuleRecommendModel>>(requestApi);

            return listData;
        }

        public void AddCheckupRuleDescription(CheckupRuleDescriptionModel chekcupDescription, int userID)
        {
            try
            {
                string requestApi = string.Format("Api/Checkup/AddCheckupRuleDescription?userID={0}", userID);
                MeditechApiHelper.Post<CheckupRuleDescriptionModel>(requestApi, chekcupDescription);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void AddCheckupRuleRecommend(CheckupRuleRecommendModel chekcupRecommend, int userID)
        {
            try
            {
                string requestApi = string.Format("Api/Checkup/AddCheckupRuleRecommend?userID={0}", userID);
                MeditechApiHelper.Post<CheckupRuleRecommendModel>(requestApi, chekcupRecommend);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool DeleteCheckupRuleDescription(int checkupRuleDescriptionUID, int userUID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/Checkup/DeleteCheckupRuleDescription?checkupRuleDescriptionUID={0}&userUID={1}", checkupRuleDescriptionUID, userUID);
                MeditechApiHelper.Delete(requestApi);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }
            return flag;
        }

        public bool DeleteCheckupRuleRecommend(int checkupRuleRecommendUID, int userUID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/Checkup/DeleteCheckupRuleRecommend?checkupRuleRecommendUID={0}&userUID={1}", checkupRuleRecommendUID, userUID);
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
