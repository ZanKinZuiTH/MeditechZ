using MediTech.DataBase;
using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Data.Entity.Migrations;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Transactions;
using System.Reflection;
using ShareLibrary;
using System.Data.Entity;

namespace MediTechWebApi.Controllers
{
    [RoutePrefix("Api/PatientHistory")]
    public class PatientHistoryController : ApiController
    {
        protected MediTechEntities db = new MediTechEntities();

        #region PatientVitalSign

        [Route("SearchPatientVitalSign")]
        [HttpGet]
        public List<PatientVitalSignModel> SearchPatientVitalSign(long patientUID, DateTime dateFrom, DateTime dateTo)
        {

            List<PatientVitalSignModel> data = db.PatientVitalSign
                .Where(p => p.PatientUID == patientUID
                && p.StatusFlag == "A"
                && (dateFrom == null || DbFunctions.TruncateTime(p.RecordedDttm) >= DbFunctions.TruncateTime(dateFrom))
                && (dateTo == null || DbFunctions.TruncateTime(p.RecordedDttm) <= DbFunctions.TruncateTime(dateTo)))
                .Select(p => new PatientVitalSignModel
                {
                    PatientVitalSignUID = p.UID,
                    PatientUID = p.PatientUID,
                    PatientVisitUID = p.PatientVisitUID,
                    Weight = p.Weight,
                    Height = p.Height,
                    RespiratoryRate = p.RespiratoryRate,
                    Temprature = p.Temprature,
                    Pulse = p.Pulse,
                    RecordedDttm = p.RecordedDttm,
                    BMIValue = p.BMIValue,
                    BSAValue = p.BSAValue,
                    BPSys = p.BPSys,
                    BPDio = p.BPDio,
                    OxygenSat = p.OxygenSat,
                    WaistCircumference = p.WaistCircumference,
                    IsPregnant = p.IsPregnant,
                    Comments = p.Comments,
                    RecordedBy = SqlFunction.fGetCareProviderName(p.CUser)
                }).ToList();

            return data;
        }

        [Route("GetPatientVitalSignByUID")]
        [HttpGet]
        public List<PatientVitalSignModel> GetPatientVitalSignByUID(long patientVitalSignUID)
        {
            List<PatientVitalSignModel> data = db.PatientVitalSign
                .Where(p => p.UID == patientVitalSignUID)
                .Select(p => new PatientVitalSignModel
                {
                    PatientVitalSignUID = p.UID,
                    PatientUID = p.PatientUID,
                    PatientVisitUID = p.PatientVisitUID,
                    Weight = p.Weight,
                    Height = p.Height,
                    RespiratoryRate = p.RespiratoryRate,
                    Temprature = p.Temprature,
                    Pulse = p.Pulse,
                    RecordedDttm = p.RecordedDttm,
                    BMIValue = p.BMIValue,
                    BSAValue = p.BSAValue,
                    BPSys = p.BPSys,
                    BPDio = p.BPDio,
                    OxygenSat = p.OxygenSat,
                    WaistCircumference = p.WaistCircumference,
                    IsPregnant = p.IsPregnant,
                    Comments = p.Comments,
                    RecordedBy = SqlFunction.fGetCareProviderName(p.CUser)
                }).ToList();

            return data;
        }

        [Route("GetPatientVitalSignByVisitUID")]
        [HttpGet]
        public List<PatientVitalSignModel> GetPatientVitalSignByVisitUID(long patientVisitUID)
        {
            List<PatientVitalSignModel> data = db.PatientVitalSign
                .Where(p => p.StatusFlag == "A" && p.PatientVisitUID == patientVisitUID)
                .Select(p => new PatientVitalSignModel
                {
                    PatientVitalSignUID = p.UID,
                    PatientUID = p.PatientUID,
                    PatientVisitUID = p.PatientVisitUID,
                    Weight = p.Weight,
                    Height = p.Height,
                    RespiratoryRate = p.RespiratoryRate,
                    Temprature = p.Temprature,
                    Pulse = p.Pulse,
                    RecordedDttm = p.RecordedDttm,
                    BMIValue = p.BMIValue,
                    BSAValue = p.BSAValue,
                    BPSys = p.BPSys,
                    BPDio = p.BPDio,
                    OxygenSat = p.OxygenSat,
                    WaistCircumference = p.WaistCircumference,
                    IsPregnant = p.IsPregnant,
                    Comments = p.Comments,
                    RecordedBy = SqlFunction.fGetCareProviderName(p.CUser)
                }).ToList();

            return data;
        }

