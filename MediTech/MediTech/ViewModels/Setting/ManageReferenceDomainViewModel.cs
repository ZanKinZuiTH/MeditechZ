using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MediTech.DataService;
using MediTech.Model;
using MediTech.Models;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace MediTech.ViewModels
{
    public class ManageReferenceDomainViewModel : MediTechViewModelBase
    {
        #region Properties

        private bool _IsUpdate = false;

        public bool IsUpdate
        {
            get { return _IsUpdate; }
            set { Set(ref _IsUpdate, value); }
        }


        private string _DomainCode;

        public string DomainCode
        {
            get { return _DomainCode; }
            set { Set(ref _DomainCode, value); }
        }

        private string _Description;

        public string Description
        {
            get { return _Description; }
            set { Set(ref _Description, value); }
        }


        private DateTime _ActiveFrom = DateTime.Now;

        public DateTime ActiveFrom
        {
            get { return _ActiveFrom; }
            set { Set(ref _ActiveFrom, value); }
        }


        private DateTime? _ActiveTo;

        public DateTime? ActiveTo
        {
            get { return _ActiveTo; }
            set { Set(ref _ActiveTo, value); }
        }


        private string _RefValueCode;

        public string RefValueCode
        {
            get { return _RefValueCode; }
            set { Set(ref _RefValueCode, value); }
        }


        private string _RefValueDescription;

        public string RefValueDescription
        {
            get { return _RefValueDescription; }
            set { Set(ref _RefValueDescription, value); }
        }

        private DateTime? _RefValueActiveFrom = DateTime.Now;

        public DateTime? RefValueActiveFrom
        {
            get { return _RefValueActiveFrom; }
            set { Set(ref _RefValueActiveFrom, value); }
        }

        private DateTime? _RefValueActiveTo;

        public DateTime? RefValueActiveTo
        {
            get { return _RefValueActiveTo; }
            set { Set(ref _RefValueActiveTo, value); }
        }

        private int? _RefValueDisplay;

        public int? RefValueDisplay
        {
            get { return _RefValueDisplay; }
            set { Set(ref _RefValueDisplay, value); }
        }

        private double? _RefValueNumericValue;

        public double? RefValueNumericValue
        {
            get { return _RefValueNumericValue; }
            set { Set(ref _RefValueNumericValue, value); }
        }

        private string _RefValueAlternateName;

        public string RefValueAlternateName
        {
            get { return _RefValueAlternateName; }
            set { Set(ref _RefValueAlternateName, value); }
        }

        private ObservableCollection<ReferencevalueModel> _ReferenceValueSource;

        public ObservableCollection<ReferencevalueModel> ReferenceValueSource
        {
            get { return _ReferenceValueSource; }
            set { Set(ref _ReferenceValueSource, value); }
        }

        private ReferencevalueModel _SelectedReferenceValue;

        public ReferencevalueModel SelectedReferenceValue
        {
            get { return _SelectedReferenceValue; }
            set
            {
                Set(ref _SelectedReferenceValue, value);
                if (_SelectedReferenceValue != null)
                {
                    RefValueCode = _SelectedReferenceValue.ValueCode;
                    RefValueDescription = _SelectedReferenceValue.Description;
                    RefValueActiveFrom = _SelectedReferenceValue.ActiveFrom;
                    RefValueActiveTo = _SelectedReferenceValue.ActiveTo;
                    RefValueDisplay = _SelectedReferenceValue.DisplayOrder;
                    RefValueNumericValue = _SelectedReferenceValue.NumericValue;
                    RefValueAlternateName = _SelectedReferenceValue.AlternateName;
                }
            }
        }


        #endregion

        #region Command

        private RelayCommand _InsertCommand;
        public RelayCommand InsertCommand
        {
            get { return _InsertCommand ?? (_InsertCommand = new RelayCommand(InsertReferenceValue)); }
        }



        private RelayCommand _UpdateCommand;
        public RelayCommand UpdateCommand
        {
            get { return _UpdateCommand ?? (_UpdateCommand = new RelayCommand(UpdateReferenceValue)); }
        }


        private RelayCommand _DeleteCommand;
        public RelayCommand DeleteCommand
        {
            get { return _DeleteCommand ?? (_DeleteCommand = new RelayCommand(DeleteReferenceValue)); }
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

        #endregion

        #region Varible
        ReferenceDomainModel refDomainModel;
        #endregion

        #region Method

        public ManageReferenceDomainViewModel()
        {
            
        }

        private void InsertReferenceValue()
        {
            if (String.IsNullOrEmpty(RefValueCode))
            {
                WarningDialog("กรุณาใส่ Value Code");
                return;
            }

            if (string.IsNullOrEmpty(RefValueDescription))
            {
                WarningDialog("กรุณาใส่เนื้อหา");
                return;
            }

            if (ReferenceValueSource == null)
            {
                ReferenceValueSource = new ObservableCollection<ReferencevalueModel>();
            }

            ReferencevalueModel newRefValue = new ReferencevalueModel();
            newRefValue.ValueCode = RefValueCode;
            newRefValue.Description = RefValueDescription;
            newRefValue.ActiveFrom = RefValueActiveFrom;
            newRefValue.ActiveTo = RefValueActiveTo;
            newRefValue.DisplayOrder = RefValueDisplay;
            newRefValue.NumericValue = RefValueNumericValue;
            newRefValue.AlternateName = RefValueAlternateName;
            ReferenceValueSource.Add(newRefValue);
            ClearControl();
        }

        private void UpdateReferenceValue()
        {
            if (SelectedReferenceValue != null)
            {
                if (String.IsNullOrEmpty(RefValueCode))
                {
                    WarningDialog("กรุณาใส่ Value Code");
                    return;
                }

                if (string.IsNullOrEmpty(RefValueDescription))
                {
                    WarningDialog("กรุณาใส่เนื้อหา");
                    return;
                }
                if (SelectedReferenceValue != null)
                {
                    SelectedReferenceValue.ValueCode = RefValueCode;
                    SelectedReferenceValue.Description = RefValueDescription;
                    SelectedReferenceValue.ActiveFrom = RefValueActiveFrom;
                    SelectedReferenceValue.ActiveTo = RefValueActiveTo;
                    SelectedReferenceValue.DisplayOrder = RefValueDisplay;
                    SelectedReferenceValue.NumericValue = RefValueNumericValue;
                    SelectedReferenceValue.AlternateName = RefValueAlternateName;
                    SelectedReferenceValue.IsUpdate = "Y";
                    OnUpdateEvent();
                    ClearControl();
                }
            }


        }

        private void DeleteReferenceValue()
        {
            if (_SelectedReferenceValue != null)
            {
                try
                {
                    DialogResult result = DeleteDialog();
                    if (result == DialogResult.Yes)
                    {
                        ReferenceValueSource.Remove(_SelectedReferenceValue);
                        ClearControl();
                    }

                }
                catch (Exception er)
                {

                    ErrorDialog(er.Message);
                }

            }


        }

        private void Save()
        {
            try
            {
                if (String.IsNullOrEmpty(DomainCode))
                {
                    WarningDialog("กรุณาใส่ Domain Code");
                    return;
                }

                if (string.IsNullOrEmpty(Description))
                {
                    WarningDialog("กรุณาใส่เนื้อหา Domain");
                    return;
                }


                if (ReferenceValueSource == null || ReferenceValueSource.Count <= 0)
                {
                    WarningDialog("กรุณาใส่ Reference Value อย่างน้อย 1 รายการ");
                    return;
                }

                AssingProperitesToModel();
                if (DataService.Technical.SaveReferenceDomain(refDomainModel, AppUtil.Current.UserID))
                {
                    SaveSuccessDialog();
                    GotoReferenceDomainPage();
                }

            }
            catch (Exception er)
            {
                ErrorDialog(er.Message);
            }

        }
        private void Cancel()
        {
            GotoReferenceDomainPage();
        }

        private void GotoReferenceDomainPage()
        {
            ListReferenceDomain page = new ListReferenceDomain();
            ChangeViewPermission(page);
        }

        public void AssingModel(ReferenceDomainModel referenceData)
        {
            refDomainModel = referenceData;
            AssingModelToProperties();
            IsUpdate = true;
        }

        public void AssingProperitesToModel()
        {
            if (refDomainModel == null)
            {
                refDomainModel = new ReferenceDomainModel();
            }
            refDomainModel.DomainCode = DomainCode;
            refDomainModel.Description = Description;
            refDomainModel.ActiveFrom = ActiveFrom;
            refDomainModel.ActiveTo = ActiveTo;
            refDomainModel.ReferenceValues = ReferenceValueSource.ToList();
        }

        public void ClearControl()
        {
            RefValueCode = string.Empty;
            RefValueDescription = string.Empty;
            RefValueActiveFrom = DateTime.Now;
            RefValueActiveTo = null;
            RefValueDisplay = null;
            RefValueNumericValue = null;
            RefValueAlternateName = string.Empty;
            SelectedReferenceValue = null;
        }
        public void AssingModelToProperties()
        {
            DomainCode = refDomainModel.DomainCode;
            Description = refDomainModel.Description;
            ActiveFrom = refDomainModel.ActiveFrom;
            ActiveTo = refDomainModel.ActiveTo;
            if (refDomainModel.ReferenceValues != null)
            {
                ReferenceValueSource = new ObservableCollection<ReferencevalueModel>(refDomainModel.ReferenceValues);
            }
        }
        #endregion

    }
}
