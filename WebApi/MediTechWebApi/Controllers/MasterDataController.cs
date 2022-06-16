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
using System.Data.Entity;

namespace MediTechWebApi.Controllers
{
    [RoutePrefix("Api/MasterData")]
    public class MasterDataController : ApiController
    {
        protected MediTechEntities db = new MediTechEntities();

        #region RequestItem

        [Route("GetRequestItems")]
        [HttpGet]
        public List<RequestItemModel> GetRequestItems()
        {
            List<RequestItemModel> data = db.RequestItem.Where(p => p.StatusFlag == "A").Select(p => new RequestItemModel()
            {
                RequestItemUID = p.UID,
                Code = p.Code,
                Description = p.Description,
                EffectiveFrom = p.EffectiveFrom,
                EffectiveTo = p.EffectiveTo,
                ItemName = p.ItemName,
                TSTTPUID = p.TSTTPUID,
                TestType = SqlFunction.fGetRfValDescription(p.TSTTPUID ?? 0),
                RIMTYPUID = p.RIMTYPUID,
                PRTGPUID = p.PRTGPUID,
                CUser = p.CUser,
                CWhen = p.CWhen,
                MUser = p.MUser,
                MWhen = p.MWhen,
                StatusFlag = p.StatusFlag
            }).ToList();

            return data;
        }

        [Route("GetRequestItemByCategory")]
        [HttpGet]
        public List<RequestItemModel> GetRequestItemByCategory(string category, bool queryResultLink = false)
        {
            DateTime now = DateTime.Now;
            int TSTTPUID = db.ReferenceValue.FirstOrDefault(p => p.DomainCode == "TSTTP" && p.Description.ToLower() == category.ToLower() && p.StatusFlag == "A").UID;
            List<RequestItemModel> data = db.RequestItem.Where(p =>
            p.TSTTPUID == TSTTPUID
            && p.StatusFlag == "A"
            && (p.EffectiveFrom == null || DbFunctions.TruncateTime(p.EffectiveFrom) <= DbFunctions.TruncateTime(now))
            && (p.EffectiveTo == null || DbFunctions.TruncateTime(p.EffectiveTo) >= DbFunctions.TruncateTime(now)))
            .Select(p => new RequestItemModel()
            {
                RequestItemUID = p.UID,
                Code = p.Code,
                Description = p.Description,
                EffectiveFrom = p.EffectiveFrom,
                EffectiveTo = p.EffectiveTo,
                ItemName = p.ItemName,
                TSTTPUID = p.TSTTPUID,
                PRTGPUID = p.PRTGPUID,
                CUser = p.CUser,
                CWhen = p.CWhen,
                MUser = p.MUser,
                MWhen = p.MWhen,
                StatusFlag = p.StatusFlag
            }).ToList();

            if (queryResultLink)
            {
                if (data != null)
                {
                    foreach (var requestItem in data)
                    {
                        requestItem.RequestResultLinks = GetRequestResultLinkByRequestItemUID(requestItem.RequestItemUID);
                    }
                }
            }

            return data;
        }

        [Route("GetRequestItemByUID")]
        [HttpGet]
        public RequestItemModel GetRequestItemByUID(int requestItemUID)
        {
            RequestItemModel data = (from re in db.RequestItem
                                     where re.StatusFlag == "A"
                                     && re.UID == requestItemUID
                                     select new RequestItemModel
                                     {
                                         Code = re.Code,
                                         CUser = re.CUser,
                                         CWhen = re.CWhen,
                                         MUser = re.MUser,
                                         MWhen = re.MWhen,
                                         StatusFlag = re.StatusFlag,
                                         RequestItemUID = re.UID,
                                         Description = re.Description,
                                         PRTGPUID = re.PRTGPUID,
                                         TSTTPUID = re.TSTTPUID,
                                         RIMTYPUID = re.RIMTYPUID,
                                         EffectiveFrom = re.EffectiveFrom,
                                         TestType = SqlFunction.fGetRfValDescription(re.TSTTPUID ?? 0),
                                         EffectiveTo = re.EffectiveTo,
                                         ItemName = re.ItemName
                                     }).FirstOrDefault();

            if (data != null)
            {
                data.RequestResultLinks = GetRequestResultLinkByRequestItemUID(data.RequestItemUID);

                data.RequestItemSpecimens = (from rsp in db.RequestItemSpecimen
                                             join spc in db.Specimen on rsp.SpecimenUID equals spc.UID
                                             where rsp.StatusFlag == "A"
                                             && rsp.RequestItemUID == data.RequestItemUID
                                             select new RequestItemSpecimenModel
                                             {
                                                 RequestItemSpecimenUID = rsp.UID,
                                                 RequestItemUID = rsp.RequestItemUID,
                                                 SpecimenUID = rsp.SpecimenUID,
                                                 SpecimenName = rsp.SpecimenName,
                                                 IsDefault = rsp.IsDefault,
                                                 VolumeCollected = spc.VolumeCollected,
                                                 CollectionRoute = SqlFunction.fGetRfValDescription(spc.CLROUUID ?? 0),
                                                 CollectionMethod = SqlFunction.fGetRfValDescription(spc.COLMDUID ?? 0),
                                                 CollectionSite = SqlFunction.fGetRfValDescription(spc.COLSTUID ?? 0),
                                                 SpecimenType = SqlFunction.fGetRfValDescription(spc.SPMTPUID ?? 0)
                                             }).ToList();

                data.RequestItemGroupResults = (from gps in db.RequestItemGroupResult
                                                join rf in db.ReferenceValue on gps.GPRSTUID equals rf.UID
                                                where gps.StatusFlag == "A" && gps.RequestItemUID == data.RequestItemUID
                                                select new RequestItemGroupResultModel
                                                {
                                                    RequestItemGroupResultUID = gps.UID,
                                                    RequestItemUID = gps.RequestItemUID,
                                                    GroupResultName = rf.Description,
                                                    GPRSTUID = gps.GPRSTUID,
                                                    PrintOrder = gps.PrintOrder
                                                }).ToList();
            }

            return data;
        }

        [Route("GetRequestItemByCode")]
        [HttpGet]
        public RequestItemModel GetRequestItemByCode(string code)
        {
            RequestItemModel data = (from re in db.RequestItem
                                     where re.StatusFlag == "A"
                                     && re.Code.ToLower() == code.ToLower()
                                     select new RequestItemModel
                                     {
                                         Code = re.Code,
                                         CUser = re.CUser,
                                         CWhen = re.CWhen,
                                         MUser = re.MUser,
                                         MWhen = re.MWhen,
                                         StatusFlag = re.StatusFlag,
                                         RequestItemUID = re.UID,
                                         Description = re.Description,
                                         PRTGPUID = re.PRTGPUID,
                                         TSTTPUID = re.TSTTPUID,
                                         RIMTYPUID = re.RIMTYPUID,
                                         EffectiveFrom = re.EffectiveFrom,
                                         EffectiveTo = re.EffectiveTo,
                                         ItemName = re.ItemName
                                     }).FirstOrDefault();

            return data;
        }

        [Route("GetRequestResultLinkByRequestItemUID")]
        [HttpGet]
        public List<RequestResultLinkModel> GetRequestResultLinkByRequestItemUID(int requestItemUID)
        {
            List<RequestResultLinkModel> data = (from rlk in db.RequestResultLink
                                                 join rei in db.ResultItem on rlk.ResultItemUID equals rei.UID
                                                 where rlk.StatusFlag == "A"
                                                 && rlk.RequestItemUID == requestItemUID
                                                 select new RequestResultLinkModel
                                                 {
                                                     ResultItemUID = rlk.ResultItemUID,
                                                     Unit = SqlFunction.fGetRfValDescription(rei.UnitofMeasure ?? 0),
                                                     PrintOrder = rlk.PrintOrder,
                                                     IsMandatory = rlk.IsMandatory,
                                                     RequestItemUID = rlk.RequestItemUID,
                                                     ResultItemName = rei.DisplyName,
                                                     ResultItemCode = rei.Code,
                                                     ResultValueType = SqlFunction.fGetRfValDescription(rei.RVTYPUID ?? 0),
                                                     RVTYPUID = rei.RVTYPUID,
                                                     RSUOMUID = rei.UnitofMeasure,
                                                     RequestResultLinkUID = rlk.UID,
                                                     ExcludeFrmPrint = rlk.ExcludeFrmPrint
                                                 }).ToList();

            return data;
        }

