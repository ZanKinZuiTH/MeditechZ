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
using ShareLibrary;
using System.Data.Entity;
using MediTech.Model.Report;

namespace MediTechWebApi.Controllers
{
    [RoutePrefix("Api/Billing")]
    public class BillingController : ApiController
    {
        protected MediTechEntities db = new MediTechEntities();

        public BillableItemDetailModel GetBillableItemPrice(List<BillableItemDetailModel> billItmDetail, int ownerOrganisationUID)
        {
            BillableItemDetailModel selectBillItemDetail = null;

            if (billItmDetail.Count(p => p.OwnerOrganisationUID == ownerOrganisationUID) > 0)
            {
                selectBillItemDetail = billItmDetail
                    .FirstOrDefault(p => p.StatusFlag == "A" && p.OwnerOrganisationUID == ownerOrganisationUID
                    && (p.ActiveFrom == null || (DbFunctions.TruncateTime(p.ActiveFrom) >= DbFunctions.TruncateTime(DateTime.Now)))
                    && (p.ActiveTo == null || (p.ActiveTo.HasValue && DbFunctions.TruncateTime(p.ActiveTo) <= DbFunctions.TruncateTime(DateTime.Now)))
                    );
            }

            return selectBillItemDetail;
        }

        //[Route("SaveOrderForRIS")]
        //[HttpPost]
        //public HttpResponseMessage SaveOrderForRIS(AddRequestForRISModel patientRequest, int userID, int ownerUID)
        //{
        //    try
        //    {
        //        using (var tran = new TransactionScope())
        //        {
        //            DateTime now = DateTime.Now;

        //            int outseqvisitUID;
        //            string seqVisitID = SEQHelper.GetSEQIDFormat("SEQVisitID", out outseqvisitUID);

        //            if (string.IsNullOrEmpty(seqVisitID))
        //            {
        //                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No SEQVisitID in SEQCONFIGURATION");
        //            }

        //            if (outseqvisitUID == 0)
        //            {
        //                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Insert SEQVisitID is Fail");
        //            }


        //            int registUID = db.ReferenceValue.FirstOrDefault(p => p.StatusFlag == "A" && p.DomainCode == "ORDST" && p.ValueCode == "REGIST").UID;
        //            int ExcuteUID = db.ReferenceValue.FirstOrDefault(p => p.StatusFlag == "A" && p.DomainCode == "ORDST" && p.ValueCode == "EXCUT").UID;

        //            #region Patient

        //            Patient patient = db.Patient.Find(patientRequest.PatientUID);
        //            if (patient != null)
        //            {
        //                db.Patient.Attach(patient);
        //                patient.LastVisitDttm = DateTime.Now.Date;
        //                patient.MUser = userID;
        //                patient.MWhen = DateTime.Now;
        //                db.SaveChanges();
        //            }
        //            #endregion

        //            #region PatientVisit

        //            PatientVisit patientvisit = new PatientVisit();

        //            patientvisit.PatientUID = patientRequest.PatientUID;
        //            patientvisit.VisitID = seqVisitID;
        //            patientvisit.VISTSUID = db.ReferenceValue.FirstOrDefault(p => p.StatusFlag == "A" && p.DomainCode == "VISTS" && p.ValueCode == "REGST").UID;
        //            patientvisit.VISTYUID = db.ReferenceValue.FirstOrDefault(p => p.StatusFlag == "A" && p.DomainCode == "VISTY" && p.ValueCode == "MBXRY").UID;
        //            patientvisit.PRITYUID = patientvisit.PRITYUID = patientRequest.PriorityUID;
        //            patientvisit.StartDttm = now;
        //            patientvisit.ArrivedDttm = now;

        //            patientvisit.Comments = "From RIS";
        //            patientvisit.OwnerOrganisationUID = ownerUID;
        //            patientvisit.CUser = userID;
        //            patientvisit.CWhen = now;
        //            patientvisit.MUser = userID;
        //            patientvisit.MWhen = now;
        //            patientvisit.StatusFlag = "A";

        //            db.PatientVisit.Add(patientvisit);
        //            db.SaveChanges();

        //            #endregion

        //            #region VisitPayor

        //            PatientVisitPayor patientVisitPayor = new PatientVisitPayor();
        //            patientVisitPayor.PatientUID = patientRequest.PatientUID;
        //            patientVisitPayor.PatientVisitUID = patientvisit.UID;
        //            patientVisitPayor.PayorDetailUID = patientRequest.PayorDetailUID;
        //            patientVisitPayor.PayorAgreementUID = patientRequest.PayorAgreementUID;
        //            patientVisitPayor.CUser = userID;
        //            patientVisitPayor.CWhen = now;
        //            patientVisitPayor.MUser = userID;
        //            patientVisitPayor.MWhen = now;
        //            patientVisitPayor.StatusFlag = "A";

        //            db.PatientVisitPayor.Add(patientVisitPayor);

        //            db.SaveChanges();

        //            #endregion

        //            #region PatientOrder

        //            PatientOrder patientOrder = new PatientOrder();

        //            int outpatientOrderUID;
        //            string seqOrderID = SEQHelper.GetSEQIDFormat("SEQPatientOrder", out outpatientOrderUID);

        //            if (string.IsNullOrEmpty(seqOrderID))
        //            {
        //                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No SEQPatientOrder in SEQCONFIGURATION");
        //            }

        //            if (outpatientOrderUID == 0)
        //            {
        //                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Insert SEQPatientOrder is Fail");
        //            }

        //            patientOrder.OrderNumber = seqOrderID;

        //            patientOrder.PatientUID = patientRequest.PatientUID;
        //            patientOrder.PatientVisitUID = patientvisit.UID;
        //            patientOrder.StartDttm = now;
        //            patientOrder.IdentifyingType = "REQUEST";
        //            patientOrder.OrderRaisedBy = userID;
        //            patientOrder.CUser = userID;
        //            patientOrder.CWhen = now;
        //            patientOrder.MUser = userID;
        //            patientOrder.MWhen = now;
        //            patientOrder.OwnerOrganisationUID = ownerUID;
        //            patientOrder.StatusFlag = "A";
        //            db.PatientOrder.Add(patientOrder);
        //            db.SaveChanges();


        //            Request request = new Request();

        //            int outrequestUID;
        //            string seqRequestID = SEQHelper.GetSEQIDFormat("SEQRequest", out outrequestUID);

        //            if (string.IsNullOrEmpty(seqRequestID))
        //            {
        //                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No SEQRequest in SEQCONFIGURATION");
        //            }

        //            if (outrequestUID == 0)
        //            {
        //                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Insert SEQRequest is Fail");
        //            }

        //            request.PatientUID = patientRequest.PatientUID;
        //            request.PatientVisitUID = patientvisit.UID;
        //            request.RequestedUserUID = userID;
        //            request.RequestedDttm = now;
        //            request.PatientOrderUID = patientOrder.UID;
        //            request.RequestNumber = seqRequestID;
        //            request.RQPRTUID = patientRequest.PriorityUID;
        //            request.BSMDDUID = db.ReferenceValue.FirstOrDefault(p => p.StatusFlag == "A" && p.DomainCode == "BSMDD" && p.ValueCode == "RADIO").UID;
        //            request.CUser = userID;
        //            request.CWhen = now;
        //            request.MUser = userID;
        //            request.MWhen = now;
        //            request.OwnerOrganisationUID = ownerUID;
        //            request.StatusFlag = "A";

        //            db.Request.Add(request);
        //            db.SaveChanges();


        //            db.PatientOrder.Attach(patientOrder);
        //            patientOrder.IdentifyingUID = request.UID;
        //            db.SaveChanges();


        //            foreach (var item in patientRequest.RequestItems)
        //            {

        //                BillableItemModel billableItem = (new MasterDataController()).GetBillableItemByUID(item.BillableItemUID);

        //                var selectBillableItemPrice = GetBillableItemPrice(billableItem.BillableItemDetails, ownerUID);

        //                PatientOrderDetail patientOrderDetail = new PatientOrderDetail();
        //                patientOrderDetail.PatientOrderUID = patientOrder.UID;
        //                patientOrderDetail.ItemCode = billableItem.Code;
        //                patientOrderDetail.ItemName = billableItem.ItemName;
        //                patientOrderDetail.StartDttm = now;
        //                patientOrderDetail.ORDSTUID = patientRequest.RadiologistUID != null ? ExcuteUID : registUID;
        //                patientOrderDetail.Quantity = 1;
        //                patientOrderDetail.UnitPrice = selectBillableItemPrice.Price;
        //                patientOrderDetail.DoctorFee = billableItem.DoctorFee;
        //                patientOrderDetail.NetAmount = patientOrderDetail.UnitPrice + (billableItem.DoctorFee ?? 0);
        //                patientOrderDetail.BillableItemUID = billableItem.BillableItemUID;
        //                patientOrderDetail.IdentifyingType = "REQUESTDETAIL";
        //                patientOrderDetail.CUser = userID;
        //                patientOrderDetail.CWhen = now;
        //                patientOrderDetail.MUser = userID;
        //                patientOrderDetail.MWhen = now;
        //                patientOrderDetail.StatusFlag = "A";
        //                patientOrderDetail.OwnerOrganisationUID = ownerUID;

        //                db.PatientOrderDetail.Add(patientOrderDetail);
        //                db.SaveChanges();


        //                RequestDetail requestDetail = new RequestDetail();
        //                requestDetail.RequestUID = request.UID;


        //                MediTech.DataBase.RequestItem requestItem = db.RequestItem.Find(billableItem.ItemUID);

        //                requestDetail.RequestitemUID = requestItem.UID;
        //                requestDetail.RequestedDttm = now;
        //                requestDetail.ResultRequiredDttm = now;
        //                requestDetail.PatientOrderDetailUID = patientOrderDetail.UID;
        //                requestDetail.AccessionNumber = item.AccessionNumber;
        //                requestDetail.RQPRTUID = patientRequest.PriorityUID;
        //                requestDetail.ORDSTUID = patientOrderDetail.ORDSTUID;
        //                requestDetail.Comments = patientRequest.Comments;
        //                requestDetail.RequestedUserUID = userID;
        //                requestDetail.RequestItemCode = requestItem.Code;
        //                requestDetail.RequestItemName = requestItem.ItemName;
        //                requestDetail.RIMTYPUID = requestItem.RIMTYPUID;
        //                requestDetail.RadiologistUID = patientRequest.RadiologistUID;
        //                if (patientRequest.RadiologistUID != null)
        //                {
        //                    requestDetail.PreparedByUID = userID;
        //                    requestDetail.PreparedDttm = now;
        //                }
        //                requestDetail.CUser = userID;
        //                requestDetail.CWhen = now;
        //                requestDetail.MUser = userID;
        //                requestDetail.MWhen = now;
        //                requestDetail.StatusFlag = "A";
        //                requestDetail.OwnerOrganisationUID = ownerUID;

        //                db.RequestDetail.Add(requestDetail);
        //                db.SaveChanges();

        //                db.PatientOrderDetail.Attach(patientOrderDetail);
        //                patientOrderDetail.IdentifyingUID = requestDetail.UID;
        //                db.SaveChanges();

        //                #region SavePatinetOrderDetailHistory

        //                PatientOrderDetailHistory patientOrderDetailHistory = new PatientOrderDetailHistory();
        //                patientOrderDetailHistory.PatientOrderDetailUID = patientOrderDetail.UID;
        //                patientOrderDetailHistory.ORDSTUID = patientOrderDetail.ORDSTUID;
        //                patientOrderDetailHistory.Comments = "From Web RIS";
        //                patientOrderDetailHistory.EditedDttm = now;
        //                patientOrderDetailHistory.EditByUserID = userID;
        //                patientOrderDetailHistory.CUser = userID;
        //                patientOrderDetailHistory.CWhen = now;
        //                patientOrderDetailHistory.MUser = userID;
        //                patientOrderDetailHistory.MWhen = now;
        //                patientOrderDetailHistory.StatusFlag = "A";
        //                db.PatientOrderDetailHistory.Add(patientOrderDetailHistory);
        //                db.SaveChanges();

