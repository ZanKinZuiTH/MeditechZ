using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class ReviewHistoryViewModel : MediTechViewModelBase
    {
        #region Properties

        public long RequestDetailUID { get; set; }

        private List<ResultRadiologyHistoryModel> _ResultHistory;

        public List<ResultRadiologyHistoryModel> ResultHistory
        {
            get { return _ResultHistory; }
            set { Set(ref _ResultHistory, value); }
        }

        private ResultRadiologyHistoryModel _SelectResultHistory;

        public ResultRadiologyHistoryModel SelectResultHistory
        {
            get { return _SelectResultHistory; }
            set
            {
                _SelectResultHistory = value;
                if (_SelectResultHistory != null)
                {
                    ReviewResult = SelectResultHistory.ResultValue;
                }
            }
        }

        private string _ReviewResult;

        public string ReviewResult
        {
            get { return _ReviewResult; }
            set { Set(ref _ReviewResult, value); }
        }


        #endregion

        #region Method

        public override void OnLoaded()
        {
            ResultHistory = DataService.Radiology.GetResultHistoryByRequestDetail(RequestDetailUID);
        }

        #endregion
    }
}
