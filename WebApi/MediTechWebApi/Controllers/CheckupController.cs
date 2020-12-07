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

namespace MediTechWebApi.Controllers
{
    [RoutePrefix("Api/Checkup")]
    public class CheckupController : ApiController
    {
        protected MediTechEntities db = new MediTechEntities();
        #region CheckupJobContact

        [Route("GetCheckupJobContactAll")]
        [HttpGet]
        public List<CheckupJobContactModel> GetCheckupJobContactAll()
        {
            List<CheckupJobContactModel> data = db.CheckupJobContact
                .Where(p => p.StatusFlag == "A")
                .Select(p => new CheckupJobContactModel
                {
                    CheckupJobContactUID = p.UID,
                    JobContactID = p.JobContactID,
                    PayorDetailUID = p.PayorDetailUID,
                    CompanyName = p.CompanyName,
                    Description = p.Description,
                    JobNumber = p.JobNumber,
                    Location = p.Location,
                    ContactPerson = p.ContactPerson,
                    ContactEmail = p.ContactEmail,
                    ContactPhone = p.ContactPhone,
                    ServiceName = p.ServiceName,
                    VisitCount = p.VisitCount,
                    StartDttm = p.StartDttm,
                    EndDttm = p.EndDttm,
                    CollectDttm = p.CollectDttm
                }).ToList();

            return data;
        }

        [Route("GetCheckupJobContactByUID")]
        [HttpGet]
        public CheckupJobContactModel GetCheckupJobContactByUID(int checkupJobConatctUID)
        {
            CheckupJobContactModel data = db.CheckupJobContact.Where(p => p.UID == checkupJobConatctUID)
                .Select(p => new CheckupJobContactModel
                {
                    CheckupJobContactUID = p.UID,
                    JobContactID = p.JobContactID,
                    PayorDetailUID = p.PayorDetailUID,
                    CompanyName = p.CompanyName,
                    Description = p.Description,
                    JobNumber = p.JobNumber,
                    Location = p.Location,
                    ContactPerson = p.ContactPerson,
                    ContactEmail = p.ContactEmail,
                    ContactPhone = p.ContactPhone,
                    ServiceName = p.ServiceName,
                    VisitCount = p.VisitCount,
                    StartDttm = p.StartDttm,
                    EndDttm = p.EndDttm,
                    CollectDttm = p.CollectDttm
                }).FirstOrDefault();

            if (data != null)
            {
                data.CheckupJobTasks = db.CheckupJobTask.Where(p => p.CheckupJobContactUID == data.CheckupJobContactUID)
                    .Select(p => new CheckupJobTaskModel
                    {
                        CheckupJobTaskUID = p.UID,
                        CheckupJobContactUID = p.CheckupJobContactUID,
                        GPRSTUID = p.GPRSTUID,
                        GroupResultName = p.GroupResultName,
                        DisplayOrder = p.DisplayOrder
                    }).ToList();
            }

            return data;
        }

        [Route("GetCheckupJobContactByPayorDetailUID")]
        [HttpGet]
        public List<CheckupJobContactModel> GetCheckupJobContactByPayorDetailUID(int payorDetailUID)
        {
            List<CheckupJobContactModel> data = db.CheckupJobContact.Where(p => p.PayorDetailUID == payorDetailUID)
                .Select(p => new CheckupJobContactModel
                {
                    CheckupJobContactUID = p.UID,
                    JobContactID = p.JobContactID,
                    PayorDetailUID = p.PayorDetailUID,
                    CompanyName = p.CompanyName,
                    Description = p.Description,
                    JobNumber = p.JobNumber,
                    Location = p.Location,
                    ContactPerson = p.ContactPerson,
                    ContactEmail = p.ContactEmail,
                    ContactPhone = p.ContactPhone,
                    ServiceName = p.ServiceName,
                    VisitCount = p.VisitCount,
                    StartDttm = p.StartDttm,
                    EndDttm = p.EndDttm,
                    CollectDttm = p.CollectDttm
                }).ToList();

            return data;
        }

