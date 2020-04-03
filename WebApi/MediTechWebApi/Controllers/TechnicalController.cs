using MediTech.DataBase;
using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web.Http;
using System.Data.SqlClient;
using System.Data.Entity;

namespace MediTechWebApi.Controllers
{
    [RoutePrefix("Api/Technical")]
    public class TechnicalController : ApiController
    {
        protected MediTechEntities db = new MediTechEntities();

        [Route("SearchReferenceDomain")]
        [HttpGet]
        public List<ReferenceDomainModel> SearchReferenceDomain(string domainCode, string description, string valueDescription)
        {
            var data = (from rd in db.ReferenceDomain
                        join rv in db.ReferenceValue on rd.UID equals rv.ReferenceDomainUID
                        where rd.StatusFlag == "A"
                        && rv.StatusFlag == "A"
                        && (rd.DomainCode.Contains(domainCode) || String.IsNullOrEmpty(domainCode))
                        && (rd.Description.Contains(description) || String.IsNullOrEmpty(description))
                        && (rv.Description.Contains(valueDescription) || rv.ValueCode.Contains(valueDescription) || String.IsNullOrEmpty(valueDescription))
                        select new ReferenceDomainModel()
                        {
                            UID = rd.UID,
                            Description = rd.Description,
                            DomainCode = rd.DomainCode,
                            ActiveFrom = rd.ActiveFrom,
                            ActiveTo = rd.ActiveTo,
                            CUser = rd.CUser,
                            CWhen = rd.CWhen,
                            MUser = rd.MUser,
                            MWhen = rd.MWhen,
                            DLFlag = rd.DLFlag,
                            IsSortByDescription = rd.IsSortByDescription ?? false,
                            StatusFlag = rd.StatusFlag
                        }).ToList();

            if (data != null)
            {
                data = data.GroupBy(p => p.UID).Select(group => group.First()).ToList();
            }

            return data;
        }


        [Route("GetReferenceDomain")]
        [HttpGet]
        public List<ReferenceDomainModel> GetReferenceDomain()
        {
            var data = db.ReferenceDomain.Where(p => p.StatusFlag == "A").Select(p => new ReferenceDomainModel()
            {
                UID = p.UID,
                Description = p.Description,
                DomainCode = p.DomainCode,
                ActiveFrom = p.ActiveFrom,
                ActiveTo = p.ActiveTo,
                CUser = p.CUser,
                CWhen = p.CWhen,
                MUser = p.MUser,
                MWhen = p.MWhen,
                DLFlag = p.DLFlag,
                IsSortByDescription = p.IsSortByDescription ?? false,
                StatusFlag = p.StatusFlag
            }).ToList();
            return data;
        }

        [Route("GetReferenceDomainByUID")]
        [HttpGet]
        public ReferenceDomainModel GetReferenceDomainByUID(int referenceDomainUID)
        {
            ReferenceDomainModel refModel = null;
            var data = db.ReferenceDomain.Find(referenceDomainUID);
            if (data != null)
            {
                refModel = new ReferenceDomainModel();
                refModel.UID = data.UID;
                refModel.DomainCode = data.DomainCode;
                refModel.Description = data.Description;
                refModel.ActiveFrom = data.ActiveFrom;
                refModel.ActiveTo = data.ActiveTo;
                refModel.CUser = data.CUser;
                refModel.CWhen = data.CWhen;
                refModel.MUser = data.MUser;
                refModel.MWhen = data.MWhen;
                refModel.DLFlag = data.DLFlag;
                refModel.IsSortByDescription = data.IsSortByDescription ?? false;
                refModel.StatusFlag = data.StatusFlag;
                refModel.ReferenceValues = db.ReferenceValue.Where(p => p.DomainCode == refModel.DomainCode && p.StatusFlag == "A").Select(p => new ReferencevalueModel
                {
                    ReferenceValueUID = p.UID,
                    ReferenceDomainUID = p.ReferenceDomainUID,
                    Description = p.Description,
                    ValueCode = p.ValueCode,
                    ActiveFrom = p.ActiveFrom,
                    ActiveTo = p.ActiveTo,
                    DisplayOrder = p.DisplayOrder,
                    AlternateName = p.AlternateName,
                    DLFlag = p.DLFlag,
                    DomainCode = p.DomainCode,
                    LANUGUID = p.LANUGUID,
                    NumericValue = p.NumericValue,
                    SpecialityUID = p.SpecialityUID
                }).ToList();
            }


            return refModel;
        }

