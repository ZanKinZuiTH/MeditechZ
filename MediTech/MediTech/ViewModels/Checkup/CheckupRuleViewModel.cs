using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class CheckupRuleViewModel : MediTechViewModelBase
    {
        #region Properties

        private List<LookupReferenceValueModel> _GroupResults;

        public List<LookupReferenceValueModel> GroupResults
        {
            get { return _GroupResults; }
            set { Set(ref _GroupResults , value); }
        }

        private LookupReferenceValueModel _SelectGroupResult;

        public LookupReferenceValueModel SelectGroupResult
        {
            get { return _SelectGroupResult; }
            set { Set(ref _SelectGroupResult, value); }
        }

        private List<LookupReferenceValueModel> _Genders;

        public List<LookupReferenceValueModel> Genders
        {
            get { return _Genders; }
            set { Set(ref _Genders, value); }
        }

        private LookupReferenceValueModel _SelectGender;

        public LookupReferenceValueModel SelectGender
        {
            get { return _SelectGender; }
            set { Set(ref _SelectGender, value); }
        }


        private List<LookupReferenceValueModel> _ResultStatus;

        public List<LookupReferenceValueModel> ResultStatus
        {
            get { return _ResultStatus; }
            set { Set(ref _ResultStatus, value); }
        }

        private LookupReferenceValueModel _SelectResultStatus;

        public LookupReferenceValueModel SelectResultStatus
        {
            get { return _SelectResultStatus; }
            set { Set(ref _SelectResultStatus, value); }
        }

        private string _RuleName;

        public string RuleName
        {
            get { return _RuleName; }
            set { Set(ref _RuleName, value); }
        }

        private int? _AgeFrom;

        public int? AgeFrom
        {
            get { return _AgeFrom; }
            set { Set(ref _AgeFrom, value); }
        }

        private int? _AgeTo;

        public int? AgeTo
        {
            get { return _AgeTo; }
            set { Set(ref _AgeTo, value); }
        }
        #endregion

        #region Command

        private RelayCommand _AddRuleCommmand;

        public RelayCommand AddRuleCommmand
        {
            get
            {
                return _AddRuleCommmand
                    ?? (_AddRuleCommmand = new RelayCommand(AddRule));
            }
        }


        private RelayCommand _DeleteRuleCommand;

        public RelayCommand DeleteRuleCommand
        {
            get
            {
                return _DeleteRuleCommand
                    ?? (_DeleteRuleCommand = new RelayCommand(DeleteRule));
            }
        }
        #endregion

        #region Method

        public CheckupRuleViewModel()
        {
            var refValueList = DataService.Technical.GetReferenceValueList("GPRST,SEXXX");
            GroupResults = refValueList.Where(p => p.DomainCode == "GPRST").ToList();
            Genders = refValueList.Where(p => p.DomainCode == "SEXXX").ToList();

        }

        void AddRule()
        {

        }

        void DeleteRule()
        {

        }
        #endregion
    }
}
