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
                data.CheckupJobTasks = db.CheckupJobTask.Where(p => p.CheckupJobContactUID == data.CheckupJobContactUID && p.StatusFlag == "A")
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

        [Route("GetCheckupJobTaskByJobUID")]
        [HttpGet]
        public List<CheckupJobTaskModel> GetCheckupJobTaskByJobUID(int checkupJobConatctUID)
        {
            List<CheckupJobTaskModel> taskData = (from ck in db.CheckupJobTask
                                                  join rf in db.ReferenceValue on ck.GPRSTUID equals rf.UID
                                                  where ck.StatusFlag == "A"
                                                  && ck.CheckupJobContactUID == checkupJobConatctUID
                                                  select new CheckupJobTaskModel
                                                  {
                                                      CheckupJobTaskUID = ck.UID,
                                                      CheckupJobContactUID = ck.CheckupJobContactUID,
                                                      GPRSTUID = ck.GPRSTUID,
                                                      GroupResultName = ck.GroupResultName,
                                                      DisplayOrder = ck.DisplayOrder,
                                                      ReportTemplate = rf.AlternateName
                                                  }).ToList();


            return taskData;
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

        #region CheckupRule

        [Route("GetCheckupRuleByGroup")]
        [HttpGet]
        public List<CheckupRuleModel> GetCheckupRuleByGroup(int GPRSTUID)
        {
            List<CheckupRuleModel> data = db.CheckupRule
                .Where(p => p.StatusFlag == "A" && p.GPRSTUID == GPRSTUID)
                .Select(p => new CheckupRuleModel
                {
                    CheckupRuleUID = p.UID,
                    Name = p.Name,
                    AgeFrom = p.AgeFrom,
                    AgeTo = p.AgeTo,
                    RABSTSUID = p.RABSTSUID,
                    SEXXXUID = p.SEXXXUID,
                    GPRSTUID = p.GPRSTUID,
                    ResultStatus = SqlFunction.fGetRfValDescription(p.RABSTSUID),
                    Gender = p.SEXXXUID != null ? SqlFunction.fGetRfValDescription(p.SEXXXUID.Value) : ""
                }).ToList();
            return data;
        }

        [Route("SaveCheckupRule")]
        [HttpPost]
        public HttpResponseMessage SaveCheckupRule(CheckupRuleModel chekcupRuleModel, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;

                CheckupRule checkupRule = db.CheckupRule.Find(chekcupRuleModel.CheckupRuleUID);
                if (checkupRule == null)
                {
                    checkupRule = new CheckupRule();
                    checkupRule.CUser = userID;
                    checkupRule.CWhen = now;
                    checkupRule.StatusFlag = "A";
                }
                checkupRule.Name = chekcupRuleModel.Name;
                checkupRule.SEXXXUID = chekcupRuleModel.SEXXXUID;
                checkupRule.AgeFrom = chekcupRuleModel.AgeFrom;
                checkupRule.AgeTo = chekcupRuleModel.AgeTo;
                checkupRule.RABSTSUID = chekcupRuleModel.RABSTSUID;
                checkupRule.GPRSTUID = chekcupRuleModel.GPRSTUID;

                checkupRule.MUser = userID;
                checkupRule.MWhen = now;

                db.CheckupRule.AddOrUpdate(checkupRule);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }


        [Route("CopyCheckupRule")]
        [HttpPost]
        public HttpResponseMessage CopyCheckupRule(CheckupRuleModel chekcupRuleModel, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                using (var tran = new TransactionScope())
                {
                    int checkupRuleUID = chekcupRuleModel.CheckupRuleUID;
                    CheckupRule checkupRule = db.CheckupRule.Find(checkupRuleUID);
                    if (checkupRule != null)
                    {
                        CheckupRule newCheckupRule = new CheckupRule();
                        newCheckupRule.Name = checkupRule.Name;
                        newCheckupRule.SEXXXUID = checkupRule.SEXXXUID;
                        newCheckupRule.AgeFrom = checkupRule.AgeFrom;
                        newCheckupRule.AgeTo = checkupRule.AgeTo;
                        newCheckupRule.RABSTSUID = checkupRule.RABSTSUID;
                        newCheckupRule.GPRSTUID = checkupRule.GPRSTUID;
                        newCheckupRule.CUser = userID;
                        newCheckupRule.CWhen = now;
                        newCheckupRule.StatusFlag = "A";
                        newCheckupRule.MUser = userID;
                        newCheckupRule.MWhen = now;
                        db.CheckupRule.Add(newCheckupRule);
                        db.SaveChanges();

                        var cehckupRuleItem = db.CheckupRuleItem.Where(p => p.CheckupRuleUID == checkupRuleUID && p.StatusFlag == "A");
                        foreach (var item in cehckupRuleItem)
                        {
                            CheckupRuleItem newCheckupRuleItem = new CheckupRuleItem();
                            newCheckupRuleItem.CheckupRuleUID = newCheckupRule.UID;
                            newCheckupRuleItem.ResultItemUID = item.ResultItemUID;
                            newCheckupRuleItem.Low = item.Low;
                            newCheckupRuleItem.Hight = item.Hight;
                            newCheckupRuleItem.Text = item.Text;
                            newCheckupRuleItem.Operator = item.Operator;
                            newCheckupRuleItem.NonCheckup = item.NonCheckup;
                            newCheckupRuleItem.CUser = userID;
                            newCheckupRuleItem.CWhen = now;
                            newCheckupRuleItem.StatusFlag = "A";
                            newCheckupRuleItem.MUser = userID;
                            newCheckupRuleItem.MWhen = now;
                            db.CheckupRuleItem.Add(newCheckupRuleItem);
                            db.SaveChanges();
                        }

                        var checkupRuleDescription = db.CheckupRuleDescription.Where(p => p.CheckupRuleUID == checkupRuleUID && p.StatusFlag == "A");
                        foreach (var item in checkupRuleDescription)
                        {
                            CheckupRuleDescription newDscription = new CheckupRuleDescription();
                            newDscription.CheckupRuleUID = newCheckupRule.UID;
                            newDscription.CheckupTextMasterUID = item.CheckupTextMasterUID;
                            newDscription.CUser = userID;
                            newDscription.CWhen = now;
                            newDscription.StatusFlag = "A";
                            newDscription.MUser = userID;
                            newDscription.MWhen = now;
                            db.CheckupRuleDescription.Add(newDscription);
                            db.SaveChanges();
                        }

                        var cehckupRuleRecommand = db.CheckupRuleRecommend.Where(p => p.CheckupRuleUID == checkupRuleUID && p.StatusFlag == "A");
                        foreach (var item in cehckupRuleRecommand)
                        {
                            CheckupRuleRecommend newRecommand = new CheckupRuleRecommend();
                            newRecommand.CheckupRuleUID = newCheckupRule.UID;
                            newRecommand.CheckupTextMasterUID = item.CheckupTextMasterUID;
                            newRecommand.CUser = userID;
                            newRecommand.CWhen = now;
                            newRecommand.StatusFlag = "A";
                            newRecommand.MUser = userID;
                            newRecommand.MWhen = now;
                            db.CheckupRuleRecommend.Add(newRecommand);
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

        [Route("DeleteCheckupRule")]
        [HttpDelete]
        public HttpResponseMessage DeleteCheckupRule(int chekcupRuleUID, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                using (var tran = new TransactionScope())
                {
                    CheckupRule checkupRule = db.CheckupRule.Find(chekcupRuleUID);
                    if (checkupRule != null)
                    {
                        db.CheckupRule.Attach(checkupRule);
                        checkupRule.MUser = userID;
                        checkupRule.MWhen = now;
                        checkupRule.StatusFlag = "D";
                        db.SaveChanges();
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


        [Route("GetCheckupRuleItemByRuleUID")]
        [HttpGet]
        public List<CheckupRuleItemModel> GetCheckupRuleItemByRuleUID(int chekcupRuleUID)
        {
            List<CheckupRuleItemModel> data = (from j in db.CheckupRuleItem
                                               join i in db.ResultItem on j.ResultItemUID equals i.UID
                                               where j.StatusFlag == "A"
                                               && j.CheckupRuleUID == chekcupRuleUID
                                               select new CheckupRuleItemModel
                                               {
                                                   CheckupRuleItemUID = j.UID,
                                                   CheckupRuleUID = j.CheckupRuleUID,
                                                   ResultItemUID = i.UID,
                                                   ResultItemName = i.DisplyName,
                                                   Unit = SqlFunction.fGetRfValDescription(i.UnitofMeasure ?? 0),
                                                   Operator = j.Operator,
                                                   Text = j.Text,
                                                   Low = j.Low,
                                                   Hight = j.Hight,
                                                   NonCheckup = j.NonCheckup
                                               }).ToList();
            return data;
        }

        [Route("SaveCheckupRuleItem")]
        [HttpPost]
        public HttpResponseMessage SaveCheckupRuleItem(CheckupRuleItemModel chekcupRuleItemModel, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;

                CheckupRuleItem checkupRuleItem = db.CheckupRuleItem.Find(chekcupRuleItemModel.CheckupRuleItemUID);
                if (checkupRuleItem == null)
                {
                    checkupRuleItem = new CheckupRuleItem();
                    checkupRuleItem.CUser = userID;
                    checkupRuleItem.CWhen = now;
                    checkupRuleItem.StatusFlag = "A";
                }
                checkupRuleItem.CheckupRuleUID = chekcupRuleItemModel.CheckupRuleUID;
                checkupRuleItem.ResultItemUID = chekcupRuleItemModel.ResultItemUID;
                checkupRuleItem.Low = chekcupRuleItemModel.Low;
                checkupRuleItem.Hight = chekcupRuleItemModel.Hight;
                checkupRuleItem.Text = chekcupRuleItemModel.Text;
                checkupRuleItem.Operator = chekcupRuleItemModel.Operator;
                checkupRuleItem.NonCheckup = chekcupRuleItemModel.NonCheckup;
                checkupRuleItem.MUser = userID;
                checkupRuleItem.MWhen = now;

                db.CheckupRuleItem.AddOrUpdate(checkupRuleItem);
                db.SaveChanges();


                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("DeleteCheckupRuleItem")]
        [HttpDelete]
        public HttpResponseMessage DeleteCheckupRuleItem(int chekcupRuleItemUID, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                using (var tran = new TransactionScope())
                {
                    CheckupRuleItem checkupRuleItem = db.CheckupRuleItem.Find(chekcupRuleItemUID);
                    if (checkupRuleItem != null)
                    {
                        db.CheckupRuleItem.Attach(checkupRuleItem);
                        checkupRuleItem.MUser = userID;
                        checkupRuleItem.MWhen = now;
                        checkupRuleItem.StatusFlag = "D";
                        db.SaveChanges();
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

        [Route("GetCheckupTextMaster")]
        [HttpGet]
        public List<CheckupTextMasterModel> GetCheckupTextMaster()
        {
            List<CheckupTextMasterModel> data = db.CheckupTextMaster.Where(p => p.StatusFlag == "A")
                .Select(p => new CheckupTextMasterModel
                {
                    CheckupTextMasterUID = p.UID,
                    ThaiWord = p.ThaiWord,
                    EngWord = p.EngWord
                }).ToList();

            return data;
        }

        [Route("SaveCheckupTextMaster")]
        [HttpPost]
        public HttpResponseMessage SaveCheckupTextMaster(CheckupTextMasterModel checkupTextMasterModel)
        {
            try
            {
                DateTime now = DateTime.Now;
                CheckupTextMaster newData = db.CheckupTextMaster.Find(checkupTextMasterModel.CheckupTextMasterUID);
                if (newData == null)
                {
                    newData = new CheckupTextMaster();
                    newData.CUser = checkupTextMasterModel.CUser;
                    newData.CWhen = now;
                }
                newData.ThaiWord = checkupTextMasterModel.ThaiWord;
                newData.EngWord = checkupTextMasterModel.EngWord;
                newData.MUser = checkupTextMasterModel.MUser;
                newData.MWhen = now;
                newData.StatusFlag = "A";
                db.CheckupTextMaster.AddOrUpdate(newData);


                db.SaveChanges();
                checkupTextMasterModel.CheckupTextMasterUID = newData.UID;
                return Request.CreateResponse(HttpStatusCode.OK, checkupTextMasterModel);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("DeleteCheckupTextMaster")]
        [HttpDelete]
        public HttpResponseMessage DeleteCheckupTextMaster(int checkupTextMasterUID, int userUID)
        {
            try
            {
                DateTime now = DateTime.Now;
                CheckupTextMaster dataEdit = db.CheckupTextMaster.Find(checkupTextMasterUID);
                if (dataEdit != null)
                {
                    db.CheckupTextMaster.Attach(dataEdit);
                    dataEdit.StatusFlag = "D";
                    dataEdit.MUser = userUID;
                    dataEdit.MWhen = now;

                    db.SaveChanges();
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }


        [Route("GetCheckupRuleDescriptionByRuleUID")]
        [HttpGet]
        public List<CheckupRuleDescriptionModel> GetCheckupRuleDescriptionByRuleUID(int chekcupRuleUID)
        {
            List<CheckupRuleDescriptionModel> data = (from j in db.CheckupRuleDescription
                                                      join k in db.CheckupTextMaster on j.CheckupTextMasterUID equals k.UID
                                                      where j.CheckupRuleUID == chekcupRuleUID
                                                      && j.StatusFlag == "A"
                                                      select new CheckupRuleDescriptionModel
                                                      {
                                                          CheckupRuleDescriptionUID = j.UID,
                                                          CheckupRuleUID = j.CheckupRuleUID,
                                                          CheckupTextMasterUID = j.CheckupTextMasterUID,
                                                          ThaiDescription = k.ThaiWord,
                                                          EngDescription = k.EngWord
                                                      }).ToList();
            return data;
        }

        [Route("GetCheckupRuleRecommendModelByRuleUID")]
        [HttpGet]
        public List<CheckupRuleRecommendModel> GetCheckupRuleRecommendModelByRuleUID(int chekcupRuleUID)
        {
            List<CheckupRuleRecommendModel> data = (from j in db.CheckupRuleRecommend
                                                    join k in db.CheckupTextMaster on j.CheckupTextMasterUID equals k.UID
                                                    where j.CheckupRuleUID == chekcupRuleUID
                                                    && j.StatusFlag == "A"
                                                    select new CheckupRuleRecommendModel
                                                    {
                                                        CheckupRuleRecommendUID = j.UID,
                                                        CheckupRuleUID = j.CheckupRuleUID,
                                                        CheckupTextMasterUID = j.CheckupTextMasterUID,
                                                        ThaiRecommend = k.ThaiWord,
                                                        EndRecommend = k.EngWord
                                                    }).ToList();
            return data;
        }

        [Route("AddCheckupRuleDescription")]
        [HttpPost]
        public HttpResponseMessage AddCheckupRuleDescription(CheckupRuleDescriptionModel chekcupDescription, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                using (var tran = new TransactionScope())
                {
                    CheckupRuleDescription checkupRuleDesc = new CheckupRuleDescription();
                    checkupRuleDesc.CheckupRuleUID = chekcupDescription.CheckupRuleUID;
                    checkupRuleDesc.CheckupTextMasterUID = chekcupDescription.CheckupTextMasterUID;
                    checkupRuleDesc.CUser = userID;
                    checkupRuleDesc.CWhen = now;
                    checkupRuleDesc.MUser = userID;
                    checkupRuleDesc.MWhen = now;
                    checkupRuleDesc.StatusFlag = "A";
                    db.CheckupRuleDescription.Add(checkupRuleDesc);
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

        [Route("AddCheckupRuleRecommend")]
        [HttpPost]
        public HttpResponseMessage AddCheckupRuleRecommend(CheckupRuleRecommendModel chekcupRecommend, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                using (var tran = new TransactionScope())
                {
                    CheckupRuleRecommend checkupRuleRecom = new CheckupRuleRecommend();
                    checkupRuleRecom.CheckupRuleUID = chekcupRecommend.CheckupRuleUID;
                    checkupRuleRecom.CheckupTextMasterUID = chekcupRecommend.CheckupTextMasterUID;
                    checkupRuleRecom.CUser = userID;
                    checkupRuleRecom.CWhen = now;
                    checkupRuleRecom.MUser = userID;
                    checkupRuleRecom.MWhen = now;
                    checkupRuleRecom.StatusFlag = "A";
                    db.CheckupRuleRecommend.Add(checkupRuleRecom);
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

        [Route("DeleteCheckupRuleDescription")]
        [HttpDelete]
        public HttpResponseMessage DeleteCheckupRuleDescription(int checkupRuleDescriptionUID, int userUID)
        {
            try
            {
                DateTime now = DateTime.Now;
                CheckupRuleDescription dataEdit = db.CheckupRuleDescription.Find(checkupRuleDescriptionUID);
                if (dataEdit != null)
                {
                    db.CheckupRuleDescription.Attach(dataEdit);
                    dataEdit.StatusFlag = "D";
                    dataEdit.MUser = userUID;
                    dataEdit.MWhen = now;

                    db.SaveChanges();
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("DeleteCheckupRuleRecommend")]
        [HttpDelete]
        public HttpResponseMessage DeleteCheckupRuleRecommend(int checkupRuleRecommendUID, int userUID)
        {
            try
            {
                DateTime now = DateTime.Now;
                CheckupRuleRecommend dataEdit = db.CheckupRuleRecommend.Find(checkupRuleRecommendUID);
                if (dataEdit != null)
                {
                    db.CheckupRuleRecommend.Attach(dataEdit);
                    dataEdit.StatusFlag = "D";
                    dataEdit.MUser = userUID;
                    dataEdit.MWhen = now;

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

        #region CheckupProcess

        [Route("SearchPatientCheckup")]
        [HttpGet]
        public List<PatientVisitModel> SearchPatientCheckup(DateTime? dateFrom, DateTime? dateTo, long? patientUID, int? payorDetailUID, int? checkupJobUID)
        {
            List<PatientVisitModel> data = null;
            DataTable dt = SqlDirectStore.pSearchPatientCheckup(dateFrom, dateTo, patientUID, payorDetailUID, checkupJobUID);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = dt.ToList<PatientVisitModel>();
            }

            return data;
        }

        [Route("GetCompanyBranchByCheckJob")]
        [HttpGet]
        public List<LookupReferenceValueModel> GetCompanyBranchByCheckJob(int checkupJobUID)
        {
            List<LookupReferenceValueModel> companyList;
            var company = db.PatientVisit
                .Where(p => p.CheckupJobUID == checkupJobUID && p.StatusFlag == "A")
                .Select(p => new LookupReferenceValueModel
                {
                    Display = p.CompanyName
                });

            companyList = company.Distinct().ToList();

            return companyList;
        }

        [Route("GetVisitCheckupGroup")]
        [HttpPost]
        public List<PatientVisitModel> GetVisitCheckupGroup(int checkupJobUID, List<int> GPRSTUIDs)
        {
            List<PatientVisitModel> returnData = new List<PatientVisitModel>();
            List<PatientVisitModel> visitData = new List<PatientVisitModel>();
            var data1 = from pv in db.PatientVisit
                        join pa in db.Patient on pv.PatientUID equals pa.UID
                        join re in db.Request on pv.UID equals re.PatientVisitUID
                        join red in db.RequestDetail on re.UID equals red.RequestUID
                        join gps in db.RequestItemGroupResult on red.RequestitemUID equals gps.RequestItemUID
                        join rs in db.Result on red.UID equals rs.RequestDetailUID
                        where pv.CheckupJobUID == checkupJobUID
                        && pv.StatusFlag == "A"
                        && re.StatusFlag == "A"
                        && red.StatusFlag == "A"
                        && red.ORDSTUID != 2848
                        && rs.StatusFlag == "A"
                        && GPRSTUIDs.Contains(gps.GPRSTUID)
                        select new PatientVisitModel
                        {
                            PatientUID = pv.PatientUID,
                            PatientVisitUID = pv.UID,
                            Age = pa.DOBDttm.HasValue ? SqlFunction.fGetAge(pa.DOBDttm.Value) : "",
                            SEXXXUID = pa.SEXXXUID
                        };

            visitData.AddRange(data1);
            if (GPRSTUIDs.Any(p => p == 3177 || p == 3178))
            {
                var data2 = from pv in db.PatientVisit
                            join pa in db.Patient on pv.PatientUID equals pa.UID
                            join vital in db.PatientVitalSign on pv.UID equals vital.PatientVisitUID
                            where pv.CheckupJobUID == checkupJobUID
                            && pv.StatusFlag == "A"
                            && vital.StatusFlag == "A"
                            select new PatientVisitModel
                            {
                                PatientUID = pv.PatientUID,
                                PatientVisitUID = pv.UID,
                                Age = pa.DOBDttm.HasValue ? SqlFunction.fGetAge(pa.DOBDttm.Value) : "",
                                SEXXXUID = pa.SEXXXUID
                            };
                visitData.AddRange(data2);
            }

            returnData = visitData.GroupBy(p => new
            {
                p.PatientVisitUID,
                p.PatientUID
            }).Select(g => new PatientVisitModel
            {
                PatientVisitUID = g.FirstOrDefault().PatientVisitUID,
                PatientUID = g.FirstOrDefault().PatientUID,
                Age = g.FirstOrDefault().Age,
                SEXXXUID = g.FirstOrDefault().SEXXXUID,
            }).ToList();

            return returnData;
        }


        [Route("GetVisitCheckupGroupNonTran")]
        [HttpPost]
        public List<PatientVisitModel> GetVisitCheckupGroupNonTran(int checkupJobUID, List<int> GPRSTUIDs)
        {
            List<PatientVisitModel> returnData = new List<PatientVisitModel>();
            List<PatientVisitModel> visitData = new List<PatientVisitModel>();
            var data1 = from pv in db.PatientVisit
                        join pa in db.Patient on pv.PatientUID equals pa.UID
                        join re in db.Request on pv.UID equals re.PatientVisitUID
                        join red in db.RequestDetail on re.UID equals red.RequestUID
                        join gps in db.RequestItemGroupResult on red.RequestitemUID equals gps.RequestItemUID
                        join rs in db.Result on red.UID equals rs.RequestDetailUID
                        where pv.CheckupJobUID == checkupJobUID
                        && pv.StatusFlag == "A"
                        && re.StatusFlag == "A"
                        && red.StatusFlag == "A"
                        && rs.StatusFlag == "A"
                        && red.ORDSTUID != 2848
                        && GPRSTUIDs.Contains(gps.GPRSTUID)
                        select new PatientVisitModel
                        {
                            PatientUID = pv.PatientUID,
                            PatientVisitUID = pv.UID,
                            Age = pa.DOBDttm.HasValue ? SqlFunction.fGetAge(pa.DOBDttm.Value) : "",
                            SEXXXUID = pa.SEXXXUID
                        };

            visitData.AddRange(data1);
            if (GPRSTUIDs.Any(p => p == 3177 || p == 3178))
            {
                var data2 = from pv in db.PatientVisit
                            join pa in db.Patient on pv.PatientUID equals pa.UID
                            join vital in db.PatientVitalSign on pv.UID equals vital.PatientVisitUID
                            where pv.CheckupJobUID == checkupJobUID
                            && pv.StatusFlag == "A"
                            && vital.StatusFlag == "A"
                            select new PatientVisitModel
                            {
                                PatientUID = pv.PatientUID,
                                PatientVisitUID = pv.UID,
                                Age = pa.DOBDttm.HasValue ? SqlFunction.fGetAge(pa.DOBDttm.Value) : "",
                                SEXXXUID = pa.SEXXXUID
                            };
                visitData.AddRange(data2);
            }

            var distinctVisit = visitData.GroupBy(p => new
            {
                p.PatientVisitUID,
                p.PatientUID
            }).Select(g => new PatientVisitModel
            {
                PatientVisitUID = g.FirstOrDefault().PatientVisitUID,
                PatientUID = g.FirstOrDefault().PatientUID,
                Age = g.FirstOrDefault().Age,
                SEXXXUID = g.FirstOrDefault().SEXXXUID,
            }).ToList();

            returnData = (from pv in distinctVisit
                          where !db.CheckupGroupResult.Any(ck => ck.PatientVisitUID == pv.PatientVisitUID && ck.StatusFlag == "A")
                          select pv).ToList();
            return returnData;
        }

        [Route("GetVisitCheckupGroupNonConfirm")]
        [HttpPost]
        public List<PatientVisitModel> GetVisitCheckupGroupNonConfirm(int checkupJobUID, List<int> GPRSTUIDs)
        {
            List<PatientVisitModel> returnData = new List<PatientVisitModel>();
            List<PatientVisitModel> visitData = new List<PatientVisitModel>();
            var data1 = from pv in db.PatientVisit
                        join pa in db.Patient on pv.PatientUID equals pa.UID
                        join re in db.Request on pv.UID equals re.PatientVisitUID
                        join red in db.RequestDetail on re.UID equals red.RequestUID
                        join gps in db.RequestItemGroupResult on red.RequestitemUID equals gps.RequestItemUID
                        join rs in db.Result on red.UID equals rs.RequestDetailUID
                        where pv.CheckupJobUID == checkupJobUID
                        && pv.StatusFlag == "A"
                        && re.StatusFlag == "A"
                        && red.StatusFlag == "A"
                        && rs.StatusFlag == "A"
                        && red.ORDSTUID != 2848
                        && GPRSTUIDs.Contains(gps.GPRSTUID)
                        select new PatientVisitModel
                        {
                            PatientUID = pv.PatientUID,
                            PatientVisitUID = pv.UID,
                            Age = pa.DOBDttm.HasValue ? SqlFunction.fGetAge(pa.DOBDttm.Value) : "",
                            SEXXXUID = pa.SEXXXUID
                        };

            visitData.AddRange(data1);
            if (GPRSTUIDs.Any(p => p == 3177 || p == 3178))
            {
                var data2 = from pv in db.PatientVisit
                            join pa in db.Patient on pv.PatientUID equals pa.UID
                            join vital in db.PatientVitalSign on pv.UID equals vital.PatientVisitUID
                            where pv.CheckupJobUID == checkupJobUID
                            && pv.StatusFlag == "A"
                            && vital.StatusFlag == "A"
                            select new PatientVisitModel
                            {
                                PatientUID = pv.PatientUID,
                                PatientVisitUID = pv.UID,
                                Age = pa.DOBDttm.HasValue ? SqlFunction.fGetAge(pa.DOBDttm.Value) : "",
                                SEXXXUID = pa.SEXXXUID
                            };
                visitData.AddRange(data2);
            }

            var distinctVisit = visitData.GroupBy(p => new
            {
                p.PatientVisitUID,
                p.PatientUID
            }).Select(g => new PatientVisitModel
            {
                PatientVisitUID = g.FirstOrDefault().PatientVisitUID,
                PatientUID = g.FirstOrDefault().PatientUID,
                Age = g.FirstOrDefault().Age,
                SEXXXUID = g.FirstOrDefault().SEXXXUID,
            }).ToList();

            returnData = (from pv in distinctVisit
                          where !db.WellnessData.Any(ck => ck.PatientVisitUID == pv.PatientVisitUID && ck.StatusFlag == "A")
                          select pv).ToList();
            return returnData;
        }

        [Route("GetCheckupGroupByVisitUID")]
        [HttpGet]
        public List<LookupReferenceValueModel> GetCheckupGroupByVisitUID(long patientVisitUID)
        {
            List<LookupReferenceValueModel> groupResult = new List<LookupReferenceValueModel>();
            var data = (from re in db.Request
                        join red in db.RequestDetail on re.UID equals red.RequestUID
                        join gps in db.RequestItemGroupResult on red.RequestitemUID equals gps.RequestItemUID
                        where re.StatusFlag == "A"
                         && re.StatusFlag == "A"
                         && red.StatusFlag == "A"
                         && gps.StatusFlag == "A"
                         && re.PatientVisitUID == patientVisitUID
                         && red.ORDSTUID != 2848
                        select new LookupReferenceValueModel
                        {
                            Key = gps.GPRSTUID,
                            Display = gps.GroupResultName
                        });

            LookupReferenceValueModel bmi = new LookupReferenceValueModel();
            bmi.Key = 3177;
            bmi.Display = "ตรวจค่าวัดดัชนีมวลกาย (BMI)";

            LookupReferenceValueModel bp_pluse = new LookupReferenceValueModel();
            bp_pluse.Key = 3178;
            bp_pluse.Display = "ตรวจวัดความดันโลหิต (BP/Pulse)";

            groupResult.Add(bmi);
            groupResult.Add(bp_pluse);

            var groupDistinct = data.Distinct().ToList();
            groupResult.AddRange(groupDistinct);

            PatientVisit patientVisit = db.PatientVisit.Find(patientVisitUID);
            if (patientVisit.CheckupJobUID != null)
            {
                foreach (var result in groupResult)
                {
                    var task = db.CheckupJobTask.FirstOrDefault(p => p.GPRSTUID == result.Key && p.CheckupJobContactUID == patientVisit.CheckupJobUID);
                    if (task != null)
                        result.DisplayOrder = task.DisplayOrder ?? 0;
                }

                groupResult = groupResult.OrderBy(p => p.DisplayOrder).ToList();
            }
            else
            {
                groupResult = groupResult.OrderBy(p => p.Display).ToList();
            }

            return groupResult;
        }

        [Route("GetCheckupRuleGroupList")]
        [HttpPost]
        public List<CheckupRuleModel> GetCheckupRuleGroupList(List<int> GPRSTUIDs)
        {
            var dataCheckupRule = db.CheckupRule
                .Where(p => GPRSTUIDs.Contains(p.GPRSTUID) && p.StatusFlag == "A")
                .Select(p => new CheckupRuleModel
                {
                    CheckupRuleUID = p.UID,
                    Name = p.Name,
                    SEXXXUID = p.SEXXXUID,
                    AgeFrom = p.AgeFrom,
                    AgeTo = p.AgeTo,
                    RABSTSUID = p.RABSTSUID,
                    GPRSTUID = p.GPRSTUID
                }).ToList();
            if (dataCheckupRule != null)
            {
                foreach (var item in dataCheckupRule)
                {
                    item.CheckupRuleDescription = (from desc in db.CheckupRuleDescription
                                                   join text in db.CheckupTextMaster on desc.CheckupTextMasterUID equals text.UID
                                                   where desc.CheckupRuleUID == item.CheckupRuleUID
                                                   && desc.StatusFlag == "A"
                                                   select new CheckupRuleDescriptionModel
                                                   {
                                                       CheckupRuleUID = desc.CheckupRuleUID,
                                                       CheckupTextMasterUID = desc.CheckupTextMasterUID,
                                                       CheckupRuleDescriptionUID = desc.UID,
                                                       ThaiDescription = text.ThaiWord,
                                                       EngDescription = text.EngWord
                                                   }).ToList();

                    item.CheckupRuleRecommend = (from recom in db.CheckupRuleRecommend
                                                 join text in db.CheckupTextMaster on recom.CheckupTextMasterUID equals text.UID
                                                 where recom.CheckupRuleUID == item.CheckupRuleUID
                                                 && recom.StatusFlag == "A"
                                                 select new CheckupRuleRecommendModel
                                                 {
                                                     CheckupRuleUID = recom.CheckupRuleUID,
                                                     CheckupTextMasterUID = recom.CheckupTextMasterUID,
                                                     CheckupRuleRecommendUID = recom.UID,
                                                     ThaiRecommend = text.ThaiWord,
                                                     EndRecommend = text.EngWord
                                                 }).ToList();

                    item.CheckupRuleItem = (from itm in db.CheckupRuleItem
                                            where itm.CheckupRuleUID == item.CheckupRuleUID
                                            && itm.StatusFlag == "A"
                                            select new CheckupRuleItemModel
                                            {
                                                CheckupRuleItemUID = itm.UID,
                                                CheckupRuleUID = itm.CheckupRuleUID,
                                                ResultItemUID = itm.ResultItemUID,
                                                Low = itm.Low,
                                                Hight = itm.Hight,
                                                Text = itm.Text,
                                                Operator = itm.Operator,
                                                NonCheckup = itm.NonCheckup
                                            }).ToList();
                }
            }

            return dataCheckupRule;
        }

        [Route("GetGroupResultComponentByVisitUID")]
        public List<ResultComponentModel> GetGroupResultComponentByVisitUID(long patientVisitUID, int GPRSTUID)
        {
            var resultComponent = (from rs in db.Result
                                   join rsc in db.ResultComponent on rs.UID equals rsc.ResultUID
                                   join red in db.RequestDetail on rs.RequestDetailUID equals red.UID
                                   join gps in db.RequestItemGroupResult on red.RequestitemUID equals gps.RequestItemUID
                                   join rti in db.RequestItem on red.RequestitemUID equals rti.UID
                                   where rs.PatientVisitUID == patientVisitUID
                                   && rsc.StatusFlag == "A"
                                   && red.StatusFlag == "A"
                                   && rs.StatusFlag == "A"
                                   && red.ORDSTUID != 2848
                                   && gps.GPRSTUID == GPRSTUID
                                   select new ResultComponentModel
                                   {
                                       ResultComponentUID = rsc.UID,
                                       ResultItemUID = rsc.ResultItemUID,
                                       ResultItemCode = rsc.ResultItemCode,
                                       ResultItemName = rsc.ResultItemName,
                                       ResultValue = rsc.ResultValue,
                                       TestType = SqlFunction.fGetRfValDescription(rti.TSTTPUID ?? 0)
                                   }).ToList();

            return resultComponent;
        }

        [Route("GetCheckupMobileResultByVisitUID")]
        [HttpGet]
        public List<PatientResultComponentModel> GetCheckupMobileResultByVisitUID(long patientUID, long patientVisitUID)
        {
            DataTable dtData = SqlDirectStore.pGetCheckupMobileResult(patientUID, patientVisitUID);

            List<PatientResultComponentModel> data = dtData.ToList<PatientResultComponentModel>();

            return data;
        }

        [Route("SaveChekcupGroupResult")]
        [HttpPost]
        public HttpResponseMessage SaveChekcupGroupResult(CheckupGroupResultModel groupResult, int userUID)
        {
            try
            {
                CheckupGroupResult checkupTran = db.CheckupGroupResult.FirstOrDefault(p => p.PatientVisitUID == groupResult.PatientVisitUID
                && p.GPRSTUID == groupResult.GPRSTUID);
                if (checkupTran == null)
                {
                    checkupTran = new CheckupGroupResult();
                    checkupTran.CUser = userUID;
                    checkupTran.CWhen = DateTime.Now;
                    checkupTran.StatusFlag = "A";
                }
                checkupTran.PatientUID = groupResult.PatientUID;
                checkupTran.PatientVisitUID = groupResult.PatientVisitUID;
                checkupTran.GPRSTUID = groupResult.GPRSTUID;
                checkupTran.RABSTSUID = groupResult.RABSTSUID;
                checkupTran.Description = groupResult.Description;
                checkupTran.Recommend = groupResult.Recommend;
                checkupTran.Conclusion = groupResult.Conclusion;
                checkupTran.MUser = userUID;
                checkupTran.MWhen = DateTime.Now;
                db.CheckupGroupResult.AddOrUpdate(checkupTran);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("TranslateCheckupAll")]
        [HttpPost]
        public HttpResponseMessage TranslateCheckupAll(int checkupJobUID, int userUID, List<int> GPRSTUIDs)
        {
            try
            {
                List<long> dataVisitUID = new List<long>();

                var dataVisit1 = from pv in db.PatientVisit
                                 join re in db.Request on pv.UID equals re.PatientVisitUID
                                 join red in db.RequestDetail on re.UID equals red.RequestUID
                                 join gps in db.RequestItemGroupResult on red.RequestitemUID equals gps.RequestItemUID
                                 join rs in db.Result on red.UID equals rs.RequestDetailUID
                                 where pv.CheckupJobUID == checkupJobUID
                                 && pv.StatusFlag == "A"
                                 && re.StatusFlag == "A"
                                 && red.StatusFlag == "A"
                                 && rs.StatusFlag == "A"
                                 && GPRSTUIDs.Contains(gps.GPRSTUID)
                                 select pv;
                dataVisitUID.AddRange(dataVisit1.Select(p => p.UID));
                if (GPRSTUIDs.Any(p => p == 3177 || p == 3178))
                {
                    var dataViist2 = from pv in db.PatientVisit
                                     join vital in db.PatientVitalSign on pv.UID equals vital.PatientVisitUID
                                     where pv.CheckupJobUID == checkupJobUID
                                     && pv.StatusFlag == "A"
                                     && vital.StatusFlag == "A"
                                     select pv;
                    dataVisitUID.AddRange(dataViist2.Select(p => p.UID));
                }
                var visitUIDs = dataVisitUID.Distinct();

                var dataCheckupRule = db.CheckupRule
                    .Where(p => GPRSTUIDs.Contains(p.GPRSTUID) && p.StatusFlag == "A")
                    .Select(p => new CheckupRuleModel
                    {
                        CheckupRuleUID = p.UID,
                        Name = p.Name,
                        SEXXXUID = p.SEXXXUID,
                        AgeFrom = p.AgeFrom,
                        AgeTo = p.AgeTo,
                        RABSTSUID = p.RABSTSUID,
                        GPRSTUID = p.GPRSTUID
                    }).ToList();
                if (dataCheckupRule != null)
                {
                    foreach (var item in dataCheckupRule)
                    {
                        item.CheckupRuleDescription = (from desc in db.CheckupRuleDescription
                                                       join text in db.CheckupTextMaster on desc.CheckupTextMasterUID equals text.UID
                                                       where desc.CheckupRuleUID == item.CheckupRuleUID
                                                       && desc.StatusFlag == "A"
                                                       select new CheckupRuleDescriptionModel
                                                       {
                                                           CheckupRuleUID = desc.CheckupRuleUID,
                                                           CheckupTextMasterUID = desc.CheckupTextMasterUID,
                                                           CheckupRuleDescriptionUID = desc.UID,
                                                           ThaiDescription = text.ThaiWord,
                                                           EngDescription = text.EngWord
                                                       }).ToList();

                        item.CheckupRuleRecommend = (from recom in db.CheckupRuleRecommend
                                                     join text in db.CheckupTextMaster on recom.CheckupTextMasterUID equals text.UID
                                                     where recom.CheckupRuleUID == item.CheckupRuleUID
                                                     && recom.StatusFlag == "A"
                                                     select new CheckupRuleRecommendModel
                                                     {
                                                         CheckupRuleUID = recom.CheckupRuleUID,
                                                         CheckupTextMasterUID = recom.CheckupTextMasterUID,
                                                         CheckupRuleRecommendUID = recom.UID,
                                                         ThaiRecommend = text.ThaiWord,
                                                         EndRecommend = text.EngWord
                                                     }).ToList();

                        item.CheckupRuleItem = (from itm in db.CheckupRuleItem
                                                where itm.CheckupRuleUID == item.CheckupRuleUID
                                                && itm.StatusFlag == "A"
                                                select new CheckupRuleItemModel
                                                {
                                                    CheckupRuleItemUID = itm.UID,
                                                    CheckupRuleUID = itm.CheckupRuleUID,
                                                    ResultItemUID = itm.ResultItemUID,
                                                    Low = itm.Low,
                                                    Hight = itm.Hight,
                                                    Text = itm.Text,
                                                    Operator = itm.Operator,
                                                    NonCheckup = itm.NonCheckup
                                                }).ToList();
                    }
                }

                foreach (var patientVisitUID in visitUIDs)
                {

                    Patient patient = (from pa in db.Patient
                                       join pv in db.PatientVisit on pa.UID equals pv.PatientUID
                                       where pv.UID == patientVisitUID
                                       select pa).FirstOrDefault();
                    string ageString = patient.DOBDttm != null ? ShareLibrary.UtilDate.calAgeFromBirthDate(patient.DOBDttm.Value) : "";
                    int? ageInt = !string.IsNullOrEmpty(ageString) ? int.Parse(ageString) : (int?)null;
                    foreach (var grpstUID in GPRSTUIDs)
                    {
                        List<CheckupRuleModel> ruleCheckupIsCorrect = new List<CheckupRuleModel>();
                        string wellNessResult = string.Empty;
                        List<ResultComponent> resultComponent = null;
                        if (grpstUID != 3177 && grpstUID != 3178)
                        {
                            var result = (from rs in db.Result
                                          join red in db.RequestDetail on rs.RequestDetailUID equals red.UID
                                          join gps in db.RequestItemGroupResult on red.RequestitemUID equals gps.RequestItemUID
                                          where rs.PatientVisitUID == patientVisitUID
                                          && gps.GPRSTUID == grpstUID
                                          && red.StatusFlag == "A"
                                          && rs.StatusFlag == "A"
                                          select rs).FirstOrDefault();

                            if (result != null)
                                resultComponent = (from rsc in db.ResultComponent
                                                   where rsc.ResultUID == result.UID
                                                   && rsc.StatusFlag == "A"
                                                   select rsc).ToList();
                        }
                        else if (grpstUID == 3177 || grpstUID == 3178)
                        {
                            resultComponent = new List<ResultComponent>();
                            ResultComponent bmiComponent = new ResultComponent() { ResultItemUID = 328, ResultItemCode = "PEBMI", ResultItemName = "", ResultValue = "" };
                            ResultComponent sdpComponent = new ResultComponent() { ResultItemUID = 329, ResultItemCode = "PESBP", ResultItemName = "", ResultValue = "" };
                            ResultComponent dbpComponent = new ResultComponent() { ResultItemUID = 330, ResultItemCode = "PEDBP", ResultItemName = "", ResultValue = "" };
                            ResultComponent pluseComponent = new ResultComponent() { ResultItemUID = 331, ResultItemCode = "PEPLUSE", ResultItemName = "", ResultValue = "" };

                            resultComponent.Add(bmiComponent);
                            resultComponent.Add(dbpComponent);
                            resultComponent.Add(sdpComponent);
                            resultComponent.Add(pluseComponent);
                        }


                        var ruleCheckups = dataCheckupRule
                            .Where(p => p.GPRSTUID == grpstUID
                            && (p.SEXXXUID == 3 || p.SEXXXUID == patient.SEXXXUID)
                            && ((p.AgeFrom == null && p.AgeTo == null) || (ageInt > p.AgeFrom && ageInt < p.AgeTo)
                            || (ageInt > p.AgeFrom && p.AgeTo == null) || (p.AgeFrom == null && ageInt < p.AgeTo))
                            ).ToList();
                        foreach (var ruleCheckup in ruleCheckups)
                        {
                            foreach (var ruleItem in ruleCheckup.CheckupRuleItem)
                            {
                                var resultItemValue = resultComponent.FirstOrDefault(p => p.ResultItemUID == ruleItem.ResultItemUID);

                                if (resultItemValue != null)
                                {
                                    if (!string.IsNullOrEmpty(ruleItem.Text))
                                    {
                                        if (resultItemValue.ResultValue.Trim() == ruleItem.Text.Trim())
                                        {

                                            ruleCheckupIsCorrect.Add(ruleCheckup);
                                            if (ruleItem.Operator == "Or")
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        double resultValueNumber;
                                        if (double.TryParse(resultItemValue.ResultValue, out resultValueNumber))
                                        {
                                            if ((resultValueNumber >= ruleItem.Low && resultValueNumber <= ruleItem.Hight)
                                                || (resultValueNumber >= ruleItem.Low && ruleItem.Hight == null)
                                                || (ruleItem.Low == null && resultValueNumber <= ruleItem.Hight)
                                                )
                                            {
                                                ruleCheckupIsCorrect.Add(ruleCheckup);
                                                if (ruleItem.Operator == "Or")
                                                {
                                                    break;
                                                }
                                            }
                                        }

                                    }
                                }

                            }

                        }


                        int RABSTSUID = 0;
                        List<CheckupRuleDescriptionModel> descriptions = new List<CheckupRuleDescriptionModel>();
                        List<CheckupRuleRecommendModel> recommands = new List<CheckupRuleRecommendModel>();
                        if (ruleCheckupIsCorrect.Any(p => p.RABSTSUID == 2882))
                        {
                            RABSTSUID = 2882;
                            foreach (var item in ruleCheckupIsCorrect.Where(p => p.RABSTSUID != 2883))
                            {
                                descriptions.AddRange(item.CheckupRuleDescription);
                                recommands.AddRange(item.CheckupRuleRecommend);
                            }

                        }
                        else
                        {
                            RABSTSUID = 2883;
                            foreach (var item in ruleCheckupIsCorrect)
                            {
                                descriptions.AddRange(item.CheckupRuleDescription);
                                recommands.AddRange(item.CheckupRuleRecommend);
                            }
                        }

                        var descriptionGroup = descriptions.GroupBy(p => new
                        {
                            p.CheckupTextMasterUID
                        })
                        .Select(g => new
                        {
                            ThaiDescription = g.FirstOrDefault().ThaiDescription,
                            EngDescription = g.FirstOrDefault().EngDescription,
                        });

                        var recommandGroup = recommands.GroupBy(p => new
                        {
                            p.CheckupTextMasterUID
                        }).Select(g => new
                        {
                            ThaiRecommend = g.FirstOrDefault().ThaiRecommend,
                            EndRecommend = g.FirstOrDefault().EndRecommend,
                        });



                        string descriptionString = string.Empty;
                        string recommandString = string.Empty;
                        foreach (var item in descriptionGroup)
                        {
                            descriptionString += string.IsNullOrEmpty(descriptionString) ? item.ThaiDescription : " " + item.ThaiDescription;
                        }

                        foreach (var item in recommandGroup)
                        {
                            recommandString += string.IsNullOrEmpty(recommandString) ? item.ThaiRecommend : " " + item.ThaiRecommend;
                        }
                        CheckupGroupResult checkupTran = db.CheckupGroupResult.FirstOrDefault(p => p.PatientVisitUID == patientVisitUID);
                        if (checkupTran == null)
                        {
                            checkupTran = new CheckupGroupResult();
                            checkupTran.CUser = userUID;
                            checkupTran.CWhen = DateTime.Now;
                            checkupTran.StatusFlag = "A";
                        }
                        checkupTran.PatientUID = patient.UID;
                        checkupTran.PatientVisitUID = patientVisitUID;
                        checkupTran.GPRSTUID = grpstUID;
                        checkupTran.RABSTSUID = RABSTSUID;
                        checkupTran.Description = descriptionString;
                        checkupTran.Recommend = recommandString;
                        checkupTran.Conclusion = descriptionString + " " + recommandString;
                        checkupTran.MUser = userUID;
                        checkupTran.MWhen = DateTime.Now;
                        db.CheckupGroupResult.AddOrUpdate(checkupTran);
                        db.SaveChanges();
                    }


                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }

        }

        [Route("GetCheckupGroupResultByJob")]
        [HttpGet]
        public List<PatientResultCheckupModel> GetCheckupGroupResultByJob(int checkupJobUID, int GPRSTUID,string companyName, int? startRow, int? endRow)
        {
            DataTable dtData = SqlDirectStore.pGetCheckupGroupResult(checkupJobUID, GPRSTUID, companyName, startRow,endRow);

            List<PatientResultCheckupModel> data = dtData.ToList<PatientResultCheckupModel>();

            return data;
        }

        [Route("GetResultCumulative")]
        [HttpGet]
        public List<PatientResultComponentModel> GetResultCumulative(long patientUID,long patientVisitUID, int requestItemUID)
        {
            List<PatientResultComponentModel> data = null;
            DataTable dt = SqlDirectStore.pGetResultCumulative(patientUID, patientVisitUID, requestItemUID);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = dt.ToList<PatientResultComponentModel>();
            }

            return data;
        }

        [Route("GetGroupResultCumulative")]
        [HttpGet]
        public List<PatientResultComponentModel> GetGroupResultCumulative(long patientUID, long patientVisitUID, int GPRSTUID, int? payorDetailUID)
        {
            List<PatientResultComponentModel> data = null;
            DataTable dt = SqlDirectStore.pGetGroupResultCumulative(patientUID, patientVisitUID, GPRSTUID, payorDetailUID);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = dt.ToList<PatientResultComponentModel>();
            }

            return data;
        }

        [Route("GetVitalSignCumulative")]
        [HttpGet]
        public List<PatientResultComponentModel> GetVitalSignCumulative(long patientUID,long patientVisitUID)
        {
            List<PatientResultComponentModel> data = null;
            DataTable dt = SqlDirectStore.pGetVitalSignCumulative(patientUID, patientVisitUID);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = dt.ToList<PatientResultComponentModel>();
            }

            return data;
        }

        [Route("GetCheckupGroupResultByVisit")]
        [HttpGet]
        public CheckupGroupResultModel GetCheckupGroupResultByVisit(long patientVisitUID, int GPRSTUID)
        {
            CheckupGroupResultModel data = db.CheckupGroupResult
                .Where(p => p.PatientVisitUID == patientVisitUID && p.GPRSTUID == GPRSTUID)
                .Select(p => new CheckupGroupResultModel
                {
                    CheckupGroupResultUID = p.UID,
                    PatientUID = p.PatientUID,
                    PatientVisitUID = p.PatientVisitUID,
                    GPRSTUID = p.GPRSTUID,
                    RABSTSUID = p.RABSTSUID,
                    Conclusion = p.Conclusion,
                    Recommend = p.Recommend,

                }).FirstOrDefault();

            return data;
        }

        [Route("GetCheckupGroupResultListByVisit")]
        [HttpGet]
        public List<CheckupGroupResultModel> GetCheckupGroupResultListByVisit(long patientUID, long patientVisitUID)
        {
            List<CheckupGroupResultModel> data = (from ck in db.CheckupGroupResult
                                                  join rf in db.ReferenceValue on ck.GPRSTUID equals rf.UID
                                                  where ck.PatientUID == patientUID
                                                  && ck.PatientVisitUID == patientVisitUID
                                                  && ck.StatusFlag == "A"
                                                  select new CheckupGroupResultModel
                                                  {
                                                      CheckupGroupResultUID = ck.UID,
                                                      PatientUID = ck.PatientUID,
                                                      PatientVisitUID = ck.PatientVisitUID,
                                                      GPRSTUID = ck.GPRSTUID,
                                                      RABSTSUID = ck.RABSTSUID,
                                                      Conclusion = ck.Conclusion,
                                                      GroupResult = rf.Description,
                                                      ResultStatus = ck.RABSTSUID == 2882 ? "ผิดปกติ" : ck.RABSTSUID == 2885 ? "เฝ้าระวัง":  "ปกติ",
                                                      Recommend = ck.Recommend,
                                                      Description = ck.Description,
                                                      GroupCode = rf.ValueCode
                                                  }).ToList();

            return data;
        }

        #endregion
    }
}