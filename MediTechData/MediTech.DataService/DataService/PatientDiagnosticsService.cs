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
    public class PatientDiagnosticsService
    {

        public List<ProblemModel> GetProblemAll()
        {
            string requestApi = string.Format("Api/PatientDiagnosis/GetProblemAll");
            List<ProblemModel> dataRequest = MeditechApiHelper.Get<List<ProblemModel>>(requestApi);

            return dataRequest;
        }

        public List<ProblemModel> SearchProblem(string problemDesc)
        {
            string requestApi = string.Format("Api/PatientDiagnosis/SearchProblem?problemDesc={0}", problemDesc);
            List<ProblemModel> dataRequest = MeditechApiHelper.Get<List<ProblemModel>>(requestApi);

            return dataRequest;
        }

        public ProblemModel GetProblemByUID(int problemUID)
        {
            string requestApi = string.Format("Api/PatientDiagnosis/GetProblemByUID?problemUID={0}", problemUID);
            ProblemModel dataRequest = MeditechApiHelper.Get<ProblemModel>(requestApi);

            return dataRequest;
        }

        public List<FavouriteItemModel> GetFavouriteItemByUser(int userUID)
        {
            string requestApi = string.Format("Api/PatientDiagnosis/GetFavouriteItemByUser?userUID={0}", userUID);
            List<FavouriteItemModel> dataRequest = MeditechApiHelper.Get<List<FavouriteItemModel>>(requestApi);

            return dataRequest;
        }

        public List<FavouriteItemModel> SearchFavouriteItem(string problemDesc,int userUID)
        {
            string requestApi = string.Format("Api/PatientDiagnosis/SearchFavouriteItem?problemDesc={0}&userUID={1}", problemDesc, userUID);
            List<FavouriteItemModel> dataRequest = MeditechApiHelper.Get<List<FavouriteItemModel>>(requestApi);

            return dataRequest;
        }

        public int? AddFavouriteItem(FavouriteItemModel model, int userUID)
        {
            int? favouriteItem;
            try
            {
                string requestApi = string.Format("Api/PatientDiagnosis/AddFavouriteItem?userUID={0}", userUID);
                favouriteItem = MeditechApiHelper.Post<FavouriteItemModel,int?>(requestApi, model);
            }
            catch (Exception)
            {
                throw;
            }
            return favouriteItem;
        }

        public bool DeleteFavouriteItem(int favouriteItemUID,int userUID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/PatientDiagnosis/DeleteFavouriteItem?favouriteItemUID={0}&userUID={1}", favouriteItemUID, userUID);
                MeditechApiHelper.Delete(requestApi);
                flag = true;
            }
            catch (Exception)
            {
                throw;
            }
            return flag;
        }



        public List<PatientProblemModel> GetPatientProblemByVisitUID(long patientVisitUID)
        {
            string requestApi = string.Format("Api/PatientDiagnosis/GetPatientProblemByVisitUID?patientVisitUID={0}", patientVisitUID);
            List<PatientProblemModel> dataRequest = MeditechApiHelper.Get<List<PatientProblemModel>>(requestApi);

            return dataRequest;
        }

        public List<PatientProblemModel> GetPatientProblemByPatientUID(long patientUID)
        {
            string requestApi = string.Format("Api/PatientDiagnosis/GetPatientProblemByPatientUID?patientUID={0}", patientUID);
            List<PatientProblemModel> dataRequest = MeditechApiHelper.Get<List<PatientProblemModel>>(requestApi);

            return dataRequest;
        }

        public bool ManagePatientProblem(List<PatientProblemModel> model, long patientVisitUID, int userUID)
        {
             bool flag = false;
             try
             {
                 string requestApi = string.Format("Api/PatientDiagnosis/ManagePatientProblem?patientVisitUID={0}&userUID={1}", patientVisitUID, userUID);
                 MeditechApiHelper.Post<List<PatientProblemModel>>(requestApi, model);
                 flag = true;
             }
             catch (Exception)
             {
                 throw;
             }
             return flag;
         }

        public bool ManagePatientProblemMass(List<PatientProblemModel> model, int userUID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/PatientDiagnosis/ManagePatientProblemMass?userUID={0}", userUID);
                MeditechApiHelper.Post<List<PatientProblemModel>>(requestApi, model);
                flag = true;
            }
            catch (Exception)
            {
                throw;
            }
            return flag;
        }

        public bool DeletePatientProblem(int patientProblemUID,int userUID)
         {
             bool flag = false;
             try
             {
                 string requestApi = string.Format("Api/PatientDiagnosis/DeletePatientProblem?patientProblemUID={0}&userUID={1}", patientProblemUID, userUID);
                 MeditechApiHelper.Delete(requestApi);
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
