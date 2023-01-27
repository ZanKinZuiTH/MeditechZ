using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using MediTech.Model;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using MediTech.DataService;
using MediTech.Views;
using System.Windows.Forms;
using MediTech.Interface;

namespace MediTech.ViewModels
{
    public class PatientDiagnosisViewModel : MediTechViewModelBase,IPatientVisitViewModel
    {

        #region Properties

        private Visibility _IsBanner = Visibility.Collapsed;

        public Visibility IsBanner
        {
            get { return _IsBanner; }
            set { Set(ref _IsBanner, value); }
        }

        private PatientVisitModel _SelectedPatientVisit;

        public PatientVisitModel SelectedPatientVisit
        {
            get { return _SelectedPatientVisit; }
            set
            {
                Set(ref _SelectedPatientVisit, value);
                PatientProblemList = null;
                SearchProblemList = null;
                if (SelectedPatientVisit != null)
                {
                    List<PatientProblemModel> data = DataService.PatientDiagnosis.GetPatientProblemByVisitUID(SelectedPatientVisit.PatientVisitUID);
                    AssignModel(data);
                }
                ClearControl();
                //else
                //{
                //    ClearControl();
                //    PatientProblemList = null;
                //    SearchProblemList = null;
                //}

            }
        }

        public string SearchDiagnosisCriteria { get; set; }

        private ObservableCollection<ProblemModel> _SearchProblemList;

        public ObservableCollection<ProblemModel> SearchProblemList
        {
            get { return _SearchProblemList; }
            set { Set(ref _SearchProblemList, value); }
        }

        private ProblemModel _SelectProblemList;

        public ProblemModel SelectProblemList
        {
            get { return _SelectProblemList; }
            set { Set(ref _SelectProblemList, value); }
        }

        public string SearchFavouritesCriteria { get; set; }

        private List<FavouriteItemModel> _AllFavouritesList;

        public List<FavouriteItemModel> AllFavouritesList
        {
            get { return _AllFavouritesList; }
            set { _AllFavouritesList = value; }
        }



        private ObservableCollection<FavouriteItemModel> _FavouritesItemList;

        public ObservableCollection<FavouriteItemModel> FavouritesItemList
        {
            get { return _FavouritesItemList; }
            set { Set(ref _FavouritesItemList, value); }
        }


        private FavouriteItemModel _SelectFavouritesItemList;

        public FavouriteItemModel SelectFavouritesItemList
        {
            get { return _SelectFavouritesItemList; }
            set { Set(ref _SelectFavouritesItemList, value); }
        }


        private ProblemModel _SelectedProblem;

        public ProblemModel SelectedProblem
        {
            get { return _SelectedProblem; }
            set { Set(ref _SelectedProblem, value); }
        }

        private List<LookupReferenceValueModel> _DiagnosisType;

        public List<LookupReferenceValueModel> DiagnosisType
        {
            get { return _DiagnosisType; }
            set { Set(ref _DiagnosisType, value); }
        }

        private LookupReferenceValueModel _SelectDiagnosisType;

        public LookupReferenceValueModel SelectDiagnosisType
        {
            get { return _SelectDiagnosisType; }
            set { Set(ref _SelectDiagnosisType, value); }
        }

        private bool _IsUnderline;

        public bool IsUnderline
        {
            get { return _IsUnderline; }
            set { Set(ref _IsUnderline, value); }
        }

        private DateTime? _OnsetDttm;

        public DateTime? OnsetDttm
        {
            get { return _OnsetDttm; }
            set { Set(ref _OnsetDttm, value); }
        }

        private List<LookupReferenceValueModel> _Severity;

        public List<LookupReferenceValueModel> Severity
        {
            get { return _Severity; }
            set { Set(ref _Severity, value); }
        }

        private LookupReferenceValueModel _SelectSeverity;

        public LookupReferenceValueModel SelectSeverity
        {
            get { return _SelectSeverity; }
            set { Set(ref _SelectSeverity, value); }
        }

        private List<LookupReferenceValueModel> _Accuracy;

