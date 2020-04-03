using DevExpress.Xpf.Editors;
using MediTech.Model;
using MediTech.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MediTech.Views
{
    /// <summary>
    /// Interaction logic for RoleManage.xaml
    /// </summary>
    public partial class ManageRole : UserControl
    {
        public bool suprassEventPage = false;
        public bool suprassEventRPT = false;
        public ManageRole()
        {
            InitializeComponent();
            this.treeView.ExpandAllNodes();
            this.treeRPTView.NodeChanged += TreeRPTView_NodeChanged;
            this.treeView.NodeChanged += TreeView_NodeChanged;
        }

        private void TreeView_NodeChanged(object sender, DevExpress.Xpf.Grid.TreeList.TreeListNodeChangedEventArgs e)
        {

            if (e.ChangeType == DevExpress.Data.TreeList.NodeChangeType.CheckBox)
            {
                if (!suprassEventPage)
                {
                    treeControl.SelectedItem = treeControl.GetRow(e.Node.RowHandle);
                    suprassEventRPT = true;
                    if (treeControl.SelectedItem is PageViewModel)
                    {
                        var selectPageview = (PageViewModel)treeControl.SelectedItem;
                        if (selectPageview.PageViewModuleUID == 11)
                        {
                            var dataRPTGroup = (List<RoleReportPermissionModel>)treeRPTControl.ItemsSource;
                            foreach (var item in dataRPTGroup)
                            {
                                item.IsChecked = e.Node.IsChecked ?? false;
                            }
                        }
                        treeRPTControl.RefreshData();
                    }
                    if (treeControl.SelectedItem is PageViewModuleModel)
                    {
                        var selectModule = (PageViewModuleModel)treeControl.SelectedItem;
                        if (selectModule.PageViewModuleUID == 11)
                        {
                            if (e.Node.IsChecked ?? false)
                            {
                                (this.DataContext as ManageRoleViewModel).ReportTemplateAll.ForEach(p => p.IsChecked = true);
                            }
                            else
                            {
                                (this.DataContext as ManageRoleViewModel).ReportTemplateAll.ForEach(p => p.IsChecked = false);
                            }
                            treeRPTControl.RefreshData();
                        }
                    }
                    suprassEventRPT = false;
                }

            }
        }

        private void TreeRPTView_NodeChanged(object sender, DevExpress.Xpf.Grid.TreeList.TreeListNodeChangedEventArgs e)
        {
            if (e.ChangeType == DevExpress.Data.TreeList.NodeChangeType.CheckBox)
            {
                if (!suprassEventRPT)
                {
                    suprassEventPage = true;
                    var dataReport = (List<RoleReportPermissionModel>)treeRPTControl.ItemsSource;
                    var selectPageview = ((PageViewModel)treeControl.SelectedItem);
                    int IsRPTCheck = dataReport.Count(p => p.IsChecked == true);
                    int GroupRPT = dataReport.Count();
                    if (IsRPTCheck == GroupRPT)
                    {
                        selectPageview.IsChecked = true;
                    }
                    else if (IsRPTCheck >= 1)
                    {
                        selectPageview.IsChecked = null;
                    }
                    else
                    {
                        selectPageview.IsChecked = false;
                    }
                    treeControl.RefreshData();
                    suprassEventPage = false;
                }


            }
        }


        public void AssignModel(int roleUID)
        {
            if (this.DataContext != null)
            {
                if (this.DataContext is ManageRoleViewModel)
                {
                    (this.DataContext as ManageRoleViewModel).AssingModel(roleUID);
                }
            }
        }

        //private void chkPageView_EditValueChanged(object sender, EditValueChangedEventArgs e)
        //{
        //var data = (ObservableCollection<PageViewModuleModel>)treeControl.ItemsSource;
        //foreach (var item in data)
        //{
        //    int IsCheckCount = item.PageViews.Count(p => p.IsChecked == true);
        //    if (item.PageViews.Count == IsCheckCount)
        //    {
        //        item.IsChecked = true;
        //    }
        //    else if (IsCheckCount >= 1)
        //    {
        //        item.IsChecked = null;
        //    }
        //    else
        //    {
        //        item.IsChecked = false;
        //    }
        //}
        ////treeview.Items.Refresh();
        //treeControl.RefreshData();
        //}

        //private void chkPageViewModule_EditValueChanged(object sender, EditValueChangedEventArgs e)
        //{
        //var data = (ObservableCollection<PageViewModuleModel>)treeControl.ItemsSource;
        //CheckEdit chk = (CheckEdit)sender;
        //var selectModule = data.FirstOrDefault(p => p.PageViewModuleUID == (int)chk.Tag);
        //foreach (var item2 in selectModule.PageViews)
        //{
        //    item2.IsChecked = (bool)e.NewValue;
        //}
        //treeControl.ItemsSource = (ObservableCollection<PageViewModuleModel>)data;
        ////treeview.Items.Refresh();
        //treeControl.RefreshData();
        //}

    }
}
