using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using MediTech.DataBase;
using System.Data;
using System.Data.Entity.Migrations;
using System.Transactions;
using ShareLibrary;
using System.Data.Entity;
using MediTech.Model.Report;

namespace MediTechWebApi.Controllers
{
    [RoutePrefix("Api/Radiology")]
    public class RadiologyController : ApiController
    {
        protected MediTechEntities db = new MediTechEntities();
        /// <summary>
        /// ดึงรายชื่อหมอ รังสี ทั้งหมด
        /// </summary>
        [Route("GetLookUpRadiologist")]
        [HttpGet]
        public List<LookupItemModel> GetLookUpRadiologist()
        {
            var data = db.Careprovider.Where(p => p.StatusFlag == "A" && (p.IsRadiologist.HasValue && p.IsRadiologist.Value)).Select(m =>
    new LookupItemModel
    {
        Key = m.UID,
        Display = m.FirstName + " " + m.LastName
    }).ToList();

            return data;
        }

        [Route("SearchRequestExamList")]
        [HttpGet]
        public List<RequestListModel> SearchRequestExamList(DateTime? requestDateFrom, DateTime? requestDateTo, string statusList, int? RQPRTUID, long? patientUID, string orderName, int? RIMTYPUID, int? radiologistUID, int? rduStaffUID, int? payorDetailUID, int? organisationUID)
        {
            DataTable dataTable = SqlDirectStore.pSearchRequestExamList(requestDateFrom, requestDateTo, statusList, RQPRTUID, patientUID, orderName, RIMTYPUID, radiologistUID, rduStaffUID, payorDetailUID, organisationUID);
            List<RequestListModel> listData = dataTable.ToList<RequestListModel>();

            return listData;
        }

        [Route("GetRequestExecuteRadiologist")]
        [HttpGet]
        public List<RequestListModel> GetRequestExecuteRadiologist(DateTime requestDateFrom, DateTime requestDateTo, int radiologistUID)
        {
            List<RequestListModel> listData = null;
            DataTable dataTable = SqlDirectStore.pGetRequestExecuteRadiologist(requestDateFrom, requestDateTo, radiologistUID);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                listData = dataTable.ToList<RequestListModel>();
            }

            return listData;
        }


        [Route("SearchRequestExamListForAssign")]
        [HttpGet]
        public List<RequestListModel> SearchRequestExamListForAssign(DateTime? dateFrom, DateTime? dateTo, int? organisationUID, long? patientUID, string requestItemName
            , int? RIMTYPUID, int? payorDetailUID, int? ORDSTUID)
        {
            DataTable dataTable = SqlDirectStore.pSearchRequestExamListForAssign(dateFrom, dateTo, organisationUID, patientUID, requestItemName, RIMTYPUID, payorDetailUID, ORDSTUID);
            List<RequestListModel> listData = dataTable.ToList<RequestListModel>();

            return listData;
        }

        //OldMethodForWeb
        [Route("GetPatientRequestReport")]
        [HttpGet]
        public PatientRequestReportModel GetPatientRequestReport(long patientUID, long requestUID, long requestDetailUID)
        {
            PatientRequestReportModel patientRequest = (from pat in db.Patient
                                                        join re in db.Request on pat.UID equals re.PatientUID
                                                        join red in db.RequestDetail on re.UID equals red.RequestUID
                                                        where re.StatusFlag == "A"
                                                        && pat.StatusFlag == "A"
                                                        && red.StatusFlag == "A"
                                                        && pat.UID == patientUID
                                                        && re.UID == requestUID
                                                        && red.UID == requestDetailUID
                                                        select new PatientRequestReportModel
                                                        {
                                                            PatientUID = pat.UID,
                                                            FirstName = pat.FirstName,
                                                            AccessionNumber = red.AccessionNumber,
                                                            LastName = pat.LastName,
                                                            Title = SqlFunction.fGetRfValDescription(pat.TITLEUID ?? 0),
                                                            Gender = SqlFunction.fGetRfValDescription(pat.SEXXXUID ?? 0),
                                                            SEXXXUID = pat.SEXXXUID,
                                                            BirthDttm = pat.DOBDttm,
                                                            AgeString = SqlFunction.fGetAgeString((DateTime)pat.DOBDttm),
                                                            PatientID = pat.PatientID,
                                                            RequestedDttm = re.RequestedDttm,
                                                            RIMTYPUID = red.RIMTYPUID,
                                                            Modality = SqlFunction.fGetRfValCode(red.RIMTYPUID ?? 0),
                                                            RequestItemName = red.RequestItemName,
                                                            RequestUID = re.UID
                                                        }).FirstOrDefault();
            if (patientRequest != null)
            {
                patientRequest.Invenstigation = GetInvenstigation(requestUID);
                patientRequest.PreviousResult = GetPreviousResult(patientUID, requestDetailUID);
                patientRequest.ResultHistory = (from rs in db.Result
                                                join rsh in db.ResultRadiologyHistory on rs.UID equals rsh.ResultUID
                                                join red in db.RequestDetail on rs.RequestDetailUID equals red.UID
                                                where red.StatusFlag == "A"
                                                && rsh.StatusFlag == "A"
                                                && rs.StatusFlag == "A"
                                                && red.UID == requestDetailUID
                                                select new ResultRadiologyHistoryModel
                                                {
                                                    CWhen = rsh.CWhen,
                                                    ResultPlainText = rsh.PlainText,
                                                    ResultRadiologyHistoryUID = rsh.UID,
                                                    ResultUID = rsh.ResultUID,
                                                    ResultValue = rsh.Value,
                                                    ResultVersion = rsh.Version
                                                }).ToList();
            }
            return patientRequest;
        }

        [Route("GetRequestForReview")]
        [HttpGet]
        public PatientRequestReportModel GetRequestForReview(long patientUID, long requestUID, long requestDetailUID)
        {
            PatientRequestReportModel patientRequest = null;
            DataTable dt = SqlDirectStore.pGetRequestByRequestDetailUID(requestDetailUID);

            if (dt != null && dt.Rows.Count > 0)
            {
                patientRequest = new PatientRequestReportModel();
                patientRequest.PatientUID = long.Parse(dt.Rows[0]["PatientUID"].ToString());
                patientRequest.FirstName = dt.Rows[0]["FirstName"].ToString();
                patientRequest.AccessionNumber = dt.Rows[0]["AccessionNumber"].ToString();
                patientRequest.RequestNumber = dt.Rows[0]["RequestNumber"].ToString();
                patientRequest.LastName = dt.Rows[0]["LastName"].ToString();
                patientRequest.Title = dt.Rows[0]["Title"].ToString();
                patientRequest.Gender = dt.Rows[0]["Gender"].ToString();
                patientRequest.SEXXXUID = dt.Rows[0]["SEXXXUID"].ToString() != "" ? int.Parse(dt.Rows[0]["SEXXXUID"].ToString()) : (int?)null;
                patientRequest.BirthDttm = dt.Rows[0]["DOBDttm"].ToString() != "" ? DateTime.Parse(dt.Rows[0]["DOBDttm"].ToString()) : (DateTime?)null;
                patientRequest.AgeString = dt.Rows[0]["AgeString"].ToString();
                patientRequest.PatientID = dt.Rows[0]["PatientID"].ToString();
                patientRequest.RequestedDttm = DateTime.Parse(dt.Rows[0]["RequestedDttm"].ToString());
                patientRequest.PreparedDttm = dt.Rows[0]["PreparedDttm"].ToString() != "" ? DateTime.Parse(dt.Rows[0]["PreparedDttm"].ToString()) : (DateTime?)null;
                patientRequest.PreparedBy = dt.Rows[0]["PreparedBy"].ToString();
                patientRequest.ProcessingNote = dt.Rows[0]["ProcessingNote"].ToString();
                patientRequest.RIMTYPUID = dt.Rows[0]["RIMTYPUID"].ToString() != "" ? int.Parse(dt.Rows[0]["RIMTYPUID"].ToString()) : (int?)null;
                patientRequest.ImageType = dt.Rows[0]["ImageType"].ToString();
                patientRequest.Modality = dt.Rows[0]["Modality"].ToString();
                patientRequest.RequestItemName = dt.Rows[0]["RequestItemName"].ToString();
                patientRequest.RequestUID = long.Parse(dt.Rows[0]["RequestUID"].ToString());
                patientRequest.RequestDetailUID = long.Parse(dt.Rows[0]["RequestDetailUID"].ToString());
                patientRequest.Comments = dt.Rows[0]["Comments"].ToString();
                patientRequest.RDUStaff = dt.Rows[0]["RDUStaff"].ToString();
                patientRequest.RDUNote = dt.Rows[0]["RDUNote"].ToString();
                patientRequest.Invenstigation = GetInvenstigation(patientRequest.RequestUID);
                patientRequest.PreviousResult = GetPreviousResult(patientRequest.PatientUID, patientRequest.RequestDetailUID);
                patientRequest.Result = GetResultRadiologyByRequest(requestDetailUID);
            }
            return patientRequest;
        }

        [Route("GetInvenstigation")]
        [HttpGet]
        public List<Invenstigation> GetInvenstigation(long requestUID)
        {
            List<Invenstigation> invenstigation = (from re in db.Request
                                                   join red in db.RequestDetail on re.UID equals red.RequestUID
                                                   join rs in db.Result on
                                                    new
                                                    {
                                                        key1 = red.UID,
                                                        key2 = "A"
                                                    }
                                                     equals
                                                     new
                                                     {
                                                         key1 = rs.RequestDetailUID,
                                                         key2 = rs.StatusFlag
                                                     } into invenst
                                                   from j in invenst.DefaultIfEmpty()
                                                   where red.StatusFlag == "A"
                                                   && re.UID == requestUID
                                                   select new Invenstigation
                                                   {
                                                       RequestDetailUID = red.UID,
                                                       AccessionNumber = red.AccessionNumber,
                                                       RequestedDttm = red.RequestedDttm,
                                                       TestName = red.RequestItemName,
                                                       Radiologist = j != null ? SqlFunction.fGetCareProviderName(j.RadiologistUID ?? 0) : SqlFunction.fGetCareProviderName(red.RadiologistUID ?? 0)
                                                   }).ToList();

            return invenstigation;

        }

        [Route("GetPreviousResult")]
        public List<PreviousResult> GetPreviousResult(long patientUID, long requestDetailUID)
        {
            List<PreviousResult> previous = (from re in db.Request
                                             join red in db.RequestDetail on re.UID equals red.RequestUID
                                             join rs in db.Result on red.UID equals rs.RequestDetailUID
                                             where red.StatusFlag == "A"
                                             && re.StatusFlag == "A"
                                             && rs.StatusFlag == "A"
                                             && re.PatientUID == patientUID
                                             && re.BSMDDUID == 2841 //Radiology
                                             && red.UID != requestDetailUID
                                             select new PreviousResult
                                             {
                                                 AccessionNumber = red.AccessionNumber,
                                                 TestName = red.RequestItemName,
                                                 ResultUID = rs.UID,
                                                 RequestedDttm = red.RequestedDttm,
                                                 Modality = SqlFunction.fGetRfValCode(red.RIMTYPUID ?? 0),
                                                 ResultEnteredDttm = rs.ResultEnteredDttm,
                                                 ResultStatus = SqlFunction.fGetRfValDescription(rs.RABSTSUID ?? 0)
                                             }).ToList();

            return previous;
        }

