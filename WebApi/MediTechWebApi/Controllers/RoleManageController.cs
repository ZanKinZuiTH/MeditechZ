using MediTech.DataBase;
using MediTech.Model;
using System;
using System.Data;
using System.Data.Entity.Migrations;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web.Http;

namespace MediTechWebApi.Controllers
{
    [RoutePrefix("Api/RoleManage")]
    public class RoleManageController : ApiController
    {
        protected MediTechEntities db = new MediTechEntities();

        [Route("GetLookUpRoleAll")]
        [HttpGet]
        public List<LookupItemModel> GetLookUpRoleAll()
        {
            List<LookupItemModel> data = db.Role.Where(p => p.StatusFlag == "A").Select(p => new LookupItemModel
            {
                Key = p.UID,
                Display = p.Name
            }).ToList();

            return data;
        }

        [Route("GetRole")]
        [HttpGet]
        public List<RoleModel> GetRole()
        {
            List<RoleModel> data = db.Role.Where(p => p.StatusFlag == "A").Select(p => new RoleModel
            {
                RoleUID = p.UID,
                Name = p.Name,
                Description = p.Description,
                CUser = p.CUser,
                CWhen = p.CWhen,
                MUser = p.MUser,
                MWhen = p.MWhen,
                StatusFlag = p.StatusFlag
            }).ToList();

            return data;
        }

        [Route("GetRoleByUID")]
        [HttpGet]
        public RoleModel GetRoleByUID(int roleUID)
        {
            RoleModel data = (from j in db.Role
                              where j.UID == roleUID
                              select new RoleModel
                                 {
                                     RoleUID = j.UID,
                                     Name = j.Name,
                                     Description = j.Description,
                                     CUser = j.CUser,
                                     CWhen = j.CWhen,
                                     MUser = j.MUser,
                                     MWhen = j.MWhen,
                                     StatusFlag = j.StatusFlag,
                                 }).FirstOrDefault();


            return data;
        }

