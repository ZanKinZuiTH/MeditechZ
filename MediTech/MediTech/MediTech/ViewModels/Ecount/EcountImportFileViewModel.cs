using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MediTech.DataService;
using MediTech.Model;
using MediTech.Models;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Globalization;

namespace MediTech.ViewModels
{
 public class EcountImportFileViewModel : MediTechViewModelBase
    {
        #region Properties


        private string _FileLocation;

        public string FileLocation
        {
            get { return _FileLocation; }
            set { Set(ref _FileLocation, value); }
        }


        private bool _IsEnableGRNItemList = true;

        public bool IsEnableGRNItemList
        {
            get { return _IsEnableGRNItemList; }
            set { Set(ref _IsEnableGRNItemList, value); }
        }

        private bool _IsEnableOrganisation = true;

        public bool IsEnableOrganisation
        {
            get { return _IsEnableOrganisation; }
            set { Set(ref _IsEnableOrganisation, value); }
        }

        private bool _IsEnableStore = true;


        public bool IsEnableStore
        {
            get { return _IsEnableStore; }
            set { Set(ref _IsEnableStore, value); }
        }

        private Visibility _VisibiltyInvoince = Visibility.Hidden;

        public Visibility VisibiltyInvoince
        {
            get { return _VisibiltyInvoince; }
            set { Set(ref _VisibiltyInvoince, value); }
        }


        public List<LookupReferenceValueModel> GRNTypes { get; set; }
        private LookupReferenceValueModel _SelectGRNType;

        public LookupReferenceValueModel SelectGRNType
        {
            get { return _SelectGRNType; }
            set
            {
                Set(ref _SelectGRNType, value);
                if (_SelectGRNType != null)
                {
                    if (_SelectGRNType.ValueCode == "CONSIG")
                    {
                        VisibiltyInvoince = Visibility.Hidden;
                    }
                    else
                    {
                        VisibiltyInvoince = Visibility.Visible;
                    }
                }
            }
        }

        public List<HealthOrganisationModel> Organisations { get; set; }
        private HealthOrganisationModel _SelectOrganisation;

        public HealthOrganisationModel SelectOrganisation
        {
            get { return _SelectOrganisation; }
            set
            {
                Set(ref _SelectOrganisation, value);
                if (_SelectOrganisation != null)
                {
                    Stores = DataService.Inventory.GetStoreByOrganisationUID(_SelectOrganisation.HealthOrganisationUID);
                }
                else
                {
                    Stores = null;
                }
            }
        }

        private List<StoreModel> _Stores;

        public List<StoreModel> Stores
        {
            get { return _Stores; }
            set { Set(ref _Stores, value); }
        }

        private StoreModel _SelectStore;

        public StoreModel SelectStore
        {
            get { return _SelectStore; }
            set
            {
                Set(ref _SelectStore, value);
                if (SelectStore != null)
                {
                    ItemMasters = DataService.Inventory.GetItemMasterQtyByStore(SelectStore.StoreUID);
                }
            }
        }

        public List<VendorDetailModel> Vendors { get; set; }
        private VendorDetailModel _SelectVendor;

        public VendorDetailModel SelectVendor
        {
            get { return _SelectVendor; }
            set { Set(ref _SelectVendor, value); }
        }

        public List<VendorDetailModel> Manufacturers { get; set; }

        private DateTime? _ReceiveDate;

        public DateTime? ReceiveDate
        {
            get { return _ReceiveDate; }
            set { Set(ref _ReceiveDate, value); }

        }

        private string _InvoinceNo;

        public string InvoinceNo
        {
            get { return _InvoinceNo; }
            set { Set(ref _InvoinceNo, value); }
        }

        private DateTime? _InvoinceDate;

        public DateTime? InvoinceDate
        {
            get { return _InvoinceDate; }
            set { Set(ref _InvoinceDate, value); }
        }



        private string _Comments;

        public string Comments
        {
            get { return _Comments; }
            set { Set(ref _Comments, value); }
        }

        private Double? _VATPercentage;

        public Double? VATPercentage
        {
            get { return _VATPercentage; }
            set { Set(ref _VATPercentage, value); }
        }


        private double _NetAmount;

        public double NetAmount
        {
            get { return _NetAmount; }
            set { Set(ref _NetAmount, value); }
        }

        private double _OtherChages;

        public double OtherChages
        {
            get { return _OtherChages; }
            set { Set(ref _OtherChages, value); CalculateNetAmount(); }
        }

        private double _Discount;

