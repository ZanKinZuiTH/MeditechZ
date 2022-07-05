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
    [RoutePrefix("Api/IPD")]
    public class IPDController : ApiController
    {
        protected MediTechEntities db = new MediTechEntities();

        [Route("GetBedALL")]
        [HttpGet]
        public List<LocationModel> GetbedALL()
        {
            var listData= db.Location.Where(p => p.StatusFlag == "A" ).Select(p => new LocationModel()
            {
                LocationUID = p.UID,
                Name = p.Name,
                Description = p.Description,
                LOTYPUID = p.LOTYPUID,
                LCTSTUID = p.LCTSTUID,
                ParentLocationUID = p.ParentLocationUID,
                ActiveFrom = p.ActiveFrom,
                ActiveTo = p.ActiveTo,
                CUser = p.CUser,
                CWhen = p.CWhen,
                MUser = p.MUser,
                EMRZONUID = p.EMZONEUID,
                OwnerOrganisationUID = p.OwnerOrganisationUID,
                IsTemporaryBed = p.IsTemporaryBed,
                MWhen = p.MWhen,
                StatusFlag = p.StatusFlag
            }).ToList();

            return listData;
        }



        [Route("GetLocationByBedUID")]
        [HttpPost]
        public HttpResponseMessage ChangeBedStatus(int BedUID, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;

                MediTech.DataBase.Location vendorDetail = db.Location.Where(p => p.UID == BedUID && p.StatusFlag == "A").FirstOrDefault();

                //if (CurrentModel == null)
                //{
                //    vendorDetail = new MediTech.DataBase.VendorDetail();
                //    vendorDetail.CUser = userID;
                //    vendorDetail.CWhen = now;
                //}

                //vendorDetail.LCTSTUID = locationModel.LCTSTUID;
                //vendorDetail.ActiveTo = locationModel.ActiveTo;
                vendorDetail.MUser = userID;
                vendorDetail.MWhen = now;
                vendorDetail.StatusFlag = "A";
                db.Location.AddOrUpdate(vendorDetail);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }




        [Route("ChangeBedStatus")]
        [HttpPost]
        public HttpResponseMessage ChangeBedStatus(LocationModel locationModel, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;

                MediTech.DataBase.Location vendorDetail = db.Location.Where(p => p.UID==locationModel.LocationUID && p.StatusFlag == "A").FirstOrDefault();

                //if (CurrentModel == null)
                //{
                //    vendorDetail = new MediTech.DataBase.VendorDetail();
                //    vendorDetail.CUser = userID;
                //    vendorDetail.CWhen = now;
                //}

                vendorDetail.LCTSTUID = locationModel.LCTSTUID;
                vendorDetail.ActiveTo = locationModel.ActiveTo;
                vendorDetail.MUser = userID;
                vendorDetail.MWhen = now;
                vendorDetail.StatusFlag = "A";
                db.Location.AddOrUpdate(vendorDetail);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }





    }

}