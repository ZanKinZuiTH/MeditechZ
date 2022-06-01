using MediTech.Model;
using ShareLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.DataService
{
    public class MasterDataService
    {
        #region RequestItem

        public List<RequestItemModel> GetRequestItems()
        {
            string requestApi = string.Format("Api/MasterData/GetRequestItems");
            List<RequestItemModel> dataRequest = MeditechApiHelper.Get<List<RequestItemModel>>(requestApi);

            return dataRequest;
        }
        public List<RequestItemModel> GetRequestItemByCategory(string category, bool queryResultLink = false)
        {
            string requestApi = string.Format("Api/MasterData/GetRequestItemByCategory?category={0}&queryResultLink={1}", category, queryResultLink);
            List<RequestItemModel> dataRequest = MeditechApiHelper.Get<List<RequestItemModel>>(requestApi);

            return dataRequest;
        }

        public RequestItemModel GetRequestItemByUID(int requestItemUID)
        {
            string requestApi = string.Format("Api/MasterData/GetRequestItemByUID?requestItemUID={0}", requestItemUID);
            RequestItemModel dataRequest = MeditechApiHelper.Get<RequestItemModel>(requestApi);

            return dataRequest;
        }

        public RequestItemModel GetRequestItemByCode(string code)
        {
            string requestApi = string.Format("Api/MasterData/GetRequestItemByCode?code={0}", code);
            RequestItemModel dataRequest = MeditechApiHelper.Get<RequestItemModel>(requestApi);

            return dataRequest;
        }

        public List<RequestResultLinkModel> GetRequestResultLinkByRequestItemUID(int requestItemUID)
        {
            string requestApi = string.Format("Api/MasterData/GetRequestResultLinkByRequestItemUID?requestItemUID={0}", requestItemUID);
            List<RequestResultLinkModel> dataRequest = MeditechApiHelper.Get<List<RequestResultLinkModel>>(requestApi);

            return dataRequest;
        }

        public bool ManageRequestItem(RequestItemModel requestItemBIll, int userID)
        {
            bool flag = false;

            try
            {
                string requestApi = string.Format("Api/MasterData/ManageRequestItem?userID={0}", userID);
                MeditechApiHelper.Post<RequestItemModel>(requestApi, requestItemBIll);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }


            return flag;

        }

        public bool DeleteRequestItem(int requestItemUID, int userID)
        {
            bool flag = false;

            try
            {
                string requestApi = string.Format("Api/MasterData/DeleteRequestItem?requestItemUID={0}&userID={1}", requestItemUID, userID);
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

        #region ResultItem

        public List<ResultItemModel> GetResultItems()
        {
            string requestApi = string.Format("Api/MasterData/GetResultItems");
            List<ResultItemModel> dataRequest = MeditechApiHelper.Get<List<ResultItemModel>>(requestApi);

            return dataRequest;
        }
        public List<ResultItemModel> SearchResultItem(string name, int? RVTYPUID)
        {
            string requestApi = string.Format("Api/MasterData/SearchResultItem?name={0}&RVTYPUID={1}", name, RVTYPUID);
            List<ResultItemModel> dataRequest = MeditechApiHelper.Get<List<ResultItemModel>>(requestApi);

            return dataRequest;
        }

        public ResultItemModel GetResultItemUID(int resultItemUID)
        {
            string requestApi = string.Format("Api/MasterData/GetResultItemUID?resultItemUID={0}", resultItemUID);
            ResultItemModel dataRequest = MeditechApiHelper.Get<ResultItemModel>(requestApi);

            return dataRequest;
        }

        public ResultItemModel GetResultItemByCode(string code)
        {
            string requestApi = string.Format("Api/MasterData/GetResultItemByCode?code={0}", code);
            ResultItemModel dataRequest = MeditechApiHelper.Get<ResultItemModel>(requestApi);

            return dataRequest;
        }


        public string GenerateResultItemCode()
        {
            string requestApi = string.Format("Api/MasterData/GenerateResultItemCode");
            string itemCode = MeditechApiHelper.Get<string>(requestApi);

            return itemCode;
        }


        public void ManageResultItem(ResultItemModel resultItemModel, int userID)
        {
            try
            {
                string requestApi = string.Format("Api/MasterData/ManageResultItem?userID={0}", userID);
                MeditechApiHelper.Post(requestApi, resultItemModel);
            }
            catch (Exception ex)
            {

                throw (ex);
            }
        }
        public void DeleteResultItem(int resultItemUID, int userID)
        {
            try
            {
                string requestApi = string.Format("Api/MasterData/DeleteResultItem?resultItemUID={0}&userID={1}", resultItemUID, userID);
                MeditechApiHelper.Delete(requestApi);
            }
            catch (Exception ex)
            {

                throw (ex);
            }
        }

        #endregion

        #region Specimen

        public List<SpecimenModel> SearhcSpecimen(string specimentext)
        {
            string requestApi = string.Format("Api/MasterData/SearhcSpecimen?specimentext={0}", specimentext);
            List<SpecimenModel> dataRequest = MeditechApiHelper.Get<List<SpecimenModel>>(requestApi);

            return dataRequest;
        }

        public SpecimenModel GetSpecimenByUID(int specimenUID)
        {
            string requestApi = string.Format("Api/MasterData/GetSpecimenByUID?specimenUID={0}", specimenUID);
            SpecimenModel dataRequest = MeditechApiHelper.Get<SpecimenModel>(requestApi);

            return dataRequest;
        }

        public SpecimenModel GetSpecimenByCode(string code)
        {
            string requestApi = string.Format("Api/MasterData/GetSpecimenByCode?code={0}", code);
            SpecimenModel dataRequest = MeditechApiHelper.Get<SpecimenModel>(requestApi);

            return dataRequest;
        }


        public void ManageSpecimen(SpecimenModel specimenModel, int userID)
        {
            try
            {
                string requestApi = string.Format("Api/MasterData/ManageSpecimen?userID={0}", userID);
                MeditechApiHelper.Post<SpecimenModel>(requestApi, specimenModel);
            }
            catch (Exception ex)
            {

                throw (ex);
            }
        }

        public void DeleteSpecimen(int specimenUID, int userID)
        {
            try
            {
                string requestApi = string.Format("Api/MasterData/DeleteSpecimen?specimenUID={0}&userID={1}", specimenUID, userID);
                MeditechApiHelper.Delete(requestApi);
            }
            catch (Exception ex)
            {

                throw (ex);
            }
        }

        #endregion

        #region BillableItem

        public List<BillableItemModel> SearchBillableItem(string code, string itemName, int? BSMDDUID, int? billingGroupUID, int? billingSubGroupUID, int? ownerUID)
        {
            string requestApi = string.Format("Api/MasterData/SearchBillableItem?code={0}&itemName={1}&BSMDDUID={2}&billingGroupUID={3}&billingSubGroupUID={4}&ownerUID={5}", code, itemName, BSMDDUID, billingGroupUID, billingSubGroupUID, ownerUID);
            List<BillableItemModel> dataRequest = MeditechApiHelper.Get<List<BillableItemModel>>(requestApi);

            return dataRequest;
        }

        public List<BillableItemModel> GetBillableItemAll()
        {
            string requestApi = string.Format("Api/MasterData/GetBillableItemAll");
            List<BillableItemModel> dataRequest = MeditechApiHelper.Get<List<BillableItemModel>>(requestApi);

            return dataRequest;
        }
        public List<BillableItemModel> GetBillableItemByCategory(string categroy)
        {
            string requestApi = string.Format("Api/MasterData/GetBillableItemByCategory?category={0}", categroy);
            List<BillableItemModel> dataRequest = MeditechApiHelper.Get<List<BillableItemModel>>(requestApi);

            return dataRequest;
        }

        public BillableItemModel GetBillableItemByUID(int billableItemUID)
        {
            string requestApi = string.Format("Api/MasterData/GetBillableItemByUID?billableItemUID={0}", billableItemUID);
            BillableItemModel dataRequest = MeditechApiHelper.Get<BillableItemModel>(requestApi);

            return dataRequest;
        }
        public List<BillableItemModel> GetBillableItemByCode(string code)
        {
            string requestApi = string.Format("Api/MasterData/GetBillableItemByCode?code={0}", code);
            List<BillableItemModel> dataRequest = MeditechApiHelper.Get<List<BillableItemModel>>(requestApi);

            return dataRequest;
        }
        

        public List<BillableItemModel> GetBillableItemByBSMDD(string billingService)
        {
            string requestApi = string.Format("Api/MasterData/GetBillableItemByBSMDD?BillingService={0}", billingService);
            List<BillableItemModel> data = MeditechApiHelper.Get<List<BillableItemModel>>(requestApi);
            return data;
        }

        public bool ManageBillableItem(BillableItemModel billableItemModel, int userID)
        {
            bool flag = false;

            try
            {
                string requestApi = string.Format("Api/MasterData/ManageBillableItem?userID={0}", userID);
                MeditechApiHelper.Post<BillableItemModel>(requestApi, billableItemModel);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }


            return flag;

        }

        public bool DeleteBillableItem(int billableItemUID, int userID)
        {
            bool flag = false;

            try
            {
                string requestApi = string.Format("Api/MasterData/DeleteBillableItem?billableItemUID={0}&userID={1}", billableItemUID, userID);
                MeditechApiHelper.Delete(requestApi);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }


            return flag;

        }

        public List<BillableItemDetailModel> GetBillableItemDetailByBillableItemUID(int billableItemUID)
        {
            string requestApi = string.Format("Api/MasterData/GetBillableItemDetailByBillableItemUID?billableItemUID={0}", billableItemUID);
            List<BillableItemDetailModel> data = MeditechApiHelper.Get<List<BillableItemDetailModel>>(requestApi);
            return data;
        }


        #endregion

        #region OrderSet

        public List<OrderSetModel> SearchOrderSet(string code, string name)
        {
            string requestApi = string.Format("Api/MasterData/SearchOrderSet?code={0}&name={1}", code, name);
            List<OrderSetModel> dataRequest = MeditechApiHelper.Get<List<OrderSetModel>>(requestApi);

            return dataRequest;
        }

        public OrderSetModel GetOrderSetByUID(int orderSetUID)
        {
            string requestApi = string.Format("Api/MasterData/GetOrderSetByUID?orderSetUID={0}", orderSetUID);
            OrderSetModel dataRequest = MeditechApiHelper.Get<OrderSetModel>(requestApi);

            return dataRequest;
        }

        public bool ManageOrderSet(OrderSetModel orderSetModel, int userID)
        {
            bool flag = false;

            try
            {
                string requestApi = string.Format("Api/MasterData/ManageOrderSet?userID={0}", userID);
                MeditechApiHelper.Post<OrderSetModel>(requestApi, orderSetModel);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }


            return flag;
        }

        public bool DeleteOrderSet(int orderSetUID, int userID)
        {
            bool flag = false;

            try
            {
                string requestApi = string.Format("Api/MasterData/DeleteOrderSet?orderSetUID={0}&userID={1}", orderSetUID, userID);
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

        #region BillgingGroup

        public List<BillingGroupModel> GetBillingGroup()
        {
            string requestApi = string.Format("Api/MasterData/GetBillingGroup");
            List<BillingGroupModel> dataRequest = MeditechApiHelper.Get<List<BillingGroupModel>>(requestApi);

            return dataRequest;
        }

        #endregion

        #region BillgingSubGroup
        public List<BillingSubGroupModel> GetBillingSubGroup()
        {
            string requestApi = string.Format("Api/MasterData/GetBillingSubGroup");
            List<BillingSubGroupModel> dataRequest = MeditechApiHelper.Get<List<BillingSubGroupModel>>(requestApi);

            return dataRequest;
        }

        public List<BillingSubGroupModel> GetBillingSubGroupByGroup(int billingGroupUID)
        {
            string requestApi = string.Format("Api/MasterData/GetBillingSubGroupByGroup?billingGroupUID={0}", billingGroupUID);
            List<BillingSubGroupModel> dataRequest = MeditechApiHelper.Get<List<BillingSubGroupModel>>(requestApi);

            return dataRequest;
        }


        #endregion

        #region HealthOrganisation
        public List<HealthOrganisationModel> GetHealthOrganisation()
        {
            string requestApi = string.Format("Api/MasterData/GetHealthOrganisation");
            List<HealthOrganisationModel> dataRequest = MeditechApiHelper.Get<List<HealthOrganisationModel>>(requestApi);

            return dataRequest;
        }

        public List<HealthOrganisationModel> GetHealthOrganisationActive()
        {
            string requestApi = string.Format("Api/MasterData/GetHealthOrganisationActive");
            List<HealthOrganisationModel> dataRequest = MeditechApiHelper.Get<List<HealthOrganisationModel>>(requestApi);

            return dataRequest;
        }

        public HealthOrganisationModel GetHealthOrganisationByUID(int healthOrganisationUID)
        {
            string requestApi = string.Format("Api/MasterData/GetHealthOrganisationByUID?healthOrganisationUID={0}", healthOrganisationUID);
            HealthOrganisationModel dataRequest = MeditechApiHelper.Get<HealthOrganisationModel>(requestApi);

            return dataRequest;
        }

        public bool ManageHealthOrganisation(HealthOrganisationModel healthOrganisationModel, int userID)
        {
            bool flag = false;

            try
            {
                string requestApi = string.Format("Api/MasterData/ManageHealthOrganisation?userID={0}", userID);
                MeditechApiHelper.Post<HealthOrganisationModel>(requestApi, healthOrganisationModel);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }


            return flag;

        }

        public bool DeleteHealthOrganisation(int healthOrganisationUID, int userID)
        {
            bool flag = false;

            try
            {
                string requestApi = string.Format("Api/MasterData/DeleteHealthOrganisation?healthOrganisationUID={0}&userID={1}", healthOrganisationUID, userID);
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

        #region PayorDetail

        public List<PayorDetailModel> SearchPayorDetail(string code, string name)
        {
            string requestApi = string.Format("Api/MasterData/SearchPayorDetail?code={0}&name={1}", code, name);
            List<PayorDetailModel> dataRequest = MeditechApiHelper.Get<List<PayorDetailModel>>(requestApi);

            return dataRequest;
        }

        public List<PayorDetailModel> GetPayorDetail()
        {
            string requestApi = string.Format("Api/MasterData/GetPayorDetail");
            List<PayorDetailModel> dataRequest = MeditechApiHelper.Get<List<PayorDetailModel>>(requestApi);

            return dataRequest;
        }

        public PayorDetailModel GetPayorDetailByUID(int payorDetailUID)
        {
            string requestApi = string.Format("Api/MasterData/GetPayorDetailByUID?payorDetailUID={0}", payorDetailUID);
            PayorDetailModel dataRequest = MeditechApiHelper.Get<PayorDetailModel>(requestApi);

            return dataRequest;
        }

        public PayorDetailModel GetPayorDetailByCode(string payorCode)
        {
            string requestApi = string.Format("Api/MasterData/GetPayorDetailByCode?payorCode={0}",payorCode);
            PayorDetailModel dataRequest = MeditechApiHelper.Get<PayorDetailModel>(requestApi);
            return dataRequest;
        }



        public bool ManagePayorDetail(PayorDetailModel payorDetailModel, int userID)
        {
            bool flag = false;

            try
            {
                string requestApi = string.Format("Api/MasterData/ManagePayorDetail?userID={0}", userID);
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
                string requestApi = string.Format("Api/MasterData/DeletePayorDetail?payorDetailUID={0}&userID={1}", payorDetailUID, userID);
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
            string requestApi = string.Format("Api/MasterData/GetAgreementByPayorDetailUID?payorDetailUID={0}", payorDetailUID);
            List<PayorAgreementModel> dataRequest = MeditechApiHelper.Get<List<PayorAgreementModel>>(requestApi);

            return dataRequest;
        }
        #endregion

        public List<LookupReferenceValueModel> GetLocationAll()
        {
            List<LookupReferenceValueModel> data = MeditechApiHelper.Get<List<LookupReferenceValueModel>>("Api/MasterData/GetLocationAll");
            return data;
        }

        public List<SpecialityModel> GetSpecialityAll()
        {
            List<SpecialityModel> data = MeditechApiHelper.Get<List<SpecialityModel>>("Api/MasterData/GetSpecialityAll");
            return data;
        }

        #region InsuranceCompany
        //public List<PayorAgreementModel> SearchPayorAgreementByINCO(int? insuranceCompanyUID)
        //{
        //    string requestApi = string.Format("Api/MasterData/SearchPayorAgreementByINCO?insuranceCompanyUID={0}", insuranceCompanyUID);
        //    List<PayorAgreementModel> dataRequest = MeditechApiHelper.Get<List<PayorAgreementModel>>(requestApi);

        //    return dataRequest;
        //}

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


        public InsuranceCompanyModel GetInsuranceCompanyByUID(int insuranceCompanyUID)
        {
            string requestApi = string.Format("Api/MasterData/GetInsuranceCompanyByUID?insuranceCompanyUID={0}", insuranceCompanyUID);
            InsuranceCompanyModel dataRequest = MeditechApiHelper.Get<InsuranceCompanyModel>(requestApi);

            return dataRequest;
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

    }
}
