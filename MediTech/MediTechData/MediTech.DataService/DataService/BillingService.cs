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

        public PatientBillModel OLDGeneratePatientBill(PatientBillModel model)
        {
            PatientBillModel returnData = null;
            try
            {
                string requestApi = string.Format("Api/Billing/OLDGeneratePatientBill");
                returnData = MeditechApiHelper.Post<PatientBillModel, PatientBillModel>(requestApi, model);
            }
            catch (Exception)
            {

                throw;
            }
            return returnData;
        }

        public PatientBillModel GeneratePatientBill(GeneratePatientBillModel model)
        {
            PatientBillModel returnData = null;
            try
            {
                string requestApi = string.Format("Api/Billing/GeneratePatientBill");
                returnData = MeditechApiHelper.Post<GeneratePatientBillModel, PatientBillModel>(requestApi, model);
            }
            catch (Exception)
            {

                throw;
            }
            return returnData;
        }

        public List<PatientBillModel> GetPatientBill(long patientUID, long patientVisitUID)
        {
            string requestApi = string.Format("Api/Billing/GetPatientBill?patientUID={0}&patientVisitUID={1}", patientUID, patientVisitUID);
            List<PatientBillModel> data = MeditechApiHelper.Get<List<PatientBillModel>>(requestApi);
            return data;
        }

        public List<PatientBillModel> GetPatientBillByVisitPayorUID(long patientVisitPayorUID)
        {
            string requestApi = string.Format("Api/Billing/GetPatientBillByVisitPayorUID?patientVisitPayorUID={0}", patientVisitPayorUID);
            List<PatientBillModel> data = MeditechApiHelper.Get<List<PatientBillModel>>(requestApi);
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

        public List<PatientVisitModel> SearchUnbilledPatients(long? patientUID, DateTime? billFromDTTM, DateTime? billToDTTM, int? ownerOrganisationUID, string IsIP)
        {
            string requestApi = string.Format("Api/Billing/SearchUnbilledPatients?patientUID={0}&billFromDTTM={1:MM/dd/yyyy}&billToDTTM={2:MM/dd/yyyy}&ownerOrganisationUID={3}&IsIP={4}", patientUID, billFromDTTM, billToDTTM, ownerOrganisationUID, IsIP);
            List<PatientVisitModel> listPatBill = MeditechApiHelper.Get<List<PatientVisitModel>>(requestApi);

            return listPatBill;
        }
        public List<PatientBillModel> SearchPatientBill(DateTime? dateFrom, DateTime? dateTo, long? patientUID, string billNumber, string isIP, int? owerOrganisationUID,int userUID)
        {
            string requestApi = string.Format("Api/Billing/SearchPatientBill?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&patientUID={2}&billNumber={3}&isIP={4}&owerOrganisationUID={5}&userUID={6}", dateFrom, dateTo, patientUID, billNumber, isIP, owerOrganisationUID,userUID);
            List<PatientBillModel> listPatBill = MeditechApiHelper.Get<List<PatientBillModel>>(requestApi);

            return listPatBill;
        }

        public List<PatientVisitModel> pSearchPatientInvoiceForAllocateBill(long? patientUID, DateTime? billFromDTTM, DateTime? billToDTTM, int? insuranceComapnyUID, int? checkupJobUID
                 , int? ownerOrganisationUID, int? userUID)
        {
            string requestApi = string.Format("Api/Billing/pSearchPatientInvoiceForAllocateBill?patientUID={0}&billFromDTTM={1:MM/dd/yyyy}&billToDTTM={2:MM/dd/yyyy}&insuranceComapnyUID={3}&checkupJobUID={4}&ownerOrganisationUID={5}&userUID={6}", patientUID, billFromDTTM, billToDTTM, insuranceComapnyUID, checkupJobUID, ownerOrganisationUID,userUID);
            List<PatientVisitModel> listPatBill = MeditechApiHelper.Get<List<PatientVisitModel>>(requestApi);

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

        public bool CancelBillLists(List<PatientBillModel> bills)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/Billing/CancelBillLists");
                MeditechApiHelper.Put(requestApi, bills);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }
            return flag;
        }

        public string CheckPatientBillStatus(long patientUID, long patientVisitUID)
        {
            string requestApi = string.Format("Api/Billing/CheckPatientBillStatus?patientUID={0}&patientVisitUID={1}", patientUID, patientVisitUID);
            string isBillComplete = MeditechApiHelper.Get<string>(requestApi);

            return isBillComplete;
        }

        public string GetCompleteBill(long patientVisitUID)
        {
            string requestApi = string.Format("Api/Billing/GetCompleteBill?patientVisitUID={0}", patientVisitUID);
            string isBillComplete = MeditechApiHelper.Get<string>(requestApi);

            return isBillComplete;
        }

        public List<PatientPaymentDetailModel> GetPatientPaymentDetailByBillUID(long patientBillUID)
        {

            string requestApi = string.Format("Api/Billing/GetPatientPaymentDetailByBillUID?patientBillUID={0}", patientBillUID);
            List<PatientPaymentDetailModel> data = MeditechApiHelper.Get<List<PatientPaymentDetailModel>>(requestApi);
            return data;
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

        public bool ManagePatientPaymentDetail(List<PatientPaymentDetailModel> paymentMethod, int userUID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/Billing/ManagePatientPaymentDetail?userUID={0}", userUID);
                MeditechApiHelper.Post<List<PatientPaymentDetailModel>>(requestApi,paymentMethod);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }
            return flag;
        }

        public bool ManagePatientPaymentDetailForMass(List<PatientPaymentDetailModel> paymentMethod, int userUID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/Billing/ManagePatientPaymentDetailForMass?userUID={0}", userUID);
                MeditechApiHelper.Post<List<PatientPaymentDetailModel>>(requestApi, paymentMethod);
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

        public List<AllocatedPatBillableItemsAccountResultModel> GetPatientBillableItemsAccount(long patientUID, long patientVisitUID, int? packageUID, long? patientVisitPayorUID, DateTime startDate, DateTime endDate, int? accountUID, int? subAccountUID, int? locationUID)
        {
            string requestApi = string.Format(@"Api/Billing/GetPatientBillableItemsAccount?patientUID={0}&patientVisitUID={1}&packageUID={2}&patientVisitPayorUID={3}&startDate={4:MM/dd/yyyy HH:MM}&endDate={5:MM/dd/yyyy HH:MM}&accountUID={6}&subAccountUID={7}&locationUID={8}", patientUID, patientVisitUID, packageUID, patientVisitPayorUID, startDate, endDate, accountUID, subAccountUID, locationUID);
            List<AllocatedPatBillableItemsAccountResultModel> listPatBill = MeditechApiHelper.Get<List<AllocatedPatBillableItemsAccountResultModel>>(requestApi);

            return listPatBill;
        }


        public List<AllocatedPatBillableItemsSubAccountResultModel> GetPatientBillableItemsSubAccount(long patientUID, long patientVisitUID, int? packageUID, long? patientVisitPayorUID, DateTime startDate, DateTime endDate, string isPackage, int? accountUID, int? subAccountUID, int? locationUID)
        {
            string requestApi = string.Format(@"Api/Billing/GetPatientBillableItemsSubAccount?patientUID={0}&patientVisitUID={1}&packageUID={2}&patientVisitPayorUID={3}&startDate={4:MM/dd/yyyy}&endDate={5:MM/dd/yyyy}&isPackage={6}&accountUID={7}&subAccountUID={8}&locationUID={9}", patientUID, patientVisitUID, packageUID, patientVisitPayorUID, startDate, endDate, isPackage, accountUID, subAccountUID, locationUID);
            List<AllocatedPatBillableItemsSubAccountResultModel> listPatBill = MeditechApiHelper.Get<List<AllocatedPatBillableItemsSubAccountResultModel>>(requestApi);

            return listPatBill;
        }

        public List<AllocatedPatBillableItemsResultModel> GetPatientBillableItemsBySA(long patientUID, long patientVisitUID, int? packageUID, int? careproviderUID, int? billableItemUID, long? patientVisitPayorUID, DateTime startDate, DateTime endDate, string isPackage, int? accountUID, int? subAccountUID, int? locationUID)
        {
            string requestApi = string.Format(@"Api/Billing/GetPatientBillableItemsBySA?patientUID={0}&patientVisitUID={1}&packageUID={2}&careproviderUID={3}&billableItemUID={4}&patientVisitPayorUID={5}&startDate={6:MM/dd/yyyy}&endDate={7:MM/dd/yyyy}&isPackage={8}&accountUID={9}&subAccountUID={10}&locationUID={11}", patientUID, patientVisitUID, packageUID, careproviderUID, billableItemUID, patientVisitPayorUID, startDate, endDate, isPackage, accountUID, subAccountUID, locationUID);
            List<AllocatedPatBillableItemsResultModel> listPatBill = MeditechApiHelper.Get<List<AllocatedPatBillableItemsResultModel>>(requestApi);

            return listPatBill;
        }

        public List<AllocatedPatBillableItemsResultModel> GetAllocatedPatBillableItemsPalm(long patientUID, long patientVisitUID, int? accountUID, int? subAccountUID
   , long? patientVisitPayorUID, int? careProviderUID, DateTime dateFrom, DateTime dateTo)
        {
            string requestApi = string.Format(@"Api/Billing/GetAllocatedPatBillableItemsPalm?patientUID={0}&patientVisitUID={1}&accountUID={2}&subAccountUID={3}&patientVisitPayorUID={4}&careProviderUID={5}&dateFrom={6:MM/dd/yyyy}&dateTo={7:MM/dd/yyyy}", patientUID, patientVisitUID, accountUID, subAccountUID, patientVisitPayorUID, careProviderUID, dateFrom, dateTo);
            List<AllocatedPatBillableItemsResultModel> listPatBill = MeditechApiHelper.Get<List<AllocatedPatBillableItemsResultModel>>(requestApi);

            return listPatBill;
        }




        public bool AllocatePatientBillableItem(AllocatePatientBillableItemModel allocateModel)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/Billing/AllocatePatientBillableItem");
                MeditechApiHelper.Post<AllocatePatientBillableItemModel>(requestApi, allocateModel);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }
            return flag;

        }

        public PatientBillModel AllocateAndGenerateInvoiceOnly(AllocatePatientBillableItemModel allocateModel)
        {
            PatientBillModel returnData = null;
            try
            {
                string requestApi = string.Format("Api/Billing/AllocateAndGenerateInvoiceOnly");
                returnData = MeditechApiHelper.Post<AllocatePatientBillableItemModel, PatientBillModel>(requestApi, allocateModel);
            }
            catch (Exception)
            {

                throw;
            }
            return returnData;

        }

        public bool ManageSplitItem(AllocateSplitItemModel splitItemModel)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/Billing/ManageSplitItem");
                MeditechApiHelper.Post<AllocateSplitItemModel>(requestApi, splitItemModel);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }
            return flag;

        }

        public bool MergeBillRecipet(long patientVisitUID, long sourcePateintVisitPayorUID, long desPatientVisitPayorUID, DateTime dateFrom, DateTime dateTo)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/Billing/MergeBillRecipet?patientVisitUID={0}&sourcePateintVisitPayorUID={1}&desPatientVisitPayorUID={2}&dateFrom={3:MM/dd/yyyy}&dateTo={4:MM/dd/yyyy}", patientVisitUID, sourcePateintVisitPayorUID, desPatientVisitPayorUID, dateFrom, dateTo); ;
                MeditechApiHelper.Put(requestApi);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }
            return flag;
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

        #region BillPackage
        public List<BillPackageModel> SearchBillPackage(string code, string name, int? orderCategoryUID, int? orderSubCategoryUID)
        {
            string requestApi = string.Format("Api/Billing/SearchBillPackage?code={0}&name={1}&orderCategoryUID={2}&orderSubCategoryUID={3}", code, name, orderCategoryUID, orderSubCategoryUID);
            List<BillPackageModel> data = MeditechApiHelper.Get<List<BillPackageModel>>(requestApi);
            return data;
        }

        public List<BillPackageDetailModel> GetBillPackageItemByUID(int billPackageUID)
        {
            string requestApi = string.Format("Api/Billing/GetBillPackageItemByUID?billPackageUID={0}", billPackageUID);
            List<BillPackageDetailModel> data = MeditechApiHelper.Get<List<BillPackageDetailModel>>(requestApi);
            return data;
        }

        public List<BillPackageModel> GetBillPackageByOrderSubCategoryUID(int orderSubCategoryUID)
        {
            string requestApi = string.Format("Api/Billing/GetBillPackageByOrderSubCategoryUID?orderSubCategoryUID={0}", orderSubCategoryUID);
            List<BillPackageModel> data = MeditechApiHelper.Get<List<BillPackageModel>>(requestApi);
            return data;
        }

        public void ManageBillPackage(BillPackageModel billPackageModel, int userID)
        {
            try
            {
                string requestApi = string.Format("Api/Billing/ManageBillPackage?userID={0}", userID);
                MeditechApiHelper.Post(requestApi, billPackageModel);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public bool DeleteBillPackage(int billPackageUID, int userID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/Billing/DeleteBillPackage?billPackageUID={0}&userID={1}", billPackageUID, userID);
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
            List<PolicyMasterModel> data = MeditechApiHelper.Get<List<PolicyMasterModel>>("Api/Billing/GetPolicyMasterAll");
            return data;
        }
        public PolicyMasterModel GetPolicyMasterByUID(int policyUID)
        {
            string requestApi = string.Format("Api/Billing/GetPolicyMasterByUID?policyUID={0}", policyUID);
            PolicyMasterModel dataRequest = MeditechApiHelper.Get<PolicyMasterModel>(requestApi);

            return dataRequest;
        }

        public List<InsuranceCompanyModel> GetInsuranceCompanyAll()
        {
            List<InsuranceCompanyModel> data = MeditechApiHelper.Get<List<InsuranceCompanyModel>>("Api/Billing/GetInsuranceCompanyAll");
            return data;
        }
        public List<InsurancePlanModel> GetInsurancePlanAll()
        {
            List<InsurancePlanModel> data = MeditechApiHelper.Get<List<InsurancePlanModel>>("Api/Billing/GetInsurancePlanAll");
            return data;
        }

        public List<InsurancePlanModel> GetInsurancePlansGroupPayorCompany()
        {
            List<InsurancePlanModel> data = MeditechApiHelper.Get<List<InsurancePlanModel>>("Api/Billing/GetInsurancePlansGroupPayorCompany");
            return data;
        }

        public List<InsuranceCompanyModel> SearchInsuranceCompany(string code, string name)
        {
            string requestApi = string.Format("Api/Billing/SearchInsuranceCompany?code={0}&name={1}", code, name);
            List<InsuranceCompanyModel> dataRequest = MeditechApiHelper.Get<List<InsuranceCompanyModel>>(requestApi);

            return dataRequest;
        }

        public List<InsurancePlanModel> SearchInsurancePlaneByINCO(int? insuranceCompanyUID)
        {
            string requestApi = string.Format("Api/Billing/SearchInsurancePlaneByINCO?insuranceCompanyUID={0}", insuranceCompanyUID);
            List<InsurancePlanModel> dataRequest = MeditechApiHelper.Get<List<InsurancePlanModel>>(requestApi);

            return dataRequest;
        }

        public void ManageInsuranceCompany(InsuranceCompanyModel insuranceCompanyModel, int userID)
        {
            try
            {
                string requestApi = string.Format("Api/Billing/ManageInsuranceCompany?userID={0}", userID);
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
                string requestApi = string.Format("Api/Billing/DeleteInsuranceCompany?insuranceCompanyUID={0}&userID={1}", insuranceCompanyUID, userID);
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
                string requestApi = string.Format("Api/Billing/ManageInsurancePlan?userID={0}", userID);
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
                string requestApi = string.Format("Api/Billing/DeleteInsurancePlan?insurancePlanUID={0}&userID={1}", insurancePlanUID, userID);
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
                string requestApi = string.Format("Api/Billing/ManagePayorOfficeDetail?userID={0}", userID);
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
                string requestApi = string.Format("Api/Billing/DeletePayorOfficeDetail?payorDetailUID={0}&userID={1}", payorDetailUID, userID);
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
                string requestApi = string.Format("Api/Billing/ManagePayorAgreement?userID={0}", userID);
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
                string requestApi = string.Format("Api/Billing/DeletePayorAgreement?payorAgreementUID={0}&userID={1}", payorAgreementUID, userID);
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
            string requestApi = string.Format("Api/Billing/SearchPayorDetailByINCO?code={0}&insuranceCompany={1}", code, insuranceCompany);
            List<PayorDetailModel> dataRequest = MeditechApiHelper.Get<List<PayorDetailModel>>(requestApi);

            return dataRequest;
        }

        public List<PayorAgreementModel> SearchPayorAgreementByINCO(string code, int? insuranceCompany)
        {
            string requestApi = string.Format("Api/Billing/SearchPayorAgreementByINCO?code={0}&insuranceCompany={1}", code, insuranceCompany);
            List<PayorAgreementModel> dataRequest = MeditechApiHelper.Get<List<PayorAgreementModel>>(requestApi);

            return dataRequest;
        }

        public InsurancePlanModel CheckInsurancePlan(int? payorDetailUID, int? payorAgreementUID)
        {
            string requestApi = string.Format("Api/Billing/CheckInsurancePlan?payorDetailUID={0}&payorAgreementUID={1}", payorDetailUID, payorAgreementUID);
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

        public List<PayorDetailModel> GetPayorDetailByInsuranceCompanyUID(int InsuranceCompanyUID)
        {
            string requestApi = string.Format("Api/Billing/GetPayorDetailByInsuranceCompanyUID?InsuranceCompanyUID={0}", InsuranceCompanyUID);
            List<PayorDetailModel> dataRequest = MeditechApiHelper.Get<List<PayorDetailModel>>(requestApi);

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
        public List<PayorAgreementModel> GetAgreementByInsuranceUID(int insuranceUID)
        {
            string requestApi = string.Format("Api/Billing/GetAgreementByInsuranceUID?insuranceUID={0}", insuranceUID);
            List<PayorAgreementModel> dataRequest = MeditechApiHelper.Get<List<PayorAgreementModel>>(requestApi);

            return dataRequest;
        }

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

        public PayorAgreementModel GetPolicyForPayorAgreement(int policyMasterUID)
        {
            string requestApi = string.Format("Api/Billing/GetPolicyForPayorAgreement?policyMasterUID={0}", policyMasterUID);
            PayorAgreementModel dataRequest = MeditechApiHelper.Get<PayorAgreementModel>(requestApi);

            return dataRequest;
        }

        public List<AgreementAccountDiscountModel> GetAgreementAccountByAgreementUID(int payorAgreementUID)
        {
            string requestApi = string.Format("Api/Billing/GetAgreementAccountByAgreementUID?payorAgreementUID={0}", payorAgreementUID);
            List<AgreementAccountDiscountModel> dataRequest = MeditechApiHelper.Get<List<AgreementAccountDiscountModel>>(requestApi);

            return dataRequest;
        }

        public List<AgreementDetailDiscountModel> GetAgreementAccountDetailByAgreementUID(int payorAgreementUID)
        {
            string requestApi = string.Format("Api/Billing/GetAgreementAccountDetailByAgreementUID?payorAgreementUID={0}", payorAgreementUID);
            List<AgreementDetailDiscountModel> dataRequest = MeditechApiHelper.Get<List<AgreementDetailDiscountModel>>(requestApi);

            return dataRequest;
        }

        public List<AgreementItemDiscountModel> GetAgreementItemByAgreementUID(int payorAgreementUID)
        {
            string requestApi = string.Format("Api/Billing/GetAgreementItemByAgreementUID?payorAgreementUID={0}", payorAgreementUID);
            List<AgreementItemDiscountModel> dataRequest = MeditechApiHelper.Get<List<AgreementItemDiscountModel>>(requestApi);

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

        public List<PolicyMasterModel> SearchPolicyMaster(string code, string name)
        {
            string requestApi = string.Format("Api/Billing/SearchPolicyMaster?code={0}&name={1}", code, name);
            List<PolicyMasterModel> dataRequest = MeditechApiHelper.Get<List<PolicyMasterModel>>(requestApi);

            return dataRequest;
        }

        public PolicyMasterModel GetPolicyOrder(int policyMasterUID)
        {
            string requestApi = string.Format("Api/Billing/GetPolicyOrder?policyMasterUID={0}", policyMasterUID);
            PolicyMasterModel dataRequest = MeditechApiHelper.Get<PolicyMasterModel>(requestApi);

            return dataRequest;
        }

        public void ManagePolicyMaster(PolicyMasterModel policyMaster, int userID)
        {
            try
            {
                string requestApi = string.Format("Api/Billing/ManagePolicyMaster?userID={0}", userID);
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
                string requestApi = string.Format("Api/Billing/DeletePolicyMaster?policyUID={0}&userID={1}", policyUID, userID);
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

    }
}
