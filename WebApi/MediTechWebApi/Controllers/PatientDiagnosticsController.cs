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

namespace MediTechWebApi.Controllers
{
    [RoutePrefix("Api/PatientDiagnosis")]
    public class PatientDiagnosticsController : ApiController
    {
        protected MediTechEntities db = new MediTechEntities();

        #region Promblem ICD10

        [Route("GetProblemAll")]
        [HttpGet]
        public List<ProblemModel> GetProblemAll(string problemDesc)
        {
            List<ProblemModel> data = db.Problem
                .Where(p => p.StatusFlag == "A"
                && (p.EndDttm != null && p.EndDttm.Value.Date >= DateTime.Now.Date))
                .Select(p =>
                new ProblemModel
                {
                    ProblemUID = p.UID,
                    Code = p.Code,
                    Name = p.Name,
                    Description = p.Description
                }).ToList();

            return data;
        }

        [Route("GetProblemByUID")]
        [HttpGet]
        public ProblemModel GetProblemByUID(int problemUID)
        {
            ProblemModel data = db.Problem.Where(p => p.UID == problemUID && p.StatusFlag == "A")
                .Select( p => new ProblemModel {
                    ProblemUID = p.UID,
                    Code = p.Code,
                    Name = p.Name,
                    Description = p.Description
                }).FirstOrDefault();

            return data;
        }

        [Route("SearchProblem")]
        [HttpGet]
        public List<ProblemModel> SearchProblem(string problemDesc)
        {
            List<ProblemModel> data = db.Problem
                .Where(p => p.StatusFlag == "A"
                && (p.Name.Contains(problemDesc) || p.Code.StartsWith(problemDesc)))
                .Select(p =>
                new ProblemModel
                {
                    ProblemUID = p.UID,
                    Code = p.Code,
                    Name = p.Name,
                    Description = p.Description
                }).ToList();

            return data;
        }

        [Route("GetFavouriteItemByUser")]
        [HttpGet]
        public List<FavouriteItemModel> GetFavouriteItemByUser(int userUID)
        {
            List<FavouriteItemModel> data = db.FavouriteItem
                .Where(p => p.StatusFlag == "A" && p.CUser == userUID)
                .Select(p =>
                new FavouriteItemModel
                {
                    ProblemUID = p.UID,
                    ProblemCode = p.ProblemCode,
                    ProblemName = p.ProblemName,
                    FavouriteItemUID = p.UID
                }).ToList();

            return data;
        }

        [Route("SearchFavouriteItem")]
        [HttpGet]
        public List<FavouriteItemModel> SearchFavouriteItem(string problemDesc, int userUID)
        {
            List<FavouriteItemModel> data = db.FavouriteItem
                .Where(p => p.StatusFlag == "A"
                && p.CUser == userUID
                && (p.ProblemName.Contains(problemDesc) || p.ProblemCode.StartsWith(problemDesc)))
                .Select(p =>
                new FavouriteItemModel
                {
                    ProblemUID = p.UID,
                    ProblemCode = p.ProblemCode,
                    ProblemName = p.ProblemName,
                    ProblemDescription = p.ProblemDescription,
                    FavouriteItemUID = p.UID
                }).ToList();

            return data;
        }