        public double Discount
        {
            get { return _Discount; }
            set { Set(ref _Discount, value); CalculateNetAmount(); }
        }


        private List<ItemMasterModel> _ItemMasters;

        public List<ItemMasterModel> ItemMasters
        {
            get { return _ItemMasters; }
            set { Set(ref _ItemMasters, value); }
        }

        private ObservableCollection<ItemMasterList> _GRNItems;

        public ObservableCollection<ItemMasterList> GRNItems
        {
            get { return _GRNItems; }
            set
            {
                Set(ref _GRNItems, value);
            }
        }

        #endregion

        #region Command

        private RelayCommand _ImportCommand;

        public RelayCommand ImportCommand
        {
            get
            {
                return _ImportCommand
                    ?? (_ImportCommand = new RelayCommand(ImportFile));
            }
        }




        private RelayCommand _ChooseCommand;

        public RelayCommand ChooseCommand
        {
            get
            {
                return _ChooseCommand
                    ?? (_ChooseCommand = new RelayCommand(ChooseFile));
            }
        }


        private RelayCommand _SaveCommand;
        public RelayCommand SaveCommand
        {
            get { return _SaveCommand ?? (_SaveCommand = new RelayCommand(Save)); }
        }

        private RelayCommand _CancelCommand;
        public RelayCommand CancelCommand
        {
            get { return _CancelCommand ?? (_CancelCommand = new RelayCommand(Cancel)); }
        }

        private RelayCommand<DevExpress.Xpf.Grid.CellValueChangedEventArgs> _ChangeValueCommand;
        public RelayCommand<DevExpress.Xpf.Grid.CellValueChangedEventArgs> ChangeValueCommand
        {
            get { return _ChangeValueCommand ?? (_ChangeValueCommand = new RelayCommand<DevExpress.Xpf.Grid.CellValueChangedEventArgs>(ChangeValue)); }
        }



        #endregion

        #region Method

        GRNDetailModel model;

        private void ImportFile()
        {
            if (SelectStore == null)
            {
                WarningDialog("กรุณาเลือก STOCK");
                return;
            }


            OleDbConnection conn;
            DataSet objDataset1;
            OleDbCommand cmd;
            DataTable dt;
            DataTable ImportData = new DataTable();
            string connectionString = string.Empty;
            // int pgBarCounter = 0;
            // TotalRecord = 0;
            EcountImportFile view = (EcountImportFile)this.View;
            try
            {
                if (FileLocation.Trim() != string.Empty)
                {
                    if (FileLocation.Trim().EndsWith(".xls"))
                    {
                        connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + FileLocation.Trim() +
                            "; Extended Properties=\"Excel 8.0; HDR=Yes; IMEX=1\"";
                    }
                    else
                    {
                        connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + FileLocation.Trim() +
                            "; Extended Properties=\"Excel 12.0 Xml; HDR=YES; IMEX=1\"";
                    }
                    using (conn = new OleDbConnection(connectionString))
                    {
                        conn.Open();
                        objDataset1 = new DataSet();
                        dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            for (int row = 0; row < dt.Rows.Count;)
                            {
                                string FileName = Convert.ToString(dt.Rows[row]["Table_Name"]);
                                cmd = conn.CreateCommand();
                                OleDbCommand objCmdSelect = new OleDbCommand("SELECT * FROM [" + FileName + "] Where ([รหัสสินค้า] <> '' OR [ลำดับ] IS NOT NULL)", conn);
                                OleDbDataAdapter objAdapter1 = new OleDbDataAdapter();
                                objAdapter1.SelectCommand = objCmdSelect;
                                objAdapter1.Fill(objDataset1);
                                //Break after reading the first sheet
                                break;
                            }
                            ImportData = objDataset1.Tables[0];
                            conn.Close();
                        }
                    }

                    int upperlimit = ImportData.Rows.Count;
                    //view.SetProgressBarLimits(0, upperlimit);
                    //view.SetProgressBarValue(upperlimit);
                   // OnUpdateEvent();
                    GRNItems = new ObservableCollection<ItemMasterList>();
                    foreach (DataRow drow in ImportData.Rows)
                    {
                        int itemUIDMaster = GetItemByCode(drow["รหัสสินค้า"].ToString().Trim());
                        ItemMasterList newRow = new ItemMasterList();
                        newRow.ItemMasterUID = itemUIDMaster;
                        newRow.ItemName = drow["รายการ"].ToString().Trim();
                        newRow.ItemCode = drow["รหัสสินค้า"].ToString().Trim();
                        newRow.Quantity = double.Parse(drow["จำนวน"].ToString().Trim());
                        DateTime checkupDttm;
                        if (DateTime.TryParse(drow["วันหมดอายุ"].ToString().Trim(), new CultureInfo("th-TH"),System.Globalization.DateTimeStyles.None, out checkupDttm))
                        newRow.ExpiryDttm = checkupDttm;
                        newRow.UnitPrice = drow["ราคาต่อหน่วย"].ToString().Trim()== "" ? 0 : double.Parse(drow["ราคาต่อหน่วย"].ToString().Trim());
                        newRow.TaxPercentage = drow["ภาษี"].ToString().Trim() == "" ? 0 : double.Parse(drow["ภาษี"].ToString().Trim());
                        newRow.SerialNumber = drow["หมายเลข Serial/Lot"].ToString().Trim();
                        newRow.BatchID = drow["Batch ID"].ToString().Trim();
                        GRNItems.Add(newRow);
                    }
                }
            }

