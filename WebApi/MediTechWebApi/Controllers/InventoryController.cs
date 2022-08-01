using MediTech.DataBase;
using MediTech.Model;
using ShareLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web.Http;

namespace MediTechWebApi.Controllers
{
    [RoutePrefix("Api/Inventory")]
    public class InventoryController : ApiController
    {
        protected MediTechEntities db = new MediTechEntities();

        #region ItemMaster

        [Route("GenerateCodeItem")]
        [HttpGet]
        public string GenerateCodeItem()
        {
            string ItemCode = string.Empty;
            int seqUID;
            ItemCode = SEQHelper.GetSEQIDFormat("SEQITCODE", out seqUID);
            return ItemCode;
        }

        [Route("SearchItemMaster")]
        [HttpGet]
        public List<ItemMasterModel> SearchItemMaster(string itemName, string itemCode, int? ITMTYPUID)
        {
            List<ItemMasterModel> data = (from p in db.ItemMaster
                                          where p.StatusFlag == "A" &&
                                          (string.IsNullOrEmpty(itemName) || p.Name.ToLower().Contains(itemName.ToLower())) &&
                                          (string.IsNullOrEmpty(itemCode) || p.Code.ToLower().Contains(itemCode.ToLower())) &&
                                          (ITMTYPUID == null || p.ITMTYPUID == ITMTYPUID)
                                          select new ItemMasterModel
                                          {
                                              ItemMasterUID = p.UID,
                                              Code = p.Code,
                                              Name = p.Name,
                                              Description = p.Description,
                                              ITMTYPUID = p.ITMTYPUID,
                                              ItemType = SqlFunction.fGetRfValDescription(p.ITMTYPUID),
                                              DrugGenaricUID = p.DrugGenaricUID,
                                              GenaricName = p.GenaricName,
                                              DispenseEnglish = p.DispenseEnglish,
                                              DispenseLocal = p.DispenseLocal,
                                              OrderInstruction = p.OrderInstruction,
                                              BaseUOM = p.BaseUOM,
                                              PrescriptionUOM = p.PrescriptionUOM,
                                              FRQNCUID = p.FRQNCUID,
                                              PDSTSUID = p.PDSTSUID,
                                              FORMMUID = p.FORMMUID,
                                              BaseUnit = SqlFunction.fGetRfValDescription(p.BaseUOM ?? 0),
                                              SalesUOM = p.SalesUOM,
                                              SalesUnit = SqlFunction.fGetRfValDescription(p.SalesUOM ?? 0),
                                              PrescriptionUnit = SqlFunction.fGetRfValDescription(p.PrescriptionUOM ?? 0),
                                              IsBatchIDMandatory = p.IsBatchIDMandatory,
                                              VATPercentage = p.VATPercentage,
                                              IsStock = p.IsStock,
                                              ManufacturerByUID = p.ManufacturerByUID,
                                              ItemCost = p.ItemCost,
                                              IsNarcotic = p.IsNarcotic,
                                              Comments = p.Comments,
                                              NRCTPUID = p.NRCTPUID,
                                              NarcoticType = SqlFunction.fGetRfValDescription(p.NRCTPUID ?? 0),
                                              CanDispenseWithOutStock = p.CanDispenseWithOutStock,
                                              MinSalesQty = p.MinSalesQty,
                                              DoseQuantity = p.DoseQuantity,
                                              ActiveFrom = p.ActiveFrom,
                                              ActiveTo = p.ActiveTo,
                                              ROUTEUID = p.ROUTEUID,
                                              Route = SqlFunction.fGetRfValDescription(p.ROUTEUID ?? 0),
                                              CUser = p.CUser,
                                              CWhen = p.CWhen,
                                              MUser = p.MUser,
                                              MWhen = p.MWhen,
                                              StatusFlag = p.StatusFlag
                                          }).ToList();

            return data;
        }



        [Route("GetItemMaster")]
        [HttpGet]
        public List<ItemMasterModel> GetItemMaster()
        {
            List<ItemMasterModel> data = db.ItemMaster.Where(p => p.StatusFlag == "A").Select(p => new ItemMasterModel()
            {
                ItemMasterUID = p.UID,
                Code = p.Code,
                Name = p.Name,
                Description = p.Description,
                ITMTYPUID = p.ITMTYPUID,
                ItemType = SqlFunction.fGetRfValDescription(p.ITMTYPUID),
                DrugGenaricUID = p.DrugGenaricUID,
                GenaricName = p.GenaricName,
                DispenseEnglish = p.DispenseEnglish,
                DispenseLocal = p.DispenseLocal,
                OrderInstruction = p.OrderInstruction,
                VATPercentage = p.VATPercentage,
                CanDispenseWithOutStock = p.CanDispenseWithOutStock,
                DoseQuantity = p.DoseQuantity,
                MinSalesQty = p.MinSalesQty,
                BaseUOM = p.BaseUOM,
                BaseUnit = SqlFunction.fGetRfValDescription(p.BaseUOM ?? 0),
                SalesUOM = p.SalesUOM,
                SalesUnit = SqlFunction.fGetRfValDescription(p.SalesUOM ?? 0),
                FRQNCUID = p.FRQNCUID,
                PDSTSUID = p.PDSTSUID,
                FORMMUID = p.FORMMUID,
                PrescriptionUOM = p.PrescriptionUOM,
                PrescriptionUnit = SqlFunction.fGetRfValDescription(p.PrescriptionUOM ?? 0),
                IsNarcotic = p.IsNarcotic,
                Comments = p.Comments,
                ItemCost = p.ItemCost,
                IsBatchIDMandatory = p.IsBatchIDMandatory,
                IsStock = p.IsStock,
                ManufacturerByUID = p.ManufacturerByUID,
                NRCTPUID = p.NRCTPUID,
                NarcoticType = SqlFunction.fGetRfValDescription(p.NRCTPUID ?? 0),
                ActiveFrom = p.ActiveFrom,
                ActiveTo = p.ActiveTo,
                ROUTEUID = p.ROUTEUID,
                CUser = p.CUser,
                CWhen = p.CWhen,
                MUser = p.MUser,
                MWhen = p.MWhen,
                StatusFlag = p.StatusFlag
            }).ToList();

            return data;
        }