        public List<LookupReferenceValueModel> Accuracy
        {
            get { return _Accuracy; }
            set { Set(ref _Accuracy, value); }
        }


        private LookupReferenceValueModel _SelectAccuracy;

        public LookupReferenceValueModel SelectAccuracy
        {
            get { return _SelectAccuracy; }
            set { Set(ref _SelectAccuracy, value); }
        }

        private List<LookupReferenceValueModel> _Encounter;

        public List<LookupReferenceValueModel> Encounter
        {
            get { return _Encounter; }
            set { Set(ref _Encounter, value); }
        }

        private LookupReferenceValueModel _SelectEncounter;

        public LookupReferenceValueModel SelectEncounter
        {
            get { return _SelectEncounter; }
            set { Set(ref _SelectEncounter, value); }
        }

        private List<LookupReferenceValueModel> _BodyLocation;

        public List<LookupReferenceValueModel> BodyLocation
        {
            get { return _BodyLocation; }
            set { Set(ref _BodyLocation, value); }
        }

        private LookupReferenceValueModel _SelectBodyLocation;

        public LookupReferenceValueModel SelectBodyLocation
        {
            get { return _SelectBodyLocation; }
            set { Set(ref _SelectBodyLocation, value); }
        }

        private DateTime? _RecordDttm;

        public DateTime? RecordDttm
        {
            get { return _RecordDttm; }
            set { Set(ref _RecordDttm, value); }
        }

        private DateTime? _CloseDttm;

        public DateTime? CloseDttm
        {
            get { return _CloseDttm; }
            set { Set(ref _CloseDttm, value); }
        }

        private string _ClosureComments;

        public string ClosureComments
        {
            get { return _ClosureComments; }
            set { Set(ref _ClosureComments, value); }
        }

        private ObservableCollection<PatientProblemModel> _PatientProblemList;

        public ObservableCollection<PatientProblemModel> PatientProblemList
        {
            get { return _PatientProblemList ?? (_PatientProblemList = new ObservableCollection<PatientProblemModel>()); }
            set { Set(ref _PatientProblemList, value); }
        }

        private PatientProblemModel _SelectPatientProblemList;

        public PatientProblemModel SelectPatientProblemList
        {
            get { return _SelectPatientProblemList; }
            set
            {
                Set(ref _SelectPatientProblemList, value);
                if (SelectPatientProblemList != null)
                {
                    SelectedProblem = new ProblemModel() { ProblemUID = SelectPatientProblemList.ProblemUID, Code = SelectPatientProblemList.ProblemCode, Name = SelectPatientProblemList.ProblemName, Description = SelectPatientProblemList.ProblemDescription };
                    IsUnderline = SelectPatientProblemList.IsUnderline == "Y" ? true : false;
                    SelectDiagnosisType = DiagnosisType.FirstOrDefault(p => p.Key == SelectPatientProblemList.DIAGTYPUID);
                    OnsetDttm = SelectPatientProblemList.OnsetDttm;
                    SelectSeverity = Severity.FirstOrDefault(p => p.Key == SelectPatientProblemList.SEVRTUID);
                    SelectAccuracy = Accuracy.FirstOrDefault(p => p.Key == SelectPatientProblemList.CERNTUID);
                    SelectEncounter = Encounter.FirstOrDefault(p => p.Key == SelectPatientProblemList.PBMTYUID);
                    SelectBodyLocation = BodyLocation.FirstOrDefault(p => p.Key == SelectPatientProblemList.BDLOCUID);
                    RecordDttm = SelectPatientProblemList.RecordedDttm;
                    CloseDttm = SelectPatientProblemList.ClosureDttm;
                    ClosureComments = SelectPatientProblemList.ClosureComments;
                }
            }
        }

        private List<PatientProblemModel> _DiagnosisHistory;

        public List<PatientProblemModel> DiagnosisHistory
        {
            get { return _DiagnosisHistory; }
            set { Set(ref _DiagnosisHistory, value); }
        }

        private PatientProblemModel _SelectDiagnosisHistory;