        [Route("GetResultHistoryByRequestDetail")]
        [HttpGet]
        public List<ResultRadiologyHistoryModel> GetResultHistoryByRequestDetail(long requestDetailUID)
        {
            List<ResultRadiologyHistoryModel> resultHistory = (from rs in db.Result
                                                               join rsh in db.ResultRadiologyHistory on rs.UID equals rsh.ResultUID
                                                               join red in db.RequestDetail on rs.RequestDetailUID equals red.UID
                                                               where red.StatusFlag == "A"
                                                               && rsh.StatusFlag == "A"
                                                               && rs.StatusFlag == "A"
                                                               && red.UID == requestDetailUID
                                                               select new ResultRadiologyHistoryModel
                                                               {
                                                                   CWhen = rsh.CWhen,
                                                                   ResultPlainText = rsh.PlainText,
                                                                   ResultRadiologyHistoryUID = rsh.UID,
                                                                   ResultUID = rsh.ResultUID,
                                                                   ResultValue = rsh.Value,
                                                                   ResultVersion = rsh.Version
                                                               }).ToList();

            return resultHistory;
        }

        [Route("GetRequestList")]
        [HttpGet]
        public List<RequestListModel> GetRequestList(DateTime? requestDateFrom, DateTime? requestDateTo, DateTime? assignDateFrom, DateTime? assignDateTo, string statusList, int? RQPRTUID, string firstName, string lastName, string patientID, string orderName, int? RIMTYPUID, int? radiologistUID)
        {
            DataTable dataTable = SqlDirectStore.pSearchRequestList(requestDateFrom, requestDateTo, assignDateFrom, assignDateTo, statusList, RQPRTUID, firstName, lastName, patientID, orderName, RIMTYPUID, radiologistUID);
            List<RequestListModel> listData = dataTable.ToList<RequestListModel>();

            return listData;
        }

        [Route("GetRequestListByRequestDetailUID")]
        [HttpGet]
        public RequestListModel GetRequestListByRequestDetailUID(long requestDetailUID)
        {
            DataTable dataTable = SqlDirectStore.pSearchRequestListByRequestDetailUID(requestDetailUID);
            RequestListModel listData = dataTable.ToList<RequestListModel>().FirstOrDefault();

            return listData;
        }

        [Route("GetRequestDetailByUID")]
        [HttpGet]
        public RequestListModel GetRequestDetailByUID(long requestDetailUID)
        {
            RequestListModel data = (from re in db.Request
                                     join red in db.RequestDetail on re.UID equals red.RequestUID
                                     where re.StatusFlag == "A"
                                     && red.StatusFlag == "A"
                                     && red.UID == requestDetailUID
                                     select new RequestListModel
                                     {
                                         PatientUID = re.PatientUID,
                                         PatientVisitUID = re.PatientVisitUID,
                                         AccessionNumber = red.AccessionNumber,
                                         PreparedDttm = red.PreparedDttm,
                                         RequestItemName = red.RequestItemName,
                                         RequestDetailUID = red.UID,
                                         OrderStatus = SqlFunction.fGetRfValDescription(red.ORDSTUID)
                                     }).FirstOrDefault();

            return data;
        }

        [Route("GetRequestDetailDocument")]
        [HttpGet]
        public RequestDetailDocumentModel GetRequestDetailDocument(long requestDetailUID)
        {
            RequestDetailDocumentModel data = db.RequestDetailDocument
                .Where(p => p.RequestDeailUID == requestDetailUID && p.StatusFlag == "A")
                .Select(p => new RequestDetailDocumentModel
                {
                    RequestDetailDocumentUID = p.UID,
                    RequestDeailUID = p.RequestDeailUID,
                    Comments = p.Comments,
                    DocumentContent = p.DocumentContent,
                    DocumentName = p.DocumentName,
                    CUser = p.CUser,
                    MUser = p.MUser,
                    CWhen = p.CWhen,
                    MWhen = p.MWhen,
                    StatusFlag = p.StatusFlag
                }).FirstOrDefault();

            return data;
        }

        [Route("SearchResultRadiologyForTranslate")]
        [HttpGet]
        public List<PatientResultRadiology> SearchResultRadiologyForTranslate(DateTime? dateFrom, DateTime? dateTo,long? patientUID, string itemName, int? RABSTSUID, int? payorDetailUID)
        {
            //List<PatientResultRadiology> data = (from pa in db.Patient
            //                                     join pv in db.PatientVisit on pa.UID equals pv.PatientUID
            //                                     join pvp in db.PatientVisitPayor on pv.UID equals pvp.PatientVisitUID
            //                                     join rs in db.Result on pv.UID equals rs.PatientVisitUID
            //                                     join red in db.RequestDetail on rs.RequestDetailUID equals red.UID
            //                                     join rsr in db.ResultRadiology on rs.UID equals rsr.ResultUID
            //                                     where rs.StatusFlag == "A"
            //                                     && rsr.StatusFlag == "A"
            //                                     && (dateFrom == null || DbFunctions.TruncateTime(rs.ResultEnteredDttm) >= DbFunctions.TruncateTime(dateFrom))
            //                                     && (dateTo == null || DbFunctions.TruncateTime(rs.ResultEnteredDttm) <= DbFunctions.TruncateTime(dateTo))
            //                                     && (payorDetailUID == null || pvp.PayorDetailUID == payorDetailUID)
            //                                     && (string.IsNullOrEmpty(itemName) || rs.RequestItemName.ToLower().Contains(itemName.ToLower()))
            //                                     && (RABSTSUID == null || rs.RABSTSUID == RABSTSUID)
            //                                     && (patientUID == null || pa.UID == patientUID)
            //                                     select new PatientResultRadiology
            //                                     {
            //                                         ResultUID = rs.UID,
            //                                         ResultStatus = SqlFunction.fGetRfValDescription(rs.RABSTSUID ?? 0),
            //                                         ResultEnteredDttm = rs.ResultEnteredDttm,
            //                                         Doctor = SqlFunction.fGetCareProviderName(rs.RadiologistUID ?? 0),
            //                                         RequestItemName = rs.RequestItemName,
            //                                         HN = SqlFunction.fGetPatientID(rs.PatientUID),
            //                                         Title = SqlFunction.fGetRfValDescription(pa.TITLEUID ?? 0),
            //                                         FirstName = pa.FirstName,
            //                                         LastName = pa.LastName,
            //                                         PatientName = SqlFunction.fGetPatientName(rs.PatientUID),
            //                                         EN = SqlFunction.fGetVisitID(rs.PatientVisitUID),
            //                                         OtherID = pa.PatientID2,
            //                                         Gender = SqlFunction.fGetRfValDescription(pa.SEXXXUID ?? 0),
            //                                         Address = SqlFunction.fGetAddressPatient(pa.UID),
            //                                         PayorName = SqlFunction.fGetPayorName(pvp.PayorDetailUID),
            //                                         DOBDttm = pa.DOBDttm,
            //                                         MobilePhone = pa.MobilePhone,
            //                                         Age = pa.DOBDttm != null ? SqlFunction.fGetAge(pa.DOBDttm.Value) : "",
            //                                         ResultValue = rsr.PlainText,
            //                                         ResultHtml = rsr.Value,
            //                                         PatientUID = rs.PatientUID,
            //                                         PatientVisitUID = rs.PatientVisitUID,
            //                                         RequestUID = red.RequestUID,
            //                                         RequestDetailUID = red.UID,
            //                                         RequestedDttm = red.RequestedDttm,
            //                                         Modality = SqlFunction.fGetRfValCode(red.RIMTYPUID ?? 0),
            //                                     }).ToList();
            List<PatientResultRadiology> data = null;
            DataTable dt = SqlDirectStore.pSearchResultRadiologyForTranslate(dateFrom, dateTo, patientUID, itemName, RABSTSUID, payorDetailUID);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = dt.ToList<PatientResultRadiology>();
            }