        [Route("GetReferenceValueMany")]
        [HttpGet]
        public List<LookupReferenceValueModel> GetReferenceValueMany(string domainCode)
        {
            DateTime now = DateTime.Now;
            List<LookupReferenceValueModel> data = db.ReferenceValue
                .Where(p =>
                p.DomainCode == domainCode
                && p.StatusFlag == "A"
                && (p.ActiveFrom == null || DbFunctions.TruncateTime(p.ActiveFrom) <= DbFunctions.TruncateTime(now))
                && (p.ActiveTo == null || DbFunctions.TruncateTime(p.ActiveTo) >= DbFunctions.TruncateTime(now))
                ).Select(p =>
            new LookupReferenceValueModel
            {
                Key = p.UID,
                Display = p.Description,
                DisplayOrder = p.DisplayOrder ?? 1,
                ValueCode = p.ValueCode,
                AlternateName = p.AlternateName
            }).ToList();
            return data;
        }

        [Route("GetReferenceValueList")]
        [HttpGet]
        public List<LookupReferenceValueModel> GetReferenceValueList(string domainCodeList)
        {
            List<LookupReferenceValueModel> datalist = new List<LookupReferenceValueModel>();

            DataTable dt = SqlDirectStore.pGetReferenceValueList(domainCodeList);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    LookupReferenceValueModel data = new LookupReferenceValueModel();
                    data.Key = int.Parse(dt.Rows[i]["UID"].ToString());
                    data.ValueCode = dt.Rows[i]["ValueCode"].ToString();
                    data.Display = dt.Rows[i]["Description"].ToString();
                    data.DisplayOrder = dt.Rows[i]["DisplayOrder"].ToString() != "" ? int.Parse(dt.Rows[i]["DisplayOrder"].ToString()) : 0;
                    data.DomainCode = dt.Rows[i]["DomainCode"].ToString();
                    data.AlternateName = dt.Rows[i]["AlternateName"].ToString();
                    datalist.Add(data);
                }
            }

