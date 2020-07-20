using MediTech.DataBase;
using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Data;
using System.Data.Entity.Migrations;
using System.Transactions;
using System.Web.Http;
using System.Data.Entity;

namespace MediTechWebApi.Controllers
{
    [RoutePrefix("Api/Purchaseing")]
    public class PurchaseingController : ApiController
    {
        protected MediTechEntities db = new MediTechEntities();

        #region Purchaseint

        [Route("SearchPurchaseOrder")]
        [HttpGet]
        public List<PurchaseOrderModel> SearchPurchaseOrder(DateTime? dateFrom, DateTime? dateTo, int? organisationUID, int? storeUID, int? venderoDetailUID, int? poStatus, string PONo)
        {
            List<PurchaseOrderModel> data = (from po in db.PurchaseOrder
                                             where po.StatusFlag == "A"
                                             && (dateFrom == null || DbFunctions.TruncateTime(po.RequestedDttm) >= DbFunctions.TruncateTime(dateFrom))
                                             && (dateTo == null || DbFunctions.TruncateTime(po.RequestedDttm) <= DbFunctions.TruncateTime(dateTo))
                                             && (venderoDetailUID == null || po.VendorDetailUID == venderoDetailUID)
                                             && (organisationUID == null || po.OwnerOrganisationUID == organisationUID)
                                             && (storeUID == null || po.DelieryToStoreUID == storeUID)
                                             && (poStatus == null || po.POSTSUID == poStatus)
                                             && (string.IsNullOrEmpty(PONo) || po.PurchaseOrderID == PONo)
                                             select new PurchaseOrderModel
                                             {
                                                 PurchaseOrderUID = po.UID,
                                                 VendorDetailUID = po.VendorDetailUID,
                                                 VendorName = SqlFunction.fGetVendorName(po.VendorDetailUID),
                                                 HealthOrganisationUID = po.OwnerOrganisationUID,
                                                 HealthOrganisationName = SqlFunction.fGetHealthOrganisationName(po.OwnerOrganisationUID ?? 0),
                                                 RequiredDttm = po.RequiredDttm,
                                                 RequestedDttm = po.RequestedDttm,
                                                 Comments = po.Comments,
                                                 POSTSUID = po.POSTSUID,
                                                 POStatus = SqlFunction.fGetRfValDescription(po.POSTSUID),
                                                 OtherCharges = po.OtherCharges,
                                                 ApprovalComments = po.ApprovalComments,
                                                 ApprovedBy = po.ApprovedBy,
                                                 Approved = SqlFunction.fGetCareProviderName(po.ApprovedBy ?? 0),
                                                 NetAmount = po.NetAmount,
                                                 Discount = po.Discount,
                                                 DelieryToStoreUID = po.DelieryToStoreUID,
                                                 StoreName = SqlFunction.fGetStoreName(po.DelieryToStoreUID ?? 0),
                                                 PurchaseOrderID = po.PurchaseOrderID
                                             }).ToList();

            return data;
        }

