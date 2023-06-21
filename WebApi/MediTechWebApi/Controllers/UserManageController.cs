using MediTech.DataBase;
using MediTech.Model;
using ShareLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web.Http;


namespace MediTechWebApi.Controllers
{
    [RoutePrefix("Api/UserManage")]
    public class UserManageController : ApiController
    {
        protected MediTechEntities db = new MediTechEntities();

        [Route("GetUserByLogin")]
        [HttpGet]
        public List<CareproviderModel> GetUserByLogin(string userName, string password)
        {
            List<CareproviderModel> data = new List<CareproviderModel>();
            DataTable dt = SqlDirectStore.pGetUserByLogins(userName, password);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                CareproviderModel care = new CareproviderModel();
                care.loginModel = new LoginModel();
                care.ActiveFrom = dt.Rows[i]["ActiveFrom"].ToString() != "" ? DateTime.Parse(dt.Rows[i]["ActiveFrom"].ToString()) : (DateTime?)null;
                care.ActiveTo = dt.Rows[i]["ActiveTo"].ToString() != "" ? DateTime.Parse(dt.Rows[i]["ActiveTo"].ToString()) : (DateTime?)null;
                care.FirstName = dt.Rows[i]["FirstName"].ToString();
                care.LastName = dt.Rows[i]["LastName"].ToString();
                care.CareproviderUID = int.Parse(dt.Rows[i]["CareproviderUID"].ToString());
                care.IsRadiologist = dt.Rows[i]["IsRadiologist"].ToString() != "" ? bool.Parse(dt.Rows[i]["IsRadiologist"].ToString()) : false;
                care.IsAdminRadiologist = dt.Rows[i]["IsAdminRadiologist"].ToString() != "" ? bool.Parse(dt.Rows[i]["IsAdminRadiologist"].ToString()) : false;
                care.IsDoctor = dt.Rows[i]["IsDoctor"].ToString() != "" ? bool.Parse(dt.Rows[i]["IsDoctor"].ToString()) : false;
                care.IsAdminRadread = dt.Rows[i]["IsAdminRadread"].ToString() != "" ? bool.Parse(dt.Rows[i]["IsAdminRadread"].ToString()) : false;
                care.loginModel.LoginUID = int.Parse(dt.Rows[i]["LoginUID"].ToString());
                care.loginModel.LoginName = dt.Rows[i]["LoginName"].ToString();
                care.loginModel.RoleName = dt.Rows[i]["RoleName"].ToString();
                care.loginModel.RoleUID = int.Parse(dt.Rows[i]["RoleUID"].ToString());
                care.loginModel.IsAdmin = dt.Rows[i]["IsAdmin"].ToString() != "" ? bool.Parse(dt.Rows[i]["IsAdmin"].ToString()) : false;
                data.Add(care);
            }

            return data;
            //List<CareproviderModel> data = (from lo in db.Login
            //                                join cp in db.Careprovider on lo.CareproviderUID equals cp.UID
            //                                join rolo in db.RoleProfile on lo.UID equals rolo.LoginUID
            //                                join ro in db.Role on rolo.RoleUID equals ro.UID
            //                                where lo.StatusFlag == "A"
            //                                && cp.StatusFlag == "A"
            //                                && rolo.StatusFlag == "A"
            //                                && ro.StatusFlag == "A"
            //                                && lo.LoginName == userName
            //                                && lo.Password == password
            //                                && lo.Type == "APP"
            //                                select new CareproviderModel
            //                                {
            //                                    ActiveFrom = lo.ActiveFrom,
            //                                    ActiveTo = lo.ActiveTo,
            //                                    FirstName = cp.FirstName,
            //                                    LastName = cp.LastName,
            //                                    CareproviderUID = lo.CareproviderUID,
            //                                    IsRadiologist = cp.IsRadiologist ?? false,
            //                                    IsDoctor = cp.IsDoctor ?? false,
            //                                    loginModel = new LoginModel
            //                                    {
            //                                        LoginUID = lo.UID,
            //                                        LoginName = lo.LoginName,
            //                                        RoleName = ro.Name,
            //                                        RoleUID = ro.UID,
            //                                        OwnerOrganisationName = SqlFunction.fGetHealthOrganisationName(lo.OwnerOrganisationUID ?? 0),
            //                                        OwnerOrganisationUID = lo.OwnerOrganisationUID,
            //                                        IsAdmin = lo.IsAdmin ?? false
            //                                    }
            //                                }).ToList();
            //return data;
        }

