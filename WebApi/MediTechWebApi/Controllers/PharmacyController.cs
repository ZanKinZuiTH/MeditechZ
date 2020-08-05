using MediTech.DataBase;
using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Data;
using System.Data.Entity.Migrations;
using System.Transactions;
using System.Web.Http;
using ShareLibrary;
using MediTech.Model.Report;

namespace MediTechWebApi.Controllers
{
    [RoutePrefix("Api/Pharmacy")]
    public class PharmacyController : ApiController
    {
        protected MediTechEntities db = new MediTechEntities();


        #region DrugGenericMaster

        [Route("GetDrugGeneric")]
        [HttpGet]
        public List<DrugGenericModel> GetDrugGeneric()
        {
            List<DrugGenericModel> data = db.DrugGeneric.Where(p => p.StatusFlag == "A").Select(p => new DrugGenericModel()
            {
                DrugGenericUID = p.UID,
                Name = p.Name,
                Description = p.Description,
                ActiveFrom = p.ActiveFrom,
                ActiveTo = p.ActiveTo,
                CUser = p.CUser,
                CWhen = p.CWhen,
                MUser = p.MUser,
                MWhen = p.MWhen,
                StatusFlag = p.StatusFlag
            }).ToList();

            return data;
        }

        [Route("GetDrugGenericCriteria")]
        [HttpGet]
        public List<DrugGenericModel> GetDrugGenericCriteria(string text)
        {
            List<DrugGenericModel> data = db.DrugGeneric.Where(p => p.StatusFlag == "A"
                && (p.Name.Contains(text))
                ).Select(p => new DrugGenericModel()
                {
                    DrugGenericUID = p.UID,
                    Name = p.Name,
                    Description = p.Description,
                    ActiveFrom = p.ActiveFrom,
                    ActiveTo = p.ActiveTo,
                    CUser = p.CUser,
                    CWhen = p.CWhen,
                    MUser = p.MUser,
                    MWhen = p.MWhen,
                    StatusFlag = p.StatusFlag
                }).ToList();

            return data;
        }

        [Route("GetDrugGenericByUID")]
        [HttpGet]
        public DrugGenericModel GetDrugGenericByUID(int drugGenaricUID)
        {
            DrugGenericModel data = db.DrugGeneric.Where(p => p.StatusFlag == "A" && p.UID == drugGenaricUID)
                .Select(p => new DrugGenericModel()
                {
                    DrugGenericUID = p.UID,
                    Name = p.Name,
                    Description = p.Description,
                    ActiveFrom = p.ActiveFrom,
                    ActiveTo = p.ActiveTo,
                    CUser = p.CUser,
                    CWhen = p.CWhen,
                    MUser = p.MUser,
                    MWhen = p.MWhen,
                    StatusFlag = p.StatusFlag
                }).FirstOrDefault();

            return data;
        }

