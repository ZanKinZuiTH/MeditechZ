using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class ResultComponentModel : INotifyPropertyChanged
    {
        public long? ResultComponentUID { get; set; }
        public long? ResultUID { get; set; }
        public long? RequestDetailUID { get; set; }
        public Nullable<int> ResultItemUID { get; set; }
        public int RVTYPUID { get; set; }

        public string ResultValueType { get; set; }

        private string _ResultValue;

        public string ResultValue
        {
            get { return _ResultValue; }
            set
            {
                _ResultValue = value; OnPropertyRaised("ResultValue");
                Abnormal();
            }
        }

        private string _ReferenceRange;

        public string ReferenceRange
        {
            get { return _ReferenceRange; }
            set
            {
                _ReferenceRange = value;
                OnPropertyRaised("ReferenceRange");
            }
        }
        public string Comments { get; set; }

        private string _IsAbnormal;

        public string IsAbnormal
        {
            get { return _IsAbnormal; }
            set { _IsAbnormal = value; OnPropertyRaised("IsAbnormal"); }
        }

        private double? _Low;

        public double? Low
        {
            get { return _Low; }
            set
            {
                _Low = value;
                OnPropertyRaised("Low");
                Abnormal();
            }
        }


        private double? _High;

        public double? High
        {
            get { return _High; }
            set
            {
                _High = value;
                OnPropertyRaised("High");
                Abnormal();
            }
        }

        private bool _ShowNumbericRange;

        public bool ShowNumbericRange
        {
            get { return _ShowNumbericRange; }
            set { _ShowNumbericRange = value; OnPropertyRaised("ShowNumbericRange"); }
        }

        private bool _ShowTextRange;

        public bool ShowTextRange
        {
            get { return _ShowTextRange; }
            set { _ShowTextRange = value; OnPropertyRaised("ShowTextRange"); }
        }

        public byte[] ImageContent { get; set; }
        public Nullable<System.DateTime> ResultDTTM { get; set; }
        public string ResultItemName { get; set; }
        public int PrintOrder { get; set; }

        public string IsMandatory { get; set; }
        public string AutoValue { get; set; }

        public List<string> AutoValueList { get; set; }

        public IList<object> CheckDataList { get; set; }
        public IList<object> TokenDataList { get; set; }

        public string ResultItemCode { get; set; }
        public string TestType { get; set; }
        public string UnitofMeasure { get; set; }
        public int? RSUOMUID { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyRaised(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }

        void Abnormal()
        {
            double resultValue;
            if (double.TryParse(ResultValue, out resultValue))
            {
                if (High != null || Low != null)
                {
                    if (resultValue > High)
                    {
                        IsAbnormal = "H";
                    }
                    else if (resultValue < Low)
                    {
                        IsAbnormal = "L";
                    }
                    else
                    {
                        IsAbnormal = null;
                    }
                }
            }
            else
            {
                if (ResultValueType == "Numeric")
                {
                    if (ResultValue != null && ResultValue.Contains("-"))
                    {
                        string[] values = ResultValue.Split('-');
                        if (values.Count() == 2)
                        {
                            if (double.TryParse(values[1], out resultValue))
                            {
                                if (High != null || Low != null)
                                {
                                    if (resultValue > High)
                                    {
                                        IsAbnormal = "H";
                                    }
                                    else if (resultValue < Low)
                                    {
                                        IsAbnormal = "L";
                                    }
                                    else
                                    {
                                        IsAbnormal = null;
                                    }
                                }
                            }
                        }

                    }
                }
            }
        }
    }
}
