using MediTech.Model;
using MediTech.Model.Report;
using ShareLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.DataService
{
    public class ReportsService
    {
        #region Reports
        public List<ReportsModel> GetReports()
        {
            string requestApi = string.Format("Api/Report/GetReports");
            List<ReportsModel> data = MeditechApiHelper.Get<List<ReportsModel>>(requestApi);

            return data;
        }
        public List<ReportsModel> GetReportsByGroup(string reportGroup)
        {
            string requestApi = string.Format("Api/Report/GetReportsByGroup?reportGroup={0}", reportGroup);
            List<ReportsModel> data = MeditechApiHelper.Get<List<ReportsModel>>(requestApi);

            return data;
        }

        public List<PayorDetailModel>GetPayordetailByDate(DateTime dateFrom,DateTime dateTo)
        {
            string requestApi = string.Format("Api/Report/GetPayordetailByDate?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}", dateFrom,dateTo);
            List<PayorDetailModel> data = MeditechApiHelper.Get<List<PayorDetailModel>>(requestApi);

            return data;
        }

        #endregion

        #region Cashier
        public List<PatientRevenueModel> GetPatientNetProfit(DateTime dateFrom, DateTime dateTo, int? vistyuid, string organisationList)
        {
            string requestApi = string.Format("Api/Report/GetPatientNetProfit?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&vistyuid={2}&organisationList={3}", dateFrom, dateTo, vistyuid, organisationList);
            List<PatientRevenueModel> data = MeditechApiHelper.Get<List<PatientRevenueModel>>(requestApi);

            return data;
        }


        public List<EcountExportModel>GetStockToEcount(DateTime dateFrom, DateTime dateTo, int? vistyuid, string organisationList)
        {
            string requestApi = string.Format("Api/Report/GetStockToEcount?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&vistyuid={2}&organisationList={3}", dateFrom, dateTo, vistyuid, organisationList);
            List<EcountExportModel> data = MeditechApiHelper.Get<List<EcountExportModel>>(requestApi);

            return data;
        }

        public List<EcountExportModel>GetEcountSumGroupReceipt(DateTime dateFrom, DateTime dateTo, int? vistyuid, string organisationList)
        {
            string requestApi = string.Format("Api/Report/GetEcountSumGroupReceipt?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&vistyuid={2}&organisationList={3}", dateFrom, dateTo, vistyuid, organisationList);
            List<EcountExportModel> data = MeditechApiHelper.Get<List<EcountExportModel>>(requestApi);

            return data;
        }

        public List<RevenuePerDayModel> GetRevenuePerDay(DateTime billGeneratedDttm, string organisationList)
        {
            string requestApi = string.Format("Api/Report/GetRevenuePerDay?billGeneratedDttm={0:MM/dd/yyyy}&organisationList={1}", billGeneratedDttm, organisationList);
            List<RevenuePerDayModel> data = MeditechApiHelper.Get<List<RevenuePerDayModel>>(requestApi);

            return data;
        }
        public List<PatientVisitModel> GetPayorSummeryCount(DateTime billGeneratedDttm, string vistyuids, int organisationUID)
        {
            string requestApi = string.Format("Api/Report/GetPayorSummeryCount?billGeneratedDttm={0:MM/dd/yyyy}&vistyuids={1}&organisationUID={2}", billGeneratedDttm, vistyuids, organisationUID);
            List<PatientVisitModel> data = MeditechApiHelper.Get<List<PatientVisitModel>>(requestApi);
            return data;
        }

        public List<PatientRevenueModel> GetUsedReport(DateTime dateFrom, DateTime dateTo, string organisationList)
        {
            string requestApi = string.Format("Api/Report/GetUsedReport?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&organisationList={2}", dateFrom, dateTo, organisationList);
            List<PatientRevenueModel> data = MeditechApiHelper.Get<List<PatientRevenueModel>>(requestApi);

            return data;
        }

        public List<PatientRevenueModel> GetDrugStoreNetProfit(DateTime dateFrom, DateTime dateTo, int? organisationUID)
        {
            string requestApi = string.Format("Api/Report/GetDrugStoreNetProfit?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&organisationUID={2}", dateFrom, dateTo, organisationUID);
            List<PatientRevenueModel> data = MeditechApiHelper.Get<List<PatientRevenueModel>>(requestApi);

            return data;
        }

        public List<DoctorFeeReportModel> GetDoctorfeeReport(DateTime dateFrom, DateTime dateTo, int? careproviderUID)
        {
            string requestApi = string.Format("Api/Report/GetDoctorfeeReport?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&careproviderUID={2}", dateFrom, dateTo, careproviderUID);
            List<DoctorFeeReportModel> data = MeditechApiHelper.Get<List<DoctorFeeReportModel>>(requestApi);

            return data;
        }

        public List<DoctorFeeReportModel> GetDoctorfeeReport2(DateTime dateFrom, DateTime dateTo, int? careproviderUID)
        {
            string requestApi = string.Format("Api/Report/GetDoctorfeeReport2?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&careproviderUID={2}", dateFrom, dateTo, careproviderUID);
            List<DoctorFeeReportModel> data = MeditechApiHelper.Get<List<DoctorFeeReportModel>>(requestApi);

            return data;
        }

        #endregion

        #region Patient

        public List<ProblemStatisticModel> PatientProblemStaistic(DateTime dateFrom, DateTime dateTo, int? vistyuid, string organisationList)
        {
            string requestApi = string.Format("Api/Report/PatientProblemStaistic?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&vistyuid={2}&organisationList={3}", dateFrom, dateTo, vistyuid, organisationList);
            List<ProblemStatisticModel> data = MeditechApiHelper.Get<List<ProblemStatisticModel>>(requestApi);

            return data;
        }

        #endregion

        #region Registration

        public List<PatientSummaryModel> PatientSummaryPerMonth(int year, string monthLists, string organisationList)
        {
            string requestApi = string.Format("Api/Report/PatientSummaryPerMonth?year={0}&monthLists={1}&organisationList={2}", year, monthLists, organisationList);
            List<PatientSummaryModel> data = MeditechApiHelper.Get<List<PatientSummaryModel>>(requestApi);

            return data;
        }

        public List<ChartStatisticModel> VisitDaysStatistic(DateTime dateFrom, DateTime dateTo, int? vistyuid, string organisationList)
        {
            string requestApi = string.Format("Api/Report/VisitDaysStatistic?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&vistyuid={2}&organisationList={3}", dateFrom, dateTo, vistyuid, organisationList);
            List<ChartStatisticModel> data = MeditechApiHelper.Get<List<ChartStatisticModel>>(requestApi);

            return data;
        }

        public List<ChartStatisticModel> VisitTimesStatistic(DateTime dateFrom, DateTime dateTo, int? vistyuid, string organisationList)
        {
            string requestApi = string.Format("Api/Report/VisitTimesStatistic?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&vistyuid={2}&organisationList={3}", dateFrom, dateTo, vistyuid, organisationList);
            List<ChartStatisticModel> data = MeditechApiHelper.Get<List<ChartStatisticModel>>(requestApi);

            return data;
        }

        public List<PatientSumByAreaModel> PatientSumByAreaPerMonth(DateTime dateFrom, DateTime dateTo, int? vistyuid, string organisationList)
        {
            string requestApi = string.Format("Api/Report/PatientSumByAreaPerMonth?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&vistyuid={2}&organisationList={3}", dateFrom, dateTo, vistyuid, organisationList);
            List<PatientSumByAreaModel> data = MeditechApiHelper.Get<List<PatientSumByAreaModel>>(requestApi);

            return data;
        }

        public List<PatientSummaryDataModel> PatientSummaryData(DateTime dateFrom, DateTime dateTo, string organisationList)
        {
            string requestApi = string.Format("Api/Report/PatientSummaryData?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&organisationList={2}", dateFrom, dateTo, organisationList);
            List<PatientSummaryDataModel> data = MeditechApiHelper.Get<List<PatientSummaryDataModel>>(requestApi);

            return data;
        }

        #endregion



        public List<OPDCardModel> PrintOPDCard(long patientUID, long patientVisitUID)
        {
            string requestApi = string.Format("Api/Report/PrintOPDCard?patientUID={0}&patientVisitUID={1}", patientUID, patientVisitUID);
            List<OPDCardModel> data = MeditechApiHelper.Get<List<OPDCardModel>>(requestApi);

            return data;
        }

        public WellNessBookModel PrintWellNessBook(long patientUID, long patientVisitUID)
        {
            string requestApi = string.Format("Api/Report/PrintWellNessBook?patientUID={0}&patientVisitUID={1}", patientUID, patientVisitUID);
            WellNessBookModel data = MeditechApiHelper.Get<WellNessBookModel>(requestApi);

            return data;
        }

        public List<CheckupBookModel> PrintCheckupBook(long patientUID, long payorDetailUID)
        {
            string requestApi = string.Format("Api/Report/PrintCheckupBook?patientUID={0}&payorDetailUID={1}", patientUID, payorDetailUID);
            List<CheckupBookModel> data = MeditechApiHelper.Get<List<CheckupBookModel>>(requestApi);

            return data;
        }

        public PatientWellnessModel PrintWellnessBook(long patientUID, long patientVisitUID, long payorDetailUID)
        {
            string requestApi = string.Format("Api/Report/PrintWellnessBook?patientUID={0}&patientVisitUID={1}&payorDetailUID={2}", patientUID, patientVisitUID, payorDetailUID);
            PatientWellnessModel data = MeditechApiHelper.Get<PatientWellnessModel>(requestApi);

            return data;
        }

        public PatientRiskBookModel PrintRiskBook(long patientUID, long patientVisitUID, int payorDetailUID)
        {
            string requestApi = string.Format("Api/Report/PrintRiskBook?patientUID={0}&patientVisitUID={1}&payorDetailUID={2}", patientUID, patientVisitUID, payorDetailUID);
            PatientRiskBookModel data = MeditechApiHelper.Get<PatientRiskBookModel>(requestApi);

            return data;
        }

        public List<CheckupSummaryModel> CheckupSummary(CheckupCompanyModel branchModel)
        {
            string requestApi = string.Format("Api/Report/CheckupSummary");
            List<CheckupSummaryModel> data = MeditechApiHelper.Post<CheckupCompanyModel, List<CheckupSummaryModel>>(requestApi, branchModel);

            return data;
        }

        public List<CheckupJobOrderModel> CheckupJobOrderSummary(int checkupjobUID, DateTime? dateFrom, DateTime? dateTo)
        {
            string requestApi = string.Format("Api/Report/CheckupJobOrderSummary?checkupjobUID={0}&dateFrom={1:MM/dd/yyyy}&dateTo={2:MM/dd/yyyy}", checkupjobUID, dateFrom, dateTo);
            List<CheckupJobOrderModel> data = MeditechApiHelper.Get<List<CheckupJobOrderModel>>(requestApi);

            return data;
        }
        public PatientVisitModel PatientInfomationWellness(long patientUID, long patientVisitUID)
        {
            string requestApi = string.Format("Api/Report/PatientInfomationWellness?patientUID={0}&patientVisitUID={1}", patientUID, patientVisitUID);
            PatientVisitModel data = MeditechApiHelper.Get<PatientVisitModel>(requestApi);

            return data;
        }

        public List<PatientResultComponentModel> CheckupLabCompare(long patientUID, long payorDetailUID)
        {
            string requestApi = string.Format("Api/Report/CheckupLabCompare?patientUID={0}&payorDetailUID={1}", patientUID, payorDetailUID);
            List<PatientResultComponentModel> data = MeditechApiHelper.Get<List<PatientResultComponentModel>>(requestApi);

            return data;
        }


        public List<CheckupGroupResultModel> CheckupGroupResult(long patientUID, long patientVisitUID)
        {
            string requestApi = string.Format("Api/Report/CheckupGroupResult?patientUID={0}&patientVisitUID={1}", patientUID, patientVisitUID);
            List<CheckupGroupResultModel> data = MeditechApiHelper.Get<List<CheckupGroupResultModel>>(requestApi);

            return data;
        }

        public List<PatientResultComponentModel> AudiogramResult(long patientUID, long patientVisitUID)
        {
            string requestApi = string.Format("Api/Report/AudiogramResult?patientUID={0}&patientVisitUID={1}", patientUID,patientVisitUID);
            List<PatientResultComponentModel> data = MeditechApiHelper.Get<List<PatientResultComponentModel>>(requestApi);

            return data;
        }


        public List<PatientResultComponentModel> GetCheckupRiskAudioTimus(long patientUID, int payorDetailUID)
        {
            string requestApi = string.Format("Api/Report/GetCheckupRiskAudioTimus?patientUID={0}&payorDetailUID={1}", patientUID, payorDetailUID);
            List<PatientResultComponentModel> data = MeditechApiHelper.Get<List<PatientResultComponentModel>>(requestApi);

            return data;
        }




        public MedicalCertificateModel PrintMedicalCertificate(long patientVisitUID)
        {
            string requestApi = string.Format("Api/Report/PrintMedicalCertificate?patientVisitUID={0}", patientVisitUID);
            MedicalCertificateModel data = MeditechApiHelper.Get<MedicalCertificateModel>(requestApi);

            return data;
        }

        public MedicalCertificateModel PrintConfinedSpaceCertificate(long patientVisitUID)
        {
            string requestApi = string.Format("Api/Report/PrintConfinedSpaceCertificate?patientVisitUID={0}", patientVisitUID);
            MedicalCertificateModel data = MeditechApiHelper.Get<MedicalCertificateModel>(requestApi);

            return data;
        }

        public MedicalCertificateModel PrintRadiologyCertificate(long patientVisitUID)
        {
            string requestApi = string.Format("Api/Report/PrintRadiologyCertificate?patientVisitUID={0}", patientVisitUID);
            MedicalCertificateModel data = MeditechApiHelper.Get<MedicalCertificateModel>(requestApi);

            return data;
        }


        public PatientBookingModel PrintPatientBooking(int bookUID)
        {
            string requestApi = string.Format("Api/Report/PrintPatientBooking?bookUID={0}", bookUID);
            PatientBookingModel data = MeditechApiHelper.Get<PatientBookingModel>(requestApi);
            return data;
        }

        public OrderRequestCardModel PrintOrderRequestCard(long patientUID, long patientVisitUID)
        {
            string requestApi = string.Format("Api/Report/PrintOrderRequestCard?patientUID={0}&patientVisitUID={1}", patientUID, patientVisitUID);
            OrderRequestCardModel data = MeditechApiHelper.Get<OrderRequestCardModel>(requestApi);
            return data;
        }

        #region Inventory

        public List<StockReportModel> StockOnHand(string organisationList)
        {
            string requestApi = string.Format("Api/Report/StockOnHand?organisationList={0}", organisationList);
            List<StockReportModel> data = MeditechApiHelper.Get<List<StockReportModel>>(requestApi);

            return data;
        }


        public List<StockReportModel> StockDispensedReport(DateTime dateFrom, DateTime dateTo, string organisationList)
        {
            string requestApi = string.Format("Api/Report/StockDispensedReport?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&organisationList={2}", dateFrom, dateTo, organisationList);
            List<StockReportModel> data = MeditechApiHelper.Get<List<StockReportModel>>(requestApi);

            return data;
        }

        public List<StockReportModel> StockNonMovement(DateTime dateFrom, DateTime dateTo, string organisationList)
        {
            string requestApi = string.Format("Api/Report/StockNonMovement?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&organisationList={2}", dateFrom, dateTo, organisationList);
            List<StockReportModel> data = MeditechApiHelper.Get<List<StockReportModel>>(requestApi);

            return data;
        }

        public List<StockReportModel> StockExpiryReport(int month, string organisationList)
        {
            string requestApi = string.Format("Api/Report/StockExpiryReport?month={0}&organisationList={1}", month, organisationList);
            List<StockReportModel> data = MeditechApiHelper.Get<List<StockReportModel>>(requestApi);

            return data;
        }

        public List<StockReportModel> StockExpiredReport(string organisationList)
        {
            string requestApi = string.Format("Api/Report/StockExpiredReport?organisationList={0}", organisationList);
            List<StockReportModel> data = MeditechApiHelper.Get<List<StockReportModel>>(requestApi);

            return data;
        }

        public List<StockReportModel> StockGoodReceiveReport(DateTime dateFrom, DateTime dateTo, string organisationList)
        {
            string requestApi = string.Format("Api/Report/StockGoodReceiveReport?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&organisationList={2}", dateFrom, dateTo, organisationList);
            List<StockReportModel> data = MeditechApiHelper.Get<List<StockReportModel>>(requestApi);

            return data;
        }


        public List<StockTransactionReportModel> StockReceiveReport(DateTime dateFrom, DateTime dateTo, string organisationList)
        {
            string requestApi = string.Format("Api/Report/StockReceiveReport?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&organisationList={2}", dateFrom, dateTo, organisationList);
            List<StockTransactionReportModel> data = MeditechApiHelper.Get<List<StockTransactionReportModel>>(requestApi);

            return data;
        }

        public List<StockReportModel> StockBalancePerMounth(int year, string monthLists, string organisationList)
        {
            string requestApi = string.Format("Api/Report/StockBalancePerMounth?year={0}&monthLists={1}&organisationList={2}", year, monthLists, organisationList);
            List<StockReportModel> data = MeditechApiHelper.Get<List<StockReportModel>>(requestApi);

            return data;
        }

        public List<StockTransactionReportModel> StockIssueReport(string issueID)
        {
            return MeditechApiHelper.Get<List<StockTransactionReportModel>>(string.Format("Api/Report/StockIssueReport?issueID={0}", issueID));
        }

        public List<StockTransactionReportModel> StockIssueReport(DateTime dateFrom, DateTime dateTo, string organisationList)
        {
            return MeditechApiHelper.Get<List<StockTransactionReportModel>>(string.Format("Api/Report/StockIssueReport?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&organisationList={2}", dateFrom, dateTo, organisationList));
        }

        public List<StockTransactionReportModel> StockTransferredOutReport(DateTime dateFrom, DateTime dateTo, string organisationList)
        {
            string requestApi = string.Format("Api/Report/StockTransferredOutReport?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&organisationList={2}", dateFrom, dateTo, organisationList);
            List<StockTransactionReportModel> data = MeditechApiHelper.Get<List<StockTransactionReportModel>>(requestApi);

            return data;
        }

        public List<StockTransactionReportModel> StockTransferredInReport(DateTime dateFrom, DateTime dateTo, string organisationList)
        {
            string requestApi = string.Format("Api/Report/StockTransferredInReport?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&organisationList={2}", dateFrom, dateTo, organisationList);
            List<StockTransactionReportModel> data = MeditechApiHelper.Get<List<StockTransactionReportModel>>(requestApi);

            return data;
        }
        public List<StockTransactionReportModel> StockRequestReport(string requestID)
        {
            string requestApi = string.Format("Api/Report/StockRequestReport?requestID={0}", requestID);
            List<StockTransactionReportModel> data = MeditechApiHelper.Get<List<StockTransactionReportModel>>(requestApi);

            return data;
        }

        public List<StockSummaryModel> StockSummary(DateTime dateFrom, DateTime dateTo, string organisationList)
        {
            string requestApi = string.Format("Api/Report/StockSummary?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&organisationList={2}", dateFrom, dateTo, organisationList);
            List<StockSummaryModel> data = MeditechApiHelper.Get<List<StockSummaryModel>>(requestApi);

            return data;
        }

        public List<StockReportModel> StockConsumption(DateTime dateFrom, DateTime dateTo, string organisationList)
        {
            string requestApi = string.Format("Api/Report/StockConsumption?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&organisationList={2}", dateFrom, dateTo, organisationList);
            List<StockReportModel> data = MeditechApiHelper.Get<List<StockReportModel>>(requestApi);

            return data;
        }

        public List<GoodReceiveReportModel> GoodReceiveReport(string grnNumber)
        {
            string requestApi = string.Format("Api/Report/GoodReceiveReport?grnNumber={0}", grnNumber);
            List<GoodReceiveReportModel> data = MeditechApiHelper.Get<List<GoodReceiveReportModel>>(requestApi);

            return data;
        }

        public List<StockReportModel> StockAdjustmentOut(DateTime dateFrom, DateTime dateTo, string organisationList)
        {
            string requestApi = string.Format("Api/Report/StockAdjustmentOut?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&organisationList={2}", dateFrom, dateTo, organisationList);
            List<StockReportModel> data = MeditechApiHelper.Get<List<StockReportModel>>(requestApi);

            return data;
        }

        public List<StockReportModel> StockAdjustmentIn(DateTime dateFrom, DateTime dateTo, string organisationList)
        {
            string requestApi = string.Format("Api/Report/StockAdjustmentIn?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&organisationList={2}", dateFrom, dateTo, organisationList);
            List<StockReportModel> data = MeditechApiHelper.Get<List<StockReportModel>>(requestApi);

            return data;
        }

        public List<StockTransactionReportModel> StockDispose(DateTime dateFrom, DateTime dateTo, string organisationList)
        {
            string requestApi = string.Format("Api/Report/StockDispose?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&organisationList={2}", dateFrom, dateTo, organisationList);
            List<StockTransactionReportModel> data = MeditechApiHelper.Get<List<StockTransactionReportModel>>(requestApi);

            return data;
        }
        #endregion


        #region Radiology

        public List<RadiologyRDUReviewModel> GetRadiologyRDUReview(DateTime dateFrom, DateTime dateTo, string organisationList)
        {
            string requestApi = string.Format("Api/Report/GetRadiologyRDUReview?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&organisationList={2}", dateFrom, dateTo, organisationList);
            List<RadiologyRDUReviewModel> data = MeditechApiHelper.Get<List<RadiologyRDUReviewModel>>(requestApi);

            return data;
        }

        public PatientResultRadiology GetPatientResultRadiology(long resultUID)
        {
            string requestApi = string.Format("Api/Report/GetPatientResultRadiology?resultUID={0}", resultUID);
            PatientResultRadiology data = MeditechApiHelper.Get<PatientResultRadiology>(requestApi);

            return data;
        }

        #endregion

        #region Lab

        public List<PatientLabResult> GetLabResultByRequestNumber(long patientVisitUID, string requestNumber)
        {
            string requestApi = string.Format("Api/Report/GetLabResultByRequestNumber?patientVisitUID={0}&requestNumber={1}", patientVisitUID, requestNumber);
            List<PatientLabResult> data = MeditechApiHelper.Get<List<PatientLabResult>>(requestApi);

            return data;
        }

        #endregion
    }
}
