using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MediTech.Models;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class ImportLabResultViewModel : MediTechViewModelBase
    {
        #region Properties

        private List<RequestItemModel> _RequestItems;

        public List<RequestItemModel> RequestItems
        {
            get { return _RequestItems; }
            set { _RequestItems = value; }
        }

        private RequestItemModel _SelectedRequestItem;

        public RequestItemModel SelectedRequestItem
        {
            get { return _SelectedRequestItem; }
            set
            {
                _SelectedRequestItem = value;
                var resultItems = DataService.MasterData.GetRequestResultLinkByRequestItemUID(SelectedRequestItem.RequestItemUID);
                if (resultItems != null)
                {
                    ColumnsResultItems = new ObservableCollection<Column>();
                    ColumnsResultItems.Add(new Column() { Header = "HN" });
                    foreach (var item in resultItems.OrderBy(p => p.PrintOrder))
                    {
                        ColumnsResultItems.Add(new Column() { Header = item.ResultItemName + " " + item.Unit });
                    }
                }

            }
        }

        private ObservableCollection<Column> _ColumnsResultItems;

        public ObservableCollection<Column> ColumnsResultItems
        {
            get { return _ColumnsResultItems; }
            set { Set(ref _ColumnsResultItems, value); }
        }

        #endregion

        #region Command


        #endregion

        #region Method

        public ImportLabResultViewModel()
        {
            RequestItems = DataService.MasterData.GetRequestItemByCategory("Lab");
            RequestItems = RequestItems?.Where(p => p.Code == "LAB111" || p.Code == "LAB311").ToList();
        }

        #endregion
    }
}
