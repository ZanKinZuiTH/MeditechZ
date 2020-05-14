using MediTech.Model;
using ShareLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MediTech.DataService
{
    public class PatientHistoryService
    {
        #region PatientVitalSign
        public List<PatientVitalSignModel> SearchPatientVitalSign(long patientUID,DateTime dateFrom, DateTime dateTo)
        {
            string requestApi = string.Format("Api/PatientHistory/SearchPatientVitalSign?patientUID={0}&dateFrom={1:MM/dd/yyyy}&dateTo={2:MM/dd/yyyy}", patientUID,dateFrom, dateTo);
            List<PatientVitalSignModel> dataRequest = MeditechApiHelper.Get<List<PatientVitalSignModel>>(requestApi);

            return dataRequest;
        }

        public List<PatientVitalSignModel> GetPatientVitalSignByUID(long patientVitalSignUID)
        {
            string requestApi = string.Format("Api/PatientHistory/GetPatientVitalSignByUID?patientVitalSignUID={0}", patientVitalSignUID);
            List<PatientVitalSignModel> dataRequest = MeditechApiHelper.Get<List<PatientVitalSignModel>>(requestApi);

            return dataRequest;
        }


        public List<PatientVitalSignModel> GetPatientVitalSignByVisitUID(long patientVisitUID)
        {
            string requestApi = string.Format("Api/PatientHistory/GetPatientVitalSignByVisitUID?patientVisitUID={0}", patientVisitUID);
            List<PatientVitalSignModel> dataRequest = MeditechApiHelper.Get<List<PatientVitalSignModel>>(requestApi);

            return dataRequest;
        }


        public bool ManagePatientVitalSign(PatientVitalSignModel model, int userID)
        {
            bool flag = false;

            try
            {
                string requestApi = string.Format("Api/PatientHistory/ManagePatientVitalSign?userID={0}", userID);
                MeditechApiHelper.Post<PatientVitalSignModel>(requestApi, model);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }


            return flag;
        }

        public bool DeletePatientVitalSign(int patientVitalSignUID, int userID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/PatientHistory/DeletePatientVitalSign?patientVitalSignUID={0}&userID={1}", patientVitalSignUID, userID);
                MeditechApiHelper.Delete(requestApi);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }


            return flag;
        }

        #endregion

        #region CCHPI

        public List<CCHPIMasterModel> GetCCHPIMaster(string type)
        {
            string requestApi = string.Format("Api/PatientHistory/GetCCHPIMaster?type={0}", type);
            List<CCHPIMasterModel> dataRequest = MeditechApiHelper.Get<List<CCHPIMasterModel>>(requestApi);

            return dataRequest;
        }

        public List<CCHPIModel> GetCCHPIByVisit(long patientVisitUID)
        {
            string requestApi = string.Format("Api/PatientHistory/GetCCHPIByVisit?patientVisitUID={0}", patientVisitUID);
            List<CCHPIModel> dataRequest = MeditechApiHelper.Get<List<CCHPIModel>>(requestApi);

            return dataRequest;
        }

        public List<CCHPIModel> GetCCHPIByPatientUID(long patientUID)
        {
            string requestApi = string.Format("Api/PatientHistory/GetCCHPIByPatientUID?patientUID={0}", patientUID);
            List<CCHPIModel> dataRequest = MeditechApiHelper.Get<List<CCHPIModel>>(requestApi);

            return dataRequest;
        }

        public bool ManageCCHPI(CCHPIModel model, int userID)
        {
            bool flag = false;

            try
            {
                string requestApi = string.Format("Api/PatientHistory/ManageCCHPI?userID={0}", userID);
                MeditechApiHelper.Post<CCHPIModel>(requestApi, model);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }


            return flag;
        }

        public bool ManageCCHPIMaster(CCHPIMasterModel model, int userID)
        {
            bool flag = false;

            try
            {
                string requestApi = string.Format("Api/PatientHistory/ManageCCHPIMaster?userID={0}", userID);
                MeditechApiHelper.Post<CCHPIMasterModel>(requestApi, model);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }


            return flag;
        }

        public bool DeleteCCHPIMaster(int CCHPIMasterUID, int userID)
        {
            bool flag = false;

            try
            {
                string requestApi = string.Format("Api/PatientHistory/DeleteCCHPIMaster?CCHPIMasterUID={0}&userID={1}", CCHPIMasterUID, userID);
                MeditechApiHelper.Delete(requestApi);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }

            return flag;

        }
        #endregion

        #region PhysicalExam
        public List<PhysicalExamTemplateModel> GetPhysicalExamTemplate()
        {
            string requestApi = string.Format("Api/PatientHistory/GetPhysicalExamTemplate");
            List<PhysicalExamTemplateModel> dataRequest = MeditechApiHelper.Get<List<PhysicalExamTemplateModel>>(requestApi);

            return dataRequest;
        }

        public List<PhysicalExamModel> GetPhysicalExamByPatientUID(long patientUID)
        {
            string requestApi = string.Format("Api/PatientHistory/GetPhysicalExamByPatientUID?patientUID={0}", patientUID);
            List<PhysicalExamModel> dataRequest = MeditechApiHelper.Get<List<PhysicalExamModel>>(requestApi);

            return dataRequest;
        }
        public List<PhysicalExamModel> GetPhysicalExamByVisit(long patientVisitUID)
        {
            string requestApi = string.Format("Api/PatientHistory/GetPhysicalExamByVisit?patientVisitUID={0}", patientVisitUID);
            List<PhysicalExamModel> dataRequest = MeditechApiHelper.Get<List<PhysicalExamModel>>(requestApi);

            return dataRequest;
        }

        public bool ManagePhysicalExam(PhysicalExamModel model, int userID)
        {
            bool flag = false;

            try
            {
                string requestApi = string.Format("Api/PatientHistory/ManagePhysicalExam?userID={0}", userID);
                MeditechApiHelper.Post<PhysicalExamModel>(requestApi, model);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }


            return flag;
        }

        public bool ManagePhysicalExamTemplate(PhysicalExamTemplateModel model, int userID)
        {
            bool flag = false;

            try
            {
                string requestApi = string.Format("Api/PatientHistory/ManagePhysicalExamTemplate?userID={0}", userID);
                MeditechApiHelper.Post<PhysicalExamTemplateModel>(requestApi, model);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }


            return flag;
        }
        public bool DeletePhysicalExamTemplate(int physicalExamTemplateUID, int userID)
        {
            bool flag = false;

            try
            {
                string requestApi = string.Format("Api/PatientHistory/DeletePhysicalExamTemplate?physicalExamTemplateUID={0}&userID={1}", physicalExamTemplateUID, userID);
                MeditechApiHelper.Delete(requestApi);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }

            return flag;

        }
        #endregion

        #region ProgressNote

        public List<ProgressNoteModel> GetProgressNoteByPatientUID(long patientUID)
        {
            string requestApi = string.Format("Api/PatientHistory/GetProgressNoteByPatientUID?patientUID={0}", patientUID);
            List<ProgressNoteModel> dataRequest = MeditechApiHelper.Get<List<ProgressNoteModel>>(requestApi);

            return dataRequest;
        }

        public List<ProgressNoteModel> GetProgressNoteByVisit(long patientVisitUID)
        {
            string requestApi = string.Format("Api/PatientHistory/GetProgressNoteByVisit?patientVisitUID={0}", patientVisitUID);
            List<ProgressNoteModel> dataRequest = MeditechApiHelper.Get<List<ProgressNoteModel>>(requestApi);

            return dataRequest;
        }

        public ProgressNoteModel GetProgressNoteByUID(long progressNoteUID)
        {
            string requestApi = string.Format("Api/PatientHistory/GetProgressNoteByUID?progressNoteUID={0}", progressNoteUID);
            ProgressNoteModel dataRequest = MeditechApiHelper.Get<ProgressNoteModel>(requestApi);

            return dataRequest;
        }

        public bool ManageProgressNote(ProgressNoteModel model, int userID)
        {
            bool flag = false;

            try
            {
                string requestApi = string.Format("Api/PatientHistory/ManageProgressNote?userID={0}", userID);
                MeditechApiHelper.Post<ProgressNoteModel>(requestApi, model);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }


            return flag;

        }

        public void DeleteProgressNote(int proGressNoteUIUD, int userID)
        {
            try
            {
                string requestApi = string.Format("Api/PatientHistory/DeleteProgressNote?proGressNoteUIUD={0}&userID={1}", proGressNoteUIUD, userID);
                MeditechApiHelper.Delete(requestApi);
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion


        #region WellnessData

        public List<WellnessDataModel> GetWellnessDataByPatient(long patientUID)
        {
            string requestApi = string.Format("Api/PatientHistory/GetWellnessDataByPatient?patientUID={0}", patientUID);
            List<WellnessDataModel> dataRequest = MeditechApiHelper.Get<List<WellnessDataModel>>(requestApi);

            return dataRequest;
        }

        public List<WellnessDataModel> GetWellnessDataByVisit(long patientVisitUID)
        {
            string requestApi = string.Format("Api/PatientHistory/GetWellnessDataByVisit?patientVisitUID={0}", patientVisitUID);
            List<WellnessDataModel> dataRequest = MeditechApiHelper.Get<List<WellnessDataModel>>(requestApi);

            return dataRequest;
        }

        public WellnessDataModel GetWellnessDataByUID(long wellnessDataUID)
        {
            string requestApi = string.Format("Api/PatientHistory/GetWellnessDataByUID?wellnessDataUID={0}", wellnessDataUID);
            WellnessDataModel dataRequest = MeditechApiHelper.Get<WellnessDataModel>(requestApi);

            return dataRequest;
        }

        public void ManageWellnessData(WellnessDataModel model, int userID)
        {
            try
            {
                string requestApi = string.Format("Api/PatientHistory/ManageWellnessData?userID={0}", userID);
                MeditechApiHelper.Post<WellnessDataModel>(requestApi, model);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void DeleteWellnessData(int wellNessUID, int userID)
        {
            try
            {
                string requestApi = string.Format("Api/PatientHistory/DeleteWellnessData?wellNessUID={0}&userID={1}", wellNessUID, userID);
                MeditechApiHelper.Delete(requestApi);
            }
            catch (Exception)
            {

                throw;
            }

        }

        #endregion
    }
}
