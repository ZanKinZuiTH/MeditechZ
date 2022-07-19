using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Models;
using MediTech.DataService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.Xpf.Core;
using System.Windows;

namespace MediTech.ViewModels
{
    public class MediTechViewModelBase : ViewModelBase
    {
        #region StaticVariable
        public static System.Windows.Window MainWindow { get; set; }

        #endregion

        #region Events

        public event EventHandler UpdateEvent;

        protected void OnUpdateEvent()
        {
            if (UpdateEvent != null) UpdateEvent(this, EventArgs.Empty);
        }
        #endregion

        #region Event On Load
        private RelayCommand _loadedCommand;
        public RelayCommand LoadedCommand
        {
            get
            {
                return _loadedCommand ?? (_loadedCommand = new RelayCommand(() =>
                {
                    OnLoaded();
                }));
            }
        }

        public virtual void OnLoaded() { }

        public virtual void UnLoaded()
        {
        }

        #endregion

        #region DataService


        private MediTechDataService _DataService;

        public MediTechDataService DataService
        {
            get { return _DataService ?? (_DataService = new MediTechDataService()); }
        }

        public List<HealthOrganisationModel> GetHealthOrganisation()
        {
            var Owngra = DataService.MasterData.GetHealthOrganisationActive();
            if (Owngra != null)
            {
                Owngra = Owngra.OrderBy(p => p.HealthOrganisationUID).ToList();
            }

            return Owngra;
        }

        public List<HealthOrganisationModel> GetHealthOrganisationRole()
        {
            var Owngra = DataService.MasterData.GetHealthOrganisationActive();
            if (Owngra != null)
            {
                Owngra = (from j in Owngra
                          join i in AppUtil.Current.CareproviderOrganisations on j.HealthOrganisationUID equals i.HealthOrganisationUID
                          select j).ToList();
            }

            return Owngra;
        }

        public List<HealthOrganisationModel> GetHealthOrganisationMedical()
        {
            var Owngra = DataService.MasterData.GetHealthOrganisationActive();
            if (Owngra != null)
            {
                Owngra = Owngra.Where(p => p.HOTYPUID != 2973).OrderBy(p => p.HealthOrganisationUID).ToList();
            }

            return Owngra;
        }

        public List<HealthOrganisationModel> GetHealthOrganisationRoleMedical()
        {
            var Owngra = DataService.MasterData.GetHealthOrganisationActive();
            if (Owngra != null)
            {
                Owngra = Owngra.Where(p => p.HOTYPUID != 2973).OrderBy(p => p.HealthOrganisationUID).ToList();
                Owngra = (from j in Owngra
                          join i in AppUtil.Current.CareproviderOrganisations on j.HealthOrganisationUID equals i.HealthOrganisationUID
                          select j).ToList();
            }

            return Owngra;
        }


        public List<LocationModel> GetLocatioinRole(int organisationUID)
        {
            var locations = DataService.MasterData.GetLocationByOrganisationUID(organisationUID);
            if (locations != null)
            {
                locations = (from j in locations
                             join i in AppUtil.Current.CareproviderLocations on j.LocationUID equals i.LocationUID
                             select j).ToList();
            }

            return locations;
        }

        public List<HealthOrganisationModel> GetHealthOrganisationIsStock()
        {
            var Owngra = DataService.MasterData.GetHealthOrganisationActive();
            if (Owngra != null)
            {
                Owngra = Owngra.Where(p => p.IsStock == "Y").OrderBy(p => p.HealthOrganisationUID).ToList();
            }

            return Owngra;
        }

        public List<HealthOrganisationModel> GetHealthOrganisationIsRoleStock()
        {
            var Owngra = DataService.MasterData.GetHealthOrganisationActive();
            if (Owngra != null)
            {
                Owngra = Owngra.Where(p => p.IsStock == "Y").OrderBy(p => p.HealthOrganisationUID).ToList();
                Owngra = (from j in Owngra
                          join i in AppUtil.Current.CareproviderOrganisations on j.HealthOrganisationUID equals i.HealthOrganisationUID
                          select j).ToList();
            }

            return Owngra;
        }

        #endregion


        private ActionDialog _ResultDialog = ActionDialog.Cancel;

        public ActionDialog ResultDialog
        {
            get { return _ResultDialog; }
            set { _ResultDialog = value; }
        }

        #region ViewManagement


        public object BackwardView { get; set; }
        public object View { get; set; }

