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
    public class PurchaseingService
    {

        #region Purchaseint
        public List<PurchaseOrderModel> SearchPurchaseOrder(DateTime? dateFrom, DateTime? dateTo, int? organisationUID, int? storeUID, int? venderoDetailUID, int? poStatus, string PONo)
        {
            string requestApi = string.Format("Api/Purchaseing/SearchPurchaseOrder?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&organisationUID={2}&storeUID={3}&venderoDetailUID={4}&poStatus={5}&PONo={6}", dateFrom, dateTo, organisationUID, storeUID, venderoDetailUID, poStatus, PONo);
            List<PurchaseOrderModel> dataRequest = MeditechApiHelper.Get<List<PurchaseOrderModel>>(requestApi);

            return dataRequest;
        }

        public PurchaseOrderModel GetPurchaseOrderByUID(int purchaseOrderUID)
        {
            string requestApi = string.Format("Api/Purchaseing/GetPurchaseOrderByUID?purchaseOrderUID={0}", purchaseOrderUID);
            PurchaseOrderModel dataRequest = MeditechApiHelper.Get<PurchaseOrderModel>(requestApi);

            return dataRequest;
        }

        public List<PurchaseOrderItemListModel> GetPurchaseOrderItemListByPurchaseOrderUID(int purchaseOrderUID)
        {
            string requestApi = string.Format("Api/Purchaseing/GetPurchaseOrderItemListByPurchaseOrderUID?purchaseOrderUID={0}", purchaseOrderUID);
            List<PurchaseOrderItemListModel> dataRequest = MeditechApiHelper.Get<List<PurchaseOrderItemListModel>>(requestApi);

            return dataRequest;
        }

        public bool CancelPurchaseOrder(int purchaseOrderUID, string cancelReason, int userID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/Purchaseing/CancelPurchaseOrder?purchaseOrderUID={0}&cancelReason={1}&userID={2}", purchaseOrderUID, cancelReason, userID);
                MeditechApiHelper.Put(requestApi);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }
            return flag;
        }

        public bool UpdatePurchaseOrderStatus(long purchaseOrderUID, int POSTSUID, string approvalComments, int userID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/Purchaseing/UpdatePurchaseOrderStatus?purchaseOrderUID={0}&POSTSUID={1}&approvalComments={2}&userID={3}", purchaseOrderUID, POSTSUID, approvalComments, userID);
                MeditechApiHelper.Put(requestApi);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }
            return flag;
        }

        public bool ManagePurchaseOrder(PurchaseOrderModel model, int userID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/Purchaseing/ManagePurchaseOrder?userID={0}", userID);
                MeditechApiHelper.Post<PurchaseOrderModel>(requestApi, model);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }
            return flag;
        }

        public List<GroupReceiptModel> SearchGroupReceipt(DateTime? dateFrom, DateTime? dateTo, int? organisationUID, int? payorDetailUID, string receiptNumber)
        {
            string requestApi = string.Format("Api/Purchaseing/SearchGroupReceipt?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&organisationUID={2}&payorDetailUID={3}&receiptNumber={4}", dateFrom, dateTo, organisationUID, payorDetailUID, receiptNumber);
            List<GroupReceiptModel> dataRequest = MeditechApiHelper.Get<List<GroupReceiptModel>>(requestApi);

            return dataRequest;
        }
        public List<GroupReceiptModel> GetGroupReceipt()
        {
            string requestApi = string.Format("Api/Purchaseing/GetGroupReceipt?");
            List<GroupReceiptModel> dataRequest = MeditechApiHelper.Get<List<GroupReceiptModel>>(requestApi);

            return dataRequest;
        }

        public GroupReceiptModel GetGroupReceiptByUID(int groupReceiptUID)
        {
            string requestApi = string.Format("Api/Purchaseing/GetGroupReceiptByUID?groupReceiptUID={0}", groupReceiptUID);
            GroupReceiptModel dataRequest = MeditechApiHelper.Get<GroupReceiptModel>(requestApi);

            return dataRequest;
        }

        public int? ManageGroupReceipt(GroupReceiptModel model, int userID)
        {
            int? groupReceiptUID;
            try
            {
                string requestApi = string.Format("Api/Purchaseing/ManageGroupReceipt?userID={0}", userID);
                groupReceiptUID = MeditechApiHelper.Post<GroupReceiptModel, int?>(requestApi, model);
            }
            catch (Exception)
            {

                throw;
            }
            return groupReceiptUID;
        }

        public bool DeleteGroupReceiptDetail(int GroupReceiptDetailUID, int userID)
        {
            bool flag = false;

            try
            {
                string requestApi = string.Format("Api/Purchaseing/DeleteGroupReceiptDetail?GroupReceiptDetailUID={0}&userID={1}", GroupReceiptDetailUID, userID);
                MeditechApiHelper.Delete(requestApi);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }


            return flag;

        }

        public bool CancelReceipt(long groupReceiptUID, string cancelReason, int userUID)
        {
            bool flag = false;

            try
            {
                string requestApi = string.Format("Api/Purchaseing/CancelReceipt?groupReceiptUID={0}&cancelReason={1}&userUID={2}", groupReceiptUID, cancelReason, userUID);
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

        #region GoodReceive

        public List<GRNDetailModel> SearchGoodReceive(DateTime? dateFrom, DateTime? dateTo, int? organisationUID, int? storeUID, int? venderoDetailUID, string invoinceNo, string GRNo)
        {
            string requestApi = string.Format("Api/Purchaseing/SearchGoodReceive?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&organisationUID={2}&storeUID={3}&venderoDetailUID={4}&invoinceNo={5}&GRNo={6}", dateFrom, dateTo, organisationUID, storeUID, venderoDetailUID, invoinceNo, GRNo);
            List<GRNDetailModel> dataRequest = MeditechApiHelper.Get<List<GRNDetailModel>>(requestApi);

            return dataRequest;
        }

        public List<GRNItemListModel> GetGoodReceiveItemByGRNDetailUID(int grnDetailUID)
        {
            string requestApi = string.Format("Api/Purchaseing/GetGoodReceiveItemByGRNDetailUID?grnDetailUID={0}", grnDetailUID);
            List<GRNItemListModel> dataRequest = MeditechApiHelper.Get<List<GRNItemListModel>>(requestApi);

            return dataRequest;
        }

        public GRNDetailModel GetGoodReceiveByUID(int grnDetailUID)
        {
            string requestApi = string.Format("Api/Purchaseing/GetGoodReceiveByUID?grnDetailUID={0}", grnDetailUID);
            GRNDetailModel dataRequest = MeditechApiHelper.Get<GRNDetailModel>(requestApi);

            return dataRequest;
        }

        public bool CreateGoodReceive(GRNDetailModel model, int userID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/Purchaseing/CreateGoodReceive?userID={0}", userID);
                MeditechApiHelper.Post<GRNDetailModel>(requestApi, model);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }
            return flag;
        }

        public bool CreateGoodReceiveFromEcount(GRNDetailModel model, int userID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/Purchaseing/CreateGoodReceiveFromEcount?userID={0}", userID);
                MeditechApiHelper.Post<GRNDetailModel>(requestApi, model);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }
            return flag;
        }


        public bool CancelGoodReceive(int grnDetailUID, string cancelReason, int userID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/Purchaseing/CancelGoodReceive?grnDetailUID={0}&cancelReason={1}&userID={2}", grnDetailUID, cancelReason, userID);
                MeditechApiHelper.Put(requestApi);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }
            return flag;
        }

        public CheckCancelTransactionInven CheckCancelGoodReceive(int grnDetailUID)
        {
            CheckCancelTransactionInven result;
            try
            {
                string requestApi = string.Format("Api/Purchaseing/CheckCancelGoodReceive?grnDetailUID={0}", grnDetailUID);
                result = MeditechApiHelper.Get<CheckCancelTransactionInven>(requestApi);
            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }

        #endregion

        #region VendorDetail
        public List<VendorDetailModel> GetVendorDetail()
        {
            string requestApi = string.Format("Api/Purchaseing/GetVendorDetail");
            List<VendorDetailModel> dataRequest = MeditechApiHelper.Get<List<VendorDetailModel>>(requestApi);

            return dataRequest;
        }

        public VendorDetailModel GetVendorDetailnByUID(int vendorDetailUID)
        {
            string requestApi = string.Format("Api/Purchaseing/GetVendorDetailnByUID?vendorDetailUID={0}", vendorDetailUID);
            VendorDetailModel dataRequest = MeditechApiHelper.Get<VendorDetailModel>(requestApi);

            return dataRequest;
        }

        public bool ManageVendorDetail(VendorDetailModel vendorDetailModel, int userID)
        {
            bool flag = false;

            try
            {
                string requestApi = string.Format("Api/Purchaseing/ManageVendorDetail?userID={0}", userID);
                MeditechApiHelper.Post<VendorDetailModel>(requestApi, vendorDetailModel);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }


            return flag;

        }

        public bool DeleteVendorDetail(int vendorDetailUID, int userID)
        {
            bool flag = false;

            try
            {
                string requestApi = string.Format("Api/Purchaseing/DeleteVendorDetail?vendorDetailUID={0}&userID={1}", vendorDetailUID, userID);
                MeditechApiHelper.Delete(requestApi);
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
