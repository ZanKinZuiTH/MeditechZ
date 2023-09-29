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

        public List<CheckupJobContactModel> SearchCheckupJobContactActive(int? payorDetailUID, DateTime? startDate, DateTime? endDate)
        {
            string requestApi = string.Format("Api/Checkup/SearchCheckupJobContactActive?payorDetailUID={0}&startDate={1:MM/dd/yyyy}&endDate={2:MM/dd/yyyy}", payorDetailUID, startDate, endDate);
            List<CheckupJobContactModel> data = MeditechApiHelper.Get<List<CheckupJobContactModel>>(requestApi);
            return data;
        }

        public List<CheckupJobContactModel> GetCheckupJobContactAllActive()
        {
            string requestApi = string.Format("Api/Checkup/GetCheckupJobContactAllActive");
            List<CheckupJobContactModel> data = MeditechApiHelper.Get<List<CheckupJobContactModel>>(requestApi);
            return data;
        }

        public List<ResultComponentModel> GetResultItemByRequestDetailUID(long requestDetailUID)
        {
            string requestApi = string.Format("Api/Checkup/GetResultItemByRequestDetailUID?requestDetailUID={0}", requestDetailUID);
            List<ResultComponentModel> listData = MeditechApiHelper.Get<List<ResultComponentModel>>(requestApi);

            return listData;
        }

        public List<CheckupJobTaskModel> GetCheckupJobTaskByJobUID(int checkupJobConatctUID)
        {
            string requestApi = string.Format("Api/Checkup/GetCheckupJobTaskByJobUID?checkupJobConatctUID={0}", checkupJobConatctUID);
            List<CheckupJobTaskModel> listData = MeditechApiHelper.Get<List<CheckupJobTaskModel>>(requestApi);

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

        public List<RequestListModel> SearchCheckupExamList(DateTime? requestDateFrom, DateTime? requestDateTo, long? patientUID, int? insuranceCompanyUID, int? checkupJobUID, int? PRTGPUID,int? requestItemUID)
        {
            string requestApi = string.Format("Api/Checkup/SearchCheckupExamList?requestDateFrom={0:MM/dd/yyyy}&requestDateTo={1:MM/dd/yyyy}&patientUID={2}&insuranceCompanyUID={3}&checkupJobUID={4}&PRTGPUID={5}&requestItemUID={6}", requestDateFrom, requestDateTo, patientUID, insuranceCompanyUID, checkupJobUID, PRTGPUID, requestItemUID);
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

        public void SaveCheckupRule(CheckupRuleModel chekcupRuleModel, int userID)
        {
            try
            {
                string requestApi = string.Format("Api/Checkup/SaveCheckupRule?userID={0}", userID);
                MeditechApiHelper.Post<CheckupRuleModel>(requestApi, chekcupRuleModel);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void CopyCheckupRule(CheckupRuleModel chekcupRuleModel, int userID)
        {
            try
            {
                string requestApi = string.Format("Api/Checkup/CopyCheckupRule?userID={0}", userID);
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

        public void SaveCheckupRuleItem(CheckupRuleItemModel chekcupRuleItemModel, int userID)
        {
            try
            {
                string requestApi = string.Format("Api/Checkup/SaveCheckupRuleItem?userID={0}", userID);
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


        public List<PatientVisitModel> SearchPatientCheckup(DateTime? dateFrom, DateTime? dateTo, long? patientUID, int? insuranceCompanyUID, int? checkupJobUID, bool isLoadDataBlife = false)
        {
            string requestApi = string.Format("Api/Checkup/SearchPatientCheckup?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&patientUID={2}&insuranceCompanyUID={3}&checkupJobUID={4}&isLoadDataBlife={5}", dateFrom,dateTo,patientUID, insuranceCompanyUID, checkupJobUID, isLoadDataBlife);
            List<PatientVisitModel> data = MeditechApiHelper.Get<List<PatientVisitModel>>(requestApi);

            return data;
        }
        public List<LookupReferenceValueModel> GetCompanyBranchByCheckJob(int checkupJobUID)
        {
            string requestApi = string.Format("Api/Checkup/GetCompanyBranchByCheckJob?checkupJobUID={0}", checkupJobUID);
            List<LookupReferenceValueModel> data = MeditechApiHelper.Get<List<LookupReferenceValueModel>>(requestApi);

            return data;
        }

        public List<PatientVisitModel> GetVisitCheckupGroup(int checkupJobUID, List<int> GPRSTUIDs)
        {
            string requestApi = string.Format("Api/Checkup/GetVisitCheckupGroup?checkupJobUID={0}", checkupJobUID);
            List<PatientVisitModel> data = MeditechApiHelper.Post<List<int>, List<PatientVisitModel>>(requestApi, GPRSTUIDs);

            return data;
        }

        public List<PatientVisitModel> GetVisitCheckupGroupNonTran(int checkupJobUID, List<int> GPRSTUIDs)
        {
            string requestApi = string.Format("Api/Checkup/GetVisitCheckupGroupNonTran?checkupJobUID={0}", checkupJobUID);
            List<PatientVisitModel> data = MeditechApiHelper.Post<List<int>, List<PatientVisitModel>>(requestApi, GPRSTUIDs);

            return data;
        }

        public List<PatientVisitModel> GetVisitCheckupGroupNonConfirm(int checkupJobUID, List<int> GPRSTUIDs)
        {
            string requestApi = string.Format("Api/Checkup/GetVisitCheckupGroupNonConfirm?checkupJobUID={0}", checkupJobUID);
            List<PatientVisitModel> data = MeditechApiHelper.Post<List<int>, List<PatientVisitModel>>(requestApi, GPRSTUIDs);

            return data;
        }

        public List<LookupReferenceValueModel> GetCheckupGroupByVisitUID(long patientVisitUID)
        {
            string requestApi = string.Format("Api/Checkup/GetCheckupGroupByVisitUID?patientVisitUID={0}", patientVisitUID);
            List<LookupReferenceValueModel> data = MeditechApiHelper.Get<List<LookupReferenceValueModel>>(requestApi);

            return data;
        }
        public List<CheckupRuleModel> GetCheckupRuleGroupList(List<int> GPRSTUIDs)
        {
            string requestApi = string.Format("Api/Checkup/GetCheckupRuleGroupList");
            List<CheckupRuleModel> data = MeditechApiHelper.Post<List<int>, List<CheckupRuleModel>>(requestApi, GPRSTUIDs);

            return data;
        }

        public List<ResultComponentModel> GetGroupResultComponentByVisitUID(long patientVisitUID, int GPRSTUID)
        {
            string requestApi = string.Format("Api/Checkup/GetGroupResultComponentByVisitUID?patientVisitUID={0}&GPRSTUID={1}", patientVisitUID, GPRSTUID);
            List<ResultComponentModel> data = MeditechApiHelper.Get<List<ResultComponentModel>>(requestApi);

            return data;
        }

        public List<ResultComponentModel> GetResultComponentByVisitUID(long patientVisitUID)
        {
            string requestApi = string.Format("Api/Checkup/GetResultComponentByVisitUID?patientVisitUID={0}", patientVisitUID);
            List<ResultComponentModel> data = MeditechApiHelper.Get<List<ResultComponentModel>>(requestApi);

            return data;
        }

        public List<ResultComponentModel> GetCheckupMobileResultByVisitUID(long patientUID, long patientVisitUID)
        {
            string requestApi = string.Format("Api/Checkup/GetCheckupMobileResultByVisitUID?patientUID={0}&patientVisitUID={1}", patientUID, patientVisitUID);
            List<ResultComponentModel> data = MeditechApiHelper.Get<List<ResultComponentModel>>(requestApi);

            return data;
        }

        public List<PatientResultCheckupModel> GetCheckupGroupResultByJob(CheckupCompanyModel branchModel)
        {
            string requestApi = string.Format("Api/Checkup/GetCheckupGroupResultByJob");
            List<PatientResultCheckupModel> data = MeditechApiHelper.Post<CheckupCompanyModel, List<PatientResultCheckupModel>>(requestApi, branchModel);

            return data;
        }

        public List<PatientResultComponentModel> GetResultCumulative(long patientUID, long patientVisitUID, int requestItemUID)
        {
            string requestApi = string.Format("Api/Checkup/GetResultCumulative?patientUID={0}&patientVisitUID={1}&requestItemUID={2}", patientUID, patientVisitUID, requestItemUID);
            List<PatientResultComponentModel> data = MeditechApiHelper.Get<List<PatientResultComponentModel>>(requestApi);

            return data;
        }

        public List<PatientResultComponentModel> GetGroupResultCumulative(long patientUID, long patientVisitUID, int GPRSTUID,int? payorDetailUID)
        {
            string requestApi = string.Format("Api/Checkup/GetGroupResultCumulative?patientUID={0}&patientVisitUID={1}&GPRSTUID={2}&payorDetailUID={3}", patientUID, patientVisitUID, GPRSTUID, payorDetailUID);
            List<PatientResultComponentModel> data = MeditechApiHelper.Get<List<PatientResultComponentModel>>(requestApi);

            return data;
        }

        public List<PatientResultComponentModel> GetVitalSignCumulative(long patientUID, long patientVisitUID)
        {
            string requestApi = string.Format("Api/Checkup/GetVitalSignCumulative?patientUID={0}&patientVisitUID={1}", patientUID, patientVisitUID);
            List<PatientResultComponentModel> data = MeditechApiHelper.Get<List<PatientResultComponentModel>>(requestApi);

            return data;
        }

        public CheckupGroupResultModel GetCheckupGroupResultByVisit(long patientVisitUID, int GPRSTUID)
        {
            string requestApi = string.Format("Api/Checkup/GetCheckupGroupResultByVisit?patientVisitUID={0}&GPRSTUID={1}", patientVisitUID, GPRSTUID);
            CheckupGroupResultModel data = MeditechApiHelper.Get<CheckupGroupResultModel>(requestApi);

            return data;
        }

        public List<CheckupGroupResultModel> GetCheckupGroupResultListByVisit(long patientUID, long patientVisitUID)
        {
            string requestApi = string.Format("Api/Checkup/GetCheckupGroupResultListByVisit?patientUID={0}&patientVisitUID={1}", patientUID, patientVisitUID);
            List<CheckupGroupResultModel> data = MeditechApiHelper.Get<List<CheckupGroupResultModel>>(requestApi);

            return data;
        }

        public void SaveCheckupGroupResult(CheckupGroupResultModel groupResult, int userUID)
        {
            try
            {
                string requestApi = string.Format("Api/Checkup/SaveChekcupGroupResult?userUID={0}", userUID);
                MeditechApiHelper.Post<CheckupGroupResultModel>(requestApi, groupResult);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void TranslateCheckupAll(int checkupJobUID, int userUID, List<int> GPRSTUIDs)
        {
            try
            {
                string requestApi = string.Format("Api/Checkup/TranslateCheckupAll?checkupJobUID={0}&userUID={1}", checkupJobUID, userUID);
                MeditechApiHelper.Post<List<int>>(requestApi, 15, GPRSTUIDs);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void SaveOldLabResult(RequestDetailItemModel resultItemRange, long patientUID, int payorDetailUID, DateTime enterDate, string codeLab, int userID, int organisationUID, int payorAgreementsUID,int? locationUID)
        {
            try
            {
                string requestApi = string.Format("Api/Checkup/SaveOldLabResult?patientUID={0}&payorDetailUID={1}&enterDate={2}&codeLab={3}&userID={4}&organisationUID={5}&payorAgreementsUID={6}&locationUID={7}", patientUID, payorDetailUID, enterDate, codeLab, userID, organisationUID, payorAgreementsUID, locationUID);
                MeditechApiHelper.Post<RequestDetailItemModel>(requestApi, resultItemRange);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void SaveRiskHistory(PatientMedicalHistoryModel patientMedical, long patientUID, int userID)
        {
            try
            {
                string requestApi = string.Format("Api/Checkup/SaveRiskHistory?patientUID={0}&userID={1}", patientUID, userID);
                MeditechApiHelper.Post<PatientMedicalHistoryModel>(requestApi, patientMedical);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void SaveInjuryHistory(PatientInjuryModel patientInjury, long patientUID, int userID)
        {
            try
            {
                string requestApi = string.Format("Api/Checkup/SaveInjuryHistory?patientUID={0}&userID={1}", patientUID, userID);
                MeditechApiHelper.Post<PatientInjuryModel>(requestApi, patientInjury);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
