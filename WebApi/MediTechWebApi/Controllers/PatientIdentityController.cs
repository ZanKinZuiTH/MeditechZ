using MediTech.DataBase;
using MediTech.Model;
using ShareLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Transactions;
using System.Web.Http;

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
                                                MobilePhone = pa.MobilePhone,
                                                NationalID = pa.IDCard,
                                                NATNLUID = pa.NATNLUID,
                                                IDPassport = pa.IDPassport,
                                                PatientID = pa.PatientID,
                                                EmployeeID = pa.EmployeeID,
                                                Department = pa.Department,
                                                Position = pa.Position,
                                                BLOODUID = pa.BLOODUID,
                                                RELGNUID = pa.RELGNUID,
                                                SecondPhone = pa.SecondPhone,
                                                SEXXXUID = pa.SEXXXUID,
                                                TITLEUID = pa.TITLEUID,
                                                LastVisitDttm = pa.LastVisitDttm,
                                                RegisterDate = pa.CWhen,
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

        [Route("GetPatientByName")]
        [HttpGet]
        public List<PatientInformationModel> GetPatientByName(string firstName, string lastName)
        {
            List<PatientInformationModel> data = (from pa in db.Patient
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
                                                  && pa.FirstName == firstName
                                                  && pa.LastName == lastName
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
                                                      IDPassport = pa.IDPassport,
                                                      PatientID = pa.PatientID,
                                                      EmployeeID = pa.EmployeeID,
                                                      Department = pa.Department,
                                                      Position = pa.Position,
                                                      BLOODUID = pa.BLOODUID,
                                                      RELGNUID = pa.RELGNUID,
                                                      SecondPhone = pa.SecondPhone,
                                                      SEXXXUID = pa.SEXXXUID,
                                                      TITLEUID = pa.TITLEUID,
                                                      LastVisitDttm = pa.LastVisitDttm,
                                                      RegisterDate = pa.CWhen,
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
                                                  }).ToList();
            return data;
        }

        [Route("GetPatientByEmployeeID")]
        [HttpGet]
        public PatientInformationModel GetPatientByEmployeeID(string EmployeeID)
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
                                            && pa.EmployeeID == EmployeeID
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
                                                IDPassport = pa.IDPassport,
                                                PatientID = pa.PatientID,
                                                EmployeeID = pa.EmployeeID,
                                                Department = pa.Department,
                                                Position = pa.Position,
                                                BLOODUID = pa.BLOODUID,
                                                RELGNUID = pa.RELGNUID,
                                                SecondPhone = pa.SecondPhone,
                                                SEXXXUID = pa.SEXXXUID,
                                                TITLEUID = pa.TITLEUID,
                                                LastVisitDttm = pa.LastVisitDttm,
                                                RegisterDate = pa.CWhen,
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
                                                IDPassport = pa.IDPassport,
                                                NATNLUID = pa.NATNLUID,
                                                PatientID = pa.PatientID,
                                                EmployeeID = pa.EmployeeID,
                                                Department = pa.Department,
                                                Position = pa.Position,
                                                BLOODUID = pa.BLOODUID,
                                                RELGNUID = pa.RELGNUID,
                                                SecondPhone = pa.SecondPhone,
                                                SEXXXUID = pa.SEXXXUID,
                                                TITLEUID = pa.TITLEUID,
                                                LastVisitDttm = pa.LastVisitDttm,
                                                RegisterDate = pa.CWhen,
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

        [Route("GetPatientByPassportNo")]
        [HttpGet]
        public PatientInformationModel GetPatientByPassportNo(string passportNo)
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
                                            && pa.IDPassport == passportNo
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
                                                IDPassport = pa.IDPassport,
                                                NATNLUID = pa.NATNLUID,
                                                PatientID = pa.PatientID,
                                                EmployeeID = pa.EmployeeID,
                                                Department = pa.Department,
                                                Position = pa.Position,
                                                BLOODUID = pa.BLOODUID,
                                                RELGNUID = pa.RELGNUID,
                                                SecondPhone = pa.SecondPhone,
                                                SEXXXUID = pa.SEXXXUID,
                                                TITLEUID = pa.TITLEUID,
                                                LastVisitDttm = pa.LastVisitDttm,
                                                RegisterDate = pa.CWhen,
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

        [Route("GetPatientInsuranceDetail")]
        [HttpGet]
        public List<PatientInsuranceDetailModel> GetPatientInsuranceDetail(long patientUID)
        {
            List<PatientInsuranceDetailModel> data = db.PatientInsuranceDetail
                .Where(p => p.StatusFlag == "A" && p.PatientUID == patientUID)
                .Select(p => new PatientInsuranceDetailModel
                {
                    PatientInsuranceDetailUID = p.UID,
                    PatientUID = p.PatientUID,
                    PatientVisitUID = p.PatientVisitUID,
                    InsuranceCompanyUID = p.InsuranceCompanyUID,
                    InsuranceCompanyName = p.InsuranceCompanyName,
                    PayorDetailUID = p.PayorDetailUID,
                    PayorName = p.PayorName,
                    PayorAgreementUID = p.PayorAgreementUID,
                    PayorAgreementName = p.PayorAgreementName,
                    PolicyMasterUID = p.PolicyMasterUID,
                    PolicyName = p.PolicyName,
                    PAYRTPUID = p.PAYRTPUID,
                    PayorType = SqlFunction.fGetRfValDescription(p.PAYRTPUID ?? 0),
                    FixedCopayAmount = p.FixedCopayAmount,
                    ClaimPercentage = p.ClaimPercentage,
                    StartDttm = p.StartDttm,
                    EndDttm = p.EndDttm,
                    EligibleAmount = p.EligibleAmount,
                    Comments = p.Comments,
                    CreatedBy = SqlFunction.fGetCareProviderName(p.CUser)
                }).ToList().OrderBy(p => int.Parse(p.PayorType ?? "99")).ToList();
            return data;
        }

        [Route("ManagePatientInsuranceDetail")]
        [HttpPost]
        public HttpResponseMessage ManagePatientInsuranceDetail(List<PatientVisitPayorModel> patientVisitPayorList, int userUID)
        {
            try
            {
                DateTime now = DateTime.Now;
                using (var tran = new TransactionScope())
                {

                    if (patientVisitPayorList != null)
                    {
                        var deletedVisitPayors = patientVisitPayorList.Where(p => p.StatusFlag == "D");
                        var activedVisitPayors = patientVisitPayorList.Where(p => p.StatusFlag == "A");

                        foreach (var deletedPayor in deletedVisitPayors)
                        {
                            PatientInsuranceDetail deleteData = db.PatientInsuranceDetail.Where(i => i.InsuranceCompanyUID == deletedPayor.InsuranceCompanyUID
                && i.PayorDetailUID == deletedPayor.PayorDetailUID && i.PayorAgreementUID == deletedPayor.PayorAgreementUID).FirstOrDefault();
                            if (deleteData != null)
                            {
                                db.PatientInsuranceDetail.Attach(deleteData);
                                deleteData.StatusFlag = "D";
                                deleteData.MUser = userUID;
                                deleteData.MWhen = now;

                                db.SaveChanges();
                            }
                        }

                        foreach (var payor in activedVisitPayors)
                        {
                            PatientInsuranceDetail detail = db.PatientInsuranceDetail.Where(i => i.InsuranceCompanyUID == payor.InsuranceCompanyUID
                            && i.PayorDetailUID == payor.PayorDetailUID && i.PayorAgreementUID == payor.PayorAgreementUID).FirstOrDefault();
                            if (detail == null)
                            {
                                detail = new PatientInsuranceDetail();
                                detail.CUser = userUID;
                                detail.CWhen = now;
                            }

                            detail.PatientUID = payor.PatientUID;
                            detail.PatientVisitUID = payor.PatientVisitUID != 0 ? payor.PatientVisitUID : (long?)null;
                            detail.PayorName = payor.PayorName;
                            detail.PayorDetailUID = payor.PayorDetailUID;
                            detail.PolicyName = payor.PolicyName;
                            detail.InsuranceCompanyUID = payor.InsuranceCompanyUID ?? 0;
                            detail.InsuranceCompanyName = payor.InsuranceName;
                            detail.PAYRTPUID = payor.PAYRTPUID;

                            detail.PolicyMasterUID = payor.PolicyMasterUID;

                            detail.EligibleAmount = payor.EligibleAmount;
                            detail.StartDttm = payor.ActiveFrom;
                            detail.EndDttm = payor.ActiveTo;
                            detail.Comments = payor.Comment;

                            detail.PayorAgreementUID = payor.PayorAgreementUID;
                            detail.PayorAgreementName = payor.AgreementName;
                            detail.ClaimPercentage = payor.ClaimPercentage;
                            if (detail.ClaimPercentage == 0)
                                detail.ClaimPercentage = null;
                            detail.FixedCopayAmount = payor.FixedCopayAmount;
                            if (detail.FixedCopayAmount == 0)
                                detail.FixedCopayAmount = null;

                            detail.MUser = payor.MUser;
                            detail.MWhen = now;
                            detail.StatusFlag = payor.StatusFlag;
                            detail.OwnerOrganisationUID = payor.OwnerOrganisationUID;
                            db.PatientInsuranceDetail.AddOrUpdate(detail);

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


        [Route("ManagePatientInsurance")]
        [HttpPost]
        public HttpResponseMessage ManagePatientInsurance(List<PatientInsuranceDetailModel> patientInsuranceDetails, int userUID)
        {
            try
            {
                DateTime now = DateTime.Now;
                using (var tran = new TransactionScope())
                {

                    foreach (var patientInsurance in patientInsuranceDetails)
                    {
                        var deletedVisitPayors = patientInsuranceDetails.Where(p => p.StatusFlag == "D");
                        var activedVisitPayors = patientInsuranceDetails.Where(p => p.StatusFlag == "A");

                        foreach (var deletedPayor in deletedVisitPayors)
                        {
                            PatientInsuranceDetail deleteData = db.PatientInsuranceDetail.Where(i => i.InsuranceCompanyUID == deletedPayor.InsuranceCompanyUID
                && i.PayorDetailUID == deletedPayor.PayorDetailUID && i.PayorAgreementUID == deletedPayor.PayorAgreementUID).FirstOrDefault();
                            if (deleteData != null)
                            {
                                db.PatientInsuranceDetail.Attach(deleteData);
                                deleteData.StatusFlag = "D";
                                deleteData.MUser = userUID;
                                deleteData.MWhen = now;

                                db.SaveChanges();
                            }
                        }


                        foreach (var payor in activedVisitPayors)
                        {
                            PatientInsuranceDetail detail = db.PatientInsuranceDetail.Where(i => i.InsuranceCompanyUID == payor.InsuranceCompanyUID
                            && i.PayorDetailUID == payor.PayorDetailUID && i.PayorAgreementUID == payor.PayorAgreementUID).FirstOrDefault();
                            if (detail == null)
                            {
                                detail = new PatientInsuranceDetail();
                                detail.CUser = userUID;
                                detail.CWhen = now;
                            }

                            detail.PatientUID = payor.PatientUID;
                            detail.PatientVisitUID = payor.PatientVisitUID != 0 ? payor.PatientVisitUID : (long?)null;
                            detail.PayorName = payor.PayorName;
                            detail.PayorDetailUID = payor.PayorDetailUID;
                            detail.PolicyName = payor.PolicyName;
                            detail.InsuranceCompanyUID = payor.InsuranceCompanyUID;
                            detail.InsuranceCompanyName = payor.InsuranceCompanyName;
                            detail.PAYRTPUID = payor.PAYRTPUID;

                            detail.PolicyMasterUID = payor.PolicyMasterUID;

                            detail.EligibleAmount = payor.EligibleAmount;
                            detail.StartDttm = payor.StartDttm;
                            detail.EndDttm = payor.EndDttm;
                            detail.Comments = payor.Comments;

                            detail.PayorAgreementUID = payor.PayorAgreementUID;
                            detail.PayorAgreementName = payor.PayorAgreementName;
                            detail.ClaimPercentage = payor.ClaimPercentage;
                            if (detail.ClaimPercentage == 0)
                                detail.ClaimPercentage = null;
                            detail.FixedCopayAmount = payor.FixedCopayAmount;
                            if (detail.FixedCopayAmount == 0)
                                detail.FixedCopayAmount = null;

                            detail.MUser = userUID;
                            detail.MWhen = now;
                            detail.StatusFlag = payor.StatusFlag;
                            detail.OwnerOrganisationUID = payor.OwnerOrganisationUID;
                            db.PatientInsuranceDetail.AddOrUpdate(detail);

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


        [Route("SearchPatient")]
        [HttpGet]
        public List<PatientInformationModel> SearchPatient(string patientID, string firstName, string middleName, string lastName, string nickName, DateTime? birthDate, int? SEXXXUID, string idCard, DateTime? lastVisitDate, string mobilePhone, string idPassport)
        {
            DataTable dataTable = SqlDirectStore.pSearchPatient(patientID, firstName, middleName, lastName, nickName, birthDate, SEXXXUID, idCard, lastVisitDate, mobilePhone, idPassport);

            List<PatientInformationModel> data = dataTable.ToList<PatientInformationModel>();

            return data;
        }

        [Route("SearchPatientEmergency")]
        [HttpGet]
        public List<PatientInformationModel> SearchPatientEmergency(string patientID, string firstName, string middleName, string lastName, string nickName, DateTime? birthDate, int? SEXXXUID, string idCard, DateTime? lastVisitDate, string mobilePhone)
        {
            DataTable dataTable = SqlDirectStore.SearchPatientEmergency(patientID, firstName, middleName, lastName, nickName, birthDate, SEXXXUID, idCard, lastVisitDate, mobilePhone);

            List<PatientInformationModel> data = dataTable.ToList<PatientInformationModel>();

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
                        patient.OwnerOrganisationUID = patientData.OwnerOrganisationUID;
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
                    patient.IDPassport = patientInfo.IDPassport;
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
                    patient.PatientOtherID = patientInfo.PatientOtherID;
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

                        patientInfo.PatientAddressUID = patientAddress.UID;
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


                    if (patientInfo.PatientInsuranceDetails != null && patientInfo.PatientInsuranceDetails.Count > 0)
                    {
                        patientInfo.PatientInsuranceDetails.ForEach(p => p.PatientUID = patientInfo.PatientUID);
                        ManagePatientInsurance(patientInfo.PatientInsuranceDetails, userID);
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

        [Route("RegisterPatientEmergency")]
        [HttpPost]
        public HttpResponseMessage RegisterPatientEmergency(PatientInformationModel patientInfo, int userID, int OwnerOrganisationUID)
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
                    patient.IDPassport = patientInfo.IDPassport;
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
                    patient.PatientOtherID = patientInfo.PatientOtherID;
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
        public PatientInformationModel CheckDupicatePatient(string firstName, string lastName)
        {
            PatientInformationModel data = SqlDirectStore.pCheckDupicatePatient(firstName, lastName).ToList<PatientInformationModel>().FirstOrDefault();
            return data;
        }

        #endregion

        #region PatientVisit

        [Route("SearchPatientVisit")]
        [HttpGet]
        public List<PatientVisitModel> SearchPatientVisit(string hn, string firstName, string lastName, int? careproviderUID
            , string statusList, DateTime? dateFrom, DateTime? dateTo, DateTime? arrivedDttm, int? ownerOrganisationUID, int? locationUID
            , int? insuranceCompanyUID, int? checkupJobUID, string encounter)
        {
            DataTable dataTable = SqlDirectStore.pSearchPatientVisit(hn, firstName, lastName, careproviderUID, statusList, dateFrom, dateTo, arrivedDttm, ownerOrganisationUID, locationUID, insuranceCompanyUID, checkupJobUID, encounter);

            List<PatientVisitModel> data = dataTable.ToList<PatientVisitModel>();

            return data;
        }

        [Route("SearchERPatientVisit")]
        [HttpGet]
        public List<PatientVisitModel> SearchERPatientVisit(string hn, string firstName, string lastName, int? careproviderUID
            , string statusList, DateTime? dateFrom, DateTime? dateTo, DateTime? arrivedDttm, int? ownerOrganisationUID, int? locationUID
            , int? insuranceCompanyUID, int? checkupJobUID, int? encounter)
        {
            DataTable dataTable = SqlDirectStore.pSearchEmergencyVisit(hn, firstName, lastName, careproviderUID, statusList, dateFrom, dateTo, arrivedDttm, ownerOrganisationUID, locationUID, insuranceCompanyUID, checkupJobUID, encounter);

            List<PatientVisitModel> data = dataTable.ToList<PatientVisitModel>();

            return data;
        }

        [Route("SearchIPDPatientVisit")]
        [HttpGet]
        public List<PatientVisitModel> SearchIPDPatientVisit(string hn, string firstName, string lastName, int? careproviderUID
           , string statusList, DateTime? dateFrom, DateTime? dateTo, DateTime? arrivedDttm, int? ownerOrganisationUID
           , int? payorDetailUID, int? checkupJobUID)
        {
            DataTable dataTable = SqlDirectStore.pSearchIPDPatientVisit(hn, firstName, lastName, careproviderUID, statusList, dateFrom, dateTo, arrivedDttm, ownerOrganisationUID, payorDetailUID, checkupJobUID);

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
                    var encounterType = db.ReferenceValue.Where(p => p.DomainCode == "ENTYP").ToList();
                    var selectEncounterType = encounterType.FirstOrDefault(p => p.UID == patientVisitInfo.ENTYPUID);
                    string seqVisitID = "";
                    switch (selectEncounterType.ValueCode)
                    {
                        case "HEAL":
                            seqVisitID = SEQHelper.GetSEQIDFormat("SEQVisitID", out outseqvisitUID);
                            seqVisitID = seqVisitID.Replace("O", "H");
                            break;
                        case "AEPAT":
                            seqVisitID = SEQHelper.GetSEQIDFormat("SEQERVisitID", out outseqvisitUID);
                            break;
                        default:
                            seqVisitID = SEQHelper.GetSEQIDFormat("SEQVisitID", out outseqvisitUID);
                            break;
                    }




                    if (string.IsNullOrEmpty(seqVisitID))
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No SEQVisitID in SEQCONFIGURATION");
                    }

                    PatientVisit patientVisit = new PatientVisit();
                    patientVisit.PatientUID = patientVisitInfo.PatientUID;
                    patientVisit.VISTYUID = patientVisitInfo.VISTYUID;
                    patientVisit.VISTSUID = patientVisitInfo.VISTSUID;
                    patientVisit.PRITYUID = patientVisitInfo.PRITYUID;
                    patientVisit.ENTYPUID = patientVisitInfo.ENTYPUID;
                    patientVisit.CheckupJobUID = patientVisitInfo.CheckupJobUID;
                    patientVisit.RefNo = patientVisitInfo.RefNo;
                    patientVisit.CompanyName = patientVisitInfo.CompanyName;
                    patientVisit.VisitID = seqVisitID;
                    patientVisit.CareProviderUID = patientVisitInfo.CareProviderUID;
                    patientVisit.BookingUID = patientVisitInfo.BookingUID;
                    patientVisit.StartDttm = patientVisitInfo.StartDttm;
                    patientVisit.ArrivedDttm = patientVisitInfo.StartDttm;
                    patientVisit.LocationUID = patientVisitInfo.LocationUID;
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
                    serviceEvent.LocationUID = patientVisit.LocationUID;
                    serviceEvent.MUser = userID;
                    serviceEvent.MWhen = now;
                    serviceEvent.CUser = userID;
                    serviceEvent.CWhen = now;
                    serviceEvent.StatusFlag = "A";

                    db.PatientServiceEvent.Add(serviceEvent);
                    #endregion

                    #region PatientVisitCareprovider
                    string careproviderName = string.Empty;
                    if (patientVisitInfo.CareProviderUID != null)
                    {
                        careproviderName = (from cr in db.Careprovider
                                            where cr.UID == patientVisitInfo.CareProviderUID
                                            select new
                                            {
                                                careproviderName = SqlFunction.fGetCareProviderName(cr.UID)
                                            }).FirstOrDefault()?.careproviderName;
                    }

                    PatientVisitCareProvider patientVisitCare = new PatientVisitCareProvider();
                    db.PatientVisitCareProvider.Attach(patientVisitCare);
                    patientVisitCare.PatientVisitUID = patientVisit.UID;
                    patientVisitCare.StartDttm = now;
                    patientVisitCare.PACLSUID = patientVisit.VISTSUID ?? 0;
                    patientVisitCare.CareproviderUID = patientVisitInfo.CareProviderUID;
                    patientVisitCare.CareProviderName = careproviderName;
                    patientVisitCare.LocationUID = patientVisitInfo.LocationUID;
                    patientVisitCare.CUser = userID;
                    patientVisitCare.CWhen = DateTime.Now;
                    patientVisitCare.MUser = userID;
                    patientVisitCare.MWhen = DateTime.Now;
                    patientVisitCare.OwnerOrganisationUID = patientVisitInfo.OwnerOrganisationUID ?? 0;
                    patientVisitCare.StatusFlag = "A";

                    db.PatientVisitCareProvider.Add(patientVisitCare);

                    #endregion

                    db.SaveChanges();


                    foreach (var aIns in patientVisitInfo.PatientVisitPayors)
                    {
                        PatientVisitPayor aPayor = new PatientVisitPayor();
                        aPayor.PatientUID = patientVisitInfo.PatientUID;
                        aPayor.PatientVisitUID = patientVisit.UID;
                        aPayor.PayorDetailUID = aIns.PayorDetailUID;
                        aPayor.PayorAgreementUID = aIns.PayorAgreementUID;
                        aPayor.PolicyMasterUID = aIns.PolicyMasterUID;
                        aPayor.PolicyName = aIns.PolicyName;
                        aPayor.InsuranceCompanyUID = aIns.InsuranceCompanyUID;
                        aPayor.EligibileAmount = aIns.EligibleAmount;
                        aPayor.PAYRTPUID = aIns.PAYRTPUID;
                        aPayor.ActiveFrom = aIns.ActiveFrom;
                        aPayor.ActiveTo = aIns.ActiveTo;
                        aPayor.ClaimPercentage = aIns.ClaimPercentage;
                        aPayor.FixedCopayAmount = aIns.FixedCopayAmount;
                        aPayor.CoveredAmount = aIns.CoveredAmount;
                        aPayor.ClaimAmount = aIns.ClaimAmount;
                        aPayor.CUser = userID;
                        aPayor.CWhen = now;
                        aPayor.MUser = userID;
                        aPayor.MWhen = now;
                        aPayor.StatusFlag = "A";

                        db.PatientVisitPayor.Add(aPayor);
                    }


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

        [Route("SaveERPatientVisit")]
        [HttpPost]
        public HttpResponseMessage SaveERPatientVisit(PatientVisitModel patientVisitInfo, int userID)
        {
            try
            {
                using (var tran = new TransactionScope())
                {
                    DateTime now = DateTime.Now;

                    int outseqervisitUID;
                    string erseqVisitID = SEQHelper.GetSEQIDFormat("SEQERVisitID", out outseqervisitUID);

                    if (string.IsNullOrEmpty(erseqVisitID))
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No SEQVisitID in SEQCONFIGURATION");
                    }

                    PatientVisit patientVisit = new PatientVisit();
                    patientVisit.PatientUID = patientVisitInfo.PatientUID;
                    patientVisit.VISTYUID = patientVisitInfo.VISTYUID;
                    patientVisit.VISTSUID = patientVisitInfo.VISTSUID;
                    patientVisit.PRITYUID = patientVisitInfo.PRITYUID;
                    patientVisit.CheckupJobUID = patientVisitInfo.CheckupJobUID;
                    patientVisit.ENTYPUID = patientVisitInfo.ENTYPUID;
                    patientVisit.LocationUID = patientVisitInfo.LocationUID;
                    patientVisit.BedUID = patientVisitInfo.BedUID;
                    patientVisit.RefNo = patientVisitInfo.RefNo;
                    patientVisit.CompanyName = patientVisitInfo.CompanyName;
                    patientVisit.VisitID = erseqVisitID;
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

                    //PatientVisitPayor visitPayor = new PatientVisitPayor();
                    //visitPayor.PatientUID = patientVisitInfo.PatientUID;
                    //visitPayor.PatientVisitUID = patientVisit.UID;
                    //visitPayor.PayorDetailUID = 1;
                    //visitPayor.PayorAgreementUID = 1;
                    //visitPayor.CUser = userID;
                    //visitPayor.CWhen = now;
                    //visitPayor.MUser = userID;
                    //visitPayor.MWhen = now;
                    //visitPayor.StatusFlag = "A";

                    //db.PatientVisitPayor.Add(visitPayor);

                    //db.SaveChanges();

                    if (patientVisit != null)
                    {
                        PatientVisitID patientVisitID = new PatientVisitID();
                        patientVisitID.PatientVisitUID = patientVisit.UID;
                        patientVisitID.VISIDUID = patientVisit.VISTYUID ?? 0;
                        patientVisitID.MainIdentifier = "Y";
                        patientVisitID.Identifier = patientVisit.VisitID;
                        patientVisitID.ActiveFrom = patientVisit.StartDttm ?? now;
                        patientVisitID.OwnerOrganisationUID = patientVisit.OwnerOrganisationUID ?? 0;
                        patientVisitID.CUser = userID;
                        patientVisitID.CWhen = now;
                        patientVisitID.MUser = userID;
                        patientVisitID.MWhen = now;
                        patientVisitID.StatusFlag = "A";

                        db.PatientVisitID.Add(patientVisitID);
                        db.SaveChanges();
                    }

                    if (patientVisitInfo.AEAdmission != null)
                    {
                        PatientAEAdmission aEAdmission = new PatientAEAdmission();
                        aEAdmission.PatientUID = patientVisit.PatientUID;
                        aEAdmission.PatientVisitUID = patientVisit.UID;
                        //aEAdmission.ADTYPUID = patientVisitInfo.AEAdmission.ADTYPUID;
                        aEAdmission.ARRMDUID = patientVisitInfo.AEAdmission.ARRMDUID;
                        aEAdmission.RELTNUID = patientVisitInfo.AEAdmission.RELTNUID;
                        aEAdmission.ESCTPUID = patientVisitInfo.AEAdmission.ESCTPUID;
                        aEAdmission.VehicleNumber = patientVisitInfo.AEAdmission.VehicleNumber;
                        aEAdmission.EmergencyOcurredDttm = patientVisitInfo.AEAdmission.EventOccuredDttm;
                        aEAdmission.EMGTPUID = patientVisitInfo.AEAdmission.EMGTPUID;
                        aEAdmission.EMGCDUID = patientVisitInfo.AEAdmission.EMGCDUID;
                        aEAdmission.ProblemUID = patientVisitInfo.AEAdmission.ProblemUID;
                        aEAdmission.InjuryReason = patientVisitInfo.AEAdmission.InjuryReason;
                        aEAdmission.EmergencyExamDetail = patientVisitInfo.AEAdmission.EmergencyExamDetail;
                        aEAdmission.IsDead = patientVisitInfo.AEAdmission.IsDead;
                        aEAdmission.Line1 = patientVisitInfo.AEAdmission.Line1;
                        aEAdmission.Line2 = patientVisitInfo.AEAdmission.Line2;
                        aEAdmission.Line3 = patientVisitInfo.AEAdmission.Line3;
                        aEAdmission.Line4 = patientVisitInfo.AEAdmission.Line4;
                        aEAdmission.DistrictUID = patientVisitInfo.AEAdmission.DistrictUID;
                        aEAdmission.AmphurUID = patientVisitInfo.AEAdmission.AmphurUID;
                        aEAdmission.ProvinceUID = patientVisitInfo.AEAdmission.ProvinceUID;
                        aEAdmission.ZipCode = patientVisitInfo.AEAdmission.ZipCode;
                        aEAdmission.PhoneNumber = patientVisitInfo.AEAdmission.PhoneNumber;
                        aEAdmission.MobileNumber = patientVisitInfo.AEAdmission.MobileNumber;
                        aEAdmission.Comments = patientVisitInfo.AEAdmission.Comments;
                        aEAdmission.CUser = userID;
                        aEAdmission.CWhen = now;
                        aEAdmission.MUser = userID;
                        aEAdmission.MWhen = now;
                        aEAdmission.StatusFlag = "A";

                        db.PatientAEAdmission.Add(aEAdmission);
                        db.SaveChanges();
                    }

                    #region PatientServiceEvent
                    PatientServiceEvent serviceEvent = new PatientServiceEvent();
                    serviceEvent.PatientVisitUID = patientVisit.UID;
                    serviceEvent.EventStartDttm = now;
                    serviceEvent.VISTSUID = patientVisit.VISTSUID ?? 0;
                    serviceEvent.LocationUID = patientVisitInfo.LocationUID;
                    serviceEvent.MUser = userID;
                    serviceEvent.MWhen = now;
                    serviceEvent.CUser = userID;
                    serviceEvent.CWhen = now;
                    serviceEvent.StatusFlag = "A";

                    db.PatientServiceEvent.Add(serviceEvent);
                    #endregion

                    #region PatientVisitCareprovider
                    string careproviderName = string.Empty;
                    if (patientVisitInfo.CareProviderUID != null)
                    {
                        careproviderName = (from cr in db.Careprovider
                                            where cr.UID == patientVisitInfo.CareProviderUID
                                            select new
                                            {
                                                careproviderName = SqlFunction.fGetCareProviderName(cr.UID)
                                            }).FirstOrDefault()?.careproviderName;
                    }

                    PatientVisitCareProvider patientVisitCare = new PatientVisitCareProvider();
                    db.PatientVisitCareProvider.Attach(patientVisitCare);
                    patientVisitCare.PatientVisitUID = patientVisit.UID;
                    patientVisitCare.StartDttm = now;
                    patientVisitCare.PACLSUID = patientVisit.VISTSUID ?? 0;
                    patientVisitCare.CareproviderUID = patientVisitInfo.CareProviderUID;
                    patientVisitCare.CareProviderName = careproviderName;
                    patientVisitCare.LocationUID = patientVisitInfo.LocationUID;
                    patientVisitCare.CUser = userID;
                    patientVisitCare.CWhen = DateTime.Now;
                    patientVisitCare.MUser = userID;
                    patientVisitCare.MWhen = DateTime.Now;
                    patientVisitCare.OwnerOrganisationUID = patientVisitInfo.OwnerOrganisationUID ?? 0;
                    patientVisitCare.StatusFlag = "A";

                    db.PatientVisitCareProvider.Add(patientVisitCare);

                    #endregion

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

                    patientVisitInfo.VisitID = erseqVisitID;
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

        [Route("SaveIPDPatientVisit")]
        [HttpPost]
        public HttpResponseMessage SaveIPDPatientVisit(PatientVisitModel patientVisitInfo, int userID)
        {
            try
            {
                using (var tran = new TransactionScope())
                {
                    DateTime now = DateTime.Now;

                    int outseqervisitUID;
                    string erseqVisitID = SEQHelper.GetSEQIDFormat("SEQIPDVisitID", out outseqervisitUID);


                    if (string.IsNullOrEmpty(erseqVisitID))
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No SEQVisitID in SEQCONFIGURATION");
                    }


                    PatientVisit patientVisit = db.PatientVisit.Find(patientVisitInfo.PatientVisitUID);
                    if (patientVisit == null)
                    {
                        patientVisit = new PatientVisit();
                        patientVisit.PatientUID = patientVisitInfo.PatientUID;
                        patientVisit.VISTYUID = patientVisitInfo.VISTYUID;
                        patientVisit.VISTSUID = patientVisitInfo.VISTSUID;
                        patientVisit.BedUID = patientVisitInfo.BedUID;
                        patientVisit.CUser = userID;
                        patientVisit.CWhen = now;
                        patientVisit.PRITYUID = patientVisitInfo.PRITYUID;

                    }
                    patientVisit.ENTYPUID = patientVisitInfo.ENTYPUID;
                    patientVisit.ENSTAUID = patientVisitInfo.ENSTAUID;
                    patientVisit.LocationUID = patientVisitInfo.LocationUID;
                    patientVisit.BedUID = patientVisitInfo.BedUID;
                    patientVisit.RefNo = patientVisitInfo.RefNo;
                    patientVisit.PRITYUID = patientVisitInfo.PRITYUID;
                    patientVisit.CompanyName = patientVisitInfo.CompanyName;
                    patientVisit.VisitID = erseqVisitID;
                    patientVisit.CareProviderUID = patientVisitInfo.CareProviderUID;
                    patientVisit.BookingUID = patientVisitInfo.BookingUID;
                    patientVisit.StartDttm = patientVisitInfo.StartDttm ?? now;
                    patientVisit.ArrivedDttm = patientVisitInfo.StartDttm ?? now;
                    patientVisit.SpecialityUID = patientVisitInfo.SpecialityUID;
                    patientVisit.IsReAdmisstion = patientVisitInfo.IsReAdmisstion;
                    patientVisit.MUser = userID;
                    patientVisit.MWhen = now;
                    patientVisit.StatusFlag = "A";
                    patientVisit.OwnerOrganisationUID = patientVisitInfo.OwnerOrganisationUID;

                    db.PatientVisit.AddOrUpdate(patientVisit);
                    db.SaveChanges();

                    //PatientVisitPayor visitPayor = new PatientVisitPayor();
                    //visitPayor.PatientUID = patientVisitInfo.PatientUID;
                    //visitPayor.PatientVisitUID = patientVisit.UID;
                    //visitPayor.PayorDetailUID = 1;
                    //visitPayor.PayorAgreementUID = 1;
                    //visitPayor.CUser = userID;
                    //visitPayor.CWhen = now;
                    //visitPayor.MUser = userID;
                    //visitPayor.MWhen = now;
                    //visitPayor.StatusFlag = "A";

                    //db.PatientVisitPayor.Add(visitPayor);
                    //db.SaveChanges();

                    if (patientVisit != null)
                    {
                        PatientVisitID patientVisitID = new PatientVisitID();
                        patientVisitID.PatientVisitUID = patientVisit.UID;
                        patientVisitID.VISIDUID = patientVisit.VISTYUID ?? 0;
                        patientVisitID.MainIdentifier = "Y";
                        patientVisitID.Identifier = patientVisit.VisitID;
                        patientVisitID.ActiveFrom = patientVisit.StartDttm ?? now;
                        patientVisitID.OwnerOrganisationUID = patientVisit.OwnerOrganisationUID ?? 0;
                        patientVisitID.CUser = userID;
                        patientVisitID.CWhen = now;
                        patientVisitID.MUser = userID;
                        patientVisitID.MWhen = now;
                        patientVisitID.StatusFlag = "A";

                        db.PatientVisitID.Add(patientVisitID);
                        db.SaveChanges();

                        AdmissionEvent admissionEvent = new AdmissionEvent();
                        admissionEvent.PatientUID = patientVisit.PatientUID;
                        admissionEvent.PatientVisitUID = patientVisit.UID;
                        admissionEvent.OwnerOrganisationUID = patientVisitInfo.AdmissionEvent.OwnerOrganisationUID;
                        admissionEvent.CareproviderUID = patientVisitInfo.AdmissionEvent.CarepoviderUID;
                        admissionEvent.ExpectedLengthOfStay = patientVisitInfo.AdmissionEvent.ExpectedLengthOfStay ?? 0;
                        admissionEvent.AdmissionDttm = patientVisitInfo.AdmissionEvent.AdmissionDttm;
                        admissionEvent.ExpectedDischargeDttm = patientVisitInfo.AdmissionEvent.ExpectedDischargeDttm;
                        admissionEvent.CUser = userID;
                        admissionEvent.CWhen = now;
                        admissionEvent.MUser = userID;
                        admissionEvent.MWhen = now;
                        admissionEvent.StatusFlag = "A";

                        db.AdmissionEvent.AddOrUpdate(admissionEvent);
                        db.SaveChanges();


                        PatientADTEvent patientADT = new PatientADTEvent();
                        patientADT.PatientUID = patientVisit.PatientUID;
                        patientADT.PatientVisitUID = patientVisit.UID;
                        patientADT.OwnerOrganisationUID = patientVisitInfo.AdmissionEvent.OwnerOrganisationUID;
                        patientADT.EventOccuredDttm = now;
                        patientADT.EVNTYUID = patientVisitInfo.ENSTAUID ?? 0;
                        patientADT.IdentifyingUID = admissionEvent.UID;
                        patientADT.IdentifyingType = "ADMISSIONEVENT";
                        patientADT.CUser = userID;
                        patientADT.CWhen = now;
                        patientADT.MUser = userID;
                        patientADT.MWhen = now;
                        patientADT.StatusFlag = "A";

                        db.PatientADTEvent.AddOrUpdate(patientADT);
                        db.SaveChanges();

                        if (patientVisitInfo.CareProviderUID != null)
                        {
                            CareproviderTransfer careproviderTransfer = new CareproviderTransfer(); //ลงแพทย์หลัก
                            careproviderTransfer.PatientUID = patientVisit.PatientUID;
                            careproviderTransfer.PatientVisitUID = patientVisit.UID;
                            careproviderTransfer.CareproviderUID = patientVisitInfo.CareProviderUID ?? 0;
                            careproviderTransfer.OwnerOrganisationUID = patientVisitInfo.OwnerOrganisationUID ?? 0;
                            careproviderTransfer.CUser = userID;
                            careproviderTransfer.CWhen = now;
                            careproviderTransfer.MUser = userID;
                            careproviderTransfer.MWhen = now;
                            careproviderTransfer.StatusFlag = "A";

                            db.CareproviderTransfer.AddOrUpdate(careproviderTransfer);
                            db.SaveChanges();
                        }

                        if (patientVisitInfo.SecondCareprovider != null)
                        {
                            foreach (var item in patientVisitInfo.SecondCareprovider)
                            {
                                PatientConsultation patientConsultation = new PatientConsultation(); //ลงแพทย์หมด แยก type Primary/second
                                patientConsultation.PatientUID = patientVisit.PatientUID;
                                patientConsultation.PatientVisitUID = patientVisit.UID;
                                patientConsultation.CareproviderUID = item.CareproviderUID;
                                patientConsultation.CareProviderName = item.FullName;
                                patientConsultation.VISTYUID = item.VISTYUID;
                                patientConsultation.RecordedDttm = now;
                                patientConsultation.CUser = userID;
                                patientConsultation.CWhen = now;
                                patientConsultation.MUser = userID;
                                patientConsultation.MWhen = now;
                                patientConsultation.StatusFlag = "A";

                                db.PatientConsultation.AddOrUpdate(patientConsultation);
                                db.SaveChanges();
                            }
                        }

                        LocationTransfer locationTransfer = new LocationTransfer(); //เก็บ location ward bed ที่เลือก
                        locationTransfer.PatientUID = patientVisit.PatientUID;
                        locationTransfer.PatientVisitUID = patientVisit.UID;
                        locationTransfer.LocationUID = patientVisitInfo.LocationUID ?? 0;
                        locationTransfer.BedUID = patientVisitInfo.BedUID;
                        locationTransfer.CUser = userID;
                        locationTransfer.CWhen = now;
                        locationTransfer.MUser = userID;
                        locationTransfer.MWhen = now;
                        locationTransfer.OwnerOrganisationUID = patientVisitInfo.OwnerOrganisationUID ?? 0;
                        locationTransfer.StatusFlag = "A";

                        db.LocationTransfer.AddOrUpdate(locationTransfer);
                        db.SaveChanges();

                    }

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

                    patientVisitInfo.VisitID = erseqVisitID;
                    patientVisitInfo.PatientVisitUID = patientVisit.UID;
                    patientVisitInfo.PatientID = patient.PatientID;
                    tran.Complete();
                }
                return Request.CreateResponse(HttpStatusCode.OK, patientVisitInfo);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("ManageEmergencyAE")]
        [HttpPost]
        public HttpResponseMessage ManageEmergencyAE(PatientAEAdmissionModel model, int userUID)
        {
            DateTime now = DateTime.Now;
            try
            {
                using (var tran = new TransactionScope())
                {
                    PatientVisit visit = db.PatientVisit.Find(model.PatientVisitUid);
                    if (visit != null)
                    {
                        visit.BedUID = model.BedUID;
                        visit.CareProviderUID = model.CareproviderUID;
                        visit.MUser = userUID;
                        visit.MWhen = now;

                        db.PatientVisit.AddOrUpdate(visit);
                        db.SaveChanges();
                    }

                    PatientAEAdmission visitAE = db.PatientAEAdmission.Find(model.Uid);
                    if (visitAE != null)
                    {
                        visitAE.ARRMDUID = model.ARRMDUID;
                        visitAE.RELTNUID = model.RELTNUID;
                        visitAE.ESCTPUID = model.ESCTPUID;
                        visitAE.VehicleNumber = model.VehicleNumber;
                        visitAE.EmergencyOcurredDttm = model.EventOccuredDttm;
                        visitAE.EMGTPUID = model.EMGTPUID;
                        visitAE.EMGCDUID = model.EMGCDUID;
                        visitAE.ProblemUID = model.ProblemUID;
                        visitAE.InjuryReason = model.InjuryReason;
                        visitAE.EmergencyExamDetail = model.EmergencyExamDetail;
                        visitAE.IsDead = model.IsDead;
                        visitAE.Line1 = model.Line1;
                        visitAE.Line2 = model.Line2;
                        visitAE.Line3 = model.Line3;
                        visitAE.Line4 = model.Line4;
                        visitAE.DistrictUID = model.DistrictUID;
                        visitAE.AmphurUID = model.AmphurUID;
                        visitAE.ProvinceUID = model.ProvinceUID;
                        visitAE.ZipCode = model.ZipCode;
                        visitAE.PhoneNumber = model.PhoneNumber;
                        visitAE.MobileNumber = model.MobileNumber;
                        visitAE.Comments = model.Comments;
                        visitAE.CUser = userUID;
                        visitAE.CWhen = now;

                        db.PatientAEAdmission.AddOrUpdate(visitAE);
                        db.SaveChanges();
                    }

                    tran.Complete();
                }

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }

        }

        [Route("SaveAEDischargeEvent")]
        [HttpPost]
        public HttpResponseMessage SaveAEDischargeEvent(AEDischargeEventModel dischargemodel, int userUID)
        {
            try
            {
                using (var tran = new TransactionScope())
                {
                    DateTime now = DateTime.Now;

                    PatientVisit patientVisit = db.PatientVisit.Find(dischargemodel.PatientVisitUID);
                    if (patientVisit != null)
                    {
                        db.PatientVisit.Attach(patientVisit);
                        patientVisit.VISTSUID = 418; // Medical Discharge
                        patientVisit.MUser = userUID;
                        patientVisit.MWhen = now;
                        db.SaveChanges();
                    }

                    if (dischargemodel.DSCTYPUID == 4351 || dischargemodel.DSCTYPUID == 4352) //Dead Non autopsy, Dead Autopsy
                    {
                        PatientDeceasedDetail deceasedDetail = new PatientDeceasedDetail();
                        deceasedDetail.PatientUID = dischargemodel.PatientUID;
                        deceasedDetail.PatientVisitUID = dischargemodel.PatientVisitUID;
                        deceasedDetail.DeathDttm = dischargemodel.DeceasedDttm ?? now;
                        deceasedDetail.DeathTime = dischargemodel.DeceasedDttm ?? now;
                        deceasedDetail.Comments = dischargemodel.Comments;
                        deceasedDetail.OwnerOrganisationUID = dischargemodel.OwnerOrganisationUID;
                        deceasedDetail.CUser = userUID;
                        deceasedDetail.CWhen = now;
                        deceasedDetail.MUser = userUID;
                        deceasedDetail.MWhen = now;
                        deceasedDetail.StatusFlag = "A";

                        db.PatientDeceasedDetail.Add(deceasedDetail);
                        db.SaveChanges();

                        Patient patient = db.Patient.Find(dischargemodel.PatientUID);
                        if (patient != null)
                        {
                            db.Patient.Attach(patient);
                            patient.DeathStatus = "Y";
                            patient.DeathDttm = dischargemodel.DeceasedDttm ?? now;
                            patient.MUser = userUID;
                            patient.MWhen = DateTime.Now;
                            db.SaveChanges();
                        }

                        PatientAEAdmission patientAE = db.PatientAEAdmission.Find(dischargemodel.PatientAEAdmissionUID);
                        if (patientAE != null)
                        {
                            db.PatientAEAdmission.Attach(patientAE);
                            patientAE.IsDead = "Y";
                            patientAE.MUser = userUID;
                            patientAE.MWhen = DateTime.Now;
                            db.SaveChanges();
                        }
                    }

                    AEDischargeEvent aEDischarge = new AEDischargeEvent();
                    aEDischarge.PatientAEAdmissionUID = dischargemodel.PatientAEAdmissionUID;
                    aEDischarge.CheckOutDttm = dischargemodel.CheckoutDttm;
                    aEDischarge.DSCCNDUID = dischargemodel.DSCCNDUID;
                    aEDischarge.DSCTYPUID = dischargemodel.DSCTYPUID;
                    aEDischarge.DESTINUID = dischargemodel.DESTINUID;
                    aEDischarge.DeceasedDttm = dischargemodel.DeceasedDttm;
                    aEDischarge.ATSTYPUID = dischargemodel.ATSTYPUID;
                    aEDischarge.DischargeEvents = dischargemodel.DischargeEvents;
                    aEDischarge.RecordedBy = dischargemodel.RecordedBy;
                    aEDischarge.OwnerOrganisationUID = dischargemodel.OwnerOrganisationUID;
                    aEDischarge.Comments = dischargemodel.Comments;
                    aEDischarge.CUser = userUID;
                    aEDischarge.CWhen = now;
                    aEDischarge.MUser = userUID;
                    aEDischarge.MWhen = now;
                    aEDischarge.StatusFlag = "A";

                    db.AEDischargeEvent.Add(aEDischarge);
                    db.SaveChanges();

                    #region PatientServiceEvent
                    PatientServiceEvent serviceEvent = new PatientServiceEvent();
                    serviceEvent.PatientVisitUID = dischargemodel.PatientVisitUID;
                    serviceEvent.EventStartDttm = now;
                    serviceEvent.VISTSUID = 418;
                    serviceEvent.LocationUID = dischargemodel.LocationUID;
                    serviceEvent.MUser = userUID;
                    serviceEvent.MWhen = now;
                    serviceEvent.CUser = userUID;
                    serviceEvent.CWhen = now;
                    serviceEvent.StatusFlag = "A";

                    db.PatientServiceEvent.Add(serviceEvent);
                    db.SaveChanges();
                    #endregion

                    tran.Complete();
                }

                return Request.CreateResponse(HttpStatusCode.OK, dischargemodel);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("SaveIPBooking")]
        [HttpPost]
        public HttpResponseMessage SaveIPBooking(IPBookingModel iPBookingModel, int userUID)
        {
            try
            {
                using (var tran = new TransactionScope())
                {
                    DateTime now = DateTime.Now;

                    //PatientVisit patientVisit = db.PatientVisit.Find(iPBookingModel.PatientVisitUID);
                    //if (patientVisit != null)
                    //{
                    //    db.PatientVisit.Attach(patientVisit);
                    //    patientVisit.VISTSUID = 427; //Change Location
                    //    //patientVisit.ENTYPUID = iPBookingModel.LocationUID;
                    //    patientVisit.MUser = userUID;
                    //    patientVisit.MWhen = now;
                    //    db.SaveChanges();
                    //}

                    IPBooking iPBooking = new IPBooking();
                    iPBooking.PatientUID = iPBookingModel.PatientUID;
                    iPBooking.PatientVisitUID = iPBookingModel.PatientVisitUID;
                    iPBooking.LocationUID = iPBookingModel.LocationUID;
                    iPBooking.RequestedByUID = iPBookingModel.RequestedByUID;
                    iPBooking.RequestedByLocationUID = iPBookingModel.RequestedByLocationUID; // locationUID ที่ร้องขอ
                    iPBooking.SpecialityUID = iPBookingModel.SpecialityUID;
                    iPBooking.CareproviderUID = iPBookingModel.CareproviderUID;
                    iPBooking.BedUID = iPBookingModel.BedUID;
                    iPBooking.AdmissionDttm = iPBookingModel.AdmissionDttm;
                    iPBooking.ExpectedDischargeDttm = iPBookingModel.ExpectedDischargeDttm;
                    iPBooking.ExpectedLengthofStay = iPBookingModel.ExpectedLengthofStay ?? 0;
                    iPBooking.BookedDttm = now;
                    iPBooking.BKSTSUID = 0;
                    iPBooking.VISTYUID = iPBookingModel.VISTYUID;
                    iPBooking.BKTYPUID = iPBookingModel.BKTYPUID;
                    iPBooking.BDCATUID = 0; //ยังไม่มี referance value
                    iPBooking.ReferredBy = iPBookingModel.ReferredBy;
                    iPBooking.ReferredByUID = iPBookingModel.ReferredByUID;
                    iPBooking.Comments = iPBookingModel.Comments;
                    iPBooking.OwnerOrganisationUID = iPBookingModel.OwnerOrganisationUID;
                    iPBooking.CUser = userUID;
                    iPBooking.CWhen = now;
                    iPBooking.MUser = userUID;
                    iPBooking.MWhen = now;
                    iPBooking.StatusFlag = "A";

                    db.IPBooking.Add(iPBooking);
                    db.SaveChanges();

                    tran.Complete();
                }
                return Request.CreateResponse(HttpStatusCode.OK, iPBookingModel);
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
                    patientVisit.VISTYUID = patientVisitInfo.VISTYUID;
                    patientVisit.StartDttm = patientVisitInfo.StartDttm;
                    patientVisit.PRITYUID = patientVisitInfo.PRITYUID;
                    patientVisit.CareProviderUID = patientVisitInfo.CareProviderUID;
                    patientVisit.CheckupJobUID = patientVisitInfo.CheckupJobUID;
                    patientVisit.Comments = patientVisitInfo.Comments;
                    patientVisit.MUser = userID;
                    patientVisit.MWhen = DateTime.Now;
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
                            PatientName = SqlFunction.fGetPatientName(p.PatientUID),
                            PatientVisitUID = p.UID,
                            StartDttm = p.StartDttm,
                            EndDttm = p.EndDttm,
                            ArrivedDttm = p.ArrivedDttm,
                            CareProviderUID = p.CareProviderUID,
                            CareProviderName = SqlFunction.fGetCareProviderName(p.CareProviderUID ?? 0),
                            Comments = p.Comments,
                            VISTYUID = p.VISTYUID,
                            VISTSUID = p.VISTSUID,
                            IsBillFinalized = p.IsBillFinalized,
                            VisitStatus = SqlFunction.fGetRfValDescription(p.VISTSUID ?? 0),
                            VisitType = SqlFunction.fGetRfValDescription(p.VISTYUID ?? 0),
                            VisitID = p.VisitID,
                            PRITYUID = p.PRITYUID,
                            ENSTAUID = p.ENSTAUID ?? 0,
                            ENTYPUID = p.ENTYPUID,
                            BedUID = p.BedUID,
                            BedName = SqlFunction.fGetLocationName(p.BedUID ?? 0),
                            OwnerOrganisationUID = p.OwnerOrganisationUID ?? 0,
                            OwnerOrganisation = SqlFunction.fGetHealthOrganisationName(p.OwnerOrganisationUID ?? 0),
                            LocationUID = p.LocationUID,
                            LocationName = SqlFunction.fGetLocationName(p.LocationUID ?? 0),
                            CUser = p.CUser,
                            CreateBy = SqlFunction.fGetCareProviderName(p.CUser)
                        }).FirstOrDefault();

            return visitData;
        }

        [Route("GetPatientVisitToChangeLocation")]
        [HttpGet]
        public List<PatientVisitModel> GetPatientVisitToChangeLocation(long? patientUID, string visitID, DateTime? dateFrom, DateTime? dateTo)
        {
            List<PatientVisitModel> visitData = db.PatientVisit.Where(p => p.VISTSUID != 423 && p.VISTSUID != 418 
                                        && p.VISTSUID != 410 && p.VISTSUID != 421
                                        && (visitID == null || p.VisitID == visitID)
                                        && (patientUID == null || p.PatientUID == patientUID)
                                        && (dateFrom == null || DbFunctions.TruncateTime(p.StartDttm) >= DbFunctions.TruncateTime(dateFrom))
                                        && (dateTo == null || DbFunctions.TruncateTime(p.StartDttm) < DbFunctions.TruncateTime(dateTo))
                                        && p.StatusFlag == "A")
                                                    .Select(p => new PatientVisitModel
                                                    {
                                                        PatientUID = p.PatientUID,
                                                        PatientVisitUID = p.UID,
                                                        PatientName = SqlFunction.fGetPatientName(p.PatientUID),
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
                                                        ENTYPUID = p.ENTYPUID,
                                                        OwnerOrganisationUID = p.OwnerOrganisationUID ?? 0,
                                                        OwnerOrganisation = SqlFunction.fGetHealthOrganisationName(p.OwnerOrganisationUID ?? 0),
                                                        LocationUID = p.LocationUID,
                                                        LocationName = SqlFunction.fGetLocationName(p.LocationUID ?? 0)
                                                    }).ToList();

            return visitData;
        }

        [Route("GetPatientAEAdmissionByUID")]
        [HttpGet]
        public PatientAEAdmissionModel GetPatientAEAdmissionByUID(long patientVisitUID)
        {
            PatientAEAdmissionModel visitData = null;
            visitData = (from ae in db.PatientAEAdmission
                         join pv in db.PatientVisit on ae.PatientVisitUID equals pv.UID
                         join pt in db.Patient on pv.PatientUID equals pt.UID
                         where ae.StatusFlag == "A"
                         && pv.StatusFlag == "A"
                         && pt.StatusFlag == "A"
                         && pv.UID == patientVisitUID
                         select new PatientAEAdmissionModel
                         {
                             Uid = ae.UID,
                             PatientUid = ae.PatientUID,
                             PatientVisitUid = ae.PatientVisitUID,
                             ARRMDUID = ae.ARRMDUID,
                             RELTNUID = ae.RELTNUID,
                             ESCTPUID = ae.ESCTPUID,
                             VehicleNumber = ae.VehicleNumber,
                             EventOccuredDttm = ae.EmergencyOcurredDttm,
                             EMGTPUID = ae.EMGTPUID,
                             EMGCDUID = ae.EMGCDUID,
                             ProblemUID = ae.ProblemUID,
                             InjuryReason = ae.InjuryReason,
                             EmergencyExamDetail = ae.EmergencyExamDetail,
                             IsDead = ae.IsDead,
                             Line1 = ae.Line1,
                             Line2 = ae.Line2,
                             Line3 = ae.Line3,
                             Line4 = ae.Line4,
                             DistrictUID = ae.DistrictUID,
                             AmphurUID = ae.AmphurUID,
                             ProvinceUID = ae.ProvinceUID,
                             ZipCode = ae.ZipCode,
                             PhoneNumber = ae.PhoneNumber,
                             MobileNumber = ae.MobileNumber,
                             Comments = ae.Comments,
                             BedUID = pv.BedUID,
                             CareproviderUID = pv.CareProviderUID,
                             TITLEUID = pt.TITLEUID,
                             SEXXXUID = pt.SEXXXUID,
                             FirstName = pt.FirstName,
                             LastName = pt.LastName,
                             MiddelName = pt.MiddleName,
                             NickName = pt.NickName,
                             SPOKLUID = pt.SPOKLUID,
                             NATNLUID = pt.NATNLUID,
                             BLOODUID = pt.BLOODUID,
                             BirthDttm = pt.DOBDttm,
                             DOBComputed = pt.DOBComputed,
                             PatientID = pt.PatientID
                         }).FirstOrDefault();

            return visitData;
        }


        [Route("GetPatientVisitByPatientUID")]
        [HttpGet]
        public List<PatientVisitModel> GetPatientVisitByPatientUID(long patientUID)
        {
            List<PatientVisitModel> visitData = null;
            visitData = (from pv in db.PatientVisit
                         where pv.StatusFlag == "A"
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
                             ENTYPUID = pv.ENTYPUID,
                             IsBillFinalized = pv.IsBillFinalized,
                             VisitStatus = SqlFunction.fGetRfValDescription(pv.VISTYUID ?? 0),
                             VisitType = SqlFunction.fGetRfValDescription(pv.VISTYUID ?? 0),
                             OwnerOrganisation = SqlFunction.fGetHealthOrganisationName(pv.OwnerOrganisationUID ?? 0),
                             VisitID = pv.VisitID,
                             PRITYUID = pv.PRITYUID,
                             OwnerOrganisationUID = pv.OwnerOrganisationUID ?? 0,
                             LocationUID = pv.LocationUID,
                             LocationName = SqlFunction.fGetLocationName(pv.LocationUID ?? 0)
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
                        ENTYPUID = p.ENTYPUID,
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

        [Route("GetPatientVisitDispensed")]
        [HttpGet]
        public List<PatientVisitModel> GetPatientVisitDispensed(long patientUID)
        {
            List<PatientVisitModel> visitData = null;
            visitData = (from pv in db.PatientVisit
                         join prs in db.Prescription on pv.UID equals prs.PatientVisitUID
                         join pit in db.PrescriptionItem on prs.UID equals pit.PrescriptionUID
                         where pv.StatusFlag == "A"
                         && prs.StatusFlag == "A"
                         && pit.StatusFlag == "A"
                         && pv.PatientUID == patientUID
                         && pv.VISTSUID != 410
                         && pit.ORDSTUID == 2861 //Dispensed
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
                             ENTYPUID = pv.ENTYPUID,
                             IsBillFinalized = pv.IsBillFinalized,
                             VisitStatus = SqlFunction.fGetRfValDescription(pv.VISTYUID ?? 0),
                             VisitType = SqlFunction.fGetRfValDescription(pv.VISTYUID ?? 0),
                             OwnerOrganisation = SqlFunction.fGetHealthOrganisationName(pv.OwnerOrganisationUID ?? 0),
                             VisitID = pv.VisitID,
                             PRITYUID = pv.PRITYUID,
                             OwnerOrganisationUID = pv.OwnerOrganisationUID ?? 0,
                             LocationUID = pv.LocationUID,
                             LocationName = SqlFunction.fGetLocationName(pv.LocationUID ?? 0)
                         }).OrderByDescending(p => p.StartDttm).ToList();

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
                            ENTYPUID = p.ENTYPUID,
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



        [Route("GetLatestPatientVisitNonClose")]
        [HttpGet]
        public PatientVisitModel GetLatestPatientVisitNonClose(long patientUID)
        {
            PatientVisitModel visitData = null;
            visitData = db.PatientVisit.Where(p => p.PatientUID == patientUID
            && p.EndDttm == null
            && p.VISTSUID != 421
            && p.VISTSUID != 410 && p.StatusFlag == "A")
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
                            ENTYPUID = p.ENTYPUID,
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

        [Route("GetLatestPatientVisitToConvert")]
        [HttpGet]
        public PatientVisitModel GetLatestPatientVisitToConvert(long patientUID)
        {
            PatientVisitModel visitData = null;
            visitData = db.PatientVisit.Where(p => p.PatientUID == patientUID
            && p.EndDttm == null
            && p.VISTSUID != 421 && p.VISTSUID != 423 && p.VISTSUID != 418
            && p.VISTSUID != 410 && p.StatusFlag == "A")
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
                            ENTYPUID = p.ENTYPUID,
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
        public HttpResponseMessage ChangeVisitStatus(long patientVisitUID, int VISTSUID, int? careProviderUID, int? locationUID, DateTime? editDttm, int userID, int? AdmissionEventUID = null, int? ENSTAUID = null)
        {
            try
            {
                int FINDIS = 421;
                DateTime now = DateTime.Now;
                PatientVisit patientvisit = db.PatientVisit.Find(patientVisitUID);
                using (var tran = new TransactionScope())
                {
                    if (patientvisit != null)
                    {
                        db.PatientVisit.Attach(patientvisit);

                        patientvisit.CareProviderUID = careProviderUID != null ? careProviderUID : patientvisit.CareProviderUID;
                        patientvisit.VISTSUID = VISTSUID;
                        patientvisit.ENSTAUID = ENSTAUID != null ? ENSTAUID : patientvisit.ENSTAUID;
                        patientvisit.EndDttm = VISTSUID == FINDIS ? DateTime.Now : (DateTime?)null;
                        patientvisit.IsBillFinalized = VISTSUID == FINDIS ? "Y" : "N";
                        patientvisit.MUser = userID;
                        patientvisit.MWhen = now;

                        #region PatientServiceEvent
                        PatientServiceEvent serviceEvent = new PatientServiceEvent();
                        serviceEvent.PatientVisitUID = patientvisit.UID;
                        serviceEvent.EventStartDttm = editDttm ?? patientvisit.MWhen;
                        serviceEvent.LocationUID = locationUID;
                        serviceEvent.VISTSUID = ENSTAUID != null ? ENSTAUID ?? 0 : VISTSUID;
                        serviceEvent.MUser = userID;
                        serviceEvent.MWhen = now;
                        serviceEvent.CUser = userID;
                        serviceEvent.CWhen = now;
                        serviceEvent.StatusFlag = "A";

                        db.PatientServiceEvent.Add(serviceEvent);

                        //db.SaveChanges();
                        #endregion

                        #region PatientVisitCareProvider
                        string careproviderName = string.Empty;
                        if (careProviderUID != null)
                        {
                            careproviderName = (from cr in db.Careprovider
                                                where cr.UID == careProviderUID
                                                select new
                                                {
                                                    careproviderName = SqlFunction.fGetCareProviderName(cr.UID)
                                                }).FirstOrDefault()?.careproviderName;

                            PatientVisitCareProvider patientVisitCare = db.PatientVisitCareProvider.FirstOrDefault(p => p.PatientVisitUID == patientVisitUID && p.LocationUID == locationUID && p.StatusFlag == "A");
                            if (patientVisitCare != null)
                            {
                                db.PatientVisitCareProvider.Attach(patientVisitCare);
                                patientVisitCare.PACLSUID = VISTSUID;
                                patientVisitCare.CareproviderUID = careProviderUID;
                                patientVisitCare.CareProviderName = careproviderName;
                                patientVisitCare.LocationUID = locationUID;
                                patientVisitCare.MUser = userID;
                                patientVisitCare.MWhen = DateTime.Now;

                            }
                            else
                            {
                                patientVisitCare = new PatientVisitCareProvider();
                                db.PatientVisitCareProvider.Attach(patientVisitCare);
                                patientVisitCare.PatientVisitUID = patientvisit.UID;
                                patientVisitCare.StartDttm = editDttm ?? patientvisit.MWhen;
                                patientVisitCare.PACLSUID = VISTSUID;
                                patientVisitCare.CareproviderUID = careProviderUID;
                                patientVisitCare.CareProviderName = careproviderName;
                                patientVisitCare.LocationUID = locationUID;
                                patientVisitCare.CUser = userID;
                                patientVisitCare.CWhen = DateTime.Now;
                                patientVisitCare.MUser = userID;
                                patientVisitCare.MWhen = DateTime.Now;
                                patientVisitCare.OwnerOrganisationUID = patientvisit.OwnerOrganisationUID ?? 0;
                                patientVisitCare.StatusFlag = "A";

                                db.PatientVisitCareProvider.Add(patientVisitCare);
                            }
                        }
                        #endregion

                        #region PatientADTEvwnt
                        if (ENSTAUID != null)
                        {
                            PatientADTEvent patientADT = new PatientADTEvent();
                            patientADT.PatientUID = patientvisit.PatientUID;
                            patientADT.PatientVisitUID = patientvisit.UID;
                            patientADT.EVNTYUID = ENSTAUID ?? 0;
                            patientADT.EventOccuredDttm = now;
                            patientADT.OwnerOrganisationUID = patientvisit.OwnerOrganisationUID ?? 0;
                            patientADT.IdentifyingUID = AdmissionEventUID ?? 0;
                            patientADT.IdentifyingType = "ARRIVEDEVENT";
                            patientADT.MUser = userID;
                            patientADT.MWhen = now;
                            patientADT.CUser = userID;
                            patientADT.CWhen = now;
                            patientADT.StatusFlag = "A";

                            db.PatientADTEvent.Add(patientADT);
                        }
                        #endregion

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

        [Route("ChangeVisitLocation")]
        [HttpPut]
        public HttpResponseMessage ChangeVisitLocation(long patientVisitUID, int locaionUID, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                PatientVisit patientvisit = db.PatientVisit.Find(patientVisitUID);
                if (patientvisit != null)
                {
                    db.PatientVisit.Attach(patientvisit);

                    patientvisit.VISTSUID = 427; //ChangeLocation

                    patientvisit.LocationUID = locaionUID;
                    patientvisit.MUser = userID;
                    patientvisit.MWhen = now;

                    #region PatientServiceEvent
                    PatientServiceEvent serviceEvent = new PatientServiceEvent();
                    serviceEvent.PatientVisitUID = patientvisit.UID;
                    serviceEvent.EventStartDttm = now;
                    serviceEvent.VISTSUID = 427;
                    serviceEvent.LocationUID = locaionUID;
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

        [Route("GetBedByPatientVisit")]
        [HttpGet]
        public List<BedStatusModel> GetBedByPatientVisit(int parentLocationUID, int ENTYPUID)
        {
            var bed = db.Location.Where(p => p.ParentLocationUID == parentLocationUID).Select(p => new BedStatusModel()
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

            var data = (from pv in db.PatientVisit
                        join b in db.Location on pv.BedUID equals b.UID
                        join pae in db.PatientAEAdmission on pv.UID equals pae.PatientVisitUID
                        join pt in db.Patient on pv.PatientUID equals pt.UID
                        where pv.StatusFlag == "A"
                        && b.StatusFlag == "A"
                        && pae.StatusFlag == "A"
                        && pt.StatusFlag == "A"
                        && pv.ENTYPUID == ENTYPUID
                        && pv.VISTSUID != 410
                        && pv.VISTSUID != 418
                        && pv.VISTSUID != 421
                        select new BedStatusModel
                        {
                            LocationUID = b.UID,
                            Name = b.Name,
                            Description = b.Description,
                            LOTYPUID = b.LOTYPUID,
                            LCTSTUID = b.LCTSTUID,
                            ParentLocationUID = b.ParentLocationUID,
                            ActiveFrom = b.ActiveFrom,
                            ActiveTo = b.ActiveTo,
                            CUser = b.CUser,
                            CWhen = b.CWhen,
                            MUser = b.MUser,
                            EMRZONUID = b.EMZONEUID,
                            OwnerOrganisationUID = b.OwnerOrganisationUID,
                            IsTemporaryBed = b.IsTemporaryBed,
                            MWhen = b.MWhen,
                            StatusFlag = b.StatusFlag,
                            PatientName = SqlFunction.fGetPatientName(pv.PatientUID),
                            EMGTPUID = SqlFunction.fGetRfValDescription(pae.EMGTPUID ?? 0),
                            EMGCDUID = SqlFunction.fGetRfValDescription(pae.EMGCDUID ?? 0),
                            Level = SqlFunction.fGetRfValCode(pae.EMGCDUID ?? 0),
                            PatientID = pt.PatientID,
                            PatientUID = pt.UID,
                            AgeString = SqlFunction.fGetAgeString(pt.DOBDttm.Value),
                            EmergencyVisitDate = pae.EmergencyOcurredDttm,
                            PatientVisitUID = pv.UID,
                            AEAdmissionUID = pae.UID,
                            Gender = SqlFunction.fGetRfValDescription(pt.SEXXXUID ?? 0),
                            CareProviderName = SqlFunction.fGetCareProviderName(pv.CareProviderUID ?? 0),
                            OwnerOrganisation = SqlFunction.fGetHealthOrganisationName(b.OwnerOrganisationUID ?? 0),
                        }).ToList();

            if (data != null)
            {
                for (int i = 0; i < bed.Count; i++)
                {
                    var used = data.Where(p => p.LocationUID == bed[i].LocationUID).FirstOrDefault();
                    if (used != null)
                    {
                        bed[i] = used;
                        bed[i].BedIsUse = "Y";
                        bed[i].Isused = true;
                    }
                    else
                    {
                        bed[i].Isused = false;
                        bed[i].BedIsUse = "N";
                    }
                }
            }

            return bed;
        }

        [Route("GetWardView")]
        [HttpGet]
        public List<BedStatusModel> GetWardView(int parentLocationUID)
        {
            DataTable dataTable = SqlDirectStore.pGetWardView(parentLocationUID);

            List<BedStatusModel> data = dataTable.ToList<BedStatusModel>();

            return data;
        }


        [Route("GetBedWardView")]
        [HttpGet]
        public List<BedStatusModel> GetBedWardView(int parentLocationUID)
        {
            DateTime now = DateTime.Now;
            var bed = db.Location.Where(p => p.ParentLocationUID == parentLocationUID).Select(p => new BedStatusModel()
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

            var data = (from pv in db.PatientVisit
                        join b in db.Location on pv.BedUID equals b.UID
                        join ad in db.AdmissionEvent on pv.UID equals ad.PatientVisitUID
                        join pt in db.Patient on pv.PatientUID equals pt.UID
                        where pv.StatusFlag == "A"
                        && b.StatusFlag == "A"
                        && ad.StatusFlag == "A"
                        && pt.StatusFlag == "A"
                        && (pv.ENSTAUID != 4421 || pv.ENSTAUID != null)
                        && pv.VISTSUID != 410
                        //&& pv.VISTSUID != 418
                        && pv.VISTSUID != 421
                        //&& pv.VISTSUID != 423
                        select new BedStatusModel
                        {
                            LocationUID = b.UID,
                            Name = b.Name,
                            Description = b.Description,
                            LOTYPUID = b.LOTYPUID,
                            LCTSTUID = b.LCTSTUID,
                            ParentLocationUID = b.ParentLocationUID,
                            ActiveFrom = b.ActiveFrom,
                            ActiveTo = b.ActiveTo,
                            CUser = b.CUser,
                            CWhen = b.CWhen,
                            MUser = b.MUser,
                            EMRZONUID = b.EMZONEUID,
                            OwnerOrganisationUID = b.OwnerOrganisationUID,
                            IsTemporaryBed = b.IsTemporaryBed,
                            ENSTAUID = pv.ENSTAUID ?? 0,
                            EncounterStatus = SqlFunction.fGetRfValDescription(pv.ENSTAUID ?? 0),
                            MWhen = b.MWhen,
                            StatusFlag = b.StatusFlag,
                            PatientName = SqlFunction.fGetPatientName(pv.PatientUID),
                            PatientID = pt.PatientID,
                            PatientUID = pt.UID,
                            PatientVisitUID = pv.UID,
                            AdmissionEventUID = ad.UID,
                            AgeString = SqlFunction.fGetAgeString(pt.DOBDttm.Value),
                            AdmissionDate = ad.AdmissionDttm,
                            ExpDischargeDate = ad.ExpectedDischargeDttm,
                            IsExpectedDischarge = DbFunctions.TruncateTime(ad.ExpectedDischargeDttm.Value) == DbFunctions.TruncateTime(DateTime.Now) ? true : false,
                            IsBillingProgress = pv.VISTSUID == 423 ? true : false,
                            IsFinancial = pv.VISTSUID == 421 ? true : false,
                            Gender = SqlFunction.fGetRfValDescription(pt.SEXXXUID ?? 0),
                            CareproviderUID = pv.CareProviderUID ?? 0,
                            CareProviderName = SqlFunction.fGetCareProviderName(pv.CareProviderUID ?? 0),
                            OwnerOrganisation = SqlFunction.fGetHealthOrganisationName(b.OwnerOrganisationUID ?? 0),
                            IsVIP = pt.IsVIP ?? false
                        }).ToList();

            if (data != null)
            {
                for (int i = 0; i < bed.Count; i++)
                {
                    var used = data.Where(p => p.LocationUID == bed[i].LocationUID).FirstOrDefault();
                    if (used != null)
                    {
                        bed[i] = used;
                        bed[i].BedIsUse = "Y";
                        bed[i].Isused = true;
                    }
                    else
                    {
                        bed[i].Isused = false;
                        bed[i].BedIsUse = "N";
                    }
                }
            }
            return bed;
        }


        [Route("GetBedLocation")]
        [HttpGet]
        public List<LocationModel> GetBedLocation(int parentLocationUID, int? entypUID)
        {
            var bed = db.Location.Where(p => p.ParentLocationUID == parentLocationUID).Select(p => new LocationModel()
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

            var data = (from pv in db.PatientVisit
                        join b in db.Location on pv.BedUID equals b.UID
                        where pv.StatusFlag == "A"
                        && b.StatusFlag == "A"
                        //&& (pv.ENTYPUID == 4310
                        && (entypUID == null || pv.ENTYPUID == entypUID)
                        && pv.VISTSUID != 410
                        && pv.VISTSUID != 418
                        && pv.VISTSUID != 421
                        select new LocationModel
                        {
                            LocationUID = b.UID,
                            Name = b.Name,
                            Description = b.Description,
                            LOTYPUID = b.LOTYPUID,
                            LCTSTUID = b.LCTSTUID,
                            ParentLocationUID = b.ParentLocationUID,
                            ActiveFrom = b.ActiveFrom,
                            ActiveTo = b.ActiveTo,
                            CUser = b.CUser,
                            CWhen = b.CWhen,
                            MUser = b.MUser,
                            EMRZONUID = b.EMZONEUID,
                            OwnerOrganisationUID = b.OwnerOrganisationUID,
                            OwnerOrganisation = SqlFunction.fGetHealthOrganisationName(b.OwnerOrganisationUID ?? 0),
                            IsTemporaryBed = b.IsTemporaryBed,
                            MWhen = b.MWhen,
                            StatusFlag = b.StatusFlag,
                            //ParentLocationName = SqlFunction.fGetLocationName(b.ParentLocationUID ?? 0)
                        }).ToList();

            if (data != null)
            {
                for (int i = 0; i < bed.Count; i++)
                {
                    var used = data.Where(p => p.LocationUID == bed[i].LocationUID).FirstOrDefault();
                    if (used != null)
                    {
                        bed[i].BedIsUse = "Y";
                    }
                    else
                    {
                        bed[i].BedIsUse = "N";
                    }
                }
            }

            return bed;
        }

        #endregion

        #region PatientVisitPayor

        [Route("GetPatientVisitPayorByVisitUID")]
        [HttpGet]
        public List<PatientVisitPayorModel> GetPatientVisitPayorByVisitUID(int patientVisitUID)
        {
            List<PatientVisitPayorModel> data = (from pvp in db.PatientVisitPayor
                                                 join ag in db.PayorAgreement on pvp.PayorAgreementUID equals ag.UID
                                                 where pvp.PatientVisitUID == patientVisitUID
                                                 && pvp.StatusFlag == "A"
                                                 select new PatientVisitPayorModel
                                                 {
                                                     PatientVisitPayorUID = pvp.UID,
                                                     PatientUID = pvp.PatientUID,
                                                     PatientVisitUID = pvp.PatientVisitUID,
                                                     InsuranceCompanyUID = pvp.InsuranceCompanyUID,
                                                     PayorAgreementUID = pvp.PayorAgreementUID,
                                                     PayorDetailUID = pvp.PayorDetailUID,
                                                     PolicyMasterUID = pvp.PolicyMasterUID,
                                                     EligibleAmount = pvp.EligibileAmount,
                                                     FixedCopayAmount = pvp.FixedCopayAmount,
                                                     ClaimPercentage = pvp.ClaimPercentage,
                                                     ActiveFrom = pvp.ActiveFrom,
                                                     ActiveTo = pvp.ActiveTo,
                                                     ClaimAmount = pvp.ClaimAmount,
                                                     PAYRTPUID = pvp.PAYRTPUID,
                                                     Comment = pvp.Comment,
                                                     InsuranceName = pvp.InsuranceCompanyUID.HasValue ? SqlFunction.fGetInsuranceCompanyName(pvp.InsuranceCompanyUID.Value) : "",
                                                     AgreementName = ag.Name,
                                                     PBTYPUID = ag.PBTYPUID,
                                                     BLTYPUID = ag.BLTYPUID,
                                                     PrimaryPBLCTUID = ag.PrimaryPBLCTUID,
                                                     SecondaryPBLCTUID = ag.SecondaryPBLCTUID,
                                                     TertiaryPBLCTUID = ag.TertiaryPBLCTUID,
                                                     PayorName = SqlFunction.fGetPayorName(pvp.PayorDetailUID),
                                                     PolicyName = pvp.PolicyName,
                                                     PayorType = SqlFunction.fGetRfValDescription(pvp.PAYRTPUID ?? 0),
                                                     CoveredAmount = pvp.CoveredAmount,
                                                     StatusFlag = pvp.StatusFlag,
                                                     CreatedBy = SqlFunction.fGetCareProviderName(pvp.CUser)
                                                 }).ToList().OrderBy(p => int.Parse(p.PayorType ?? "99")).ToList();

            foreach (var item in data)
            {
                item.Description = item.PayorType + " " + item.InsuranceName + "-" + item.AgreementName;
            }



            return data;
        }


        [Route("ManagePatientVisitPayor")]
        [HttpPost]
        public HttpResponseMessage ManagePatientVisitPayor(List<PatientVisitPayorModel> patientVisitPayorList, int userUID)
        {
            try
            {
                DateTime now = DateTime.Now;
                using (var tran = new TransactionScope())
                {

                    if (patientVisitPayorList != null)
                    {
                        var deletedVisitPayors = patientVisitPayorList.Where(p => p.StatusFlag == "D");
                        var activedVisitPayors = patientVisitPayorList.Where(p => p.StatusFlag == "A");

                        foreach (var deletedItem in deletedVisitPayors)
                        {
                            PatientVisitPayor patientVisitPayor = db.PatientVisitPayor.Find(deletedItem.PatientVisitPayorUID);
                            if (patientVisitPayor != null)
                            {
                                db.PatientVisitPayor.Attach(patientVisitPayor);
                                patientVisitPayor.StatusFlag = "D";
                                patientVisitPayor.MUser = userUID;
                                patientVisitPayor.MWhen = now;

                                db.SaveChanges();
                            }
                        }

                        foreach (var inVisitPayor in activedVisitPayors)
                        {
                            PatientVisitPayor patientVisitPayor = db.PatientVisitPayor.Find(inVisitPayor.PatientVisitPayorUID);
                            if (patientVisitPayor == null)
                            {
                                patientVisitPayor = new PatientVisitPayor();
                                patientVisitPayor.CUser = userUID;
                                patientVisitPayor.CWhen = now;
                            }

                            patientVisitPayor.MUser = userUID;
                            patientVisitPayor.MWhen = now;
                            patientVisitPayor.StatusFlag = "A";
                            patientVisitPayor.PatientUID = inVisitPayor.PatientUID;
                            patientVisitPayor.PatientVisitUID = inVisitPayor.PatientVisitUID;
                            patientVisitPayor.PayorDetailUID = inVisitPayor.PayorDetailUID;
                            patientVisitPayor.PayorAgreementUID = inVisitPayor.PayorAgreementUID;
                            patientVisitPayor.InsuranceCompanyUID = inVisitPayor.InsuranceCompanyUID;
                            patientVisitPayor.PolicyMasterUID = inVisitPayor.PolicyMasterUID;
                            patientVisitPayor.PolicyName = inVisitPayor.PolicyName;
                            patientVisitPayor.EligibileAmount = inVisitPayor.EligibleAmount;
                            patientVisitPayor.PAYRTPUID = inVisitPayor.PAYRTPUID;
                            patientVisitPayor.ActiveFrom = inVisitPayor.ActiveFrom;
                            patientVisitPayor.ActiveTo = inVisitPayor.ActiveTo;
                            patientVisitPayor.ClaimPercentage = inVisitPayor.ClaimPercentage;
                            patientVisitPayor.FixedCopayAmount = inVisitPayor.FixedCopayAmount;
                            patientVisitPayor.CoveredAmount = inVisitPayor.CoveredAmount;
                            patientVisitPayor.ClaimAmount = inVisitPayor.ClaimAmount;
                            patientVisitPayor.Comment = inVisitPayor.Comment;

                            db.PatientVisitPayor.AddOrUpdate(patientVisitPayor);
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
            , long? patientUID, int? bookStatus, int? PATMSGUID, int? ownerOrganisationUID, int? locationUID)
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
                                          && (locationUID == null || bki.LocationUID == locationUID)
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
                                           OwnerOrganisationName = SqlFunction.fGetHealthOrganisationName(bki.OwnerOrganisationUID),
                                           PATMSGUID = bki.PATMSGUID,
                                           PatientReminderMessage = SqlFunction.fGetRfValDescription(bki.PATMSGUID ?? 0),
                                           LocationUID = bki.LocationUID,
                                           Location = SqlFunction.fGetLocationName(bki.LocationUID)
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
                                                  LocationUID = bki.LocationUID,
                                                  Location = SqlFunction.fGetLocationName(bki.LocationUID)
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
                data.LocationUID = bki.LocationUID;
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
                bki.LocationUID = model.LocationUID;

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

        #region IPD
        [Route("SearchIPBooking")]
        [HttpGet]
        public List<IPBookingModel> SearchIPBooking(string patientID, DateTime? dateFrom, DateTime? dateTo, int? bktypUID, int? wardUID)
        {
            DataTable dataTable = SqlDirectStore.SearchIPBooking(patientID, dateFrom, dateTo, bktypUID, wardUID);

            List<IPBookingModel> data = dataTable.ToList<IPBookingModel>();

            return data;
        }

        [Route("GetIPBookingByVisitUID")]
        [HttpGet]
        public IPBookingModel GetIPBookingByVisitUID(long patientVisitUID)
        {

            var data = db.IPBooking.Where(p => p.StatusFlag == "A")
                .Select(p => new IPBookingModel
                {
                    BookingUID = p.UID,
                    PatientUID = p.PatientUID,
                    PatientVisitUID = p.PatientVisitUID,
                    LocationUID = p.LocationUID,
                    SpecialityUID = p.SpecialityUID,
                    CareproviderUID = p.CareproviderUID,
                    BedUID = p.BedUID,
                    AdmissionDttm = p.AdmissionDttm,
                    ExpectedDischargeDttm = p.ExpectedDischargeDttm,
                    ExpectedLengthofStay = p.ExpectedLengthofStay,
                    BookedDttm = p.BookedDttm,
                    BKSTSUID = p.BKSTSUID,
                    VISTYUID = p.VISTYUID,
                    BKTYPUID = p.BKTYPUID,
                    BDCATUID = p.BDCATUID,
                    ReferredBy = p.ReferredBy,
                    ReferredByUID = p.ReferredByUID,
                    RequestedByLocationUID = p.RequestedByLocationUID,
                    RequestedByUID = p.RequestedByUID,
                    CANRSUID = p.CANRSUID,
                    CancelledBy = p.CancelledBy,
                    CancelledDttm = p.CancelledDttm,
                    Comments = p.Comments,
                    OwnerOrganisationUID = p.OwnerOrganisationUID
                }).FirstOrDefault();

            return data;
        }

        [Route("ChangeStatusIPBooking")]
        [HttpPut]
        public HttpResponseMessage ChangeStatusIPBooking(long ipBookingUID, int statusUID, int userID)
        {
            try
            {
                IPBooking booking = db.IPBooking.Find(ipBookingUID);
                if (booking != null)
                {
                    db.IPBooking.Attach(booking);
                    booking.MUser = userID;
                    booking.MWhen = DateTime.Now;
                    booking.BKTYPUID = statusUID;

                    db.SaveChanges();
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("DropIPBooking")]
        [HttpPut]
        public HttpResponseMessage DropIPBooking(long ipBookingUID, int userUID)
        {
            try
            {
                IPBooking booking = db.IPBooking.Find(ipBookingUID);
                if (booking != null)
                {
                    db.IPBooking.Attach(booking);
                    booking.MUser = userUID;
                    booking.MWhen = DateTime.Now;
                    booking.BKTYPUID = 5358;

                    db.SaveChanges();
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("GetAdmissionEventByPatientVisitUID")]
        [HttpGet]
        public AdmissionEventModel GetAdmissionEventByPatientVisitUID(long PatientVisitUID)
        {
            AdmissionEventModel data = db.AdmissionEvent.Where(p => p.PatientVisitUID == PatientVisitUID)
                .Select(p => new AdmissionEventModel
                {
                    AdmissionEventUID = p.UID,
                    PatientUID = p.PatientUID,
                    PatientVisitUID = p.PatientVisitUID,
                    CarepoviderUID = p.CareproviderUID,
                    ExpectedLengthOfStay = p.ExpectedLengthOfStay,
                    AdmissionDttm = p.AdmissionDttm,
                    ExpectedDischargeDttm = p.ExpectedDischargeDttm,
                    IPBookingUID = p.IPBookingUID,
                    PreviousEventUID = p.PreviousEventUID,
                    Comments = p.Comments,
                    OwnerOrganisationUID = p.OwnerOrganisationUID,
                    RequestingLocationUID = p.RequestingLocationUID,
                    ValidToDttm = p.ValidToDttm,
                    ValidFromDttm = p.ValidFromDttm,
                    IsNoVisitorAllowed = p.IsNoVisitorAllowed
                }).FirstOrDefault();

            return data;
        }

        [Route("ManageAdmissionEvent")]
        [HttpPost]
        public HttpResponseMessage ManageAdmissionEvent(AdmissionEventModel model, int userUID)
        {
            try
            {
                using (var tran = new TransactionScope())
                {
                    DateTime now = DateTime.Now;

                    AdmissionEvent admissionEvent = db.AdmissionEvent.Find(model.AdmissionEventUID);

                    if (admissionEvent == null)
                    {
                        admissionEvent = new AdmissionEvent();
                        admissionEvent.CUser = userUID;
                        admissionEvent.CWhen = now;
                    }

                    admissionEvent.PatientUID = model.PatientUID;
                    admissionEvent.PatientVisitUID = model.PatientVisitUID;
                    admissionEvent.CareproviderUID = model.CarepoviderUID;
                    //admissionEvent.ExpectedLengthOfStay = model.ExpectedLengthOfStay;
                    admissionEvent.AdmissionDttm = model.AdmissionDttm;
                    admissionEvent.IPBookingUID = model.IPBookingUID;
                    admissionEvent.PreviousEventUID = model.PreviousEventUID;
                    admissionEvent.Comments = model.Comments;
                    admissionEvent.OwnerOrganisationUID = model.OwnerOrganisationUID;
                    admissionEvent.ValidFromDttm = model.ValidFromDttm;
                    admissionEvent.ValidToDttm = model.ValidToDttm;
                    admissionEvent.IsNoVisitorAllowed = model.IsNoVisitorAllowed;
                    admissionEvent.RequestingLocationUID = model.RequestingLocationUID;
                    admissionEvent.CUser = userUID;
                    admissionEvent.CWhen = now;
                    admissionEvent.StatusFlag = "A";


                    db.AdmissionEvent.AddOrUpdate(admissionEvent);
                    db.SaveChanges();

                    tran.Complete();
                }
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("ManageIPDDischargeEvent")]
        [HttpPost]
        public HttpResponseMessage ManageIPDDischargeEvent(DischargeEventModel dischargemodel, int userUID)
        {
            try
            {
                DateTime now = DateTime.Now;
                PatientVisit patientVisit = db.PatientVisit.Find(dischargemodel.PatientVisitUID);
                if (patientVisit != null)
                {
                    db.PatientVisit.Attach(patientVisit);
                    if (dischargemodel.VISTSUID != null)
                    {
                        patientVisit.VISTSUID = dischargemodel.VISTSUID;
                    }
                    patientVisit.ENSTAUID = dischargemodel.ENSTAUID;
                    patientVisit.MUser = userUID;
                    patientVisit.MWhen = now;
                    db.SaveChanges();
                }

                if (dischargemodel.DSCSTUID == 4436 || dischargemodel.DSCSTUID == 4436) //Dead Stillbirth, Dead
                {
                    PatientDeceasedDetail deceasedDetail = new PatientDeceasedDetail();
                    deceasedDetail.PatientUID = dischargemodel.PatientUID;
                    deceasedDetail.PatientVisitUID = dischargemodel.PatientVisitUID;
                    deceasedDetail.DeathDttm = dischargemodel.MedicalDischargeDttm ?? now;
                    deceasedDetail.DeathTime = dischargemodel.MedicalDischargeDttm ?? now;
                    deceasedDetail.Comments = dischargemodel.DischargeComments;
                    deceasedDetail.OwnerOrganisationUID = dischargemodel.OwnerOrganisationUID;
                    deceasedDetail.CUser = userUID;
                    deceasedDetail.CWhen = now;
                    deceasedDetail.MUser = userUID;
                    deceasedDetail.MWhen = now;
                    deceasedDetail.StatusFlag = "A";

                    db.PatientDeceasedDetail.Add(deceasedDetail);
                    db.SaveChanges();

                    Patient patient = db.Patient.Find(dischargemodel.PatientUID);
                    if (patient != null)
                    {
                        db.Patient.Attach(patient);
                        patient.DeathStatus = "Y";
                        patient.DeathDttm = dischargemodel.MedicalDischargeDttm ?? now;
                        patient.MUser = userUID;
                        patient.MWhen = DateTime.Now;
                        db.Patient.AddOrUpdate(patient);
                        db.SaveChanges();
                    }

                }

                DischargeEvent dischargeEvent = db.DischargeEvent.Find(dischargemodel.DischargeEventUID);

                if (dischargeEvent == null)
                {
                    dischargeEvent = new DischargeEvent();
                    dischargeEvent.CUser = userUID;
                    dischargeEvent.CWhen = now;
                    dischargeEvent.StatusFlag = "A";
                }

                dischargeEvent.AdmissionEventUID = dischargemodel.AdmissionEventUID;
                dischargeEvent.DSCSTUID = dischargemodel.DSCSTUID;
                dischargeEvent.MedicalDischargeDttm = dischargemodel.MedicalDischargeDttm;
                dischargeEvent.ActualDischargeDttm = dischargemodel.ActualDischargeDttm;
                dischargeEvent.RecordedBy = dischargemodel.RecordedBy;
                dischargeEvent.DischargeComments = dischargemodel.DischargeComments;
                dischargeEvent.MDTRNUID = dischargemodel.MDTRNUID;
                dischargeEvent.DSGDSUID = dischargemodel.DSGDSUID;
                dischargeEvent.DSCTYUID = dischargemodel.DSCTYUID;
                dischargeEvent.INFCTUID = dischargemodel.INFCTUID;
                dischargeEvent.AdvicedBy = dischargemodel.AdvicedBy;
                dischargeEvent.DischargeAdviceDttm = dischargemodel.DischargeAdviceDttm;
                dischargeEvent.DSHTYPUID = dischargemodel.DSHTYPUID;
                dischargeEvent.DSOCMUID = dischargemodel.DSOCMUID;
                dischargeEvent.CancelledDttm = dischargemodel.CancelledDttm;
                dischargeEvent.CARNSUID = dischargemodel.CARNSUID;
                dischargeEvent.CancelledBy = dischargemodel.CancelledBy;
                dischargeEvent.OwnerOrganisationUID = dischargemodel.OwnerOrganisationUID;
                dischargeEvent.MUser = userUID;
                dischargeEvent.MWhen = now;
                dischargeEvent.StatusFlag = "A";

                db.DischargeEvent.AddOrUpdate(dischargeEvent);
                db.SaveChanges();

                PatientADTEvent patientADT = new PatientADTEvent();
                patientADT.PatientUID = dischargemodel.PatientUID;
                patientADT.PatientVisitUID = dischargemodel.PatientVisitUID;
                patientADT.OwnerOrganisationUID = dischargemodel.OwnerOrganisationUID;
                patientADT.EventOccuredDttm = now;
                patientADT.EVNTYUID = dischargemodel.ENSTAUID ?? 0;
                patientADT.IdentifyingUID = dischargeEvent.UID;
                patientADT.IdentifyingType = "DISCHARGEEVENT";
                patientADT.CUser = userUID;
                patientADT.CWhen = now;
                patientADT.MUser = userUID;
                patientADT.MWhen = now;
                patientADT.StatusFlag = "A";

                db.PatientADTEvent.Add(patientADT);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("GetPatientADTEventType")]
        [HttpGet]
        public PatientADTEventModel GetPatientADTEventType(long patientVisitUID, string type)
        {
            PatientADTEventModel data = db.PatientADTEvent.Where(p => p.PatientVisitUID == patientVisitUID
            && p.IdentifyingType == type
            && p.StatusFlag == "A")
                .Select(p => new PatientADTEventModel
                {
                    PatientADTEventUID = p.UID,
                    PatientUID = p.PatientUID,
                    PatientVisitUID = p.PatientVisitUID,
                    ENVTYUID = p.EVNTYUID,
                    ENVTY = SqlFunction.fGetRfValDescription(p.EVNTYUID),
                    EventOccuredDttm = p.EventOccuredDttm,
                    OwnerOrganisationUID = p.OwnerOrganisationUID,
                    IdentifyingUID = p.IdentifyingUID,
                    IdentifyingType = p.IdentifyingType
                }).FirstOrDefault();

            return data;
        }

        [Route("CancelDischargeEvent")]
        [HttpPost]
        public HttpResponseMessage CancelDischargeEvent(DischargeEventModel model, int userUID)
        {
            try
            {
                DateTime now = DateTime.Now;

                PatientVisit patientVisit = db.PatientVisit.Find(model.PatientVisitUID);
                if (patientVisit != null)
                {
                    db.PatientVisit.Attach(patientVisit);
                    patientVisit.ENSTAUID = model.ENSTAUID ?? 0;
                    patientVisit.VISTSUID = model.VISTSUID != null ? model.VISTSUID : patientVisit.VISTSUID;
                    patientVisit.MUser = userUID;
                    patientVisit.MWhen = now;
                    db.SaveChanges();
                }

                DischargeEvent dischargeEvent = db.DischargeEvent.Where(p => p.AdmissionEventUID == model.AdmissionEventUID && p.StatusFlag == "A").FirstOrDefault();
                if (dischargeEvent != null)
                {
                    db.DischargeEvent.Attach(dischargeEvent);
                    dischargeEvent.CancelledBy = model.CancelledBy;
                    dischargeEvent.CancelledDttm = model.CancelledDttm;
                    dischargeEvent.CARNSUID = model.CARNSUID;
                    dischargeEvent.StatusFlag = "D";
                    dischargeEvent.MUser = userUID;
                    dischargeEvent.MWhen = now;
                    db.SaveChanges();
                }

                PatientADTEvent patientADTEvent = db.PatientADTEvent.Where(p => p.IdentifyingUID == dischargeEvent.UID && p.StatusFlag == "A").FirstOrDefault();
                if (patientADTEvent != null)
                {
                    db.PatientADTEvent.Attach(patientADTEvent);
                    patientADTEvent.StatusFlag = "D";
                    patientADTEvent.MUser = userUID;
                    patientADTEvent.MWhen = now;
                    db.SaveChanges();
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("CancelAdmission")]
        [HttpPut]
        public HttpResponseMessage CancelAdmission(int patientVisitUID, int locationUID, int statusUID, int userUID)
        {
            try
            {
                DateTime now = DateTime.Now;

                List<PatientVisitID> patientVisitID = db.PatientVisitID.Where(p => p.PatientVisitUID == patientVisitUID && p.StatusFlag == "A").ToList();
                if (patientVisitID.Count != 0)
                {
                    if (patientVisitID.Count == 1)
                    {
                        PatientVisit model = db.PatientVisit.Find(patientVisitUID);
                        db.PatientVisit.Attach(model);
                        model.StatusFlag = "C";
                        model.MUser = userUID;
                        model.MWhen = now;
                        db.SaveChanges();
                    }

                    PatientVisitID visitID = patientVisitID.Where(p => p.Identifier.Contains("I")).FirstOrDefault();
                    db.PatientVisitID.Attach(visitID);
                    visitID.MUser = userUID;
                    visitID.CWhen = now;
                    visitID.StatusFlag = "D";
                    db.SaveChanges();
                }

                AdmissionEvent admissionEvent = db.AdmissionEvent.Where(p => p.PatientVisitUID == patientVisitUID && p.StatusFlag == "A").FirstOrDefault();
                if (admissionEvent != null)
                {
                    db.AdmissionEvent.Attach(admissionEvent);
                    admissionEvent.MUser = userUID;
                    admissionEvent.CWhen = now;
                    admissionEvent.StatusFlag = "D";
                    db.SaveChanges();
                }

                List<PatientADTEvent> events = db.PatientADTEvent.Where(p => p.PatientVisitUID == patientVisitUID && p.StatusFlag == "A").ToList();
                if (events != null)
                {
                    foreach (var item in events)
                    {
                        db.PatientADTEvent.Attach(item);
                        item.MUser = userUID;
                        item.CWhen = now;
                        item.StatusFlag = "D";
                        db.SaveChanges();
                    }
                }

                PatientServiceEvent serviceEvent = new PatientServiceEvent();
                serviceEvent.PatientVisitUID = patientVisitUID;
                serviceEvent.EventStartDttm = now;
                serviceEvent.VISTSUID = statusUID;
                serviceEvent.LocationUID = locationUID;
                serviceEvent.MUser = userUID;
                serviceEvent.MWhen = now;
                serviceEvent.CUser = userUID;
                serviceEvent.CWhen = now;
                serviceEvent.StatusFlag = "A";

                db.PatientServiceEvent.Add(serviceEvent);
                db.SaveChanges();



                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }

        }

        [Route("GetDischargeEventByAdmissionUID")]
        [HttpGet]
        public DischargeEventModel GetDischargeEventByAdmissionUID(long admissionEventUID)
        {
            DischargeEventModel data = db.DischargeEvent.Where(p => p.AdmissionEventUID == admissionEventUID && p.StatusFlag == "A")
                .Select(p => new DischargeEventModel()
                {
                    DischargeEventUID = p.UID,
                    AdmissionEventUID = p.AdmissionEventUID,
                    DSCSTUID = p.DSCSTUID,
                    MedicalDischargeDttm = p.MedicalDischargeDttm,
                    ActualDischargeDttm = p.ActualDischargeDttm,
                    RecordedBy = p.RecordedBy,
                    DischargeComments = p.DischargeComments,
                    MDTRNUID = p.MDTRNUID,
                    DSGDSUID = p.DSGDSUID,
                    DSCTYUID = p.DSCTYUID,
                    INFCTUID = p.INFCTUID,
                    AdvicedBy = p.AdvicedBy,
                    DischargeAdviceDttm = p.DischargeAdviceDttm,
                    DSHTYPUID = p.DSHTYPUID,
                    DSOCMUID = p.DSOCMUID,
                    CARNSUID = p.CARNSUID,
                    CancelledBy = p.CancelledBy,
                    CancelledDttm = p.CancelledDttm,
                    OwnerOrganisationUID = p.OwnerOrganisationUID
                }).FirstOrDefault();

            return data;
        }

        [Route("EditExpDischarged")]
        [HttpPost]
        public HttpResponseMessage EditExpDischarged(AdmissionEventModel admissionModel, int userUID)
        {
            try
            {
                DateTime now = DateTime.Now;
                AdmissionEvent admissionEvent = db.AdmissionEvent.Find(admissionModel.AdmissionEventUID);
                if (admissionEvent != null)
                {
                    db.AdmissionEvent.Attach(admissionEvent);
                    admissionEvent.ExpectedDischargeDttm = admissionModel.ExpectedDischargeDttm;
                    admissionEvent.MUser = userUID;
                    admissionEvent.CWhen = now;
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

        #region Patient DemographicLog

        [Route("GetPatientDemographicLogByUID")]
        [HttpGet]
        public List<PatientDemographicLogModel> GetPatientDemographicLogByUID(long patientUID)
        {
            List<PatientDemographicLogModel> data = db.PatientDemographicLog.Where(p => p.PatientUID == patientUID && p.StatusFlag == "A")
                .Select(p => new PatientDemographicLogModel()
                {
                    UID = p.UID,
                    PatientUID = p.PatientUID ?? 0,
                    FiledName = p.FiledName,
                    TableName = p.TableName,
                    OldValue = p.OldValue,
                    NewValue = p.NewValue,
                    Modifiedby = p.Modifiedby ?? 0,
                    ModifiedDttm = p.ModifiedDttm,
                    ModifiedbyName = SqlFunction.fGetCareProviderName(p.Modifiedby ?? 0)
                }).OrderByDescending(p => p.ModifiedDttm).ToList();

            return data;
        }

        #endregion

        #region Patient Tracking

        [Route("GetPatientServiceEventByUID")]
        [HttpGet]
        public List<PatientServiceEventModel> GetPatientServiceEventByUID(long patientVisitUID)
        {
            List<PatientServiceEventModel> data = db.PatientServiceEvent.Where(
                p => p.PatientVisitUID == patientVisitUID && p.StatusFlag == "A")
                .Select(p => new PatientServiceEventModel()
                {
                    PatientServiceEventUID = p.UID,
                    PatientVistUID = p.PatientVisitUID,
                    EventStartDttm = p.EventStartDttm,
                    VISTSUID = p.VISTSUID,
                    VisitStatus = SqlFunction.fGetRfValDescription(p.VISTSUID),
                    LocationUID = p.LocationUID ?? 0,
                    Location = SqlFunction.fGetLocationName(p.LocationUID ?? 0),
                    UserName = SqlFunction.fGetCareProviderName(p.CUser)
                }).OrderBy(p => p.EventStartDttm).ToList();
            return data;
        }

        #endregion

        #region Consult

        [Route("GetAppointmentRequestbyUID")]
        [HttpGet]
        public List<AppointmentRequestModel> GetAppointmentRequestbyUID(int patientUID, int patientVisitUID, int BKSTSUID)
        {
            List<AppointmentRequestModel> data = db.AppointmentRequest.Where(p => p.PatientUID == patientUID
                                            && p.PatientVisitUID == patientVisitUID
                                            && p.BKSTSUID == BKSTSUID
                                            && p.StatusFlag == "A")
                                            .Select(p => new AppointmentRequestModel()
                                            {
                                                AppointmentRequestUID = p.UID,
                                                PatientUID = p.PatientUID,
                                                PatientVisitUID = p.PatientVisitUID ?? 0,
                                                PatientName = SqlFunction.fGetPatientName(p.PatientUID),
                                                AppointmentDttm = p.AppointmentDttm,
                                                Comments = p.Comments,
                                                CareProviderUID = p.CareproviderUID,
                                                CareProviderName = SqlFunction.fGetCareProviderName(p.CareproviderUID ?? 0),
                                                BKSTSUID = p.BKSTSUID ?? 0,
                                                RequestStatus = SqlFunction.fGetRfValDescription(p.BKSTSUID ?? 0),
                                                OwnerOrganisationUID = p.OwnerOrganisationUID,
                                                OwnerOrganisationName = SqlFunction.fGetHealthOrganisationName(p.OwnerOrganisationUID),
                                                LocationUID = p.LocationUID ?? 0,
                                                LocationName = SqlFunction.fGetLocationName(p.LocationUID ?? 0)
                                            }).ToList();

            return data;
        }

        [Route("ManageAppointmentRequest")]
        [HttpPost]
        public HttpResponseMessage ManageAppointmentRequest(List<AppointmentRequestModel> appointmentRequests, int userUID)
        {
            try
            {
                DateTime now = DateTime.Now;
                foreach (var request in appointmentRequests)
                {
                    AppointmentRequest detail = db.AppointmentRequest.Find(request.AppointmentRequestUID);
                    if (detail == null)
                    {
                        detail = new AppointmentRequest();
                        detail.CUser = userUID;
                        detail.CWhen = now;
                    }

                    if (detail != null && request.StatusFlag == "A")
                    {
                        detail.PatientUID = request.PatientUID;
                        detail.PatientVisitUID = request.PatientVisitUID;
                        detail.AppointmentDttm = request.AppointmentDttm;
                        detail.CareproviderUID = request.CareProviderUID;
                        detail.BKSTSUID = request.BKSTSUID;
                        detail.RequestedBy = request.RequestedBy;
                        detail.RequestedDate = request.RequestedDate;
                        detail.Comments = request.Comments;
                        detail.LocationUID = request.LocationUID;
                        detail.OwnerOrganisationUID = request.OwnerOrganisationUID;
                        detail.MUser = userUID;
                        detail.MWhen = now;
                        detail.StatusFlag = request.StatusFlag;
                    }
                    else
                    {
                        detail.StatusFlag = request.StatusFlag;
                        detail.MUser = userUID;
                        detail.MWhen = now;
                    }

                    db.AppointmentRequest.AddOrUpdate(detail);
                    db.SaveChanges();
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("SearchAppointmentRequest")]
        [HttpGet]
        public List<AppointmentRequestModel> SearchAppointmentRequest(DateTime? dateFrom, DateTime? dateTo, int? locationUID, int? bookStatus, int? ownerOrganisationUID, int? careproviderUID)
        {
            var status = db.ReferenceValue.Where(p => p.DomainCode == "BKSTS" && p.StatusFlag == "A").ToList();
            int requestStatus = status.FirstOrDefault(p => p.ValueCode == "REQTED").UID;
            int dropStatus = status.FirstOrDefault(p => p.ValueCode == "CANCD").UID;
            int arrivedStatus = status.FirstOrDefault(p => p.ValueCode == "ARRVD").UID;

            List<AppointmentRequestModel> data = new List<AppointmentRequestModel>();

            if (bookStatus == arrivedStatus)
            {
                data = (from app in db.AppointmentRequest
                        join pa in db.Patient on app.PatientUID equals pa.UID
                        join p in db.PatientVisitCareProvider on app.PatientVisitUID equals p.PatientVisitUID into pvc
                        from p in pvc.DefaultIfEmpty()
                        where app.StatusFlag == "A"
                           && (dateFrom == null || DbFunctions.TruncateTime(app.AppointmentDttm) >= DbFunctions.TruncateTime(dateFrom))
                           && (dateTo == null || DbFunctions.TruncateTime(app.AppointmentDttm) < DbFunctions.TruncateTime(dateTo))
                           && (careproviderUID == null || app.CareproviderUID == careproviderUID)
                           && (app.BKSTSUID != requestStatus && app.BKSTSUID != dropStatus)
                           && (ownerOrganisationUID == null || app.OwnerOrganisationUID == ownerOrganisationUID)
                           && (locationUID == null || app.LocationUID == locationUID)
                           && (p.LocationUID != locationUID)
                        select new AppointmentRequestModel
                        {
                            AppointmentRequestUID = app.UID,
                            PatientUID = app.PatientUID,
                            PatientVisitUID = app.PatientVisitUID ?? 0,
                            PatientName = SqlFunction.fGetPatientName(app.PatientUID),
                            AppointmentDttm = app.AppointmentDttm,
                            Comments = app.Comments,
                            CareProviderUID = app.CareproviderUID,
                            CareProviderName = SqlFunction.fGetCareProviderName(app.CareproviderUID ?? 0),
                            BKSTSUID = app.BKSTSUID ?? 0,
                            RequestStatus = SqlFunction.fGetRfValDescription(app.BKSTSUID ?? 0),
                            OwnerOrganisationUID = app.OwnerOrganisationUID,
                            OwnerOrganisationName = SqlFunction.fGetHealthOrganisationName(app.OwnerOrganisationUID),
                            LocationUID = app.LocationUID ?? 0,
                            LocationName = SqlFunction.fGetLocationName(app.LocationUID ?? 0),
                            IsCheckin = app.BKSTSUID != requestStatus ? true : false
                        }).ToList();
            }
            else
            {
                 data = (from app in db.AppointmentRequest
                         join pa in db.Patient on app.PatientUID equals pa.UID
                         join p in db.PatientVisitCareProvider on app.PatientVisitUID equals p.PatientVisitUID into pvc
                         from p in pvc.DefaultIfEmpty()
                         where app.StatusFlag == "A"
                            && (dateFrom == null || DbFunctions.TruncateTime(app.AppointmentDttm) >= DbFunctions.TruncateTime(dateFrom))
                            && (dateTo == null || DbFunctions.TruncateTime(app.AppointmentDttm) < DbFunctions.TruncateTime(dateTo))
                            && (careproviderUID == null || app.CareproviderUID == careproviderUID)
                            && (bookStatus == null || app.BKSTSUID == bookStatus)
                            && (ownerOrganisationUID == null || app.OwnerOrganisationUID == ownerOrganisationUID)
                            && (locationUID == null || app.LocationUID == locationUID)
                            && (p.LocationUID != locationUID)
                         select new AppointmentRequestModel
                         {
                             AppointmentRequestUID = app.UID,
                             PatientUID = app.PatientUID,
                             PatientVisitUID = app.PatientVisitUID ?? 0,
                             PatientName = SqlFunction.fGetPatientName(app.PatientUID),
                             AppointmentDttm = app.AppointmentDttm,
                             Comments = app.Comments,
                             CareProviderUID = app.CareproviderUID,
                             CareProviderName = SqlFunction.fGetCareProviderName(app.CareproviderUID ?? 0),
                             BKSTSUID = app.BKSTSUID ?? 0,
                             RequestStatus = SqlFunction.fGetRfValDescription(app.BKSTSUID ?? 0),
                             OwnerOrganisationUID = app.OwnerOrganisationUID,
                             OwnerOrganisationName = SqlFunction.fGetHealthOrganisationName(app.OwnerOrganisationUID),
                             LocationUID = app.LocationUID ?? 0,
                             LocationName = SqlFunction.fGetLocationName(app.LocationUID ?? 0),
                             IsCheckin = app.BKSTSUID != requestStatus ? true : false
                         }).ToList();
            }

            return data;
        }

        [Route("ChangeAppointmentRequest")]
        [HttpPut]
        public HttpResponseMessage ChangeAppointmentRequest(int appointmentRequestUID, int statusUID, int userID)
        {
            try
            {
                AppointmentRequest requestModel = db.AppointmentRequest.Find(appointmentRequestUID);
                if (requestModel != null)
                {
                    db.AppointmentRequest.Attach(requestModel);
                    requestModel.BKSTSUID = statusUID;
                    requestModel.MUser = userID;
                    requestModel.MWhen = DateTime.Now;

                    db.SaveChanges();
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("SavePatientVisitCareprovider")]
        [HttpPost]
        public HttpResponseMessage SavePatientVisitCareprovider(PatientVisitCareproviderModel patientVisitCareprovider, int appointmentRequestUID)
        {
            try
            {
                DateTime now = DateTime.Now;

                AppointmentRequest appointment = db.AppointmentRequest.Find(appointmentRequestUID);
                if (appointment != null)
                {
                    db.AppointmentRequest.Attach(appointment);
                    appointment.BKSTSUID = patientVisitCareprovider.PACLSUID;
                    appointment.MUser = patientVisitCareprovider.CUser;
                    appointment.MWhen = DateTime.Now;

                    db.SaveChanges();
                }

                PatientVisit patientVisit = db.PatientVisit.Find(patientVisitCareprovider.PatientVisitUID);
                if (patientVisit != null)
                {
                    db.PatientVisit.Attach(patientVisit);
                    patientVisit.VISTSUID = patientVisitCareprovider.PACLSUID;
                    patientVisit.MUser = patientVisitCareprovider.CUser;
                    patientVisit.MWhen = DateTime.Now;

                    db.SaveChanges();
                }

                PatientVisitCareProvider detail = new PatientVisitCareProvider();
                detail.PatientVisitUID = patientVisitCareprovider.PatientVisitUID;
                detail.StartDttm = patientVisitCareprovider.StartDttm;
                detail.CareproviderUID = patientVisitCareprovider.CareProviderUID;
                detail.CareProviderName = patientVisitCareprovider.CareProviderName;
                detail.PACLSUID = patientVisitCareprovider.PACLSUID;
                detail.ReferralUID = patientVisitCareprovider.ReferralUID;
                detail.BookingUID = patientVisitCareprovider.BookingUID;
                detail.LocationUID = patientVisitCareprovider.LocationUID;
                detail.OwnerOrganisationUID = patientVisitCareprovider.OwnerOrganisationUID;
                detail.MUser = patientVisitCareprovider.CUser;
                detail.MWhen = now;
                detail.CUser = patientVisitCareprovider.CUser;
                detail.CWhen = now;
                detail.StatusFlag = "A";

                db.PatientVisitCareProvider.Add(detail);
                db.SaveChanges();


                #region PatientServiceEvent
                PatientServiceEvent serviceEvent = new PatientServiceEvent();
                serviceEvent.PatientVisitUID = patientVisitCareprovider.PatientVisitUID;
                serviceEvent.EventStartDttm = now;
                serviceEvent.VISTSUID = patientVisitCareprovider.PACLSUID ?? 0;
                serviceEvent.LocationUID = patientVisitCareprovider.LocationUID;
                serviceEvent.MUser = patientVisitCareprovider.CUser;
                serviceEvent.MWhen = now;
                serviceEvent.CUser = patientVisitCareprovider.CUser;
                serviceEvent.CWhen = now;
                serviceEvent.StatusFlag = "A";

                db.PatientServiceEvent.Add(serviceEvent);
                db.SaveChanges();
                #endregion

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("ChangePatientVisitCareproviderStatus")]
        [HttpPut]
        public HttpResponseMessage ChangePatientVisitCareproviderStatus(int patientVisitCareproviderUID, int statusUID, int userID)
        {
            try
            {
                PatientVisitCareProvider patientVisitCare = db.PatientVisitCareProvider.Find(patientVisitCareproviderUID);
                if (patientVisitCare != null)
                {
                    db.PatientVisitCareProvider.Attach(patientVisitCare);
                    patientVisitCare.PACLSUID = statusUID;
                    patientVisitCare.MUser = userID;
                    patientVisitCare.MWhen = DateTime.Now;

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

        #region Patient Alert

        [Route("GetPatientAlertByPatientUID")]
        [HttpGet]
        public List<PatientAlertModel> GetPatientAlertByPatientUID(long patientUID, long? patientVisitUID)
        {
            List<PatientAlertModel> data = db.PatientAlert.Where(p => p.PatientUID == patientUID
                                            && (patientVisitUID == null || p.PatientVisitUID == patientVisitUID)
                                            && p.PatientUID == patientUID
                                            && p.StatusFlag == "A")
                                            .Select(p => new PatientAlertModel()
                                            {
                                                PatientAlertUID = p.UID,
                                                PatientUID = p.PatientUID,
                                                PatientVisitUID = p.PatientVisitUID,
                                                AlertDescription = p.AlertDescription,
                                                ALRTYUID = p.ALRTYUID,
                                                AlertType = SqlFunction.fGetRfValDescription(p.ALRTYUID ?? 0),
                                                ALTSTUID = p.ALTSTUID,
                                                Alert = SqlFunction.fGetRfValDescription(p.ALTSTUID ?? 0),
                                                SEVTYUID = p.SEVTYUID,
                                                Severity = SqlFunction.fGetRfValDescription(p.SEVTYUID ?? 0),
                                                OnsetDttm = p.OnsetDttm,
                                                ClosureDttm = p.ClosureDttm,
                                                ALRPRTUID = p.ALRPRTUID,
                                                Priority = SqlFunction.fGetRfValDescription(p.ALRPRTUID ?? 0),
                                                OwnerOrganisationUID = p.OwnerOrganisationUID,
                                                OwnerOrganisation = SqlFunction.fGetHealthOrganisationName(p.OwnerOrganisationUID),
                                                LocationUID = p.LocationUID ?? 0,
                                                Location = SqlFunction.fGetLocationName(p.LocationUID ?? 0),
                                                StatusFlag = p.StatusFlag
                                            }).ToList();

            return data;
        }

        [Route("ManagePatientAlert")]
        [HttpPost]
        public HttpResponseMessage ManagePatientAlert(List<PatientAlertModel> patientModel, int userUID)
        {
            try
            {
                DateTime now = DateTime.Now;
                foreach (var patientAlert in patientModel)
                {
                    PatientAlert detail = db.PatientAlert.Find(patientAlert.PatientAlertUID);
                    if (detail == null)
                    {
                        detail = new PatientAlert();
                        detail.CUser = userUID;
                        detail.CWhen = now;
                    }

                    detail.PatientUID = patientAlert.PatientUID;
                    detail.PatientVisitUID = patientAlert.PatientVisitUID != 0 ? patientAlert.PatientVisitUID : null;
                    detail.AlertDescription = patientAlert.AlertDescription;
                    detail.ALRPRTUID = patientAlert.ALRPRTUID;
                    detail.ALRTYUID = patientAlert.ALRTYUID;
                    detail.ALTSTUID = patientAlert.ALTSTUID;
                    detail.SEVTYUID = patientAlert.SEVTYUID;
                    detail.OnsetDttm = patientAlert.OnsetDttm;
                    detail.ClosureDttm = patientAlert.ClosureDttm;
                    detail.LocationUID = patientAlert.LocationUID != 0 ? patientAlert.LocationUID : null;
                    detail.IsVisitSpecific = patientAlert.IsVisitSpecific;
                    detail.OwnerOrganisationUID = patientAlert.OwnerOrganisationUID;
                    detail.MUser = userUID;
                    detail.MWhen = now;
                    detail.StatusFlag = patientAlert.StatusFlag;

                    db.PatientAlert.AddOrUpdate(detail);
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

                            if (!list.ContainsKey(property.Name) && property.Name != "OwnerOrganisationUID")
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