        public void OpenPage(PageViewModel pageView)
        {
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<PageViewModel>(pageView);
        }

        public object LaunchViewDialogNonPermiss(object pageView, bool IsSizeToContent, bool IsMaximized = false)
        {
            try
            {
                System.Windows.Controls.UserControl usercontrol = (System.Windows.Controls.UserControl)pageView;
                string className = usercontrol.GetType().Name;
                string Namespace = usercontrol.GetType().Namespace;

                usercontrol.Loaded += usercontrol_Loaded;

                System.Windows.Window window = new System.Windows.Window();
                window.Closed += window_Closed;

                (usercontrol.DataContext as MediTechViewModelBase).View = pageView;

                window.Content = usercontrol;
                window.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                if (IsSizeToContent == true)
                    window.SizeToContent = System.Windows.SizeToContent.WidthAndHeight;

                if (IsMaximized)
                    window.WindowState = System.Windows.WindowState.Maximized;

                window.ShowInTaskbar = false;
                window.Owner = MainWindow;
                window.ShowDialog();
                return usercontrol.DataContext;
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
                return null;
            }
            finally
            {
                MediTech.Helpers.MemoryManagement.FlushMemory();
            }
        }

        public object LaunchViewDialog(object pageView, string viewCode, bool IsSizeToContent, bool IsMaximized = false)
        {
            try
            {
                System.Windows.Controls.UserControl usercontrol = (System.Windows.Controls.UserControl)pageView;
                string className = usercontrol.GetType().Name;
                string Namespace = usercontrol.GetType().Namespace;
                var permission = AppUtil.Current.PageViewPermission.FirstOrDefault(p =>
                    p.NamespaceName == Namespace
                    && p.ClassName == className
                    && p.ViewCode == viewCode);
                if (permission == null)
                {
                    WarningDialog("คุณไม่มีสิทธิ์กระทำการใดๆ นี้ ถ้าจำเป็นต้องการใช้งานส่วนนี้โปรดติดต่อ Admin");
                    return null;
                }

                usercontrol.Loaded += usercontrol_Loaded;

                System.Windows.Window window = new System.Windows.Window();
                window.Closed += window_Closed;

                (usercontrol.DataContext as MediTechViewModelBase).View = pageView;

                window.Content = usercontrol;
                window.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                if (IsSizeToContent == true)
                    window.SizeToContent = System.Windows.SizeToContent.WidthAndHeight;

                if (IsMaximized)
                    window.WindowState = System.Windows.WindowState.Maximized;

                window.ShowInTaskbar = false;
                window.Title = permission.LocalName;
                window.Owner = MainWindow;
                window.ShowDialog();

                return usercontrol.DataContext;
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
                return null;
            }
            finally
            {
                MediTech.Helpers.MemoryManagement.FlushMemory();
            }
        }

        public object LaunchViewShow(object pageView, string viewCode, bool IsSizeToContent, bool IsMaximized = false)
        {
            try
            {
                System.Windows.Controls.UserControl usercontrol = (System.Windows.Controls.UserControl)pageView;
                string className = usercontrol.GetType().Name;
                string Namespace = usercontrol.GetType().Namespace;
                var permission = AppUtil.Current.PageViewPermission.FirstOrDefault(p =>
                    p.NamespaceName == Namespace
                    && p.ClassName == className
                    && p.ViewCode == viewCode);
                if (permission == null)
                {
                    WarningDialog("คุณไม่มีสิทธิ์กระทำการใดๆ นี้ ถ้าจำเป็นต้องการใช้งานส่วนนี้โปรดติดต่อ Admin");
                    return null;
                }

                usercontrol.Loaded += usercontrol_Loaded;

                System.Windows.Window window = new System.Windows.Window();
                window.Closed += window_Closed;

                (usercontrol.DataContext as MediTechViewModelBase).View = pageView;

                window.Content = usercontrol;
                window.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                if (IsSizeToContent == true)
                    window.SizeToContent = System.Windows.SizeToContent.WidthAndHeight;

                if (IsMaximized)
                    window.WindowState = System.Windows.WindowState.Maximized;

                window.ShowInTaskbar = false;
                window.Title = permission.LocalName;
                window.Owner = MainWindow;
                window.Show();

                return usercontrol.DataContext;
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
                return null;
            }
            finally
            {
                MediTech.Helpers.MemoryManagement.FlushMemory();
            }
        }

