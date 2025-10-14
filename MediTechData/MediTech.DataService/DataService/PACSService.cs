using MediTech.Model;
using ShareLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.DataService
{
    public class PACSService
    {
        public List<StudiesModel> SearchPACSWorkList(DateTime? dateFrom, DateTime? dateTo, string modality, string sex, string patientID, string patientName, string accessionNumber, string studyID, string studyDescription, string bodypart)
        {
            string requestApi = string.Format("Api/PACS/SearchPACSWorkList?&dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&modality={2}&sex={3}&patientID={4}&patientName={5}&accessionNumber={6}&studyID={7}&studyDescription={8}&{9}&bodypart={9}", dateFrom, dateTo, modality, sex, patientID, patientName, accessionNumber, studyID, studyDescription, bodypart);

            List<StudiesModel> dataRequest = PACSApiHelper.Get<List<StudiesModel>>(requestApi);

            return dataRequest;
        }
        public List<StudiesModel> GetStudiesList()
        {
            string requestApi = string.Format("Api/PACS/GetStudiesList");
            List<StudiesModel> dataRequest = PACSApiHelper.Get<List<StudiesModel>>(requestApi);

            return dataRequest;
        }

        public List<ImageSerires> GetImageSeries(string studyInstanceUID, string seriesInstanceUID)
        {
            string requestApi = string.Format("Api/PACS/GetImageSeries?studyInstanceUID={0}&seriesInstanceUID={1}", studyInstanceUID, seriesInstanceUID);
            List<ImageSerires> dataRequest = PACSApiHelper.Get<List<ImageSerires>>(requestApi);

            return dataRequest;
        }
        public List<SeriesModel> GetSeriesByStudyUID(string studyUID)
        {
            string requestApi = string.Format("Api/PACS/GetSeriesByStudyUID?studyUID={0}", studyUID);
            List<SeriesModel> dataRequest = PACSApiHelper.Get<List<SeriesModel>>(requestApi);

            return dataRequest;
        }

        public List<byte[]> GetDicomFiles(string studyInstanceUID, string seriesInstanceUID)
        {
            try
            {
                string requestApi = string.Format("Api/PACS/GetDicomFiles?studyInstanceUID={0}&seriesInstanceUID={1}", studyInstanceUID, seriesInstanceUID);
                List<byte[]> dataRequest = PACSApiHelper.Get<List<byte[]>>(requestApi);

                return dataRequest;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<byte[]> GetDicomFileByPatientID(string patientID, DateTime studyDate, string modality,bool IsSINE, string bodyPartExam = "")
        {
            try
            {
                string requestApi = string.Format("Api/PACS/GetDicomFileByPatientID?patientID={0}&studyDate={1:MM/dd/yyyy}&modality={2}&IsSINE={3}&bodyPartExam={4}", patientID, studyDate, modality, IsSINE, bodyPartExam);
                List<byte[]> dataRequest = PACSApiHelper.Get<List<byte[]>>(requestApi);

                return dataRequest;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<InstancesCopyToModel> GetDicomFilesInstancesList(List<InstancesModel> instancesList)
        {
            try
            {
                string requestApi = string.Format("Api/PACS/GetDicomFilesInstancesList");
                List<InstancesCopyToModel> dataRequest = PACSApiHelper.Post<List<InstancesModel>
                    , List<InstancesCopyToModel>>(requestApi, instancesList);

                return dataRequest;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool ChangeHNDicomData(string newHN, string oldHN)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/PACS/ChangeHNDicomData?newHN={0}&oldHN={1}", newHN, oldHN);
                flag = PACSApiHelper.Post<object, bool>(requestApi, null);
            }
            catch (Exception)
            {

                throw;
            }
            return flag;
        }

        public void EditStudy(StudiesModel studiesModel)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/PACS/EditStudy");
                flag = PACSApiHelper.Post<object, bool>(requestApi, studiesModel);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool SendDicomFile(SendDicomFilesModel sendDicomModel)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/PACS/SendDicomFile");
                flag = PACSApiHelper.Post<SendDicomFilesModel, bool>(requestApi, sendDicomModel);
            }
            catch (Exception)
            {

                throw;
            }
            return flag;
        }

        public bool UpdateStudyDetailsWithAudit(UpdateStudyDetailsRequest request)
        {
            try
            {
                string requestApi = string.Format("Api/PACS/UpdateStudyDetailsWithAudit");
                var result = PACSApiHelper.Post<UpdateStudyDetailsRequest, bool>(requestApi, request);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<StudyAuditLogEntry> GetStudyAuditHistory(string studyInstanceUID)
        {
            try
            {
                string requestApi = string.Format("Api/PACS/GetStudyAuditHistory?studyInstanceUID={0}", studyInstanceUID);
                var result = PACSApiHelper.Get<List<StudyAuditLogEntry>>(requestApi);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<StudyAuditLogEntry> GetAuditReport(DateTime from, DateTime to, int? userId = null)
        {
            try
            {
                string requestApi = userId.HasValue
                    ? string.Format("Api/PACS/GetAuditReport?from={0:MM/dd/yyyy}&to={1:MM/dd/yyyy}&userId={2}", from, to, userId.Value)
                    : string.Format("Api/PACS/GetAuditReport?from={0:MM/dd/yyyy}&to={1:MM/dd/yyyy}", from, to);
                var result = PACSApiHelper.Get<List<StudyAuditLogEntry>>(requestApi);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
