using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class ResultRadiologyTemplateModel
    {
        public virtual int ResultRadiologyTemplateUID { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual System.Nullable<int> DisplayOrder { get; set; }
        public virtual bool IsPrimary { get; set; }
        public virtual System.Nullable<int> SEXXXUID { get; set; }
        public virtual System.Nullable<int> RIMTYPUID { get; set; }
        public virtual System.Nullable<int> RABSTSUID { get; set; }
        public virtual string Gender { get; set; }
        public virtual string ImageType { get; set; }
        public virtual string ResultStatus { get; set; }
        public virtual bool IsPublic { get; set; }

        public virtual string IsChecked
        {
            get
            {
                return IsPrimary ? "checked=\"checked\"" : "";
            }
        }
        public virtual string PlainText { get; set; }
        public virtual string Value { get; set; }
    }
}
