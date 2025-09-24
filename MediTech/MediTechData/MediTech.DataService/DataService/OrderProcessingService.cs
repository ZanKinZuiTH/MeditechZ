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

        public List<BillPackageModel> SearchBillPackage(string text, int? orderCategoryUID, int? orderSubCategoryUID)
        {
            string requestApi = string.Format("Api/OrderProcessing/SearchBillPackage?text={0}&orderCategoryUID={1}&orderSubCategoryUID={2}", text, orderCategoryUID, orderSubCategoryUID);
            List<BillPackageModel> data = MeditechApiHelper.Get<List<BillPackageModel>>(requestApi);
            return data;
        }

        public bool CreateOrder(long patientUID, long patientVisitUID, int userUID,int locationUID, int ownerOrganisationUID, List<PatientOrderDetailModel> orderDetails)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/OrderProcessing/CreateOrder?patientUID={0}&patientVisitUID={1}&userUID={2}&locationUID={3}&ownerOrganisationUID={4}", patientUID, patientVisitUID, userUID, locationUID, ownerOrganisationUID);
                MeditechApiHelper.Post<List<PatientOrderDetailModel>>(requestApi, orderDetails);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }

            return flag;
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

        public List<PatientOrderAlertModel> CriteriaOrderAlert(long patientUID,long patientVisitUID, BillableItemModel billItemModel)
        {
            string requestApi = string.Format("Api/OrderProcessing/CriteriaOrderAlert?patientUID={0}&patientVisitUID={1}", patientUID,patientVisitUID);
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

        public bool ClosureStandingOrder(string patientOrderDetailUIDs, int userUID, DateTime endDttm, string comments)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/OrderProcessing/ClosureStandingOrder?patientOrderDetailUIDs={0}&userUID={1}&endDttm={2:MM/dd/yyyy HH:mm}&comments={3}", patientOrderDetailUIDs, userUID, endDttm, comments);
                MeditechApiHelper.Put(requestApi);
                flag = true;
            }
            catch (Exception)
            {
                throw;
            }
            return flag;
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

        #region PatientPackage
        public bool AddPatientPackage(PatientPackageModel model)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/OrderProcessing/AddPatientPackage");
                MeditechApiHelper.Post<PatientPackageModel>(requestApi, model);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }

            return flag;
        }

        public bool DeletePatientPackage(long patientPackakgeUID, int userID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/OrderProcessing/DeletePatientPackage?patientPackakgeUID={0}&userID={0}", patientPackakgeUID,userID);
                MeditechApiHelper.Delete(requestApi);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }

            return flag;
        }

        public List<PatientPackageModel> GetPatientPackageByVisitUID(long patientVisitUID)
        {
            string requestApi = string.Format("Api/OrderProcessing/GetPatientPackageByVisitUID?patientVisitUID={0}", patientVisitUID);
            List<PatientPackageModel> data = MeditechApiHelper.Get<List<PatientPackageModel>>(requestApi);
            return data;
        }

        public List<PatientPackageItemModel> GetPatientPackageItemByPatientPakcageUID(long patientPakcageUID)
        {
            string requestApi = string.Format("Api/OrderProcessing/GetPatientPackageItemByPatientPakcageUID?patientPakcageUID={0}", patientPakcageUID);
            List<PatientPackageItemModel> data = MeditechApiHelper.Get<List<PatientPackageItemModel>>(requestApi);
            return data;
        }

        public List<AdjustablePackageItemModel> GetAdjustablePackageItems(long patientUID, long patientVisitUID, long patientPackageUID)
        {
            string requestApi = string.Format("Api/OrderProcessing/GetAdjustablePackageItems?patientUID={0}&patientVisitUID={1}&patientPackageUID={2}", patientUID,patientVisitUID, patientPackageUID);
            List<AdjustablePackageItemModel> data = MeditechApiHelper.Get<List<AdjustablePackageItemModel>>(requestApi);
            return data;
        }

        public List<LinkPackageModel> GetLinkPackage(int billableItemUID, long patientVisitUID)
        {
            string requestApi = string.Format("Api/OrderProcessing/GetLinkPackage?billableItemUID={0}&patientVisitUID={1}", billableItemUID, patientVisitUID);
            List<LinkPackageModel> data = MeditechApiHelper.Get<List<LinkPackageModel>>(requestApi);
            return data;
        }

        public bool AdjustOrderDetailForPackage(long patientUID, long patientVisitUID, long? patientPackageUID, int billableItemUID, List<long> patientOrderDetailUIDs)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/OrderProcessing/AdjustOrderDetailForPackage?patientUID={0}&patientVisitUID={1}&patientPackageUID={2}&billableItemUID={3}", patientUID, patientVisitUID, patientPackageUID, billableItemUID);
                MeditechApiHelper.Put<List<long>>(requestApi, patientOrderDetailUIDs);
                flag = true;
            }
            catch (Exception)
            {
                throw;
            }
            return flag;
        }

        public bool UpdateLinkPackage(long patientOrderDetailUID, long patientPackageUID, bool linkFlag, int userUID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/OrderProcessing/UpdateLinkPackage?patientOrderDetailUID={0}&patientPackageUID={1}&linkFlag={2}&userUID={3}", patientOrderDetailUID, patientPackageUID, linkFlag, userUID);
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
