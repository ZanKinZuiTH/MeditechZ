using MediTech.Model;
using ShareLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MediTech.DataService
{
    public class IcheckupService
    {

        public string ichecktest()
        {
            string requestApi = string.Format("/OutBound/ichecktest2");
            string data = IcheckupHelper.Get<string>(requestApi);
            return data;

        }

        public void outbrouondcollrection(IcheckupModel model, long userUID)
        {

            try
            {

                string requestApi = string.Format("/OutBound/OutbroundLabCollection?userUID={0}", userUID);
                IcheckupHelper.Post(requestApi, model);


            }
            catch (Exception)
            {

                throw;
            }

        }


        public void InsertVisitHC(VISIT_HC model, long userUID)
        {

            try
            {

                string requestApi = string.Format("/OutBound/InsertVisitHC?userUID={0}", userUID);
                IcheckupHelper.Post(requestApi, model);


            }
            catch (Exception)
            {

                throw;
            }

        }






        public void UpdateRequestDetailSpecimens(List<RequestDetailSpecimenModel> requestDetailSpecimens, int userID)
        {
            try
            {
                string requestApi = string.Format("Api/Lab/UpdateRequestDetailSpecimens?userID={0}", userID);
                MeditechApiHelper.Post<List<RequestDetailSpecimenModel>>(requestApi, requestDetailSpecimens);
            }
            catch (Exception)
            {

                throw;
            }

        }


    }

}