        public object LaunchViewShow(object pageView, System.Windows.Window owner, string viewCode, bool IsSizeToContent, bool IsMaximized = false)
        {
            try
            {
                System.Windows.Controls.UserControl usercontrol = (System.Windows.Controls.UserControl)pageView;
                string className = usercontrol.GetType().Name;
                string Namespace = usercontrol.GetType().Namespace;
                var permission = AppUtil.Current.PageViewPermission.FirstOrDefault(p =>
                    p.NamespaceName == Namespace
                    && p.ClassName == className
                    && p.ViewCode == viewCode);
                if (permission == null)
                {
                    WarningDialog("คุณไม่มีสิทธิ์กระทำการใดๆ นี้ ถ้าจำเป็นต้องการใช้งานส่วนนี้โปรดติดต่อ Admin");
                    return null;
                }

                usercontrol.Loaded += usercontrol_Loaded;

                System.Windows.Window window = new System.Windows.Window();
                window.Closed += window_Closed;

                (usercontrol.DataContext as MediTechViewModelBase).View = pageView;

                window.Content = usercontrol;
                window.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                if (IsSizeToContent == true)
                    window.SizeToContent = System.Windows.SizeToContent.WidthAndHeight;

                if (IsMaximized)
                    window.WindowState = System.Windows.WindowState.Maximized;

                window.ShowInTaskbar = false;
                window.Title = permission.LocalName;
                window.Owner = owner;
                window.Show();

                return usercontrol.DataContext;
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
                return null;
            }
            finally
            {
                MediTech.Helpers.MemoryManagement.FlushMemory();
            }
        }
        public object ShowModalDialogUsingViewModel(object pageView, MediTechViewModelBase viewModel, bool IsSizeToContent, bool IsMaximized = false)
        {
            try
            {
                System.Windows.Controls.UserControl usercontrol = (System.Windows.Controls.UserControl)pageView;

                usercontrol.Loaded += usercontrol_Loaded;

                System.Windows.Window window = new System.Windows.Window();
                window.Closed += window_Closed;

                usercontrol.DataContext = viewModel;

                (usercontrol.DataContext as MediTechViewModelBase).View = pageView; ;

                window.Content = usercontrol;
                window.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                if (IsSizeToContent == true)
                    window.SizeToContent = System.Windows.SizeToContent.WidthAndHeight;

                if (IsMaximized)
                    window.WindowState = System.Windows.WindowState.Maximized;

                window.ShowInTaskbar = false;
                window.Owner = MainWindow;
                window.ShowDialog();

                return usercontrol.DataContext;
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
                return null;
            }
            finally
            {
                MediTech.Helpers.MemoryManagement.FlushMemory();
            }
        }

        void usercontrol_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            System.Windows.Controls.UserControl usercontrol = (System.Windows.Controls.UserControl)sender;
            if (usercontrol.DataContext is MediTechViewModelBase)
            {
                ((MediTechViewModelBase)usercontrol.DataContext).OnLoaded();
            }
        }

        void window_Closed(object sender, EventArgs e)
        {
            ViewModelLocator.Cleanup();
        }

        public void ChangeView(object pageView, string title, object backwardView = null)
        {
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<LaunchPageMassage>(new LaunchPageMassage
            {
                PageObject = pageView,
                BackwardView = backwardView
                ,
                TitleName = title
            });
        }

        public void ChangeViewPermission(object pageView, object backwardView = null)
        {
            if (pageView != null)
            {
                System.Windows.Controls.UserControl usercontrol = (System.Windows.Controls.UserControl)pageView;
                string className = usercontrol.GetType().Name;
                string Namespace = usercontrol.GetType().Namespace;
                var permission = AppUtil.Current.PageViewPermission.FirstOrDefault(p => p.NamespaceName == Namespace && p.ClassName == className);
                if (permission == null)
                {
                    WarningDialog("คุณไม่มีสิทธิ์กระทำการใดๆ นี้ ถ้าจำเป็นต้องการใช้งานส่วนนี้โปรดติดต่อ Admin");
                    return;
                }

                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<LaunchPageMassage>(new LaunchPageMassage { PageObject = pageView, BackwardView = backwardView, PageView = permission });
            }
            else
            {
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<LaunchPageMassage>(new LaunchPageMassage { PageObject = pageView, BackwardView = backwardView, TitleName = "" });
            }

        }

