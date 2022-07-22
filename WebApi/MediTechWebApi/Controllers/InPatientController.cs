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
    [RoutePrefix("Api/InPatient")]
    public class InPatientController : ApiController
    {
        protected MediTechEntities db = new MediTechEntities();




        [Route("InsertPatientConsult")]
        [HttpPost]
        public HttpResponseMessage InsertPatientConsult(List<IPDConsultModel> sendmodels ,int userUID)
        {
            try
            {
                DateTime now = DateTime.Now;
                using (var tran = new TransactionScope())
                {

                    if ( sendmodels != null)
                    {
                        var deletedPatientConsult = sendmodels.Where(p => p.StatusFlag == "D");
                        var activedPatientConsult= sendmodels.Where(p => p.StatusFlag == "A");

                        foreach (var deletedItem in deletedPatientConsult)
                        {
                            PatientConsultation patientconsultation = db.PatientConsultation.Find(deletedItem.PatientConsultionUID);
                            if (patientconsultation != null)
                            {
                                db.PatientConsultation.Attach(patientconsultation);
                                patientconsultation.StatusFlag = "D";
                                patientconsultation.MUser = userUID;
                                patientconsultation.MWhen = now;

                                db.SaveChanges();
                            }
                        }

                        foreach (var inVisitConsult in activedPatientConsult)
                        {
                            PatientConsultation inpatientvisitconsult = db.PatientConsultation.Find(inVisitConsult.PatientConsultionUID);
                            if (inpatientvisitconsult == null)
                            {
                                inpatientvisitconsult = new PatientConsultation();
                                inpatientvisitconsult.CUser = userUID;
                                inpatientvisitconsult.CWhen = now;
                            }
                            inpatientvisitconsult.PatientUID = inVisitConsult.PatientUID;
                            inpatientvisitconsult.PatientVisitUID = inVisitConsult.PatientVisitUID??0;
                            inpatientvisitconsult.CareproviderUID = inVisitConsult.CareProviderUID??0;
                            inpatientvisitconsult.RecordedDttm = now;
                            inpatientvisitconsult.Comments = inVisitConsult.Note;

                            inpatientvisitconsult.MUser = userUID;
                            inpatientvisitconsult.MWhen = now;
                            inpatientvisitconsult.StatusFlag = "A";

                            inpatientvisitconsult.OwnerOrganisationUID = inVisitConsult.OwnerOrganisationUID??0;
                           
                            inpatientvisitconsult.CareProviderName = inVisitConsult.CareProviderName;
                            inpatientvisitconsult.VISTYUID = inVisitConsult.VISTSUID;
                            inpatientvisitconsult.StartDttm = inVisitConsult.StartConsultDate;
                            inpatientvisitconsult.EndDttm = inVisitConsult.EndConsultDate;
                            inpatientvisitconsult.CONSTSUID = inVisitConsult.CONSTSUID;
                            //inpatientvisitconsult.CONTYUID = inVisitConsult.CONTYPUID;





                            db.PatientConsultation.AddOrUpdate(inpatientvisitconsult);
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

        [Route("GetLocationByBedUID")]
        [HttpPost]
        public HttpResponseMessage test(int userID)
        {


            return Request.CreateResponse(HttpStatusCode.OK);
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

                MediTech.DataBase.Location vendorDetail = db.Location.Where(p => p.UID == locationModel.LocationUID && p.StatusFlag == "A").FirstOrDefault();

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

        [Route("GetPatientVisitConsult")]
        [HttpGet]
        public List<IPDConsultModel> GetPatientVisitConsult(int patientVisitUID)
        {
            List<IPDConsultModel> data = (from pvc in db.PatientConsultation
                                          where pvc.PatientVisitUID == patientVisitUID
                                          && pvc.StatusFlag == "A"
                                          select new IPDConsultModel
                                          {
                                              PatientConsultionUID = pvc.UID,
                                              PatientUID = pvc.PatientUID,
                                              PatientVisitUID = pvc.PatientVisitUID,
                                              PatientName = SqlFunction.fGetPatientName(pvc.PatientUID),
                                              CareProviderUID = pvc.CareproviderUID,
                                              CareProviderName = SqlFunction.fGetCareProviderName(pvc.CareproviderUID),
                                              //CONTYPUID = pvc.CONTYUID,
                                              CONSTSUID = pvc.CONSTSUID,
                                              ConultStatusStr = SqlFunction.fGetRfValDescription(pvc.CONSTSUID?? 0),
                                              //ConultTypeStr = SqlFunction.fGetRfValDescription(pvc.CONTYUID?? 0),
                                              StartConsultDate = pvc.StartDttm,
                                              EndConsultDate = pvc.EndDttm,
                                              OwnerOrganisationUID = pvc.OwnerOrganisationUID,
                                              VISTSUID = pvc.VISTYUID,
                                              Note = pvc.Comments,
                                              StatusFlag = pvc.StatusFlag
                                          }).ToList();
            return data;
        }
    }
}