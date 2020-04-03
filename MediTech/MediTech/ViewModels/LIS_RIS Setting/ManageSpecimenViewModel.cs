using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class ManageSpecimenViewModel : MediTechViewModelBase
    {
        #region Properties

        private string _Code;

        public string Code
        {
            get { return _Code; }
            set { Set(ref _Code, value); }
        }



        private string _SpecimenName;

        public string SpecimenName
        {
            get { return _SpecimenName; }
            set { Set(ref _SpecimenName, value); }
        }

        private List<LookupReferenceValueModel> _SpecimenType;

        public List<LookupReferenceValueModel> SpecimenType
        {
            get { return _SpecimenType; }
            set { Set(ref _SpecimenType, value); }
        }

        private List<LookupReferenceValueModel> _CollectionRequired;

        public List<LookupReferenceValueModel> CollectionRequired
        {
            get { return _CollectionRequired; }
            set { Set(ref _CollectionRequired, value); }
        }

        private double? _VolumenToBeCollection;

        public double? VolumenToBeCollection
        {
            get { return _VolumenToBeCollection; }
            set { Set(ref _VolumenToBeCollection, value); }
        }

        private List<LookupReferenceValueModel> _UnitofMeasure;

        public List<LookupReferenceValueModel> UnitofMeasure
        {
            get { return _UnitofMeasure; }
            set
            {
                Set(ref _UnitofMeasure, value);
            }
        }

        private DateTime? _ExpiryDate;

        public DateTime? ExpiryDate
        {
            get { return _ExpiryDate; }
            set { Set(ref _ExpiryDate, value); }
        }

        private List<LookupReferenceValueModel> _CollectionSite;

        public List<LookupReferenceValueModel> CollectionSite
        {
            get { return _CollectionSite; }
            set { Set(ref _CollectionSite, value); }
        }

        private List<LookupReferenceValueModel> _CollectionRoute;

        public List<LookupReferenceValueModel> CollectionRoute
        {
            get { return _CollectionRoute; }
            set { Set(ref _CollectionRoute, value); }
        }

        private List<LookupReferenceValueModel> _CollectionMethod;

        public List<LookupReferenceValueModel> CollectionMethod
        {
            get { return _CollectionMethod; }
            set { Set(ref _CollectionMethod, value); }
        }

        private string _StorageInstruction;

        public string StorageInstruction
        {
            get { return _StorageInstruction; }
            set { Set(ref _StorageInstruction, value); }
        }

        private string _HandingInstruction;

        public string HandingInstruction
        {
            get { return _HandingInstruction; }
            set { Set(ref _HandingInstruction, value); }
        }

        private LookupReferenceValueModel _SelectSpecimenType;

        public LookupReferenceValueModel SelectSpecimenType
        {
            get { return _SelectSpecimenType; }
            set { Set(ref _SelectSpecimenType, value); }
        }

        private LookupReferenceValueModel _SelectCollectionRequired;

        public LookupReferenceValueModel SelectCollectionRequired
        {
            get { return _SelectCollectionRequired; }
            set { Set(ref _SelectCollectionRequired, value); }
        }


        private LookupReferenceValueModel _SelectUnitofMeasure;

        public LookupReferenceValueModel SelectUnitofMeasure
        {
            get { return _SelectUnitofMeasure; }
            set { Set(ref _SelectUnitofMeasure, value); }
        }

        private LookupReferenceValueModel _SelectCollectionSite;

        public LookupReferenceValueModel SelectCollectionSite
        {
            get { return _SelectCollectionSite; }
            set { Set(ref _SelectCollectionSite, value); }
        }


        private LookupReferenceValueModel _SelectCollectionRoute;

        public LookupReferenceValueModel SelectCollectionRoute
        {
            get { return _SelectCollectionRoute; }
            set { Set(ref _SelectCollectionRoute, value); }
        }

        private LookupReferenceValueModel _SelectCollectionMethod;

        public LookupReferenceValueModel SelectCollectionMethod
        {
            get { return _SelectCollectionMethod; }
            set { Set(ref _SelectCollectionMethod, value); }
        }

        #endregion

        #region Command

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

        private bool _IsEnabledEdit = true;

        public bool IsEnabledEdit
        {
            get { return _IsEnabledEdit; }
            set { Set(ref _IsEnabledEdit, value); }
        }

        #endregion

        #region Method

        SpecimenModel DataModel;

        public ManageSpecimenViewModel()
        {
            var refValue = DataService.Technical.GetReferenceValueList("SPMTP,RIUOM,COLST,CLROU,COLMD");
            CollectionRequired = new List<LookupReferenceValueModel>();
            CollectionRequired.Add(new LookupReferenceValueModel { Key = 0, Display = "No" });
            CollectionRequired.Add(new LookupReferenceValueModel { Key = 1, Display = "Yes" });

            SpecimenType = refValue.Where(p => p.DomainCode == "SPMTP").ToList();
            UnitofMeasure = refValue.Where(p => p.DomainCode == "RIUOM").ToList();
            CollectionSite = refValue.Where(p => p.DomainCode == "COLST").ToList();
            CollectionRoute = refValue.Where(p => p.DomainCode == "CLROU").ToList();
            CollectionMethod = refValue.Where(p => p.DomainCode == "COLMD").ToList();
        }

        private void Save()
        {
            try
            {
                if (string.IsNullOrEmpty(Code))
                {
                    WarningDialog("กรุณาใส่ Code");
                    return;
                }
                if (string.IsNullOrEmpty(SpecimenName))
                {
                    WarningDialog("กรุณาใส่ Name");
                    return;
                }

                if (DataModel == null)
                {
                    var dupicateCode = DataService.MasterData.GetSpecimenByCode(Code);
                    if (dupicateCode != null)
                    {
                        WarningDialog("Code ซ้ำ โปรดตรวจสอบ");
                        return;
                    }
                }

                AssignPropertiesToModel();
                DataService.MasterData.ManageSpecimen(DataModel, AppUtil.Current.UserID);
                SaveSuccessDialog();
                ListSpecimen pageList = new ListSpecimen();
                ChangeViewPermission(pageList);
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }

        private void Cancel()
        {
            ListSpecimen pageList = new ListSpecimen();
            ChangeViewPermission(pageList);
        }

        public void EditData(int specimenUID)
        {
            DataModel = DataService.MasterData.GetSpecimenByUID(specimenUID);
            BidingData();
        }

        private void BidingData()
        {
            Code = DataModel.Code;
            SpecimenName = DataModel.Name;
            SelectSpecimenType = SpecimenType.FirstOrDefault(p => p.Key == DataModel.SPMTPUID);
            SelectCollectionRequired = CollectionRequired.FirstOrDefault(p => p.Key == (DataModel.IsVolumeCollectionReqd ?? false ? 1 : 0));
            SelectUnitofMeasure = UnitofMeasure.FirstOrDefault(p => p.Key == DataModel.VOUNTUID);
            SelectCollectionSite = CollectionSite.FirstOrDefault(p => p.Key == DataModel.COLSTUID);
            SelectCollectionRoute = CollectionRoute.FirstOrDefault(p => p.Key == DataModel.CLROUUID);
            SelectCollectionMethod = CollectionMethod.FirstOrDefault(p => p.Key == DataModel.COLMDUID);
            VolumenToBeCollection = DataModel.VolumeCollected;
            StorageInstruction = DataModel.StorageInstuction;
            HandingInstruction = DataModel.HandlingInstruction;
            ExpiryDate = DataModel.ExpiryDttm;

            IsEnabledEdit = false;
        }



        private void AssignPropertiesToModel()
        {
            if (DataModel == null)
            {
                DataModel = new SpecimenModel();
            }
            DataModel.Code = Code;
            DataModel.Name = SpecimenName;
            if (SelectSpecimenType != null)
                DataModel.SPMTPUID = SelectSpecimenType.Key;
            if (SelectCollectionRequired != null)
                DataModel.IsVolumeCollectionReqd = SelectCollectionRequired.Key == 1 ? true : false;
            if (SelectUnitofMeasure != null)
                DataModel.VOUNTUID = SelectUnitofMeasure.Key;
            if (SelectCollectionSite != null)
                DataModel.COLSTUID = SelectCollectionSite.Key;
            if (SelectCollectionRoute != null)
                DataModel.CLROUUID = SelectCollectionRoute.Key;
            if (SelectCollectionMethod != null)
                DataModel.COLMDUID = SelectCollectionMethod.Key;
            DataModel.VolumeCollected = VolumenToBeCollection;
            DataModel.StorageInstuction = StorageInstruction;
            DataModel.HandlingInstruction = HandingInstruction;
            DataModel.ExpiryDttm = ExpiryDate;

        }

        #endregion
    }
}
