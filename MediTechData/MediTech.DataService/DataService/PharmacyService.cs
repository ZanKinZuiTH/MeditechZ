using MediTech.Model;
using MediTech.Model.Report;
using ShareLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.DataService
{
    public class PharmacyService
    {
        #region DrugGenericMaster

        public List<DrugGenericModel> GetDrugGeneric()
        {
            string requestApi = string.Format("Api/Pharmacy/GetDrugGeneric");
            List<DrugGenericModel> dataRequest = MeditechApiHelper.Get<List<DrugGenericModel>>(requestApi);

            return dataRequest;
        }

        public List<DrugGenericModel> GetDrugGenericCriteria(string text)
        {
            string requestApi = string.Format("Api/Pharmacy/GetDrugGenericCriteria?text={0}", text);
            List<DrugGenericModel> dataRequest = MeditechApiHelper.Get<List<DrugGenericModel>>(requestApi);

            return dataRequest;
        }

        public DrugGenericModel GetDrugGenericByUID(int drugGenaricUID)
        {
            string requestApi = string.Format("Api/Pharmacy/GetDrugGenericByUID?drugGenaricUID={0}", drugGenaricUID);
            DrugGenericModel dataRequest = MeditechApiHelper.Get<DrugGenericModel>(requestApi);

            return dataRequest;
        }

        public bool ManageDrugGeneric(DrugGenericModel drugGenericModel, int userID)
        {
            bool flag = false;

            try
            {
                string requestApi = string.Format("Api/Pharmacy/ManageDrugGeneric?userID={0}", userID);
                MeditechApiHelper.Post<DrugGenericModel>(requestApi, drugGenericModel);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }


            return flag;
        }

        public bool DeleteDrugGeneric(int drugGenericUID, int userID)
        {
            bool flag = false;

            try
            {
                string requestApi = string.Format("Api/Pharmacy/DeleteDrugGeneric?drugGenericUID={0}&userID={1}", drugGenericUID, userID);
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

        #region DrugFrequency
        public List<FrequencyDefinitionModel> GetDrugFrequency()
        {
            string requestApi = string.Format("Api/Pharmacy/GetDrugFrequency");
            List<FrequencyDefinitionModel> dataRequest = MeditechApiHelper.Get<List<FrequencyDefinitionModel>>(requestApi);

            return dataRequest;
        }

        public FrequencyDefinitionModel GetDrugFrequencyByUID(int drugFrequencyUID)
        {
            string requestApi = string.Format("Api/Pharmacy/DrugFrequencyModel?drugFrequencyUID={0}", drugFrequencyUID);
            FrequencyDefinitionModel dataRequest = MeditechApiHelper.Get<FrequencyDefinitionModel>(requestApi);

            return dataRequest;
        }

        public bool ManageDrugFrequency(FrequencyDefinitionModel drugFrequencyModel, int userID)
        {
            bool flag = false;

            try
            {
                string requestApi = string.Format("Api/Pharmacy/ManageDrugFrequency?userID={0}", userID);
                MeditechApiHelper.Post<FrequencyDefinitionModel>(requestApi, drugFrequencyModel);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }


            return flag;
        }

        public bool DeleteDrugFrequency(int drugFrequencyUID, int userID)
        {
            bool flag = false;

            try
            {
                string requestApi = string.Format("Api/Pharmacy/DeleteDrugFrequency?drugFrequencyUID={0}&userID={1}", drugFrequencyUID, userID);
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

        public List<ItemMasterModel> GetDrugCriteria(string text)
        {
            string requestApi = string.Format("Api/Pharmacy/GetDrugCriteria?text={0}", text);
            List<ItemMasterModel> dataRequest = MeditechApiHelper.Get<List<ItemMasterModel>>(requestApi);

            return dataRequest;
        }

        public List<PatientOrderDetailModel> GetDrugStoreDispense(long prescriptionItemUID)
        {
            string requestApi = string.Format("Api/Pharmacy/GetDrugStoreDispense?prescriptionItemUID={0}", prescriptionItemUID);
            List<PatientOrderDetailModel> dataRequest = MeditechApiHelper.Get<List<PatientOrderDetailModel>>(requestApi);

            return dataRequest;
        }

        public List<PatientOrderDetailModel> GetDrugStoreDispense(int itemMasterUID, double useQty, int IMUOMUID, int StoreUID)
        {
            string requestApi = string.Format("Api/Pharmacy/GetDrugStoreDispense?itemMasterUID={0}&useQty={1}&IMUOMUID={2}&StoreUID={3}", itemMasterUID, useQty, IMUOMUID, StoreUID);
            List<PatientOrderDetailModel> dataRequest = MeditechApiHelper.Get<List<PatientOrderDetailModel>>(requestApi);

            return dataRequest;
        }

        public List<DrugStickerModel> PrintStrickerDrug(long prescriptionItemUID)
        {
            string requestApi = string.Format("Api/Pharmacy/PrintStrickerDrug?prescriptionItemUID={0}", prescriptionItemUID);
            List<DrugStickerModel> dataRequest = MeditechApiHelper.Get<List<DrugStickerModel>>(requestApi);

            return dataRequest;
        }


        #region Prescription

        public List<PrescriptionModel> Searchprescription(DateTime? dateFrom, DateTime? dateTo, string statusList, long? patientUID
    , string prescriptionNumber, int? organisationUID)
        {
            string requestApi = string.Format("Api/Pharmacy/Searchprescription?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&statusList={2}&patientUID={3}&prescriptionNumber={4}&organisationUID={5}", dateFrom, dateTo, statusList, patientUID, prescriptionNumber, organisationUID);
            List<PrescriptionModel> returnData = MeditechApiHelper.Get<List<PrescriptionModel>>(requestApi);

            return returnData;
        }


        public List<PrescriptionItemModel> GetPrescriptionItemByPrescriptionUID(long? prescriptionUID)
        {
            List<PrescriptionItemModel> result;
            try
            {
                string requestApi = string.Format("Api/Pharmacy/GetPrescriptionItemByPrescriptionUID?prescriptionUID={0}", prescriptionUID);
                result = MeditechApiHelper.Get<List<PrescriptionItemModel>>(requestApi);
            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }



        public List<PrescriptionModel> GetprescriptionList(int prescriptionUID)
        {
            string requestApi = string.Format("Api/Pharmacy/GetprescriptionList?prescriptionUID={0}", prescriptionUID);
            List<PrescriptionModel> returnData = MeditechApiHelper.Get<List<PrescriptionModel>>(requestApi);

            return returnData;
        }

        public PrescriptionModel Getprescription(int prescriptionUID)
        {
            string requestApi = string.Format("Api/Pharmacy/Getprescription?prescriptionUID={0}", prescriptionUID);
            PrescriptionModel returnData = MeditechApiHelper.Get<PrescriptionModel>(requestApi);

            return returnData;
        }

        public bool UpdatePrescriptionLabelSticker(long prescriptionItemUID, String localInstructionText, int userUID)
        {
            bool flag = false;

            try
            {
                string requestApi = string.Format("Api/Pharmacy/UpdatePrescriptionLabelSticker?prescriptionItemUID={0}&localInstructionText={1}&userID={2}", prescriptionItemUID, localInstructionText, userUID);
                MeditechApiHelper.Put(requestApi);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }


            return flag;
        }

        public bool CancelDispensed(PrescriptionItemModel prescriptionItemModel, int userUID)
        {
            bool flag = false;

            try
            {
                string requestApi = string.Format("Api/Pharmacy/CancelDispensed?userUID={0}", userUID);
                MeditechApiHelper.Post(requestApi, prescriptionItemModel);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }


            return flag;
        }

        public bool DispensePrescription(PrescriptionModel prescription, int userUID)
        {
            bool flag = false;

            try
            {
                string requestApi = string.Format("Api/Pharmacy/DispensePrescription?userUID={0}", userUID);
                MeditechApiHelper.Post(requestApi, prescription);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }


            return flag;
        }


        public bool DispensePrescriptionItem(PrescriptionItemModel prescriptionItemModel, int userUID)
        {
            bool flag = false;

            try
            {
                string requestApi = string.Format("Api/Pharmacy/DispensePrescriptionItem?userUID={0}", userUID);
                MeditechApiHelper.Post(requestApi, prescriptionItemModel);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }


            return flag;
        }
        #endregion

        public List<PatientOrderStandingModel> GetPatientOrderStanding(int? wardUID, int storeUID)
        {
            string requestApi = string.Format("Api/Pharmacy/GetPatientOrderStanding?wardUID={0}&storeUID={1}", wardUID, storeUID);
            List<PatientOrderStandingModel> dataRequest = MeditechApiHelper.Get<List<PatientOrderStandingModel>>(requestApi);

            return dataRequest;
        }
    }
}