        //                #endregion

        //                #endregion

        //            }


        //            tran.Complete();
        //            return Request.CreateResponse(HttpStatusCode.OK);
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
        //    }
        //}


        [Route("GetPatientVisitForRISBilling")]
        [HttpGet]
        public List<PatientVisitModel> GetPatientVisitForRISBilling(DateTime? dateFrom, DateTime? dateTo, string firstName, string lastName, string patientID)
        {
            DataTable dataTable = SqlDirectStore.pSearchPatientVisitForRISBilling(dateFrom, dateTo, firstName, lastName, patientID);
            List<PatientVisitModel> listData = dataTable.ToList<PatientVisitModel>();

            return listData;
        }

        [Route("GeneratePatientBill")]
        [HttpPost]
        public HttpResponseMessage GeneratePatientBill(PatientBillModel model)
        {
            try
            {
                using (var tran = new TransactionScope())
                {

                    DateTime now = DateTime.Now;
                    var refValue = db.ReferenceValue.Where(p => (p.DomainCode == "BSMDD" || p.DomainCode == "BLTYP" || p.DomainCode == "PBTYP") && p.StatusFlag == "A");

                    int BSMDD_LAB = refValue.FirstOrDefault(p => p.ValueCode == "LABBB" && p.DomainCode == "BSMDD").UID;
                    int BSMDD_RADIO = refValue.FirstOrDefault(p => p.ValueCode == "RADIO" && p.DomainCode == "BSMDD").UID;
                    int BSMDD_STORE = refValue.FirstOrDefault(p => p.ValueCode == "STORE" && p.DomainCode == "BSMDD").UID;
                    //int BSMDD_ORDITEM = 2839;
                    int BSMDD_MDSLP = refValue.FirstOrDefault(p => p.ValueCode == "MDSLP" && p.DomainCode == "BSMDD").UID;
                    int BSMDD_SULPY = refValue.FirstOrDefault(p => p.ValueCode == "SUPLY" && p.DomainCode == "BSMDD").UID;

                    int BLTYP_Cash = refValue.FirstOrDefault(p => p.ValueCode == "CASHBL" && p.DomainCode == "BLTYP").UID;
                    int BLTYP_Credit = refValue.FirstOrDefault(p => p.ValueCode == "CREDBL" && p.DomainCode == "BLTYP").UID;

                    int BLTYP_Receive = refValue.FirstOrDefault(p => p.ValueCode == "RECEI" && p.DomainCode == "PBTYP").UID;
                    int BLTYP_Invoice = refValue.FirstOrDefault(p => p.ValueCode == "INVOC" && p.DomainCode == "PBTYP").UID;


                    PatientVisit patpv = db.PatientVisit.Find(model.PatientVisitUID);
                    var refVisitType = db.ReferenceValue.Where(p => p.StatusFlag == "A" && p.DomainCode == "VISTY" && (p.ValueCode == "NONMED" || p.ValueCode == "BUSNT"));
                    ReferenceValue nonMed = refVisitType.FirstOrDefault(p => p.ValueCode == "NONMED");
                    ReferenceValue businessUnits = refVisitType.FirstOrDefault(p => p.ValueCode == "BUSNT");
                    PatientBill patBill = new PatientBill();
                    int seqBillID = 0;
                    string patientBillID = string.Empty;

                    var visitPayor = db.PatientVisitPayor.FirstOrDefault(p => p.PatientVisitUID == patpv.UID && p.StatusFlag == "A");
                    PayorDetail payorDetail = db.PayorDetail.Find(visitPayor.PayorDetailUID);
                    IEnumerable<HealthOrganisationID> healthOrganisationIDs;
                    if (patpv.VISTYUID == nonMed?.UID)
                    {
                        healthOrganisationIDs = db.HealthOrganisationID.Where(p => p.HealthOrganisationUID == 2 && p.StatusFlag == "A"); //Nonmed
                    }
                    else
                    {
                        healthOrganisationIDs = db.HealthOrganisationID.Where(p => p.HealthOrganisationUID == model.OwnerOrganisationUID && p.StatusFlag == "A");
                    }

                    var agreement = db.PayorAgreement.FirstOrDefault(p => p.UID == visitPayor.PayorAgreementUID);
                    string billType = "";


                    HealthOrganisationID healthIDBillType = null;
                    if (agreement.PBTYPUID == BLTYP_Receive)
                    {
                        if (healthOrganisationIDs != null && healthOrganisationIDs.FirstOrDefault(p => p.BLTYPUID == BLTYP_Cash) != null)
                        {
                            billType = "Cash";
                            healthIDBillType = healthOrganisationIDs.FirstOrDefault(p => p.BLTYPUID == BLTYP_Cash);
                            if (healthIDBillType == null)
                            {
                                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No HealthOranisationID Type " + billType + " in HealthOranisation");
                            }
                            db.HealthOrganisationID.Attach(healthIDBillType);
                            if (healthIDBillType.LastRenumberDttm == null)
                            {
                                healthIDBillType.LastRenumberDttm = now;
                            }
                            else
                            {
                                double dateDiff = ((now.Year - healthIDBillType.LastRenumberDttm.Value.Year) * 12) + now.Month - healthIDBillType.LastRenumberDttm.Value.Month;
                                if (dateDiff >= 1)
                                {
                                    healthIDBillType.LastRenumberDttm = now;
                                    healthIDBillType.NumberValue = 1;
                                }
                            }

                            patientBillID = SEQHelper.GetSEQBillNumber(healthIDBillType.IDFormat, healthIDBillType.IDLength.Value, healthIDBillType.NumberValue.Value);
                            seqBillID = healthIDBillType.NumberValue.Value;

                            healthIDBillType.NumberValue = ++healthIDBillType.NumberValue;

                            db.SaveChanges();
                        }
                        else
                        {
                            patientBillID = SEQHelper.GetSEQIDFormat("SEQPatientBill", out seqBillID);
                            if (string.IsNullOrEmpty(patientBillID))
                            {
                                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No SEQPatientBill Or SEQPatientINVBill in SEQCONFIGURATION");
                            }
                        }


                    }
                    else if (agreement.PBTYPUID == BLTYP_Invoice)
                    {
                        if (healthOrganisationIDs != null && healthOrganisationIDs.FirstOrDefault(p => p.BLTYPUID == BLTYP_Credit) != null)
                        {
                            billType = "Credit";
                            healthIDBillType = healthOrganisationIDs.FirstOrDefault(p => p.BLTYPUID == BLTYP_Credit);
                            if (healthIDBillType == null)
                            {
                                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No HealthOranisationID Type " + billType + " in HealthOranisation");
                            }
                            db.HealthOrganisationID.Attach(healthIDBillType);
                            if (healthIDBillType.LastRenumberDttm == null)
                            {
                                healthIDBillType.LastRenumberDttm = now;
                            }
                            else
                            {
                                double dateDiff = ((now.Year - healthIDBillType.LastRenumberDttm.Value.Year) * 12) + now.Month - healthIDBillType.LastRenumberDttm.Value.Month;
                                if (dateDiff >= 1)
                                {
                                    healthIDBillType.LastRenumberDttm = now;
                                    healthIDBillType.NumberValue = 1;
                                }
                            }

                            patientBillID = SEQHelper.GetSEQBillNumber(healthIDBillType.IDFormat, healthIDBillType.IDLength.Value, healthIDBillType.NumberValue.Value);
                            seqBillID = healthIDBillType.NumberValue.Value;

                            healthIDBillType.NumberValue = ++healthIDBillType.NumberValue;

                            db.SaveChanges();
                        }
                        else
                        {
                            patientBillID = SEQHelper.GetSEQIDFormat("SEQPatientINVBill", out seqBillID);
                            if (string.IsNullOrEmpty(patientBillID))
                            {
                                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No SEQPatientBill Or SEQPatientINVBill in SEQCONFIGURATION");
                            }
                        }

                    }




                    if (seqBillID == 0)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Insert SEQPatientBill Or SEQPatientINVBill is Fail");
                    }
                    patBill.CUser = model.CUser;
                    patBill.CWhen = now;
                    patBill.BillNumber = patientBillID;
                    //patBill.BillGeneratedDttm = patpv.StartDttm;
                    patBill.BillGeneratedDttm = now;

                    patBill.MUser = model.MUser;
                    patBill.MWhen = DateTime.Now;
                    patBill.StatusFlag = "A";
                    patBill.OwnerOrganisationUID = model.OwnerOrganisationUID;
                    patBill.PatientUID = model.PatientUID;
                    patBill.PatientVisitUID = model.PatientVisitUID;
                    patBill.Comments = model.Comments;
                    patBill.PaidAmount = model.PaidAmount;
                    patBill.TotalAmount = model.TotalAmount;
                    patBill.DiscountAmount = model.DiscountAmount;
                    patBill.NetAmount = model.NetAmount;
                    patBill.ChangeAmount = model.ChangeAmount;

                    PatientVisitPayor patvtpay = db.PatientVisitPayor.FirstOrDefault(p => p.PatientVisitUID == model.PatientVisitUID);
                    if (patvtpay != null)
                    {
                        patBill.PatientVisitPayorUID = patvtpay.UID;
                        patBill.PayorDetailUID = patvtpay.PayorDetailUID;
                        patBill.PayorAgreementUID = patvtpay.PayorAgreementUID;
                    }

                    db.PatientBill.AddOrUpdate(patBill);
                    db.SaveChanges();


                    //if (model.PatientBillUID != 0)
                    //{
                    //    IEnumerable<PatientBilledItem> oldPatBilled = db.PatientBilledItem.Where(p => p.PatientBillUID == model.PatientBillUID);
                    //    foreach (var item in oldPatBilled)
                    //    {
                    //        var data = model.PatientBilledItems.FirstOrDefault(p => p.PatientBilledItemUID == item.UID);
                    //        if (data == null)
                    //        {
                    //            db.PatientBilledItem.Attach(item);
                    //            item.MUser = model.MUser;
                    //            item.MWhen = now;
                    //            item.StatusFlag = "D";

                    //            db.SaveChanges();

                    //        }
                    //    }

                    //}

                    List<long> prescriptonUIDs = new List<long>();
                    foreach (var item in model.PatientBilledItems)
                    {
                        BillableItem billItem = db.BillableItem.Find(item.BillableItemUID);
                        PatientOrderDetail patientOrderDetail = db.PatientOrderDetail.Find(item.PatientOrderDetailUID);
                        if (billItem.BSMDDUID == BSMDD_MDSLP || billItem.BSMDDUID == BSMDD_STORE || billItem.BSMDDUID == BSMDD_SULPY)
                        {
                            if (item.IdentifyingUID != null)
                            {
                                DataTable dtStockUsed = SqlDirectStore.pDispensePrescriptionItem(item.IdentifyingUID.Value, model.CUser);
                                if (dtStockUsed != null && dtStockUsed.Rows.Count > 0)
                                {
                                    double doctorFee = 0;
                                    double discount = 0;
                                    //double amount = 0;
                                    if ((billItem.DoctorFee ?? 0) != 0)
                                    {
                                        doctorFee = billItem.DoctorFee.Value / dtStockUsed.Rows.Count;
                                    }

                                    if ((item.Discount ?? 0) != 0)
                                    {
                                        discount = item.Discount.Value / dtStockUsed.Rows.Count;
                                    }

                                    double unitPrice = patientOrderDetail.IsPriceOverwrite == "Y" ? patientOrderDetail.OverwritePrice.Value : patientOrderDetail.UnitPrice ?? 0;
                                    //amount = item.Amount.Value / dtStockUsed.Rows.Count;
                                    foreach (DataRow stockUsed in dtStockUsed.Rows)
                                    {
                                        PatientBilledItem patBilled = new PatientBilledItem();
                                        patBilled.CWhen = now;
                                        patBilled.StatusFlag = "A";
                                        patBilled.CUser = model.CUser;
                                        patBilled.BSMDDUID = billItem.BSMDDUID;
                                        patBilled.BillingGroupUID = billItem.BillingGroupUID;
                                        patBilled.BillingSubGroupUID = billItem.BillingSubGroupUID;

                                        patBilled.BillableItemUID = item.BillableItemUID;
                                        patBilled.PatientOrderDetailUID = item.PatientOrderDetailUID;
                                        patBilled.PatientBillUID = patBill.UID;
                                        patBilled.MUser = model.MUser;
                                        patBilled.MWhen = now;
                                        patBilled.OwnerOrganisationUID = model.OwnerOrganisationUID;
                                        patBilled.ItemName = item.ItemName;

                                        patBilled.StoreUID = Convert.ToInt32(stockUsed["StoreUID"]);
                                        patBilled.StockUID = Convert.ToInt32(stockUsed["StockUID"]);
                                        patBilled.BatchID = stockUsed["BatchID"].ToString();
                                        patBilled.ItemCost = Convert.ToDouble(stockUsed["UnitCost"]) + doctorFee;
                                        patBilled.ItemMutiplier = Convert.ToDouble(stockUsed["Qty"]);
                                        patBilled.Amount = unitPrice * patBilled.ItemMutiplier;
                                        patBilled.Discount = discount;
                                        patBilled.NetAmount = patBilled.Amount - patBilled.Discount;


                                        db.PatientBilledItem.AddOrUpdate(patBilled);
                                    }
                                    //if (dtStockUsed.Rows.Count > 1)
                                    //{
                                    //    double sumCost = dtStockUsed.AsEnumerable().Sum(x => x.Field<double>("ItemCost"));
                                    //    double sumQty = dtStockUsed.AsEnumerable().Sum(x => x.Field<double>("Qty"));
                                    //    patBilled.ItemCost = sumCost / sumQty;
                                    //}
                                    //else
                                    //{
                                    //    patBilled.ItemCost = Convert.ToDouble(dtStockUsed.Rows[0]["UnitCost"]);
                                    //}
                                    //patBilled.ItemCost = Convert.ToDouble(dtStockUsed.Rows[0]["UnitCost"]) + (billItem.DoctorFee ?? 0);

                                    long prescriptionUID = long.Parse(dtStockUsed.Rows[0]["PrescriptionUID"].ToString());
                                    if (!prescriptonUIDs.Any(p => p == prescriptionUID))
                                    {
                                        prescriptonUIDs.Add(prescriptionUID);
                                    }

                                }

                                //if (patpv.VISTYUID == careAtHome.UID)
                                //{
                                //    if (careAtHome.NumericValue != null)
                                //    {
                                //        patBilled.ExtendCost = Math.Round(patBilled.ItemCost ?? 0 * (careAtHome.NumericValue.Value / 100), 2);
                                //    }

                                //}

                            }

                        }
                        else
                        {
                            BillableItemDetail billItemDetail = db.BillableItemDetail.FirstOrDefault(p => p.StatusFlag == "A"
                            && p.OwnerOrganisationUID == model.OwnerOrganisationUID
                            && p.BillableItemUID == item.BillableItemUID
                            && (p.ActiveFrom == null || (DbFunctions.TruncateTime(p.ActiveFrom) <= DbFunctions.TruncateTime(DateTime.Now)))
                            && (p.ActiveTo == null || (p.ActiveTo.HasValue && DbFunctions.TruncateTime(p.ActiveTo) >= DbFunctions.TruncateTime(DateTime.Now))));

                            PatientBilledItem patBilled = new PatientBilledItem();
                            patBilled.CWhen = now;
                            patBilled.StatusFlag = "A";
                            patBilled.CUser = model.CUser;

                            patBilled.BSMDDUID = billItem.BSMDDUID;
                            patBilled.BillingGroupUID = billItem.BillingGroupUID;
                            patBilled.BillingSubGroupUID = billItem.BillingSubGroupUID;

                            patBilled.BillableItemUID = item.BillableItemUID;
                            patBilled.PatientOrderDetailUID = item.PatientOrderDetailUID;
                            patBilled.PatientBillUID = patBill.UID;
                            patBilled.MUser = model.MUser;
                            patBilled.MWhen = now;
                            patBilled.OwnerOrganisationUID = model.OwnerOrganisationUID;
                            patBilled.ItemName = item.ItemName;
                            patBilled.ItemCost = billItemDetail?.Cost;
                            patBilled.ItemMutiplier = item.ItemMutiplier;
                            patBilled.Amount = item.Amount;
                            patBilled.Discount = item.Discount;
                            patBilled.NetAmount = item.NetAmount;
                            patBilled.DoctorFee = item.DoctorFee;
                            patBilled.CareproviderUID = item.CareproviderUID ?? 0;
                            //if (billItem.DoctorFee != null && billItem.DoctorFee > 0)
                            //{
                            //    patBilled.DoctorFee = (billItem.DoctorFee / 100) * item.NetAmount;
                            //}


                            db.PatientBilledItem.AddOrUpdate(patBilled);
                        }
                        //if (item.StockUID != null && item.StockUID != 0)
                        //{
                        //    Stock stock = db.Stock.Find(item.StockUID);
                        //    patBilled.ItemCost = stock.ItemCost + (billItem.DoctorFee ?? 0);
                        //}
                        //else
                        //{
                        //    patBilled.ItemCost = billItem.TotalCost;
                        //}


                        db.SaveChanges();

                        if (prescriptonUIDs != null && prescriptonUIDs.Count > 0)
                        {
                            foreach (var prescriptionUID in prescriptonUIDs)
                            {
                                var prescription = db.Prescription.Find(prescriptionUID);

                                db.Prescription.Attach(prescription);
                                prescription.MUser = model.MUser;
                                prescription.MWhen = now;
                                prescription.DispensedDttm = now;
                                prescription.ORDSTUID = (new InventoryController()).CheckPrescriptionStatus(prescription.UID);

                                db.SaveChanges();
                            }

                        }
                    }


                    if (patpv != null)
                    {
                        db.PatientVisit.Attach(patpv);
                        patpv.MUser = model.MUser;
                        patpv.MWhen = now;
                        patpv.IsBillFinalized = "Y";
                        patpv.VISTSUID = 421;
                        patpv.EndDttm = now;
                        db.SaveChanges();


                        #region PatientServiceEvent
                        PatientServiceEvent serviceEvent = new PatientServiceEvent();
                        serviceEvent.PatientVisitUID = patpv.UID;
                        serviceEvent.EventStartDttm = now;
                        serviceEvent.VISTSUID = patpv.VISTSUID.Value;
                        serviceEvent.MUser = model.MUser;
                        serviceEvent.MWhen = now;
                        serviceEvent.CUser = model.MUser;
                        serviceEvent.CWhen = now;
                        serviceEvent.StatusFlag = "A";

                        db.PatientServiceEvent.Add(serviceEvent);
                        #endregion

                        db.SaveChanges();
                    }

                    tran.Complete();

                    if (model.PatientBillUID == 0)
                    {
                        model.PatientBillUID = patBill.UID;
                        model.BillGeneratedDttm = patBill.BillGeneratedDttm;
                        model.BillNumber = patBill.BillNumber;
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, model);
                }

            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("GetPatientBillingGroup")]
        [HttpGet]
        public List<PatientBilledItemModel> GetPatientBillingGroup(long PatientBillUID)
        {
            List<PatientBilledItemModel> data = new List<PatientBilledItemModel>();
            data = db.PatientBilledItem
                .Where(w => w.PatientBillUID == PatientBillUID && w.StatusFlag == "A")
                .GroupBy(p => new { p.BillingGroupUID, p.BillingSubGroupUID })
                .Select(f => new PatientBilledItemModel
                {
                    Amount = f.Sum(x => x.Amount),
                    NetAmount = f.Sum(x => x.NetAmount),
                    Discount = f.Sum(x => x.Discount),
                    BillingGroupUID = f.Key.BillingGroupUID ?? 0,
                    BillingSubGroupUID = f.Key.BillingSubGroupUID ?? 0,
                    BillingGroup = SqlFunction.fGetBillingGroupDesc(f.Key.BillingGroupUID ?? 0, "G"),
                    BillinsgSubGroup = SqlFunction.fGetBillingGroupDesc(f.Key.BillingSubGroupUID ?? 0, "S")
                }).ToList();
            return data;
        }