        [Route("ManageDrugGeneric")]
        [HttpPost]
        public HttpResponseMessage ManageDrugGeneric(DrugGenericModel drugGenericModel, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;


                DrugGeneric drugGenericMaster = db.DrugGeneric.Find(drugGenericModel.DrugGenericUID);

                if (drugGenericMaster == null)
                {
                    drugGenericMaster = new DrugGeneric();
                    drugGenericMaster.CUser = userID;
                    drugGenericMaster.CWhen = now;
                }
                drugGenericMaster.Name = drugGenericModel.Name;
                drugGenericMaster.Description = drugGenericModel.Description;
                drugGenericMaster.ActiveFrom = drugGenericModel.ActiveFrom;
                drugGenericMaster.ActiveTo = drugGenericModel.ActiveTo;
                drugGenericMaster.MUser = userID;
                drugGenericMaster.MWhen = now;
                drugGenericMaster.StatusFlag = "A";

                db.DrugGeneric.AddOrUpdate(drugGenericMaster);
                db.SaveChanges();


                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("DeleteDrugGeneric")]
        [HttpDelete]
        public HttpResponseMessage DeleteDrugGeneric(int drugGenericUID, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                DrugGeneric drugGeneric = db.DrugGeneric.Find(drugGenericUID);
                if (drugGeneric != null)
                {
                    db.DrugGeneric.Attach(drugGeneric);
                    drugGeneric.MUser = userID;
                    drugGeneric.MWhen = now;
                    drugGeneric.StatusFlag = "D";
                    db.SaveChanges();
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        #endregion

        #region DrugFrequency

        [Route("GetDrugFrequency")]
        [HttpGet]
        public List<FrequencyDefinitionModel> GetDrugFrequency()
        {

            List<FrequencyDefinitionModel> data = db.FrequencyDefinition.Where(p => p.StatusFlag == "A").Select(p => new FrequencyDefinitionModel()
            {
                FrequencyUID = p.UID,
                Code = p.Code,
                Name = p.Name,
                Comments = p.Comments,
                AmountPerTimes = p.AmountPerTimes,
                ActiveFrom = p.ActiveFrom,
                ActiveTo = p.ActiveTo,
                CUser = p.CUser,
                CWhen = p.CWhen,
                MUser = p.MUser,
                MWhen = p.MWhen,
                StatusFlag = p.StatusFlag
            }).ToList();

            return data;
        }

        [Route("GetDrugFrequencyByUID")]
        [HttpGet]
        public FrequencyDefinitionModel GetDrugFrequencyByUID(int drugFrequencyUID)
        {
            FrequencyDefinitionModel data = db.FrequencyDefinition.Where(p => p.StatusFlag == "A" && p.UID == drugFrequencyUID)
                .Select(p => new FrequencyDefinitionModel()
                {
                    FrequencyUID = p.UID,
                    Code = p.Code,
                    Name = p.Name,
                    Comments = p.Comments,
                    AmountPerTimes = p.AmountPerTimes,
                    ActiveFrom = p.ActiveFrom,
                    ActiveTo = p.ActiveTo,
                    CUser = p.CUser,
                    CWhen = p.CWhen,
                    MUser = p.MUser,
                    MWhen = p.MWhen,
                    StatusFlag = p.StatusFlag
                }).FirstOrDefault();

            return data;
        }

        [Route("ManageDrugFrequency")]
        [HttpPost]
        public HttpResponseMessage ManageDrugFrequency(FrequencyDefinitionModel drugFrequencyModel, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;


                FrequencyDefinition drugFrequency = db.FrequencyDefinition.Find(drugFrequencyModel.FrequencyUID);

                if (drugFrequency == null)
                {
                    drugFrequency = new FrequencyDefinition();
                    drugFrequency.CUser = userID;
                    drugFrequency.CWhen = now;
                }

                drugFrequency.Code = drugFrequencyModel.Code;
                drugFrequency.Name = drugFrequencyModel.Name;
                drugFrequency.Comments = drugFrequencyModel.Comments;
                drugFrequency.AmountPerTimes = drugFrequencyModel.AmountPerTimes;
                drugFrequency.ActiveFrom = drugFrequencyModel.ActiveFrom;
                drugFrequency.ActiveTo = drugFrequencyModel.ActiveTo;
                drugFrequency.MUser = userID;
                drugFrequency.MWhen = now;
                drugFrequency.StatusFlag = "A";

                db.FrequencyDefinition.AddOrUpdate(drugFrequency);
                db.SaveChanges();


                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("DeleteDrugFrequency")]
        [HttpDelete]
        public HttpResponseMessage DeleteDrugFrequency(int drugFrequencyUID, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                FrequencyDefinition drugFrequency = db.FrequencyDefinition.Find(drugFrequencyUID);
                if (drugFrequency != null)
                {
                    db.FrequencyDefinition.Attach(drugFrequency);
                    drugFrequency.MUser = userID;
                    drugFrequency.MWhen = now;
                    drugFrequency.StatusFlag = "D";
                    db.SaveChanges();
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        #endregion


        [Route("GetDrugCriteria")]
        [HttpGet]
        public List<ItemMasterModel> GetDrugCriteria(string text)
        {
            List<ItemMasterModel> data = db.ItemMaster.Where(p => p.StatusFlag == "A"
                && (p.Name.Contains(text) || p.Code.Contains(text))
                && p.ITMTYPUID == 632
                ).Select(p => new ItemMasterModel()
                {
                    ItemMasterUID = p.UID,
                    Code = p.Code,
                    Name = p.Name,
                    Description = p.Description,
                    ActiveFrom = p.ActiveFrom,
                    ActiveTo = p.ActiveTo,
                    CUser = p.CUser,
                    CWhen = p.CWhen,
                    MUser = p.MUser,
                    MWhen = p.MWhen,
                    StatusFlag = p.StatusFlag
                }).ToList();

            return data;
        }

        [Route("GetDrugStoreDispense")]
        [HttpGet]
        public List<PatientOrderDetailModel> GetDrugStoreDispense(long prescriptionItemUID)
        {
            List<PatientOrderDetailModel> listDrugDispense = new List<PatientOrderDetailModel>();
            PrescriptionItem presItem = db.PrescriptionItem.Find(prescriptionItemUID);
            Store store = db.Store.Find(presItem.StoreUID);
            int? baseUOM;
            ItemMaster itemMaster = db.ItemMaster.Find(presItem.ItemMasterUID);
            baseUOM = itemMaster.BaseUOM;
            IEnumerable<Stock> stockItem = db.Stock
                .Where(p => p.StatusFlag == "A"
                && p.ItemMasterUID == presItem.ItemMasterUID
                && p.StoreUID == presItem.StoreUID && p.Quantity > 0);
            if (stockItem != null && stockItem.Count() > 0)
            {
                if (store.STDTPUID == 2901)
                    stockItem = stockItem.OrderBy(p => p.ExpiryDttm);
                else if (store.STDTPUID == 2902)
                    stockItem = stockItem.OrderBy(p => p.CUser);

                if (presItem.IMUOMUID != itemMaster.BaseUOM)
                {
                    double? convertValue;
                    var storeConvert = db.StoreUOMConversion.FirstOrDefault(p => p.BaseUOMUID == itemMaster.BaseUOM
                    && p.ConversionUOMUID == presItem.IMUOMUID
                    && p.StatusFlag == "A");
                    if (storeConvert != null)
                    {
                        convertValue = storeConvert.ConversionValue;
                        if (convertValue.HasValue && convertValue > 0)
                        {
                            presItem.Quantity = (presItem.Quantity.Value / convertValue.Value);
                        }
                    }
                }

                int i = 0;
                foreach (var item in stockItem)
                {
                    i++;
                    PatientOrderDetailModel drugDetail = new PatientOrderDetailModel();
                    drugDetail.ItemUID = presItem.ItemMasterUID;
                    drugDetail.ItemCode = presItem.ItemCode;
                    drugDetail.ItemName = presItem.ItemName;
                    drugDetail.PDSTSUID = presItem.PDSTSUID;
                    if (presItem.PDSTSUID != null && presItem.PDSTSUID != 0)
                        drugDetail.InstructionRoute = db.ReferenceValue.Find(presItem.PDSTSUID).Description;

                    drugDetail.Dosage = presItem.Dosage;
                    drugDetail.FRQNCUID = presItem.FRQNCUID;
                    if (presItem.FRQNCUID != null && presItem.FRQNCUID != 0)
                        drugDetail.DrugFrequency = db.FrequencyDefinition.Find(presItem.FRQNCUID).Comments;

                    drugDetail.IdentifyingUID = presItem.UID;
                    drugDetail.InstructionText = presItem.InstructionText;
                    drugDetail.LocalInstructionText = presItem.LocalInstructionText;
                    drugDetail.ClinicalComments = presItem.ClinicalComments;
                    drugDetail.StockUID = item.UID;
                    drugDetail.BalQty = item.Quantity;
                    drugDetail.ExpiryDate = item.ExpiryDttm;
                    drugDetail.StoreName = store.Name;
                    drugDetail.StoreUID = store.UID;
                    drugDetail.BatchID = item.BatchID;
                    drugDetail.DFORMUID = presItem.DFORMUID;
                    if (presItem.DFORMUID != null && presItem.DFORMUID != 0)
                        drugDetail.TypeDrug = db.ReferenceValue.Find(presItem.DFORMUID).Description;

                    drugDetail.QNUOMUID = presItem.IMUOMUID;
                    if (presItem.IMUOMUID != null && presItem.IMUOMUID != 0)
                        drugDetail.QuantityUnit = db.ReferenceValue.Find(presItem.IMUOMUID).Description;




                    if (item.Quantity >= presItem.Quantity)
                    {
                        drugDetail.Quantity = presItem.Quantity;
                        listDrugDispense.Add(drugDetail);
                        break;
                    }
                    else
                    {
                        if (i == stockItem.Count())
                        {
                            drugDetail.Quantity = presItem.Quantity;
                        }
                        else
                        {
                            drugDetail.Quantity = item.Quantity;
                            presItem.Quantity = presItem.Quantity - drugDetail.Quantity;
                        }

                    }

                    listDrugDispense.Add(drugDetail);
                }
            }
            else
            {
                PatientOrderDetailModel drugDetail = new PatientOrderDetailModel();
                drugDetail.ItemUID = presItem.ItemMasterUID;
                drugDetail.ItemCode = presItem.ItemCode;
                drugDetail.ItemName = presItem.ItemName;
                drugDetail.PDSTSUID = presItem.PDSTSUID;
                if (presItem.PDSTSUID != null && presItem.PDSTSUID != 0)
                    drugDetail.InstructionRoute = db.ReferenceValue.Find(presItem.PDSTSUID).Description;

                drugDetail.Dosage = presItem.Dosage;
                drugDetail.FRQNCUID = presItem.FRQNCUID;
                if (presItem.FRQNCUID != null && presItem.FRQNCUID != 0)
                    drugDetail.DrugFrequency = db.FrequencyDefinition.Find(presItem.FRQNCUID).Comments;

                drugDetail.IdentifyingUID = presItem.UID;
                drugDetail.InstructionText = presItem.InstructionText;
                drugDetail.LocalInstructionText = presItem.LocalInstructionText;
                drugDetail.ClinicalComments = presItem.ClinicalComments;
                drugDetail.StockUID = 0;
                drugDetail.BalQty = 0;
                drugDetail.ExpiryDate = null;
                drugDetail.StoreName = store.Name;
                drugDetail.StoreUID = store.UID;
                drugDetail.BatchID = "";
                drugDetail.DFORMUID = presItem.DFORMUID;
                if (presItem.DFORMUID != null && presItem.DFORMUID != 0)
                    drugDetail.TypeDrug = db.ReferenceValue.Find(presItem.DFORMUID).Description;

                drugDetail.QNUOMUID = presItem.IMUOMUID;
                if (presItem.IMUOMUID != null && presItem.IMUOMUID != 0)
                    drugDetail.QuantityUnit = db.ReferenceValue.Find(presItem.IMUOMUID).Description;


                if (presItem.IMUOMUID != itemMaster.BaseUOM)
                {
                    double? convertValue;
                    var storeConvert = db.StoreUOMConversion.FirstOrDefault(p => p.BaseUOMUID == itemMaster.BaseUOM
                    && p.ConversionUOMUID == presItem.IMUOMUID
                    && p.StatusFlag == "A");
                    if (storeConvert != null)
                    {
                        convertValue = storeConvert.ConversionValue;
                        if (convertValue.HasValue && convertValue > 0)
                        {
                            presItem.Quantity = (presItem.Quantity.Value / convertValue.Value);
                        }
                    }
                }
                drugDetail.Quantity = presItem.Quantity;

                listDrugDispense.Add(drugDetail);
            }
            return listDrugDispense;
        }


        [Route("GetDrugStoreDispense")]
        [HttpGet]
        public List<PatientOrderDetailModel> GetDrugStoreDispense(int itemMasterUID, double useQty, int IMUOMUID, int StoreUID)
        {
            List<PatientOrderDetailModel> listDrugDispense = new List<PatientOrderDetailModel>();
            Store store = db.Store.Find(StoreUID);
            int? baseUOM;
            ItemMaster itemMaster = db.ItemMaster.Find(itemMasterUID);
            baseUOM = itemMaster.BaseUOM;
            IEnumerable<Stock> stockItem = db.Stock
    .Where(p => p.StatusFlag == "A"
    && p.ItemMasterUID == itemMasterUID
    && p.StoreUID == StoreUID && p.Quantity > 0);
            if (stockItem != null && stockItem.Count() > 0)
            {
                if (store.STDTPUID == 2901)
                    stockItem = stockItem.OrderBy(p => p.ExpiryDttm);
                else if (store.STDTPUID == 2902)
                    stockItem = stockItem.OrderBy(p => p.CUser);

                if (IMUOMUID != itemMaster.BaseUOM)
                {
                    double? convertValue;
                    var storeConvert = db.StoreUOMConversion.FirstOrDefault(p => p.BaseUOMUID == itemMaster.BaseUOM
                    && p.ConversionUOMUID == IMUOMUID
                    && p.StatusFlag == "A");
                    if (storeConvert != null)
                    {
                        convertValue = storeConvert.ConversionValue;
                        if (convertValue.HasValue && convertValue > 0)
                        {
                            useQty = (useQty / convertValue.Value);
                        }
                    }
                }

                int i = 0;
                foreach (var item in stockItem)
                {
                    i++;
                    PatientOrderDetailModel drugDetail = new PatientOrderDetailModel();
                    drugDetail.ItemUID = itemMaster.UID;
                    drugDetail.ItemCode = itemMaster.Code;
                    drugDetail.ItemName = itemMaster.Name;

                    drugDetail.StockUID = item.UID;
                    drugDetail.BalQty = item.Quantity;
                    drugDetail.ExpiryDate = item.ExpiryDttm;
                    drugDetail.StoreName = store.Name;
                    drugDetail.StoreUID = store.UID;
                    drugDetail.BatchID = item.BatchID;


                    drugDetail.QNUOMUID = IMUOMUID;
                    drugDetail.QuantityUnit = db.ReferenceValue.Find(IMUOMUID).Description;




                    if (item.Quantity >= useQty)
                    {
                        drugDetail.Quantity = useQty;
                        listDrugDispense.Add(drugDetail);
                        break;
                    }
                    else
                    {
                        if (i == stockItem.Count())
                        {
                            drugDetail.Quantity = useQty;
                        }
                        else
                        {
                            drugDetail.Quantity = item.Quantity;
                            useQty = useQty - (drugDetail.Quantity ?? 0);
                        }

                    }

                    listDrugDispense.Add(drugDetail);
                }
            }
            else
            {
                PatientOrderDetailModel drugDetail = new PatientOrderDetailModel();
                drugDetail.ItemUID = itemMaster.UID;
                drugDetail.ItemCode = itemMaster.Code;
                drugDetail.ItemName = itemMaster.Name;

                drugDetail.StockUID = 0;
                drugDetail.BalQty = 0;
                drugDetail.ExpiryDate = null;
                drugDetail.StoreName = store.Name;
                drugDetail.StoreUID = store.UID;
                drugDetail.BatchID = "";

                drugDetail.QNUOMUID = IMUOMUID;
                drugDetail.QuantityUnit = db.ReferenceValue.Find(IMUOMUID).Description;


                if (IMUOMUID != itemMaster.BaseUOM)
                {
                    double? convertValue;
                    var storeConvert = db.StoreUOMConversion.FirstOrDefault(p => p.BaseUOMUID == itemMaster.BaseUOM
                    && p.ConversionUOMUID == IMUOMUID
                    && p.StatusFlag == "A");
                    if (storeConvert != null)
                    {
                        convertValue = storeConvert.ConversionValue;
                        if (convertValue.HasValue && convertValue > 0)
                        {
                            useQty = (useQty / convertValue.Value);
                        }
                    }
                }
                drugDetail.Quantity = useQty;

                listDrugDispense.Add(drugDetail);
            }

            return listDrugDispense;
        }

        [Route("PrintStrickerDrug")]
        [HttpGet]
        public List<DrugStickerModel> PrintStrickerDrug(long prescriptionItemUID)
        {
            DataTable dtSticker = SqlDirectStore.pPrintStrickerDrug(prescriptionItemUID);
            List<DrugStickerModel> sticker = dtSticker.ToList<DrugStickerModel>();

            return sticker;
        }

    }
}
