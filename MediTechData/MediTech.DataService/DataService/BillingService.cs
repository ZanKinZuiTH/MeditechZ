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

        #region Billconfiguration

        public BillConfigurationModel GetBillConFiguration(int ownerOrganisationUID)
        {
            string requestApi = string.Format("Api/Billing/GetBillConFiguration?ownerOrganisationUID={0}", ownerOrganisationUID);
            BillConfigurationModel dataRequest = MeditechApiHelper.Get<BillConfigurationModel>(requestApi);

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

        public InsuranceCompanyModel GetInsuranceCompanyByUID(int insuranceCompanyUID)
        {
            string requestApi = string.Format("Api/Billing/GetInsuranceCompanyByUID?insuranceCompanyUID={0}", insuranceCompanyUID);
            InsuranceCompanyModel dataRequest = MeditechApiHelper.Get<InsuranceCompanyModel>(requestApi);

            return dataRequest;
        }

        public List<InsurancePlanModel> GetInsurancePlans(int insuranceCompanyUID)
        {
            string requestApi = string.Format("Api/Billing/GetInsurancePlans?insuranceCompanyUID={0}", insuranceCompanyUID);
            List<InsurancePlanModel> dataRequest = MeditechApiHelper.Get<List<InsurancePlanModel>>(requestApi);

            return dataRequest;
        }

        public List<PolicyMasterModel> GetPolicyMasterAll()
        {
            List<PolicyMasterModel> data = MeditechApiHelper.Get<List<PolicyMasterModel>>("Api/MasterData/GetPolicyMasterAll");
            return data;
        }
        public PolicyMasterModel GetPolicyMasterByUID(int policyUID)
        {
            string requestApi = string.Format("Api/MasterData/GetPolicyMasterByUID?policyUID={0}", policyUID);
            PolicyMasterModel dataRequest = MeditechApiHelper.Get<PolicyMasterModel>(requestApi);

            return dataRequest;
        }

        public List<InsuranceCompanyModel> GetInsuranceCompanyAll()
        {
            List<InsuranceCompanyModel> data = MeditechApiHelper.Get<List<InsuranceCompanyModel>>("Api/MasterData/GetInsuranceCompanyAll");
            return data;
        }
        public List<InsurancePlanModel> GetInsurancePlanAll()
        {
            List<InsurancePlanModel> data = MeditechApiHelper.Get<List<InsurancePlanModel>>("Api/MasterData/GetInsurancePlanAll");
            return data;
        }


        public List<InsuranceCompanyModel> SearchInsuranceCompany(string code, string name)
        {
            string requestApi = string.Format("Api/MasterData/SearchInsuranceCompany?code={0}&name={1}", code, name);
            List<InsuranceCompanyModel> dataRequest = MeditechApiHelper.Get<List<InsuranceCompanyModel>>(requestApi);

            return dataRequest;
        }

        public List<InsurancePlanModel> SearchInsurancePlaneByINCO(int? insuranceCompanyUID)
        {
            string requestApi = string.Format("Api/MasterData/SearchInsurancePlaneByINCO?insuranceCompanyUID={0}", insuranceCompanyUID);
            List<InsurancePlanModel> dataRequest = MeditechApiHelper.Get<List<InsurancePlanModel>>(requestApi);

            return dataRequest;
        }

        public void ManageInsuranceCompany(InsuranceCompanyModel insuranceCompanyModel, int userID)
        {
            try
            {
                string requestApi = string.Format("Api/MasterData/ManageInsuranceCompany?userID={0}", userID);
                MeditechApiHelper.Post(requestApi, insuranceCompanyModel);
            }
            catch (Exception ex)
            {

                throw (ex);
            }
        }

        public bool DeleteInsuranceCompany(int insuranceCompanyUID, int userID)
        {
            bool flag = false;

            try
            {
                string requestApi = string.Format("Api/MasterData/DeleteInsuranceCompany?insuranceCompanyUID={0}&userID={1}", insuranceCompanyUID, userID);
                MeditechApiHelper.Delete(requestApi);
                flag = true;
            }
            catch (Exception)
            {
                throw;
            }

            return flag;
        }

        public void ManageInsurancePlan(InsurancePlanModel insurancePlanModel, int userID)
        {
            try
            {
                string requestApi = string.Format("Api/MasterData/ManageInsurancePlan?userID={0}", userID);
                MeditechApiHelper.Post(requestApi, insurancePlanModel);
            }
            catch (Exception ex)
            {

                throw (ex);
            }
        }
        public bool DeleteInsurancePlan(int insurancePlanUID, int userID)
        {
            bool flag = false;

            try
            {
                string requestApi = string.Format("Api/MasterData/DeleteInsurancePlan?insurancePlanUID={0}&userID={1}", insurancePlanUID, userID);
                MeditechApiHelper.Delete(requestApi);
                flag = true;
            }
            catch (Exception)
            {
                throw;
            }

            return flag;
        }

        public void ManagePayorOfficeDetail(PayorDetailModel payorDetailModel, int userID)
        {
            try
            {
                string requestApi = string.Format("Api/MasterData/ManagePayorOfficeDetail?userID={0}", userID);
                MeditechApiHelper.Post(requestApi, payorDetailModel);
            }
            catch (Exception ex)
            {

                throw (ex);
            }
        }

        public bool DeletePayorOfficeDetail(int payorDetailUID, int userID)
        {
            bool flag = false;

            try
            {
                string requestApi = string.Format("Api/MasterData/DeletePayorOfficeDetail?payorDetailUID={0}&userID={1}", payorDetailUID, userID);
                MeditechApiHelper.Delete(requestApi);
                flag = true;
            }
            catch (Exception)
            {
                throw;
            }

            return flag;
        }

        public void ManagePayorAgreement(PayorAgreementModel payorAgreementModel, int userID)
        {
            try
            {
                string requestApi = string.Format("Api/MasterData/ManagePayorAgreement?userID={0}", userID);
                MeditechApiHelper.Post(requestApi, payorAgreementModel);
            }
            catch (Exception ex)
            {

                throw (ex);
            }
        }

        public bool DeletePayorAgreement(int payorAgreementUID, int userID)
        {
            bool flag = false;

            try
            {
                string requestApi = string.Format("Api/MasterData/DeletePayorAgreement?payorAgreementUID={0}&userID={1}", payorAgreementUID, userID);
                MeditechApiHelper.Delete(requestApi);
                flag = true;
            }
            catch (Exception)
            {
                throw;
            }

            return flag;
        }

        public List<PayorDetailModel> SearchPayorDetailByINCO(string code, int? insuranceCompany)
        {
            string requestApi = string.Format("Api/MasterData/SearchPayorDetailByINCO?code={0}&insuranceCompany={1}", code, insuranceCompany);
            List<PayorDetailModel> dataRequest = MeditechApiHelper.Get<List<PayorDetailModel>>(requestApi);

            return dataRequest;
        }

        public List<PayorAgreementModel> SearchPayorAgreementByINCO(string code, int? insuranceCompany)
        {
            string requestApi = string.Format("Api/MasterData/SearchPayorAgreementByINCO?code={0}&insuranceCompany={1}", code, insuranceCompany);
            List<PayorAgreementModel> dataRequest = MeditechApiHelper.Get<List<PayorAgreementModel>>(requestApi);

            return dataRequest;
        }
        public List<PolicyMasterModel> SearchPolicyMaster(string code, string name)
        {
            string requestApi = string.Format("Api/MasterData/SearchPolicyMaster?code={0}&name={1}", code, name);
            List<PolicyMasterModel> dataRequest = MeditechApiHelper.Get<List<PolicyMasterModel>>(requestApi);

            return dataRequest;
        }

        public void ManagePolicyMaster(PolicyMasterModel policyMaster, int userID)
        {
            try
            {
                string requestApi = string.Format("Api/MasterData/ManagePolicyMaster?userID={0}", userID);
                MeditechApiHelper.Post(requestApi, policyMaster);
            }
            catch (Exception ex)
            {

                throw (ex);
            }
        }

        public bool DeletePolicyMaster(int policyUID, int userID)
        {
            bool flag = false;

            try
            {
                string requestApi = string.Format("Api/MasterData/DeletePolicyMaster?policyUID={0}&userID={1}", policyUID, userID);
                MeditechApiHelper.Delete(requestApi);
                flag = true;
            }
            catch (Exception)
            {
                throw;
            }
            return flag;
        }

        public InsurancePlanModel CheckInsurancePlan(int? payorDetailUID, int? payorAgreementUID)
        {
            string requestApi = string.Format("Api/MasterData/CheckInsurancePlan?payorDetailUID={0}&payorAgreementUID={1}", payorDetailUID, payorAgreementUID);
            InsurancePlanModel dataRequest = MeditechApiHelper.Get<InsurancePlanModel>(requestApi);

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
        public List<PayorAgreementModel> GetPayorAgreementByPayorDetailUID(int payorDetailUID)
        {
            string requestApi = string.Format("Api/Billing/GetAgreementByPayorDetailUID?payorDetailUID={0}", payorDetailUID);
            List<PayorAgreementModel> dataRequest = MeditechApiHelper.Get<List<PayorAgreementModel>>(requestApi);

            return dataRequest;
        }

        public PayorAgreementModel GetPayorAgreementByUID(int agreementUID)
        {
            string requestApi = string.Format("Api/Billing/GetPayorAgreementByUID?agreementUID={0}", agreementUID);
            PayorAgreementModel dataRequest = MeditechApiHelper.Get<PayorAgreementModel>(requestApi);

            return dataRequest;
        }
        #endregion

        #region PolicyMaster
        public PolicyMasterModel GetPolicyMaster(int policyMasterUID)
        {
            string requestApi = string.Format("Api/Billing/GetPolicyMaster?policyMasterUID={0}", policyMasterUID);
            PolicyMasterModel dataRequest = MeditechApiHelper.Get<PolicyMasterModel>(requestApi);

            return dataRequest;
        }

        #endregion

    }
}