        [Route("GetGroupReceiptDetail")]
        [HttpGet]
        public List<GroupReceiptDetailModel> GetGroupReceiptDetail(int groupReceiptUID)
        {

            List<GroupReceiptDetailModel> data = (from p in db.GroupReceiptDetail
                                                  where p.GroupReceiptUID == groupReceiptUID
                                                  && p.StatusFlag == "A"
                                                  select new GroupReceiptDetailModel
                                                  {
                                                      GroupReceiptUID = p.GroupReceiptUID,
                                                      GroupReceiptDetailUID = p.UID,
                                                      ItemName = p.ItemName,
                                                      BillableItemUID = p.BillableItemUID,
                                                      OrderSetUID = p.OrderSetUID,
                                                      Quantity = p.Quantity,
                                                      UnitItem = p.Unit,
                                                      PriceUnit = p.Price,
                                                      TotalPrice = p.TotalPrice,
                                                      Discount = p.Discount
                                                  }).ToList();

            return data;
        }

        [Route("GetPatientBilledItem")]
        [HttpGet]
        public List<PatientBilledItemModel> GetPatientBilledItem(long PatientBillUID)
        {
            List<PatientBilledItemModel> data = new List<PatientBilledItemModel>(); ;
            data = db.PatientBilledItem
                .Where(w => w.PatientBillUID == PatientBillUID && w.StatusFlag == "A")
                .Select(f => new PatientBilledItemModel
                {
                    Amount = f.Amount,
                    NetAmount = f.NetAmount,
                    Discount = f.Discount,
                    ItemName = f.ItemName,
                    ItemMutiplier = f.ItemMutiplier,
                    BillingGroupUID = f.BillingGroupUID ?? 0,
                    BillingSubGroupUID = f.BillingSubGroupUID ?? 0,
                    BillingGroup = SqlFunction.fGetBillingGroupDesc(f.BillingGroupUID ?? 0, "G"),
                    BillinsgSubGroup = SqlFunction.fGetBillingGroupDesc(f.BillingSubGroupUID ?? 0, "S")
                }).ToList();
            return data;
        }

        [Route("GetPatientBilledOrderDetail")]
        [HttpGet]
        public List<PatientBilledItemModel> GetPatientBilledOrderDetail(long patientBillUID)
        {
            List<PatientBilledItemModel> data = new List<PatientBilledItemModel>(); ;
            data = (from billed in db.PatientBilledItem
                    join pod in db.PatientOrderDetail on billed.PatientOrderDetailUID equals pod.UID
                    where billed.StatusFlag == "A"
                    && billed.PatientBillUID == patientBillUID
                    select new PatientBilledItemModel
                    {
                        PatientBillUID = billed.PatientBillUID,
                        Amount = billed.Amount,
                        NetAmount = billed.NetAmount,
                        Discount = billed.Discount,
                        ItemName = billed.ItemName,
                        ItemMutiplier = billed.ItemMutiplier,
                        Unit = SqlFunction.fGetRfValDescription(pod.QNUOMUID ?? 0),
                        BillingGroupUID = billed.BillingGroupUID ?? 0,
                        BillingSubGroupUID = billed.BillingSubGroupUID ?? 0,
                        BillingGroup = SqlFunction.fGetBillingGroupDesc(billed.BillingGroupUID ?? 0, "G"),
                        BillinsgSubGroup = SqlFunction.fGetBillingGroupDesc(billed.BillingSubGroupUID ?? 0, "S"),
                        PatientOrderDetailUID = pod.UID,
                        OrderSetUID = pod.OrderSetUID,
                        BillableItemUID = billed.BillableItemUID
                    }).ToList();
            return data;
        }

