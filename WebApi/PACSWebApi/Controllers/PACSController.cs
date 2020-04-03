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

namespace MediTechWebApi.Controllers
{
    [RoutePrefix("Api/PACS")]
    public class PACSController : ApiController
    {
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
        public List<byte[]> GetDicomFileByPatientID(string patientID, DateTime studyDate, string modality)
        {
            List<byte[]> dicomFiles = null;
            try
            {
                DataTable dt = new DataTable();
                Connect();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = @"SELECT Distinct sty.[StudyInstanceUID]
,ser.SeriesInstanceUID
,Convert(Date, sty.StudyDate) StudyDate
,TRY_CONVERT(TIME, sty.StudyTime) StudyTime
FROM [dicom].[dbo].[Studies] sty
inner join [dicom].[dbo].[Series] ser
on Sty.StudyInstanceUID = ser.StudyInstanceUID
Where sty.PatientID = @PatientID
and sty.ModalitiesInStudy IN (" + modality + ")" +
@" and Convert(Date,sty.StudyDate) <= Convert(Date,@StudyDate)
and DATEDIFF(year,Convert(Date,sty.StudyDate), Convert(Date,@StudyDate)) <= 7
Order by Convert(Date, sty.StudyDate) DESC,TRY_CONVERT(TIME, sty.StudyTime) DESC";

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
                        string fullPath = dicomPath + studyInstanceUID + "\\" + seriesInstanceUID;
                        var dirInfo = new DirectoryInfo(fullPath);
                        foreach (var file in dirInfo.GetFiles("*.*", SearchOption.AllDirectories))
                        {
                            var dicomFile = Dicom.DicomFile.Open(file.FullName);
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
            SafeTokenHandle safeTokenHandle;
            bool flag = false;
            try
            {
                MemoryStream ms = new MemoryStream(sendDicomModel.DicomArray);
                var dicomFile = Dicom.DicomFile.Open(ms);

                PACSFunctions.SaveDicomFile(sendDicomModel.StudyInstanceUID, sendDicomModel.SeriesInstanceUID, sendDicomModel.SOPInstanceUID, dicomFile);


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

    }

}
