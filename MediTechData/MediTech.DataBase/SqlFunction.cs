using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.DataBase
{
    public static class SqlFunction
    {
        [DbFunction("MediTechModel.Store", "fGetRfValDescription")]
        public static string fGetRfValDescription(int referencevalueUID)
        {
            throw new NotSupportedException("Direct calls are not supported.");
        }

        [DbFunction("MediTechModel.Store", "fGetRfValCode")]
        public static string fGetRfValCode(int referencevalueUID)
        {
            throw new NotSupportedException("Direct calls are not supported.");
        }

        [DbFunction("MediTechModel.Store", "fGetAge")]
        public static string fGetAge(DateTime birthDttm)
        {
            throw new NotSupportedException("Direct calls are not supported.");
        }


        [DbFunction("MediTechModel.Store", "fGetAgeString")]
        public static string fGetAgeString(DateTime birthDttm)
        {
            throw new NotSupportedException("Direct calls are not supported.");
        }

        [DbFunction("MediTechModel.Store", "fGetCareProviderName")]
        public static string fGetCareProviderName(int careproviderUID)
        {
            throw new NotSupportedException("Direct calls are not supported.");
        }

        [DbFunction("MediTechModel.Store", "fGetCareProviderEngName")]
        public static string fGetCareProviderEngName(int careproviderUID)
        {
            throw new NotSupportedException("Direct calls are not supported.");
        }

        [DbFunction("MediTechModel.Store", "fGetCareProviderLicenseNo")]
        public static string fGetCareProviderLicenseNo(int careproviderUID)
        {
            throw new NotSupportedException("Direct calls are not supported.");
        }

        [DbFunction("MediTechModel.Store", "fGetPatientName")]
        public static string fGetPatientName(long patientUID)
        {
            throw new NotSupportedException("Direct calls are not supported.");
        }

        [DbFunction("MediTechModel.Store", "fGetPatientID")]
        public static string fGetPatientID(long patientUID)
        {
            throw new NotSupportedException("Direct calls are not supported.");
        }

        [DbFunction("MediTechModel.Store", "fGetVisitID")]
        public static string fGetVisitID(long patientVisitUID)
        {
            throw new NotSupportedException("Direct calls are not supported.");
        }

        [DbFunction("MediTechModel.Store", "fGetRfValUIDByCode")]
        public static int fGetRfValUIDByCode(string domainCode, string valueCode)
        {
            throw new NotSupportedException("Direct calls are not supported.");
        }

        [DbFunction("MediTechModel.Store", "fGetHealthOrganisationName")]
        public static string fGetHealthOrganisationName(int ownerOrganisationUID)
        {
            throw new NotSupportedException("Direct calls are not supported.");
        }

        [DbFunction("MediTechModel.Store", "fGetHealthOrganisationHeader")]
        public static string fGetHealthOrganisationHeader(int ownerOrganisationUID)
        {
            throw new NotSupportedException("Direct calls are not supported.");
        }

        [DbFunction("MediTechModel.Store", "fGetVendorName")]
        public static string fGetVendorName(int vendorDetailUID)
        {
            throw new NotSupportedException("Direct calls are not supported.");
        }

        [DbFunction("MediTechModel.Store", "fGetStoreName")]
        public static string fGetStoreName(int storeUID)
        {
            throw new NotSupportedException("Direct calls are not supported.");
        }
        [DbFunction("MediTechModel.Store", "fGetAddressPatient")]
        public static string fGetAddressPatient(long patientUID)
        {
            throw new NotSupportedException("Direct calls are not supported.");
        }


        [DbFunction("MediTechModel.Store", "fGetAddressOrganisation")]
        public static string fGetAddressOrganisation(int organisationUID)
        {
            throw new NotSupportedException("Direct calls are not supported.");
        }
        [DbFunction("MediTechModel.Store", "fGetAddressVendor")]
        public static string fGetAddressVendor(int vendorDetalUID)
        {
            throw new NotSupportedException("Direct calls are not supported.");
        }


        [DbFunction("MediTechModel.Store", "fGetAddressPayorDetail")]
        public static string fGetAddressPayorDetail(int payorDetailUID)
        {
            throw new NotSupportedException("Direct calls are not supported.");
        }

        [DbFunction("MediTechModel.Store", "fGetPayorName")]
        public static string fGetPayorName(int payorDetailUID)
        {
            throw new NotSupportedException("Direct calls are not supported.");
        }

        [DbFunction("MediTechModel.Store", "fGetStockQuantity")]
        public static string fGetStockQuantity(int itemMasterUID, int storeUID)
        {
            throw new NotSupportedException("Direct calls are not supported.");
        }

        [DbFunction("MediTechModel.Store", "fGetItemTotalQuantity")]
        public static double fGetItemTotalQuantity(int itemMasterUID, int storeUID,int ownerOrganisation)
        {
            throw new NotSupportedException("Direct calls are not supported.");
        }

        [DbFunction("MediTechModel.Store", "fGetItemAverageCost")]
        public static double fGetItemAverageCost(int itemMasterUID, int ownerOrganisation)
        {
            throw new NotSupportedException("Direct calls are not supported.");
        }

        [DbFunction("MediTechModel.Store", "fGetBillingGroupDesc")]
        public static string fGetBillingGroupDesc(int groupUID,string type)
        {
            throw new NotSupportedException("Direct calls are not supported.");
        }

        [DbFunction("MediTechModel.Store", "fGetBillableItemPrice")]
        public static Double fGetBillableItemPrice(int billingItemUID, int ownerOrganisationUID)
        {
            throw new NotSupportedException("Direct calls are not supported.");
        }

        [DbFunction("MediTechModel.Store", "fGetPatientAllergy")]
        public static string fGetPatientAllergy(long patientUID)
        {
            throw new NotSupportedException("Direct calls are not supported.");
        }


        [DbFunction("MediTechModel.Store", "fSetItemNameSearch")]
        public static string fSetItemNameSearch(string itemName)
        {
            throw new NotSupportedException("Direct calls are not supported.");
        }
    }

    public static class SqlDirectStore
    {
        public static bool pCancelDispensed(long PatientOrderDetailUID, int userUID)
        {
            bool flag = false;
            MediTechEntities entities = new MediTechEntities();

            SqlConnection con = new SqlConnection(entities.Database.Connection.ConnectionString);
            try
            {

                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "pCancelDispensed";

                cmd.Parameters.AddWithValue("@P_PatientOrderDetailUID", PatientOrderDetailUID);
                cmd.Parameters.AddWithValue("@P_UserUID", userUID);

                cmd.ExecuteScalar();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                con.Close();
            }
            return flag;
        }
        public static DataTable pDispensePrescriptionItem(long prescriptionItemUID, int userUID)
        {
            DataTable dt = new DataTable();
            MediTechEntities entities = new MediTechEntities();

            SqlConnection con = new SqlConnection(entities.Database.Connection.ConnectionString);
            try
            {
                SqlDataAdapter da = new SqlDataAdapter();
                 da.SelectCommand = new SqlCommand("pDispensePrescriptionItem", con);
                da.SelectCommand.Parameters.AddWithValue("@P_PrescriptionItemUID", prescriptionItemUID);
                da.SelectCommand.Parameters.AddWithValue("@P_UserUID", userUID);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;

                DataSet ds = new DataSet();
                da.Fill(ds);
                dt = ds.Tables[ds.Tables.Count - 1];
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                //con.Close();
            }
            return dt;
        }


        public static bool pInvenAdjustStock(string stockAdjustmentID,int storeUID,int stockUID,int itemMasterUID,string itemName,string batchID,double actualQuantity,int actualUOM,double quantityAdjusted,double adjustedQuantity,int adjustedUOM,string comments,double itemCost,DateTime? expiryDate,int userUID,int organisationUID)
        {
            bool flag = false;
            MediTechEntities entities = new MediTechEntities();

            SqlConnection con = new SqlConnection(entities.Database.Connection.ConnectionString);
            try
            {

                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "pInvenAdjustStock";
                cmd.Parameters.AddWithValue("@P_StockAdjustmentID", stockAdjustmentID);
                cmd.Parameters.AddWithValue("@P_StoreUID", storeUID);
                cmd.Parameters.AddWithValue("@P_StockUID", stockUID);
                cmd.Parameters.AddWithValue("@P_ItemMasterUID", itemMasterUID);
                cmd.Parameters.AddWithValue("@P_ItemName", itemName);
                cmd.Parameters.AddWithValue("@P_BatchID", batchID);
                cmd.Parameters.AddWithValue("@P_ActualQuantity", actualQuantity);
                cmd.Parameters.AddWithValue("@P_ActualUOM", actualUOM);
                cmd.Parameters.AddWithValue("@P_QuantityAdjusted", quantityAdjusted);
                cmd.Parameters.AddWithValue("@P_AdjustedQuantity", adjustedQuantity);
                cmd.Parameters.AddWithValue("@P_AdjustedUOM", adjustedUOM);
                cmd.Parameters.AddWithValue("@P_Comments", comments ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@P_ItemCost", itemCost);
                cmd.Parameters.AddWithValue("@P_ExpiryDate", expiryDate ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@P_UserID", userUID);
                cmd.Parameters.AddWithValue("@P_OrganisationUID", organisationUID);

                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                con.Close();
            }
            return flag;
        }
        public static bool pInvenGoodReceive(int itemMasterUID, int storeUID,int? toStoreUID, int userUID, int organisationUID, string comments, int IMUOMUID, DateTime? expiryDate, string batchID, double quantity, double itemCost, int? VendorDetailUID, int? ManufacturerUID, int? GRNDeailUID, DateTime? GRNDttm, string refNo, string refTable, int? refUID, string noteMovement)
        {
            bool flag = false;
            MediTechEntities entities = new MediTechEntities();

            SqlConnection con = new SqlConnection(entities.Database.Connection.ConnectionString);
            try
            {

                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "pInvenGoodReceive";

                cmd.Parameters.AddWithValue("@P_ItemMasterUID", itemMasterUID);
                cmd.Parameters.AddWithValue("@P_StoreUID", storeUID);
                cmd.Parameters.AddWithValue("@P_ToStoreUID", toStoreUID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@P_UserUID", userUID);
                cmd.Parameters.AddWithValue("@P_Organisation", organisationUID);
                cmd.Parameters.AddWithValue("@P_Comments", comments ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@P_IMUOMUID", IMUOMUID);
                cmd.Parameters.AddWithValue("@P_ExpiryDate", expiryDate ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@P_BatchID", batchID ?? "");
                cmd.Parameters.AddWithValue("@P_Quantity", quantity);
                cmd.Parameters.AddWithValue("@P_ItemCost", itemCost);
                cmd.Parameters.AddWithValue("@P_VendorDetailUID", VendorDetailUID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@P_ManufacturerUID", ManufacturerUID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@P_GRNDetailUID", GRNDeailUID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@P_GRNDttm", GRNDttm ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@P_RefNo", refNo ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@P_RefTable", refTable ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@P_RefUID", refUID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@P_NoteMovement", noteMovement ?? (object)DBNull.Value);

                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                con.Close();
            }
            return flag;
        }

        public static bool pInvenIssueItem(int itemIssueUID, int userUID)
        {
            bool flag = false;
            MediTechEntities entities = new MediTechEntities();

            SqlConnection con = new SqlConnection(entities.Database.Connection.ConnectionString);
            try
            {

                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "pInvenIssueItem";

                cmd.Parameters.AddWithValue("@P_ItemIssueUID", itemIssueUID);
                cmd.Parameters.AddWithValue("@P_UserUID", userUID);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                con.Close();
            }

            return flag;
        }

        public static bool pInvenTransferItem(int itemIssueUID,int userUID)
        {
            bool flag = false;
            MediTechEntities entities = new MediTechEntities();

            SqlConnection con = new SqlConnection(entities.Database.Connection.ConnectionString);
            try
            {

                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "pInvenTransferItem";

                cmd.Parameters.AddWithValue("@P_ItemIssueUID", itemIssueUID);
                cmd.Parameters.AddWithValue("@P_UserUID", userUID);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                con.Close();
            }

            return flag;
        }

        public static bool pInvenReceiveItem(int itemReceiveUID, int userUID)
        {
            bool flag = false;
            MediTechEntities entities = new MediTechEntities();

            SqlConnection con = new SqlConnection(entities.Database.Connection.ConnectionString);
            try
            {

                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "pInvenReceiveItem";

                cmd.Parameters.AddWithValue("@P_ItemReceiveUID", itemReceiveUID);
                cmd.Parameters.AddWithValue("@P_UserUID", userUID);

                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                con.Close();
            }

            return flag;
        }

        public static bool pInvenInsertStockBalance(int storeUID, int itemMasterUID, int userUID)
        {
            bool flag = false;
            MediTechEntities entities = new MediTechEntities();

            SqlConnection con = new SqlConnection(entities.Database.Connection.ConnectionString);
            try
            {

                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "pInvenInsertStockBalance";

                cmd.Parameters.AddWithValue("@P_StoreUID", storeUID);
                cmd.Parameters.AddWithValue("@P_ItemMasterUID", itemMasterUID);
                cmd.Parameters.AddWithValue("@P_UserUID", userUID);

                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                con.Close();
            }
            return flag;
        }

        public static bool pInvenInsertItemAverageCost(int itemMasterUID, int inQty, double unitCost, int IMUOMUID, int beforeQty, int beforeCost, string refNo, string refTable, int? refUID, string note, int userUID, int organisationUID)
        {
            bool flag = false;
            MediTechEntities entities = new MediTechEntities();

            SqlConnection con = new SqlConnection(entities.Database.Connection.ConnectionString);
            try
            {

                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "pInvenInsertItemAverageCost";

                cmd.Parameters.AddWithValue("@P_ItemMasterUID", itemMasterUID);
                cmd.Parameters.AddWithValue("@P_InQty", inQty);
                cmd.Parameters.AddWithValue("@P_UnitCost", unitCost);
                cmd.Parameters.AddWithValue("@P_IMUOMUID", IMUOMUID);
                cmd.Parameters.AddWithValue("@P_BeforeQty", beforeQty);
                cmd.Parameters.AddWithValue("@P_BeforeCost", beforeCost);
                cmd.Parameters.AddWithValue("@P_RefNo", refNo);
                cmd.Parameters.AddWithValue("@P_RefTable", refTable ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@P_RefUID", refUID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@P_Note", note ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@P_UserUID", userUID);
                cmd.Parameters.AddWithValue("@P_OrganisationUID", organisationUID);

                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                con.Close();
            }
            return flag;
        }
        public static bool pInvenInsertStockMovement(int stockUID, int storeUID,int? toStoreUID, int itemMasterUID, string batchID, DateTime stockDttm,double totalBFQty
            , double bfQty, double inQty, double outQty, double balQty,double totalBalQty, int IMUOMUID,double unitCost ,string refNo, string refTable, int? refUID, long? refPatientUID, string note, int userUID)
        {
            bool flag = false;
            MediTechEntities entities = new MediTechEntities();

            SqlConnection con = new SqlConnection(entities.Database.Connection.ConnectionString);
            try
            {

                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "pInvenInsertStockMovement";

                cmd.Parameters.AddWithValue("@P_StockUID", stockUID);
                cmd.Parameters.AddWithValue("@P_StoreUID", storeUID);
                cmd.Parameters.AddWithValue("@P_ToStoreUID", toStoreUID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@P_ItemMasterUID", itemMasterUID);
                cmd.Parameters.AddWithValue("@P_BatchID", batchID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@P_StokDttm", stockDttm);
                cmd.Parameters.AddWithValue("@p_TotalBFQty", totalBFQty);
                cmd.Parameters.AddWithValue("@P_BFQty", bfQty);
                cmd.Parameters.AddWithValue("@P_INQty", inQty);
                cmd.Parameters.AddWithValue("@P_OUTQty", outQty);
                cmd.Parameters.AddWithValue("@P_BalQty", balQty);
                cmd.Parameters.AddWithValue("@P_TotalBalQty", totalBalQty);
                cmd.Parameters.AddWithValue("@P_IMUOMUID", IMUOMUID);
                cmd.Parameters.AddWithValue("@P_UnitCost", unitCost);
                cmd.Parameters.AddWithValue("@P_RefNo", refNo ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@P_RefTable", refTable ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@P_RefUID", refUID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@P_RefPatientUID", refPatientUID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@P_Note", note ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@P_UserUID", userUID);

                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                con.Close();
            }
            return flag;
        }

        public static bool pInvenCancelGRNDetail(int grnDetailUID, string cancelReason, int userUID)
        {
            bool flag = false;
            MediTechEntities entities = new MediTechEntities();

            SqlConnection con = new SqlConnection(entities.Database.Connection.ConnectionString);
            try
            {

                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "pInvenCancelGRNDetail";

                cmd.Parameters.AddWithValue("@P_GRNDetailUID", grnDetailUID);
                cmd.Parameters.AddWithValue("@P_CancelReason", cancelReason);
                cmd.Parameters.AddWithValue("@P_UserUID", userUID);

                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                con.Close();
            }

            return flag;
        }

        public static bool pInvenCancelItemIssue(int itemIssueUID, string cancelReason, int userUID)
        {
            bool flag = false;
            MediTechEntities entities = new MediTechEntities();

            SqlConnection con = new SqlConnection(entities.Database.Connection.ConnectionString);
            try
            {

                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "pInvenCancelItemIssue";

                cmd.Parameters.AddWithValue("@P_ItemIssueUID", itemIssueUID);
                cmd.Parameters.AddWithValue("@P_CancelReason", cancelReason ?? "");
                cmd.Parameters.AddWithValue("@P_UserUID", userUID);

                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                con.Close();
            }

            return flag;
        }

        public static bool pInvenCancelTransferItem(int itemIssueUID, string cancelReason, int userUID)
        {
            bool flag = false;
            MediTechEntities entities = new MediTechEntities();

            SqlConnection con = new SqlConnection(entities.Database.Connection.ConnectionString);
            try
            {

                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "pInvenCancelTransferItem";

                cmd.Parameters.AddWithValue("@P_ItemIssueUID", itemIssueUID);
                cmd.Parameters.AddWithValue("@P_CancelReason", cancelReason ?? "");
                cmd.Parameters.AddWithValue("@P_UserUID", userUID);

                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                con.Close();
            }

            return flag;
        }

        public static bool pInvenCancelItemReceive(int itemReceiveUID, string cancelReason, int userUID)
        {
            bool flag = false;
            MediTechEntities entities = new MediTechEntities();

            SqlConnection con = new SqlConnection(entities.Database.Connection.ConnectionString);
            try
            {

                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "pInvenCancelItemReceive";

                cmd.Parameters.AddWithValue("@P_ItemReceiveUID", itemReceiveUID);
                cmd.Parameters.AddWithValue("@P_CancelReason", cancelReason ?? "");
                cmd.Parameters.AddWithValue("@P_UserUID", userUID);

                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                con.Close();
            }

            return flag;
        }

        public static bool pInvenClearItemIssueDetail(int itemIssueUID, int userUID)
        {
            bool flag = false;
            MediTechEntities entities = new MediTechEntities();

            SqlConnection con = new SqlConnection(entities.Database.Connection.ConnectionString);
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "pInvenClearItemIssueDetail";
                cmd.Parameters.AddWithValue("@P_ItemIssueUID", itemIssueUID);
                cmd.Parameters.AddWithValue("@P_UserUID", userUID);

                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                con.Close();
            }

            return flag;
        }

        public static DataTable pStoreConvertUOM(int itemMasterUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pStoreConvertUOM", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@ItemMasterUID", itemMasterUID);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pItemConvertUOM(int itemMasterUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pItemConvertUOM", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@ItemMasterUID", itemMasterUID);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pGetUserByLogins(string userName, string password)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pGetUserByLogins", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_UserName", userName);
            adp.SelectCommand.Parameters.AddWithValue("@P_Password", password);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }
        public static DataTable pGetPatientDataForBanner(long patientUID, long patientVisitUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pGetPatientDataForBanner", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_PatientUID", patientUID);
            adp.SelectCommand.Parameters.AddWithValue("@P_PatientVisitUID", patientVisitUID);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pGetStoreQtyByItemMasterUID(int itemMasterUID, int OrganisationUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pGetStoreQtyByItemMasterUID", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_ItemMasterUID", itemMasterUID);
            adp.SelectCommand.Parameters.AddWithValue("@P_OrganisationUID", OrganisationUID);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pGetRequestByRequestDetailUID(long requestDetailUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pGetRequestByRequestDetailUID", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_RequestDetailUID", requestDetailUID);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pSearchOrderItem(string text, int ownerOrganisationUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pSearchOrderItem", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.Parameters.AddWithValue("@P_Text", text);
            adp.SelectCommand.Parameters.AddWithValue("@P_OwnerOrganisation", ownerOrganisationUID);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pSearchPatient(string patientID, string firstName, string middleName, string lastName, string nickName, DateTime? birthDate
            , int? SEXXXUID, string idCard, DateTime? lastVisitDate)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pSearchPatient", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_PatientID", patientID ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_FirstName", firstName ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_MiddleName ", middleName ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_LastName", lastName ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_NickName", nickName ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_BirthDate", birthDate ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_SEXXXUID", SEXXXUID ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_IDCard", idCard ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_LastVisitDate", lastVisitDate ?? (object)DBNull.Value);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pGetPatientByUID(long patientUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pGetPatientByUID", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_PatientUID", patientUID);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pCheckDupicatePatient(string firstName, string lastName, DateTime birthDate, int SEXXXUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pCheckDupicatePatient", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_FirstName", firstName);
            adp.SelectCommand.Parameters.AddWithValue("@P_LastName", lastName);
            adp.SelectCommand.Parameters.AddWithValue("@P_BirthDttm", birthDate);
            adp.SelectCommand.Parameters.AddWithValue("@P_SEXXXUID", SEXXXUID);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pSearchPatientBill(DateTime? dateFrom, DateTime? dateTo, long? patientUID, string billNumber, int? owerOrganisationUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pSearchPatientBill", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_DateFrom", dateFrom != DateTime.MinValue && dateFrom != null ? dateFrom : (Object)(DBNull.Value));
            adp.SelectCommand.Parameters.AddWithValue("@P_DateTo", dateTo != DateTime.MinValue && dateTo != null ? dateTo : (Object)(DBNull.Value));
            adp.SelectCommand.Parameters.AddWithValue("@P_PatientUID", patientUID != null ? patientUID : (Object)(DBNull.Value));
            adp.SelectCommand.Parameters.AddWithValue("@P_BillNumber", !string.IsNullOrEmpty(billNumber) ? billNumber : (Object)(DBNull.Value));
            adp.SelectCommand.Parameters.AddWithValue("@P_OwnerOrganisatinUID", owerOrganisationUID != null ? owerOrganisationUID : (Object)(DBNull.Value));
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pSearchPatientVisit(string hn, string firstName, string lastName, int? careproviderUID
     , string statusList, DateTime? dateFrom, DateTime? dateTo, DateTime? arrivedDttm, int? ownerOrganisationUID,int? PayorDetailUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pSearchPatientVisit", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@HN", hn ?? "");
            adp.SelectCommand.Parameters.AddWithValue("@FirstName", firstName ?? "");
            adp.SelectCommand.Parameters.AddWithValue("@LastName", lastName ?? "");
            adp.SelectCommand.Parameters.AddWithValue("@CareproViderUID", careproviderUID != null ? careproviderUID : (Object)(DBNull.Value));
            adp.SelectCommand.Parameters.AddWithValue("@StatusList", statusList ?? "");
            adp.SelectCommand.Parameters.AddWithValue("@DateFrom", dateFrom != DateTime.MinValue && dateFrom != null ? dateFrom : (Object)(DBNull.Value));
            adp.SelectCommand.Parameters.AddWithValue("@DateTo", dateTo != DateTime.MinValue && dateTo != null ? dateTo : (Object)(DBNull.Value));
            adp.SelectCommand.Parameters.AddWithValue("@ArrivedDttm", arrivedDttm != DateTime.MinValue && arrivedDttm != null ? arrivedDttm : (Object)(DBNull.Value));
            adp.SelectCommand.Parameters.AddWithValue("@OwnerOrganisation", ownerOrganisationUID != null ? ownerOrganisationUID : (Object)(DBNull.Value));
            adp.SelectCommand.Parameters.AddWithValue("@PayorDetailUID", PayorDetailUID != null ? PayorDetailUID : (Object)(DBNull.Value));
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pSearchPatientMedicalDischarge(string hn, string firstName, string lastName, int? careproviderUID
            , DateTime? dateFrom, DateTime? dateTo, int? ownerOrganisationUID, int? PayorDetailUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pSearchPatientMedicalDischarge", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@HN", hn ?? "");
            adp.SelectCommand.Parameters.AddWithValue("@FirstName", firstName ?? "");
            adp.SelectCommand.Parameters.AddWithValue("@LastName", lastName ?? "");
            adp.SelectCommand.Parameters.AddWithValue("@CareproViderUID", careproviderUID != null ? careproviderUID : (Object)(DBNull.Value));
            adp.SelectCommand.Parameters.AddWithValue("@DateFrom", dateFrom != DateTime.MinValue && dateFrom != null ? dateFrom : (Object)(DBNull.Value));
            adp.SelectCommand.Parameters.AddWithValue("@DateTo", dateTo != DateTime.MinValue && dateTo != null ? dateTo : (Object)(DBNull.Value));
            adp.SelectCommand.Parameters.AddWithValue("@OwnerOrganisation", ownerOrganisationUID != null ? ownerOrganisationUID : (Object)(DBNull.Value));
            adp.SelectCommand.Parameters.AddWithValue("@PayorDetailUID", PayorDetailUID != null ? PayorDetailUID : (Object)(DBNull.Value));
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }
        public static DataTable pSearchPatientVisitForRISBilling(DateTime? dateFrom, DateTime? dateTo, string firstName, string lastName, string patientID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pSearchPatientVisitForRISBilling", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_DateFrom", dateFrom ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_DateTo", dateTo ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_PatientName_1", firstName ?? "");
            adp.SelectCommand.Parameters.AddWithValue("@P_PatientName_2", lastName ?? "");
            adp.SelectCommand.Parameters.AddWithValue("@P_PatientID", patientID ?? "");
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }
        public static DataTable pSearchRequestList(DateTime? requestDateFrom, DateTime? requestDateTo, DateTime? assignDateFrom, DateTime? assignDateTo
            , string statusList, int? RQPRTUID, string firstName, string lastName, string patientID, string orderName, int? RIMTYPUID, int? radiologistUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pSearchRequestList", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_RequestDateFrom", requestDateFrom ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_RequestDateTo", requestDateTo ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_AssignDateFrom", assignDateFrom ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_AssignDateTo", assignDateTo ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_List_ORDSTUID", statusList ?? "");
            adp.SelectCommand.Parameters.AddWithValue("@P_RQPRTUID", RQPRTUID ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_RIMTYPUID", RIMTYPUID ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_PatientName_1", firstName ?? "");
            adp.SelectCommand.Parameters.AddWithValue("@P_PatientName_2", lastName ?? "");
            adp.SelectCommand.Parameters.AddWithValue("@P_PatientID", patientID ?? "");
            adp.SelectCommand.Parameters.AddWithValue("@P_OrderName", orderName ?? "");
            adp.SelectCommand.Parameters.AddWithValue("@P_RadiologistUID", radiologistUID ?? (object)DBNull.Value);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pSearchRequestExamListForAssign(DateTime? dateFrom,DateTime? dateTo, int? organisationUID,long? patientUID,string requestItemName
            , int? RIMTYPUID,int? payorDetailUID,int? ORDSTUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pSearchRequestExamListForAssign", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_DateFrom", dateFrom ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_DateTo", dateTo ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_OwnerOrganisationUID", organisationUID ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_PatientUID", patientUID ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_RequestItemName", !string.IsNullOrEmpty(requestItemName) ? requestItemName : (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_RIMTYPUID", RIMTYPUID ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_PayorDetailUID", payorDetailUID ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_ORDSTUID", ORDSTUID ?? (object)DBNull.Value);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        

        public static DataTable pGetDoctorFee(DateTime dateFrom, DateTime dateTo, int? radiologistUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pGetDoctorFee", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_DateFrom", dateFrom);
            adp.SelectCommand.Parameters.AddWithValue("@P_DateTo", dateTo);
            adp.SelectCommand.Parameters.AddWithValue("@P_RadiologistUID", radiologistUID ?? (object)DBNull.Value);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pGetDoctorFeeNonPay(DateTime dateFrom,DateTime dateTo,int? radiologistUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pGetDoctorFeeNonPay", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_DateFrom", dateFrom);
            adp.SelectCommand.Parameters.AddWithValue("@P_DateTo", dateTo);
            adp.SelectCommand.Parameters.AddWithValue("@P_RadiologistUID", radiologistUID ?? (object)DBNull.Value);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pSearchStockBatch(int? ownerOrganisationUID, int? storeUID, int? itemType,string itemCode, string itemName)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pSearchStockBatch", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_OwnerOrganisationUID", ownerOrganisationUID ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_StoreUID", storeUID ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_ItemType", itemType ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_itemCode", string.IsNullOrEmpty(itemCode) ? (object)DBNull.Value : itemCode);
            adp.SelectCommand.Parameters.AddWithValue("@P_itemName", string.IsNullOrEmpty(itemName) ? (object)DBNull.Value : itemName);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pSearchStockForDispose(DateTime? dateFrom, DateTime? dateTo, int? storeUID, string batchID, string itemName)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pSearchStockForDispose", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                        adp.SelectCommand.Parameters.AddWithValue("@P_DateFrom", dateFrom ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_DateTo", dateTo ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_StoreUID", storeUID ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_BatchID", string.IsNullOrEmpty(batchID) ? (object)DBNull.Value : batchID);
            adp.SelectCommand.Parameters.AddWithValue("@P_itemName", string.IsNullOrEmpty(itemName) ? (object)DBNull.Value : itemName);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pSearchStockOnHand(int? ownerOrganisationUID, int? storeUID, int? itemType, string itemCode, string itemName)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pSearchStockOnHand", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_OwnerOrganisationUID", ownerOrganisationUID ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_StoreUID", storeUID ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_ItemType", itemType ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_ItemCode", string.IsNullOrEmpty(itemCode) ? (object)DBNull.Value : itemCode);
            adp.SelectCommand.Parameters.AddWithValue("@P_itemName", string.IsNullOrEmpty(itemName) ? (object)DBNull.Value : itemName);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pSearchStockMovement(int? ownerOrganisationUID, int? storeUID, string itemCode,string itemName,
             string transactionType, DateTime? dateFrom, DateTime? dateTo)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pSearchStockMovement", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_OwnerOrganisationUID", ownerOrganisationUID ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_StoreUID", storeUID ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_Itemcode", string.IsNullOrEmpty(itemCode) ? (object)DBNull.Value : itemCode);
            adp.SelectCommand.Parameters.AddWithValue("@p_ItemName",string.IsNullOrEmpty(itemName) ? (object)DBNull.Value : itemName);
            adp.SelectCommand.Parameters.AddWithValue("@P_TransactionType", transactionType ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_DateFrom", dateFrom ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_DateTo", dateTo ?? (object)DBNull.Value);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pSearchStockBalance(int? ownerOrganisationUID, int? storeUID, string itemCode,string itemName, DateTime? dateFrom, DateTime? dateTo)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pSearchStockBalance", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_OwnerOrganisationUID", ownerOrganisationUID ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_StoreUID", storeUID ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_Itemcode", string.IsNullOrEmpty(itemCode) ? (object)DBNull.Value : itemCode);
            adp.SelectCommand.Parameters.AddWithValue("@p_ItemName",string.IsNullOrEmpty(itemName) ? (object)DBNull.Value : itemName);
            adp.SelectCommand.Parameters.AddWithValue("@P_DateFrom", dateFrom ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_DateTo", dateTo ?? (object)DBNull.Value);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pSearchStockWorkList(DateTime? dateFrom, DateTime? dateTo, int? organisationUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pSearchStockWorkList", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_DateFrom", dateFrom ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_DateTo", dateTo ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_OrganisationUID", organisationUID ?? (object)DBNull.Value);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }


        public static DataTable pSearchRequestLabList(DateTime? requestDateFrom, DateTime? requestDateTo,string statusList, long? patientUID, int? requestItemUID,string labNumber, int? payorDetailUID, int? organisationUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pSearchRequestLabList", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_RequestDateFrom", requestDateFrom ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_RequestDateTo", requestDateTo ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_List_ORDSTUID", statusList ?? "");
            adp.SelectCommand.Parameters.AddWithValue("@P_RequestItemUID", requestItemUID ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_PatientUID", patientUID ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_LabNumber", labNumber ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_PayorDetailUID", payorDetailUID ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_OrganisationUID", organisationUID ?? (object)DBNull.Value);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pSearchRequestExamList(DateTime? requestDateFrom, DateTime? requestDateTo,string statusList, int? RQPRTUID,long? patientUID, string orderName, int? RIMTYPUID, int? radiologistUID,int? rduStaffUID,int? payorDetailUID, int? organisationUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pSearchRequestExamList", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_RequestDateFrom", requestDateFrom ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_RequestDateTo", requestDateTo ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_List_ORDSTUID", statusList ?? "");
            adp.SelectCommand.Parameters.AddWithValue("@P_RQPRTUID", RQPRTUID ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_RIMTYPUID", RIMTYPUID ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_PatientUID", patientUID ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_OrderName", orderName ?? "");
            adp.SelectCommand.Parameters.AddWithValue("@P_RadiologistUID", radiologistUID ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_RDUStaffUID", rduStaffUID ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_PayorDetailUID", payorDetailUID ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_OrganisationUID", organisationUID ?? (object)DBNull.Value);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pSearchResultLabList(DateTime? dateFrom, DateTime? dateTo, long? patientUID, int? payorDetailUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pSearchResultLabList", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandTimeout = 3000;
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_DateFrom", dateFrom ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_DateTo", dateTo ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_PatientUID", patientUID ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_PayorDetailUID ", payorDetailUID ?? (object)DBNull.Value);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pSearchResultRadiologyForTranslate(DateTime? dateFrom, DateTime? dateTo, long? patientUID,string itemName,int? RABSTSUID, int? payorDetailUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pSearchResultRadiologyForTranslate", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandTimeout = 3000;
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_DateFrom", dateFrom ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_DateTo", dateTo ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_PatientUID", patientUID ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_ItemName", itemName ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_RABSTSUID", RABSTSUID ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_PayorDetailUID ", payorDetailUID ?? (object)DBNull.Value);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }


        public static DataTable pGetRequestExecuteRadiologist(DateTime requestDateFrom, DateTime requestDateTo,int radiologistUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pGetRequestExecuteRadiologist", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_RequestDateFrom", requestDateFrom);
            adp.SelectCommand.Parameters.AddWithValue("@P_RequestDateTo", requestDateTo);
            adp.SelectCommand.Parameters.AddWithValue("@P_RadiologistUID", radiologistUID);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            if (ds.Tables.Count <= 0)
            {
                return null;
            }
            return ds.Tables[0];
        }
        public static DataTable pSearchRequestListByRequestDetailUID(long requestDetailUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pSearchRequestListByRequestDetailUID", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_RequestDetailUID", requestDetailUID);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pPrintStrickerDrug(long prescriptionItemUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pPrintStrickerDrug", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@PrescriptionItemUID", prescriptionItemUID);

            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pPrintOPDCard(long patientUID,long patientVisitUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pPrintOPDCard", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_PatientUID", patientUID);
            adp.SelectCommand.Parameters.AddWithValue("@P_PatientVisitUID", patientVisitUID);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pPrintStatementBill(long patientBillUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pPrintStatementBill", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_PatientBillUID", patientBillUID);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pGetReferenceValueList(string DomainCode)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pGetReferenceValueList", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@DomainCode", DomainCode);

            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pGetItemMasterQtyByStore(int storeUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pGetItemMasterQtyByStore", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_StoreUID", storeUID);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pGetItemMasterForIssue(int organisationUID, int storeUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pGetItemMasterForIssue", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_OwnerOrganisationUID", organisationUID);
            adp.SelectCommand.Parameters.AddWithValue("@P_StoreUID", storeUID);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pGetOrderALLByPatientUID(long patientUID, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pGetOrderALLByPatientUID", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandTimeout = 3000;
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_PatientUID", patientUID);
            adp.SelectCommand.Parameters.AddWithValue("@P_DateFrom", dateFrom ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_DateTo", dateTo ?? (object)DBNull.Value);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pGetOrderAllByVisitUID(long patientVisitUID, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pGetOrderAllByVisitUID", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_VisitUID", patientVisitUID);
            adp.SelectCommand.Parameters.AddWithValue("@P_DateFrom", dateFrom ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_DateTo", dateTo ?? (object)DBNull.Value);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pGetOrderDrugByPatientUID(long patientUID, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pGetOrderDrugByPatientUID", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_PatientUID", patientUID);
            adp.SelectCommand.Parameters.AddWithValue("@P_DateFrom", dateFrom ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_DateTo", dateTo ?? (object)DBNull.Value);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }
        public static DataTable pGetOrderDrugByVisitUID(long patientVisitUID, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pGetOrderDrugByVisitUID", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_VisitUID", patientVisitUID);
            adp.SelectCommand.Parameters.AddWithValue("@P_DateFrom", dateFrom ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_DateTo", dateTo ?? (object)DBNull.Value);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pGetOrderMedicalByVisitUID(long patientVisitUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pGetOrderMedicalByVisitUID", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_VisitUID", patientVisitUID);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pGetOrderItemByVisitUID(long patientVisitUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pGetOrderItemByVisitUID", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_VisitUID", patientVisitUID);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pGetOrderRequestByVisitUID(long patientVisitUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pGetOrderRequestByVisitUID", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_VisitUID", patientVisitUID);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pGetOrderDuplicate(long patientUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pGetOrderDuplicate", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_PatientUID", patientUID);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pRPTPatientProblemStatistic(DateTime dateFrom, DateTime dateTo, int? vistyuid, int? organisationUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pRPTPatientProblemStatistic", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_DateFrom", dateFrom);
            adp.SelectCommand.Parameters.AddWithValue("@P_DateTo", dateTo);
            adp.SelectCommand.Parameters.AddWithValue("@P_VisitTypeUID", vistyuid ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_OrganisationUID", organisationUID ?? (object)DBNull.Value);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pRPTPatientSummaryPerMonth(int year, string monthLists, int? organisationUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pRPTPatientSummaryPerMonth", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandTimeout = 3000;
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_Year", year);
            adp.SelectCommand.Parameters.AddWithValue("@P_MonthLists", monthLists);
            adp.SelectCommand.Parameters.AddWithValue("@P_OrganisationUID", (organisationUID != null && organisationUID != 0) ? organisationUID : (object)DBNull.Value);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pRPTPatientSummaryData(DateTime dateFrom, DateTime dateTo,int? organisationUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pRPTPatientSummaryData", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandTimeout = 3000;
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_DateFrom", dateFrom);
            adp.SelectCommand.Parameters.AddWithValue("@P_DateTo", dateTo);
            adp.SelectCommand.Parameters.AddWithValue("@P_OrganisationUID", organisationUID ?? (object)DBNull.Value);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }
        public static DataTable pRPTVisitDaysStatistic(DateTime dateFrom, DateTime dateTo, int? vistyuid, int? organisationUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pRPTVisitDaysStatistic", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandTimeout = 5000;
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_DateFrom", dateFrom);
            adp.SelectCommand.Parameters.AddWithValue("@P_DateTo", dateTo);
            adp.SelectCommand.Parameters.AddWithValue("@P_VisitTypeUID", vistyuid ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_OrganisationUID", organisationUID ?? (object)DBNull.Value);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataSet pRPTPatientSumByAreaPerMonth(DateTime dateFrom, DateTime dateTo, int? vistyuid, int? organisationUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pRPTPatientSumByAreaPerMonth", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_DateFrom", dateFrom);
            adp.SelectCommand.Parameters.AddWithValue("@P_DateTo", dateTo);
            adp.SelectCommand.Parameters.AddWithValue("@P_VisitTypeUID", vistyuid ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_OrganisationUID", organisationUID ?? (object)DBNull.Value);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds;
        }

        public static DataTable pRPTVisitTimesStatistic(DateTime dateFrom, DateTime dateTo, int? vistyuid, int? organisationUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pRPTVisitTimesStatistic", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_DateFrom", dateFrom);
            adp.SelectCommand.Parameters.AddWithValue("@P_DateTo", dateTo);
            adp.SelectCommand.Parameters.AddWithValue("@P_VisitTypeUID", vistyuid ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_OrganisationUID", organisationUID ?? (object)DBNull.Value);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pRPTPatientNetProfit(DateTime dateFrom, DateTime dateTo, int? vistyuid, int? organisationUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pRPTPatientNetProfit", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_DateFrom", dateFrom);
            adp.SelectCommand.Parameters.AddWithValue("@P_DateTo", dateTo);
            adp.SelectCommand.Parameters.AddWithValue("@P_VisitTypeUID", vistyuid ?? (object)DBNull.Value);
            adp.SelectCommand.Parameters.AddWithValue("@P_OrganisationUID", (organisationUID != null && organisationUID != 0) ? organisationUID : (object)DBNull.Value);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pRPTUsedReport(DateTime dateFrom, DateTime dateTo, int? organisationUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pRPTUsedReport", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_DateFrom", dateFrom);
            adp.SelectCommand.Parameters.AddWithValue("@P_DateTo", dateTo);
            adp.SelectCommand.Parameters.AddWithValue("@P_OrganisationUID", (organisationUID != null && organisationUID != 0) ? organisationUID : (object)DBNull.Value);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }


        public static DataTable pRPTDrugStoreNetProfit(DateTime dateFrom, DateTime dateTo, int? organisationUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pRPTDrugStoreNetProfit", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_DateFrom", dateFrom);
            adp.SelectCommand.Parameters.AddWithValue("@P_DateTo", dateTo);
            adp.SelectCommand.Parameters.AddWithValue("@P_OrganisationUID", (organisationUID != null && organisationUID != 0) ? organisationUID : (object)DBNull.Value);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pRPTDoctorFee(DateTime dateFrom, DateTime dateTo, int? careproviderUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pRPTDoctorFee", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_DateFrom", dateFrom);
            adp.SelectCommand.Parameters.AddWithValue("@P_DateTo", dateTo);
            adp.SelectCommand.Parameters.AddWithValue("@P_CareproviderUID", (careproviderUID != null && careproviderUID != 0) ? careproviderUID : (object)DBNull.Value);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pRPTStockTransferredOut(DateTime dateFrom, DateTime dateTo, int? organisationUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pRPTStockTransferredOut", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_DateFrom", dateFrom);
            adp.SelectCommand.Parameters.AddWithValue("@P_DateTo", dateTo);
            adp.SelectCommand.Parameters.AddWithValue("@P_OrganisationUID", (organisationUID != null && organisationUID != 0) ? organisationUID : (object)DBNull.Value);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pRPTStockTransferredIn(DateTime dateFrom, DateTime dateTo, int? organisationUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pRPTStockTransferredIn", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_DateFrom", dateFrom);
            adp.SelectCommand.Parameters.AddWithValue("@P_DateTo", dateTo);
            adp.SelectCommand.Parameters.AddWithValue("@P_OrganisationUID", (organisationUID != null && organisationUID != 0) ? organisationUID : (object)DBNull.Value);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pRPTStockDispensed(DateTime dateFrom, DateTime dateTo, int? organisationUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pRPTStockDispensed", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_DateFrom", dateFrom);
            adp.SelectCommand.Parameters.AddWithValue("@P_DateTo", dateTo);
            adp.SelectCommand.Parameters.AddWithValue("@P_OrganisationUID", (organisationUID != null && organisationUID != 0) ? organisationUID : (object)DBNull.Value);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pRPTStockOnHand(int? organisationUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pRPTStockOnHand", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_OrganisationUID", (organisationUID != null && organisationUID != 0) ? organisationUID : (object)DBNull.Value);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pRPTStockNonMovement(DateTime dateFrom, DateTime dateTo, int? organisationUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pRPTStockNonMovement", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_DateFrom", dateFrom);
            adp.SelectCommand.Parameters.AddWithValue("@P_DateTo", dateTo);
            adp.SelectCommand.Parameters.AddWithValue("@P_OrganisationUID", (organisationUID != null && organisationUID != 0) ? organisationUID : (object)DBNull.Value);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pRPTStockExpired(int? organisationUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pRPTStockExpired", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_OrganisationUID", (organisationUID != null && organisationUID != 0) ? organisationUID : (object)DBNull.Value);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pRPTStockExpiry(int month,int? organisationUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pRPTStockExpiry", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_Month", month);
            adp.SelectCommand.Parameters.AddWithValue("@P_OrganisationUID", (organisationUID != null && organisationUID != 0) ? organisationUID : (object)DBNull.Value);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pRPTStockGoodReceive(DateTime dateFrom, DateTime dateTo, int? organisationUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pRPTStockGoodReceive", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_DateFrom", dateFrom);
            adp.SelectCommand.Parameters.AddWithValue("@P_DateTo", dateTo);
            adp.SelectCommand.Parameters.AddWithValue("@P_OrganisationUID", (organisationUID != null && organisationUID != 0) ? organisationUID : (object)DBNull.Value);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pRPTStockReceive(DateTime dateFrom, DateTime dateTo, int? organisationUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pRPTStockReceive", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_DateFrom", dateFrom);
            adp.SelectCommand.Parameters.AddWithValue("@P_DateTo", dateTo);
            adp.SelectCommand.Parameters.AddWithValue("@P_OrganisationUID", (organisationUID != null && organisationUID != 0) ? organisationUID : (object)DBNull.Value);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pRPTStockIssued(DateTime dateFrom, DateTime dateTo, int? organisationUID)
        {
            MediTechEntities mediTechEntities = new MediTechEntities();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("pRPTStockIssued", mediTechEntities.Database.Connection.ConnectionString);
            sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@P_DateFrom", dateFrom);
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@P_DateTo", dateTo);
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@P_OrganisationUID", (organisationUID != null && organisationUID != 0) ? organisationUID : (object)DBNull.Value);
            DataSet dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);
            return dataSet.Tables[0];
        }


        public static DataTable pRPTStockBalancePerMounth(int year, string monthLists, int? organisationUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pRPTStockBalancePerMounth", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_Year", year);
            adp.SelectCommand.Parameters.AddWithValue("@P_MonthLists", monthLists);
            adp.SelectCommand.Parameters.AddWithValue("@P_OrganisationUID", (organisationUID != null && organisationUID != 0) ? organisationUID : (object)DBNull.Value);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pRPTStockConsumtion(DateTime dateFrom, DateTime dateTo, int? organisationUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pRPTStockConsumption", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_DateFrom", dateFrom);
            adp.SelectCommand.Parameters.AddWithValue("@P_DateTo", dateTo);
            adp.SelectCommand.Parameters.AddWithValue("@P_OrganisationUID", (organisationUID != null && organisationUID != 0) ? organisationUID : (object)DBNull.Value);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pRPTStockAdjustmentOut(DateTime dateFrom, DateTime dateTo, int? organisationUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pRPTStockAdjustmentOut", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_DateFrom", dateFrom);
            adp.SelectCommand.Parameters.AddWithValue("@P_DateTo", dateTo);
            adp.SelectCommand.Parameters.AddWithValue("@P_OrganisationUID", (organisationUID != null && organisationUID != 0) ? organisationUID : (object)DBNull.Value);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pRPTStockAdjustmentIn(DateTime dateFrom, DateTime dateTo, int? organisationUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pRPTStockAdjustmentIn", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_DateFrom", dateFrom);
            adp.SelectCommand.Parameters.AddWithValue("@P_DateTo", dateTo);
            adp.SelectCommand.Parameters.AddWithValue("@P_OrganisationUID", (organisationUID != null && organisationUID != 0) ? organisationUID : (object)DBNull.Value);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pRPTStockSummery(DateTime dateFrom,DateTime dateTo,int? organisationUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pRPTStockSummery", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_DateFrom", dateFrom);
            adp.SelectCommand.Parameters.AddWithValue("@P_DateTo", dateTo);
            adp.SelectCommand.Parameters.AddWithValue("@P_OrganisationUID", (organisationUID != null && organisationUID != 0) ? organisationUID : (object)DBNull.Value);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pRPTStockDispose(DateTime dateFrom, DateTime dateTo, int? organisationUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pRPTStockDispose", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_DateFrom", dateFrom);
            adp.SelectCommand.Parameters.AddWithValue("@P_DateTo", dateTo);
            adp.SelectCommand.Parameters.AddWithValue("@P_OrganisationUID", (organisationUID != null && organisationUID != 0) ? organisationUID : (object)DBNull.Value);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }


        public static DataTable pRPTOrderRequestCard(long patientUID, long patientVisitUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pRPTOrderRequestCard", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_PatientUID", patientUID);
            adp.SelectCommand.Parameters.AddWithValue("@P_PatientVisitUID", patientVisitUID);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static DataTable pRPTWellNessBook(long patientUID, long patientVisitUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pRPTWellNessBook", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_PatientUID", patientUID);
            adp.SelectCommand.Parameters.AddWithValue("@P_PatientVisitUID", patientVisitUID);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }
        public static DataTable pRPTCheckupBook(long patientUID, long patientVisitUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pRPTCheckupBook", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_PatientUID", patientUID);
            adp.SelectCommand.Parameters.AddWithValue("@P_PatientVisitUID", patientVisitUID);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }
        public static DataTable pRPTRadiologyRDUReview(DateTime dateFrom, DateTime dateTo, int? organisationUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlDataAdapter adp = new SqlDataAdapter("pRPTRadiologyRDUReview", entities.Database.Connection.ConnectionString);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.AddWithValue("@P_DateFrom", dateFrom);
            adp.SelectCommand.Parameters.AddWithValue("@P_DateTo", dateTo);
            adp.SelectCommand.Parameters.AddWithValue("@P_OrganisationUID", (organisationUID != null && organisationUID != 0) ? organisationUID : (object)DBNull.Value);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        public static bool pMergePatient(long majorPatientUID, long minorPatientUID, char address, char gender, char phone, char photo, char blood, int userUID)
        {
            bool flag = false;
            MediTechEntities entities = new MediTechEntities();

            SqlConnection con = new SqlConnection(entities.Database.Connection.ConnectionString);
            try
            {

                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "pMergePatient";

                cmd.Parameters.AddWithValue("@P_MajorPatientUID", majorPatientUID);
                cmd.Parameters.AddWithValue("@P_MinorPatientUID", minorPatientUID);
                cmd.Parameters.AddWithValue("@P_Address", address);
                cmd.Parameters.AddWithValue("@P_Gender", gender);
                cmd.Parameters.AddWithValue("@P_Phone", phone);
                cmd.Parameters.AddWithValue("@P_Photo", photo);
                cmd.Parameters.AddWithValue("@P_Blood", blood);
                cmd.Parameters.AddWithValue("@P_UserID", userUID);

                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                con.Close();
            }

            return flag;
        }

        public static bool pUnMergePatient(int patientMergeUID, int userUID)
        {
            bool flag = false;
            MediTechEntities entities = new MediTechEntities();

            SqlConnection con = new SqlConnection(entities.Database.Connection.ConnectionString);
            try
            {

                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "pUnMergePatient";

                cmd.Parameters.AddWithValue("@P_PatientMergeUID", patientMergeUID);
                cmd.Parameters.AddWithValue("@P_UserID", userUID);

                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                con.Close();
            }

            return flag;
        }

        public static bool pEncounterMergePatient(long majorPatientUID, long minorPatientUID,string minorVisitUIDS, int userUID)
        {
            bool flag = false;
            MediTechEntities entities = new MediTechEntities();

            SqlConnection con = new SqlConnection(entities.Database.Connection.ConnectionString);
            try
            {

                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "pEncounterMergePatient";

                cmd.Parameters.AddWithValue("@P_MajorPatientUID", majorPatientUID);
                cmd.Parameters.AddWithValue("@P_MinorPatientUID", minorPatientUID);
                cmd.Parameters.AddWithValue("@P_MinorVisitUIDS", minorVisitUIDS);
                cmd.Parameters.AddWithValue("@P_UserID", userUID);

                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                con.Close();
            }

            return flag;
        }
        public static System.Nullable<int> pGetSEQID(string seqName)
        {
            System.Nullable<int> seqUID = null;
            MediTechEntities entities = new MediTechEntities();

            SqlConnection con = new SqlConnection(entities.Database.Connection.ConnectionString);

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "pGetSEQID";
                cmd.Parameters.AddWithValue("@P_SEQTableName", seqName);

                object value = cmd.ExecuteScalar();
                if (value != null)
                {
                    seqUID = int.Parse(value.ToString());
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                con.Close();
            }

            return seqUID;
        }


    }

    public static class SqlStatement
    {
        public static double GetItemTotalQuantity(int itemMasterUID, int storeUID,int ownerOrganisation)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlConnection con = new SqlConnection(entities.Database.Connection.ConnectionString);
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.Text;
                command.Connection = con;
                command.CommandText = "Select dbo.fGetItemTotalQuantity(@ItemMasterUID,@StoreUID,@OwnerOrganisation)";
                command.Parameters.AddWithValue("@ItemMasterUID", itemMasterUID);
                command.Parameters.AddWithValue("@StoreUID", storeUID);
                command.Parameters.AddWithValue("@OwnerOrganisation", ownerOrganisation);
                object value = command.ExecuteScalar();
                return value != null ? (double)value : 0;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }

        public static DataTable GetRadiologistSession(long requestDetailUID)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlConnection con = new SqlConnection(entities.Database.Connection.ConnectionString);
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                DataTable dt = new DataTable();
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.Text;
                command.Connection = con;
                command.CommandText = @"Declare @Day int 
SET DATEFIRST 1;
select @Day = DATEPART (weekday, Getdate());  

select sd.CareproviderUID,
dbo.fGetCareProviderName(sd.CareproviderUID) CareproviderName,
Day1,
Day2,
Day3,
Day4,
Day5,
Day6,
Day7,
(select Count(*) 
From RequestDetail
Where RequestDetail.StatusFlag = 'A'
and ISNULL(RadiologistUID,(Select Top 1 RadiologistUID from Result Where RequestDetailUID = RequestDetail.UID and StatusFlag = 'A')) = ca.UID
and UID <> @RequestDetailUID
and MONTH(RequestDetail.RequestedDttm) = MONTH(getdate())
and Year(RequestDetail.RequestedDttm) = Year(getdate())) RequestNumber,
(select Count(*) 
From RequestDetail
Where RequestDetail.StatusFlag = 'A'
and ISNULL(RadiologistUID,(Select Top 1 RadiologistUID from Result Where RequestDetailUID = RequestDetail.UID and StatusFlag = 'A'))  = ca.UID
and Comments = 'RDU Review'
and UID <> @RequestDetailUID
and MONTH(RequestDetail.RequestedDttm) = MONTH(getdate())
and Year(RequestDetail.RequestedDttm) = Year(getdate())) RequestBulkNumber
from SessionDefinition sd
inner join Careprovider ca
on sd.CareproviderUID = ca.UID
where ca.IsRadiologist = 1
and sd.StatusFlag = 'A'
and ca.StatusFlag = 'A'
and Convert(time,SessionStartDttm) <= Convert(time,getdate())
and Convert(time,SessionEndDttm) >= Convert(time,getdate())
and (ca.ActiveFrom is null or Convert(date,ca.ActiveFrom)  <= Convert(date,getdate()))
and (ca.ActiveTo is null or Convert(date,ca.ActiveTo)  >= Convert(date,getdate()))
and ( sd.Day1 = CASE WHEN @Day = 1 THEN 1 END
or sd.Day2 = CASE WHEN @Day = 2 THEN 1 END
or sd.Day3 = CASE WHEN @Day = 3 THEN 1 END
or sd.Day4 = CASE WHEN @Day = 4 THEN 1 END
or sd.Day5 = CASE WHEN @Day = 5 THEN 1 END )
or sd.Day6 = CASE WHEN @Day = 6 THEN 1 END
or sd.Day7 = CASE WHEN @Day = 7 THEN 1 END
and sd.UID not in (
select SessionDefinitionUID from SessionWithdrawnDetail
where Convert(date,StartDttm)  <= Convert(date,getdate())
and Convert(date,EndDttm) >= Convert(date,getdate())
and StatusFlag = 'A')";
                command.Parameters.AddWithValue("@RequestDetailUID", requestDetailUID);
                dt.Load(command.ExecuteReader());
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }

        public static DataTable GetNotificationTaskResult(string healthOrganisationCode)
        {
            MediTechEntities entities = new MediTechEntities();
            SqlConnection con = new SqlConnection(entities.Database.Connection.ConnectionString);
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                DataTable dt = new DataTable();
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.Text;
                command.Connection = con;
                command.CommandText = @"select top 10 * from NotificationTaskResult
Where OwnerOrganisationCode = @OwnerOrganisationCode
and Status = -1";
                command.Parameters.AddWithValue("@OwnerOrganisationCode", healthOrganisationCode);
                dt.Load(command.ExecuteReader());
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
    }
}
