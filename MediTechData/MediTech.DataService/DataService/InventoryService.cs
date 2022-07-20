using MediTech.Model;
using ShareLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.DataService
{
    public class InventoryService
    {

        #region Itemmaster

        public string GenerateCodeItem()
        {
            string requestApi = "Api/Inventory/GenerateCodeItem";
            string itemCode = MeditechApiHelper.Get<string>(requestApi);

            return itemCode;
        }
        public List<ItemMasterModel> SearchItemMaster(string itemName, string itemCode, int? ITMTYPUID)
        {
            string requestApi = string.Format("Api/Inventory/SearchItemMaster?itemName={0}&itemCode={1}&ITMTYPUID={2}", itemName, itemCode, ITMTYPUID);
            List<ItemMasterModel> listItemMaster = MeditechApiHelper.Get<List<ItemMasterModel>>(requestApi);

            return listItemMaster;
        }

        public ItemMasterModel GetItemMasterByUID(int itemMasterUID)
        {
            string requestApi = string.Format("Api/Inventory/GetItemMasterByUID?itemMasterUID={0}", itemMasterUID);
            ItemMasterModel dataRequest = MeditechApiHelper.Get<ItemMasterModel>(requestApi);

            return dataRequest;
        }

        public ItemMasterModel GetItemMasterByCode(string itemCode)
        {
            string requestApi = string.Format("Api/Inventory/GetItemMasterByCode?itemCode={0}", itemCode);
            ItemMasterModel dataRequest = MeditechApiHelper.Get<ItemMasterModel>(requestApi);

            return dataRequest;
        }

        public List<EcountMassFileModel>GetEcountMassFile(int storeUID,int itemMasterUID,string serialNumber,DateTime? expiryDate)
        {
            string requestApi = string.Format("Api/Inventory/GetEcountMassFile?storeUID={0}&itemMasterUID={1}&serialNumber={2}&expiryDate={3}", storeUID,itemMasterUID,serialNumber,expiryDate);
            List<EcountMassFileModel> dataRequest = MeditechApiHelper.Get<List<EcountMassFileModel>>(requestApi);
            return dataRequest;
        }

        public List<ItemMasterModel> GetItemMaster()
        {
            string requestApi = string.Format("Api/Inventory/GetItemMaster");
            List<ItemMasterModel> dataRequest = MeditechApiHelper.Get<List<ItemMasterModel>>(requestApi);

            return dataRequest;
        }

        public List<ItemMasterModel> GetItemMasterByType(string itemType)
        {
            string requestApi = string.Format("Api/Inventory/GetItemMasterByType?itemType={0}", itemType);
            List<ItemMasterModel> dataRequest = MeditechApiHelper.Get<List<ItemMasterModel>>(requestApi);

            return dataRequest;

        }
        public List<ItemMasterModel> GetItemMasterQtyByStore(int storeUID)
        {
            string requestApi = string.Format("Api/Inventory/GetItemMasterQtyByStore?storeUID={0}", storeUID);
            List<ItemMasterModel> dataRequest = MeditechApiHelper.Get<List<ItemMasterModel>>(requestApi);

            return dataRequest;
        }


        public List<ItemMasterModel> GetItemMasterForIssue(int organisationUID, int storeUID)
        {
            string requestApi = string.Format("Api/Inventory/GetItemMasterForIssue?organisationUID={0}&storeUID={1}", organisationUID, storeUID);
            List<ItemMasterModel> dataRequest = MeditechApiHelper.Get<List<ItemMasterModel>>(requestApi);

            return dataRequest;
        }

        public bool ManageItemMaster(ItemMasterModel itemMasterModel, int userID)
        {
            bool flag = false;

            try
            {
                string requestApi = string.Format("Api/Inventory/ManageItemMaster?userID={0}", userID);
                MeditechApiHelper.Post<ItemMasterModel>(requestApi, itemMasterModel);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }


            return flag;
        }


        public bool DeleteItemMaster(int itemMasterUID, int userID)
        {
            bool flag = false;

            try
            {
                string requestApi = string.Format("Api/Inventory/DeleteItemMaster?itemMasterUID={0}&userID={1}", itemMasterUID, userID);
                MeditechApiHelper.Delete(requestApi);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }


            return flag;
        }

        public List<ItemAverageCostModel> GetItemAverageCost(int itemMasterUID, int? organisationUID)
        {
            string requestApi = string.Format("Api/Inventory/GetItemAverageCost?itemMasterUID={0}&organisationUID={1}", itemMasterUID, organisationUID);
            List<ItemAverageCostModel> dataRequest = MeditechApiHelper.Get<List<ItemAverageCostModel>>(requestApi);

            return dataRequest;
        }

        #endregion

        #region ItemUOMConversion
        public List<ItemUOMConversionModel> GetItemUOMConversionByItemMasterUID(int itemMasterUID)
        {
            string requestApi = string.Format("Api/Inventory/GetItemUOMConversionByItemMasterUID?itemMasterUID={0}", itemMasterUID);
            List<ItemUOMConversionModel> dataRequest = MeditechApiHelper.Get<List<ItemUOMConversionModel>>(requestApi);

            return dataRequest;
        }

        public List<ItemUOMConversionModel> GetItemUOMConversion()
        {
            string requestApi = "Api/Inventory/GetItemUOMConversion";
            List<ItemUOMConversionModel> dataRequest = MeditechApiHelper.Get<List<ItemUOMConversionModel>>(requestApi);

            return dataRequest;
        }

        public ItemUOMConversionModel GetItemUOMConversionByUID(int itemMasterUID)
        {
            string requestApi = string.Format("Api/Inventory/GetItemUOMConversionByUID?itemMasterUID={0}", itemMasterUID);
            ItemUOMConversionModel dataRequest = MeditechApiHelper.Get<ItemUOMConversionModel>(requestApi);

            return dataRequest;
        }

        public bool ManageItemUOMConversion(ItemUOMConversionModel itemUOMConversionModel, int userID)
        {
            bool flag = false;

            try
            {
                string requestApi = string.Format("Api/Inventory/ManageItemUOMConversion?userID={0}", userID);
                MeditechApiHelper.Post<ItemUOMConversionModel>(requestApi, itemUOMConversionModel);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }


            return flag;
        }

        public bool DeleteItemUOMConversion(int itemUOMConversionUID, int userID)
        {
            bool flag = false;

            try
            {
                string requestApi = string.Format("Api/Inventory/DeleteItemUOMConversion?itemUOMConversionUID={0}&userID={1}", itemUOMConversionUID, userID);
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

        #region ItemVendorDetail

        public List<ItemVendorDetailModel> GetItemVendorDetailByItemMasterUID(int itemMasterUID)
        {
            string requestApi = string.Format("Api/Inventory/GetItemVendorDetailByItemMasterUID?itemMasterUID={0}", itemMasterUID);
            List<ItemVendorDetailModel> dataRequest = MeditechApiHelper.Get<List<ItemVendorDetailModel>>(requestApi);

            return dataRequest;
        }

        #endregion

        #region StoreUOMConversion

        public List<StoreUOMConversionModel> GetStoreUOMConversion()
        {
            string requestApi = "Api/Inventory/GetStoreUOMConversion";
            List<StoreUOMConversionModel> dataRequest = MeditechApiHelper.Get<List<StoreUOMConversionModel>>(requestApi);

            return dataRequest;
        }

        public List<StoreUOMConversionModel> GetStoreConvertUOM(int itemMasterUID)
        {
            string requestApi = string.Format("Api/Inventory/GetStoreConvertUOM?itemMasterUID={0}", itemMasterUID);
            List<StoreUOMConversionModel> dataRequest = MeditechApiHelper.Get<List<StoreUOMConversionModel>>(requestApi);

            return dataRequest;
        }

        public List<ItemUOMConversionModel> GetItemConvertUOM(int itemMasterUID)
        {
            string requestApi = string.Format("Api/Inventory/GetItemConvertUOM?itemMasterUID={0}", itemMasterUID);
            List<ItemUOMConversionModel> dataRequest = MeditechApiHelper.Get<List<ItemUOMConversionModel>>(requestApi);

            return dataRequest;
        }

        public StoreUOMConversionModel GetStoreUOMConversionByUID(int storeUOMConversionUID)
        {
            string requestApi = string.Format("Api/Inventory/GetStoreUOMConversionByUID?storeUOMConversionUID={0}", storeUOMConversionUID);
            StoreUOMConversionModel dataRequest = MeditechApiHelper.Get<StoreUOMConversionModel>(requestApi);

            return dataRequest;
        }

        public bool ManageStoreUOMConversion(StoreUOMConversionModel storeUOMConversionModel, int userID)
        {
            bool flag = false;

            try
            {
                string requestApi = string.Format("Api/Inventory/ManageStoreUOMConversion?userID={0}", userID);
                MeditechApiHelper.Post<StoreUOMConversionModel>(requestApi, storeUOMConversionModel);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }


            return flag;
        }

        public bool DeleteStoreUOMConversion(int storeUOMConversionUID, int userID)
        {
            bool flag = false;

            try
            {
                string requestApi = string.Format("Api/Inventory/DeleteStoreUOMConversion?storeUOMConversionUID={0}&userID={1}", storeUOMConversionUID, userID);
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

        #region Store

        public List<StoreModel> GetStore()
        {
            string requestApi = "Api/Inventory/GetStore";
            List<StoreModel> dataRequest = MeditechApiHelper.Get<List<StoreModel>>(requestApi);

            return dataRequest;
        }

        public StoreModel GetStoreByUID(int storeUID)
        {
            string requestApi = string.Format("Api/Inventory/GetStoreByUID?storeUID={0}", storeUID);
            StoreModel dataRequest = MeditechApiHelper.Get<StoreModel>(requestApi);

            return dataRequest;
        }

        public List<StoreModel> GetStoreByOrganisationUID(int organisationUID)
        {
            string requestApi = string.Format("Api/Inventory/GetStoreByOrganisationUID?organisationUID={0}", organisationUID);
            List<StoreModel> dataRequest = MeditechApiHelper.Get<List<StoreModel>>(requestApi);

            return dataRequest;
        }

        public List<StoreModel> GetStoreByLocationUID(int locationUID)
        {
            string requestApi = string.Format("Api/Inventory/GetStoreByLocationUID?locationUID={0}", locationUID);
            List<StoreModel> dataRequest = MeditechApiHelper.Get<List<StoreModel>>(requestApi);

            return dataRequest;
        }

        public bool ManageStore(StoreModel storeModel, int userID)
        {
            bool flag = false;

            try
            {
                string requestApi = string.Format("Api/Inventory/ManageStore?userID={0}", userID);
                MeditechApiHelper.Post<StoreModel>(requestApi, storeModel);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }


            return flag;
        }

        public bool DeleteStore(int storeUID, int userID)
        {
            bool flag = false;

            try
            {
                string requestApi = string.Format("Api/Inventory/DeleteStore?storeUID={0}&userID={1}", storeUID, userID);
                MeditechApiHelper.Delete(requestApi);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }


            return flag;
        }

        public List<StockOnHandModel> SearchStockOnHand(int? ownerOrganisationUID, int? locationUID, int? storeUID, int? itemType, string itemCode, string itemName)
        {
            string requestApi = string.Format("Api/Inventory/SearchStockOnHand?ownerOrganisationUID={0}&locationUID={1}&storeUID={2}&itemType={3}&itemCode={4}&itemName={5}", ownerOrganisationUID, locationUID, storeUID, itemType, itemCode, itemName);
            List<StockOnHandModel> dataRequest = MeditechApiHelper.Get<List<StockOnHandModel>>(requestApi);

            return dataRequest;
        }

        public List<StockMovementModel> SearchStockMovement(int? ownerOrganisationUID, int? locationUID, int? storeUID, string itemCode, string itemName, string transactionType, DateTime? dateFrom, DateTime? dateTo)
        {
            string requestApi = string.Format("Api/Inventory/SearchStockMovement?ownerOrganisationUID={0}&locationUID={1}&storeUID={2}&itemCode={3}&itemName={4}&transactionType={5}&dateFrom={6:MM/dd/yyyy}&dateTo={7:MM/dd/yyyy}", ownerOrganisationUID, locationUID, storeUID, itemCode, itemName, transactionType, dateFrom, dateTo);
            List<StockMovementModel> dataRequest = MeditechApiHelper.Get<List<StockMovementModel>>(requestApi);

            return dataRequest;
        }

        public List<StockBalanceModel> SearchStockBalance(int? ownerOrganisationUID, int? locationUID, int? storeUID, string itemCode, string itemName, DateTime? dateFrom, DateTime? dateTo)
        {
            string requestApi = string.Format("Api/Inventory/SearchStockBalance?ownerOrganisationUID={0}&locationUID={1}&storeUID={2}&itemCode={3}&itemName={4}&dateFrom={5:MM/dd/yyyy}&dateTo={6:MM/dd/yyyy}", ownerOrganisationUID, locationUID, storeUID, itemCode, itemName, dateFrom, dateTo);
            List<StockBalanceModel> dataRequest = MeditechApiHelper.Get<List<StockBalanceModel>>(requestApi);

            return dataRequest;
        }

        public List<StockOnHandModel> SearchStockBatchByStoreUID(int storeUID, int itemMasterUID)
        {
            string requestApi = string.Format("Api/Inventory/SearchStockBatchByStoreUID?storeUID={0}&itemMasterUID={1}", storeUID, itemMasterUID);
            List<StockOnHandModel> dataRequest = MeditechApiHelper.Get<List<StockOnHandModel>>(requestApi);

            return dataRequest;
        }

        #endregion

        #region Stock

        public List<StockModel> GetStockRemainByItemMasterUID(int itemMasterUID, int organisation)
        {
            string requestApi = string.Format("Api/Inventory/GetStockRemainByItemMasterUID?itemMasterUID={0}&organisation={1}", itemMasterUID, organisation);
            List<StockModel> data = MeditechApiHelper.Get<List<StockModel>>(requestApi);

            return data;
        }

        public List<StockModel> GetStockRemainForDispensedByItemMasterUID(int itemMasterUID, int organisation)
        {
            string requestApi = string.Format("Api/Inventory/GetStockRemainForDispensedByItemMasterUID?itemMasterUID={0}&organisation={1}", itemMasterUID, organisation);
            List<StockModel> data = MeditechApiHelper.Get<List<StockModel>>(requestApi);

            return data;
        }

        public List<StockModel> GetStoreEcounByItemMaster(int itemMasterUID, int organisation)
        {
            string requestApi = string.Format("Api/Inventory/GetStoreEcounByItemMaster?itemMasterUID={0}&organisation={1}", itemMasterUID, organisation);
            List<StockModel> data = MeditechApiHelper.Get<List<StockModel>>(requestApi);

            return data;
        }


        public List<StockModel> SearchStockBatch(int? organisationUID,int? locationUID, int? storeUID, int? itemType, string itemCode, string itemName)
        {
            string requestApi = string.Format("Api/Inventory/SearchStockBatch?OrganisationUID={0}&locationUID={1}&storeUID={2}&itemType={3}&itemCode={4}&itemName={5}", organisationUID,locationUID, storeUID, itemType, itemCode, itemName);
            List<StockModel> data = MeditechApiHelper.Get<List<StockModel>>(requestApi);

            return data;
        }

        public List<StockModel> SearchStockForDispose(DateTime? dateFrom, DateTime? dateTo, int storeUID, string batchID, string itemName)
        {
            string requestApi = string.Format("Api/Inventory/SearchStockForDispose?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&storeUID={2}&batchID={3}&itemName={4}", dateFrom, dateTo, storeUID, batchID, itemName);
            List<StockModel> data = MeditechApiHelper.Get<List<StockModel>>(requestApi);

            return data;
        }

        public bool AdjustStock(StockAdjustmentModel model, int userUID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/Inventory/AdjustStock?userUID={0}", userUID);
                MeditechApiHelper.Post<StockAdjustmentModel>(requestApi, model);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }
            return flag;
        }

        public bool DisposeStock(List<StockModel> stockModel, int storeUID, int disposeReasonUID, string comments, int userUID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/Inventory/DisposeStock?storeUID={0}&disposeReasonUID={1}&comments={2}&userUID={3}", storeUID
                    , disposeReasonUID, comments, userUID);
                MeditechApiHelper.Post<List<StockModel>>(requestApi, stockModel);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }
            return flag;
        }

        public List<StockWorkListModel> SearchStockWorkList(DateTime? dateFrom, DateTime? dateTo, int? organisationToUID,int? locationUID)
        {
            string requestApi = string.Format("Api/Inventory/SearchStockWorkList?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&organisationUID={2}&locationUID={3}", dateFrom, dateTo, organisationToUID,locationUID);
            List<StockWorkListModel> returnData = MeditechApiHelper.Get<List<StockWorkListModel>>(requestApi);

            return returnData;
        }

        #endregion

        #region ItemRequest
        public List<ItemRequestModel> SearchItemRequest(DateTime? dateFrom, DateTime? dateTo, string requestID, int? organisationUID, int? locationUID, int? requestOnOrganistaionUID, int? requestOnLocationUID, int? requestStatus, int? priority)
        {
            string requestApi = string.Format("Api/Inventory/SearchItemRequest?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&requestID={2}&organisationUID={3}&locationUID={4}&requestOnOrganistaionUID={5}&requestOnLocationUID={6}&requestStatus={7}&priority={8}", dateFrom, dateTo, requestID, organisationUID, locationUID, requestOnOrganistaionUID, requestOnLocationUID, requestStatus, priority);
            List<ItemRequestModel> listItemRequest = MeditechApiHelper.Get<List<ItemRequestModel>>(requestApi);

            return listItemRequest;
        }

        public ItemRequestModel GetItemRequestByUID(int itemRequestUID)
        {
            string requestApi = string.Format("Api/Inventory/GetItemRequestByUID?itemRequestUID={0}", itemRequestUID);
            ItemRequestModel listItemRequest = MeditechApiHelper.Get<ItemRequestModel>(requestApi);

            return listItemRequest;
        }

        public List<ItemRequestDetailModel> GetItemRequestDetailByItemRequestUID(int itemRequestUID)
        {
            string requestApi = string.Format("Api/Inventory/GetItemRequestDetailByItemRequestUID?itemRequestUID={0}", itemRequestUID);
            List<ItemRequestDetailModel> returnData = MeditechApiHelper.Get<List<ItemRequestDetailModel>>(requestApi);

            return returnData;
        }

        public bool CancelItemRequest(int itemRequestUID, string cancelReason, int userUID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/Inventory/CancelItemRequest?itemRequestUID={0}&cancelReason={1}&userUID={2}", itemRequestUID, cancelReason, userUID);
                MeditechApiHelper.Put(requestApi);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }
            return flag;
        }

        public bool ManageItemRequest(ItemRequestModel model, int userUID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/Inventory/ManageItemRequest?userUID={0}", userUID);
                MeditechApiHelper.Post<ItemRequestModel>(requestApi, model);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }
            return flag;
        }

        #endregion

        #region ItemIssue

        public List<ItemIssueModel> SearchItemIssue(DateTime? dateFrom, DateTime? dateTo, string issueID, int ISSTPUID, int? issueStatus, int? organisationUID, int? locationUID, int? organisationToUID, int? locationToUID)
        {
            string requestApi = string.Format("Api/Inventory/SearchItemIssue?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&issueID={2}&ISSTPUID={3}&issueStatus={4}&organisationUID={5}&locationUID={6}&organisationToUID={7}&locationToUID={8}", dateFrom, dateTo, issueID, ISSTPUID, issueStatus, organisationUID,locationUID, organisationToUID,locationToUID);
            List<ItemIssueModel> returnData = MeditechApiHelper.Get<List<ItemIssueModel>>(requestApi);

            return returnData;
        }


        public List<ItemIssueModel> SearchItemIssueForListIssue(DateTime? dateFrom, DateTime? dateTo, string issueID, int? issueStatus, int? organisationUID, int? locationUID, int? organisationToUID, int? locationToUID)
        {
            string requestApi = string.Format("Api/Inventory/SearchItemIssueForListIssue?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&issueID={2}&issueStatus={3}&organisationUID={4}&locationUID={5}&organisationToUID={6}&locationToUID={7}", dateFrom, dateTo, issueID, issueStatus, organisationUID, locationUID, organisationToUID, locationToUID);
            List<ItemIssueModel> returnData = MeditechApiHelper.Get<List<ItemIssueModel>>(requestApi);

            return returnData;
        }

        public ItemIssueModel GetItemIssueByUID(int itemIssueUID)
        {
            string requestApi = string.Format("Api/Inventory/GetItemIssueByUID?itemIssueUID={0}", itemIssueUID);
            ItemIssueModel returnData = MeditechApiHelper.Get<ItemIssueModel>(requestApi);

            return returnData;
        }

        public List<ItemIssueDetailModel> GetItemIssueDetailByItemIssueUID(int itemIssueUID)
        {
            string requestApi = string.Format("Api/Inventory/GetItemIssueDetailByItemIssueUID?itemIssueUID={0}", itemIssueUID);
            List<ItemIssueDetailModel> returnData = MeditechApiHelper.Get<List<ItemIssueDetailModel>>(requestApi);

            return returnData;
        }

        public bool CancelItemIssue(int itemIssueUID, string cancelReason, int userUID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/Inventory/CancelItemIssue?itemIssueUID={0}&cancelReason={1}&userUID={2}", itemIssueUID, cancelReason, userUID);
                MeditechApiHelper.Put(requestApi);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }
            return flag;
        }

        public bool ManageItemIssue(ItemIssueModel model, int userUID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/Inventory/ManageItemIssue?userUID={0}", userUID);
                MeditechApiHelper.Post<ItemIssueModel>(requestApi, model);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }
            return flag;
        }

        public bool ManageItemIssueEount(ItemIssueModel model, int userUID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/Inventory/ManageItemIssueEount?userUID={0}", userUID);
                MeditechApiHelper.Post<ItemIssueModel>(requestApi, model);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }
            return flag;
        }




        public bool ConsumptionItem(IEnumerable<ItemIssueDetailModel> itemIssueDetailsModel,string comments, int userUID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/Inventory/ConsumptionItem?comments={0}&userUID={1}", comments, userUID);
                MeditechApiHelper.Post<IEnumerable<ItemIssueDetailModel>>(requestApi, itemIssueDetailsModel);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }
            return flag;
        }
        #endregion

        #region ItemTransfer

        public List<ItemIssueModel> SearchItemTranfer(DateTime? dateFrom, DateTime? dateTo, string issueID, int? issueStatus, int? organisationUID, int? locationUID, int? organisationToUID, int? locationToUID)
        {
            string requestApi = string.Format("Api/Inventory/SearchItemTranfer?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&issueID={2}&issueStatus={3}&organisationUID={4}&locationUID={5}&organisationToUID={6}&locationToUID={7}", dateFrom, dateTo, issueID, issueStatus, organisationUID, locationUID, organisationToUID, locationToUID);
            List<ItemIssueModel> returnData = MeditechApiHelper.Get<List<ItemIssueModel>>(requestApi);

            return returnData;
        }

        public bool ManageItemTransferEcount(ItemIssueModel model, int userUID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/Inventory/ManageItemTransferEcount?userUID={0}", userUID);
                MeditechApiHelper.Post<ItemIssueModel>(requestApi, model);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }
            return flag;
        }


        public bool ManageItemTransfer(ItemIssueModel model, int userUID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/Inventory/ManageItemTransfer?userUID={0}", userUID);
                MeditechApiHelper.Post<ItemIssueModel>(requestApi, model);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }
            return flag;
        }

        public bool CancelItemTransfer(int itemIssueUID, string cancelReason, int userUID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/Inventory/CancelItemTransfer?itemIssueUID={0}&cancelReason={1}&userUID={2}", itemIssueUID, cancelReason, userUID);
                MeditechApiHelper.Put(requestApi);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }
            return flag;
        }

        public CheckCancelTransactionInven CheckCancelTransfer(int itemIssueUID)
        {
            CheckCancelTransactionInven result;
            try
            {
                string requestApi = string.Format("Api/Inventory/CheckCancelTransfer?itemIssueUID={0}", itemIssueUID);
                result = MeditechApiHelper.Get<CheckCancelTransactionInven>(requestApi);
            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }

        #endregion

        #region ItemReceive

        public List<ItemReceiveModel> SearchItemReceive(DateTime? dateFrom, DateTime? dateTo, string receiveID, int? organisationIssueUID, int? locationIssueUID, int? organisationReceiveUID, int? locationReceiveUID)
        {
            string requestApi = string.Format("Api/Inventory/SearchItemReceive?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&receiveID={2}&organisationIssueUID={3}&locationIssueUID={4}&organisationReceiveUID={5}&locationReceiveUID={6}", dateFrom, dateTo, receiveID, organisationIssueUID,locationIssueUID, organisationReceiveUID,locationReceiveUID);
            List<ItemReceiveModel> returnData = MeditechApiHelper.Get<List<ItemReceiveModel>>(requestApi);

            return returnData;
        }

        public ItemReceiveModel GetItemReceiveByUID(int itemReceiveUID)
        {
            string requestApi = string.Format("Api/Inventory/GetItemReceiveByUID?itemReceiveUID={0}", itemReceiveUID);
            ItemReceiveModel returnData = MeditechApiHelper.Get<ItemReceiveModel>(requestApi);

            return returnData;
        }

        public List<ItemReceiveDetailModel> GetItemReceiveDetailByItemReceiveUID(int itemReceiveUID)
        {
            string requestApi = string.Format("Api/Inventory/GetItemReceiveDetailByItemReceiveUID?itemReceiveUID={0}", itemReceiveUID);
            List<ItemReceiveDetailModel> returnData = MeditechApiHelper.Get<List<ItemReceiveDetailModel>>(requestApi);

            return returnData;
        }

        public bool CancelItemReceive(int itemReceiveUID, string cancelReason, int userUID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/Inventory/CancelItemReceive?itemReceiveUID={0}&cancelReason={1}&userUID={2}", itemReceiveUID, cancelReason, userUID);
                MeditechApiHelper.Put(requestApi);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }
            return flag;
        }

        public CheckCancelTransactionInven CheckCancelReceive(int itemReceiveUID)
        {
            CheckCancelTransactionInven result;
            try
            {
                string requestApi = string.Format("Api/Inventory/CheckCancelReceive?itemReceiveUID={0}", itemReceiveUID);
                result = MeditechApiHelper.Get<CheckCancelTransactionInven>(requestApi);
            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }

        public bool ManageItemReceive(ItemReceiveModel model, int userUID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/Inventory/ManageItemReceive?userUID={0}", userUID);
                MeditechApiHelper.Post<ItemReceiveModel>(requestApi, model);
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