            catch (Exception er)
            {

                System.Windows.Forms.MessageBox.Show(er.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private int GetItemByCode(string code)
        {
            int itemID = DataService.Inventory.GetItemMasterByCode(code).ItemMasterUID;
            return itemID;
            //throw new NotImplementedException();
        }

        private void ChooseFile()
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Excel 2007 (*.xlsx)|*.xlsx|Excel 1997 - 2003 (*.xls)|*.xls"; ;
            openDialog.InitialDirectory = @"c:\";
            openDialog.ShowDialog();
            if (openDialog.FileName.Trim() != "")
            {
                try
                {
                    FileLocation = openDialog.FileName.Trim();
                }
                catch (Exception ex)
                {
                    ErrorDialog(ex.Message);
                }
            }
        }




        public EcountImportFileViewModel()
        {
            ReceiveDate = DateTime.Now.Date;
            GRNTypes = DataService.Technical.GetReferenceValueMany("GRNTYP");
            Organisations = GetHealthOrganisationIsRoleStock();
            var tempvendor = DataService.Purchaseing.GetVendorDetail();
            Vendors = tempvendor.Where(p => p.MNFTPUID == 2937).ToList();
            Manufacturers = tempvendor.Where(p => p.MNFTPUID == 2938).ToList();
            GRNItems = new ObservableCollection<ItemMasterList>();

            if (GRNTypes != null)
            {
                SelectGRNType = GRNTypes.FirstOrDefault();
            }


            if (Organisations != null)
            {
                SelectOrganisation = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == AppUtil.Current.OwnerOrganisationUID);
            }
            //ItemMasters = DataService.Inventory.GetItemMasterQtyByStore(0);
        }

        public void ChangeValue(DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            CalculateNetAmount();
            CalculateVat();
        }

        public void CalculateNetAmount()
        {
            NetAmount = 0;
            foreach (var item in GRNItems)
            {
                NetAmount += item.NetAmount;
            }

            NetAmount += OtherChages - Discount;

            NetAmount = Math.Round(NetAmount, 2);
        }

        public void CalculateVat()
        {
            VATPercentage = 0;
            foreach (var item in GRNItems)
            {
                if ((item.TaxPercentage ?? 0) != 0)
                {
                    VATPercentage += ((item.NetAmount * item.TaxPercentage) / (100 + item.TaxPercentage));
                }

            }
            VATPercentage = Math.Round(VATPercentage ?? 0, 2);
        }

