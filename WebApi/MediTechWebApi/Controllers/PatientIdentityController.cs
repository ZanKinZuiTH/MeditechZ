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
    [RoutePrefix("Api/PatientIdentity")]
    public class PatientIdentityController : ApiController
    {
        protected MediTechEntities db = new MediTechEntities();

        #region Pateint
        [Route("GetPatientByHN")]
        [HttpGet]
        public PatientInformationModel GetPatientByHN(string HN)
        {
            PatientInformationModel data = (from pa in db.Patient
                                            join pdd in db.PatientAddress on
                                            new
                                            {
                                                key1 = pa.UID,
                                                key2 = 401, //DefaultAddress
                                                key3 = "A"
                                            }
                                            equals
                                            new
                                            {
                                                key1 = pdd.PatientUID,
                                                key2 = pdd.ADTYPUID ?? 0,
                                                key3 = pdd.StatusFlag
                                            }
                                            into joined
                                            from j in joined.DefaultIfEmpty()
                                            where pa.StatusFlag == "A"
                                            && pa.PatientID == HN
                                            select new PatientInformationModel
                                            {
                                                PatientUID = pa.UID,
                                                AgeString = SqlFunction.fGetAgeString(pa.DOBDttm.Value),
                                                BirthDttm = pa.DOBDttm.Value,
                                                DOBComputed = pa.DOBComputed,
                                                Email = pa.Email,
                                                FirstName = pa.FirstName,
                                                LastName = pa.LastName,
                                                PatientName = SqlFunction.fGetPatientName(pa.UID),
                                                MobilePhone = pa.MobilePhone,
                                                NationalID = pa.IDCard,
                                                NATNLUID = pa.NATNLUID,
                                                PatientID = pa.PatientID,
                                                EmployeeID = pa.EmployeeID,
                                                RELGNUID = pa.RELGNUID,
                                                SecondPhone = pa.SecondPhone,
                                                SEXXXUID = pa.SEXXXUID,
                                                TITLEUID = pa.TITLEUID,
                                                Title = SqlFunction.fGetRfValDescription(pa.TITLEUID ?? 0),
                                                LastVisitDttm = pa.LastVisitDttm,
                                                PatientAddressUID = j.UID,
                                                Line1 = j.Line1,
                                                Line2 = j.Line2,
                                                Line3 = j.Line3,
                                                AmphurUID = j.AmphurUID,
                                                DistrictUID = j.DistrictUID,
                                                ProvinceUID = j.ProvinceUID,
                                                ZipCode = j.ZipCode,
                                                UserUID = pa.CUser,
                                                IsVIP = pa.IsVIP ?? false,
                                                OwnerOrganisationUID = pa.OwnerOrganisationUID ?? 0
                                            }).FirstOrDefault();
            return data;
        }

        [Route("GetPatientByIDCard")]
        [HttpGet]
        public PatientInformationModel GetPatientByIDCard(string idCard)
        {
            PatientInformationModel data = (from pa in db.Patient
                                            join pdd in db.PatientAddress on
                                            new
                                            {
                                                key1 = pa.UID,
                                                key2 = 401, //DefaultAddress
                                                key3 = "A"
                                            }
                                            equals
                                            new
                                            {
                                                key1 = pdd.PatientUID,
                                                key2 = pdd.ADTYPUID ?? 0,
                                                key3 = pdd.StatusFlag
                                            }
                                            into joined
                                            from j in joined.DefaultIfEmpty()
                                            where pa.StatusFlag == "A"
                                            && pa.IDCard == idCard
                                            select new PatientInformationModel
                                            {
                                                PatientUID = pa.UID,
                                                AgeString = SqlFunction.fGetAgeString(pa.DOBDttm.Value),
                                                BirthDttm = pa.DOBDttm.Value,
                                                DOBComputed = pa.DOBComputed,
                                                Email = pa.Email,
                                                FirstName = pa.FirstName,
                                                LastName = pa.LastName,
                                                MobilePhone = pa.MobilePhone,
                                                NationalID = pa.IDCard,
                                                NATNLUID = pa.NATNLUID,
                                                PatientID = pa.PatientID,
                                                EmployeeID = pa.EmployeeID,
                                                RELGNUID = pa.RELGNUID,
                                                SecondPhone = pa.SecondPhone,
                                                SEXXXUID = pa.SEXXXUID,
                                                TITLEUID = pa.TITLEUID,
                                                LastVisitDttm = pa.LastVisitDttm,
                                                PatientAddressUID = j.UID,
                                                Line1 = j.Line1,
                                                Line2 = j.Line2,
                                                Line3 = j.Line3,
                                                AmphurUID = j.AmphurUID,
                                                DistrictUID = j.DistrictUID,
                                                ProvinceUID = j.ProvinceUID,
                                                ZipCode = j.ZipCode,
                                                UserUID = pa.CUser,
                                                IsVIP = pa.IsVIP ?? false,
                                                OwnerOrganisationUID = pa.OwnerOrganisationUID ?? 0
                                            }).FirstOrDefault();
            return data;
        }

        [Route("GetPatientByUID")]
        [HttpGet]
        public PatientInformationModel GetPatientByUID(long patientUID)
        {
            PatientInformationModel data = SqlDirectStore.pGetPatientByUID(patientUID).ToList<PatientInformationModel>().FirstOrDefault();
            return data;
        }

        [Route("GetPatientAddressByPatientUID")]
        [HttpGet]
        public List<PatientAddressModel> GetPatientAddressByPatientUID(long patientUID)
        {
            List<PatientAddressModel> patAdd = db.PatientAddress.Where(p => p.StatusFlag == "A" && p.PatientUID == patientUID)
                .Select(p => new PatientAddressModel
                {
                    PatientAddressUID = p.UID,
                    PatientUID = p.PatientUID,
                    Line1 = p.Line1,
                    Line2 = p.Line2,
                    Line3 = p.Line3,
                    Line4 = p.Line4,
                    DistrictUID = p.DistrictUID,
                    AmphurUID = p.AmphurUID,
                    ProvinceUID = p.ProvinceUID,
                    DistrictName = SqlFunction.fGetDistrictName(p.DistrictUID ?? 0),
                    AmphurName = SqlFunction.fGetAmphurName(p.AmphurUID ?? 0),
                    ProvinceName = SqlFunction.fGetProvinceName(p.ProvinceUID ?? 0),
                    ZipCode = p.ZipCode,
                    ADTYPUID = p.ADTYPUID,
                    Phone = p.Phone,
                    AddressType = SqlFunction.fGetRfValDescription(p.ADTYPUID ?? 0)
                }).ToList();

           return patAdd;
        }

        [Route("SearchPatient")]
        [HttpGet]
        public List<PatientInformationModel> SearchPatient(string patientID, string firstName, string middleName, string lastName, string nickName, DateTime? birthDate, int? SEXXXUID, string idCard, DateTime? lastVisitDate)
        {
            DataTable dataTable = SqlDirectStore.pSearchPatient(patientID, firstName, middleName, lastName, nickName, birthDate, SEXXXUID, idCard, lastVisitDate);

            List<PatientInformationModel> data = dataTable.ToList<PatientInformationModel>();
            //List<PatientInformationModel> data = (from pa in db.Patient
            //                                      join pdd in db.PatientAddress on
            //                                      new
            //                                      {
            //                                          key1 = pa.UID,
            //                                          key2 = SqlFunction.fGetRfValUIDByCode("ADTYP", "DEFADD"),
            //                                          key3 = "A"
            //                                      }
            //                                      equals
            //                                      new
            //                                      {
            //                                          key1 = pdd.PatientUID,
            //                                          key2 = pdd.ADTYPUID ?? 0,
            //                                          key3 = pdd.StatusFlag
            //                                      }
            //                                      into joined
            //                                      from j in joined.DefaultIfEmpty()
            //                                      where pa.StatusFlag == "A"
            //                                      && (pa.FirstName.Contains(firstName) || string.IsNullOrEmpty(firstName))
            //                                      && (pa.LastName.Contains(lastName) || string.IsNullOrEmpty(lastName))
            //                                      && (pa.IDCard == idCard || string.IsNullOrEmpty(idCard))
            //                                      && (pa.DOBDttm == birthDttm || birthDttm == null)
            //                                      select new PatientInformationModel
            //                                      {
            //                                          PatientUID = pa.UID,
            //                                          AgeString = pa.DOBDttm.Value != null ? SqlFunction.fGetAgeString(pa.DOBDttm.Value) : "",
            //                                          BirthDttm = pa.DOBDttm.Value,
            //                                          DOBComputed = pa.DOBComputed,
            //                                          Email = pa.Email,
            //                                          FirstName = pa.FirstName,
            //                                          LastName = pa.LastName,
            //                                          MobilePhone = pa.MobilePhon,
            //                                          NatinalID = pa.IDCard,
            //                                          NATNLUID = pa.NATNLUID,
            //                                          PatientID = pa.PatientID,
            //                                          RELGNUID = pa.RELGNUID,
            //                                          SecondPhone = pa.SecondPhone,
            //                                          SEXXXUID = pa.SEXXXUID,
            //                                          TITLEUID = pa.TITLEUID,
            //                                          Address = j.Address,
            //                                          AmphurUID = j.AmphurUID,
            //                                          DistrictUID = j.DistrictUID,
            //                                          ProvinceUID = j.ProvinceUID,
            //                                          ZipCode = j.ZipCode,
            //                                      }).Take(50).ToList();

            return data;
        }

        [Route("RegisterPatient")]
        [HttpPost]
        public HttpResponseMessage RegisterPatient(PatientInformationModel patientInfo, int userID, int OwnerOrganisationUID)
        {
            try
            {
                DateTime now = DateTime.Now;
                using (var tran = new TransactionScope())
                {
                    Patient patient = new Patient();
                    Patient patientData = db.Patient.Find(patientInfo.PatientUID);
                    if (patientData == null)
                    {
                        int seqUID;
                        string patientID = "";

                        if (String.IsNullOrEmpty(patientInfo.PatientID))
                        {
                            patientID = SEQHelper.GetSEQIDFormat("SEQPatientID", out seqUID);
                            if (string.IsNullOrEmpty(patientID))
                            {
                                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No SEQPatientID in SEQCONFIGURATION");
                            }

                            if (seqUID == 0)
                            {
                                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Insert SEQPatientID is Fail");
                            }


                            patientInfo.PatientID = patientID;
                        }
                        else
                        {
                            patientInfo.PatientID = patientInfo.PatientID;
                        }


                        patient.CUser = userID;
                        patient.CWhen = now;
                        patient.OwnerOrganisationUID = OwnerOrganisationUID;
                    }
                    else
                    {
                        patient.UID = patientData.UID;
                        patient.CUser = patientData.CUser;
                        patient.CWhen = patientData.CWhen;
                    }


                    patient.FirstName = patientInfo.FirstName;
                    patient.MiddleName = patientInfo.MiddelName;
                    patient.LastName = patientInfo.LastName;
                    patient.NickName = patientInfo.NickName;
                    patient.SEXXXUID = patientInfo.SEXXXUID;
                    patient.TITLEUID = patientInfo.TITLEUID;
                    patient.DOBDttm = patientInfo.BirthDttm;
                    patient.DOBComputed = patientInfo.DOBComputed;
                    patient.BLOODUID = patientInfo.BLOODUID;
                    patient.IsVIP = patientInfo.IsVIP;
                    patient.IDCard = patientInfo.NationalID;
                    patient.MobilePhone = patientInfo.MobilePhone;
                    patient.SecondPhone = patientInfo.SecondPhone;
                    patient.Email = patientInfo.Email;
                    patient.IDLine = patientInfo.IDLine;
                    patient.SPOKLUID = patientInfo.SPOKLUID;
                    patient.NATNLUID = patientInfo.NATNLUID;
                    patient.MARRYUID = patientInfo.MARRYUID;
                    patient.RELGNUID = patientInfo.RELGNUID;
                    patient.OCCUPUID = patientInfo.OCCUPUID;
                    patient.PatientID = patientInfo.PatientID;
                    patient.EmployeeID = patientInfo.EmployeeID;
                    patient.Department = patientInfo.Department;
                    patient.Position = patientInfo.Position;

                    patient.MUser = userID;
                    patient.MWhen = now;
                    patient.StatusFlag = "A";
                    patient.IsVIP = patientInfo.IsVIP;


                    //PatientDemolog
                    Dictionary<string, List<string>> patientModifiedInfo = new Dictionary<string, List<string>>();
                    patientModifiedInfo = GenerateAuditLogMessages(patientData, patient);

                    if (patientModifiedInfo != null && patientModifiedInfo.Count > 0)
                    {
                        foreach (KeyValuePair<string, List<string>> val in patientModifiedInfo)
                        {
                            if (!val.Key.Contains("MWhen") && !val.Key.Contains("MUser") && !val.Key.Contains("LastVisitDttm"))
                            {
                                PatientDemographicLog patLog = new PatientDemographicLog();
                                patLog.PatientUID = patient.UID;
                                patLog.FiledName = val.Key;
                                patLog.OldValue = val.Value.First();
                                patLog.NewValue = val.Value.Last();
                                patLog.Modifiedby = userID;
                                patLog.ModifiedDttm = now;
                                patLog.StatusFlag = "A";
                                patLog.CUser = userID;
                                patLog.CWhen = now;
                                patLog.MUser = userID;
                                patLog.MWhen = now;
                                db.PatientDemographicLog.Add(patLog);
                            }

                        }
                    }

                    db.Patient.AddOrUpdate(patient);
                    db.SaveChanges();
                    if (patientInfo.PatientUID == 0)
                    {
                        patientInfo.PatientUID = patient.UID;
                    }

                    VIPPatient vipPat = db.VIPPatient.FirstOrDefault(p => p.PatientUID == patient.UID && p.StatusFlag == "A");
                    if (patient.IsVIP ?? false)
                    {
                        if (vipPat == null)
                        {
                            vipPat = new VIPPatient();
                            vipPat.CUser = userID;
                            vipPat.CWhen = now;
                            vipPat.StatusFlag = "A";
                        }
                        vipPat.PatientUID = patient.UID;
                        vipPat.VIPTPUID = patientInfo.VIPTPUID.HasValue ? patientInfo.VIPTPUID.Value : 0;
                        vipPat.ActiveFrom = patientInfo.VIPActiveFrom;
                        vipPat.ActiveTo = patientInfo.VIPActiveTo;
                        vipPat.MUser = userID;
                        vipPat.MWhen = now;

                        db.VIPPatient.AddOrUpdate(vipPat);
                    }
                    else
                    {
                        if (vipPat != null)
                        {
                            db.VIPPatient.Attach(vipPat);
                            vipPat.MUser = userID;
                            vipPat.MWhen = now;
                            vipPat.StatusFlag = "D";
                        }
                    }

                    db.SaveChanges();

                    if (!string.IsNullOrEmpty(patientInfo.Line1) || !string.IsNullOrEmpty(patientInfo.Line2)
                        || !string.IsNullOrEmpty(patientInfo.Line3) ||
                        patientInfo.ProvinceUID != null)
                    {

                        PatientAddress patientAddress = db.PatientAddress.Find(patientInfo.PatientAddressUID);

                        if (patientAddress == null)
                        {
                            patientAddress = new PatientAddress();
                            patientAddress.CUser = userID;
                            patientAddress.CWhen = now;
                        }


                        patientAddress.PatientUID = patient.UID;
                        patientAddress.Line1 = patientInfo.Line1;
                        patientAddress.Line2 = patientInfo.Line2;
                        patientAddress.Line3 = patientInfo.Line3;
                        patientAddress.ADTYPUID = db.ReferenceValue.FirstOrDefault(p => p.DomainCode == "ADTYP" && p.ValueCode == "DEFADD").UID;
                        patientAddress.DistrictUID = patientInfo.DistrictUID;
                        patientAddress.AmphurUID = patientInfo.AmphurUID;
                        patientAddress.ProvinceUID = patientInfo.ProvinceUID;
                        patientAddress.ZipCode = patientInfo.ZipCode;
                        patientAddress.MUser = userID;
                        patientAddress.MWhen = now;
                        patientAddress.StatusFlag = "A";

                        db.PatientAddress.AddOrUpdate(patientAddress);
                        db.SaveChanges();
                    }

                    if (patientInfo.PatientImage != null)
                    {
                        PatientImage patientImage = db.PatientImage.FirstOrDefault(p => p.PatientUID == patient.UID);
                        if (patientImage == null)
                        {
                            patientImage = new PatientImage();
                            patientImage.CUser = userID;
                            patientImage.CWhen = now;
                        }
                        patientImage.ImageContent = patientInfo.PatientImage;
                        patientImage.PatientUID = patient.UID;
                        patientImage.MUser = userID;
                        patientImage.MWhen = now;
                        patientImage.StatusFlag = "A";
                        db.PatientImage.AddOrUpdate(patientImage);
                        db.SaveChanges();
                    }
                    else
                    {
                        PatientImage patientImage = db.PatientImage.FirstOrDefault(p => p.PatientUID == patient.UID);
                        if (patientImage != null)
                        {
                            db.PatientImage.Remove(patientImage);
                            db.SaveChanges();
                        }
                    }


                    tran.Complete();
                }

                return Request.CreateResponse(HttpStatusCode.OK, patientInfo);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }


        [Route("CheckDupicatePatient")]
        [HttpGet]
        public PatientInformationModel CheckDupicatePatientByIDCard(string firstName, string lastName, DateTime? birthDate, int SEXXXUID)
        {
            PatientInformationModel data = SqlDirectStore.pCheckDupicatePatient(firstName, lastName, birthDate, SEXXXUID).ToList<PatientInformationModel>().FirstOrDefault();
            return data;
        }

        #endregion

        #region PatientVisit

        [Route("SearchPatientVisit")]
        [HttpGet]
        public List<PatientVisitModel> SearchPatientVisit(string hn, string firstName, string lastName, int? careproviderUID
            , string statusList, DateTime? dateFrom, DateTime? dateTo, DateTime? arrivedDttm, int? ownerOrganisationUID
            , int? payorDetailUID, int? checkupJobUID)
        {
            DataTable dataTable = SqlDirectStore.pSearchPatientVisit(hn, firstName, lastName, careproviderUID, statusList, dateFrom, dateTo, arrivedDttm, ownerOrganisationUID, payorDetailUID, checkupJobUID);

            List<PatientVisitModel> data = dataTable.ToList<PatientVisitModel>();

            return data;
        }


        [Route("SearchPatientMedicalDischarge")]
        [HttpGet]
        public List<PatientVisitModel> SearchPatientMedicalDischarge(string hn, string firstName, string lastName, int? careproviderUID,
            DateTime? dateFrom, DateTime? dateTo, int? ownerOrganisationUID, int? payorDetailUID)
        {
            DataTable dataTable = SqlDirectStore.pSearchPatientMedicalDischarge(hn, firstName, lastName, careproviderUID, dateFrom, dateTo, ownerOrganisationUID, payorDetailUID);

            List<PatientVisitModel> data = dataTable.ToList<PatientVisitModel>();

            return data;
        }

        [Route("SavePatientVisit")]
        [HttpPost]
        public HttpResponseMessage SavePatientVisit(PatientVisitModel patientVisitInfo, int userID)
        {
            try
            {
                using (var tran = new TransactionScope())
                {
                    DateTime now = DateTime.Now;

                    int outseqvisitUID;
                    string seqVisitID = SEQHelper.GetSEQIDFormat("SEQVisitID", out outseqvisitUID);

                    if (string.IsNullOrEmpty(seqVisitID))
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No SEQVisitID in SEQCONFIGURATION");
                    }

                    PatientVisit patientVisit = new PatientVisit();
                    patientVisit.PatientUID = patientVisitInfo.PatientUID;
                    patientVisit.VISTYUID = patientVisitInfo.VISTYUID;
                    patientVisit.VISTSUID = patientVisitInfo.VISTSUID;
                    patientVisit.PRITYUID = patientVisitInfo.PRITYUID;
                    patientVisit.CheckupJobUID = patientVisitInfo.CheckupJobUID;
                    patientVisit.RefNo = patientVisitInfo.RefNo;
                    patientVisit.CompanyName = patientVisitInfo.CompanyName;
                    patientVisit.VisitID = seqVisitID;
                    patientVisit.CareProviderUID = patientVisitInfo.CareProviderUID;
                    patientVisit.BookingUID = patientVisitInfo.BookingUID;
                    patientVisit.StartDttm = patientVisitInfo.StartDttm;
                    patientVisit.ArrivedDttm = patientVisitInfo.StartDttm;
                    patientVisit.CUser = userID;
                    patientVisit.CWhen = now;
                    patientVisit.MUser = userID;
                    patientVisit.MWhen = now;
                    patientVisit.StatusFlag = "A";
                    patientVisit.OwnerOrganisationUID = patientVisitInfo.OwnerOrganisationUID;

                    db.PatientVisit.Add(patientVisit);
                    db.SaveChanges();

                    #region PatientServiceEvent
                    PatientServiceEvent serviceEvent = new PatientServiceEvent();
                    serviceEvent.PatientVisitUID = patientVisit.UID;
                    serviceEvent.EventStartDttm = now;
                    serviceEvent.VISTSUID = patientVisit.VISTSUID ?? 0;
                    serviceEvent.MUser = userID;
                    serviceEvent.MWhen = now;
                    serviceEvent.CUser = userID;
                    serviceEvent.CWhen = now;
                    serviceEvent.StatusFlag = "A";

                    db.PatientServiceEvent.Add(serviceEvent);
                    #endregion

                    db.SaveChanges();

                    PatientVisitPayor visitPayor = new PatientVisitPayor();
                    visitPayor.PatientUID = patientVisitInfo.PatientUID;
                    visitPayor.PatientVisitUID = patientVisit.UID;
                    visitPayor.PayorDetailUID = patientVisitInfo.PayorDetailUID;
                    visitPayor.PayorAgreementUID = patientVisitInfo.PayorAgreementUID;
                    visitPayor.CUser = userID;
                    visitPayor.CWhen = now;
                    visitPayor.MUser = userID;
                    visitPayor.MWhen = now;
                    visitPayor.StatusFlag = "A";

                    db.PatientVisitPayor.Add(visitPayor);
                    db.SaveChanges();



                    #region Patient

                    Patient patient = db.Patient.Find(patientVisit.PatientUID);
                    if (patient != null)
                    {
                        db.Patient.Attach(patient);
                        patient.LastVisitDttm = patientVisit.StartDttm;
                        patient.MUser = userID;
                        patient.MWhen = DateTime.Now;
                        db.SaveChanges();
                    }
                    #endregion

                    patientVisitInfo.VisitID = seqVisitID;
                    patientVisitInfo.PatientVisitUID = patientVisit.UID;
                    tran.Complete();
                }
                return Request.CreateResponse(HttpStatusCode.OK, patientVisitInfo);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("ModifyPatientVisit")]
        [HttpPut]
        public HttpResponseMessage ModifyPatientVisit(PatientVisitModel patientVisitInfo, int userID)
        {
            try
            {
                PatientVisit patientVisit = db.PatientVisit.Find(patientVisitInfo.PatientVisitUID);
                if (patientVisit != null)
                {
                    db.PatientVisit.Attach(patientVisit);
                    patientVisit.OwnerOrganisationUID = patientVisitInfo.OwnerOrganisationUID;
                    patientVisit.VISTYUID = patientVisitInfo.VISTYUID;
                    patientVisit.StartDttm = patientVisitInfo.StartDttm;
                    patientVisit.PRITYUID = patientVisitInfo.PRITYUID;
                    patientVisit.CareProviderUID = patientVisitInfo.CareProviderUID;
                    patientVisit.CheckupJobUID = patientVisitInfo.CheckupJobUID;
                    patientVisit.Comments = patientVisitInfo.Comments;
                    patientVisit.MUser = userID;
                    patientVisit.MWhen = DateTime.Now;

                    PatientVisitPayor patientVisitPayor = db.PatientVisitPayor.FirstOrDefault(p => p.PatientVisitUID == patientVisit.UID && p.StatusFlag == "A");
                    if (patientVisitPayor != null &&
                        (patientVisitPayor.UID != patientVisitInfo.PayorDetailUID
                        || patientVisitPayor.PayorAgreementUID != patientVisitInfo.PayorAgreementUID))
                    {
                        db.PatientVisitPayor.Attach(patientVisitPayor);
                        patientVisitPayor.PayorDetailUID = patientVisitInfo.PayorDetailUID;
                        patientVisitPayor.PayorAgreementUID = patientVisitInfo.PayorAgreementUID;
                        patientVisitPayor.MUser = userID;
                        patientVisitPayor.MWhen = DateTime.Now;
                    }
                }

                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, patientVisitInfo);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("CancelVisit")]
        [HttpPut]
        public HttpResponseMessage CancelVisit(long patientVisitUID, int userID)
        {
            try
            {
                PatientVisit patientVisit = db.PatientVisit.Find(patientVisitUID);
                if (patientVisit != null)
                {
                    db.PatientVisit.Attach(patientVisit);
                    patientVisit.VISTSUID = 410; //Cancelled
                    patientVisit.MUser = userID;
                    patientVisit.MWhen = DateTime.Now;
                    patientVisit.StatusFlag = "C";

                    db.SaveChanges();
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("GetPatientVisitByUID")]
        [HttpGet]
        public PatientVisitModel GetPatientVisitByUID(long patientVisitUID)
        {
            PatientVisitModel visitData = null;
            visitData = db.PatientVisit.Where(p => p.UID == patientVisitUID && p.StatusFlag == "A")
                        .Select(p => new PatientVisitModel
                        {
                            PatientUID = p.PatientUID,
                            PatientVisitUID = p.UID,
                            StartDttm = p.StartDttm,
                            EndDttm = p.EndDttm,
                            ArrivedDttm = p.ArrivedDttm,
                            CareProviderUID = p.CareProviderUID,
                            Comments = p.Comments,
                            VISTYUID = p.VISTYUID,
                            VISTSUID = p.VISTSUID,
                            IsBillFinalized = p.IsBillFinalized,
                            VisitStatus = SqlFunction.fGetRfValDescription(p.VISTSUID ?? 0),
                            VisitType = SqlFunction.fGetRfValDescription(p.VISTYUID ?? 0),
                            VisitID = p.VisitID,
                            PRITYUID = p.PRITYUID,
                            OwnerOrganisationUID = p.OwnerOrganisationUID ?? 0
                        }).FirstOrDefault();

            return visitData;
        }

        [Route("GetPatientVisitByPatientUID")]
        [HttpGet]
        public List<PatientVisitModel> GetPatientVisitByPatientUID(long patientUID)
        {
            List<PatientVisitModel> visitData = null;
            visitData = (from pv in db.PatientVisit
                         join pvp in db.PatientVisitPayor on pv.UID equals pvp.PatientVisitUID
                         where pv.StatusFlag == "A"
                         && pvp.StatusFlag == "A"
                         && pv.PatientUID == patientUID
                         && pv.VISTSUID != 410
                         select new PatientVisitModel
                         {
                             PatientUID = pv.PatientUID,
                             PatientVisitUID = pv.UID,
                             StartDttm = pv.StartDttm,
                             EndDttm = pv.EndDttm,
                             ArrivedDttm = pv.ArrivedDttm,
                             CareProviderUID = pv.CareProviderUID,
                             Comments = pv.Comments,
                             VISTYUID = pv.VISTYUID,
                             VISTSUID = pv.VISTSUID,
                             IsBillFinalized = pv.IsBillFinalized,
                             VisitStatus = SqlFunction.fGetRfValDescription(pv.VISTYUID ?? 0),
                             VisitType = SqlFunction.fGetRfValDescription(pv.VISTYUID ?? 0),
                             OwnerOrganisation = SqlFunction.fGetHealthOrganisationName(pv.OwnerOrganisationUID ?? 0),
                             VisitID = pv.VisitID,
                             PRITYUID = pv.PRITYUID,
                             PayorDetailUID = pvp.PayorDetailUID,
                             OwnerOrganisationUID = pv.OwnerOrganisationUID ?? 0,
                         }).ToList();

            return visitData;
        }

        [Route("GetPatientVisitByVisitType")]
        [HttpGet]
        public List<PatientVisitModel> GetPatientVisitByVisitType(long patientUID, string visitType)
        {
            ReferenceValue refve = db.ReferenceValue.FirstOrDefault(p => p.DomainCode == "VISTY" && p.Description == visitType && p.StatusFlag == "A");
            List<PatientVisitModel> visitData = null;
            if (refve != null)
            {
                visitData = new List<PatientVisitModel>();
                visitData = db.PatientVisit.Where(p => p.PatientUID == patientUID && p.VISTSUID != 410 && p.VISTYUID == refve.UID && p.StatusFlag == "A" && p.EndDttm == null)
                    .Select(p => new PatientVisitModel
                    {
                        PatientUID = p.PatientUID,
                        PatientVisitUID = p.UID,
                        ArrivedDttm = p.ArrivedDttm,
                        CareProviderUID = p.CareProviderUID,
                        Comments = p.Comments,
                        VISTYUID = p.VISTYUID,
                        VISTSUID = p.VISTSUID,
                        StartDttm = p.StartDttm,
                        IsBillFinalized = p.IsBillFinalized,
                        VisitStatus = SqlFunction.fGetRfValDescription(p.VISTYUID ?? 0),
                        VisitType = SqlFunction.fGetRfValDescription(p.VISTYUID ?? 0),
                        VisitID = p.VisitID,
                        PRITYUID = p.PRITYUID,
                        OwnerOrganisationUID = p.OwnerOrganisationUID ?? 0
                    }).ToList();

            }

            return visitData;
        }


        [Route("GetLatestPatientVisit")]
        [HttpGet]
        public PatientVisitModel GetLatestPatientVisit(long patientUID)
        {
            PatientVisitModel visitData = null;
            visitData = db.PatientVisit.Where(p => p.PatientUID == patientUID && p.VISTSUID != 410 && p.StatusFlag == "A")
                        .Select(p => new PatientVisitModel
                        {
                            PatientUID = p.PatientUID,
                            PatientVisitUID = p.UID,
                            StartDttm = p.StartDttm,
                            EndDttm = p.EndDttm,
                            ArrivedDttm = p.ArrivedDttm,
                            CareProviderUID = p.CareProviderUID,
                            Comments = p.Comments,
                            VISTYUID = p.VISTYUID,
                            VISTSUID = p.VISTSUID,
                            IsBillFinalized = p.IsBillFinalized,
                            VisitStatus = SqlFunction.fGetRfValDescription(p.VISTYUID ?? 0),
                            VisitType = SqlFunction.fGetRfValDescription(p.VISTYUID ?? 0),
                            OwnerOrganisation = SqlFunction.fGetHealthOrganisationName(p.OwnerOrganisationUID ?? 0),
                            VisitID = p.VisitID,
                            PRITYUID = p.PRITYUID,
                            CWhen = p.CWhen,
                            OwnerOrganisationUID = p.OwnerOrganisationUID ?? 0
                        }).OrderByDescending(p => p.StartDttm).FirstOrDefault();

            return visitData;
        }

        [Route("ChangeVisitStatus")]
        [HttpPut]
        public HttpResponseMessage ChangeVisitStatus(long patientVisitUID, int VISTSUID, int? careProviderUID, DateTime? editDttm, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                PatientVisit patientvisit = db.PatientVisit.Find(patientVisitUID);
                if (patientvisit != null)
                {
                    db.PatientVisit.Attach(patientvisit);

                    patientvisit.CareProviderUID = careProviderUID;
                    patientvisit.VISTSUID = VISTSUID;

                    patientvisit.MUser = userID;
                    patientvisit.MWhen = now;

                    #region PatientServiceEvent
                    PatientServiceEvent serviceEvent = new PatientServiceEvent();
                    serviceEvent.PatientVisitUID = patientvisit.UID;
                    serviceEvent.EventStartDttm = editDttm ?? patientvisit.MWhen;
                    serviceEvent.VISTSUID = VISTSUID;
                    serviceEvent.MUser = userID;
                    serviceEvent.MWhen = now;
                    serviceEvent.CUser = userID;
                    serviceEvent.CWhen = now;
                    serviceEvent.StatusFlag = "A";

                    db.PatientServiceEvent.Add(serviceEvent);
                    #endregion



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

        #region PatientBanner
        [Route("GetPatientDataForBanner")]
        [HttpGet]
        public PatientBannerModel GetPatientDataForBanner(long patientUID, long patientVisitUID)
        {
            PatientBannerModel data = SqlDirectStore.pGetPatientDataForBanner(patientUID, patientVisitUID).ToList<PatientBannerModel>().FirstOrDefault();
            return data;
        }

        #endregion

        #region PatientAllergy

        [Route("GetPatientAllergyByPatientUID")]
        [HttpGet]
        public List<PatientAllergyModel> GetPatientAllergyByPatientUID(long patientUID)
        {
            List<PatientAllergyModel> data = db.PatientAllergy.Where(p => p.StatusFlag == "A"
                && p.PatientUID == patientUID).Select(p => new PatientAllergyModel
                {
                    PatientAllergyUID = p.UID,
                    PatientUID = p.PatientUID,
                    PatientVisitUID = p.PatientVisitUID,
                    CERNTUID = p.CERNTUID,
                    Certanity = SqlFunction.fGetRfValDescription(p.CERNTUID ?? 0),
                    SEVRTUID = p.SEVRTUID,
                    Severity = SqlFunction.fGetRfValDescription(p.SEVRTUID ?? 0),
                    AllergyDescription = p.AllergyDescription,
                    ALRCLUID = p.ALRCLUID,
                    AllergyClass = SqlFunction.fGetRfValDescription(p.ALRCLUID ?? 0),
                    AllergicTo = p.AllergicTo,
                    IdentifyingUID = p.IdentifyingUID,
                    IdentifyingType = p.IdentifyingType,
                    RecordByName = SqlFunction.fGetCareProviderName(p.CUser),
                    CUser = p.CUser,
                    CWhen = p.CWhen
                }).ToList();
            return data;
        }

        [Route("ManagePatientAllergy")]
        [HttpPost]
        public HttpResponseMessage ManagePatientAllergy(long patientUID, List<PatientAllergyModel> model, int userUID)
        {
            try
            {
                DateTime now = DateTime.Now;
                using (var tran = new TransactionScope())
                {
                    List<PatientAllergy> oldAllergy = db.PatientAllergy.Where(p => p.PatientUID == patientUID).ToList();
                    foreach (var item in oldAllergy)
                    {
                        if (model.FirstOrDefault(p => p.PatientAllergyUID == item.UID) == null)
                        {
                            db.PatientAllergy.Attach(item);
                            item.StatusFlag = "D";
                            item.MWhen = now;
                            item.MUser = userUID;
                            db.SaveChanges();
                        }
                    }

                    foreach (var item in model)
                    {
                        PatientAllergy patAllergy = db.PatientAllergy.Find(item.PatientAllergyUID);
                        if (patAllergy == null)
                        {
                            patAllergy = new PatientAllergy();
                            patAllergy.CUser = userUID;
                            patAllergy.CWhen = now;
                        }

                        patAllergy.MUser = userUID;
                        patAllergy.MWhen = now;
                        patAllergy.StatusFlag = "A";
                        patAllergy.PatientUID = item.PatientUID;
                        patAllergy.PatientVisitUID = item.PatientVisitUID;
                        patAllergy.ALRCLUID = item.ALRCLUID;
                        patAllergy.AllergyDescription = item.AllergyDescription;
                        patAllergy.AllergicTo = item.AllergicTo;
                        patAllergy.IdentifyingUID = item.IdentifyingUID;
                        patAllergy.IdentifyingType = item.IdentifyingType;
                        patAllergy.SEVRTUID = item.SEVRTUID;
                        patAllergy.CERNTUID = item.CERNTUID;

                        db.PatientAllergy.AddOrUpdate(patAllergy);
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

        [Route("DeletePatientAllergy")]
        [HttpDelete]
        public HttpResponseMessage DeletePatientAllergy(int patientAllergyUID, int userUID)
        {
            try
            {
                PatientAllergy prob = db.PatientAllergy.Find(patientAllergyUID);
                if (prob != null)
                {
                    db.PatientAllergy.Attach(prob);
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

        #endregion

        #region Booking

        [Route("SearchBooking")]
        [HttpGet]
        public List<BookingModel> SearchBooking(DateTime? dateFrom, DateTime? dateTo, int? careproviderUID
            , long? patientUID, int? bookStatus, int? PATMSGUID, int? ownerOrganisationUID)
        {
            List<BookingModel> data = (from bki in db.Booking
                                       join pa in db.Patient on bki.PatientUID equals pa.UID
                                       where bki.StatusFlag == "A"
                                          && (dateFrom == null || DbFunctions.TruncateTime(bki.AppointmentDttm) >= DbFunctions.TruncateTime(dateFrom))
                                          && (dateTo == null || DbFunctions.TruncateTime(bki.AppointmentDttm) <= DbFunctions.TruncateTime(dateTo))
                                          && (careproviderUID == null || bki.CareProviderUID == careproviderUID)
                                          && (patientUID == null || bki.PatientUID == patientUID)
                                          && (bookStatus == null || bki.BKSTSUID == bookStatus)
                                          && (PATMSGUID == null || bki.PATMSGUID == PATMSGUID)
                                          && (ownerOrganisationUID == null || bki.OwnerOrganisationUID == ownerOrganisationUID)
                                       select new BookingModel
                                       {
                                           BookingUID = bki.UID,
                                           PatientUID = bki.PatientUID,
                                           PatientName = SqlFunction.fGetPatientName(bki.PatientUID),
                                           Phone = !string.IsNullOrEmpty(pa.MobilePhone) ? pa.MobilePhone : pa.SecondPhone,
                                           AppointmentDttm = bki.AppointmentDttm,
                                           Comments = bki.Comments,
                                           CareProviderUID = bki.CareProviderUID,
                                           CareProviderName = SqlFunction.fGetCareProviderName(bki.CareProviderUID ?? 0),
                                           BKSTSUID = bki.BKSTSUID,
                                           BookingStatus = SqlFunction.fGetRfValDescription(bki.BKSTSUID),
                                           OwnerOrganisationUID = bki.OwnerOrganisationUID,
                                           PATMSGUID = bki.PATMSGUID,
                                           PatientReminderMessage = SqlFunction.fGetRfValDescription(bki.PATMSGUID ?? 0),
                                       }).ToList();


            return data;
        }


        [Route("SearchBookingNotExistsVisit")]
        [HttpGet]
        public List<BookingModel> SearchBookingNotExistsVisit(DateTime? dateFrom, DateTime? dateTo, int? careproviderUID
    , long? patientUID, int? bookStatus, int? PATMSGUID, int? ownerOrganisationUID)
        {
            List<BookingModel> dataNonExistsVisit = new List<BookingModel>();
            List<BookingModel> dataBooking = (from bki in db.Booking
                                              join pa in db.Patient on bki.PatientUID equals pa.UID
                                              where bki.StatusFlag == "A"
                                                 && (dateFrom == null || DbFunctions.TruncateTime(bki.AppointmentDttm) >= DbFunctions.TruncateTime(dateFrom))
                                                 && (dateTo == null || DbFunctions.TruncateTime(bki.AppointmentDttm) <= DbFunctions.TruncateTime(dateTo))
                                                 && (careproviderUID == null || bki.CareProviderUID == careproviderUID)
                                                 && (patientUID == null || bki.PatientUID == patientUID)
                                                 && (bookStatus == null || bki.BKSTSUID == bookStatus)
                                                 && (PATMSGUID == null || bki.PATMSGUID == PATMSGUID)
                                                 && (ownerOrganisationUID == null || bki.OwnerOrganisationUID == ownerOrganisationUID)
                                              select new BookingModel
                                              {
                                                  BookingUID = bki.UID,
                                                  PatientUID = bki.PatientUID,
                                                  PatientName = SqlFunction.fGetPatientName(bki.PatientUID),
                                                  Phone = !string.IsNullOrEmpty(pa.MobilePhone) ? pa.MobilePhone : pa.SecondPhone,
                                                  AppointmentDttm = bki.AppointmentDttm,
                                                  Comments = bki.Comments,
                                                  CareProviderUID = bki.CareProviderUID,
                                                  CareProviderName = SqlFunction.fGetCareProviderName(bki.CareProviderUID ?? 0),
                                                  BKSTSUID = bki.BKSTSUID,
                                                  BookingStatus = SqlFunction.fGetRfValDescription(bki.BKSTSUID),
                                                  OwnerOrganisationUID = bki.OwnerOrganisationUID,
                                                  PATMSGUID = bki.PATMSGUID,
                                                  PatientReminderMessage = SqlFunction.fGetRfValDescription(bki.PATMSGUID ?? 0),
                                              }).ToList();

            if (dataBooking != null)
            {
                foreach (var booking in dataBooking)
                {
                    PatientVisit visitbooking = db.PatientVisit.FirstOrDefault(p => p.BookingUID == booking.BookingUID && p.StatusFlag == "A");
                    if (visitbooking == null)
                    {
                        dataNonExistsVisit.Add(booking);
                    }
                }
            }


            return dataNonExistsVisit;
        }

        [Route("GetBookingByUID")]
        [HttpGet]
        public BookingModel GetBookingByUID(int bookingUID)
        {
            BookingModel data = new BookingModel();
            Booking bki = db.Booking.Find(bookingUID);
            if (bki != null)
            {
                data.CareProviderUID = bki.CareProviderUID;
                data.AppointmentDttm = bki.AppointmentDttm;
                data.BKSTSUID = bki.BKSTSUID;
                data.PATMSGUID = bki.PATMSGUID;
                data.BookingUID = bki.UID;
                data.Comments = bki.Comments;
                data.PatientUID = bki.PatientUID;
                data.OwnerOrganisationUID = bki.OwnerOrganisationUID;
            }

            return data;
        }

        [Route("ManageBooking")]
        [HttpPost]
        public HttpResponseMessage ManageBooking(BookingModel model)
        {
            DateTime now = DateTime.Now;
            try
            {
                Booking bki = db.Booking.Find(model.BookingUID);
                if (bki == null)
                {
                    bki = new Booking();
                    bki.CUser = model.CUser;
                    bki.CWhen = now;
                }

                bki.MUser = model.MUser;
                bki.MWhen = now;
                bki.StatusFlag = "A";
                bki.OwnerOrganisationUID = model.OwnerOrganisationUID;
                bki.PatientUID = model.PatientUID;
                bki.PATMSGUID = model.PATMSGUID;
                bki.AppointmentDttm = model.AppointmentDttm;
                bki.BKSTSUID = model.BKSTSUID;
                bki.Comments = model.Comments;
                bki.CareProviderUID = model.CareProviderUID;

                db.Booking.AddOrUpdate(bki);
                db.SaveChanges();

                model.BookingUID = bki.UID;
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }

        }

        [Route("UpdateBookingArrive")]
        [HttpPut]
        public HttpResponseMessage UpdateBookingArrive(int bookingUID, int userUID)
        {
            try
            {
                Booking booking = db.Booking.Find(bookingUID);
                if (booking != null)
                {
                    db.Booking.Attach(booking);
                    booking.MUser = userUID;
                    booking.MWhen = DateTime.Now;
                    booking.BKSTSUID = 2943;

                    db.SaveChanges();
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("CancelBooking")]
        [HttpPut]
        public HttpResponseMessage CancelBooking(int bookingUID, int userUID)
        {
            try
            {
                Booking booking = db.Booking.Find(bookingUID);
                if (booking != null)
                {
                    db.Booking.Attach(booking);
                    booking.MUser = userUID;
                    booking.MWhen = DateTime.Now;
                    booking.BKSTSUID = 2945;

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

        #region PatientScannedDocument

        [Route("UploadedScannedDocument")]
        [HttpPost]
        public HttpResponseMessage UploadedScannedDocument(PatientScannedDocumentModel patScanDoc, int userUID)
        {
            try
            {
                PatientScannedDocument patientScanDoc = new PatientScannedDocument();
                DateTime now = DateTime.Now;
                patientScanDoc.PatientUID = patScanDoc.PatientUID;
                patientScanDoc.PatientVisitUID = patScanDoc.PatientVisitUID;
                patientScanDoc.DocumentName = patScanDoc.DocumentName;
                patientScanDoc.ScannedDocument = patScanDoc.ScannedDocument;
                patientScanDoc.DocUploadedDttm = patScanDoc.DocUploadedDttm;
                patientScanDoc.DocUploadedBy = userUID;
                patientScanDoc.Comments = patScanDoc.Comments;
                patientScanDoc.SCDTYUID = patScanDoc.SCDTYUID;
                patientScanDoc.CUser = userUID;
                patientScanDoc.CWhen = now;
                patientScanDoc.MUser = userUID;
                patientScanDoc.MWhen = now;
                patientScanDoc.StatusFlag = "A";
                db.PatientScannedDocument.Add(patientScanDoc);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("GetPatientScannedDocumentByPatientUID")]
        [HttpGet]
        public List<PatientScannedDocumentModel> GetPatientScannedDocumentByPatientUID(long patientUID)
        {
            List<PatientScannedDocumentModel> data = db.PatientScannedDocument.Where(p => p.PatientUID == patientUID && p.StatusFlag == "A")
                .Select(p => new PatientScannedDocumentModel
                {
                    PatientScannedDocumentUID = p.UID,
                    Comments = p.Comments,
                    DocumentName = p.DocumentName,
                    DocUploadedBy = p.DocUploadedBy,
                    DocUploadedByName = SqlFunction.fGetCareProviderName(p.DocUploadedBy),
                    DocUploadedDttm = p.DocUploadedDttm,
                    PatientUID = p.PatientUID,
                    PatientVisitUID = p.PatientVisitUID,
                    SCDTYUID = p.SCDTYUID,
                    ScanDocumentType = SqlFunction.fGetRfValDescription(p.SCDTYUID)
                }).ToList();

            return data;
        }


        [Route("GetPatientScannedDocumentContent")]
        [HttpGet]
        public byte[] GetPatientScannedDocumentContent(int patScanDocumentUID)
        {
            byte[] data = null;
            PatientScannedDocument patScanDoc = db.PatientScannedDocument.Find(patScanDocumentUID);
            if (patScanDoc != null)
            {
                data = patScanDoc.ScannedDocument;
            }

            return data;
        }

        [Route("DeletePatientScannedDocument")]
        [HttpDelete]
        public HttpResponseMessage DeletePatientScannedDocument(int patientScannedUID, int userUID)
        {
            try
            {
                PatientScannedDocument patScanDocument = db.PatientScannedDocument.Find(patientScannedUID);
                db.PatientScannedDocument.Attach(patScanDocument);
                patScanDocument.MUser = userUID;
                patScanDocument.MWhen = DateTime.Now;
                patScanDocument.StatusFlag = "D";
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        #endregion

        #region PatientMerge


        [Route("SearchPatientMerge")]
        [HttpGet]
        public List<PatientMergeModel> SearchPatientMerge(DateTime? dateFrom, DateTime? dateTo, long? patientUID, string isUnMerge)
        {
            List<PatientMergeModel> lstPatmerge = (from p in db.PatientMerge
                                                   where p.StatusFlag == "A"
                                                   && (patientUID == null || p.MajorPatientUID == patientUID || p.MinorPatientUID == patientUID)
                                                   && (string.IsNullOrEmpty(isUnMerge) || p.IsUnMerge == isUnMerge)
                                                   && (dateFrom == null || DbFunctions.TruncateTime(p.MergedDttm) >= DbFunctions.TruncateTime(dateFrom))
                                                   && (dateTo == null || DbFunctions.TruncateTime(p.MergedDttm) <= DbFunctions.TruncateTime(dateTo))
                                                   select new PatientMergeModel
                                                   {
                                                       PatientMergeUID = p.UID,
                                                       MajorPatientUID = p.MajorPatientUID,
                                                       MajorPatientID = SqlFunction.fGetPatientID(p.MajorPatientUID),
                                                       MajorPatientName = SqlFunction.fGetPatientName(p.MajorPatientUID),
                                                       IsUnMerge = p.IsUnMerge == "Y" ? "UNMERGE" : "MERGE",
                                                       MinorPatientUID = p.MinorPatientUID,
                                                       MinorPatientID = SqlFunction.fGetPatientID(p.MinorPatientUID),
                                                       MinorPatientName = SqlFunction.fGetPatientName(p.MinorPatientUID),
                                                       MergedDttm = p.MergedDttm,
                                                       MergedBy = SqlFunction.fGetCareProviderName(p.MergedByUID ?? 0),
                                                       MergedByUID = p.MergedByUID,
                                                       MergeType = SqlFunction.fGetRfValDescription(p.MRGTPUID ?? 0),
                                                       MRGTPUID = p.MRGTPUID
                                                   }).ToList();

            return lstPatmerge;
        }

        [Route("MergePatient")]
        [HttpPut]
        public HttpResponseMessage MergePatient(long majorPatientUID, long minorPatientUID, char address, char gender, char phone, char photo, char blood, int userUID)
        {
            try
            {
                SqlDirectStore.pMergePatient(majorPatientUID, minorPatientUID, address, gender, phone, photo, blood, userUID);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }


        [Route("UnMergePatient")]
        [HttpPut]
        public HttpResponseMessage UnMergePatient(int patientMergeUID, int userUID)
        {
            try
            {
                SqlDirectStore.pUnMergePatient(patientMergeUID, userUID);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("EncounterMergePatient")]
        [HttpPut]
        public HttpResponseMessage EncounterMergePatient(long majorPatientUID, long minorPatientUID, string minorVisitUIDS, int userUID)
        {
            try
            {
                SqlDirectStore.pEncounterMergePatient(majorPatientUID, minorPatientUID, minorVisitUIDS, userUID);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        #endregion

        public static Dictionary<string, List<string>> GenerateAuditLogMessages(object originalObject, object changedObject)
        {

            Dictionary<string, List<string>> list = new Dictionary<string, List<string>>();
            if (originalObject != null)
            {
                string className = string.Concat("[", originalObject.GetType().Name, "] ");
                foreach (PropertyInfo property in originalObject.GetType().GetProperties())
                {
                    List<string> OldValNewVal = new List<string>();
                    Type comparable = property.PropertyType.GetInterface("System.IComparable");

                    if (comparable == null && property.PropertyType.IsValueType)
                    {
                        Type underlyingType = Nullable.GetUnderlyingType(property.PropertyType);
                        // Nullable.GetUnderlyingType only returns a non-null value if the     
                        // supplied type was indeed a nullable type.     
                        if (underlyingType != null)
                            comparable = underlyingType.GetInterface("IComparable");
                    }
                    if (comparable != null)
                    {

                        string originalPropertyValue = Convert.ToString(property.GetValue(originalObject, null));//as string;
                        string newPropertyValue = Convert.ToString(property.GetValue(changedObject, null));// as string;
                        if (originalPropertyValue != newPropertyValue)
                        {
                            OldValNewVal.Add(originalPropertyValue);
                            OldValNewVal.Add(newPropertyValue);

                            if (!list.ContainsKey(property.Name))
                                list.Add(originalObject.GetType().Name + " - " + property.Name, OldValNewVal);
                        }
                    }
                }
            }

            return list;
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