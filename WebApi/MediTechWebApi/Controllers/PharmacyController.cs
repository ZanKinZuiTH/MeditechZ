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
using System.Data.Entity;

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
                    stockItem = stockItem.OrderBy(p => p.ExpiryDttm).ThenBy(p => p.CWhen);
                else if (store.STDTPUID == 2902)
                    stockItem = stockItem.OrderBy(p => p.CWhen).ThenBy(p => p.ExpiryDttm);

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
                    stockItem = stockItem.OrderBy(p => p.ExpiryDttm).ThenBy(p => p.CWhen);
                else if (store.STDTPUID == 2902)
                    stockItem = stockItem.OrderBy(p => p.CWhen).ThenBy(p => p.ExpiryDttm);

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
                    drugDetail.OwnerOrganisationUID = item.OwnerOrganisationUID;

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


        #region Prescription

        [Route("Searchprescription")]
        [HttpGet]
        public List<PrescriptionModel> Searchprescription(DateTime? dateFrom, DateTime? dateTo, string statusList, long? patientUID
            , string prescriptionNumber, int? organisationUID)
        {

            List<int> ordstList = !string.IsNullOrEmpty(statusList) ? statusList.Split(',').Select(p => int.Parse(p)).ToList() : new List<int>();
            List<PrescriptionModel> data = (from ps in db.Prescription
                                            join pa in db.Patient on ps.PatientUID equals pa.UID
                                            join pv in db.PatientVisit on ps.PatientVisitUID equals pv.UID
                                            where ps.StatusFlag == "A"
                 && (dateFrom == null || DbFunctions.TruncateTime(ps.PrescribedDttm) >= DbFunctions.TruncateTime(dateFrom))
                 && (dateTo == null || DbFunctions.TruncateTime(ps.PrescribedDttm) <= DbFunctions.TruncateTime(dateTo))
                 && (statusList == null || ordstList.Any(p => p == ps.ORDSTUID))
                 && (patientUID == null || ps.PatientUID == patientUID)
                 && (string.IsNullOrEmpty(prescriptionNumber) || ps.PrescriptionNumber == prescriptionNumber)
                 && (organisationUID == null || ps.OwnerOrganisationUID == organisationUID)
                                            select new PrescriptionModel
                                            {
                                                PrescriptionUID = ps.UID,
                                                PrescriptionNumber = ps.PrescriptionNumber,
                                                PrescriptionStatus = SqlFunction.fGetRfValDescription(ps.ORDSTUID ?? 0),
                                                PatientID = pa.PatientID,
                                                PRSTYPUID = ps.PRSTYPUID,
                                                PrescriptionType = SqlFunction.fGetRfValDescription(ps.PRSTYPUID ?? 0),
                                                PatientName = SqlFunction.fGetPatientName(ps.PatientUID),
                                                PatientVisitUID = ps.PatientVisitUID,
                                                AgeString = pa.DOBDttm.HasValue ? SqlFunction.fGetAgeString(pa.DOBDttm.Value) : "",
                                                DOBDttm = pa.DOBDttm.HasValue ? pa.DOBDttm : null,
                                                Gender = SqlFunction.fGetRfValDescription(pa.SEXXXUID ?? 0),
                                                EncounterType = SqlFunction.fGetRfValDescription(pv.ENTYPUID ?? 0),
                                                PrescribedDttm = ps.PrescribedDttm,
                                                IsBilled = pv.IsBillFinalized == null ? "N" : pv.IsBillFinalized,
                                                LocationName = SqlFunction.fGetLocationName(pv.LocationUID ?? 0),
                                                OrganisationName = SqlFunction.fGetHealthOrganisationName(ps.OwnerOrganisationUID ?? 0),
                                                OwnerOrganisationUID = ps.OwnerOrganisationUID,
                                                VisitID = pv.VisitID
                                            }).ToList();




            if (data != null)
            {
                foreach (var prescription in data)
                {
                    var prescriptionItems = db.PrescriptionItem
                    .Where(p => p.PrescriptionUID == prescription.PrescriptionUID && p.StatusFlag == "A")
                    .Select(p => new PrescriptionItemModel
                    {
                        PrescriptionItemUID = p.UID,
                        PrescriptionUID = p.PrescriptionUID,
                        PrestionItemStatus = SqlFunction.fGetRfValDescription(p.ORDSTUID ?? 0),
                        ORDSTUID = p.ORDSTUID,
                        ItemCode = p.ItemCode,
                        ItemName = p.ItemName,
                        ItemMasterUID = p.ItemMasterUID,
                        Quantity = p.Quantity,
                        QuantityUnit = SqlFunction.fGetRfValDescription(p.IMUOMUID ?? 0),
                        IMUOMUID = p.IMUOMUID,
                        DFORMUID = p.DFORMUID,
                        DrugForm = SqlFunction.fGetRfValDescription(p.DFORMUID ?? 0),
                        StoreUID = p.StoreUID,
                        StoreName = SqlFunction.fGetStoreName(p.StoreUID ?? 0),
                        InstructionRoute = SqlFunction.fGetRfValDescription(p.PDSTSUID ?? 0),
                        Dosage = p.Dosage,
                        FRQNCUID = p.FRQNCUID,
                        Frequency = (p.FRQNCUID != null && p.FRQNCUID != 0) ? SqlFunction.fGetFrequencyDescription(p.FRQNCUID ?? 0, "TH") : "",
                        InstructionText = p.InstructionText,
                        LocalInstructionText = p.LocalInstructionText,
                        ClinicalComments = p.ClinicalComments,
                        DrugType = SqlFunction.fGetRfValDescription(p.DFORMUID ?? 0),
                        OwnerOrganisationUID = p.OwnerOrganisationUID
                    });

                    prescription.PrescriptionItems = new System.Collections.ObjectModel.ObservableCollection<PrescriptionItemModel>(prescriptionItems);

                }
            }


            return data;


        }

        [Route("GetPrescriptionItemByPrescriptionUID")]
        public List<PrescriptionItemModel> GetPrescriptionItemByPrescriptionUID(long? prescriptionUID)
        {
            List<PrescriptionItemModel> data = db.PrescriptionItem
                .Where(p => p.PrescriptionUID == prescriptionUID && p.StatusFlag == "A")
                .Select(p => new PrescriptionItemModel
                {
                    PrescriptionItemUID = p.UID,
                    PrescriptionUID = p.PrescriptionUID,
                    PrestionItemStatus = SqlFunction.fGetRfValDescription(p.ORDSTUID ?? 0),
                    ORDSTUID = p.ORDSTUID,
                    ItemCode = p.ItemCode,
                    ItemName = p.ItemName,
                    ItemMasterUID = p.ItemMasterUID,
                    Quantity = p.Quantity,
                    QuantityUnit = SqlFunction.fGetRfValDescription(p.IMUOMUID ?? 0),
                    IMUOMUID = p.IMUOMUID,
                    DFORMUID = p.DFORMUID,
                    DrugForm = SqlFunction.fGetRfValDescription(p.DFORMUID ?? 0),
                    StoreUID = p.StoreUID,
                    StoreName = SqlFunction.fGetStoreName(p.StoreUID ?? 0),
                    InstructionRoute = SqlFunction.fGetRfValDescription(p.PDSTSUID ?? 0),
                    Dosage = p.Dosage,
                    FRQNCUID = p.FRQNCUID,
                    //Frequency = (p.FRQNCUID != null && p.FRQNCUID != 0) ? db.FrequencyDefinition.Find(p.FRQNCUID).Comments : "",
                    InstructionText = p.InstructionText,
                    LocalInstructionText = p.LocalInstructionText,
                    ClinicalComments = p.ClinicalComments,
                    DrugType = SqlFunction.fGetRfValDescription(p.DFORMUID ?? 0)
                }).ToList();

            foreach (var item in data)
            {

                IEnumerable<Stock> stockItem = db.Stock
                .Where(p => p.StatusFlag == "A"
                && p.ItemMasterUID == item.ItemMasterUID
                && p.StoreUID == item.StoreUID && p.Quantity > 0);
                if (stockItem != null && stockItem.Count() > 0)
                {
                    Store store = db.Store.Find(item.StoreUID);
                    if (store.STDTPUID == 2901)
                        stockItem = stockItem.OrderBy(p => p.ExpiryDttm).ThenBy(p => p.CWhen);
                    else if (store.STDTPUID == 2902)
                        stockItem = stockItem.OrderBy(p => p.CWhen).ThenBy(p => p.ExpiryDttm);

                    item.ExpiryDate = stockItem.FirstOrDefault().ExpiryDttm;

                }

                if (item.FRQNCUID != null && item.FRQNCUID != 0)
                {
                    item.Frequency = db.FrequencyDefinition.Find(item.FRQNCUID).Comments;
                }

            }


            return data;
        }

        [Route("GetprescriptionList")]
        [HttpGet]
        public List<PrescriptionModel> GetprescriptionList(long? prescriptionUID)
        {

            List<PrescriptionModel> data = (from ps in db.Prescription
                                            join pa in db.Patient on ps.PatientUID equals pa.UID
                                            join pv in db.PatientVisit on ps.PatientVisitUID equals pv.UID
                                            where ps.StatusFlag == "A" &&  ps.UID  == prescriptionUID
                                            select new PrescriptionModel
                                            {
                                                PrescriptionUID = ps.UID,
                                                PrescriptionNumber = ps.PrescriptionNumber,
                                                PrescriptionStatus = SqlFunction.fGetRfValDescription(ps.ORDSTUID ?? 0),
                                                PatientID = pa.PatientID,
                                                PRSTYPUID = ps.PRSTYPUID,
                                                PrescriptionType = SqlFunction.fGetRfValDescription(ps.PRSTYPUID ?? 0),
                                                PatientName = SqlFunction.fGetPatientName(ps.PatientUID),
                                                PatientVisitUID = ps.PatientVisitUID,
                                                AgeString = pa.DOBDttm.HasValue ? SqlFunction.fGetAgeString(pa.DOBDttm.Value) : "",
                                                DOBDttm = pa.DOBDttm.HasValue ? pa.DOBDttm : null,
                                                Gender = SqlFunction.fGetRfValDescription(pa.SEXXXUID ?? 0),
                                                EncounterType = SqlFunction.fGetRfValDescription(pv.ENTYPUID ?? 0),
                                                PrescribedDttm = ps.PrescribedDttm,
                                                IsBilled = pv.IsBillFinalized == null ? "N" : pv.IsBillFinalized,
                                                LocationName = SqlFunction.fGetLocationName(pv.LocationUID ?? 0),
                                                OrganisationName = SqlFunction.fGetHealthOrganisationName(ps.OwnerOrganisationUID ?? 0),
                                                OwnerOrganisationUID = ps.OwnerOrganisationUID,
                                                VisitID = pv.VisitID,
                                                DrugAllergy = SqlFunction.fGetPatientAllergy(pa.UID)
                                            }).ToList();




            if (data != null)
            {
                foreach (var prescription in data)
                {
                    var prescriptionItems = db.PrescriptionItem
                    .Where(p => p.PrescriptionUID == prescription.PrescriptionUID && p.StatusFlag == "A")
                    .Select(p => new PrescriptionItemModel
                    {
                        PrescriptionItemUID = p.UID,
                        PrescriptionUID = p.PrescriptionUID,
                        PrestionItemStatus = SqlFunction.fGetRfValDescription(p.ORDSTUID ?? 0),
                        ORDSTUID = p.ORDSTUID,
                        ItemCode = p.ItemCode,
                        ItemName = p.ItemName,
                        ItemMasterUID = p.ItemMasterUID,
                        Quantity = p.Quantity,
                        QuantityUnit = SqlFunction.fGetRfValDescription(p.IMUOMUID ?? 0),
                        IMUOMUID = p.IMUOMUID,
                        DFORMUID = p.DFORMUID,
                        DrugForm = SqlFunction.fGetRfValDescription(p.DFORMUID ?? 0),
                        StoreUID = p.StoreUID,
                        StoreName = SqlFunction.fGetStoreName(p.StoreUID ?? 0),
                        InstructionRoute = SqlFunction.fGetRfValDescription(p.PDSTSUID ?? 0),
                        Dosage = p.Dosage,
                        FRQNCUID = p.FRQNCUID,
                        Frequency = (p.FRQNCUID != null && p.FRQNCUID != 0) ? SqlFunction.fGetFrequencyDescription(p.FRQNCUID ?? 0, "TH") : "",
                        InstructionText = p.InstructionText,
                        LocalInstructionText = p.LocalInstructionText,
                        ClinicalComments = p.ClinicalComments,
                        DrugType = SqlFunction.fGetRfValDescription(p.DFORMUID ?? 0)
                    });

                    prescription.PrescriptionItems = new System.Collections.ObjectModel.ObservableCollection<PrescriptionItemModel>(prescriptionItems);

                }
            }


            return data;


        }


        [Route("Getprescription")]
        [HttpGet]
        public PrescriptionModel Getprescription(long? prescriptionUID)
        {

            PrescriptionModel data = (from ps in db.Prescription
                                            join pa in db.Patient on ps.PatientUID equals pa.UID
                                            join pv in db.PatientVisit on ps.PatientVisitUID equals pv.UID
                                            where ps.StatusFlag == "A" && ps.UID == prescriptionUID
                                            select new PrescriptionModel
                                            {
                                                PrescriptionUID = ps.UID,
                                                PrescriptionNumber = ps.PrescriptionNumber,
                                                PrescriptionStatus = SqlFunction.fGetRfValDescription(ps.ORDSTUID ?? 0),
                                                PatientID = pa.PatientID,
                                                PRSTYPUID = ps.PRSTYPUID,
                                                PrescriptionType = SqlFunction.fGetRfValDescription(ps.PRSTYPUID ?? 0),
                                                PatientName = SqlFunction.fGetPatientName(ps.PatientUID),
                                                PatientVisitUID = ps.PatientVisitUID,
                                                AgeString = pa.DOBDttm.HasValue ? SqlFunction.fGetAgeString(pa.DOBDttm.Value) : "",
                                                DOBDttm = pa.DOBDttm.HasValue ? pa.DOBDttm : null,
                                                Gender = SqlFunction.fGetRfValDescription(pa.SEXXXUID ?? 0),
                                                EncounterType = SqlFunction.fGetRfValDescription(pv.ENTYPUID ?? 0),
                                                PrescribedDttm = ps.PrescribedDttm,
                                                IsBilled = pv.IsBillFinalized == null ? "N" : pv.IsBillFinalized,
                                                LocationName = SqlFunction.fGetLocationName(pv.LocationUID ?? 0),
                                                OrganisationName = SqlFunction.fGetHealthOrganisationName(ps.OwnerOrganisationUID ?? 0),
                                                OwnerOrganisationUID = ps.OwnerOrganisationUID
                                            }).FirstOrDefault();

            return data;
        }


        [Route("UpdatePrescriptionLabelSticker")]
        [HttpPut]
        public HttpResponseMessage UpdatePrescriptionLabelSticker(long prescriptionItemUID, String localInstructionText, int userID)
        {
            try
            {
                PrescriptionItem prescriptionItem = db.PrescriptionItem.Find(prescriptionItemUID);
                if (prescriptionItem != null)
                {
                    db.PrescriptionItem.Attach(prescriptionItem);
                    prescriptionItem.LocalInstructionText = localInstructionText;
                    prescriptionItem.MUser = userID;
                    prescriptionItem.MWhen = DateTime.Now;

                    db.SaveChanges();
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }


        [Route("CancelDispensed")]
        [HttpPost]
        public HttpResponseMessage CancelDispensed(PrescriptionItemModel prescriptionItemModel, int userUID)
        {
            try
            {
                DateTime now = DateTime.Now;
                using (var tran = new TransactionScope())
                {
                    var stockmovement = (from pre in db.PrescriptionItem
                                         join dis in db.DispensedItem on pre.UID equals dis.PrescriptionItemUID
                                         join stv in db.StockMovement on new { key1 = dis.UID, key2 = "DispensedItem" } equals new { key1 = stv.RefUID ?? 0, key2 = stv.RefTable }
                                         where pre.StatusFlag == "A"
                                         && dis.StatusFlag == "A"
                                         && stv.StatusFlag == "A"
                                         && dis.ORDSTUID == 2861 // Dispensed
                                         && pre.UID == prescriptionItemModel.PrescriptionItemUID
                                         select new
                                         {
                                             StockUID = stv.StockUID,
                                             OutQty = stv.OutQty,
                                             RefUID = stv.RefUID,
                                             StoreUID = stv.StoreUID,
                                             StockmovementUID = stv.UID,
                                             DispensedItemUID = dis.UID,
                                             PresciptionItemUID = pre.UID,
                                             PrescriptionUID = pre.PrescriptionUID
                                         }).FirstOrDefault();

                    Stock stock = db.Stock.Find(stockmovement.StockUID);

                    double totalBFQty = SqlStatement.GetItemTotalQuantity(stock.ItemMasterUID, stock.StoreUID, stock.OwnerOrganisationUID);
                    double bfQty = stock.Quantity;

                    DispensedItem dispensedItem = db.DispensedItem.Find(stockmovement.DispensedItemUID);
                    db.DispensedItem.Attach(dispensedItem);
                    dispensedItem.MUser = userUID;
                    dispensedItem.MWhen = now;
                    dispensedItem.ORDSTUID = 2875;


                    PrescriptionItem prescriptionItem = db.PrescriptionItem.Find(stockmovement.PresciptionItemUID);
                    db.PrescriptionItem.Attach(prescriptionItem);
                    prescriptionItem.Comments = prescriptionItemModel.Comments;
                    prescriptionItem.MUser = userUID;
                    prescriptionItem.MWhen = now;
                    prescriptionItem.ORDSTUID = 2875;
                    db.SaveChanges();

                    Prescription prescription = db.Prescription.Find(stockmovement.PrescriptionUID);
                    db.Prescription.Attach(prescription);
                    prescription.MUser = userUID;
                    prescription.MWhen = now;
                    prescription.ORDSTUID = (new PharmacyController()).CheckPrescriptionStatus(prescription.UID);



                    PatientOrderDetail patientOrderDetail = db.PatientOrderDetail.Find(prescriptionItem.PatientOrderDetailUID);
                    db.PatientOrderDetail.Attach(patientOrderDetail);
                    patientOrderDetail.MUser = userUID;
                    patientOrderDetail.MWhen = now;
                    patientOrderDetail.ORDSTUID = 2875;

                    PatientOrderDetailHistory patOrderDetailHistory = new PatientOrderDetailHistory();
                    patOrderDetailHistory.PatientOrderDetailUID = patientOrderDetail.UID;
                    patOrderDetailHistory.ORDSTUID = patientOrderDetail.ORDSTUID;
                    patOrderDetailHistory.EditByUserID = userUID;
                    patOrderDetailHistory.EditedDttm = now;
                    patOrderDetailHistory.CUser = userUID;
                    patOrderDetailHistory.CWhen = now;
                    patOrderDetailHistory.MUser = userUID;
                    patOrderDetailHistory.MWhen = now;
                    patOrderDetailHistory.StatusFlag = "A";
                    db.PatientOrderDetailHistory.Add(patOrderDetailHistory);

                    db.Stock.Attach(stock);
                    stock.Quantity = stock.Quantity + stockmovement.OutQty;
                    stock.MUser = userUID;
                    stock.MWhen = now;
                    stock.StatusFlag = "A";


                    StockMovement stockMovement = db.StockMovement.Find(stockmovement.StockmovementUID);
                    db.StockMovement.Attach(stockMovement);
                    stockMovement.MUser = userUID;
                    stockMovement.MWhen = now;
                    stockMovement.Note = "SaleReturn";


                    db.SaveChanges();



                    SaleReturn saleReturn = new SaleReturn();


                    saleReturn.ReturnedBy = userUID;
                    saleReturn.ReturnDttm = now;
                    saleReturn.PatientUID = prescription.PatientUID;
                    saleReturn.PatientVisitUID = prescription.PatientVisitUID;
                    saleReturn.StoreUID = stockmovement.StoreUID ?? 0;
                    saleReturn.OwnerOrganisationUID = stock.OwnerOrganisationUID;
                    saleReturn.MUser = userUID;
                    saleReturn.MWhen = now;
                    saleReturn.CUser = userUID;
                    saleReturn.CWhen = now;
                    saleReturn.StatusFlag = "A";

                    db.SaleReturn.Add(saleReturn);
                    db.SaveChanges();

                    SaleReturnList saleReturnList = new SaleReturnList();
                    saleReturnList.SaleReturnUID = saleReturn.UID;
                    saleReturnList.StockUID = stock.UID;
                    saleReturnList.BatchID = stock.BatchID;
                    saleReturnList.ItemMasterUID = stock.ItemMasterUID;
                    saleReturnList.ItemName = stock.ItemName;
                    saleReturnList.Quantity = stockmovement.OutQty;
                    saleReturnList.IMUOMUID = stock.IMUOMUID;
                    saleReturnList.ItemCost = stock.ItemCost;
                    saleReturnList.DispensedItemUID = stockmovement.RefUID ?? 0;
                    saleReturnList.PatientOrderDetailUID = prescriptionItem.PatientOrderDetailUID;
                    //saleReturnList.PatientBilledItemUID = item.UID;
                    saleReturnList.OwnerOrganisationUID = stock.OwnerOrganisationUID;
                    saleReturnList.MUser = userUID;
                    saleReturnList.MWhen = now;
                    saleReturnList.CUser = userUID;
                    saleReturnList.CWhen = now;
                    saleReturnList.StatusFlag = "A";
                    db.SaleReturnList.Add(saleReturnList);
                    db.SaveChanges();

                    double totalBalQty = SqlStatement.GetItemTotalQuantity(saleReturnList.ItemMasterUID, stock.StoreUID, stock.OwnerOrganisationUID);
                    double balQty = stock.Quantity;
                    SqlDirectStore.pInvenInsertStockMovement(saleReturnList.StockUID, stock.StoreUID, null, saleReturnList.ItemMasterUID, saleReturnList.BatchID, now, totalBFQty, bfQty, saleReturnList.Quantity, 0, balQty, totalBalQty, stock.IMUOMUID, stock.ItemCost ?? 0, null, "SaleReturnList", saleReturnList.UID, null, null, userUID);

                    SqlDirectStore.pInvenInsertStockBalance(stock.StoreUID, stock.ItemMasterUID, userUID);

                    tran.Complete();
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("DispensePrescription")]
        [HttpPost]
        public HttpResponseMessage DispensePrescription(PrescriptionModel prescriptionModel, int userUID)
        {
            try
            {
                int RAISEDUID = 2847;
                var prescriptionItems = db.PrescriptionItem.Where(p => p.PrescriptionUID == prescriptionModel.PrescriptionUID
                && p.StatusFlag == "A"
                && p.ORDSTUID == RAISEDUID
                );
                DateTime now = DateTime.Now;
                foreach (var item in prescriptionItems)
                {
                    DataTable dtStockUsed = SqlDirectStore.pDispensePrescriptionItem(item.UID, userUID);
                }

                var prescription = db.Prescription.Find(prescriptionModel.PrescriptionUID);

                db.Prescription.Attach(prescription);
                prescription.MUser = userUID;
                prescription.MWhen = now;
                prescription.DispensedDttm = now;
                prescription.ORDSTUID = (new PharmacyController()).CheckPrescriptionStatus(prescription.UID);

                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("DispensePrescriptionItem")]
        [HttpPost]
        public HttpResponseMessage DispensePrescriptionItem(PrescriptionItemModel prescriptionItemModel, int userUID)
        {
            try
            {
                DateTime now = DateTime.Now;
                DataTable dtStockUsed = SqlDirectStore.pDispensePrescriptionItem(prescriptionItemModel.PrescriptionItemUID, userUID);

                var prescription = db.Prescription.Find(prescriptionItemModel.PrescriptionUID);

                db.Prescription.Attach(prescription);
                prescription.MUser = userUID;
                prescription.MWhen = now;
                prescription.DispensedDttm = now;
                prescription.ORDSTUID = (new PharmacyController()).CheckPrescriptionStatus(prescription.UID);

                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }
        public int? CheckPrescriptionStatus(long prescriptionUID)
        {
            int? ORDSTUID = null;
            List<PrescriptionItem> listPrescriptionItem = db.PrescriptionItem.Where(p => p.PrescriptionUID == prescriptionUID).ToList();
            List<ReferenceValue> listOrderStatus = db.ReferenceValue.Where(p => p.DomainCode == "ORDST").ToList();
            ReferenceValue dispensed = listOrderStatus.Where(p => p.ValueCode == "DISPE").FirstOrDefault();
            ReferenceValue cancel = listOrderStatus.Where(p => p.ValueCode == "CANCLD").FirstOrDefault();
            ReferenceValue cancelDispensed = listOrderStatus.Where(p => p.ValueCode == "DISPCANCL").FirstOrDefault();
            ReferenceValue partiallyDispensed = listOrderStatus.Where(p => p.ValueCode == "OPDISP").FirstOrDefault();
            ReferenceValue partiallyCancelDispensed = listOrderStatus.Where(p => p.ValueCode == "OPCANDISP").FirstOrDefault();
            ReferenceValue partiallycancel = listOrderStatus.Where(p => p.ValueCode == "PRCAN").FirstOrDefault();
            int dispensedCount = listPrescriptionItem.Where(p => p.ORDSTUID == dispensed.UID).Count();
            int cancelCount = listPrescriptionItem.Where(p => p.ORDSTUID == cancel.UID).Count();
            int cancelDispensedCount = listPrescriptionItem.Where(p => p.ORDSTUID == cancelDispensed.UID).Count();
            int PrescriptionDetailCount = listPrescriptionItem.Count();
            if (listPrescriptionItem != null)
            {
                if (dispensedCount == PrescriptionDetailCount)
                {
                    ORDSTUID = dispensed.UID;
                }
                else if (cancelCount == PrescriptionDetailCount)
                {
                    ORDSTUID = cancel.UID;
                }
                else if (cancelDispensedCount == PrescriptionDetailCount)
                {
                    ORDSTUID = cancelDispensed.UID;
                }
                else if (dispensedCount >= 1)
                {
                    ORDSTUID = partiallyDispensed.UID;
                }
                else if (cancelDispensedCount >= 1)
                {
                    ORDSTUID = partiallyCancelDispensed.UID;
                }
                else if (cancelCount >= 1)
                {
                    ORDSTUID = partiallycancel.UID;
                }
            }
            return ORDSTUID;
        }

        #endregion


        [Route("GetPatientOrderStanding")]
        [HttpGet]
        public List<PatientOrderStandingModel> GetPatientOrderStanding(int? wardUID, int storeUID,int fillDays,DateTime? excludeTime)
        {
            DateTime currentDate = DateTime.Now.Date;
            DateTime currentDateEnd = currentDate.Date.AddSeconds(86399);
            DateTime endDate = currentDate.AddDays(fillDays);

            List<PatientOrderStandingModel> data = (from ptod in db.PatientOrderDetail
                                                  join pto in db.PatientOrder on ptod.PatientOrderUID equals pto.UID
                                                  join pv in db.PatientVisit on pto.PatientVisitUID equals pv.UID
                                                  join b in db.BillableItem on ptod.BillableItemUID equals b.UID
                                                  where pv.StatusFlag == "A"
                                                  && pto.StatusFlag == "A"
                                                  && ptod.StatusFlag == "A"
                                                  && b.StatusFlag == "A"
                                                  && pv.VISTSUID != 423 //Billing Inprogress
                                                  && ptod.ORDSTUID != 2848 // cancel
                                                  && pv.ENSTAUID != 4423 //fit for discharge
                                                  && pto.PRSTYPUID == 4416 //Standing Order
                                                  //&& ptod.ORDSTUID == 2847 //Raised
                                                  && ptod.ORDSTUID != 2845 //complete
                                                  && ptod.IsStandingOrder == "Y"
                                                  && pto.IsContinuous == "Y"
                                                  && ptod.StartDttm <= endDate
                                                  && (ptod.EndDttm == null || ptod.EndDttm >= endDate)
                                                  && (excludeTime == null || ptod.StartDttm <= excludeTime)
                                                  && ptod.StoreUID == storeUID
                                                  && ( wardUID == null || pv.LocationUID == wardUID)
                                                  select new PatientOrderStandingModel
                                                  {
                                                      PatientOrderUID = pto.UID,
                                                      PatientUID = pv.PatientUID,
                                                      PatientVisitUID = pv.UID,
                                                      PatientOrderDetailUID = ptod.UID,
                                                      IdentifyingUID = pto.IdentifyingUID,
                                                      PatientID = SqlFunction.fGetPatientID(pv.PatientUID),
                                                      PatientName = SqlFunction.fGetPatientName(pv.PatientUID),
                                                      ItemUID = b.ItemUID,
                                                      Dosage = ptod.Dosage,
                                                      DrugDuration = ptod.DrugDuration,
                                                      BSMDDUID = b.BSMDDUID,
                                                      DFORMUID = ptod.DFORMUID,
                                                      PDSTSUID = ptod.PDSTSUID,
                                                      FRQNCUID = ptod.FRQNCUID,
                                                      StoreUID = ptod.StoreUID,
                                                      QNUOMUID = ptod.QNUOMUID,
                                                      Quantity = ptod.Quantity,
                                                      UnitPrice = ptod.UnitPrice,
                                                      NetAmount = ptod.NetAmount,
                                                      OverwritePrice = ptod.OverwritePrice,
                                                      OriginalUnitPrice = ptod.OriginalUnitPrice,
                                                      ROUTEUID = ptod.ROUTEUID,
                                                      BedUID = pv.BedUID ?? 0,
                                                      BedName = SqlFunction.fGetLocationName(pv.BedUID ?? 0),
                                                      WardUID = pv.LocationUID ?? 0,
                                                      WardName = SqlFunction.fGetLocationName(pv.LocationUID ?? 0),
                                                      OrderNumber = pto.OrderNumber,
                                                      StartDttm = pto.StartDttm,
                                                      ItemName = ptod.ItemName,
                                                      ItemCode = ptod.ItemCode,
                                                      BillableItemUID = b.UID,
                                                      PRSTYPUID = pto.PRSTYPUID,
                                                      DrugFrequency = SqlFunction.fGetFrequencyDescription(ptod.FRQNCUID ?? 0, "TH")
                                                  }).ToList();

            if (data.Count != 0)
            {
                foreach (var item in data.ToList())
                {
                    var d = (from o in db.PatientOrder
                             join odd in db.PatientOrderDetail on o.UID equals odd.PatientOrderUID
                             where o.IsIPFill == "Y"
                             && o.ParentUID == item.PatientOrderUID
                             && odd.ORDSTUID == 2861
                             && o.StartDttm >= currentDate
                             && o.StartDttm <= currentDateEnd select o).FirstOrDefault();

                    if (d != null)
                    {
                        data.Remove(item);
                    }
                }
            }

            return data;
        }
    }
}
