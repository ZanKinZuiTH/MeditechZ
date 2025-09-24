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
    public class InPatientService
    {

        public void InsertPatientConsult(List<IPDConsultModel> models,int userUID)
        {
            try
            {

                string requestApi = string.Format("Api/InPatient/InsertPatientConsult?userUID={0}", userUID);
                MeditechApiHelper.Post<List<IPDConsultModel>>(requestApi, models);
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

        public bool InsertIPDConsult(IPDConsultModel currentModel, int userID)
        {
            bool flag = false;

            try
            {
                string requestApi = string.Format("Api/InPatietn/ChangeBedStatus?userID={0}", userID);
                MeditechApiHelper.Post<IPDConsultModel>(requestApi, currentModel);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }


            return flag;

        }


        public List<IPDConsultModel> GetPatientVisitConsult(long patientvisitUID)
        {

            string requestApi = string.Format("Api/InPatient/GetPatientVisitConsult?patientVisitUID={0}", patientvisitUID);
            List<IPDConsultModel> data = MeditechApiHelper.Get<List<IPDConsultModel>>(requestApi);
            return data;

        }


        public List<LocationModel> GetBedALL()
        {
            string requestApi = string.Format("Api/InPatient/GetBedALL");
            List<LocationModel> listItem = MeditechApiHelper.Get<List<LocationModel>>(requestApi);

            return listItem;
        }



    }
}
