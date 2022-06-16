using System;
using MediTech.DataService;
using MediTech.ViewModels;
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
using System.Windows.Threading;
using System.Threading;
using MediTech.Model;

namespace MediTech
{
    /// <summary>
    /// Interaction logic for RoleScreen.xaml
    /// </summary>
    public partial class RoleScreen : Window
    {
        #region DataService

        private UserManageService userManageService;

        #endregion
        public RoleScreen()
        {
            InitializeComponent();
            this.Loaded += RoleScreen_Loaded;
            btnContinue.Click += btnContinue_Click;
            btnCancel.Click += btnCancel_Click;
            lkeOrganisation.EditValueChanged += LkeOrganisation_EditValueChanged;
            userManageService = new UserManageService();
        }

 
        void RoleScreen_Loaded(object sender, RoutedEventArgs e)
        {
            var organisations = userManageService.GetCareProviderOrganisationByUser(AppUtil.Current.UserID);
            if (organisations != null)
            {
                lkeOrganisation.ItemsSource = organisations;
                lkeOrganisation.SelectedIndex = 0;
                AppUtil.Current.CareproviderOrganisations = organisations;
            }
            //grdRole.ItemsSource = roleProfile;
            //#if DEBUG
            //            btnContinue_Click(null, null);
            //#endif
        }

        private void LkeOrganisation_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
           if(lkeOrganisation.EditValue != null)
            {
                List<RoleProfileModel> roleProfile = userManageService.GetRoleProfileByLoginUID(AppUtil.Current.LoginID, (int)lkeOrganisation.EditValue);
                grdRole.ItemsSource = roleProfile;

                _careproviderLocations = userManageService.GetCareProviderLocationByUser(AppUtil.Current.UserID, (int)lkeOrganisation.EditValue);
            }
        }

        private List<CareproviderLocationModel> _careproviderLocations;
        void btnContinue_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (grdRole.SelectedItem == null)
                {
                    System.Windows.Forms.MessageBox.Show("กรุณาเลือกสิทธิ์", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return;
                }

                AppUtil.Current.RoleUID = ((RoleProfileModel)grdRole.SelectedItem).RoleUID;
                AppUtil.Current.LocationUID = ((RoleProfileModel)grdRole.SelectedItem).LocationUID;
                AppUtil.Current.OwnerOrganisationUID = ((RoleProfileModel)grdRole.SelectedItem).HealthOrganisationUID;

                AppUtil.Current.CareproviderLocations = _careproviderLocations;
                userManageService.StampAppLastAccess(AppUtil.Current.LoginID);
                this.Hide();
                MainWindow mainWindow = new MainWindow();
                MainViewModel.MainWindow = mainWindow;
                mainWindow.Show();
                this.Close();
                //Thread thread = new Thread(LoadSpashScreen);
                //thread.IsBackground = true;
                //thread.Start();
            }
            catch (Exception ex)
            {

                MessageBoxResult result = MessageBox.Show(ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
                if (result == MessageBoxResult.OK)
                {
                    this.Close();
                }
            }


        }

        void LoadSpashScreen()
        {

            SplashScreen splashScreen = null;
            try
            {
                System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, (Action)delegate
                {
                    splashScreen = new SplashScreen();
                    splashScreen.Show();
                });


                System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    MainWindow mainWindow = new MainWindow();
                    MainViewModel.MainWindow = mainWindow;
                    splashScreen.CloseSplashScreen();
                    mainWindow.Show();
                    this.Close();
                });
            }
            catch (Exception ex)
            {

                System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    if (splashScreen != null)
                    {
                        splashScreen.Close();
                    }
                    this.Show();
                });

                System.Windows.Forms.MessageBox.Show(ex.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }
        void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            LoginScreen login = new LoginScreen();
            login.Show();
            this.Close();
        }
    }
}
