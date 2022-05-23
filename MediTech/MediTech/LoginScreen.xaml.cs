using MediTech.DataService;
using MediTech.Model;
using MediTech.ViewModels;
using ShareLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MediTech
{
    /// <summary>
    /// Interaction logic for LoginScreen.xaml
    /// </summary>
    public partial class LoginScreen : Window
    {
        #region DataService

        private UserManageService userManageService;

        #endregion

        string password;
        string userName;
        public LoginScreen()
        {
            InitializeComponent();
            btnLoginButton.Click += btnLoginButton_Click;
            btnCancelButton.Click += btnCancelButton_Click;
            this.Loaded += LoginScreen_Loaded;
            txtUserName.Focus();
            userManageService = new UserManageService();
        }

        void LoginScreen_Loaded(object sender, RoutedEventArgs e)
        {
#if DEBUG
            txtUserName.Text = "admin";
            txtPassword.Text = "abc123";
            btnLoginButton_Click(null, null);
#endif

            //#if DEBUG
            //            txtUserName.Text = "015000837";
            //            txtPassword.Text = "0837";
            //            btnLoginButton_Click(null, null);
            //#endif
            string applicationId = System.Configuration.ConfigurationManager.AppSettings["ApplicationId"];

            if (applicationId == "AEC")
            {
                Image myImage3 = new Image();
                BitmapImage bi3 = new BitmapImage();
                bi3.BeginInit();
                Uri uri = new Uri(@"pack://application:,,,/MediTech;component/Untitled-2.ico");
                bi3.UriSource = uri;
                bi3.EndInit();
                frmLogin.Icon = bi3;
            }

        }

        void btnCancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        void btnLoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUserName.Text.Trim()))
            {
                System.Windows.Forms.MessageBox.Show("กรุณาระบุ ผู้ใช้งาน", "Warining", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUserName.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtPassword.Text.Trim()))
            {
                System.Windows.Forms.MessageBox.Show("กรุณาใส่ รหัสผ่าน", "Warining", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }
            password = Encryption.Encrypt(txtPassword.Text.Trim());
            userName = txtUserName.Text.Trim();
            this.Hide();
            ExecuteLogin();
        }




        public void ExecuteLogin()
        {
            try
            {
                LoginModel loginModel = new LoginModel();
                loginModel.LoginName = userName;
                loginModel.Password = password;
                var users = userManageService.CheckUserByLogin(loginModel);
                if (users.Count() > 0)
                {

                    var user = users.FirstOrDefault();
                    if ((user.ActiveFrom != null || user.ActiveFrom <= DateTime.Now) && (user.ActiveTo != null || user.ActiveTo >= DateTime.Now))
                    {
                        System.Windows.Forms.MessageBox.Show("ผู้ใช้งาน หมดอายุการใช้งาน", "Warining", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {

                        AppUtil.Current.LoginID = user.loginModel.LoginUID;
                        AppUtil.Current.UserID = user.CareproviderUID;
                        //AppUtil.Current.RoleUID = user.loginModel.RoleUID;
                        AppUtil.Current.LoginName = user.loginModel.LoginName;
                        AppUtil.Current.UserName = user.FirstName + " " + user.LastName;
                        AppUtil.Current.IsAdmin = user.loginModel.IsAdmin;
                        AppUtil.Current.IsDoctor = user.IsDoctor;
                        AppUtil.Current.IsRadiologist = user.IsRadiologist;
                        AppUtil.Current.IsAdminRadiologist = user.IsAdminRadiologist;
                        AppUtil.Current.IsAdminRadread = user.IsAdminRadread;
                        AppUtil.Current.IsRDUStaff = user.IsRDUStaff;
                        AppUtil.Current.RoleName = user.loginModel.RoleName;
                        //AppUtil.Current.OwnerOrganisationUID = user.loginModel.OwnerOrganisationUID ?? 0;

                        AppUtil.Current.ApplicationVersion = System.Configuration.ConfigurationManager.AppSettings["ApplicationVersion"];
                        AppUtil.Current.ApplicationId = System.Configuration.ConfigurationManager.AppSettings["ApplicationId"];
                        AppUtil.Current.ApplicationStaus = "online";

                        //List<RoleProfileModel> roleProfile = userManageService.GetRoleProfileByLoginUID(AppUtil.Current.LoginID);



                        //if (roleProfile != null)
                        //{
                        //    roleProfile = roleProfile.OrderBy(p => p.RoleUID).ToList();
                        //    List<CareproviderOrganisationModel> listCareOrgan = new List<CareproviderOrganisationModel>();
                        //    var healthOrganisationIDs = roleProfile.Select(p => p.HealthOrganisationUID).Distinct();
                        //    foreach (var healthOrganisationID in healthOrganisationIDs)
                        //    {
                        //        CareproviderOrganisationModel newObject = new CareproviderOrganisationModel();
                        //        newObject.CareproviderUID = user.CareproviderUID;
                        //        newObject.HealthOrganisationUID = healthOrganisationID;
                        //        listCareOrgan.Add(newObject);
                        //    }
                        //    AppUtil.Current.CareproviderOrganisations = listCareOrgan;
                        //}

                        RoleScreen roleScreen = new RoleScreen();
                        roleScreen.Show();
                        this.Close();
                        // AppUtil.Current.OwnerOrganisationUID = int.Parse(System.Configuration.ConfigurationManager.AppSettings["OwnerOrganisationUID"].ToString());

                    }

                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("ผู้ใช้งาน/รหัสผ่าน ไม่ถูกต้อง", "Warining", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Show();
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Show();
            }

        }

    }
}