        private void Save()
        {

            try
            {
                if (SelectStore == null)
                {
                    WarningDialog("กรุณาระบุ Store ที่รับเข้า");
                    return;
                }
                if (SelectVendor == null)
                {
                    WarningDialog("กรุณาเลือก ผู้จัดจำหน่าย");
                    return;
                }

                if (SelectGRNType != null && SelectGRNType.ValueCode != "CONSIG")
                {
                    if (string.IsNullOrEmpty(InvoinceNo))
                    {
                        WarningDialog("กรุณาระบุ InvoinceNo");
                        return;
                    }
                }

                if (GRNItems.Count <= 0)
                {
                    WarningDialog("กรุณาสร้างรายการสินค้า");
                    return;
                }

                AssingPropertiesToModel();
                foreach (var item in model.GRNItemLists)
                {
                    if ( item.PurchaseCost == 0 )
                    {
                        if (item.FreeQuantity != 0 || item.FreeQuantity != null)
                        {
                            WarningDialog("กรุณา check ราคาต้นทุน และจำนวนฟรี ที่รับเข้า");
                            return;
                        }
                    } 
                }

                DataService.Purchaseing.CreateGoodReceiveFromEcount(model, AppUtil.Current.UserID);
                SaveSuccessDialog();
                ListGRN listPage = new ListGRN();
                ChangeView_CloseViewDialog(listPage, ActionDialog.Save);
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }

        private void Cancel()
        {
            ListGRN listPage = new ListGRN();
            ChangeView_CloseViewDialog(listPage, ActionDialog.Cancel);
        }

        public void AssignModel(GRNDetailModel modelData, bool isEnableItem)
        {
            model = modelData;
            AssingModelToProperties();
            IsEnableGRNItemList = isEnableItem;
            IsEnableOrganisation = isEnableItem;
            IsEnableStore = isEnableItem;
        }

        public void AssingModelToProperties()
        {
            ReceiveDate = model.RecievedDttm;

            if (Organisations != null)
            {
                SelectOrganisation = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == model.RecievedOrganisationUID);
            }

            if (Stores != null)
            {
                SelectStore = Stores.FirstOrDefault(p => p.StoreUID == model.RecievedStoreUID);
            }

            SelectGRNType = model.GRNTYPUID != null ? GRNTypes.FirstOrDefault(p => p.Key == model.GRNTYPUID) : GRNTypes.FirstOrDefault();
            SelectVendor = Vendors.FirstOrDefault(p => p.VendorDetailUID == model.VendorDetailUID);
            InvoinceNo = model.InvoiceNo;
            InvoinceDate = model.InvoiceDate;
            Comments = model.Comments;
            OtherChages = model.OtherCharges;
            Discount = model.Discount;
            NetAmount = model.NetAmount;
            GRNItems = new ObservableCollection<ItemMasterList>();
            foreach (var item in model.GRNItemLists)
            {
                ItemMasterList newItems = new ItemMasterList();
                newItems.ItemListUID = item.GRNItemListUID;
                newItems.ItemMasterUID = item.ItemMasterUID;
                newItems.ItemCode = item.ItemCode;
                newItems.ItemName = item.ItemName;
                newItems.IMUOMUID = item.IMUOMUID;
                newItems.Quantity = item.Quantity;
                newItems.BatchID = item.BatchID;
                newItems.ExpiryDttm = item.ExpiryDttm;
                newItems.UnitPrice = item.PurchaseCost;
                newItems.FreeQuantity = item.FreeQuantity;
                newItems.Discount = item.Discount;
                newItems.TaxPercentage = item.TaxPercentage;
                newItems.NetAmount = item.NetAmount ?? 0;
                newItems.SerialNumber = item.SerialNumber;
                GRNItems.Add(newItems);
            }
        }

        public void AssingPropertiesToModel()
        {
            if (model == null)
            {
                model = new GRNDetailModel();
            }
            model.GRNTYPUID = SelectGRNType.Key;
            model.VendorDetailUID = SelectVendor.VendorDetailUID;
            model.RecievedOrganisationUID = SelectOrganisation.HealthOrganisationUID;
            model.RecievedStoreUID = SelectStore.StoreUID;
            model.InvoiceNo = InvoinceNo;
            model.InvoiceDate = InvoinceDate;
            model.RecievedDttm = ReceiveDate;
            model.Comments = Comments;
            model.NetAmount = NetAmount;
            model.OtherCharges = OtherChages;
            model.Discount = Discount;



            model.GRNItemLists = new List<GRNItemListModel>();
            foreach (var item in GRNItems)
            {

                GRNItemListModel newRow = new GRNItemListModel();
                newRow.GRNItemListUID = item.ItemListUID;
                newRow.ItemMasterUID = item.ItemMasterUID;
                newRow.ItemName = item.ItemName;
                newRow.ItemCode = item.ItemCode;
                newRow.Quantity = item.Quantity ?? 0;
                newRow.IMUOMUID = item.IMUOMUID;
                newRow.ExpiryDttm = item.ExpiryDttm;
                newRow.BatchID = item.BatchID;
                newRow.ManufacturerUID = item.ManufacturerUID;
                newRow.PurchaseCost = item.UnitPrice ?? 0;
                newRow.FreeQuantity = item.FreeQuantity;
                newRow.Discount = item.Discount;
                newRow.TaxPercentage = item.TaxPercentage;
                newRow.NetAmount = item.NetAmount;
                newRow.SerialNumber = item.SerialNumber;
                model.GRNItemLists.Add(newRow);
            }
        }
        #endregion
    }
}