        [Route("GetPurchaseOrderByUID")]
        [HttpGet]
        public PurchaseOrderModel GetPurchaseOrderByUID(int purchaseOrderUID)
        {
            PurchaseOrderModel data = (from po in db.PurchaseOrder
                                       where po.UID == purchaseOrderUID
                                       select new PurchaseOrderModel
                                       {
                                           PurchaseOrderUID = po.UID,
                                           VendorDetailUID = po.VendorDetailUID,
                                           VendorName = SqlFunction.fGetVendorName(po.VendorDetailUID),
                                           HealthOrganisationUID = po.OwnerOrganisationUID,
                                           HealthOrganisationName = SqlFunction.fGetHealthOrganisationName(po.OwnerOrganisationUID ?? 0),
                                           RequiredDttm = po.RequiredDttm,
                                           RequestedDttm = po.RequestedDttm,
                                           Comments = po.Comments,
                                           POSTSUID = po.POSTSUID,
                                           POStatus = SqlFunction.fGetRfValDescription(po.POSTSUID),
                                           ApprovalComments = po.ApprovalComments,
                                           ApprovedBy = po.ApprovedBy,
                                           Approved = SqlFunction.fGetCareProviderName(po.ApprovedBy ?? 0),
                                           OtherCharges = po.OtherCharges,
                                           NetAmount = po.NetAmount,
                                           Discount = po.Discount,
                                           DelieryToStoreUID = po.DelieryToStoreUID,
                                           StoreName = SqlFunction.fGetStoreName(po.DelieryToStoreUID ?? 0),
                                           PurchaseOrderID = po.PurchaseOrderID
                                       }).FirstOrDefault();
            if (data != null)
            {
                data.PurchaseOrderItemList = db.PurchaseOrderItemList
                        .Where(p => p.PurchaseOrderUID == data.PurchaseOrderUID && p.StatusFlag == "A")
                        .Select(p => new PurchaseOrderItemListModel
                        {
                            PurchaseOrderItemListUID = p.UID,
                            PurchaseOrderUID = p.PurchaseOrderUID,
                            ItemMasterUID = p.ItemMasterUID,
                            ItemName = p.ItemName,
                            ItemCode = p.ItemCode,
                            Quantity = p.Quantity,
                            IMUOMUID = p.IMUOMUID,
                            NetAmount = p.NetAmount,
                            Unit = SqlFunction.fGetRfValDescription(p.IMUOMUID),
                            UnitPrice = p.UitPrice,
                            Comments = p.Comments
                        }).ToList();

                data.PurchaseOrderPayments = db.PurchaseOrderPayment
                        .Where(p => p.PurchaseOrderUID == data.PurchaseOrderUID && p.StatusFlag == "A")
                        .Select(p => new PurchaseOrderPaymentModel
                        {
                            PurchaseOrderUID = p.PurchaseOrderUID,
                            PurchaseOrderPaymentUID = p.UID,
                            Amount = p.Amount,
                            CURNCUID = p.CURNCUID,
                            PAYMDUID = p.PAYMDUID,
                            CurrencyType = SqlFunction.fGetRfValDescription(p.CURNCUID),
                            PaymentType = SqlFunction.fGetRfValDescription(p.PAYMDUID),
                            ExpiryDttm = p.ExpiryDttm,
                            PaidDttm = p.PaidDttm

                        }).ToList();

            }

            return data;
        }

        [Route("GetPurchaseOrderItemListByPurchaseOrderUID")]
        [HttpGet]
        public List<PurchaseOrderItemListModel> GetPurchaseOrderItemListByPurchaseOrderUID(int purchaseOrderUID)
        {
            List<PurchaseOrderItemListModel> data = db.PurchaseOrderItemList
                        .Where(p => p.PurchaseOrderUID == purchaseOrderUID && p.StatusFlag == "A")
                        .Select(p => new PurchaseOrderItemListModel
                        {
                            PurchaseOrderItemListUID = p.UID,
                            PurchaseOrderUID = p.PurchaseOrderUID,
                            ItemMasterUID = p.ItemMasterUID,
                            ItemName = p.ItemName,
                            ItemCode = p.ItemCode,
                            Quantity = p.Quantity,
                            IMUOMUID = p.IMUOMUID,
                            NetAmount = p.NetAmount,
                            Unit = SqlFunction.fGetRfValDescription(p.IMUOMUID),
                            UnitPrice = p.UitPrice,
                            Comments = p.Comments
                        }).ToList();

            return data;
        }

