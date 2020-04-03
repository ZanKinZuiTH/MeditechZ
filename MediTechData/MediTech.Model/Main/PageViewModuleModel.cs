using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class PageViewModuleModel
    {
        public virtual int PageViewModuleUID { get; set; }
        public virtual string ModuleName { get; set; }
        public string LocalName { get; set; }
        public virtual Nullable<int> DisplayOrder { get; set; }
        public virtual int CUser { get; set; }
        public virtual string Image { get; set; }
        public virtual string Icon { get; set; }
        public virtual string ImagePath { get; set; }
        public virtual System.DateTime CWhen { get; set; }
        public virtual int MUser { get; set; }
        public virtual System.DateTime MWhen { get; set; }
        public virtual string StatusFlag { get; set; }
        public virtual ObservableCollection<PageViewModel> PageViews { get; set; }

        public virtual bool? IsChecked { get; set; }

    }
}