        [Route("ManagePatientVitalSign")]
        [HttpPost]
        public HttpResponseMessage ManagePatientVitalSign(PatientVitalSignModel model, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;


                PatientVitalSign vitalsign = db.PatientVitalSign.Find(model.PatientVitalSignUID);

                if (vitalsign == null)
                {
                    vitalsign = new PatientVitalSign();
                    vitalsign.CUser = userID;
                    vitalsign.CWhen = now;
                }

                vitalsign.PatientUID = model.PatientUID;
                vitalsign.PatientVisitUID = model.PatientVisitUID;
                vitalsign.Weight = model.Weight;
                vitalsign.Height = model.Height;
                vitalsign.RespiratoryRate = model.RespiratoryRate;
                vitalsign.Temprature = model.Temprature;
                vitalsign.Pulse = model.Pulse;
                vitalsign.RecordedDttm = model.RecordedDttm;
                vitalsign.BMIValue = model.BMIValue;
                vitalsign.BSAValue = model.BSAValue;
                vitalsign.BPSys = model.BPSys;
                vitalsign.BPDio = model.BPDio;
                vitalsign.OxygenSat = model.OxygenSat;
                vitalsign.WaistCircumference = model.WaistCircumference;
                vitalsign.IsPregnant = model.IsPregnant;
                vitalsign.Comments = model.Comments;
                vitalsign.MUser = userID;
                vitalsign.MWhen = now;
                vitalsign.StatusFlag = "A";

                db.PatientVitalSign.AddOrUpdate(vitalsign);
                db.SaveChanges();


                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("DeletePatientVitalSign")]
        [HttpDelete]
        public HttpResponseMessage DeletePatientVitalSign(int patientVitalSignUID, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                PatientVitalSign patVital = db.PatientVitalSign.Find(patientVitalSignUID);
                if (patVital != null)
                {
                    db.PatientVitalSign.Attach(patVital);
                    patVital.MUser = userID;
                    patVital.MWhen = now;
                    patVital.StatusFlag = "D";
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

        #region CCHPI

        [Route("GetCCHPIMaster")]
        [HttpGet]
        public List<CCHPIMasterModel> GetCCHPIMaster(string type)
        {
            var cchpiMaster = db.CCHPIMaster.Where(p => p.StatusFlag == "A" && p.Type == type)
                .Select(p => new CCHPIMasterModel
                {
                    CCHPIMasterUID = p.UID,
                    Name = p.Name,
                    Description = p.Description,
                    CUser = p.CUser,
                    CWhen = p.CWhen,
                    MWhen = p.MWhen,
                    MUser = p.MUser
                }).ToList();

            return cchpiMaster;
        }

        [Route("GetCCHPIByVisit")]
        [HttpGet]
        public List<CCHPIModel> GetCCHPIByVisit(long patientVisitUID)
        {
            var cchpiVisit = db.CCHPI.Where(p => p.StatusFlag == "A" && p.PatientVisitUID == patientVisitUID)
                .Select(p => new CCHPIModel
                {
                    CCHPIUID = p.UID,
                    CCHPIMasterUID = p.CCHPIMasterUID,
                    Complaint = p.Complaint,
                    Presentillness = p.Presentillness,
                    PatientUID = p.PatientUID,
                    PatientVisitUID = p.PatientVisitUID,
                    AGUOMUID = p.AGUOMUID,
                    DateUnit = SqlFunction.fGetRfValDescription(p.AGUOMUID ?? 0),
                    Period = p.Period,
                    CUser = p.CUser,
                    CWhen = p.CWhen,
                    MUser = p.MUser,
                    MWhen = p.MWhen,
                    RecordBy = SqlFunction.fGetCareProviderName(p.MUser)
                }).ToList();

            return cchpiVisit;
        }

        [Route("GetCCHPIByPatientUID")]
        [HttpGet]
        public List<CCHPIModel> GetCCHPIByPatientUID(long patientUID)
        {
            var cchpiVisit = (from pv in db.PatientVisit
                              join cc in db.CCHPI on pv.UID equals cc.PatientVisitUID
                              where pv.StatusFlag == "A"
                              && cc.StatusFlag == "A"
                              && pv.PatientUID == patientUID
                              select new CCHPIModel
                              {
                                  CCHPIUID = cc.UID,
                                  CCHPIMasterUID = cc.CCHPIMasterUID,
                                  Complaint = cc.Complaint,
                                  Presentillness = cc.Presentillness,
                                  PatientUID = cc.PatientUID,
                                  PatientVisitUID = cc.PatientVisitUID,
                                  AGUOMUID = cc.AGUOMUID,
                                  DateUnit = SqlFunction.fGetRfValDescription(cc.AGUOMUID ?? 0),
                                  Period = cc.Period,
                                  CUser = cc.CUser,
                                  CWhen = cc.CWhen,
                                  MUser = cc.MUser,
                                  MWhen = cc.MWhen,
                                  RecordBy = SqlFunction.fGetCareProviderName(cc.MUser)
                              }).ToList();

            return cchpiVisit;
        }

        [Route("ManageCCHPI")]
        [HttpPost]
        public HttpResponseMessage ManageCCHPI(CCHPIModel model, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;


                CCHPI patCCHPI = db.CCHPI.Find(model.CCHPIUID);
                if (patCCHPI == null)
                {
                    patCCHPI = new CCHPI();
                    patCCHPI.CUser = userID;
                    patCCHPI.CWhen = now;
                    patCCHPI.StatusFlag = "A";
                }
                patCCHPI.MUser = userID;
                patCCHPI.MWhen = now;
                patCCHPI.PatientUID = model.PatientUID;
                patCCHPI.PatientVisitUID = model.PatientVisitUID;
                patCCHPI.Complaint = model.Complaint;
                patCCHPI.CCHPIMasterUID = model.CCHPIMasterUID;
                patCCHPI.Presentillness = model.Presentillness;
                patCCHPI.Period = model.Period;
                patCCHPI.AGUOMUID = model.AGUOMUID;
                db.CCHPI.AddOrUpdate(patCCHPI);
                db.SaveChanges();


                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("ManageCCHPIMaster")]
        [HttpPost]
        public HttpResponseMessage ManageCCHPIMaster(CCHPIMasterModel model, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;


                CCHPIMaster cchpiMaster = db.CCHPIMaster.Find(model.CCHPIMasterUID);
                if (cchpiMaster == null)
                {
                    cchpiMaster = new CCHPIMaster();
                    cchpiMaster.CUser = userID;
                    cchpiMaster.CWhen = now;
                    cchpiMaster.StatusFlag = "A";
                }
                cchpiMaster.Name = model.Name;
                cchpiMaster.Description = model.Description;
                cchpiMaster.Type = model.Type;
                cchpiMaster.MUser = userID;
                cchpiMaster.MWhen = now;
                db.CCHPIMaster.AddOrUpdate(cchpiMaster);
                db.SaveChanges();


                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("DeleteCCHPIMaster")]
        [HttpDelete]
        public HttpResponseMessage DeleteCCHPIMaster(int CCHPIMasterUID, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;


                CCHPIMaster cchpiMaster = db.CCHPIMaster.Find(CCHPIMasterUID);
                if (cchpiMaster != null)
                {
                    db.CCHPIMaster.Attach(cchpiMaster);
                    cchpiMaster.MUser = userID;
                    cchpiMaster.MWhen = now;
                    cchpiMaster.StatusFlag = "D";
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

        #region PhysicalExam

        [Route("GetPhysicalExamTemplate")]
        [HttpGet]
        public List<PhysicalExamTemplateModel> GetPhysicalExamTemplate()
        {
            var phyexamTem = db.PhysicalExamTemplate.Where(p => p.StatusFlag == "A")
                .Select(p => new PhysicalExamTemplateModel
                {
                    PhysicalExamTemplateUID = p.UID,
                    Name = p.Name,
                    TemplateValue = p.TemplateValue,
                    CUser = p.CUser,
                    CWhen = p.CWhen
                }).ToList();

            return phyexamTem;
        }

        [Route("GetPhysicalExamByPatientUID")]
        [HttpGet]
        public List<PhysicalExamModel> GetPhysicalExamByPatientUID(long patientUID)
        {
            var physicalExam = (from pv in db.PatientVisit
                                join pe in db.PhysicalExam on pv.UID equals pe.PatientVisitUID
                                where pv.StatusFlag == "A"
                                && pe.StatusFlag == "A"
                                && pv.PatientUID == patientUID
                                select new PhysicalExamModel
                                {
                                    PhysicalExamUID = pe.UID,
                                    PatientUID = pe.PatientUID,
                                    PatientVisitUID = pe.PatientVisitUID,
                                    PlainText = pe.PlainText,
                                    Value = pe.Value,
                                    CUser = pe.CUser,
                                    CWhen = pe.CWhen,
                                    MUser = pe.MUser,
                                    MWhen = pe.MWhen,
                                    RecordBy = SqlFunction.fGetCareProviderName(pe.MUser)
                                }).ToList();
            return physicalExam;
        }

        [Route("GetPhysicalExamByVisit")]
        [HttpGet]
        public List<PhysicalExamModel> GetPhysicalExamByVisit(long patientVisitUID)
        {
            var physicalExam = db.PhysicalExam.Where(p => p.StatusFlag == "A"
                && p.PatientVisitUID == patientVisitUID)
                .Select(p => new PhysicalExamModel
                {
                    PhysicalExamUID = p.UID,
                    PatientUID = p.PatientUID,
                    PatientVisitUID = p.PatientVisitUID,
                    PlainText = p.PlainText,
                    Value = p.Value,
                    CUser = p.CUser,
                    CWhen = p.CWhen,
                    MUser = p.MUser,
                    MWhen = p.MWhen,
                    RecordBy = SqlFunction.fGetCareProviderName(p.MUser)
                }).ToList();

            return physicalExam;
        }

        [Route("ManagePhysicalExam")]
        [HttpPost]
        public HttpResponseMessage ManagePhysicalExam(PhysicalExamModel model, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;


                PhysicalExam pe = db.PhysicalExam.Find(model.PhysicalExamUID);
                if (pe == null)
                {
                    pe = new PhysicalExam();
                    pe.CUser = userID;
                    pe.CWhen = now;
                    pe.StatusFlag = "A";
                }
                pe.MUser = userID;
                pe.MWhen = now;
                pe.PatientUID = model.PatientUID;
                pe.PatientVisitUID = model.PatientVisitUID;
                pe.PlainText = model.PlainText;
                pe.Value = model.Value;
                db.PhysicalExam.AddOrUpdate(pe);
                db.SaveChanges();


                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("ManagePhysicalExamTemplate")]
        [HttpPost]
        public HttpResponseMessage ManagePhysicalExamTemplate(PhysicalExamTemplateModel model, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;


                PhysicalExamTemplate peTemp = db.PhysicalExamTemplate.Find(model.PhysicalExamTemplateUID);
                if (peTemp == null)
                {
                    peTemp = new PhysicalExamTemplate();
                    peTemp.CUser = userID;
                    peTemp.CWhen = now;
                    peTemp.StatusFlag = "A";
                }
                peTemp.MUser = userID;
                peTemp.MWhen = now;
                peTemp.Name = model.Name;
                peTemp.TemplateValue = model.TemplateValue;
                db.PhysicalExamTemplate.AddOrUpdate(peTemp);
                db.SaveChanges();


                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("DeletePhysicalExamTemplate")]
        [HttpDelete]
        public HttpResponseMessage DeletePhysicalExamTemplate(int physicalExamTemplateUID, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;


                PhysicalExamTemplate peTemplate = db.PhysicalExamTemplate.Find(physicalExamTemplateUID);
                if (peTemplate != null)
                {
                    db.PhysicalExamTemplate.Attach(peTemplate);
                    peTemplate.MUser = userID;
                    peTemplate.MWhen = now;
                    peTemplate.StatusFlag = "D";
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

        #region ProgressNote

        [Route("GetProgressNoteByPatientUID")]
        [HttpGet]
        public List<ProgressNoteModel> GetProgressNoteByPatientUID(long patientUID)
        {
            var progressNote = (from pv in db.PatientVisit
                                join pn in db.ProgressNote on pv.UID equals pn.PatientVisitUID
                                where pv.StatusFlag == "A"
                                && pn.StatusFlag == "A"
                                && pv.PatientUID == patientUID
                                select new ProgressNoteModel
                                {
                                    ProgressNoteUID = pn.UID,
                                    PatientUID = pn.PatientUID,
                                    PatientVisitUID = pn.PatientVisitUID,
                                    Note = pn.Note,
                                    CUser = pn.CUser,
                                    CWhen = pn.CWhen,
                                    MUser = pn.MUser,
                                    MWhen = pn.MWhen,
                                    RecordBy = SqlFunction.fGetCareProviderName(pn.MUser)
                                }).ToList();
            return progressNote;
        }

        [Route("GetProgressNoteByVisit")]
        [HttpGet]
        public List<ProgressNoteModel> GetProgressNoteByVisit(long patientVisitUID)
        {
            var progressNote = db.ProgressNote.Where(p => p.StatusFlag == "A"
                && p.PatientVisitUID == patientVisitUID)
                .Select(p => new ProgressNoteModel
                {
                    ProgressNoteUID = p.UID,
                    PatientUID = p.PatientUID,
                    PatientVisitUID = p.PatientVisitUID,
                    Note = p.Note,
                    CUser = p.CUser,
                    CWhen = p.CWhen,
                    MUser = p.MUser,
                    MWhen = p.MWhen,
                    RecordBy = SqlFunction.fGetCareProviderName(p.MUser)
                }).ToList();

            return progressNote;
        }

        [Route("GetProgressNoteByUID")]
        [HttpGet]
        public ProgressNoteModel GetProgressNoteByUID(long progressNoteUID)
        {
            ProgressNoteModel returnData = null;
            var progressNote = db.ProgressNote.Find(progressNoteUID);
            if (progressNote != null)
            {
                returnData = new ProgressNoteModel();
                returnData.ProgressNoteUID = progressNote.UID;
                returnData.Note = progressNote.Note;
                returnData.PatientUID = progressNote.PatientUID;
                returnData.PatientVisitUID = progressNote.PatientVisitUID;
                returnData.CUser = progressNote.CUser;
                returnData.CWhen = progressNote.CWhen;
                returnData.MUser = progressNote.MUser;
                returnData.MWhen = progressNote.MWhen;
            }
            return returnData;
        }

        [Route("ManageProgressNote")]
        [HttpPost]
        public HttpResponseMessage ManageProgressNote(ProgressNoteModel model, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;


                ProgressNote pe = db.ProgressNote.Find(model.ProgressNoteUID);
                if (pe == null)
                {
                    pe = new ProgressNote();
                    pe.CUser = userID;
                    pe.CWhen = now;
                    pe.StatusFlag = "A";
                }
                pe.MUser = userID;
                pe.MWhen = now;
                pe.PatientUID = model.PatientUID;
                pe.PatientVisitUID = model.PatientVisitUID;
                pe.Note = model.Note;
                db.ProgressNote.AddOrUpdate(pe);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("DeleteProgressNote")]
        [HttpDelete]
        public HttpResponseMessage DeleteProgressNote(int proGressNoteUIUD, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;


                ProgressNote pe = db.ProgressNote.Find(proGressNoteUIUD);
                db.ProgressNote.Attach(pe);
                pe.MUser = userID;
                pe.MWhen = now;
                pe.StatusFlag = "D";
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }


        #endregion

        #region WellnessData

        [Route("GetWellnessDataByPatient")]
        [HttpGet]
        public List<WellnessDataModel> GetWellnessDataByPatient(long patientUID)
        {
            var wellNessData = (from pv in db.PatientVisit
                                join pvp in db.PatientVisitPayor on pv.UID equals pvp.PatientVisitUID
                                join well in db.WellnessData on pv.UID equals well.PatientVisitUID
                                where pv.StatusFlag == "A"
                                && well.StatusFlag == "A"
                                && pvp.StatusFlag == "A"
                                && pv.PatientUID == patientUID
                                select new WellnessDataModel
                                {
                                    WellnessDataUID = well.UID,
                                    PatientUID = well.PatientUID,
                                    PatientVisitUID = well.PatientVisitUID,
                                    PayorDetailUID = pvp.PayorDetailUID,
                                    WellnessResult = well.WellnessResult,
                                    CUser = well.CUser,
                                    CWhen = well.CWhen,
                                    MUser = well.MUser,
                                    MWhen = well.MWhen,
                                    RecordBy = SqlFunction.fGetCareProviderName(well.MUser)
                                }).ToList();

            return wellNessData;
        }

        [Route("GetWellnessDataByVisit")]
        [HttpGet]
        public List<WellnessDataModel> GetWellnessDataByVisit(long patientVisitUID)
        {
            var wellNessData = (from well in db.WellnessData
                                join pvp in db.PatientVisitPayor on well.PatientVisitUID equals pvp.PatientVisitUID
                                where well.StatusFlag == "A"
                                && pvp.StatusFlag == "A"
                                && well.PatientVisitUID == patientVisitUID
                                select new WellnessDataModel
                                {
                                    WellnessDataUID = well.UID,
                                    PatientUID = well.PatientUID,
                                    PatientVisitUID = well.PatientVisitUID,
                                    WellnessResult = well.WellnessResult,
                                    PayorDetailUID = pvp.PayorDetailUID,
                                    CUser = well.CUser,
                                    CWhen = well.CWhen,
                                    MUser = well.MUser,
                                    MWhen = well.MWhen,
                                    RecordBy = SqlFunction.fGetCareProviderName(well.MUser)
                                }).OrderBy(p => p.MWhen).ToList();

            return wellNessData;
        }

        [Route("GetWellnessDataByUID")]
        [HttpGet]
        public WellnessDataModel GetWellnessDataByUID(long wellnessDataUID)
        {
            WellnessDataModel returnData = null;
            var wellNessData = db.WellnessData.Find(wellnessDataUID);
            if (wellNessData != null)
            {
                wellNessData = new WellnessData();
                returnData.WellnessDataUID = wellNessData.UID;
                returnData.WellnessResult = wellNessData.WellnessResult;
                returnData.PatientUID = wellNessData.PatientUID;
                returnData.PatientVisitUID = wellNessData.PatientVisitUID;
                returnData.CUser = wellNessData.CUser;
                returnData.CWhen = wellNessData.CWhen;
                returnData.MUser = wellNessData.MUser;
                returnData.MWhen = wellNessData.MWhen;
            }
            return returnData;
        }

        [Route("ManageWellnessData")]
        [HttpPost]
        public HttpResponseMessage ManageWellnessData(WellnessDataModel model, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;


                WellnessData wellNess = db.WellnessData.FirstOrDefault(p => p.PatientUID == model.PatientUID && p.PatientVisitUID == model.PatientVisitUID);
                if (wellNess == null)
                {
                    wellNess = new WellnessData();
                    wellNess.CUser = userID;
                    wellNess.CWhen = now;
                    wellNess.StatusFlag = "A";
                }
                wellNess.MUser = userID;
                wellNess.MWhen = now;
                wellNess.PatientUID = model.PatientUID;
                wellNess.PatientVisitUID = model.PatientVisitUID;
                wellNess.WellnessResult = model.WellnessResult;
                db.WellnessData.AddOrUpdate(wellNess);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("SendWellnessToBLIFE")]
        [HttpPost]
        public HttpResponseMessage SendWellnessToBLIFE(WellnessDataModel model, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;


                WellnessData wellNess = db.WellnessData.Find(model.WellnessDataUID);
                if (wellNess != null)
                {
                    wellNess.MUser = userID;
                    wellNess.MWhen = now;
                    wellNess.OnBLIFE = "Y";
                    db.WellnessData.AddOrUpdate(wellNess);
                    db.SaveChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                Patient patient = db.Patient.Find(model.PatientUID);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, patient.FirstName + " " + patient.LastName + " ยังไม่ได้ผ่านการยืนยันข้อมูล โปรดตรวจสอบ");

            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("DeleteWellnessData")]
        [HttpDelete]
        public HttpResponseMessage DeleteWellnessData(int wellNessUID, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;


                WellnessData wellNess = db.WellnessData.Find(wellNessUID);
                db.WellnessData.Attach(wellNess);
                wellNess.MUser = userID;
                wellNess.MWhen = now;
                wellNess.StatusFlag = "D";
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        #endregion


        #region RiskBookData

        [Route("GetPatientMedicalHistoryByPatientUID")]
        [HttpGet]
        public PatientMedicalHistoryModel GetPatientMedicalHistoryByPatientUID(long patientUID)
        {
            PatientMedicalHistoryModel data = null;
            PatientMedicalHistory medicalData = db.PatientMedicalHistory.FirstOrDefault(p => p.StatusFlag == "A" && p.PatientUID == patientUID);
            if (medicalData != null)
            {

                data = new PatientMedicalHistoryModel();
                data.PatientMedicalHistoryUID = medicalData.UID;
                data.PatientUID = medicalData.PatientUID;
                data.ChronicDisease = medicalData.ChronicDisease;
                data.SurgicalDetail = medicalData.SurgicalDetail;
                data.ImmunizationDetail = medicalData.ImmunizationDetail;
                data.Familyhistory = medicalData.Familyhistory;
                data.LongTemMedication = medicalData.LongTemMedication;
                data.AllergyDescription = medicalData.AllergyDescription;
                data.Smoke = medicalData.Smoke;
                data.SmokePeriodYear = medicalData.SmokePeriodYear;
                data.SmokePeriodMonth = medicalData.SmokePeriodMonth;
                data.BFQuitSmoke = medicalData.BFQuitSmoke;
                data.Alcohol = medicalData.Alcohol;
                data.AlcohoPeriodYear = medicalData.AlcohoPeriodYear;
                data.AlcohoPeriodMonth = medicalData.AlcohoPeriodMonth;
                data.Narcotic = medicalData.Narcotic;
                data.Comments = medicalData.Comments;
            }

            if (data != null)
            {
                data.PastMedicalHistorys = db.PastMedicalHistory.Where(p => p.StatusFlag == "A" && p.PatientMedicalHistoryUID == data.PatientMedicalHistoryUID)
                    .Select(p => new PastMedicalHistoryModel
                    {
                        PastMedicalHistoryUID = p.UID,
                        PatientMedicalHistoryUID = p.PatientMedicalHistoryUID,
                        MedicalDttm = p.MedicalDttm,
                        MedicalName = p.MedicalName
                    }).ToList();
            }
            return data;
        }

        [Route("GetInjuryByPatientUID")]
        [HttpGet]
        public List<PatientInjuryModel> GetInjuryByPatientUID(long patientUID)
        {
            List<PatientInjuryModel> data = db.PatientInjury.Where(p => p.StatusFlag == "A" && p.PatientUID == patientUID)
                .Select(p => new PatientInjuryModel
                {
                    PatientInjuryUID = p.UID,
                    PatientUID = p.PatientUID,
                    BodyLocation = p.BodyLocation,
                    InjuryDetail = p.InjuryDetail,
                    INRYSEVUID = p.INRYSEVUID,
                    InjuryServerity = SqlFunction.fGetRfValDescription(p.INRYSEVUID ?? 0),
                    OccuredDate = p.OccuredDate
                }).ToList();
            return data;
        }

        [Route("GetPatientWorkHistoryByPatientUID")]
        [HttpGet]
        public List<PatientWorkHistoryModel> GetPatientWorkHistoryByPatientUID(long patientUID)
        {
            List<PatientWorkHistoryModel> data = db.PatientWorkHistory.Where(p => p.StatusFlag == "A" && p.PatientUID == patientUID)
                .Select(p => new PatientWorkHistoryModel
                {
                    PatientWorkHistoryUID = p.UID,
                    PatientUID = p.PatientUID,
                    CompanyName = p.CompanyName,
                    Business = p.Business,
                    Description = p.Description,
                    Riskfactor = p.Riskfactor,
                    Equipment = p.Equipment,
                    Timeperiod = p.Timeperiod

                }).ToList();
            return data;
        }


        #endregion
    }
}
