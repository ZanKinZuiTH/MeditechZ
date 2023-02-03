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
using System.Xml.Linq;
using System.Web.UI.WebControls;
using static System.Net.Mime.MediaTypeNames;

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
                    addItem.OrderDttm = DateTime.Parse(item["OrderDttm"].ToString());
                    addItem.StartDttm = DateTime.Parse(item["StartDttm"].ToString());
                    addItem.EndDttm = item["EndDttm"].ToString() != "" ? DateTime.Parse(item["EndDttm"].ToString()) : (DateTime?)null;
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
                    addItem.BillPackageUID = item["BillPackageUID"].ToString() != "" ? int.Parse(item["BillPackageUID"].ToString()) : 0;
                    addItem.PackageName = item["PackageName"].ToString();
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
                    addItem.OrderDttm = DateTime.Parse(item["OrderDttm"].ToString());
                    addItem.StartDttm = DateTime.Parse(item["StartDttm"].ToString());
                    addItem.EndDttm = item["EndDttm"].ToString() != "" ? DateTime.Parse(item["EndDttm"].ToString()) : (DateTime?)null;
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
                    addItem.LocationUID = item["LocationUID"].ToString() != "" ? int.Parse(item["LocationUID"].ToString()) : 0;
                    addItem.LocationName = item["LocationName"].ToString();
                    addItem.BillPackageUID = item["BillPackageUID"].ToString() != "" ? int.Parse(item["BillPackageUID"].ToString()) : 0;
                    addItem.PackageName = item["PackageName"].ToString();

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
                        if (dataOrderDetail.IdentifyingType == "DRUG" || dataOrderDetail.IdentifyingType == "MEDICALSUPPLIES" || dataOrderDetail.IdentifyingType == "SUPPLY")
                        {
                            var dataPrescriptionItem = db.PrescriptionItem.FirstOrDefault(p => p.PatientOrderDetailUID == dataOrderDetail.UID && p.StatusFlag == "A");
                            if (dataPrescriptionItem != null)
                            {
                                db.PrescriptionItem.Attach(dataPrescriptionItem);
                                dataPrescriptionItem.ORDSTUID = 2848;
                                dataPrescriptionItem.MUser = userUID;
                                dataPrescriptionItem.MWhen = now;
                                prescriptionUID = dataPrescriptionItem.PrescriptionUID;
                            }
                        }

                        if (dataOrderDetail.IdentifyingType == "REQUESTITEM")
                        {
                            var dataRequestDetail = db.RequestDetail.FirstOrDefault(p => p.PatientOrderDetailUID == dataOrderDetail.UID && p.StatusFlag == "A");

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
                            //item.ORDSTUID = 2848;
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
        public HttpResponseMessage CriteriaOrderAlert(long patientUID, long patientVisitUID, BillableItemModel billItemModel)
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

                DataTable dtDupicate = SqlDirectStore.pCheckOrderDuplicate(patientUID, patientVisitUID, billItemModel.BillableItemUID);
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
                                orderAlert.AlertType = "DuplicateOrder";
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
                DateTime now = DateTime.Now;
                int RAISEDUID = 2847;
                int REGISTUID = 2869;

                var refValue = db.ReferenceValue.Where(p => (p.DomainCode == "BSMDD" || p.DomainCode == "ENTYP" || p.DomainCode == "PRSTYP") && p.StatusFlag == "A");

                int BSMDD_LAB = refValue.FirstOrDefault(p => p.DomainCode == "BSMDD" && p.ValueCode == "LABBB").UID;
                int BSMDD_RADIO = refValue.FirstOrDefault(p => p.DomainCode == "BSMDD" && p.ValueCode == "RADIO").UID;
                int BSMDD_MBCUP = refValue.FirstOrDefault(p => p.DomainCode == "BSMDD" && p.ValueCode == "MBCUP").UID;
                int BSMDD_STORE = refValue.FirstOrDefault(p => p.DomainCode == "BSMDD" && p.ValueCode == "STORE").UID;
                //int BSMDD_ORDITEM = 2839;
                int BSMDD_MDSLP = refValue.FirstOrDefault(p => p.DomainCode == "BSMDD" && p.ValueCode == "MDSLP").UID;
                int BSMDD_SULPY = refValue.FirstOrDefault(p => p.DomainCode == "BSMDD" && p.ValueCode == "SUPLY").UID;

                int ENTYP_INPAT = refValue.FirstOrDefault(p => p.DomainCode == "ENTYP" && p.ValueCode == "INPAT").UID;

                int PRST_STAN = refValue.FirstOrDefault(p => p.DomainCode == "PRSTYP" && p.ValueCode == "STORD").UID;

                int RQPRTUID = 440; //Priority Normal
                int statusOrder;

                var patientVisit = db.PatientVisit.Find(patientVisitUID);
                using (var tran = new TransactionScope())
                {
                    IEnumerable<int> groupBSMDD = orderDetails.Select(p => p.BSMDDUID).Distinct();
                    IEnumerable<int> groupPriority = orderDetails.Select(p => p.ORDPRUID ?? RQPRTUID).Distinct();
                    IEnumerable<int> groupOrderType = orderDetails.Select(p => p.PRSTYPUID ?? 0).Distinct();
                    IEnumerable<int> groupRouteToStore = orderDetails.Select(p => p.StoreUID ?? 0).Distinct();
                    IEnumerable<string> isContinuousTypes = orderDetails.Select(p => p.IsContinuous ?? "N").Distinct();
                    foreach (var priority in groupPriority)
                    {
                        foreach (var OrderType in groupOrderType)
                        {
                            foreach (int BSMDDUID in groupBSMDD)
                            {
                                foreach (var store in groupRouteToStore)
                                {

                                    foreach (string isContinuous in isContinuousTypes)
                                    {
                                        var dataInOrderDetail = orderDetails.Where(p => p.BSMDDUID == BSMDDUID && p.ORDPRUID == priority && p.PRSTYPUID == OrderType && (p.StoreUID ?? 0) == store && (p.IsContinuous ?? "N") == isContinuous);
                                        List<PatientOrderDetailModel> stadingOrderLists = new List<PatientOrderDetailModel>();
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
                                            patientOrder.EndDttm = isContinuous == "Y" ? dataInOrderDetail.Max(p => p.EndDttm) : now;

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
                                            patientOrder.IsContinuous = OrderType == PRST_STAN ? "Y" : "N";
                                            patientOrder.MUser = userUID;
                                            patientOrder.MWhen = now;
                                            patientOrder.StatusFlag = "A";
                                            patientOrder.PatientUID = patientUID;
                                            patientOrder.PatientVisitUID = patientVisitUID;
                                            patientOrder.OrderLocationUID = locationUID;
                                            patientOrder.OrderToLocationStoreUID = store != 0 ? store : (int?)null;
                                            patientOrder.OrderCategoryUID = dataInOrderDetail.FirstOrDefault()?.OrderCatagoryUID;
                                            patientOrder.OwnerOrganisationUID = ownerOrganisationUID;
                                            patientOrder.IdentifyingType = (BSMDDUID == BSMDD_STORE || BSMDDUID == BSMDD_MDSLP || BSMDDUID == BSMDD_SULPY) ? "PRESCRIPTION" : "PATIENTORDER";
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


                                                request.RQPRTUID = priority;


                                                request.PatientUID = patientUID;
                                                request.PatientVisitUID = patientVisitUID;
                                                request.PatientOrderUID = patientOrder.UID;

                                                request.MUser = userUID;
                                                request.MWhen = now;
                                                request.OrderLocationUID = locationUID;
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
                                            if ((BSMDDUID == BSMDD_STORE || BSMDDUID == BSMDD_MDSLP || BSMDDUID == BSMDD_SULPY) && isContinuous != "Y")
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
                                                    if (patientVisit.ENTYPUID == ENTYP_INPAT)
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
                                                presc.OrderLocationUID = locationUID;
                                                presc.OrderToLocationStoreUID = store;
                                                presc.OwnerOrganisationUID = ownerOrganisationUID;

                                                db.Prescription.Add(presc);
                                                db.SaveChanges();

                                                db.PatientOrder.Attach(patientOrder);
                                                patientOrder.IdentifyingUID = presc.UID;
                                                patientOrder.IdentifyingType = "PRESCRIPTION";
                                                db.SaveChanges();
                                            }
                                            #endregion


                                            foreach (var item in dataInOrderDetail)
                                            {
                                                #region OrderDetail
                                                string identifyingType = (BSMDDUID == BSMDD_LAB || BSMDDUID == BSMDD_RADIO || BSMDDUID == BSMDD_MBCUP) ? "REQUESTITEM" : BSMDDUID == BSMDD_STORE ? "DRUG" : BSMDDUID == BSMDD_MDSLP ? "MEDICALSUPPLIES" : BSMDDUID == BSMDD_SULPY ? "SUPPLY" : "ORDERITEM";

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
                                                orderDetail.OrderCategoryUID = item.OrderCatagoryUID;
                                                orderDetail.OrderSubCategoryUID = item.OrderSubCategoryUID;
                                                orderDetail.IsStockItem = item.IsStock;
                                                orderDetail.StoreUID = item.StoreUID;
                                                orderDetail.Comments = item.Comments;
                                                orderDetail.OrderSetUID = item.OrderSetUID;
                                                orderDetail.OrderSetBillableItemUID = item.OrderSetBillableItemUID;
                                                orderDetail.IdentifyingType = identifyingType;
                                                orderDetail.IdentifyingUID = item.ItemUID;
                                                orderDetail.IsStandingOrder = item.IsStandingOrder;
                                                orderDetail.BillPackageUID = item.BillPackageUID;
                                                orderDetail.PatientPackageUID = item.PatientPackageUID;
                                                orderDetail.PatientPackageItemUID = item.PatientPackageItemUID;
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

                                                long? requestDetailUID = null;
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
                                                    requestDetail.RQPRTUID = priority;
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

                                                    //db.PatientOrderDetail.Attach(orderDetail);
                                                    //orderDetail.IdentifyingType = "REQUESTITEM";
                                                    //orderDetail.IdentifyingUID = requestItem.UID;
                                                    //db.SaveChanges();

                                                    requestDetailUID = requestDetail.UID;
                                                }

                                                #endregion

                                                #region PrescrtionItem

                                                long? prescrtionItemUID = null;
                                                if ((BSMDDUID == BSMDD_STORE || BSMDDUID == BSMDD_MDSLP || BSMDDUID == BSMDD_SULPY) && isContinuous != "Y")
                                                {
                                                    //string identifyingType = BSMDDUID == BSMDD_STORE ? "DRUG" : BSMDDUID == BSMDD_MDSLP ? "MEDICALSUPPLIES" : BSMDDUID == BSMDD_SULPY ? "SUPPLY" : "ORDERITEM";

                                                    PrescriptionItem prescrItem = new PrescriptionItem();
                                                    prescrItem.CUser = userUID;
                                                    prescrItem.CWhen = now;
                                                    prescrItem.StartDttm = item.StartDttm;
                                                    prescrItem.ORDSTUID = RAISEDUID;

                                                    prescrItem.PrescriptionUID = patientOrder.IdentifyingUID.Value;
                                                    prescrItem.PatientOrderDetailUID = orderDetail.UID;
                                                    prescrItem.MUser = userUID;
                                                    prescrItem.MWhen = now;
                                                    prescrItem.StatusFlag = "A";
                                                    prescrItem.OwnerOrganisationUID = item.OwnerOrganisationUID;
                                                    prescrItem.ItemCode = item.ItemCode;
                                                    prescrItem.ItemName = item.ItemName;
                                                    prescrItem.ROUTEUID = item.ROUTEUID;
                                                    prescrItem.FRQNCUID = item.FRQNCUID;
                                                    prescrItem.DFORMUID = item.DFORMUID;
                                                    prescrItem.DrugDuration = item.DrugDuration;
                                                    prescrItem.Dosage = item.Dosage;
                                                    prescrItem.Quantity = item.Quantity;
                                                    prescrItem.IMUOMUID = item.QNUOMUID;
                                                    prescrItem.PDSTSUID = item.PDSTSUID;
                                                    prescrItem.ItemMasterUID = item.ItemUID;
                                                    prescrItem.BillableItemUID = item.BillableItemUID;
                                                    prescrItem.StoreUID = item.StoreUID;
                                                    prescrItem.ClinicalComments = item.ClinicalComments;
                                                    prescrItem.InstructionText = item.InstructionText;
                                                    prescrItem.LocalInstructionText = item.LocalInstructionText;
                                                    prescrItem.Dosage = item.Dosage;
                                                    prescrItem.Comments = orderDetail.Comments;
                                                    db.PrescriptionItem.Add(prescrItem);
                                                    db.SaveChanges();

                                                    //db.PatientOrderDetail.Attach(orderDetail);
                                                    //orderDetail.IdentifyingUID = item.ItemUID;
                                                    //orderDetail.IdentifyingType = identifyingType;
                                                    //db.SaveChanges();

                                                    prescrtionItemUID = prescrItem.UID;
                                                }

                                                #endregion

                                                #region SavePatientBillableItem

                                                BillableItem billableItem = db.BillableItem.Find(orderDetail.BillableItemUID);
                                                PatientBillableItem patBillableItem = new PatientBillableItem();
                                                patBillableItem.PatientUID = patientOrder.PatientUID;
                                                patBillableItem.PatientVisitUID = patientOrder.PatientVisitUID;
                                                patBillableItem.BillableItemUID = orderDetail.BillableItemUID;
                                                patBillableItem.IdentifyingUID = orderDetail.IdentifyingUID ?? 0;
                                                switch (orderDetail.IdentifyingType)
                                                {
                                                    case "DRUG":
                                                    case "MEDICALSUPPLIES":
                                                    case "SUPPLY":
                                                        patBillableItem.IdentifyingType = "STORE";
                                                        break;
                                                    default:
                                                        patBillableItem.IdentifyingType = orderDetail.IdentifyingType;
                                                        break;

                                                }

                                                switch (orderDetail.IdentifyingType)
                                                {
                                                    case "ORDERITEM":
                                                        patBillableItem.OrderType = "PATIENTORDER";
                                                        patBillableItem.OrderTypeUID = orderDetail.PatientOrderUID;
                                                        break;
                                                    case "DRUG":
                                                    case "MEDICALSUPPLIES":
                                                    case "SUPPLY":
                                                        patBillableItem.OrderType = "PRESCRIPTIONITEM";
                                                        patBillableItem.OrderTypeUID = prescrtionItemUID;
                                                        break;
                                                    case "REQUESTITEM":
                                                        patBillableItem.OrderType = "REQUESTDETAIL";
                                                        patBillableItem.OrderTypeUID = requestDetailUID;
                                                        break;
                                                    default:
                                                        patBillableItem.OrderType = "PATIENTORDER";
                                                        patBillableItem.OrderTypeUID = orderDetail.PatientOrderUID;
                                                        break;

                                                }

                                                patBillableItem.BSMDDUID = BSMDDUID;
                                                patBillableItem.GroupUID = billableItem?.BillingGroupUID;
                                                patBillableItem.SubGroupUID = billableItem?.BillingSubGroupUID;
                                                //patBillableItem.ORDSTUID = orderDetail.ORDSTUID;
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
                                                patBillableItem.PatientOrderDetailUID = orderDetail.UID;
                                                patBillableItem.OrderSetUID = orderDetail.OrderSetUID;
                                                patBillableItem.OrderSetBillableItemUID = orderDetail.OrderSetBillableItemUID;
                                                patBillableItem.PatientFixPriceUID = orderDetail.PatientFixPriceUID;
                                                patBillableItem.BillPackageUID = orderDetail.PatientPackageUID;
                                                patBillableItem.CUser = userUID;
                                                patBillableItem.CWhen = now;
                                                patBillableItem.MUser = userUID;
                                                patBillableItem.MWhen = now;
                                                patBillableItem.StatusFlag = "A";
                                                patBillableItem.OwnerOrganisationUID = orderDetail.OwnerOrganisationUID;
                                                db.PatientBillableItem.Add(patBillableItem);
                                                db.SaveChanges();
                                                #endregion


                                                #region AddStadingOrderChild

                                                if (item.StandingPatientOrder != null && isContinuous == "Y")
                                                {
                                                    item.StandingPatientOrder.ParentUID = orderDetail.UID;
                                                    item.StandingPatientOrder.ORDSTUID = orderDetail.ORDSTUID;
                                                    stadingOrderLists.Add(item.StandingPatientOrder);
                                                }

                                                #endregion
                                            }


                                            #region GenStadingOrder
                                            if (stadingOrderLists != null && stadingOrderLists.Count > 0)
                                            {
                                                #region PatientOrderStading
                                                PatientOrder patientOrderStading = new PatientOrder();
                                                patientOrderStading.CUser = userUID;
                                                patientOrderStading.CWhen = now;

                                                int seqPatientOrderStadingID;
                                                string patientOrderStadingID = SEQHelper.GetSEQIDFormat("SEQPatientOrder", out seqPatientOrderStadingID);

                                                if (string.IsNullOrEmpty(patientOrderStadingID))
                                                {
                                                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No SEQPatientOrder in SEQCONFIGURATION");
                                                }

                                                if (seqPatientOrderID == 0)
                                                {
                                                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Insert SEQPatientOrder is Fail");
                                                }

                                                patientOrderStading.ParentUID = patientOrder.UID;
                                                patientOrderStading.OrderNumber = patientOrderStadingID;
                                                patientOrderStading.StartDttm = now;
                                                patientOrderStading.EndDttm = now;

                                                patientOrderStading.PRSTYPUID = OrderType;
                                                patientOrderStading.OrderRaisedBy = userUID;
                                                patientOrderStading.IsContinuous = "N";
                                                patientOrderStading.MUser = userUID;
                                                patientOrderStading.MWhen = now;
                                                patientOrderStading.StatusFlag = "A";
                                                patientOrderStading.PatientUID = patientUID;
                                                patientOrderStading.PatientVisitUID = patientVisitUID;
                                                patientOrderStading.OrderLocationUID = locationUID;
                                                patientOrderStading.OrderCategoryUID = stadingOrderLists.FirstOrDefault()?.OrderCatagoryUID;
                                                patientOrderStading.OwnerOrganisationUID = ownerOrganisationUID;
                                                patientOrderStading.IdentifyingType = (BSMDDUID == BSMDD_STORE || BSMDDUID == BSMDD_MDSLP || BSMDDUID == BSMDD_SULPY) ? "PRESCRIPTION" : "PATIENTORDER";
                                                db.PatientOrder.Add(patientOrderStading);

                                                db.SaveChanges();

                                                #endregion

                                                #region Presction
                                                MediTech.DataBase.Prescription prescStading = new MediTech.DataBase.Prescription();
                                                prescStading.CUser = userUID;
                                                prescStading.CWhen = now;

                                                int seqPrescriptionStadingID;
                                                string prescriptionStadingID = SEQHelper.GetSEQIDFormat("SEQPrescription", out seqPrescriptionStadingID);

                                                if (string.IsNullOrEmpty(prescriptionStadingID))
                                                {
                                                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No SEQPrescription in SEQCONFIGURATION");
                                                }

                                                if (seqPrescriptionStadingID == 0)
                                                {
                                                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Insert SEQPrescription is Fail");
                                                }

                                                if (seqPrescriptionStadingID != 0)
                                                {
                                                    if (patientVisit.ENTYPUID == ENTYP_INPAT)
                                                    {
                                                        prescriptionStadingID = "I" + prescriptionStadingID;
                                                    }
                                                    else
                                                    {
                                                        prescriptionStadingID = "O" + prescriptionStadingID;
                                                    }
                                                }

                                                prescStading.PrescriptionNumber = prescriptionStadingID;
                                                prescStading.PrescribedDttm = now;
                                                prescStading.PrescribedBy = userUID;
                                                prescStading.BSMDDUID = BSMDDUID;

                                                prescStading.ORDSTUID = statusOrder;
                                                prescStading.PatientUID = patientUID;
                                                prescStading.PatientVisitUID = patientVisitUID;

                                                prescStading.PRSTYPUID = OrderType;
                                                prescStading.MUser = userUID;
                                                prescStading.MWhen = now;
                                                prescStading.PatientOrderUID = patientOrder.UID;

                                                prescStading.StatusFlag = "A";
                                                prescStading.OrderLocationUID = locationUID;
                                                prescStading.OwnerOrganisationUID = ownerOrganisationUID;

                                                db.Prescription.Add(prescStading);
                                                db.SaveChanges();

                                                db.PatientOrder.Attach(patientOrderStading);
                                                patientOrderStading.IdentifyingUID = prescStading.UID;
                                                patientOrderStading.IdentifyingType = "PRESCRIPTION";
                                                db.SaveChanges();
                                                #endregion

                                                foreach (var orderStadingDetail in stadingOrderLists)
                                                {
                                                    #region PatientOrderDetailStading
                                                    string identifyingType = (BSMDDUID == BSMDD_LAB || BSMDDUID == BSMDD_RADIO) ? "REQUESTITEM" : BSMDDUID == BSMDD_STORE ? "DRUG" : BSMDDUID == BSMDD_MDSLP ? "MEDICALSUPPLIES" : BSMDDUID == BSMDD_SULPY ? "SUPPLY" : "ORDERITEM";

                                                    PatientOrderDetail orderDetailStading = new PatientOrderDetail();
                                                    orderDetailStading.ParentUID = orderStadingDetail.ParentUID;
                                                    orderDetailStading.PatientOrderUID = patientOrderStading.UID;

                                                    orderDetailStading.CUser = userUID;
                                                    orderDetailStading.CWhen = now;
                                                    orderDetailStading.StartDttm = orderStadingDetail.StartDttm;
                                                    orderDetailStading.EndDttm = orderStadingDetail.EndDttm;

                                                    orderDetailStading.ORDSTUID = orderStadingDetail.ORDSTUID;

                                                    orderDetailStading.MUser = userUID;
                                                    orderDetailStading.MWhen = now;
                                                    orderDetailStading.StatusFlag = "A";
                                                    orderDetailStading.OwnerOrganisationUID = orderStadingDetail.OwnerOrganisationUID;
                                                    orderDetailStading.ItemCode = orderStadingDetail.ItemCode;
                                                    orderDetailStading.ItemName = orderStadingDetail.ItemName;
                                                    orderDetailStading.Dosage = orderStadingDetail.Dosage;
                                                    orderDetailStading.Quantity = orderStadingDetail.Quantity;
                                                    orderDetailStading.QNUOMUID = orderStadingDetail.QNUOMUID;
                                                    orderDetailStading.FRQNCUID = orderStadingDetail.FRQNCUID;
                                                    orderDetailStading.UnitPrice = orderStadingDetail.UnitPrice;
                                                    orderDetailStading.IsPriceOverwrite = orderStadingDetail.IsPriceOverwrite;
                                                    orderDetailStading.OverwritePrice = orderStadingDetail.OverwritePrice;
                                                    orderDetailStading.OriginalUnitPrice = orderStadingDetail.OriginalUnitPrice;
                                                    orderDetailStading.DoctorFee = orderStadingDetail.DoctorFee;
                                                    orderDetailStading.CareproviderUID = orderStadingDetail.CareproviderUID;
                                                    orderDetailStading.NetAmount = orderStadingDetail.NetAmount;
                                                    orderDetailStading.ROUTEUID = orderStadingDetail.ROUTEUID;
                                                    orderDetailStading.DFORMUID = orderStadingDetail.DFORMUID;
                                                    orderDetailStading.PDSTSUID = orderStadingDetail.PDSTSUID;
                                                    orderDetailStading.DrugDuration = orderStadingDetail.DrugDuration;
                                                    orderDetailStading.InstructionText = orderStadingDetail.InstructionText;
                                                    orderDetailStading.LocalInstructionText = orderStadingDetail.LocalInstructionText;
                                                    orderDetailStading.BillableItemUID = orderStadingDetail.BillableItemUID;
                                                    orderDetailStading.OrderCategoryUID = orderStadingDetail.OrderCatagoryUID;
                                                    orderDetailStading.OrderSubCategoryUID = orderStadingDetail.OrderSubCategoryUID;
                                                    orderDetailStading.IsStockItem = orderStadingDetail.IsStock;
                                                    orderDetailStading.StoreUID = orderStadingDetail.StoreUID;
                                                    orderDetailStading.Comments = orderStadingDetail.Comments;
                                                    orderDetailStading.OrderSetUID = orderStadingDetail.OrderSetUID;
                                                    orderDetailStading.OrderSetBillableItemUID = orderStadingDetail.OrderSetBillableItemUID;
                                                    orderDetailStading.IdentifyingType = identifyingType;
                                                    orderDetailStading.IdentifyingUID = orderStadingDetail.ItemUID;
                                                    orderDetailStading.IsStandingOrder = orderStadingDetail.IsStandingOrder;
                                                    orderDetailStading.BillPackageUID = orderStadingDetail.BillPackageUID;
                                                    orderDetailStading.PatientPackageUID = orderStadingDetail.PatientPackageUID;
                                                    orderDetailStading.PatientPackageItemUID = orderStadingDetail.PatientPackageItemUID;
                                                    db.PatientOrderDetail.Add(orderDetailStading);
                                                    db.SaveChanges();
                                                    #endregion

                                                    #region SavePatinetOrderDetailHistory

                                                    PatientOrderDetailHistory patientOrderStadingDetailHistory = new PatientOrderDetailHistory();
                                                    patientOrderStadingDetailHistory.PatientOrderDetailUID = orderDetailStading.UID;
                                                    patientOrderStadingDetailHistory.ORDSTUID = orderDetailStading.ORDSTUID;
                                                    patientOrderStadingDetailHistory.EditedDttm = now;
                                                    patientOrderStadingDetailHistory.EditByUserID = userUID;
                                                    patientOrderStadingDetailHistory.CUser = userUID;
                                                    patientOrderStadingDetailHistory.CWhen = now;
                                                    patientOrderStadingDetailHistory.MUser = userUID;
                                                    patientOrderStadingDetailHistory.MWhen = now;
                                                    patientOrderStadingDetailHistory.StatusFlag = "A";
                                                    db.PatientOrderDetailHistory.Add(patientOrderStadingDetailHistory);
                                                    db.SaveChanges();

                                                    #endregion



                                                    #region PrescrtionItem

                                                    long? prescrtionItemStadingUID = null;
                                                    //string identifyingType = BSMDDUID == BSMDD_STORE ? "DRUG" : BSMDDUID == BSMDD_MDSLP ? "MEDICALSUPPLIES" : BSMDDUID == BSMDD_SULPY ? "SUPPLY" : "ORDERITEM";

                                                    PrescriptionItem prescrItemStading = new PrescriptionItem();
                                                    prescrItemStading.CUser = userUID;
                                                    prescrItemStading.CWhen = now;
                                                    prescrItemStading.StartDttm = orderStadingDetail.StartDttm;
                                                    prescrItemStading.ORDSTUID = RAISEDUID;

                                                    prescrItemStading.PrescriptionUID = patientOrderStading.IdentifyingUID.Value;
                                                    prescrItemStading.PatientOrderDetailUID = orderDetailStading.UID;
                                                    prescrItemStading.MUser = userUID;
                                                    prescrItemStading.MWhen = now;
                                                    prescrItemStading.StatusFlag = "A";
                                                    prescrItemStading.OwnerOrganisationUID = orderStadingDetail.OwnerOrganisationUID;
                                                    prescrItemStading.ItemCode = orderStadingDetail.ItemCode;
                                                    prescrItemStading.ItemName = orderStadingDetail.ItemName;
                                                    prescrItemStading.ROUTEUID = orderStadingDetail.ROUTEUID;
                                                    prescrItemStading.FRQNCUID = orderStadingDetail.FRQNCUID;
                                                    prescrItemStading.DFORMUID = orderStadingDetail.DFORMUID;
                                                    prescrItemStading.DrugDuration = orderStadingDetail.DrugDuration;
                                                    prescrItemStading.Dosage = orderStadingDetail.Dosage;
                                                    prescrItemStading.Quantity = orderStadingDetail.Quantity;
                                                    prescrItemStading.IMUOMUID = orderStadingDetail.QNUOMUID;
                                                    prescrItemStading.PDSTSUID = orderStadingDetail.PDSTSUID;
                                                    prescrItemStading.ItemMasterUID = orderStadingDetail.ItemUID;
                                                    prescrItemStading.BillableItemUID = orderStadingDetail.BillableItemUID;
                                                    prescrItemStading.StoreUID = orderStadingDetail.StoreUID;
                                                    prescrItemStading.ClinicalComments = orderStadingDetail.ClinicalComments;
                                                    prescrItemStading.InstructionText = orderStadingDetail.InstructionText;
                                                    prescrItemStading.LocalInstructionText = orderStadingDetail.LocalInstructionText;
                                                    prescrItemStading.Dosage = orderStadingDetail.Dosage;
                                                    prescrItemStading.Comments = orderStadingDetail.Comments;
                                                    db.PrescriptionItem.Add(prescrItemStading);
                                                    db.SaveChanges();

                                                    //db.PatientOrderDetail.Attach(orderDetailStading);
                                                    //orderDetailStading.IdentifyingUID = orderStadingDetail.ItemUID;
                                                    //orderDetailStading.IdentifyingType = identifyingType;
                                                    //db.SaveChanges();

                                                    prescrtionItemStadingUID = prescrItemStading.UID;


                                                    #endregion

                                                    #region SavePatientBillableItem

                                                    PatientBillableItem patBillableItemStading = new PatientBillableItem();
                                                    patBillableItemStading.PatientUID = patientOrderStading.PatientUID;
                                                    patBillableItemStading.PatientVisitUID = patientOrderStading.PatientVisitUID;
                                                    patBillableItemStading.BillableItemUID = orderDetailStading.BillableItemUID;
                                                    patBillableItemStading.IdentifyingUID = orderDetailStading.IdentifyingUID ?? 0;
                                                    switch (orderDetailStading.IdentifyingType)
                                                    {
                                                        case "DRUG":
                                                        case "MEDICALSUPPLIES":
                                                        case "SUPPLY":
                                                            patBillableItemStading.IdentifyingType = "STORE";
                                                            break;
                                                        default:
                                                            patBillableItemStading.IdentifyingType = orderDetailStading.IdentifyingType;
                                                            break;

                                                    }

                                                    switch (orderDetailStading.IdentifyingType)
                                                    {
                                                        case "ORDERITEM":
                                                            patBillableItemStading.OrderType = "PATIENTORDER";
                                                            patBillableItemStading.OrderTypeUID = orderDetailStading.PatientOrderUID;
                                                            break;
                                                        case "DRUG":
                                                        case "MEDICALSUPPLIES":
                                                        case "SUPPLY":
                                                            patBillableItemStading.OrderType = "PRESCRIPTIONITEM";
                                                            patBillableItemStading.OrderTypeUID = prescrtionItemStadingUID;
                                                            break;
                                                        default:
                                                            patBillableItemStading.OrderType = "PATIENTORDER";
                                                            patBillableItemStading.OrderTypeUID = orderDetailStading.PatientOrderUID;
                                                            break;

                                                    }

                                                    patBillableItemStading.BSMDDUID = BSMDDUID;
                                                    patBillableItemStading.Amount = orderDetailStading.UnitPrice;
                                                    patBillableItemStading.Discount = orderDetailStading.Discount;
                                                    patBillableItemStading.NetAmount = orderDetailStading.NetAmount;
                                                    patBillableItemStading.ItemMultiplier = orderDetailStading.Quantity;
                                                    patBillableItemStading.StartDttm = orderDetailStading.StartDttm;
                                                    patBillableItemStading.EndDttm = orderDetailStading.EndDttm;
                                                    patBillableItemStading.ItemName = orderDetailStading.ItemName;
                                                    patBillableItemStading.CareProviderUID = orderDetailStading.CareproviderUID;
                                                    patBillableItemStading.EventOccuredDttm = orderDetailStading.StartDttm;
                                                    patBillableItemStading.QNUOMUID = orderDetailStading.QNUOMUID;
                                                    patBillableItemStading.PayorDetailUID = orderDetailStading.PayorDetailUID;
                                                    patBillableItemStading.StoreUID = orderDetailStading.StoreUID;
                                                    patBillableItemStading.PatientOrderDetailUID = orderDetailStading.UID;
                                                    patBillableItemStading.OrderSetUID = orderDetailStading.OrderSetUID;
                                                    patBillableItemStading.OrderSetBillableItemUID = orderDetailStading.OrderSetBillableItemUID;
                                                    patBillableItemStading.PatientFixPriceUID = orderDetailStading.PatientFixPriceUID;
                                                    patBillableItemStading.BillPackageUID = orderDetailStading.PatientPackageUID;
                                                    patBillableItemStading.CUser = userUID;
                                                    patBillableItemStading.CWhen = now;
                                                    patBillableItemStading.MUser = userUID;
                                                    patBillableItemStading.MWhen = now;
                                                    patBillableItemStading.StatusFlag = "A";
                                                    patBillableItemStading.OwnerOrganisationUID = orderDetailStading.OwnerOrganisationUID;
                                                    db.PatientBillableItem.Add(patBillableItemStading);
                                                    db.SaveChanges();

                                                    #endregion
                                                }
                                            }

                                            #endregion

                                        }
                                    }
                                }



                            }
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

        [Route("ClosureStandingOrder")]
        [HttpPut]
        public HttpResponseMessage ClosureStandingOrder(string patientOrderDetailUIDs, int userUID, DateTime endDttm, string comments)
        {
            try
            {
                SqlDirectStore.pHourlyComplete(patientOrderDetailUIDs, userUID, endDttm, comments);
                return Request.CreateResponse(HttpStatusCode.OK);
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


        #region Package
        [Route("SearchBillPackage")]
        [HttpGet]
        public List<BillPackageModel> SearchBillPackage(string text, int? orderCategoryUID, int? orderSubCategoryUID)
        {
            List<BillPackageModel> data = SqlDirectStore.pSearchBillPackageItem(text, orderCategoryUID, orderSubCategoryUID).ToList<BillPackageModel>();

            return data;
        }

        [Route("AddPatientPackage")]
        [HttpPost]
        public HttpResponseMessage AddPatientPackage(PatientPackageModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            try
            {
                DateTime now = DateTime.Now;

                using (var tran = new TransactionScope())
                {


                    PatientPackage patPackage = new PatientPackage();
                    patPackage.PatientUID = model.PatientUID;
                    patPackage.PatientVisitUID = model.PatientVisitUID;
                    patPackage.PackageName = model.PackageName;
                    patPackage.BillPackageUID = model.BillPackageUID;
                    patPackage.TotalAmount = model.TotalAmount;
                    patPackage.ActiveFrom = now;
                    patPackage.ActiveTo = model.ActiveTo;
                    patPackage.PackageCreatedByUID = model.PackageCreatedByUID;
                    patPackage.PackageCreatedDttm = now;
                    patPackage.Comments = model.Comments;
                    patPackage.BillingGroupUID = model.BillingGroupUID;
                    patPackage.BillingSubGroupUID = model.BillingSubGroupUID;
                    patPackage.IsConsiderItem = model.IsConsiderItem;
                    patPackage.Qty = model.Qty;
                    patPackage.OwnerOrganisationUID = model.OwnerOrganisationUID;
                    patPackage.StatusFlag = "A";
                    patPackage.MUser = model.MUser;
                    patPackage.CUser = model.CUser;
                    patPackage.MWhen = now;
                    patPackage.CWhen = now;

                    db.PatientPackage.Add(patPackage);

                    db.SaveChanges();

                    IEnumerable<BillPackageItem> billPackageItems = db.BillPackageItem.Where(p => p.BillPackageUID == model.BillPackageUID && p.StatusFlag == "A");

                    foreach (var item in billPackageItems)
                    {
                        BillableItem billableItem = new BillableItem();

                        PatientPackageItem patPackageItem = new PatientPackageItem();
                        patPackageItem.PatientPackageUID = patPackage.UID;
                        patPackageItem.BillableItemUID = item.BillableItemUID ?? 0;
                        patPackageItem.BSMDDUID = item.BSMDDUID;
                        patPackageItem.ItemName = billableItem.ItemName;
                        patPackageItem.Amount = item.Amount;
                        patPackageItem.ItemMultiplier = item.Quantity;
                        patPackageItem.StatusFlag = "A";
                        patPackageItem.MUser = model.MUser;
                        patPackageItem.CUser = model.CUser;
                        patPackageItem.MWhen = now;
                        patPackageItem.CWhen = now;
                        patPackageItem.OwnerOrganisationUID = model.OwnerOrganisationUID;
                        db.PatientPackageItem.Add(patPackageItem);

                    }

                    db.SaveChanges();


                    PatientBillableItem patBillableItem = new PatientBillableItem();
                    patBillableItem.PatientUID = model.PatientUID;
                    patBillableItem.PatientVisitUID = model.PatientVisitUID;
                    patBillableItem.IdentifyingUID = model.BillPackageUID ?? 0;
                    patBillableItem.IdentifyingType = "BillPackage";
                    patBillableItem.Amount = model.TotalAmount;
                    patBillableItem.Discount = 0;
                    patBillableItem.NetAmount = model.TotalAmount;
                    patBillableItem.ItemMultiplier = model.Qty;
                    patBillableItem.ItemName = model.PackageName;
                    patBillableItem.EventOccuredDttm = now;
                    patBillableItem.BillPackageUID = (int?)patPackage.UID;
                    patBillableItem.StatusFlag = "A";
                    patBillableItem.MUser = model.MUser;
                    patBillableItem.CUser = model.CUser;
                    patBillableItem.MWhen = now;
                    patBillableItem.CWhen = now;
                    patBillableItem.OwnerOrganisationUID = model.OwnerOrganisationUID;

                    db.PatientBillableItem.Add(patBillableItem);

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


        [Route("DeletePatientPackage")]
        [HttpDelete]
        public HttpResponseMessage DeletePatientPackage(long patientPackakgeUID, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                var patientPackage = db.PatientPackage.Find(patientPackakgeUID);
                if (patientPackage == null) return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Date Not Found");
                using (var tran = new TransactionScope())
                {
                    db.PatientPackage.Attach(patientPackage);
                    patientPackage.MUser = userID;
                    patientPackage.MWhen = now;
                    patientPackage.StatusFlag = "D";
                    db.SaveChanges();


                    IEnumerable<PatientPackageItem> patPackageItems = db.PatientPackageItem.Where(p => p.PatientPackageUID == patientPackage.UID);
                    foreach (var item in patPackageItems)
                    {
                        db.PatientPackageItem.Attach(item);
                        item.MUser = userID;
                        item.MWhen = now;
                        item.StatusFlag = "D";

                    }
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


        [Route("GetPatientPackageByVisitUID")]
        [HttpGet]
        public List<PatientPackageModel> GetPatientPackageByVisitUID(long patientVisitUID)
        {
            List<PatientPackageModel> patientPackages = db.PatientPackage
                 .Where(p => p.PatientVisitUID == patientVisitUID && p.StatusFlag == "A")
                 .Select(p => new PatientPackageModel
                 {
                     PatientPackageUID = p.UID,
                     PackageName = p.PackageName,
                     PackageCreatedByUID = p.PackageCreatedByUID,
                     PackageCreatedDttm = p.PackageCreatedDttm,
                     PackageCreatedByName = SqlFunction.fGetCareProviderName(p.PackageCreatedByUID),
                     TotalAmount = p.TotalAmount,
                     PatientUID = p.PatientUID,
                     PatientVisitUID = p.PatientVisitUID,
                     BillPackageUID = p.BillPackageUID,
                     BillingGroupUID = p.BillingGroupUID,
                     BillingSubGroupUID = p.BillingSubGroupUID,
                     BillingGroup = (p.BillingGroupUID != 0 && p.BillingGroupUID != null) ? SqlFunction.fGetBillingGroupDesc(p.BillingGroupUID ?? 0, "G") : "",
                     BillingSubGroup = (p.BillingSubGroupUID != 0 && p.BillingSubGroupUID != null) ? SqlFunction.fGetBillingGroupDesc(p.BillingSubGroupUID ?? 0, "S") : ""

                 }).ToList();

            if (patientPackages != null && patientPackages.Count > 0)
            {
                foreach (var patPackage in patientPackages)
                {
                    patPackage.BillPackageDetails = GetPatientPackageItemByPatientPakcageUID(patPackage.PatientPackageUID);
                }
            }

            return patientPackages;
        }

        [Route("GetPatientPackageItemByPatientPakcageUID")]
        [HttpGet]
        public List<PatientPackageItemModel> GetPatientPackageItemByPatientPakcageUID(long patientPakcageUID)
        {

           List<PatientPackageItemModel> data = (from pki in db.PatientPackageItem
                                                  join bl in db.BillableItem on pki.BillableItemUID equals bl.UID
                                                  join pk in db.PatientPackage on pki.PatientPackageUID equals pk.UID
                                                  where pki.PatientPackageUID == patientPakcageUID
                                                  && pki.StatusFlag == "A"
                                                  && bl.StatusFlag == "A"
                                                  select new PatientPackageItemModel
                                                  {
                                                      PatientPackageItemUID = pki.UID,
                                                      PatientPackageUID = pki.PatientPackageUID,
                                                      BillableItemUID = pki.BillableItemUID,
                                                      Amount = pki.Amount,
                                                      Discount = pki.Discount,
                                                      ActualAmount = pki.ActualAmount,
                                                      ItemMultiplier = pki.ItemMultiplier,
                                                      BillPackageUID = pk.BillPackageUID ?? 0,
                                                      BSMDDUID = pki.BSMDDUID,
                                                      ItemCode = bl.Code,
                                                      ItemName = bl.ItemName,
                                                  }).ToList();
            foreach (var item in data)
            {
                item.NetAmount = Math.Round(item.Amount * item.ItemMultiplier ?? 0, 2);
            }
            return data;
        }

        [Route("GetAdjustablePackageItems")]
        [HttpGet]
        public List<AdjustablePackageItemModel> GetAdjustablePackageItems(long patientUID, long patientVisitUID, long patientPackageUID)
        {
            List<AdjustablePackageItemModel> data = SqlDirectStore.pGetAdjustablePackageItems(patientUID, patientVisitUID, patientPackageUID).ToList<AdjustablePackageItemModel>();

            return data;
        }

        [Route("GetLinkPackage")]
        [HttpGet]
        public List<LinkPackageModel> GetLinkPackage(int billableItemUID, long patientVisitUID)
        {
            List<LinkPackageModel> data = SqlDirectStore.pGetLinkPackage(billableItemUID, patientVisitUID).ToList<LinkPackageModel>();

            return data;
        }

        [Route("AdjustOrderDetailForPackage")]
        [HttpPut]
        public HttpResponseMessage AdjustOrderDetailForPackage(long patientUID, long patientVisitUID, long? patientPackageUID, int billableItemUID, List<long> patientOrderDetailUIDs)
        {
            try
            {
                DateTime now = DateTime.Now;

                using (var tran = new TransactionScope())
                {
                    var billPackageUID = db.PatientPackage.Find(patientPackageUID)?.BillPackageUID;
                    if (patientOrderDetailUIDs == null || patientOrderDetailUIDs.Count < 0)
                    {
                        patientOrderDetailUIDs = db.PatientOrderDetail.Where(p => p.PatientPackageUID == patientPackageUID && p.StatusFlag == "A").Select(p => p.UID).ToList();
                    }
                    foreach (var orderDetailUID in patientOrderDetailUIDs)
                    {
                        var patientOrderDetail = db.PatientOrderDetail.Find(orderDetailUID);

                        if (patientOrderDetail != null)
                        {
                            db.PatientOrderDetail.Attach(patientOrderDetail);
                            patientOrderDetail.BillPackageUID = null;
                            patientOrderDetail.PatientPackageUID = null;
                            patientOrderDetail.MWhen = now;
                            patientOrderDetail.AltBillableItemUID = patientOrderDetail.BillableItemUID != billableItemUID ? billableItemUID : patientOrderDetail.AltBillableItemUID;

                        }

                        var patientBillableItem = db.PatientBillableItem.FirstOrDefault(p => p.PatientOrderDetailUID == orderDetailUID);
                        if (patientBillableItem != null)
                        {
                            db.PatientBillableItem.Attach(patientBillableItem);
                            patientBillableItem.BillPackageUID = null;
                            patientBillableItem.MWhen = now;
                        }

                        var billPackage = from i in db.PatientPackage
                                          join j in db.BillPackage on i.BillPackageUID equals j.UID
                                          where i.UID == patientPackageUID
                                          && i.StatusFlag == "A"
                                          && j.StatusFlag == "A"
                                          && j.AllowOverridePrice == "Y"
                                          select j;
                        if (billPackage != null && billPackage.Count() > 0)
                        {
                            var amount = db.PatientBillableItem
                                .Where(p => p.PatientUID == patientUID && p.PatientVisitUID == patientVisitUID && p.StatusFlag == "A" && (p.BillPackageUID != null || p.BillPackageUID > 0)
                                && p.IdentifyingType != "BillPackage" && p.BillPackageUID == patientPackageUID)
                                .Sum(p => p.Amount * p.ItemMultiplier);

                            var patientBillableItem1 = db.PatientBillableItem.Where(p => p.PatientUID == patientUID && p.PatientVisitUID == patientVisitUID && p.StatusFlag == "A"
                                && p.IdentifyingType == "BillPackage" && p.BillPackageUID == patientPackageUID).FirstOrDefault();
                            if (patientBillableItem1 != null)
                            {
                                patientBillableItem1.Amount = amount;
                                patientBillableItem1.NetAmount = amount * patientBillableItem1.ItemMultiplier - patientBillableItem1.Discount;
                                patientBillableItem1.MWhen = now;
                                db.PatientBillableItem.Attach(patientBillableItem1);
                            }
                        }

                    }
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


        [Route("UpdateLinkPackage")]
        [HttpPut]
        public HttpResponseMessage UpdateLinkPackage(long patientOrderDetailUID, long patientPackageUID, bool linkFlag, int userUID)
        {
            try
            {
                DateTime now = DateTime.Now;

                var patOrder = (from pd in db.PatientOrderDetail
                                join p in db.PatientOrder on pd.PatientOrderUID equals p.UID
                                where pd.UID == patientOrderDetailUID
                                && pd.StatusFlag == "A"
                                && p.StatusFlag == "A"
                                select new
                                {
                                    PatientOrderDetailUID = pd.UID,
                                    PatientVisitUID = p.PatientVisitUID,
                                    PatientFixPriceUID = pd.PatientFixPriceUID ?? 0,
                                    ORDSTUID = pd.ORDSTUID,
                                    PatientPackageUID = pd.PatientPackageUID ?? 0,
                                    OwnerOrganisationUID = pd.OwnerOrganisationUID,
                                    BillableItemUID = pd.BillableItemUID,
                                    StartDttm = pd.StartDttm,
                                    Quantity = pd.Quantity,
                                    OverwritePrice = pd.OverwritePrice,
                                    PatientPackageItemUID = pd.PatientPackageItemUID
                                }).ToList().FirstOrDefault();



                using (var tran = new TransactionScope())
                {

                    if (patOrder != null && patOrder.ORDSTUID != 2848 && patOrder.PatientFixPriceUID == 0)
                    {
                        if (patOrder.PatientPackageUID == 0 || patOrder.PatientPackageUID == patientPackageUID)
                        {
                            var patientBillbaleItemUIDs = db.PatientBillableItem.Where(p => p.StatusFlag == "A" && p.PatientOrderDetailUID == patOrder.PatientOrderDetailUID).Select(p => p.UID).ToList();

                            var countBill = (from i in db.PatientBill
                                             join j in db.PatientBilledItem on i.UID equals j.PatientBillUID
                                             where i.StatusFlag == "A"
                                             && j.StatusFlag == "A"
                                             && i.IsRefund == null
                                             && i.PatientVisitUID == patOrder.PatientVisitUID
                                             && patientBillbaleItemUIDs.Contains(j.PatientBillableItemUID ?? 0)
                                             select i).Count();
                            if (countBill == 0)
                            {
                                if (linkFlag && patOrder.PatientPackageUID == 0)
                                {
             

                                    var patientPackage = db.PatientPackage.FirstOrDefault(p => p.UID == patientPackageUID);
                                    var patientPackageItem = db.PatientPackageItem.FirstOrDefault(p => p.PatientPackageUID == patientPackageUID && p.BillableItemUID == patOrder.BillableItemUID && p.StatusFlag == "A");
                                    var maxQty = patientPackage.Qty;
                                    var quantity = db.BillPackageItem.FirstOrDefault(p => p.BillPackageUID == patientPackage.BillPackageUID && p.BillableItemUID == patOrder.BillableItemUID && p.StatusFlag == "A")?.Quantity;

                                    var patientOrder = db.PatientOrder.Where(p => p.PatientVisitUID == patOrder.PatientVisitUID && p.StatusFlag == "A");

                                    var quantityN = (from pd in db.PatientOrderDetail
                                                     join po in db.PatientOrder on pd.PatientOrderUID equals po.UID
                                                     where pd.StatusFlag == "A"
                                                     && pd.ORDSTUID != 2848
                                                     && pd.BillableItemUID == patOrder.BillableItemUID
                                                     && pd.BillPackageUID == patientPackage.BillPackageUID
                                                     && pd.PatientPackageUID == patientPackage.UID
                                                     select pd.Quantity).Sum();
                                    var checkItemQty = quantityN ?? 0 - quantity ?? 0 * maxQty ?? 0;
                                    if (checkItemQty + patOrder.Quantity <= 0)
                                    {
                                        var patientOrderDetail = db.PatientOrderDetail.Find(patOrder.PatientOrderDetailUID);
                                        db.PatientOrderDetail.Attach(patientOrderDetail);
                                        patientOrderDetail.PatientPackageUID = patientPackage.UID;
                                        patientOrderDetail.BillPackageUID = patientPackage.BillPackageUID;
                                        patientOrderDetail.PatientPackageItemUID = patientPackageItem.UID;
                                        patientOrderDetail.UnitPrice = patientPackageItem.Amount;
                                        patientOrderDetail.NetAmount = patientPackageItem.Amount * patientOrderDetail.Quantity;


                                        var patientBillableItem = db.PatientBillableItem.FirstOrDefault(p => p.PatientVisitUID == patOrder.PatientVisitUID && p.PatientOrderDetailUID == patOrder.PatientOrderDetailUID);
                                        db.PatientBillableItem.Attach(patientBillableItem);
                                        patientBillableItem.BillPackageUID = patientPackage.UID;
                                        patientBillableItem.IsModified = "Y";
                                    }

                                 }
                                else if (!linkFlag && patOrder.PatientPackageUID == patientPackageUID)
                                {
                                    var billingCategory = db.ReferenceValue.Where(p => p.DomainCode == "PBLCT").ToList();
                                    var patientVisitPayors = (new PatientIdentityController()).GetPatientVisitPayorByVisitUID(patOrder.PatientVisitUID);
                                    var firstVisitPayors = patientVisitPayors.FirstOrDefault();
                                    int? PBLCTUID = billingCategory.FirstOrDefault(p => p.ValueCode == "OPDTRF")?.UID;

                                    if (firstVisitPayors != null)
                                    {
                                        PBLCTUID = firstVisitPayors.PrimaryPBLCTUID;
                                    }

                                    var selectBillItemDetail = db.BillableItemDetail
    .FirstOrDefault(p => p.StatusFlag == "A" && p.BillableItemUID == patOrder.BillableItemUID && p.OwnerOrganisationUID == patOrder.OwnerOrganisationUID && p.PBLCTUID == PBLCTUID
    && (p.ActiveFrom == null || (DbFunctions.TruncateTime(p.ActiveFrom) <= DbFunctions.TruncateTime(DateTime.Now)))
    && (p.ActiveTo == null || (p.ActiveTo.HasValue && DbFunctions.TruncateTime(p.ActiveTo) >= DbFunctions.TruncateTime(DateTime.Now)))
    );

                                    var patientOrderDetail = db.PatientOrderDetail.Find(patOrder.PatientOrderDetailUID);
                                    db.PatientOrderDetail.Attach(patientOrderDetail);
                                    patientOrderDetail.PatientPackageUID = null;
                                    patientOrderDetail.BillPackageUID = null;
                                    patientOrderDetail.PatientPackageItemUID = null;
                                    patientOrderDetail.UnitPrice = selectBillItemDetail.Price;
                                    patientOrderDetail.NetAmount = patientOrderDetail.OverwritePrice != null ? patOrder.OverwritePrice * patientOrderDetail.Quantity : selectBillItemDetail.Price * patientOrderDetail.Quantity;


                                    var patientBillableItem = db.PatientBillableItem.FirstOrDefault(p => p.PatientVisitUID == patOrder.PatientVisitUID && p.PatientOrderDetailUID == patOrder.PatientOrderDetailUID);
                                    db.PatientBillableItem.Attach(patientBillableItem);
                                    patientBillableItem.BillPackageUID = null;
                                    patientBillableItem.IsModified = "N";



                                }
                            }
                        }
                    }

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

        private HttpResponseMessage HttpNotFound()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