        [Route("CheckUserByLogin")]
        [HttpPost]
        public HttpResponseMessage CheckUserByLogin(LoginModel loginModel)
        {
            try
            {
                List<CareproviderModel> data = new List<CareproviderModel>();
                DataTable dt = SqlDirectStore.pGetUserByLogins(loginModel.LoginName, loginModel.Password);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CareproviderModel care = new CareproviderModel();
                    care.loginModel = new LoginModel();
                    care.ActiveFrom = dt.Rows[i]["ActiveFrom"].ToString() != "" ? DateTime.Parse(dt.Rows[i]["ActiveFrom"].ToString()) : (DateTime?)null;
                    care.ActiveTo = dt.Rows[i]["ActiveTo"].ToString() != "" ? DateTime.Parse(dt.Rows[i]["ActiveTo"].ToString()) : (DateTime?)null;
                    care.Code = dt.Rows[i]["Code"].ToString();
                    care.FirstName = dt.Rows[i]["FirstName"].ToString();
                    care.LastName = dt.Rows[i]["LastName"].ToString();
                    care.CareproviderUID = int.Parse(dt.Rows[i]["CareproviderUID"].ToString());
                    care.IsRadiologist = dt.Rows[i]["IsRadiologist"].ToString() != "" ? bool.Parse(dt.Rows[i]["IsRadiologist"].ToString()) : false;
                    care.IsAdminRadiologist = dt.Rows[i]["IsAdminRadiologist"].ToString() != "" ? bool.Parse(dt.Rows[i]["IsAdminRadiologist"].ToString()) : false;
                    care.IsDoctor = dt.Rows[i]["IsDoctor"].ToString() != "" ? bool.Parse(dt.Rows[i]["IsDoctor"].ToString()) : false;
                    care.IsAdminRadread = dt.Rows[i]["IsAdminRadread"].ToString() != "" ? bool.Parse(dt.Rows[i]["IsAdminRadread"].ToString()) : false;
                    care.IsRDUStaff = dt.Rows[i]["IsRDUStaff"].ToString() != "" ? bool.Parse(dt.Rows[i]["IsRDUStaff"].ToString()) : false;
                    care.loginModel.LoginUID = int.Parse(dt.Rows[i]["LoginUID"].ToString());
                    care.loginModel.LoginName = dt.Rows[i]["LoginName"].ToString();
                    care.loginModel.RoleName = dt.Rows[i]["RoleName"].ToString();
                    care.loginModel.RoleUID = int.Parse(dt.Rows[i]["RoleUID"].ToString());
                    care.loginModel.IsAdmin = dt.Rows[i]["IsAdmin"].ToString() != "" ? bool.Parse(dt.Rows[i]["IsAdmin"].ToString()) : false;
                    data.Add(care);
                }

                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("StampAppLastAccess")]
        [HttpPut]
        public HttpResponseMessage StampAppLastAccess(int loginUID)
        {
            try
            {
                Login login = db.Login.Find(loginUID);
                login.LastAccessedDTTM = DateTime.Now;
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("StampWebLastAccess")]
        [HttpPut]
        public HttpResponseMessage StampWebLastAccess(int loginUID)
        {
            try
            {
                Login login = db.Login.Find(loginUID);
                login.LastAccessedWebDttm = DateTime.Now;
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("GetCareproviderAll")]
        [HttpGet]
        public List<CareproviderModel> GetCareproviderAll()
        {
            List<CareproviderModel> data = (from ca in db.Careprovider
                                            join lo in db.Login on
                                            new
                                            {
                                                key1 = ca.UID,
                                                key2 = "A"
                                            }
                                            equals
                                            new
                                            {
                                                key1 = lo.CareproviderUID,
                                                key2 = lo.StatusFlag
                                            }
                                            into joinedlogin
                                            from jologin in joinedlogin.DefaultIfEmpty()
                                            where ca.StatusFlag == "A"
                                            select new CareproviderModel
                                            {
                                                CareproviderUID = ca.UID,
                                                Code = ca.Code,
                                                TITLEUID = ca.TITLEUID,
                                                TitleDesc = ca.TITLEUID != null ? SqlFunction.fGetRfValDescription(ca.TITLEUID.Value) : "",
                                                FirstName = ca.FirstName,
                                                MiddleName = ca.MiddleName,
                                                LastName = ca.LastName,
                                                FullName = SqlFunction.fGetCareProviderName(ca.UID),
                                                SEXXXUID = ca.SEXXXUID,
                                                SexDesc = ca.SEXXXUID != null ? SqlFunction.fGetRfValDescription(ca.SEXXXUID.Value) : "",
                                                EnglishName = ca.EnglishName,
                                                ImgPath = ca.ImgPath,
                                                LicenseNo = ca.LicenseNo,
                                                LicenseIssueDttm = ca.LicenseIssueDttm,
                                                LicenseExpiryDttm = ca.LicenseExpiryDttm,
                                                DOBDttm = ca.DOBDttm,
                                                IsDoctor = ca.IsDoctor ?? false,
                                                IsRadiologist = ca.IsRadiologist ?? false,
                                                IsAdminRadread = ca.IsAdminRadread ?? false,
                                                IsRDUStaff = ca.IsRDUStaff ?? false,
                                                Tel = ca.Tel,
                                                Email = ca.Email,
                                                Qualification = ca.Qualification,
                                                ActiveFrom = ca.ActiveFrom,
                                                ActiveTo = ca.ActiveTo,
                                                MWhen = ca.MWhen,
                                                loginModel = jologin == null ? null : new LoginModel
                                                {
                                                    LoginUID = jologin.UID,
                                                    LoginName = jologin.LoginName,
                                                    ActiveFrom = jologin.ActiveFrom,
                                                    ActiveTo = jologin.ActiveTo,
                                                    LastAccessedDttm = jologin.LastAccessedDTTM,
                                                    LastAccessedWebDttm = jologin.LastAccessedWebDttm,
                                                    LastPasswordModifiredDTTM = jologin.LastPasswordModifiredDTTM
                                                }

                                            }).ToList();

            //if (data != null && data.Count > 0)
            //{
            //    foreach (var careprovider in data)
            //    {
            //        careprovider.loginModel = new LoginModel();

            //    }
            //}

            return data;
        }

        [Route("GetCareproviderByUID")]
        [HttpGet]
        public CareproviderModel GetCareproviderByUID(int careproviderUID)
        {
            CareproviderModel data = (from ca in db.Careprovider
                                      join lo in db.Login on
                                      new
                                      {
                                          key1 = ca.UID,
                                          key2 = "A"
                                      }
                                      equals
                                      new
                                      {
                                          key1 = lo.CareproviderUID,
                                          key2 = lo.StatusFlag
                                      }
                                      into joinedlogin
                                      from jologin in joinedlogin.DefaultIfEmpty()
                                      join pro in db.RoleProfile
                                          on jologin.UID equals pro.LoginUID
                                      into joinedprofile
                                      from joprofile in joinedprofile.DefaultIfEmpty()
                                      join role in db.Role
                                          on joprofile.RoleUID equals role.UID
                                      into joinedapprole
                                      from joapprole in joinedapprole.DefaultIfEmpty()
                                      where ca.StatusFlag == "A"
                                      && ca.UID == careproviderUID
                                      select new CareproviderModel
                                      {
                                          CareproviderUID = ca.UID,
                                          Code = ca.Code,
                                          TITLEUID = ca.TITLEUID,
                                          TitleDesc = ca.TITLEUID != null ? SqlFunction.fGetRfValDescription(ca.TITLEUID.Value) : "",
                                          FirstName = ca.FirstName,
                                          MiddleName = ca.MiddleName,
                                          LastName = ca.LastName,
                                          SEXXXUID = ca.SEXXXUID,
                                          SexDesc = ca.SEXXXUID != null ? SqlFunction.fGetRfValDescription(ca.SEXXXUID.Value) : "",
                                          EnglishName = ca.EnglishName,
                                          ImgPath = ca.ImgPath,
                                          LicenseNo = ca.LicenseNo,
                                          LicenseIssueDttm = ca.LicenseIssueDttm,
                                          LicenseExpiryDttm = ca.LicenseExpiryDttm,
                                          DOBDttm = ca.DOBDttm,
                                          IsDoctor = ca.IsDoctor ?? false,
                                          IsRadiologist = ca.IsRadiologist ?? false,
                                          IsAdminRadiologist = ca.IsAdminRadiologist ?? false,
                                          IsAdminRadread = ca.IsAdminRadread ?? false,
                                          IsRDUStaff = ca.IsRDUStaff ?? false,
                                          CPTYPUID = ca.CPTYPUID,
                                          Tel = ca.Tel,
                                          Email = ca.Email,
                                          Qualification = ca.Qualification,
                                          LineID = ca.LineID,
                                          ActiveFrom = ca.ActiveFrom,
                                          ActiveTo = ca.ActiveTo,
                                          MWhen = ca.MWhen,
                                          loginModel = jologin == null ? null : new LoginModel
                                          {
                                              LoginUID = jologin.UID,
                                              LoginName = jologin.LoginName,
                                              ActiveFrom = jologin.ActiveFrom,
                                              ActiveTo = jologin.ActiveTo,
                                              RoleUID = joapprole.UID,
                                              RoleName = joapprole.Name,
                                              LastAccessedDttm = jologin.LastAccessedDTTM,
                                              LastAccessedWebDttm = jologin.LastAccessedWebDttm,
                                              LastPasswordModifiredDTTM = jologin.LastPasswordModifiredDTTM
                                          }

                                      }).FirstOrDefault();

            if (data != null && data.loginModel != null)
            {
                data.loginModel.RoleProfiles = new List<RoleProfileModel>();
                data.loginModel.RoleProfiles = (from j in db.RoleProfile
                                                join i in db.Role on j.RoleUID equals i.UID
                                                join l in db.Login on j.LoginUID equals l.UID
                                                where j.StatusFlag == "A"
                                                && i.StatusFlag == "A"
                                                && j.LoginUID == data.loginModel.LoginUID
                                                select new RoleProfileModel
                                                {
                                                    RoleProfileUID = j.UID,
                                                    RoleUID = j.RoleUID,
                                                    RoleName = i.Name,
                                                    LoginUID = j.LoginUID,
                                                    StatusFlag = j.StatusFlag
                                                }).ToList();
            }

            return data;
        }

        [Route("GetCareproviderByCode")]
        [HttpGet]
        public CareproviderModel GetCareproviderByCode(string careproviderCode)
        {
            CareproviderModel data = (from ca in db.Careprovider
                                      join lo in db.Login on
                                      new
                                      {
                                          key1 = ca.UID,
                                          key2 = "A"
                                      }
                                      equals
                                      new
                                      {
                                          key1 = lo.CareproviderUID,
                                          key2 = lo.StatusFlag
                                      }
                                      into joinedlogin
                                      from jologin in joinedlogin.DefaultIfEmpty()
                                      join pro in db.RoleProfile
                                          on jologin.UID equals pro.LoginUID
                                      into joinedprofile
                                      from joprofile in joinedprofile.DefaultIfEmpty()
                                      join role in db.Role
                                          on joprofile.RoleUID equals role.UID
                                      into joinedapprole
                                      from joapprole in joinedapprole.DefaultIfEmpty()
                                      where ca.StatusFlag == "A"
                                      && ca.Code == careproviderCode
                                      select new CareproviderModel
                                      {
                                          CareproviderUID = ca.UID,
                                          Code = ca.Code,
                                          TITLEUID = ca.TITLEUID,
                                          TitleDesc = ca.TITLEUID != null ? SqlFunction.fGetRfValDescription(ca.TITLEUID.Value) : "",
                                          FirstName = ca.FirstName,
                                          MiddleName = ca.MiddleName,
                                          LastName = ca.LastName,
                                          SEXXXUID = ca.SEXXXUID,
                                          SexDesc = ca.SEXXXUID != null ? SqlFunction.fGetRfValDescription(ca.SEXXXUID.Value) : "",
                                          EnglishName = ca.EnglishName,
                                          ImgPath = ca.ImgPath,
                                          LicenseNo = ca.LicenseNo,
                                          LicenseIssueDttm = ca.LicenseIssueDttm,
                                          LicenseExpiryDttm = ca.LicenseExpiryDttm,
                                          DOBDttm = ca.DOBDttm,
                                          IsDoctor = ca.IsDoctor ?? false,
                                          IsRadiologist = ca.IsRadiologist ?? false,
                                          IsAdminRadiologist = ca.IsAdminRadiologist ?? false,
                                          IsAdminRadread = ca.IsAdminRadread ?? false,
                                          IsRDUStaff = ca.IsRDUStaff ?? false,
                                          Tel = ca.Tel,
                                          Email = ca.Email,
                                          Qualification = ca.Qualification,
                                          LineID = ca.LineID,
                                          ActiveFrom = ca.ActiveFrom,
                                          ActiveTo = ca.ActiveTo,
                                          MWhen = ca.MWhen,
                                          loginModel = jologin == null ? null : new LoginModel
                                          {
                                              LoginUID = jologin.UID,
                                              LoginName = jologin.LoginName,
                                              ActiveFrom = jologin.ActiveFrom,
                                              ActiveTo = jologin.ActiveTo,
                                              RoleUID = joapprole.UID,
                                              RoleName = joapprole.Name,
                                              LastAccessedDttm = jologin.LastAccessedDTTM,
                                              LastAccessedWebDttm = jologin.LastAccessedWebDttm,
                                              LastPasswordModifiredDTTM = jologin.LastPasswordModifiredDTTM
                                          }

                                      }).FirstOrDefault();

            if (data != null && data.loginModel != null)
            {
                data.loginModel.RoleProfiles = new List<RoleProfileModel>();
                data.loginModel.RoleProfiles = (from j in db.RoleProfile
                                                join i in db.Role on j.RoleUID equals i.UID
                                                join l in db.Login on j.LoginUID equals l.UID
                                                where j.StatusFlag == "A"
                                                && i.StatusFlag == "A"
                                                && j.LoginUID == data.loginModel.LoginUID
                                                select new RoleProfileModel
                                                {
                                                    RoleProfileUID = j.UID,
                                                    RoleUID = j.RoleUID,
                                                    RoleName = i.Name,
                                                    LoginUID = j.LoginUID,
                                                    StatusFlag = j.StatusFlag
                                                }).ToList();
            }

            return data;
        }

        [Route("GetRoleProfileByLoginUID")]
        [HttpGet]
        public List<RoleProfileModel> GetRoleProfileByLoginUID(int loginUID, int organisationUID)
        {
            DateTime dateNow = DateTime.Now;
            List<RoleProfileModel> roleProFileData = (from j in db.RoleProfile
                                                      join i in db.Role on j.RoleUID equals i.UID
                                                      join l in db.Login on j.LoginUID equals l.UID
                                                      join c in db.Careprovider on l.CareproviderUID equals c.UID
                                                      join h in db.CareproviderLocation on c.UID equals h.CareproviderUID
                                                      join lt in db.Location on h.LocationUID equals lt.UID
                                                      where j.StatusFlag == "A"
                                                      && i.StatusFlag == "A"
                                                      && c.StatusFlag == "A"
                                                      && h.StatusFlag == "A"
                                                      && lt.StatusFlag == "A"
                                                      && lt.Description != "Pac_Bed"
                                                      && j.LoginUID == loginUID
                                                      && h.HealthOrganisationUID == organisationUID
                                                      && (lt.ActiveFrom == null || DbFunctions.TruncateTime(lt.ActiveFrom) <= DbFunctions.TruncateTime(dateNow))
                                                      && (lt.ActiveTo == null || DbFunctions.TruncateTime(lt.ActiveTo) >= DbFunctions.TruncateTime(dateNow))
                                                      && (h.ActiveFrom == null || DbFunctions.TruncateTime(h.ActiveFrom) <= DbFunctions.TruncateTime(dateNow))
                                                      && (h.ActiveTo == null || DbFunctions.TruncateTime(h.ActiveTo) >= DbFunctions.TruncateTime(dateNow))
                                                      select new RoleProfileModel
                                                      {
                                                          RoleProfileUID = j.UID,
                                                          RoleUID = j.RoleUID,
                                                          RoleName = i.Name,
                                                          LoginUID = j.LoginUID,
                                                          LocationUID = h.LocationUID,
                                                          LocationName = SqlFunction.fGetLocationName(h.LocationUID),
                                                          HealthOrganisationUID = h.HealthOrganisationUID,
                                                          HealthOrganisationName = SqlFunction.fGetHealthOrganisationName(h.HealthOrganisationUID),
                                                          StatusFlag = j.StatusFlag
                                                      }).OrderBy(p => p.LocationName).ToList();
            return roleProFileData;
        }

        [Route("ManageCareProvider")]
        [HttpPost]
        public HttpResponseMessage ManageCareProvider(CareproviderModel careproviderData, int userID)
        {
            try
            {

                DateTime now = DateTime.Now;
                using (var tran = new TransactionScope())
                {
                    Careprovider careprovider = db.Careprovider.Find(careproviderData.CareproviderUID);
                    if (careprovider == null)
                    {
                        careprovider = new Careprovider();
                        careprovider.CUser = userID;
                        careprovider.CWhen = now;
                    }

                    careprovider.Code = careproviderData.Code;
                    careprovider.TITLEUID = careproviderData.TITLEUID;
                    careprovider.FirstName = careproviderData.FirstName;
                    careprovider.MiddleName = careproviderData.MiddleName;
                    careprovider.LastName = careproviderData.LastName;
                    careprovider.SEXXXUID = careproviderData.SEXXXUID;
                    careprovider.EnglishName = careproviderData.EnglishName;
                    careprovider.CPTYPUID = careproviderData.CPTYPUID;
                    careprovider.ImgPath = careproviderData.ImgPath;
                    careprovider.LicenseNo = careproviderData.LicenseNo;
                    careprovider.LicenseIssueDttm = careproviderData.LicenseIssueDttm;
                    careprovider.LicenseExpiryDttm = careproviderData.LicenseExpiryDttm;
                    careprovider.DOBDttm = careproviderData.DOBDttm;
                    careprovider.IsDoctor = careproviderData.IsDoctor;
                    careprovider.IsRadiologist = careproviderData.IsRadiologist;
                    careprovider.IsAdminRadiologist = careproviderData.IsAdminRadiologist;
                    careprovider.IsAdminRadread = careproviderData.IsAdminRadread;
                    careprovider.IsRDUStaff = careproviderData.IsRDUStaff;
                    careprovider.Tel = careproviderData.Tel;
                    careprovider.Email = careproviderData.Email;
                    careprovider.LineID = careproviderData.LineID;
                    careprovider.ActiveFrom = careproviderData.ActiveFrom;
                    careprovider.ActiveTo = careproviderData.ActiveTo;
                    careprovider.StatusFlag = "A";
                    careprovider.MUser = userID;
                    careprovider.MWhen = now;
                    careprovider.Qualification = careproviderData.Qualification;

                    db.Careprovider.AddOrUpdate(careprovider);

                    db.SaveChanges();
                    if (careproviderData.loginModel != null && !string.IsNullOrEmpty(careproviderData.loginModel.LoginName))
                    {
                        Login login = db.Login.Find(careproviderData.loginModel.LoginUID);
                        if (login == null)
                        {
                            login = new Login();
                            login.CUser = userID;
                            login.CWhen = now;

                            string password = Encryption.Encrypt("1234");
                            login.Password = password;
                        }



                        login.LoginName = careproviderData.loginModel.LoginName;

                        login.ActiveFrom = careproviderData.loginModel.ActiveFrom;
                        login.ActiveTo = careproviderData.loginModel.ActiveTo;
                        login.CareproviderUID = careprovider.UID;
                        login.MUser = userID;
                        login.MWhen = now;
                        login.StatusFlag = "A";

                        db.Login.AddOrUpdate(login);

                        db.SaveChanges();

                        #region Delete RoleProfile

                        IEnumerable<RoleProfile> roleProFiles = db.RoleProfile.Where(p => p.LoginUID == login.UID);

                        if (careproviderData.loginModel != null && careproviderData.loginModel.RoleProfiles == null)
                        {
                            foreach (var item in roleProFiles)
                            {
                                db.RoleProfile.Attach(item);
                                item.MUser = userID;
                                item.MWhen = now;
                                item.StatusFlag = "D";
                            }
                        }
                        else if (careproviderData.loginModel != null && careproviderData.loginModel.RoleProfiles != null)
                        {
                            foreach (var item in roleProFiles)
                            {
                                var data = careproviderData.loginModel.RoleProfiles.FirstOrDefault(p => p.RoleProfileUID == item.UID);
                                if (data == null)
                                {
                                    db.RoleProfile.Attach(item);
                                    item.MUser = userID;
                                    item.MWhen = now;
                                    item.StatusFlag = "D";
                                }

                            }
                        }

                        db.SaveChanges();
                        #endregion

                        if (careproviderData.loginModel != null && careproviderData.loginModel.RoleProfiles != null)
                        {
                            foreach (var item in careproviderData.loginModel.RoleProfiles)
                            {
                                RoleProfile rolePro = db.RoleProfile.Find(item.RoleProfileUID);

                                if (rolePro == null)
                                {
                                    rolePro = new RoleProfile();
                                    rolePro.CUser = userID;
                                    rolePro.CWhen = now;
                                    rolePro.MUser = userID;
                                    rolePro.MWhen = now;
                                    rolePro.StatusFlag = "A";
                                }
                                else
                                {
                                    if (item.MWhen != DateTime.MinValue)
                                    {
                                        rolePro.MUser = userID;
                                        rolePro.MWhen = now;
                                    }
                                }
                                rolePro.LoginUID = login.UID;
                                rolePro.RoleUID = item.RoleUID;

                                db.RoleProfile.AddOrUpdate(rolePro);
                                db.SaveChanges();
                            }
                        }

                    }

                    tran.Complete();
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }


        [Route("ChangePassword")]
        [HttpPost]
        public HttpResponseMessage ChangePassword(LoginModel loginModel)
        {
            try
            {
                Login login = db.Login.Find(loginModel.LoginUID);
                if (login != null)
                {
                    db.Login.Attach(login);

                    login.Password = loginModel.Password;
                    login.LastPasswordModifiredDTTM = DateTime.Now;
                    login.MWhen = DateTime.Now;

                    db.SaveChanges();
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("DeleteCareProvider")]
        [HttpDelete]
        public HttpResponseMessage DeleteCareProvider(int careproviderUID, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                Careprovider careprovider = db.Careprovider.Find(careproviderUID);
                if (careprovider != null)
                {
                    db.Careprovider.Attach(careprovider);
                    careprovider.StatusFlag = "D";
                    careprovider.MUser = userID;
                    careprovider.MWhen = now;
                }

                List<Login> login = db.Login.Where(p => p.CareproviderUID == careproviderUID).ToList();
                if (login != null)
                {
                    foreach (var item in login)
                    {
                        db.Login.Attach(item);
                        item.StatusFlag = "D";
                        item.MUser = userID;
                        item.MWhen = now;

                        List<RoleProfile> roleProfile = db.RoleProfile.Where(p => p.LoginUID == item.UID).ToList();
                        if (roleProfile != null)
                        {
                            foreach (var itemRolePro in roleProfile)
                            {
                                db.RoleProfile.Attach(itemRolePro);
                                itemRolePro.StatusFlag = "D";
                                itemRolePro.MUser = userID;
                                itemRolePro.MWhen = now;
                            }
                        }
                    }

                }

                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("GetCareProviderByType")]
        [HttpGet]
        public List<CareproviderModel> GetCareProviderByType(int type)
        {
            List<CareproviderModel> data = db.Careprovider.Where(p => p.CPTYPUID == type && p.StatusFlag == "A").Select(ca => new CareproviderModel
            {
                CareproviderUID = ca.UID,
                Code = ca.Code,
                TITLEUID = ca.TITLEUID,
                TitleDesc = ca.TITLEUID != null ? SqlFunction.fGetRfValDescription(ca.TITLEUID.Value) : "",
                FirstName = ca.FirstName,
                MiddleName = ca.MiddleName,
                LastName = ca.LastName,
                FullName = SqlFunction.fGetCareProviderName(ca.UID),
                SEXXXUID = ca.SEXXXUID,
                SexDesc = ca.SEXXXUID != null ? SqlFunction.fGetRfValDescription(ca.SEXXXUID.Value) : "",
                EnglishName = ca.EnglishName,
                ImgPath = ca.ImgPath,
                LicenseNo = ca.LicenseNo,
                LicenseIssueDttm = ca.LicenseIssueDttm,
                LicenseExpiryDttm = ca.LicenseExpiryDttm,
                DOBDttm = ca.DOBDttm,
                Qualification = ca.Qualification,
                IsDoctor = ca.IsDoctor ?? false,
                IsRadiologist = ca.IsRadiologist ?? false,
                IsAdminRadread = ca.IsAdminRadread ?? false,
                Tel = ca.Tel,
                Email = ca.Email,
                LineID = ca.LineID,
                ActiveFrom = ca.ActiveFrom,
                ActiveTo = ca.ActiveTo,
                MWhen = ca.MWhen,
            }).ToList();

            return data;
        }

        [Route("GetCareProviderDoctor")]
        [HttpGet]
        public List<CareproviderModel> GetCareProviderDoctor()
        {
            List<CareproviderModel> data = db.Careprovider.Where(p => p.IsDoctor.Value && p.StatusFlag == "A").Select(ca => new CareproviderModel
            {
                CareproviderUID = ca.UID,
                Code = ca.Code,
                TITLEUID = ca.TITLEUID,
                TitleDesc = ca.TITLEUID != null ? SqlFunction.fGetRfValDescription(ca.TITLEUID.Value) : "",
                FirstName = ca.FirstName,
                MiddleName = ca.MiddleName,
                LastName = ca.LastName,
                FullName = SqlFunction.fGetCareProviderName(ca.UID),
                SEXXXUID = ca.SEXXXUID,
                SexDesc = ca.SEXXXUID != null ? SqlFunction.fGetRfValDescription(ca.SEXXXUID.Value) : "",
                EnglishName = ca.EnglishName,
                ImgPath = ca.ImgPath,
                LicenseNo = ca.LicenseNo,
                LicenseIssueDttm = ca.LicenseIssueDttm,
                LicenseExpiryDttm = ca.LicenseExpiryDttm,
                DOBDttm = ca.DOBDttm,
                Qualification = ca.Qualification,
                IsDoctor = ca.IsDoctor ?? false,
                IsRadiologist = ca.IsRadiologist ?? false,
                IsAdminRadread = ca.IsAdminRadread ?? false,
                Tel = ca.Tel,
                Email = ca.Email,
                LineID = ca.LineID,
                ActiveFrom = ca.ActiveFrom,
                ActiveTo = ca.ActiveTo,
                MWhen = ca.MWhen,
            }).ToList();

            return data;
        }

        [Route("GetCareProviderDoctorByOrganisation")]
        [HttpGet]
        public List<CareproviderModel> GetCareProviderDoctorByOrganisation(int organisationUID)
        {


            List<CareproviderModel> data = (from ca in db.Careprovider
                                            join og in db.CareproviderOrganisation on ca.UID equals og.CareproviderUID
                                            where ca.StatusFlag == "A"
                                            && og.StatusFlag == "A"
                                            && ca.IsDoctor == true
                                            && og.HealthOrganisationUID == organisationUID
                                            select new CareproviderModel
                                            {
                                                CareproviderUID = ca.UID,
                                                Code = ca.Code,
                                                TITLEUID = ca.TITLEUID,
                                                TitleDesc = ca.TITLEUID != null ? SqlFunction.fGetRfValDescription(ca.TITLEUID.Value) : "",
                                                FirstName = ca.FirstName,
                                                MiddleName = ca.MiddleName,
                                                LastName = ca.LastName,
                                                FullName = SqlFunction.fGetCareProviderName(ca.UID),
                                                SEXXXUID = ca.SEXXXUID,
                                                SexDesc = ca.SEXXXUID != null ? SqlFunction.fGetRfValDescription(ca.SEXXXUID.Value) : "",
                                                EnglishName = ca.EnglishName,
                                                ImgPath = ca.ImgPath,
                                                LicenseNo = ca.LicenseNo,
                                                LicenseIssueDttm = ca.LicenseIssueDttm,
                                                LicenseExpiryDttm = ca.LicenseExpiryDttm,
                                                DOBDttm = ca.DOBDttm,
                                                Qualification = ca.Qualification,
                                                IsDoctor = ca.IsDoctor ?? false,
                                                IsRadiologist = ca.IsRadiologist ?? false,
                                                IsAdminRadread = ca.IsAdminRadread ?? false,
                                                Tel = ca.Tel,
                                                Email = ca.Email,
                                                LineID = ca.LineID,
                                                ActiveFrom = ca.ActiveFrom,
                                                ActiveTo = ca.ActiveTo,
                                                MWhen = ca.MWhen,
                                            }).ToList();

            return data;
        }

        [Route("GetCareProviderOrganisation")]
        [HttpGet]
        public List<CareproviderOrganisationModel> GetCareProviderOrganisation()
        {
            DateTime dateNow = DateTime.Now.Date;
            List<CareproviderOrganisationModel> data = (from j in db.CareproviderOrganisation
                                                        join i in db.Careprovider on j.CareproviderUID equals i.UID
                                                        where j.StatusFlag == "A"
                                                        && i.StatusFlag == "A"
                                                        && (i.ActiveFrom == null || DbFunctions.TruncateTime(i.ActiveFrom) <= DbFunctions.TruncateTime(dateNow))
                                                        && (i.ActiveTo == null || DbFunctions.TruncateTime(i.ActiveTo) >= DbFunctions.TruncateTime(dateNow))
                                                        select new CareproviderOrganisationModel
                                                        {
                                                            CareproviderOrganisationUID = j.UID,
                                                            HealthOrganisationUID = j.HealthOrganisationUID,
                                                            HealthOrganisationName = SqlFunction.fGetHealthOrganisationName(j.HealthOrganisationUID),
                                                            CareproviderUID = j.CareproviderUID,
                                                            CareproviderName = SqlFunction.fGetCareProviderName(j.CareproviderUID),
                                                            ActiveFrom = j.ActiveFrom,
                                                            ActiveTo = j.ActiveTo
                                                        }).ToList();

            return data;
        }


        [Route("GetCareProviderOrganisation")]
        [HttpGet]
        public List<CareproviderOrganisationModel> GetCareProviderOrganisation(int organisationUID)
        {
            DateTime dateNow = DateTime.Now.Date;
            List<CareproviderOrganisationModel> data = (from j in db.CareproviderOrganisation
                                                        join i in db.Careprovider on j.CareproviderUID equals i.UID
                                                        where j.StatusFlag == "A"
                                                        && i.StatusFlag == "A"
                                                        && j.HealthOrganisationUID == organisationUID
                                                        && (i.ActiveFrom == null || DbFunctions.TruncateTime(i.ActiveFrom) <= DbFunctions.TruncateTime(dateNow))
                                                        && (i.ActiveTo == null || DbFunctions.TruncateTime(i.ActiveTo) >= DbFunctions.TruncateTime(dateNow))
                                                        select new CareproviderOrganisationModel
                                                        {
                                                            CareproviderOrganisationUID = j.UID,
                                                            HealthOrganisationUID = j.HealthOrganisationUID,
                                                            HealthOrganisationName = SqlFunction.fGetHealthOrganisationName(j.HealthOrganisationUID),
                                                            CareproviderUID = j.CareproviderUID,
                                                            CareproviderName = SqlFunction.fGetCareProviderName(j.CareproviderUID),
                                                            ActiveFrom = j.ActiveFrom,
                                                            ActiveTo = j.ActiveTo
                                                        }).ToList();

            return data;
        }

        [Route("GetCareProviderOrganisationByUser")]
        [HttpGet]
        public List<CareproviderOrganisationModel> GetCareProviderOrganisationByUser(int careproviderUID)
        {
            DateTime dateNow = DateTime.Now.Date;
            List<CareproviderOrganisationModel> data = (from j in db.CareproviderOrganisation
                                                        join i in db.HealthOrganisation on j.HealthOrganisationUID equals i.UID
                                                        where j.StatusFlag == "A"
                                                        && i.StatusFlag == "A"
                                                        && j.CareproviderUID == careproviderUID
                                                        select new CareproviderOrganisationModel
                                                        {
                                                            CareproviderOrganisationUID = j.UID,
                                                            HealthOrganisationUID = j.HealthOrganisationUID,
                                                            HealthOrganisationName = SqlFunction.fGetHealthOrganisationName(j.HealthOrganisationUID),
                                                            CareproviderUID = careproviderUID,
                                                            CareproviderName = SqlFunction.fGetCareProviderName(j.CareproviderUID),
                                                            ActiveFrom = j.ActiveFrom,
                                                            ActiveTo = j.ActiveTo
                                                        }).ToList();

            return data;
        }

        [Route("ManageCareProviderOrganisation")]
        [HttpPost]
        public HttpResponseMessage ManageCareProviderOrganisation(CareproviderOrganisationModel careOrgnModel, int userID)
        {
            try
            {

                DateTime now = DateTime.Now;

                CareproviderOrganisation careOrgan = new CareproviderOrganisation();

                careOrgan.CareproviderUID = careOrgnModel.CareproviderUID;
                careOrgan.HealthOrganisationUID = careOrgnModel.HealthOrganisationUID;
                careOrgan.ActiveFrom = careOrgnModel.ActiveFrom;
                careOrgan.ActiveTo = careOrgnModel.ActiveTo;
                careOrgan.CUser = userID;
                careOrgan.CWhen = now;
                careOrgan.MUser = userID;
                careOrgan.MWhen = now;
                careOrgan.StatusFlag = "A";

                db.CareproviderOrganisation.Add(careOrgan);
                db.SaveChanges();


                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("DeleteCareproviderOrganisation")]
        [HttpDelete]
        public HttpResponseMessage DeleteCareproviderOrganisation(int uid, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                CareproviderOrganisation careOrgn = db.CareproviderOrganisation.Find(uid);
                if (careOrgn != null)
                {
                    db.CareproviderOrganisation.Remove(careOrgn);
                }
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        #region Blife
        [Route("BLIFEVerifyPatientIdentity")]
        [HttpPut]
        public HttpResponseMessage BLIFEVerifyPatientIdentity(int patientUID, string natinalID,int userID)
        {
            try
            {
                var citizenID =  ShareLibrary.Encryption.EncryptBLifeAccess(natinalID);
                var dtBlife = SqlStatement.BLIFEGetUsersByNationalID(citizenID);
                if (dtBlife != null && dtBlife.Rows.Count > 0)
                {
                    var blifeUserUID = int.Parse(dtBlife.Rows[0]["UID"].ToString());
                    var flagVerify = SqlStatement.BLIFEVerifyPatientIdentity(blifeUserUID);

                    if (flagVerify == true)
                    {
                        var patient = db.Patient.Find(patientUID);
                        if (patient != null)
                        {
                            db.Patient.Attach(patient);
                            patient.IsIdentityOnBLIFE = "Y";
                            patient.MWhen = DateTime.Now;
                            patient.MUser = userID;
                            db.SaveChanges();
                        }
                    }

                }


                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }
        #endregion


        [Route("GetCareProviderLocation")]
        [HttpGet]
        public List<CareproviderLocationModel> GetCareProviderLocation(int locationUID)
        {
            DateTime dateNow = DateTime.Now.Date;
            List<CareproviderLocationModel> data = (from j in db.CareproviderLocation
                                                    join i in db.Careprovider on j.CareproviderUID equals i.UID
                                                    where j.StatusFlag == "A"
                                                    && i.StatusFlag == "A"
                                                    && j.LocationUID == locationUID
                                                    && (i.ActiveFrom == null || DbFunctions.TruncateTime(i.ActiveFrom) <= DbFunctions.TruncateTime(dateNow))
                                                    && (i.ActiveTo == null || DbFunctions.TruncateTime(i.ActiveTo) >= DbFunctions.TruncateTime(dateNow))
                                                    select new CareproviderLocationModel
                                                    {
                                                        CareproviderLocationUID = j.UID,
                                                        LocationUID = j.LocationUID,
                                                        LocationName = SqlFunction.fGetLocationName(j.LocationUID),
                                                        HealthOrganisationUID = j.HealthOrganisationUID,
                                                        HealthOrganisationName = SqlFunction.fGetHealthOrganisationName(j.HealthOrganisationUID),
                                                        CareproviderUID = j.CareproviderUID,
                                                        CareproviderName = SqlFunction.fGetCareProviderName(j.CareproviderUID),
                                                        ActiveFrom = j.ActiveFrom,
                                                        ActiveTo = j.ActiveTo
                                                    }).ToList();

            return data;
        }

        [Route("GetCareProviderLocationByUser")]
        [HttpGet]
        public List<CareproviderLocationModel> GetCareProviderLocationByUser(int careproviderUID, int organisationUID)
        {
            DateTime dateNow = DateTime.Now.Date;
            List<CareproviderLocationModel> data = (from j in db.CareproviderLocation
                                                    join i in db.Careprovider on j.CareproviderUID equals i.UID
                                                    where j.StatusFlag == "A"
                                                    && i.StatusFlag == "A"
                                                    && i.UID == careproviderUID
                                                    && j.HealthOrganisationUID == organisationUID
                                                    && (i.ActiveFrom == null || DbFunctions.TruncateTime(i.ActiveFrom) <= DbFunctions.TruncateTime(dateNow))
                                                    && (i.ActiveTo == null || DbFunctions.TruncateTime(i.ActiveTo) >= DbFunctions.TruncateTime(dateNow))
                                                    select new CareproviderLocationModel
                                                    {
                                                        CareproviderLocationUID = j.UID,
                                                        LocationUID = j.LocationUID,
                                                        LocationName = SqlFunction.fGetLocationName(j.LocationUID),
                                                        HealthOrganisationUID = j.HealthOrganisationUID,
                                                        HealthOrganisationName = SqlFunction.fGetHealthOrganisationName(j.HealthOrganisationUID),
                                                        CareproviderUID = j.CareproviderUID,
                                                        CareproviderName = SqlFunction.fGetCareProviderName(j.CareproviderUID),
                                                        ActiveFrom = j.ActiveFrom,
                                                        ActiveTo = j.ActiveTo
                                                    }).ToList();

            return data;
        }

        [Route("ManageCareProviderLocation")]
        [HttpPost]
        public HttpResponseMessage ManageCareProviderLocation(CareproviderLocationModel careLocationModel, int userID)
        {
            try
            {

                DateTime now = DateTime.Now;

                CareproviderLocation careLocation = new CareproviderLocation();

                careLocation.CareproviderUID = careLocationModel.CareproviderUID;
                careLocation.LocationUID = careLocationModel.LocationUID;
                careLocation.HealthOrganisationUID = careLocationModel.HealthOrganisationUID;
                careLocation.ActiveFrom = careLocationModel.ActiveFrom;
                careLocation.ActiveTo = careLocationModel.ActiveTo;
                careLocation.CUser = userID;
                careLocation.CWhen = now;
                careLocation.MUser = userID;
                careLocation.MWhen = now;
                careLocation.StatusFlag = "A";

                db.CareproviderLocation.Add(careLocation);
                db.SaveChanges();


                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("DeleteCareproviderLocation")]
        [HttpDelete]
        public HttpResponseMessage DeleteCareproviderLocation(int uid, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                CareproviderLocation careLoc = db.CareproviderLocation.Find(uid);
                if (careLoc != null)
                {
                    db.CareproviderLocation.Remove(careLoc);
                }
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