        [Route("CancelPurchaseOrder")]
        [HttpPut]
        public HttpResponseMessage CancelPurchaseOrder(int purchaseOrderUID, string cancelReason, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                PurchaseOrder purchaseOrder = db.PurchaseOrder.Find(purchaseOrderUID);
                if (purchaseOrder != null)
                {
                    db.PurchaseOrder.Attach(purchaseOrder);
                    purchaseOrder.MUser = userID;
                    purchaseOrder.MWhen = now;
                    purchaseOrder.POSTSUID = 2909;
                    purchaseOrder.CancelReason = cancelReason;
                    db.SaveChanges();
                    //using (var tran = new TransactionScope())
                    //{
                    //    db.PurchaseOrder.Attach(purchaseOrder);
                    //    purchaseOrder.MUser = userID;
                    //    purchaseOrder.MWhen = now;
                    //    purchaseOrder.CancelReason = cancelReason;
                    //    db.SaveChanges();

                    //    List<PurchaseOrderItemList> polist = db.PurchaseOrderItemList.Where(p => p.PurchaseOrderUID == purchaseOrder.UID).ToList();
                    //    foreach (var item in polist)
                    //    {
                    //        db.PurchaseOrderItemList.Attach(item);
                    //        item.MUser = userID;
                    //        item.MWhen = now;
                    //        item.StatusFlag = "D";
                    //        db.SaveChanges();
                    //    }

                    //    List<PurchaseOrderPayment> poPay = db.PurchaseOrderPayment.Where(p => p.PurchaseOrderUID == purchaseOrder.UID).ToList();
                    //    foreach (var item in poPay)
                    //    {
                    //        db.PurchaseOrderPayment.Attach(item);
                    //        item.MUser = userID;
                    //        item.MWhen = now;
                    //        item.StatusFlag = "D";
                    //        db.SaveChanges();
                    //    }

                    //    tran.Complete();
                    //}

                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("UpdatePurchaseOrderStatus")]
        [HttpPut]
        public HttpResponseMessage UpdatePurchaseOrderStatus(int purchaseOrderUID, int POSTSUID, string approvalComments, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                PurchaseOrder purchase = db.PurchaseOrder.Find(purchaseOrderUID);
                if (purchase != null)
                {
                    purchase.POSTSUID = POSTSUID;
                    purchase.MUser = userID;
                    purchase.MWhen = now;

                    if (POSTSUID == 2904)
                    {
                        purchase.ApprovedBy = userID;
                        purchase.ApprovalComments = approvalComments;
                    }

                    db.SaveChanges();
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("ManagePurchaseOrder")]
        [HttpPost]
        public HttpResponseMessage ManagePurchaseOrder(PurchaseOrderModel model, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;

                using (var tran = new TransactionScope())
                {
                    PurchaseOrder po = db.PurchaseOrder.Find(model.PurchaseOrderUID);
                    if (po == null)
                    {
                        po = new PurchaseOrder();
                        po.CUser = userID;
                        po.CWhen = now;

                        int purchaseID;
                        string POID = SEQHelper.GetSEQIDFormat("SEQPROID", out purchaseID);


                        if (string.IsNullOrEmpty(POID))
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No SEQPROID in SEQCONFIGURATION");
                        }

                        if (purchaseID == 0)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Insert SEQPROID is Fail");
                        }

                        po.PurchaseOrderID = POID;
                    }

                    po.MUser = userID;
                    po.MWhen = now;
                    po.StatusFlag = "A";
                    po.VendorDetailUID = model.VendorDetailUID;
                    po.RequiredDttm = model.RequiredDttm;
                    po.RequestedDttm = model.RequestedDttm;
                    po.DelieryToStoreUID = model.DelieryToStoreUID;
                    po.OwnerOrganisationUID = model.HealthOrganisationUID;
                    po.Comments = model.Comments;
                    po.POSTSUID = model.POSTSUID;
                    po.OtherCharges = model.OtherCharges;
                    po.NetAmount = model.NetAmount;
                    po.Discount = model.Discount;
                    db.PurchaseOrder.AddOrUpdate(po);

                    db.SaveChanges();

                    #region DeletePurchaseItemList

                    List<PurchaseOrderItemList> purchaseOrderItemList = db.PurchaseOrderItemList.Where(p => p.PurchaseOrderUID == model.PurchaseOrderUID && p.StatusFlag == "A").ToList();

                    foreach (var item in purchaseOrderItemList)
                    {
                        var data = model.PurchaseOrderItemList.FirstOrDefault(p => p.PurchaseOrderItemListUID == item.UID);
                        if (data == null)
                        {
                            db.PurchaseOrderItemList.Attach(item);
                            item.MUser = userID;
                            item.MWhen = now;
                            item.StatusFlag = "D";
                        }

                    }

                    db.SaveChanges();
                    #endregion

                    #region SavePurchaseItemList

                    foreach (var item in model.PurchaseOrderItemList)
                    {

                        PurchaseOrderItemList poItemList = db.PurchaseOrderItemList.Find(item.PurchaseOrderItemListUID);
                        if (poItemList == null)
                        {
                            poItemList = new PurchaseOrderItemList();
                            poItemList.CUser = userID;
                            poItemList.CWhen = now;
                        }

                        poItemList.PurchaseOrderUID = po.UID;
                        poItemList.MUser = userID;
                        poItemList.MWhen = now;
                        poItemList.StatusFlag = "A";
                        poItemList.ItemMasterUID = item.ItemMasterUID;
                        poItemList.ItemName = item.ItemName;
                        poItemList.ItemCode = item.ItemCode;
                        poItemList.Quantity = item.Quantity;
                        poItemList.IMUOMUID = item.IMUOMUID;
                        poItemList.UitPrice = item.UnitPrice;
                        poItemList.NetAmount = item.NetAmount;
                        poItemList.Comments = item.Comments;
                        db.PurchaseOrderItemList.AddOrUpdate(poItemList);

                        db.SaveChanges();

                    }


                    #endregion


                    #region DeletePurchaseOrderPayment
                    List<PurchaseOrderPayment> purchaseOrderPayments = db.PurchaseOrderPayment.Where(p => p.PurchaseOrderUID == model.PurchaseOrderUID && p.StatusFlag == "A").ToList();
                    if (model.PurchaseOrderPayments == null)
                    {
                        foreach (var item in purchaseOrderPayments)
                        {
                            db.PurchaseOrderPayment.Attach(item);
                            item.MUser = userID;
                            item.MWhen = now;
                            item.StatusFlag = "D";
                        }
                    }
                    else
                    {

                        foreach (var item in purchaseOrderPayments)
                        {
                            var data = model.PurchaseOrderPayments.FirstOrDefault(p => p.PurchaseOrderPaymentUID == item.UID);
                            if (data == null)
                            {
                                db.PurchaseOrderPayment.Attach(item);
                                item.MUser = userID;
                                item.MWhen = now;
                                item.StatusFlag = "D";
                            }

                        }
                    }

                    db.SaveChanges();
                    #endregion

                    #region SavePurchaseOrderPayment

                    if (model.PurchaseOrderPayments != null)
                    {
                        foreach (var item in model.PurchaseOrderPayments)
                        {

                            PurchaseOrderPayment poPayment = db.PurchaseOrderPayment.Find(item.PurchaseOrderPaymentUID);
                            if (poPayment == null)
                            {
                                poPayment = new PurchaseOrderPayment();
                                poPayment.CUser = userID;
                                poPayment.CWhen = now;
                            }

                            poPayment.PurchaseOrderUID = po.UID;
                            poPayment.MUser = userID;
                            poPayment.MWhen = now;
                            poPayment.StatusFlag = "A";
                            poPayment.Amount = item.Amount;
                            poPayment.CURNCUID = item.CURNCUID;
                            poPayment.PAYMDUID = item.PAYMDUID;
                            poPayment.ExpiryDttm = item.ExpiryDttm;
                            poPayment.PaidDttm = item.PaidDttm;
                            db.PurchaseOrderPayment.AddOrUpdate(poPayment);

                            db.SaveChanges();

                        }
                    }



                    #endregion

                    tran.Complete();
                }



                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        #endregion

        #region GoodReceive
        [Route("SearchGoodReceive")]
        [HttpGet]
        public List<GRNDetailModel> SearchGoodReceive(DateTime? dateFrom, DateTime? dateTo, int? organisationUID, int? storeUID, int? venderoDetailUID, string invoinceNo, string GRNo)
        {
            List<GRNDetailModel> data = (from gr in db.GRNDetail
                                         where gr.StatusFlag == "A"
                                         && (dateFrom == null || DbFunctions.TruncateTime(gr.RecievedDttm) >= DbFunctions.TruncateTime(dateFrom))
                                         && (dateTo == null || DbFunctions.TruncateTime(gr.RecievedDttm) <= DbFunctions.TruncateTime(dateTo))
                                         && (organisationUID == null || gr.RecievedOrganisationUID == organisationUID)
                                         && (storeUID == null || gr.RecievedStoreUID == storeUID)
                                         && (venderoDetailUID == null || gr.VendorDetailUID == venderoDetailUID)
                                         && (string.IsNullOrEmpty(invoinceNo) || gr.InvoiceNo == invoinceNo)
                                         && (string.IsNullOrEmpty(GRNo) || gr.GRNNumber == GRNo)
                                         select new GRNDetailModel
                                         {
                                             GRNDetailUID = gr.UID,
                                             VendorDetailUID = gr.VendorDetailUID,
                                             VendorName = SqlFunction.fGetVendorName(gr.VendorDetailUID),
                                             GRNNumber = gr.GRNNumber,
                                             RecievedStoreUID = gr.RecievedStoreUID,
                                             RecievedOrganisationUID = gr.RecievedOrganisationUID,
                                             RecievedOrganisationName = SqlFunction.fGetHealthOrganisationName(gr.RecievedOrganisationUID),
                                             StoreName = SqlFunction.fGetStoreName(gr.RecievedStoreUID),
                                             Comments = gr.Comments,
                                             RecievedDttm = gr.RecievedDttm,
                                             Discount = gr.Discount,
                                             NetAmount = gr.NetAmount,
                                             OtherCharges = gr.OtherCharges,
                                             InvoiceDate = gr.InvoiceDate,
                                             InvoiceNo = gr.InvoiceNo,
                                             GRNTYPUID = gr.GRNTYPUID,
                                             GRNType = SqlFunction.fGetRfValDescription(gr.GRNTYPUID ?? 0),
                                             GRNSTSUID = gr.GRNSTSUID,
                                             GRNStatus = SqlFunction.fGetRfValDescription(gr.GRNSTSUID ?? 0),
                                             CancelReason = gr.CancelReason,
                                             PurchaseOrderID = gr.PurchaseOrderID
                                         }).ToList();
            return data;
        }


        [Route("GetGoodReceiveItemByGRNDetailUID")]
        [HttpGet]
        public List<GRNItemListModel> GetGoodReceiveItemByGRNDetailUID(int grnDetailUID)
        {
            List<GRNItemListModel> data = db.GRNItemList
                        .Where(p => p.GRNDetailUID == grnDetailUID)
                        .Select(p => new GRNItemListModel
                        {
                            GRNDetailUID = p.GRNDetailUID,
                            GRNItemListUID = p.UID,
                            ItemMasterUID = p.ItemMasterUID,
                            ItemName = p.ItemName,
                            Quantity = p.Quantity,
                            FreeQuantity = p.FreeQuantity,
                            ManufacturerUID = p.ManufacturerUID,
                            ManufacturerName = SqlFunction.fGetRfValDescription(p.ManufacturerUID ?? 0),
                            Discount = p.Discount,
                            TaxPercentage = p.PTaxPercentage,
                            IMUOMUID = p.IMUOMUID,
                            Unit = SqlFunction.fGetRfValDescription(p.IMUOMUID),
                            BatchID = p.BatchID,
                            ExpiryDttm = p.ExpiryDttm,
                            PurchaseCost = p.PurchaseCost,
                            NetAmount = p.NetAmount,
                            ITCATUID = p.ITCATUID,
                        }).ToList();
            return data;
        }

        [Route("GetGoodReceiveByUID")]
        [HttpGet]
        public GRNDetailModel GetGoodReceiveByUID(int grnDetailUID)
        {
            GRNDetailModel data = (from gr in db.GRNDetail
                                   where gr.StatusFlag == "A" && gr.UID == grnDetailUID
                                   select new GRNDetailModel
                                   {
                                       GRNDetailUID = gr.UID,
                                       VendorDetailUID = gr.VendorDetailUID,
                                       VendorName = SqlFunction.fGetVendorName(gr.VendorDetailUID),
                                       GRNNumber = gr.GRNNumber,
                                       RecievedStoreUID = gr.RecievedStoreUID,
                                       RecievedOrganisationUID = gr.RecievedOrganisationUID,
                                       RecievedOrganisationName = SqlFunction.fGetHealthOrganisationName(gr.RecievedOrganisationUID),
                                       StoreName = SqlFunction.fGetStoreName(gr.RecievedStoreUID),
                                       Comments = gr.Comments,
                                       RecievedDttm = gr.RecievedDttm,
                                       Discount = gr.Discount,
                                       NetAmount = gr.NetAmount,
                                       OtherCharges = gr.OtherCharges,
                                       InvoiceDate = gr.InvoiceDate,
                                       InvoiceNo = gr.InvoiceNo,
                                       GRNSTSUID = gr.GRNSTSUID,
                                       GRNStatus = SqlFunction.fGetRfValDescription(gr.GRNSTSUID ?? 0),
                                       GRNTYPUID = gr.GRNTYPUID,
                                       GRNType = SqlFunction.fGetRfValDescription(gr.GRNTYPUID ?? 0),
                                       CancelReason = gr.CancelReason,
                                       PurchaseOrderID = gr.PurchaseOrderID
                                   }).FirstOrDefault();
            if (data != null)
            {

                data.GRNItemLists = db.GRNItemList
                        .Where(p => p.GRNDetailUID == data.GRNDetailUID)
                        .Select(p => new GRNItemListModel
                        {
                            GRNDetailUID = p.GRNDetailUID,
                            GRNItemListUID = p.UID,
                            ItemMasterUID = p.ItemMasterUID,
                            ItemName = p.ItemName,
                            Quantity = p.Quantity,
                            FreeQuantity = p.FreeQuantity,
                            Discount = p.Discount,
                            TaxPercentage = p.PTaxPercentage,
                            IMUOMUID = p.IMUOMUID,
                            Unit = SqlFunction.fGetRfValDescription(p.IMUOMUID),
                            BatchID = p.BatchID,
                            ExpiryDttm = p.ExpiryDttm,
                            PurchaseCost = p.PurchaseCost,
                            NetAmount = p.NetAmount,
                            ITCATUID = p.ITCATUID,
                        }).ToList();

            }

            return data;
        }

        [Route("CreateGoodReceive")]
        [HttpPost]
        public HttpResponseMessage CreateGoodReceive(GRNDetailModel model, int userID)
        {

            try
            {
                DateTime now = DateTime.Now;

                using (var tran = new TransactionScope())
                {
                    GRNDetail GR = new GRNDetail();
                    GR.CUser = userID;
                    GR.CWhen = now;

                    int seqID;
                    string GRID = SEQHelper.GetSEQIDFormat("SEQGRNID", out seqID);


                    if (string.IsNullOrEmpty(GRID))
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No SEQGRNID in SEQCONFIGURATION");
                    }

                    if (seqID == 0)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Insert SEQGRNID is Fail");
                    }

                    GR.GRNNumber = GRID;

                    GR.MUser = userID;
                    GR.MWhen = now;
                    GR.StatusFlag = "A";
                    GR.VendorDetailUID = model.VendorDetailUID;
                    GR.RecievedStoreUID = model.RecievedStoreUID;
                    GR.RecievedOrganisationUID = model.RecievedOrganisationUID;
                    GR.RecievedDttm = now;
                    GR.Comments = model.Comments;
                    GR.OtherCharges = model.OtherCharges;
                    GR.NetAmount = model.NetAmount;
                    GR.Discount = model.Discount;
                    GR.InvoiceNo = model.InvoiceNo;
                    GR.GRNSTSUID = 2910;
                    GR.InvoiceDate = model.InvoiceDate;
                    //GR.GRNStatusUID = model.GRNStatusUID;
                    GR.CancelReason = model.CancelReason;
                    GR.PurchaseOrderID = model.PurchaseOrderID;
                    GR.GRNTYPUID = model.GRNTYPUID;
                    db.GRNDetail.AddOrUpdate(GR);

                    db.SaveChanges();


                    if (!string.IsNullOrEmpty(model.PurchaseOrderID))
                    {
                        PurchaseOrder purchaseOrder = db.PurchaseOrder.FirstOrDefault(p => p.PurchaseOrderID == model.PurchaseOrderID);
                        if (purchaseOrder != null)
                        {
                            db.PurchaseOrder.Attach(purchaseOrder);
                            purchaseOrder.MUser = userID;
                            purchaseOrder.MWhen = now;
                            purchaseOrder.GRNNumber = GR.GRNNumber;
                            purchaseOrder.POSTSUID = 2905;
                            db.SaveChanges();
                        }
                    }

                    foreach (var item in model.GRNItemLists)
                    {
                        GRNItemList poItemList = new GRNItemList();
                        poItemList.CUser = userID;
                        poItemList.CWhen = now;
                        poItemList.GRNDetailUID = GR.UID;
                        poItemList.MUser = userID;
                        poItemList.MWhen = now;
                        poItemList.StatusFlag = "A";
                        poItemList.ItemMasterUID = item.ItemMasterUID;
                        poItemList.ItemName = item.ItemName;
                        poItemList.ItemCode = item.ItemCode;
                        poItemList.Quantity = item.Quantity;
                        poItemList.FreeQuantity = item.FreeQuantity;
                        poItemList.Discount = item.Discount;
                        poItemList.PTaxPercentage = item.TaxPercentage;
                        poItemList.ManufacturerUID = item.ManufacturerUID;
                        poItemList.IMUOMUID = item.IMUOMUID;
                        poItemList.ExpiryDttm = item.ExpiryDttm;
                        poItemList.PurchaseCost = item.PurchaseCost;
                        poItemList.NetAmount = item.NetAmount ?? 0;
                        poItemList.BatchID = item.BatchID;
                        poItemList.ITCATUID = item.ITCATUID;
                        db.GRNItemList.Add(poItemList);

                        db.SaveChanges();

                        double totalQuantity = poItemList.Quantity + (item.FreeQuantity ?? 0);
                        SqlDirectStore.pInvenGoodReceive(poItemList.ItemMasterUID, model.RecievedStoreUID, null, userID, GR.RecievedOrganisationUID, model.Comments, poItemList.IMUOMUID, poItemList.ExpiryDttm, poItemList.BatchID, totalQuantity, poItemList.PurchaseCost, GR.VendorDetailUID, poItemList.ManufacturerUID, GR.UID, GR.RecievedDttm, GR.GRNNumber, "GRNItemList", poItemList.UID, null);

                    }
                    tran.Complete();
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("CancelGoodReceive")]
        [HttpPut]
        public HttpResponseMessage CancelGoodReceive(int grnDetailUID, string cancelReason, int userID)
        {
            try
            {
                SqlDirectStore.pInvenCancelGRNDetail(grnDetailUID, cancelReason, userID);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("CheckCancelGoodReceive")]
        [HttpGet]
        public CheckCancelTransactionInven CheckCancelGoodReceive(int grnDetailUID)
        {
            var grnItemLists = from i in db.GRNDetail
                               join j in db.GRNItemList on i.UID equals j.GRNDetailUID
                               where i.StatusFlag == "A"
                               && j.StatusFlag == "A"
                               && i.UID == grnDetailUID
                               select j;
            foreach (var grnItem in grnItemLists)
            {
                var stockMovement = (from i in db.StockMovement
                                     where i.StatusFlag == "A"
                                     && i.RefTable == "GRNItemList"
                                     && i.RefUID == grnItem.UID
                                     select i).FirstOrDefault();
                if (stockMovement != null)
                {
                    var stockTransaction = (from i in db.StockMovement
                                            where i.StatusFlag == "A"
                                            && i.StockUID == stockMovement.StockUID
                                            select i);
                    if (stockTransaction != null)
                    {
                        stockTransaction = stockTransaction.Where(p => p.RefTable != "StockAdjustment"
                        && p.RefTable != "SaleReturnList"
                        && ((p.RefTable == "DispensedItem" && p.Note == "SaleReturn") ? "SaleReturnList" : "DispensedItem") != "SaleReturnList"
                        );

                        if (stockTransaction.Count() > 1)
                        {
                            return new CheckCancelTransactionInven() { IsActive = false };
                        }
                    }
                }

            }

            return new CheckCancelTransactionInven() { IsActive = true };
        }

        #endregion

        #region Vendor

        [Route("GetVendorDetail")]
        [HttpGet]
        public List<VendorDetailModel> GetVendorDetail()
        {
            List<VendorDetailModel> data = db.VendorDetail.Where(p => p.StatusFlag == "A").Select(p => new VendorDetailModel()
            {
                VendorDetailUID = p.UID,
                CompanyName = p.CompanyName,
                ContactPerson = p.ContactPerson,
                MobileNo = p.MobileNo,
                FaxNo = p.FaxNo,
                Email = p.Email,
                Address = p.Address,
                MNFTPUID = p.MNFTPUID,
                VendorType = SqlFunction.fGetRfValDescription(p.MNFTPUID),
                TINNo = p.TINNo,
                ProvinceUID = p.ProvinceUID,
                DistrictUID = p.DistrictUID,
                AmphurUID = p.AmphurUID,
                ZipCode = p.ZipCode,
                AddressFull = SqlFunction.fGetAddressVendor(p.UID),
                ActiveFrom = p.ActiveFrom,
                ActiveTo = p.ActiveTo,
                CUser = p.CUser,
                CWhen = p.CWhen,
                MUser = p.MUser,
                MWhen = p.MWhen,
                StatusFlag = p.StatusFlag
            }).ToList();

            return data;
        }

        [Route("GetVendorDetailnByUID")]
        [HttpGet]
        public VendorDetailModel GetVendorDetailnByUID(int vendorDetailUID)
        {
            var vendor = db.VendorDetail.Find(vendorDetailUID);
            VendorDetailModel data = null;
            if (vendor != null)
            {
                data = new VendorDetailModel();
                data.VendorDetailUID = vendor.UID;
                data.ContactPerson = vendor.ContactPerson;
                data.CompanyName = vendor.CompanyName;
                data.MobileNo = vendor.MobileNo;
                data.FaxNo = vendor.FaxNo;
                data.Email = vendor.Email;
                data.Address = vendor.Address;
                data.MNFTPUID = vendor.MNFTPUID;
                data.TINNo = vendor.TINNo;
                data.ProvinceUID = vendor.ProvinceUID;
                data.DistrictUID = vendor.DistrictUID;
                data.AmphurUID = vendor.AmphurUID;
                data.ZipCode = vendor.ZipCode;
                data.ActiveFrom = vendor.ActiveFrom;
                data.ActiveTo = vendor.ActiveTo;
                data.CUser = vendor.CUser;
                data.CWhen = vendor.CWhen;
                data.MUser = vendor.MUser;
                data.MWhen = vendor.MWhen;
                data.StatusFlag = vendor.StatusFlag;
            }


            return data;
        }


        [Route("ManageVendorDetail")]
        [HttpPost]
        public HttpResponseMessage ManageVendorDetail(VendorDetailModel vendorDetailModel, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;

                MediTech.DataBase.VendorDetail vendorDetail = db.VendorDetail.Find(vendorDetailModel.VendorDetailUID);

                if (vendorDetail == null)
                {
                    vendorDetail = new MediTech.DataBase.VendorDetail();
                    vendorDetail.CUser = userID;
                    vendorDetail.CWhen = now;
                }

                vendorDetail.ContactPerson = vendorDetailModel.ContactPerson;
                vendorDetail.CompanyName = vendorDetailModel.CompanyName;
                vendorDetail.MobileNo = vendorDetailModel.MobileNo;
                vendorDetail.FaxNo = vendorDetailModel.FaxNo;
                vendorDetail.Email = vendorDetailModel.Email;
                vendorDetail.Address = vendorDetailModel.Address;
                vendorDetail.MNFTPUID = vendorDetailModel.MNFTPUID;
                vendorDetail.ProvinceUID = vendorDetailModel.ProvinceUID;
                vendorDetail.TINNo = vendorDetailModel.TINNo;
                vendorDetail.DistrictUID = vendorDetailModel.DistrictUID;
                vendorDetail.AmphurUID = vendorDetailModel.AmphurUID;
                vendorDetail.ZipCode = vendorDetailModel.ZipCode;
                vendorDetail.ActiveFrom = vendorDetailModel.ActiveFrom;
                vendorDetail.ActiveTo = vendorDetailModel.ActiveTo;
                vendorDetail.MUser = userID;
                vendorDetail.MWhen = now;
                vendorDetail.StatusFlag = "A";


                db.VendorDetail.AddOrUpdate(vendorDetail);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("DeleteVendorDetail")]
        [HttpDelete]
        public HttpResponseMessage DeleteVendorDetail(int vendorDetailUID, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                MediTech.DataBase.VendorDetail vendorDetail = db.VendorDetail.Find(vendorDetailUID);
                if (vendorDetail != null)
                {
                    db.VendorDetail.Attach(vendorDetail);
                    vendorDetail.MUser = userID;
                    vendorDetail.MWhen = now;
                    vendorDetail.StatusFlag = "D";
                    db.SaveChanges();
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        #endregion
    }
}
