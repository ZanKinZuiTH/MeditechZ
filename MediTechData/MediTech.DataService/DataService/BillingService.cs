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

    }
}
