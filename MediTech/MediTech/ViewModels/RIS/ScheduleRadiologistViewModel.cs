using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class ScheduleRadiologistViewModel : MediTechViewModelBase
    {
        #region Properties

        private ObservableCollection<ScheduleRadiologistModel> _RadiologistSchedules;

        public ObservableCollection<ScheduleRadiologistModel> RadiologistSchedules
        {
            get { return _RadiologistSchedules; }
            set { Set(ref _RadiologistSchedules, value); }
        }
        #endregion

        #region Command

        #endregion

        #region Method

        public ScheduleRadiologistViewModel()
        {
           
        }

        public void RenderScheduleRadiologist(DateTime startDate,DateTime endDate)
        {
            RadiologistSchedules = new ObservableCollection<ScheduleRadiologistModel>(DataService.Radiology.GetScheduleRadiologist(startDate, endDate));
        }

        #endregion
    }
}