        [Route("GetItemMasterByType")]
        [HttpGet]
        public List<ItemMasterModel> GetItemMasterByType(string itemType)
        {
            DateTime now = DateTime.Now;
            int ITMTYP = db.ReferenceValue.FirstOrDefault(p => p.DomainCode == "ITMTYP" && p.Description.ToLower() == itemType.ToLower() && p.StatusFlag == "A").UID;
            List<ItemMasterModel> data = db.ItemMaster
                .Where(p => p.StatusFlag == "A"
                && p.ITMTYPUID == ITMTYP
                && (p.ActiveFrom == null || DbFunctions.TruncateTime(p.ActiveFrom) <= DbFunctions.TruncateTime(now))
                && (p.ActiveTo == null || DbFunctions.TruncateTime(p.ActiveTo) >= DbFunctions.TruncateTime(now))
                )
                .Select(p => new ItemMasterModel()
                {
                    ItemMasterUID = p.UID,
                    Code = p.Code,
                    Name = p.Name,
                    Description = p.Description,
                    ITMTYPUID = p.ITMTYPUID,
                    ItemType = SqlFunction.fGetRfValDescription(p.ITMTYPUID),
                    DrugGenaricUID = p.DrugGenaricUID,
                    GenaricName = p.GenaricName,
                    DispenseEnglish = p.DispenseEnglish,
                    DispenseLocal = p.DispenseLocal,
                    OrderInstruction = p.OrderInstruction,
                    MinSalesQty = p.MinSalesQty,
                    DoseQuantity = p.DoseQuantity,
                    CanDispenseWithOutStock = p.CanDispenseWithOutStock,
                    BaseUOM = p.BaseUOM,
                    BaseUnit = SqlFunction.fGetRfValDescription(p.BaseUOM ?? 0),
                    SalesUOM = p.SalesUOM,
                    SalesUnit = SqlFunction.fGetRfValDescription(p.SalesUOM ?? 0),
                    FRQNCUID = p.FRQNCUID,
                    PDSTSUID = p.PDSTSUID,
                    FORMMUID = p.FORMMUID,
                    ROUTEUID = p.ROUTEUID,
                    VATPercentage = p.VATPercentage,
                    PrescriptionUOM = p.PrescriptionUOM,
                    PrescriptionUnit = SqlFunction.fGetRfValDescription(p.PrescriptionUOM ?? 0),
                    IsNarcotic = p.IsNarcotic,
                    Comments = p.Comments,
                    ItemCost = p.ItemCost,
                    IsBatchIDMandatory = p.IsBatchIDMandatory,
                    IsStock = p.IsStock,
                    ManufacturerByUID = p.ManufacturerByUID,
                    NRCTPUID = p.NRCTPUID,
                    NarcoticType = SqlFunction.fGetRfValDescription(p.NRCTPUID ?? 0),
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

        [Route("GetItemMasterQtyByStore")]
        [HttpGet]
        public List<ItemMasterModel> GetItemMasterQtyByStore(int storeUID)
        {
            List<ItemMasterModel> data = SqlDirectStore.pGetItemMasterQtyByStore(storeUID).ToList<ItemMasterModel>();

            return data;
        }


        [Route("GetItemMasterForIssue")]
        [HttpGet]
        public List<ItemMasterModel> GetItemMasterForIssue(int organisationUID, int storeUID)
        {
            List<ItemMasterModel> data = SqlDirectStore.pGetItemMasterForIssue(organisationUID, storeUID).ToList<ItemMasterModel>();

            return data;
        }


        [Route("GetItemMasterStock")]
        [HttpGet]
        public List<ItemMasterModel> GetItemMasterStock()
        {
            List<ItemMasterModel> data = db.ItemMaster.Where(p => p.StatusFlag == "A" && p.IsStock.ToUpper() == "Y").Select(p => new ItemMasterModel()
            {
                ItemMasterUID = p.UID,
                Code = p.Code,
                Name = p.Name,
                Description = p.Description,
                ITMTYPUID = p.ITMTYPUID,
                ItemType = SqlFunction.fGetRfValDescription(p.ITMTYPUID),
                DrugGenaricUID = p.DrugGenaricUID,
                GenaricName = p.GenaricName,
                DispenseEnglish = p.DispenseEnglish,
                DispenseLocal = p.DispenseLocal,
                OrderInstruction = p.OrderInstruction,
                CanDispenseWithOutStock = p.CanDispenseWithOutStock,
                DoseQuantity = p.DoseQuantity,
                MinSalesQty = p.MinSalesQty,
                BaseUOM = p.BaseUOM,
                BaseUnit = SqlFunction.fGetRfValDescription(p.BaseUOM ?? 0),
                SalesUOM = p.SalesUOM,
                SalesUnit = SqlFunction.fGetRfValDescription(p.SalesUOM ?? 0),
                FRQNCUID = p.FRQNCUID,
                PDSTSUID = p.PDSTSUID,
                FORMMUID = p.FORMMUID,
                PrescriptionUOM = p.PrescriptionUOM,
                PrescriptionUnit = SqlFunction.fGetRfValDescription(p.PrescriptionUOM ?? 0),
                IsNarcotic = p.IsNarcotic,
                Comments = p.Comments,
                VATPercentage = p.VATPercentage,
                ItemCost = p.ItemCost,
                IsBatchIDMandatory = p.IsBatchIDMandatory,
                IsStock = p.IsStock,
                ManufacturerByUID = p.ManufacturerByUID,
                NRCTPUID = p.NRCTPUID,
                NarcoticType = SqlFunction.fGetRfValDescription(p.NRCTPUID ?? 0),
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

        [Route("GetItemMasterByUID")]
        [HttpGet]
        public ItemMasterModel GetItemMasterByUID(int itemMasterUID)
        {
            ItemMasterModel data = db.ItemMaster.Where(p => p.StatusFlag == "A" && p.UID == itemMasterUID).Select(p => new ItemMasterModel()
            {
                ItemMasterUID = p.UID,
                Code = p.Code,
                Name = p.Name,
                Description = p.Description,
                ITMTYPUID = p.ITMTYPUID,
                ItemType = SqlFunction.fGetRfValDescription(p.ITMTYPUID),
                DrugGenaricUID = p.DrugGenaricUID,
                GenaricName = p.GenaricName,
                DispenseEnglish = p.DispenseEnglish,
                DispenseLocal = p.DispenseLocal,
                OrderInstruction = p.OrderInstruction,
                CanDispenseWithOutStock = p.CanDispenseWithOutStock,
                MinSalesQty = p.MinSalesQty,
                DoseQuantity = p.DoseQuantity,
                BaseUOM = p.BaseUOM,
                BaseUnit = SqlFunction.fGetRfValDescription(p.BaseUOM ?? 0),
                SalesUOM = p.SalesUOM,
                SalesUnit = SqlFunction.fGetRfValDescription(p.SalesUOM ?? 0),
                PrescriptionUOM = p.PrescriptionUOM,
                PrescriptionUnit = SqlFunction.fGetRfValDescription(p.PrescriptionUOM ?? 0),
                FRQNCUID = p.FRQNCUID,
                PDSTSUID = p.PDSTSUID,
                FORMMUID = p.FORMMUID,
                ROUTEUID = p.ROUTEUID,
                IsNarcotic = p.IsNarcotic,
                Comments = p.Comments,
                ItemCost = p.ItemCost,
                IsBatchIDMandatory = p.IsBatchIDMandatory,
                IsStock = p.IsStock,
                VATPercentage = p.VATPercentage,
                ManufacturerByUID = p.ManufacturerByUID,
                NRCTPUID = p.NRCTPUID,
                NarcoticType = SqlFunction.fGetRfValDescription(p.NRCTPUID ?? 0),
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

        [Route("GetItemMasterByCode")]
        [HttpGet]
        public ItemMasterModel GetItemMasterByCode(string itemCode)
        {
            ItemMasterModel data = db.ItemMaster.Where(p => p.StatusFlag == "A" && p.Code == itemCode).Select(p => new ItemMasterModel()
            {
                ItemMasterUID = p.UID,
                Code = p.Code,
                Name = p.Name,
                Description = p.Description,
                ITMTYPUID = p.ITMTYPUID,
                ItemType = SqlFunction.fGetRfValDescription(p.ITMTYPUID),
                DrugGenaricUID = p.DrugGenaricUID,
                GenaricName = p.GenaricName,
                DispenseEnglish = p.DispenseEnglish,
                DispenseLocal = p.DispenseLocal,
                OrderInstruction = p.OrderInstruction,
                CanDispenseWithOutStock = p.CanDispenseWithOutStock,
                DoseQuantity = p.DoseQuantity,
                MinSalesQty = p.MinSalesQty,
                BaseUOM = p.BaseUOM,
                ROUTEUID = p.ROUTEUID,
                BaseUnit = SqlFunction.fGetRfValDescription(p.BaseUOM ?? 0),
                SalesUOM = p.SalesUOM,
                SalesUnit = SqlFunction.fGetRfValDescription(p.SalesUOM ?? 0),
                PrescriptionUOM = p.PrescriptionUOM,
                FRQNCUID = p.FRQNCUID,
                PDSTSUID = p.PDSTSUID,
                FORMMUID = p.FORMMUID,
                PrescriptionUnit = SqlFunction.fGetRfValDescription(p.PrescriptionUOM ?? 0),
                IsNarcotic = p.IsNarcotic,
                Comments = p.Comments,
                ItemCost = p.ItemCost,
                IsBatchIDMandatory = p.IsBatchIDMandatory,
                IsStock = p.IsStock,
                VATPercentage = p.VATPercentage,
                ManufacturerByUID = p.ManufacturerByUID,
                NRCTPUID = p.NRCTPUID,
                NarcoticType = SqlFunction.fGetRfValDescription(p.NRCTPUID ?? 0),
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

        [Route("ManageItemMaster")]
        [HttpPost]
        public HttpResponseMessage ManageItemMaster(ItemMasterModel itemMasterModel, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;

                using (var tran = new TransactionScope())
                {
                    ItemMaster itemMaster = db.ItemMaster.Find(itemMasterModel.ItemMasterUID);

                    if (itemMaster == null)
                    {
                        itemMaster = new ItemMaster();
                        itemMaster.CUser = userID;
                        itemMaster.CWhen = now;
                    }

                    itemMaster.Code = itemMasterModel.Code;
                    itemMaster.Name = itemMasterModel.Name;
                    itemMaster.Description = itemMasterModel.Description;
                    itemMaster.ITMTYPUID = itemMasterModel.ITMTYPUID;
                    itemMaster.ActiveFrom = itemMasterModel.ActiveFrom;
                    itemMaster.ActiveTo = itemMasterModel.ActiveTo;
                    itemMaster.ItemCost = itemMasterModel.ItemCost;
                    itemMaster.DrugGenaricUID = itemMasterModel.DrugGenaricUID;
                    itemMaster.GenaricName = itemMasterModel.GenaricName;
                    if (!string.IsNullOrEmpty(itemMasterModel.GenaricName))
                    {
                        itemMaster.GenaricNameSearch = (new MasterDataController()).SetItemNameSearch(itemMasterModel.GenaricName);
                    }
                    itemMaster.DispenseEnglish = itemMasterModel.DispenseEnglish;
                    itemMaster.DispenseLocal = itemMasterModel.DispenseLocal;
                    itemMaster.OrderInstruction = itemMasterModel.OrderInstruction;
                    itemMaster.BaseUOM = itemMasterModel.BaseUOM;
                    itemMaster.SalesUOM = itemMasterModel.SalesUOM;
                    itemMaster.PrescriptionUOM = itemMasterModel.PrescriptionUOM;
                    itemMaster.DoseQuantity = itemMasterModel.DoseQuantity;
                    itemMaster.FRQNCUID = itemMasterModel.FRQNCUID;
                    itemMaster.FORMMUID = itemMasterModel.FORMMUID;
                    itemMaster.PDSTSUID = itemMasterModel.PDSTSUID;
                    itemMaster.IsBatchIDMandatory = itemMasterModel.IsBatchIDMandatory;
                    itemMaster.IsStock = itemMasterModel.IsStock;
                    itemMaster.IsNarcotic = itemMasterModel.IsNarcotic;
                    itemMaster.ItemCost = itemMasterModel.ItemCost;
                    itemMaster.CanDispenseWithOutStock = itemMasterModel.CanDispenseWithOutStock;
                    itemMaster.MinSalesQty = itemMasterModel.MinSalesQty;
                    itemMaster.VATPercentage = itemMasterModel.VATPercentage;
                    itemMaster.Comments = itemMasterModel.Comments;
                    itemMaster.ManufacturerByUID = itemMasterModel.ManufacturerByUID;
                    itemMaster.NRCTPUID = itemMasterModel.NRCTPUID;
                    itemMaster.ROUTEUID = itemMasterModel.ROUTEUID;
                    itemMaster.MUser = userID;
                    itemMaster.MWhen = now;
                    itemMaster.StatusFlag = "A";

                    db.ItemMaster.AddOrUpdate(itemMaster);
                    db.SaveChanges();

                    int bsmddUID = 0;
                    if (itemMaster.ITMTYPUID == 632)
                    {
                        bsmddUID = 2826;
                    }
                    else if (itemMaster.ITMTYPUID == 631)
                    {
                        bsmddUID = 2953;
                    }

                    BillableItem billItm = db.BillableItem.Where(p => p.ItemUID == itemMaster.UID && p.BSMDDUID == bsmddUID && p.StatusFlag == "A").FirstOrDefault();
                    if (billItm != null)
                    {
                        if (itemMaster.Code != billItm.Code)
                        {
                            db.BillableItem.Attach(billItm);
                            billItm.ItemName = itemMaster.Name;
                            billItm.Code = itemMaster.Code;
                            billItm.MUser = userID;
                            billItm.MWhen = now;
                            db.SaveChanges();
                        }

                    }

                    #region Delete itemUOMConversion
                    IEnumerable<ItemUOMConversion> itemUOMConversions = db.ItemUOMConversion.Where(p => p.ItemMasterUID == itemMaster.UID);

                    if (itemMasterModel.ItemUOMConversions == null)
                    {
                        foreach (var item in itemUOMConversions)
                        {
                            db.ItemUOMConversion.Attach(item);
                            item.MUser = userID;
                            item.MWhen = now;
                            item.StatusFlag = "D";
                        }
                    }
                    else
                    {
                        foreach (var item in itemUOMConversions)
                        {
                            var data = itemMasterModel.ItemUOMConversions.FirstOrDefault(p => p.ItemMasterUID == item.UID);
                            if (data == null)
                            {
                                db.ItemUOMConversion.Attach(item);
                                item.MUser = userID;
                                item.MWhen = now;
                                item.StatusFlag = "D";
                            }

                        }
                    }

                    db.SaveChanges();

                    #endregion

                    if (itemMasterModel.ItemUOMConversions != null && itemMasterModel.ItemUOMConversions.Count > 0)
                    {
                        foreach (var item in itemMasterModel.ItemUOMConversions)
                        {
                            ItemUOMConversion itemUOM = db.ItemUOMConversion.Find(item.ItemUOMConversionUID);
                            if (itemUOM == null)
                            {
                                itemUOM = new ItemUOMConversion();
                                itemUOM.CWhen = now;
                                itemUOM.CUser = userID;
                                itemUOM.StatusFlag = "A";
                            }
                            itemUOM.ItemMasterUID = itemMaster.UID;
                            itemUOM.BaseUOMUID = itemMasterModel.BaseUOM ?? 0;
                            itemUOM.ConversionUOMUID = item.ConversionUOMUID;
                            itemUOM.ConversionValue = item.ConversionValue;
                            itemUOM.MWhen = now;
                            itemUOM.MUser = userID;

                            db.ItemUOMConversion.AddOrUpdate(itemUOM);
                            db.SaveChanges();
                        }
                    }


                    #region Delete ItemVendorDetail
                    IEnumerable<ItemVendorDetail> itemVendorDetails = db.ItemVendorDetail.Where(p => p.ItemMasterUID == itemMaster.UID);

                    if (itemMasterModel.ItemVendorDetails == null)
                    {
                        foreach (var item in itemVendorDetails)
                        {
                            db.ItemVendorDetail.Attach(item);
                            item.MUser = userID;
                            item.MWhen = now;
                            item.StatusFlag = "D";
                        }
                    }
                    else
                    {
                        foreach (var item in itemVendorDetails)
                        {
                            var data = itemMasterModel.ItemVendorDetails.FirstOrDefault(p => p.ItemMasterUID == item.UID);
                            if (data == null)
                            {
                                db.ItemVendorDetail.Attach(item);
                                item.MUser = userID;
                                item.MWhen = now;
                                item.StatusFlag = "D";
                            }

                        }
                    }

                    db.SaveChanges();

                    #endregion


                    if (itemMasterModel.ItemVendorDetails != null && itemMasterModel.ItemVendorDetails.Count > 0)
                    {
                        foreach (var item in itemMasterModel.ItemVendorDetails)
                        {
                            ItemVendorDetail itemVendor = db.ItemVendorDetail.Find(item.ItemVendorDetailUID);
                            if (itemVendor == null)
                            {
                                itemVendor = new ItemVendorDetail();
                                itemVendor.CWhen = now;
                                itemVendor.CUser = userID;
                                itemVendor.StatusFlag = "A";
                            }

                            itemVendor.ItemMasterUID = itemMaster.UID;
                            itemVendor.VendorDetailUID = item.VendorDetailUID;
                            itemVendor.ItemAmount = item.ItemAmount;
                            itemVendor.MWhen = now;
                            itemVendor.MUser = userID;

                            db.ItemVendorDetail.AddOrUpdate(itemVendor);
                            db.SaveChanges();

                        }
                    }

                    tran.Complete();
                }


                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("DeleteItemMaster")]
        [HttpDelete]
        public HttpResponseMessage DeleteItemMaster(int itemMasterUID, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                ItemMaster itemMaster = db.ItemMaster.Find(itemMasterUID);
                if (itemMaster != null)
                {
                    db.ItemMaster.Attach(itemMaster);
                    itemMaster.MUser = userID;
                    itemMaster.MWhen = now;
                    itemMaster.StatusFlag = "D";
                    db.SaveChanges();
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("GetItemAverageCost")]
        [HttpGet]
        public List<ItemAverageCostModel> GetItemAverageCost(int itemMasterUID, int? organisationUID)
        {
            List<ItemAverageCostModel> ItemAverageList = (from j in db.ItemAverageCost
                                                          join i in db.HealthOrganisation on j.OwnerOrganisationUID equals i.UID
                                                          where j.StatusFlag == "A"
                                                          && i.StatusFlag == "A"
                                                          && j.ItemMasterUID == itemMasterUID
                                                          && (organisationUID == null || j.OwnerOrganisationUID == organisationUID)
                                                          && j.EndDttm == null
                                                          select new ItemAverageCostModel
                                                          {
                                                              ItemAverageCostUID = j.UID,
                                                              ItemMasterUID = j.ItemMasterUID,
                                                              OwnerOrganisationUID = j.OwnerOrganisationUID,
                                                              OwnerOrganisationName = SqlFunction.fGetHealthOrganisationName(j.OwnerOrganisationUID),
                                                              AvgCost = j.AvgCost,
                                                              BFAvgCost = j.BFAvgCost,
                                                              IMUOMUID = j.IMUOMUID,
                                                              InQty = j.InQty,
                                                              UnitCost = j.UnitCost,
                                                              BFQty = j.BFQty,
                                                              Unit = SqlFunction.fGetRfValDescription(j.IMUOMUID)
                                                          }).ToList();

            return ItemAverageList;
        }

        #endregion

        #region ItemUOMConversion

        [Route("GetItemUOMConversionByItemMasterUID")]
        public List<ItemUOMConversionModel> GetItemUOMConversionByItemMasterUID(int itemMasterUID)
        {
            List<ItemUOMConversionModel> data = db.ItemUOMConversion.Where(p => p.StatusFlag == "A" && p.ItemMasterUID == itemMasterUID)
                .Select(p => new ItemUOMConversionModel
                {
                    ItemUOMConversionUID = p.UID,
                    BaseUOMUID = p.BaseUOMUID,
                    ConversionUOMUID = p.ConversionUOMUID,
                    ConversionValue = p.ConversionValue,
                    StatusFlag = p.StatusFlag
                }).ToList();

            return data;
        }

        [Route("GetItemUOMConversion")]
        [HttpGet]
        public List<ItemUOMConversionModel> GetItemUOMConversion()
        {
            List<ItemUOMConversionModel> data = db.ItemUOMConversion.Where(p => p.StatusFlag == "A").Select(p => new ItemUOMConversionModel()
            {
                ItemMasterUID = p.ItemMasterUID,
                ItemUOMConversionUID = p.UID,
                BaseUOMUID = p.BaseUOMUID,
                ConversionUOMUID = p.ConversionUOMUID,
                ConversionValue = p.ConversionValue,
                CUser = p.CUser,
                CWhen = p.CWhen,
                MUser = p.MUser,
                MWhen = p.MWhen,
                StatusFlag = p.StatusFlag
            }).ToList();

            return data;
        }

        [Route("GetItemUOMConversionByUID")]
        [HttpGet]
        public ItemUOMConversionModel GetItemUOMConversionByUID(int itemMasterUID)
        {
            ItemUOMConversion itemUOMCon = db.ItemUOMConversion.Find(itemMasterUID);
            ItemUOMConversionModel data = null;
            if (itemUOMCon != null)
            {
                data = new ItemUOMConversionModel();
                data.ItemMasterUID = itemUOMCon.ItemMasterUID;
                data.ItemUOMConversionUID = itemUOMCon.UID;
                data.BaseUOMUID = itemUOMCon.BaseUOMUID;
                data.ConversionUOMUID = itemUOMCon.ConversionUOMUID;
                data.ConversionValue = itemUOMCon.ConversionValue;
                data.CUser = itemUOMCon.CUser;
                data.CWhen = itemUOMCon.CWhen;
                data.MUser = itemUOMCon.MUser;
                data.MWhen = itemUOMCon.MWhen;
                data.StatusFlag = itemUOMCon.StatusFlag;
            }

            return data;
        }

        [Route("ManageItemUOMConversion")]
        [HttpPost]
        public HttpResponseMessage ManageItemUOMConversion(ItemUOMConversionModel itemUOMConversionModel, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                ItemUOMConversion itemUOMCon = db.ItemUOMConversion.Find(itemUOMConversionModel.ItemUOMConversionUID);

                if (itemUOMCon == null)
                {
                    itemUOMCon = new ItemUOMConversion();
                    itemUOMCon.CUser = userID;
                    itemUOMCon.CWhen = now;
                }

                itemUOMCon.ItemMasterUID = itemUOMConversionModel.ItemMasterUID;
                itemUOMCon.UID = itemUOMConversionModel.ItemUOMConversionUID;
                itemUOMCon.BaseUOMUID = itemUOMConversionModel.BaseUOMUID;
                itemUOMCon.ConversionUOMUID = itemUOMConversionModel.ConversionUOMUID;
                itemUOMCon.ConversionValue = itemUOMConversionModel.ConversionValue;
                itemUOMCon.MUser = itemUOMConversionModel.MUser;
                itemUOMCon.MWhen = itemUOMConversionModel.MWhen;
                itemUOMCon.StatusFlag = "A";

                db.ItemUOMConversion.AddOrUpdate(itemUOMCon);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("DeleteItemUOMConversion")]
        [HttpDelete]
        public HttpResponseMessage DeleteItemUOMConversion(int itemUOMConversionUID, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                ItemUOMConversion itemUOMCon = db.ItemUOMConversion.Find(itemUOMConversionUID);
                if (itemUOMCon != null)
                {
                    db.ItemUOMConversion.Attach(itemUOMCon);
                    itemUOMCon.MUser = userID;
                    itemUOMCon.MWhen = now;
                    itemUOMCon.StatusFlag = "D";
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

        #region ItemVendorDetail
        [Route("GetItemVendorDetailByItemMasterUID")]
        public List<ItemVendorDetailModel> GetItemVendorDetailByItemMasterUID(int itemMasterUID)
        {
            List<ItemVendorDetailModel> data = db.ItemVendorDetail.Where(p => p.StatusFlag == "A" && p.ItemMasterUID == itemMasterUID)
                .Select(p => new ItemVendorDetailModel
                {
                    ItemVendorDetailUID = p.UID,
                    VendorDetailUID = p.VendorDetailUID,
                    ItemMasterUID = p.ItemMasterUID,
                    ItemAmount = p.ItemAmount,
                    StatusFlag = p.StatusFlag
                }).ToList();

            return data;
        }

        #endregion

        #region StoreUOMConversion

        [Route("GetStoreUOMConversion")]
        [HttpGet]
        public List<StoreUOMConversionModel> GetStoreUOMConversion()
        {
            List<StoreUOMConversionModel> data = db.StoreUOMConversion.Where(p => p.StatusFlag == "A").Select(p => new StoreUOMConversionModel()
            {
                StoreUOMConversionUID = p.UID,
                BaseUOMUID = p.BaseUOMUID,
                BaseUnit = SqlFunction.fGetRfValDescription(p.BaseUOMUID),
                ConversionUnit = SqlFunction.fGetRfValDescription(p.ConversionUOMUID),
                ConversionUOMUID = p.ConversionUOMUID,
                ConversionValue = p.ConversionValue,
                CUser = p.CUser,
                CWhen = p.CWhen,
                MUser = p.MUser,
                MWhen = p.MWhen,
                StatusFlag = p.StatusFlag
            }).ToList();

            return data;
        }

        [Route("GetStoreUOMConversionByUID")]
        [HttpGet]
        public StoreUOMConversionModel GetStoreUOMConversionByUID(int storeUOMConversionUID)
        {
            StoreUOMConversion storeUOM = db.StoreUOMConversion.Find(storeUOMConversionUID);
            StoreUOMConversionModel data = null;
            if (storeUOM != null)
            {
                data = new StoreUOMConversionModel();
                data.StoreUOMConversionUID = storeUOM.UID;
                data.BaseUOMUID = storeUOM.BaseUOMUID;
                data.ConversionUOMUID = storeUOM.ConversionUOMUID;
                data.ConversionValue = storeUOM.ConversionValue;
                data.CUser = storeUOM.CUser;
                data.CWhen = storeUOM.CWhen;
                data.MUser = storeUOM.MUser;
                data.MWhen = storeUOM.MWhen;
                data.StatusFlag = storeUOM.StatusFlag;
            }

            return data;
        }

        [Route("ManageStoreUOMConversion")]
        [HttpPost]
        public HttpResponseMessage ManageStoreUOMConversion(StoreUOMConversionModel storeUOMConversionModel, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                StoreUOMConversion storeUOMCon = db.StoreUOMConversion.Find(storeUOMConversionModel.StoreUOMConversionUID);

                if (storeUOMCon == null)
                {
                    storeUOMCon = new StoreUOMConversion();
                    storeUOMCon.CUser = userID;
                    storeUOMCon.CWhen = now;
                }

                storeUOMCon.UID = storeUOMConversionModel.StoreUOMConversionUID;
                storeUOMCon.BaseUOMUID = storeUOMConversionModel.BaseUOMUID;
                storeUOMCon.ConversionUOMUID = storeUOMConversionModel.ConversionUOMUID;
                storeUOMCon.ConversionValue = storeUOMConversionModel.ConversionValue;
                storeUOMCon.MUser = storeUOMConversionModel.MUser;
                storeUOMCon.MWhen = now;
                storeUOMCon.StatusFlag = "A";

                db.StoreUOMConversion.AddOrUpdate(storeUOMCon);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("DeleteStoreUOMConversion")]
        [HttpDelete]
        public HttpResponseMessage DeleteStoreUOMConversion(int storeUOMConversionUID, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                StoreUOMConversion storeUOMCon = db.StoreUOMConversion.Find(storeUOMConversionUID);
                if (storeUOMCon != null)
                {
                    db.StoreUOMConversion.Attach(storeUOMCon);
                    storeUOMCon.MUser = userID;
                    storeUOMCon.MWhen = now;
                    storeUOMCon.StatusFlag = "D";
                    db.SaveChanges();
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("GetStoreConvertUOM")]
        [HttpGet]
        public List<StoreUOMConversionModel> GetStoreConvertUOM(int itemMasterUID)
        {
            DataTable dataTable = SqlDirectStore.pStoreConvertUOM(itemMasterUID);
            List<StoreUOMConversionModel> data = dataTable.ToList<StoreUOMConversionModel>();
            return data;
        }

        public List<ItemUOMConversionModel> GetItemConvertUOM(int itemMasterUID)
        {
            DataTable dataTable = SqlDirectStore.pItemConvertUOM(itemMasterUID);
            List<ItemUOMConversionModel> data = dataTable.ToList<ItemUOMConversionModel>();
            return data;
        }

        #endregion

        #region Store

        [Route("GetStore")]
        [HttpGet]
        public List<StoreModel> GetStore()
        {
            List<StoreModel> data = db.Store.Where(p => p.StatusFlag == "A").Select(p => new StoreModel()
            {
                StoreUID = p.UID,
                Name = p.Name,
                Description = p.Description,
                STDTPUID = p.STDTPUID,
                StorePolicyType = SqlFunction.fGetRfValDescription(p.STDTPUID ?? 0),
                OwnerOrganisationUID = p.OwnerOrganisationUID,
                OwnerOrganisationName = SqlFunction.fGetHealthOrganisationName(p.OwnerOrganisationUID),
                LocationUID = p.LocationUID ?? 0,
                LocationName = SqlFunction.fGetLocationName(p.LocationUID ?? 0),
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

        [Route("GetStoreByUID")]
        [HttpGet]
        public StoreModel GetStoreByUID(int storeUID)
        {
            StoreModel data = db.Store.Where(p => p.StatusFlag == "A" && p.UID == storeUID).Select(p => new StoreModel()
            {
                StoreUID = p.UID,
                Name = p.Name,
                Description = p.Description,
                STDTPUID = p.STDTPUID,
                StorePolicyType = SqlFunction.fGetRfValDescription(p.STDTPUID ?? 0),
                OwnerOrganisationUID = p.OwnerOrganisationUID,
                OwnerOrganisationName = SqlFunction.fGetHealthOrganisationName(p.OwnerOrganisationUID),
                LocationUID = p.LocationUID ?? 0,
                LocationName = SqlFunction.fGetLocationName(p.LocationUID ?? 0),
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

        [Route("GetStoreByOrganisationUID")]
        [HttpGet]
        public List<StoreModel> GetStoreByOrganisationUID(int organisationUID)
        {
            List<StoreModel> data = db.Store.Where(p => p.StatusFlag == "A" && p.OwnerOrganisationUID == organisationUID).Select(p => new StoreModel()
            {
                StoreUID = p.UID,
                Name = p.Name,
                Description = p.Description,
                STDTPUID = p.STDTPUID,
                StorePolicyType = SqlFunction.fGetRfValDescription(p.STDTPUID ?? 0),
                OwnerOrganisationUID = p.OwnerOrganisationUID,
                OwnerOrganisationName = SqlFunction.fGetHealthOrganisationName(p.OwnerOrganisationUID),
                LocationUID = p.LocationUID ?? 0,
                LocationName = SqlFunction.fGetLocationName(p.LocationUID ?? 0),
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

        [Route("GetStoreByLocationUID")]
        [HttpGet]
        public List<StoreModel> GetStoreByLocationUID(int locationUID)
        {
            List<StoreModel> data = db.Store.Where(p => p.StatusFlag == "A" && p.LocationUID == locationUID).Select(p => new StoreModel()
            {
                StoreUID = p.UID,
                Name = p.Name,
                Description = p.Description,
                STDTPUID = p.STDTPUID,
                StorePolicyType = SqlFunction.fGetRfValDescription(p.STDTPUID ?? 0),
                OwnerOrganisationUID = p.OwnerOrganisationUID,
                OwnerOrganisationName = SqlFunction.fGetHealthOrganisationName(p.OwnerOrganisationUID),
                LocationUID = p.LocationUID ?? 0,
                LocationName = SqlFunction.fGetLocationName(p.LocationUID ?? 0),
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

        [Route("ManageStore")]
        [HttpPost]
        public HttpResponseMessage ManageStore(StoreModel storeModel, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                Store store = db.Store.Find(storeModel.StoreUID);

                if (store == null)
                {
                    store = new Store();
                    store.CUser = userID;
                    store.CWhen = now;
                }

                store.Name = storeModel.Name;
                store.Description = storeModel.Description;
                store.STDTPUID = storeModel.STDTPUID;
                store.OwnerOrganisationUID = storeModel.OwnerOrganisationUID;
                store.LocationUID = storeModel.LocationUID;
                store.ActiveFrom = storeModel.ActiveFrom;
                store.ActiveTo = storeModel.ActiveTo;
                store.MUser = storeModel.MUser;
                store.MWhen = now;
                store.StatusFlag = "A";

                db.Store.AddOrUpdate(store);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("DeleteStore")]
        [HttpDelete]
        public HttpResponseMessage DeleteStore(int storeUID, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                Store store = db.Store.Find(storeUID);
                if (store != null)
                {
                    db.Store.Attach(store);
                    store.MUser = userID;
                    store.MWhen = now;
                    store.StatusFlag = "D";
                    db.SaveChanges();
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("SearchStockOnHand")]
        [HttpGet]
        public List<StockOnHandModel> SearchStockOnHand(int? ownerOrganisationUID, int? locationUID, int? storeUID, int? itemType, string itemCode, string itemName)
        {
            DataTable dt = SqlDirectStore.pSearchStockOnHand(ownerOrganisationUID, locationUID, storeUID, itemType, itemCode, itemName);
            List<StockOnHandModel> data = dt.ToList<StockOnHandModel>();

            return data;
        }

        [Route("SearchStockMovement")]
        [HttpGet]
        public List<StockMovementModel> SearchStockMovement(int? ownerOrganisationUID, int? locationUID, int? storeUID, string itemCode, string itemName, string transactionType, DateTime? dateFrom, DateTime? dateTo)
        {
            DataTable dt = SqlDirectStore.pSearchStockMovement(ownerOrganisationUID, locationUID, storeUID, itemCode, itemName, transactionType, dateFrom, dateTo);
            List<StockMovementModel> data = dt.ToList<StockMovementModel>();

            return data;
        }

        [Route("SearchStockBalance")]
        [HttpGet]
        public List<StockBalanceModel> SearchStockBalance(int? ownerOrganisationUID, int? locationUID, int? storeUID, string itemCode, string itemName, DateTime? dateFrom, DateTime? dateTo)
        {
            DataTable dt = SqlDirectStore.pSearchStockBalance(ownerOrganisationUID, locationUID, storeUID, itemCode, itemName, dateFrom, dateTo);
            List<StockBalanceModel> data = dt.ToList<StockBalanceModel>();
            return data;
        }

        [Route("SearchStockBatchByStoreUID")]
        [HttpGet]
        public List<StockOnHandModel> SearchStockBatchByStoreUID(int storeUID, int itemMasterUID)
        {
            List<StockOnHandModel> data = db.Stock
                .Where(p => p.StatusFlag == "A"
                    && p.StoreUID == storeUID && p.ItemMasterUID == itemMasterUID)
                .Select(p => new StockOnHandModel
                {
                    OrganisationUID = p.OwnerOrganisationUID,
                    OrganisationName = SqlFunction.fGetHealthOrganisationName(p.OwnerOrganisationUID),
                    StockUID = p.UID,
                    StoreUID = p.StoreUID,
                    StoreName = SqlFunction.fGetStoreName(p.StoreUID),
                    ItemMasterUID = p.ItemMasterUID,
                    VendorName = SqlFunction.fGetVendorName(p.VendorDetailUID ?? 0),
                    ItemName = p.ItemName,
                    BatchID = p.BatchID,
                    Unit = SqlFunction.fGetRfValDescription(p.IMUOMUID),
                    Quantity = p.Quantity,
                    IsExpiry = p.IsExpired,
                    ExpiryDttm = p.ExpiryDttm,
                    CWhen = p.CWhen
                }).OrderBy(p => p.ExpiryDttm).ThenBy(p => p.CWhen).ToList();

            return data;
        }

        #endregion

        #region Stock

        [Route("GetStockRemainByItemMasterUID")]
        [HttpGet]
        public List<StockModel> GetStockRemainByItemMasterUID(int itemMasterUID, int organisation)
        {
            DataTable dt = SqlDirectStore.pGetStoreQtyByItemMasterUID(itemMasterUID, organisation);
            List<StockModel> data = dt.ToList<StockModel>();
            return data;
        }

        [Route("GetStockRemainForDispensedByItemMasterUID")]
        [HttpGet]
        public List<StockModel> GetStockRemainForDispensedByItemMasterUID(int itemMasterUID, int organisation)
        {
            DataTable dt = SqlDirectStore.pGetStoreQtyByItemMasterUID(itemMasterUID, organisation);
            List<StockModel> data = dt.ToList<StockModel>();

            if (data != null && data.Count > 0)
            {
                foreach (StockModel item in data)
                {
                    //PrescriptionItem Raised Status
                    var prescriptionItems = db.PrescriptionItem.Where(p => p.ItemMasterUID == itemMasterUID
                    && p.StoreUID == item.StoreUID && p.StatusFlag == "A" && p.ORDSTUID == 2847).ToList();

                    if (prescriptionItems != null && prescriptionItems?.Count > 0)
                    {
                        item.Quantity = item.Quantity - (prescriptionItems.Sum(p => p.Quantity) ?? 0);
                    }

                }
            }

            return data;
        }

        [Route("GetStoreEcounByItemMaster")]
        [HttpGet]
        public List<StockModel> GetStoreEcounByItemMaster(int itemMasterUID, int organisation)
        {
            DataTable dt = SqlDirectStore.pGetStoreEcounByItemMaster(itemMasterUID, organisation);
            List<StockModel> data = dt.ToList<StockModel>();
            return data;
        }


        [Route("SearchStockBatch")]
        [HttpGet]
        public List<StockModel> SearchStockBatch(int? organisationUID, int? locationUID, int? storeUID, int? itemType, string itemCode, string itemName)
        {
            DataTable data = SqlDirectStore.pSearchStockBatch(organisationUID, locationUID, storeUID, itemType, itemCode, itemName);
            List<StockModel> returnData = data.ToList<StockModel>();

            return returnData;
        }



        [Route("GetEcountMassFile")]
        [HttpGet]
        public List<EcountMassFileModel> GetEcountMassFile(int storeUID, int itemMasterUID, string serialNumber, DateTime? expiryDate)
        {
            DataTable data = SqlDirectStore.pStockMassFile(storeUID, itemMasterUID, serialNumber, expiryDate);
            List<EcountMassFileModel> returnData = data.ToList<EcountMassFileModel>();
            return returnData;
        }



        [Route("AdjustStock")]
        [HttpPost]
        public HttpResponseMessage AdjustStock(StockAdjustmentModel model, int userUID)
        {
            try
            {
                DateTime now = DateTime.Now;
                using (var tran = new TransactionScope())
                {

                    int adjustID;
                    string AJID = SEQHelper.GetSEQIDFormat("SEQSTAID", out adjustID);


                    if (string.IsNullOrEmpty(AJID))
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No SEQSTAID in SEQCONFIGURATION");
                    }

                    if (adjustID == 0)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Insert SEQSTAID is Fail");
                    }


                    SqlDirectStore.pInvenAdjustStock(AJID, model.StoreUID, model.StockUID, model.ItemMasterUID, model.ItemName, model.NewBatchID, model.ActualQuantity, model.ActualUOM, model.QuantityAdjusted, model.AdjustedQuantity, model.AdjustedUOM, model.Comments, model.ItemCost ?? 0, model.ExpiryDate, userUID, model.OwnerOrganisationUID);

                    tran.Complete();

                }


                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception er)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, er.Message, er);
            }
        }

        [Route("SearchStockWorkList")]
        [HttpGet]
        public List<StockWorkListModel> SearchStockWorkList(DateTime? dateFrom, DateTime? dateTo, int? organisationUID, int? locationUID)
        {
            DataTable data = SqlDirectStore.pSearchStockWorkList(dateFrom, dateTo, organisationUID, locationUID);
            List<StockWorkListModel> returnData = data.ToList<StockWorkListModel>();

            return returnData;
        }

        [Route("SearchStockForDispose")]
        [HttpGet]
        public List<StockModel> SearchStockForDispose(DateTime? dateFrom, DateTime? dateTo, int storeUID, string batchID, string itemName)
        {
            DataTable data = SqlDirectStore.pSearchStockForDispose(dateFrom, dateTo, storeUID, batchID, itemName);
            List<StockModel> returnData = data.ToList<StockModel>();

            return returnData;
        }

        [Route("DisposeStock")]
        [HttpPost]
        public HttpResponseMessage DisposeStock(List<StockModel> stockModel, int storeUID, int disposeReasonUID, string comments, int userUID)
        {
            try
            {
                DateTime now = DateTime.Now;
                using (var tran = new TransactionScope())
                {

                    int disposeID;
                    string DISID = SEQHelper.GetSEQIDFormat("SEQDISPOSEID", out disposeID);


                    if (string.IsNullOrEmpty(DISID))
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No SEQDISPOSEID in SEQCONFIGURATION");
                    }

                    if (disposeID == 0)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Insert SEQDISPOSEID is Fail");
                    }

                    DisposeStock disposeStock = new DisposeStock();
                    disposeStock.StoreUID = storeUID;
                    disposeStock.DisposeID = DISID;
                    disposeStock.DisposeDttm = now;
                    disposeStock.DSPSTUID = disposeReasonUID;
                    disposeStock.Comments = comments;
                    disposeStock.CUser = userUID;
                    disposeStock.MUser = userUID;
                    disposeStock.CWhen = now;
                    disposeStock.MWhen = now;
                    disposeStock.StatusFlag = "A";
                    db.DisposeStock.Add(disposeStock);

                    db.SaveChanges();

                    foreach (var item in stockModel)
                    {
                        DisposeItemList disposeItem = new DisposeItemList();
                        disposeItem.DisposeStockUID = disposeStock.UID;
                        disposeItem.ItemMasterUID = item.ItemMasterUID;
                        disposeItem.ItemName = item.ItemName;
                        disposeItem.VendorDetailUID = item.VenderDetailUID;
                        disposeItem.BatchID = item.BatchID;
                        disposeItem.StockUID = item.StockUID;
                        disposeItem.AvailableQty = item.Quantity;
                        disposeItem.DisposedQty = item.DisposeQty;
                        disposeItem.IMUOMUID = item.IMUOMUID;
                        disposeItem.ExpiryDttm = item.ExpiryDttm;
                        disposeItem.CUser = userUID;
                        disposeItem.MUser = userUID;
                        disposeItem.CWhen = now;
                        disposeItem.MWhen = now;
                        disposeItem.StatusFlag = "A";
                        db.DisposeItemList.Add(disposeItem);

                        double totalBFQty = SqlStatement.GetItemTotalQuantity(item.ItemMasterUID, item.StoreUID, item.OrganisationUID);

                        Stock stock = db.Stock.Find(item.StockUID);
                        double bfQty = stock.Quantity;
                        if (stock != null)
                        {
                            db.Stock.Attach(stock);
                            stock.MUser = userUID;
                            stock.MWhen = now;
                            stock.Quantity = (disposeItem.AvailableQty - disposeItem.DisposedQty);
                            if (stock.Quantity <= 0)
                            {
                                stock.IsExpired = "Y";
                                stock.DSPSTUID = disposeReasonUID;
                                stock.DisposeComments = comments;
                            }

                        }

                        db.SaveChanges();
                        double totalBalQty = SqlStatement.GetItemTotalQuantity(item.ItemMasterUID, item.StoreUID, item.OrganisationUID);
                        double balQty = stock.Quantity;
                        SqlDirectStore.pInvenInsertStockMovement(item.StockUID, item.StoreUID, null, item.ItemMasterUID, item.BatchID, now, totalBFQty, bfQty, 0, item.DisposeQty, balQty, totalBalQty, stock.IMUOMUID, stock.ItemCost ?? 0, DISID, "DisposeItemList", disposeItem.UID, null, null, userUID);
                    }


                    tran.Complete();

                }


                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception er)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, er.Message, er);
            }
        }
        #endregion

        #region ItemRequest

        [Route("SearchItemRequest")]
        [HttpGet]
        public List<ItemRequestModel> SearchItemRequest(DateTime? dateFrom, DateTime? dateTo, string requestID, int? organisationUID, int? locationUID, int? requestOnOrganistaionUID, int? requestOnLocationUID, int? requestStatus, int? priority)
        {
            List<ItemRequestModel> data = (from j in db.ItemRequest
                                           where j.StatusFlag == "A"
                                           && (dateFrom == null || DbFunctions.TruncateTime(j.RequestedDttm) >= DbFunctions.TruncateTime(dateFrom))
                                           && (dateTo == null || DbFunctions.TruncateTime(j.RequestedDttm) <= DbFunctions.TruncateTime(dateTo))
                                           && (string.IsNullOrEmpty(requestID) || j.ItemRequestID == requestID)
                                           && (organisationUID == null || j.OrganisationUID == organisationUID)
                                               && (locationUID == null || j.LocationUID == locationUID)
                                           && (requestOnOrganistaionUID == null || j.RequestOnOrganistaionUID == requestOnOrganistaionUID)
                                           && (requestOnLocationUID == null || j.RequestOnLocationUID == requestOnLocationUID)
                                           && (requestStatus == null || j.RQSTSUID == requestStatus)
                                           && (priority == null || j.IRPRIUID == priority)
                                           select new ItemRequestModel
                                           {
                                               OrganisationUID = j.OrganisationUID,
                                               StoreUID = j.StoreUID,
                                               LocationUID = j.LocationUID ?? 0,
                                               RequestOnOrganistaionUID = j.RequestOnOrganistaionUID,
                                               RequestOnLocationUID = j.RequestOnLocationUID ?? 0,
                                               RequestOnStoreUID = j.RequestOnStoreUID,
                                               Organisation = SqlFunction.fGetHealthOrganisationName(j.OrganisationUID),
                                               LocationName = SqlFunction.fGetLocationName(j.LocationUID ?? 0),
                                               Store = SqlFunction.fGetStoreName(j.StoreUID),
                                               RequestOnOrganisation = SqlFunction.fGetHealthOrganisationName(j.RequestOnOrganistaionUID),
                                               RequestOnLocationName = SqlFunction.fGetLocationName(j.RequestOnLocationUID ?? 0),
                                               RequestOnStore = SqlFunction.fGetStoreName(j.RequestOnStoreUID),
                                               RQSTSUID = j.RQSTSUID,
                                               RequestStatus = SqlFunction.fGetRfValDescription(j.RQSTSUID ?? 0),
                                               IRPRIUID = j.IRPRIUID,
                                               Priority = SqlFunction.fGetRfValDescription(j.IRPRIUID ?? 0),
                                               RequestedBy = j.RequestedBy,
                                               RequestedByName = SqlFunction.fGetCareProviderName(j.RequestedBy),
                                               PreferredVendorUID = j.PreferredVendorUID,
                                               PreferredVendorName = SqlFunction.fGetVendorName(j.PreferredVendorUID ?? 0),
                                               RequestedDttm = j.RequestedDttm,
                                               ItemRequestID = j.ItemRequestID,
                                               ItemIssueID = j.ItemIssueID,
                                               Comments = j.Comments,
                                               CancelReason = j.CancelReason,
                                               ItemRequestUID = j.UID
                                           }).ToList();

            return data;
        }

        [Route("GetItemRequestByUID")]
        [HttpGet]
        public ItemRequestModel GetItemRequestByUID(int itemRequestUID)
        {
            ItemRequestModel data = (from j in db.ItemRequest
                                     where j.StatusFlag == "A"
                                     && j.UID == itemRequestUID
                                     select new ItemRequestModel
                                     {
                                         OrganisationUID = j.OrganisationUID,
                                         LocationUID = j.LocationUID ?? 0,
                                         StoreUID = j.StoreUID,
                                         RequestOnOrganistaionUID = j.RequestOnOrganistaionUID,
                                         RequestOnLocationUID = j.RequestOnLocationUID ?? 0,
                                         RequestOnStoreUID = j.RequestOnStoreUID,
                                         Organisation = SqlFunction.fGetHealthOrganisationName(j.OrganisationUID),
                                         LocationName = SqlFunction.fGetLocationName(j.LocationUID ?? 0),
                                         Store = SqlFunction.fGetStoreName(j.StoreUID),
                                         RequestOnOrganisation = SqlFunction.fGetHealthOrganisationName(j.RequestOnOrganistaionUID),
                                         RequestOnLocationName = SqlFunction.fGetLocationName(j.RequestOnLocationUID ?? 0),
                                         RequestOnStore = SqlFunction.fGetStoreName(j.RequestOnStoreUID),
                                         RQSTSUID = j.RQSTSUID,
                                         RequestStatus = SqlFunction.fGetRfValDescription(j.RQSTSUID ?? 0),
                                         IRPRIUID = j.IRPRIUID,
                                         Priority = SqlFunction.fGetRfValDescription(j.IRPRIUID ?? 0),
                                         RequestedBy = j.RequestedBy,
                                         RequestedByName = SqlFunction.fGetCareProviderName(j.RequestedBy),
                                         PreferredVendorUID = j.PreferredVendorUID,
                                         PreferredVendorName = SqlFunction.fGetVendorName(j.PreferredVendorUID ?? 0),
                                         RequestedDttm = j.RequestedDttm,
                                         ItemIssueID = j.ItemIssueID,
                                         Comments = j.Comments,
                                         CancelReason = j.CancelReason,
                                         ItemRequestID = j.ItemRequestID,
                                         ItemRequestUID = j.UID
                                     }).FirstOrDefault();

            if (data != null)
            {
                data.ItemRequestDetail = GetItemRequestDetailByItemRequestUID(data.ItemRequestUID);
                //data.ItemRequestDetail = db.ItemRequestDetail
                //    .Where(p => p.StatusFlag == "A" && p.ItemRequestUID == data.ItemRequestUID)
                //    .Select(p => new ItemRequestDetailModel
                //    {
                //        ItemRequestDetailUID = p.UID,
                //        ItemRequestUID = p.ItemRequestUID,
                //        ItemMasterUID = p.ItemMasterUID,
                //        ItemCode = p.ItemCode,
                //        ItemName = p.ItemName,
                //        Quantity = p.Quantity,
                //        IMUOMUID = p.IMUOMUID,
                //        CurrentQuantity = p.CurrentQuantity
                //    }).ToList();
            }

            return data;
        }

        [Route("GetItemRequestDetailByItemRequestUID")]
        [HttpGet]
        public List<ItemRequestDetailModel> GetItemRequestDetailByItemRequestUID(int itemRequestUID)
        {
            List<ItemRequestDetailModel> data = db.ItemRequestDetail
                    .Where(p => p.StatusFlag == "A" && p.ItemRequestUID == itemRequestUID)
                    .Select(p => new ItemRequestDetailModel
                    {
                        ItemRequestDetailUID = p.UID,
                        ItemRequestUID = p.ItemRequestUID,
                        ItemCode = p.ItemCode,
                        ItemMasterUID = p.ItemMasterUID,
                        ItemName = p.ItemName,
                        Quantity = p.Quantity,
                        IMUOMUID = p.IMUOMUID,
                        Unit = SqlFunction.fGetRfValDescription(p.IMUOMUID),
                        CurrentQuantity = p.CurrentQuantity
                    }).ToList();

            return data;
        }

        [Route("CancelItemRequest")]
        [HttpPut]
        public HttpResponseMessage CancelItemRequest(int itemRequestUID, string cancelReason, int userUID)
        {
            try
            {
                DateTime now = DateTime.Now;
                ItemRequest itemRequest = db.ItemRequest.Find(itemRequestUID);
                if (itemRequest != null)
                {
                    db.ItemRequest.Attach(itemRequest);
                    itemRequest.MUser = userUID;
                    itemRequest.MWhen = now;
                    itemRequest.RQSTSUID = 2929;
                    itemRequest.CancelReason = cancelReason;
                    db.SaveChanges();
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }

        }

        [Route("ManageItemRequest")]
        [HttpPost]
        public HttpResponseMessage ManageItemRequest(ItemRequestModel model, int userUID)
        {
            try
            {
                DateTime now = DateTime.Now;

                using (var tran = new TransactionScope())
                {
                    ItemRequest itemREQ = db.ItemRequest.Find(model.ItemRequestUID);
                    if (itemREQ == null)
                    {
                        itemREQ = new ItemRequest();
                        itemREQ.CUser = userUID;
                        itemREQ.CWhen = now;

                        int itmREQID;
                        string REID = SEQHelper.GetSEQIDFormat("SEQItemRequest", out itmREQID);


                        if (string.IsNullOrEmpty(REID))
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No SEQItemRequest in SEQCONFIGURATION");
                        }

                        if (itmREQID == 0)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Insert SEQItemRequest is Fail");
                        }

                        itemREQ.ItemRequestID = REID;
                    }

                    itemREQ.MUser = userUID;
                    itemREQ.MWhen = now;
                    itemREQ.OrganisationUID = model.OrganisationUID;
                    itemREQ.LocationUID = model.LocationUID;
                    itemREQ.StoreUID = model.StoreUID;
                    itemREQ.RequestOnOrganistaionUID = model.RequestOnOrganistaionUID;
                    itemREQ.RequestOnLocationUID = model.RequestOnLocationUID;
                    itemREQ.RequestOnStoreUID = model.RequestOnStoreUID;
                    itemREQ.RQSTSUID = model.RQSTSUID;
                    itemREQ.RequestedBy = userUID;
                    itemREQ.PreferredVendorUID = model.PreferredVendorUID;
                    itemREQ.RequestedDttm = model.RequestedDttm;
                    itemREQ.IRPRIUID = model.IRPRIUID;
                    itemREQ.Comments = model.Comments;
                    itemREQ.StatusFlag = "A";
                    db.ItemRequest.AddOrUpdate(itemREQ);

                    db.SaveChanges();


                    #region DeleteItemRequestDetail

                    List<ItemRequestDetail> itemREQDE = db.ItemRequestDetail.Where(p => p.ItemRequestUID == model.ItemRequestUID && p.StatusFlag == "A").ToList();

                    foreach (var item in itemREQDE)
                    {
                        var data = model.ItemRequestDetail.FirstOrDefault(p => p.ItemRequestDetailUID == item.UID);
                        if (data == null)
                        {
                            db.ItemRequestDetail.Attach(item);
                            item.MUser = userUID;
                            item.MWhen = now;
                            item.StatusFlag = "D";

                            db.SaveChanges();
                        }

                    }


                    #endregion

                    #region SaveItemRequestDetail
                    foreach (var item in model.ItemRequestDetail)
                    {
                        ItemRequestDetail itemRequestDetail = db.ItemRequestDetail.Find(item.ItemRequestDetailUID);
                        if (itemRequestDetail == null)
                        {
                            itemRequestDetail = new ItemRequestDetail();
                            itemRequestDetail.CUser = userUID;
                            itemRequestDetail.CWhen = now;
                        }
                        itemRequestDetail.ItemRequestUID = itemREQ.UID;
                        itemRequestDetail.ItemMasterUID = item.ItemMasterUID;
                        itemRequestDetail.ItemCode = item.ItemCode;
                        itemRequestDetail.ItemName = item.ItemName;
                        itemRequestDetail.Quantity = item.Quantity;
                        itemRequestDetail.IMUOMUID = item.IMUOMUID;
                        DataTable dt = SqlDirectStore.pGetStoreQtyByItemMasterUID(item.ItemMasterUID, itemREQ.OrganisationUID);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            double sumQuantity = dt.AsEnumerable().Sum(p => double.Parse(p["Quantity"].ToString() == "" ? "0" : p["Quantity"].ToString()));
                            itemRequestDetail.CurrentQuantity = sumQuantity;
                        }
                        else
                        {
                            itemRequestDetail.CurrentQuantity = 0;
                        }
                        itemRequestDetail.MUser = userUID;
                        itemRequestDetail.MWhen = now;
                        itemRequestDetail.StatusFlag = "A";
                        db.ItemRequestDetail.AddOrUpdate(itemRequestDetail);

                        db.SaveChanges();
                    }



                    #endregion

                    tran.Complete();
                }



                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        #endregion

        #region ItemIssue

        [Route("SearchItemIssue")]
        [HttpGet]
        public List<ItemIssueModel> SearchItemIssue(DateTime? dateFrom, DateTime? dateTo, string issueID, int ISSTPUID, int? issueStatus, int? organisationUID, int? locationUID, int? organisationToUID, int? locationToUID)
        {
            List<ItemIssueModel> data = (from j in db.ItemIssue
                                         where j.StatusFlag == "A"
                                         && j.ISSTPUID == ISSTPUID
                                         && (issueStatus == null || j.ISUSTUID == issueStatus)
                                         && (dateFrom == null || DbFunctions.TruncateTime(j.ItemIssueDttm) >= DbFunctions.TruncateTime(dateFrom))
                                         && (dateTo == null || DbFunctions.TruncateTime(j.ItemIssueDttm) <= DbFunctions.TruncateTime(dateTo))
                                         && (string.IsNullOrEmpty(issueID) || j.ItemIssueID == issueID)
                                         && (organisationUID == null || j.OrganisationUID == organisationUID)
                                         && (locationUID == null || j.LocationUID == locationUID)
                                         && (organisationToUID == null || j.RequestedByOrganisationUID == organisationToUID)
                                         && (locationToUID == null || j.RequestedByLocationUID == locationToUID)
                                         select new ItemIssueModel
                                         {
                                             OrganisationUID = j.OrganisationUID,
                                             LocationUID = j.LocationUID ?? 0,
                                             LocationName = SqlFunction.fGetLocationName(j.LocationUID ?? 0),
                                             StoreUID = j.StoreUID,
                                             RequestedByOrganisationUID = j.RequestedByOrganisationUID,
                                             RequestedByLocationName = SqlFunction.fGetLocationName(j.RequestedByLocationUID ?? 0),
                                             RequestedByLocationUID = j.RequestedByLocationUID ?? 0,
                                             RequestedByStoreUID = j.RequestedByStoreUID,
                                             Organisation = SqlFunction.fGetHealthOrganisationName(j.OrganisationUID),
                                             Store = SqlFunction.fGetStoreName(j.StoreUID),
                                             RequestedByOrganisation = SqlFunction.fGetHealthOrganisationName(j.RequestedByOrganisationUID),
                                             RequestedByStore = SqlFunction.fGetStoreName(j.RequestedByStoreUID),
                                             ItemIssueDttm = j.ItemIssueDttm,
                                             ItemIssueID = j.ItemIssueID,
                                             ISUSTUID = j.ISUSTUID,
                                             IssueType = SqlFunction.fGetRfValDescription(j.ISSTPUID ?? 0),
                                             IssueStatus = SqlFunction.fGetRfValDescription(j.ISUSTUID ?? 0),
                                             IssueBy = j.IssueBy,
                                             IssueByName = SqlFunction.fGetCareProviderName(j.IssueBy),
                                             Comments = j.Comments,
                                             ItemRequestID = j.ItemRequestID,
                                             ItemReceiveID = j.ItemReceiveID,
                                             ItemRequestUID = j.ItemRequestUID,
                                             ItemReceiveUID = j.ItemReceiveUID,
                                             CancelReason = j.CancelReason,
                                             ISSTPUID = j.ISSTPUID,
                                             ItemIssueUID = j.UID,
                                             NetAmount = j.NetAmount,
                                             OtherCharges = j.OtherCharges
                                         }).ToList();

            return data;
        }

        [Route("SearchItemIssueForListIssue")]
        [HttpGet]
        public List<ItemIssueModel> SearchItemIssueForListIssue(DateTime? dateFrom, DateTime? dateTo, string issueID, int? issueStatus, int? organisationUID, int? locationUID, int? organisationToUID, int? locationToUID)
        {
            int issue = 2917;
            int consumption = 2921;
            List<ItemIssueModel> data = (from j in db.ItemIssue
                                         where j.StatusFlag == "A"
                                         && (j.ISSTPUID == issue || j.ISSTPUID == consumption)
                                         && (issueStatus == null || j.ISUSTUID == issueStatus)
                                         && (dateFrom == null || DbFunctions.TruncateTime(j.ItemIssueDttm) >= DbFunctions.TruncateTime(dateFrom))
                                         && (dateTo == null || DbFunctions.TruncateTime(j.ItemIssueDttm) <= DbFunctions.TruncateTime(dateTo))
                                         && (string.IsNullOrEmpty(issueID) || j.ItemIssueID == issueID)
                                         && (organisationUID == null || j.OrganisationUID == organisationUID)
                                         && (locationUID == null || j.LocationUID == locationUID)
                                         && (organisationToUID == null || j.RequestedByOrganisationUID == organisationToUID)
                                         && (locationToUID == null || j.RequestedByLocationUID == locationToUID)
                                         select new ItemIssueModel
                                         {
                                             OrganisationUID = j.OrganisationUID,
                                             LocationUID = j.LocationUID ?? 0,
                                             LocationName = SqlFunction.fGetLocationName(j.LocationUID ?? 0),
                                             StoreUID = j.StoreUID,
                                             RequestedByOrganisationUID = j.RequestedByOrganisationUID,
                                             RequestedByLocationName = SqlFunction.fGetLocationName(j.RequestedByLocationUID ?? 0),
                                             RequestedByLocationUID = j.RequestedByLocationUID ?? 0,
                                             RequestedByStoreUID = j.RequestedByStoreUID,
                                             Organisation = SqlFunction.fGetHealthOrganisationName(j.OrganisationUID),
                                             Store = SqlFunction.fGetStoreName(j.StoreUID),
                                             RequestedByOrganisation = SqlFunction.fGetHealthOrganisationName(j.RequestedByOrganisationUID),
                                             RequestedByStore = SqlFunction.fGetStoreName(j.RequestedByStoreUID),
                                             ItemIssueDttm = j.ItemIssueDttm,
                                             ItemIssueID = j.ItemIssueID,
                                             ISUSTUID = j.ISUSTUID,
                                             IssueType = SqlFunction.fGetRfValDescription(j.ISSTPUID ?? 0),
                                             IssueStatus = SqlFunction.fGetRfValDescription(j.ISUSTUID ?? 0),
                                             IssueBy = j.IssueBy,
                                             IssueByName = SqlFunction.fGetCareProviderName(j.IssueBy),
                                             Comments = j.Comments,
                                             ItemRequestID = j.ItemRequestID,
                                             ItemReceiveID = j.ItemReceiveID,
                                             ItemRequestUID = j.ItemRequestUID,
                                             ItemReceiveUID = j.ItemReceiveUID,
                                             CancelReason = j.CancelReason,
                                             ISSTPUID = j.ISSTPUID,
                                             ItemIssueUID = j.UID,
                                             NetAmount = j.NetAmount,
                                             OtherCharges = j.OtherCharges
                                         }).ToList();

            return data;
        }

        [Route("GetItemIssueByUID")]
        [HttpGet]
        public ItemIssueModel GetItemIssueByUID(int itemIssueUID)
        {
            ItemIssueModel data = (from j in db.ItemIssue
                                   where j.StatusFlag == "A"
                                   && j.UID == itemIssueUID
                                   select new ItemIssueModel
                                   {
                                       OrganisationUID = j.OrganisationUID,
                                       LocationUID = j.LocationUID ?? 0,
                                       StoreUID = j.StoreUID,
                                       RequestedByOrganisationUID = j.RequestedByOrganisationUID,
                                       RequestedByLocationUID = j.RequestedByLocationUID ?? 0,
                                       RequestedByStoreUID = j.RequestedByStoreUID,
                                       Organisation = SqlFunction.fGetHealthOrganisationName(j.OrganisationUID),
                                       Store = SqlFunction.fGetStoreName(j.StoreUID),
                                       RequestedByOrganisation = SqlFunction.fGetHealthOrganisationName(j.RequestedByOrganisationUID),
                                       RequestedByStore = SqlFunction.fGetStoreName(j.RequestedByStoreUID),
                                       OtherCharges = j.OtherCharges,
                                       NetAmount = j.NetAmount,
                                       ItemIssueDttm = j.ItemIssueDttm,
                                       ItemIssueID = j.ItemIssueID,
                                       ISUSTUID = j.ISUSTUID,
                                       IssueStatus = SqlFunction.fGetRfValDescription(j.ISUSTUID ?? 0),
                                       IssueBy = j.IssueBy,
                                       IssueByName = SqlFunction.fGetCareProviderName(j.IssueBy),
                                       Comments = j.Comments,
                                       CancelReason = j.CancelReason,
                                       ItemRequestID = j.ItemRequestID,
                                       ItemReceiveID = j.ItemReceiveID,
                                       ItemRequestUID = j.ItemRequestUID,
                                       ItemReceiveUID = j.ItemReceiveUID,
                                       ISSTPUID = j.ISSTPUID,
                                       ItemIssueUID = j.UID
                                   }).FirstOrDefault();

            if (data != null)
            {
                data.ItemIssueDetail = GetItemIssueDetailByItemIssueUID(data.ItemIssueUID);
            }

            return data;
        }


        [Route("GetItemIssueDetailByItemIssueUID")]
        [HttpGet]
        public List<ItemIssueDetailModel> GetItemIssueDetailByItemIssueUID(int itemIssueUID)
        {
            List<ItemIssueDetailModel> data = db.ItemIssueDetail
                    .Where(p => p.StatusFlag == "A" && p.ItemIssueUID == itemIssueUID)
                    .Select(p => new ItemIssueDetailModel
                    {
                        ItemIssueDetailUID = p.UID,
                        ItemIssueUID = p.ItemIssueUID,
                        ItemMasterUID = p.ItemMasterUID,
                        ItemCode = p.ItemCode,
                        ItemName = p.ItemName,
                        UnitPrice = p.UnitPrice,
                        NetAmount = p.NetAmount,
                        ItemCost = p.ItemCost,
                        Quantity = p.Quantity,
                        IMUOMUID = p.IMUOMUID,
                        Unit = SqlFunction.fGetRfValDescription(p.IMUOMUID),
                        BatchID = p.BatchID,
                        StockUID = p.StockUID,
                        ExpiryDttm = p.ExpiryDttm
                    }).ToList();

            return data;
        }


        [Route("CancelItemIssue")]
        [HttpPut]
        public HttpResponseMessage CancelItemIssue(int itemIssueUID, string cancelReason, int userUID)
        {
            try
            {
                //SqlDirectStore.pInvenCancelItemIssue(itemIssueUID, cancelReason, userUID);
                ItemIssue itemIssue = db.ItemIssue.Find(itemIssueUID);
                if (itemIssue != null)
                {
                    DateTime dateNow = DateTime.Now;
                    db.ItemIssue.Attach(itemIssue);
                    itemIssue.CancelReason = cancelReason;
                    itemIssue.ISUSTUID = 2916;
                    itemIssue.MUser = userUID;
                    itemIssue.MWhen = dateNow;

                    ItemRequest itemRequest = db.ItemRequest.Find(itemIssue.ItemRequestUID);
                    if (itemRequest != null)
                    {
                        db.ItemRequest.Attach(itemRequest);
                        itemRequest.RQSTSUID = 2926;
                        itemRequest.MUser = userUID;
                        itemRequest.MWhen = dateNow;
                    }

                    SqlDirectStore.pInvenClearItemIssueDetail(itemIssue.UID, userUID);

                    db.SaveChanges();
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }

        }


        [Route("ManageItemIssue")]
        [HttpPost]
        public HttpResponseMessage ManageItemIssue(ItemIssueModel model, int userUID)
        {
            try
            {
                DateTime now = DateTime.Now;

                using (var tran = new TransactionScope())
                {
                    ItemIssue itemIssue = db.ItemIssue.Find(model.ItemIssueUID);
                    if (itemIssue == null)
                    {
                        itemIssue = new ItemIssue();
                        itemIssue.CUser = userUID;
                        itemIssue.CWhen = now;

                        int itmISSID;
                        string ISSID = SEQHelper.GetSEQIDFormat("SEQItemIssue", out itmISSID);


                        if (string.IsNullOrEmpty(ISSID))
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No SEQItemIssue in SEQCONFIGURATION");
                        }

                        if (itmISSID == 0)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Insert SEQItemIssue is Fail");
                        }

                        itemIssue.ItemIssueID = ISSID;
                    }
                    itemIssue.MUser = userUID;
                    itemIssue.MWhen = now;
                    itemIssue.OrganisationUID = model.OrganisationUID;
                    itemIssue.LocationUID = model.LocationUID;
                    itemIssue.StoreUID = model.StoreUID;
                    itemIssue.RequestedByOrganisationUID = model.RequestedByOrganisationUID;
                    itemIssue.RequestedByLocationUID = model.RequestedByLocationUID;
                    itemIssue.RequestedByStoreUID = model.RequestedByStoreUID;
                    itemIssue.ItemIssueDttm = model.ItemIssueDttm;
                    itemIssue.ISSTPUID = 2917;
                    itemIssue.ISUSTUID = 2913;
                    itemIssue.IssueBy = userUID;
                    itemIssue.Comments = model.Comments;
                    itemIssue.NetAmount = model.NetAmount;
                    itemIssue.OtherCharges = model.OtherCharges;
                    itemIssue.ItemRequestID = model.ItemRequestID;
                    itemIssue.ItemReceiveID = model.ItemReceiveID;
                    itemIssue.ItemRequestUID = model.ItemRequestUID;
                    itemIssue.ItemReceiveUID = model.ItemReceiveUID;
                    itemIssue.StatusFlag = "A";
                    db.ItemIssue.AddOrUpdate(itemIssue);
                    db.SaveChanges();

                    //#region DeleteItemIssueDetail

                    //List<ItemIssueDetail> itemISSDE = db.ItemIssueDetail.Where(p => p.ItemIssueUID == model.ItemIssueUID && p.StatusFlag == "A").ToList();

                    //foreach (var item in itemISSDE)
                    //{
                    //    var data = model.ItemIssueDetail.FirstOrDefault(p => p.ItemIssueDetailUID == item.UID);
                    //    if (data == null)
                    //    {
                    //        db.ItemIssueDetail.Attach(item);
                    //        item.MUser = userUID;
                    //        item.MWhen = now;
                    //        item.StatusFlag = "D";

                    //        db.SaveChanges();
                    //    }

                    //}

                    //#endregion


                    #region SaveItemIssueDetail
                    foreach (var item in model.ItemIssueDetail)
                    {
                        ItemIssueDetail itemIssueDetail = db.ItemIssueDetail.Find(item.ItemIssueDetailUID);
                        if (itemIssueDetail == null)
                        {
                            itemIssueDetail = new ItemIssueDetail();
                            itemIssueDetail.CUser = userUID;
                            itemIssueDetail.CWhen = now;
                        }
                        itemIssueDetail.ItemIssueUID = itemIssue.UID;
                        itemIssueDetail.ItemMasterUID = item.ItemMasterUID;
                        itemIssueDetail.ItemCode = item.ItemCode;
                        itemIssueDetail.ItemName = item.ItemName;
                        itemIssueDetail.UnitPrice = item.UnitPrice;
                        itemIssueDetail.NetAmount = item.NetAmount;
                        itemIssueDetail.ItemCost = item.ItemCost;
                        itemIssueDetail.Quantity = item.Quantity;
                        itemIssueDetail.IMUOMUID = item.IMUOMUID;
                        itemIssueDetail.BatchID = item.BatchID;
                        itemIssueDetail.StockUID = item.StockUID;
                        itemIssueDetail.ExpiryDttm = item.ExpiryDttm;
                        itemIssueDetail.MUser = userUID;
                        itemIssueDetail.MWhen = now;
                        itemIssueDetail.StatusFlag = "A";
                        itemIssueDetail.SerialNumber = item.SerialNumber;
                        db.ItemIssueDetail.AddOrUpdate(itemIssueDetail);

                        db.SaveChanges();
                    }


                    ItemRequest itemRequest = db.ItemRequest.Find(itemIssue.ItemRequestUID);
                    if (itemRequest != null)
                    {
                        db.ItemRequest.Attach(itemRequest);
                        itemRequest.MWhen = now;
                        itemRequest.MUser = userUID;
                        itemRequest.ItemIssueID = itemIssue.ItemIssueID;
                        itemRequest.RQSTSUID = 2927;
                        db.SaveChanges();
                    }

                    SqlDirectStore.pInvenIssueItem(itemIssue.UID, userUID);
                    #endregion

                    tran.Complete();
                }



                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }


        [Route("ManageItemIssueEount")]
        [HttpPost]
        public HttpResponseMessage ManageItemIssueEcount(ItemIssueModel model, int userUID)
        {
            try
            {
                DateTime now = DateTime.Now;

                using (var tran = new TransactionScope())
                {
                    ItemIssue itemIssue = db.ItemIssue.Find(model.ItemIssueUID);
                    if (itemIssue == null)
                    {
                        itemIssue = new ItemIssue();
                        itemIssue.CUser = userUID;
                        itemIssue.CWhen = now;

                        int itmISSID;
                        string ISSID = SEQHelper.GetSEQIDFormat("SEQItemIssue", out itmISSID);


                        if (string.IsNullOrEmpty(ISSID))
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No SEQItemIssue in SEQCONFIGURATION");
                        }

                        if (itmISSID == 0)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Insert SEQItemIssue is Fail");
                        }

                        itemIssue.ItemIssueID = ISSID;
                    }
                    itemIssue.MUser = userUID;
                    itemIssue.MWhen = now;
                    itemIssue.OrganisationUID = model.OrganisationUID;
                    itemIssue.StoreUID = model.StoreUID;
                    itemIssue.RequestedByOrganisationUID = model.RequestedByOrganisationUID;
                    itemIssue.RequestedByStoreUID = model.RequestedByStoreUID;
                    itemIssue.ItemIssueDttm = model.ItemIssueDttm;
                    itemIssue.ISSTPUID = 2917;
                    itemIssue.ISUSTUID = 2913;
                    itemIssue.IssueBy = userUID;
                    itemIssue.Comments = model.Comments;
                    itemIssue.NetAmount = model.NetAmount;
                    itemIssue.OtherCharges = model.OtherCharges;
                    itemIssue.ItemRequestID = model.ItemRequestID;
                    itemIssue.ItemReceiveID = model.ItemReceiveID;
                    itemIssue.ItemRequestUID = model.ItemRequestUID;
                    itemIssue.ItemReceiveUID = model.ItemReceiveUID;
                    itemIssue.StatusFlag = "A";
                    db.ItemIssue.AddOrUpdate(itemIssue);
                    db.SaveChanges();



                    #region SaveItemIssueDetail
                    foreach (var item in model.ItemIssueDetail)
                    {
                        ItemIssueDetail itemIssueDetail = db.ItemIssueDetail.Find(item.ItemIssueDetailUID);
                        if (itemIssueDetail == null)
                        {
                            itemIssueDetail = new ItemIssueDetail();
                            itemIssueDetail.CUser = userUID;
                            itemIssueDetail.CWhen = now;
                        }
                        itemIssueDetail.ItemIssueUID = itemIssue.UID;
                        itemIssueDetail.ItemMasterUID = item.ItemMasterUID;
                        itemIssueDetail.ItemCode = item.ItemCode;
                        itemIssueDetail.ItemName = item.ItemName;
                        itemIssueDetail.UnitPrice = item.UnitPrice;
                        itemIssueDetail.NetAmount = item.NetAmount;
                        itemIssueDetail.ItemCost = item.ItemCost;
                        itemIssueDetail.Quantity = item.Quantity;
                        itemIssueDetail.IMUOMUID = item.IMUOMUID;
                        itemIssueDetail.BatchID = item.BatchID;
                        itemIssueDetail.StockUID = item.StockUID;
                        itemIssueDetail.ExpiryDttm = item.ExpiryDttm;
                        itemIssueDetail.MUser = userUID;
                        itemIssueDetail.MWhen = now;
                        itemIssueDetail.StatusFlag = "A";
                        itemIssueDetail.SerialNumber = item.SerialNumber;
                        db.ItemIssueDetail.AddOrUpdate(itemIssueDetail);

                        db.SaveChanges();
                    }


                    ItemRequest itemRequest = db.ItemRequest.Find(itemIssue.ItemRequestUID);
                    if (itemRequest != null)
                    {
                        db.ItemRequest.Attach(itemRequest);
                        itemRequest.MWhen = now;
                        itemRequest.MUser = userUID;
                        itemRequest.ItemIssueID = itemIssue.ItemIssueID;
                        itemRequest.RQSTSUID = 2927;
                        db.SaveChanges();
                    }

                    SqlDirectStore.pInvenIssueItemEcount(itemIssue.UID, userUID);
                    #endregion

                    tran.Complete();
                }



                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }


        [Route("ConsumptionItem")]
        [HttpPost]
        public HttpResponseMessage ConsumptionItem(IEnumerable<ItemIssueDetailModel> itemIssueDetailsModel, string comments, int userUID)
        {
            try
            {
                DateTime now = DateTime.Now;
                var itemIssueGroupStore = itemIssueDetailsModel
                    .GroupBy(p => new { p.OrganisationUID, p.StoreUID })
                    .Select(p => new
                    {
                        StoreUID = p.Key.StoreUID,
                        OrganisationUID = p.Key.OrganisationUID,
                        NetAmount = p.Sum(x => x.ItemCost * x.Quantity)
                    });

                var referenValue = db.ReferenceValue.Where(p => (p.DomainCode == "ISSTP" || p.DomainCode == "ISUST") && p.StatusFlag == "A");
                foreach (var itemIssue in itemIssueGroupStore)
                {
                    using (var tran = new TransactionScope())
                    {
                        int itmISSID;
                        string ISSID = SEQHelper.GetSEQIDFormat("SEQItemIssue", out itmISSID);


                        if (string.IsNullOrEmpty(ISSID))
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No SEQItemIssue in SEQCONFIGURATION");
                        }

                        if (itmISSID == 0)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Insert SEQItemIssue is Fail");
                        }



                        ItemIssue newItemIssue = new ItemIssue();
                        newItemIssue.ItemIssueID = ISSID;
                        newItemIssue.OrganisationUID = itemIssue.OrganisationUID;
                        newItemIssue.StoreUID = itemIssue.StoreUID;
                        newItemIssue.ItemIssueDttm = now;
                        newItemIssue.ISUSTUID = referenValue.FirstOrDefault(p => p.DomainCode == "ISUST" && p.ValueCode == "ACCPT").UID; //Accepted
                        newItemIssue.ISSTPUID = referenValue.FirstOrDefault(p => p.DomainCode == "ISSTP" && p.ValueCode == "IMCNS").UID; //Consumption
                        newItemIssue.NetAmount = itemIssue.NetAmount;
                        newItemIssue.Comments = comments;
                        newItemIssue.IssueBy = userUID;
                        newItemIssue.MUser = userUID;
                        newItemIssue.CUser = userUID;
                        newItemIssue.MWhen = now;
                        newItemIssue.CWhen = now;
                        newItemIssue.StatusFlag = "A";
                        db.ItemIssue.Add(newItemIssue);

                        db.SaveChanges();

                        var itemIssueDetails = itemIssueDetailsModel.Where(p => p.OrganisationUID == itemIssue.OrganisationUID && p.StoreUID == itemIssue.StoreUID);
                        foreach (var itemIssueDetail in itemIssueDetails)
                        {
                            ItemIssueDetail newIssueDetail = new ItemIssueDetail();
                            newIssueDetail.ItemIssueUID = newItemIssue.UID;
                            newIssueDetail.ItemMasterUID = itemIssueDetail.ItemMasterUID;
                            newIssueDetail.ItemCode = itemIssueDetail.ItemCode;
                            newIssueDetail.ItemName = itemIssueDetail.ItemName;
                            newIssueDetail.ItemCost = itemIssueDetail.ItemCost;
                            newIssueDetail.UnitPrice = itemIssueDetail.ItemCost;
                            newIssueDetail.Quantity = itemIssueDetail.Quantity;
                            newIssueDetail.NetAmount = itemIssueDetail.Quantity * itemIssueDetail.ItemCost;
                            newIssueDetail.IMUOMUID = itemIssueDetail.IMUOMUID;
                            newIssueDetail.BatchID = itemIssueDetail.BatchID;
                            newIssueDetail.StockUID = itemIssueDetail.StockUID;
                            newIssueDetail.ExpiryDttm = itemIssueDetail.ExpiryDttm;
                            newIssueDetail.LocationUID = itemIssueDetail.LocationUID;
                            newIssueDetail.MUser = userUID;
                            newIssueDetail.CUser = userUID;
                            newIssueDetail.MWhen = now;
                            newIssueDetail.CWhen = now;
                            newIssueDetail.StatusFlag = "A";

                            db.ItemIssueDetail.Add(newIssueDetail);
                        }

                        db.SaveChanges();


                        SqlDirectStore.pInvenIssueItem(newItemIssue.UID, userUID);

                        tran.Complete();
                    }



                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }



        #endregion

        #region ItemTransfer


        [Route("SearchItemTranfer")]
        [HttpGet]
        public List<ItemIssueModel> SearchItemTranfer(DateTime? dateFrom, DateTime? dateTo, string issueID, int? issueStatus, int? organisationUID, int? locationUID, int? organisationToUID, int? locationToUID)
        {
            int tranfer = 2918;
            List<ItemIssueModel> data = (from j in db.ItemIssue
                                         where j.StatusFlag == "A"
                                         && (j.ISSTPUID == tranfer)
                                         && (issueStatus == null || j.ISUSTUID == issueStatus)
                                         && (dateFrom == null || DbFunctions.TruncateTime(j.ItemIssueDttm) >= DbFunctions.TruncateTime(dateFrom))
                                         && (dateTo == null || DbFunctions.TruncateTime(j.ItemIssueDttm) <= DbFunctions.TruncateTime(dateTo))
                                         && (string.IsNullOrEmpty(issueID) || j.ItemIssueID == issueID)
                                         && (organisationUID == null || j.OrganisationUID == organisationUID)
                                         && (locationUID == null || j.LocationUID == locationUID)
                                         && (organisationToUID == null || j.RequestedByOrganisationUID == organisationToUID)
                                         && (locationToUID == null || j.RequestedByLocationUID == locationToUID)
                                         select new ItemIssueModel
                                         {
                                             OrganisationUID = j.OrganisationUID,
                                             LocationUID = j.LocationUID ?? 0,
                                             LocationName = SqlFunction.fGetLocationName(j.LocationUID ?? 0),
                                             StoreUID = j.StoreUID,
                                             RequestedByOrganisationUID = j.RequestedByOrganisationUID,
                                             RequestedByLocationName = SqlFunction.fGetLocationName(j.RequestedByLocationUID ?? 0),
                                             RequestedByLocationUID = j.RequestedByLocationUID ?? 0,
                                             RequestedByStoreUID = j.RequestedByStoreUID,
                                             Organisation = SqlFunction.fGetHealthOrganisationName(j.OrganisationUID),
                                             Store = SqlFunction.fGetStoreName(j.StoreUID),
                                             RequestedByOrganisation = SqlFunction.fGetHealthOrganisationName(j.RequestedByOrganisationUID),
                                             RequestedByStore = SqlFunction.fGetStoreName(j.RequestedByStoreUID),
                                             ItemIssueDttm = j.ItemIssueDttm,
                                             ItemIssueID = j.ItemIssueID,
                                             ISUSTUID = j.ISUSTUID,
                                             IssueType = SqlFunction.fGetRfValDescription(j.ISSTPUID ?? 0),
                                             IssueStatus = SqlFunction.fGetRfValDescription(j.ISUSTUID ?? 0),
                                             IssueBy = j.IssueBy,
                                             IssueByName = SqlFunction.fGetCareProviderName(j.IssueBy),
                                             Comments = j.Comments,
                                             ItemRequestID = j.ItemRequestID,
                                             ItemReceiveID = j.ItemReceiveID,
                                             ItemRequestUID = j.ItemRequestUID,
                                             ItemReceiveUID = j.ItemReceiveUID,
                                             CancelReason = j.CancelReason,
                                             ISSTPUID = j.ISSTPUID,
                                             ItemIssueUID = j.UID,
                                             NetAmount = j.NetAmount,
                                             OtherCharges = j.OtherCharges
                                         }).ToList();

            return data;
        }


        [Route("ManageItemTransfer")]
        [HttpPost]
        public HttpResponseMessage ManageItemTransfer(ItemIssueModel model, int userUID)
        {
            try
            {
                DateTime now = DateTime.Now;

                using (var tran = new TransactionScope())
                {
                    ItemIssue itemIssue = db.ItemIssue.Find(model.ItemIssueUID);
                    if (itemIssue == null)
                    {
                        itemIssue = new ItemIssue();
                        itemIssue.CUser = userUID;
                        itemIssue.CWhen = now;

                        int itmISSID;
                        string ISSID = SEQHelper.GetSEQIDFormat("SEQItemIssue", out itmISSID);


                        if (string.IsNullOrEmpty(ISSID))
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No SEQItemIssue in SEQCONFIGURATION");
                        }

                        if (itmISSID == 0)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Insert SEQItemIssue is Fail");
                        }

                        itemIssue.ItemIssueID = ISSID;
                    }
                    itemIssue.MUser = userUID;
                    itemIssue.MWhen = now;
                    itemIssue.OrganisationUID = model.OrganisationUID;
                    itemIssue.LocationUID = model.LocationUID;
                    itemIssue.StoreUID = model.StoreUID;
                    itemIssue.RequestedByOrganisationUID = model.RequestedByOrganisationUID;
                    itemIssue.RequestedByLocationUID = model.RequestedByLocationUID;
                    itemIssue.RequestedByStoreUID = model.RequestedByStoreUID;
                    itemIssue.ItemIssueDttm = model.ItemIssueDttm;
                    itemIssue.ISSTPUID = 2918;
                    itemIssue.ISUSTUID = 2914;
                    itemIssue.IssueBy = userUID;
                    itemIssue.OtherCharges = model.OtherCharges;
                    itemIssue.NetAmount = model.NetAmount;
                    itemIssue.Comments = model.Comments;
                    itemIssue.ItemRequestID = model.ItemRequestID;
                    itemIssue.ItemReceiveID = model.ItemReceiveID;
                    itemIssue.ItemRequestUID = model.ItemRequestUID;
                    itemIssue.ItemReceiveUID = model.ItemReceiveUID;
                    itemIssue.StatusFlag = "A";
                    db.ItemIssue.AddOrUpdate(itemIssue);

                    db.SaveChanges();

                    //#region DeleteItemIssueDetail

                    //List<ItemIssueDetail> itemISSDE = db.ItemIssueDetail.Where(p => p.ItemIssueUID == model.ItemIssueUID && p.StatusFlag == "A").ToList();

                    //foreach (var item in itemISSDE)
                    //{
                    //    var data = model.ItemIssueDetail.FirstOrDefault(p => p.ItemIssueDetailUID == item.UID);
                    //    if (data == null)
                    //    {
                    //        db.ItemIssueDetail.Attach(item);
                    //        item.MUser = userUID;
                    //        item.MWhen = now;
                    //        item.StatusFlag = "D";

                    //        db.SaveChanges();
                    //    }

                    //}

                    //#endregion


                    #region SaveItemIssueDetail
                    foreach (var item in model.ItemIssueDetail)
                    {
                        //ItemIssueDetail itemIssueDetail = db.ItemIssueDetail.Find(item.ItemIssueDetailUID);
                        //if (itemIssueDetail == null)
                        //{
                        //    itemIssueDetail = new ItemIssueDetail();
                        //    itemIssueDetail.CUser = userUID;
                        //    itemIssueDetail.CWhen = now;
                        //}
                        ItemIssueDetail itemIssueDetail = new ItemIssueDetail();
                        itemIssueDetail.CUser = userUID;
                        itemIssueDetail.CWhen = now;
                        itemIssueDetail.ItemIssueUID = itemIssue.UID;
                        itemIssueDetail.ItemMasterUID = item.ItemMasterUID;
                        itemIssueDetail.ItemCode = item.ItemCode;
                        itemIssueDetail.ItemName = item.ItemName;
                        itemIssueDetail.UnitPrice = item.UnitPrice;
                        itemIssueDetail.NetAmount = item.NetAmount;
                        itemIssueDetail.ItemCost = item.ItemCost;
                        itemIssueDetail.Quantity = item.Quantity;
                        itemIssueDetail.IMUOMUID = item.IMUOMUID;
                        itemIssueDetail.BatchID = item.BatchID;
                        itemIssueDetail.StockUID = item.StockUID;
                        itemIssueDetail.ExpiryDttm = item.ExpiryDttm;
                        itemIssueDetail.MUser = userUID;
                        itemIssueDetail.MWhen = now;
                        itemIssueDetail.StatusFlag = "A";
                        itemIssueDetail.SerialNumber = item.SerialNumber;
                        db.ItemIssueDetail.AddOrUpdate(itemIssueDetail);

                        db.SaveChanges();
                    }


                    ItemRequest itemRequest = db.ItemRequest.Find(itemIssue.ItemRequestUID);
                    if (itemRequest != null)
                    {
                        db.ItemRequest.Attach(itemRequest);
                        itemRequest.MWhen = now;
                        itemRequest.MUser = userUID;
                        itemRequest.ItemIssueID = itemIssue.ItemIssueID;
                        itemRequest.RQSTSUID = 2928;
                        db.SaveChanges();
                    }

                    SqlDirectStore.pInvenTransferItem(itemIssue.UID, userUID);
                    #endregion

                    tran.Complete();
                }



                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }




        [Route("ManageItemTransferEcount")]
        [HttpPost]
        public HttpResponseMessage ManageItemTransferEcount(ItemIssueModel model, int userUID)
        {
            try
            {
                DateTime now = DateTime.Now;

                using (var tran = new TransactionScope())
                {
                    ItemIssue itemIssue = db.ItemIssue.Find(model.ItemIssueUID);
                    if (itemIssue == null)
                    {
                        itemIssue = new ItemIssue();
                        itemIssue.CUser = userUID;
                        itemIssue.CWhen = now;

                        int itmISSID;
                        string ISSID = SEQHelper.GetSEQIDFormat("SEQItemIssue", out itmISSID);


                        if (string.IsNullOrEmpty(ISSID))
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No SEQItemIssue in SEQCONFIGURATION");
                        }

                        if (itmISSID == 0)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Insert SEQItemIssue is Fail");
                        }

                        itemIssue.ItemIssueID = ISSID;
                    }
                    itemIssue.MUser = userUID;
                    itemIssue.MWhen = now;
                    itemIssue.OrganisationUID = model.OrganisationUID;
                    itemIssue.StoreUID = model.StoreUID;
                    itemIssue.RequestedByOrganisationUID = model.RequestedByOrganisationUID;
                    itemIssue.RequestedByStoreUID = model.RequestedByStoreUID;
                    itemIssue.ItemIssueDttm = model.ItemIssueDttm;
                    itemIssue.ISSTPUID = 2918;
                    itemIssue.ISUSTUID = 2914;
                    itemIssue.IssueBy = userUID;
                    itemIssue.OtherCharges = model.OtherCharges;
                    itemIssue.NetAmount = model.NetAmount;
                    itemIssue.Comments = model.Comments;
                    itemIssue.ItemRequestID = model.ItemRequestID;
                    itemIssue.ItemReceiveID = model.ItemReceiveID;
                    itemIssue.ItemRequestUID = model.ItemRequestUID;
                    itemIssue.ItemReceiveUID = model.ItemReceiveUID;
                    itemIssue.StatusFlag = "A";
                    db.ItemIssue.AddOrUpdate(itemIssue);

                    db.SaveChanges();

                    //#region DeleteItemIssueDetail

                    //List<ItemIssueDetail> itemISSDE = db.ItemIssueDetail.Where(p => p.ItemIssueUID == model.ItemIssueUID && p.StatusFlag == "A").ToList();

                    //foreach (var item in itemISSDE)
                    //{
                    //    var data = model.ItemIssueDetail.FirstOrDefault(p => p.ItemIssueDetailUID == item.UID);
                    //    if (data == null)
                    //    {
                    //        db.ItemIssueDetail.Attach(item);
                    //        item.MUser = userUID;
                    //        item.MWhen = now;
                    //        item.StatusFlag = "D";

                    //        db.SaveChanges();
                    //    }

                    //}

                    //#endregion


                    #region SaveItemIssueDetail
                    foreach (var item in model.ItemIssueDetail)
                    {
                        //ItemIssueDetail itemIssueDetail = db.ItemIssueDetail.Find(item.ItemIssueDetailUID);
                        //if (itemIssueDetail == null)
                        //{
                        //    itemIssueDetail = new ItemIssueDetail();
                        //    itemIssueDetail.CUser = userUID;
                        //    itemIssueDetail.CWhen = now;
                        //}
                        ItemIssueDetail itemIssueDetail = new ItemIssueDetail();
                        itemIssueDetail.CUser = userUID;
                        itemIssueDetail.CWhen = now;
                        itemIssueDetail.ItemIssueUID = itemIssue.UID;
                        itemIssueDetail.ItemMasterUID = item.ItemMasterUID;
                        itemIssueDetail.ItemCode = item.ItemCode;
                        itemIssueDetail.ItemName = item.ItemName;
                        itemIssueDetail.UnitPrice = item.UnitPrice;
                        itemIssueDetail.NetAmount = item.NetAmount;
                        itemIssueDetail.ItemCost = item.ItemCost;
                        itemIssueDetail.Quantity = item.Quantity;
                        itemIssueDetail.IMUOMUID = item.IMUOMUID;
                        itemIssueDetail.BatchID = item.BatchID;
                        itemIssueDetail.StockUID = item.StockUID;
                        itemIssueDetail.ExpiryDttm = item.ExpiryDttm;
                        itemIssueDetail.MUser = userUID;
                        itemIssueDetail.MWhen = now;
                        itemIssueDetail.StatusFlag = "A";
                        itemIssueDetail.SerialNumber = item.SerialNumber;
                        db.ItemIssueDetail.AddOrUpdate(itemIssueDetail);

                        db.SaveChanges();
                    }


                    ItemRequest itemRequest = db.ItemRequest.Find(itemIssue.ItemRequestUID);
                    if (itemRequest != null)
                    {
                        db.ItemRequest.Attach(itemRequest);
                        itemRequest.MWhen = now;
                        itemRequest.MUser = userUID;
                        itemRequest.ItemIssueID = itemIssue.ItemIssueID;
                        itemRequest.RQSTSUID = 2928;
                        db.SaveChanges();
                    }

                    //SqlDirectStore.pInvenTransferItem(itemIssue.UID, userUID);
                    SqlDirectStore.pInvenTransferItemEcount(itemIssue.UID, userUID);
                    #endregion

                    tran.Complete();
                }



                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }





        [Route("CancelItemTransfer")]
        [HttpPut]
        public HttpResponseMessage CancelItemTransfer(int itemIssueUID, string cancelReason, int userUID)
        {
            try
            {
                using (var tran = new TransactionScope())
                {
                    ItemIssue itemIssue = db.ItemIssue.Find(itemIssueUID);
                    if (itemIssue != null)
                    {
                        ItemRequest itemRequest = db.ItemRequest.Find(itemIssue.ItemRequestUID);
                        if (itemRequest != null)
                        {
                            db.ItemRequest.Attach(itemRequest);
                            itemRequest.RQSTSUID = 2926;
                            itemRequest.MUser = userUID;
                            itemRequest.MWhen = DateTime.Now;
                        }

                        db.SaveChanges();
                    }
                    SqlDirectStore.pInvenCancelTransferItem(itemIssueUID, cancelReason, userUID);
                    tran.Complete();
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }

        }

        [Route("CheckCancelTransfer")]
        [HttpGet]
        public CheckCancelTransactionInven CheckCancelTransfer(int itemIssueUID)
        {

            var itemIssueDetails = from i in db.ItemIssue
                                   join j in db.ItemIssueDetail on i.UID equals j.ItemIssueUID
                                   where i.StatusFlag == "A"
                                   && j.StatusFlag == "A"
                                   && i.UID == itemIssueUID
                                   select j;

            foreach (var itemIssueDetail in itemIssueDetails)
            {
                var stockMovement = (from i in db.StockMovement
                                     where i.StatusFlag == "A"
                                     && i.Note == "ReceiveTransfer"
                                     && i.RefUID == itemIssueDetail.UID
                                     select i).FirstOrDefault();
                if (stockMovement != null)
                {
                    var stockTransaction = (from i in db.StockMovement
                                            where i.StatusFlag == "A"
                                            && i.StockUID == stockMovement.StockUID
                                            select i);
                    if (stockTransaction != null)
                    {
                        stockTransaction = stockTransaction.Where(p => p.RefTable != "StockAdjustment"
                       && p.RefTable != "SaleReturnList"
                       && ((p.RefTable == "DispensedItem" && p.Note == "SaleReturn") ? "SaleReturnList" : "DispensedItem") != "SaleReturnList"
                       );

                        if (stockTransaction.Count() > 1)
                        {
                            return new CheckCancelTransactionInven() { IsActive = false };
                        }
                    }
                }

            }



            return new CheckCancelTransactionInven() { IsActive = true };

        }

        #endregion

        #region ItemReceive
        [HttpGet]
        public List<ItemReceiveModel> SearchItemReceive(DateTime? dateFrom, DateTime? dateTo, string receiveID, int? organisationIssueUID, int? locationIssueUID, int? organisationReceiveUID, int? locationReceiveUID)
        {
            List<ItemReceiveModel> data = (from j in db.ItemReceive
                                           where j.StatusFlag == "A"
                                           && (dateFrom == null || DbFunctions.TruncateTime(j.ReceivedDttm) >= DbFunctions.TruncateTime(dateFrom))
                                           && (dateTo == null || DbFunctions.TruncateTime(j.ReceivedDttm) <= DbFunctions.TruncateTime(dateTo))
                                           && (string.IsNullOrEmpty(receiveID) || j.ItemReceiveID == receiveID)
                                           && (organisationReceiveUID == null || j.OrganisationUID == organisationReceiveUID)
                                           && (locationReceiveUID == null || j.LocationUID == locationReceiveUID)
                                           && (organisationIssueUID == null || j.IssuedByOrganisationUID == organisationIssueUID)
                                           && (locationIssueUID == null || j.IssuedByLocationUID == locationIssueUID)
                                           select new ItemReceiveModel
                                           {
                                               OrganisationUID = j.OrganisationUID,
                                               LocationUID = j.LocationUID ?? 0,
                                               StoreUID = j.StoreUID,
                                               IssuedByOrganisationUID = j.IssuedByOrganisationUID,
                                               IssuedByLocationUID = j.IssuedByLocationUID ?? 0,
                                               IssuedByStoreUID = j.IssuedByStoreUID,
                                               Organisation = SqlFunction.fGetHealthOrganisationName(j.OrganisationUID),
                                               LocationName = SqlFunction.fGetLocationName(j.LocationUID ?? 0),
                                               Store = SqlFunction.fGetStoreName(j.StoreUID),
                                               IssuedByOrganisation = SqlFunction.fGetHealthOrganisationName(j.IssuedByOrganisationUID),
                                               IssuedByLocation = SqlFunction.fGetLocationName(j.IssuedByLocationUID ?? 0),
                                               IssuedByStore = SqlFunction.fGetStoreName(j.IssuedByStoreUID),
                                               ReceivedDttm = j.ReceivedDttm,
                                               ItemReceiveID = j.ItemReceiveID,
                                               ItemIssueUID = j.ItemIssueUID,
                                               ItemIssueID = j.ItemIssueID,
                                               NetAmount = j.NetAmount,
                                               OtherCharges = j.OtherCharges,
                                               RCSTSUID = j.RCSTSUID,
                                               ReceiveStatus = SqlFunction.fGetRfValDescription(j.RCSTSUID ?? 0),
                                               ReceiveBy = j.ReceiveBy,
                                               ReceiveByName = SqlFunction.fGetCareProviderName(j.ReceiveBy),
                                               ItemRecieveUID = j.UID,
                                               Comments = j.Comments,
                                               CancelReason = j.CancelReason
                                           }).ToList();

            return data;
        }

        [Route("GetItemReceiveByUID")]
        [HttpGet]
        public ItemReceiveModel GetItemReceiveByUID(int itemReceiveUID)
        {
            ItemReceiveModel data = (from j in db.ItemReceive
                                     where j.StatusFlag == "A"
                                     && j.UID == itemReceiveUID
                                     select new ItemReceiveModel
                                     {
                                         OrganisationUID = j.OrganisationUID,
                                         LocationUID = j.LocationUID ?? 0,
                                         StoreUID = j.StoreUID,
                                         IssuedByOrganisationUID = j.IssuedByOrganisationUID,
                                         IssuedByLocationUID = j.IssuedByLocationUID ?? 0,
                                         IssuedByStoreUID = j.IssuedByStoreUID,
                                         Organisation = SqlFunction.fGetHealthOrganisationName(j.OrganisationUID),
                                         Store = SqlFunction.fGetStoreName(j.StoreUID),
                                         IssuedByOrganisation = SqlFunction.fGetHealthOrganisationName(j.IssuedByOrganisationUID),
                                         IssuedByStore = SqlFunction.fGetStoreName(j.IssuedByStoreUID),
                                         ReceivedDttm = j.ReceivedDttm,
                                         ItemReceiveID = j.ItemReceiveID,
                                         NetAmount = j.NetAmount,
                                         OtherCharges = j.OtherCharges,
                                         RCSTSUID = j.RCSTSUID,
                                         ItemIssueUID = j.ItemIssueUID,
                                         ItemIssueID = j.ItemIssueID,
                                         ReceiveStatus = SqlFunction.fGetRfValDescription(j.RCSTSUID ?? 0),
                                         ReceiveBy = j.ReceiveBy,
                                         ReceiveByName = SqlFunction.fGetCareProviderName(j.ReceiveBy),
                                         Comments = j.Comments,
                                         ItemRecieveUID = j.UID,
                                         CancelReason = j.CancelReason
                                     }).FirstOrDefault();

            if (data != null)
            {
                data.ItemReceiveDetail = db.ItemReceiveDetail
                    .Where(p => p.StatusFlag == "A" && p.ItemRecieveUID == data.ItemRecieveUID)
                    .Select(p => new ItemReceiveDetailModel
                    {
                        ItemRecieveDetailUID = p.UID,
                        ItemRecieveUID = p.ItemRecieveUID,
                        ItemMasterUID = p.ItemMasterUID,
                        ItemCode = p.ItemCode,
                        ItemName = p.ItemName,
                        ItemCost = p.ItemCost,
                        UnitPrice = p.UnitPrice,
                        NetAmount = p.NetAmount,
                        Quantity = p.Quantity,
                        IMUOMUID = p.IMUOMUID,
                        BatchID = p.BatchID,
                        ExpiryDttm = p.ExpiryDttm
                    }).ToList();
            }

            return data;
        }


        [Route("GetItemReceiveDetailByItemReceiveUID")]
        [HttpGet]
        public List<ItemReceiveDetailModel> GetItemReceiveDetailByItemReceiveUID(int itemReceiveUID)
        {
            List<ItemReceiveDetailModel> data = db.ItemReceiveDetail
                    .Where(p => p.StatusFlag == "A" && p.ItemRecieveUID == itemReceiveUID)
                    .Select(p => new ItemReceiveDetailModel
                    {
                        ItemRecieveDetailUID = p.UID,
                        ItemRecieveUID = p.ItemRecieveUID,
                        ItemMasterUID = p.ItemMasterUID,
                        ItemCode = p.ItemCode,
                        ItemName = p.ItemName,
                        ItemCost = p.ItemCost,
                        UnitPrice = p.UnitPrice,
                        NetAmount = p.NetAmount,
                        Quantity = p.Quantity,
                        IMUOMUID = p.IMUOMUID,
                        Unit = SqlFunction.fGetRfValDescription(p.IMUOMUID),
                        BatchID = p.BatchID,
                        ExpiryDttm = p.ExpiryDttm
                    }).ToList();

            return data;
        }

        [Route("CancelItemReceive")]
        [HttpPut]
        public HttpResponseMessage CancelItemReceive(int itemReceiveUID, string cancelReason, int userUID)
        {
            try
            {
                using (var tran = new TransactionScope())
                {
                    ItemReceive itemReceive = db.ItemReceive.Find(itemReceiveUID);
                    SqlDirectStore.pInvenCancelItemReceive(itemReceiveUID, cancelReason, userUID);

                    //if (itemReceive.ItemIssueUID.HasValue)
                    //    SqlDirectStore.pInvenClearItemIssueDetail(itemReceive.ItemIssueUID.Value, userUID);

                    if (itemReceive.ItemIssueUID.HasValue)
                    {
                        ItemIssue itemIssue = db.ItemIssue.Find(itemReceive.ItemIssueUID);
                        if (itemIssue != null)
                        {
                            db.ItemIssue.Attach(itemIssue);
                            itemIssue.MUser = userUID;
                            itemIssue.MWhen = DateTime.Now;
                            itemIssue.ISUSTUID = 2913;
                            db.SaveChanges();
                        }
                    }

                    tran.Complete();
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }

        }

        [Route("CheckCancelReceive")]
        [HttpGet]
        public CheckCancelTransactionInven CheckCancelReceive(int itemReceiveUID)
        {

            var ItemReceiveDetails = from i in db.ItemReceive
                                     join j in db.ItemReceiveDetail on i.UID equals j.ItemRecieveUID
                                     where i.StatusFlag == "A"
                                     && j.StatusFlag == "A"
                                     && i.UID == itemReceiveUID
                                     select j;

            foreach (var itemReceiveDetail in ItemReceiveDetails)
            {
                var stockMovement = (from i in db.StockMovement
                                     where i.StatusFlag == "A"
                                     && i.RefTable == "ItemReceiveDetail"
                                     && i.RefUID == itemReceiveDetail.UID
                                     select i).FirstOrDefault();
                if (stockMovement != null)
                {
                    var stockTransaction = (from i in db.StockMovement
                                            where i.StatusFlag == "A"
                                            && i.StockUID == stockMovement.StockUID
                                            select i);
                    if (stockTransaction != null)
                    {
                        stockTransaction = stockTransaction.Where(p => p.RefTable != "StockAdjustment"
                          && p.RefTable != "SaleReturnList"
                          && ((p.RefTable == "DispensedItem" && p.Note == "SaleReturn") ? "SaleReturnList" : "DispensedItem") != "SaleReturnList"
                          );

                        if (stockTransaction.Count() > 1)
                        {
                            return new CheckCancelTransactionInven() { IsActive = false };
                        }
                    }
                }


                //if (stockMovement != null)
                //{
                //    var stock = db.Stock.Find(stockMovement.StockUID);
                //    if (stock.Quantity != stockMovement.BalQty)
                //    {
                //        return new CanTransactionInven() { IsActive = false };
                //    }
                //}

            }



            return new CheckCancelTransactionInven() { IsActive = true };

        }


        [Route("ManageItemReceive")]
        [HttpPost]
        public HttpResponseMessage ManageItemReceive(ItemReceiveModel model, int userUID)
        {
            try
            {
                DateTime now = DateTime.Now;

                using (var tran = new TransactionScope())
                {
                    ItemReceive itemReceive = new ItemReceive();
                    itemReceive.CUser = userUID;
                    itemReceive.CWhen = now;

                    int itmIRCID;
                    string IRCID = SEQHelper.GetSEQIDFormat("SEQItemReceive", out itmIRCID);


                    if (string.IsNullOrEmpty(IRCID))
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No SEQItemReceive in SEQCONFIGURATION");
                    }

                    if (itmIRCID == 0)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Insert SEQItemReceive is Fail");
                    }

                    itemReceive.ItemReceiveID = IRCID;



                    itemReceive.MUser = userUID;
                    itemReceive.MWhen = now;
                    itemReceive.OrganisationUID = model.OrganisationUID;
                    itemReceive.LocationUID = model.LocationUID;
                    itemReceive.StoreUID = model.StoreUID;
                    itemReceive.IssuedByOrganisationUID = model.IssuedByOrganisationUID;
                    itemReceive.IssuedByLocationUID = model.IssuedByLocationUID;
                    itemReceive.IssuedByStoreUID = model.IssuedByStoreUID;
                    itemReceive.ReceivedDttm = model.ReceivedDttm;
                    itemReceive.ItemIssueUID = model.ItemIssueUID;
                    itemReceive.ItemIssueID = model.ItemIssueID;
                    itemReceive.NetAmount = model.NetAmount;
                    itemReceive.OtherCharges = model.OtherCharges;
                    itemReceive.RCSTSUID = 2930;
                    itemReceive.ReceiveBy = userUID;
                    itemReceive.Comments = model.Comments;
                    itemReceive.StatusFlag = "A";
                    db.ItemReceive.Add(itemReceive);

                    db.SaveChanges();


                    #region SaveItemReceiveDetail
                    foreach (var item in model.ItemReceiveDetail)
                    {
                        ItemReceiveDetail itemReceiveDetail = new ItemReceiveDetail();
                        itemReceiveDetail.CUser = userUID;
                        itemReceiveDetail.CWhen = now;
                        itemReceiveDetail.ItemRecieveUID = itemReceive.UID;
                        itemReceiveDetail.ItemMasterUID = item.ItemMasterUID;
                        itemReceiveDetail.ItemCode = item.ItemCode;
                        itemReceiveDetail.ItemName = item.ItemName;
                        itemReceiveDetail.ItemCost = item.ItemCost;
                        itemReceiveDetail.NetAmount = item.NetAmount;
                        itemReceiveDetail.UnitPrice = item.UnitPrice;
                        itemReceiveDetail.IssuedByStockUID = item.IssuedByStockUID;
                        itemReceiveDetail.Quantity = item.Quantity;
                        itemReceiveDetail.IMUOMUID = item.IMUOMUID;
                        itemReceiveDetail.BatchID = item.BatchID;
                        itemReceiveDetail.ExpiryDttm = item.ExpiryDttm;
                        itemReceiveDetail.MUser = userUID;
                        itemReceiveDetail.MWhen = now;
                        itemReceiveDetail.StatusFlag = "A";
                        db.ItemReceiveDetail.Add(itemReceiveDetail);

                        db.SaveChanges();
                    }

                    ItemIssue itemIssue = db.ItemIssue.Find(itemReceive.ItemIssueUID);
                    if (itemIssue != null)
                    {

                        //SqlDirectStore.pInvenIssueItem(itemIssue.UID, userUID);
                        db.ItemIssue.Attach(itemIssue);
                        itemIssue.MWhen = now;
                        itemIssue.MUser = userUID;
                        itemIssue.ItemReceiveUID = itemReceive.UID;
                        itemIssue.ItemReceiveID = itemReceive.ItemReceiveID;
                        itemIssue.ISUSTUID = 2914;

                        ItemRequest itemRequest = db.ItemRequest.Find(itemIssue.ItemRequestUID);
                        if (itemRequest != null)
                        {
                            db.ItemRequest.Attach(itemRequest);
                            itemRequest.MWhen = now;
                            itemRequest.MUser = userUID;
                            itemRequest.RQSTSUID = 2928;
                            db.SaveChanges();
                        }

                        db.SaveChanges();
                    }

                    SqlDirectStore.pInvenReceiveItem(itemReceive.UID, userUID);

                    #endregion

                    tran.Complete();
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception er)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, er.Message, er);
            }
        }

        #endregion


        #region IPFills

        [Route("SearchIPFills")]
        [HttpGet]
        public List<IPFillProcessModel> SearchIPFills(DateTime? dateFrom, DateTime? dateTo, int storeUID)
        {
            List<IPFillProcessModel> data = db.IPFillProcess.Where(p => p.StatusFlag == "A"
            && (dateFrom == null || DbFunctions.TruncateTime(p.FillDttm) >= DbFunctions.TruncateTime(dateFrom))
            && (dateTo == null || DbFunctions.TruncateTime(p.FillDttm) <= DbFunctions.TruncateTime(dateTo))
            && p.StoreUID == storeUID)
                .Select(p => new IPFillProcessModel()
                {
                    IPFillProcessUID = p.UID,
                    FillID = p.FillID,
                    StoreUID = p.StoreUID,
                    StoreName = SqlFunction.fGetStoreName(p.StoreUID ?? 0),
                    OwnerOrganisationUID = p.OwnerOrganisationUID,
                    OwnerOrganisationName = SqlFunction.fGetHealthOrganisationName(p.OwnerOrganisationUID),
                    FillByUserUID = p.FillByUserUID,
                    FillByUser = SqlFunction.fGetCareProviderName(p.FillByUserUID ?? 0),
                    FillDttm = p.FillDttm,
                    ExcludePriorHour = p.ExcludePriorHour,
                    WardUID = p.WardUID,
                    WardName = SqlFunction.fGetLocationName(p.WardUID ?? 0),
                    Comment = p.Comments
                }).OrderByDescending(p => p.FillDttm).ToList();

            return data;
        }

        [Route("GetIPFillDetail")]
        [HttpGet]
        public List<IPFillDetailModel> GetIPFillDetail(long iPFillProcessUID)
        {
            List<IPFillDetailModel> data = (from p in db.IPFillDetail
                                            join item in db.ItemMaster on p.ItemMasterUID equals item.UID
                                            where p.StatusFlag == "A"
                                            && item.StatusFlag == "A"
                                            && p.IPFillProcessUID == iPFillProcessUID
                                            select new IPFillDetailModel()
                                            {
                                                IPFillProcessUID = p.IPFillProcessUID,
                                                IPFillDetailUID = p.UID,
                                                PatientFillID = p.PatientFillID,
                                                PatientUID = p.PatientUID,
                                                PatientID = SqlFunction.fGetPatientID(p.PatientUID),
                                                PatientName = SqlFunction.fGetPatientName(p.PatientUID),
                                                StockUID = p.StockUID,
                                                BatchID = p.BatchID,
                                                Quantity = p.Quantity,
                                                IMUOMUID = p.IMUOMUID ?? 0,
                                                IMUOM = SqlFunction.fGetRfValDescription(p.IMUOMUID ?? 0),
                                                PatientOrderUID = p.PatientOrderUID ?? 0,
                                                PatientVisitUID = p.PatientVisitUID ?? 0,
                                                OwnerOrganisationUID = p.OwnerOrganisationUID,
                                                OwnerOrganisationName = SqlFunction.fGetHealthOrganisationName(p.OwnerOrganisationUID),
                                                PatientOrderDetailUID = p.PatientOrderDetailUID,
                                                ItemMasterUID = p.ItemMasterUID,
                                                ItemName = item.Name,
                                                WardUID = p.WardUID,
                                                WardName = SqlFunction.fGetLocationName(p.WardUID ?? 0),
                                                BedUID = p.BedUID,
                                                BedName = SqlFunction.fGetLocationName(p.BedUID ?? 0),
                                                ExpiryDttm = p.ExpiryDttm
                                            }).ToList();

            return data;
        }

        [Route("DispenseIPFills")]
        [HttpPost]
        public HttpResponseMessage DispenseIPFills(IPFillProcessModel iPFillProcessModel, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;

                IPFillProcess iPFill = new IPFillProcess();

                int iPFillID;
                string fillID = SEQHelper.GetSEQIDFormat("SEQIPFILLID", out iPFillID);

                if (string.IsNullOrEmpty(fillID))
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No SEQIPFILLID in SEQCONFIGURATION");
                }

                if (iPFillID == 0)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Insert SEQIPFILLID is Fail");
                }

                iPFill.FillID = fillID;

                iPFill.FillDttm = now;
                iPFill.FillByUserUID = userID;
                iPFill.FillForDays = iPFillProcessModel.FillForDay;
                iPFill.ExcludePriorHour = iPFillProcessModel.ExcludePriorHour;
                iPFill.WardUID = iPFillProcessModel.WardUID;
                iPFill.StoreUID = iPFillProcessModel.StoreUID;
                iPFill.OwnerOrganisationUID = iPFillProcessModel.OwnerOrganisationUID;
                iPFill.MUser = userID;
                iPFill.MWhen = now;
                iPFill.CUser = userID;
                iPFill.CWhen = now;
                iPFill.StatusFlag = "A";

                db.IPFillProcess.Add(iPFill);
                db.SaveChanges();

                if (iPFillProcessModel.StandingModels != null && iPFillProcessModel.StandingModels.Count != 0)
                {
                    foreach (var item in iPFillProcessModel.StandingModels)
                    {
                        int BSMDDUID = item.BSMDDUID;

                        #region PatientOrder

                        PatientOrder patientOrder = new PatientOrder();

                        int seqPatientOrderID;
                        string patientOrderID = SEQHelper.GetSEQIDFormat("SEQPatientOrder", out seqPatientOrderID);

                        if (string.IsNullOrEmpty(patientOrderID))
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No SEQPatientOrder in SEQCONFIGURATION");
                        }

                        if (seqPatientOrderID == 0)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Insert SEQPatientOrder is Fail");
                        }

                        patientOrder.OrderNumber = patientOrderID;
                        patientOrder.ParentUID = item.PatientOrderUID;
                        patientOrder.PatientUID = item.PatientUID;
                        patientOrder.StartDttm = now;
                        patientOrder.EndDttm = now;
                        patientOrder.PRSTYPUID = item.PRSTYPUID;
                        patientOrder.PatientVisitUID = item.PatientVisitUID;
                        patientOrder.OrderRaisedBy = userID;
                        patientOrder.MUser = userID;
                        patientOrder.MWhen = now;
                        patientOrder.CUser = userID;
                        patientOrder.CWhen = now;
                        patientOrder.IsContinuous = "Y";
                        patientOrder.IsIPFill = "Y";
                        patientOrder.StatusFlag = "A";
                        patientOrder.OrderLocationUID = item.LocationUID;
                        patientOrder.OwnerOrganisationUID = item.OwnerOrganisationUID;
                        patientOrder.IdentifyingType = "PATIENTORDER";
                        db.PatientOrder.Add(patientOrder);
                        db.SaveChanges();

                        #endregion

                        #region PatientOrderDetail

                        PatientOrderDetail orderDetail = new PatientOrderDetail();

                        orderDetail.PatientOrderUID = patientOrder.UID;
                        orderDetail.ParentUID = item.ParentOrderDetalUID;
                        orderDetail.MUser = userID;
                        orderDetail.MWhen = now;
                        orderDetail.CUser = userID;
                        orderDetail.CWhen = now;
                        orderDetail.StatusFlag = "A";
                        orderDetail.IsStandingOrder = "Y";
                        orderDetail.IsStockItem = "Y";
                        orderDetail.OwnerOrganisationUID = item.OwnerOrganisationUID;
                        orderDetail.ItemCode = item.ItemCode;
                        orderDetail.ItemName = item.ItemName;
                        orderDetail.StartDttm = now;
                        orderDetail.EndDttm = now;
                        orderDetail.ORDSTUID = 2861;
                        orderDetail.Dosage = item.Dosage;
                        orderDetail.Quantity = item.Quantity;
                        orderDetail.QNUOMUID = item.QNUOMUID;
                        orderDetail.FRQNCUID = item.FRQNCUID;
                        orderDetail.UnitPrice = item.UnitPrice;
                        orderDetail.IsPriceOverwrite = item.IsPriceOverwrite;
                        orderDetail.OverwritePrice = item.OverwritePrice;
                        orderDetail.OriginalUnitPrice = item.OriginalUnitPrice;
                        orderDetail.DoctorFee = item.DoctorFee;
                        orderDetail.CareproviderUID = item.CareproviderUID;
                        orderDetail.NetAmount = item.NetAmount;
                        orderDetail.ROUTEUID = item.ROUTEUID;
                        orderDetail.DFORMUID = item.DFORMUID;
                        orderDetail.PDSTSUID = item.PDSTSUID;
                        orderDetail.DrugDuration = item.DrugDuration;
                        orderDetail.InstructionText = item.InstructionText;
                        orderDetail.LocalInstructionText = item.LocalInstructionText;
                        orderDetail.BillableItemUID = item.BillableItemUID;
                        orderDetail.IsStockItem = item.IsStock;
                        orderDetail.StoreUID = item.StoreUID;
                        orderDetail.Comments = item.Comments;
                        orderDetail.OrderSetUID = item.OrderSetUID;
                        orderDetail.OrderSetBillableItemUID = item.OrderSetBillableItemUID;
                        orderDetail.IdentifyingType = "ORDERITEM";
                        db.PatientOrderDetail.Add(orderDetail);
                        db.SaveChanges();

                        #endregion

                        #region IpFillDetail
                        IPFillDetail iPFillDetail = new IPFillDetail();

                        int iPFillDetailID;
                        string fillDetailID = SEQHelper.GetSEQIDFormat("SEQFILLDETAILID", out iPFillDetailID);

                        if (string.IsNullOrEmpty(fillDetailID))
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No SEQFILLDETAILID in SEQCONFIGURATION");
                        }

                        if (iPFillDetailID == 0)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Insert SEQFILLDETAILID is Fail");
                        }

                        iPFillDetail.PatientUID = item.PatientUID;
                        iPFillDetail.PatientVisitUID = item.PatientVisitUID;
                        iPFillDetail.PatientFillID = fillDetailID;
                        iPFillDetail.IPFillProcessUID = iPFill.UID;
                        iPFillDetail.PatientOrderUID = patientOrder.UID;
                        iPFillDetail.PatientOrderDetailUID = orderDetail.UID;
                        iPFillDetail.ItemMasterUID = item.ItemUID;
                        iPFillDetail.Quantity = item.Quantity;
                        iPFillDetail.BatchID = item.BatchID;
                        iPFillDetail.IMUOMUID = item.QNUOMUID;
                        iPFillDetail.WardUID = item.WardUID;
                        iPFillDetail.BedUID = item.BedUID;
                        iPFillDetail.ExpiryDttm = item.ExpiryDate ?? now;
                        iPFillDetail.StockUID = item.StockUID;
                        iPFillDetail.OwnerOrganisationUID = item.OwnerOrganisationUID;
                        iPFillDetail.MUser = userID;
                        iPFillDetail.MWhen = now;
                        iPFillDetail.CUser = userID;
                        iPFillDetail.CWhen = now;
                        iPFillDetail.StatusFlag = "A";

                        db.IPFillDetail.Add(iPFillDetail);
                        db.SaveChanges();

                        #endregion

                        #region Prescription

                        Prescription presc = new Prescription();

                        int seqPrescriptionID;
                        string prescriptionID = SEQHelper.GetSEQIDFormat("SEQPrescription", out seqPrescriptionID);

                        if (string.IsNullOrEmpty(prescriptionID))
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No SEQPrescription in SEQCONFIGURATION");
                        }

                        if (seqPrescriptionID == 0)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Insert SEQPrescription is Fail");
                        }

                        if (seqPrescriptionID != 0)
                        {
                            prescriptionID = "I" + prescriptionID;
                        }

                        presc.PrescriptionNumber = prescriptionID;
                        presc.PrescribedDttm = now;
                        presc.PrescribedBy = userID;
                        presc.BSMDDUID = 2826; //Drug;
                        presc.ORDSTUID = 2861; //Dispensed
                        presc.DispensedDttm = now;
                        presc.PatientUID = item.PatientUID;
                        presc.PatientVisitUID = item.PatientVisitUID;
                        presc.PRSTYPUID = item.PRSTYPUID;
                        presc.MUser = userID;
                        presc.MWhen = now;
                        presc.PatientOrderUID = patientOrder.UID;
                        presc.CUser = userID;
                        presc.CWhen = now;
                        presc.StatusFlag = "A";
                        presc.IsIPFill = "Y";
                        presc.OwnerOrganisationUID = item.OwnerOrganisationUID;

                        db.Prescription.Add(presc);
                        db.SaveChanges();

                        db.PatientOrderDetail.Attach(orderDetail);
                        orderDetail.IdentifyingUID = presc.UID;
                        orderDetail.IdentifyingType = "PRESCRIPTIONITEM";
                        db.SaveChanges();
                        #endregion

                        #region PrescriptionItem

                        PrescriptionItem prescritem = new PrescriptionItem();
                        prescritem.StartDttm = item.StartDttm;
                        prescritem.ORDSTUID = 2861; //Dispensed
                        prescritem.PrescriptionUID = presc.UID;
                        prescritem.PatientOrderDetailUID = orderDetail.UID;
                        prescritem.MUser = userID;
                        prescritem.MWhen = now;
                        prescritem.CUser = userID;
                        prescritem.CWhen = now;
                        prescritem.StatusFlag = "A";
                        prescritem.OwnerOrganisationUID = item.OwnerOrganisationUID;
                        prescritem.ItemCode = item.ItemCode;
                        prescritem.ItemName = item.ItemName;
                        prescritem.ROUTEUID = item.ROUTEUID;
                        prescritem.FRQNCUID = item.FRQNCUID;
                        prescritem.DFORMUID = item.DFORMUID;
                        prescritem.DrugDuration = item.DrugDuration;
                        prescritem.Dosage = item.Dosage;
                        prescritem.Quantity = item.Quantity;
                        prescritem.IMUOMUID = item.QNUOMUID;
                        prescritem.PDSTSUID = item.PDSTSUID;
                        prescritem.ItemMasterUID = item.ItemUID;
                        prescritem.BillableItemUID = item.BillableItemUID;
                        prescritem.StoreUID = item.StoreUID;
                        prescritem.ClinicalComments = item.ClinicalComments;
                        prescritem.InstructionText = item.InstructionText;
                        prescritem.LocalInstructionText = item.LocalInstructionText;
                        prescritem.Dosage = item.Dosage;
                        prescritem.Comments = orderDetail.Comments;
                        db.PrescriptionItem.Add(prescritem);
                        db.SaveChanges();
                        #endregion

                        #region SavePatinetOrderDetailHistory

                        PatientOrderDetailHistory patientOrderDetailHistory = new PatientOrderDetailHistory();
                        patientOrderDetailHistory.PatientOrderDetailUID = orderDetail.UID;
                        patientOrderDetailHistory.ORDSTUID = 2861;
                        patientOrderDetailHistory.EditedDttm = now;
                        patientOrderDetailHistory.EditByUserID = userID;
                        patientOrderDetailHistory.CUser = userID;
                        patientOrderDetailHistory.CWhen = now;
                        patientOrderDetailHistory.MUser = userID;
                        patientOrderDetailHistory.MWhen = now;
                        patientOrderDetailHistory.StatusFlag = "A";
                        db.PatientOrderDetailHistory.Add(patientOrderDetailHistory);
                        db.SaveChanges();

                        #endregion

                        #region PatientBillableItem

                        PatientBillableItem patBillableItem = new PatientBillableItem();
                        patBillableItem.PatientUID = patientOrder.PatientUID;
                        patBillableItem.PatientVisitUID = patientOrder.PatientVisitUID;
                        patBillableItem.BillableItemUID = orderDetail.BillableItemUID;
                        patBillableItem.IdentifyingUID = orderDetail.IdentifyingUID ?? 0;
                        patBillableItem.IdentifyingType = "STORE";
                        patBillableItem.OrderType = "PRESCRIPTIONITEM";
                        patBillableItem.IPFillProcessUID = iPFill.UID;
                        patBillableItem.OrderTypeUID = orderDetail.PatientOrderUID;
                        patBillableItem.BSMDDUID = BSMDDUID;
                        patBillableItem.ORDSTUID = 2861;
                        patBillableItem.Amount = orderDetail.UnitPrice;
                        patBillableItem.Discount = orderDetail.Discount;
                        patBillableItem.NetAmount = orderDetail.NetAmount;
                        patBillableItem.ItemMultiplier = orderDetail.Quantity;
                        patBillableItem.StartDttm = orderDetail.StartDttm;
                        patBillableItem.EndDttm = orderDetail.EndDttm;
                        patBillableItem.ItemName = orderDetail.ItemName;
                        patBillableItem.CareProviderUID = orderDetail.CareproviderUID;
                        patBillableItem.EventOccuredDttm = orderDetail.StartDttm;
                        patBillableItem.QNUOMUID = orderDetail.QNUOMUID;
                        patBillableItem.PayorDetailUID = orderDetail.PayorDetailUID;
                        patBillableItem.StoreUID = orderDetail.StoreUID;
                        patBillableItem.BillPackageUID = orderDetail.BillPackageUID;
                        patBillableItem.PatientOrderDetailUID = orderDetail.UID;
                        patBillableItem.OrderSetUID = orderDetail.OrderSetUID;
                        patBillableItem.OrderSetBillableItemUID = orderDetail.OrderSetBillableItemUID;
                        patBillableItem.PatientFixPriceUID = orderDetail.PatientFixPriceUID;
                        patBillableItem.CUser = userID;
                        patBillableItem.CWhen = now;
                        patBillableItem.MUser = userID;
                        patBillableItem.MWhen = now;
                        patBillableItem.StatusFlag = "A";
                        patBillableItem.OwnerOrganisationUID = orderDetail.OwnerOrganisationUID;
                        db.PatientBillableItem.Add(patBillableItem);
                        db.SaveChanges();

                        #endregion

                        DataTable dtStockUsed = SqlDirectStore.pDispensePrescriptionItem(prescritem.UID, userID);
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("CancelDispenseIPFills")]
        [HttpPut]
        public HttpResponseMessage CancelDispenseIPFills(int ipfillProccessUID, int ownerOrganisationUID, int userUID)
        {
            try
            {
                DateTime now = DateTime.Now;

                List<IPFillDetail> iPFillDetails = db.IPFillDetail.Where(p => p.IPFillProcessUID == ipfillProccessUID && p.StatusFlag == "A").ToList();

                if (iPFillDetails != null && iPFillDetails.Count != 0)
                {
                    foreach (var item in iPFillDetails)
                    {
                        db.IPFillDetail.Attach(item);
                        item.MUser = userUID;
                        item.MWhen = now;
                        item.StatusFlag = "D";

                        var stockmovement = (from pre in db.PrescriptionItem
                                             join dis in db.DispensedItem on pre.UID equals dis.PrescriptionItemUID
                                             join stv in db.StockMovement on new { key1 = dis.UID, key2 = "DispensedItem" } equals new { key1 = stv.RefUID ?? 0, key2 = stv.RefTable }
                                             where pre.StatusFlag == "A"
                                             && dis.StatusFlag == "A"
                                             && stv.StatusFlag == "A"
                                             && dis.ORDSTUID == 2861 // Dispensed
                                             && pre.PatientOrderDetailUID == item.PatientOrderDetailUID
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

                        if (stockmovement != null)
                        {
                            Stock stock = db.Stock.Find(stockmovement.StockUID);

                            double totalBFQty = SqlStatement.GetItemTotalQuantity(stock.ItemMasterUID, stock.StoreUID, stock.OwnerOrganisationUID);
                            double bfQty = stock.Quantity;

                            //SqlDirectStore.pCancelDispensed(item.PatientOrderDetailUID, userUID);

                            DispensedItem dispensedItem = db.DispensedItem.Find(stockmovement.DispensedItemUID);
                            db.DispensedItem.Attach(dispensedItem);
                            dispensedItem.MUser = userUID;
                            dispensedItem.MWhen = now;
                            dispensedItem.ORDSTUID = 2875;


                            PrescriptionItem prescriptionItem = db.PrescriptionItem.Find(stockmovement.PresciptionItemUID);
                            db.PrescriptionItem.Attach(prescriptionItem);
                            prescriptionItem.MUser = userUID;
                            prescriptionItem.MWhen = now;
                            prescriptionItem.ORDSTUID = 2875;


                            Prescription prescription = db.Prescription.Find(stockmovement.PrescriptionUID);
                            db.Prescription.Attach(prescription);
                            prescription.MUser = userUID;
                            prescription.MWhen = now;
                            prescription.ORDSTUID = 2875;


                            PatientOrderDetail patientOrderDetail = db.PatientOrderDetail.Find(item.PatientOrderDetailUID);
                            db.PatientOrderDetail.Attach(patientOrderDetail);
                            patientOrderDetail.MUser = userUID;
                            patientOrderDetail.MWhen = now;
                            patientOrderDetail.ORDSTUID = 2875;


                            PatientBillableItem patientBillable = db.PatientBillableItem.Where(p => p.PatientOrderDetailUID == item.PatientOrderDetailUID && p.IPFillProcessUID == ipfillProccessUID && p.StatusFlag == "A").FirstOrDefault(); ;
                            if (patientBillable != null)
                            {
                                db.PatientBillableItem.Attach(patientBillable);
                                patientBillable.MUser = userUID;
                                patientBillable.MWhen = now;
                                patientBillable.ORDSTUID = 2875;
                                patientBillable.StatusFlag = "D";
                            }

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

                            PatientOrderDetailHistory patOrderDetailHistory = new PatientOrderDetailHistory();
                            patOrderDetailHistory.PatientOrderDetailUID = patientOrderDetail.UID;
                            patOrderDetailHistory.ORDSTUID = 2875;
                            patOrderDetailHistory.EditByUserID = userUID;
                            patOrderDetailHistory.EditedDttm = now;
                            patOrderDetailHistory.CUser = userUID;
                            patOrderDetailHistory.CWhen = now;
                            patOrderDetailHistory.MUser = userUID;
                            patOrderDetailHistory.MWhen = now;
                            patOrderDetailHistory.StatusFlag = "A";
                            db.PatientOrderDetailHistory.Add(patOrderDetailHistory);
                            db.SaveChanges();

                            SaleReturn saleReturn = new SaleReturn();
                            int saleReturnID;
                            string returnID = SEQHelper.GetSEQIDFormat("SEQSaleReturn", out saleReturnID);

                            if (string.IsNullOrEmpty(returnID))
                            {
                                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No SEQSaleReturn in SEQCONFIGURATION");
                            }

                            if (saleReturnID == 0)
                            {
                                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Insert SEQSaleReturn is Fail");
                            }
                            saleReturn.ReturnID = returnID;
                            saleReturn.IsCancelDispense = "Y";
                            saleReturn.ReturnedBy = userUID;
                            saleReturn.ReturnDttm = now;
                            saleReturn.OwnerOrganisationUID = ownerOrganisationUID;
                            saleReturn.PatientUID = item.PatientUID;
                            saleReturn.PatientVisitUID = item.PatientVisitUID;
                            saleReturn.StoreUID = patientOrderDetail.StoreUID ?? 0;
                            saleReturn.CUser = userUID;
                            saleReturn.CWhen = now;
                            saleReturn.MUser = userUID;
                            saleReturn.MWhen = now;
                            saleReturn.StatusFlag = "A";
                            db.SaleReturn.Add(saleReturn);
                            db.SaveChanges();

                            SaleReturnList saleReturnList = new SaleReturnList();
                            saleReturnList.SaleReturnUID = saleReturn.UID;
                            saleReturnList.BatchID = stock.BatchID;
                            saleReturnList.ItemMasterUID = stock.ItemMasterUID;
                            saleReturnList.ItemName = stock.ItemName;
                            saleReturnList.Quantity = item.Quantity ?? 0;
                            saleReturnList.DispensedItemUID = dispensedItem.UID;
                            saleReturnList.IMUOMUID = stock.IMUOMUID;
                            saleReturnList.ItemCost = stock.ItemCost;
                            saleReturnList.StockUID = stock.UID;
                            saleReturnList.OwnerOrganisationUID = ownerOrganisationUID;
                            saleReturnList.PatientOrderDetailUID = patientOrderDetail.UID;
                            saleReturnList.PatientBilledItemUID = patientBillable.UID;
                            saleReturnList.CUser = userUID;
                            saleReturnList.CWhen = now;
                            saleReturnList.MUser = userUID;
                            saleReturnList.MWhen = now;
                            saleReturnList.StatusFlag = "A";
                            db.SaleReturnList.Add(saleReturnList);
                            db.SaveChanges();
                        }
                    }
                }

                IPFillProcess fillProcess = db.IPFillProcess.Find(ipfillProccessUID);
                if (fillProcess != null)
                {
                    db.IPFillProcess.Attach(fillProcess);
                    fillProcess.MUser = userUID;
                    fillProcess.MWhen = now;
                    fillProcess.StatusFlag = "D";
                    db.SaveChanges();
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("SearchDispenseReturn")]
        [HttpGet]
        public List<SaleReturnModel> SearchDispenseReturn(DateTime? dateFrom, DateTime? dateTo, long? patientUID, int? storeUID)
        {
            List<SaleReturnModel> data = db.SaleReturn.Where(p => p.StatusFlag == "A"
             && (dateFrom == null || DbFunctions.TruncateTime(p.ReturnDttm) >= DbFunctions.TruncateTime(dateFrom))
             && (dateTo == null || DbFunctions.TruncateTime(p.ReturnDttm) <= DbFunctions.TruncateTime(dateTo))
             && (storeUID == null || p.StoreUID == storeUID)
             && (patientUID == null || p.PatientUID == patientUID))
                .Select(p => new SaleReturnModel()
                {
                    SaleReturnUID = p.UID,
                    StoreUID = p.StoreUID,
                    StoreName = SqlFunction.fGetStoreName(p.StoreUID),
                    PatientUID = p.PatientUID ?? 0,
                    PatientName = SqlFunction.fGetPatientName(p.PatientUID ?? 0),
                    Comments = p.Comments,
                    PatientVisitUID = p.PatientVisitUID ?? 0,
                    IsCancelDispense = p.IsCancelDispense,
                    SaleUID = p.SaleUID ?? 0,
                    OwnerOrganisationUID = p.OwnerOrganisationUID,
                    ReturnDttm = p.ReturnDttm,
                    ReturnedBy = p.ReturnedBy,
                    ReturnedByName = SqlFunction.fGetCareProviderName(p.ReturnedBy),
                    ReturnID = p.ReturnID
                }).OrderByDescending(p => p.ReturnDttm).ToList();

            return data;
        }

        [Route("GetDispenseReturnList")]
        [HttpGet]
        public List<SaleReturnListModel> GetDispenseReturnList(int saleReturnUID)
        {
            List<SaleReturnListModel> data = db.SaleReturnList.Where(p => p.StatusFlag == "A" && p.SaleReturnUID == saleReturnUID)
                .Select(p => new SaleReturnListModel()
                {
                    SaleReturnListUID = p.UID,
                    SaleReturnUID = p.SaleReturnUID,
                    StockUID = p.StockUID,
                    BatchID = p.BatchID,
                    ItemMasterUID = p.ItemMasterUID,
                    ItemName = p.ItemName,
                    Quantity = p.Quantity,
                    IMUOMUID = p.IMUOMUID,
                    IMUOM = SqlFunction.fGetRfValDescription(p.IMUOMUID),
                    Comments = p.Comments,
                    DispensedItemUID = p.DispensedItemUID ?? 0,
                    PatientBilledItemUID = p.PatientBilledItemUID ?? 0,
                    PatientOrderDetailUID = p.PatientOrderDetailUID ?? 0,
                    ItemCost = p.ItemCost ?? 0,

                }).ToList();

            return data;
        }

        [Route("GetPrescriptionDispenseReturn")]
        [HttpGet]
        public List<PrescriptionItemModel> GetPrescriptionDispenseReturn(long? patientUID, int? storeUID)
        {
            List<PrescriptionItemModel> data = (from ps in db.Prescription
                                                join pst in db.PrescriptionItem on ps.UID equals pst.PrescriptionUID
                                                join pa in db.Patient on ps.PatientUID equals pa.UID
                                                join pv in db.PatientVisit on ps.PatientVisitUID equals pv.UID
                                                where ps.StatusFlag == "A"
                                                select new PrescriptionItemModel
                                                {
                                                    PrescriptionItemUID = pst.UID,
                                                    PrescriptionUID = ps.UID,
                                                    PrestionItemStatus = SqlFunction.fGetRfValDescription(ps.ORDSTUID ?? 0),
                                                    ORDSTUID = ps.ORDSTUID,
                                                    ItemCode = pst.ItemCode,
                                                    ItemName = pst.ItemName,
                                                    ItemMasterUID = pst.ItemMasterUID,
                                                    Quantity = pst.Quantity,
                                                    QuantityUnit = SqlFunction.fGetRfValDescription(pst.IMUOMUID ?? 0),
                                                    IMUOMUID = pst.IMUOMUID,
                                                    DFORMUID = pst.DFORMUID,
                                                    DrugForm = SqlFunction.fGetRfValDescription(pst.DFORMUID ?? 0),
                                                    StoreUID = pst.StoreUID,
                                                    StoreName = SqlFunction.fGetStoreName(pst.StoreUID ?? 0),
                                                    InstructionRoute = SqlFunction.fGetRfValDescription(pst.PDSTSUID ?? 0),
                                                    Dosage = pst.Dosage,
                                                    FRQNCUID = pst.FRQNCUID,
                                                    Frequency = SqlFunction.fGetFrequencyDescription(pst.FRQNCUID ?? 0, "TH"),
                                                    InstructionText = pst.InstructionText,
                                                    LocalInstructionText = pst.LocalInstructionText,
                                                    ClinicalComments = pst.ClinicalComments,
                                                    DrugType = SqlFunction.fGetRfValDescription(pst.DFORMUID ?? 0)
                                                }).ToList();

            //foreach (var item in data)
            //{

            //    IEnumerable<Stock> stockItem = db.Stock
            //    .Where(p => p.StatusFlag == "A"
            //    && p.ItemMasterUID == item.ItemMasterUID
            //    && p.StoreUID == item.StoreUID && p.Quantity > 0);
            //    if (stockItem != null && stockItem.Count() > 0)
            //    {
            //        Store store = db.Store.Find(item.StoreUID);
            //        if (store.STDTPUID == 2901)
            //            stockItem = stockItem.OrderBy(p => p.ExpiryDttm).ThenBy(p => p.CWhen);
            //        else if (store.STDTPUID == 2902)
            //            stockItem = stockItem.OrderBy(p => p.CWhen).ThenBy(p => p.ExpiryDttm);

            //        item.ExpiryDate = stockItem.FirstOrDefault().ExpiryDttm;

            //    }

            //    if (item.FRQNCUID != null && item.FRQNCUID != 0)
            //    {
            //        item.Frequency = db.FrequencyDefinition.Find(item.FRQNCUID).Comments;
            //    }
            //}
            return data;

        }
        #endregion
    }
}
