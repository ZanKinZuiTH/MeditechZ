using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MediTech.DataService;
using MediTech.Model;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MediTech.ViewModels
{
    public class ManageRoleViewModel : MediTechViewModelBase
    {
        #region Properties

        private int? _RoleUID;

        public int? RoleUID
        {
            get { return _RoleUID; }
            set { _RoleUID = value; }
        }


        private string _RoleName;

        public string RoleName
        {
            get { return _RoleName; }
            set { Set(ref _RoleName, value); }
        }


        private string _Description;

        public string Description
        {
            get { return _Description; }
            set { Set(ref _Description, value); }
        }



        private ObservableCollection<PageViewModuleModel> _PageViewModuleSource;

        public ObservableCollection<PageViewModuleModel> PageViewModuleSource
        {
            get { return _PageViewModuleSource; }
            set { Set(ref _PageViewModuleSource, value); }
        }


        private object _SelectPageView;

        public object SelectPageView
        {
            get { return _SelectPageView; }
            set
            {
                Set(ref _SelectPageView, value);
                if (_SelectPageView != null)
                {
                    if (SelectPageView is PageViewModel)
                    {
                        if ((SelectPageView as PageViewModel).PageViewModuleUID == 11) // Module Report
                        {
                            VisitbilityReport = Visibility.Visible;
                            ReportTemplateList = ReportTemplateAll.Where(p => p.ReportGroup == (SelectPageView as PageViewModel).ClassName).ToList();
                        }
                        else
                        {
                            VisitbilityReport = Visibility.Collapsed;
                        }
                    }
                    else if(SelectPageView is PageViewModuleModel)
                    {
                        VisitbilityReport = Visibility.Collapsed;
                    }
                }
            }
        }

        private Visibility _VisitbilityReport = Visibility.Collapsed;

        public Visibility VisitbilityReport
        {
            get { return _VisitbilityReport; }
            set { Set(ref _VisitbilityReport, value); }
        }

        private List<RoleReportPermissionModel> _ReportTemplateList;

        public List<RoleReportPermissionModel> ReportTemplateList
        {
            get { return _ReportTemplateList; }
            set { Set(ref _ReportTemplateList, value); }
        }

        private List<RoleReportPermissionModel> _ReportTemplateAll;

        public List<RoleReportPermissionModel> ReportTemplateAll
        {
            get { return _ReportTemplateAll; }
            set { Set(ref _ReportTemplateAll, value); }
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

        #endregion

        #region Variable



        #endregion

        #region Method

        public ManageRoleViewModel()
        {
            PageViewModuleSource = new ObservableCollection<PageViewModuleModel>(DataService.MainManage.GetPageViewModule().OrderBy(p => p.DisplayOrder).ToList());
            List<PageViewModel> pageView = DataService.RoleManage.GetPageViewByPermission("Read");
            foreach (var item in PageViewModuleSource)
            {
                item.PageViews = new ObservableCollection<PageViewModel>(pageView.Where(p => p.PageViewModuleUID == item.PageViewModuleUID).OrderBy(p => p.DisplayOrder).ToList());
                item.IsChecked = false;
                foreach (var item2 in item.PageViews)
                {
                    item2.IsChecked = false;
                }
            }
            PageViewModuleSource = new ObservableCollection<PageViewModuleModel>(PageViewModuleSource.Where(p => p.PageViews.Count > 0));
            ReportTemplateAll = DataService.RoleManage.GetReportTemplateByPermission("read");
        }

        private void Save()
        {
            try
            {
                if (string.IsNullOrEmpty(RoleName))
                {
                    WarningDialog("กรุณาระบุสิทธิ์");
                    return;
                }
                if (string.IsNullOrEmpty(Description))
                {
                    WarningDialog("กรุณาระบุเนื้อหา");
                    return;
                }
                RoleModel roleModel = new RoleModel();
                roleModel.RoleUID = RoleUID ?? 0;
                roleModel.Name = RoleName;
                roleModel.Description = Description;
                roleModel.RoleView = new List<RoleViewPermissionModel>();
                roleModel.RoleReport = new List<RoleReportPermissionModel>();
                foreach (var item in PageViewModuleSource)
                {
                    foreach (var item2 in item.PageViews)
                    {
                        if (item2.IsChecked == true || (item2.PageViewModuleUID == 11 && item2.IsChecked != false))
                        {
                            RoleViewPermissionModel roleViewPermiss = new RoleViewPermissionModel();
                            roleViewPermiss.RoleUID = RoleUID ?? 0;
                            roleViewPermiss.PageViewPermissionUID = item2.PageViewPermissionUID ?? 0;
                            roleModel.RoleView.Add(roleViewPermiss);
                        }
                    }
                }
                foreach (var rpt in ReportTemplateAll)
                {
                    if (rpt.IsChecked == true)
                    {
                        RoleReportPermissionModel roleReportPermiss = new RoleReportPermissionModel();
                        roleReportPermiss.RoleUID = RoleUID ?? 0;
                        roleReportPermiss.ReportPermissionUID = rpt.ReportPermissionUID;
                        roleModel.RoleReport.Add(roleReportPermiss);
                    }

                }
                DataService.RoleManage.ManageRole(roleModel, AppUtil.Current.UserID);
                SaveSuccessDialog();
                ListRoles pageRoles = new ListRoles();
                ChangeViewPermission(pageRoles);
            }
            catch (Exception er)
            {
                ErrorDialog(er.Message);
            }
        }

        private void Cancel()
        {
            ListRoles pageView = new ListRoles();
            ChangeViewPermission(pageView);
        }

        public void AssingModel(int roleUID)
        {
            var role = DataService.RoleManage.GetRoleByUID(roleUID);
            if (role != null)
            {
                RoleUID = role.RoleUID;
                RoleName = role.Name;
                Description = role.Description;
                List<SecurityPermissionModel> securityPermiss = DataService.RoleManage.GetPageViewPermission(roleUID);
                List<RoleReportPermissionModel> rolePermiss = DataService.RoleManage.GetReportTemplatePermission(roleUID);
                (this.View as ManageRole).suprassEventPage = true;
                (this.View as ManageRole).suprassEventRPT = true;

                if (rolePermiss != null)
                {
                    foreach (var item in ReportTemplateAll)
                    {
                        if (rolePermiss.Count(p => p.ReportsUID == item.ReportsUID) > 0)
                        {
                            item.IsChecked = true;
                        }
                        else
                        {
                            item.IsChecked = false;
                        }
                    }
                    ReportTemplateAll = ReportTemplateAll;
                }

                if (securityPermiss != null)
                {
                    foreach (var item in PageViewModuleSource)
                    {
                        foreach (var item2 in item.PageViews)
                        {
                            if (securityPermiss.Count(p => p.PageViewUID == item2.PageViewUID) > 0)
                            {
                                item2.IsChecked = true;
                            }
                            else
                            {
                                item2.IsChecked = false;
                            }

                            if (item2.PageViewModuleUID == 11)
                            {
                                int IsRPTCheck = ReportTemplateAll.Count(p => p.IsChecked == true && p.ReportGroup == item2.ClassName);
                                int GroupRPT = ReportTemplateAll.Count(p => p.ReportGroup == item2.ClassName);
                                if (IsRPTCheck == GroupRPT)
                                {
                                    item2.IsChecked = true;
                                }
                                else if (IsRPTCheck >= 1)
                                {
                                    item2.IsChecked = null;
                                }
                                else
                                {
                                    item2.IsChecked = false;
                                }
                            }
                        }
                        int IsPageCheck = item.PageViews.Count(p => p.IsChecked == true);
                        if (item.PageViews.Count == IsPageCheck)
                        {
                            item.IsChecked = true;
                        }
                        else if (IsPageCheck >= 1)
                        {
                            item.IsChecked = null;
                        }
                        else
                        {
                            item.IsChecked = false;
                        }
                    }
                    PageViewModuleSource = new ObservableCollection<PageViewModuleModel>(PageViewModuleSource);
                }

                (this.View as ManageRole).suprassEventPage = false;
                (this.View as ManageRole).suprassEventRPT = false;
            }
        }

        #endregion
    }
}
