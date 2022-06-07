using MediTech.DataBase;
using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ShareLibrary;
using System.Data.Entity;
using MediTech.Model.Report;
using static MediTech.Model.Report.PatientSumByAreaModel;

namespace MediTechWebApi.Controllers
{
    [RoutePrefix("Api/Report")]
    public class ReportsController : ApiController
    {
        protected MediTechEntities db = new MediTechEntities();

        #region Reports
        [Route("GetReports")]
        [HttpGet]
        public List<ReportsModel> GetReports()
        {
            List<ReportsModel> data = db.Reports
                .Where(p => p.StatusFlag == "A")
                .Select(p => new ReportsModel
                {
                    ReportsUID = p.UID,
                    Description = p.Description,
                    Name = p.Name,
                    NamespaceName = p.NamespaceName,
                    ReportGroup = p.ReportGroup,
                    ViewCode = p.ViewCode
                }).ToList();

            return data;
        }

        [Route("GetReportsByGroup")]
        [HttpGet]
        public List<ReportsModel> GetReportsByGroup(string reportGroup)
        {
            List<ReportsModel> data = db.Reports
                .Where(p => p.StatusFlag == "A" && p.ReportGroup == reportGroup)
                .Select(p => new ReportsModel
                {
                    ReportsUID = p.UID,
                    Description = p.Description,
                    Name = p.Name,
                    NamespaceName = p.NamespaceName,
                    ReportGroup = p.ReportGroup,
                    ViewCode = p.ViewCode
                }).ToList();

            return data;
        }
        #endregion

        #region Cashier
        [Route("GetStockToEcount")]
        [HttpGet]
        public List<EcountExportModel> GetStockToEcount(DateTime dateFrom, DateTime dateTo, int? vistyuid, string organisationList)
        {
            List<EcountExportModel> data = null;
            DataTable dt = SqlDirectStore.pRPTStockToEcount(dateFrom, dateTo, vistyuid, organisationList);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<EcountExportModel>();
                data = dt.ToList<EcountExportModel>();
            }

            return data;
        }


        [Route("GetEcountSumGroupReceipt")]
        [HttpGet]
        public List<EcountExportModel> GetEcountSumGroupReceipt(DateTime dateFrom, DateTime dateTo, int? vistyuid, string organisationList)
        {
            List<EcountExportModel> data = null;
            DataTable dt = SqlDirectStore.pRPTEcoutSumGroupReceipt(dateFrom, dateTo, vistyuid, organisationList);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<EcountExportModel>();
                data = dt.ToList<EcountExportModel>();
            }

