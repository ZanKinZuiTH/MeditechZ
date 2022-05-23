using DevExpress.Xpf.NavBar;
using DevExpress.Xpf.Ribbon;
using DevExpress.Xpf.WindowsUI;
using DevExpress.Xpf.WindowsUI.Navigation;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using MediTech.ViewModels;
using GalaSoft.MvvmLight.Ioc;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Core;

namespace MediTech
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : DXRibbonWindow
    {
        bool logOff = false;
        public MainWindow()
        {
            InitializeComponent();
            documentFrame.Navigated += OnDocumentFrameNavigated;
            documentFrame.Navigating += OnDocumentFrameNavigating;
            navPanelView.NavPaneExpandedChanged += navPanelView_NavPaneExpandedChanged;
            navPanelView.NavPaneExpandedChanging += navPanelView_NavPaneExpandedChanging;
            navPanelView.ActiveGroupChanged += navPanelView_ActiveGroupChanged;

            btnLogOff.Click += btnLogOff_Click;
            btnExit.Click += btnExit_Click;
            this.Closing += MainWindow_Closing;
            this.Closed += MainWindow_Closed;
            LoadTheme();

            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<MediTech.Model.PageViewModel>(this, LaunchPage);
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<MediTech.Models.LaunchPageMassage>(this, ChangeView);
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<bool>(this, "Ribbon", RibbonExpand);
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<bool>(this, "NavPanel", NavPanelExpand);

            if (AppUtil.Current.ApplicationId == "AEC")
            {
                Image myImage3 = new Image();
                BitmapImage bi3 = new BitmapImage();
                bi3.BeginInit();
                Uri uri = new Uri(@"pack://application:,,,/MediTech;component/Untitled-2.ico");
                bi3.UriSource = uri;
                bi3.EndInit();
                frmMain.Icon = bi3;
            }

        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (logOff == false)
            {
                MessageBoxResult result = DXMessageBox.Show("คุณต้องการปิดโปรแกรมใช้หรือไม่ ?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result != MessageBoxResult.Yes)
                {
                    e.Cancel = true;
                }
            }

        }

        void btnLogOff_Click(object sender, RoutedEventArgs e)
        {
            AppUtil.Current = null;
            LoginScreen login = new LoginScreen();
            logOff = true;
            this.Close();
            login.Show();

        }

        void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        void RibbonExpand(bool IsExpanded)
        {

            ribbon.IsMinimized = !IsExpanded;
        }

        void NavPanelExpand(bool IsExpanded)
        {
            navPanelView.IsExpanded = IsExpanded;
        }


        void MainWindow_Closed(object sender, EventArgs e)
        {
            SaveTheme();

            GalaSoft.MvvmLight.Messaging.Messenger.Reset();
            if (SimpleIoc.Default.IsRegistered<MainViewModel>())
                SimpleIoc.Default.Unregister<MainViewModel>();
        }

        void LaunchPage(MediTech.Model.PageViewModel pageView)
        {
            try
            {

                if (pageView != null)
                {

                    var viewSource = Activator.CreateInstance(Type.GetType(pageView.NamespaceName + "." + pageView.ClassName));
                    ((viewSource as UserControl).DataContext as MediTechViewModelBase).View = viewSource;
                    documentFrame.Navigate(viewSource);
                    txtViewName.Text = pageView.LocalName;

                    if (navPanelView.IsPopupOpen)
                        navPanelView.IsPopupOpen = false;

                    if (pageView.ViewCode == "DCTOR" || pageView.ViewCode == "REGTS")
                        txtViewName.Visibility = System.Windows.Visibility.Collapsed;
                    else
                        txtViewName.Visibility = System.Windows.Visibility.Visible;

                    ((UserControl)viewSource).Loaded += NavigateContent_Loaded;
                    ((UserControl)viewSource).Unloaded += NavigateContent_Unloaded;

                }
                else
                {
                    documentFrame.Navigate(null);
                    if (this.DataContext is MainViewModel)
                    {
                        MainViewModel mainViewModel = (MainViewModel)this.DataContext;
                        mainViewModel.SelectedPage = null;
                        txtViewName.Text = "";
                    }
                }
            }
            catch (Exception er)
            {

                MessageBox.Show(er.Message);
                documentFrame.Navigate(null);
                MainViewModel mainViewModel = (MainViewModel)this.DataContext;
                mainViewModel.SelectedPage = null;
                txtViewName.Text = "";
            }
            finally
            {
                MediTech.Helpers.MemoryManagement.FlushMemory();
                ViewModelLocator.Cleanup();
            }
        }

        void NavigateContent_Loaded(object sender, RoutedEventArgs e)
        {
            UserControl source = (UserControl)sender;
            if (source.DataContext is MediTechViewModelBase)
            {
                ((MediTechViewModelBase)source.DataContext).OnLoaded();
            }
        }

        private void NavigateContent_Unloaded(object sender, RoutedEventArgs e)
        {
            UserControl source = (UserControl)sender;
            if (source.DataContext is MediTechViewModelBase)
            {
                ((MediTechViewModelBase)source.DataContext).UnLoaded();
            }
        }

        void ChangeView(MediTech.Models.LaunchPageMassage launchPageMassage)
        {
            try
            {

                if (launchPageMassage.PageObject != null)
                {
                    ((launchPageMassage.PageObject as UserControl).DataContext as MediTechViewModelBase).View = launchPageMassage.PageObject;

                    if (launchPageMassage.BackwardView != null)
                    {
                        ((launchPageMassage.PageObject as UserControl).DataContext as MediTechViewModelBase).BackwardView = launchPageMassage.BackwardView;
                    }


                    if (launchPageMassage.PageView != null)
                    {
                        if (launchPageMassage.PageView.Type.ToLower() == "menu")
                        {
                            int? pageViewModuleUID = launchPageMassage.PageView.PageViewModuleUID;
                            int? pageViewUID = launchPageMassage.PageView.PageViewUID;
                            MainViewModel mainViewModel = (MainViewModel)this.DataContext;
                            mainViewModel.ClicktPageEvent = false;
                            mainViewModel.SelectedPage = mainViewModel.PageViewModule.FirstOrDefault(p => p.PageViewModuleUID == pageViewModuleUID)
                                .PageViews.FirstOrDefault(p => p.PageViewUID == pageViewUID);

                        }

                        documentFrame.Navigate(launchPageMassage.PageObject);

                        if (!string.IsNullOrEmpty(launchPageMassage.TitleName))
                        {
                            txtViewName.Text = launchPageMassage.TitleName;
                        }
                        else
                        {
                            txtViewName.Text = launchPageMassage.PageView.LocalName;
                        }

                        ((UserControl)launchPageMassage.PageObject).Loaded += NavigateContent_Loaded;
                        ((UserControl)launchPageMassage.PageObject).Unloaded += NavigateContent_Unloaded;


                    }
                    else
                    {
                        documentFrame.Navigate(launchPageMassage.PageObject);
                        txtViewName.Text = launchPageMassage.TitleName;
                    }

                }
                else
                {
                    documentFrame.Navigate(null);
                    if (this.DataContext is MainViewModel)
                    {
                        MainViewModel mainViewModel = (MainViewModel)this.DataContext;
                        mainViewModel.SelectedPage = null;
                        txtViewName.Text = "";
                    }
                }
            }
            catch (Exception er)
            {

                MessageBox.Show(er.Message);
            }
            finally
            {
                MediTech.Helpers.MemoryManagement.FlushMemory();
                ViewModelLocator.Cleanup();
            }
        }

        void OnDocumentFrameNavigating(object sender, NavigatingEventArgs e)
        {
            if (e.Cancel) return;
            NavigationFrame frame = (NavigationFrame)sender;
            FrameworkElement oldContent = (FrameworkElement)frame.Content;
            if (oldContent != null)
            {
                RibbonMergingHelper.SetMergeWith(oldContent, null);
            }
        }
        void OnDocumentFrameNavigated(object sender, NavigationEventArgs e)
        {
            FrameworkElement newContent = (FrameworkElement)e.Content;
            if (newContent != null)
            {
                RibbonMergingHelper.SetMergeWith(newContent, ribbon);
            }
        }

        private void LoadTheme()
        {
            string defaultTheme = Properties.Settings.Default.ThemeName;
            if (string.IsNullOrEmpty(defaultTheme))
            {
                defaultTheme = Theme.DXStyleName;
            }
            DevExpress.Xpf.Core.ApplicationThemeHelper.ApplicationThemeName = defaultTheme;
        }

        private void SaveTheme()
        {
            Properties.Settings.Default.ThemeName = DevExpress.Xpf.Core.ApplicationThemeHelper.ApplicationThemeName;
            Properties.Settings.Default.Save();
        }

        void navPanelView_NavPaneExpandedChanged(object sender, NavPaneExpandedChangedEventArgs e)
        {
            layoutPanel.ItemWidth = GridLength.Auto;
        }
        void navPanelView_NavPaneExpandedChanging(object sender, NavPaneExpandedChangingEventArgs e)
        {
            if (navPanelView.Expander != null)
            {
                navPanelView.Expander.MaxWidth = e.IsExpanded ? Double.PositiveInfinity : navPanelView.Expander.ActualWidth;
            }
        }


        private void navPanelView_ActiveGroupChanged(object sender, NavBarActiveGroupChangedEventArgs e)
        {
            if (!navPanelView.IsExpanded)
            {
                navPanelView.IsPopupOpen = true;
            }
        }

    }
}