        public PatientProblemModel SelectDiagnosisHistory
        {
            get { return _SelectDiagnosisHistory; }
            set { Set(ref _SelectDiagnosisHistory, value); }
        }


        private int _SelectIndexDiasHis;

        public int SelectIndexDiasHis
        {
            get { return _SelectIndexDiasHis; }
            set
            {
                Set(ref _SelectIndexDiasHis, value);
                LoadPatientHistoryProblem();
            }
        }

        private bool _IsVisitMass = false;

        public bool IsVisitMass
        {
            get { return _IsVisitMass; }
            set { _IsVisitMass = value; }
        }

        #endregion

        #region Command


        private RelayCommand _AddProblemCommand;

        public RelayCommand AddProblemCommand
        {
            get { return _AddProblemCommand ?? (_AddProblemCommand = new RelayCommand(AddProblem)); }
        }

        private RelayCommand _EditProblemCommand;

        public RelayCommand EditProblemCommand
        {
            get { return _EditProblemCommand ?? (_EditProblemCommand = new RelayCommand(EditProblem)); }
        }


        private RelayCommand _RemoveProblemCommand;

        public RelayCommand RemoveProblemCommand
        {
            get { return _RemoveProblemCommand ?? (_RemoveProblemCommand = new RelayCommand(RemoveProblem)); }
        }

        private RelayCommand _SearchProblemDoubleClickCommand;

        public RelayCommand SearchProblemDoubleClickCommand
        {
            get { return _SearchProblemDoubleClickCommand ?? (_SearchProblemDoubleClickCommand = new RelayCommand(SearchProblemDoubleClick)); }
        }

        private RelayCommand<System.Windows.Input.KeyEventArgs> _SearchDiagnosisEnterCommand;

        public RelayCommand<System.Windows.Input.KeyEventArgs> SearchDiagnosisEnterCommand
        {
            get { return _SearchDiagnosisEnterCommand ?? (_SearchDiagnosisEnterCommand = new RelayCommand<System.Windows.Input.KeyEventArgs>(SearchDiagnosisEnter)); }
        }



        private RelayCommand _AddFavoritesCommand;

        public RelayCommand AddFavoritesCommand
        {
            get { return _AddFavoritesCommand ?? (_AddFavoritesCommand = new RelayCommand(AddFavorites)); }
        }

        private RelayCommand _RemoveFavoritesCommand;

        public RelayCommand RemoveFavoritesCommand
        {
            get { return _RemoveFavoritesCommand ?? (_RemoveFavoritesCommand = new RelayCommand(RemoveFavorites)); }
        }

        private RelayCommand _SearchFavouritesDobuleClickCommand;

        public RelayCommand SearchFavouritesDobuleClickCommand
        {
            get { return _SearchFavouritesDobuleClickCommand ?? (_SearchFavouritesDobuleClickCommand = new RelayCommand(SearchFavouritesDobuleClick)); }
        }


        private RelayCommand<System.Windows.Input.KeyEventArgs> _SearchFavoritesEnterCommand;
        public RelayCommand<System.Windows.Input.KeyEventArgs> SearchFavoritesEnterCommand
        {
            get { return _SearchFavoritesEnterCommand ?? (_SearchFavoritesEnterCommand = new RelayCommand<System.Windows.Input.KeyEventArgs>(SearchFavoritesEnter)); }
        }

        private RelayCommand _DiagnosisHistoryDoubleClickCommand;

        public RelayCommand DiagnosisHistoryDoubleClickCommand
        {
            get { return _DiagnosisHistoryDoubleClickCommand ?? (_DiagnosisHistoryDoubleClickCommand = new RelayCommand(DiagnosisHistoryDoubleClick)); }
        }

        private RelayCommand _SaveCommand;

        public RelayCommand SaveCommand
        {
            get { return _SaveCommand ?? (_SaveCommand = new RelayCommand(SavePatientProblem)); }
        }

        private RelayCommand _CancelCommand;

        public RelayCommand CancelCommand
        {
            get { return _CancelCommand ?? (_CancelCommand = new RelayCommand(Cancel)); }
        }

        #endregion

