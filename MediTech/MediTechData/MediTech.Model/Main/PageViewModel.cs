using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class PageViewModel
    {
        public virtual int PageViewUID { get; set; }
        public virtual Nullable<int> PageViewModuleUID { get; set; }
        public virtual Nullable<int> PageViewPermissionUID { get; set; }
        public virtual string ViewCode { get; set; }
        public virtual string Name { get; set; }
        public string LocalName { get; set; }
        public virtual string Description { get; set; }
        public virtual Nullable<int> DisplayOrder { get; set; }
        public virtual string NamespaceName { get; set; }
        public virtual string ClassName { get; set; }
        public virtual string Type { get; set; }
        public virtual Nullable<int> ParentUID { get; set; }
        public virtual string Image { get; set; }
        public string ControllerName { get; set; }
        public string ControllerAction { get; set; }
        public string Icon { get; set; }
        public string ImagePath { get; set; }
        public virtual int CUser { get; set; }
        public virtual System.DateTime CWhen { get; set; }
        public virtual int MUser { get; set; }
        public virtual System.DateTime MWhen { get; set; }
        public virtual string StatusFlag { get; set; }
        public virtual bool? IsChecked { get; set; }
    }
}
