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
            List<CheckupJobContactModel> data = db.CheckupJobContact.Where(p => p.StatusFlag == "A")
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
    }
}