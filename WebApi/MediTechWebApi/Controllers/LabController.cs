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
    [RoutePrefix("Api/Lab")]
    public class LabController : ApiController
    {
        protected MediTechEntities db = new MediTechEntities();

        [Route("SearchRequestLabList")]
        [HttpGet]
        public List<RequestLabModel> SearchRequestLabList(DateTime? requestDateFrom, DateTime? requestDateTo, string statusLis, long? patientUID, int? requestItemUID, string labNumber, int? payorDetailUID, int? organisationUID)
        {
            DataTable dataTable = SqlDirectStore.pSearchRequestLabList(requestDateFrom, requestDateTo, statusLis, patientUID, requestItemUID, labNumber, payorDetailUID, organisationUID);
            List<RequestLabModel> listData = dataTable.ToList<RequestLabModel>();
            return listData;
        }

        [Route("SearchResultLabList")]
        [HttpGet]
        public List<PatientResultLabModel> SearchResultLabList(DateTime? dateFrom, DateTime? dateTo, long? patientUID, int? payorDetailUID)
        {
            List<PatientResultLabModel> data = null;
            DataTable dt = SqlDirectStore.pSearchResultLabList(dateFrom, dateTo, patientUID, payorDetailUID);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<PatientResultLabModel>();
                data = dt.ToList<PatientResultLabModel>();
            }

            return data;
        }


        [Route("GetRequesDetailLabByRequestUID")]
        [HttpGet]
        public List<RequestDetailItemModel> GetRequesDetailLabByRequestUID(long requestUID)
        {
            List<RequestDetailItemModel> data = db.RequestDetail.Where(p => p.RequestUID == requestUID && p.StatusFlag == "A")
                .Select(p => new RequestDetailItemModel
                {
                    RequestDetailUID = p.UID,
                    RequestItemName = p.RequestItemName,
                    RequestItemCode = p.RequestItemCode,
                    OrderStatus = SqlFunction.fGetRfValDescription(p.ORDSTUID),
                    PriorityStatus = SqlFunction.fGetRfValDescription(p.RQPRTUID ?? 0),
                    Comments = p.Comments
                }).ToList();

            return data;
        }

        public List<RequestDetailSpecimenModel> GetRequestDetailSpecimenByRequestUID(long requestUID)
        {
            List<RequestDetailSpecimenModel> data = (from rd in db.RequestDetail
                                                     join rds in db.RequestDetailSpecimen
                                                     on new { key1 = rd.UID, key2 = "A" } equals new { key1 = rds.RequestDetailUID, key2 = rds.StatusFlag } into gj
                                                     from subrds in gj.DefaultIfEmpty()
                                                     join sp in db.Specimen
                                                     on new { key1 = subrds.SpecimenUID, key2 = "A" } equals new { key1 = sp.UID, key2 = sp.StatusFlag } into spm
                                                     from susp in spm.DefaultIfEmpty()
                                                     where rd.StatusFlag == "A"
                                                     && rd.RequestUID == requestUID
                                                     select new RequestDetailSpecimenModel
                                                     {
                                                         RequestUID = rd.RequestUID,
                                                         RequestDetailUID = rd.UID,
                                                         RequestDetailSpecimenUID = subrds.UID,
                                                         RequestItemUID = rd.RequestitemUID,
                                                         RequestItemCode = rd.RequestItemCode,
                                                         RequestItemName = rd.RequestItemName,
                                                         SpecimenUID = subrds.SpecimenUID,
                                                         SpecimenCode = susp.Code,
                                                         SpecimenName = subrds.SpecimenName,
                                                         SpecimenType = SqlFunction.fGetRfValDescription(subrds.SPMTPUID),
                                                         //SpecimenStatus = SqlFunction.fGetRfValDescription(subrds.SPSTSUID ?? 0),
                                                         SpecimenStatus = SqlFunction.fGetRfValDescription(subrds.SPSTSUID ?? 2847),
                                                         SPSTSUID = subrds.SPSTSUID ?? 2847,
                                                         VolumeCollected = subrds.VolumeCollected,
                                                         VolumnUnit = SqlFunction.fGetRfValDescription(subrds.VOUNTUID ?? 0),
                                                         CollectionDttm = subrds.CollectionDttm,
                                                         CollectedByName = SqlFunction.fGetCareProviderName(subrds.CollectedBy ?? 0),
                                                         CollectionSite = SqlFunction.fGetRfValDescription(subrds.COLSTUID ?? 0),
                                                         CollectionRoute = SqlFunction.fGetRfValDescription(subrds.CLROUUID ?? 0),
                                                         CollectionMethod = SqlFunction.fGetRfValDescription(subrds.COLMDUID ?? 0),
                                                         StorageInstuction = subrds.StorageInstuction,
                                                         HandlingInstruction = subrds.HandlingInstruction,
                                                         Suffix = susp.Suffix,
                                                         ReviewComments = subrds.ReviewComments
                                                     }).ToList();

            if (data != null && data.Count() > 0)
            {
                foreach (var item in data.ToList())
                {
                    item.RequestItemSpecimens = (from rsp in db.RequestItemSpecimen
                                                 join spc in db.Specimen on rsp.SpecimenUID equals spc.UID
                                                 where rsp.StatusFlag == "A"
                                                 && rsp.RequestItemUID == item.RequestItemUID
                                                 select new RequestItemSpecimenModel
                                                 {
                                                     RequestItemSpecimenUID = rsp.UID,
                                                     RequestItemUID = rsp.RequestItemUID,
                                                     SpecimenUID = rsp.SpecimenUID,
                                                     SpecimenName = rsp.SpecimenName,
                                                     IsDefault = rsp.IsDefault,
                                                     VolumeCollected = spc.VolumeCollected,
                                                     VolumeUnit = SqlFunction.fGetRfValDescription(spc.VOUNTUID ?? 0),
                                                     CollectionRoute = SqlFunction.fGetRfValDescription(spc.CLROUUID ?? 0),
                                                     CollectionMethod = SqlFunction.fGetRfValDescription(spc.COLMDUID ?? 0),
                                                     CollectionSite = SqlFunction.fGetRfValDescription(spc.COLSTUID ?? 0),
                                                     SpecimenType = SqlFunction.fGetRfValDescription(spc.SPMTPUID ?? 0),
                                                     Suffix = spc.Suffix
                                                 }).ToList();
                    if (item.RequestItemSpecimens != null && item.RequestItemSpecimens.Count > 0)
                    {
                        if (item.SpecimenUID != null && item.SpecimenUID != 0)
                        {
                            item.SelectRequestItemSpecimen = item.RequestItemSpecimens.FirstOrDefault(p => p.SpecimenUID == item.SpecimenUID);
                        }
                        else
                        {
                            item.SelectRequestItemSpecimen = item.RequestItemSpecimens.FirstOrDefault(p => p.IsDefault == "Y");
                            if (item.SelectRequestItemSpecimen == null)
                            {
                                item.SelectRequestItemSpecimen = item.RequestItemSpecimens.FirstOrDefault();
                            }
                        }
                        //item.SpecimenUID = item.SelectRequestItemSpecimen.SpecimenUID;
                        //item.SpecimenName = item.SelectRequestItemSpecimen.SpecimenName;
                    }
                    else
                    {
                        data.Remove(item);
                    }

                }

            }



            return data;
        }

        [Route("GetRequesDetailLabForImport")]
        [HttpGet]
        public RequestDetailItemModel GetRequesDetailLabForImport(string patientID, int ownerOrganisationUID, int payorDetailUID
            , int requestItemUID, DateTime? dateFrom, DateTime? dateTo = null)
        {
            DataTable dataTable = SqlDirectStore.pGetRequesDetailLabForImport(patientID, ownerOrganisationUID, payorDetailUID
                , requestItemUID, dateFrom, dateTo);
            List<RequestDetailItemModel> returnData = dataTable.ToList<RequestDetailItemModel>();
            return returnData.FirstOrDefault();
        }

        [Route("GetResultLabByPatientVisitUID")]
        [HttpGet]
        public List<ResultModel> GetResultLabByPatientVisitUID(long patientVisitUID)
        {
            List<ResultModel> data = (from re in db.Request
                                      join red in db.RequestDetail on re.UID equals red.RequestUID
                                      join rs in db.Result on red.UID equals rs.RequestDetailUID
                                      where rs.StatusFlag == "A"
                                      && red.StatusFlag == "A"
                                      && re.BSMDDUID == 2813
                                      && rs.PatientVisitUID == patientVisitUID
                                      select new ResultModel
                                      {
                                          RequestItemCode = red.RequestItemCode,
                                          RequestItemName = red.RequestItemName,
                                          ResultEnteredDttm = rs.ResultEnteredDttm,
                                          ResultNumber = rs.ResultNumber,
                                          LabNumber = re.RequestNumber,
                                          ResultEnteredUserUID = rs.ResultedByUID,
                                          ResultEnteredUser = SqlFunction.fGetCareProviderName(rs.ResultedByUID ?? 0),
                                          ORDSTUID = rs.ORDSTUID,
                                          OrderStatus = SqlFunction.fGetCareProviderName(rs.ORDSTUID)
                                      }).ToList();



            return data;
        }

        [Route("GetResultLabGroupRequestNumberByVisit")]
        [HttpGet]
        public List<RequestLabModel> GetResultLabGroupRequestNumberByVisit(long patientVisitUID, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            int complete = 2845;
            int partiallyReviewed = 2864;
            int partiallyCompleted = 2876;
            int reviewed = 2863;
            List<RequestLabModel> dataRequest = (from re in db.Request
                                                 where re.StatusFlag == "A"
                                                 && re.PatientVisitUID == patientVisitUID
                                                 && (re.ORDSTUID == complete || re.ORDSTUID == partiallyReviewed || re.ORDSTUID == partiallyCompleted || re.ORDSTUID == reviewed)
                                                 && (dateFrom == null || DbFunctions.TruncateTime(re.RequestedDttm) >= DbFunctions.TruncateTime(dateFrom))
                                                 && re.BSMDDUID == 2813
                                                 && (dateTo == null || DbFunctions.TruncateTime(re.RequestedDttm) <= DbFunctions.TruncateTime(dateTo))
                                                 select new RequestLabModel
                                                 {
                                                     RequestUID = re.UID,
                                                     LabNumber = re.RequestNumber,
                                                     RequestedDttm = re.RequestedDttm,
                                                     PatientName = SqlFunction.fGetPatientName(re.PatientUID),
                                                     PatientID = SqlFunction.fGetPatientID(re.PatientUID),
                                                     OrderStatus = SqlFunction.fGetRfValDescription(re.ORDSTUID ?? 0),
                                                     PatientUID = re.PatientUID,
                                                     PatientVisitUID = re.PatientVisitUID
                                                 }).ToList();

            if (dataRequest != null)
            {
                foreach (var request in dataRequest)
                {
                    request.RequestDetailLabs = (from red in db.RequestDetail
                                                 join rs in db.Result on red.UID equals rs.RequestDetailUID
                                                 where rs.StatusFlag == "A"
                                                 && red.StatusFlag == "A"
                                                 && red.RequestUID == request.RequestUID
                                                 && (red.ORDSTUID == complete || red.ORDSTUID == reviewed)
                                                 select new RequestDetailItemModel
                                                 {
                                                     RequestUID = red.RequestUID,
                                                     RequestDetailUID = red.UID,
                                                     RequestItemCode = red.RequestItemCode,
                                                     RequestItemName = red.RequestItemName,
                                                     ResultUID = rs.UID,
                                                     ResultEnteredDttm = rs.ResultEnteredDttm,
                                                     ResultedEnterBy = SqlFunction.fGetCareProviderName(rs.ResultEnteredUserUID ?? 0),
                                                     OrderStatus = SqlFunction.fGetRfValDescription(red.ORDSTUID)
                                                 }).ToList();

                    if (request.RequestDetailLabs != null)
                    {
                        foreach (var reqeustDetail in request.RequestDetailLabs)
                        {
                            reqeustDetail.ResultComponents = new System.Collections.ObjectModel.ObservableCollection<ResultComponentModel>(
                                db.ResultComponent.Where(p => p.ResultUID == reqeustDetail.ResultUID && p.StatusFlag == "A")
                                .Select(p => new ResultComponentModel
                                {
                                    ResultUID = p.ResultUID,
                                    ReferenceRange = p.ReferenceRange,
                                    ResultItemCode = p.ResultItemCode,
                                    ResultValue = p.ResultValue,
                                    ResultComponentUID = p.UID,
                                    RVTYPUID = p.RVTYPUID,
                                    ResultValueType = SqlFunction.fGetRfValDescription(p.RVTYPUID),
                                    Low = p.Low,
                                    High = p.High,
                                    ResultItemName = p.ResultItemName,
                                    UnitofMeasure = SqlFunction.fGetRfValDescription(p.RSUOMUID ?? 0),
                                    IsAbnormal = p.IsAbnormal
                                }).ToList());
                        }
                        request.ReviewedBy = request.RequestDetailLabs.FirstOrDefault().ResultedEnterBy;
                    }

                }
            }


            return dataRequest.OrderByDescending(p => p.RequestedDttm).ToList();
        }

        [Route("GetResultLabGroupRequestNumberByPatient")]
        [HttpGet]
        public List<RequestLabModel> GetResultLabGroupRequestNumberByPatient(long patientUID, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            int complete = 2845;
            int partiallyReviewed = 2864;
            int partiallyCompleted = 2876;
            int reviewed = 2863;
            List<RequestLabModel> dataRequest = (from pv in db.PatientVisit
                                                 join re in db.Request on pv.UID equals re.PatientVisitUID
                                                 where pv.StatusFlag == "A"
                                                 && re.StatusFlag == "A"
                                                 && re.PatientUID == patientUID
                                                 && (re.ORDSTUID == complete || re.ORDSTUID == partiallyReviewed || re.ORDSTUID == partiallyCompleted || re.ORDSTUID == reviewed)
                                                 && (dateFrom == null || DbFunctions.TruncateTime(re.RequestedDttm) >= DbFunctions.TruncateTime(dateFrom))
                                                 && re.BSMDDUID == 2813
                                                 && (dateTo == null || DbFunctions.TruncateTime(re.RequestedDttm) <= DbFunctions.TruncateTime(dateTo))
                                                 select new RequestLabModel
                                                 {
                                                     RequestUID = re.UID,
                                                     LabNumber = re.RequestNumber,
                                                     RequestedDttm = re.RequestedDttm,
                                                     PatientName = SqlFunction.fGetPatientName(re.PatientUID),
                                                     PatientID = SqlFunction.fGetPatientID(re.PatientUID),
                                                     OrderStatus = SqlFunction.fGetRfValDescription(re.ORDSTUID ?? 0),
                                                     PatientUID = re.PatientUID,
                                                     PatientVisitUID = re.PatientVisitUID
                                                 }).ToList();

            if (dataRequest != null)
            {
                foreach (var request in dataRequest)
                {
                    request.RequestDetailLabs = (from red in db.RequestDetail
                                                 join rs in db.Result on red.UID equals rs.RequestDetailUID
                                                 where rs.StatusFlag == "A"
                                                 && red.StatusFlag == "A"
                                                 && red.RequestUID == request.RequestUID
                                                 && (red.ORDSTUID == complete || red.ORDSTUID == reviewed)
                                                 select new RequestDetailItemModel
                                                 {
                                                     RequestUID = red.RequestUID,
                                                     RequestDetailUID = red.UID,
                                                     RequestItemCode = red.RequestItemCode,
                                                     RequestItemName = red.RequestItemName,
                                                     ResultUID = rs.UID,
                                                     ResultEnteredDttm = rs.ResultEnteredDttm,
                                                     ResultedEnterBy = SqlFunction.fGetCareProviderName(rs.ResultEnteredUserUID ?? 0),
                                                     OrderStatus = SqlFunction.fGetRfValDescription(red.ORDSTUID)
                                                 }).ToList();

                    if (request.RequestDetailLabs != null)
                    {
                        foreach (var reqeustDetail in request.RequestDetailLabs)
                        {
                            reqeustDetail.ResultComponents = new System.Collections.ObjectModel.ObservableCollection<ResultComponentModel>(
                                db.ResultComponent.Where(p => p.ResultUID == reqeustDetail.ResultUID && p.StatusFlag == "A")
                                .Select(p => new ResultComponentModel
                                {
                                    ResultUID = p.ResultUID,
                                    ReferenceRange = p.ReferenceRange,
                                    ResultItemCode = p.ResultItemCode,
                                    ResultValue = p.ResultValue,
                                    ResultComponentUID = p.UID,
                                    ResultValueType = SqlFunction.fGetRfValDescription(p.RVTYPUID),
                                    RVTYPUID = p.RVTYPUID,
                                    Low = p.Low,
                                    High = p.High,
                                    ResultItemName = p.ResultItemName,
                                    UnitofMeasure = SqlFunction.fGetRfValDescription(p.RSUOMUID ?? 0),
                                    IsAbnormal = p.IsAbnormal
                                }).ToList());
                        }
                        request.ReviewedBy = request.RequestDetailLabs.FirstOrDefault().ResultedEnterBy;
                    }

                }
            }


            return dataRequest.OrderByDescending(p => p.RequestedDttm).ToList();
        }

        [Route("GetResultLabGroupRequestNumber")]
        [HttpGet]
        public List<RequestLabModel> GetResultLabGroupRequestNumber(long patientVisitUID)
        {
            int complete = 2845;
            int partiallyReviewed = 2864;
            int partiallyCompleted = 2876;
            int reviewed = 2863;
            List<RequestLabModel> dataRequest = (from re in db.Request
                                                 where re.StatusFlag == "A"
                                                 && re.PatientVisitUID == patientVisitUID
                                                 && (re.ORDSTUID == complete || re.ORDSTUID == partiallyReviewed || re.ORDSTUID == partiallyCompleted || re.ORDSTUID == reviewed)
                                                 && re.BSMDDUID == 2813
                                                 select new RequestLabModel
                                                 {
                                                     RequestUID = re.UID,
                                                     LabNumber = re.RequestNumber,
                                                     RequestedDttm = re.RequestedDttm,
                                                     PatientName = SqlFunction.fGetPatientName(re.PatientUID),
                                                     PatientID = SqlFunction.fGetPatientID(re.PatientUID),
                                                     OrderStatus = SqlFunction.fGetRfValDescription(re.ORDSTUID ?? 0)
                                                 }).ToList();

            if (dataRequest != null)
            {
                foreach (var request in dataRequest)
                {
                    request.RequestDetailLabs = (from red in db.RequestDetail
                                                 join rs in db.Result on red.UID equals rs.RequestDetailUID
                                                 where rs.StatusFlag == "A"
                                                 && red.StatusFlag == "A"
                                                 && red.RequestUID == request.RequestUID
                                                 && (red.ORDSTUID == complete || red.ORDSTUID == reviewed)
                                                 select new RequestDetailItemModel
                                                 {
                                                     RequestUID = red.RequestUID,
                                                     RequestDetailUID = red.UID,
                                                     RequestItemCode = red.RequestItemCode,
                                                     RequestItemName = red.RequestItemName,
                                                     ResultUID = rs.UID,
                                                     ResultEnteredDttm = rs.ResultEnteredDttm,
                                                     ResultedEnterBy = SqlFunction.fGetCareProviderName(rs.ResultEnteredUserUID ?? 0),
                                                     OrderStatus = SqlFunction.fGetRfValDescription(red.ORDSTUID)
                                                 }).ToList();

                    if (request.RequestDetailLabs != null)
                    {
                        foreach (var reqeustDetail in request.RequestDetailLabs)
                        {
                            reqeustDetail.ResultComponents = new System.Collections.ObjectModel.ObservableCollection<ResultComponentModel>(
                                db.ResultComponent.Where(p => p.ResultUID == reqeustDetail.ResultUID && p.StatusFlag == "A")
                                .Select(p => new ResultComponentModel
                                {
                                    ResultUID = p.ResultUID,
                                    ReferenceRange = p.ReferenceRange,
                                    ResultItemCode = p.ResultItemCode,
                                    ResultValue = p.ResultValue,
                                    ResultComponentUID = p.UID,
                                    ResultValueType = SqlFunction.fGetRfValDescription(p.RVTYPUID),
                                    RVTYPUID = p.RVTYPUID,
                                    Low = p.Low,
                                    High = p.High,
                                    ResultItemName = p.ResultItemName,
                                    UnitofMeasure = SqlFunction.fGetRfValDescription(p.RSUOMUID ?? 0),
                                    IsAbnormal = p.IsAbnormal
                                }).ToList());
                        }
                        if (request.RequestDetailLabs != null && request.RequestDetailLabs.Count > 0)
                        {
                            request.ReviewedBy = request.RequestDetailLabs.FirstOrDefault().ResultedEnterBy;
                        }
                    }

                }
            }


            return dataRequest;
        }


        [Route("GetResultLabByRequestUID")]
        [HttpGet]
        public List<RequestDetailItemModel> GetResultLabByRequestUID(long requestUID)
        {
            var data = (from re in db.Request
                        join red in db.RequestDetail on re.UID equals red.RequestUID
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
                        where red.StatusFlag == "A"
                        && red.RequestUID == requestUID
                        select new RequestDetailItemModel
                        {
                            RequestDetailUID = red.UID,
                            RequestUID = red.RequestUID,
                            RequestItemName = red.RequestItemName,
                            RequestItemCode = red.RequestItemCode,
                            PatientVisitUID = re.PatientVisitUID,
                            PatientUID = re.PatientUID,
                            OrderStatus = SqlFunction.fGetRfValDescription(red.ORDSTUID),
                            PriorityStatus = SqlFunction.fGetRfValDescription(red.RQPRTUID ?? 0),
                            ResultUID = rs.UID,
                            Comments = red.Comments
                        }).ToList();

            if (data != null)
            {
                foreach (var item in data)
                {
                    var ResultComponents = (from ri in db.ResultItem
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
                                            where red.UID == item.RequestDetailUID
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
                                                ReferenceRange = resultCom.ReferenceRange,
                                                Low = resultCom.Low,
                                                High = resultCom.High,
                                                ResultDTTM = resultCom.ResultDTTM,
                                                RSUOMUID = ri.UnitofMeasure,
                                                UnitofMeasure = SqlFunction.fGetRfValDescription(ri.UnitofMeasure ?? 0),
                                                IsAbnormal = resultCom.IsAbnormal,
                                            }).OrderBy(p => p.PrintOrder).ToList();

                    foreach (var component in ResultComponents)
                    {
                        component.ImageContent = component.ResultValueType == "Image"
                            ? GetResultImageContentByComponentUID(component.ResultComponentUID ?? 0) : null;
                    }
                    item.ResultComponents = new System.Collections.ObjectModel.ObservableCollection<ResultComponentModel>(ResultComponents);
                }
            }

            return data;
        }

        [Route("GetResultLabByRequestNumber")]
        [HttpGet]
        public List<RequestDetailItemModel> GetResultLabByRequestNumber(string requestNumber)
        {
            var data = (from re in db.Request
                        join red in db.RequestDetail on re.UID equals red.RequestUID
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
                        where red.StatusFlag == "A"
                        && re.RequestNumber == requestNumber
                        select new RequestDetailItemModel
                        {
                            RequestDetailUID = red.UID,
                            RequestUID = red.RequestUID,
                            RequestItemName = red.RequestItemName,
                            RequestItemCode = red.RequestItemCode,
                            PatientVisitUID = re.PatientVisitUID,
                            PatientUID = re.PatientUID,
                            OrderStatus = SqlFunction.fGetRfValDescription(red.ORDSTUID),
                            PriorityStatus = SqlFunction.fGetRfValDescription(red.RQPRTUID ?? 0),
                            ResultUID = rs.UID,
                            Comments = red.Comments
                        }).ToList();

            if (data != null)
            {
                foreach (var item in data)
                {
                    var ResultComponents = (from ri in db.ResultItem
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
                                            where red.UID == item.RequestDetailUID
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
                                                ReferenceRange = resultCom.ReferenceRange,
                                                Low = resultCom.Low,
                                                High = resultCom.High,
                                                ResultDTTM = resultCom.ResultDTTM,
                                                RSUOMUID = ri.UnitofMeasure,
                                                UnitofMeasure = SqlFunction.fGetRfValDescription(ri.UnitofMeasure ?? 0),
                                                IsAbnormal = resultCom.IsAbnormal
                                            }).OrderBy(p => p.PrintOrder).ToList();

                    ResultComponents.ForEach(p => p.ImageContent = p.ResultValueType == "Image" ? GetResultImageContentByComponentUID(p.ResultComponentUID ?? 0) : null);
                    item.ResultComponents = new System.Collections.ObjectModel.ObservableCollection<ResultComponentModel>(ResultComponents);
                }
            }

            return data;
        }

        [Route("GetResultItemRangeByLABRAMUID")]
        [HttpGet]
        public List<ResultItemRangeModel> GetResultItemRangeByLABRAMUID(long LABRAMUID)
        {
            List<ResultItemRangeModel> data = db.ResultItemRange.Where(p => p.StatusFlag == "A" && p.LABRAMUID == LABRAMUID)
                .Select(p => new ResultItemRangeModel
                {
                    ResultItemRangeUID = p.UID,
                    ResultItemUID = p.ResultItemUID,
                    SEXXXUID = p.SEXXXUID,
                    DisplayValue = p.DisplayValue,
                    Low = p.Low,
                    High = p.High,
                    Comments = p.Comments,
                    LABRAMUID = p.LABRAMUID
                }).ToList();

            return data;
        }

        [Route("GetResultItemRangeByRequestItemUID")]
        [HttpGet]
        public List<ResultItemRangeModel> GetResultItemRangeByRequestItemUID(int requestItemUID)
        {
            List<ResultItemRangeModel> data = (from ri in db.RequestItem
                                               join rsl in db.RequestResultLink on ri.UID equals rsl.RequestItemUID
                                               join rang in db.ResultItemRange on rsl.ResultItemUID equals rang.ResultItemUID
                                               where ri.UID == requestItemUID
                                               && rsl.StatusFlag == "A"
                                               && rang.StatusFlag == "A"
                                               select new ResultItemRangeModel
                                               {
                                                   ResultItemRangeUID = rang.UID,
                                                   ResultItemUID = rang.ResultItemUID,
                                                   SEXXXUID = rang.SEXXXUID,
                                                   DisplayValue = rang.DisplayValue,
                                                   Low = rang.Low,
                                                   High = rang.High,
                                                   Comments = rang.Comments,
                                                   LABRAMUID = rang.LABRAMUID
                                               }).ToList();

            return data;
        }

        [Route("GetResultImageByComponentUID")]
        [HttpGet]
        public ResultImageModel GetResultImageByComponentUID(long resultComponentUID)
        {
            ResultImageModel returnData = db.ResultImage.Where(p => p.ResultComponentUID == resultComponentUID && p.StatusFlag == "A").Select(p => new ResultImageModel
            {
                ResultComponentUID = p.ResultComponentUID,
                ImageContent = p.ImageContent,
                ResultImageUID = p.UID,
                Comments = p.Comments
            }).FirstOrDefault();

            return returnData;
        }

        [Route("GetResultImageContentByComponentUID")]
        [HttpGet]
        public byte[] GetResultImageContentByComponentUID(long resultComponentUID)
        {
            byte[] imageContent = null;
            ResultImageModel returnData = db.ResultImage.Where(p => p.ResultComponentUID == resultComponentUID && p.StatusFlag == "A")
                .Select(p => new ResultImageModel
                {
                    ResultComponentUID = p.ResultComponentUID,
                    ImageContent = p.ImageContent,
                    ResultImageUID = p.UID,
                    Comments = p.Comments
                }).FirstOrDefault();

            if (returnData != null)
            {
                imageContent = returnData.ImageContent;
            }

            return imageContent;
        }

        [Route("ReviewLabResult")]
        [HttpPost]
        public HttpResponseMessage ReviewLabResult(List<RequestDetailItemModel> labRequestDetails, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                int reviewStatus = 2863;
                int imageType = 2516;
                using (var tran = new TransactionScope())
                {
                    Request request = db.Request.Find(labRequestDetails.FirstOrDefault().RequestUID);
                    //var requestDetails = db.RequestDetail.Where(p => p.RequestUID == request.UID);

                    foreach (var delRequestDetail in labRequestDetails)
                    {
                        Result delResult = db.Result.FirstOrDefault(p => p.RequestDetailUID == delRequestDetail.RequestDetailUID && p.StatusFlag == "A");

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
                    }

                    foreach (var labRequestDetail in labRequestDetails)
                    {
                        RequestDetail requestDetail = db.RequestDetail.Find(labRequestDetail.RequestDetailUID);


                        SEQResult seqResultID = new SEQResult();
                        seqResultID.StatusFlag = "A";

                        db.SEQResult.Add(seqResultID);
                        db.SaveChanges();

                        Result result = new Result();
                        result.CUser = userID;
                        result.CWhen = now;
                        result.StatusFlag = "A";
                        result.ResultNumber = seqResultID.UID.ToString();

                        result.RequestDetailUID = labRequestDetail.RequestDetailUID;
                        result.MUser = userID;
                        result.MWhen = now;
                        result.ResultEnteredDttm = now;
                        result.ResultEnteredUserUID = userID;
                        result.ORDSTUID = reviewStatus;
                        result.PatientUID = labRequestDetail.PatientUID;
                        result.PatientVisitUID = labRequestDetail.PatientVisitUID;
                        result.ResultedByUID = userID;
                        result.RequestItemCode = labRequestDetail.RequestItemCode;
                        result.RequestItemName = labRequestDetail.RequestItemName;
                        db.Result.Add(result);
                        db.SaveChanges();


                        foreach (var components in labRequestDetail.ResultComponents)
                        {
                            ResultComponent resultComponent = new ResultComponent();
                            resultComponent.CUser = userID;
                            resultComponent.CWhen = now;
                            resultComponent.MUser = userID;
                            resultComponent.MWhen = now;
                            resultComponent.StatusFlag = "A";
                            resultComponent.ResultUID = result.UID;
                            resultComponent.ResultItemUID = components.ResultItemUID;
                            resultComponent.RSUOMUID = components.RSUOMUID;
                            resultComponent.RVTYPUID = components.RVTYPUID;
                            resultComponent.ResultValue = components.ResultValue;
                            resultComponent.ResultItemName = components.ResultItemName;
                            resultComponent.ResultItemCode = components.ResultItemCode;



                            //double resultValue;

                            //if (double.TryParse(resultComponent.ResultValue, out resultValue))
                            //{
                            //if (string.IsNullOrEmpty(components.ReferenceRange))
                            //{
                            if (components.High != null && components.Low != null)
                            {
                                components.ReferenceRange = components.Low.ToString() + "-" + components.High.ToString();


                                //if (resultValue > components.High)
                                //{
                                //    isAbnormal = "H";
                                //}

                                //if (resultValue < components.Low)
                                //{
                                //    isAbnormal = "L";
                                //}

                            }
                            else if (components.High == null && components.Low != null)
                            {
                                components.ReferenceRange = ">" + components.Low.ToString();


                                //if (resultValue < components.Low)
                                //{
                                //    isAbnormal = "L";
                                //}
                            }
                            else if (components.Low == null && components.High != null)
                            {
                                components.ReferenceRange = "<" + components.High.ToString();

                            }



                            resultComponent.IsAbnormal = components.IsAbnormal;
                            resultComponent.ReferenceRange = components.ReferenceRange;
                            resultComponent.Low = components.Low;
                            resultComponent.High = components.High;
                            resultComponent.ResultDTTM = components.ResultDTTM ?? now;
                            resultComponent.Comments = components.Comments;
                            db.ResultComponent.Add(resultComponent);
                            db.SaveChanges();

                            if (components.RVTYPUID == imageType)
                            {
                                ResultImage resultImage = new ResultImage();
                                resultImage.ResultComponentUID = resultComponent.UID;
                                resultImage.ImageContent = components.ImageContent;
                                resultImage.CUser = userID;
                                resultImage.CWhen = now;
                                resultImage.MUser = userID;
                                resultImage.MWhen = now;
                                resultImage.StatusFlag = "A";

                                db.ResultImage.Add(resultImage);

                                db.SaveChanges();
                            }
                        }


                        db.RequestDetail.Attach(requestDetail);
                        requestDetail.ORDSTUID = reviewStatus;
                        requestDetail.MUser = userID;
                        requestDetail.MWhen = now;

                        var requestDetialSpecimen = db.RequestDetailSpecimen.FirstOrDefault(p => p.StatusFlag == "A" && p.RequestDetailUID == requestDetail.UID);
                        if (requestDetialSpecimen != null)
                        {
                            db.RequestDetailSpecimen.Attach(requestDetialSpecimen);
                            requestDetialSpecimen.SPSTSUID = reviewStatus;
                            requestDetialSpecimen.MUser = userID;
                            requestDetialSpecimen.MWhen = now;
                        }


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

        [Route("UpdateRequestDetailSpecimens")]
        [HttpPost]
        public HttpResponseMessage UpdateRequestDetailSpecimens(List<RequestDetailSpecimenModel> requestDetailSpecimens, int userID)
        {
            try
            {
                using (var tran = new TransactionScope())
                {
                    DateTime now = DateTime.Now;
                    long? requestUID = null;
                    foreach (var requestDetailSpecimen in requestDetailSpecimens)
                    {
                        RequestDetail requestDetail = db.RequestDetail.Find(requestDetailSpecimen.RequestDetailUID);
                        RequestDetailSpecimen detailSpecimen = db.RequestDetailSpecimen.Find(requestDetailSpecimen.RequestDetailSpecimenUID);
                        if (requestUID == null)
                        {
                            requestUID = requestDetail.RequestUID;
                        }
                        if (detailSpecimen == null)
                        {
                            detailSpecimen = new RequestDetailSpecimen();
                            detailSpecimen.CUser = userID;
                            detailSpecimen.CWhen = now;
                            detailSpecimen.StatusFlag = "A";
                        }

                        detailSpecimen.MUser = userID;
                        detailSpecimen.MWhen = now;
                        detailSpecimen.RequestDetailUID = requestDetailSpecimen.RequestDetailUID;
                        detailSpecimen.SpecimenUID = requestDetailSpecimen.SpecimenUID.Value;
                        detailSpecimen.SpecimenName = requestDetailSpecimen.SpecimenName;
                        detailSpecimen.SPMTPUID = requestDetailSpecimen.SPMTPUID;
                        detailSpecimen.ContainerUID = requestDetailSpecimen.ContainerUID;
                        detailSpecimen.VolumeCollected = requestDetailSpecimen.VolumeCollected;
                        detailSpecimen.VOUNTUID = requestDetailSpecimen.VOUNTUID;
                        detailSpecimen.COLSTUID = requestDetailSpecimen.COLSTUID;
                        detailSpecimen.CLROUUID = requestDetailSpecimen.CLROUUID;
                        detailSpecimen.COLMDUID = requestDetailSpecimen.COLMDUID;
                        detailSpecimen.Comments = requestDetailSpecimen.Comments;
                        detailSpecimen.StorageInstuction = requestDetailSpecimen.StorageInstuction;
                        detailSpecimen.HandlingInstruction = requestDetailSpecimen.HandlingInstruction;
                        detailSpecimen.ReviewedBy = requestDetailSpecimen.ReviewedBy;
                        detailSpecimen.ReviewComments = requestDetailSpecimen.ReviewComments;
                        detailSpecimen.SPSTSUID = requestDetailSpecimen.SPSTSUID;


                        db.RequestDetail.Attach(requestDetail);
                        requestDetail.ORDSTUID = requestDetailSpecimen.SPSTSUID ?? 0;
                        requestDetail.MUser = userID;
                        requestDetail.MWhen = now;

                        if (detailSpecimen.SPSTSUID == 2865)
                        {
                            requestDetail.PreparedDttm = now;
                            requestDetail.PreparedByUID = userID;
                        }
                        else if (detailSpecimen.SPSTSUID == 2862)
                        {
                            detailSpecimen.CollectionDttm = now;
                            detailSpecimen.CollectedBy = userID;
                        }

                        db.RequestDetailSpecimen.AddOrUpdate(detailSpecimen);

                        PatientOrderDetail patientOrderDetail = db.PatientOrderDetail.Find(requestDetail.PatientOrderDetailUID);

                        //PatientOrderDetail
                        if (patientOrderDetail != null)
                        {
                            db.PatientOrderDetail.Attach(patientOrderDetail);
                            patientOrderDetail.ORDSTUID = requestDetail.ORDSTUID;
                            patientOrderDetail.MUser = userID;
                            patientOrderDetail.MWhen = DateTime.Now;


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

                        }

                        db.SaveChanges();

                    }

                    Request request = db.Request.Find(requestUID);
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
    }
}