        [Route("AddFavouriteItem")]
        [HttpPost]
        public HttpResponseMessage AddFavouriteItem(FavouriteItemModel fav, int userUID)
        {
            try
            {
                DateTime now = DateTime.Now;
                FavouriteItem newfav = new FavouriteItem();
                newfav.ProblemUID = fav.ProblemUID;
                newfav.ProblemCode = fav.ProblemCode;
                newfav.ProblemName = fav.ProblemName;
                newfav.ProblemDescription = fav.ProblemDescription;
                newfav.CUser = userUID;
                newfav.CWhen = now;
                newfav.MUser = userUID;
                newfav.MWhen = now;
                newfav.StatusFlag = "A";
                db.FavouriteItem.Add(newfav);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, newfav.UID);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("DeleteFavouriteItem")]
        [HttpDelete]
        public HttpResponseMessage DeleteFavouriteItem(int favouriteItemUID, int userUID)
        {
            try
            {
                FavouriteItem fav = db.FavouriteItem.Find(favouriteItemUID);
                if (fav != null)
                {
                    db.FavouriteItem.Attach(fav);
                    fav.StatusFlag = "D";
                    fav.MWhen = DateTime.Now;
                    fav.MUser = userUID;
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


        [Route("GetPatientProblemByVisitUID")]
        [HttpGet]
        public List<PatientProblemModel> GetPatientProblemByVisitUID(long patientVisitUID)
        {
            List<PatientProblemModel> data = db.PatientProblem.Where(p => p.StatusFlag == "A" && p.PatientVisitUID == patientVisitUID).Select(p =>
                new PatientProblemModel
                {
                    PatientProblemUID = p.UID,
                    PatientUID = p.PatientUID,
                    PatientVisitUID = p.PatientVisitUID,
                    ProblemUID = p.ProblemUID,
                    ProblemCode = p.ProblemCode,
                    ProblemName = p.ProblemName,
                    ProblemDescription = p.ProblemDescription,
                    RecordedBy = p.RecordedBy,
                    RecordedName = SqlFunction.fGetCareProviderName(p.RecordedBy),
                    RecordedDttm = p.RecordedDttm,
                    DIAGTYPUID = p.DIAGTYPUID,
                    DiagnosisType = SqlFunction.fGetRfValDescription(p.DIAGTYPUID ?? 0),
                    IsPrimary = p.IsPrimary,
                    IsUnderline = p.IsUnderline,
                    OnsetDttm = p.OnsetDttm,
                    ClosureDttm = p.ClosureDttm,
                    ClosureComments = p.ClosureComments,
                    SEVRTUID = p.SEVRTUID,
                    Severity = SqlFunction.fGetRfValDescription(p.SEVRTUID ?? 0),
                    PBMTYUID = p.PBMTYUID,
                    ProblemType = SqlFunction.fGetRfValDescription(p.PBMTYUID ?? 0),
                    CERNTUID = p.CERNTUID,
                    Certanity = SqlFunction.fGetRfValDescription(p.CERNTUID ?? 0),
                    BDLOCUID = p.BDLOCUID,
                    BodyLocation = SqlFunction.fGetRfValDescription(p.BDLOCUID ?? 0)
                }).ToList();

            return data;
        }

        [Route("GetPatientProblemByPatientUID")]
        [HttpGet]
        public List<PatientProblemModel> GetPatientProblemByPatientUID(long patientUID)
        {
            List<PatientProblemModel> data = (from pv in db.PatientVisit
                                              join pb in db.PatientProblem on pv.UID equals pb.PatientVisitUID
                                              where pv.StatusFlag == "A"
                                              && pb.StatusFlag == "A"
                                              && pv.PatientUID == patientUID
                                              select new PatientProblemModel
                                              {
                                                  PatientProblemUID = pb.UID,
                                                  PatientUID = pb.PatientUID,
                                                  PatientVisitUID = pb.PatientVisitUID,
                                                  ProblemUID = pb.ProblemUID,
                                                  ProblemCode = pb.ProblemCode,
                                                  ProblemName = pb.ProblemName,
                                                  ProblemDescription = pb.ProblemDescription,
                                                  RecordedBy = pb.RecordedBy,
                                                  RecordedName = SqlFunction.fGetCareProviderName(pb.RecordedBy),
                                                  RecordedDttm = pb.RecordedDttm,
                                                  DIAGTYPUID = pb.DIAGTYPUID,
                                                  DiagnosisType = SqlFunction.fGetRfValDescription(pb.DIAGTYPUID ?? 0),
                                                  IsPrimary = pb.IsPrimary,
                                                  IsUnderline = pb.IsUnderline,
                                                  OnsetDttm = pb.OnsetDttm,
                                                  ClosureDttm = pb.ClosureDttm,
                                                  ClosureComments = pb.ClosureComments,
                                                  SEVRTUID = pb.SEVRTUID,
                                                  Severity = SqlFunction.fGetRfValDescription(pb.SEVRTUID ?? 0),
                                                  PBMTYUID = pb.PBMTYUID,
                                                  ProblemType = SqlFunction.fGetRfValDescription(pb.PBMTYUID ?? 0),
                                                  CERNTUID = pb.CERNTUID,
                                                  Certanity = SqlFunction.fGetRfValDescription(pb.CERNTUID ?? 0),
                                                  BDLOCUID = pb.BDLOCUID,
                                                  BodyLocation = SqlFunction.fGetRfValDescription(pb.BDLOCUID ?? 0)
                                              }).ToList();

            return data;
        }

        [Route("ManagePatientProblem")]
        [HttpPost]
        public HttpResponseMessage ManagePatientProblem(List<PatientProblemModel> model, long patientVisitUID, int userUID)
        {
            try
            {
                DateTime now = DateTime.Now;
                using (var tran = new TransactionScope())
                {
                    List<PatientProblem> oldProblem = db.PatientProblem.Where(p => p.PatientVisitUID == patientVisitUID).ToList();
                    foreach (var item in oldProblem)
                    {
                        if (model.FirstOrDefault(p => p.PatientProblemUID == item.UID) == null)
                        {
                            db.PatientProblem.Attach(item);
                            item.StatusFlag = "D";
                            item.MWhen = now;
                            item.MUser = userUID;

                            db.SaveChanges();
                        }
                    }
                    foreach (var item in model)
                    {
                        PatientProblem patProblem = db.PatientProblem.Find(item.PatientProblemUID);
                        if (patProblem == null)
                        {
                            patProblem = new PatientProblem();
                            patProblem.CUser = userUID;
                            patProblem.CWhen = now;
                        }

                        patProblem.MUser = userUID;
                        patProblem.MWhen = now;
                        patProblem.StatusFlag = "A";
                        patProblem.PatientUID = item.PatientUID;
                        patProblem.PatientVisitUID = item.PatientVisitUID;
                        patProblem.ProblemUID = item.ProblemUID;
                        patProblem.ProblemCode = item.ProblemCode;
                        patProblem.ProblemName = item.ProblemName;
                        patProblem.ProblemDescription = item.ProblemDescription;
                        patProblem.RecordedBy = userUID;
                        patProblem.RecordedDttm = now;
                        patProblem.IsPrimary = item.IsPrimary;
                        patProblem.IsUnderline = item.IsUnderline;
                        patProblem.OnsetDttm = item.OnsetDttm;
                        patProblem.ClosureDttm = item.ClosureDttm;
                        patProblem.ClosureComments = item.ClosureComments;
                        patProblem.SEVRTUID = item.SEVRTUID;
                        patProblem.PBMTYUID = item.PBMTYUID;
                        patProblem.DIAGTYPUID = item.DIAGTYPUID;
                        patProblem.CERNTUID = item.CERNTUID;
                        patProblem.BDLOCUID = item.BDLOCUID;

                        db.PatientProblem.AddOrUpdate(patProblem);
                        db.SaveChanges();
                    }


                    tran.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }

            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("DeletePatientProblem")]
        [HttpDelete]
        public HttpResponseMessage DeletePatientProblem(int patientProblemUID, int userUID)
        {
            try
            {
                PatientProblem prob = db.PatientProblem.Find(patientProblemUID);
                if (prob != null)
                {
                    db.PatientProblem.Attach(prob);
                    prob.MUser = userUID;
                    prob.MWhen = DateTime.Now;
                    prob.StatusFlag = "D";

                    db.SaveChanges();
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }



    }
}
