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
using System.Data.Entity;

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
        public List<StockOnHandModel> SearchStockOnHand(int? ownerOrganisationUID, int? storeUID, int? itemType, string itemCode, string itemName)
        {
            DataTable dt = SqlDirectStore.pSearchStockOnHand(ownerOrganisationUID, storeUID, itemType, itemCode, itemName);
            List<StockOnHandModel> data = dt.ToList<StockOnHandModel>();

            return data;
        }

        [Route("SearchStockMovement")]
        [HttpGet]
        public List<StockMovementModel> SearchStockMovement(int? ownerOrganisationUID, int? storeUID, string itemCode, string itemName, string transactionType, DateTime? dateFrom, DateTime? dateTo)
        {
            DataTable dt = SqlDirectStore.pSearchStockMovement(ownerOrganisationUID, storeUID, itemCode, itemName, transactionType, dateFrom, dateTo);
            List<StockMovementModel> data = dt.ToList<StockMovementModel>();

            return data;
        }

        [Route("SearchStockBalance")]
        [HttpGet]
        public List<StockBalanceModel> SearchStockBalance(int? ownerOrganisationUID, int? storeUID, string itemCode, string itemName, DateTime? dateFrom, DateTime? dateTo)
        {
            DataTable dt = SqlDirectStore.pSearchStockBalance(ownerOrganisationUID, storeUID, itemCode, itemName, dateFrom, dateTo);
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

        [Route("SearchStockBatch")]
        [HttpGet]
        public List<StockModel> SearchStockBatch(int? organisationUID, int? storeUID, int? itemType, string itemCode, string itemName)
        {
            DataTable data = SqlDirectStore.pSearchStockBatch(organisationUID, storeUID, itemType, itemCode, itemName);
            List<StockModel> returnData = data.ToList<StockModel>();

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
        public List<StockWorkListModel> SearchStockWorkList(DateTime? dateFrom, DateTime? dateTo, int? organisationUID)
        {
            DataTable data = SqlDirectStore.pSearchStockWorkList(dateFrom, dateTo, organisationUID);
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
        public List<ItemRequestModel> SearchItemRequest(DateTime? dateFrom, DateTime? dateTo, string requestID, int? organisationUID, int? organisationToUID, int? requestStatus, int? priority)
        {
            List<ItemRequestModel> data = (from j in db.ItemRequest
                                           where j.StatusFlag == "A"
                                           && (dateFrom == null || DbFunctions.TruncateTime(j.RequestedDttm) >= DbFunctions.TruncateTime(dateFrom))
                                           && (dateTo == null || DbFunctions.TruncateTime(j.RequestedDttm) <= DbFunctions.TruncateTime(dateTo))
                                           && (string.IsNullOrEmpty(requestID) || j.ItemRequestID == requestID)
                                           && (organisationUID == null || j.OrganisationUID == organisationUID)
                                           && (organisationToUID == null || j.RequestOnOrganistaionUID == organisationToUID)
                                           && (requestStatus == null || j.RQSTSUID == requestStatus)
                                           && (priority == null || j.IRPRIUID == priority)
                                           select new ItemRequestModel
                                           {
                                               OrganisationUID = j.OrganisationUID,
                                               StoreUID = j.StoreUID,
                                               RequestOnOrganistaionUID = j.RequestOnOrganistaionUID,
                                               RequestOnStoreUID = j.RequestOnStoreUID,
                                               Organisation = SqlFunction.fGetHealthOrganisationName(j.OrganisationUID),
                                               Store = SqlFunction.fGetStoreName(j.StoreUID),
                                               RequestOnOrganisation = SqlFunction.fGetHealthOrganisationName(j.RequestOnOrganistaionUID),
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
                                         StoreUID = j.StoreUID,
                                         RequestOnOrganistaionUID = j.RequestOnOrganistaionUID,
                                         RequestOnStoreUID = j.RequestOnStoreUID,
                                         Organisation = SqlFunction.fGetHealthOrganisationName(j.OrganisationUID),
                                         Store = SqlFunction.fGetStoreName(j.StoreUID),
                                         RequestOnOrganisation = SqlFunction.fGetHealthOrganisationName(j.RequestOnOrganistaionUID),
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
                    itemREQ.StoreUID = model.StoreUID;
                    itemREQ.RequestOnOrganistaionUID = model.RequestOnOrganistaionUID;
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
        public List<ItemIssueModel> SearchItemIssue(DateTime? dateFrom, DateTime? dateTo, string issueID, int ISSTPUID, int? issueStatus, int? organisationUID, int? organisationToUID)
        {
            List<ItemIssueModel> data = (from j in db.ItemIssue
                                         where j.StatusFlag == "A"
                                         && j.ISSTPUID == ISSTPUID
                                         && (issueStatus == null || j.ISUSTUID == issueStatus)
                                         && (dateFrom == null || DbFunctions.TruncateTime(j.ItemIssueDttm) >= DbFunctions.TruncateTime(dateFrom))
                                         && (dateTo == null || DbFunctions.TruncateTime(j.ItemIssueDttm) <= DbFunctions.TruncateTime(dateTo))
                                         && (string.IsNullOrEmpty(issueID) || j.ItemIssueID == issueID)
                                         && (organisationUID == null || j.OrganisationUID == organisationUID)
                                         && (organisationToUID == null || j.RequestedByOrganisationUID == organisationToUID)
                                         select new ItemIssueModel
                                         {
                                             OrganisationUID = j.OrganisationUID,
                                             StoreUID = j.StoreUID,
                                             RequestedByOrganisationUID = j.RequestedByOrganisationUID,
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
        public List<ItemIssueModel> SearchItemIssueForListIssue(DateTime? dateFrom, DateTime? dateTo, string issueID, int? issueStatus, int? organisationUID, int? organisationToUID)
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
                                         && (organisationToUID == null || j.RequestedByOrganisationUID == organisationToUID)
                                         select new ItemIssueModel
                                         {
                                             OrganisationUID = j.OrganisationUID,
                                             StoreUID = j.StoreUID,
                                             RequestedByOrganisationUID = j.RequestedByOrganisationUID,
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
                                       StoreUID = j.StoreUID,
                                       RequestedByOrganisationUID = j.RequestedByOrganisationUID,
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

        [Route("ConsumptionItem")]
        [HttpPost]
        public HttpResponseMessage ConsumptionItem(IEnumerable<ItemIssueDetailModel> itemIssueDetailsModel,string comments, int userUID)
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
        public List<ItemReceiveModel> SearchItemReceive(DateTime? dateFrom, DateTime? dateTo, string receiveID, int? organisationIssueUID, int? organisationReceiveUID)
        {
            List<ItemReceiveModel> data = (from j in db.ItemReceive
                                           where j.StatusFlag == "A"
                                           && (dateFrom == null || DbFunctions.TruncateTime(j.ReceivedDttm) >= DbFunctions.TruncateTime(dateFrom))
                                           && (dateTo == null || DbFunctions.TruncateTime(j.ReceivedDttm) <= DbFunctions.TruncateTime(dateTo))
                                           && (string.IsNullOrEmpty(receiveID) || j.ItemReceiveID == receiveID)
                                           && (organisationReceiveUID == null || j.OrganisationUID == organisationReceiveUID)
                                           && (organisationIssueUID == null || j.IssuedByOrganisationUID == organisationIssueUID)
                                           select new ItemReceiveModel
                                           {
                                               OrganisationUID = j.OrganisationUID,
                                               StoreUID = j.StoreUID,
                                               IssuedByOrganisationUID = j.IssuedByOrganisationUID,
                                               IssuedByStoreUID = j.IssuedByStoreUID,
                                               Organisation = SqlFunction.fGetHealthOrganisationName(j.OrganisationUID),
                                               Store = SqlFunction.fGetStoreName(j.StoreUID),
                                               IssuedByOrganisation = SqlFunction.fGetHealthOrganisationName(j.IssuedByOrganisationUID),
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
                                         StoreUID = j.StoreUID,
                                         IssuedByOrganisationUID = j.IssuedByOrganisationUID,
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
                    itemReceive.StoreUID = model.StoreUID;
                    itemReceive.IssuedByOrganisationUID = model.IssuedByOrganisationUID;
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

        #region Prescription

        [Route("Searchprescription")]
        [HttpGet]
        public List<PrescriptionModel> Searchprescription(DateTime? dateFrom, DateTime? dateTo, int? ORDSTUID, long? patientUID
            , string prescriptionNumber, int? organisationUID)
        {
            List<PrescriptionModel> data = db.Prescription
                .Where(p => p.StatusFlag == "A"
                && (dateFrom == null || DbFunctions.TruncateTime(p.PrescribedDttm) >= DbFunctions.TruncateTime(dateFrom))
                && (dateTo == null || DbFunctions.TruncateTime(p.PrescribedDttm) <= DbFunctions.TruncateTime(dateTo))
                && (ORDSTUID == null || p.ORDSTUID == ORDSTUID)
                && (patientUID == null || p.PatientUID == patientUID)
                && (string.IsNullOrEmpty(prescriptionNumber) || p.PrescriptionNumber == prescriptionNumber)
                && (organisationUID == null || p.OwnerOrganisationUID == organisationUID)
                ).Select(p => new PrescriptionModel
                {
                    PrescriptionUID = p.UID,
                    PrescriptionNumber = p.PrescriptionNumber,
                    PrescriptionStatus = SqlFunction.fGetRfValDescription(p.ORDSTUID ?? 0),
                    PatientID = SqlFunction.fGetPatientID(p.PatientUID),
                    PatientName = SqlFunction.fGetPatientName(p.PatientUID),
                    PrescribedDttm = p.PrescribedDttm,
                    OrganisationName = SqlFunction.fGetHealthOrganisationName(p.OwnerOrganisationUID ?? 0),
                    OwnerOrganisationUID = p.OwnerOrganisationUID
                }).ToList();


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
                    PrestionItemStatus = SqlFunction.fGetRfValDescription(p.ORDSTUID ?? 0),
                    ItemName = p.ItemName,
                    ItemMasterUID = p.ItemMasterUID,
                    Quantity = p.Quantity,
                    QuantityUnit = SqlFunction.fGetRfValDescription(p.IMUOMUID ?? 0),
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

    }
}
