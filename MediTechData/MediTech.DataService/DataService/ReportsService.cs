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

        #endregion

        #region Cashier
        public List<PatientRevenueModel> GetPatientNetProfit(DateTime dateFrom, DateTime dateTo, int? vistyuid, int? organisationUID)
        {
            string requestApi = string.Format("Api/Report/GetPatientNetProfit?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&vistyuid={2}&organisationUID={3}", dateFrom, dateTo, vistyuid, organisationUID);
            List<PatientRevenueModel> data = MeditechApiHelper.Get<List<PatientRevenueModel>>(requestApi);

            return data;
        }

        public List<PatientRevenueModel> GetUsedReport(DateTime dateFrom, DateTime dateTo, int? organisationUID)
        {
            string requestApi = string.Format("Api/Report/GetUsedReport?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&organisationUID={2}", dateFrom, dateTo, organisationUID);
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

        #endregion

        #region Patient

        public List<ProblemStatisticModel> PatientProblemStaistic(DateTime dateFrom, DateTime dateTo, int? vistyuid, int? organisationUID)
        {
            string requestApi = string.Format("Api/Report/PatientProblemStaistic?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&vistyuid={2}&organisationUID={3}", dateFrom, dateTo, vistyuid, organisationUID);
            List<ProblemStatisticModel> data = MeditechApiHelper.Get<List<ProblemStatisticModel>>(requestApi);

            return data;
        }

        #endregion

        #region Registration

        public List<PatientSummaryModel> PatientSummaryPerMonth(int year, string monthLists, int? organisationUID)
        {
            string requestApi = string.Format("Api/Report/PatientSummaryPerMonth?year={0}&monthLists={1}&organisationUID={2}", year, monthLists, organisationUID);
            List<PatientSummaryModel> data = MeditechApiHelper.Get<List<PatientSummaryModel>>(requestApi);

            return data;
        }

        public List<ChartStatisticModel> VisitDaysStatistic(DateTime dateFrom, DateTime dateTo, int? vistyuid, int? organisationUID)
        {
            string requestApi = string.Format("Api/Report/VisitDaysStatistic?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&vistyuid={2}&organisationUID={3}", dateFrom, dateTo, vistyuid, organisationUID);
            List<ChartStatisticModel> data = MeditechApiHelper.Get<List<ChartStatisticModel>>(requestApi);

            return data;
        }

        public List<ChartStatisticModel> VisitTimesStatistic(DateTime dateFrom, DateTime dateTo, int? vistyuid, int? organisationUID)
        {
            string requestApi = string.Format("Api/Report/VisitTimesStatistic?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&vistyuid={2}&organisationUID={3}", dateFrom, dateTo, vistyuid, organisationUID);
            List<ChartStatisticModel> data = MeditechApiHelper.Get<List<ChartStatisticModel>>(requestApi);

            return data;
        }

        public List<PatientSumByAreaModel> PatientSumByAreaPerMonth(DateTime dateFrom, DateTime dateTo, int? vistyuid, int? organisationUID)
        {
            string requestApi = string.Format("Api/Report/PatientSumByAreaPerMonth?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&vistyuid={2}&organisationUID={3}", dateFrom, dateTo, vistyuid, organisationUID);
            List<PatientSumByAreaModel> data = MeditechApiHelper.Get<List<PatientSumByAreaModel>>(requestApi);

            return data;
        }

        public List<PatientSummaryDataModel> PatientSummeryData(DateTime dateFrom, DateTime dateTo, int? organisationUID)
        {
            string requestApi = string.Format("Api/Report/PatientSummaryData?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&organisationUID={2}", dateFrom, dateTo, organisationUID);
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

        public List<CheckupBookModel> PrintCheckupBook(long patientUID, long patientVisitUID)
        {
            string requestApi = string.Format("Api/Report/PrintCheckupBook?patientUID={0}&patientVisitUID={1}", patientUID, patientVisitUID);
            List<CheckupBookModel> data = MeditechApiHelper.Get<List<CheckupBookModel>>(requestApi);

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

        public List<StockReportModel> StockOnHand(int? organisationUID)
        {
            string requestApi = string.Format("Api/Report/StockOnHand?organisationUID={0}", organisationUID);
            List<StockReportModel> data = MeditechApiHelper.Get<List<StockReportModel>>(requestApi);

            return data;
        }


        public List<StockReportModel> StockDispensedReport(DateTime dateFrom, DateTime dateTo, int? organisationUID)
        {
            string requestApi = string.Format("Api/Report/StockDispensedReport?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&organisationUID={2}", dateFrom, dateTo, organisationUID);
            List<StockReportModel> data = MeditechApiHelper.Get<List<StockReportModel>>(requestApi);

            return data;
        }

        public List<StockReportModel> StockNonMovement(DateTime dateFrom, DateTime dateTo, int? organisationUID)
        {
            string requestApi = string.Format("Api/Report/StockNonMovement?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&organisationUID={2}", dateFrom, dateTo, organisationUID);
            List<StockReportModel> data = MeditechApiHelper.Get<List<StockReportModel>>(requestApi);

            return data;
        }

        public List<StockReportModel> StockExpiryReport(int month, int? organisationUID)
        {
            string requestApi = string.Format("Api/Report/StockExpiryReport?month={0}&organisationUID={1}", month, organisationUID);
            List<StockReportModel> data = MeditechApiHelper.Get<List<StockReportModel>>(requestApi);

            return data;
        }

        public List<StockReportModel> StockExpiredReport(int? organisationUID)
        {
            string requestApi = string.Format("Api/Report/StockExpiredReport?organisationUID={0}", organisationUID);
            List<StockReportModel> data = MeditechApiHelper.Get<List<StockReportModel>>(requestApi);

            return data;
        }

        public List<StockReportModel> StockGoodReceiveReport(DateTime dateFrom, DateTime dateTo, int? organisationUID)
        {
            string requestApi = string.Format("Api/Report/StockGoodReceiveReport?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&organisationUID={2}", dateFrom, dateTo, organisationUID);
            List<StockReportModel> data = MeditechApiHelper.Get<List<StockReportModel>>(requestApi);

            return data;
        }


        public List<StockReceiveReportModel> StockReceiveReportAll(DateTime dateFrom, DateTime dateTo, int? organisationUID)
        {
            string requestApi = string.Format("Api/Report/StockReceiveReportAll?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&organisationUID={2}", dateFrom, dateTo, organisationUID);
            List<StockReceiveReportModel> data = MeditechApiHelper.Get<List<StockReceiveReportModel>>(requestApi);

            return data;
        }

        public List<StockReportModel> StockBalancePerMounth(int year, string monthLists, int? organisationUID)
        {
            string requestApi = string.Format("Api/Report/StockBalancePerMounth?year={0}&monthLists={1}&organisationUID={2}", year, monthLists, organisationUID);
            List<StockReportModel> data = MeditechApiHelper.Get<List<StockReportModel>>(requestApi);

            return data;
        }

        public List<StockTransactionReportModel> StockIssueReport(string issueID)
        {
            string requestApi = string.Format("Api/Report/StockIssueReport?issueID={0}", issueID);
            List<StockTransactionReportModel> data = MeditechApiHelper.Get<List<StockTransactionReportModel>>(requestApi);

            return data;
        }

        public List<StockTransactionReportModel> StockTransferredReport(DateTime dateFrom, DateTime dateTo, int? organisationUID)
        {
            string requestApi = string.Format("Api/Report/StockTransferredReport?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&organisationUID={2}", dateFrom, dateTo, organisationUID);
            List<StockTransactionReportModel> data = MeditechApiHelper.Get<List<StockTransactionReportModel>>(requestApi);

            return data;
        }

        public List<StockTransactionReportModel> StockRequestReport(string requestID)
        {
            string requestApi = string.Format("Api/Report/StockRequestReport?requestID={0}", requestID);
            List<StockTransactionReportModel> data = MeditechApiHelper.Get<List<StockTransactionReportModel>>(requestApi);

            return data;
        }

        public List<StockSummeryModel> StockSummery(DateTime dateFrom, DateTime dateTo, int? organisationUID)
        {
            string requestApi = string.Format("Api/Report/StockSummery?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&organisationUID={2}", dateFrom, dateTo, organisationUID);
            List<StockSummeryModel> data = MeditechApiHelper.Get<List<StockSummeryModel>>(requestApi);

            return data;
        }

        public List<StockReportModel> StockConsumption(DateTime dateFrom, DateTime dateTo, int? organisationUID)
        {
            string requestApi = string.Format("Api/Report/StockConsumption?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&organisationUID={2}", dateFrom, dateTo, organisationUID);
            List<StockReportModel> data = MeditechApiHelper.Get<List<StockReportModel>>(requestApi);

            return data;
        }

        public List<GoodReceiveReportModel> GoodReceiveReport(string grnNumber)
        {
            string requestApi = string.Format("Api/Report/GoodReceiveReport?grnNumber={0}", grnNumber);
            List<GoodReceiveReportModel> data = MeditechApiHelper.Get<List<GoodReceiveReportModel>>(requestApi);

            return data;
        }

        public List<StockReportModel> StockAdjustmentOut(DateTime dateFrom, DateTime dateTo, int? organisationUID)
        {
            string requestApi = string.Format("Api/Report/StockAdjustmentOut?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&organisationUID={2}", dateFrom, dateTo, organisationUID);
            List<StockReportModel> data = MeditechApiHelper.Get<List<StockReportModel>>(requestApi);

            return data;
        }

        public List<StockReportModel> StockAdjustmentIn(DateTime dateFrom, DateTime dateTo, int? organisationUID)
        {
            string requestApi = string.Format("Api/Report/StockAdjustmentIn?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&organisationUID={2}", dateFrom, dateTo, organisationUID);
            List<StockReportModel> data = MeditechApiHelper.Get<List<StockReportModel>>(requestApi);

            return data;
        }

        #endregion


        #region Radiology

        public List<RadiologyRDUReviewModel> GetRadiologyRDUReview(DateTime dateFrom, DateTime dateTo, int? organisationUID)
        {
            string requestApi = string.Format("Api/Report/GetRadiologyRDUReview?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&organisationUID={2}", dateFrom, dateTo, organisationUID);
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