        #region Method

        #region Varible

        List<PatientProblemModel> model;

        #endregion

        public PatientDiagnosisViewModel()
        {
            AllFavouritesList = DataService.PatientDiagnosis.GetFavouriteItemByUser(AppUtil.Current.UserID);
            var refData = DataService.Technical.GetReferenceValueList("SEVRT,PBMTY,DIAGTYP,CERNT,BDLOC");
            DiagnosisType = refData.Where(p => p.DomainCode == "DIAGTYP").ToList();
            Severity = refData.Where(p => p.DomainCode == "SEVRT").ToList();
            Encounter = refData.Where(p => p.DomainCode == "PBMTY").ToList();
            Accuracy = refData.Where(p => p.DomainCode == "CERNT").ToList();
            BodyLocation = refData.Where(p => p.DomainCode == "BDLOC").ToList();
            RecordDttm = DateTime.Now;

            FavouritesItemList = new ObservableCollection<FavouriteItemModel>(AllFavouritesList);
            //SaveSuccessDialog("CTR");
        }

        public override void OnLoaded()
        {
            if (!IsVisitMass)
            {
                LoadPatientHistoryProblem();
            }

            AutoSelectPrincipaltype();
        }
 

        private void SearchProblemDoubleClick()
        {
            if (SelectProblemList != null)
            {
                SelectedProblem = SelectProblemList;
            }
        }

        private void SearchDiagnosisEnter(System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                if (!String.IsNullOrEmpty(SearchDiagnosisCriteria) && SearchDiagnosisCriteria.Length >= 3)
                {
                    var SearchProblemListitem = DataService.PatientDiagnosis.SearchProblem(SearchDiagnosisCriteria);
                    SearchProblemList = new ObservableCollection<ProblemModel>(SearchProblemListitem);
                } 
            }

        }