        [Route("ManageRole")]
        [HttpPost]
        public HttpResponseMessage ManageRole(RoleModel roleData, int userID)
        {
            try
            {
                DateTime now = DateTime.Now;
                using (var tran = new TransactionScope())
                {
                    Role role = db.Role.Find(roleData.RoleUID);
                    if (role == null)
                    {
                        role = new Role();
                        role.CUser = userID;
                        role.CWhen = now;
                    }

                    role.Name = roleData.Name;
                    role.Description = roleData.Description;
                    role.MUser = userID;
                    role.MWhen = now;
                    role.StatusFlag = "A";

                    db.Role.AddOrUpdate(role);

                    db.SaveChanges();

                    List<RoleViewPermission> rolePermiss = db.RoleViewPermission.Where(p => p.StatusFlag == "A" && p.RoleUID == role.UID).ToList();

                    db.RoleViewPermission.RemoveRange(rolePermiss);

                    if (roleData.RoleView != null)
                    {
                        foreach (var item in roleData.RoleView)
                        {
                            RoleViewPermission roleViewPer = new RoleViewPermission();
                            roleViewPer.RoleUID = role.UID;
                            roleViewPer.PageViewPermissionUID = item.PageViewPermissionUID;
                            roleViewPer.CUser = userID;
                            roleViewPer.CWhen = now;
                            roleViewPer.MUser = userID;
                            roleViewPer.MWhen = now;
                            roleViewPer.StatusFlag = "A";
                            db.RoleViewPermission.Add(roleViewPer);
                        }
                    }


                    db.SaveChanges();


                    List<RoleReportPermission> roleRPTPermiss = db.RoleReportPermission.Where(p => p.StatusFlag == "A" && p.RoleUID == role.UID).ToList();

                    db.RoleReportPermission.RemoveRange(roleRPTPermiss);

                    if (roleData.RoleReport != null)
                    {
                        foreach (var item in roleData.RoleReport)
                        {
                            RoleReportPermission roleRPTPer = new RoleReportPermission();
                            roleRPTPer.RoleUID = role.UID;
                            roleRPTPer.ReportPermissionUID = item.ReportPermissionUID;
                            roleRPTPer.CUser = userID;
                            roleRPTPer.CWhen = now;
                            roleRPTPer.MUser = userID;
                            roleRPTPer.MWhen = now;
                            roleRPTPer.StatusFlag = "A";
                            db.RoleReportPermission.Add(roleRPTPer);
                        }
                    }


                    db.SaveChanges();

                    tran.Complete();
                }


                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("DeleteRole")]
        [HttpDelete]
        public HttpResponseMessage DeleteRole(int roleUID, int userID)
        {
            try
            {
                using (var tran = new TransactionScope())
                {
                    Role role = db.Role.Find(roleUID);
                    if (role != null)
                    {
                        db.Role.Attach(role);
                        role.StatusFlag = "D";
                        role.MUser = userID;
                        role.MWhen = DateTime.Now;
                    }

                    List<RoleProfile> roleProfile = db.RoleProfile.Where(p => p.RoleUID == roleUID).ToList();
                    if (roleProfile != null)
                    {
                        foreach (var item in roleProfile)
                        {
                            db.RoleProfile.Attach(item);
                            item.StatusFlag = "D";
                            item.MUser = userID;
                            item.MWhen = DateTime.Now;
                        }

                    }

                    List<RoleViewPermission> rolePermiss = db.RoleViewPermission.Where(p => p.RoleUID == roleUID).ToList();
                    if (rolePermiss != null)
                    {
                        foreach (var item in rolePermiss)
                        {
                            db.RoleViewPermission.Attach(item);
                            item.StatusFlag = "D";
                            item.MUser = userID;
                            item.MWhen = DateTime.Now;
                        }

                    }

                    db.SaveChanges();

                    tran.Complete();
                }


                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        #region Pageview
        [Route("GetPageViewByPermission")]
        [HttpGet]
        public List<PageViewModel> GetPageViewByPermission(string permission)
        {
            int permissionUID = db.Permission.FirstOrDefault(p => p.Description.ToLower() == permission.ToLower()).UID;
            List<PageViewModel> data = (from p in db.PageView
                                        join pvp in db.PageViewPermission on p.UID equals pvp.PageViewUID
                                        where pvp.PermissionUID == permissionUID
                                          && pvp.StatusFlag == "A"
                                          && p.StatusFlag == "A"
                                        select new PageViewModel
                                        {
                                            PageViewModuleUID = p.PageViewModuleUID,
                                            PageViewPermissionUID = pvp.UID,
                                            ClassName = p.ClassName,
                                            NamespaceName = p.NamespaceName,
                                            Description = p.Description,
                                            DisplayOrder = p.DisplayOrder,
                                            Type = p.Type,
                                            Image = p.Image,
                                            ControllerAction = p.ControllerAction,
                                            ControllerName = p.ControllerName,
                                            Name = p.Name,
                                            LocalName = p.LocalName,
                                            PageViewUID = p.UID
                                        }).ToList();

            return data;
        }

        [Route("GetPageViewPermission")]
        [HttpGet]
        public List<SecurityPermissionModel> GetPageViewPermission(int roleUID)
        {
            List<SecurityPermissionModel> data = (from RO in db.Role
                                                  join ROPV in db.RoleViewPermission on RO.UID equals ROPV.RoleUID
                                                  join PVP in db.PageViewPermission on ROPV.PageViewPermissionUID equals PVP.UID
                                                  join PV in db.PageView on PVP.PageViewUID equals PV.UID
                                                  join PER in db.Permission on PVP.PermissionUID equals PER.UID
                                                  where RO.UID == roleUID
                                                  select new SecurityPermissionModel
                                                  {
                                                      PageViewName = PV.Name,
                                                      PageViewUID = PV.UID,
                                                      LocalName = PV.LocalName,
                                                      NameSpace = PV.NamespaceName,
                                                      ClassName = PV.ClassName,
                                                      ControllerAction = PV.ControllerAction,
                                                      ControllerName = PV.ControllerName,
                                                      Permission = PER.Description,
                                                      PermissionUID = PER.UID,
                                                      PageViewPermissionUID = PVP.UID,
                                                      RoleUID = RO.UID
                                                  }).ToList();

            return data;
        }

        #endregion

        #region RoleReportTemplate

        [Route("GetReportTemplateByPermission")]
        [HttpGet]
        public List<RoleReportPermissionModel> GetReportTemplateByPermission(string permission)
        {
            int permissionUID = db.Permission.FirstOrDefault(p => p.Description.ToLower() == permission.ToLower()).UID;
            List<RoleReportPermissionModel> data = (from i in db.Reports
                                                    join j in db.ReportPermission on i.UID equals j.ReportsUID
                                                    where i.StatusFlag == "A"
                                                    && j.StatusFlag == "A"
                                                    && j.PermissionUID == permissionUID
                                                    select new RoleReportPermissionModel
                                                    {
                                                        ReportsUID = i.UID,
                                                        ReportPermissionUID = j.UID,
                                                        Description = i.Description,
                                                        Name = i.Name,
                                                        NamespaceName = i.NamespaceName,
                                                        ReportGroup = i.ReportGroup,
                                                        ViewCode = i.ViewCode
                                                    }).ToList();

            return data;
        }

        [Route("GetReportTemplatePermission")]
        [HttpGet]
        public List<RoleReportPermissionModel> GetReportTemplatePermission(int roleUID)
        {
            List<RoleReportPermissionModel> data = (from RO in db.Role
                                                  join ROPT in db.RoleReportPermission on RO.UID equals ROPT.RoleUID
                                                  join RPP in db.ReportPermission on ROPT.ReportPermissionUID equals RPP.UID
                                                  join RPT in db.Reports on RPP.ReportsUID equals RPT.UID
                                                  join PER in db.Permission on RPP.PermissionUID equals PER.UID
                                                  where RO.UID == roleUID
                                                  select new RoleReportPermissionModel
                                                  {
                                                      ReportsUID = RPT.UID,
                                                      ReportPermissionUID = ROPT.UID,
                                                      Description = RPT.Description,
                                                      Name = RPT.Name,
                                                      NamespaceName = RPT.NamespaceName,
                                                      ReportGroup = RPT.ReportGroup,
                                                      ViewCode = RPT.ViewCode,
                                                      RoleUID = RO.UID
                                                  }).ToList();

            return data;
        }

        [Route("GetReportGroupPermission")]
        [HttpGet]
        public List<RoleReportPermissionModel> GetReportGroupPermission(int roleUID,string reportGroup)
        {
            List<RoleReportPermissionModel> data = (from RO in db.Role
                                                    join ROPT in db.RoleReportPermission on RO.UID equals ROPT.RoleUID
                                                    join RPP in db.ReportPermission on ROPT.ReportPermissionUID equals RPP.UID
                                                    join RPT in db.Reports on RPP.ReportsUID equals RPT.UID
                                                    join PER in db.Permission on RPP.PermissionUID equals PER.UID
                                                    where RO.UID == roleUID
                                                    && RPT.ReportGroup == reportGroup
                                                    select new RoleReportPermissionModel
                                                    {
                                                        ReportsUID = RPT.UID,
                                                        ReportPermissionUID = ROPT.UID,
                                                        Description = RPT.Description,
                                                        Name = RPT.Name,
                                                        NamespaceName = RPT.NamespaceName,
                                                        ReportGroup = RPT.ReportGroup,
                                                        ViewCode = RPT.ViewCode,
                                                        RoleUID = RO.UID
                                                    }).ToList();

            return data;
        }

        #endregion


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
