using MediTech.Model;
using MediTech.Model.Report;
using ShareLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Migrations;
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
            string requestApi = string.Format("Api/Pharmacy/GetDrugCriteria?text={0}",text);
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
            string requestApi = string.Format("Api/Pharmacy/GetDrugStoreDispense?itemMasterUID={0}&useQty={1}&IMUOMUID={2}&StoreUID={3}", itemMasterUID,useQty,IMUOMUID,StoreUID);
            List<PatientOrderDetailModel> dataRequest = MeditechApiHelper.Get<List<PatientOrderDetailModel>>(requestApi);

            return dataRequest;
        }

        public List<DrugStickerModel> PrintStrickerDrug(long prescriptionItemUID)
        {
            string requestApi = string.Format("Api/Pharmacy/PrintStrickerDrug?prescriptionItemUID={0}", prescriptionItemUID);
            List<DrugStickerModel> dataRequest = MeditechApiHelper.Get<List<DrugStickerModel>>(requestApi);

            return dataRequest;
        }
    }
}
