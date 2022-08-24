using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class SearchItemViewModel :MediTechViewModelBase
    {
        #region Properties
        
        private string _ItemCode;
        public string ItemCode
        {
            get { return _ItemCode; }
            set { Set(ref _ItemCode, value); }
        }

        private string _ItemName;
        public string ItemName
        {
            get { return _ItemName; }
            set { Set(ref _ItemName, value); }
        }

        private List<LookupReferenceValueModel> _ServiceTypes;
        public List<LookupReferenceValueModel> ServiceTypes
        {
            get { return _ServiceTypes; }
            set { Set(ref _ServiceTypes, value); }
        }

        private LookupReferenceValueModel _SelectServiceTypes;
        public  LookupReferenceValueModel SelectServiceTypes
        {
            get { return _SelectServiceTypes; }
            set { Set(ref _SelectServiceTypes, value);
                if (_SelectServiceTypes != null)
                {
                }
            }
        }

        private List<ItemServiceModel> _ItemMaster;
        public List<ItemServiceModel> ItemMaster
        {
            get { return _ItemMaster; }
            set { Set(ref _ItemMaster, value); }
        }

        private ItemServiceModel _SelectItemMaster;
        public ItemServiceModel SelectItemMaster
        {
            get { return _SelectItemMaster; }
            set { Set(ref _SelectItemMaster, value); }
        }


        #endregion

        #region Command
        private RelayCommand _SearchCommand;
        public RelayCommand SearchCommand
        {
            get { return _SearchCommand ?? (_SearchCommand = new RelayCommand(Search)); }
        }

        private RelayCommand _ClaerCommand;
        public RelayCommand ClaerCommand
        {
            get { return _ClaerCommand ?? (_ClaerCommand = new RelayCommand(Clear)); }
        }

        private RelayCommand _AddCommand;
        public RelayCommand AddCommand
        {
            get { return _AddCommand ?? (_AddCommand = new RelayCommand(Add)); }
        }

        private RelayCommand _CancelCommand;
        public RelayCommand CancelCommand
        {
            get { return _CancelCommand ?? (_CancelCommand = new RelayCommand(Cancel)); }
        }
        #endregion

        #region Method
        
        public SearchItemViewModel()
        {
            ServiceTypes = DataService.Technical.GetReferenceValueMany("BSMDD");
        }

        public void AssignModel(int type)
        {
            SelectServiceTypes = ServiceTypes.FirstOrDefault(p => p.Key == type);
           
        }

        public void Search()
        {
            if(SelectServiceTypes != null)
            {
                var ITMTYP = DataService.Technical.GetReferenceValueMany("ITMTYP");
                var TSTTP = DataService.Technical.GetReferenceValueMany("TSTTP");
                int? ITMTYPUID = 0;
                int TSTTPUID = 0;
                if (SelectServiceTypes.Display == "Drug")
                {
                    ITMTYPUID = ITMTYP.FirstOrDefault(p => p.ValueCode.Contains("DGITM")).Key;
                    var data = DataService.Inventory.SearchItemMaster(ItemName, ItemCode, ITMTYPUID);
                    ItemMaster = data.Select(p => new ItemServiceModel
                    {
                        ItemUID = p.ItemMasterUID,
                        Code = p.Code,
                        Name = p.Name,
                        ActiveFrom = p.ActiveFrom,
                        ActiveTo = p.ActiveTo,
                        Manufacturer = p.Manufacturer
                    }).ToList();

                }
                else if (SelectServiceTypes.Display == "Medical Supplies")
                {
                    ITMTYPUID = ITMTYP.FirstOrDefault(p => p.ValueCode.Contains("MDITM")).Key;
                    var data = DataService.Inventory.SearchItemMaster(ItemName, ItemCode, ITMTYPUID);
                    ItemMaster = data.Select(p => new ItemServiceModel
                    {
                        ItemUID = p.ItemMasterUID,
                        Code = p.Code,
                        Name = p.Name,
                        ActiveFrom = p.ActiveFrom,
                        ActiveTo = p.ActiveTo
                    }).ToList();
                }
                else if (SelectServiceTypes.Display == "Supply")
                {
                    ITMTYPUID = ITMTYP.FirstOrDefault(p => p.ValueCode.Contains("MDITM")).Key;
                    var data = DataService.Inventory.SearchItemMaster(ItemName, ItemCode, ITMTYPUID);
                    ItemMaster = data.Select(p => new ItemServiceModel
                    {
                        ItemUID = p.ItemMasterUID,
                        Code = p.Code,
                        Name = p.Name,
                        ActiveFrom = p.ActiveFrom,
                        ActiveTo = p.ActiveTo
                    }).ToList();
                }
                else if(SelectServiceTypes.Display == "Lab Test")
                {
                    TSTTPUID = TSTTP.FirstOrDefault(p => p.ValueCode.Contains("LABTP")).Key ?? 0;
                    var data = DataService.MasterData.SearchRequestItemByCategory(TSTTPUID, ItemCode, ItemName);
                    ItemMaster = data.Select(p => new ItemServiceModel
                    {
                        ItemUID = p.RequestItemUID,
                        Code = p.Code,
                        Name = p.ItemName,
                        ActiveFrom = p.EffectiveFrom,
                        ActiveTo = p.EffectiveTo
                    }).ToList();
                }
                else if (SelectServiceTypes.Display == "Radiology")
                {
                    TSTTPUID = TSTTP.FirstOrDefault(p => p.ValueCode.Contains("RADTP")).Key ?? 0;
                    var data = DataService.MasterData.SearchRequestItemByCategory(TSTTPUID, ItemCode, ItemName);
                    ItemMaster = data.Select(p => new ItemServiceModel
                    {
                        ItemUID = p.RequestItemUID,
                        Code = p.Code,
                        Name = p.ItemName,
                        ActiveFrom = p.EffectiveFrom,
                        ActiveTo = p.EffectiveTo
                    }).ToList();
                }
                else if (SelectServiceTypes.Display == "Mobile Checkup")
                {
                    TSTTPUID = TSTTP.FirstOrDefault(p => p.ValueCode.Contains("RADTP")).Key ?? 0;
                    var data = DataService.MasterData.SearchRequestItemByCategory(TSTTPUID, ItemCode, ItemName);
                    ItemMaster = data.Select(p => new ItemServiceModel
                    {
                        ItemUID = p.RequestItemUID,
                        Code = p.Code,
                        Name = p.ItemName,
                        ActiveFrom = p.EffectiveFrom,
                        ActiveTo = p.EffectiveTo
                    }).ToList();
                }


                //GetItemMasterByType("Drug");
                //GetItemMasterByType("Medical Supplies");
                //GetItemMasterByType("Supply");
                var fsd = DataService.Inventory.SearchItemMaster("", "", SelectServiceTypes.Key);
            }
        }

        public void Add()
        {
            if(SelectItemMaster == null)
            {
                WarningDialog("กรุณาเลือก Item");
                return;
            }

            CloseViewDialog(ActionDialog.Save);
        }

        public void Clear()
        {
            ItemName = null;
            ItemCode = null;
        }

        public void Cancel()
        {
            CloseViewDialog(ActionDialog.Cancel);
        }
        #endregion
    }
}