            return data;
        }

        [Route("SearchPatientResultRadiologyForTranslate")]
        [HttpGet]
        public PatientResultRadiology SearchPatientResultRadiologyForTranslate(DateTime? dateFrom, DateTime? dateTo, string patientID, string itemName, int? RABSTSUID, int? payorDetailUID)
        {
            PatientResultRadiology data = (from pa in db.Patient
                                           join pv in db.PatientVisit on pa.UID equals pv.PatientUID
                                           join pvp in db.PatientVisitPayor on pv.UID equals pvp.PatientVisitUID
                                           join rs in db.Result on pv.UID equals rs.PatientVisitUID
                                           join red in db.RequestDetail on rs.RequestDetailUID equals red.UID
                                           join rsr in db.ResultRadiology on rs.UID equals rsr.ResultUID
                                           where rs.StatusFlag == "A"
                                           && rsr.StatusFlag == "A"
                                           && pa.StatusFlag == "A"
                                           && pa.PatientID == patientID
                                           && (payorDetailUID == null || pvp.PayorDetailUID == payorDetailUID)
                                           && (string.IsNullOrEmpty(itemName) || rs.RequestItemName.ToLower().Contains(itemName.ToLower()))
                                           && (RABSTSUID == null || rs.RABSTSUID == RABSTSUID)
                                           && (dateFrom == null || DbFunctions.TruncateTime(rs.ResultEnteredDttm) >= DbFunctions.TruncateTime(dateFrom))
                                           && (dateTo == null || DbFunctions.TruncateTime(rs.ResultEnteredDttm) <= DbFunctions.TruncateTime(dateTo))
                                           select new PatientResultRadiology
                                           {
                                               ResultUID = rs.UID,
                                               ResultStatus = SqlFunction.fGetRfValDescription(rs.RABSTSUID ?? 0),
                                               ResultEnteredDttm = rs.ResultEnteredDttm,
                                               Doctor = SqlFunction.fGetCareProviderName(rs.RadiologistUID ?? 0),
                                               RequestItemName = rs.RequestItemName,
                                               HN = SqlFunction.fGetPatientID(rs.PatientUID),
                                               PatientName = SqlFunction.fGetPatientName(rs.PatientUID),
                                               FirstName = pa.FirstName,
                                               LastName = pa.LastName,
                                               Title = SqlFunction.fGetRfValDescription(pa.TITLEUID ?? 0),
                                               DOBDttm = pa.DOBDttm,
                                               EN = SqlFunction.fGetVisitID(rs.PatientVisitUID),
                                               Gender = SqlFunction.fGetRfValDescription(pa.SEXXXUID ?? 0),
                                               Address = SqlFunction.fGetAddressPatient(pa.UID),
                                               Age = pa.DOBDttm != null ? SqlFunction.fGetAge(pa.DOBDttm.Value) : "",
                                               ResultValue = rsr.PlainText,
                                               ResultHtml = rsr.Value,
                                               PatientUID = rs.PatientUID,
                                               PatientVisitUID = rs.PatientVisitUID,
                                               MobilePhone = pa.MobilePhone,
                                               RequestUID = red.RequestUID,
                                               RequestDetailUID = red.UID,
                                               RequestedDttm = red.RequestedDttm,
                                               Modality = SqlFunction.fGetRfValCode(red.RIMTYPUID ?? 0),
                                           }).OrderByDescending(p => p.ResultEnteredDttm).FirstOrDefault();

            return data;
        }

        [Route("SaveXrayTranslateMapping")]
        [HttpPost]
        public HttpResponseMessage SaveXrayTranslateMapping(XrayTranslateMappingModel mappingModel)
        {
            try
            {
                DateTime now = DateTime.Now;
                XrayTranslateMapping newData = db.XrayTranslateMapping.Find(mappingModel.XrayTranslateMappingUID);
                if (newData == null)
                {
                    newData = new XrayTranslateMapping();
                    newData.CUser = mappingModel.CUser;
                    newData.CWhen = now;
                }
                newData.IsKeyword = mappingModel.IsKeyword;
                newData.EngResult = mappingModel.EngResult;
                newData.ThaiResult = mappingModel.ThaiResult;
                newData.Type = mappingModel.Type;
                newData.MUser = mappingModel.MUser;
                newData.MWhen = now;
                newData.StatusFlag = "A";
                db.XrayTranslateMapping.AddOrUpdate(newData);



                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("DeleteXrayTranslateMapping")]
        [HttpDelete]
        public HttpResponseMessage DeleteXrayTranslateMapping(int xrayTranslateMappingUID, int userUID)
        {
            try
            {
                DateTime now = DateTime.Now;
                XrayTranslateMapping dataEdit = db.XrayTranslateMapping.Find(xrayTranslateMappingUID);
                if (dataEdit != null)
                {
                    db.XrayTranslateMapping.Attach(dataEdit);
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

        [Route("GetXrayTranslateMapping")]
        [HttpGet]
        public List<XrayTranslateMappingModel> GetXrayTranslateMapping()
        {
            List<XrayTranslateMappingModel> data = db.XrayTranslateMapping.Where(p => p.StatusFlag == "A")
                .Select(p => new XrayTranslateMappingModel
                {
                    XrayTranslateMappingUID = p.UID,
                    Type = p.Type,
                    ThaiResult = p.ThaiResult,
                    IsKeyword = p.IsKeyword,
                    EngResult = p.EngResult,
                }).ToList();

            return data;
        }

        [Route("GetXrayTranslateCondition")]
        [HttpGet]
        public List<XrayTranslateConditionModel> GetXrayTranslateCondition(int typeid)
        {
            List<XrayTranslateConditionModel> data = db.XrayTranslateCondition
                .Where(p => p.Type == typeid && p.StatusFlag == "A")
                .Select(p => new XrayTranslateConditionModel
                {
                    XrayTranslateConditionUID = p.UID,
                    Name = p.Name,
                    Type = p.Type
                }).ToList();

            return data;
        }

        [Route("GetXrayTranslateConditionDetailByConditonUID")]
        [HttpGet]
        public List<XrayTranslateConditionDetailModel> GetXrayTranslateConditionDetailByConditonUID(int xrayTranslateConditionUID)
        {
            List<XrayTranslateConditionDetailModel> data = db.XrayTranslateConditionDetail
                .Where(p => p.XrayTranslateConditionUID == xrayTranslateConditionUID && p.StatusFlag == "A")
                .Select(p => new XrayTranslateConditionDetailModel
                {
                    XrayTranslateConditionDetailUID = p.UID,
                    XrayTranslateConditionUID = p.XrayTranslateConditionUID,
                    Name = p.Name,
                    Description = p.Description
                }).ToList();

            return data;
        }


        [Route("GetXrayTranslateConditionDetailByDetailName")]
        [HttpGet]
        public List<XrayTranslateConditionDetailModel> GetXrayTranslateConditionDetailByDetailName(string xrayTranslateConditionDetailName)
        {
            List<XrayTranslateConditionDetailModel> data = db.XrayTranslateConditionDetail
                .Where(p => p.Name == xrayTranslateConditionDetailName && p.StatusFlag == "A")
                .Select(p => new XrayTranslateConditionDetailModel
                {
                    XrayTranslateConditionDetailUID = p.UID,
                    XrayTranslateConditionUID = p.XrayTranslateConditionUID,
                    Name = p.Name,
                    Description = p.Description
                }).ToList();

            return data;
        }

        [Route("GetXrayTranslateConditionDetailByConditionName")]
        [HttpGet]
        public List<XrayTranslateConditionDetailModel> GetXrayTranslateConditionDetailByConditionName(string xrayTranslateConditionName)
        {
            List<XrayTranslateConditionDetailModel> data = (from j in db.XrayTranslateCondition
                                                            join i in db.XrayTranslateConditionDetail on j.UID equals i.XrayTranslateConditionUID
                                                            where j.StatusFlag == "A"
                                                            && i.StatusFlag == "A"
                                                            && j.Name == xrayTranslateConditionName
                                                            select new XrayTranslateConditionDetailModel
                                                            {
                                                                XrayTranslateConditionDetailUID = i.UID,
                                                                XrayTranslateConditionUID = i.XrayTranslateConditionUID,
                                                                Name = i.Name,
                                                                Description = i.Description
                                                            }).ToList();
            return data;
        }


        [Route("SaveTemplateTransalteMaster")]
        [HttpPut]
        public HttpResponseMessage SaveTemplateTransalteMaster(string translateMastername
            , string translateDetailname
            , string thaiValue
            , int type
            , int userID)
        {
            try
            {
                DateTime now = DateTime.Now;

                XrayTranslateCondition xraycon = db.XrayTranslateCondition.FirstOrDefault(p => p.Name == translateMastername
                && p.Type == type
                && p.StatusFlag == "A");
                if (xraycon == null)
                {
                    xraycon = new XrayTranslateCondition();
                    xraycon.StatusFlag = "A";
                    xraycon.CWhen = now;
                    xraycon.CUser = userID;
                }
                xraycon.MWhen = now;
                xraycon.CUser = userID;
                xraycon.Type = type;
                xraycon.Name = translateMastername;

                db.XrayTranslateCondition.AddOrUpdate(xraycon);
                db.SaveChanges();

                XrayTranslateConditionDetail xrayDetail = db.XrayTranslateConditionDetail.FirstOrDefault(p => p.Name == translateDetailname
                && p.XrayTranslateConditionUID == xraycon.UID
                && p.StatusFlag == "A");


                if (xrayDetail == null)
                {
                    xrayDetail = new XrayTranslateConditionDetail();
                    xrayDetail.StatusFlag = "A";
                    xrayDetail.CWhen = now;
                    xrayDetail.CUser = userID;
                }
                xrayDetail.MWhen = now;
                xrayDetail.CUser = userID;
                xrayDetail.XrayTranslateConditionUID = xraycon.UID;
                xrayDetail.Name = translateDetailname;
                xrayDetail.Description = thaiValue;
                db.XrayTranslateConditionDetail.AddOrUpdate(xrayDetail);
                db.SaveChanges();



                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("GetResultRadiologyByRequest")]
        [HttpGet]
        public ResultRadiologyModel GetResultRadiologyByRequest(long requestdetailUID)
        {
            ResultRadiologyModel data = (from rs in db.Result
                                         join rsr in db.ResultRadiology on rs.UID equals rsr.ResultUID
                                         where rs.StatusFlag == "A"
                                         && rsr.StatusFlag == "A"
                                         && rs.RequestDetailUID == requestdetailUID
                                         select new ResultRadiologyModel
                                         {
                                             ResultUID = rs.UID,
                                             ResultRadiologyUID = rsr.UID,
                                             Comments = rs.Comments,
                                             HasHistory = rsr.HasHistory,
                                             ORDSTUID = rs.ORDSTUID,
                                             PatientUID = rs.PatientUID,
                                             RABSTSUID = rs.RABSTSUID,
                                             ResultStatus = SqlFunction.fGetRfValDescription(rs.RABSTSUID ?? 0),
                                             RadiologistUID = rs.RadiologistUID,
                                             Radiologist = SqlFunction.fGetCareProviderName(rs.RadiologistUID ?? 0),
                                             RequestDetailUID = rs.RequestDetailUID,
                                             ResultEnteredDttm = rs.ResultEnteredDttm,
                                             PlainText = rsr.PlainText,
                                             Value = rsr.Value,
                                             Version = rsr.Version,
                                             IsCaseStudy = rs.IsCaseStudy
                                         }).FirstOrDefault();

            return data;
        }

        [Route("GetResultRadiologyByResultUID")]
        [HttpGet]
        public ResultRadiologyModel GetResultRadiologyByResultUID(long resultUID)
        {
            ResultRadiologyModel data = (from rs in db.Result
                                         join rsr in db.ResultRadiology on rs.UID equals rsr.ResultUID
                                         where rs.StatusFlag == "A"
                                         && rsr.StatusFlag == "A"
                                         && rs.UID == resultUID
                                         select new ResultRadiologyModel
                                         {
                                             ResultUID = rs.UID,
                                             ResultRadiologyUID = rsr.UID,
                                             Comments = rs.Comments,
                                             HasHistory = rsr.HasHistory,
                                             ORDSTUID = rs.ORDSTUID,
                                             PatientUID = rs.PatientUID,
                                             RABSTSUID = rs.RABSTSUID,
                                             ResultStatus = SqlFunction.fGetRfValDescription(rs.RABSTSUID ?? 0),
                                             RadiologistUID = rs.RadiologistUID,
                                             Radiologist = SqlFunction.fGetCareProviderName(rs.RadiologistUID ?? 0),
                                             RequestDetailUID = rs.RequestDetailUID,
                                             ResultEnteredDttm = rs.ResultEnteredDttm,
                                             PlainText = rsr.PlainText,
                                             Value = rsr.Value,
                                             Version = rsr.Version
                                         }).FirstOrDefault();
            return data;
        }

        [Route("GetResultRadiologyByPatientUID")]
        [HttpGet]
        public List<ResultRadiologyModel> GetResultRadiologyByPatientUID(long patientUID, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            List<ResultRadiologyModel> data = (from pv in db.PatientVisit 
                                               join rs in db.Result on pv.UID equals rs.PatientVisitUID
                                               join rsr in db.ResultRadiology on rs.UID equals rsr.ResultUID
                                               join rd in db.RequestDetail on rs.RequestDetailUID equals rd.UID
                                               where pv.StatusFlag == "A"
                                               && rs.StatusFlag == "A"
                                               && rsr.StatusFlag == "A"
                                               && rd.StatusFlag == "A"
                                               && rs.ORDSTUID != 2848
                                               && rs.PatientUID == patientUID
                                               && (dateFrom == null || DbFunctions.TruncateTime(rs.ResultEnteredDttm) >= DbFunctions.TruncateTime(dateFrom))
                                               && (dateTo == null || DbFunctions.TruncateTime(rs.ResultEnteredDttm) <= DbFunctions.TruncateTime(dateTo))
                                               select new ResultRadiologyModel
                                               {
                                                   ResultUID = rs.UID,
                                                   ResultRadiologyUID = rsr.UID,
                                                   Comments = rs.Comments,
                                                   HasHistory = rsr.HasHistory,
                                                   ORDSTUID = rs.ORDSTUID,
                                                   RequestItemCode = rs.RequestItemCode,
                                                   RequestItemName = rs.RequestItemName,
                                                   PatientUID = rs.PatientUID,
                                                   PatientID = SqlFunction.fGetPatientID(rs.PatientUID),
                                                   AccessionNumber = rd.AccessionNumber,
                                                   Modality = SqlFunction.fGetRfValCode(rd.RIMTYPUID ?? 0),
                                                   RABSTSUID = rs.RABSTSUID,
                                                   ResultStatus = SqlFunction.fGetRfValDescription(rs.RABSTSUID ?? 0),
                                                   RadiologistUID = rs.RadiologistUID,
                                                   Radiologist = SqlFunction.fGetCareProviderName(rs.RadiologistUID ?? 0),
                                                   RequestDetailUID = rs.RequestDetailUID,
                                                   ResultEnteredDttm = rs.ResultEnteredDttm,
                                                   PlainText = rsr.PlainText,
                                                   Value = rsr.Value,
                                                   Version = rsr.Version
                                               }).ToList();

            //if (data != null)
            //{
            //    foreach (var item in data)
            //    {
            //        item.NavigationImage = GetNavigationImage(item.AccessionNumber, item.PatientID, item.Modality);
            //    }
            //}
            return data;
        }

        [Route("GetResultRadiologyByVisitUID")]
        [HttpGet]
        public List<ResultRadiologyModel> GetResultRadiologyByVisitUID(long patientVisitUID, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            List<ResultRadiologyModel> data = (from rs in db.Result
                                               join rsr in db.ResultRadiology on rs.UID equals rsr.ResultUID
                                               join rd in db.RequestDetail on rs.RequestDetailUID equals rd.UID
                                               where rs.StatusFlag == "A"
                                               && rsr.StatusFlag == "A"
                                               && rd.StatusFlag == "A"
                                               && rs.ORDSTUID != 2848
                                               && rs.PatientVisitUID == patientVisitUID
                                               && (dateFrom == null || DbFunctions.TruncateTime(rs.ResultEnteredDttm) >= DbFunctions.TruncateTime(dateFrom))
                                               && (dateTo == null || DbFunctions.TruncateTime(rs.ResultEnteredDttm) <= DbFunctions.TruncateTime(dateTo))
                                               select new ResultRadiologyModel
                                               {
                                                   ResultUID = rs.UID,
                                                   ResultRadiologyUID = rsr.UID,
                                                   Comments = rs.Comments,
                                                   HasHistory = rsr.HasHistory,
                                                   ORDSTUID = rs.ORDSTUID,
                                                   RequestItemCode = rs.RequestItemCode,
                                                   RequestItemName = rs.RequestItemName,
                                                   PatientUID = rs.PatientUID,
                                                   PatientID = SqlFunction.fGetPatientID(rs.PatientUID),
                                                   AccessionNumber = rd.AccessionNumber,
                                                   Modality = SqlFunction.fGetRfValCode(rd.RIMTYPUID ?? 0),
                                                   RABSTSUID = rs.RABSTSUID,
                                                   ResultStatus = SqlFunction.fGetRfValDescription(rs.RABSTSUID ?? 0),
                                                   RadiologistUID = rs.RadiologistUID,
                                                   Radiologist = SqlFunction.fGetCareProviderName(rs.RadiologistUID ?? 0),
                                                   RequestDetailUID = rs.RequestDetailUID,
                                                   ResultEnteredDttm = rs.ResultEnteredDttm,
                                                   PlainText = rsr.PlainText,
                                                   Value = rsr.Value,
                                                   Version = rsr.Version
                                               }).ToList();

            //if (data != null)
            //{
            //    foreach (var item in data)
            //    {
            //        item.NavigationImage = GetNavigationImage(item.AccessionNumber, item.PatientID, item.Modality);
            //    }
            //}
            return data;
        }

        [Route("SaveReviewResult")]
        [HttpPost]
        public HttpResponseMessage SaveReviewResult(ResultRadiologyModel resultData, int userID)
        {
            try
            {
                long resultUID = 0;
                int reviewStatus = 2863;
                RequestDetail requestDetail = db.RequestDetail.Find(resultData.RequestDetailUID);
                PatientOrderDetail patientOrderDetail = db.PatientOrderDetail.Find(requestDetail.PatientOrderDetailUID);
                Request request = db.Request.Find(requestDetail.RequestUID);
                PatientOrder patientOrder = db.PatientOrder.Find(request.PatientOrderUID);
                Result result = db.Result.FirstOrDefault(p => p.RequestDetailUID == requestDetail.UID);
                DateTime now = DateTime.Now;
                using (var tran = new TransactionScope())
                {
                    if (result == null)
                    {

                        SEQResult seqResultID = new SEQResult();
                        seqResultID.StatusFlag = "A";

                        db.SEQResult.Add(seqResultID);
                        db.SaveChanges();

                        result = new Result();
                        result.CUser = userID;
                        result.CWhen = now;
                        result.StatusFlag = "A";
                        result.ResultNumber = seqResultID.UID.ToString();
                    }

                    result.RequestDetailUID = resultData.RequestDetailUID;
                    result.MUser = userID;
                    result.MWhen = now;
                    result.ResultEnteredDttm = resultData.ResultEnteredDttm != null ? resultData.ResultEnteredDttm : now;
                    result.ResultEnteredUserUID = userID;
                    result.ORDSTUID = resultData.ORDSTUID ?? 0;
                    result.RABSTSUID = resultData.RABSTSUID;
                    result.PatientUID = resultData.PatientUID;
                    result.Comments = resultData.Comments;
                    result.PatientVisitUID = resultData.PatientVisitUID;
                    result.ResultedByUID = resultData.ResultedByUID;
                    result.RadiologistUID = resultData.RadiologistUID;
                    result.RequestItemCode = requestDetail.RequestItemCode;
                    result.RequestItemName = requestDetail.RequestItemName;
                    result.IsCaseStudy = resultData.IsCaseStudy;
                    if (result.RadiologistUID == null || result.RadiologistUID == 0)
                    {
                        DataTable dtSession = SqlStatement.GetRadiologistSession(requestDetail.UID);
                        if (dtSession != null && dtSession.Rows.Count > 0)
                        {
                            if (result.Comments == "RDU Review")
                            {
                                List<SessionDefinitionModel> sessionRadio = dtSession.ToList<SessionDefinitionModel>();
                                int radiologistUID = sessionRadio.Where(p => p.CareproviderUID == 2
                                    || p.CareproviderUID == 3
                                    || p.CareproviderUID == 4
                                    || p.CareproviderUID == 5).OrderBy(p => p.RequestBulkNumber).FirstOrDefault().CareproviderUID;
                                result.RadiologistUID = radiologistUID;
                            }
                            else
                            {
                                List<SessionDefinitionModel> sessionRadio = dtSession.ToList<SessionDefinitionModel>();
                                int radiologistUID = sessionRadio.Where(p => p.CareproviderUID == 2
                                    || p.CareproviderUID == 3
                                    || p.CareproviderUID == 4
                                    || p.CareproviderUID == 5).OrderBy(p => p.RequestNumber).FirstOrDefault().CareproviderUID;
                                result.RadiologistUID = radiologistUID;
                            }
                        }
                    }

                    db.Result.AddOrUpdate(result);
                    db.SaveChanges();

                    ResultRadiology resultRadiology = db.ResultRadiology.FirstOrDefault(p => p.ResultUID == result.UID);

                    if (resultRadiology == null)
                    {
                        resultRadiology = new ResultRadiology();
                        resultRadiology.CUser = userID;
                        resultRadiology.CWhen = now;
                        resultRadiology.StatusFlag = "A";
                        resultRadiology.HasHistory = "N";
                        resultRadiology.Version = 1;
                    }
                    else
                    {
                        ResultRadiologyHistory resultRadiologyHistory = new ResultRadiologyHistory();
                        resultRadiologyHistory.CUser = userID;
                        resultRadiologyHistory.MUser = userID;
                        resultRadiologyHistory.CWhen = now;
                        resultRadiologyHistory.MWhen = now;
                        resultRadiologyHistory.StatusFlag = "A";
                        resultRadiologyHistory.ResultUID = result.UID;
                        resultRadiologyHistory.Value = resultData.Value;
                        resultRadiologyHistory.PlainText = resultData.PlainText;
                        resultRadiologyHistory.Version = resultData.Version ?? resultRadiology.Version ?? 0;

                        db.ResultRadiologyHistory.Add(resultRadiologyHistory);

                        resultRadiology.HasHistory = "Y";
                        resultRadiology.Version += 1;
                    }

                    resultRadiology.ResultUID = result.UID;
                    resultRadiology.Value = resultData.Value;
                    resultRadiology.PlainText = resultData.PlainText;
                    resultRadiology.MUser = userID;
                    resultRadiology.MWhen = now;

                    db.ResultRadiology.AddOrUpdate(resultRadiology);

                    //RequestDetail
                    db.RequestDetail.Attach(requestDetail);
                    requestDetail.ORDSTUID = (int)result.ORDSTUID;
                    requestDetail.MUser = userID;
                    requestDetail.MWhen = now;

                    if (result.Comments == "RDU Review")
                    {
                        requestDetail.Comments = result.Comments;
                        requestDetail.RDUResultedDttm = now;
                        requestDetail.RDUStaffUID = result.ResultedByUID;
                        requestDetail.RABSTSUID = result.RABSTSUID;
                    }

                    //PatientOrderDetail
                    db.PatientOrderDetail.Attach(patientOrderDetail);
                    patientOrderDetail.ORDSTUID = (int)result.ORDSTUID;
                    patientOrderDetail.MUser = userID;
                    patientOrderDetail.MWhen = DateTime.Now;

                    db.SaveChanges();


                    #region SavePatinetOrderDetailHistory

                    PatientOrderDetailHistory patientOrderDetailHistory = new PatientOrderDetailHistory();
                    patientOrderDetailHistory.PatientOrderDetailUID = patientOrderDetail.UID;
                    patientOrderDetailHistory.ORDSTUID = patientOrderDetail.ORDSTUID;
                    //patientOrderDetailHistory.Comments = "From Web RIS";
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


                    db.Request.Attach(request);
                    request.MUser = userID;
                    request.MWhen = now;
                    request.ORDSTUID = CheckRequestStatus(request.UID);

                    db.SaveChanges();

                    if (!string.IsNullOrEmpty(requestDetail.NetworkPartnerID))
                    {
                        if (result.ORDSTUID == reviewStatus)
                        {
                            NotificationTaskResult notification = new NotificationTaskResult();

                            notification.ResultUID = result.UID;
                            notification.NetworkPartnerID = requestDetail.NetworkPartnerID;
                            notification.OwnerOrganisationUID = requestDetail.OwnerOrganisationUID.Value;
                            notification.OwnerOrganisationCode = db.HealthOrganisation.Find(requestDetail.OwnerOrganisationUID.Value).Code;
                            notification.ResultEnteredDttm = result.ResultEnteredDttm.Value;
                            notification.CWhen = now;
                            notification.Status = -1;

                            db.NotificationTaskResult.Add(notification);

                            db.SaveChanges();
                        }

                    }

                    tran.Complete();
                    resultUID = result.UID;
                }
                return Request.CreateResponse(HttpStatusCode.OK, resultUID);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("UpdateRequestDetailToReviewing")]
        [HttpPut]
        public HttpResponseMessage UpdateRequestDetailToReviewing(int requestDetailUID, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                RequestDetail requestDetail = db.RequestDetail.Find(requestDetailUID);
                PatientOrderDetail patientOrderDetail = db.PatientOrderDetail.Find(requestDetail.PatientOrderDetailUID);
                int reviewingStatus = db.ReferenceValue.FirstOrDefault(p => p.DomainCode == "ORDST" && p.ValueCode == "REVIN" && p.StatusFlag == "A").UID;
                if (requestDetail != null)
                {
                    db.RequestDetail.Attach(requestDetail);
                    requestDetail.ORDSTUID = reviewingStatus;
                    requestDetail.MUser = userID;
                    requestDetail.MWhen = now;

                    db.PatientOrderDetail.Attach(patientOrderDetail);
                    patientOrderDetail.ORDSTUID = reviewingStatus;
                    patientOrderDetail.MUser = userID;
                    patientOrderDetail.MWhen = now;


                    db.SaveChanges();
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }

        }

        [Route("UnReviewingReqeustDetail")]
        [HttpPut]
        public HttpResponseMessage UnReviewingReqeustDetail(long requestDetailUID, int userID)
        {
            try
            {
                RequestDetail requestDetail = db.RequestDetail.Find(requestDetailUID);
                PatientOrderDetail patientOrderDetail = db.PatientOrderDetail.Find(requestDetail.PatientOrderDetailUID);

                List<PatientOrderDetailHistory> orderDetailHistory = db.PatientOrderDetailHistory.Where(p => p.PatientOrderDetailUID == patientOrderDetail.UID && p.StatusFlag == "A").OrderByDescending(p => p.EditedDttm).ToList();
                int reviewing = db.ReferenceValue.FirstOrDefault(p => p.DomainCode == "ORDST" && p.ValueCode == "REVIN").UID;
                if (patientOrderDetail.ORDSTUID == reviewing)
                {
                    if (orderDetailHistory != null && orderDetailHistory.Count > 0)
                    {

                        DateTime now = DateTime.Now;
                        db.RequestDetail.Attach(requestDetail);
                        requestDetail.ORDSTUID = orderDetailHistory.FirstOrDefault().ORDSTUID;
                        requestDetail.MUser = userID;
                        requestDetail.MWhen = now;

                        db.PatientOrderDetail.Attach(patientOrderDetail);
                        patientOrderDetail.ORDSTUID = requestDetail.ORDSTUID;
                        patientOrderDetail.MUser = userID;
                        patientOrderDetail.MWhen = now;


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

        [Route("UnReviewingAndGetRequestListByRequestDetailUID")]
        [HttpGet]
        public RequestListModel UnReviewingAndGetRequestListByRequestDetailUID(long requestDetailUID, int userID)
        {

            UnReviewingReqeustDetail(requestDetailUID, userID);
            RequestListModel listData = GetRequestListByRequestDetailUID(requestDetailUID);

            return listData;
        }

        [Route("GetResultRaiologyTemplateByUID")]
        [HttpGet]
        public ResultRadiologyTemplateModel GetResultRaiologyTemplateByUID(int resultRadiologyTemplateUID)
        {
            ResultRadiologyTemplate resultRadiologyTemplate = db.ResultRadiologyTemplate.Find(resultRadiologyTemplateUID);

            ResultRadiologyTemplateModel data = new ResultRadiologyTemplateModel();
            if (resultRadiologyTemplate != null)
            {
                data.ResultRadiologyTemplateUID = resultRadiologyTemplate.UID;
                data.Value = resultRadiologyTemplate.Value;
                data.PlainText = resultRadiologyTemplate.PlainText;
                data.IsPublic = resultRadiologyTemplate.IsPublic;
                data.SEXXXUID = resultRadiologyTemplate.SEXXXUID;
                data.RIMTYPUID = resultRadiologyTemplate.RIMTYPUID;
                data.RABSTSUID = resultRadiologyTemplate.RABSTSUID;
                data.Description = resultRadiologyTemplate.Description;
                data.DisplayOrder = resultRadiologyTemplate.DisplayOrder;
                data.IsPrimary = resultRadiologyTemplate.IsPrimary ?? false;
                data.Name = resultRadiologyTemplate.Name;
            }

            return data;
        }

        [Route("GetResultRadiologyTemplateByDoctor")]
        [HttpGet]
        public List<ResultRadiologyTemplateModel> GetResultRadiologyTemplateByDoctor(int userID)
        {
            List<ResultRadiologyTemplateModel> data = (from rtp in db.ResultRadiologyTemplate
                                                       where rtp.StatusFlag == "A" && rtp.CUser == userID
                                                       select new ResultRadiologyTemplateModel
                                                       {
                                                           ResultRadiologyTemplateUID = rtp.UID,
                                                           //Value = rtp.Value,
                                                           Description = rtp.Description,
                                                           DisplayOrder = rtp.DisplayOrder,
                                                           SEXXXUID = rtp.SEXXXUID,
                                                           Gender = SqlFunction.fGetRfValDescription(rtp.SEXXXUID ?? 0),
                                                           RIMTYPUID = rtp.RIMTYPUID,
                                                           ImageType = SqlFunction.fGetRfValDescription(rtp.RIMTYPUID ?? 0),
                                                           RABSTSUID = rtp.RABSTSUID,
                                                           ResultStatus = SqlFunction.fGetRfValDescription(rtp.RABSTSUID ?? 0),
                                                           IsPrimary = rtp.IsPrimary ?? false,
                                                           IsPublic = rtp.IsPublic,
                                                           Name = rtp.Name
                                                       }).ToList();
            return data;
        }

        [Route("GetResultRadiologyTemplateByReportEditor")]
        [HttpGet]
        public List<ResultRadiologyTemplateModel> GetResultRadiologyTemplateByReportEditor(int? RIMTYPUID, int? SEXXXUID, int userID)
        {
            List<ResultRadiologyTemplateModel> data = (from rtp in db.ResultRadiologyTemplate
                                                       where rtp.StatusFlag == "A"
                                                       && (rtp.CUser == userID || rtp.IsPublic)
                                                       && (rtp.RIMTYPUID == RIMTYPUID || (rtp.RIMTYPUID == 0 || rtp.RIMTYPUID == null))
                                                       && (rtp.SEXXXUID == SEXXXUID || (rtp.SEXXXUID == 0 || rtp.SEXXXUID == null))
                                                       select new ResultRadiologyTemplateModel
                                                       {
                                                           ResultRadiologyTemplateUID = rtp.UID,
                                                           // Value = rtp.Value,
                                                           Description = rtp.Description,
                                                           DisplayOrder = rtp.DisplayOrder,
                                                           Gender = SqlFunction.fGetRfValDescription(rtp.SEXXXUID ?? 0),
                                                           ImageType = SqlFunction.fGetRfValDescription(rtp.RIMTYPUID ?? 0),
                                                           ResultStatus = SqlFunction.fGetRfValDescription(rtp.RABSTSUID ?? 0),
                                                           SEXXXUID = rtp.SEXXXUID,
                                                           RIMTYPUID = rtp.RIMTYPUID,
                                                           RABSTSUID = rtp.RABSTSUID,
                                                           IsPublic = rtp.IsPublic,
                                                           IsPrimary = rtp.IsPrimary ?? false,
                                                           Name = rtp.Name
                                                       }).ToList();
            return data;
        }


        [Route("SaveResultRadiologyTemplate")]
        [HttpPost]
        public HttpResponseMessage SaveResultRadiologyTemplate(ResultRadiologyTemplateModel resultRadTemp, int userID)
        {
            try
            {
                int resultRadiologytemplateUID = 0;
                DateTime now = DateTime.Now;

                ResultRadiologyTemplate data = db.ResultRadiologyTemplate.Find(resultRadTemp.ResultRadiologyTemplateUID);
                if (data == null)
                {
                    data = new ResultRadiologyTemplate();
                    data.CUser = userID;
                    data.CWhen = now;
                }
                data.Name = resultRadTemp.Name;
                data.Description = resultRadTemp.Description;
                data.DisplayOrder = resultRadTemp.DisplayOrder;
                data.IsPrimary = resultRadTemp.IsPrimary;
                data.IsPublic = resultRadTemp.IsPublic;
                data.SEXXXUID = resultRadTemp.SEXXXUID;
                data.RIMTYPUID = resultRadTemp.RIMTYPUID;
                data.Value = resultRadTemp.Value;
                data.PlainText = resultRadTemp.PlainText;
                data.RABSTSUID = resultRadTemp.RABSTSUID;
                data.MUser = userID;
                data.MWhen = now;
                data.StatusFlag = "A";
                db.ResultRadiologyTemplate.AddOrUpdate(data);
                db.SaveChanges();
                resultRadiologytemplateUID = data.UID;

                return Request.CreateResponse(HttpStatusCode.OK, resultRadiologytemplateUID);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("SaveDisplayOrderResultRadiologyTempalte")]
        [HttpPut]
        public HttpResponseMessage SaveDisplayOrderResultRadiologyTempalte(int resultTemplateUID, int displayOrder, int userID)
        {
            try
            {
                List<ResultRadiologyTemplate> resultTemplateAfter = db.ResultRadiologyTemplate.Where(p => p.CUser == userID && p.UID != resultTemplateUID)
    .OrderBy(p => p.DisplayOrder).ToList();
                int i = displayOrder;
                foreach (var item in resultTemplateAfter)
                {
                    if (item.DisplayOrder - i == 0)
                    {
                        db.ResultRadiologyTemplate.Attach(item);
                        item.DisplayOrder = item.DisplayOrder + 1;
                        i++;
                    }
                }

                ResultRadiologyTemplate resultTemplate = db.ResultRadiologyTemplate.Find(resultTemplateUID);
                db.ResultRadiologyTemplate.Attach(resultTemplate);
                resultTemplate.DisplayOrder = displayOrder;
                resultTemplate.MUser = userID;
                resultTemplate.MWhen = DateTime.Now;
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }

        }

        [Route("UpdateResultRadiologyTemplate")]
        [HttpPut]
        public HttpResponseMessage UpdateResultRadiologyTemplate(ResultRadiologyTemplateModel resultRadTemp, int userID)
        {
            try
            {

                ResultRadiologyTemplate data = db.ResultRadiologyTemplate.Find(resultRadTemp.ResultRadiologyTemplateUID);
                db.ResultRadiologyTemplate.Attach(data);
                data.MWhen = DateTime.Now;
                data.MUser = userID;
                data.Value = resultRadTemp.Value;
                data.PlainText = resultRadTemp.PlainText;
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }

        }


        [Route("DeleteResultRadiologyTemplate")]
        [HttpDelete]
        public HttpResponseMessage DeleteResultRadiologyTemplate(int resultRadiologyTemplateUID, int userID)
        {
            try
            {
                ResultRadiologyTemplate data = db.ResultRadiologyTemplate.Find(resultRadiologyTemplateUID);
                db.ResultRadiologyTemplate.Attach(data);
                data.StatusFlag = "D";
                data.MWhen = DateTime.Now;
                data.MUser = userID;
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);

            }

        }

        //OLDMethodFromWeb
        [Route("AssignRadiologist")]
        [HttpPut]
        public HttpResponseMessage AssignRadiologist(int radiologistUID, long requestDetailUID, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                RequestDetail data = db.RequestDetail.Find(requestDetailUID);

                int executedUID = db.ReferenceValue.FirstOrDefault(p => p.StatusFlag == "A" && p.DomainCode == "ORDST" && p.ValueCode == "EXCUT").UID;
                if (data != null)
                {
                    db.RequestDetail.Attach(data);
                    data.PreparedByUID = userID;
                    data.PreparedDttm = now;
                    data.RadiologistUID = radiologistUID;
                    data.ORDSTUID = executedUID;
                    data.MWhen = DateTime.Now;
                    data.MUser = userID;

                }

                PatientOrderDetail patientOrderDetail = db.PatientOrderDetail.Find(data.PatientOrderDetailUID);

                if (patientOrderDetail != null)
                {
                    db.PatientOrderDetail.Attach(patientOrderDetail);
                    patientOrderDetail.ORDSTUID = executedUID;
                    patientOrderDetail.MWhen = DateTime.Now;
                    patientOrderDetail.MUser = userID;
                }

                Request request = db.Request.Find(data.RequestUID);
                PatientOrder patientOrder = db.PatientOrder.Find(request.PatientOrderUID);


                #region SavePatinetOrderDetailHistory

                PatientOrderDetailHistory patientOrderDetailHistory = new PatientOrderDetailHistory();
                patientOrderDetailHistory.PatientOrderDetailUID = patientOrderDetail.UID;
                patientOrderDetailHistory.ORDSTUID = patientOrderDetail.ORDSTUID;
                patientOrderDetailHistory.Comments = "From Web RIS";
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

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        //OLDMethodFromWeb
        [Route("AssignRadiologistList")]
        [HttpPut]
        public HttpResponseMessage AssignRadiologistList(List<RequestListModel> requestlist, int radiologistUID, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;

                foreach (var item in requestlist)
                {
                    RequestDetail requestDetail = db.RequestDetail.Find(item.RequestDetailUID);

                    PatientOrderDetail patientOrderDetail = db.PatientOrderDetail.Find(requestDetail.PatientOrderDetailUID);

                    if (requestDetail != null)
                    {

                        int executedUID = db.ReferenceValue.FirstOrDefault(p => p.StatusFlag == "A" && p.DomainCode == "ORDST" && p.ValueCode == "EXCUT").UID;


                        db.RequestDetail.Attach(requestDetail);
                        requestDetail.PreparedByUID = userID;
                        requestDetail.PreparedDttm = item.PreparedDttm;
                        requestDetail.ProcessingNote = item.ProcessingNote;
                        requestDetail.RadiologistUID = radiologistUID;
                        requestDetail.ORDSTUID = executedUID;
                        requestDetail.MWhen = now;
                        requestDetail.MUser = userID;

                        db.PatientOrderDetail.Attach(patientOrderDetail);
                        patientOrderDetail.ORDSTUID = executedUID;
                        patientOrderDetail.MWhen = DateTime.Now;
                        patientOrderDetail.MUser = userID;

                        #region SavePatinetOrderDetailHistory

                        PatientOrderDetailHistory patientOrderDetailHistory = new PatientOrderDetailHistory();
                        patientOrderDetailHistory.PatientOrderDetailUID = patientOrderDetail.UID;
                        patientOrderDetailHistory.ORDSTUID = patientOrderDetail.ORDSTUID;
                        patientOrderDetailHistory.Comments = "From Web RIS";
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

                    //Request request = db.Request.Find(requestDetail.RequestUID);
                    //PatientOrder patientOrder = db.PatientOrder.Find(request.PatientOrderUID);
                }
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("AssignRadiologist")]
        [HttpPut]
        public HttpResponseMessage AssignRadiologist(List<RequestListModel> requestlist, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;

                foreach (var item in requestlist)
                {
                    RequestDetail requestDetail = db.RequestDetail.Find(item.RequestDetailUID);

                    PatientOrderDetail patientOrderDetail = db.PatientOrderDetail.Find(requestDetail.PatientOrderDetailUID);

                    if (requestDetail != null)
                    {

                        int executedUID = db.ReferenceValue.FirstOrDefault(p => p.StatusFlag == "A" && p.DomainCode == "ORDST" && p.ValueCode == "EXCUT").UID;


                        db.RequestDetail.Attach(requestDetail);
                        requestDetail.PreparedByUID = item.ExecuteByUID;
                        requestDetail.PreparedDttm = item.PreparedDttm;
                        requestDetail.ProcessingNote = item.ProcessingNote;
                        requestDetail.RadiologistUID = item.RadiologistUID;

                        if (requestDetail.RadiologistUID == null || requestDetail.RadiologistUID == 0)
                        {
                            DataTable dtSession = SqlStatement.GetRadiologistSession(requestDetail.UID);
                            if (dtSession != null && dtSession.Rows.Count > 0)
                            {
                                List<SessionDefinitionModel> sessionRadio = dtSession.ToList<SessionDefinitionModel>();
                                int radiologistUID = sessionRadio.OrderBy(p => p.RequestNumber).FirstOrDefault().CareproviderUID;
                                requestDetail.RadiologistUID = radiologistUID;
                            }
                        }

                        requestDetail.ORDSTUID = executedUID;
                        requestDetail.MWhen = now;
                        requestDetail.MUser = userID;
                        requestDetail.Comments = item.Comments;

                        db.PatientOrderDetail.Attach(patientOrderDetail);
                        patientOrderDetail.ORDSTUID = executedUID;
                        patientOrderDetail.MWhen = DateTime.Now;
                        patientOrderDetail.MUser = userID;

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

                    //Request request = db.Request.Find(requestDetail.RequestUID);
                    //PatientOrder patientOrder = db.PatientOrder.Find(request.PatientOrderUID);
                }
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("AssignRadiologistForMassResult")]
        [HttpPut]
        public HttpResponseMessage AssignRadiologistForMassResult(RequestListModel requestlist, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;

                RequestDetail requestDetail = db.RequestDetail.Find(requestlist.RequestDetailUID);

                PatientOrderDetail patientOrderDetail = db.PatientOrderDetail.Find(requestDetail.PatientOrderDetailUID);

                if (requestDetail != null)
                {

                    int completedUID = db.ReferenceValue.FirstOrDefault(p => p.StatusFlag == "A" && p.DomainCode == "ORDST" && p.ValueCode == "CMPLT").UID;


                    db.RequestDetail.Attach(requestDetail);
                    requestDetail.PreparedByUID = requestlist.ExecuteByUID;
                    requestDetail.PreparedDttm = requestlist.PreparedDttm;
                    requestDetail.ProcessingNote = requestlist.ProcessingNote;
                    requestDetail.RDUResultedDttm = requestlist.RDUResultedDttm;
                    requestDetail.RDUNote = requestlist.RDUNote;
                    requestDetail.RDUStaffUID = requestlist.RDUStaffUID;
                    requestDetail.RABSTSUID = requestlist.RABSTSUID;
                    //requestDetail.RadiologistUID = requestlist.RadiologistUID;

                    if (requestlist.RadiologistUID == null || requestlist.RadiologistUID == 0)
                    {
                        DataTable dtSession = SqlStatement.GetRadiologistSession(requestDetail.UID);
                        if (dtSession != null && dtSession.Rows.Count > 0)
                        {
                            List<SessionDefinitionModel> sessionRadio = dtSession.ToList<SessionDefinitionModel>();
                            int radiologistUID = sessionRadio.OrderBy(p => p.RequestBulkNumber).FirstOrDefault().CareproviderUID;
                            requestDetail.RadiologistUID = radiologistUID;
                        }
                    }
                    else
                    {
                        requestDetail.RadiologistUID = requestlist.RadiologistUID;
                    }

                    requestDetail.ORDSTUID = completedUID;
                    requestDetail.MWhen = now;
                    requestDetail.MUser = userID;
                    requestDetail.Comments = requestlist.Comments;

                    db.PatientOrderDetail.Attach(patientOrderDetail);
                    patientOrderDetail.ORDSTUID = completedUID;
                    patientOrderDetail.MWhen = DateTime.Now;
                    patientOrderDetail.MUser = userID;

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


                    //Request request = db.Request.Find(requestDetail.RequestUID);
                    //PatientOrder patientOrder = db.PatientOrder.Find(request.PatientOrderUID);
                }
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("AssignToRDUStaff")]
        [HttpPut]
        public HttpResponseMessage AssignToRDUStaff(List<RequestListModel> requestlist, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;

                foreach (var item in requestlist)
                {
                    RequestDetail requestDetail = db.RequestDetail.Find(item.RequestDetailUID);

                    PatientOrderDetail patientOrderDetail = db.PatientOrderDetail.Find(requestDetail.PatientOrderDetailUID);

                    if (requestDetail != null)
                    {

                        int allocatedUID = db.ReferenceValue.FirstOrDefault(p => p.StatusFlag == "A" && p.DomainCode == "ORDST" && p.ValueCode == "OALLC").UID;
                        int executedUID = db.ReferenceValue.FirstOrDefault(p => p.StatusFlag == "A" && p.DomainCode == "ORDST" && p.ValueCode == "EXCUT").UID;

                        db.RequestDetail.Attach(requestDetail);
                        requestDetail.AssignedByUID = item.AssignedByUID;
                        requestDetail.AssignedDttm = item.AssignedDttm;
                        requestDetail.AssignedToUID = item.AssignedToUID;



                        if (requestDetail.ORDSTUID != executedUID)
                            requestDetail.ORDSTUID = allocatedUID;
                        requestDetail.MWhen = now;
                        requestDetail.MUser = userID;

                        db.PatientOrderDetail.Attach(patientOrderDetail);
                        if (patientOrderDetail.ORDSTUID != executedUID)
                            patientOrderDetail.ORDSTUID = allocatedUID;
                        patientOrderDetail.MWhen = DateTime.Now;
                        patientOrderDetail.MUser = userID;

                        #region SavePatinetOrderDetailHistory

                        if (patientOrderDetail.ORDSTUID != executedUID)
                        {
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
                        }

                        #endregion

                        db.SaveChanges();


                        //Request request = db.Request.Find(requestDetail.RequestUID);
                        //PatientOrder patientOrder = db.PatientOrder.Find(request.PatientOrderUID);
                    }
                }

                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("CancelRequest")]
        [HttpPut]
        public HttpResponseMessage CancelRequest(long requestDetailUID, int userID)
        {
            try
            {
                using (var tran = new TransactionScope())
                {
                    DateTime now = DateTime.Now;
                    RequestDetail requestDetail = db.RequestDetail.Find(requestDetailUID);
                    PatientOrderDetail patientOrderDetail = db.PatientOrderDetail.Find(requestDetail.PatientOrderDetailUID);

                    int cancelStatus = db.ReferenceValue.FirstOrDefault(p => p.DomainCode == "ORDST"
                        && p.ValueCode == "CANCLD" && p.StatusFlag == "A").UID;

                    db.RequestDetail.Attach(requestDetail);

                    requestDetail.ORDSTUID = cancelStatus;
                    requestDetail.MUser = userID;
                    requestDetail.MWhen = now;

                    db.PatientOrderDetail.Attach(patientOrderDetail);

                    patientOrderDetail.ORDSTUID = cancelStatus;
                    patientOrderDetail.MUser = userID;
                    patientOrderDetail.MWhen = now;

                    #region SavePatinetOrderDetailHistory

                    PatientOrderDetailHistory patientOrderDetailHistory = new PatientOrderDetailHistory();
                    patientOrderDetailHistory.PatientOrderDetailUID = patientOrderDetail.UID;
                    patientOrderDetailHistory.ORDSTUID = cancelStatus;
                    patientOrderDetailHistory.Comments = "From Web RIS";
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


                    var request = db.Request.Find(requestDetail.RequestUID);

                    db.Request.Attach(request);
                    request.MUser = userID;
                    request.MWhen = now;
                    request.ORDSTUID = CheckRequestStatus(request.UID);

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
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public int? CheckRequestStatus(long requestuid)
        {
            int? ORDSTUID = null;
            List<RequestDetail> listRequestDetail = db.RequestDetail.Where(p => p.RequestUID == requestuid).ToList();
            List<ReferenceValue> listOrderStatus = db.ReferenceValue.Where(p => p.DomainCode == "ORDST").ToList();
            ReferenceValue review = listOrderStatus.Where(p => p.ValueCode == "REVIW").FirstOrDefault();
            ReferenceValue complete = listOrderStatus.Where(p => p.ValueCode == "CMPLT").FirstOrDefault();
            ReferenceValue specimenCollect = listOrderStatus.Where(p => p.ValueCode == "SAMPLCOL").FirstOrDefault();
            ReferenceValue specimenAccept = listOrderStatus.Where(p => p.ValueCode == "ACPSMP").FirstOrDefault();
            ReferenceValue specimenRejected = listOrderStatus.Where(p => p.ValueCode == "REJSMP").FirstOrDefault();
            ReferenceValue cancel = listOrderStatus.Where(p => p.ValueCode == "CANCLD").FirstOrDefault();
            ReferenceValue partiallyReview = listOrderStatus.Where(p => p.ValueCode == "PREVI").FirstOrDefault();
            ReferenceValue partiallycomplete = listOrderStatus.Where(p => p.ValueCode == "PARCMP").FirstOrDefault();
            ReferenceValue partiallycollect = listOrderStatus.Where(p => p.ValueCode == "PARCOLLEC").FirstOrDefault();
            ReferenceValue partiallycancel = listOrderStatus.Where(p => p.ValueCode == "PRCAN").FirstOrDefault();
            int reviewCount = listRequestDetail.Where(p => p.ORDSTUID == review.UID).Count();
            int completeCount = listRequestDetail.Where(p => p.ORDSTUID == complete.UID).Count();
            int collectCount = listRequestDetail.Where(p => p.ORDSTUID == specimenCollect.UID).Count();
            int acceptCount = listRequestDetail.Where(p => p.ORDSTUID == specimenAccept.UID).Count();
            int RejectCount = listRequestDetail.Where(p => p.ORDSTUID == specimenRejected.UID).Count();
            int cancelCount = listRequestDetail.Where(p => p.ORDSTUID == cancel.UID).Count();
            int RequestDetailCount = listRequestDetail.Count();
            if (listRequestDetail != null)
            {
                if (reviewCount == RequestDetailCount)
                {
                    ORDSTUID = review.UID;
                }
                else if (completeCount == RequestDetailCount)
                {
                    ORDSTUID = complete.UID;
                }
                else if (acceptCount == RequestDetailCount)
                {
                    ORDSTUID = specimenAccept.UID;
                }
                else if(collectCount == RequestDetailCount)
                {
                    ORDSTUID = specimenCollect.UID;
                }
                else if (cancelCount == RequestDetailCount)
                {
                    ORDSTUID = cancel.UID;
                }
                else if(RejectCount == RequestDetailCount)
                {
                    ORDSTUID = specimenRejected.UID;
                }
                else if (reviewCount >= 1)
                {
                    ORDSTUID = partiallyReview.UID;
                    //ORDSTUID = review.UID;
                }
                else if (completeCount >= 1 || acceptCount >= 1)
                {
                    ORDSTUID = partiallycomplete.UID;
                    //ORDSTUID = complete.UID;
                }
                else if (collectCount >= 1)
                {
                    ORDSTUID = partiallycollect.UID;
                }
                else if (cancelCount >= 1)
                {
                    ORDSTUID = partiallycancel.UID;
                }
            }
            return ORDSTUID;
        }

        public string GetNavigationImage(string accessionNumber, string patientID, string modality)
        {
            string pacUrl = string.Empty;
            var IsMatch = (new TechnicalController()).CheckAccessionNumberInPacs(accessionNumber);
            if (IsMatch != "Cannot Open Database Pacs" && IsMatch != "Not Found")
            {
                pacUrl = "http://192.168.2.3/viewer/view.html#id=admin&password=password" + "&accessionnumber=" + accessionNumber;
            }
            if (string.IsNullOrEmpty(pacUrl))
            {
                pacUrl = "http://192.168.2.3/viewer/list.html#id=admin&password=password" + "&patientid=" + patientID + "&Modality=" + modality;
            }
            return pacUrl;

        }

        [Route("GetScheduleRadiologist")]
        [HttpGet]
        public List<ScheduleRadiologistModel> GetScheduleRadiologist(DateTime startDate, DateTime endDate)
        {
            List<ScheduleRadiologistModel> scheduleData = (from s in db.ScheduleRadiologist
                                                           where s.StatusFlag == "A"
                                                           && (startDate == null || DbFunctions.TruncateTime(s.StartWorkTime) <= DbFunctions.TruncateTime(endDate))
                                                           && (endDate == null || DbFunctions.TruncateTime(s.EndWorkTime) >= DbFunctions.TruncateTime(startDate))
                                                           select new ScheduleRadiologistModel
                                                           {
                                                               ScheduleRadiologistUID = s.UID,
                                                               CareproviderUID = s.CareproviderUID,
                                                               CareproviderName = SqlFunction.fGetCareProviderName(s.CareproviderUID),
                                                               RIMTYPUID = s.RIMTYPUID,
                                                               ImageType = SqlFunction.fGetRfValCode(s.RIMTYPUID ?? 0),
                                                               StartWorkTime = s.StartWorkTime,
                                                               EndWorkTime = s.EndWorkTime,
                                                               AllDay = false,
                                                               Status = 2,
                                                               Label = 1,
                                                               EventType = 0,
                                                           }).ToList();
            scheduleData.ForEach(c =>
            {
                c.StartWorkTime = c.StartWorkTime.ToString("HH:mm") == "01:00" ? c.StartWorkTime.AddDays(-1) : c.StartWorkTime;
                c.EndWorkTime = c.EndWorkTime.ToString("HH:mm") == "07:00" ? c.EndWorkTime.AddDays(-1) : c.EndWorkTime.ToString("HH:mm") == "00:59" ? c.EndWorkTime.AddHours(-1) : c.EndWorkTime;
            });

            return scheduleData;
        }

        [Route("GetScheduleRadiologistByUID")]
        [HttpGet]
        public ScheduleRadiologistModel GetScheduleRadiologistByUID(int scheduleRadiologistUID)
        {
            ScheduleRadiologistModel scheduleRadiologist = db.ScheduleRadiologist
                .Where(p => p.UID == scheduleRadiologistUID)
                .Select(s => new ScheduleRadiologistModel
                {
                    ScheduleRadiologistUID = s.UID,
                    CareproviderUID = s.CareproviderUID,
                    CareproviderName = SqlFunction.fGetCareProviderName(s.CareproviderUID),
                    RIMTYPUID = s.RIMTYPUID,
                    ImageType = SqlFunction.fGetRfValCode(s.RIMTYPUID ?? 0),
                    StartWorkTime = s.StartWorkTime,
                    EndWorkTime = s.EndWorkTime,
                    AllDay = false,
                    Status = 2,
                    Label = 1,
                    EventType = 0,
                }).FirstOrDefault();

            return scheduleRadiologist;
        }

        [Route("AddOrUpdateScheDuleRadiologist")]
        [HttpPost]
        public HttpResponseMessage AddOrUpdateScheDuleRadiologist(ScheduleRadiologistModel scheduleModel,int userUID)
        {
            try
            {
                DateTime now = DateTime.Now;
                ScheduleRadiologist scheduleRadiologist = db.ScheduleRadiologist.Find(scheduleModel.ScheduleRadiologistUID);
                if (scheduleRadiologist == null)
                {
                    scheduleRadiologist = new ScheduleRadiologist();
                    scheduleRadiologist.CUser = userUID;
                    scheduleRadiologist.CWhen = now;
                    scheduleRadiologist.StatusFlag = "A";
                }

                scheduleRadiologist.CareproviderUID = scheduleModel.CareproviderUID;
                scheduleRadiologist.StartWorkTime = scheduleModel.StartWorkTime;
                scheduleRadiologist.EndWorkTime = scheduleModel.EndWorkTime;
                scheduleRadiologist.RIMTYPUID = scheduleModel.RIMTYPUID;
                scheduleRadiologist.MUser = userUID;
                scheduleRadiologist.MWhen = now;
                db.ScheduleRadiologist.AddOrUpdate(scheduleRadiologist);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, scheduleRadiologist.UID);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("DeleteScheDuleRadiologist")]
        [HttpDelete]
        public HttpResponseMessage DeleteScheDuleRadiologist(int scheduleRadiologistUID, int userUID)
        {
            try
            {
                DateTime now = DateTime.Now;
                ScheduleRadiologist scheduleRadiologist = db.ScheduleRadiologist.Find(scheduleRadiologistUID);
                db.ScheduleRadiologist.Attach(scheduleRadiologist);
                scheduleRadiologist.StatusFlag = "D";
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("SearchSession")]
        [HttpGet]
        public List<SessionDefinitionModel> SearchSession(DateTime? startDate, DateTime? endDate, int? careproviderUID)
        {
            List<SessionDefinitionModel> sessionData = (from s in db.SessionDefinition
                                                        where s.StatusFlag == "A"
                                                        && (startDate == null || DbFunctions.TruncateTime(s.StartDttm) <= DbFunctions.TruncateTime(endDate))
                                                        && (endDate == null || DbFunctions.TruncateTime(s.EndDttm) >= DbFunctions.TruncateTime(startDate))
                                                        && (careproviderUID == null || s.CareproviderUID == careproviderUID)
                                                        select new SessionDefinitionModel
                                                        {
                                                            SessionDefinitionUID = s.UID,
                                                            StartDttm = s.StartDttm,
                                                            EndDttm = s.EndDttm,
                                                            SessionStartDttm = s.SessionStartDttm,
                                                            SessionEndDttm = s.SessionEndDttm,
                                                            CareproviderUID = s.CareproviderUID,
                                                            CareproviderName = SqlFunction.fGetCareProviderName(s.CareproviderUID),
                                                            Day1 = s.Day1,
                                                            Day2 = s.Day2,
                                                            Day3 = s.Day3,
                                                            Day4 = s.Day4,
                                                            Day5 = s.Day5,
                                                            Day6 = s.Day6,
                                                            Day7 = s.Day7,
                                                            CWhen = s.CWhen,
                                                            MWhen = s.MWhen,
                                                            StatusFlag = s.StatusFlag,
                                                            CUser = s.CUser,
                                                            MUser = s.MUser
                                                        }).ToList();

            return sessionData;
        }

        [Route("ManageSession")]
        [HttpPost]
        public HttpResponseMessage ManageSession(SessionDefinitionModel sessionModel, int userUID)
        {
            try
            {
                DateTime now = DateTime.Now;
                SessionDefinition sessionDefiniton = db.SessionDefinition.Find(sessionModel.SessionDefinitionUID);
                if (sessionDefiniton == null)
                {
                    sessionDefiniton = new SessionDefinition();
                    sessionDefiniton.CUser = userUID;
                    sessionDefiniton.CWhen = now;
                    sessionDefiniton.StatusFlag = "A";
                }

                sessionDefiniton.CareproviderUID = sessionModel.CareproviderUID;
                sessionDefiniton.SessionStartDttm = sessionModel.SessionStartDttm;
                sessionDefiniton.SessionEndDttm = sessionModel.SessionEndDttm;
                sessionDefiniton.StartDttm = sessionModel.StartDttm;
                sessionDefiniton.EndDttm = sessionModel.EndDttm;
                sessionDefiniton.Day1 = sessionModel.Day1;
                sessionDefiniton.Day2 = sessionModel.Day2;
                sessionDefiniton.Day3 = sessionModel.Day3;
                sessionDefiniton.Day4 = sessionModel.Day4;
                sessionDefiniton.Day5 = sessionModel.Day5;
                sessionDefiniton.Day6 = sessionModel.Day6;
                sessionDefiniton.Day7 = sessionModel.Day7;
                sessionDefiniton.MUser = userUID;
                sessionDefiniton.MWhen = now;

                db.SessionDefinition.AddOrUpdate(sessionDefiniton);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("GetWithDrawSessionBySessionUID")]
        [HttpGet]
        public List<SessionWithdrawnDetailModel> GetWithDrawSessionBySessionUID(int sessionUID)
        {
            List<SessionWithdrawnDetailModel> withDrawData = db.SessionWithdrawnDetail.Where(p => p.StatusFlag == "A" && p.SessionDefinitionUID == sessionUID).Select(p => new SessionWithdrawnDetailModel
            {
                SessionWithdrawnUID = p.UID,
                SessionDefinitionUID = p.SessionDefinitionUID,
                WTHRSUID = p.WTHRSUID,
                WithDrawReason = SqlFunction.fGetRfValDescription(p.WTHRSUID),
                Comments = p.Comments,
                StartDttm = p.StartDttm,
                EndDttm = p.EndDttm,
                CUser = p.CUser,
                MUser = p.MUser,
                CWhen = p.CWhen,
                MWhen = p.MWhen
            }).ToList();

            return withDrawData;
        }

        [Route("WithDrawSession")]
        [HttpPost]
        public HttpResponseMessage WithDrawSession(List<SessionWithdrawnDetailModel> withDrawSessionModels, int sessionUID, int userUID)
        {
            try
            {
                DateTime now = DateTime.Now;
                using (var tran = new TransactionScope())
                {
                    int sessionDefinitionUId = sessionUID;
                    #region Delete SessionWithdrawnDetail
                    IEnumerable<SessionWithdrawnDetail> sessionWithDraws = db.SessionWithdrawnDetail.Where(p => p.StatusFlag == "A" && p.SessionDefinitionUID == sessionDefinitionUId);

                    if (withDrawSessionModels == null)
                    {
                        foreach (var item in sessionWithDraws)
                        {
                            db.SessionWithdrawnDetail.Attach(item);
                            item.MUser = userUID;
                            item.MWhen = now;
                            item.StatusFlag = "D";
                        }
                    }
                    else
                    {
                        foreach (var item in sessionWithDraws)
                        {
                            var data = withDrawSessionModels.FirstOrDefault(p => p.SessionWithdrawnUID == item.UID);
                            if (data == null)
                            {
                                db.SessionWithdrawnDetail.Attach(item);
                                item.MUser = userUID;
                                item.MWhen = now;
                                item.StatusFlag = "D";
                            }

                        }
                    }

                    db.SaveChanges();

                    #endregion
                    foreach (var item in withDrawSessionModels)
                    {

                        SessionWithdrawnDetail withDrawSession = db.SessionWithdrawnDetail.Find(item.SessionWithdrawnUID);
                        if (withDrawSession == null)
                        {
                            withDrawSession = new SessionWithdrawnDetail();
                            withDrawSession.CUser = userUID;
                            withDrawSession.CWhen = now;
                            withDrawSession.StatusFlag = "A";
                        }
                        withDrawSession.SessionDefinitionUID = item.SessionDefinitionUID;
                        withDrawSession.WTHRSUID = item.WTHRSUID;
                        withDrawSession.StartDttm = item.StartDttm;
                        withDrawSession.EndDttm = item.EndDttm;
                        withDrawSession.Comments = item.Comments;
                        withDrawSession.MUser = userUID;
                        withDrawSession.MWhen = now;

                        db.SessionWithdrawnDetail.AddOrUpdate(withDrawSession);
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

        [Route("DeleteSession")]
        [HttpPut]
        public HttpResponseMessage DeleteSession(int sessionUID, int userUID)
        {
            try
            {
                SessionDefinition session = db.SessionDefinition.Find(sessionUID);
                db.SessionDefinition.Attach(session);
                session.StatusFlag = "D";
                session.MWhen = DateTime.Now;
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("GetRadiologistReport")]
        [HttpGet]
        public List<DoctorFeeModel> GetRadiologistReport(DateTime dateFrom, DateTime dateTo, int? radiologistUID)
        {
            List<DoctorFeeModel> listRadiologsitReport = (from rs in db.Result
                                                          join rd in db.RequestDetail on rs.RequestDetailUID equals rd.UID
                                                          join pd in db.PatientOrderDetail on rd.PatientOrderDetailUID equals pd.UID
                                                          where rs.StatusFlag == "A"
                                                          && rd.StatusFlag == "A"
                                                          && pd.StatusFlag == "A"
                                                          && rs.ORDSTUID == 2863
                                                          && pd.ORDSTUID == 2863
                                                          && (dateFrom == null || DbFunctions.TruncateTime(rs.ResultEnteredDttm) >= DbFunctions.TruncateTime(dateFrom))
                                                          && (dateTo == null || DbFunctions.TruncateTime(rs.ResultEnteredDttm) <= DbFunctions.TruncateTime(dateTo))
                                                          && (radiologistUID == null || rs.RadiologistUID == radiologistUID)
                                                          select new DoctorFeeModel
                                                          {
                                                              PatientName = SqlFunction.fGetPatientID(rs.PatientUID) + " : " + SqlFunction.fGetPatientName(rs.PatientUID),
                                                              CareproviderName = SqlFunction.fGetCareProviderName(rs.RadiologistUID ?? 0),
                                                              ResultEnteredDate = rs.ResultEnteredDttm.Value,
                                                              ResultEnteredTime = rs.ResultEnteredDttm.Value,
                                                              ItemName = rs.RequestItemCode + " : " + rs.RequestItemName,
                                                              DoctorFee = pd.NetAmount ?? 0
                                                          }).ToList();

            return listRadiologsitReport;
        }

        [Route("GetDoctorFeeNonPay")]
        [HttpGet]
        public List<DoctorFeeModel> GetDoctorFeeNonPay(DateTime dateFrom, DateTime dateTo, int? radiologistUID)
        {
            DataTable dt = SqlDirectStore.pGetDoctorFeeNonPay(dateFrom, dateTo, radiologistUID);
            List<DoctorFeeModel> listData = new List<DoctorFeeModel>();
            if (dt != null && dt.Rows.Count > 0)
            {
                listData = dt.ToList<DoctorFeeModel>();
            }

            return listData;
        }

        [Route("GetDoctorFee")]
        [HttpGet]
        public List<DoctorFeeModel> GetDoctorFee(DateTime dateFrom, DateTime dateTo, int? radiologistUID)
        {
            DataTable dt = SqlDirectStore.pGetDoctorFee(dateFrom, dateTo, radiologistUID);
            List<DoctorFeeModel> listData = new List<DoctorFeeModel>();
            if (dt != null && dt.Rows.Count > 0)
            {
                listData = dt.ToList<DoctorFeeModel>();
            }

            return listData;
        }

        [Route("AddDoctorFee")]
        [HttpPost]
        public HttpResponseMessage AddDoctorFee(List<DoctorFeeModel> DoctorFeesModel, int userUID)
        {
            try
            {
                foreach (var item in DoctorFeesModel)
                {
                    DoctorFee newModel = new DoctorFee();
                    newModel.CareproviderUID = item.CareproviderUID;
                    newModel.ItemName = item.ItemName;
                    newModel.ItemCode = item.ItemCode;
                    newModel.PatientOrderDetailUID = item.PatientOrderDetailUID;
                    newModel.PatientUID = item.PatientUID;
                    newModel.PatientVisitUID = item.PatientVisitUID;
                    newModel.NetAmount = item.NetAmount;
                    newModel.DoctorFee1 = item.DoctorFee;
                    newModel.Vat = item.Vat;
                    newModel.Radread = item.Radread;
                    newModel.StartDttm = item.StartDttm;
                    newModel.CUser = userUID;
                    newModel.CWhen = DateTime.Now;
                    newModel.MUser = userUID;
                    newModel.MWhen = DateTime.Now;
                    newModel.StatusFlag = "A";
                    db.DoctorFee.Add(newModel);

                    db.SaveChanges();
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("RemoveDoctorFee")]
        [HttpPut]
        public HttpResponseMessage RemoveDoctorFee(List<DoctorFeeModel> DoctorFeesModel, int userUID)
        {
            try
            {
                foreach (var item in DoctorFeesModel)
                {
                    DoctorFee model = db.DoctorFee.Find(item.DoctorFeeUID);
                    db.DoctorFee.Attach(model);
                    model.StatusFlag = "D";
                    model.MUser = userUID;
                    model.MWhen = DateTime.Now;
                    db.SaveChanges();
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("GetNotificationTaskResult")]
        [HttpGet]
        public List<NotificationTaskResultModel> GetNotificationTaskResult(string healthOrganisationCode)
        {
            DataTable dt = SqlStatement.GetNotificationTaskResult(healthOrganisationCode);
            List<NotificationTaskResultModel> listData = new List<NotificationTaskResultModel>();
            if (dt != null && dt.Rows.Count > 0)
            {
                listData = dt.ToList<NotificationTaskResultModel>();
            }

            return listData;
        }

        [Route("UpdateNotificationTaskResult")]
        [HttpPut]
        public HttpResponseMessage UpdateNotificationTaskResult(long notificationTaskUID)
        {
            try
            {
                var notficationData = db.NotificationTaskResult.Find(notificationTaskUID);
                if (notficationData != null)
                {
                    db.NotificationTaskResult.Attach(notficationData);
                    notficationData.Status = 1;
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