        [Route("GetResultItemByRequestDetailUID")]
        [HttpGet]
        public List<ResultComponentModel> GetResultItemByRequestDetailUID(long requestDetailUID)
        {

            List<ResultComponentModel> data;
            data = (from ri in db.ResultItem
                    join rsl in db.RequestResultLink on ri.UID equals rsl.ResultItemUID
                    join red in db.RequestDetail on rsl.RequestItemUID equals red.RequestitemUID
                    join r in db.Result on
                    new
                    {
                        key1 = red.UID,
                        key2 = "A"
                    }
                    equals
                    new
                    {
                        key1 = r.RequestDetailUID,
                        key2 = r.StatusFlag
                    }
                    into result
                    from rs in result.DefaultIfEmpty()
                    join rsc in db.ResultComponent on
                    new
                    {
                        Key1 = rs.UID,
                        Key2 = ri.UID
                    }
                    equals
                    new
                    {
                        Key1 = rsc.ResultUID,
                        Key2 = rsc.ResultItemUID ?? 0
                    }
                    into resultComponent
                    from resultCom in resultComponent.DefaultIfEmpty()
                    where red.UID == requestDetailUID
                    && ri.StatusFlag == "A"
                    && rsl.StatusFlag == "A"
                    select new ResultComponentModel
                    {
                        ResultComponentUID = resultCom.UID,
                        ResultUID = resultCom.ResultUID,
                        RequestDetailUID = red.UID,
                        ResultValue = resultCom.ResultValue,
                        RVTYPUID = ri.RVTYPUID ?? 0,
                        ResultValueType = SqlFunction.fGetRfValDescription(ri.RVTYPUID ?? 0),
                        ResultItemUID = ri.UID,
                        ResultItemCode = ri.Code,
                        ResultItemName = ri.DisplyName,
                        PrintOrder = rsl.PrintOrder ?? 0,
                        IsMandatory = rsl.IsMandatory ?? "N",
                        ReferenceRange = resultCom.ReferenceRange,
                        Low = resultCom.Low,
                        High = resultCom.High,
                        ResultDTTM = resultCom.ResultDTTM,
                        RSUOMUID = ri.UnitofMeasure,
                        AutoValue = ri.AutoValue,
                        UnitofMeasure = SqlFunction.fGetRfValDescription(ri.UnitofMeasure ?? 0),
                        IsAbnormal = resultCom.IsAbnormal,
                    }).OrderBy(p => p.PrintOrder).ToList();

            return data;
        }


