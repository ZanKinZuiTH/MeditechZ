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
                    && (p.ActiveFrom == null || (p.ActiveFrom.HasValue && p.ActiveFrom.Value.Date >= DateTime.Now.Date))
                    && (p.ActiveTo == null || (p.ActiveTo.HasValue && p.ActiveTo.Value.Date <= DateTime.Now.Date))
                    );
            }
            else
            {
                selectBillItemDetail = billItmDetail
    .FirstOrDefault(p => p.StatusFlag == "A" && p.OwnerOrganisationUID == 0
    && (p.ActiveFrom == null || (p.ActiveFrom.HasValue && p.ActiveFrom.Value.Date <= DateTime.Now.Date))
    && (p.ActiveTo == null || (p.ActiveTo.HasValue && p.ActiveTo.Value.Date >= DateTime.Now.Date))
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
                    ReferenceValue careAtHome = db.ReferenceValue.FirstOrDefault(p => p.StatusFlag == "A" && p.DomainCode == "VISTY" && p.ValueCode == "CATHOM");
                    PatientBill patBill = new PatientBill();
                    int seqBillID = 0;
                    string patientBillID = string.Empty;

                    var visitPayor = db.PatientVisitPayor.FirstOrDefault(p => p.PatientVisitUID == patpv.UID && p.StatusFlag == "A");
                    var healthOrganisationIDs = db.HealthOrganisationID.Where(p => p.HealthOrganisationUID == model.OwnerOrganisationUID && p.StatusFlag == "A");
                    if (healthOrganisationIDs != null && healthOrganisationIDs.Count() > 0)
                    {
                        var agreement = db.PayorAgreement.FirstOrDefault(p => p.UID == visitPayor.PayorAgreementUID);
                        string billType = "";
                        HealthOrganisationID healthIDBillType = null;
                        if (agreement.PBTYPUID == BLTYP_Receive)
                        {
                            healthIDBillType = healthOrganisationIDs.FirstOrDefault(p => p.BLTYPUID == BLTYP_Cash);
                            billType = "Cash";
                        }
                        else if (agreement.PBTYPUID == BLTYP_Invoice)
                        {
                            healthIDBillType = healthOrganisationIDs.FirstOrDefault(p => p.BLTYPUID == BLTYP_Credit);
                            billType = "Credit";
                        }
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
                        PayorDetail payorDetail = db.PayorDetail.Find(visitPayor.PayorDetailUID);
                        if (payorDetail != null && (payorDetail.IsGenerateBillNumber ?? false))
                        {
                            db.PayorDetail.Attach(payorDetail);
                            if (payorDetail.LastRenumberDttm == null)
                            {
                                payorDetail.LastRenumberDttm = now;
                            }
                            else
                            {
                                double dateDiff = ((now.Year - payorDetail.LastRenumberDttm.Value.Year) * 12) + now.Month - payorDetail.LastRenumberDttm.Value.Month;
                                if (dateDiff >= 1)
                                {
                                    payorDetail.LastRenumberDttm = now;
                                    payorDetail.NumberValue = 1;
                                }

                            }

                            patientBillID = SEQHelper.GetSEQBillNumber(payorDetail.IDFormat, payorDetail.IDLength.Value, payorDetail.NumberValue.Value);
                            seqBillID = payorDetail.NumberValue.Value;

                            payorDetail.NumberValue = ++payorDetail.NumberValue;

                            db.SaveChanges();
                        }
                        else
                        {
                            patientBillID = SEQHelper.GetSEQIDFormat("SEQPatientBill", out seqBillID);
                        }
                    }


                    if (string.IsNullOrEmpty(patientBillID))
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No SEQPatientBill in SEQCONFIGURATION");
                    }

                    if (seqBillID == 0)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Insert SEQPatientBill is Fail");
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
                            patBilled.ItemCost = billItem.TotalCost;
                            patBilled.ItemMutiplier = item.ItemMutiplier;
                            patBilled.Amount = item.Amount;
                            patBilled.Discount = item.Discount;
                            patBilled.NetAmount = item.NetAmount;

                            if (billItem.DoctorFee != null && billItem.DoctorFee > 0)
                            {
                                patBilled.DoctorFee = (billItem.DoctorFee / 100) * item.NetAmount;
                            }


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
            List<PatientBilledItemModel> data = new List<PatientBilledItemModel>(); ;
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

        [Route("SearchPatientBill")]
        [HttpGet]
        public List<PatientBillModel> SearchPatientBill(DateTime? dateFrom, DateTime? dateTo, long? patientUID, string billNumber, int? owerOrganisationUID)
        {
            List<PatientBillModel> data = null;
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
    }
}