            return data;
        }


        [Route("GetPatientNetProfit")]
        [HttpGet]
        public List<PatientRevenueModel> GetPatientNetProfit(DateTime dateFrom, DateTime dateTo, int? vistyuid, string organisationList)
        {
            List<PatientRevenueModel> data = null;
            DataTable dt = SqlDirectStore.pRPTPatientNetProfit(dateFrom, dateTo, vistyuid, organisationList);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<PatientRevenueModel>();
                data = dt.ToList<PatientRevenueModel>();
            }

            return data;
        }




        [Route("GetRevenuePerDay")]
        [HttpGet]
        public List<RevenuePerDayModel> GetRevenuePerDay(DateTime billGeneratedDttm, string organisationList)
        {
            List<RevenuePerDayModel> data = null;
            DataTable dt = SqlDirectStore.pRPTRevenuePerDay(billGeneratedDttm, organisationList);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<RevenuePerDayModel>();
                data = dt.ToList<RevenuePerDayModel>();
            }

            return data;
        }



        [Route("GetPayorDetailByDate")]
        [HttpGet]
        public List<PayorDetailModel> GetPayorDetailByDate(DateTime dateFrom, DateTime dateTo)
        {
            List<PayorDetailModel> data = db.PayorDetail.Where(p => p.StatusFlag == "A"&& ( DbFunctions.TruncateTime(p.CWhen) >= DbFunctions.TruncateTime(dateFrom))
                                             && ( DbFunctions.TruncateTime(p.CWhen) <= DbFunctions.TruncateTime(dateTo)))
                .Select(p => new PayorDetailModel
                {
                    Code = p.Code,
                    PayorName = p.PayorName,
                    Address1 = p.Address1,
                    Address2 = p.Address2,
                    MobileNumber = p.MobileNumber
                    ,PhoneNumber = p.PhoneNumber
                    ,Email = p.EmailAddress 
                    ,StatusFlag = p.StatusFlag

                }).ToList();

            return data;
        }





        [Route("GetPayorSummeryCount")]
        [HttpGet]
        public List<PatientVisitModel> GetPayorSummeryCount(DateTime billGeneratedDttm, string vistyuids, int organisationUID)
        {
            List<PatientVisitModel> data = new List<PatientVisitModel>();
            var listvisittype = vistyuids.Split(',').Select(Int32.Parse).ToList();
            var patientVisit = (from pv in db.PatientVisit
                                join pvp in db.PatientVisitPayor on pv.UID equals pvp.PatientVisitUID
                                join pb in db.PatientBill on pv.UID equals pb.PatientVisitUID
                                where pv.StatusFlag == "A"
                                && pvp.StatusFlag == "A"
                                && pb.StatusFlag == "A"
                                && (pb.IsRefund == null || pb.IsRefund != "Y")
                                && listvisittype.Contains(pv.VISTYUID ?? 0)
                                && pb.OwnerOrganisationUID == organisationUID
                                && DbFunctions.TruncateTime(pb.BillGeneratedDttm) == DbFunctions.TruncateTime(billGeneratedDttm)
                                select new PatientVisitModel
                                {
                                    PayorDetailUID = pvp.PayorDetailUID,
                                    PayorName = SqlFunction.fGetPayorName(pvp.PayorDetailUID),
                                    VISTYUID = pv.VISTYUID,
                                    VisitType = SqlFunction.fGetRfValDescription(pv.VISTYUID ?? 0)
                                }).ToList();

            if (patientVisit != null && patientVisit.Count() > 0)
                data = patientVisit
                    .GroupBy(p => new { p.VISTYUID, p.PayorDetailUID })
                    .Select(p => new PatientVisitModel
                    {
                        VISTYUID = p.FirstOrDefault().VISTYUID,
                        VisitType = p.FirstOrDefault().VisitType,
                        PayorDetailUID = p.FirstOrDefault().PayorDetailUID,
                        PayorName = p.FirstOrDefault().PayorName,
                        RowNumber = p.Count()
                    }).ToList();

            return data;
        }

        [Route("GetUsedReport")]
        [HttpGet]
        public List<PatientRevenueModel> GetUsedReport(DateTime dateFrom, DateTime dateTo, string organisationList)
        {
            List<PatientRevenueModel> data = null;
            DataTable dt = SqlDirectStore.pRPTUsedReport(dateFrom, dateTo, organisationList);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<PatientRevenueModel>();
                data = dt.ToList<PatientRevenueModel>();
            }

            return data;
        }


        [Route("GetDrugStoreNetProfit")]
        [HttpGet]
        public List<PatientRevenueModel> GetDrugStoreNetProfit(DateTime dateFrom, DateTime dateTo, int? organisationUID)
        {
            List<PatientRevenueModel> data = null;
            DataTable dt = SqlDirectStore.pRPTDrugStoreNetProfit(dateFrom, dateTo, organisationUID);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<PatientRevenueModel>();
                data = dt.ToList<PatientRevenueModel>();
            }

            return data;
        }

        [Route("GetDoctorfeeReport")]
        [HttpGet]
        public List<DoctorFeeReportModel> GetDoctorfeeReport(DateTime dateFrom, DateTime dateTo, int? careproviderUID)
        {
            List<DoctorFeeReportModel> data = null;
            DataTable dt = SqlDirectStore.pRPTDoctorFee(dateFrom, dateTo, careproviderUID);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<DoctorFeeReportModel>();
                data = dt.ToList<DoctorFeeReportModel>();
            }
            return data;
        }
        #endregion

        #region Patient

        [Route("PatientProblemStaistic")]
        [HttpGet]
        public List<ProblemStatisticModel> PatientProblemStaistic(DateTime dateFrom, DateTime dateTo, int? vistyuid, string organisationList)
        {
            List<ProblemStatisticModel> data = null;
            DataTable dt = SqlDirectStore.pRPTPatientProblemStatistic(dateFrom, dateTo, vistyuid, organisationList);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<ProblemStatisticModel>();
                data = dt.ToList<ProblemStatisticModel>();

            }

            return data;
        }

        #endregion

        #region Registration

        [Route("PatientSummaryPerMonth")]
        [HttpGet]
        public List<PatientSummaryModel> PatientSummaryPerMonth(int year, string monthLists, string organisationList)
        {
            List<PatientSummaryModel> data = null;
            DataTable dt = SqlDirectStore.pRPTPatientSummaryPerMonth(year, monthLists, organisationList);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<PatientSummaryModel>();
                foreach (DataRow item in dt.Rows)
                {
                    PatientSummaryModel newRow = new PatientSummaryModel();
                    newRow.Male = item["MALE"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(item["MALE"]);
                    newRow.Female = item["Female"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(item["Female"]);
                    newRow.Unknown = item["UNKNoWn"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(item["UNKNoWn"]);
                    newRow.Child = item["child"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(item["child"]);
                    newRow.Adut = item["Adult"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(item["Adult"]);
                    newRow.age0_4 = item["age 0-4"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(item["age 0-4"]);
                    newRow.age5_9 = item["age 5-9"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(item["age 5-9"]);
                    newRow.age10_14 = item["age 10-14"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(item["age 10-14"]);
                    newRow.age15_19 = item["age 15-19"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(item["age 15-19"]);
                    newRow.age20_24 = item["age 20-24"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(item["age 20-24"]);
                    newRow.age25_29 = item["age 25-29"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(item["age 25-29"]);
                    newRow.age30_34 = item["age 30-34"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(item["age 30-34"]);
                    newRow.age35_39 = item["age 35-39"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(item["age 35-39"]);
                    newRow.age40_44 = item["age 40-44"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(item["age 40-44"]);
                    newRow.age45_49 = item["age 45-49"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(item["age 45-49"]);
                    newRow.age50_54 = item["age 50-54"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(item["age 50-54"]);
                    newRow.age55_59 = item["age 55-59"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(item["age 55-59"]);
                    newRow.age60_64 = item["age 60-64"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(item["age 60-64"]);
                    newRow.age65_69 = item["age 65-69"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(item["age 65-69"]);
                    newRow.age70 = item["age >=70"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(item["age >=70"]);
                    newRow.Thai = item["Thai"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(item["Thai"]);
                    newRow.ThaiNew = item["Thai New"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(item["Thai New"]);
                    newRow.ThaiOld = item["Thai Old"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(item["Thai Old"]);
                    newRow.Foreign = item["Foreign"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(item["Foreign"]);
                    newRow.ForeignNew = item["Foreign New"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(item["Foreign New"]);
                    newRow.ForeignOld = item["Foreign Old"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(item["Foreign Old"]);
                    newRow.NoInputNation = item["No Input Nation"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(item["No Input Nation"]);
                    newRow.NoInputNationNew = item["No Input Nation New"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(item["No Input Nation New"]);
                    newRow.NoInputNationOld = item["No Input Nation Old"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(item["No Input Nation Old"]);
                    newRow.Counsult = item["Consult"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(item["Consult"]);
                    newRow.Repeat = item["Repeat"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(item["Repeat"]);
                    newRow.Year = item["Year"].ToString();
                    newRow.Month = int.Parse(item["Month"].ToString());
                    newRow.MonthName = item["MonthName"].ToString();
                    data.Add(newRow);
                }

            }

            return data;
        }

        [Route("VisitDaysStatistic")]
        [HttpGet]
        public List<ChartStatisticModel> VisitDaysStatistic(DateTime dateFrom, DateTime dateTo, int? vistyuid, string organisationList)
        {
            List<ChartStatisticModel> data = null;
            DataTable dt = SqlDirectStore.pRPTVisitDaysStatistic(dateFrom, dateTo, vistyuid, organisationList);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<ChartStatisticModel>();
                foreach (DataRow item in dt.Rows)
                {
                    ChartStatisticModel newRow = new ChartStatisticModel();
                    newRow.DisplayName = item["Day"].ToString();
                    newRow.Argument = item["DayName"].ToString();
                    newRow.Value = Convert.ToInt32(item["PatientCount"]);
                    data.Add(newRow);
                }

            }

            return data;
        }


        [Route("VisitTimesStatistic")]
        [HttpGet]
        public List<ChartStatisticModel> VisitTimesStatistic(DateTime dateFrom, DateTime dateTo, int? vistyuid, string organisationList)
        {
            List<ChartStatisticModel> data = null;
            DataTable dt = SqlDirectStore.pRPTVisitTimesStatistic(dateFrom, dateTo, vistyuid, organisationList);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<ChartStatisticModel>();
                foreach (DataRow item in dt.Rows)
                {
                    ChartStatisticModel newRow = new ChartStatisticModel();
                    newRow.Argument = item["Time"].ToString();
                    newRow.Value = Convert.ToInt32(item["PatientCount"]);
                    data.Add(newRow);
                }

            }

            return data;
        }

        [Route("PatientSumByAreaPerMonth")]
        [HttpGet]
        public List<PatientSumByAreaModel> PatientSumByAreaPerMonth(DateTime dateFrom, DateTime dateTo, int? vistyuid, string organisationList)
        {
            List<PatientSumByAreaModel> data = null;
            DataSet ds = SqlDirectStore.pRPTPatientSumByAreaPerMonth(dateFrom, dateTo, vistyuid, organisationList);

            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    data = ds.Tables[0].ToList<PatientSumByAreaModel>();

                    if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {
                        var tempMoo = ds.Tables[1].ToList<DetailAreaModel>();
                        foreach (var item in data)
                        {
                            item.ListMooNo = tempMoo.Where(p => p.DistrictName == item.DistrictName).ToList();
                        }
                    }
                }
            }


            return data;
        }

        [Route("PatientSummaryData")]
        [HttpGet]
        public List<PatientSummaryDataModel> PatientSummaryData(DateTime dateFrom, DateTime dateTo, string organisationList)
        {
            List<PatientSummaryDataModel> data = null;
            DataTable dt = SqlDirectStore.pRPTPatientSummaryData(dateFrom, dateTo, organisationList);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<PatientSummaryDataModel>();
                data = dt.ToList<PatientSummaryDataModel>();

            }

            return data;
        }
        #endregion


        [Route("PrintOPDCard")]
        [HttpGet]
        public List<OPDCardModel> PrintOPDCard(long patientUID, long patientVisitUID)
        {
            List<OPDCardModel> opdCardData = null;
            DataTable dt = SqlDirectStore.pPrintOPDCard(patientUID, patientVisitUID);

            if (dt != null && dt.Rows.Count > 0)
            {
                opdCardData = new List<OPDCardModel>();
                opdCardData = dt.ToList<OPDCardModel>();

                foreach (var item in opdCardData)
                {
                    item.PateintProblem = new List<PatientProblemModel>();
                    item.PatientDrugDetail = new List<PatientOrderDetailModel>();

                    var patProm = (new PatientDiagnosticsController()).GetPatientProblemByVisitUID(patientVisitUID);
                    var drugDeatail = (new OrderProcessingController()).GetOrderDrugByVisitUID(patientVisitUID);
                    if (patProm != null && patProm.Count > 0)
                    {
                        item.PateintProblem = patProm;
                    }

                    if (drugDeatail != null && drugDeatail.PatientOrderDetail != null)
                    {
                        foreach (var itemDrug in drugDeatail.PatientOrderDetail)
                        {
                            item.PatientDrugDetail.Add(itemDrug);
                        }
                    }
                }
            }

            return opdCardData;
        }


        [Route("PrintPatientBooking")]
        [HttpGet]
        public PatientBookingModel PrintPatientBooking(int bookUID)
        {
            PatientBookingModel data = (from pa in db.Patient
                                        join pv in db.PatientVisit on pa.UID equals pv.PatientUID
                                        join b in db.Booking on pa.UID equals b.PatientUID
                                        where
                                         pa.StatusFlag == "A"
                                         && pv.StatusFlag == "A"
                                         && b.StatusFlag == "A"
                                         && b.UID == bookUID
                                        //&& pv.UID == patientVisitUID
                                        select new PatientBookingModel
                                        {
                                            No = pv.UID,
                                            VisitID = pv.VisitID,
                                            Age = pa.DOBDttm != null ? SqlFunction.fGetAgeString(pa.DOBDttm.Value) : "",
                                            Doctor = SqlFunction.fGetCareProviderName(b.CareProviderUID ?? 0),
                                            Gender = SqlFunction.fGetRfValDescription(pa.SEXXXUID ?? 0),
                                            PatientID = pa.PatientID,
                                            PatientName = SqlFunction.fGetPatientName(pa.UID),
                                            AppointmentDttm = b.AppointmentDttm,
                                            Comments = b.Comments,
                                            CUserAppointment = SqlFunction.fGetCareProviderName(b.CUser),
                                            PatientReminderMessage = SqlFunction.fGetRfValDescription(b.PATMSGUID ?? 0),
                                            DrugAllergy = SqlFunction.fGetPatientAllergy(pa.UID),
                                            DOB = pa.DOBDttm.ToString(),
                                            strVisitData = pv.StartDttm.ToString(),
                                        }).FirstOrDefault();


            //if (db.PatientAllergy.Where(w=>w.PatientUID == patientUID && w.StatusFlag == "A").ToList().Count > 0 )
            //{
            //    //List<DetailAllergy> test = (from pa in db.Patient
            //    //                            join al in db.PatientAllergy on pa.UID equals al.PatientUID
            //    //                            where al.PatientUID == patientUID && pa.StatusFlag == "A" && al.StatusFlag == "A"
            //    //                            select al.AllergicTo ).ToList();


            //    //List<DetailAllergy> test  = db.PatientAllergy.Where(w => w.PatientUID == patientUID && w.StatusFlag == "A").Select(p => new DetailAllergy() { Detail = p.AllergicTo }).ToList();
            //    //foreach (var item in test)
            //    //{
            //    //    data.DrugAllergy.Add(item.Detail);
            //    //}

            //}

            return data;
        }

        [Route("PrintMedicalCertificate")]
        [HttpGet]
        public MedicalCertificateModel PrintMedicalCertificate(long patientVisitUID)
        {
            MedicalCertificateModel data = (from pa in db.Patient
                                            join pv in db.PatientVisit on pa.UID equals pv.PatientUID
                                            join pvp in db.PatientVisitPayor on pv.UID equals pvp.PatientVisitUID
                                            where pa.StatusFlag == "A"
                                             && pv.StatusFlag == "A"
                                             && pv.UID == patientVisitUID
                                             && pvp.StatusFlag == "A"
                                            select new MedicalCertificateModel
                                            {
                                                No = pv.UID,
                                                IDCard = pa.IDCard,
                                                VisitID = pv.VisitID,
                                                AgeString = pa.DOBDttm != null ? SqlFunction.fGetAgeString(pa.DOBDttm.Value) : "",
                                                AgeYear = pa.DOBDttm != null ? SqlFunction.fGetAge(pa.DOBDttm.Value) : "",
                                                Gender = SqlFunction.fGetRfValDescription(pa.SEXXXUID ?? 0),
                                                strVisitData = pv.StartDttm,
                                                PatientID = pa.PatientID,
                                                DateOfBirth = pa.DOBDttm,
                                                PatientName = SqlFunction.fGetPatientName(pa.UID),
                                                Doctor = SqlFunction.fGetCareProviderName(pv.CareProviderUID ?? 0),
                                                DoctorEngName = SqlFunction.fGetCareProviderEngName(pv.CareProviderUID ?? 0),
                                                DoctorLicenseNo = SqlFunction.fGetCareProviderLicenseNo(pv.CareProviderUID ?? 0),
                                                PatientAddress = SqlFunction.fGetAddressPatient(pv.PatientUID),
                                                PatientPayor = SqlFunction.fGetPayorName(pvp.PayorDetailUID),
                                                MobilePhone = pa.MobilePhone,
                                                PatientEmail = pa.Email

                                            }).FirstOrDefault();

            return data;
        }

        [Route("PrintConfinedSpaceCertificate")]
        [HttpGet]
        public MedicalCertificateModel PrintConfinedSpaceCertificate(long patientVisitUID)
        {

            MedicalCertificateModel data = (from pa in db.Patient
                                            join pv in db.PatientVisit on pa.UID equals pv.PatientUID
                                            join pvs in db.PatientVitalSign on
                                            new
                                            {
                                                key1 = pv.UID,
                                                key2 = "A"
                                            } equals
                                            new
                                            {
                                                key1 = pvs.PatientVisitUID,
                                                key2 = pvs.StatusFlag
                                            } into lefpvs
                                            from j in lefpvs.DefaultIfEmpty()
                                            where pa.StatusFlag == "A"
                                             && pv.StatusFlag == "A"
                                             && pv.UID == patientVisitUID
                                            select new MedicalCertificateModel
                                            {
                                                No = pv.UID,
                                                IDCard = pa.IDCard,
                                                VisitID = pv.VisitID,
                                                AgeString = pa.DOBDttm != null ? SqlFunction.fGetAgeString(pa.DOBDttm.Value) : "",
                                                AgeYear = pa.DOBDttm != null ? SqlFunction.fGetAge(pa.DOBDttm.Value) : "",
                                                Gender = SqlFunction.fGetRfValDescription(pa.SEXXXUID ?? 0),
                                                strVisitData = pv.StartDttm,
                                                PatientID = pa.PatientID,
                                                DateOfBirth = pa.DOBDttm,
                                                PatientName = SqlFunction.fGetPatientName(pa.UID),
                                                Doctor = SqlFunction.fGetCareProviderName(pv.CareProviderUID ?? 0),
                                                DoctorEngName = SqlFunction.fGetCareProviderEngName(pv.CareProviderUID ?? 0),
                                                DoctorLicenseNo = SqlFunction.fGetCareProviderLicenseNo(pv.CareProviderUID ?? 0),
                                                PatientAddress = SqlFunction.fGetAddressPatient(pv.PatientUID),
                                                VitalSignRecordDttm = j.RecordedDttm,
                                                Weight = j.Weight,
                                                Height = j.Height,
                                                PassportID = pa.IDPassport,
                                                BMI = j.BMIValue,
                                                Pulse = j.Pulse,
                                                BPSys = j.BPSys,
                                                BPDio = j.BPDio,
                                                Temp = j.Temprature,

                                            }).OrderByDescending(p => p.VitalSignRecordDttm).FirstOrDefault();

            return data;
        }


        [Route("PrintRadiologyCertificate")]
        [HttpGet]
        public MedicalCertificateModel PrintRadiologyCertificate(long patientVisitUID)
        {
            MedicalCertificateModel data = (from pa in db.Patient
                                            join pv in db.PatientVisit on pa.UID equals pv.PatientUID
                                            join rs in db.Result on pv.UID equals rs.PatientVisitUID
                                            where pa.StatusFlag == "A"
                                             && pv.StatusFlag == "A"
                                             && rs.StatusFlag == "A"
                                             && pv.UID == patientVisitUID
                                             && rs.RadiologistUID != null
                                            select new MedicalCertificateModel
                                            {
                                                No = pv.UID,
                                                IDCard = pa.IDCard,
                                                VisitID = pv.VisitID,
                                                AgeString = pa.DOBDttm != null ? SqlFunction.fGetAgeString(pa.DOBDttm.Value) : "",
                                                AgeYear = pa.DOBDttm != null ? SqlFunction.fGetAge(pa.DOBDttm.Value) : "",
                                                Gender = SqlFunction.fGetRfValDescription(pa.SEXXXUID ?? 0),
                                                strVisitData = pv.StartDttm,
                                                PatientID = pa.PatientID,
                                                DateOfBirth = pa.DOBDttm,
                                                PatientName = SqlFunction.fGetPatientName(pa.UID),
                                                Doctor = SqlFunction.fGetCareProviderName(rs.RadiologistUID ?? 0),
                                                DoctorLicenseNo = SqlFunction.fGetCareProviderLicenseNo(rs.RadiologistUID ?? 0),
                                                PatientAddress = SqlFunction.fGetAddressPatient(pv.PatientUID)

                                            }).FirstOrDefault();

            return data;
        }


        [Route("PrintOrderRequestCard")]
        [HttpGet]
        public OrderRequestCardModel PrintOrderRequestCard(long patientUID, long patientVisitUID)
        {
            OrderRequestCardModel data = (from pa in db.Patient
                                          join pv in db.PatientVisit on pa.UID equals pv.PatientUID
                                          join po in db.PatientOrder on pv.UID equals po.PatientVisitUID
                                          join pod in db.PatientOrderDetail on po.UID equals pod.PatientOrderUID
                                          join bi in db.BillableItem on pod.BillableItemUID equals bi.UID
                                          where
                                           pa.StatusFlag == "A"
                                           && pv.StatusFlag == "A"
                                           && pa.UID == patientUID
                                           && pv.UID == patientVisitUID
                                          select new OrderRequestCardModel
                                          {
                                              No = pv.UID,
                                              VisitID = pv.VisitID,
                                              Age = pa.DOBDttm != null ? SqlFunction.fGetAgeString(pa.DOBDttm.Value) : "",
                                              Gender = SqlFunction.fGetRfValDescription(pa.SEXXXUID ?? 0),
                                              strVisitData = pv.StartDttm.ToString(),
                                              VisitDate = pv.StartDttm,
                                              PatientID = pa.PatientID,
                                              DOB = pa.DOBDttm,
                                              PatientName = SqlFunction.fGetPatientName(pa.UID),
                                              DrugAllergy = SqlFunction.fGetPatientAllergy(pa.UID),
                                              Staff = SqlFunction.fGetCareProviderName(pod.CUser)

                                          }).FirstOrDefault();




            List<DetailOrderDetailCard> dataStore = null;
            DataTable dt = SqlDirectStore.pRPTOrderRequestCard(patientUID, patientVisitUID);
            if (dt != null && dt.Rows.Count > 0)
            {
                dataStore = new List<DetailOrderDetailCard>();
                dataStore = dt.ToList<DetailOrderDetailCard>();
            }

            if (data != null)
            {
                //data.OrderDetail = new List<DetailOrderDetailCard>();
                data.OrderDetail = dataStore;
            }




            return data;
        }

        [Route("PrintWellNessBook")]
        [HttpGet]
        public WellNessBookModel PrintWellNessBook(long patientUID, long patientVisitUID)
        {
            WellNessBookModel data = null;
            DataTable dt = SqlDirectStore.pRPTWellNessBook(patientUID, patientVisitUID);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new WellNessBookModel();
                data = dt.ToList<WellNessBookModel>().FirstOrDefault();
            }

            return data;
        }

        [Route("PrintCheckupBook")]
        [HttpGet]
        public List<CheckupBookModel> PrintCheckupBook(long patientUID, long payorDetailUID)
        {
            List<CheckupBookModel> data = null;

            DataTable dt = SqlDirectStore.pRPTCheckupBook(patientUID, payorDetailUID);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<CheckupBookModel>();
                data = dt.ToList<CheckupBookModel>();
            }

            return data;
        }

        [Route("PrintWellnessBook")]
        [HttpGet]
        public PatientWellnessModel PrintWellnessBook(long patientUID, long patientVisitUID, int payorDetailUID)
        {
            PatientWellnessModel data = new PatientWellnessModel();

            data.PatientInfomation = PatientInfomationWellness(patientUID, patientVisitUID);
            data.Radiology = (new RadiologyController()).GetResultRadiologyByPayor(patientUID, payorDetailUID);
            data.MobileResult = (new CheckupController()).GetCheckupMobileResultByVisitUID(patientUID, patientVisitUID);
            data.LabCompare = CheckupLabCompare(patientUID, payorDetailUID);
            data.GroupResult = (new CheckupController()).GetCheckupGroupResultListByVisit(patientUID, patientVisitUID);

            return data;
        }


        [Route("CheckupGroupResult")]
        [HttpGet]
        public List<CheckupGroupResultModel> CheckupGroupResult(long patientUID, long patientVisitUID)
        {
            List<CheckupGroupResultModel> data = new List<CheckupGroupResultModel>();
            data = (new CheckupController()).GetCheckupGroupResultListByVisit(patientUID, patientVisitUID);

            return data;
        }



        [Route("PrintRiskBook")]
        [HttpGet]
        public PatientRiskBookModel PrintRiskBook(long patientUID, long patientVisitUID, int payorDetailUID)
        {
            PatientRiskBookModel data = new PatientRiskBookModel();
            var wellnessData = PrintWellnessBook(patientUID, patientVisitUID, payorDetailUID);
            data.PatientInfomation = wellnessData.PatientInfomation;
            data.Radiology = wellnessData.Radiology;
            data.MobileResult = wellnessData.MobileResult;
            data.LabCompare = wellnessData.LabCompare;
            data.GroupResult = wellnessData.GroupResult;
            PatientHistoryController hisController = new PatientHistoryController();
            data.MedicalHistory = hisController.GetPatientMedicalHistoryByPatientUID(patientUID);
            data.WorkHistorys = hisController.GetPatientWorkHistoryByPatientUID(patientUID);
            data.InjuryDetails = hisController.GetInjuryByPatientUID(patientUID);
            data.PatientAddresses = (new PatientIdentityController()).GetPatientAddressByPatientUID(patientUID);

            return data;
        }


        [Route("GetCheckupRiskAudioTimus")]
        [HttpGet]
        public List<PatientResultComponentModel> GetCheckupRiskAudioTimus(long patientUID,int payorDetailUID)
        {
            List<PatientResultComponentModel> data = null;
            DataTable dt = SqlDirectStore.pGetCheckupRiskAudioTimus(patientUID,payorDetailUID);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<PatientResultComponentModel>();
                data = dt.ToList<PatientResultComponentModel>();
            }
            return data;
        }


        [Route("CheckupSummary")]
        [HttpPost]
        public List<CheckupSummaryModel> CheckupSummary(CheckupCompanyModel branchModel)
        {
            List<CheckupSummaryModel> data = null;

            DataTable dt = SqlDirectStore.pRPTCheckupSummary(branchModel.CheckupJobUID, branchModel.GPRSTUIDs, branchModel.CompanyName, branchModel.DateFrom, branchModel.DateTo);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<CheckupSummaryModel>();
                data = dt.ToList<CheckupSummaryModel>();
            }

            return data;
        }

        [Route("CheckupJobOrderSummary")]
        [HttpGet]
        public List<CheckupJobOrderModel> CheckupJobOrderSummary(int checkupjobUID, DateTime? dateFrom, DateTime? dateTo)
        {
            List<CheckupJobOrderModel> data = null;

            DataTable dt = SqlDirectStore.pRPTCheckupJobOrderSummary(checkupjobUID, dateFrom, dateTo);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<CheckupJobOrderModel>();
                data = dt.ToList<CheckupJobOrderModel>();
            }

            return data;
        }


        [Route("PatientInfomationWellness")]
        [HttpGet]
        public PatientVisitModel PatientInfomationWellness(long patientUID, long patientVisitUID)
        {
            PatientVisitModel data = null;

            DataTable dt = SqlDirectStore.pRPTPatientWellness(patientUID, patientVisitUID);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = dt.ToList<PatientVisitModel>().FirstOrDefault();
            }

            return data;
        }

        [Route("CheckupLabCompare")]
        [HttpGet]
        public List<PatientResultComponentModel> CheckupLabCompare(long patientUID, long payorDetailUID)
        {
            List<PatientResultComponentModel> data = null;

            DataTable dt = SqlDirectStore.pRPTCheckupLabCompare(patientUID, payorDetailUID);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<PatientResultComponentModel>();
                data = dt.ToList<PatientResultComponentModel>();
            }

            return data;
        }

        [Route("AudiogramResult")]
        [HttpGet]
        public List<PatientResultComponentModel> AudiogramResult(long patientUID, long patientVisitUID)
        {
            List<PatientResultComponentModel> data = (from rst in db.ResultComponent
                                                      join rs in db.Result
                                                      on rst.ResultUID equals rs.UID
                                                      join pv in db.PatientVisit
                                                      on rs.PatientVisitUID equals pv.UID
                                                      join pt in db.Patient
                                                      on pv.PatientUID equals pt.UID
                                                      join vts in db.PatientVitalSign
                                                      on pv.UID equals vts.PatientVisitUID
                                                      where rst.StatusFlag == "A"
                                                      && rs.StatusFlag == "A"
                                                      && pv.StatusFlag == "A"
                                                      && rs.RequestItemCode == "AUDIO"
                                                      && rs.PatientVisitUID == patientVisitUID
                                                    
                                                      select new PatientResultComponentModel
                                                      {
                                                          PatientName = SqlFunction.fGetPatientName(pt.UID),
                                                          PatientID = pt.PatientID,
                                                          EmployeeID = pt.EmployeeID,
                                                          Department = pt.Department,
                                                          Age = pt.DOBDttm != null ? SqlFunction.fGetAge(pt.DOBDttm.Value) : "",
                                                          Gender = SqlFunction.fGetRfValDescription(pt.SEXXXUID ?? 0),
                                                          BirthDttm = pt.DOBDttm,
                                                          ResultItemCode = rst.ResultItemCode,
                                                          ResultItemName = rst.ResultItemName,
                                                          ResultValue = rst.ResultValue,
                                                          Weight = vts.Weight,
                                                          Height = vts.Height,
                                                          StartDttm = pv.StartDttm

                                                      }).ToList();
            return data;
        }


        #region Inventory

        [Route("StockOnHand")]
        [HttpGet]
        public List<StockReportModel> StockOnHand(string organisationList)
        {
            List<StockReportModel> data = null;
            DataTable dt = SqlDirectStore.pRPTStockOnHand(organisationList);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<StockReportModel>();
                data = dt.ToList<StockReportModel>();
            }

            return data;
        }


        [Route("StockDispensedReport")]
        [HttpGet]
        public List<StockReportModel> StockDispensedReport(DateTime dateFrom, DateTime dateTo, string organisationList)
        {
            List<StockReportModel> data = null;
            DataTable dt = SqlDirectStore.pRPTStockDispensed(dateFrom, dateTo, organisationList);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<StockReportModel>();
                data = dt.ToList<StockReportModel>();
            }

            return data;
        }

        [Route("StockNonMovement")]
        [HttpGet]
        public List<StockReportModel> StockNonMovement(DateTime dateFrom, DateTime dateTo, string organisationList)
        {
            List<StockReportModel> data = null;
            DataTable dt = SqlDirectStore.pRPTStockNonMovement(dateFrom, dateTo, organisationList);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<StockReportModel>();
                data = dt.ToList<StockReportModel>();
            }

            return data;
        }

        [Route("StockExpiryReport")]
        [HttpGet]
        public List<StockReportModel> StockExpiryReport(int month, string organisationList)
        {
            List<StockReportModel> data = null;
            DataTable dt = SqlDirectStore.pRPTStockExpiry(month, organisationList);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<StockReportModel>();
                data = dt.ToList<StockReportModel>();
            }

            return data;
        }

        [Route("StockExpiredReport")]
        [HttpGet]
        public List<StockReportModel> StockExpiredReport(string organisationList)
        {
            List<StockReportModel> data = null;
            DataTable dt = SqlDirectStore.pRPTStockExpired(organisationList);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<StockReportModel>();
                data = dt.ToList<StockReportModel>();
            }

            return data;
        }

        [Route("StockGoodReceiveReport")]
        [HttpGet]
        public List<StockReportModel> StockGoodReceiveReport(DateTime dateFrom, DateTime dateTo, string organisationList)
        {
            List<StockReportModel> data = null;
            DataTable dt = SqlDirectStore.pRPTStockGoodReceive(dateFrom, dateTo, organisationList);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<StockReportModel>();
                data = dt.ToList<StockReportModel>();
            }

            return data;
        }


        [Route("StockReceiveReport")]
        [HttpGet]
        public List<StockTransactionReportModel> StockReceiveReport(DateTime dateFrom, DateTime dateTo, string organisationList)
        {
            List<StockTransactionReportModel> data = null;
            DataTable dt = SqlDirectStore.pRPTStockReceive(dateFrom, dateTo, organisationList);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<StockTransactionReportModel>();
                data = dt.ToList<StockTransactionReportModel>();
            }

            return data;
        }

        [Route("StockBalancePerMounth")]
        [HttpGet]
        public List<StockReportModel> StockBalancePerMounth(int year, string monthLists, string organisationList)
        {
            List<StockReportModel> data = null;
            DataTable dt = SqlDirectStore.pRPTStockBalancePerMounth(year, monthLists, organisationList);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<StockReportModel>();
                data = dt.ToList<StockReportModel>();
            }

            return data;
        }

        [Route("StockIssueReport")]
        [HttpGet]
        public List<StockTransactionReportModel> StockIssueReport(string issueID)
        {
            List<StockTransactionReportModel> data = (from i in db.ItemIssue
                                                      join j in db.ItemIssueDetail on i.UID equals j.ItemIssueUID
                                                      join k in db.Stock on j.StockUID equals k.UID
                                                      where i.ItemIssueID == issueID
                                                      select new StockTransactionReportModel
                                                      {
                                                          BatchID = k.BatchID,
                                                          Comments = i.Comments,
                                                          ExpiryDttm = k.ExpiryDttm,
                                                          IssueBy = SqlFunction.fGetCareProviderName(i.IssueBy),
                                                          IssuedDttm = i.ItemIssueDttm,
                                                          IssuedOrganisation = SqlFunction.fGetHealthOrganisationName(i.OrganisationUID),
                                                          IssuedStore = SqlFunction.fGetStoreName(i.StoreUID),
                                                          IssueID = i.ItemIssueID,
                                                          ItemCode = j.ItemCode,
                                                          Itemcost = j.ItemCost,
                                                          NetCost = Math.Round(j.ItemCost * j.Quantity, 3),
                                                          ItemName = j.ItemName,
                                                          Price = j.UnitPrice,
                                                          Quantity = j.Quantity,
                                                          RequestID = i.ItemRequestID,
                                                          RequestOrganisation = SqlFunction.fGetHealthOrganisationName(i.RequestedByOrganisationUID),
                                                          RequestStore = SqlFunction.fGetStoreName(i.RequestedByStoreUID),
                                                          Unit = SqlFunction.fGetRfValDescription(j.IMUOMUID)
                                                      }).ToList();


            return data;
        }

        [Route("StockIssueReport")]
        [HttpGet]
        public List<StockTransactionReportModel> StockIssueReport(DateTime dateFrom, DateTime dateTo, string organisationList)
        {
            List<StockTransactionReportModel> result = null;
            DataTable dataTable = SqlDirectStore.pRPTStockIssued(dateFrom, dateTo, organisationList);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                result = new List<StockTransactionReportModel>();
                result = Extension.ToList<StockTransactionReportModel>(dataTable);
            }
            return result;
        }

        [Route("StockTransferredOutReport")]
        [HttpGet]
        public List<StockTransactionReportModel> StockTransferredOutReport(DateTime dateFrom, DateTime dateTo, string organisationList)
        {
            List<StockTransactionReportModel> data = null;
            DataTable dt = SqlDirectStore.pRPTStockTransferredOut(dateFrom, dateTo, organisationList);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<StockTransactionReportModel>();
                data = dt.ToList<StockTransactionReportModel>();
            }

            return data;
        }

        [Route("StockTransferredInReport")]
        [HttpGet]
        public List<StockTransactionReportModel> StockTransferredInReport(DateTime dateFrom, DateTime dateTo, string organisationList)
        {
            List<StockTransactionReportModel> data = null;
            DataTable dt = SqlDirectStore.pRPTStockTransferredIn(dateFrom, dateTo, organisationList);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<StockTransactionReportModel>();
                data = dt.ToList<StockTransactionReportModel>();
            }

            return data;
        }

        [Route("StockRequestReport")]
        [HttpGet]
        public List<StockTransactionReportModel> StockRequestReport(string requestID)
        {
            List<StockTransactionReportModel> data = (from i in db.ItemRequest
                                                      join j in db.ItemRequestDetail on i.UID equals j.ItemRequestUID
                                                      where i.ItemRequestID == requestID
                                                      select new StockTransactionReportModel
                                                      {
                                                          Comments = i.Comments,
                                                          RequestBy = SqlFunction.fGetCareProviderName(i.RequestedBy),
                                                          RequestedDttm = i.RequestedDttm,
                                                          ItemCode = j.ItemCode,
                                                          ItemName = j.ItemName,
                                                          Quantity = j.Quantity,
                                                          CurrentQuantity = j.CurrentQuantity,
                                                          MainCurrentQuantity = SqlFunction.fGetItemTotalQuantity(j.ItemMasterUID, i.RequestOnStoreUID, i.RequestOnOrganistaionUID),
                                                          RequestID = i.ItemRequestID,
                                                          RequestOrganisation = SqlFunction.fGetHealthOrganisationName(i.OrganisationUID),
                                                          RequestStore = SqlFunction.fGetStoreName(i.StoreUID),
                                                          Unit = SqlFunction.fGetRfValDescription(j.IMUOMUID)
                                                      }).ToList();


            return data;
        }

        [Route("GoodReceiveReport")]
        [HttpGet]
        public List<GoodReceiveReportModel> GoodReceiveReport(string grnNumber)
        {
            List<GoodReceiveReportModel> data = (from i in db.GRNDetail
                                                 join j in db.GRNItemList on i.UID equals j.GRNDetailUID
                                                 where i.GRNNumber == grnNumber
                                                 select new GoodReceiveReportModel
                                                 {
                                                     BatchID = j.BatchID,
                                                     Discount = i.Discount,
                                                     ExpiryDttm = j.ExpiryDttm,
                                                     FreeQuantity = j.FreeQuantity,
                                                     GrnNumber = i.GRNNumber,
                                                     GRNType = SqlFunction.fGetRfValDescription(i.GRNTYPUID ?? 0),
                                                     InvoiceNo = i.InvoiceNo,
                                                     ItemCode = j.ItemCode,
                                                     ItemName = j.ItemName,
                                                     Manufacturer = SqlFunction.fGetVendorName(j.ManufacturerUID ?? 0),
                                                     NetAmount = i.NetAmount,
                                                     NetAmountCost = j.NetAmount,
                                                     OtherCharges = i.OtherCharges,
                                                     PurchaseCost = j.PurchaseCost,
                                                     Quantity = j.Quantity,
                                                     ReceiveBy = SqlFunction.fGetCareProviderName(i.CUser),
                                                     ReceivedOrganisation = SqlFunction.fGetHealthOrganisationName(i.RecievedOrganisationUID),
                                                     ReceivedStore = SqlFunction.fGetStoreName(i.RecievedStoreUID),
                                                     ReceiveDttm = i.RecievedDttm,
                                                     Unit = SqlFunction.fGetRfValDescription(j.IMUOMUID),
                                                     VendorName = SqlFunction.fGetVendorName(i.VendorDetailUID)
                                                 }).ToList();


            return data;
        }

        [Route("StockSummary")]
        [HttpGet]
        public List<StockSummaryModel> StockSummary(DateTime dateFrom, DateTime dateTo, string organisationList)
        {
            List<StockSummaryModel> data = null;
            DataTable dt = SqlDirectStore.pRPTStockSummary(dateFrom, dateTo, organisationList);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<StockSummaryModel>();
                data = dt.ToList<StockSummaryModel>();
            }

            return data;
        }

        [Route("StockConsumption")]
        [HttpGet]
        public List<StockReportModel> StockConsumption(DateTime dateFrom, DateTime dateTo, string organisationList)
        {
            List<StockReportModel> data = null;
            DataTable dt = SqlDirectStore.pRPTStockConsumtion(dateFrom, dateTo, organisationList);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<StockReportModel>();
                data = dt.ToList<StockReportModel>();
            }

            return data;
        }

        [Route("StockAdjustmentOut")]
        [HttpGet]
        public List<StockReportModel> StockAdjustmentOut(DateTime dateFrom, DateTime dateTo, string organisationList)
        {
            List<StockReportModel> data = null;
            DataTable dt = SqlDirectStore.pRPTStockAdjustmentOut(dateFrom, dateTo, organisationList);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<StockReportModel>();
                data = dt.ToList<StockReportModel>();
            }

            return data;
        }

        [Route("StockAdjustmentIn")]
        [HttpGet]
        public List<StockReportModel> StockAdjustmentIn(DateTime dateFrom, DateTime dateTo, string organisationList)
        {
            List<StockReportModel> data = null;
            DataTable dt = SqlDirectStore.pRPTStockAdjustmentIn(dateFrom, dateTo, organisationList);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<StockReportModel>();
                data = dt.ToList<StockReportModel>();
            }

            return data;
        }

        [Route("StockDispose")]
        [HttpGet]
        public List<StockTransactionReportModel> StockDispose(DateTime dateFrom, DateTime dateTo, string organisationList)
        {
            List<StockTransactionReportModel> data = null;
            DataTable dt = SqlDirectStore.pRPTStockDispose(dateFrom, dateTo, organisationList);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<StockTransactionReportModel>();
                data = dt.ToList<StockTransactionReportModel>();
            }

            return data;
        }

        #endregion

        #region Radiology

        [Route("GetRadiologyRDUReview")]
        [HttpGet]
        public List<RadiologyRDUReviewModel> GetRadiologyRDUReview(DateTime dateFrom, DateTime dateTo, string organisationList)
        {
            List<RadiologyRDUReviewModel> data = null;
            DataTable dt = SqlDirectStore.pRPTRadiologyRDUReview(dateFrom, dateTo, organisationList);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<RadiologyRDUReviewModel>();
                data = dt.ToList<RadiologyRDUReviewModel>();
            }

            return data;
        }


        [Route("GetPatientResultRadiology")]
        [HttpGet]
        public PatientResultRadiology GetPatientResultRadiology(long resultUID)
        {
            PatientResultRadiology data = (from pa in db.Patient
                                           join pv in db.PatientVisit on pa.UID equals pv.PatientUID
                                           join re in db.Request on pv.UID equals re.PatientVisitUID
                                           join red in db.RequestDetail on re.UID equals red.RequestUID
                                           join rs in db.Result on red.UID equals rs.RequestDetailUID
                                           join rsr in db.ResultRadiology on rs.UID equals rsr.ResultUID
                                           where pa.StatusFlag == "A"
                                           && pv.StatusFlag == "A"
                                           && re.StatusFlag == "A"
                                           && red.StatusFlag == "A"
                                           && rs.StatusFlag == "A"
                                           && rsr.StatusFlag == "A"
                                           && rs.UID == resultUID
                                           select new PatientResultRadiology
                                           {
                                               Age = pa.DOBDttm != null ? SqlFunction.fGetAge(pa.DOBDttm.Value) : "",
                                               Doctor = SqlFunction.fGetCareProviderName(rs.RadiologistUID ?? 0),
                                               DoctorLicense = SqlFunction.fGetCareProviderLicenseNo(rs.RadiologistUID ?? 0),
                                               Gender = SqlFunction.fGetRfValDescription(pa.SEXXXUID ?? 0),
                                               HN = pa.PatientID,
                                               Location = SqlFunction.fGetHealthOrganisationName(pv.OwnerOrganisationUID ?? 0),
                                               //Location = "โรงพยาบาลกรุงเทพระยอง",
                                               PatientName = SqlFunction.fGetPatientName(pa.UID),
                                               RequestedDttm = re.RequestedDttm,
                                               RequestItemName = red.RequestItemName,
                                               ResultEnteredDttm = rs.ResultEnteredDttm,
                                               OtherID = pa.EmployeeID,
                                               ResultHtml = rsr.Value,
                                               ResultValue = rsr.PlainText,
                                               OrderStatus = SqlFunction.fGetRfValDescription(rs.ORDSTUID),
                                               ResultStatus = SqlFunction.fGetRfValDescription(rs.RABSTSUID ?? 0),
                                               OgranastionAddress = SqlFunction.fGetAddressOrganisation(pv.OwnerOrganisationUID ?? 0)
                                           }).FirstOrDefault();

            return data;
        }

        #endregion

        #region Lab


        [Route("GetLabResultByRequestNumber")]
        [HttpGet]
        public List<PatientLabResult> GetLabResultByRequestNumber(long patientVisitUID, string requestNumber)
        {
            List<PatientLabResult> data = (from pa in db.Patient
                                           join pv in db.PatientVisit on pa.UID equals pv.PatientUID
                                           join or in db.HealthOrganisation on pv.OwnerOrganisationUID equals or.UID
                                           join re in db.Request on pv.UID equals re.PatientVisitUID
                                           join red in db.RequestDetail on re.UID equals red.RequestUID
                                           join rs in db.Result on red.UID equals rs.RequestDetailUID
                                           join rit in db.RequestItem on red.RequestitemUID equals rit.UID
                                           join rsc in db.ResultComponent on rs.UID equals rsc.ResultUID
                                           //join rslk in db.RequestResultLink on new { key1 = red.RequestitemUID, key2 = rsc.ResultItemUID ?? 0 } equals new { key1 = rslk.RequestItemUID, key2 = rslk.ResultItemUID }
                                           where pa.StatusFlag == "A"
                                           && pv.StatusFlag == "A"
                                           && re.StatusFlag == "A"
                                           && red.StatusFlag == "A"
                                           && rs.StatusFlag == "A"
                                           && rsc.StatusFlag == "A"
                                           //&& rslk.StatusFlag == "A"
                                           && pv.UID == patientVisitUID
                                           && re.RequestNumber == requestNumber
                                           && red.ORDSTUID != 2848
                                           select new PatientLabResult
                                           {
                                               PatientUID = pa.UID,
                                               PatientVisitUID = pv.UID,
                                               PatientName = SqlFunction.fGetPatientName(pa.UID),
                                               Age = pa.DOBDttm != null ? SqlFunction.fGetAgeString(pa.DOBDttm.Value) : "",
                                               VisitID = pv.VisitID,
                                               DOBDttm = pa.DOBDttm,
                                               FirstName = pa.FirstName,
                                               LastName = pa.LastName,
                                               Gender = SqlFunction.fGetRfValDescription(pa.SEXXXUID ?? 0),
                                               MobilePhone = pa.MobilePhone,
                                               PatientID = pa.PatientID,
                                               RequestItemName = red.RequestItemName,
                                               RequestNumber = re.RequestNumber,
                                               Title = SqlFunction.fGetRfValDescription(pa.TITLEUID ?? 0),
                                               RequestBy = SqlFunction.fGetCareProviderName(pv.CareProviderUID ?? 0),
                                               ReviewedBy = SqlFunction.fGetCareProviderName(rs.ResultEnteredUserUID ?? 0),
                                               Abnormal = rsc.IsAbnormal,
                                               ReferenceRange = rsc.ReferenceRange,
                                               RequestDetailUID = red.UID,
                                               RequestedDttm = re.RequestedDttm,
                                               RequestUID = re.UID,
                                               ResultComponentUID = rsc.UID,
                                               ResultEnteredDttm = rs.ResultEnteredDttm,
                                               ResultItemName = rsc.ResultItemName,
                                               //PrintOrder = rslk.PrintOrder ?? 0,
                                               ResultUID = rs.UID,
                                               ResultValue = rsc.ResultValue + (!string.IsNullOrEmpty(rsc.IsAbnormal) ? " " + rsc.IsAbnormal : ""),
                                               ResultValueType = SqlFunction.fGetRfValDescription(rsc.RVTYPUID),
                                               Unit = SqlFunction.fGetRfValDescription(rsc.RSUOMUID ?? 0),
                                               PrintGroup = SqlFunction.fGetRfValDescription(rit.PRTGPUID ?? 0),
                                               OrganisationName = or.Name + (or.LicenseNo != null ? " ใบอนุญาตเลขที่ " + or.LicenseNo : ""),
                                               OrganisationCode = or.Code,
                                               OrganisationAddress = SqlFunction.fGetAddressOrganisation(pv.OwnerOrganisationUID ?? 0)
                                           }).ToList();

            return data;
        }

        #endregion
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