        [Route("SearchPatientBill")]
        [HttpGet]
        public List<PatientBillModel> SearchPatientBill(DateTime? dateFrom, DateTime? dateTo, long? patientUID, string billNumber, int? owerOrganisationUID)
        {
            List<PatientBillModel> data = new List<PatientBillModel>();
            DataTable dt = SqlDirectStore.pSearchPatientBill(dateFrom, dateTo, patientUID, billNumber, owerOrganisationUID);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<PatientBillModel>();
                data = dt.ToList<PatientBillModel>();
            }
            return data;
        }



        [Route("CancelBill")]
        [HttpPut]
        public HttpResponseMessage CancelBill(long patientBillUID, string cancelReason, int userUID)
        {

            try
            {
                using (var tran = new TransactionScope())
                {
                    PatientBill patientBill = db.PatientBill.Find(patientBillUID);
                    DateTime now = DateTime.Now;
                    if (patientBill != null)
                    {

                        db.PatientBill.Attach(patientBill);
                        patientBill.CancelReason = cancelReason;
                        patientBill.CancelledDttm = now;
                        patientBill.IsRefund = "Y";
                        patientBill.MUser = userUID;
                        patientBill.MWhen = now;
                        db.SaveChanges();
                        List<PatientBilledItem> patBilledItem = db.PatientBilledItem.Where(p => p.PatientBillUID == patientBill.UID && p.StatusFlag == "A").ToList();
                        if (patBilledItem != null)
                        {
                            foreach (var item in patBilledItem)
                            {

                                if (item.BSMDDUID == 2826 || item.BSMDDUID == 2953 || item.BSMDDUID == 3078)
                                {

                                    var stockmovement = (from pre in db.PrescriptionItem
                                                         join dis in db.DispensedItem on pre.UID equals dis.PrescriptionItemUID
                                                         join stv in db.StockMovement on new { key1 = dis.UID, key2 = "DispensedItem" } equals new { key1 = stv.RefUID ?? 0, key2 = stv.RefTable }
                                                         where pre.StatusFlag == "A"
                                                         && dis.StatusFlag == "A"
                                                         && stv.StatusFlag == "A"
                                                         && dis.ORDSTUID == 2861 // Dispensed
                                                         && pre.PatientOrderDetailUID == item.PatientOrderDetailUID
                                                         select new
                                                         {
                                                             StockUID = stv.StockUID,
                                                             OutQty = stv.OutQty,
                                                             RefUID = stv.RefUID,
                                                             StoreUID = stv.StoreUID,
                                                             StockmovementUID = stv.UID,
                                                             DispensedItemUID = dis.UID,
                                                             PresciptionItemUID = pre.UID,
                                                             PrescriptionUID = pre.PrescriptionUID
                                                         }).FirstOrDefault();


                                    Stock stock = db.Stock.Find(stockmovement.StockUID);

                                    double totalBFQty = SqlStatement.GetItemTotalQuantity(stock.ItemMasterUID, stock.StoreUID, stock.OwnerOrganisationUID);
                                    double bfQty = stock.Quantity;

                                    //SqlDirectStore.pCancelDispensed(item.PatientOrderDetailUID, userUID);

                                    DispensedItem dispensedItem = db.DispensedItem.Find(stockmovement.DispensedItemUID);
                                    db.DispensedItem.Attach(dispensedItem);
                                    dispensedItem.MUser = userUID;
                                    dispensedItem.MWhen = now;
                                    dispensedItem.ORDSTUID = 2875;


                                    PrescriptionItem prescriptionItem = db.PrescriptionItem.Find(stockmovement.PresciptionItemUID);
                                    db.PrescriptionItem.Attach(prescriptionItem);
                                    prescriptionItem.MUser = userUID;
                                    prescriptionItem.MWhen = now;
                                    prescriptionItem.ORDSTUID = 2875;
                                    db.SaveChanges();

                                    Prescription prescription = db.Prescription.Find(stockmovement.PrescriptionUID);
                                    db.Prescription.Attach(prescription);
                                    prescription.MUser = userUID;
                                    prescription.MWhen = now;
                                    prescription.ORDSTUID = (new InventoryController()).CheckPrescriptionStatus(prescription.UID);



                                    PatientOrderDetail patientOrderDetail = db.PatientOrderDetail.Find(item.PatientOrderDetailUID);
                                    db.PatientOrderDetail.Attach(patientOrderDetail);
                                    patientOrderDetail.MUser = userUID;
                                    patientOrderDetail.MWhen = now;
                                    patientOrderDetail.ORDSTUID = 2875;

                                    PatientOrderDetailHistory patOrderDetailHistory = new PatientOrderDetailHistory();
                                    patOrderDetailHistory.PatientOrderDetailUID = patientOrderDetail.UID;
                                    patOrderDetailHistory.ORDSTUID = patientOrderDetail.ORDSTUID;
                                    patOrderDetailHistory.EditByUserID = userUID;
                                    patOrderDetailHistory.EditedDttm = now;
                                    patOrderDetailHistory.CUser = userUID;
                                    patOrderDetailHistory.CWhen = now;
                                    patOrderDetailHistory.MUser = userUID;
                                    patOrderDetailHistory.MWhen = now;
                                    patOrderDetailHistory.StatusFlag = "A";
                                    db.PatientOrderDetailHistory.Add(patOrderDetailHistory);

                                    db.Stock.Attach(stock);
                                    stock.Quantity = stock.Quantity + stockmovement.OutQty;
                                    stock.MUser = userUID;
                                    stock.MWhen = now;
                                    stock.StatusFlag = "A";


                                    StockMovement stockMovement = db.StockMovement.Find(stockmovement.StockmovementUID);
                                    db.StockMovement.Attach(stockMovement);
                                    stockMovement.MUser = userUID;
                                    stockMovement.MWhen = now;
                                    stockMovement.Note = "SaleReturn";


                                    db.SaveChanges();



                                    SaleReturn saleReturn = new SaleReturn();


                                    db.SaleReturn.Add(saleReturn);
                                    saleReturn.ReturnedBy = userUID;
                                    saleReturn.ReturnDttm = now;
                                    saleReturn.PatientUID = patientBill.PatientUID;
                                    saleReturn.PatientVisitUID = patientBill.PatientVisitUID;
                                    saleReturn.StoreUID = stockmovement.StoreUID ?? 0;
                                    saleReturn.OwnerOrganisationUID = stock.OwnerOrganisationUID;
                                    saleReturn.MUser = userUID;
                                    saleReturn.MWhen = now;
                                    saleReturn.CUser = userUID;
                                    saleReturn.CWhen = now;
                                    saleReturn.StatusFlag = "A";
                                    db.SaveChanges();

                                    SaleReturnList saleReturnList = new SaleReturnList();
                                    saleReturnList.SaleReturnUID = saleReturn.UID;
                                    saleReturnList.StockUID = stock.UID;
                                    saleReturnList.BatchID = stock.BatchID;
                                    saleReturnList.ItemMasterUID = stock.ItemMasterUID;
                                    saleReturnList.ItemName = stock.ItemName;
                                    saleReturnList.Quantity = stockmovement.OutQty;
                                    saleReturnList.IMUOMUID = stock.IMUOMUID;
                                    saleReturnList.ItemCost = stock.ItemCost;
                                    saleReturnList.DispensedItemUID = stockmovement.RefUID ?? 0;
                                    saleReturnList.PatientOrderDetailUID = item.PatientOrderDetailUID;
                                    saleReturnList.PatientBilledItemUID = item.UID;
                                    saleReturnList.OwnerOrganisationUID = stock.OwnerOrganisationUID;
                                    saleReturnList.MUser = userUID;
                                    saleReturnList.MWhen = now;
                                    saleReturnList.CUser = userUID;
                                    saleReturnList.CWhen = now;
                                    saleReturnList.StatusFlag = "A";
                                    db.SaleReturnList.Add(saleReturnList);
                                    db.SaveChanges();

                                    double totalBalQty = SqlStatement.GetItemTotalQuantity(saleReturnList.ItemMasterUID, stock.StoreUID, stock.OwnerOrganisationUID);
                                    double balQty = stock.Quantity;
                                    SqlDirectStore.pInvenInsertStockMovement(saleReturnList.StockUID, stock.StoreUID, null, saleReturnList.ItemMasterUID, saleReturnList.BatchID, now, totalBFQty, bfQty, saleReturnList.Quantity, 0, balQty, totalBalQty, stock.IMUOMUID, stock.ItemCost ?? 0, null, "SaleReturnList", saleReturnList.UID, null, null, userUID);

                                    SqlDirectStore.pInvenInsertStockBalance(stock.StoreUID, stock.ItemMasterUID, userUID);

                                }
                            }
                        }

                        PatientVisit patpv = db.PatientVisit.Find(patientBill.PatientVisitUID);
                        if (patpv != null)
                        {
                            db.PatientVisit.Attach(patpv);
                            patpv.MUser = userUID;
                            patpv.MWhen = now;
                            patpv.IsBillFinalized = null;
                            patpv.VISTSUID = 418;
                            patpv.EndDttm = null;
                            db.SaveChanges();

                            #region PatientServiceEvent

                            PatientServiceEvent serviceEvent = new PatientServiceEvent();
                            serviceEvent.PatientVisitUID = patpv.UID;
                            serviceEvent.EventStartDttm = now;
                            serviceEvent.VISTSUID = patpv.VISTSUID.Value;
                            serviceEvent.MUser = userUID;
                            serviceEvent.MWhen = now;
                            serviceEvent.CUser = userUID;
                            serviceEvent.CWhen = now;
                            serviceEvent.StatusFlag = "A";

                            db.PatientServiceEvent.Add(serviceEvent);
                            #endregion

                            db.SaveChanges();
                        }

                        tran.Complete();
                    }


                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("UpdatePaymentMethod")]
        [HttpPut]
        public HttpResponseMessage UpdatePaymentMethod(long patientBillUID, int PAYMDUID, int userUID)
        {
            try
            {
                var patientBill = db.PatientBill.Find(patientBillUID);
                if (patientBill != null)
                {
                    db.PatientBill.Attach(patientBill);
                    patientBill.PAYMDUID = PAYMDUID;
                    patientBill.MUser = userUID;
                    patientBill.MWhen = DateTime.Now;
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("PrintStatementBill")]
        [HttpGet]
        public List<PatientBillModel> PrintStatementBill(long patientBillUID)
        {
            List<PatientBillModel> data = null;
            DataTable dt = SqlDirectStore.pPrintStatementBill(patientBillUID);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<PatientBillModel>();
                data = dt.ToList<PatientBillModel>();
            }
            return data;
        }

        #region BillingGroup
        [Route("GetBillingGroup")]
        [HttpGet]
        public List<BillingGroupModel> GetBillingGroup()
        {
            List<BillingGroupModel> billGroup = db.BillingGroup.Where(p => p.StatusFlag == "A")
                .Select(p => new BillingGroupModel
                {
                    BillingGroupUID = p.UID,
                    Name = p.Name,
                    Description = p.Description,
                    DisplayOrder = p.DisplayOrder,
                    CUser = p.CUser,
                    MUser = p.MUser,
                    CWhen = p.CWhen,
                    MWhen = p.MWhen,
                    StatusFlag = p.StatusFlag
                }).ToList();

            return billGroup;
        }

        #endregion

        #region BillingSubGroup

        [Route("GetBillingSubGroup")]
        [HttpGet]
        public List<BillingSubGroupModel> GetBillingSubGroup()
        {
            List<BillingSubGroupModel> billGroup = db.BillingSubGroup.Where(p => p.StatusFlag == "A")
                .Select(p => new BillingSubGroupModel
                {
                    BillingSubGroupUID = p.UID,
                    Name = p.Name,
                    Description = p.Description,
                    DisplayOrder = p.DisplayOrder,
                    BillingGroupUID = p.BillingGroupUID,
                    CUser = p.CUser,
                    MUser = p.MUser,
                    CWhen = p.CWhen,
                    MWhen = p.MWhen,
                    StatusFlag = p.StatusFlag
                }).ToList();

            return billGroup;
        }

        [Route("GetBillingSubGroupByGroup")]
        [HttpGet]
        public List<BillingSubGroupModel> GetBillingSubGroupByGroup(int billingGroupUID)
        {
            List<BillingSubGroupModel> billGroup = db.BillingSubGroup.Where(p => p.StatusFlag == "A" && p.BillingGroupUID == billingGroupUID)
                .Select(p => new BillingSubGroupModel
                {
                    BillingSubGroupUID = p.UID,
                    Name = p.Name,
                    Description = p.Description,
                    DisplayOrder = p.DisplayOrder,
                    BillingGroupUID = p.BillingGroupUID,
                    CUser = p.CUser,
                    MUser = p.MUser,
                    CWhen = p.CWhen,
                    MWhen = p.MWhen,
                    StatusFlag = p.StatusFlag
                }).ToList();

            return billGroup;
        }
        #endregion

        #region Billconfiguration


        [Route("GetBillConFiguration")]
        [HttpGet]
        public BillConfigurationModel GetBillConFiguration(int ownerOrganisationUID)
        {
            BillConfigurationModel billConData = null;
            var data =
                db.BillConfiguration
                .Where(p => p.StatusFlag == "A"
                && p.OwnerOrganisationUID == ownerOrganisationUID).FirstOrDefault();
            if (data != null)
            {
                DateTime now = DateTime.Now;
                billConData = new BillConfigurationModel();
                billConData.InsuranceCompanyUID = data.InsuranceCompanyUID;
                billConData.PayorUID = data.PayorUID;
                billConData.PayorAgreementUID = data.PayorAgreementUID;
                billConData.IsDisAuthorizationReqd = data.IsDisAuthorizationReqd;
            }
            return billConData;
        }

        #endregion

        #region BillPackage
        [Route("SearchBillPackage")]
        [HttpGet]
        public List<BillPackageModel> SearchBillPackage(string code, string name)
        {
            List<BillPackageModel> data = db.BillPackage.Where(p => p.StatusFlag == "A"
            && (string.IsNullOrEmpty(code) || p.Code.ToLower().Contains(code.ToLower()))
            && (string.IsNullOrEmpty(name) || p.PackageName.ToLower().Contains(name.ToLower()))
            ).Select(p => new BillPackageModel
            {
                BillPackageUID = p.UID,
                PackageName = p.PackageName,
                Description = p.Description,
                NoofDays = p.NoofDays,
                TotalAmount = p.TotalAmount,
                CURNCUID = p.CURNCUID,
                PBLCTUID = p.PBLCTUID ?? 0,
                ActiveFrom = p.ActiveFrom,
                ActiveTo = p.ActiveTo,
                MaxValue = p.MaxValue,
                MinValue = p.MinValue,
                Code = p.Code,
                OrderCategoryUID = p.OrderCategoryUID,
                OrderSubCategoryUID = p.OrderSubCategoryUID,
                OwnerOrganisationUID = p.OwnerOrganisationUID,
            }).ToList();
            
            return data;
        }

        [Route("GetBillPackageItemByUID")]
        [HttpGet]
        public List<BillPackageDetailModel> GetBillPackageItemByUID(int billPackageUID)
        {
            List<BillPackageDetailModel> data = (from p in db.BillPackageItem
                                                 join b in db.ItemMaster on p.ItemUID equals b.UID
                                                 where p.StatusFlag == "A"
                                                 && p.BillPackageUID == billPackageUID
                                                 && b.StatusFlag == "A"
                                                 select new BillPackageDetailModel
                                                    {
                                                        BillPackageUID = p.UID,
                                                        BillableItemUID = p.BillableItemUID,
                                                        Amount = p.Amount,
                                                        Quantity = p.Quantity,
                                                        ItemUID = p.ItemUID,
                                                        ItemCode = b.Code,
                                                        ItemName = b.Name,
                                                        CURNCUID = p.CURNCUID,
                                                        ActiveFrom = p.ActiveFrom,
                                                        ActiveTo = p.ActiveTo,
                                                        OrderCategoryUID = p.OrderCategoryUID,
                                                        OrderSubCategoryUID = p.OrderSubCategoryUID,
                                                        OwnerOrganisationUID = p.OwnerOrganisationUID,
                                                    }).ToList();

            return data;
        }

        [Route("ManageBillPackage")]
        [HttpPost]
        public HttpResponseMessage ManageBillPackage(BillPackageModel billPackageModel, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                using (var tran = new TransactionScope())
                {
                    BillPackageModel patient = new BillPackageModel();
                    var billPackage = db.BillPackage.Find(billPackageModel.BillPackageUID);

                    if (billPackage == null)
                    {
                        billPackage = new BillPackage();
                        billPackage.CUser = userID;
                        billPackage.CWhen = now;
                        billPackage.StatusFlag = "A";
                    }
                    billPackage.PackageName = billPackageModel.PackageName;
                    billPackage.Description = billPackageModel.Description;
                    billPackage.NoofDays = billPackageModel.NoofDays;
                    billPackage.TotalAmount = billPackageModel.TotalAmount;
                    billPackage.CURNCUID = billPackageModel.CURNCUID;
                    billPackage.PBLCTUID = billPackageModel.PBLCTUID;
                    billPackage.MaxValue = billPackageModel.MaxValue;
                    billPackage.MinValue = billPackageModel.MinValue;
                    billPackage.ActiveFrom = billPackageModel.ActiveFrom;
                    billPackage.ActiveTo = billPackageModel.ActiveTo;
                    billPackage.OrderCategoryUID = billPackageModel.OrderCategoryUID;
                    billPackage.OrderSubCategoryUID = billPackageModel.OrderSubCategoryUID;
                    billPackage.OwnerOrganisationUID = billPackageModel.OwnerOrganisationUID;
                    billPackage.Code = billPackageModel.Code;
                    billPackage.MUser = userID;
                    billPackage.MWhen = now;
                    billPackage.StatusFlag = "A";

                    db.BillPackage.AddOrUpdate(billPackage);
                    db.SaveChanges();

                    IEnumerable<BillPackageItem> groupReceiptDetails = db.BillPackageItem.Where(p => p.StatusFlag == "A" && p.BillPackageUID == billPackageModel.BillPackageUID);

                    if (billPackageModel.BillableItemDetails == null)
                    {
                        foreach (var item in groupReceiptDetails)
                        {
                            db.BillPackageItem.Attach(item);
                            item.MUser = userID;
                            item.MWhen = now;
                            item.StatusFlag = "D";
                        }
                    }
                    else
                    {
                        foreach (var item in groupReceiptDetails)
                        {
                            var data = billPackageModel.BillableItemDetails.FirstOrDefault(p => p.BillPackageDetailUID == item.UID);
                            if (data == null)
                            {
                                db.BillPackageItem.Attach(item);
                                item.MUser = userID;
                                item.MWhen = now;
                                item.StatusFlag = "D";
                            }

                        }
                    }

                    if (billPackageModel.BillableItemDetails != null)
                    {
                        foreach (var item in billPackageModel.BillableItemDetails)
                        {
                            var billPackageItem = db.BillPackageItem.Find(item.BillPackageDetailUID);
                            if (billPackageItem == null)
                            {
                                billPackageItem = new BillPackageItem();
                                billPackageItem.CUser = userID;
                                billPackageItem.CWhen = now;
                                billPackageItem.MUser = userID;
                                billPackageItem.MWhen = now;
                                billPackageItem.StatusFlag = "A";
                            }
                            else
                            {
                                if (item.MWhen != DateTime.MinValue)
                                {
                                    item.MUser = userID;
                                    item.MWhen = now;
                                }
                            }

                            billPackageItem.BillPackageUID = billPackage.UID;
                            billPackageItem.BillableItemUID = item.BillableItemUID;
                            billPackageItem.BSMDDUID = item.BSMDDUID;
                            billPackageItem.Comments = item.Comments;
                            billPackageItem.CURNCUID = item.CURNCUID;
                            billPackageItem.Amount = item.Amount;
                            billPackageItem.Quantity = item.Quantity;
                            billPackageItem.ItemUID = item.ItemUID;
                            billPackageItem.OrderCategoryUID = item.OrderCategoryUID;
                            billPackageItem.OrderSubCategoryUID = item.OrderSubCategoryUID;
                            billPackageItem.OwnerOrganisationUID = item.OwnerOrganisationUID;
                            billPackageItem.ActiveFrom = null;
                            billPackageItem.ActiveTo = null; 

                            db.BillPackageItem.AddOrUpdate(billPackageItem);
                            db.SaveChanges();
                        }
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

        [Route("DeleteBillPackage")]
        [HttpDelete]
        public HttpResponseMessage DeleteBillPackage(int billPackageUID, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                using (var tran = new TransactionScope())
                {
                    var billPackage = db.BillPackage.Find(billPackageUID);
                    if(billPackage != null)
                    {
                        db.BillPackage.Attach(billPackage);
                        billPackage.MUser = userID;
                        billPackage.MWhen = now;
                        billPackage.StatusFlag = "D";
                        db.SaveChanges();
                    }

                    var billPackageItems = db.BillPackageItem.Where(p => p.BillPackageUID == billPackageUID);
                    if (billPackageItems != null)
                    {
                        foreach (var item in billPackageItems)
                        {
                            db.BillPackageItem.Attach(item);
                            item.MUser = userID;
                            item.MWhen = now;
                            item.StatusFlag = "D";
                            db.SaveChanges();
                        }
                    }

                    tran.Complete();
                };

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        #endregion

        #region Insurance
        [Route("GetInsuranceCompanies")]
        [HttpGet]
        public List<InsuranceCompanyModel> GetInsuranceCompanies()
        {
            DateTime now = DateTime.Now;
            var data = db.InsuranceCompany
                .Where(p => p.StatusFlag == "A"
                && p.ActiveFrom == null || DbFunctions.TruncateTime(p.ActiveFrom) <= DbFunctions.TruncateTime(now)
                && p.ActiveTo == null || DbFunctions.TruncateTime(p.ActiveTo) >= DbFunctions.TruncateTime(now))
                .Select(p => new InsuranceCompanyModel
                {
                    InsuranceCompanyUID = p.UID,
                    CompanyName = p.CompanyName,
                    Code = p.Code,
                    CMPTPUID = p.CMPTPUID
                }).ToList();
            return data;
        }

        [Route("GetInsuranceCompanyByUID")]
        [HttpGet]
        public InsuranceCompanyModel GetInsuranceCompanyByUID(int insuranceCompanyUID)
        {
            DateTime now = DateTime.Now;
            InsuranceCompanyModel data = null;
            var insuranceCompany = db.InsuranceCompany.Find(insuranceCompanyUID);
            if (insuranceCompany != null)
            {
                data = new InsuranceCompanyModel();
                data.InsuranceCompanyUID = insuranceCompany.UID;
                data.CompanyName = insuranceCompany.CompanyName;
                data.Code = insuranceCompany.Code;
                data.CMPTPUID = insuranceCompany.CMPTPUID;
            }

            return data;
        }

        [Route("GetInsurancePlans")]
        [HttpGet]
        public List<InsurancePlanModel> GetInsurancePlans(int insuranceCompanyUID)
        {
            DateTime now = DateTime.Now;
            var data = (from i in db.InsurancePlan
                        join j in db.PayorAgreement on i.PayorAgreementUID equals j.UID
                        where i.InsuranceCompanyUID == insuranceCompanyUID
                        && i.StatusFlag == "A"
                        && i.ActiveFrom == null || DbFunctions.TruncateTime(i.ActiveFrom) <= DbFunctions.TruncateTime(now)
                        && i.ActiveTo == null || DbFunctions.TruncateTime(i.ActiveTo) >= DbFunctions.TruncateTime(now)
                        select new InsurancePlanModel
                        {
                            InsurancePlanUID = i.UID,
                            InsuranceCompanyUID = i.InsuranceCompanyUID,
                            PayorAgreementUID = i.PayorAgreementUID,
                            PayorAgreementName = j.Name,
                            PayorDetailUID = i.PayorDetailUID,
                            PayorName = SqlFunction.fGetPayorName(i.PayorDetailUID),
                            ClaimPercentage = j.ClaimPercentage,
                            FixedCopayAmount = j.FixedCopayAmount,
                            OPDCoverPerDay = j.OPDCoverPerDay,
                            PolicyMasterUID = j.PolicyMasterUID ?? 0,
                            PolicyName = SqlFunction.fGetPolicyName(j.PolicyMasterUID ?? 0),
                            StatusFlag = i.StatusFlag
                        }).ToList();
            return data;
        }

        [Route("GetPolicyMasterAll")]
        [HttpGet]
        public List<PolicyMasterModel> GetPolicyMasterAll()
        {
            var data = db.PolicyMaster.Where(p => p.StatusFlag == "A").Select(p => new PolicyMasterModel()
            {
                PolicyMasterUID = p.UID,
                Code = p.Code,
                PolicyName = p.PolicyName,
                Description = p.Description,
                AGTYPUID = p.AGTYPUID,
                AgreementType = SqlFunction.fGetRfValDescription(p.AGTYPUID ?? 0)
            }).ToList();

            return data;
        }

        [Route("GetPolicyMasterByUID")]
        [HttpGet]
        public PolicyMasterModel GetPolicyMasterByUID(int policyUID)
        {
            var data = db.PolicyMaster.Where(p => p.StatusFlag == "A" && p.UID == policyUID).Select(p => new PolicyMasterModel()
            {
                PolicyMasterUID = p.UID,
                Code = p.Code,
                PolicyName = p.PolicyName,
                Description = p.Description,
                AGTYPUID = p.AGTYPUID,
                AgreementType = SqlFunction.fGetRfValDescription(p.AGTYPUID ?? 0)
            }).FirstOrDefault();

            return data;
        }

        [Route("GetInsuranceCompanyAll")]
        [HttpGet]
        public List<InsuranceCompanyModel> GetInsuranceCompanyAll()
        {
            var data = db.InsuranceCompany.Where(p => p.StatusFlag == "A").Select(p => new InsuranceCompanyModel()
            {
                InsuranceCompanyUID = p.UID,
                Code = p.Code,
                CompanyName = p.CompanyName,
                Description = p.Description,
                CMPTPUID = p.CMPTPUID,
                ActiveFrom = p.ActiveFrom,
                ActiveTo = p.ActiveTo
            }).ToList();

            return data;
        }

        [Route("GetInsurancePlanAll")]
        [HttpGet]
        public List<InsurancePlanModel> GetInsurancePlanAll()
        {
            var data = db.InsurancePlan.Where(p => p.StatusFlag == "A").Select(p => new InsurancePlanModel()
            {
                InsurancePlanUID = p.UID,
                InsuranceCompanyUID = p.InsuranceCompanyUID,
                PayorAgreementUID = p.PayorAgreementUID,
                PayorDetailUID = p.PayorDetailUID,
                PolicyMasterUID = p.PolicyMasterUID,
                ActiveFrom = p.ActiveFrom,
                ActiveTo = p.ActiveTo,
                OwnerOrganisationUID = p.OwnerOrganisationUID
            }).ToList();

            return data;
        }

        [Route("GetInsurancePlansGroupPayorCompany")]
        [HttpGet]
        public List<InsurancePlanModel> GetInsurancePlansGroupPayorCompany()
        {
           
            var data = db.InsurancePlan.Where(p => p.StatusFlag == "A").GroupBy(g => new {g.InsuranceCompanyUID})
                .Select(x => new InsurancePlanModel()
                {
                    InsuranceCompanyUID = x.Key.InsuranceCompanyUID,
                    InsuranceCompanyName = SqlFunction.fGetInsuranceCompanyName(x.Key.InsuranceCompanyUID ?? 0),
                    //ActiveFrom = p.ActiveFrom,
                    //ActiveTo = p.ActiveTo,
                    //OwnerOrganisationUID = p.OwnerOrganisationUID
                }).ToList();

            return data;
        }

        [Route("SearchInsuranceCompany")]
        [HttpGet]
        public List<InsuranceCompanyModel> SearchInsuranceCompany(string code, string name)
        {
            var data = db.InsuranceCompany.Where(p => p.StatusFlag == "A"
                && (String.IsNullOrEmpty(code) || p.Code.ToLower().Contains(code.ToLower()))
                && (String.IsNullOrEmpty(name) || p.CompanyName.ToLower().Contains(name.ToLower()))).Select(p => new InsuranceCompanyModel()
                {
                    InsuranceCompanyUID = p.UID,
                    Code = p.Code,
                    CompanyName = p.CompanyName,
                    Description = p.Description,
                    CMPTPUID = p.CMPTPUID,
                    ActiveFrom = p.ActiveFrom,
                    ActiveTo = p.ActiveTo
                }).ToList();

            return data;
        }

        [Route("ManageInsuranceCompany")]
        [HttpPost]
        public HttpResponseMessage ManageInsuranceCompany(InsuranceCompanyModel insuranceCompanyModel, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                using (var tran = new TransactionScope())
                {
                    var insuranceCompany = db.InsuranceCompany.Find(insuranceCompanyModel.InsuranceCompanyUID);

                    if (insuranceCompany == null)
                    {
                        insuranceCompany = new InsuranceCompany();
                        insuranceCompany.CUser = userID;
                        insuranceCompany.CWhen = now;
                        insuranceCompany.StatusFlag = "A";
                    }
                    insuranceCompany.Code = insuranceCompanyModel.Code;
                    insuranceCompany.CompanyName = insuranceCompanyModel.CompanyName;
                    insuranceCompany.Description = insuranceCompanyModel.Description;
                    insuranceCompany.ActiveFrom = insuranceCompanyModel.ActiveFrom;
                    insuranceCompany.ActiveTo = insuranceCompanyModel.ActiveTo;
                    insuranceCompany.CMPTPUID = insuranceCompanyModel.CMPTPUID;
                    insuranceCompany.MUser = userID;
                    insuranceCompany.MWhen = now;
                    insuranceCompany.StatusFlag = "A";

                    db.InsuranceCompany.AddOrUpdate(insuranceCompany);
                    db.SaveChanges();

                    tran.Complete();
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("DeleteInsuranceCompany")]
        [HttpDelete]
        public HttpResponseMessage DeleteInsuranceCompany(int insuranceCompanyUID, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                using (var tran = new TransactionScope())
                {
                    var insuranceCompany = db.InsuranceCompany.Find(insuranceCompanyUID);
                    if (insuranceCompany != null)
                    {
                        db.InsuranceCompany.Attach(insuranceCompany);
                        insuranceCompany.MUser = userID;
                        insuranceCompany.MWhen = now;
                        insuranceCompany.StatusFlag = "D";
                        db.SaveChanges();
                    }

                    var payorOffice = db.PayorDetail.Where(p => p.InsuranceCompanyUID == insuranceCompanyUID);
                    if (payorOffice != null)
                    {
                        foreach (var item in payorOffice)
                        {
                            db.PayorDetail.Attach(item);
                            item.MUser = userID;
                            item.MWhen = now;
                            item.StatusFlag = "D";
                            db.SaveChanges();
                        }
                    }

                    var agreements = db.PayorAgreement.Where(p => p.InsuranceCompanyUID == insuranceCompanyUID);
                    if (agreements != null)
                    {
                        foreach (var item in agreements)
                        {
                            db.PayorAgreement.Attach(item);
                            item.MUser = userID;
                            item.MWhen = now;
                            item.StatusFlag = "D";
                            db.SaveChanges();
                        }
                    }

                    var insurancePlans = db.InsurancePlan.Where(p => p.InsuranceCompanyUID == insuranceCompanyUID);
                    if (insurancePlans != null)
                    {
                        foreach (var item in insurancePlans)
                        {
                            db.InsurancePlan.Attach(item);
                            item.MUser = userID;
                            item.MWhen = now;
                            item.StatusFlag = "D";
                            db.SaveChanges();
                        }
                    }

                    tran.Complete();
                };

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("ManageInsurancePlan")]
        [HttpPost]
        public HttpResponseMessage ManageInsurancePlan(InsurancePlanModel insurancePlanModel, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                using (var tran = new TransactionScope())
                {
                    var plan = db.InsurancePlan.Find(insurancePlanModel.InsurancePlanUID);

                    if (plan == null)
                    {
                        plan = new InsurancePlan();
                        plan.CUser = userID;
                        plan.CWhen = now;
                        plan.StatusFlag = "A";
                    }
                    plan.InsuranceCompanyUID = insurancePlanModel.InsuranceCompanyUID;
                    plan.OwnerOrganisationUID = insurancePlanModel.OwnerOrganisationUID;
                    plan.PayorAgreementUID = insurancePlanModel.PayorAgreementUID;
                    plan.PayorDetailUID = insurancePlanModel.PayorDetailUID;
                    plan.PolicyMasterUID = insurancePlanModel.PolicyMasterUID;
                    plan.ActiveFrom = insurancePlanModel.ActiveFrom ?? now;
                    plan.ActiveTo = insurancePlanModel.ActiveTo;
                    plan.MUser = userID;
                    plan.MWhen = now;
                    plan.StatusFlag = "A";

                    db.InsurancePlan.AddOrUpdate(plan);
                    db.SaveChanges();

                    tran.Complete();
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("DeleteInsurancePlan")]
        [HttpDelete]
        public HttpResponseMessage DeleteInsurancePlan(int insurancePlanUID, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                using (var tran = new TransactionScope())
                {
                    var insurancePlans = db.InsurancePlan.Where(p => p.UID == insurancePlanUID).FirstOrDefault();
                    if (insurancePlans != null)
                    {

                        db.InsurancePlan.Attach(insurancePlans);
                        insurancePlans.MUser = userID;
                        insurancePlans.MWhen = now;
                        insurancePlans.StatusFlag = "D";
                        db.SaveChanges();

                    }
                    tran.Complete();
                };
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("ManagePayorOfficeDetail")]
        [HttpPost]
        public HttpResponseMessage ManagePayorOfficeDetail(PayorDetailModel payorDetailModel, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                using (var tran = new TransactionScope())
                {
                    var payorDetail = db.PayorDetail.Find(payorDetailModel.PayorDetailUID);

                    if (payorDetail == null)
                    {
                        payorDetail = new PayorDetail();
                        payorDetail.CUser = userID;
                        payorDetail.CWhen = now;
                        payorDetail.StatusFlag = "A";
                    }
                    payorDetail.InsuranceCompanyUID = payorDetailModel.InsuranceCompanyUID;
                    payorDetail.Code = payorDetailModel.Code;
                    payorDetail.PayorName = payorDetailModel.PayorName;
                    payorDetail.Address1 = payorDetailModel.Address1;
                    payorDetail.Address2 = payorDetailModel.Address2;
                    payorDetail.DistrictUID = payorDetailModel.DistrictUID;
                    payorDetail.ContactPersonName = payorDetailModel.ContactPersonName;
                    payorDetail.ProvinceUID = payorDetailModel.ProvinceUID;
                    payorDetail.AmphurUID = payorDetailModel.AmphurUID;
                    payorDetail.ZipCode = payorDetailModel.ZipCode;
                    payorDetail.PhoneNumber = payorDetailModel.PhoneNumber;
                    payorDetail.MobileNumber = payorDetailModel.MobileNumber;
                    payorDetail.FaxNumber = payorDetailModel.FaxNumber;
                    payorDetail.EmailAddress = payorDetailModel.Email;
                    payorDetail.Note = payorDetailModel.Note;
                    payorDetail.ActiveFrom = payorDetailModel.ActiveFrom;
                    payorDetail.ActiveTo = payorDetailModel.ActiveTo;
                    payorDetail.CRDTRMUID = payorDetailModel.CRDTRMUID;
                    payorDetail.PYRACATUID = payorDetailModel.PYRACATUID;
                    payorDetail.ActiveFrom = payorDetailModel.ActiveFrom ?? now;
                    payorDetail.ActiveTo = payorDetailModel.ActiveTo;
                    payorDetail.MUser = userID;
                    payorDetail.MWhen = now;
                    payorDetail.StatusFlag = "A";

                    db.PayorDetail.AddOrUpdate(payorDetail);
                    db.SaveChanges();

                    tran.Complete();
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("DeletePayorOfficeDetail")]
        [HttpDelete]
        public HttpResponseMessage DeletePayorOfficeDetail(int payorDetailUID, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                using (var tran = new TransactionScope())
                {
                    MediTech.DataBase.PayorDetail payorDetail = db.PayorDetail.Find(payorDetailUID);
                    if (payorDetail != null)
                    {
                        db.PayorDetail.Attach(payorDetail);
                        payorDetail.MUser = userID;
                        payorDetail.MWhen = now;
                        payorDetail.StatusFlag = "D";
                        db.SaveChanges();
                    }

                    tran.Complete();
                };
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("ManagePayorAgreement")]
        [HttpPost]
        public HttpResponseMessage ManagePayorAgreement(PayorAgreementModel payorAgreementModel, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                using (var tran = new TransactionScope())
                {
                    var payorAgreement = db.PayorAgreement.Find(payorAgreementModel.PayorAgreementUID);

                    if (payorAgreement == null)
                    {
                        payorAgreement = new PayorAgreement();
                        payorAgreement.CUser = userID;
                        payorAgreement.CWhen = now;
                        payorAgreement.StatusFlag = "A";
                    }
                    payorAgreement.Code = payorAgreementModel.Code;
                    payorAgreement.Name = payorAgreementModel.Name;
                    payorAgreement.Description = payorAgreementModel.Description;
                    payorAgreement.PBTYPUID = payorAgreementModel.PBTYPUID;
                    payorAgreement.ActiveFrom = payorAgreementModel.ActiveFrom ?? now;
                    payorAgreement.ActiveTo = payorAgreementModel.ActiveTo;
                    payorAgreement.InsuranceCompanyUID = payorAgreementModel.InsuranceCompanyUID;
                    payorAgreement.PolicyMasterUID = payorAgreementModel.PolicyMasterUID;
                    payorAgreement.AgentName = payorAgreementModel.AgentName;
                    payorAgreement.BLTYPUID = payorAgreementModel.BLTYPUID;
                    payorAgreement.ClaimPercentage = payorAgreementModel.ClaimPercentage;
                    payorAgreement.CRDTRMUID = payorAgreementModel.CRDTRMUID;
                    payorAgreement.FixedCopayAmount = payorAgreementModel.FixedCopayAmount;
                    payorAgreement.IsForeign = payorAgreementModel.IsForeign;
                    payorAgreement.IsPackageDiscountAllowed = payorAgreementModel.IsPackageDiscountAllowed;
                    payorAgreement.OldAgreemntUID = payorAgreementModel.OldAgreemntUID;
                    payorAgreement.OPDCoverPerDay = payorAgreementModel.OPDCoverPerDay;
                    payorAgreement.PrimaryPBLCTUID = payorAgreementModel.PrimaryPBLCTUID;
                    payorAgreement.SecondaryPBLCTUID = payorAgreementModel.SecondaryPBLCTUID;
                    payorAgreement.TertiaryPBLCTUID = payorAgreementModel.TertiaryPBLCTUID;
                    payorAgreement.AGTYPUID = payorAgreementModel.AGTYPUID;
                    payorAgreement.MUser = userID;
                    payorAgreement.MWhen = now;
                    payorAgreement.StatusFlag = "A";

                    db.PayorAgreement.AddOrUpdate(payorAgreement);
                    db.SaveChanges();

                    tran.Complete();
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("DeletePayorAgreement")]
        [HttpDelete]
        public HttpResponseMessage DeletePayorAgreement(int payorAgreementUID, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                using (var tran = new TransactionScope())
                {
                    var agreements = db.PayorAgreement.Where(p => p.UID == payorAgreementUID).FirstOrDefault();
                    if (agreements != null)
                    {
                        db.PayorAgreement.Attach(agreements);
                        agreements.MUser = userID;
                        agreements.MWhen = now;
                        agreements.StatusFlag = "D";
                        db.SaveChanges();
                    }

                    tran.Complete();
                };
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }


        [Route("SearchPayorDetailByINCO")] //สร้างใหม่
        [HttpGet]
        public List<PayorDetailModel> SearchPayorDetailByINCO(string code, int? insuranceCompany)
        {
            List<PayorDetailModel> data = db.PayorDetail
                .Where(p => p.StatusFlag == "A"
                && (String.IsNullOrEmpty(code) || p.Code.ToLower().Contains(code.ToLower()))
                && (insuranceCompany == null || p.InsuranceCompanyUID == insuranceCompany)
                ).Select(p => new PayorDetailModel()
                {
                    PayorDetailUID = p.UID,
                    Code = p.Code,
                    PayorName = p.PayorName,
                    Address1 = p.Address1,
                    Address2 = p.Address2,
                    DistrictUID = p.DistrictUID,
                    AmphurUID = p.AmphurUID,
                    AddressFull = SqlFunction.fGetAddressPayorDetail(p.UID),
                    ProvinceUID = p.AmphurUID,
                    ZipCode = p.ZipCode,
                    ContactPersonName = p.ContactPersonName,
                    PYRACATUID = p.PYRACATUID,
                    PayorCategory = SqlFunction.fGetRfValDescription(p.PYRACATUID ?? 0),
                    CRDTRMUID = p.CRDTRMUID,
                    PaymentTerms = SqlFunction.fGetRfValDescription(p.CRDTRMUID ?? 0),
                    PhoneNumber = p.PhoneNumber,
                    MobileNumber = p.MobileNumber,
                    ActiveFrom = p.ActiveFrom,
                    ActiveTo = p.ActiveTo,
                    Email = p.EmailAddress,
                    InsuranceCompanyUID = p.InsuranceCompanyUID,
                    Note = p.Note,
                    CUser = p.CUser,
                    CWhen = p.CWhen,
                    MUser = p.MUser,
                    MWhen = p.MWhen,
                    StatusFlag = p.StatusFlag
                }).ToList();

            return data;
        }

        [Route("SearchInsurancePlaneByINCO")]
        [HttpGet]
        public List<InsurancePlanModel> SearchInsurancePlaneByINCO(int? insuranceCompanyUID)
        {
            List<InsurancePlanModel> data = db.InsurancePlan.Where(p => p.StatusFlag == "A"
                   && (insuranceCompanyUID == null || p.InsuranceCompanyUID == insuranceCompanyUID)
                   ).Select(p => new InsurancePlanModel()
                   {
                       PayorDetailUID = p.PayorDetailUID,
                       PayorAgreementUID = p.PayorAgreementUID,
                       InsurancePlanUID = p.UID,
                       InsuranceCompanyUID = p.InsuranceCompanyUID,
                       PolicyMasterUID = p.PolicyMasterUID,
                       OwnerOrganisationUID = p.OwnerOrganisationUID,
                       ActiveFrom = p.ActiveFrom,
                       ActiveTo = p.ActiveTo,
                       PayorName = SqlFunction.fGetPayorName(p.PayorDetailUID),
                       PayorAgreementName = SqlFunction.fGetPayorAgreementName(p.PayorAgreementUID),
                       InsuranceCompanyName = SqlFunction.fGetInsuranceCompanyName(p.InsuranceCompanyUID ?? 0),
                       PolicyName = SqlFunction.fGetPolicyName(p.PolicyMasterUID)
                   }).ToList();

            return data;
        }


        [Route("SearchPayorAgreementByINCO")] //สร้างใหม่
        [HttpGet]
        public List<PayorAgreementModel> SearchPayorAgreementByINCO(string code, int? insuranceCompany)
        {
            List<PayorAgreementModel> data = db.PayorAgreement
                   .Where(p => p.StatusFlag == "A"
                   && (String.IsNullOrEmpty(code) || p.Code.ToLower().Contains(code.ToLower()))
                   && (insuranceCompany == null || p.InsuranceCompanyUID == insuranceCompany)
                   ).Select(p => new PayorAgreementModel()
                   {
                       PayorAgreementUID = p.UID,
                       Name = p.Name,
                       Code = p.Code,
                       PayorBillType = SqlFunction.fGetRfValDescription(p.PBTYPUID ?? 0),
                       PBTYPUID = p.PBTYPUID,
                       PaymentTerms = SqlFunction.fGetRfValDescription(p.CRDTRMUID ?? 0),
                       CRDTRMUID = p.CRDTRMUID,
                       ActiveFrom = p.ActiveFrom,
                       ActiveTo = p.ActiveTo,
                       Description = p.Description,
                       AgentName = p.AgentName,
                       IsForeign = p.IsForeign,
                       BLTYPUID = p.BLTYPUID,
                       AGTYPUID = p.AGTYPUID,
                       PrimaryPBLCTUID = p.PrimaryPBLCTUID,
                       SecondaryPBLCTUID = p.SecondaryPBLCTUID,
                       TertiaryPBLCTUID = p.TertiaryPBLCTUID,
                       OPDCoverPerDay = p.OPDCoverPerDay,
                       ClaimPercentage = p.ClaimPercentage,
                       FixedCopayAmount = p.FixedCopayAmount,
                       IsPackageDiscountAllowed = p.IsPackageDiscountAllowed,
                       IsLimitAfterDiscount = p.IsLimitAfterDiscount,
                       DisplayOrder = p.DisplayOrder,
                       InsuranceCompanyUID = p.InsuranceCompanyUID,
                       PolicyMasterUID = p.PolicyMasterUID,
                       OldAgreemntUID = p.OldAgreemntUID
                   }).ToList();

            return data;
        }

        [Route("SearchPolicyMaster")]
        [HttpGet]
        public List<PolicyMasterModel> SearchPolicyMaster(string code, string name)
        {
            List<PolicyMasterModel> data = db.PolicyMaster
                    .Where(p => p.StatusFlag == "A"
                    && (String.IsNullOrEmpty(p.Code) || p.Code.ToLower().Contains(code.ToLower()))
                    && (String.IsNullOrEmpty(p.PolicyName) || p.PolicyName.ToLower().Contains(code.ToLower()))
                    ).Select(p => new PolicyMasterModel()
                    {
                        PolicyMasterUID = p.UID,
                        Code = p.Code,
                        PolicyName = p.PolicyName,
                        Description = p.Description,
                        AGTYPUID = p.AGTYPUID
                    }).ToList();
            return data;
        }

        [Route("ManagePolicyMaster")]
        [HttpPost]
        public HttpResponseMessage ManagePolicyMaster(PolicyMasterModel policyMaster, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                using (var tran = new TransactionScope())
                {
                    var policy = db.PolicyMaster.Find(policyMaster.PolicyMasterUID);

                    if (policy == null)
                    {
                        policy = new PolicyMaster();
                        policy.CUser = userID;
                        policy.CWhen = now;
                    }
                    policy.PolicyName = policyMaster.PolicyName;
                    policy.Code = policyMaster.Code;
                    policy.AGTYPUID = policyMaster.AGTYPUID;
                    policy.Description = policyMaster.Description;
                    policy.MUser = userID;
                    policy.MWhen = now;
                    policy.StatusFlag = "A";

                    db.PolicyMaster.AddOrUpdate(policy);
                    db.SaveChanges();

                    tran.Complete();
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("DeletePolicyMaster")]
        [HttpDelete]
        public HttpResponseMessage DeletePolicyMaster(int policyUID, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                using (var tran = new TransactionScope())
                {
                    var policy = db.PolicyMaster.Where(p => p.UID == policyUID).FirstOrDefault();
                    if (policy != null)
                    {
                        db.PolicyMaster.Attach(policy);
                        policy.MUser = userID;
                        policy.MWhen = now;
                        policy.StatusFlag = "D";
                        db.SaveChanges();
                    }
                    tran.Complete();
                };
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("CheckInsurancePlan")]
        [HttpGet]
        public InsurancePlanModel CheckInsurancePlan(int? payorDetailUID, int? payorAgreementUID)
        {
            InsurancePlanModel data = db.InsurancePlan.Where(p => p.StatusFlag == "A"
            && (payorDetailUID == null || p.PayorDetailUID == payorDetailUID)
            && (payorAgreementUID == null || p.PayorAgreementUID == payorAgreementUID))
                .Select(p => new InsurancePlanModel()
                {
                    InsurancePlanUID = p.UID,
                    InsuranceCompanyUID = p.InsuranceCompanyUID,
                    PayorAgreementUID = p.PayorAgreementUID,
                    PayorDetailUID = p.PayorDetailUID,
                    PolicyMasterUID = p.PolicyMasterUID,
                    ActiveFrom = p.ActiveFrom,
                    ActiveTo = p.ActiveTo,
                    OwnerOrganisationUID = p.OwnerOrganisationUID
                }).FirstOrDefault();
            return data;
        }

        #endregion



        #region PayorDetail

        [Route("SearchPayorDetail")]
        [HttpGet]
        public List<PayorDetailModel> SearchPayorDetail(string code, string name)
        {
            List<PayorDetailModel> data = db.PayorDetail
                .Where(p => p.StatusFlag == "A"
                && (String.IsNullOrEmpty(code) || p.Code.ToLower().Contains(code.ToLower()))
                && (String.IsNullOrEmpty(name) || p.PayorName.ToLower().Contains(name.ToLower()))
                ).Select(p => new PayorDetailModel()
                {
                    PayorDetailUID = p.UID,
                    Code = p.Code,
                    PayorName = p.PayorName,
                    AmphurUID = p.AmphurUID,
                    AddressFull = SqlFunction.fGetAddressPayorDetail(p.UID),
                    ProvinceUID = p.AmphurUID,
                    ContactPersonName = p.ContactPersonName,
                    PYRACATUID = p.PYRACATUID,
                    PayorCategory = SqlFunction.fGetRfValDescription(p.PYRACATUID ?? 0),
                    PhoneNumber = p.PhoneNumber,
                    MobileNumber = p.MobileNumber,
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

        [Route("GetPayorDetail")]
        [HttpGet]
        public List<PayorDetailModel> GetPayorDetail()
        {
            List<PayorDetailModel> data = db.PayorDetail.Where(p => p.StatusFlag == "A").Select(p => new PayorDetailModel()
            {
                PayorDetailUID = p.UID,
                Code = p.Code,
                PayorName = p.PayorName,
                AmphurUID = p.AmphurUID,
                AddressFull = SqlFunction.fGetAddressPayorDetail(p.UID),
                ProvinceUID = p.AmphurUID,
                ContactPersonName = p.ContactPersonName,
                PYRACATUID = p.PYRACATUID,
                PayorCategory = SqlFunction.fGetRfValDescription(p.PYRACATUID ?? 0),
                PhoneNumber = p.PhoneNumber,
                MobileNumber = p.MobileNumber,
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

        [Route("GetPayorDetailByUID")]
        [HttpGet]
        public PayorDetailModel GetPayorDetailByUID(int payorDetailUID)
        {
            var PayorDetail = db.PayorDetail.Find(payorDetailUID);
            PayorDetailModel data = null;
            if (PayorDetail != null)
            {
                data = new PayorDetailModel();
                data.PayorDetailUID = PayorDetail.UID;
                data.Code = PayorDetail.Code;
                data.PayorName = PayorDetail.PayorName;
                data.ProvinceUID = PayorDetail.ProvinceUID;
                data.AmphurUID = PayorDetail.AmphurUID;
                data.ContactPersonName = PayorDetail.ContactPersonName;
                data.PhoneNumber = PayorDetail.PhoneNumber;
                data.MobileNumber = PayorDetail.MobileNumber;
                data.FaxNumber = PayorDetail.FaxNumber;
                data.PYRACATUID = PayorDetail.PYRACATUID;
                data.ActiveFrom = PayorDetail.ActiveFrom;
                data.ActiveTo = PayorDetail.ActiveTo;
                data.CUser = PayorDetail.CUser;
                data.CWhen = PayorDetail.CWhen;
                data.MUser = PayorDetail.MUser;
                data.MWhen = PayorDetail.MWhen;
                data.StatusFlag = PayorDetail.StatusFlag;
            }


            return data;
        }


        [Route("GetPayorDetailByCode")]
        [HttpGet]
        public PayorDetailModel GetPayorDetailByCode(string payorCode)
        {
            var PayorDetail = db.PayorDetail.Where(w => w.Code == payorCode).FirstOrDefault();
            PayorDetailModel data = null;
            if (PayorDetail != null)
            {
                data = new PayorDetailModel();
                data.PayorDetailUID = PayorDetail.UID;
                data.Code = PayorDetail.Code;
                data.PayorName = PayorDetail.PayorName;
                data.ProvinceUID = PayorDetail.ProvinceUID;
                data.AmphurUID = PayorDetail.AmphurUID;
                data.ContactPersonName = PayorDetail.ContactPersonName;
                data.PhoneNumber = PayorDetail.PhoneNumber;
                data.MobileNumber = PayorDetail.MobileNumber;
                data.FaxNumber = PayorDetail.FaxNumber;
                data.PYRACATUID = PayorDetail.PYRACATUID;
                data.ActiveFrom = PayorDetail.ActiveFrom;
                data.ActiveTo = PayorDetail.ActiveTo;
                data.CUser = PayorDetail.CUser;
                data.CWhen = PayorDetail.CWhen;
                data.MUser = PayorDetail.MUser;
                data.MWhen = PayorDetail.MWhen;
                data.StatusFlag = PayorDetail.StatusFlag;
            }


            return data;
        }


        //[Route("CheckPayorDetailByCode")]
        //[HttpGet]
        //public bool CheckPayorDetailByCode(string codePayor)
        //{
        //    bool result = false;
        //    int PayorDetail = db.PayorDetail.Where(w=> w.Code == codePayor).Count();
        //    result = PayorDetail > 0 ? true : false;
        //    return result;
        //}



        [Route("ManagePayorDetail")]
        [HttpPost]
        public HttpResponseMessage ManagePayorDetail(PayorDetailModel payorDetailModel, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                MediTech.DataBase.PayorDetail payorDetail = db.PayorDetail.Find(payorDetailModel.PayorDetailUID);

                if (payorDetail == null)
                {
                    payorDetail = new MediTech.DataBase.PayorDetail();
                    payorDetail.CUser = userID;
                    payorDetail.CWhen = now;
                    payorDetail.MUser = userID;
                    payorDetail.MWhen = now;
                    payorDetail.StatusFlag = "A";
                }
                payorDetail.Code = payorDetailModel.Code;
                payorDetail.PayorName = payorDetailModel.PayorName;
                payorDetail.ContactPersonName = payorDetailModel.ContactPersonName;
                payorDetail.ProvinceUID = payorDetailModel.ProvinceUID;
                payorDetail.AmphurUID = payorDetailModel.AmphurUID;
                payorDetail.PhoneNumber = payorDetailModel.PhoneNumber;
                payorDetail.MobileNumber = payorDetailModel.MobileNumber;
                payorDetail.FaxNumber = payorDetailModel.FaxNumber;
                payorDetail.ActiveFrom = payorDetailModel.ActiveFrom;
                payorDetail.ActiveTo = payorDetailModel.ActiveTo;
                payorDetail.MUser = userID;
                payorDetail.MWhen = now;


                db.PayorDetail.AddOrUpdate(payorDetail);
                db.SaveChanges();







                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("DeletePayorDetail")]
        [HttpDelete]
        public HttpResponseMessage DeletePayorDetail(int payorDetailUID, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                MediTech.DataBase.PayorDetail payorDetail = db.PayorDetail.Find(payorDetailUID);
                if (payorDetail != null)
                {
                    db.PayorDetail.Attach(payorDetail);
                    payorDetail.MUser = userID;
                    payorDetail.MWhen = now;
                    payorDetail.StatusFlag = "D";
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

        #region PayorAgreeemnt
        [Route("GetAgreementByInsuranceUID")]
        [HttpGet]
        public List<PayorAgreementModel> GetAgreementByInsuranceUID(int insuranceUID)
        {
            List<PayorAgreementModel> data = db.PayorAgreement.Where(p => p.StatusFlag == "A" && p.InsuranceCompanyUID == insuranceUID).Select(p => new PayorAgreementModel()
            {
                Name = p.Name,
                PayorBillType = SqlFunction.fGetRfValDescription(p.PBTYPUID ?? 0),
                PBTYPUID = p.PBTYPUID,
                ActiveFrom = p.ActiveFrom,
                ActiveTo = p.ActiveTo,
                PayorAgreementUID = p.UID
            }).ToList();

            return data;
        }

        [Route("GetPayorAgreementByUID")]
        [HttpGet]
        public PayorAgreementModel GetPayorAgreementByUID(int agreementUID)
        {
            DateTime now = DateTime.Now;
            PayorAgreementModel data = null;
            var agreementData = db.PayorAgreement.Find(agreementUID);
            if (agreementData != null)
            {
                data = new PayorAgreementModel();
                data.Name = agreementData.Name;
                data.PBTYPUID = agreementData.PBTYPUID;
                data.ActiveFrom = agreementData.ActiveFrom;
                data.ActiveTo = agreementData.ActiveTo;
                data.PayorAgreementUID = agreementData.UID;

                var policyMaster = db.PolicyMaster.Find(agreementData.PolicyMasterUID);
                if(policyMaster != null)
                {
                    data.PolicyMasterUID = policyMaster.UID;
                    data.PolicyName = policyMaster.PolicyName;
                }
            }

            return data;
        }

        #endregion

        #region PolicyMaster
        [Route("GetPolicyMaster")]
        [HttpGet]
        public PolicyMasterModel GetPolicyMaster(int policyMasterUID)
        {
            PolicyMasterModel data = null;
            var policyMaster = db.PolicyMaster.Find(policyMasterUID);
            if (policyMaster != null)
            {
                data = new PolicyMasterModel();
                data.PolicyMasterUID = policyMaster.UID;
                data.Code = policyMaster.Code;
                data.PolicyName = policyMaster.PolicyName;
                data.Description = policyMaster.Description;
            }

            return data;
        }

        #endregion
    }
}