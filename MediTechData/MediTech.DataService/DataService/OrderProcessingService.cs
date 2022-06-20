using MediTech.Model;
using MediTech.Model.Report;
using ShareLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.DataService
{
    public class OrderProcessingService
    {
        public List<SearchOrderItem> SearchOrderItem(string text, int ownerOrganisationUID)
        {
            string requestApi = string.Format("Api/OrderProcessing/SearchOrderItem?text={0}&ownerOrganisationUID={1}", text, ownerOrganisationUID);
            List<SearchOrderItem> data = MeditechApiHelper.Get<List<SearchOrderItem>>(requestApi);
            return data;
        }

        public string CreateOrder(long patientUID, long patientVisitUID, int userUID, int locationUID,int ownerOrganisationUID, List<PatientOrderDetailModel> orderDetails)
        {
            string requestApi = string.Format("Api/OrderProcessing/CreateOrder?patientUID={0}&patientVisitUID={1}&userUID={2}&locationUID={3}&ownerOrganisationUID={4}", patientUID, patientVisitUID, userUID, locationUID, ownerOrganisationUID);
            string message = MeditechApiHelper.Post<List<PatientOrderDetailModel>, string>(requestApi, orderDetails);
            return message;

        }

        public bool CancelOrders(List<long> patientOrderDetailUIDs, string cancelReason, int userUID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/OrderProcessing/CancelOrders?cancelReason={0}&userUID={1}", cancelReason, userUID);
                MeditechApiHelper.Put<List<long>>(requestApi, patientOrderDetailUIDs);
                flag = true;
            }
            catch (Exception)
            {
                throw;
            }
            return flag;
        }

        public List<PatientOrderAlertModel> CriteriaOrderAlert(long patientUID, BillableItemModel billItemModel)
        {
            string requestApi = string.Format("Api/OrderProcessing/CriteriaOrderAlert?patientUID={0}", patientUID);
            List<PatientOrderAlertModel> data = MeditechApiHelper.Post<BillableItemModel, List<PatientOrderAlertModel>>(requestApi, billItemModel);
            return data;
        }

        public List<PatientOrderDetailModel> GetOrderAllByVisitUID(long visitUID, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            string requestApi = string.Format("Api/OrderProcessing/GetOrderAllByVisitUID?visitUID={0}&dateFrom={1:MM/dd/yyyy}&dateTo={2:MM/dd/yyyy}", visitUID, dateFrom, dateTo);
            List<PatientOrderDetailModel> data = MeditechApiHelper.Get<List<PatientOrderDetailModel>>(requestApi);
            return data;
        }
        public List<PatientOrderDetailModel> GetOrderAllByPatientUID(long patientUID, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            string requestApi = string.Format("Api/OrderProcessing/GetOrderAllByPatientUID?patientUID={0}&dateFrom={1:MM/dd/yyyy}&dateTo={2:MM/dd/yyyy}", patientUID, dateFrom, dateTo);
            List<PatientOrderDetailModel> data = MeditechApiHelper.Get<List<PatientOrderDetailModel>>(requestApi);
            return data;
        }


        #region OrderDrugItem

        public PatientOrderModel GetOrderDrugByPatientUID(long patientUID, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            string requestApi = string.Format("Api/OrderProcessing/GetOrderDrugByPatientUID?patientUID={0}&dateFrom={1:MM/dd/yyyy}&dateTo={2:MM/dd/yyyy}", patientUID, dateFrom, dateTo);
            PatientOrderModel data = MeditechApiHelper.Get<PatientOrderModel>(requestApi);
            return data;
        }

        public PatientOrderModel GetOrderDrugByVisitUID(long visitUID, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            string requestApi = string.Format("Api/OrderProcessing/GetOrderDrugByVisitUID?visitUID={0}&dateFrom={1:MM/dd/yyyy}&dateTo={2:MM/dd/yyyy}", visitUID, dateFrom, dateTo);
            PatientOrderModel data = MeditechApiHelper.Get<PatientOrderModel>(requestApi);
            return data;
        }

        #endregion

        #region MedicalItem

        public PatientOrderModel GetOrderMedicalByVisitUID(long visitUID)
        {
            string requestApi = string.Format("Api/OrderProcessing/GetOrderMedicalByVisitUID?visitUID={0}", visitUID);
            PatientOrderModel data = MeditechApiHelper.Get<PatientOrderModel>(requestApi);
            return data;
        }

        #endregion

        #region OrderItem

        public PatientOrderModel GetOrderItemByVisitUID(long visitUID)
        {
            string requestApi = string.Format("Api/OrderProcessing/GetOrderItemByVisitUID?visitUID={0}", visitUID);
            PatientOrderModel data = MeditechApiHelper.Get<PatientOrderModel>(requestApi);
            return data;
        }

        #endregion

        #region RequestItem

        public List<PatientOrderModel> GetOrderRequestByVisitUID(long visitUID)
        {
            string requestApi = string.Format("Api/OrderProcessing/GetOrderRequestByVisitUID?visitUID={0}", visitUID);
            List<PatientOrderModel> data = MeditechApiHelper.Get<List<PatientOrderModel>>(requestApi);
            return data;
        }

        public List<SearchOrderItem> GetOrderRequestItem()
        {
            string requestApi = string.Format("Api/OrderProcessing/GetOrderRequestItem");
            List<SearchOrderItem> data = MeditechApiHelper.Get<List<SearchOrderItem>>(requestApi);
            return data;
        }

        #endregion
    }
}
