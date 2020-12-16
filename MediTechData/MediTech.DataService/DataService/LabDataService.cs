using MediTech.Model;
using MediTech.Model.Report;
using ShareLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace MediTech.DataService
{
    public class LabDataService
    {
        public List<RequestLabModel> SearchRequestLabList(DateTime? requestDateFrom, DateTime? requestDateTo, string statusLis, long? patientUID, int? requestItemUID, string labNumber, int? payorDetailUID, int? organisationUID)
        {
            string requestApi = string.Format("Api/Lab/SearchRequestLabList?requestDateFrom={0:MM/dd/yyyy}&requestDateTo={1:MM/dd/yyyy}&statusLis={2}&patientUID={3}&requestItemUID={4}&labNumber={5}&payorDetailUID={6}&organisationUID={7}", requestDateFrom, requestDateTo, statusLis, patientUID, requestItemUID, labNumber, payorDetailUID, organisationUID);
            List<RequestLabModel> listData = MeditechApiHelper.Get<List<RequestLabModel>>(requestApi);
            return listData;
        }


        public List<PatientResultComponentModel> SearchResultLabList(DateTime? dateFrom, DateTime? dateTo, long? patientUID, int? payorDetailUID)
        {
            string requestApi = string.Format("Api/Lab/SearchResultLabList?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&patientUID={2}&payorDetailUID={3}", dateFrom, dateTo, patientUID, payorDetailUID);
            List<PatientResultComponentModel> listData = MeditechApiHelper.Get<List<PatientResultComponentModel>>(requestApi);
            return listData;
        }

        public List<RequestDetailItemModel> GetRequesDetailLabByRequestUID(long requestUID)
        {
            string requestApi = string.Format("Api/Lab/GetRequesDetailLabByRequestUID?requestUID={0}", requestUID);
            List<RequestDetailItemModel> listData = MeditechApiHelper.Get<List<RequestDetailItemModel>>(requestApi);

            return listData;
        }

        public RequestDetailItemModel GetRequesDetailLabForImport(string patientID, int? ownerOrganisationUID, int? payorDetailUID
    , int requestItemUID, DateTime? dateFrom, DateTime? dateTo = null)
        {
            string requestApi = string.Format("Api/Lab/GetRequesDetailLabForImport?patientID={0}&ownerOrganisationUID={1}" +
                "&payorDetailUID={2}&requestItemUID={3}&dateFrom={4:MM/dd/yyyy}&dateTo={5:MM/dd/yyyy}", patientID,ownerOrganisationUID,payorDetailUID,requestItemUID
                , dateFrom,dateTo);
            RequestDetailItemModel data = MeditechApiHelper.Get<RequestDetailItemModel>(requestApi);
            return data;
        }
        public List<RequestDetailSpecimenModel> GetRequestDetailSpecimenByRequestUID(long requestUID)
        {
            string requestApi = string.Format("Api/Lab/GetRequestDetailSpecimenByRequestUID?requestUID={0}", requestUID);
            List<RequestDetailSpecimenModel> listData = MeditechApiHelper.Get<List<RequestDetailSpecimenModel>>(requestApi);

            return listData;
        }

        public List<RequestDetailItemModel> GetResultLabByRequestUID(long requestUID)
        {
            string requestApi = string.Format("Api/Lab/GetResultLabByRequestUID?requestUID={0}", requestUID);
            List<RequestDetailItemModel> listData = MeditechApiHelper.Get<List<RequestDetailItemModel>>(requestApi);

            return listData;
        }

        public List<RequestDetailItemModel> GetResultLabByRequestNumber(string requestNumber)
        {
            string requestApi = string.Format("Api/Lab/GetResultLabByRequestNumber?requestNumber={0}", requestNumber);
            List<RequestDetailItemModel> listData = MeditechApiHelper.Get<List<RequestDetailItemModel>>(requestApi);

            return listData;
        }


        public List<ResultModel> GetResultLabByPatientVisitUID(long patientVisitUID)
        {
            string requestApi = string.Format("Api/Lab/GetResultLabByPatientVisitUID?patientVisitUID={0}", patientVisitUID);
            List<ResultModel> listData = MeditechApiHelper.Get<List<ResultModel>>(requestApi);

            return listData;
        }

        public List<RequestLabModel> GetResultLabGroupRequestNumberByVisit(long patientVisitUID, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            string requestApi = string.Format("Api/Lab/GetResultLabGroupRequestNumberByVisit?patientVisitUID={0}&dateFrom={1:MM/dd/yyyy}&dateTo={2:MM/dd/yyyy}", patientVisitUID, dateFrom, dateTo);
            List<RequestLabModel> listData = MeditechApiHelper.Get<List<RequestLabModel>>(requestApi);

            return listData;
        }

        public List<RequestLabModel> GetResultLabGroupRequestNumberByPatient(long patientUID, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            string requestApi = string.Format("Api/Lab/GetResultLabGroupRequestNumberByPatient?patientUID={0}&dateFrom={1:MM/dd/yyyy}&dateTo={2:MM/dd/yyyy}", patientUID, dateFrom, dateTo);
            List<RequestLabModel> listData = MeditechApiHelper.Get<List<RequestLabModel>>(requestApi);

            return listData;
        }

        public List<RequestLabModel> GetResultLabGroupRequestNumber(long patientVisitUID)
        {
            string requestApi = string.Format("Api/Lab/GetResultLabGroupRequestNumber?patientVisitUID={0}", patientVisitUID);
            List<RequestLabModel> listData = MeditechApiHelper.Get<List<RequestLabModel>>(requestApi);

            return listData;
        }

        public List<ResultItemRangeModel> GetResultItemRangeByLABRAMUID(long LABRAMUID)
        {
            string requestApi = string.Format("Api/Lab/GetResultItemRangeByLABRAMUID?LABRAMUID={0}", LABRAMUID);
            List<ResultItemRangeModel> listData = MeditechApiHelper.Get<List<ResultItemRangeModel>>(requestApi);

            return listData;
        }

        public List<ResultItemRangeModel> GetResultItemRangeByRequestItemUID(int requestItemUID)
        {
            string requestApi = string.Format("Api/Lab/GetResultItemRangeByRequestItemUID?requestItemUID={0}", requestItemUID);
            List<ResultItemRangeModel> listData = MeditechApiHelper.Get<List<ResultItemRangeModel>>(requestApi);

            return listData;
        }

        public ResultImageModel GetResultImageByComponentUID(long resultComponentUID)
        {
            string requestApi = string.Format("Api/Lab/GetResultImageByComponentUID?resultComponentUID={0}", resultComponentUID);
            ResultImageModel listData = MeditechApiHelper.Get<ResultImageModel>(requestApi);

            return listData;
        }

        public void ReviewLabResult(List<RequestDetailItemModel> labRequestDetails, int userID)
        {
            try
            {
                string requestApi = string.Format("Api/Lab/ReviewLabResult?userID={0}", userID);
                MeditechApiHelper.Post<List<RequestDetailItemModel>>(requestApi, labRequestDetails);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void UpdateRequestDetailSpecimens(List<RequestDetailSpecimenModel> requestDetailSpecimens, int userID)
        {
            try
            {
                string requestApi = string.Format("Api/Lab/UpdateRequestDetailSpecimens?userID={0}", userID);
                MeditechApiHelper.Post<List<RequestDetailSpecimenModel>>(requestApi, requestDetailSpecimens);
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
