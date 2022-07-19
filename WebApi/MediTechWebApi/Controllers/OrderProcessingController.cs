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
    [RoutePrefix("Api/OrderProcessing")]
    public class OrderProcessingController : ApiController
    {
        protected MediTechEntities db = new MediTechEntities();

        [Route("SearchOrderItem")]
        [HttpGet]
        public List<SearchOrderItem> SearchOrderItem(string text, int ownerOrganisationUID)
        {
            List<SearchOrderItem> data = SqlDirectStore.pSearchOrderItem(text, ownerOrganisationUID).ToList<SearchOrderItem>();

            return data;
        }


        [Route("GetOrderAllByPatientUID")]
        [HttpGet]
        public List<PatientOrderDetailModel> GetOrderAllByPatientUID(long patientUID, DateTime? dateFrom, DateTime? dateTo)
        {
            List<PatientOrderDetailModel> data = new List<PatientOrderDetailModel>(); ;
            DataTable dt = SqlDirectStore.pGetOrderALLByPatientUID(patientUID, dateFrom, dateTo);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {

                    PatientOrderDetailModel addItem = new PatientOrderDetailModel();
                    addItem.PatientOrderUID = long.Parse(dt.Rows[0]["PatientOrderUID"].ToString());
                    addItem.PatientOrderDetailUID = long.Parse(item["PatientOrderDetailUID"].ToString());
                    addItem.ItemCode = item["ItemCode"].ToString();
                    addItem.ItemName = item["ItemName"].ToString();
                    addItem.ItemUID = item["ItemUID"].ToString() != "" ? int.Parse(item["ItemUID"].ToString()) : (int?)null;
                    addItem.BillableItemUID = int.Parse(item["BillableItemUID"].ToString());
                    addItem.BSMDDUID = int.Parse(item["BSMDDUID"].ToString());
                    addItem.OrderNumber = item["OrderNumber"].ToString();
                    addItem.BillingService = item["BillingService"].ToString();
                    addItem.IdentifyingUID = item["IdentifyingUID2"].ToString() != "" ? long.Parse(item["IdentifyingUID2"].ToString()) : (long?)null;
                    addItem.IdentifyingType = item["IdentifyingType2"].ToString();
                    addItem.OrderSetUID = item["OrderSetUID"].ToString() != "" ? int.Parse(item["OrderSetUID"].ToString()) : (int?)null;
                    addItem.OrderSetName = item["OrderSetName"].ToString();
                    addItem.StartDttm = DateTime.Parse(item["StartDttm2"].ToString());
                    addItem.ORDSTUID = int.Parse(item["ORDSTUID2"].ToString());
                    addItem.OrderDetailStatus = item["OrderDetailStatus"].ToString();
                    addItem.Dosage = item["Dosage"].ToString() != "" ? double.Parse(item["Dosage"].ToString()) : (double?)null;
                    addItem.Quantity = item["Quantity"].ToString() != "" ? double.Parse(item["Quantity"].ToString()) : (double?)null;
                    addItem.QNUOMUID = item["QNUOMUID"].ToString() != "" ? int.Parse(item["QNUOMUID"].ToString()) : (int?)null;
                    addItem.QuantityUnit = item["QuantityUnit"].ToString();
                    addItem.FRQNCUID = item["FRQNCUID"].ToString() != "" ? int.Parse(item["FRQNCUID"].ToString()) : (int?)null;
                    addItem.DrugFrequency = item["DrugFrequency"].ToString();
                    //addItem.DrugGenaricUID = int.Parse(item["DrugGenaricUID"].ToString());
                    //addItem.GenericName = item["GenericName"].ToString();
                    addItem.UnitPrice = item["UnitPrice"].ToString() != "" ? double.Parse(item["UnitPrice"].ToString()) : (double?)null;
                    addItem.DoctorFee = item["DoctorFee"].ToString() != "" ? double.Parse(item["DoctorFee"].ToString()) : (double?)null;
                    addItem.CareproviderUID = item["CareproviderUID"].ToString() != "" ? int.Parse(item["CareproviderUID"].ToString()) : (int?)null;
                    addItem.CareproviderName = item["CareproviderName"].ToString();
                    addItem.NetAmount = item["NetAmount"].ToString() != "" ? double.Parse(item["NetAmount"].ToString()) : (double?)null;
                    addItem.ROUTEUID = item["ROUTEUID"].ToString() != "" ? int.Parse(item["ROUTEUID"].ToString()) : (int?)null;
                    addItem.DFORMUID = item["DFORMUID"].ToString() != "" ? int.Parse(item["DFORMUID"].ToString()) : (int?)null;
                    addItem.TypeDrug = item["TypeDrug"].ToString();
                    addItem.PDSTSUID = item["PDSTSUID"].ToString() != "" ? int.Parse(item["PDSTSUID"].ToString()) : (int?)null;
                    addItem.InstructionRoute = item["InstructionRoute"].ToString();
                    addItem.InstructionText = item["InstructionText"].ToString();
                    addItem.DrugDuration = item["DrugDuration"].ToString() != "" ? int.Parse(item["DrugDuration"].ToString()) : (int?)null;
                    addItem.LocalInstructionText = item["LocalInstructionText"].ToString();
                    addItem.IsStock = item["IsStock"].ToString();
                    addItem.StoreUID = item["StoreUID"].ToString() != "" ? int.Parse(item["StoreUID"].ToString()) : (int?)null;
                    //addItem.ExpiryDate = item["ExpiryDate"].ToString() != "" ? DateTime.Parse(item["ExpiryDate"].ToString()) : (DateTime?)null;
                    //addItem.BalQty = item["BalQty"].ToString() != "" ? double.Parse(item["BalQty"].ToString()) : (double?)null;
                    //addItem.BatchID = item["BatchID"].ToString();
                    //addItem.StockUID = item["StockUID"].ToString() != "" ? int.Parse(item["StockUID"].ToString()) : (int?)null;
                    addItem.IsPriceOverwrite = item["IsPriceOverwrite"].ToString();
                    addItem.OverwritePrice = item["OverwritePrice"].ToString() != "" ? double.Parse(item["OverwritePrice"].ToString()) : (double?)null;
                    addItem.Comments = item["Comments"].ToString();
                    addItem.CWhen = DateTime.Parse(item["CWhen"].ToString());
                    addItem.PaymentStatus = item["PaymentStatus"].ToString();
                    addItem.OwnerOrganisationName = item["OwnerOrganisationName"].ToString();
                    addItem.OwnerOrganisationUID = int.Parse(item["OwnerOrganisationUID"].ToString());
                    if (addItem.IsPriceOverwrite == "Y")
                    {
                        addItem.DisplayPrice = addItem.OverwritePrice;
                    }
                    else
                    {
                        addItem.DisplayPrice = addItem.UnitPrice;
                    }

                    addItem.OrderBy = item["OrderBy"].ToString();
                    data.Add(addItem);
                }
            }


            return data;
        }

        [Route("GetOrderAllByVisitUID")]
        [HttpGet]
        public List<PatientOrderDetailModel> GetOrderAllByVisitUID(long visitUID, DateTime? dateFrom, DateTime? dateTo)
        {
            List<PatientOrderDetailModel> data = new List<PatientOrderDetailModel>(); ;
            DataTable dt = SqlDirectStore.pGetOrderAllByVisitUID(visitUID, dateFrom, dateTo);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {

                    PatientOrderDetailModel addItem = new PatientOrderDetailModel();
                    addItem.PatientOrderUID = long.Parse(dt.Rows[0]["PatientOrderUID"].ToString());
                    addItem.PatientOrderDetailUID = long.Parse(item["PatientOrderDetailUID"].ToString());
                    addItem.ItemCode = item["ItemCode"].ToString();
                    addItem.ItemName = item["ItemName"].ToString();
                    addItem.ItemUID = item["ItemUID"].ToString() != "" ? int.Parse(item["ItemUID"].ToString()) : (int?)null;
                    addItem.BillableItemUID = int.Parse(item["BillableItemUID"].ToString());
                    addItem.BSMDDUID = int.Parse(item["BSMDDUID"].ToString());
                    addItem.OrderNumber = item["OrderNumber"].ToString();
                    addItem.BillingService = item["BillingService"].ToString();
                    addItem.IdentifyingUID = item["IdentifyingUID2"].ToString() != "" ? long.Parse(item["IdentifyingUID2"].ToString()) : (long?)null;
                    addItem.IdentifyingType = item["IdentifyingType2"].ToString();
                    addItem.OrderSetUID = item["OrderSetUID"].ToString() != "" ? int.Parse(item["OrderSetUID"].ToString()) : (int?)null;
                    addItem.OrderSetName = item["OrderSetName"].ToString();
                    addItem.StartDttm = DateTime.Parse(item["StartDttm2"].ToString());
                    addItem.ORDSTUID = int.Parse(item["ORDSTUID2"].ToString());
                    addItem.OrderDetailStatus = item["OrderDetailStatus"].ToString();
                    addItem.Dosage = item["Dosage"].ToString() != "" ? double.Parse(item["Dosage"].ToString()) : (double?)null;
                    addItem.Quantity = item["Quantity"].ToString() != "" ? double.Parse(item["Quantity"].ToString()) : (double?)null;
                    addItem.QNUOMUID = item["QNUOMUID"].ToString() != "" ? int.Parse(item["QNUOMUID"].ToString()) : (int?)null;
                    addItem.QuantityUnit = item["QuantityUnit"].ToString();
                    addItem.FRQNCUID = item["FRQNCUID"].ToString() != "" ? int.Parse(item["FRQNCUID"].ToString()) : (int?)null;
                    addItem.DrugFrequency = item["DrugFrequency"].ToString();
                    //addItem.DrugGenaricUID = int.Parse(item["DrugGenaricUID"].ToString());
                    //addItem.GenericName = item["GenericName"].ToString();
                    addItem.UnitPrice = item["UnitPrice"].ToString() != "" ? double.Parse(item["UnitPrice"].ToString()) : (double?)null;
                    addItem.DoctorFee = item["DoctorFee"].ToString() != "" ? double.Parse(item["DoctorFee"].ToString()) : (double?)null;
                    addItem.CareproviderUID = item["CareproviderUID"].ToString() != "" ? int.Parse(item["CareproviderUID"].ToString()) : (int?)null;
                    addItem.CareproviderName = item["CareproviderName"].ToString();
                    addItem.NetAmount = item["NetAmount"].ToString() != "" ? double.Parse(item["NetAmount"].ToString()) : (double?)null;
                    addItem.ROUTEUID = item["ROUTEUID"].ToString() != "" ? int.Parse(item["ROUTEUID"].ToString()) : (int?)null;
                    addItem.DFORMUID = item["DFORMUID"].ToString() != "" ? int.Parse(item["DFORMUID"].ToString()) : (int?)null;
                    addItem.TypeDrug = item["TypeDrug"].ToString();
                    addItem.PDSTSUID = item["PDSTSUID"].ToString() != "" ? int.Parse(item["PDSTSUID"].ToString()) : (int?)null;
                    addItem.InstructionRoute = item["InstructionRoute"].ToString();
                    addItem.InstructionText = item["InstructionText"].ToString();
                    addItem.DrugDuration = item["DrugDuration"].ToString() != "" ? int.Parse(item["DrugDuration"].ToString()) : (int?)null;
                    addItem.LocalInstructionText = item["LocalInstructionText"].ToString();
                    addItem.IsStock = item["IsStock"].ToString();
                    addItem.StoreUID = item["StoreUID"].ToString() != "" ? int.Parse(item["StoreUID"].ToString()) : (int?)null;
                    //addItem.ExpiryDate = item["ExpiryDate"].ToString() != "" ? DateTime.Parse(item["ExpiryDate"].ToString()) : (DateTime?)null;
                    //addItem.BalQty = item["BalQty"].ToString() != "" ? double.Parse(item["BalQty"].ToString()) : (double?)null;
                    //addItem.BatchID = item["BatchID"].ToString();
                    //addItem.StockUID = item["StockUID"].ToString() != "" ? int.Parse(item["StockUID"].ToString()) : (int?)null;
                    addItem.IsPriceOverwrite = item["IsPriceOverwrite"].ToString();
                    addItem.OverwritePrice = item["OverwritePrice"].ToString() != "" ? double.Parse(item["OverwritePrice"].ToString()) : (double?)null;
                    addItem.Comments = item["Comments"].ToString();
                    addItem.CWhen = DateTime.Parse(item["CWhen"].ToString());
                    addItem.PaymentStatus = item["PaymentStatus"].ToString();
                    addItem.OwnerOrganisationName = item["OwnerOrganisationName"].ToString();
                    addItem.OwnerOrganisationUID = int.Parse(item["OwnerOrganisationUID"].ToString());
                    if (addItem.IsPriceOverwrite == "Y")
                    {
                        addItem.DisplayPrice = addItem.OverwritePrice;
                    }
                    else
                    {
                        addItem.DisplayPrice = addItem.UnitPrice;
                    }

                    addItem.OrderBy = item["OrderBy"].ToString();
                    data.Add(addItem);
                }
            }


            return data;
        }

        [Route("CancelOrders")]
        [HttpPut]
        public HttpResponseMessage CancelOrders(List<long> patientOrderDetailUIDs, string cancelReason, int userUID)
        {
            try
            {
                DateTime now = DateTime.Now;
                long? requestUID = null;
                long? prescriptionUID = null;
                foreach (var orderDetailUID in patientOrderDetailUIDs)
                {
                    var dataOrderDetail = db.PatientOrderDetail.Find(orderDetailUID);
                    if (dataOrderDetail != null)
                    {
                        db.PatientOrderDetail.Attach(dataOrderDetail);
                        dataOrderDetail.ORDSTUID = 2848;
                        dataOrderDetail.MUser = userUID;
                        dataOrderDetail.MWhen = now;
                        dataOrderDetail.CancelledByUserUID = userUID;
                        dataOrderDetail.CancelledDttm = now;
                        dataOrderDetail.CancelledReason = cancelReason;
                        if (dataOrderDetail.IdentifyingType == "PRESCRIPTIONITEM")
                        {
                            var dataPrescriptionItem = db.PrescriptionItem.Find(dataOrderDetail.IdentifyingUID);
                            if (dataPrescriptionItem != null)
                            {
                                db.PrescriptionItem.Attach(dataPrescriptionItem);
                                dataPrescriptionItem.ORDSTUID = 2848;
                                dataPrescriptionItem.MUser = userUID;
                                dataPrescriptionItem.MWhen = now;
                                prescriptionUID = dataPrescriptionItem.PrescriptionUID;
                            }
                        }

                        if (dataOrderDetail.IdentifyingType == "REQUESTDETAIL")
                        {
                            var dataRequestDetail = db.RequestDetail.Find(dataOrderDetail.IdentifyingUID);

                            if (dataRequestDetail != null)
                            {
                                db.RequestDetail.Attach(dataRequestDetail);
                                dataRequestDetail.ORDSTUID = 2848;
                                dataRequestDetail.MUser = userUID;
                                dataRequestDetail.MWhen = now;
                                requestUID = dataRequestDetail.RequestUID;

                                var dataRequestDetailSpecimen = db.RequestDetailSpecimen.FirstOrDefault(p => p.RequestDetailUID == dataRequestDetail.UID);

                                if (dataRequestDetailSpecimen != null)
                                {
                                    db.RequestDetailSpecimen.Attach(dataRequestDetailSpecimen);
                                    dataRequestDetailSpecimen.SPSTSUID = 2848;
                                    dataRequestDetailSpecimen.MUser = userUID;
                                    dataRequestDetailSpecimen.MWhen = now;
                                }

                                var result = db.Result.FirstOrDefault(p => p.RequestDetailUID == dataRequestDetail.UID && p.StatusFlag == "A");
                                if (result != null)
                                {
                                    db.Result.Attach(result);
                                    result.ORDSTUID = 2848;
                                    result.MUser = userUID;
                                    result.MWhen = now;

                                    var itemGroupResult = db.RequestItemGroupResult.Where(p => p.RequestItemUID == dataRequestDetail.RequestitemUID && p.StatusFlag == "A");
                                    if (itemGroupResult != null && itemGroupResult.Count() > 0)
                                    {
                                        foreach (var itemResult in itemGroupResult)
                                        {
                                            var groupResult = db.CheckupGroupResult.FirstOrDefault(p => p.StatusFlag == "A"
                                            && p.PatientVisitUID == result.PatientVisitUID
                                            && p.GPRSTUID == itemResult.GPRSTUID);
                                            if (groupResult != null)
                                            {
                                                db.CheckupGroupResult.Attach(groupResult);
                                                groupResult.StatusFlag = "D";
                                                groupResult.MUser = userUID;
                                                groupResult.MWhen = now;
                                            }
                                        }

                                    }
                                }
                            }
                        }



                        #region SavePatinetOrderDetailHistory

                        PatientOrderDetailHistory patientOrderDetailHistory = new PatientOrderDetailHistory();
                        patientOrderDetailHistory.PatientOrderDetailUID = dataOrderDetail.UID;
                        patientOrderDetailHistory.ORDSTUID = 2848;
                        patientOrderDetailHistory.EditedDttm = now;
                        patientOrderDetailHistory.EditByUserID = userUID;
                        patientOrderDetailHistory.CUser = userUID;
                        patientOrderDetailHistory.CWhen = now;
                        patientOrderDetailHistory.MUser = userUID;
                        patientOrderDetailHistory.MWhen = now;
                        patientOrderDetailHistory.StatusFlag = "A";
                        db.PatientOrderDetailHistory.Add(patientOrderDetailHistory);

                        #endregion


                        List<PatientBillableItem> listpatBillableItem = db.PatientBillableItem.Where(p => p.StatusFlag == "A" && p.PatientOrderDetailUID == dataOrderDetail.UID).ToList();
                        foreach (var item in listpatBillableItem)
                        {
                            db.PatientBillableItem.Attach(item);
                            item.ORDSTUID = 2848;
                            item.MUser = userUID;
                            item.MWhen = now;
                            item.StatusFlag = "D";

                            db.SaveChanges();
                        }


                        if (requestUID != null)
                        {
                            var request = db.Request.Find(requestUID);

                            db.Request.Attach(request);
                            request.MUser = userUID;
                            request.MWhen = now;
                            request.ORDSTUID = (new RadiologyController()).CheckRequestStatus(request.UID);

                            db.SaveChanges();
                        }

                        if (prescriptionUID != null)
                        {
                            var prescription = db.Prescription.Find(prescriptionUID);

                            db.Prescription.Attach(prescription);
                            prescription.MUser = userUID;
                            prescription.MWhen = now;
                            prescription.ORDSTUID = (new PharmacyController()).CheckPrescriptionStatus(prescription.UID);

                            db.SaveChanges();
                        }

                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("CriteriaOrderAlert")]
        [HttpPost]
        public HttpResponseMessage CriteriaOrderAlert(long patientUID, BillableItemModel billItemModel)
        {
            try
            {
                List<PatientOrderAlertModel> orderAlerts = new List<PatientOrderAlertModel>();

                if (billItemModel.BillingServiceMetaData == "Drug")
                {
                    var patientAllergy = (new PatientIdentityController()).GetPatientAllergyByPatientUID(patientUID);
                    var ItemMaster = db.ItemMaster.Find(billItemModel.ItemUID);
                    if (patientAllergy != null)
                    {

                        var allergyGeneric = patientAllergy.FirstOrDefault(p => p.IdentifyingType == "ชื่อยาสามัญ" && p.IdentifyingUID == ItemMaster.DrugGenaricUID);
                        if (allergyGeneric != null)
                        {
                            PatientOrderAlertModel orderAlert = new PatientOrderAlertModel();
                            orderAlert.AlertType = "DrugAllergy";
                            orderAlert.AlertMessage = allergyGeneric.AllergicTo == null ? ItemMaster.Name : allergyGeneric.AllergicTo;
                            orderAlerts.Add(orderAlert);
                        }
                        var allergyDrug = patientAllergy.FirstOrDefault(p => p.IdentifyingType == "ยา" && p.IdentifyingUID == ItemMaster.UID);
                        if (allergyDrug != null)
                        {
                            PatientOrderAlertModel orderAlert = new PatientOrderAlertModel();
                            orderAlert.AlertType = "DrugAllergy";
                            orderAlert.AlertMessage = allergyDrug.AllergicTo == null ? ItemMaster.Name : allergyDrug.AllergicTo;
                            orderAlerts.Add(orderAlert);
                        }

                    }
                }

                DataTable dtDupicate = SqlDirectStore.pGetOrderDuplicate(patientUID);
                if (dtDupicate != null && dtDupicate.Rows.Count > 0)
                {
                    DataRow[] rowDuplicate = dtDupicate.Select("BillableItemUID = " + billItemModel.BillableItemUID);
                    if (rowDuplicate != null)
                    {
                        for (int i = 0; i < rowDuplicate.Count(); i++)
                        {
                            PatientOrderAlertModel orderAlert = new PatientOrderAlertModel();
                            if (billItemModel.BillingServiceMetaData == "Drug")
                            {
                                orderAlert.AlertType = "DrugDuplicate";

                            }
                            else
                            {
                                orderAlert.AlertType = "OrderDuplicate";
                            }

                            orderAlert.AlertMessage = rowDuplicate[i]["ItemName"].ToString() + " => " + Convert.ToDateTime(rowDuplicate[i]["StartDttm"]).ToString("dd/MM/yyyy HH:mm") + " => " + "รายการนี้มีการคีย์ซ้ำภายใน 24 ชั่วโมง";
                            orderAlerts.Add(orderAlert);
                        }

                    }
                }


                return Request.CreateResponse(HttpStatusCode.OK, orderAlerts);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }

        }

        [Route("CreateOrder")]
        [HttpPost]
        public HttpResponseMessage CreateOrder(long patientUID, long patientVisitUID, int userUID, int locationUID, int ownerOrganisationUID, List<PatientOrderDetailModel> orderDetails)
        {
            try
            {
                string OrderNumber = "";
                DateTime now = DateTime.Now;
                int RAISEDUID = 2847;
                int REGISTUID = 2869;

                var refValue = db.ReferenceValue.Where(p => (p.DomainCode == "BSMDD" || p.DomainCode == "ENTYP")&& p.StatusFlag == "A");

                int BSMDD_LAB = refValue.FirstOrDefault(p => p.DomainCode == "BSMDD" && p.ValueCode == "LABBB").UID;
                int BSMDD_RADIO = refValue.FirstOrDefault(p => p.DomainCode == "BSMDD" && p.ValueCode == "RADIO").UID;
                int BSMDD_MBCUP = refValue.FirstOrDefault(p => p.DomainCode == "BSMDD" && p.ValueCode == "MBCUP").UID;
                int BSMDD_STORE = refValue.FirstOrDefault(p => p.DomainCode == "BSMDD" && p.ValueCode == "STORE").UID;
                //int BSMDD_ORDITEM = 2839;
                int BSMDD_MDSLP = refValue.FirstOrDefault(p => p.DomainCode == "BSMDD" && p.ValueCode == "MDSLP").UID;
                int BSMDD_SULPY = refValue.FirstOrDefault(p => p.DomainCode == "BSMDD" && p.ValueCode == "SUPLY").UID;

                int ENTYP_INPAT = refValue.FirstOrDefault(p => p.DomainCode == "ENTYP" && p.ValueCode == "INPAT").UID;

                int RQPRTUID = 440; //Priority Normal
                int statusOrder;
                var patientVisit = db.PatientVisit.Find(patientVisitUID);
                using (var tran = new TransactionScope())
                {
                    IEnumerable<int> groupBSMDD = orderDetails.Select(p => p.BSMDDUID).Distinct();
                    IEnumerable<int> groupOrderType = orderDetails.Select(p => p.PRSTYPUID ?? 0).Distinct();

                    foreach (var OrderType in groupOrderType)
                    {
                        foreach (int BSMDDUID in groupBSMDD)
                        {

                            var dataInOrderDetail = orderDetails.Where(p => p.BSMDDUID == BSMDDUID && p.PRSTYPUID == OrderType);
                            if (dataInOrderDetail != null && dataInOrderDetail.Count() > 0)
                            {

                                #region PatientOrder

                                PatientOrder patientOrder = new PatientOrder();
                                patientOrder.CUser = userUID;
                                patientOrder.CWhen = now;

                                int seqPatientOrderID;
                                string patientOrderID = SEQHelper.GetSEQIDFormat("SEQPatientOrder", out seqPatientOrderID);

                                if (string.IsNullOrEmpty(patientOrderID))
                                {
                                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No SEQPatientOrder in SEQCONFIGURATION");
                                }

                                if (seqPatientOrderID == 0)
                                {
                                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Insert SEQPatientOrder is Fail");
                                }

                                patientOrder.OrderNumber = patientOrderID;
                                patientOrder.StartDttm = now;
                                patientOrder.EndDttm = now;

                                if (BSMDDUID == 2841) //Radiology
                                {
                                    statusOrder = REGISTUID;
                                }
                                else
                                {
                                    statusOrder = RAISEDUID;
                                }
                                patientOrder.PRSTYPUID = OrderType;
                                patientOrder.OrderRaisedBy = userUID;
                                patientOrder.MUser = userUID;
                                patientOrder.MWhen = now;
                                patientOrder.StatusFlag = "A";
                                patientOrder.PatientUID = patientUID;
                                patientOrder.PatientVisitUID = patientVisitUID;
                                patientOrder.OrderLocationUID = locationUID;
                                patientOrder.OwnerOrganisationUID = ownerOrganisationUID;
                                patientOrder.IdentifyingType = "PATIENTORDER";
                                db.PatientOrder.Add(patientOrder);

                                db.SaveChanges();

                                #endregion

                                #region Request

                                if (BSMDDUID == BSMDD_LAB || BSMDDUID == BSMDD_RADIO || BSMDDUID == BSMDD_MBCUP)
                                {
                                    Request request = new Request();
                                    request.CUser = userUID;
                                    request.CWhen = now;
                                    request.StatusFlag = "A";
                                    int outrequestUID;
                                    string seqRequestID;

                                    if (BSMDDUID == BSMDD_LAB)
                                    {
                                        seqRequestID = SEQHelper.GetSEQIDFormat("SEQLISRequest", out outrequestUID);
                                    }
                                    else
                                    {
                                        seqRequestID = SEQHelper.GetSEQIDFormat("SEQRISRequest", out outrequestUID);
                                    }


                                    if (string.IsNullOrEmpty(seqRequestID))
                                    {
                                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No SEQRequest in SEQCONFIGURATION");
                                    }

                                    if (outrequestUID == 0)
                                    {
                                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Insert SEQRequest is Fail");
                                    }


                                    request.RequestNumber = seqRequestID;
                                    request.RequestedDttm = now;
                                    request.RequestedUserUID = userUID;
                                    request.BSMDDUID = BSMDDUID;
                                    request.ORDSTUID = statusOrder;


                                    request.RQPRTUID = RQPRTUID;


                                    request.PatientUID = patientUID;
                                    request.PatientVisitUID = patientVisitUID;
                                    request.PatientOrderUID = patientOrder.UID;
                                    request.MUser = userUID;
                                    request.MWhen = now;
                                    request.OwnerOrganisationUID = ownerOrganisationUID;


                                    db.Request.Add(request);
                                    db.SaveChanges();


                                    db.PatientOrder.Attach(patientOrder);
                                    patientOrder.IdentifyingType = "REQUEST";
                                    patientOrder.IdentifyingUID = request.UID;
                                    db.SaveChanges();
                                }

                                #endregion

                                #region Store
                                if (BSMDDUID == BSMDD_STORE || BSMDDUID == BSMDD_MDSLP || BSMDDUID == BSMDD_SULPY)
                                {
                                    MediTech.DataBase.Prescription presc = new MediTech.DataBase.Prescription();
                                    presc.CUser = userUID;
                                    presc.CWhen = now;

                                    int seqPrescriptionID;
                                    string prescriptionID = SEQHelper.GetSEQIDFormat("SEQPrescription", out seqPrescriptionID);

                                    if (string.IsNullOrEmpty(prescriptionID))
                                    {
                                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No SEQPrescription in SEQCONFIGURATION");
                                    }

                                    if (seqPrescriptionID == 0)
                                    {
                                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Insert SEQPrescription is Fail");
                                    }

                                    if (seqPrescriptionID != 0)
                                    {
                                        if(patientVisit.ENTYPUID == ENTYP_INPAT)
                                        {
                                            prescriptionID = "I" + prescriptionID;
                                        }
                                        else
                                        {
                                            prescriptionID = "O" + prescriptionID;
                                        }
                                    }

                                    presc.PrescriptionNumber = prescriptionID;
                                    presc.PrescribedDttm = now;
                                    presc.PrescribedBy = userUID;
                                    presc.BSMDDUID = BSMDDUID;

                                    presc.ORDSTUID = statusOrder;
                                    presc.PatientUID = patientUID;
                                    presc.PatientVisitUID = patientVisitUID;

                                    presc.PRSTYPUID = OrderType;
                                    presc.MUser = userUID;
                                    presc.MWhen = now;
                                    presc.PatientOrderUID = patientOrder.UID;

                                    presc.StatusFlag = "A";
                                    presc.OwnerOrganisationUID = ownerOrganisationUID;

                                    db.Prescription.Add(presc);
                                    db.SaveChanges();

                                    db.PatientOrder.Attach(patientOrder);
                                    patientOrder.IdentifyingUID = presc.UID;
                                    patientOrder.IdentifyingType = "PRESCRIPTION";
                                    db.SaveChanges();
                                }
                                #endregion

                                #region OrderDetail


                                foreach (var item in dataInOrderDetail)
                                {
                                    PatientOrderDetail orderDetail = new PatientOrderDetail();
                                    orderDetail.CUser = userUID;
                                    orderDetail.CWhen = now;
                                    orderDetail.StartDttm = item.StartDttm;
                                    orderDetail.EndDttm = item.EndDttm;
                                    orderDetail.ORDSTUID = statusOrder;


                                    orderDetail.PatientOrderUID = patientOrder.UID;
                                    orderDetail.MUser = userUID;
                                    orderDetail.MWhen = now;
                                    orderDetail.StatusFlag = "A";
                                    orderDetail.OwnerOrganisationUID = item.OwnerOrganisationUID;
                                    orderDetail.ItemCode = item.ItemCode;
                                    orderDetail.ItemName = item.ItemName;
                                    orderDetail.Dosage = item.Dosage;
                                    orderDetail.Quantity = item.Quantity;
                                    orderDetail.QNUOMUID = item.QNUOMUID;
                                    orderDetail.FRQNCUID = item.FRQNCUID;
                                    orderDetail.UnitPrice = item.UnitPrice;
                                    orderDetail.IsPriceOverwrite = item.IsPriceOverwrite;
                                    orderDetail.OverwritePrice = item.OverwritePrice;
                                    orderDetail.OriginalUnitPrice = item.OriginalUnitPrice;
                                    orderDetail.DoctorFee = item.DoctorFee;
                                    orderDetail.CareproviderUID = item.CareproviderUID;
                                    orderDetail.NetAmount = item.NetAmount;
                                    orderDetail.ROUTEUID = item.ROUTEUID;
                                    orderDetail.DFORMUID = item.DFORMUID;
                                    orderDetail.PDSTSUID = item.PDSTSUID;
                                    orderDetail.DrugDuration = item.DrugDuration;
                                    orderDetail.InstructionText = item.InstructionText;
                                    orderDetail.LocalInstructionText = item.LocalInstructionText;
                                    orderDetail.BillableItemUID = item.BillableItemUID;
                                    orderDetail.IsStockItem = item.IsStock;
                                    orderDetail.StoreUID = item.StoreUID;
                                    orderDetail.Comments = item.Comments;
                                    orderDetail.OrderSetUID = item.OrderSetUID;
                                    orderDetail.OrderSetBillableItemUID = item.OrderSetBillableItemUID;
                                    orderDetail.IdentifyingType = "ORDERITEM";
                                    db.PatientOrderDetail.Add(orderDetail);
                                    db.SaveChanges();

                                    #endregion

                                    #region SavePatinetOrderDetailHistory

                                    PatientOrderDetailHistory patientOrderDetailHistory = new PatientOrderDetailHistory();
                                    patientOrderDetailHistory.PatientOrderDetailUID = orderDetail.UID;
                                    patientOrderDetailHistory.ORDSTUID = orderDetail.ORDSTUID;
                                    patientOrderDetailHistory.EditedDttm = now;
                                    patientOrderDetailHistory.EditByUserID = userUID;
                                    patientOrderDetailHistory.CUser = userUID;
                                    patientOrderDetailHistory.CWhen = now;
                                    patientOrderDetailHistory.MUser = userUID;
                                    patientOrderDetailHistory.MWhen = now;
                                    patientOrderDetailHistory.StatusFlag = "A";
                                    db.PatientOrderDetailHistory.Add(patientOrderDetailHistory);
                                    db.SaveChanges();

                                    #endregion



                                    #region SaveOrderAlert

                                    if (item.PatientOrderAlert != null)
                                    {
                                        foreach (var itemOrderAlert in item.PatientOrderAlert)
                                        {
                                            PatientOrderAlert patOrderAlert = new PatientOrderAlert();
                                            patOrderAlert.PatientOrderDetailUID = orderDetail.UID;
                                            patOrderAlert.AlertType = itemOrderAlert.AlertType;
                                            patOrderAlert.AlertMessage = itemOrderAlert.AlertMessage;
                                            patOrderAlert.AlertMessage = itemOrderAlert.AlertMessage;
                                            patOrderAlert.OverrideByUserUID = userUID;
                                            patOrderAlert.OverrideRemarks = itemOrderAlert.OverrideRemarks;
                                            patOrderAlert.OverrideRSNUID = itemOrderAlert.OverrideRSNUID;
                                            patOrderAlert.CUser = userUID;
                                            patOrderAlert.MUser = userUID;
                                            patOrderAlert.CWhen = now;
                                            patOrderAlert.MWhen = now;
                                            patOrderAlert.StatusFlag = "A";
                                            db.PatientOrderAlert.Add(patOrderAlert);
                                        }

                                        db.SaveChanges();
                                    }

                                    #endregion

                                    #region RequestDetail

                                    if (BSMDDUID == BSMDD_LAB || BSMDDUID == BSMDD_RADIO || BSMDDUID == BSMDD_MBCUP)
                                    {
                                        MediTech.DataBase.RequestItem requestItem = db.RequestItem.Find(item.ItemUID ?? 0);
                                        RequestDetail requestDetail = new RequestDetail();
                                        requestDetail.CUser = userUID;
                                        requestDetail.CWhen = now;
                                        requestDetail.StatusFlag = "A";

                                        if (item.BSMDDUID == BSMDD_RADIO) //Radiology
                                        {
                                            requestDetail.AccessionNumber = (new TechnicalController()).GetAccessionNumber(item.OwnerOrganisationUID);
                                            requestDetail.RIMTYPUID = requestItem.RIMTYPUID;
                                        }




                                        requestDetail.RequestUID = patientOrder.IdentifyingUID.Value;
                                        requestDetail.RequestitemUID = item.ItemUID ?? 0;
                                        requestDetail.RequestedDttm = item.StartDttm ?? now;
                                        requestDetail.ResultRequiredDttm = now;
                                        requestDetail.PatientOrderDetailUID = orderDetail.UID;



                                        requestDetail.ORDSTUID = statusOrder;
                                        requestDetail.RQPRTUID = RQPRTUID;
                                        requestDetail.ORDSTUID = orderDetail.ORDSTUID;
                                        requestDetail.Comments = orderDetail.Comments;
                                        requestDetail.RequestedUserUID = item.CUser;
                                        requestDetail.RequestItemCode = requestItem.Code;
                                        requestDetail.RequestItemName = requestItem.ItemName;
                                        requestDetail.MUser = userUID;
                                        requestDetail.MWhen = now;

                                        requestDetail.OwnerOrganisationUID = item.OwnerOrganisationUID;

                                        db.RequestDetail.Add(requestDetail);
                                        db.SaveChanges();

                                        db.PatientOrderDetail.Attach(orderDetail);
                                        orderDetail.IdentifyingType = "REQUESTDETAIL";
                                        orderDetail.IdentifyingUID = requestDetail.UID;
                                        db.SaveChanges();
                                    }

                                    #endregion


                                    #region PrescrtionItem


                                    if (BSMDDUID == BSMDD_STORE || BSMDDUID == BSMDD_MDSLP || BSMDDUID == BSMDD_SULPY)
                                    {
                                        PrescriptionItem prescritem = new PrescriptionItem();
                                        prescritem.CUser = userUID;
                                        prescritem.CWhen = now;
                                        prescritem.StartDttm = item.StartDttm;
                                        prescritem.ORDSTUID = RAISEDUID;

                                        prescritem.PrescriptionUID = patientOrder.IdentifyingUID.Value;
                                        prescritem.PatientOrderDetailUID = orderDetail.UID;
                                        prescritem.MUser = userUID;
                                        prescritem.MWhen = now;
                                        prescritem.StatusFlag = "A";
                                        prescritem.OwnerOrganisationUID = item.OwnerOrganisationUID;
                                        prescritem.ItemCode = item.ItemCode;
                                        prescritem.ItemName = item.ItemName;
                                        prescritem.ROUTEUID = item.ROUTEUID;
                                        prescritem.FRQNCUID = item.FRQNCUID;
                                        prescritem.DFORMUID = item.DFORMUID;
                                        prescritem.DrugDuration = item.DrugDuration;
                                        prescritem.Dosage = item.Dosage;
                                        prescritem.Quantity = item.Quantity;
                                        prescritem.IMUOMUID = item.QNUOMUID;
                                        prescritem.PDSTSUID = item.PDSTSUID;
                                        prescritem.ItemMasterUID = item.ItemUID;
                                        prescritem.BillableItemUID = item.BillableItemUID;
                                        prescritem.StoreUID = item.StoreUID;
                                        prescritem.ClinicalComments = item.ClinicalComments;
                                        prescritem.InstructionText = item.InstructionText;
                                        prescritem.LocalInstructionText = item.LocalInstructionText;
                                        prescritem.Dosage = item.Dosage;
                                        prescritem.Comments = orderDetail.Comments;
                                        db.PrescriptionItem.Add(prescritem);
                                        db.SaveChanges();

                                        db.PatientOrderDetail.Attach(orderDetail);
                                        orderDetail.IdentifyingUID = prescritem.UID;
                                        orderDetail.IdentifyingType = "PRESCRIPTIONITEM";
                                        db.SaveChanges();
                                    }

                                    #endregion


                                    #region SavePatientBillableItem

                                    PatientBillableItem patBillableItem = new PatientBillableItem();
                                    patBillableItem.PatientUID = patientOrder.PatientUID;
                                    patBillableItem.PatientVisitUID = patientOrder.PatientVisitUID;
                                    patBillableItem.BillableItemUID = orderDetail.BillableItemUID;
                                    patBillableItem.IdentifyingUID = orderDetail.IdentifyingUID ?? 0;
                                    switch (orderDetail.IdentifyingType)
                                    {
                                        case "ORDERITEM":
                                            patBillableItem.IdentifyingType = "ORDERITEM";
                                            break;
                                        case "PRESCRIPTIONITEM":
                                            patBillableItem.IdentifyingType = "STORE";
                                            break;
                                        case "REQUESTDETAIL":
                                            patBillableItem.IdentifyingType = "REQUESTITEM";
                                            break;
                                        default:
                                            patBillableItem.IdentifyingType = orderDetail.IdentifyingType;
                                            break;

                                    }

                                    switch (orderDetail.IdentifyingType)
                                    {
                                        case "ORDERITEM":
                                            patBillableItem.OrderType = "PATIENTORDER";
                                            break;
                                        case "PRESCRIPTIONITEM":
                                            patBillableItem.OrderType = "PRESCRIPTIONITEM";
                                            break;
                                        case "REQUESTDETAIL":
                                            patBillableItem.OrderType = "REQUEST";
                                            break;
                                        default:
                                            patBillableItem.OrderType = "PATIENTORDER";
                                            break;

                                    }
                                    patBillableItem.OrderTypeUID = orderDetail.PatientOrderUID;
                                    patBillableItem.BSMDDUID = BSMDDUID;
                                    patBillableItem.ORDSTUID = orderDetail.ORDSTUID;
                                    patBillableItem.Amount = orderDetail.UnitPrice;
                                    patBillableItem.Discount = orderDetail.Discount;
                                    patBillableItem.NetAmount = orderDetail.NetAmount;
                                    patBillableItem.ItemMultiplier = orderDetail.Quantity;
                                    patBillableItem.StartDttm = orderDetail.StartDttm;
                                    patBillableItem.EndDttm = orderDetail.EndDttm;
                                    patBillableItem.ItemName = orderDetail.ItemName;
                                    patBillableItem.CareProviderUID = orderDetail.CareproviderUID;
                                    patBillableItem.EventOccuredDttm = orderDetail.StartDttm;
                                    patBillableItem.QNUOMUID = orderDetail.QNUOMUID;
                                    patBillableItem.PayorDetailUID = orderDetail.PayorDetailUID;
                                    patBillableItem.StoreUID = orderDetail.StoreUID;
                                    patBillableItem.BillPackageUID = orderDetail.BillPackageUID;
                                    patBillableItem.PatientOrderDetailUID = orderDetail.UID;
                                    patBillableItem.OrderSetUID = orderDetail.OrderSetUID;
                                    patBillableItem.OrderSetBillableItemUID = orderDetail.OrderSetBillableItemUID;
                                    patBillableItem.PatientFixPriceUID = orderDetail.PatientFixPriceUID;
                                    patBillableItem.CUser = userUID;
                                    patBillableItem.CWhen = now;
                                    patBillableItem.MUser = userUID;
                                    patBillableItem.MWhen = now;
                                    patBillableItem.StatusFlag = "A";
                                    patBillableItem.OwnerOrganisationUID = orderDetail.OwnerOrganisationUID;
                                    db.PatientBillableItem.Add(patBillableItem);
                                    db.SaveChanges();
                                    #endregion
                                }
                            }

                        }
                    }




                    tran.Complete();
                }
                return Request.CreateResponse(HttpStatusCode.OK, OrderNumber);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        #region OrderDrugItem

        [Route("GetOrderDrugByPatientUID")]
        public PatientOrderModel GetOrderDrugByPatientUID(long patientUID, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            PatientOrderModel data = null;

            DataTable dt = SqlDirectStore.pGetOrderDrugByPatientUID(patientUID, dateFrom, dateTo);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new PatientOrderModel();
                data.PatientOrderUID = long.Parse(dt.Rows[0]["PatientOrderUID"].ToString());
                data.PatientUID = long.Parse(dt.Rows[0]["PatientUID"].ToString());
                data.PatientVisitUID = long.Parse(dt.Rows[0]["PatientVisitUID"].ToString());
                data.StartDttm = DateTime.Parse(dt.Rows[0]["StartDttm"].ToString());
                data.Comments = dt.Rows[0]["Comments"].ToString();
                data.OrderNumber = dt.Rows[0]["OrderNumber"].ToString();
                data.OrderRaisedBy = int.Parse(dt.Rows[0]["OrderRaisedBy"].ToString());
                data.IdentifyingUID = dt.Rows[0]["IdentifyingUID"].ToString() != "" ? long.Parse(dt.Rows[0]["IdentifyingUID"].ToString()) : (long?)null;
                data.IdentifyingType = dt.Rows[0]["IdentifyingType"].ToString();

                data.PatientOrderDetail = new List<PatientOrderDetailModel>();
                foreach (DataRow item in dt.Rows)
                {

                    PatientOrderDetailModel addItem = new PatientOrderDetailModel();
                    addItem.PatientOrderUID = long.Parse(item["PatientOrderUID"].ToString());
                    addItem.PatientOrderDetailUID = long.Parse(item["PatientOrderDetailUID"].ToString());
                    addItem.ItemCode = item["ItemCode"].ToString();
                    addItem.ItemName = item["ItemName"].ToString();
                    addItem.BillableItemUID = int.Parse(item["BillableItemUID"].ToString());
                    addItem.ItemUID = item["ItemUID"].ToString() != "" ? int.Parse(item["ItemUID"].ToString()) : (int?)null;
                    addItem.BSMDDUID = int.Parse(item["BSMDDUID"].ToString());
                    addItem.BillingService = item["BillingService"].ToString();
                    addItem.IdentifyingUID = item["IdentifyingUID2"].ToString() != "" ? long.Parse(item["IdentifyingUID2"].ToString()) : (long?)null;
                    addItem.IdentifyingType = item["IdentifyingType2"].ToString();
                    addItem.StartDttm = DateTime.Parse(item["StartDttm2"].ToString());
                    addItem.ORDSTUID = int.Parse(item["ORDSTUID2"].ToString());
                    addItem.OrderDetailStatus = item["OrderDetailStatus"].ToString();
                    addItem.Quantity = item["Quantity"].ToString() != "" ? double.Parse(item["Quantity"].ToString()) : (double?)null;
                    addItem.QNUOMUID = item["QNUOMUID"].ToString() != "" ? int.Parse(item["QNUOMUID"].ToString()) : (int?)null;
                    addItem.QuantityUnit = item["QuantityUnit"].ToString();
                    addItem.FRQNCUID = item["FRQNCUID"].ToString() != "" ? int.Parse(item["FRQNCUID"].ToString()) : (int?)null;
                    addItem.DrugFrequency = item["DrugFrequency"].ToString();
                    //addItem.DrugGenaricUID = int.Parse(item["DrugGenaricUID"].ToString());
                    //addItem.GenericName = item["GenericName"].ToString();
                    addItem.UnitPrice = item["UnitPrice"].ToString() != "" ? double.Parse(item["UnitPrice"].ToString()) : (double?)null;
                    addItem.DoctorFee = item["DoctorFee"].ToString() != "" ? double.Parse(item["DoctorFee"].ToString()) : (double?)null;
                    addItem.NetAmount = item["NetAmount"].ToString() != "" ? double.Parse(item["NetAmount"].ToString()) : (double?)null;
                    addItem.ROUTEUID = item["ROUTEUID"].ToString() != "" ? int.Parse(item["ROUTEUID"].ToString()) : (int?)null;
                    addItem.DFORMUID = item["DFORMUID"].ToString() != "" ? int.Parse(item["DFORMUID"].ToString()) : (int?)null;
                    addItem.TypeDrug = item["TypeDrug"].ToString();
                    addItem.PDSTSUID = item["PDSTSUID"].ToString() != "" ? int.Parse(item["PDSTSUID"].ToString()) : (int?)null;
                    addItem.InstructionRoute = item["InstructionRoute"].ToString();
                    addItem.InstructionText = item["InstructionText"].ToString();
                    addItem.DrugDuration = item["DrugDuration"].ToString() != "" ? int.Parse(item["DrugDuration"].ToString()) : (int?)null;
                    addItem.LocalInstructionText = item["LocalInstructionText"].ToString();
                    addItem.IsStock = item["IsStock"].ToString();
                    addItem.StoreUID = item["StoreUID"].ToString() != "" ? int.Parse(item["StoreUID"].ToString()) : (int?)null;
                    addItem.ClinicalComments = item["ClinicalComments"].ToString();
                    data.PatientOrderDetail.Add(addItem);
                }
            }


            return data;
        }

        [Route("GetOrderDrugByVisitUID")]
        public PatientOrderModel GetOrderDrugByVisitUID(long visitUID, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            PatientOrderModel data = null;

            DataTable dt = SqlDirectStore.pGetOrderDrugByVisitUID(visitUID, dateFrom, dateTo);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new PatientOrderModel();
                data.PatientOrderUID = long.Parse(dt.Rows[0]["PatientOrderUID"].ToString());
                data.PatientUID = long.Parse(dt.Rows[0]["PatientUID"].ToString());
                data.PatientVisitUID = long.Parse(dt.Rows[0]["PatientVisitUID"].ToString());
                data.StartDttm = DateTime.Parse(dt.Rows[0]["StartDttm"].ToString());
                data.Comments = dt.Rows[0]["Comments"].ToString();
                data.OrderNumber = dt.Rows[0]["OrderNumber"].ToString();
                data.OrderRaisedBy = int.Parse(dt.Rows[0]["OrderRaisedBy"].ToString());
                data.IdentifyingUID = dt.Rows[0]["IdentifyingUID"].ToString() != "" ? long.Parse(dt.Rows[0]["IdentifyingUID"].ToString()) : (long?)null;
                data.IdentifyingType = dt.Rows[0]["IdentifyingType"].ToString();

                data.PatientOrderDetail = new List<PatientOrderDetailModel>();
                foreach (DataRow item in dt.Rows)
                {

                    PatientOrderDetailModel addItem = new PatientOrderDetailModel();
                    addItem.PatientOrderUID = long.Parse(item["PatientOrderUID"].ToString());
                    addItem.PatientOrderDetailUID = long.Parse(item["PatientOrderDetailUID"].ToString());
                    addItem.ItemCode = item["ItemCode"].ToString();
                    addItem.ItemName = item["ItemName"].ToString();
                    addItem.BillableItemUID = int.Parse(item["BillableItemUID"].ToString());
                    addItem.ItemUID = item["ItemUID"].ToString() != "" ? int.Parse(item["ItemUID"].ToString()) : (int?)null;
                    addItem.BSMDDUID = int.Parse(item["BSMDDUID"].ToString());
                    addItem.BillingService = item["BillingService"].ToString();
                    addItem.IdentifyingUID = item["IdentifyingUID2"].ToString() != "" ? long.Parse(item["IdentifyingUID2"].ToString()) : (long?)null;
                    addItem.IdentifyingType = item["IdentifyingType2"].ToString();
                    addItem.StartDttm = DateTime.Parse(item["StartDttm2"].ToString());
                    addItem.ORDSTUID = int.Parse(item["ORDSTUID2"].ToString());
                    addItem.OrderDetailStatus = item["OrderDetailStatus"].ToString();
                    addItem.Quantity = item["Quantity"].ToString() != "" ? double.Parse(item["Quantity"].ToString()) : (double?)null;
                    addItem.QNUOMUID = item["QNUOMUID"].ToString() != "" ? int.Parse(item["QNUOMUID"].ToString()) : (int?)null;
                    addItem.QuantityUnit = item["QuantityUnit"].ToString();
                    addItem.FRQNCUID = item["FRQNCUID"].ToString() != "" ? int.Parse(item["FRQNCUID"].ToString()) : (int?)null;
                    addItem.DrugFrequency = item["DrugFrequency"].ToString();
                    //addItem.DrugGenaricUID = int.Parse(item["DrugGenaricUID"].ToString());
                    //addItem.GenericName = item["GenericName"].ToString();
                    addItem.UnitPrice = item["UnitPrice"].ToString() != "" ? double.Parse(item["UnitPrice"].ToString()) : (double?)null;
                    addItem.DoctorFee = item["DoctorFee"].ToString() != "" ? double.Parse(item["DoctorFee"].ToString()) : (double?)null;
                    addItem.NetAmount = item["NetAmount"].ToString() != "" ? double.Parse(item["NetAmount"].ToString()) : (double?)null;
                    addItem.ROUTEUID = item["ROUTEUID"].ToString() != "" ? int.Parse(item["ROUTEUID"].ToString()) : (int?)null;
                    addItem.DFORMUID = item["DFORMUID"].ToString() != "" ? int.Parse(item["DFORMUID"].ToString()) : (int?)null;
                    addItem.TypeDrug = item["TypeDrug"].ToString();
                    addItem.PDSTSUID = item["PDSTSUID"].ToString() != "" ? int.Parse(item["PDSTSUID"].ToString()) : (int?)null;
                    addItem.InstructionRoute = item["InstructionRoute"].ToString();
                    addItem.InstructionText = item["InstructionText"].ToString();
                    addItem.DrugDuration = item["DrugDuration"].ToString() != "" ? int.Parse(item["DrugDuration"].ToString()) : (int?)null;
                    addItem.LocalInstructionText = item["LocalInstructionText"].ToString();
                    addItem.IsStock = item["IsStock"].ToString();
                    addItem.StoreUID = item["StoreUID"].ToString() != "" ? int.Parse(item["StoreUID"].ToString()) : (int?)null;
                    addItem.ClinicalComments = item["ClinicalComments"].ToString();
                    data.PatientOrderDetail.Add(addItem);
                }
            }


            return data;
        }

        #endregion

        #region MedicalItem

        [Route("GetOrderMedicalByVisitUID")]
        public PatientOrderModel GetOrderMedicalByVisitUID(long visitUID)
        {
            PatientOrderModel data = null;

            DataTable dt = SqlDirectStore.pGetOrderMedicalByVisitUID(visitUID);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new PatientOrderModel();
                data.PatientOrderUID = long.Parse(dt.Rows[0]["PatientOrderUID"].ToString());
                data.PatientUID = long.Parse(dt.Rows[0]["PatientUID"].ToString());
                data.PatientVisitUID = long.Parse(dt.Rows[0]["PatientVisitUID"].ToString());
                data.StartDttm = DateTime.Parse(dt.Rows[0]["StartDttm"].ToString());
                data.Comments = dt.Rows[0]["Comments"].ToString();
                data.OrderNumber = dt.Rows[0]["OrderNumber"].ToString();
                data.OrderRaisedBy = int.Parse(dt.Rows[0]["OrderRaisedBy"].ToString());
                data.IdentifyingUID = dt.Rows[0]["IdentifyingUID"].ToString() != "" ? long.Parse(dt.Rows[0]["IdentifyingUID"].ToString()) : (long?)null;
                data.IdentifyingType = dt.Rows[0]["IdentifyingType"].ToString();

                data.PatientOrderDetail = new List<PatientOrderDetailModel>();
                foreach (DataRow item in dt.Rows)
                {

                    PatientOrderDetailModel addItem = new PatientOrderDetailModel();
                    addItem.PatientOrderUID = long.Parse(item["PatientOrderUID"].ToString());
                    addItem.PatientOrderDetailUID = long.Parse(item["PatientOrderDetailUID"].ToString());
                    addItem.ItemCode = item["ItemCode"].ToString();
                    addItem.ItemName = item["ItemName"].ToString();
                    addItem.BillableItemUID = int.Parse(item["BillableItemUID"].ToString());
                    addItem.ItemUID = item["ItemUID"].ToString() != "" ? int.Parse(item["ItemUID"].ToString()) : (int?)null;
                    addItem.BSMDDUID = int.Parse(item["BSMDDUID"].ToString());
                    addItem.BillingService = item["BillingService"].ToString();
                    addItem.IdentifyingUID = item["IdentifyingUID2"].ToString() != "" ? long.Parse(item["IdentifyingUID2"].ToString()) : (long?)null;
                    addItem.IdentifyingType = item["IdentifyingType2"].ToString();
                    addItem.StartDttm = DateTime.Parse(item["StartDttm2"].ToString());
                    addItem.ORDSTUID = int.Parse(item["ORDSTUID2"].ToString());
                    addItem.OrderDetailStatus = item["OrderDetailStatus"].ToString();
                    addItem.Quantity = item["Quantity"].ToString() != "" ? double.Parse(item["Quantity"].ToString()) : (double?)null;
                    addItem.QNUOMUID = item["QNUOMUID"].ToString() != "" ? int.Parse(item["QNUOMUID"].ToString()) : (int?)null;
                    addItem.QuantityUnit = item["QuantityUnit"].ToString();

                    addItem.UnitPrice = item["UnitPrice"].ToString() != "" ? double.Parse(item["UnitPrice"].ToString()) : (double?)null;
                    addItem.DoctorFee = item["DoctorFee"].ToString() != "" ? double.Parse(item["DoctorFee"].ToString()) : (double?)null;
                    addItem.NetAmount = item["NetAmount"].ToString() != "" ? double.Parse(item["NetAmount"].ToString()) : (double?)null;
                    addItem.IsPriceOverwrite = item["IsPriceOverwrite"].ToString();
                    addItem.OverwritePrice = item["OverwritePrice"].ToString() != "" ? double.Parse(item["OverwritePrice"].ToString()) : (double?)null;
                    addItem.InstructionText = item["InstructionText"].ToString();
                    addItem.LocalInstructionText = item["LocalInstructionText"].ToString();
                    addItem.IsStock = item["IsStock"].ToString();
                    addItem.StoreUID = item["StoreUID"].ToString() != "" ? int.Parse(item["StoreUID"].ToString()) : (int?)null;
                    data.PatientOrderDetail.Add(addItem);
                }
            }


            return data;
        }

        #endregion

        #region OrderItem

        [Route("Get")]


        [Route("GetOrderItemByVisitUID")]
        public PatientOrderModel GetOrderItemByVisitUID(long visitUID)
        {
            PatientOrderModel data = null;

            DataTable dt = SqlDirectStore.pGetOrderItemByVisitUID(visitUID);

            if (dt != null && dt.Rows.Count > 0)
            {
                data = new PatientOrderModel();
                data.PatientOrderUID = long.Parse(dt.Rows[0]["PatientOrderUID"].ToString());
                data.PatientUID = long.Parse(dt.Rows[0]["PatientUID"].ToString());
                data.PatientVisitUID = long.Parse(dt.Rows[0]["PatientVisitUID"].ToString());
                data.StartDttm = DateTime.Parse(dt.Rows[0]["StartDttm"].ToString());
                data.Comments = dt.Rows[0]["Comments"].ToString();
                data.OrderNumber = dt.Rows[0]["OrderNumber"].ToString();
                data.OrderRaisedBy = int.Parse(dt.Rows[0]["OrderRaisedBy"].ToString());
                data.IdentifyingUID = dt.Rows[0]["IdentifyingUID"].ToString() != "" ? long.Parse(dt.Rows[0]["IdentifyingUID"].ToString()) : (long?)null;
                data.IdentifyingType = dt.Rows[0]["IdentifyingType"].ToString();

                data.PatientOrderDetail = new List<PatientOrderDetailModel>();
                foreach (DataRow item in dt.Rows)
                {
                    PatientOrderDetailModel addItem = new PatientOrderDetailModel();
                    addItem.PatientOrderUID = long.Parse(item["PatientOrderUID"].ToString());
                    addItem.PatientOrderDetailUID = long.Parse(item["PatientOrderDetailUID"].ToString());
                    addItem.ItemCode = item["ItemCode"].ToString();
                    addItem.ItemName = item["ItemName"].ToString();
                    addItem.ItemUID = item["ItemUID"].ToString() != "" ? int.Parse(item["ItemUID"].ToString()) : (int?)null;
                    addItem.BillableItemUID = int.Parse(item["BillableItemUID"].ToString());
                    addItem.BSMDDUID = int.Parse(item["BSMDDUID"].ToString());
                    addItem.BillingService = item["BillingService"].ToString();
                    addItem.IdentifyingUID = item["IdentifyingUID2"].ToString() != "" ? long.Parse(item["IdentifyingUID2"].ToString()) : (long?)null;
                    addItem.IdentifyingType = item["IdentifyingType2"].ToString();
                    addItem.StartDttm = DateTime.Parse(item["StartDttm2"].ToString());
                    addItem.ORDSTUID = int.Parse(item["ORDSTUID2"].ToString());
                    addItem.OrderDetailStatus = item["OrderDetailStatus"].ToString();
                    addItem.Quantity = item["Quantity"].ToString() != "" ? double.Parse(item["Quantity"].ToString()) : (double?)null;
                    addItem.QNUOMUID = item["QNUOMUID"].ToString() != "" ? int.Parse(item["QNUOMUID"].ToString()) : (int?)null;
                    addItem.QuantityUnit = item["QuantityUnit"].ToString();

                    addItem.UnitPrice = item["UnitPrice"].ToString() != "" ? double.Parse(item["UnitPrice"].ToString()) : (double?)null;
                    addItem.DoctorFee = item["DoctorFee"].ToString() != "" ? double.Parse(item["DoctorFee"].ToString()) : (double?)null;
                    addItem.NetAmount = item["NetAmount"].ToString() != "" ? double.Parse(item["NetAmount"].ToString()) : (double?)null;
                    addItem.IsPriceOverwrite = item["IsPriceOverwrite"].ToString();
                    addItem.OverwritePrice = item["OverwritePrice"].ToString() != "" ? double.Parse(item["OverwritePrice"].ToString()) : (double?)null;
                    data.PatientOrderDetail.Add(addItem);
                }

            }
            return data;
        }


        #endregion

        #region RequestItem

        [Route("GetOrderRequestByVisitUID")]
        public List<PatientOrderModel> GetOrderRequestByVisitUID(long visitUID)
        {
            List<PatientOrderModel> data = null;

            DataTable dt = SqlDirectStore.pGetOrderRequestByVisitUID(visitUID);

            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<PatientOrderModel>();
                DataView view = new DataView(dt);
                DataTable distinctValues = view.ToTable(true, "PatientOrderUID", "PatientUID", "PatientVisitUID", "StartDttm", "Comments", "OrderNumber", "BSMDDUID", "BillingService", "ORDSTUID", "OrderRaisedBy", "IdentifyingUID", "IdentifyingType");
                for (int i = 0; i < distinctValues.Rows.Count; i++)
                {
                    PatientOrderModel patientOrder = new PatientOrderModel();
                    patientOrder.PatientOrderUID = long.Parse(distinctValues.Rows[i]["PatientOrderUID"].ToString());
                    patientOrder.PatientUID = long.Parse(distinctValues.Rows[i]["PatientUID"].ToString());
                    patientOrder.PatientVisitUID = long.Parse(distinctValues.Rows[i]["PatientVisitUID"].ToString());
                    patientOrder.StartDttm = DateTime.Parse(distinctValues.Rows[i]["StartDttm"].ToString());
                    patientOrder.Comments = distinctValues.Rows[i]["Comments"].ToString();
                    patientOrder.OrderNumber = distinctValues.Rows[i]["OrderNumber"].ToString();
                    patientOrder.OrderRaisedBy = int.Parse(distinctValues.Rows[i]["OrderRaisedBy"].ToString());
                    patientOrder.BSMDDUID = int.Parse(distinctValues.Rows[i]["BSMDDUID"].ToString());
                    patientOrder.OrderRaisedBy = int.Parse(distinctValues.Rows[i]["OrderRaisedBy"].ToString());
                    patientOrder.IdentifyingUID = distinctValues.Rows[i]["IdentifyingUID"].ToString() != "" ? long.Parse(distinctValues.Rows[0]["IdentifyingUID"].ToString()) : (long?)null;
                    patientOrder.IdentifyingType = distinctValues.Rows[i]["IdentifyingType"].ToString();

                    data.Add(patientOrder);
                }

                if (data != null && data.Count > 0)
                {
                    foreach (var patorder in data)
                    {
                        patorder.PatientOrderDetail = new List<PatientOrderDetailModel>();
                        foreach (DataRow item in dt.Rows)
                        {
                            if (patorder.PatientOrderUID.ToString() == item["PatientOrderUID"].ToString())
                            {
                                PatientOrderDetailModel addItem = new PatientOrderDetailModel();
                                addItem.PatientOrderUID = long.Parse(item["PatientOrderUID"].ToString());
                                addItem.PatientOrderDetailUID = long.Parse(item["PatientOrderDetailUID"].ToString());
                                addItem.ItemCode = item["ItemCode"].ToString();
                                addItem.ItemName = item["ItemName"].ToString();
                                addItem.BillableItemUID = int.Parse(item["BillableItemUID"].ToString());
                                addItem.ItemUID = int.Parse(item["ItemUID"].ToString());
                                addItem.BSMDDUID = int.Parse(item["BSMDDUID"].ToString());
                                addItem.BillingService = item["BillingService"].ToString();
                                addItem.IdentifyingUID = item["IdentifyingUID2"].ToString() != "" ? long.Parse(item["IdentifyingUID2"].ToString()) : (long?)null;
                                addItem.IdentifyingType = item["IdentifyingType2"].ToString();
                                addItem.StartDttm = DateTime.Parse(item["StartDttm2"].ToString());
                                addItem.ORDSTUID = int.Parse(item["ORDSTUID2"].ToString());
                                addItem.OrderDetailStatus = item["OrderDetailStatus"].ToString();
                                addItem.Quantity = item["Quantity"].ToString() != "" ? double.Parse(item["Quantity"].ToString()) : (double?)null;
                                addItem.QNUOMUID = item["QNUOMUID"].ToString() != "" ? int.Parse(item["QNUOMUID"].ToString()) : (int?)null;
                                addItem.QuantityUnit = item["QuantityUnit"].ToString();

                                addItem.UnitPrice = item["UnitPrice"].ToString() != "" ? double.Parse(item["UnitPrice"].ToString()) : (double?)null;
                                addItem.DoctorFee = item["DoctorFee"].ToString() != "" ? double.Parse(item["DoctorFee"].ToString()) : (double?)null;
                                addItem.NetAmount = item["NetAmount"].ToString() != "" ? double.Parse(item["NetAmount"].ToString()) : (double?)null;
                                addItem.IsPriceOverwrite = item["IsPriceOverwrite"].ToString();
                                addItem.OverwritePrice = item["OverwritePrice"].ToString() != "" ? double.Parse(item["OverwritePrice"].ToString()) : (double?)null;

                                patorder.BSMDDUID = addItem.BSMDDUID;
                                patorder.PatientOrderDetail.Add(addItem);
                            }

                        }
                    }

                }


            }
            return data;
        }

        [Route("GetOrderRequestItem")]
        [HttpGet]
        public List<SearchOrderItem> GetOrderRequestItem()
        {

            List<SearchOrderItem> data = (from i in db.BillableItem
                                          where i.StatusFlag == "A"
                                          && i.BSMDDUID == 2841 //Radiology
                                          && (i.ActiveFrom == null || DbFunctions.TruncateTime(DateTime.Now) >= DbFunctions.TruncateTime(i.ActiveFrom))
                                          && (i.ActiveTo == null || DbFunctions.TruncateTime(DateTime.Now) <= DbFunctions.TruncateTime(i.ActiveTo))
                                          select new SearchOrderItem
                                          {
                                              BillableItemUID = i.UID,
                                              Code = i.Code,
                                              ItemName = i.ItemName
                                          }).ToList();

            return data;
        }

        #endregion
    }
}