            return datalist;
        }

        [Route("GetReferenceValueByCode")]
        [HttpGet]
        public LookupReferenceValueModel GetReferenceValueByCode(string domainCode, string valueCode)
        {
            DateTime now = DateTime.Now;
            LookupReferenceValueModel data = db.ReferenceValue
                .Where(p => p.DomainCode 
                == domainCode 
                && p.ValueCode == valueCode
                && p.StatusFlag == "A"
                && (p.ActiveFrom == null || DbFunctions.TruncateTime(p.ActiveFrom) <= DbFunctions.TruncateTime(now))
                && (p.ActiveTo == null || DbFunctions.TruncateTime(p.ActiveTo) >= DbFunctions.TruncateTime(now))).Select(p =>
            new LookupReferenceValueModel
            {
                Key = p.UID,
                Display = p.Description,
                DisplayOrder = p.DisplayOrder ?? 1,
                ValueCode = p.ValueCode,
                AlternateName = p.AlternateName
            }).FirstOrDefault();
            return data;
        }

        [Route("GetReferenceValueByDescription")]
        [HttpGet]
        public LookupReferenceValueModel GetReferenceValueByDescription(string domainCode, string description)
        {
            DateTime now = DateTime.Now;
            LookupReferenceValueModel data = db.ReferenceValue
            .Where(p => p.DomainCode == domainCode 
            && p.Description == description
            && p.StatusFlag == "A"
            && (p.ActiveFrom == null || DbFunctions.TruncateTime(p.ActiveFrom) <= DbFunctions.TruncateTime(now))
            && (p.ActiveTo == null || DbFunctions.TruncateTime(p.ActiveTo) >= DbFunctions.TruncateTime(now))
            ).Select(p =>
            new LookupReferenceValueModel
            {
                Key = p.UID,
                Display = p.Description,
                DisplayOrder = p.DisplayOrder ?? 1,
                ValueCode = p.ValueCode,
                AlternateName = p.AlternateName
            }).FirstOrDefault();
            return data;
        }

        [Route("GetReferenceValue")]
        [HttpGet]
        public LookupReferenceValueModel GetReferenceValue(int referencevalueUID)
        {
            ReferenceValue refValue = db.ReferenceValue.Find(referencevalueUID);
            LookupReferenceValueModel data = null;
            if (refValue != null)
            {
                data = new LookupReferenceValueModel();
                data.Key = refValue.UID;
                data.Display = refValue.Description;
                data.DisplayOrder = refValue.DisplayOrder ?? 1;
                data.ValueCode = refValue.ValueCode;
                data.AlternateName = refValue.AlternateName;
            }

            return data;
        }

        [Route("GetReferenceRealationShip")]
        [HttpGet]
        public List<ReferenceRelationShipModel> GetReferenceRealationShip(string sourceReferenceDomainCode, string targetReferenceDomainCode)
        {
            List<ReferenceRelationShip> refShip = db.ReferenceRelationShip.Where(p => p.SourceReferenceDomainCode == sourceReferenceDomainCode
                && p.TargetReferenceDomainCode == targetReferenceDomainCode && p.StatusFlag == "A").ToList();
            List<ReferenceRelationShipModel> data = null;
            if (refShip != null)
            {
                data = refShip.Select(p => new ReferenceRelationShipModel
                {
                    ReferenceRelationShipUID = p.UID,
                    SourceReferenceValueUID = p.SourceReferenceValueUID,
                    SourceReferenceDomainCode = p.SourceReferenceDomainCode,
                    SourceReferenceDomainUID = p.SourceReferenceDomainUID,
                    TargetReferenceValueUID = p.TargetReferenceValueUID,
                    TargetReferenceDomainCode = p.TargetReferenceDomainCode,
                    TargetReferenceDomainUID = p.TargetReferenceDomainUID,
                    CUser = p.CUser,
                    MUser = p.MUser,
                    CWhen = p.CWhen,
                    MWhen = p.MWhen
                }).ToList();
            }

            return data;
        }

        [Route("SaveReferenceDomain")]
        [HttpPost]
        public HttpResponseMessage SaveReferenceDomain(ReferenceDomainModel referenceDomainData, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                using (var tran = new TransactionScope())
                {
                    ReferenceDomain referenceDomain = db.ReferenceDomain.Find(referenceDomainData.UID);

                    if (referenceDomain == null)
                    {
                        referenceDomain = new ReferenceDomain();
                        referenceDomain.CUser = userID;
                        referenceDomain.CWhen = now;
                    }
                    referenceDomain.DomainCode = referenceDomainData.DomainCode;
                    referenceDomain.Description = referenceDomainData.Description;
                    referenceDomain.ActiveFrom = referenceDomainData.ActiveFrom;
                    referenceDomain.ActiveTo = referenceDomainData.ActiveTo;
                    referenceDomain.IsSortByDescription = referenceDomainData.IsSortByDescription;
                    referenceDomain.StatusFlag = "A";
                    referenceDomain.MUser = userID;
                    referenceDomain.MWhen = now;

                    db.ReferenceDomain.AddOrUpdate(referenceDomain);


                    #region DeleteReferevalue
                    List<ReferenceValue> referenceAll = db.ReferenceValue.Where(p => p.DomainCode == referenceDomainData.DomainCode && p.StatusFlag == "A").ToList();
                    if (referenceDomainData.ReferenceValues == null)
                    {
                        foreach (var item in referenceAll)
                        {
                            db.ReferenceValue.Attach(item);
                            item.MUser = userID;
                            item.MWhen = now;
                            item.StatusFlag = "D";
                        }
                    }
                    else
                    {

                        foreach (var item in referenceAll)
                        {
                            var data = referenceDomainData.ReferenceValues.FirstOrDefault(p => p.ReferenceValueUID == item.UID);
                            if (data == null)
                            {
                                db.ReferenceValue.Attach(item);
                                item.MUser = userID;
                                item.MWhen = now;
                                item.StatusFlag = "D";
                            }

                        }
                    }
                    #endregion

                    db.SaveChanges();

                    if (referenceDomainData.ReferenceValues != null)
                    {
                        foreach (var item in referenceDomainData.ReferenceValues)
                        {
                            ReferenceValue referenceValue = db.ReferenceValue.Find(item.ReferenceValueUID);



                            if (referenceValue == null)
                            {
                                referenceValue = new ReferenceValue();
                                referenceValue.CUser = userID;
                                referenceValue.CWhen = now;
                                referenceValue.ValueCode = item.ValueCode;
                                referenceValue.ReferenceDomainUID = referenceDomain.UID;
                                referenceValue.Description = item.Description;
                                referenceValue.DisplayOrder = item.DisplayOrder;
                                referenceValue.DomainCode = referenceDomainData.DomainCode;
                                referenceValue.SpecialityUID = item.SpecialityUID;
                                referenceValue.LANUGUID = item.LANUGUID;
                                referenceValue.DLFlag = item.DLFlag;
                                referenceValue.NumericValue = item.NumericValue;
                                referenceValue.AlternateName = item.AlternateName;
                                referenceValue.ActiveFrom = item.ActiveFrom;
                                referenceValue.ActiveTo = item.ActiveTo;
                                referenceValue.StatusFlag = "A";
                                referenceValue.MUser = userID;
                                referenceValue.MWhen = now;
                                db.ReferenceValue.Add(referenceValue);

                                db.SaveChanges();
                            }
                            else
                            {
                                if (item.IsUpdate == "Y")
                                {
                                    db.ReferenceValue.Attach(referenceValue);
                                    referenceValue.ValueCode = item.ValueCode;
                                    referenceValue.ReferenceDomainUID = referenceDomain.UID;
                                    referenceValue.Description = item.Description;
                                    referenceValue.DisplayOrder = item.DisplayOrder;
                                    referenceValue.DomainCode = referenceDomainData.DomainCode;
                                    referenceValue.SpecialityUID = item.SpecialityUID;
                                    referenceValue.LANUGUID = item.LANUGUID;
                                    referenceValue.DLFlag = item.DLFlag;
                                    referenceValue.NumericValue = item.NumericValue;
                                    referenceValue.AlternateName = item.AlternateName;
                                    referenceValue.ActiveFrom = item.ActiveFrom;
                                    referenceValue.ActiveTo = item.ActiveTo;
                                    referenceValue.StatusFlag = "A";
                                    referenceValue.MUser = userID;
                                    referenceValue.MWhen = now;

                                    db.SaveChanges();
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

        [Route("DeleteReferenceDomain")]
        [HttpDelete]
        public HttpResponseMessage DeleteReferenceDomain(int referenceDomainUID, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                ReferenceDomain data = db.ReferenceDomain.Find(referenceDomainUID);
                if (data != null)
                {
                    db.ReferenceDomain.Attach(data);
                    data.StatusFlag = "D";
                    data.MUser = userID;
                    data.MWhen = now;
                }

                List<ReferenceValue> refDataValue = db.ReferenceValue.Where(p => p.ReferenceDomainUID == referenceDomainUID).ToList();
                foreach (var item in refDataValue)
                {
                    db.ReferenceValue.Attach(item);
                    item.StatusFlag = "D";
                    item.MUser = userID;
                    item.MWhen = now;
                }

                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("GetProvine")]
        [HttpGet]
        public List<LookupItemModel> GetProvine()
        {
            List<LookupItemModel> data = db.Province.Where(p => p.StatusFlag == "A").OrderBy(p => p.ProvinceName)
             .Select(p => new LookupItemModel
             {
                 Key = p.UID,
                 Display = p.ProvinceName
             }).ToList();

            return data;
        }

        [Route("GetAmphurByPronvince")]
        [HttpGet]
        public List<LookupItemModel> GetAmphurByPronvince(int provinceUID)
        {
            List<LookupItemModel> data = db.Amphur.Where(p => p.StatusFlag == "A" && p.ProvinceUID == provinceUID).OrderBy(p => p.AmphurName)
           .Select(p => new LookupItemModel
           {
               Key = p.UID,
               Display = p.AmphurName
           }).ToList();

            return data;
        }

        [Route("GetDistrictByAmphur")]
        [HttpGet]
        public List<LookupReferenceValueModel> GetDistrictByAmphur(int amphurUID)
        {
            List<LookupReferenceValueModel> data = (from ds in db.District
                                                    join pos in db.PostalCode on ds.UID equals pos.DistrictUID
                                                    where ds.StatusFlag == "A"
                                                    && pos.StatusFlag == "A"
                                                    && ds.AmphurUID == amphurUID
                                                    select new LookupReferenceValueModel
                                                    {
                                                        Key = ds.UID,
                                                        Display = ds.DistrictName,
                                                        ValueCode = pos.PostalCodes
                                                    }).ToList();

            return data;
        }

        [Route("GetPostalCode")]
        [HttpGet]
        public List<MediTech.Model.PostalCode> GetPostalCode(string postalCode)
        {
            List<MediTech.Model.PostalCode> data = (from po in db.PostalCode
                                                    join dis in db.District on po.DistrictUID equals dis.UID
                                                    join amp in db.Amphur on po.AmphurUID equals amp.UID
                                                    join pro in db.Province on po.ProvinceUID equals pro.UID
                                                    where po.StatusFlag == "A"
                                                    && dis.StatusFlag == "A"
                                                    && amp.StatusFlag == "A"
                                                    && pro.StatusFlag == "A"
                                                    && po.PostalCodes == postalCode
                                                    select new MediTech.Model.PostalCode
                                                    {
                                                        Amphur = amp.AmphurName,
                                                        AmphurUID = amp.UID,
                                                        District = dis.DistrictName,
                                                        DistrictUID = dis.UID,
                                                        Province = pro.ProvinceName,
                                                        ProvinceUID = pro.UID,
                                                        ZipCode = po.PostalCodes
                                                    }).ToList();
            return data;
        }

        [Route("GetAccessionNumber")]
        [HttpGet]
        public string GetAccessionNumber(int OwnerOrganisationUID)
        {
            DateTime now = DateTime.Now;
            string accessionNumber = string.Empty;
            accessionNumber = now.ToString("yyyyMMddHHmm");

            HealthOrganisation healthOrganisationCode = db.HealthOrganisation.FirstOrDefault(p => p.UID == OwnerOrganisationUID);

            accessionNumber = (healthOrganisationCode != null ? healthOrganisationCode.Code : "00") + "-" + accessionNumber;

            return accessionNumber;
        }

        [Route("CheckAccessionNumberInPacs")]
        [HttpGet]
        public string CheckAccessionNumberInPacs(string accessionNumber)
        {
            string returnString = "";
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SonicDICOM"].ConnectionString;
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "select AccessionNumber from Studies where AccessionNumber = '" + accessionNumber + "'";
                dt.Load(cmd.ExecuteReader());
                if (dt != null && dt.Rows.Count > 0)
                {
                    returnString = dt.Rows[0]["AccessionNumber"].ToString();
                }
                else
                {
                    returnString = "Not Found";
                }
            }
            catch (Exception)
            {

                returnString = "Cannot Open Database Pacs";
            }
            finally
            {
                conn.Close();
            }
            return returnString;
        }

        [Route("GetMediTechInterfaceByCode")]
        [HttpGet]
        public List<MediTechInterfechModel> GetMediTechInterfaceByCode(string code)
        {
            List<MediTechInterfechModel> data = (from i in db.MediTechInterface
                                                join j in db.MediTechInterfaceDetail on i.UID equals j.MediTechInterfaceUID
                                                where i.StatusFlag == "A"
                                                && j.StatusFlag == "A"
                                                && i.Code == code
                                                select new MediTechInterfechModel {
                                                    MediTechInterfechUID = i.UID,
                                                    MediTechInterfechDetailUID = j.UID,
                                                    Code = i.Code,
                                                    Description = i.Description,
                                                    ParentUID = i.ParentUID,
                                                    ValueCode = j.ValueCode,
                                                    ValueDescription = j.Description,
                                                    Value = j.Value
                                                }).ToList();

            return data;
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