        [Route("ManageRequestItem")]
        [HttpPost]
        public HttpResponseMessage ManageRequestItem(RequestItemModel requestItemModel, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                using (var tran = new TransactionScope())
                {
                    MediTech.DataBase.RequestItem requestItem = db.RequestItem.Find(requestItemModel.RequestItemUID);

                    if (requestItem == null)
                    {
                        requestItem = new MediTech.DataBase.RequestItem();
                        requestItem.CUser = userID;
                        requestItem.CWhen = now;
                    }

                    requestItem.Code = requestItemModel.Code;
                    requestItem.ItemName = requestItemModel.ItemName;
                    requestItem.Description = requestItemModel.Description;
                    requestItem.EffectiveFrom = requestItemModel.EffectiveFrom;
                    requestItem.EffectiveTo = requestItemModel.EffectiveTo;
                    if (requestItemModel.TSTTPUID != null && requestItemModel.TSTTPUID != 0)
                    {
                        requestItem.TSTTPUID = requestItemModel.TSTTPUID;
                    }
                    else
                    {
                        requestItem.TSTTPUID = db.ReferenceValue.FirstOrDefault(p => p.StatusFlag == "A" && p.DomainCode == "TSTTP" && p.ValueCode == "RADTP").UID;

                    }
                    requestItem.PRTGPUID = requestItemModel.PRTGPUID;
                    requestItem.RIMTYPUID = requestItemModel.RIMTYPUID;
                    requestItem.MUser = userID;
                    requestItem.MWhen = now;
                    requestItem.StatusFlag = "A";

                    db.RequestItem.AddOrUpdate(requestItem);
                    db.SaveChanges();

                    BillableItem billItm = db.BillableItem.Where(p => p.ItemUID == requestItem.UID && (p.BSMDDUID == 2813 || p.BSMDDUID == 2841)).FirstOrDefault();
                    if (billItm != null)
                    {
                        if (requestItem.Code != billItm.Code)
                        {
                            db.BillableItem.Attach(billItm);
                            billItm.Code = requestItem.Code;
                            billItm.MUser = userID;
                            billItm.MWhen = now;
                            billItm.StatusFlag = "A";
                            db.SaveChanges();
                        }

                    }

                    #region Delete RequestResultLink
                    IEnumerable<RequestResultLink> requestResultLink1 = db.RequestResultLink.Where(p => p.StatusFlag == "A" && p.RequestItemUID == requestItem.UID);

                    if (requestItemModel.RequestResultLinks == null)
                    {
                        foreach (var item in requestResultLink1)
                        {
                            db.RequestResultLink.Attach(item);
                            item.MUser = userID;
                            item.MWhen = now;
                            item.StatusFlag = "D";
                        }
                    }
                    else
                    {
                        foreach (var item in requestResultLink1)
                        {
                            var data = requestItemModel.RequestResultLinks.FirstOrDefault(p => p.RequestResultLinkUID == item.UID);
                            if (data == null)
                            {
                                db.RequestResultLink.Attach(item);
                                item.MUser = userID;
                                item.MWhen = now;
                                item.StatusFlag = "D";
                            }

                        }
                    }

                    db.SaveChanges();

                    #endregion

                    if (requestItemModel.RequestResultLinks != null)
                    {
                        foreach (var item in requestItemModel.RequestResultLinks)
                        {
                            RequestResultLink requestResultLink = db.RequestResultLink.Find(item.RequestResultLinkUID);
                            if (requestResultLink == null)
                            {
                                requestResultLink = new RequestResultLink();
                                requestResultLink.CUser = userID;
                                requestResultLink.CWhen = now;
                                requestResultLink.MUser = userID;
                                requestResultLink.MWhen = now;
                                requestResultLink.StatusFlag = "A";
                            }
                            else
                            {
                                if (item.MWhen != DateTime.MinValue)
                                {
                                    requestResultLink.MUser = userID;
                                    requestResultLink.MWhen = now;
                                }
                            }

                            requestResultLink.RequestItemUID = requestItem.UID;
                            requestResultLink.ResultItemUID = item.ResultItemUID;
                            requestResultLink.ResultItemName = item.ResultItemName;
                            requestResultLink.PrintOrder = item.PrintOrder;
                            requestResultLink.IsMandatory = item.IsMandatory;

                            db.RequestResultLink.AddOrUpdate(requestResultLink);
                            db.SaveChanges();
                        }
                    }


                    #region Delete RequestItemSpecimens
                    IEnumerable<RequestItemSpecimen> requestItemSpecimens1 = db.RequestItemSpecimen.Where(p => p.StatusFlag == "A" && p.RequestItemUID == requestItem.UID);

                    if (requestItemModel.RequestItemSpecimens == null)
                    {
                        foreach (var item in requestItemSpecimens1)
                        {
                            db.RequestItemSpecimen.Attach(item);
                            item.MUser = userID;
                            item.MWhen = now;
                            item.StatusFlag = "D";
                        }
                    }
                    else
                    {
                        foreach (var item in requestItemSpecimens1)
                        {
                            var data = requestItemModel.RequestItemSpecimens.FirstOrDefault(p => p.RequestItemSpecimenUID == item.UID);
                            if (data == null)
                            {
                                db.RequestItemSpecimen.Attach(item);
                                item.MUser = userID;
                                item.MWhen = now;
                                item.StatusFlag = "D";
                            }
                        }
                    }

                    db.SaveChanges();

                    #endregion

                    if (requestItemModel.RequestItemSpecimens != null)
                    {
                        foreach (var item in requestItemModel.RequestItemSpecimens)
                        {
                            RequestItemSpecimen requestItemSpecimen = db.RequestItemSpecimen.Find(item.RequestItemSpecimenUID);
                            if (requestItemSpecimen == null)
                            {
                                requestItemSpecimen = new RequestItemSpecimen();
                                requestItemSpecimen.CUser = userID;
                                requestItemSpecimen.CWhen = now;
                                requestItemSpecimen.MUser = userID;
                                requestItemSpecimen.MWhen = now;
                                requestItemSpecimen.StatusFlag = "A";
                            }
                            else
                            {
                                if (item.MWhen != DateTime.MinValue)
                                {
                                    requestItemSpecimen.MUser = userID;
                                    requestItemSpecimen.MWhen = now;
                                }
                            }

                            requestItemSpecimen.CollectionInterval = "";
                            requestItemSpecimen.IsDefault = item.IsDefault;
                            requestItemSpecimen.IsDFT = "";
                            requestItemSpecimen.RequestItemUID = requestItem.UID;
                            requestItemSpecimen.SpecimenName = item.SpecimenName;
                            requestItemSpecimen.SpecimenUID = item.SpecimenUID;
                            db.RequestItemSpecimen.AddOrUpdate(requestItemSpecimen);
                            db.SaveChanges();
                        }
                    }

                    #region Delete RequestItemGroupResult
                    IEnumerable<RequestItemGroupResult> requestItemGroupResult1 = db.RequestItemGroupResult.Where(p => p.StatusFlag == "A" && p.RequestItemUID == requestItem.UID);
                    if (requestItemModel.RequestItemGroupResults == null)
                    {
                        foreach (var item in requestItemGroupResult1)
                        {
                            db.RequestItemGroupResult.Attach(item);
                            item.MUser = userID;
                            item.MWhen = now;
                            item.StatusFlag = "D";
                        }
                    }
                    else
                    {
                        foreach (var item in requestItemGroupResult1)
                        {
                            var data = requestItemModel.RequestItemGroupResults.FirstOrDefault(p => p.RequestItemGroupResultUID == item.UID);
                            if (data == null)
                            {
                                db.RequestItemGroupResult.Attach(item);
                                item.MUser = userID;
                                item.MWhen = now;
                                item.StatusFlag = "D";
                            }
                        }
                    }
                    db.SaveChanges();
                    #endregion

                    if (requestItemModel.RequestItemGroupResults != null)
                    {
                        foreach (var item in requestItemModel.RequestItemGroupResults)
                        {
                            RequestItemGroupResult requestItemGroupResult = db.RequestItemGroupResult.Find(item.RequestItemGroupResultUID);
                            if (requestItemGroupResult == null)
                            {
                                requestItemGroupResult = new RequestItemGroupResult();
                                requestItemGroupResult.CUser = userID;
                                requestItemGroupResult.CWhen = now;
                                requestItemGroupResult.MUser = userID;
                                requestItemGroupResult.MWhen = now;
                                requestItemGroupResult.StatusFlag = "A";
                            }
                            else
                            {
                                if (item.MWhen != DateTime.MinValue)
                                {
                                    requestItemGroupResult.MUser = userID;
                                    requestItemGroupResult.MWhen = now;
                                }
                            }
                            requestItemGroupResult.RequestItemUID = requestItem.UID;
                            requestItemGroupResult.GPRSTUID = item.GPRSTUID;
                            requestItemGroupResult.PrintOrder = item.PrintOrder;
                            db.RequestItemGroupResult.AddOrUpdate(requestItemGroupResult);
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

        [Route("DeleteRequestItem")]
        [HttpDelete]
        public HttpResponseMessage DeleteRequestItem(int requestItemUID, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                MediTech.DataBase.RequestItem requestItem = db.RequestItem.Find(requestItemUID);
                if (requestItem != null)
                {
                    db.RequestItem.Attach(requestItem);
                    requestItem.MUser = userID;
                    requestItem.MWhen = now;
                    requestItem.StatusFlag = "D";
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

        #region ResultItem
        [Route("SearchResultItem")]
        [HttpGet]
        public List<ResultItemModel> SearchResultItem(string name, int? RVTYPUID)
        {
            List<ResultItemModel> resultItemData = db.ResultItem
                .Where(p => p.StatusFlag == "A"
                && (string.IsNullOrEmpty(name)
                || p.Code == name
                || p.DisplyName.Contains(name))
                && ((RVTYPUID ?? 0) == 0 || p.RVTYPUID == RVTYPUID)).Select(p => new ResultItemModel
                {
                    ResultItemUID = p.UID,
                    DisplyName = p.DisplyName,
                    Description = p.Description,
                    Code = p.Code,
                    EffectiveFrom = p.EffectiveFrom,
                    EffectiveTo = p.EffectiveTo,
                    UnitofMeasure = p.UnitofMeasure,
                    UOM = SqlFunction.fGetRfValDescription(p.UnitofMeasure ?? 0),
                    RVTYPUID = p.RVTYPUID,
                    ResultType = SqlFunction.fGetRfValDescription(p.RVTYPUID ?? 0),
                    IsCumulative = p.IsCumulative,
                    StatusFlag = p.StatusFlag,
                    CUser = p.CUser,
                    CWhen = p.CWhen,
                    MUser = p.MUser,
                    MWhen = p.MWhen
                }).ToList();

            return resultItemData;
        }


        [Route("GetResultItems")]
        [HttpGet]
        public List<ResultItemModel> GetResultItems()
        {
            List<ResultItemModel> resultItemData = db.ResultItem
            .Where(p => p.StatusFlag == "A").Select(p => new ResultItemModel
            {
                ResultItemUID = p.UID,
                DisplyName = p.DisplyName,
                Description = p.Description,
                Code = p.Code,
                EffectiveFrom = p.EffectiveFrom,
                EffectiveTo = p.EffectiveTo,
                UnitofMeasure = p.UnitofMeasure,
                UOM = SqlFunction.fGetRfValDescription(p.UnitofMeasure ?? 0),
                RVTYPUID = p.RVTYPUID,
                ResultType = SqlFunction.fGetRfValDescription(p.RVTYPUID ?? 0),
                IsCumulative = p.IsCumulative,
                AutoValue = p.AutoValue,
                StatusFlag = p.StatusFlag,
                CUser = p.CUser,
                CWhen = p.CWhen,
                MUser = p.MUser,
                MWhen = p.MWhen
            }).ToList();

            return resultItemData;
        }

        [Route("GetResultItemUID")]
        [HttpGet]
        public ResultItemModel GetResultItemUID(int resultItemUID)
        {
            ResultItemModel resultItemData = db.ResultItem
            .Where(p => p.UID == resultItemUID).Select(p => new ResultItemModel
            {
                ResultItemUID = p.UID,
                DisplyName = p.DisplyName,
                Description = p.Description,
                Code = p.Code,
                EffectiveFrom = p.EffectiveFrom,
                EffectiveTo = p.EffectiveTo,
                UnitofMeasure = p.UnitofMeasure,
                UOM = SqlFunction.fGetRfValDescription(p.UnitofMeasure ?? 0),
                RVTYPUID = p.RVTYPUID,
                GPRSTUID = p.GPRSTUID,
                ResultType = SqlFunction.fGetRfValDescription(p.RVTYPUID ?? 0),
                IsCumulative = p.IsCumulative,
                AutoValue = p.AutoValue,
                StatusFlag = p.StatusFlag,
                CUser = p.CUser,
                CWhen = p.CWhen,
                MUser = p.MUser,
                MWhen = p.MWhen
            }).FirstOrDefault();

            if (resultItemData != null)
            {
                resultItemData.ResultItemRanges = db.ResultItemRange.Where(p => p.StatusFlag == "A" && p.ResultItemUID == resultItemData.ResultItemUID)
                    .Select(p => new MediTech.Model.ResultItemRangeModel
                    {
                        ResultItemUID = p.ResultItemUID,
                        ResultItemRangeUID = p.UID,
                        Comments = p.Comments,
                        DisplayValue = p.DisplayValue,
                        SEXXXUID = p.SEXXXUID,
                        Gender = SqlFunction.fGetRfValDescription(p.SEXXXUID ?? 0),
                        LABRAMUID = p.LABRAMUID,
                        LabRangeMaster = SqlFunction.fGetRfValDescription(p.LABRAMUID),
                        Low = p.Low,
                        High = p.High
                    }).ToList();

            }

            return resultItemData;
        }

        [Route("GetResultItemByCode")]
        [HttpGet]
        public ResultItemModel GetResultItemByCode(string code)
        {
            ResultItemModel resultItemData = db.ResultItem
            .Where(p => p.StatusFlag == "A"
            && p.Code.ToLower() == code.ToLower()).Select(p => new ResultItemModel
            {
                ResultItemUID = p.UID,
                DisplyName = p.DisplyName,
                Description = p.Description,
                Code = p.Code,
                EffectiveFrom = p.EffectiveFrom,
                EffectiveTo = p.EffectiveTo,
                UnitofMeasure = p.UnitofMeasure,
                UOM = SqlFunction.fGetRfValDescription(p.UnitofMeasure ?? 0),
                RVTYPUID = p.RVTYPUID,
                ResultType = SqlFunction.fGetRfValDescription(p.RVTYPUID ?? 0),
                IsCumulative = p.IsCumulative,
                AutoValue = p.AutoValue,
                StatusFlag = p.StatusFlag,
                CUser = p.CUser,
                CWhen = p.CWhen,
                MUser = p.MUser,
                MWhen = p.MWhen
            }).FirstOrDefault();

            return resultItemData;
        }


        [Route("ManageResultItem")]
        [HttpPost]
        public HttpResponseMessage ManageResultItem(ResultItemModel resultItemModel, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                MediTech.DataBase.ResultItem resultItem = db.ResultItem.Find(resultItemModel.ResultItemUID);
                int NUMRC = 2514;
                int FTF = 2524;
                using (var tran = new TransactionScope())
                {
                    if (resultItem == null)
                    {
                        resultItem = new ResultItem();
                        resultItem.StatusFlag = "A";
                        resultItem.CUser = userID;
                        resultItem.CWhen = now;
                    }

                    resultItem.Code = resultItemModel.Code;
                    resultItem.DisplyName = resultItemModel.DisplyName;
                    resultItem.Description = resultItemModel.Description;
                    resultItem.EffectiveFrom = resultItemModel.EffectiveFrom;
                    resultItem.EffectiveTo = resultItemModel.EffectiveTo;
                    resultItem.RVTYPUID = resultItemModel.RVTYPUID;
                    resultItem.GPRSTUID = resultItemModel.GPRSTUID;
                    resultItem.UnitofMeasure = resultItemModel.UnitofMeasure;
                    resultItem.IsCumulative = resultItemModel.IsCumulative;
                    resultItem.AutoValue = resultItemModel.AutoValue;
                    resultItem.MUser = userID;
                    resultItem.MWhen = now;

                    db.ResultItem.AddOrUpdate(resultItem);
                    db.SaveChanges();

                    if (resultItem.RVTYPUID == NUMRC || resultItem.RVTYPUID == FTF)
                    {
                        #region Delete ResultItemRange
                        IEnumerable<ResultItemRange> resultItemRanges = db.ResultItemRange.Where(p => p.StatusFlag == "A" && p.ResultItemUID == resultItem.UID);

                        if (resultItemModel.ResultItemRanges == null)
                        {
                            foreach (var item in resultItemRanges)
                            {
                                db.ResultItemRange.Attach(item);
                                item.MUser = userID;
                                item.MWhen = now;
                                item.StatusFlag = "D";
                            }
                        }
                        else
                        {
                            foreach (var item in resultItemRanges)
                            {
                                var data = resultItemModel.ResultItemRanges.FirstOrDefault(p => p.ResultItemRangeUID == item.UID);
                                if (data == null)
                                {
                                    db.ResultItemRange.Attach(item);
                                    item.MUser = userID;
                                    item.MWhen = now;
                                    item.StatusFlag = "D";
                                }

                            }
                        }

                        db.SaveChanges();

                        #endregion

                        if (resultItemModel.ResultItemRanges != null)
                        {
                            foreach (var item in resultItemModel.ResultItemRanges)
                            {
                                ResultItemRange resultItemRan = db.ResultItemRange.Find(item.ResultItemRangeUID);
                                if (resultItemRan == null)
                                {
                                    resultItemRan = new ResultItemRange();
                                    resultItemRan.CUser = userID;
                                    resultItemRan.CWhen = now;
                                    resultItemRan.MUser = userID;
                                    resultItemRan.MWhen = now;
                                    resultItemRan.StatusFlag = "A";
                                }
                                else
                                {
                                    if (item.MWhen != DateTime.MinValue)
                                    {
                                        resultItemRan.MUser = userID;
                                        resultItemRan.MWhen = now;
                                    }
                                }

                                resultItemRan.Comments = item.Comments;
                                resultItemRan.DisplayValue = item.DisplayValue;
                                resultItemRan.High = item.High;
                                resultItemRan.Low = item.Low;
                                resultItemRan.LABRAMUID = item.LABRAMUID;
                                resultItemRan.ResultItemUID = resultItem.UID;
                                resultItemRan.SEXXXUID = item.SEXXXUID;

                                db.ResultItemRange.AddOrUpdate(resultItemRan);
                                db.SaveChanges();
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

        [Route("DeleteResultItem")]
        [HttpDelete]
        public HttpResponseMessage DeleteResultItem(int resultItemUID, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                MediTech.DataBase.ResultItem resultItem = db.ResultItem.Find(resultItemUID);
                if (resultItem != null)
                {
                    db.ResultItem.Attach(resultItem);
                    resultItem.MUser = userID;
                    resultItem.MWhen = now;
                    resultItem.StatusFlag = "D";
                    db.SaveChanges();
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("GenerateResultItemCode")]
        [HttpGet]
        public string GenerateResultItemCode()
        {
            string ItemCode = string.Empty;
            int seqUID;
            ItemCode = SEQHelper.GetSEQIDFormat("SEQTestCodeID", out seqUID);
            return ItemCode;
        }
        #endregion

        #region Specimen
        [Route("SearhcSpecimen")]
        [HttpGet]
        public List<SpecimenModel> SearhcSpecimen(string specimentext)
        {
            List<SpecimenModel> specimenData = db.Specimen
            .Where(p => p.StatusFlag == "A"
            && (string.IsNullOrEmpty(specimentext)
            || p.Code == specimentext
            || p.Name.ToLower().Contains(specimentext.ToLower()))).Select(p => new SpecimenModel
            {
                CLROUUID = p.CLROUUID,
                CollectionRoute = SqlFunction.fGetRfValDescription(p.CLROUUID ?? 0),
                Code = p.Code,
                COLMDUID = p.COLMDUID,
                CollectionMethod = SqlFunction.fGetRfValDescription(p.COLMDUID ?? 0),
                COLSTUID = p.COLSTUID,
                CollectionSite = SqlFunction.fGetRfValDescription(p.COLSTUID ?? 0),
                ExpiryDttm = p.ExpiryDttm,
                HandlingInstruction = p.HandlingInstruction,
                StorageInstuction = p.StorageInstuction,
                Name = p.Name,
                SPMTPUID = p.SPMTPUID,
                SpecimenType = SqlFunction.fGetRfValDescription(p.SPMTPUID ?? 0),
                IsVolumeCollectionReqd = p.IsVolumeCollectionReqd,
                SpecimenUID = p.UID,
                VolumeCollected = p.VolumeCollected,
                VolumnUnit = SqlFunction.fGetRfValDescription(p.VOUNTUID ?? 0),
                VOUNTUID = p.VOUNTUID,
                StatusFlag = p.StatusFlag,
                CUser = p.CUser,
                CWhen = p.CWhen,
                MUser = p.MUser,
                MWhen = p.MWhen
            }).ToList();
            return specimenData;
        }

        [Route("GetSpecimenByUID")]
        [HttpGet]
        public SpecimenModel GetSpecimenByUID(int specimenUID)
        {
            SpecimenModel specimenData = db.Specimen
            .Where(p => p.StatusFlag == "A"
            && p.UID == specimenUID).Select(p => new SpecimenModel
            {
                CLROUUID = p.CLROUUID,
                CollectionRoute = SqlFunction.fGetRfValDescription(p.CLROUUID ?? 0),
                Code = p.Code,
                COLMDUID = p.COLMDUID,
                CollectionMethod = SqlFunction.fGetRfValDescription(p.COLMDUID ?? 0),
                COLSTUID = p.COLSTUID,
                CollectionSite = SqlFunction.fGetRfValDescription(p.COLSTUID ?? 0),
                ExpiryDttm = p.ExpiryDttm,
                HandlingInstruction = p.HandlingInstruction,
                StorageInstuction = p.StorageInstuction,
                Name = p.Name,
                SPMTPUID = p.SPMTPUID,
                SpecimenType = SqlFunction.fGetRfValDescription(p.SPMTPUID ?? 0),
                IsVolumeCollectionReqd = p.IsVolumeCollectionReqd,
                SpecimenUID = p.UID,
                VolumeCollected = p.VolumeCollected,
                VolumnUnit = SqlFunction.fGetRfValDescription(p.VOUNTUID ?? 0),
                VOUNTUID = p.VOUNTUID,
                StatusFlag = p.StatusFlag,
                CUser = p.CUser,
                CWhen = p.CWhen,
                MUser = p.MUser,
                MWhen = p.MWhen
            }).FirstOrDefault();
            return specimenData;
        }

        [Route("GetSpecimenByCode")]
        [HttpGet]
        public SpecimenModel GetSpecimenByCode(string code)
        {
            SpecimenModel specimenData = db.Specimen
            .Where(p => p.StatusFlag == "A"
            && p.Code.ToLower() == code.ToLower()).Select(p => new SpecimenModel
            {
                CLROUUID = p.CLROUUID,
                CollectionRoute = SqlFunction.fGetRfValDescription(p.CLROUUID ?? 0),
                Code = p.Code,
                COLMDUID = p.COLMDUID,
                CollectionMethod = SqlFunction.fGetRfValDescription(p.COLMDUID ?? 0),
                COLSTUID = p.COLSTUID,
                CollectionSite = SqlFunction.fGetRfValDescription(p.COLSTUID ?? 0),
                ExpiryDttm = p.ExpiryDttm,
                HandlingInstruction = p.HandlingInstruction,
                StorageInstuction = p.StorageInstuction,
                Name = p.Name,
                SPMTPUID = p.SPMTPUID,
                SpecimenType = SqlFunction.fGetRfValDescription(p.SPMTPUID ?? 0),
                IsVolumeCollectionReqd = p.IsVolumeCollectionReqd,
                SpecimenUID = p.UID,
                VolumeCollected = p.VolumeCollected,
                VolumnUnit = SqlFunction.fGetRfValDescription(p.VOUNTUID ?? 0),
                VOUNTUID = p.VOUNTUID,
                StatusFlag = p.StatusFlag,
                CUser = p.CUser,
                CWhen = p.CWhen,
                MUser = p.MUser,
                MWhen = p.MWhen
            }).FirstOrDefault();
            return specimenData;
        }

        [Route("ManageSpecimen")]
        [HttpPost]
        public HttpResponseMessage ManageSpecimen(SpecimenModel specimenModel, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                MediTech.DataBase.Specimen specimen = db.Specimen.Find(specimenModel.SpecimenUID);
                if (specimen == null)
                {
                    specimen = new Specimen();
                    specimen.StatusFlag = "A";
                    specimen.CUser = userID;
                    specimen.CWhen = now;
                }

                specimen.CLROUUID = specimenModel.CLROUUID;
                specimen.Code = specimenModel.Code;
                specimen.COLMDUID = specimenModel.COLMDUID;
                specimen.COLSTUID = specimenModel.COLSTUID;
                specimen.ExpiryDttm = specimenModel.ExpiryDttm;
                specimen.HandlingInstruction = specimenModel.HandlingInstruction;
                specimen.StorageInstuction = specimenModel.StorageInstuction;
                specimen.Name = specimenModel.Name;
                specimen.SPMTPUID = specimenModel.SPMTPUID;
                specimen.IsVolumeCollectionReqd = specimenModel.IsVolumeCollectionReqd;
                specimen.VolumeCollected = specimenModel.VolumeCollected;
                specimen.VOUNTUID = specimenModel.VOUNTUID;
                specimen.MUser = userID;
                specimen.MWhen = now;

                db.Specimen.AddOrUpdate(specimen);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("DeleteSpecimen")]
        [HttpDelete]
        public HttpResponseMessage DeleteSpecimen(int specimenUID, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                MediTech.DataBase.Specimen specimen = db.Specimen.Find(specimenUID);
                if (specimen != null)
                {
                    db.Specimen.Attach(specimen);
                    specimen.MUser = userID;
                    specimen.MWhen = now;
                    specimen.StatusFlag = "D";
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

        #region BillableItem

        [Route("SearchBillableItem")]
        [HttpGet]
        public List<BillableItemModel> SearchBillableItem(string code, string itemName, int? BSMDDUID, int? billingGroupUID, int? billingSubGroupUID, int? ownerUID)
        {
            List<BillableItemModel> data = (from p in db.BillableItem
                                            where p.StatusFlag == "A"
                                            && (string.IsNullOrEmpty(code) || p.Code.ToLower().Contains(code.ToLower()))
                                            && (string.IsNullOrEmpty(itemName) || p.ItemNameSearch.ToLower().Contains(itemName.ToLower()))
                                            && (string.IsNullOrEmpty(itemName) || p.ItemName.ToLower().Contains(itemName.ToLower()))
                                            && (BSMDDUID == null || p.BSMDDUID == BSMDDUID)
                                            && (billingGroupUID == null || p.BillingGroupUID == billingGroupUID)
                                            && (billingSubGroupUID == null || p.BillingSubGroupUID == billingSubGroupUID)
                                            select new BillableItemModel
                                            {
                                                BillableItemUID = p.UID,
                                                Code = p.Code,
                                                Description = p.Description,
                                                ItemName = p.ItemName,
                                                DoctorFee = p.DoctorFee,
                                                Cost = (p.BSMDDUID == 2826 || p.BSMDDUID == 2953) ? SqlFunction.fGetItemAverageCost(p.ItemUID ?? 0, ownerUID ?? 0)
                                                : SqlFunction.fGetBillableItemCost(p.UID, ownerUID ?? 0),
                                                BSMDDUID = p.BSMDDUID,
                                                Price = SqlFunction.fGetBillableItemPrice(p.UID, ownerUID ?? 0),
                                                BillingServiceMetaData = SqlFunction.fGetRfValDescription(p.BSMDDUID),
                                                BillingGroupUID = p.BillingGroupUID,
                                                BillingSubGroupUID = p.BillingSubGroupUID,
                                                IsShareDoctor = p.IsShareDoctor,
                                                Comments = p.Comments,
                                                ItemUID = p.ItemUID,
                                                ActiveFrom = p.ActiveFrom,
                                                ActiveTo = p.ActiveTo,
                                                CUser = p.CUser,
                                                CWhen = p.CWhen,
                                                MUser = p.MUser,
                                                MWhen = p.MWhen,
                                                StatusFlag = p.StatusFlag
                                            }).ToList();

            data.ForEach(p => p.Profit = p.Price - p.Cost);

            return data;
        }

        [Route("GetBillableItemAll")]
        public List<BillableItemModel> GetBillableItemAll()
        {
            DateTime now = DateTime.Now.Date;
            List<BillableItemModel> data = (from p in db.BillableItem
                                            where p.StatusFlag == "A"
                                            && (p.ActiveFrom == null || DbFunctions.TruncateTime(p.ActiveFrom) <= DbFunctions.TruncateTime(now))
                                            && (p.ActiveTo == null || DbFunctions.TruncateTime(p.ActiveTo) >= DbFunctions.TruncateTime(now))
                                            select new BillableItemModel
                                            {
                                                BillableItemUID = p.UID,
                                                Code = p.Code,
                                                Description = p.Description,
                                                ItemName = p.ItemName,
                                                Cost = p.Cost,
                                                DoctorFee = p.DoctorFee,
                                                TotalCost = p.TotalCost,
                                                BSMDDUID = p.BSMDDUID,
                                                BillingServiceMetaData = SqlFunction.fGetRfValDescription(p.BSMDDUID),
                                                BillingGroupUID = p.BillingGroupUID,
                                                BillingSubGroupUID = p.BillingSubGroupUID,
                                                IsShareDoctor = p.IsShareDoctor,
                                                Comments = p.Comments,
                                                ItemUID = p.ItemUID,
                                                ActiveFrom = p.ActiveFrom,
                                                ActiveTo = p.ActiveTo,
                                                CUser = p.CUser,
                                                CWhen = p.CWhen,
                                                MUser = p.MUser,
                                                MWhen = p.MWhen,
                                                StatusFlag = p.StatusFlag
                                            }).ToList();

            return data;
        }

        [Route("GetBillableItemByCategory")]
        [HttpGet]
        public List<BillableItemModel> GetBillableItemByCategory(string category)
        {
            DateTime now = DateTime.Now.Date;
            int BSMDDUID = db.ReferenceValue.FirstOrDefault(p => p.DomainCode == "BSMDD" && p.Description.ToLower() == category.ToLower() && p.StatusFlag == "A").UID;
            List<BillableItemModel> data = db.BillableItem
                .Where(p => p.BSMDDUID == BSMDDUID && p.StatusFlag == "A"
                 && (p.ActiveFrom == null || DbFunctions.TruncateTime(p.ActiveFrom) <= DbFunctions.TruncateTime(now))
                 && (p.ActiveTo == null || DbFunctions.TruncateTime(p.ActiveTo) >= DbFunctions.TruncateTime(now))
                )
                .Select(p => new BillableItemModel()
                {
                    BillableItemUID = p.UID,
                    Code = p.Code,
                    Description = p.Description,
                    ItemName = p.ItemName,
                    Cost = p.Cost,
                    DoctorFee = p.DoctorFee,
                    TotalCost = p.TotalCost,
                    BSMDDUID = p.BSMDDUID,
                    IsShareDoctor = p.IsShareDoctor,
                    Comments = p.Comments,
                    BillingGroupUID = p.BillingGroupUID,
                    BillingSubGroupUID = p.BillingSubGroupUID,
                    ItemUID = p.ItemUID,
                    ActiveFrom = p.ActiveFrom,
                    ActiveTo = p.ActiveTo,
                    CUser = p.CUser,
                    CWhen = p.CWhen,
                    MUser = p.MUser,
                    MWhen = p.MWhen,
                    StatusFlag = p.StatusFlag
                }).ToList();

            return data;
        }

        [Route("GetBillableItemByUID")]
        [HttpGet]
        public BillableItemModel GetBillableItemByUID(int billableItemUID)
        {
            BillableItemModel data = (from bl in db.BillableItem
                                      where bl.StatusFlag == "A"
                                      && bl.UID == billableItemUID
                                      select new BillableItemModel
                                      {
                                          BillableItemUID = bl.UID,
                                          Code = bl.Code,
                                          Description = bl.Description,
                                          ItemName = bl.ItemName,
                                          Cost = bl.Cost,
                                          DoctorFee = bl.DoctorFee,
                                          TotalCost = bl.TotalCost,
                                          BSMDDUID = bl.BSMDDUID,
                                          BillingGroupUID = bl.BillingGroupUID,
                                          BillingSubGroupUID = bl.BillingSubGroupUID,
                                          IsShareDoctor = bl.IsShareDoctor,
                                          BillingServiceMetaData = SqlFunction.fGetRfValDescription(bl.BSMDDUID),
                                          Comments = bl.Comments,
                                          ItemUID = bl.ItemUID,
                                          ActiveFrom = bl.ActiveFrom,
                                          ActiveTo = bl.ActiveTo,
                                          CUser = bl.CUser,
                                          CWhen = bl.CWhen,
                                          MUser = bl.MUser,
                                          MWhen = bl.MWhen,
                                          StatusFlag = bl.StatusFlag
                                      }).FirstOrDefault();

            if (data != null)
            {
                data.BillableItemDetails = GetBillableItemDetailByBillableItemUID(billableItemUID);
            }

            return data;
        }



        [Route("GetBillableItemDetailByBillableItemUID")]
        [HttpGet]
        public List<BillableItemDetailModel> GetBillableItemDetailByBillableItemUID(int billableItemUID)
        {
            List<BillableItemDetailModel> data = db.BillableItemDetail
                    .Where(p => p.StatusFlag == "A" && p.BillableItemUID == billableItemUID)
                    .Select(p => new BillableItemDetailModel
                    {
                        BillableItemDetailUID = p.UID,
                        BillableItemUID = p.BillableItemUID,
                        ActiveFrom = p.ActiveFrom ,
                        ActiveTo = p.ActiveTo,
                        Price = p.Price,
                        Cost = p.Cost ?? 0,
                        StatusFlag = p.StatusFlag,
                        OwnerOrganisationUID = p.OwnerOrganisationUID,
                        OwnerOrganisationName = p.OwnerOrganisationUID != 0 ? SqlFunction.fGetHealthOrganisationName(p.OwnerOrganisationUID) : "ราคามาตรฐานส่วนกลาง",
                        CURNCUID = p.CURNCUID,
                        Unit = SqlFunction.fGetRfValDescription(p.CURNCUID)
                    }).ToList();

            return data;
        }

        [Route("GetBillableItemByCode")]
        [HttpGet]
        public List<BillableItemModel> GetBillableItemByCode(string code)
        {
            DateTime now = DateTime.Now.Date;
            List<BillableItemModel> data = (from bl in db.BillableItem
                                            where bl.StatusFlag == "A"
                                            && bl.Code == code
                                            && (bl.ActiveFrom == null || DbFunctions.TruncateTime(bl.ActiveFrom) <= DbFunctions.TruncateTime(now))
                                            && (bl.ActiveTo == null || DbFunctions.TruncateTime(bl.ActiveTo) >= DbFunctions.TruncateTime(now))
                                            select new BillableItemModel
                                            {
                                                BillableItemUID = bl.UID,
                                                Code = bl.Code,
                                                Description = bl.Description,
                                                ItemName = bl.ItemName,
                                                Cost = bl.Cost,
                                                DoctorFee = bl.DoctorFee,
                                                TotalCost = bl.TotalCost,
                                                BSMDDUID = bl.BSMDDUID,
                                                IsShareDoctor = bl.IsShareDoctor,
                                                BillingServiceMetaData = SqlFunction.fGetRfValDescription(bl.BSMDDUID),
                                                BillingGroupUID = bl.BillingGroupUID,
                                                BillingSubGroupUID = bl.BillingSubGroupUID,
                                                Comments = bl.Comments,
                                                ItemUID = bl.ItemUID,
                                                ActiveFrom = bl.ActiveFrom,
                                                ActiveTo = bl.ActiveTo,
                                                CUser = bl.CUser,
                                                CWhen = bl.CWhen,
                                                MUser = bl.MUser,
                                                MWhen = bl.MWhen,
                                                StatusFlag = bl.StatusFlag
                                            }).ToList();

            return data;
        }


        [Route("GetBillableItemByBSMDD")]
        [HttpGet]
        public List<BillableItemModel> GetBillableItemByBSMDD(string BillingService)
        {
            List<BillableItemModel> data = null;
            DateTime now = DateTime.Now;
            int? BSMDDUID = null;
            ReferenceValue refvalue = db.ReferenceValue.FirstOrDefault(p => p.DomainCode == "BSMDD" && p.Description == BillingService && p.StatusFlag == "A");
            if (refvalue != null)
            {
                BSMDDUID = refvalue.UID;
            }

            if (BSMDDUID != null && BSMDDUID > 0)
            {
                data = db.BillableItem.Where(p => p.StatusFlag == "A" && (p.ActiveFrom == null || (p.ActiveFrom.HasValue && p.ActiveFrom.Value <= now.Date))
                    && (p.ActiveTo == null || (p.ActiveTo.HasValue && p.ActiveTo.Value >= now.Date)) && p.BSMDDUID == BSMDDUID
                    ).Select(f => new BillableItemModel
                    {
                        BillableItemUID = f.UID,
                        Code = f.Code,
                        ItemName = f.ItemName,
                        Cost = f.Cost,
                        DoctorFee = f.DoctorFee,
                        TotalCost = f.TotalCost,
                        ItemUID = f.ItemUID,
                        BSMDDUID = f.BSMDDUID,
                        BillingGroupUID = f.BillingGroupUID,
                        BillingSubGroupUID = f.BillingSubGroupUID,
                        BillingServiceMetaData = SqlFunction.fGetRfValDescription(f.BSMDDUID),
                        ActiveFrom = f.ActiveFrom,
                        ActiveTo = f.ActiveTo,
                        Description = f.Description
                    }).ToList();
            }


            return data;
        }

        [Route("ManageBillableItem")]
        [HttpPost]
        public HttpResponseMessage ManageBillableItem(BillableItemModel billableItemModel, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                using (var tran = new TransactionScope())
                {
                    MediTech.DataBase.BillableItem billableItem = db.BillableItem.Find(billableItemModel.BillableItemUID);

                    if (billableItem == null)
                    {
                        billableItem = new MediTech.DataBase.BillableItem();
                        billableItem.CUser = userID;
                        billableItem.CWhen = now;
                    }

                    billableItem.Code = billableItemModel.Code;
                    billableItem.ItemName = billableItemModel.ItemName;
                    billableItem.ItemNameSearch = SetItemNameSearch(billableItem.ItemName);
                    billableItem.Description = billableItemModel.Description;
                    billableItem.Comments = billableItemModel.Comments;
                    billableItem.ItemUID = billableItemModel.ItemUID;
                    billableItem.Cost = billableItemModel.Cost;
                    billableItem.DoctorFee = billableItemModel.DoctorFee;
                    billableItem.TotalCost = billableItemModel.TotalCost;
                    billableItem.ActiveFrom = billableItemModel.ActiveFrom;
                    billableItem.ActiveTo = billableItemModel.ActiveTo;
                    billableItem.IsShareDoctor = billableItemModel.IsShareDoctor;
                    if (billableItemModel.BSMDDUID != 0)
                    {
                        billableItem.BSMDDUID = billableItemModel.BSMDDUID;
                    }
                    else
                    {
                        billableItem.BSMDDUID = db.ReferenceValue.FirstOrDefault(p => p.StatusFlag == "A" && p.DomainCode == "BSMDD" && p.ValueCode == "RADIO").UID;
                    }

                    billableItem.BillingGroupUID = billableItemModel.BillingGroupUID;
                    billableItem.BillingSubGroupUID = billableItemModel.BillingSubGroupUID;
                    billableItem.MUser = userID;
                    billableItem.MWhen = now;
                    billableItem.StatusFlag = "A";

                    db.BillableItem.AddOrUpdate(billableItem);
                    db.SaveChanges();

                    //#region Delete BillableItemDetail
                    //IEnumerable<BillableItemDetail> billItmDetails = db.BillableItemDetail.Where(p => p.StatusFlag == "A" && p.BillableItemUID == billableItem.UID);

                    //if (billableItemModel.BillableItemDetails == null)
                    //{
                    //    foreach (var item in billItmDetails)
                    //    {
                    //        db.BillableItemDetail.Attach(item);
                    //        item.MUser = userID;
                    //        item.MWhen = now;
                    //        item.StatusFlag = "D";
                    //    }
                    //}
                    //else
                    //{
                    //    foreach (var item in billItmDetails)
                    //    {
                    //        var data = billableItemModel.BillableItemDetails.FirstOrDefault(p => p.BillableItemDetailUID == item.UID);
                    //        if (data == null)
                    //        {
                    //            db.BillableItemDetail.Attach(item);
                    //            item.MUser = userID;
                    //            item.MWhen = now;
                    //            item.StatusFlag = "D";
                    //        }

                    //    }
                    //}

                    //db.SaveChanges();

                    //#endregion

                    if (billableItemModel.BillableItemDetails != null)
                    {
                        foreach (var item in billableItemModel.BillableItemDetails)
                        {
                            BillableItemDetail billItemDetail = db.BillableItemDetail.Find(item.BillableItemDetailUID);


                            if (billItemDetail == null)
                            {
                                billItemDetail = new BillableItemDetail();
                                billItemDetail.CUser = userID;
                                billItemDetail.CWhen = now;
                                billItemDetail.MUser = userID;
                                billItemDetail.MWhen = now;
                                billItemDetail.StatusFlag = item.StatusFlag;
                                billItemDetail.BillableItemUID = billableItem.UID;
                                billItemDetail.ActiveFrom = item.ActiveFrom;
                                billItemDetail.ActiveTo = item.ActiveTo;
                                billItemDetail.Price = item.Price;
                                billItemDetail.Cost = item.Cost;
                                billItemDetail.OwnerOrganisationUID = item.OwnerOrganisationUID;
                                billItemDetail.CURNCUID = item.CURNCUID;
                            }
                            else if (item.StatusFlag == "A")
                            {
                                if (item.MWhen != DateTime.MinValue)
                                {
                                    billItemDetail.MUser = userID;
                                    billItemDetail.MWhen = now;
                                    billItemDetail.BillableItemUID = billableItem.UID;
                                    billItemDetail.ActiveFrom = item.ActiveFrom;
                                    billItemDetail.ActiveTo = item.ActiveTo;
                                    billItemDetail.Price = item.Price;
                                    billItemDetail.Cost = item.Cost;
                                    billItemDetail.OwnerOrganisationUID = item.OwnerOrganisationUID;
                                    billItemDetail.CURNCUID = item.CURNCUID;
                                }

                            }
                            else if (item.StatusFlag == "D")
                            {
                                billItemDetail.MUser = userID;
                                billItemDetail.MWhen = now;
                                billItemDetail.StatusFlag = "D";
                            }



                            db.BillableItemDetail.AddOrUpdate(billItemDetail);
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

        [Route("DeleteBillableItem")]
        [HttpDelete]
        public HttpResponseMessage DeleteBillableItem(int billableItemUID, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                MediTech.DataBase.BillableItem billableItem = db.BillableItem.Find(billableItemUID);
                if (billableItem != null)
                {
                    db.BillableItem.Attach(billableItem);
                    billableItem.MUser = userID;
                    billableItem.MWhen = now;
                    billableItem.StatusFlag = "D";
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

        #region OrderSet

        [Route("SearchOrderSet")]
        [HttpGet]
        public List<OrderSetModel> SearchOrderSet(string code, string name)
        {
            List<OrderSetModel> data = (from p in db.OrderSet
                                        where p.StatusFlag == "A"
                                        && (string.IsNullOrEmpty(code) || p.Code.ToLower().Contains(code.ToLower()))
                                        && (string.IsNullOrEmpty(name) || p.OrdersetNameSearch.ToLower().Contains(name.ToLower()))
                                        select new OrderSetModel
                                        {
                                            OrderSetUID = p.UID,
                                            Code = p.Code,
                                            Description = p.Description,
                                            Name = p.Name,
                                            OrdersetNameSearch = p.OrdersetNameSearch,
                                            ActiveFrom = p.ActiveFrom,
                                            ActiveTo = p.ActiveTo,
                                            CUser = p.CUser,
                                            CWhen = p.CWhen,
                                            MUser = p.MUser,
                                            MWhen = p.MWhen,
                                            StatusFlag = p.StatusFlag
                                        }).ToList();

            return data;
        }

        [Route("GetOrderSetByUID")]
        [HttpGet]
        public OrderSetModel GetOrderSetByUID(int orderSetUID)
        {
            OrderSetModel data = (from p in db.OrderSet
                                  where p.UID == orderSetUID
                                  && p.StatusFlag == "A"
                                  select new OrderSetModel
                                  {
                                      OrderSetUID = p.UID,
                                      Code = p.Code,
                                      Description = p.Description,
                                      Name = p.Name,
                                      OrdersetNameSearch = p.OrdersetNameSearch,
                                      ActiveFrom = p.ActiveFrom,
                                      ActiveTo = p.ActiveTo,
                                      CUser = p.CUser,
                                      CWhen = p.CWhen,
                                      MUser = p.MUser,
                                      MWhen = p.MWhen,
                                      StatusFlag = p.StatusFlag
                                  }).FirstOrDefault();

            if (data != null)
            {
                data.OrderSetBillableItems = (from ord in db.OrderSetBillableItem
                                              join bill in db.BillableItem on ord.BillableItemUID equals bill.UID
                                              where ord.StatusFlag == "A"
                                              && ord.OrderSetUID == data.OrderSetUID
                                              select new OrderSetBillableItemModel
                                              {
                                                  OrderSetBillableItemUID = ord.UID,
                                                  OrderSetUID = ord.OrderSetUID,
                                                  BillableItemUID = ord.BillableItemUID,
                                                  Code = bill.Code,
                                                  OrderCatalogName = bill.ItemName,
                                                  ActiveFrom = ord.ActiveFrom,
                                                  ActiveTo = ord.ActiveTo,
                                                  FRQNCUID = ord.FRQNCUID,
                                                  BillingServiceMetaData = SqlFunction.fGetRfValDescription(bill.BSMDDUID),
                                                  DoseQty = ord.DoseQty,
                                                  Price = ord.Price ?? 0,
                                                  NetPrice = (ord.Price ?? 0) * ord.Quantity,
                                                  DoctorFee = ord.DoctorFee ?? 0,
                                                  CareproviderUID = ord.CareproviderUID,
                                                  CareproviderName = ord.CareproviderUID != null ? SqlFunction.fGetCareProviderName(ord.CareproviderUID.Value) : "",
                                                  Quantity = ord.Quantity,
                                                  ProcessingNotes = ord.ProcessingNotes,
                                                  StatusFlag = ord.StatusFlag
                                              }).ToList();

                if (data.OrderSetBillableItems != null)
                {
                    foreach (var item in data.OrderSetBillableItems)
                    {
                        item.BillableItemDetails = GetBillableItemDetailByBillableItemUID(item.BillableItemUID);
                    }

                }

            }

            return data;
        }

        [Route("ManageOrderSet")]
        [HttpPost]
        public HttpResponseMessage ManageOrderSet(OrderSetModel orderSetModel, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                using (var tran = new TransactionScope())
                {

                    OrderSet orderSet = db.OrderSet.Find(orderSetModel.OrderSetUID);
                    if (orderSet == null)
                    {
                        orderSet = new OrderSet();
                        orderSet.CUser = userID;
                        orderSet.CWhen = now;
                    }

                    orderSet.Code = orderSetModel.Code;
                    orderSet.Name = orderSetModel.Name;
                    orderSet.Description = orderSetModel.Description;
                    orderSet.OrdersetNameSearch = SetItemNameSearch(orderSet.Name);
                    orderSet.ActiveFrom = orderSetModel.ActiveFrom;
                    orderSet.ActiveTo = orderSetModel.ActiveTo;
                    orderSet.MUser = userID;
                    orderSet.MWhen = now;
                    orderSet.StatusFlag = "A";

                    db.OrderSet.AddOrUpdate(orderSet);
                    db.SaveChanges();

                    #region Delete OrdersetBillableItem
                    IEnumerable<OrderSetBillableItem> orderSetBills = db.OrderSetBillableItem.Where(p => p.StatusFlag == "A" && p.OrderSetUID == orderSet.UID);

                    if (orderSetModel.OrderSetBillableItems == null)
                    {
                        foreach (var item in orderSetBills)
                        {
                            db.OrderSetBillableItem.Attach(item);
                            item.MUser = userID;
                            item.MWhen = now;
                            item.StatusFlag = "D";
                        }
                    }
                    else
                    {
                        foreach (var item in orderSetBills)
                        {
                            var data = orderSetModel.OrderSetBillableItems.FirstOrDefault(p => p.OrderSetBillableItemUID == item.UID);
                            if (data == null)
                            {
                                db.OrderSetBillableItem.Attach(item);
                                item.MUser = userID;
                                item.MWhen = now;
                                item.StatusFlag = "D";
                            }

                        }
                    }

                    db.SaveChanges();

                    #endregion

                    if (orderSetModel.OrderSetBillableItems != null)
                    {
                        foreach (var item in orderSetModel.OrderSetBillableItems)
                        {
                            OrderSetBillableItem orderSetBill = db.OrderSetBillableItem.Find(item.OrderSetBillableItemUID);
                            if (orderSetBill == null)
                            {
                                orderSetBill = new OrderSetBillableItem();
                                orderSetBill.CUser = userID;
                                orderSetBill.CWhen = now;
                                orderSetBill.MUser = userID;
                                orderSetBill.MWhen = now;
                                orderSetBill.StatusFlag = "A";
                                orderSetBill.OrderSetUID = orderSet.UID;
                                orderSetBill.BillableItemUID = item.BillableItemUID;
                                orderSetBill.ActiveFrom = item.ActiveFrom;
                                orderSetBill.ActiveTo = item.ActiveTo;
                                orderSetBill.OrderCatalogName = item.OrderCatalogName;
                                orderSetBill.Quantity = item.Quantity;
                                orderSetBill.FRQNCUID = item.FRQNCUID;
                                orderSetBill.DoseQty = item.DoseQty;
                                orderSetBill.Price = item.Price;
                                orderSetBill.DoctorFee = item.DoctorFee;
                                orderSetBill.CareproviderUID = item.CareproviderUID;
                                orderSetBill.ProcessingNotes = item.ProcessingNotes;
                            }
                            else
                            {
                                if (item.StatusFlag == "A")
                                {
                                    if (item.MWhen != DateTime.MinValue)
                                    {
                                        orderSetBill.MUser = userID;
                                        orderSetBill.MWhen = now;
                                        orderSetBill.OrderSetUID = orderSet.UID;
                                        orderSetBill.BillableItemUID = item.BillableItemUID;
                                        orderSetBill.ActiveFrom = item.ActiveFrom;
                                        orderSetBill.ActiveTo = item.ActiveTo;
                                        orderSetBill.OrderCatalogName = item.OrderCatalogName;
                                        orderSetBill.Quantity = item.Quantity;
                                        orderSetBill.FRQNCUID = item.FRQNCUID;
                                        orderSetBill.DoseQty = item.DoseQty;
                                        orderSetBill.Price = item.Price;
                                        orderSetBill.DoctorFee = item.DoctorFee;
                                        orderSetBill.CareproviderUID = item.CareproviderUID;
                                        orderSetBill.ProcessingNotes = item.ProcessingNotes;
                                    }
                                }
                                else if (item.StatusFlag == "D")
                                {
                                    orderSetBill.StatusFlag = "D";
                                    orderSetBill.MUser = userID;
                                    orderSetBill.MWhen = now;
                                }

                            }

                            db.OrderSetBillableItem.AddOrUpdate(orderSetBill);
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

        [Route("DeleteOrderSet")]
        [HttpDelete]
        public HttpResponseMessage DeleteOrderSet(int orderSetUID, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                MediTech.DataBase.OrderSet orderSet = db.OrderSet.Find(orderSetUID);
                if (orderSet != null)
                {
                    db.OrderSet.Attach(orderSet);
                    orderSet.MUser = userID;
                    orderSet.MWhen = now;
                    orderSet.StatusFlag = "D";
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

        #region HealthOrganisation

        [Route("GetHealthOrganisation")]
        [HttpGet]
        public List<HealthOrganisationModel> GetHealthOrganisation()
        {

            List<HealthOrganisationModel> data = db.HealthOrganisation
                .Where(p => p.StatusFlag == "A").Select(p => new HealthOrganisationModel()
                {
                    HealthOrganisationUID = p.UID,
                    Code = p.Code,
                    Name = p.Name,
                    Description = p.Description,
                    MobileNo = p.MobileNo,
                    FaxNo = p.FaxNo,
                    Email = p.Email,
                    Address = p.Address,
                    Address2 = p.Address2,
                    ProvinceUID = p.ProvinceUID,
                    DistrictUID = p.DistrictUID,
                    AmphurUID = p.AmphurUID,
                    ZipCode = p.ZipCode,
                    HOTYPUID = p.HOTYPUID,
                    IsStock = p.IsStock,
                    HealthOrganisationType = SqlFunction.fGetRfValDescription(p.HOTYPUID),
                    TINNo = p.TINNo,
                    LicenseNo = p.LicenseNo,
                    AddressFull = SqlFunction.fGetAddressOrganisation(p.UID),
                    ActiveFrom = p.ActiveFrom,
                    ActiveTo = p.ActiveTo,
                    CUser = p.CUser,
                    CWhen = p.CWhen,
                    MUser = p.MUser,
                    MWhen = p.MWhen,
                    LogoImage = p.LogoImage,
                    Comment = p.Comment,
                    StatusFlag = p.StatusFlag
                }).ToList();

            if (data != null && data.Count() > 0)
            {
                foreach (var item in data)
                {
                    item.HealthOrganisationIDs = GetHealthOrganisationIDByOrganiUID(item.HealthOrganisationUID);
                }
            }

            return data;
        }

        [Route("GetHealthOrganisationActive")]
        [HttpGet]
        public List<HealthOrganisationModel> GetHealthOrganisationActive()
        {
            List<HealthOrganisationModel> data = db.HealthOrganisation
                .Where(p => p.StatusFlag == "A"
                && (p.ActiveFrom == null || DbFunctions.TruncateTime(p.ActiveFrom) <= DbFunctions.TruncateTime(DateTime.Now))
                && (p.ActiveTo == null || DbFunctions.TruncateTime(p.ActiveTo) >= DbFunctions.TruncateTime(DateTime.Now)))
                .Select(p => new HealthOrganisationModel()
                {
                    HealthOrganisationUID = p.UID,
                    Code = p.Code,
                    Name = p.Name,
                    Description = p.Description,
                    MobileNo = p.MobileNo,
                    FaxNo = p.FaxNo,
                    Email = p.Email,
                    Address = p.Address,
                    ProvinceUID = p.ProvinceUID,
                    DistrictUID = p.DistrictUID,
                    AmphurUID = p.AmphurUID,
                    ZipCode = p.ZipCode,
                    HOTYPUID = p.HOTYPUID,
                    IsStock = p.IsStock,
                    HealthOrganisationType = SqlFunction.fGetRfValDescription(p.HOTYPUID),
                    TINNo = p.TINNo,
                    LicenseNo = p.LicenseNo,
                    AddressFull = SqlFunction.fGetAddressOrganisation(p.UID),
                    ActiveFrom = p.ActiveFrom,
                    ActiveTo = p.ActiveTo,
                    CUser = p.CUser,
                    CWhen = p.CWhen,
                    MUser = p.MUser,
                    MWhen = p.MWhen,
                    StatusFlag = p.StatusFlag
                }).ToList();

            return data;
        }

        [Route("GetHealthOrganisationByUID")]
        [HttpGet]
        public HealthOrganisationModel GetHealthOrganisationByUID(int healthOrganisationUID)
        {
            var healthOwn = db.HealthOrganisation.Find(healthOrganisationUID);
            HealthOrganisationModel data = null;
            if (healthOwn != null)
            {
                data = new HealthOrganisationModel();
                data.HealthOrganisationUID = healthOwn.UID;
                data.Code = healthOwn.Code;
                data.Description = healthOwn.Description;
                data.Name = healthOwn.Name;
                data.MobileNo = healthOwn.MobileNo;
                data.FaxNo = healthOwn.FaxNo;
                data.Email = healthOwn.Email;
                data.Address = healthOwn.Address;
                data.Address2 = healthOwn.Address2;
                data.ProvinceUID = healthOwn.ProvinceUID;
                data.DistrictUID = healthOwn.DistrictUID;
                data.AmphurUID = healthOwn.AmphurUID;
                data.ZipCode = healthOwn.ZipCode;
                data.HOTYPUID = healthOwn.HOTYPUID;
                data.IsStock = healthOwn.IsStock;
                data.TINNo = healthOwn.TINNo;
                data.LicenseNo = healthOwn.LicenseNo;
                data.ActiveFrom = healthOwn.ActiveFrom;
                data.ActiveTo = healthOwn.ActiveTo;
                data.CUser = healthOwn.CUser;
                data.CWhen = healthOwn.CWhen;
                data.MUser = healthOwn.MUser;
                data.MWhen = healthOwn.MWhen;
                data.LogoImage = healthOwn.LogoImage;
                data.Comment = healthOwn.Comment;
                data.StatusFlag = healthOwn.StatusFlag;
            }

            if (data != null)
            {
                data.HealthOrganisationIDs = GetHealthOrganisationIDByOrganiUID(data.HealthOrganisationUID);
            }

            return data;
        }

        [Route("GetHealthOrganisationIDByOrganiUID")]
        public List<HealthOrganisationIDModel> GetHealthOrganisationIDByOrganiUID(int healthOrganisationUID)
        {
            List<HealthOrganisationIDModel> healthIDs = db.HealthOrganisationID.Where(p => p.HealthOrganisationUID == healthOrganisationUID
            && p.StatusFlag == "A")
            .Select(p => new HealthOrganisationIDModel
            {
                HealthOrganisationIDUID = p.UID,
                HealthOrganisationUID = p.HealthOrganisationUID,
                BLTYPUID = p.BLTYPUID,
                BillType = SqlFunction.fGetRfValDescription(p.BLTYPUID ?? 0),
                IDFormat = p.IDFormat,
                IDLength = p.IDLength,
                LastRenumberDttm = p.LastRenumberDttm,
                NumberValue = p.NumberValue,
                ActiveFrom = p.ActiveFrom,
                ActiveTo = p.ActiveTo,
                StatusFlag = p.StatusFlag

            }).ToList();

            return healthIDs;
        }

        [Route("ManageHealthOrganisation")]
        [HttpPost]
        public HttpResponseMessage ManageHealthOrganisation(HealthOrganisationModel healthOrganisationModel, int userID)
        {
            try
            {

                DateTime now = DateTime.Now;
                using (var tran = new TransactionScope())
                {
                    MediTech.DataBase.HealthOrganisation healthOrganisation = db.HealthOrganisation.Find(healthOrganisationModel.HealthOrganisationUID);

                    if (healthOrganisation == null)
                    {
                        healthOrganisation = new MediTech.DataBase.HealthOrganisation();
                        healthOrganisation.CUser = userID;
                        healthOrganisation.CWhen = now;
                    }

                    healthOrganisation.Code = healthOrganisationModel.Code;
                    healthOrganisation.IsStock = healthOrganisationModel.IsStock;
                    healthOrganisation.Name = healthOrganisationModel.Name;
                    healthOrganisation.Description = healthOrganisationModel.Description;
                    healthOrganisation.MobileNo = healthOrganisationModel.MobileNo != "" ? healthOrganisationModel.MobileNo : null;
                    healthOrganisation.FaxNo = healthOrganisationModel.FaxNo;
                    healthOrganisation.Email = healthOrganisationModel.Email;
                    healthOrganisation.Address = healthOrganisationModel.Address != "" ? healthOrganisationModel.Address : null;
                    healthOrganisation.ProvinceUID = healthOrganisationModel.ProvinceUID;
                    healthOrganisation.DistrictUID = healthOrganisationModel.DistrictUID;
                    healthOrganisation.AmphurUID = healthOrganisationModel.AmphurUID;
                    healthOrganisation.ZipCode = healthOrganisationModel.ZipCode != "" ? healthOrganisationModel.ZipCode : null;
                    healthOrganisation.HOTYPUID = healthOrganisationModel.HOTYPUID;
                    healthOrganisation.TINNo = healthOrganisationModel.TINNo;
                    healthOrganisation.LicenseNo = healthOrganisationModel.LicenseNo;
                    healthOrganisation.ActiveFrom = healthOrganisationModel.ActiveFrom;
                    healthOrganisation.ActiveTo = healthOrganisationModel.ActiveTo;
                    healthOrganisation.MUser = userID;
                    healthOrganisation.MWhen = now;
                    healthOrganisation.StatusFlag = "A";
                    healthOrganisation.Address2 = healthOrganisationModel.Address2;
                    healthOrganisation.LogoImage = healthOrganisationModel.LogoImage;
                    healthOrganisation.Comment = healthOrganisationModel.Comment;


                    db.HealthOrganisation.AddOrUpdate(healthOrganisation);
                    db.SaveChanges();



                    #region DeleteHealthIDs
                    List<HealthOrganisationID> healthIDs = db.HealthOrganisationID.Where(p => p.HealthOrganisationUID == healthOrganisationModel.HealthOrganisationUID && p.StatusFlag == "A").ToList();
                    if (healthOrganisationModel.HealthOrganisationIDs == null)
                    {
                        foreach (var item in healthIDs)
                        {
                            db.HealthOrganisationID.Attach(item);
                            item.MUser = userID;
                            item.MWhen = now;
                            item.StatusFlag = "D";
                        }
                    }
                    else
                    {
                        foreach (var item in healthIDs)
                        {
                            var data = healthOrganisationModel.HealthOrganisationIDs.FirstOrDefault(p => p.HealthOrganisationIDUID == item.UID);
                            if (data == null)
                            {
                                db.HealthOrganisationID.Attach(item);
                                item.MUser = userID;
                                item.MWhen = now;
                                item.StatusFlag = "D";
                            }

                        }
                    }

                    db.SaveChanges();

                    #endregion

                    if (healthOrganisationModel.HealthOrganisationIDs != null)
                    {
                        foreach (var item in healthOrganisationModel.HealthOrganisationIDs)
                        {
                            HealthOrganisationID healthOrganID = db.HealthOrganisationID.Find(item.HealthOrganisationIDUID);
                            if (healthOrganID == null)
                            {
                                healthOrganID = new HealthOrganisationID();
                                healthOrganID.CUser = userID;
                                healthOrganID.CWhen = now;
                                healthOrganID.MUser = userID;
                                healthOrganID.MWhen = now;
                                healthOrganID.StatusFlag = "A";
                            }
                            else
                            {
                                if (item.MWhen != DateTime.MinValue)
                                {
                                    healthOrganID.MUser = userID;
                                    healthOrganID.MWhen = now;
                                }
                            }
                            healthOrganID.HealthOrganisationUID = healthOrganisation.UID;
                            healthOrganID.BLTYPUID = item.BLTYPUID;
                            healthOrganID.IDFormat = item.IDFormat;
                            healthOrganID.IDLength = item.IDLength;
                            healthOrganID.LastRenumberDttm = item.LastRenumberDttm;
                            healthOrganID.NumberValue = item.NumberValue;
                            healthOrganID.ActiveFrom = item.ActiveFrom;
                            healthOrganID.ActiveTo = item.ActiveTo;
                            db.HealthOrganisationID.AddOrUpdate(healthOrganID);
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

        [Route("DeleteHealthOrganisation")]
        [HttpDelete]
        public HttpResponseMessage DeleteHealthOrganisation(int healthOrganisationUID, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                using (var tran = new TransactionScope())
                {
                    MediTech.DataBase.HealthOrganisation healthOrganisation = db.HealthOrganisation.Find(healthOrganisationUID);
                    if (healthOrganisation != null)
                    {
                        db.HealthOrganisation.Attach(healthOrganisation);
                        healthOrganisation.MUser = userID;
                        healthOrganisation.MWhen = now;
                        healthOrganisation.StatusFlag = "D";
                        db.SaveChanges();
                    }

                    var healthIDs = db.HealthOrganisationID.Where(p => p.HealthOrganisationUID == healthOrganisationUID);
                    if (healthIDs != null)
                    {
                        foreach (var item in healthIDs)
                        {
                            db.HealthOrganisationID.Attach(item);
                            item.MUser = userID;
                            item.MWhen = now;
                            item.StatusFlag = "D";
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

        #region Location
        [Route("GetLocationAll")]
        [HttpGet]
        public List<LookupReferenceValueModel> GetLocationAll()
        {
            var data = db.Location.Where(p => p.StatusFlag == "A").Select(p => new LookupReferenceValueModel()
            {
                Key = p.UID,
                Display = p.Name,
                DisplayOrder = p.DisplayOrder ?? 1
            }).ToList();

            return data;
        }

        [Route("GetLocationByOrganisationUID")]
        [HttpGet]
        public List<LocationModel> GetLocationByOrganisationUID(int ownerOrganisationUID)
        {
            var data = db.Location.Where(p => p.StatusFlag == "A" && p.OwnerOrganisationUID == ownerOrganisationUID).Select(p => new LocationModel()
            {
                LocationUID = p.UID,
                Name = p.Name,
                Description = p.Description,
                LOTYPUID = p.LOTYPUID,
                LocationType = SqlFunction.fGetRfValDescription(p.LOTYPUID),
                DisplayOrder = p.DisplayOrder ?? 1,
                ParentLocationUID = p.ParentLocationUID,
                PhoneNumber = p.PhoneNumber,
                IsRegistrationAllowed = p.IsRegistrationAllowed,
                IsCanOrder = p.IsCanOrder,
                IsTemporaryBed = p.IsTemporaryBed,
                LCTSTUID = p.LCTSTUID,
                EMRZONUID = p.EMZONEUID,
                OwnerOrganisationUID = p.OwnerOrganisationUID,
            }).ToList();

            return data;
        }

        [Route("GetLocationIsRegister")]
        [HttpGet]
        public List<LocationModel> GetLocationIsRegister(int ownerOrganisationUID)
        {
            DateTime now = DateTime.Now;
            var data = db.Location.Where(p => p.StatusFlag == "A" && p.IsRegistrationAllowed == "Y"
            && p.OwnerOrganisationUID == ownerOrganisationUID
            && (p.ActiveTo == null || DbFunctions.TruncateTime(p.ActiveTo) >= DbFunctions.TruncateTime(now))).Select(p => new LocationModel()
            {
                LocationUID = p.UID,
                Name = p.Name,
                Description = p.Description,
                LOTYPUID = p.LOTYPUID,
                LocationType = SqlFunction.fGetRfValDescription(p.LOTYPUID),
                DisplayOrder = p.DisplayOrder ?? 1,
                ParentLocationUID = p.ParentLocationUID,
                PhoneNumber = p.PhoneNumber,
                IsRegistrationAllowed = p.IsRegistrationAllowed,
                IsCanOrder = p.IsCanOrder,
                IsTemporaryBed = p.IsTemporaryBed,
                LCTSTUID = p.LCTSTUID,
                EMRZONUID = p.EMZONEUID,
                OwnerOrganisationUID = p.OwnerOrganisationUID,
            }).ToList();

            return data;
        }

        #region Speciality
        [Route("GetSpecialityAll")]
        [HttpGet]
        public List<SpecialityModel> GetSpecialityAll()
        {
            var data = db.Speciality.Where(p => p.StatusFlag == "A").Select(p => new SpecialityModel()
            {
                SpecialityUID = p.UID,
                Name = p.Name,
                Description = p.Description,
                OwnerOrganisationUID = p.OwnerOrganisationUID,
                HealthOrganisationUID = p.HealthOrganisationUID
            }).ToList();

            return data;
        }
        #endregion

        #endregion

        #region OrderCategory
        [Route("GetOrderCategory")]
        [HttpGet]
        public List<OrderCategoryModel> GetOrderCategory()
        {
            var data = db.OrderCategory.Where(p => p.StatusFlag == "A").Select(p => new OrderCategoryModel()
            {
                OrderCategoryUID = p.UID,
                Name = p.Name,
                Description = p.Description,
                Code = p.Code,
                DisplayOrder = p.DisplayOrder ?? 1
            }).ToList();

            return data;
        }

        [Route("GetOrderSubCategoryByUID")]
        [HttpGet]
        public List<OrderSubCategoryModel> GetOrderSubCategoryByUID(int orderCateogoryUID)
        {
            var data = db.OrderSubCategory.Where(p => p.StatusFlag == "A" && p.OrderCategoryUID == orderCateogoryUID)
            .Select(p => new OrderSubCategoryModel()
            {
                OrderSubCategoryUID = p.UID,
                OrderCategoryUID = p.OrderCategoryUID,
                Name = p.Name,
                Description = p.Description,
                DisplayOrder = p.DisplayOrder ?? 1
            }).ToList();

            return data;
        }

        #endregion


        public string SetItemNameSearch(string itemName)
        {
            itemName = itemName.Replace(",", "");
            itemName = itemName.Replace(".", "");
            itemName = itemName.Replace("(", "");
            itemName = itemName.Replace(")", "");
            itemName = itemName.Replace(";", "");
            itemName = itemName.Replace("#", "");
            itemName = itemName.Replace("<", "");
            itemName = itemName.Replace(">", "");
            itemName = itemName.Replace("?", "");
            itemName = itemName.Replace("\"", "");
            itemName = itemName.Replace("*", "");
            itemName = itemName.Replace("&", "");
            itemName = itemName.Replace("^", "");
            itemName = itemName.Replace("$", "");
            itemName = itemName.Replace("@", "");
            itemName = itemName.Replace("!", "");
            itemName = itemName.Replace("|", "");
            itemName = itemName.Replace("}", "");
            itemName = itemName.Replace("{", "");
            itemName = itemName.Replace(":", "");
            itemName = itemName.Replace("\\", "");
            itemName = itemName.Replace("/", "");
            itemName = itemName.Replace(" ", "");
            itemName = itemName.Replace("	", "");
            itemName = itemName.Replace("-", "");
            itemName = itemName.Replace("+", "");
            itemName = itemName.Replace("=", "");
            itemName = itemName.Replace("_", "");

            return itemName;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
