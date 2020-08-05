using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ShareLibrary;
using Dicom.Imaging;
using System.IO;
using Dicom;
using MediTech.Model;
using System.Threading;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.Runtime.ConstrainedExecution;
using System.Security;
using System.Security.Principal;
using PACSWebApi.Helpter;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Migrations;
using PACS.DataBase;

namespace MediTechWebApi.Controllers
{

    [RoutePrefix("Api/PACS")]
    public class PACSController : ApiController
    {
        protected PACSEntities dbPACS = new PACSEntities();
        string connectionString;
        string dicomPath;
        SqlConnection conn;
        void Connect()
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();
        }
        void Disconnect()
        {
            if (conn.State == ConnectionState.Open)
                conn.Close();
        }
        public PACSController()
        {
            connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["PACSEntities"].ConnectionString;
            EntityConnectionStringBuilder entityConnect = new EntityConnectionStringBuilder(connectionString);
            SqlConnectionStringBuilder sqlConnect = new SqlConnectionStringBuilder(entityConnect.ProviderConnectionString);
            conn = new SqlConnection(sqlConnect.ConnectionString);
            dicomPath = System.Configuration.ConfigurationManager.AppSettings["DICOMPath"].ToString();
        }

        [Route("SearchPACSWorkList")]
        [HttpGet]
        public List<StudiesModel> SearchPACSWorkList(DateTime? dateFrom, DateTime? dateTo, string modality, string sex, string patientID, string patientName, string accessionNumber, string studyID, string studyDescription, string bodypart)
        {
            List<StudiesModel> studiesList = null;
            DataTable dt = new DataTable();
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;

                string coditionString = "";

                if (dateFrom != null && dateFrom.Value != DateTime.MinValue)
                {
                    coditionString += " and Convert(Date,sty.StudyDate) >= Convert(Date,@DateFrom)";
                    cmd.Parameters.AddWithValue("@DateFrom", dateFrom);
                }

                if (dateTo != null && dateTo.Value != DateTime.MinValue)
                {
                    coditionString += " and Convert(Date,sty.StudyDate) <= Convert(Date,@DateTo)";
                    cmd.Parameters.AddWithValue("@DateTo", dateTo);
                }
                if (!string.IsNullOrEmpty(modality))
                {
                    coditionString += " and sty.ModalitiesInStudy IN (" + modality + ")";
                }
                if (!string.IsNullOrEmpty(sex))
                {
                    if (sex == "Male")
                    {
                        coditionString += " and sty.PatientSex = 'M'";
                    }
                    else if (sex == "Female")
                    {
                        coditionString += " and sty.PatientSex = 'F'";
                    }
                }

                if (!string.IsNullOrEmpty(patientID))
                {
                    coditionString += " and sty.PatientID = '" + patientID + "'";
                }
                if (!string.IsNullOrEmpty(patientName))
                {
                    coditionString += " and sty.PatientName like N'%" + patientName + "%'";
                }

                if (!string.IsNullOrEmpty(accessionNumber))
                {
                    coditionString += " and sty.AccessionNumber = '" + accessionNumber + "'";
                }

                if (!string.IsNullOrEmpty(studyID))
                {
                    coditionString += " and sty.StudyID = '" + studyID + "'";
                }

                if (!string.IsNullOrEmpty(studyDescription))
                {
                    coditionString += " and sty.StudyDescription like '%" + studyDescription + "%'";
                }

                if (!string.IsNullOrEmpty(bodypart))
                {
                    coditionString += " and sty.BodyPartsInStudy like '%" + bodypart + "%'";
                }
                cmd.CommandText = @"SELECT Distinct TOP 1000 sty.[StudyInstanceUID]
      ,sty.[PatientID]
      ,sty.[PatientName]
      ,sty.[PatientComments]
      ,sty.[PatientBirthDate]
      ,sty.[PatientSex]
      ,sty.[StudyDescription]
      ,sty.[StudyID]
      ,Convert(Date,sty.StudyDate) StudyDate
      ,TRY_CONVERT(TIME,sty.StudyTime) StudyTime
      ,sty.[AccessionNumber]
      ,sty.[InstitutionName]
      ,sty.[ModalitiesInStudy]
      ,sty.[BodyPartsInStudy]
      ,sty.[NumberOfStudyRelatedSeries]
      ,sty.[NumberOfStudyRelatedInstances]
      ,sty.[SpecificCharacterSet]
      ,sty.[PatientAge]
      ,sty.[Edited]
      ,sty.[ReportStatus]
  FROM [dicom].[dbo].[Studies] sty
  Where 1 = 1" + coditionString + @"
  Order by Convert(Date,sty.StudyDate) DESC,TRY_CONVERT(TIME,sty.StudyTime) DESC";

                dt.Load(cmd.ExecuteReader());

                if (dt != null && dt.Rows.Count > 0)
                {
                    studiesList = dt.ToList<StudiesModel>();

                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            finally
            {
                Disconnect();
            }
            return studiesList;
        }


        [Route("GetStudiesList")]
        [HttpGet]
        public List<StudiesModel> GetStudiesList()
        {
            List<StudiesModel> studiesList = null;
            DataTable dt = new DataTable();
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = @"SELECT TOP 1000 [StudyInstanceUID]
      ,[PatientID]
      ,[PatientName]
      ,[PatientComments]
      ,[PatientBirthDate]
      ,[PatientSex]
      ,[StudyDescription]
      ,[StudyID]
      ,TRY_CONVERT(Date,StudyDate) StudyDate
      ,TRY_CONVERT(TIME,StudyTime) StudyTime
      ,[AccessionNumber]
      ,[InstitutionName]
      ,[ModalitiesInStudy]
      ,[BodyPartsInStudy]
      ,[NumberOfStudyRelatedSeries]
      ,[NumberOfStudyRelatedInstances]
      ,[SpecificCharacterSet]
      ,[PatientAge]
      ,[Edited]
      ,[ReportStatus]
  FROM [dicom].[dbo].[Studies]
  Order by Convert(Date,StudyDate) DESC,TRY_CONVERT(TIME,StudyTime) DESC";
                dt.Load(cmd.ExecuteReader());

                if (dt != null && dt.Rows.Count > 0)
                {
                    studiesList = dt.ToList<StudiesModel>();

                }
            }
            finally
            {
                Disconnect();
            }
            return studiesList;

        }


