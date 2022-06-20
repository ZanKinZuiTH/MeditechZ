using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MediTech.DataService;
using MediTech.Model;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace MediTech.ViewModels
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : MediTechViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        /// 

        #region Properties

        private string _ApplicationVersion;

        public string ApplicationVersion
        {
            get { return _ApplicationVersion; }
            set { Set(ref _ApplicationVersion , value); }
        }

        private string _ApplicationStatus;

        public string ApplicationStatus
        {
            get { return _ApplicationStatus; }
            set { Set(ref _ApplicationStatus, value); }
        }

        private string _LoginUser;

        public string LoginUser
        {
            get { return _LoginUser; }
            set { Set(ref _LoginUser, value); }
        }

        private string _LoginDate;

        public string LoginDate
        {
            get { return _LoginDate; }
            set { Set(ref _LoginDate, value); }
        }

        private String _UserName;

        public String UserName
        {
            get { return _UserName; }
            set { Set(ref _UserName, value); }
        }

        private string _RoleName;

        public string RoleName
        {
            get { return _RoleName; }
            set { Set(ref _RoleName, value); }
        }


        private String _OrganisationName;

        public String OrganisationName
        {
            get { return _OrganisationName; }
            set { Set(ref _OrganisationName, value); }
        }


        private ObservableCollection<PageViewModuleModel>  _PageViewModule;

        public ObservableCollection<PageViewModuleModel>  PageViewModule
        {
            get { return _PageViewModule; }
            set { Set(ref _PageViewModule, value); }
        }

        private bool _ClicktPageEvent = true;

        public bool ClicktPageEvent
        {
            get { return _ClicktPageEvent; }
            set { _ClicktPageEvent = value; }
        }


        private PageViewModel _SelectedPage;

        public PageViewModel SelectedPage
        {
            get { return _SelectedPage; }
            set
            {
                _SelectedPage = value;
                RaisePropertyChanged("SelectedPage");
                if (_SelectedPage != null)
                {
                    if (ClicktPageEvent)
                    {
                        OpenPage(_SelectedPage);
                    }
                    ClicktPageEvent = true;
                }
            }
        }



        #region PatientSearch

        private string _SearchPatientCriteria;

        public string SearchPatientCriteria
        {
            get { return _SearchPatientCriteria; }
            set
            {
                Set(ref _SearchPatientCriteria, value);
                PatientsSearchSource = null;
            }
        }


        private List<PatientInformationModel> _PatientsSearchSource;

        public List<PatientInformationModel> PatientsSearchSource
        {
            get { return _PatientsSearchSource; }
            set { Set(ref _PatientsSearchSource, value); }
        }

        private PatientInformationModel _SelectedPateintSearch;

        public PatientInformationModel SelectedPateintSearch
        {
            get { return _SelectedPateintSearch; }
            set
            {
                _SelectedPateintSearch = value;
                if (_SelectedPateintSearch != null)
                {
                    EMRView pageview = new EMRView();
                    PatientVisitModel visitModel = new PatientVisitModel();
                    visitModel.PatientUID = SelectedPateintSearch.PatientUID;
                    (pageview.DataContext as EMRViewViewModel).AssingPatientVisit(visitModel);
                    EMRViewViewModel result = (EMRViewViewModel)LaunchViewDialog(pageview, "EMRVE", false, true);

                    SearchPatientCriteria = "";
                }
            }
        }

        #endregion
        #endregion


        #region Command



        private RelayCommand _ChangePasswordCommand;
        public RelayCommand ChangePasswordCommand
        {
            get { return _ChangePasswordCommand ?? (_ChangePasswordCommand = new RelayCommand(ChangePassword)); }
        }

        private RelayCommand _PatientSearchCommand;

        public RelayCommand PatientSearchCommand
        {
            get { return _PatientSearchCommand ?? (_PatientSearchCommand = new RelayCommand(PatientSearch)); }
        }

        #endregion

        public int PageModules { get; set; }
        public MainViewModel()
        {
            if (IsInDesignMode)
            {
                // Code runs in Blend --> create design time data.
            }
            else
            {
                // Code runs "for real"
                //CreateMenuByRole();
                //AssingStatuBar();
            }
        }

        public override void OnLoaded()
        {
            CreateMenuByRole();
            AssingUserInformation();
            AssingStatuBar();
        }

        private void AssingUserInformation()
        {

            UserName = AppUtil.Current.UserName + " @ " + AppUtil.Current.LocationName;
            RoleName = AppUtil.Current.RoleName;
            OrganisationName = AppUtil.Current.OwnerOrganisationName;
        }
        private void AssingStatuBar()
        {
            LoginUser = AppUtil.Current.LoginName;
            LoginDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            ApplicationVersion = AppUtil.Current.ApplicationVersion;
            ApplicationStatus = AppUtil.Current.ApplicationStaus;
        }
        private void CreateMenuByRole()
        {
            if (AppUtil.Current.RoleUID != 0)
            {

                var pageViewModule = DataService.MainManage.GetPageViewModule().OrderBy(p => p.DisplayOrder).ToList();
                List<PageViewModel> pageView = DataService.MainManage.GetPageView(AppUtil.Current.RoleUID,"win");

                if (pageView != null)
                {
                    AppUtil.Current.PageViewPermission = pageView;
                }

                foreach (var item in pageViewModule)
                {
                    item.PageViews = new ObservableCollection<PageViewModel>(pageView.Where(p => p.PageViewModuleUID == item.PageViewModuleUID && p.Type.ToUpper() == "MENU").OrderBy(p => p.DisplayOrder));
                }

                PageViewModule = new ObservableCollection<PageViewModuleModel>((pageViewModule.Where(p => p.PageViews.Count > 0)));
            }
            else
            {
                MessageBox.Show("�������ö���ҧ������ �ô�Դ��� admin");
            }
        }

        private void ChangePassword()
        {
            MediTech.Views.ChangePassword changePassword = new Views.ChangePassword();
            ChangeView(changePassword,"Change Password");
        }

        public void PatientSearch()
        {
            string patientID = string.Empty;
            string firstName = string.Empty;
            string lastName = string.Empty;
            if (SearchPatientCriteria.Length >= 3)
            {
                string[] patientName = SearchPatientCriteria.Split(' ');
                if (patientName.Length >= 2)
                {
                    firstName = patientName[0];
                    lastName = patientName[1];
                }
                else
                {
                    int num = 0;
                    foreach (var ch in SearchPatientCriteria)
                    {
                        if (ShareLibrary.CheckValidate.IsNumber(ch.ToString()))
                        {
                            num++;
                        }
                    }
                    if (num >= 5)
                    {
                        patientID = SearchPatientCriteria;
                    }
                    else if (num <= 2)
                    {
                        firstName = SearchPatientCriteria;
                        lastName = "empty";
                    }

                }
                List<PatientInformationModel> searchResult = DataService.PatientIdentity.SearchPatient(patientID, firstName, "", lastName, "", null, null, "", null, "");
                PatientsSearchSource = searchResult;
            }
            else
            {
                PatientsSearchSource = null;
            }

        }

    }
}