        public void ChangeViewUsingViewModelPermission(object pageView, MediTechViewModelBase viewModel, object backwardView = null)
        {
            if (pageView != null)
            {
                System.Windows.Controls.UserControl usercontrol = (System.Windows.Controls.UserControl)pageView;
                string className = usercontrol.GetType().Name;
                string Namespace = usercontrol.GetType().Namespace;
                var permission = AppUtil.Current.PageViewPermission.FirstOrDefault(p => p.NamespaceName == Namespace && p.ClassName == className);
                if (permission == null)
                {
                    WarningDialog("คุณไม่มีสิทธิ์กระทำการใดๆ นี้ ถ้าจำเป็นต้องการใช้งานส่วนนี้โปรดติดต่อ Admin");
                    return;
                }
                usercontrol.DataContext = viewModel;

                (usercontrol.DataContext as MediTechViewModelBase).View = pageView; ;
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<LaunchPageMassage>(new LaunchPageMassage { PageObject = pageView, BackwardView = backwardView, PageView = permission });
            }
            else
            {
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<LaunchPageMassage>(new LaunchPageMassage { PageObject = pageView, BackwardView = backwardView, TitleName = "" });
            }

        }

        public void CloseViewDialog(ActionDialog status)
        {
            if (View != null)
            {
                if (View is System.Windows.Controls.UserControl)
                {
                    var parent = ((System.Windows.Controls.UserControl)View).Parent;

                    if (parent is System.Windows.Window)
                    {
                        ResultDialog = status;
                        ViewModelLocator.Cleanup();
                        (parent as System.Windows.Window).Close();
                    }
                }
            }
        }

        public void ChangeView_CloseViewDialog(object pageView, ActionDialog actionStatus)
        {

            var parent = ((System.Windows.Controls.UserControl)pageView).Parent;
            if (parent == null)
            {
                if (BackwardView == null)
                {
                    ChangeViewPermission(pageView);
                }
                else
                {
                    ChangeViewPermission(BackwardView);
                }

            }
            else if (parent is System.Windows.Window)
            {
                CloseViewDialog(actionStatus);
            }

        }

        public void RibbonExpand(bool IsMinimized)
        {
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<bool>(IsMinimized, "Ribbon");
        }

        public void NavPanelExpand(bool IsMinimized)
        {
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<bool>(IsMinimized, "NavPanel");
        }

        #endregion

        #region DialogMessage

        public void SaveSuccessDialog()
        {
            DXMessageBox.Show("บันทึกข้อมูลเรียบร้อย", "Sucess", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        public void SaveSuccessDialog(FrameworkElement owner)
        {
            DXMessageBox.Show(owner, "บันทึกข้อมูลเรียบร้อย", "Sucess", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        public void DeleteSuccessDialog()
        {
            DXMessageBox.Show("ลบข้อมูลเรียบร้อย", "Sucess", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void SaveSuccessDialog(string message)
        {
            DXMessageBox.Show(message, "Sucess", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        public void InformationDialog(string message)
        {
            DXMessageBox.Show(message, "", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        public void WarningDialog(string message)
        {
            DXMessageBox.Show(message, "Warining", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        public void ErrorDialog(string errorMessage)
        {
            DXMessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void ErrorDialog(FrameworkElement owner,string errorMessage)
        {
            DXMessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        public MessageBoxResult DeleteDialog()
        {
            return DXMessageBox.Show("คุณต้องการลบรายที่เลือก ใช้หรือไม่ ?", "Question", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
        }
        public MessageBoxResult QuestionDialog(string massage)
        {
            return DXMessageBox.Show(massage, "Question", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
        }

        public string ShowSaveFileDialog(string title, string filter)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            string name = System.Windows.Forms.Application.ProductName;
            int n = name.LastIndexOf(".") + 1;
            if (n > 0) name = name.Substring(n, name.Length - n);
            dlg.Title = "Export To " + title;
            dlg.FileName = name;
            dlg.Filter = filter;
            if (dlg.ShowDialog() == DialogResult.OK) return dlg.FileName;
            return "";
        }

        public void OpenFile(string fileName)
        {
            if (System.Windows.Forms.MessageBox.Show("Do you want to open this file?", "Export To...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    process.StartInfo.FileName = fileName;
                    process.StartInfo.Verb = "Open";
                    process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                    process.Start();
                }
                catch
                {
                    System.Windows.Forms.MessageBox.Show("Cannot find an application on your system suitable for openning the file with exported data.", System.Windows.Forms.Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        #endregion



    }
}
