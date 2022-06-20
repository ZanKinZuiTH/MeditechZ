using MediTech.Model;
using ShareLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.DataService
{
   public class IPDService
    {
       
        public List<LocationModel> GetBedALL()
        {
            string requestApi = string.Format("Api/IPD/GetBedALL");
            List<LocationModel> listItem = MeditechApiHelper.Get<List<LocationModel>>(requestApi);

            return listItem;
        }

        public void ChangeBedStatus2(LocationModel locationModel, int userID)
        {
            try
            {
                string requestApi = string.Format("Api/IPD/UpdateRequestDetailSpecimens?userID={0}", userID);
                MeditechApiHelper.Post<LocationModel>(requestApi, locationModel);
            }
            catch (Exception)
            {

                throw;
            }

        }


        public bool ChangeBedStatus(LocationModel locationModel, int userID)
        {
            bool flag = false;

            try
            {
                string requestApi = string.Format("Api/IPD/ChangeBedStatus?userID={0}", userID);
                MeditechApiHelper.Post<LocationModel>(requestApi, locationModel);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }


            return flag;

        }



    }
}