        [Route("DeleteCheckupJobContact")]
        [HttpDelete]
        public HttpResponseMessage DeleteCheckupJobContact(int checkupJobContactUID, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                CheckupJobContact checkupJobContact = db.CheckupJobContact.Find(checkupJobContactUID);
                if (checkupJobContact != null)
                {
                    db.CheckupJobContact.Attach(checkupJobContact);
                    checkupJobContact.MUser = userID;
                    checkupJobContact.MWhen = now;
                    checkupJobContact.StatusFlag = "D";
                    db.SaveChanges();


                    var checkupJobTask = db.CheckupJobTask.Where(p => p.CheckupJobContactUID == checkupJobContact.UID);

                    foreach (var item in checkupJobTask)
                    {
                        db.CheckupJobTask.Attach(item);
                        item.MUser = userID;
                        item.MWhen = now;
                        item.StatusFlag = "D";
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

        [Route("SaveCheckupJobContact")]
        [HttpPost]
        public HttpResponseMessage SaveCheckupJobContact(CheckupJobContactModel checkupJobContactModel, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;

                using (var tran = new TransactionScope())
                {
                    CheckupJobContact checkupJob = db.CheckupJobContact.Find(checkupJobContactModel.CheckupJobContactUID);
                    if (checkupJob == null)
                    {
                        checkupJob = new CheckupJobContact();
                        checkupJob.JobContactID = Guid.NewGuid();
                        checkupJob.CUser = userID;
                        checkupJob.CWhen = now;
                        checkupJob.StatusFlag = "A";
                        int jobID;
                        string jobNumber = SEQHelper.GetSEQIDFormat("SEQCheckupJobNumber", out jobID);


                        if (string.IsNullOrEmpty(jobNumber))
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No SEQCheckupJobNumber in SEQCONFIGURATION");
                        }

                        if (jobID == 0)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Insert SEQCheckupJobNumber is Fail");
                        }

                        checkupJob.JobNumber = jobNumber;
                    }

                    checkupJob.PayorDetailUID = checkupJobContactModel.PayorDetailUID;
                    checkupJob.CompanyName = checkupJobContactModel.CompanyName;
                    checkupJob.Description = checkupJobContactModel.Description;
                    checkupJob.Location = checkupJobContactModel.Location;
                    checkupJob.ContactPerson = checkupJobContactModel.ContactPerson;
                    checkupJob.ContactPhone = checkupJobContactModel.ContactPhone;
                    checkupJob.ContactEmail = checkupJobContactModel.ContactEmail;
                    checkupJob.ServiceName = checkupJobContactModel.ServiceName;
                    checkupJob.VisitCount = checkupJobContactModel.VisitCount;
                    checkupJob.StartDttm = checkupJobContactModel.StartDttm;
                    checkupJob.EndDttm = checkupJobContactModel.EndDttm;
                    checkupJob.CollectDttm = checkupJobContactModel.CollectDttm;
                    checkupJob.MUser = userID;
                    checkupJob.MWhen = now;

                    db.CheckupJobContact.AddOrUpdate(checkupJob);

                    #region Delete CheckupJobTaskModel
                    IEnumerable<CheckupJobTask> checkupJobTaskdel = db.CheckupJobTask.Where(p => p.StatusFlag == "A" && p.CheckupJobContactUID == checkupJobContactModel.CheckupJobContactUID);

                    if (checkupJobContactModel.CheckupJobTasks == null)
                    {
                        foreach (var item in checkupJobTaskdel)
                        {
                            db.CheckupJobTask.Attach(item);
                            item.MUser = userID;
                            item.MWhen = now;
                            item.StatusFlag = "D";
                        }
                    }
                    else
                    {
                        foreach (var item in checkupJobTaskdel)
                        {
                            var data = checkupJobContactModel.CheckupJobTasks.FirstOrDefault(p => p.CheckupJobTaskUID == item.UID);
                            if (data == null)
                            {
                                db.CheckupJobTask.Attach(item);
                                item.MUser = userID;
                                item.MWhen = now;
                                item.StatusFlag = "D";
                            }

                        }
                    }

                    db.SaveChanges();

                    #endregion

                    if (checkupJobContactModel.CheckupJobTasks != null)
                    {
                        foreach (var item in checkupJobContactModel.CheckupJobTasks)
                        {
                            CheckupJobTask checkupJobTask = db.CheckupJobTask.Find(item.CheckupJobTaskUID);
                            if (checkupJobTask == null)
                            {
                                checkupJobTask = new CheckupJobTask();
                                checkupJobTask.CUser = userID;
                                checkupJobTask.CWhen = now;
                                checkupJobTask.MUser = userID;
                                checkupJobTask.MWhen = now;
                                checkupJobTask.StatusFlag = "A";
                            }
                            else
                            {
                                if (item.MWhen != DateTime.MinValue)
                                {
                                    checkupJobTask.MUser = userID;
                                    checkupJobTask.MWhen = now;
                                }
                            }
                            checkupJobTask.CheckupJobContactUID = checkupJob.UID;
                            checkupJobTask.GPRSTUID = item.GPRSTUID;
                            checkupJobTask.GroupResultName = item.GroupResultName;
                            checkupJobTask.DisplayOrder = item.DisplayOrder;
                            db.CheckupJobTask.AddOrUpdate(checkupJobTask);
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

        #endregion

        #region HealthExamList
        [Route("SearchCheckupExamList")]
        [HttpGet]
        public List<RequestListModel> SearchCheckupExamList(DateTime? requestDateFrom, DateTime? requestDateTo, long? patientUID, int? payorDetailUID, int? checkupJobUID, int? PRTGPUID)
        {
            DataTable dataTable = SqlDirectStore.pSearchCheckupExamList(requestDateFrom, requestDateTo, patientUID, payorDetailUID, checkupJobUID, PRTGPUID);
            List<RequestListModel> listData = dataTable.ToList<RequestListModel>();

            return listData;
        }

        [Route("SaveOccmedExamination")]
        [HttpPost]
        public HttpResponseMessage SaveOccmedExamination(RequestDetailItemModel requestDetails, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                int reviewStatus = 2863;
                int imageType = 2516;
                using (var tran = new TransactionScope())
                {
                    Request request = db.Request.Find(requestDetails.RequestUID);
                    //var requestDetails = db.RequestDetail.Where(p => p.RequestUID == request.UID);

                    Result delResult = db.Result.FirstOrDefault(p => p.RequestDetailUID == requestDetails.RequestDetailUID && p.StatusFlag == "A");

                    if (delResult != null)
                    {
                        List<ResultComponent> delResultcomponents = db.ResultComponent.Where(p => p.ResultUID == delResult.UID && p.StatusFlag == "A").ToList();

                        foreach (var item in delResultcomponents)
                        {
                            item.StatusFlag = "D";

                            if (item.RVTYPUID == imageType)
                            {
                                ResultImage resultImage = db.ResultImage.FirstOrDefault(p => p.ResultComponentUID == item.UID);
                                if (resultImage != null)
                                {
                                    db.ResultImage.Remove(resultImage);
                                }

                            }
                        }

                        delResult.StatusFlag = "D";
                        db.SaveChanges();
                    }


                    RequestDetail requestDetail = db.RequestDetail.Find(requestDetails.RequestDetailUID);


                    SEQResult seqResultID = new SEQResult();
                    seqResultID.StatusFlag = "A";

                    db.SEQResult.Add(seqResultID);
                    db.SaveChanges();

                    Result result = new Result();
                    result.CUser = userID;
                    result.CWhen = now;
                    result.StatusFlag = "A";
                    result.ResultNumber = seqResultID.UID.ToString();

                    result.RequestDetailUID = requestDetails.RequestDetailUID;
                    result.MUser = userID;
                    result.MWhen = now;
                    result.ResultEnteredDttm = now;
                    result.ResultEnteredUserUID = userID;
                    result.ORDSTUID = reviewStatus;
                    result.PatientUID = requestDetails.PatientUID;
                    result.PatientVisitUID = requestDetails.PatientVisitUID;
                    result.ResultedByUID = userID;
                    result.RequestItemCode = requestDetails.RequestItemCode;
                    result.RequestItemName = requestDetails.RequestItemName;
                    db.Result.Add(result);
                    db.SaveChanges();


                    foreach (var components in requestDetails.ResultComponents)
                    {
                        ResultComponent resultComponent = new ResultComponent();
                        resultComponent.CUser = userID;
                        resultComponent.CWhen = now;
                        resultComponent.MUser = userID;
                        resultComponent.MWhen = now;
                        resultComponent.StatusFlag = "A";
                        resultComponent.ResultUID = result.UID;
                        resultComponent.ResultItemUID = components.ResultItemUID;
                        resultComponent.RVTYPUID = components.RVTYPUID;
                        resultComponent.ResultItemName = components.ResultItemName;
                        resultComponent.ResultItemCode = components.ResultItemCode;
                        resultComponent.ResultValue = components.ResultValue;

                        resultComponent.ResultDTTM = components.ResultDTTM ?? now;
                        resultComponent.Comments = components.Comments;
                        db.ResultComponent.Add(resultComponent);
                        db.SaveChanges();
                    }


                    db.RequestDetail.Attach(requestDetail);
                    requestDetail.ORDSTUID = reviewStatus;
                    requestDetail.MUser = userID;
                    requestDetail.MWhen = now;


                    PatientOrderDetail patientOrderDetail = db.PatientOrderDetail.Find(requestDetail.PatientOrderDetailUID);

                    //PatientOrderDetail
                    if (patientOrderDetail != null)
                    {
                        db.PatientOrderDetail.Attach(patientOrderDetail);
                        patientOrderDetail.ORDSTUID = requestDetail.ORDSTUID;
                        patientOrderDetail.MUser = userID;
                        patientOrderDetail.MWhen = DateTime.Now;

                        db.SaveChanges();


                        #region SavePatinetOrderDetailHistory

                        PatientOrderDetailHistory patientOrderDetailHistory = new PatientOrderDetailHistory();
                        patientOrderDetailHistory.PatientOrderDetailUID = patientOrderDetail.UID;
                        patientOrderDetailHistory.ORDSTUID = patientOrderDetail.ORDSTUID;
                        patientOrderDetailHistory.EditedDttm = now;
                        patientOrderDetailHistory.EditByUserID = userID;
                        patientOrderDetailHistory.CUser = userID;
                        patientOrderDetailHistory.CWhen = now;
                        patientOrderDetailHistory.MUser = userID;
                        patientOrderDetailHistory.MWhen = now;
                        patientOrderDetailHistory.StatusFlag = "A";
                        db.PatientOrderDetailHistory.Add(patientOrderDetailHistory);

                        #endregion

                        db.SaveChanges();
                    }

                    db.Request.Attach(request);
                    request.ORDSTUID = (new RadiologyController()).CheckRequestStatus(request.UID);
                    request.MUser = userID;
                    request.MWhen = now;
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


        [Route("CancelOccmedResult")]
        [HttpPut]
        public HttpResponseMessage CancelOccmedResult(long requestDetailUID, int userUID)
        {
            try
            {
                DateTime now = DateTime.Now;
                using (var tran = new TransactionScope())
                {

                    var dataRequestDetail = db.RequestDetail.Find(requestDetailUID);

                    if (dataRequestDetail != null)
                    {
                        db.RequestDetail.Attach(dataRequestDetail);
                        dataRequestDetail.ORDSTUID = 2847; //RAISED
                        dataRequestDetail.MUser = userUID;
                        dataRequestDetail.MWhen = now;
                        db.SaveChanges();

                        var request = db.Request.Find(dataRequestDetail.RequestUID);

                        db.Request.Attach(request);
                        request.MUser = userUID;
                        request.MWhen = now;
                        request.ORDSTUID = (new RadiologyController()).CheckRequestStatus(request.UID);

                        db.SaveChanges();

                        var result = db.Result.FirstOrDefault(p => p.RequestDetailUID == dataRequestDetail.UID && p.StatusFlag == "A");
                        if (result != null)
                        {
                            List<ResultComponent> delResultcomponents = db.ResultComponent.Where(p =>
                            p.ResultUID == result.UID
                            && p.StatusFlag == "A").ToList();

                            foreach (var item in delResultcomponents)
                            {
                                item.StatusFlag = "D";
                            }


                            db.Result.Attach(result);
                            result.StatusFlag = "D";
                            result.MUser = userUID;
                            result.MWhen = now;

                            db.SaveChanges();
                        }

                        var dataOrderDetail = db.PatientOrderDetail.Find(dataRequestDetail.PatientOrderDetailUID);
                        if (dataOrderDetail != null)
                        {
                            db.PatientOrderDetail.Attach(dataOrderDetail);
                            dataOrderDetail.ORDSTUID = 2848;
                            dataOrderDetail.MUser = userUID;
                            dataOrderDetail.MWhen = now;

                            PatientOrderDetailHistory patientOrderDetailHistory = db.PatientOrderDetailHistory.FirstOrDefault(p => p.PatientOrderDetailUID 
                            == dataOrderDetail.UID && p.ORDSTUID == 2863);
                            if (patientOrderDetailHistory != null)
                            {
                                db.PatientOrderDetailHistory.Attach(patientOrderDetailHistory);
                                patientOrderDetailHistory.StatusFlag = "D";
                                patientOrderDetailHistory.MUser = userUID;
                                patientOrderDetailHistory.MWhen = now;
                            }
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
        #endregion
    }
}