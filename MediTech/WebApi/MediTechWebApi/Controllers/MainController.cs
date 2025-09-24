using MediTech.DataBase;
using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MediTechWebApi.Controllers
{
    [RoutePrefix("Api/Main")]
    public class MainController : ApiController
    {
        MediTechEntities db = new MediTechEntities();

        #region DesktopApp
        [Route("GetPageViewModule")]
        [HttpGet]
        public List<PageViewModuleModel> GetPageViewModule()
        {
            List<PageViewModuleModel> listPageViewModule = db.PageViewModule.Where(p => p.StatusFlag == "A").Select(p =>
            new PageViewModuleModel
            {
                PageViewModuleUID = p.UID,
                ModuleName = p.ModuleName,
                LocalName = p.LocalName,
                DisplayOrder = p.DisplayOrder,
                Image = p.Image,
                Icon = p.Icon,
                ImagePath = p.ImagePath
            }).ToList();

            return listPageViewModule;
        }

        [Route("GetPageView")]
        [HttpGet]
        public List<PageViewModel> GetPageView(int roleUID,string mode)
        {
            int read = db.Permission.Single(p => p.Description == "Read" && p.StatusFlag == "A").UID;
            List<PageViewModel> listPageview = (from ro in db.Role
                                                   join wpr in db.RoleViewPermission
                                                   on ro.UID equals wpr.RoleUID
                                                   join wpp in db.PageViewPermission
                                                   on wpr.PageViewPermissionUID equals wpp.UID
                                                   join wp in db.PageView
                                                   on wpp.PageViewUID equals wp.UID
                                                   where wpp.PermissionUID == read
                                                   && ro.UID == roleUID
                                                   && ro.StatusFlag == "A"
                                                   && wpr.StatusFlag == "A"
                                                   && wpp.StatusFlag == "A"
                                                   && wp.StatusFlag == "A"
                                                  select new PageViewModel
                                                   {
                                                       PageViewModuleUID = wp.PageViewModuleUID,
                                                       ViewCode = wp.ViewCode,
                                                       Name = wp.Name,
                                                       LocalName = wp.LocalName,
                                                       Description = wp.Description,
                                                       DisplayOrder = wp.DisplayOrder,
                                                       Image = wp.Image,
                                                       NamespaceName = wp.NamespaceName,
                                                       ClassName = wp.ClassName,
                                                       ControllerName = wp.ControllerName,
                                                       ControllerAction = wp.ControllerAction,
                                                       Icon = wp.Icon,
                                                       ImagePath = wp.ImagePath,
                                                       Type = wp.Type,
                                                       ParentUID = wp.ParentUID,
                                                       PageViewUID = wp.UID,
                                                       PageViewPermissionUID = wpp.UID
                                                   }).ToList();
            if (mode == "win")
            {
                listPageview = listPageview.Where(p => !String.IsNullOrEmpty(p.ClassName)).ToList();
            }
            else if(mode == "web")
            {
                listPageview = listPageview.Where(p => !String.IsNullOrEmpty(p.ControllerName)).ToList();
            }

            return listPageview;
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