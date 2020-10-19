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
                    PayorName = SqlFunction.fGetPayorName(p.PayorDetailUID),
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
                    PayorName = SqlFunction.fGetPayorName(p.PayorDetailUID),
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
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [HttpPost]
        public HttpResponseMessage ManageCheckupJobContact(CheckupJobContactModel checkupJobContactModel, int userID)
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
                    checkupJob.PayorName = checkupJobContactModel.PayorName;
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