        private void SearchFavoritesEnter(System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                if (!string.IsNullOrEmpty(SearchFavouritesCriteria))
                {
                    FavouritesItemList = new ObservableCollection<FavouriteItemModel>(AllFavouritesList
                        .Where(p => p.ProblemName.ToUpper().Contains(SearchFavouritesCriteria.ToUpper())
                        || p.ProblemCode.ToUpper().StartsWith(SearchFavouritesCriteria.ToUpper())));
                }
                else
                {
                    FavouritesItemList = new ObservableCollection<FavouriteItemModel>(AllFavouritesList);
                }
            }
  
        }

        private void SearchFavouritesDobuleClick()
        {
            if (SelectFavouritesItemList != null)
            {
                ProblemModel prob = new ProblemModel();
                prob.ProblemUID = SelectFavouritesItemList.ProblemUID;
                prob.Code = SelectFavouritesItemList.ProblemCode;
                prob.Name = SelectFavouritesItemList.ProblemName;
                prob.Description = SelectFavouritesItemList.ProblemDescription;
                SelectedProblem = prob;
            }
        }

        private void DiagnosisHistoryDoubleClick()
        {
            if (SelectDiagnosisHistory != null)
            {
                ProblemModel prob = new ProblemModel();
                prob.ProblemUID = SelectDiagnosisHistory.ProblemUID;
                prob.Code = SelectDiagnosisHistory.ProblemCode;
                prob.Name = SelectDiagnosisHistory.ProblemName;
                prob.Description = SelectDiagnosisHistory.ProblemDescription;
                SelectedProblem = prob;
            }
        }

        private void LoadPatientHistoryProblem()
        {
            DiagnosisHistory = DataService.PatientDiagnosis.GetPatientProblemByPatientUID(SelectedPatientVisit.PatientUID);
            DiagnosisHistory = DiagnosisHistory
                .Where(p => p.PatientVisitUID != SelectedPatientVisit.PatientVisitUID)
                .OrderByDescending(p => p.PatientProblemUID).ToList();
            if (SelectIndexDiasHis == 1)
            {
                if(DiagnosisHistory != null && DiagnosisHistory.Count > 0)
                {
                    var lastVisit = DiagnosisHistory.OrderByDescending(p => p.PatientVisitUID).FirstOrDefault().PatientVisitUID;
                    DiagnosisHistory = DiagnosisHistory.Where(p => p.PatientVisitUID == lastVisit).OrderByDescending(p => p.PatientProblemUID).ToList();
                }

            }
        }

        private void AddFavorites()
        {
            try
            {
                if (SelectProblemList != null)
                {

                    if (AllFavouritesList.FirstOrDefault(p => p.ProblemUID == SelectProblemList.ProblemUID) != null)
                    {
                        WarningDialog("มีรายการนี้อยู่ในระบบแล้ว");
                        return;
                    }
                    FavouriteItemModel newFav = new FavouriteItemModel();
                    newFav.ProblemUID = SelectProblemList.ProblemUID;
                    newFav.ProblemCode = SelectProblemList.Code;
                    newFav.ProblemName = SelectProblemList.Name;
                    newFav.ProblemDescription = SelectProblemList.Description;
                    int? favUID = DataService.PatientDiagnosis.AddFavouriteItem(newFav, AppUtil.Current.UserID);
                    if (favUID != null)
                    {
                        newFav.FavouriteItemUID = favUID.Value;
                        SaveSuccessDialog();
                        AllFavouritesList.Add(newFav);
                        FavouritesItemList = new ObservableCollection<FavouriteItemModel>(AllFavouritesList);
                    }
                    else
                    {
                        ErrorDialog("ไม่สามารถเพิ่มข้อมูลได้ ติดต่อ Admin");
                    }


                }
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }


        }
        private void RemoveFavorites()
        {
            try
            {
                if (SelectFavouritesItemList != null)
                {
                    DataService.PatientDiagnosis.DeleteFavouriteItem(SelectFavouritesItemList.FavouriteItemUID, AppUtil.Current.UserID);
                    DeleteSuccessDialog();
                    AllFavouritesList.Remove(SelectFavouritesItemList);
                    FavouritesItemList = new ObservableCollection<FavouriteItemModel>(AllFavouritesList);
                }

            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }

        }


        private bool varidateProblem()
        {
            if (SelectedProblem == null)
            {
                WarningDialog("กรุณาเลือก ICD10");
                return false;
            }
            if (CloseDttm != null)
            {
                if (CloseDttm.Value.Date < DateTime.Today)
                {
                    WarningDialog("วันปิดไม่อนุญาติให้น้อยกว่าวันปัจจุบัน");
                    CloseDttm = DateTime.Today;
                    return false;
                }
            }

            if (SelectDiagnosisType == null)
            {
                WarningDialog("กรุณาเลือกประเภท");
                return false;
            }




            return true;
        }

        private void AddProblem()
        {
            if (SelectedPatientVisit == null && !IsVisitMass)
            {
                WarningDialog("กรุณาเลือกผู้ป่วย");
                return;
            }

            if (!varidateProblem())
            {
                return;
            }

            if ((PatientProblemList.FirstOrDefault(p => p.ProblemUID == SelectedProblem.ProblemUID) != null))
            {
                WarningDialog("มีข้อมูลรหัสโรค "+ SelectedProblem.Code  +" บันทึกอยู่แล้ว");
                return;
            }
            if (PatientProblemList != null)
            {
                if (SelectDiagnosisType != null && SelectDiagnosisType.Key == 445)
                {
                    if ((PatientProblemList.FirstOrDefault(p => p.DIAGTYPUID == 445) != null))
                    {
                        WarningDialog("ไม่อนุญาติบันทึกข้อมูล โรคหลัก ได้มากกว่า 1");
                        return;
                    }
                }
            }

            //if (PatientProblemList == null)
            //{
            //    PatientProblemList = new ObservableCollection<PatientProblemModel>();
            //}

            PatientProblemModel patProblem = new PatientProblemModel();
            patProblem.PatientUID = !IsVisitMass ? SelectedPatientVisit.PatientUID : 0;
            patProblem.PatientVisitUID = !IsVisitMass ? SelectedPatientVisit.PatientVisitUID: 0;
            patProblem.ProblemUID = SelectedProblem.ProblemUID;
            patProblem.ProblemCode = SelectedProblem.Code;
            patProblem.ProblemName = SelectedProblem.Name;
            patProblem.ProblemDescription = SelectedProblem.Description;
            patProblem.DIAGTYPUID = SelectDiagnosisType.Key;
            patProblem.DiagnosisType = SelectDiagnosisType.Display;
            patProblem.IsUnderline = IsUnderline == true ? "Y" : "N";
            patProblem.SEVRTUID = SelectSeverity != null ? SelectSeverity.Key : (int?)null;
            patProblem.CERNTUID = SelectAccuracy != null ? SelectAccuracy.Key : (int?)null;
            patProblem.PBMTYUID = SelectEncounter != null ? SelectEncounter.Key : (int?)null;
            patProblem.BDLOCUID = SelectBodyLocation != null ? SelectBodyLocation.Key : (int?)null;
            patProblem.Severity = SelectSeverity != null ? SelectSeverity.Display : null;
            patProblem.Certanity = SelectAccuracy != null ? SelectAccuracy.Display : null;
            patProblem.ProblemType = SelectEncounter != null ? SelectEncounter.Display : null;
            patProblem.BodyLocation = SelectBodyLocation != null ? SelectBodyLocation.Display : null;
            patProblem.OnsetDttm = OnsetDttm;
            patProblem.ClosureDttm = CloseDttm;
            patProblem.ClosureComments = ClosureComments;
            patProblem.RecordedDttm = RecordDttm;
            patProblem.RecordedName = AppUtil.Current.UserName;
            PatientProblemList.Add(patProblem);
            ClearControl();


        }

        private void EditProblem()
        {
            if (SelectedPatientVisit == null && !IsVisitMass)
            {
                WarningDialog("กรุณาเลือกผู้ป่วย");
                return;
            }
            if (!varidateProblem())
            {
                return;
            }

            if ((PatientProblemList.Count(p => p.ProblemUID == SelectedProblem.ProblemUID && !p.Equals(SelectPatientProblemList)) >= 1))
            {
                WarningDialog("มีข้อมูลรหัสโรค " + SelectedProblem.Code + " บันทึกอยู่แล้ว");
                return;
            }
            if (SelectPatientProblemList != null && SelectPatientProblemList.DIAGTYPUID != 445)
            {
                if (SelectDiagnosisType != null && SelectDiagnosisType.Key == 445)
                {
                    if ((PatientProblemList.FirstOrDefault(p => p.DIAGTYPUID == 445) != null))
                    {
                        WarningDialog("ไม่อนุญาติบันทึกข้อมูล โรคหลัก ได้มากกว่า 1");
                        return;
                    }
                }

            }
            if (SelectPatientProblemList != null)
            {
                if (!varidateProblem())
                    return;

                SelectPatientProblemList.ProblemUID = SelectedProblem.ProblemUID;
                SelectPatientProblemList.ProblemCode = SelectedProblem.Code;
                SelectPatientProblemList.ProblemName = SelectedProblem.Name;
                SelectPatientProblemList.ProblemDescription = SelectedProblem.Description;
                SelectPatientProblemList.DIAGTYPUID = SelectDiagnosisType.Key;
                SelectPatientProblemList.DiagnosisType = SelectDiagnosisType.Display;
                SelectPatientProblemList.IsUnderline = IsUnderline == true ? "Y" : "N";
                SelectPatientProblemList.SEVRTUID = SelectSeverity != null ? SelectSeverity.Key : (int?)null;
                SelectPatientProblemList.CERNTUID = SelectAccuracy != null ? SelectAccuracy.Key : (int?)null;
                SelectPatientProblemList.PBMTYUID = SelectEncounter != null ? SelectEncounter.Key : (int?)null;
                SelectPatientProblemList.BDLOCUID = SelectBodyLocation != null ? SelectBodyLocation.Key : (int?)null;
                SelectPatientProblemList.Severity = SelectSeverity != null ? SelectSeverity.Display : null;
                SelectPatientProblemList.Certanity = SelectAccuracy != null ? SelectAccuracy.Display : null;
                SelectPatientProblemList.ProblemType = SelectEncounter != null ? SelectEncounter.Display : null;
                SelectPatientProblemList.BodyLocation = SelectBodyLocation != null ? SelectBodyLocation.Display : null;
                SelectPatientProblemList.OnsetDttm = OnsetDttm;
                SelectPatientProblemList.ClosureDttm = CloseDttm;
                SelectPatientProblemList.ClosureComments = ClosureComments;
                SelectPatientProblemList.RecordedDttm = RecordDttm;
                SelectPatientProblemList.RecordedName = AppUtil.Current.UserName;
                OnUpdateEvent();
                ClearControl();
            }

        }

        private void RemoveProblem()
        {
            if (SelectPatientProblemList != null)
            {
                MessageBoxResult result = DeleteDialog();
                if (result == MessageBoxResult.Yes)
                {
                    PatientProblemList.Remove(SelectPatientProblemList);
                    ClearControl();
                }
            }


        }
        public void SavePatientProblem()
        {
            try
            {
                if (!IsVisitMass)
                {
                    if (SelectedPatientVisit != null)
                    {
                        if (PatientProblemList == null || PatientProblemList.Count <= 0)
                        {
                            MessageBoxResult resultDaig = QuestionDialog("ไม่มีข้อมูลการวินิจฉัย คุณต้องการบันทึก หรื่อไม่ ?");
                            if (resultDaig != MessageBoxResult.Yes)
                            {
                                return;
                            }
                        }
                        AssignPropertiesModel();
                        DataService.PatientDiagnosis.ManagePatientProblem(model, SelectedPatientVisit.PatientVisitUID, AppUtil.Current.UserID);
                        SaveSuccessDialog();
                        CloseViewDialog(ActionDialog.Save);
                    }
                }
                else
                {
                    if (PatientProblemList == null || PatientProblemList.Count <= 0)
                    {
                        MessageBoxResult resultDaig = QuestionDialog("ไม่มีข้อมูลการวินิจฉัย คุณต้องการบันทึก หรื่อไม่ ?");
                        if (resultDaig != MessageBoxResult.Yes)
                        {
                            return;
                        }
                    }
                    AssignPropertiesModel();
                    CloseViewDialog(ActionDialog.Save);
                }

            }
            catch (Exception ex)
            {

                ErrorDialog(ex.Message);
            }

        }

        public void Cancel()
        {
            CloseViewDialog(ActionDialog.Cancel);
        }
        public void ClearControl()
        {
            SelectDiagnosisType = null;
            OnsetDttm = null;
            SelectSeverity = null;
            SelectAccuracy = null;
            SelectEncounter = null;
            SelectBodyLocation = null;
            RecordDttm = DateTime.Now;
            CloseDttm = null;
            ClosureComments = string.Empty;
            IsUnderline = false;
            SelectedProblem = null;
            SelectPatientProblemList = null;
            AutoSelectPrincipaltype();
        }

        void AutoSelectPrincipaltype()
        {
            int countPrincipal = 0;
            if (PatientProblemList != null)
            {
                countPrincipal = PatientProblemList.Count(p => p.DIAGTYPUID == 445);
            }
            if (countPrincipal == 0)
            {
                SelectDiagnosisType = DiagnosisType.FirstOrDefault(p => p.Key == 445);
            }
            else
            {
                SelectDiagnosisType = DiagnosisType.FirstOrDefault(p => p.Key != 445);
            }
        }

        public void AssignModel(List<PatientProblemModel> model)
        {
            this.model = model;
            AssignModelToProperties();
        }

        public void AssignPropertiesModel()
        {
            model = new List<PatientProblemModel>();
            model.AddRange(PatientProblemList);
        }
        public void AssignModelToProperties()
        {
            foreach (var item in model)
            {
                PatientProblemList.Add(item);
            }
        }

        public void AssignPatientVisit(PatientVisitModel patVisitData)
        {
            SelectedPatientVisit = patVisitData;
        }

        #endregion
    }
}
