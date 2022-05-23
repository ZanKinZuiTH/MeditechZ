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
    public class BillingService
    {
        public bool SaveOrderForRIS(AddRequestForRISModel patientRequest, int userID, int ownerUID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/Billing/SaveOrderForRIS?userID={0}&ownerUID={1}", userID, ownerUID);
                MeditechApiHelper.Post<AddRequestForRISModel>(requestApi, patientRequest);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }
            return flag;

        }

        public PatientBillModel GeneratePatientBill(PatientBillModel model)
        {
            PatientBillModel returnData = null;
            try
            {
                string requestApi = string.Format("Api/Billing/GeneratePatientBill");
                returnData = MeditechApiHelper.Post<PatientBillModel, PatientBillModel>(requestApi, model);
            }
            catch (Exception)
            {

                throw;
            }
            return returnData;
        }

        public PatientBillModel GetPatientBill(long patientUID, long patientVisitUID)
        {
            string requestApi = string.Format("Api/Billing/GetPatientBill?patientUID={0}&patientVisitUID={1}", patientUID, patientVisitUID);
            PatientBillModel data = MeditechApiHelper.Get<PatientBillModel>(requestApi);
            return data;
        }

        public List<PatientBilledItemModel> GetPatientBillingGroup(long PatientBillUID)
        {

            string requestApi = string.Format("Api/Billing/GetPatientBillingGroup?PatientBillUID={0}", PatientBillUID);
            List<PatientBilledItemModel> data = MeditechApiHelper.Get<List<PatientBilledItemModel>>(requestApi);
            return data;
        }

        public List<PatientBilledItemModel> GetPatientBilledItem(long patientBillUID)
        {

            string requestApi = string.Format("Api/Billing/GetPatientBilledItem?patientBillUID={0}", patientBillUID);
            List<PatientBilledItemModel> data = MeditechApiHelper.Get<List<PatientBilledItemModel>>(requestApi);
            return data;
        }

        public List<PatientBilledItemModel> GetPatientBilledOrderDetail(long patientBillUID)
        {

            string requestApi = string.Format("Api/Billing/GetPatientBilledOrderDetail?patientBillUID={0}", patientBillUID);
            List<PatientBilledItemModel> data = MeditechApiHelper.Get<List<PatientBilledItemModel>>(requestApi);
            return data;
        }

        public List<PatientBillModel> SearchPatientBill(DateTime? dateFrom, DateTime? dateTo, long? patientUID, string billNumber, int? owerOrganisationUID)
        {
            string requestApi = string.Format("Api/Billing/SearchPatientBill?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&patientUID={2}&billNumber={3}&owerOrganisationUID={4}", dateFrom, dateTo, patientUID, billNumber, owerOrganisationUID);
            List<PatientBillModel> listPatBill = MeditechApiHelper.Get<List<PatientBillModel>>(requestApi);

            return listPatBill;
        }

        public bool CancelBill(long patientBillUID, string cancelReason, int userUID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/Billing/CancelBill?patientBillUID={0}&cancelReason={1}&userUID={2}", patientBillUID, cancelReason, userUID);
                MeditechApiHelper.Put(requestApi);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }
            return flag;
        }

        public bool UpdatePaymentMethod(long patientBillUID, int PAYMDUID, int userUID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/Billing/UpdatePaymentMethod?patientBillUID={0}&PAYMDUID={1}&userUID={2}", patientBillUID, PAYMDUID, userUID);
                MeditechApiHelper.Put(requestApi);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }
            return flag;
        }

        public List<PatientBillModel> PrintStatementBill(long patientBillUID)
        {
            string requestApi = string.Format("Api/Billing/PrintStatementBill?patientBillUID={0}", patientBillUID);
            List<PatientBillModel> listPatBill = MeditechApiHelper.Get<List<PatientBillModel>>(requestApi);

            return listPatBill;
        }

        public List<GroupReceiptDetailModel> GetGroupReceiptDetail(int groupReceiptUID)
        {
            string requestApi = string.Format("Api/Billing/GetGroupReceiptDetail?groupReceiptUID={0}", groupReceiptUID);
            List<GroupReceiptDetailModel> data = MeditechApiHelper.Get<List<GroupReceiptDetailModel>>(requestApi);
            return data;
        }


        #region BillgingGroup

        public List<BillingGroupModel> GetBillingGroup()
        {
            string requestApi = string.Format("Api/Billing/GetBillingGroup");
            List<BillingGroupModel> dataRequest = MeditechApiHelper.Get<List<BillingGroupModel>>(requestApi);

            return dataRequest;
        }

        #endregion

        #region BillgingSubGroup
        public List<BillingSubGroupModel> GetBillingSubGroup()
        {
            string requestApi = string.Format("Api/Billing/GetBillingSubGroup");
            List<BillingSubGroupModel> dataRequest = MeditechApiHelper.Get<List<BillingSubGroupModel>>(requestApi);

            return dataRequest;
        }

        public List<BillingSubGroupModel> GetBillingSubGroupByGroup(int billingGroupUID)
        {
            string requestApi = string.Format("Api/Billing/GetBillingSubGroupByGroup?billingGroupUID={0}", billingGroupUID);
            List<BillingSubGroupModel> dataRequest = MeditechApiHelper.Get<List<BillingSubGroupModel>>(requestApi);

            return dataRequest;
        }


        #endregion

        #region Insurance
        public List<InsuranceCompanyModel> GetInsuranceCompanies()
        {
            string requestApi = string.Format("Api/Billing/GetInsuranceCompanies");
            List<InsuranceCompanyModel> dataRequest = MeditechApiHelper.Get<List<InsuranceCompanyModel>>(requestApi);

            return dataRequest;
        }

        public List<InsurancePlanModel> GetInsurancePlans(int insuranceCompanyUID)
        {
            string requestApi = string.Format("Api/Billing/GetInsurancePlans?insuranceCompanyUID={0}", insuranceCompanyUID);
            List<InsurancePlanModel> dataRequest = MeditechApiHelper.Get<List<InsurancePlanModel>>(requestApi);

            return dataRequest;
        }
        #endregion

        #region PayorDetail

        public List<PayorDetailModel> SearchPayorDetail(string code, string name)
        {
            string requestApi = string.Format("Api/Billing/SearchPayorDetail?code={0}&name={1}", code, name);
            List<PayorDetailModel> dataRequest = MeditechApiHelper.Get<List<PayorDetailModel>>(requestApi);

            return dataRequest;
        }
        public List<PayorDetailModel> GetPayorDetail()
        {
            string requestApi = string.Format("Api/Billing/GetPayorDetail");
            List<PayorDetailModel> dataRequest = MeditechApiHelper.Get<List<PayorDetailModel>>(requestApi);

            return dataRequest;
        }

        public PayorDetailModel GetPayorDetailByUID(int payorDetailUID)
        {
            string requestApi = string.Format("Api/Billing/GetPayorDetailByUID?payorDetailUID={0}", payorDetailUID);
            PayorDetailModel dataRequest = MeditechApiHelper.Get<PayorDetailModel>(requestApi);

            return dataRequest;
        }

        public PayorDetailModel GetPayorDetailByCode(string payorCode)
        {
            string requestApi = string.Format("Api/Billing/GetPayorDetailByCode?payorCode={0}", payorCode);
            PayorDetailModel dataRequest = MeditechApiHelper.Get<PayorDetailModel>(requestApi);
            return dataRequest;
        }



        public bool ManagePayorDetail(PayorDetailModel payorDetailModel, int userID)
        {
            bool flag = false;

            try
            {
                string requestApi = string.Format("Api/Billing/ManagePayorDetail?userID={0}", userID);
                MeditechApiHelper.Post<PayorDetailModel>(requestApi, payorDetailModel);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }


            return flag;

        }

        public bool DeletePayorDetail(int payorDetailUID, int userID)
        {
            bool flag = false;

            try
            {
                string requestApi = string.Format("Api/Billing/DeletePayorDetail?payorDetailUID={0}&userID={1}", payorDetailUID, userID);
                MeditechApiHelper.Delete(requestApi);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }


            return flag;

        }

        #endregion

        #region PayorAgreement
        public List<PayorAgreementModel> GetAgreementByPayorDetailUID(int payorDetailUID)
        {
            string requestApi = string.Format("Api/Billing/GetAgreementByPayorDetailUID?payorDetailUID={0}", payorDetailUID);
            List<PayorAgreementModel> dataRequest = MeditechApiHelper.Get<List<PayorAgreementModel>>(requestApi);

            return dataRequest;
        }
        #endregion

    }
}