        [Route("GetSeriesByStudyUID")]
        [HttpGet]
        public List<SeriesModel> GetSeriesByStudyUID(string studyUID)
        {
            List<SeriesModel> seriesList = null;

            DataTable dt = new DataTable();
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = @"SELECT TOP 1000 [SeriesInstanceUID]
      ,[StudyInstanceUID]
      ,[PatientID]
      ,[SeriesNumber]
      ,TRY_CONVERT(Date,SeriesDate) SeriesDate
      ,TRY_CONVERT(TIME,SeriesTime) SeriesTime
      ,[SeriesDescription]
      ,[Modality]
      ,[BodypartExamined]
      ,[NumberOfSeriesRelatedInstances]
      ,[SpecificCharacterSet]
      ,[Edited]
	  ,(Select Top 1 SOPInstanceUID 
	   From Instances 
	   Where Instances.SeriesInstanceUID = [Series].SeriesInstanceUID 
	   and Instances.InstanceNumber = 1) DisplayInstanceUID
  FROM [dicom].[dbo].[Series]
  Where StudyInstanceUID = '" + studyUID + @"'
  Order By SeriesNumber";
                dt.Load(cmd.ExecuteReader());

                if (dt != null && dt.Rows.Count > 0)
                {
                    seriesList = dt.ToList<SeriesModel>();

                }
                foreach (var series in seriesList)
                {
                    string fullPath = dicomPath + series.StudyInstanceUID + "\\" + series.SeriesInstanceUID;
                    var directory = new DirectoryInfo(fullPath);
                    FileInfo file;
                    file = directory.GetFiles().FirstOrDefault(f => f.Name == (series.DisplayInstanceUID + ".dcm"));

                    if (file == null || file.Length <= 0)
                    {
                        file = directory.GetFiles().OrderBy(f => f.LastWriteTime).FirstOrDefault();
                    }

                    MemoryStream ms = new MemoryStream();
                    var dicomimage = new DicomImage(file.FullName);
                    var bitmap = dicomimage.RenderImage().AsClonedBitmap();

                    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                    //series.ImageDisplay = ms.ToArray();
                    series.ImageDisplay = ConvertImage.ResizeImage(ms.ToArray(), 64, 64, false);
                    ms.Dispose();


                    cmd.CommandText = @"SELECT TOP (1000) SOPInstanceUID,
SeriesInstanceUID,
StudyInstanceUID,
PatientID,
Modality,
SeriesDescription,
SeriesInstanceUID,
SeriesNumber,
InstanceNumber,
TRY_CONVERT(Date,SeriesDate) SeriesDate,
TRY_CONVERT(TIME,SeriesTime) SeriesTime,
NumberOfFrames,
(CASE WHEN FileSize < 1000000 THEN
           CONCAT(CEILING(FileSize / 1024.0), 'KB')
      ELSE 
           CONCAT(FORMAT(FileSize / 1048576.0, 'N2'), 'MB')
 END) AS FileSizeDisplay
FROM [dicom].[dbo].[Instances]
where SeriesInstanceUID = '" + series.SeriesInstanceUID + @"'
Order By InstanceNumber";
                    dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        series.Instances = dt.ToList<InstancesModel>();

                    }
                }
            }
            catch (Exception er)
            {
                throw;
            }
            finally
            {
                Disconnect();
            }
            return seriesList;

        }

        [Route("GetImageSeries")]
        [HttpGet]
        public List<ImageSerires> GetImageSeries(string studyInstanceUID, string seriesInstanceUID)
        {
            List<ImageSerires> imageSeries = new List<ImageSerires>();

            try
            {
                string fullPath = dicomPath + studyInstanceUID + "\\" + seriesInstanceUID;
                var directory = new DirectoryInfo(fullPath);
                var files = directory.GetFiles().OrderByDescending(f => f.LastWriteTime);
                foreach (var file in files)
                {
                    MemoryStream ms = new MemoryStream();
                    var dicomimage = new DicomImage(file.FullName);
                    var bitmap = dicomimage.RenderImage().AsClonedBitmap();
                    ImageSerires imageSeri = new ImageSerires();

                    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                    //imageSeri.ImageSources = ms.ToArray();
                    imageSeri.ImageSources = ConvertImage.ResizeImage(ms.ToArray(), 64, 64, false);
                    imageSeries.Add(imageSeri);
                    ms.Dispose();
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {

            }

            return imageSeries;
        }


        [Route("GetDicomFiles")]
        [HttpGet]
        public List<byte[]> GetDicomFiles(string studyInstanceUID, string seriesInstanceUID)
        {
            List<byte[]> dicomFiles = new List<byte[]>();
            string fullPath = dicomPath + studyInstanceUID + "\\" + seriesInstanceUID;
            var dirInfo = new DirectoryInfo(fullPath);
            foreach (var file in dirInfo.GetFiles("*.*", SearchOption.AllDirectories))
            {
                var dicomFile = Dicom.DicomFile.Open(file.FullName);
                MemoryStream ms = new MemoryStream();
                dicomFile.Save(ms);
                dicomFiles.Add(ms.ToArray());
            }

            return dicomFiles;
        }

        [Route("GetDicomFileByPatientID")]
        [HttpGet]
        public List<byte[]> GetDicomFileByPatientID(string patientID, DateTime studyDate, string modality, bool IsSINE)
        {
            List<byte[]> dicomFiles = null;
            try
            {
                DataTable dt = new DataTable();
                string newLine = Environment.NewLine;
                Connect();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;

                string SelectQuery = @"SELECT Distinct sty.[StudyInstanceUID]
,ser.SeriesInstanceUID
,ins.SOPInstanceUID
,Convert(Date, sty.StudyDate) StudyDate
,TRY_CONVERT(TIME, sty.StudyTime) StudyTime
FROM [dicom].[dbo].[Studies] sty
inner join [dicom].[dbo].[Series] ser
on Sty.StudyInstanceUID = ser.StudyInstanceUID
inner join [dicom].[dbo].[Instances] ins
on ser.SeriesInstanceUID = ins.SeriesInstanceUID";

                string whereConditionQuery = @"Where sty.PatientID = @PatientID
and sty.ModalitiesInStudy IN(" + modality + ")" +
@" and Convert(Date,sty.StudyDate) <= Convert(Date,@StudyDate)
and DATEDIFF(year,Convert(Date,sty.StudyDate), Convert(Date,@StudyDate)) <= 7";

                if (IsSINE == true)
                {
                    whereConditionQuery += newLine + "and (ins.NumberOfFrames is not null and NumberOfFrames > 1)";
                }
                else
                {
                    whereConditionQuery += newLine + "and (ins.NumberOfFrames is null OR ins.NumberOfFrames = 1)";
                }

                string orderByQuery = "Order by Convert(Date, sty.StudyDate) DESC,TRY_CONVERT(TIME, sty.StudyTime) DESC";

                cmd.CommandText = SelectQuery + newLine + whereConditionQuery + newLine + orderByQuery;

                cmd.Parameters.AddWithValue("@PatientID", patientID);
                cmd.Parameters.AddWithValue("@StudyDate", studyDate);
                dt.Load(cmd.ExecuteReader());

                if (dt != null && dt.Rows.Count > 0)
                {
                    dicomFiles = new List<byte[]>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string studyInstanceUID = dt.Rows[i]["StudyInstanceUID"].ToString();
                        string seriesInstanceUID = dt.Rows[i]["SeriesInstanceUID"].ToString();
                        string instanceUID = dt.Rows[i]["SOPInstanceUID"].ToString();
                        string fullName = dicomPath + studyInstanceUID + "\\" + seriesInstanceUID + "\\" + instanceUID + ".dcm";
                        if (File.Exists(fullName))
                        {
                            var dicomFile = Dicom.DicomFile.Open(fullName);
                            MemoryStream ms = new MemoryStream();
                            dicomFile.Save(ms);
                            dicomFiles.Add(ms.ToArray());

                        }

                    }

                }


            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            finally
            {
                Disconnect();
            }
            return dicomFiles;
        }

        [Route("SendDicomFile")]
        [HttpPost]
        public HttpResponseMessage SendDicomFile(SendDicomFilesModel sendDicomModel)
        {
            string dicomPath = System.Configuration.ConfigurationManager.AppSettings["DICOMPath"].ToString();
            string studyPath = dicomPath + sendDicomModel.StudyInstanceUID;
            string seriesPath = studyPath + "\\" + sendDicomModel.SeriesInstanceUID;
            string instancePath = seriesPath + "\\" + sendDicomModel.SOPInstanceUID + ".dcm";
            bool flag = false;
            try
            {
                MemoryStream ms = new MemoryStream(sendDicomModel.DicomArray);
                var dicomFile = Dicom.DicomFile.Open(ms);

                JIUN.DSP.Entities.Instance instancesSonic;
                PACSFunctions.SaveDicomFile(studyPath, seriesPath, instancePath, dicomFile, out instancesSonic);
                if (instancesSonic != null)
                {

                    SaveDicomData(dicomFile.Dataset, instancesSonic);
                }


                flag = true;
                return Request.CreateResponse(HttpStatusCode.OK, flag);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }

        }

        [Route("GetDicomFilesInstancesList")]
        [HttpPost]
        public List<InstancesCopyToModel> GetDicomFilesInstancesList(List<InstancesModel> instancesList)
        {
            List<InstancesCopyToModel> dataInstances = new List<InstancesCopyToModel>();
            foreach (var instance in instancesList)
            {
                string fullPath = dicomPath + instance.StudyInstanceUID + "\\" + instance.SeriesInstanceUID;
                string fileName = instance.SOPInstanceUID + ".dcm";
                string fullName = fullPath + "\\" + fileName;

                if (File.Exists(fullName))
                {
                    var dicomFile = Dicom.DicomFile.Open(fullName);
                    MemoryStream ms = new MemoryStream();
                    dicomFile.Save(ms);

                    InstancesCopyToModel newInstance = new InstancesCopyToModel();
                    newInstance.SOPInstanceUID = instance.SOPInstanceUID;
                    newInstance.DicomFiles = ms.ToArray();
                    newInstance.InstanceNumber = instance.InstanceNumber;
                    newInstance.NumberOfFrames = instance.NumberOfFrames;
                    newInstance.FileSizeDisplay = instance.FileSizeDisplay;
                    dataInstances.Add(newInstance);

                }

            }

            return dataInstances;
        }

        [Route("ChangeHNDicomData")]
        [HttpPost]
        public HttpResponseMessage ChangeHNDicomData(string newHN, string oldHN)
        {
            bool flag = false;
            DataTable dt = new DataTable();
            Connect();
            // SqlTransaction transaction = conn.BeginTransaction(); ;
            try
            {
                SqlCommand cmd = new SqlCommand();
                //cmd.Transaction = transaction;
                cmd.Connection = conn;
                cmd.CommandText = @"select pa.PatientID,stu.StudyInstanceUID,ser.SeriesInstanceUID
from Patients pa
inner join Studies stu
on pa.PatientID = stu.PatientID
inner join Series ser
on stu.StudyInstanceUID = ser.StudyInstanceUID
Where pa.PatientID = '" + oldHN + "'";
                dt.Load(cmd.ExecuteReader());

                if (dt != null && dt.Rows.Count > 0)
                {
                    cmd.CommandText = @"
Update Patients Set PatientID = @NewHN Where PatientID = @OldHN

Update Studies Set PatientID = @NewHN  Where PatientID = @OldHN

Update Series Set PatientID = @NewHN  Where PatientID = @OldHN

Update Instances Set PatientID = @NewHN  Where PatientID = @OldHN
"
;
                    cmd.Parameters.AddWithValue("@NewHN", newHN);
                    cmd.Parameters.AddWithValue("@OldHN", oldHN);

                    cmd.ExecuteNonQuery();


                    //string backupPath = @"C:\Backup Dicom\";
                    //foreach (DataRow item in dt.Rows)
                    //{
                    //    if (!Directory.Exists(backupPath + item["StudyInstanceUID"]))
                    //    {
                    //        Directory.CreateDirectory(backupPath + item["StudyInstanceUID"]);
                    //        if (!Directory.Exists(backupPath + item["StudyInstanceUID"] + "\\" + item["SeriesInstanceUID"]))
                    //        {
                    //            Directory.CreateDirectory(backupPath + item["StudyInstanceUID"] + "\\" + item["SeriesInstanceUID"]);
                    //        }
                    //    }
                    //    string fullPath = dicomPath + item["StudyInstanceUID"] + "\\" + item["SeriesInstanceUID"];
                    //    var dirInfo = new DirectoryInfo(fullPath);
                    //    foreach (var file in dirInfo.GetFiles("*.*", SearchOption.AllDirectories))
                    //    {
                    //        var dicomFile = Dicom.DicomFile.Open(file.FullName);
                    //        string backupFile = backupPath + item["StudyInstanceUID"] + "\\" + item["SeriesInstanceUID"] + "\\" + file.Name;
                    //        if (!File.Exists(backupFile))
                    //        {
                    //            dicomFile.File.Move(backupFile);
                    //        }
                    //        else
                    //        {
                    //            dicomFile.File.Move(Path.GetTempPath() + "\\" + file.Name);
                    //        }
                    //        dicomFile.Dataset.AddOrUpdate(DicomTag.PatientID, newHN);   
                    //        dicomFile.Save(file.FullName);
                    //        File.Delete(Path.GetTempPath() + "\\" + file.Name);
                    //    }
                    //}

                    //transaction.Commit();
                    flag = true;
                }
                return Request.CreateResponse(HttpStatusCode.OK, flag);
            }
            catch (Exception ex)
            {
                //transaction.Rollback();
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
            finally
            {
                Disconnect();
            }
        }

        [Route("EditStudy")]
        [HttpPost]
        public HttpResponseMessage EditStudy(StudiesModel studiesModel)
        {
            try
            {
                DateTime dateNow = DateTime.Now;

                Studies study = dbPACS.Studies.Find(studiesModel.StudyInstanceUID);

                if (study != null)
                {
                    Patients patients2 = dbPACS.Patients.Find(studiesModel.PatientID2);
                    if (studiesModel.PatientID.Trim() != studiesModel.PatientID2.Trim())
                    {
                        if (patients2 == null)
                        {
                            patients2 = new Patients();
                            patients2.PatientID = studiesModel.PatientID2;
                            patients2.PatientName = studiesModel.PatientName;
                            patients2.PatientBirthDate = studiesModel.PatientBirthDate;
                            patients2.PatientSex = studiesModel.PatientSex;
                            patients2.Edited = dateNow;
                            dbPACS.Patients.Add(patients2);
                        }
                    }

                    dbPACS.Studies.Attach(study);

                    study.PatientID = studiesModel.PatientID2;
                    study.PatientName = studiesModel.PatientName;
                    study.PatientSex = studiesModel.PatientSex;
                    study.PatientBirthDate = studiesModel.PatientBirthDate;
                    study.AccessionNumber = studiesModel.AccessionNumber;
                    study.StudyDate = studiesModel.StudyDate.ToString("yyyy/MM/dd");
                    study.StudyTime = studiesModel.StudyTime;
                    study.ModalitiesInStudy = studiesModel.ModalitiesInStudy;
                    study.Edited = dateNow;
                    IEnumerable<Series> listSeries = dbPACS.Series.Where(p => p.StudyInstanceUID == study.StudyInstanceUID);
                    foreach (var series in listSeries)
                    {
                        dbPACS.Series.Attach(series);

                        series.PatientID = studiesModel.PatientID2;
                        series.SeriesDate = studiesModel.StudyDate.ToString("yyyy/MM/dd");
                        series.SeriesTime = studiesModel.StudyTime;
                        series.Edited = dateNow;
                        series.Modality = studiesModel.ModalitiesInStudy;

                        IEnumerable<Instances> listInstances = dbPACS.Instances.Where(p => p.SeriesInstanceUID == series.SeriesInstanceUID);

                        foreach (var instances in listInstances)
                        {
                            dbPACS.Instances.Attach(instances);

                            instances.PatientID = studiesModel.PatientID2;
                            instances.PatientName = studiesModel.PatientName;
                            instances.PatientSex = studiesModel.PatientSex;
                            instances.PatientBirthDate = studiesModel.PatientBirthDate;
                            instances.AccessionNumber = studiesModel.AccessionNumber;
                            instances.StudyDate = studiesModel.StudyDate.ToString("yyyy/MM/dd");
                            instances.StudyTime = studiesModel.StudyTime;
                            instances.SeriesDate = studiesModel.StudyDate.ToString("yyyy/MM/dd");
                            instances.SeriesTime = studiesModel.StudyTime;
                            instances.Modality = studiesModel.ModalitiesInStudy;
                        }
                    }



                    dbPACS.SaveChanges();

                    if (studiesModel.PatientID.Trim() != studiesModel.PatientID2.Trim())
                    {

                        Patients patient1 = dbPACS.Patients.Find(studiesModel.PatientID);
                        patient1.NumberOfPatientRelatedStudies = dbPACS.Studies.Count(p => p.PatientID == patient1.PatientID);
                        patient1.NumberOfPatientRelatedSeries = dbPACS.Series.Count(p => p.PatientID == patient1.PatientID);
                        patient1.NumberOfPatientRelatedInstances = dbPACS.Instances.Count(p => p.PatientID == patient1.PatientID);


                        dbPACS.Patients.Attach(patients2);
                        patients2.NumberOfPatientRelatedStudies = dbPACS.Studies.Count(p => p.PatientID == patients2.PatientID);
                        patients2.NumberOfPatientRelatedSeries = dbPACS.Series.Count(p => p.PatientID == patients2.PatientID);
                        patients2.NumberOfPatientRelatedInstances = dbPACS.Instances.Count(p => p.PatientID == patients2.PatientID);

                    }


                    int studyCount = dbPACS.Studies.Count(p => p.PatientID == studiesModel.PatientID);
                    if (studyCount == 0)
                    {
                        Patients oldPatient = dbPACS.Patients.Find(studiesModel.PatientID);
                        if (oldPatient != null)
                        {
                            dbPACS.Patients.Remove(oldPatient);
                        }

                    }
                    dbPACS.SaveChanges();
                }


                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                //transaction.Rollback();
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }

        [Route("SaveDicomData")]
        [HttpPost]
        public void SaveDicomData(DicomDataset dicomDataSet, JIUN.DSP.Entities.Instance instancesSonic)
        {
            try
            {
                DateTime now = DateTime.Now;
                string patientID = dicomDataSet.GetSingleValue<string>(DicomTag.PatientID);
                Patients pat = dbPACS.Patients.Find(patientID);
                if (pat == null)
                {
                    pat = new Patients();
                    pat.PatientID = dicomDataSet.GetSingleValue<string>(DicomTag.PatientID);
                    pat.PatientName = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.PatientName, null);
                    pat.PatientComments = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.PatientComments, null);
                    pat.PatientBirthDate = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.PatientBirthDate, null);
                    pat.PatientSex = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.PatientSex, null);
                    pat.SpecificCharacterSet = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.SpecificCharacterSet, instancesSonic.SpecificCharacterSet);
                }


                pat.Edited = now;

                dbPACS.Patients.AddOrUpdate(pat);
                dbPACS.SaveChanges();

                string StudyInstanceUID = dicomDataSet.GetSingleValue<string>(DicomTag.StudyInstanceUID);
                Studies studie = dbPACS.Studies.Find(StudyInstanceUID);

                if (studie == null)
                {
                    studie = new Studies();
                }
                studie.StudyInstanceUID = dicomDataSet.GetSingleValue<string>(DicomTag.StudyInstanceUID);
                studie.StudyInstanceUID = dicomDataSet.GetSingleValue<string>(DicomTag.StudyInstanceUID);
                studie.PatientID = dicomDataSet.GetSingleValue<string>(DicomTag.PatientID);
                studie.PatientName = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.PatientName, null);
                //studie.PerformingPhysicianName = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.PerformingPhysicianName, null);
                //studie.ReferringPhysicianName = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.ReferringPhysicianName, null);
                studie.PatientComments = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.PatientComments, null);
                studie.PatientBirthDate = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.PatientBirthDate, null);
                studie.PatientSex = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.PatientSex, null);
                studie.StudyDescription = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.StudyDescription, null);
                studie.StudyID = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.StudyID, null);

                DateTime studyDateTime = dicomDataSet.GetDateTime(DicomTag.StudyDate, DicomTag.StudyTime);
                studie.StudyDate = studyDateTime.ToString("yyyy/MM/dd");
                studie.StudyTime = studyDateTime.ToString("HH:mm:ss");


                studie.AccessionNumber = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.AccessionNumber, null);
                studie.InstitutionName = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.InstitutionName, null);
                studie.ModalitiesInStudy = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.Modality, null);
                studie.BodyPartsInStudy = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.BodyPartExamined, null);
                //studie.NumberOfStudyRelatedSeries = dicomDataSet.GetSingleValueOrDefault<int>(DicomTag.NumberOfStudyRelatedSeries,1);
                //studie.NumberOfStudyRelatedInstances = dicomDataSet.GetSingleValueOrDefault<int>(DicomTag.NumberOfStudyRelatedInstances,1);
                studie.SpecificCharacterSet = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.SpecificCharacterSet, instancesSonic.SpecificCharacterSet);
                studie.PatientAge = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.PatientAge, null);
                studie.ReportStatus = 0;
                studie.Edited = now;

                dbPACS.Studies.AddOrUpdate(studie);
                dbPACS.SaveChanges();

                string SeriesInstanceUID = dicomDataSet.GetSingleValue<string>(DicomTag.SeriesInstanceUID);
                Series series = dbPACS.Series.Find(SeriesInstanceUID);

                if (series == null)
                {
                    series = new Series();

                }
                series.SeriesInstanceUID = dicomDataSet.GetSingleValue<string>(DicomTag.SeriesInstanceUID);
                series.SeriesInstanceUID = dicomDataSet.GetSingleValue<string>(DicomTag.SeriesInstanceUID);
                series.StudyInstanceUID = dicomDataSet.GetSingleValue<string>(DicomTag.StudyInstanceUID);
                series.PatientID = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.PatientID, null);
                series.SeriesNumber = dicomDataSet.GetSingleValueOrDefault<int?>(DicomTag.SeriesNumber, null);


                DateTime seriesDateTime = dicomDataSet.GetDateTime(DicomTag.SeriesDate, DicomTag.SeriesTime);
                series.SeriesDate = seriesDateTime.ToString("yyyy/MM/dd");
                series.SeriesTime = seriesDateTime.ToString("HH:mm:ss");



                series.SeriesDescription = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.SeriesDescription, null);
                series.Modality = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.Modality, null);
                series.BodypartExamined = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.BodyPartExamined, null);
                //series.NumberOfSeriesRelatedInstances = dicomDataSet.GetSingleValueOrDefault<int>(DicomTag.NumberOfSeriesRelatedInstances,1);
                series.SpecificCharacterSet = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.SpecificCharacterSet, instancesSonic.SpecificCharacterSet);
                series.Edited = now;

                dbPACS.Series.AddOrUpdate(series);
                dbPACS.SaveChanges();

                Instances instances = dbPACS.Instances.Find(dicomDataSet.GetSingleValue<string>(DicomTag.SOPInstanceUID));
                if (instances == null)
                {
                    instances = new Instances();
                }
                instances.SOPInstanceUID = dicomDataSet.GetSingleValue<string>(DicomTag.SOPInstanceUID);
                instances.SOPInstanceUID = dicomDataSet.GetSingleValue<string>(DicomTag.SOPInstanceUID);
                instances.SeriesInstanceUID = dicomDataSet.GetSingleValue<string>(DicomTag.SeriesInstanceUID);
                instances.StudyInstanceUID = dicomDataSet.GetSingleValue<string>(DicomTag.StudyInstanceUID);
                instances.SOPClassUID = dicomDataSet.GetSingleValue<string>(DicomTag.SOPClassUID);
                instances.TransferSyntaxUID = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.TransferSyntaxUID, instancesSonic.TransferSyntaxUID);
                instances.PatientID = dicomDataSet.GetSingleValue<string>(DicomTag.PatientID);
                instances.PatientName = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.PatientName, null);
                //instances.ReferringPhysicianName = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.ReferringPhysicianName, null);
                //instances.PerformingPhysicianName = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.PerformingPhysicianName, null);
                instances.PatientComments = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.PatientComments, null);

                instances.PatientBirthDate = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.PatientBirthDate, null);
                instances.PatientSex = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.PatientSex, null);
                instances.StudyDescription = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.StudyDescription, null);
                instances.StudyID = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.StudyID, null);
                instances.StudyDate = studie.StudyDate;
                instances.StudyTime = studie.StudyTime;
                //instances.StudyDate = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.StudyDate, null);
                //instances.StudyTime = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.StudyTime, null);
                instances.AccessionNumber = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.AccessionNumber, null);
                instances.InstitutionName = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.InstitutionName, null);
                instances.SeriesNumber = dicomDataSet.GetSingleValueOrDefault<int?>(DicomTag.SeriesNumber, null);

                instances.SeriesDate = series.SeriesDate;
                instances.SeriesTime = series.SeriesTime;
                //instances.SeriesDate = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.SeriesDate, null);
                //instances.SeriesTime = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.SeriesTime, null);
                instances.SeriesDescription = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.SeriesDescription, null);
                instances.Modality = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.Modality, null);
                instances.BodypartExamined = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.BodyPartExamined, null);
                instances.InstanceNumber = dicomDataSet.GetSingleValueOrDefault<int?>(DicomTag.InstanceNumber, null);
                instances.NumberOfFrames = dicomDataSet.GetSingleValueOrDefault<int?>(DicomTag.NumberOfFrames, null);
                instances.PixelSpacing = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.PixelSpacing, null);
                instances.PixelAspectRatio = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.PixelAspectRatio, null);
                instances.SliceThickness = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.SliceThickness, null);
                instances.SpacingBetweenSlices = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.SpacingBetweenSlices, null);
                instances.SliceLocation = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.SliceLocation, null);
                instances.PatientOrientation = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.PatientOrientation, null);
                instances.ImagePositionPatient = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.ImagePositionPatient, null);
                instances.ImageOrientationPatient = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.ImageOrientationPatient, null);
                instances.Width = instancesSonic.Width;
                instances.Height = instancesSonic.Height;
                instances.RescaleIntercept = dicomDataSet.GetSingleValueOrDefault<int?>(DicomTag.RescaleIntercept, null);
                instances.RescaleSlope = dicomDataSet.GetSingleValueOrDefault<int?>(DicomTag.RescaleSlope, null);
                instances.RescaleType = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.RescaleType, null);
                instances.WindowWidth = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.WindowWidth, null);
                instances.WindowCenter = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.WindowCenter, null);
                instances.SamplesPerPixel = dicomDataSet.GetSingleValueOrDefault<int?>(DicomTag.SamplesPerPixel, null);
                instances.PlanarConfiguration = dicomDataSet.GetSingleValueOrDefault<int?>(DicomTag.PlanarConfiguration, null);
                instances.BitsAllocated = dicomDataSet.GetSingleValueOrDefault<int?>(DicomTag.BitsAllocated, null);
                instances.BitsStored = dicomDataSet.GetSingleValueOrDefault<int?>(DicomTag.BitsStored, null);
                instances.HighBit = dicomDataSet.GetSingleValueOrDefault<int?>(DicomTag.HighBit, null);
                instances.PixelRepresentation = dicomDataSet.GetSingleValueOrDefault<int?>(DicomTag.PixelRepresentation, null);


                //instances.PixelData = dicomDataSet.GetSingleValueOrDefault<int?>(DicomTag.PixelData, 1034);

                instances.PixelData = instancesSonic.PixelData;

                string pixelSpacing = "";
                dicomDataSet.TryGetString(DicomTag.PixelSpacing, out pixelSpacing);

                instances.PixelSpacing = pixelSpacing;

                instances.FileSize = instancesSonic.FileSize;

                instances.SpecificCharacterSet = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.SpecificCharacterSet, instancesSonic.SpecificCharacterSet);
                instances.ImageType = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.ImageType, instancesSonic.ImageType);

                string photometricInterpretation = dicomDataSet.GetSingleValue<string>(DicomTag.PhotometricInterpretation);

                //instances.PhotometricInterpretation = photometricInterpretation; //192.168.2.2
                instances.PhotoMatricInterpretation = photometricInterpretation; //192.168.2.3

                instances.PatientAge = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.PatientAge, null);
                instances.LossyImageCompression = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.LossyImageCompression, instancesSonic.LossyImageCompression);

                instances.Method = 2;
                instances.Created = now;
                dbPACS.Instances.AddOrUpdate(instances);
                dbPACS.SaveChanges();


                //NumberOfPatientRelated
                dbPACS.Patients.Attach(pat);
                pat.NumberOfPatientRelatedStudies = dbPACS.Studies.Count(p => p.PatientID == pat.PatientID);
                pat.NumberOfPatientRelatedSeries = dbPACS.Series.Count(p => p.PatientID == pat.PatientID);
                pat.NumberOfPatientRelatedInstances = dbPACS.Instances.Count(p => p.PatientID == pat.PatientID);

                //NumberOfStudyRelated
                dbPACS.Studies.Attach(studie);
                studie.NumberOfStudyRelatedSeries = dbPACS.Series.Count(p => p.StudyInstanceUID == studie.StudyInstanceUID);
                studie.NumberOfStudyRelatedInstances = dbPACS.Instances.Count(p => p.StudyInstanceUID == studie.StudyInstanceUID);

                //NumberOfSeriesRelated
                dbPACS.Series.Attach(series);
                series.NumberOfSeriesRelatedInstances = dbPACS.Instances.Count(p => p.SeriesInstanceUID == series.SeriesInstanceUID);


                dbPACS.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }

}
