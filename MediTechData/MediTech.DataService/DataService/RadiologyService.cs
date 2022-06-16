using MediTech.Model;
using MediTech.Model.Report;
using ShareLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace MediTech.DataService
{
    public class RadiologyService
    {
        public List<LookupItemModel> GetLookUpRadiologist()
        {
            var data = MeditechApiHelper.Get<List<LookupItemModel>>("Api/Radiology/GetLookUpRadiologist");
            return data;
        }

        public List<RequestListModel> SearchRequestExamListForAssign(DateTime? dateFrom, DateTime? dateTo, int? organisationUID, long? patientUID, string requestItemName
            , int? RIMTYPUID, int? locationUID, int? insuranceCompanyUID, int? ORDSTUID)
        {
            string requestApi = string.Format("Api/Radiology/SearchRequestExamListForAssign?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&organisationUID={2}&patientUID={3}&insuranceCompanyUID={4}&requestItemName={4}&RIMTYPUID={5}&locationUID={6}&insuranceCompanyUID={7}&ORDSTUID={8}", dateFrom, dateTo, organisationUID, patientUID, requestItemName, RIMTYPUID, locationUID, insuranceCompanyUID, ORDSTUID);
            List<RequestListModel> listData = MeditechApiHelper.Get<List<RequestListModel>>(requestApi);
            return listData;
        }

        public List<RequestListModel> SearchRequestExamList(DateTime? requestDateFrom, DateTime? requestDateTo, string statusList, int? RQPRTUID, long? patientUID, string orderName, int? RIMTYPUID, int? radiologistUID, int? rduStaffUID, int? payorDetailUID, int? organisationUID)
        {
            string requestApi = string.Format("Api/Radiology/SearchRequestExamList?requestDateFrom={0:MM/dd/yyyy}&requestDateTo={1:MM/dd/yyyy}&statusList={2}&RQPRTUID={3}&patientUID={4}&orderName={5}&RIMTYPUID={6}&radiologistUID={7}&rduStaffUID={8}&payorDetailUID={9}&organisationUID={10}", requestDateFrom, requestDateTo, statusList, RQPRTUID, patientUID, orderName, RIMTYPUID, radiologistUID, rduStaffUID, payorDetailUID, organisationUID);
            List<RequestListModel> listData = MeditechApiHelper.Get<List<RequestListModel>>(requestApi);
            return listData;
        }

        public List<RequestListModel> GetRequestExecuteRadiologist(DateTime requestDateFrom, DateTime requestDateTo, int radiologistUID)
        {
            string requestApi = string.Format("Api/Radiology/GetRequestExecuteRadiologist?requestDateFrom={0:MM/dd/yyyy}&requestDateTo={1:MM/dd/yyyy}&radiologistUID={2}", requestDateFrom, requestDateTo, radiologistUID);
            List<RequestListModel> listData = MeditechApiHelper.Get<List<RequestListModel>>(requestApi);
            return listData;
        }
        public PatientRequestReportModel GetPatientRequestReport(long patientUID, long requestUID, long requestDetailUID)
        {
            string requestApi = string.Format("Api/Radiology/GetPatientRequestReport?patientUID={0}&requestUID={1}&requestDetailUID={2}", patientUID, requestUID, requestDetailUID);
            PatientRequestReportModel patientRequest = MeditechApiHelper.Get<PatientRequestReportModel>(requestApi);

            return patientRequest;

        }

        public PatientRequestReportModel GetRequestForReview(long patientUID, long requestUID, long requestDetailUID)
        {
            string requestApi = string.Format("Api/Radiology/GetRequestForReview?patientUID={0}&requestUID={1}&requestDetailUID={2}", patientUID, requestUID, requestDetailUID);
            PatientRequestReportModel patientRequest = MeditechApiHelper.Get<PatientRequestReportModel>(requestApi);

            return patientRequest;

        }

        public List<PreviousResult> GetPreviousResult(long patientUID, long requestDetailUID)
        {
            string requestApi = string.Format("Api/Radiology/GetPreviousResult?patientUID={0}&requestDetailUID={1}", patientUID, requestDetailUID);
            List<PreviousResult> previousResult = MeditechApiHelper.Get<List<PreviousResult>>(requestApi);

            return previousResult;
        }

        public List<ResultRadiologyHistoryModel> GetResultHistoryByRequestDetail(long requestDetailUID)
        {
            string requestApi = string.Format("Api/Radiology/GetResultHistoryByRequestDetail?requestDetailUID={0}", requestDetailUID);
            List<ResultRadiologyHistoryModel> resultHistory = MeditechApiHelper.Get<List<ResultRadiologyHistoryModel>>(requestApi);

            return resultHistory;
        }
        public List<RequestListModel> GetRequestList(DateTime? requestDateFrom, DateTime? requestDateTo, DateTime? assignDateFrom, DateTime? assignDateTo, string statusList, int? RQPRTUID, string firstName, string lastName, string patientID, string orderName, int? RIMTYPUID, int? radiologistUID)
        {
            string requestApi = string.Format("Api/Radiology/GetRequestList?requestDateFrom={0:MM/dd/yyyy}&requestDateTo={1:MM/dd/yyyy}&assignDateFrom={2:MM/dd/yyyy}&assignDateTo={3:MM/dd/yyyy}&statusList={4}&RQPRTUID={5}&firstName={6}&lastName={7}&patientID={8}&orderName={9}&RIMTYPUID={10}&radiologistUID={11}",
                requestDateFrom.HasValue ? requestDateFrom.Value : requestDateFrom, requestDateTo.HasValue ? requestDateTo.Value : requestDateTo,
                 assignDateFrom.HasValue ? assignDateFrom.Value : assignDateFrom, assignDateTo.HasValue ? assignDateTo.Value : assignDateTo,
                statusList, RQPRTUID, firstName, lastName, patientID, orderName, RIMTYPUID, radiologistUID);
            List<RequestListModel> listData = MeditechApiHelper.Get<List<RequestListModel>>(requestApi);
            return listData;
        }

        public List<PatientResultRadiology> SearchResultRadiologyForTranslate(DateTime? dateFrom, DateTime? dateTo, long? patientUID, string itemName, int? RABSTSUID, int? insuranceCompany)
        {
            string requestApi = string.Format("Api/Radiology/SearchResultRadiologyForTranslate?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&patientUID={2}&itemName={3}&RABSTSUID={4}&insuranceCompany={5}",
                dateFrom, dateTo, patientUID, itemName, RABSTSUID, insuranceCompany);
            List<PatientResultRadiology> listData = MeditechApiHelper.Get<List<PatientResultRadiology>>(requestApi);
            return listData;

        }

        public PatientResultRadiology SearchPatientResultRadiologyForTranslate(DateTime? dateFrom, DateTime? dateTo, string patientID, string itemName, int? RABSTSUID, int? insuranceCompanyUID)
        {
            string requestApi = string.Format("Api/Radiology/SearchPatientResultRadiologyForTranslate?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&patientID={2}&itemName={3}&RABSTSUID={4}&insuranceCompanyUID={5}", dateFrom, dateTo, patientID, itemName, RABSTSUID, insuranceCompanyUID);
            PatientResultRadiology listData = MeditechApiHelper.Get<PatientResultRadiology>(requestApi);
            return listData;

        }

        public XrayTranslateMappingModel SaveXrayTranslateMapping(XrayTranslateMappingModel mappingModel)
        {
            XrayTranslateMappingModel returnData;
            try
            {
                string requestApi = string.Format("Api/Radiology/SaveXrayTranslateMapping");
                returnData = MeditechApiHelper.Post<XrayTranslateMappingModel, XrayTranslateMappingModel>(requestApi, mappingModel);
            }
            catch (Exception)
            {

                throw;
            }

            return returnData;
        }

        public bool DeleteXrayTranslateMapping(int xrayTranslateMappingUID, int userUID)
        {
            bool flag = false;

            try
            {
                string requestApi = string.Format("Api/Radiology/DeleteXrayTranslateMapping?xrayTranslateMappingUID={0}&userUID={1}", xrayTranslateMappingUID, userUID);
                MeditechApiHelper.Delete(requestApi);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }

            return flag;

        }
        public List<XrayTranslateMappingModel> GetXrayTranslateMapping()
        {
            string requestApi = "Api/Radiology/GetXrayTranslateMapping";
            List<XrayTranslateMappingModel> listData = MeditechApiHelper.Get<List<XrayTranslateMappingModel>>(requestApi);
            return listData;
        }

        public List<XrayTranslateConditionModel> GetXrayTranslateCondition(int typeid)
        {
            string requestApi = string.Format("Api/Radiology/GetXrayTranslateCondition?typeid={0}", typeid);
            List<XrayTranslateConditionModel> listData = MeditechApiHelper.Get<List<XrayTranslateConditionModel>>(requestApi);
            return listData;
        }

        public List<XrayTranslateConditionDetailModel> GetXrayTranslateConditionDetailByConditonUID(int xrayTranslateConditionUID)
        {
            string requestApi = string.Format("Api/Radiology/GetXrayTranslateConditionDetailByConditonUID?xrayTranslateConditionUID={0}", xrayTranslateConditionUID);
            List<XrayTranslateConditionDetailModel> listData = MeditechApiHelper.Get<List<XrayTranslateConditionDetailModel>>(requestApi);
            return listData;
        }

        public List<XrayTranslateConditionDetailModel> GetXrayTranslateConditionDetailByDetailName(string xrayTranslateConditionDetailName)
        {
            string requestApi = string.Format("Api/Radiology/GetXrayTranslateConditionDetailByDetailName?xrayTranslateConditionDetailName={0}", xrayTranslateConditionDetailName);
            List<XrayTranslateConditionDetailModel> listData = MeditechApiHelper.Get<List<XrayTranslateConditionDetailModel>>(requestApi);
            return listData;
        }

        public List<XrayTranslateConditionDetailModel> GetXrayTranslateConditionDetailByConditionName(string xrayTranslateConditionName)
        {
            string requestApi = string.Format("Api/Radiology/GetXrayTranslateConditionDetailByConditionName?xrayTranslateConditionName={0}", xrayTranslateConditionName);
            List<XrayTranslateConditionDetailModel> listData = MeditechApiHelper.Get<List<XrayTranslateConditionDetailModel>>(requestApi);
            return listData;
        }


        public bool SaveTemplateTransalteMaster(string translateMastername, string translateDetailname, string thaiValue, int type, int userID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/Radiology/SaveTemplateTransalteMaster?translateMastername={0}&translateDetailname={1}&thaiValue={2}&type={3}&userID={4}"
                    , translateMastername, translateDetailname, thaiValue, type, userID);
                MeditechApiHelper.Put(requestApi);
            }
            catch (Exception)
            {

                throw;
            }



            return flag;
        }

        public RequestListModel GetRequestListByRequestDetailUID(long requestDetailUID)
        {
            string requestApi = string.Format("Api/Radiology/GetRequestListByRequestDetailUID?requestDetailUID={0}", requestDetailUID);
            RequestListModel listData = MeditechApiHelper.Get<RequestListModel>(requestApi);
            return listData;
        }

        public RequestListModel GetRequestDetailByUID(long requestDetailUID)
        {
            string requestApi = string.Format("Api/Radiology/GetRequestDetailByUID?requestDetailUID={0}", requestDetailUID);
            RequestListModel listData = MeditechApiHelper.Get<RequestListModel>(requestApi);
            return listData;
        }

        public RequestDetailDocumentModel GetRequestDetailDocument(long requestDetailUID)
        {
            string requestApi = string.Format("Api/Radiology/GetRequestDetailDocument?requestDetailUID={0}", requestDetailUID);
            RequestDetailDocumentModel listData = MeditechApiHelper.Get<RequestDetailDocumentModel>(requestApi);
            return listData;
        }

        public ResultRadiologyModel GetResultRadiologyByRequest(long requestdetailUID)
        {
            string requestApi = string.Format("Api/Radiology/GetResultRadiologyByRequest?requestdetailUID={0}", requestdetailUID);
            ResultRadiologyModel data = MeditechApiHelper.Get<ResultRadiologyModel>(requestApi);

            return data;
        }

        public ResultRadiologyModel GetResultRadiologyByResultUID(long resultUID)
        {
            string requestApi = string.Format("Api/Radiology/GetResultRadiologyByResultUID?resultUID={0}", resultUID);
            ResultRadiologyModel data = MeditechApiHelper.Get<ResultRadiologyModel>(requestApi);
            return data;
        }
        public List<ResultRadiologyModel> GetResultRadiologyByPayor(long patientUID, int payorDetailUID)
        {
            string requestApi = string.Format("Api/Radiology/GetResultRadiologyByPayor?patientUID={0}&payorDetailUID={1}", patientUID, payorDetailUID);
            List<ResultRadiologyModel> data = MeditechApiHelper.Get<List<ResultRadiologyModel>>(requestApi);
            return data;
        }

        public List<ResultRadiologyModel> GetResultRadiologyByPatientUID(long patientUID, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            string requestApi = string.Format("Api/Radiology/GetResultRadiologyByPatientUID?patientUID={0}&dateFrom={1:MM/dd/yyyy}&dateTo={2:MM/dd/yyyy}", patientUID, dateFrom, dateTo);
            List<ResultRadiologyModel> data = MeditechApiHelper.Get<List<ResultRadiologyModel>>(requestApi);
            return data;
        }

        public List<ResultRadiologyModel> GetResultRadiologyByVisitUID(long patientVisitUID, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            string requestApi = string.Format("Api/Radiology/GetResultRadiologyByVisitUID?patientVisitUID={0}&dateFrom={1:MM/dd/yyyy}&dateTo={2:MM/dd/yyyy}", patientVisitUID, dateFrom, dateTo);
            List<ResultRadiologyModel> data = MeditechApiHelper.Get<List<ResultRadiologyModel>>(requestApi);
            return data;
        }

        public long SaveReviewResult(ResultRadiologyModel resultData, int userID)
        {
            long resultUID = 0;
            string requestApi = string.Format("Api/Radiology/SaveReviewResult?userID={0}", userID);
            var result = MeditechApiHelper.Post<ResultRadiologyModel>(requestApi, resultData);

            if (result != null)
            {
                resultUID = Convert.ToInt32(result);
            }

            return resultUID;
        }

        public bool UpdateRequestDetailToReviewing(long requestDetailUID, int userID)
        {
            bool flag = false;

            try
            {
                string requestApi = string.Format("Api/Radiology/UpdateRequestDetailToReviewing?requestDetailUID={0}&userID={1}", requestDetailUID, userID);
                MeditechApiHelper.Put(requestApi);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }

            return flag;
        }

        public bool UnReviewingReqeustDetail(long requestDetailUID, int userID)
        {
            bool flag = false;

            try
            {
                string requestApi = string.Format("Api/Radiology/UnReviewingReqeustDetail?requestDetailUID={0}&userID={1}", requestDetailUID, userID);
                MeditechApiHelper.Put(requestApi);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }

            return flag;
        }

        public RequestListModel UnReviewingAndGetRequestListByRequestDetailUID(long requestDetailUID, int userID)
        {
            RequestListModel listData = null;
            try
            {
                string requestApi = string.Format("Api/Radiology/UnReviewingAndGetRequestListByRequestDetailUID?requestDetailUID={0}&userID={1}", requestDetailUID, userID);
                listData = MeditechApiHelper.Get<RequestListModel>(requestApi);
            }
            catch (Exception)
            {

                throw;
            }

            return listData;
        }
        public ResultRadiologyTemplateModel GetResultRaiologyTemplateByUID(int resultRadiologyTemplateUID)
        {
            string requestApi = string.Format("Api/Radiology/GetResultRaiologyTemplateByUID?resultRadiologyTemplateUID={0}", resultRadiologyTemplateUID);
            ResultRadiologyTemplateModel data = MeditechApiHelper.Get<ResultRadiologyTemplateModel>(requestApi);
            return data;
        }
        public List<ResultRadiologyTemplateModel> GetResultRadiologyTemplateByDoctor(int userID)
        {
            string requestApi = string.Format("Api/Radiology/GetResultRadiologyTemplateByDoctor?userID={0}", userID);
            List<ResultRadiologyTemplateModel> data = MeditechApiHelper.Get<List<ResultRadiologyTemplateModel>>(requestApi);
            return data;
        }

        public List<ResultRadiologyTemplateModel> GetResultRadiologyTemplateByReportEditor(int? RIMTYPUID, int? SEXXXUID, int userID)
        {
            string requestApi = string.Format("Api/Radiology/GetResultRadiologyTemplateByReportEditor?RIMTYPUID={0}&SEXXXUID={1}&userID={2}", RIMTYPUID, SEXXXUID, userID);
            List<ResultRadiologyTemplateModel> data = MeditechApiHelper.Get<List<ResultRadiologyTemplateModel>>(requestApi);
            return data;
        }

        public int SaveResultRadiologyTemplate(ResultRadiologyTemplateModel resultRadTemp, int userID)
        {
            int resultRadiologytemplateUID = 0;
            string requestApi = string.Format("Api/Radiology/SaveResultRadiologyTemplate?userID={0}", userID);

            var result = MeditechApiHelper.Post<ResultRadiologyTemplateModel>(requestApi, resultRadTemp);

            if (result != null)
            {
                resultRadiologytemplateUID = Convert.ToInt32(result);
            }

            return resultRadiologytemplateUID;
        }

        public bool UpdateResultRadiologyTemplate(ResultRadiologyTemplateModel resultRadTemp, int userID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/Radiology/UpdateResultRadiologyTemplate?userID={0}", userID);
                MeditechApiHelper.Put<ResultRadiologyTemplateModel>(requestApi, resultRadTemp);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }
            return flag;
        }

        public bool DeleteResultRadiologyTemplate(int resultRadiologyTemplateUID, int userID)
        {
            bool flag = false;

            try
            {
                string requestApi = string.Format("Api/Radiology/DeleteResultRadiologyTemplate?resultRadiologyTemplateUID={0}&userID={1}", resultRadiologyTemplateUID, userID);
                MeditechApiHelper.Delete(requestApi);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }


            return flag;

        }

        public bool SaveDisplayOrderResultRadiologyTempalte(int resultTemplateUID, int displayOrder, int userID)
        {
            bool flag;
            try
            {
                string requestApi = string.Format("Api/Radiology/SaveDisplayOrderResultRadiologyTempalte?resultTemplateUID={0}&displayOrder={1}&userID={2}", resultTemplateUID, displayOrder, userID);
                MeditechApiHelper.Put(requestApi);
                flag = true;
            }
            catch (Exception)
            {
                flag = false;
                throw;
            }
            return flag;

        }

        public bool AssignRadiologist(int radiologistUID, long requestDetailUID, int userID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/Radiology/AssignRadiologist?radiologistUID={0}&requestDetailUID={1}&userID={2}", radiologistUID, requestDetailUID, userID);
                MeditechApiHelper.Put(requestApi);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }
            return flag;
        }

        public bool AssignRadiologistList(List<RequestListModel> requestList, int radiologistUID, int userID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/Radiology/AssignRadiologistList?radiologistUID={0}&userID={1}", radiologistUID, userID);
                MeditechApiHelper.Put<List<RequestListModel>>(requestApi, requestList);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }
            return flag;
        }

        public bool AssignRadiologist(List<RequestListModel> requestList, int userID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/Radiology/AssignRadiologist?userID={0}", userID);
                MeditechApiHelper.Put<List<RequestListModel>>(requestApi, requestList);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }
            return flag;

        }

        public bool AssignRadiologistForMassResult(RequestListModel requestList, int userID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/Radiology/AssignRadiologistForMassResult?userID={0}", userID);
                MeditechApiHelper.Put<RequestListModel>(requestApi, requestList);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }
            return flag;
        }
        public bool AssignToRDUStaff(List<RequestListModel> requestList, int userID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/Radiology/AssignToRDUStaff?userID={0}", userID);
                MeditechApiHelper.Put<List<RequestListModel>>(requestApi, requestList);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }
            return flag;

        }

        public bool CancelRequest(long requestDetailUID, int userID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/Radiology/CancelRequest?requestDetailUID={0}&userID={1}", requestDetailUID, userID);
                MeditechApiHelper.Put(requestApi);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }
            return flag;
        }

        public List<ScheduleRadiologistModel> GetScheduleRadiologist(DateTime startDate, DateTime endDate)
        {
            string requestApi = string.Format("Api/Radiology/GetScheduleRadiologist?startDate={0:MM/dd/yyyy}&endDate={1:MM/dd/yyyy}", startDate, endDate);
            List<ScheduleRadiologistModel> data = MeditechApiHelper.Get<List<ScheduleRadiologistModel>>(requestApi);
            return data;
        }

        public ScheduleRadiologistModel GetScheduleRadiologistByUID(int scheduleRadiologistUID)
        {
            string requestApi = string.Format("Api/Radiology/GetScheduleRadiologistByUID?scheduleRadiologistUID={0}", scheduleRadiologistUID);
            ScheduleRadiologistModel data = MeditechApiHelper.Get<ScheduleRadiologistModel>(requestApi);
            return data;
        }

        public int AddOrUpdateScheDuleRadiologist(ScheduleRadiologistModel scheduleModel, int userUID)
        {
            try
            {
                string requestApi = string.Format("Api/Radiology/AddOrUpdateScheDuleRadiologist?userUID={0}", userUID);
                object result = MeditechApiHelper.Post<ScheduleRadiologistModel>(requestApi, scheduleModel);
                return int.Parse(result.ToString());
            }
            catch (Exception)
            {

                throw;
            }
        }


        public void DeleteScheDuleRadiologist(int scheduleRadiologistUID, int userUID)
        {
            try
            {
                string requestApi = string.Format("Api/Radiology/DeleteScheDuleRadiologist?scheduleRadiologistUID={0}&userUID={1}", scheduleRadiologistUID,userUID);
                MeditechApiHelper.Delete(requestApi);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<SessionDefinitionModel> SearchSession(DateTime? startDate, DateTime? endDate, int? careproviderUID)
        {
            string requestApi = string.Format("Api/Radiology/SearchSession?startDate={0:MM/dd/yyyy}&endDate={1:MM/dd/yyyy}&careproviderUID={2}", startDate, endDate, careproviderUID);
            List<SessionDefinitionModel> data = MeditechApiHelper.Get<List<SessionDefinitionModel>>(requestApi);
            return data;
        }

        public bool ManageSession(SessionDefinitionModel sessionModel, int userUID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/Radiology/ManageSession?userUID={0}", userUID);
                MeditechApiHelper.Post<SessionDefinitionModel>(requestApi, sessionModel);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }
            return flag;
        }

        public List<SessionWithdrawnDetailModel> GetWithDrawSessionBySessionUID(int sessionUID)
        {
            string requestApi = string.Format("Api/Radiology/GetWithDrawSessionBySessionUID?sessionUID={0}", sessionUID);
            List<SessionWithdrawnDetailModel> data = MeditechApiHelper.Get<List<SessionWithdrawnDetailModel>>(requestApi);
            return data;
        }

        public bool WithDrawSession(List<SessionWithdrawnDetailModel> withDrawSessionModel, int sessionUID, int userUID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/Radiology/WithDrawSession?sessionUID={0}&userUID={1}", sessionUID, userUID);
                MeditechApiHelper.Post<List<SessionWithdrawnDetailModel>>(requestApi, withDrawSessionModel);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }
            return flag;
        }

        public bool DeleteSession(int sessionUID, int userUID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/Radiology/DeleteSession?sessionUID={0}&userUID={1}", sessionUID, userUID);
                MeditechApiHelper.Put(requestApi);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }
            return flag;
        }

        public List<DoctorFeeModel> GetRadiologistReport(DateTime dateFrom, DateTime dateTo, int? radiologistUID)
        {
            string requestApi = string.Format("Api/Radiology/GetRadiologistReport?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&radiologistUID={2}", dateFrom, dateTo, radiologistUID);
            List<DoctorFeeModel> data = MeditechApiHelper.Get<List<DoctorFeeModel>>(requestApi);
            return data;
        }
        public List<DoctorFeeModel> GetDoctorFeeNonPay(DateTime dateFrom, DateTime dateTo, int? radiologistUID)
        {
            string requestApi = string.Format("Api/Radiology/GetDoctorFeeNonPay?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&radiologistUID={2}", dateFrom, dateTo, radiologistUID);
            List<DoctorFeeModel> data = MeditechApiHelper.Get<List<DoctorFeeModel>>(requestApi);
            return data;
        }

        public List<DoctorFeeModel> GetDoctorFee(DateTime dateFrom, DateTime dateTo, int? radiologistUID)
        {
            string requestApi = string.Format("Api/Radiology/GetDoctorFee?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&radiologistUID={2}", dateFrom, dateTo, radiologistUID);
            List<DoctorFeeModel> data = MeditechApiHelper.Get<List<DoctorFeeModel>>(requestApi);
            return data;
        }

        public bool AddDoctorFee(List<DoctorFeeModel> DoctorFeesModel, int userUID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/Radiology/AddDoctorFee?userUID={0}", userUID);
                MeditechApiHelper.Post<List<DoctorFeeModel>>(requestApi, DoctorFeesModel);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }
            return flag;
        }

        public bool RemoveDoctorFee(List<DoctorFeeModel> DoctorFeesModel, int userUID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/Radiology/RemoveDoctorFee?userUID={0}", userUID);
                MeditechApiHelper.Put<List<DoctorFeeModel>>(requestApi, DoctorFeesModel);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }
            return flag;
        }

        public List<NotificationTaskResultModel> GetNotificationTaskResult(string healthOrganisationCode)
        {
            string requestApi = string.Format("Api/Radiology/GetNotificationTaskResult?healthOrganisationCode={0}", healthOrganisationCode);
            List<NotificationTaskResultModel> data = MeditechApiHelper.Get<List<NotificationTaskResultModel>>(requestApi);
            return data;
        }

        public void UpdateNotificationTaskResult(long notificationTaskUID)
        {
            try
            {
                string requestApi = string.Format("Api/Radiology/UpdateNotificationTaskResult?notificationTaskUID={0}", notificationTaskUID);
                MeditechApiHelper.Put(requestApi);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
