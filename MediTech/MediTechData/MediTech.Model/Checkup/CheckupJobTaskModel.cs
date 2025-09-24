using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class CheckupJobTaskModel : INotifyPropertyChanged
    {
        public int CheckupJobTaskUID { get; set; }
        public int CheckupJobContactUID { get; set; }
        public int GPRSTUID { get; set; }
        public string GroupResultName { get; set; }
        public int? DisplayOrder { get; set; }
        public int? TempDisplayOrder { get; set; }
        public string ReportTemplate { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }

        private bool _IsSelected;
        public bool IsSelected
        {
            get
            {
                return _IsSelected;
            }
            set
            {
                _IsSelected = value;
                OnPropertyRaised("IsSelected");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyRaised(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }
    }
}
