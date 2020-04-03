
using Dicom;
using Dicom.IO.Buffer;
using MediTech.DataBase;
using MediTech.Model;
using MediTech.Model.PACS;
using MediTechWebApi.Controllers;
using PACS.DataBase;
using PACSWebApi.Helpter;
using ShareLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Http;
using System.Web.Http.Description;

namespace PACSWebApi.Controllers
{
    [RoutePrefix("Api/TERC")]
    public class TERCController : ApiController
    {
        protected MediTechEntities dbMediTech = new MediTechEntities();
        protected PACSEntities dbPACS = new PACSEntities();

        //[Route("RegisterDataAndImportDicom")]
        //[HttpPost]
        //public HttpResponseMessage RegisterDataAndImportDicom(DataInbound inboundData)
        //{
        //    try
        //    {
        //        DateTime now = DateTime.Now;

        //        return Request.CreateResponse(HttpStatusCode.OK);
        //    }
        //    catch (Exception ex)
        //    {

        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
        //    }
        //}

        [Route("RegisterDataAndImportDicom")]
        [HttpPost]
        public async Task<HttpResponseMessage> RegisterDataAndImportDicom()
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed);
            }
            try
            {
                var data = await Request.Content.ReadAsByteArrayAsync();
                var inboundData = ByteArrayToObject<DataInbound>(data);
                DateTime now = DateTime.Now;
                using (var tran = new TransactionScope())
                {
                    int organisationUID = 0;
                    int userUID = 9999;
                    var refValue = dbMediTech.ReferenceValue.Where(p => (p.DomainCode == "BSMDD" || p.DomainCode == "ORDST") && p.StatusFlag == "A");
                    var refValueBSMDD = refValue.Where(p => p.DomainCode == "BSMDD");
                    var refValueORDST = refValue.Where(p => p.DomainCode == "ORDST");
                    int BSMDD_LAB = refValueBSMDD.FirstOrDefault(p => p.ValueCode == "LABBB").UID;
                    int BSMDD_RADIO = refValueBSMDD.FirstOrDefault(p => p.ValueCode == "RADIO").UID;
                    int BSMDD_STORE = refValueBSMDD.FirstOrDefault(p => p.ValueCode == "STORE").UID;
                    //int BSMDD_ORDITEM = 2839;
                    int BSMDD_MDSLP = refValueBSMDD.FirstOrDefault(p => p.ValueCode == "MDSLP").UID;
                    int BSMDD_SULPY = refValueBSMDD.FirstOrDefault(p => p.ValueCode == "SUPLY").UID;


                    var billableItem = dbMediTech.BillableItem.FirstOrDefault(p => p.StatusFlag == "A"
&& (p.ActiveFrom == null || DbFunctions.TruncateTime(DateTime.Now) >= DbFunctions.TruncateTime(p.ActiveFrom))
&& (p.ActiveTo == null || DbFunctions.TruncateTime(DateTime.Now) <= DbFunctions.TruncateTime(p.ActiveTo))
&& p.Code == inboundData.RequestItemCode);

                    PatientInformationModel patientInfo = new PatientInformationModel();
                    var healthOrganisation = dbMediTech.HealthOrganisation.FirstOrDefault(p =>
                    p.StatusFlag == "A"
                    && p.Code == inboundData.HealthOrganisationCode);

                    var referenceValue = dbMediTech.ReferenceValue.Where(p => p.StatusFlag == "A"
                    && (p.DomainCode == "RQPRT" || p.DomainCode == "VISTY" || p.DomainCode == "VISTS"));

                    var payorDeail = (from i in dbMediTech.PayorDetail
                                      join j in dbMediTech.PayorAgreement on i.UID equals j.PayorDetailUID
                                      where i.Code == inboundData.HealthOrganisationCode
                                      && i.StatusFlag == "A"
                                      select new
                                      {
                                          PayorDetailUID = i.UID,
                                          PayorName = i.PayorName,
                                          PayorAgreementUID = j.UID
                                      }).FirstOrDefault();

                    if (billableItem == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Not Set BillableItem");
                    }

                    if (payorDeail == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Not Set Payor", new Exception("TEST"));
                    }

                    if (healthOrganisation == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Not Set Organisation");
                    }

                    organisationUID = healthOrganisation.UID;

                    patientInfo.PatientID = inboundData.PatientID;
                    patientInfo.FirstName = inboundData.FirstName;
                    patientInfo.LastName = inboundData.LastName;
                    patientInfo.BirthDttm = inboundData.BirthDate;

                    var gender = dbMediTech.ReferenceValue.FirstOrDefault(p =>
                    p.StatusFlag == "A"
                    && p.DomainCode == "SEXXX"
                    && p.Description == inboundData.Gender);

                    var title = dbMediTech.ReferenceValue.FirstOrDefault(p =>
                 p.StatusFlag == "A"
                 && p.DomainCode == "TITLE"
                 && p.Description == inboundData.Title);

                    if (gender != null)
                    {
                        patientInfo.SEXXXUID = gender.UID;
                    }

                    if (title != null)
                    {
                        patientInfo.TITLEUID = title.UID;
                    }


                    Patient patient = dbMediTech.Patient.FirstOrDefault(p => p.PatientID == patientInfo.PatientID
                    && p.OwnerOrganisationUID == organisationUID
                    && p.StatusFlag == "A"
                    );

                    if (patient == null)
                    {
                        patient = new Patient();
                        patient.CUser = userUID;
                        patient.CWhen = now;

                        patient.PatientID = patientInfo.PatientID;
                    }

                    patient.FirstName = patientInfo.FirstName;
                    patient.MiddleName = patientInfo.MiddelName;
                    patient.LastName = patientInfo.LastName;
                    patient.NickName = patientInfo.NickName;
                    patient.SEXXXUID = patientInfo.SEXXXUID;
                    patient.TITLEUID = patientInfo.TITLEUID;
                    patient.DOBDttm = patientInfo.BirthDttm;
                    patient.OwnerOrganisationUID = organisationUID;
                    patient.MUser = userUID;
                    patient.MWhen = now;
                    patient.StatusFlag = "A";

                    dbMediTech.Patient.AddOrUpdate(patient);
                    dbMediTech.SaveChanges();



                    int outseqvisitUID;
                    string seqVisitID = GetSEQIDFormat("SEQVisitID", out outseqvisitUID);

                    if (string.IsNullOrEmpty(seqVisitID))
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No SEQVisitID in SEQCONFIGURATION");
                    }


                    PatientVisit patientVisit = new PatientVisit();
                    patientVisit.PatientUID = patient.UID;
                    patientVisit.VISTYUID = referenceValue.FirstOrDefault(p => p.StatusFlag == "A" && p.DomainCode == "VISTY" && p.ValueCode == "XYNGF").UID;
                    patientVisit.VISTSUID = referenceValue.FirstOrDefault(p => p.StatusFlag == "A" && p.DomainCode == "VISTS" && p.ValueCode == "REGST").UID;
                    patientVisit.PRITYUID = referenceValue.FirstOrDefault(p => p.StatusFlag == "A" && p.DomainCode == "RQPRT" && p.ValueCode == "NORML").UID;
                    patientVisit.VisitID = seqVisitID;
                    patientVisit.StartDttm = now;
                    patientVisit.ArrivedDttm = now;
                    patientVisit.CUser = userUID;
                    patientVisit.CWhen = now;
                    patientVisit.MUser = userUID;
                    patientVisit.MWhen = now;
                    patientVisit.StatusFlag = "A";
                    patientVisit.OwnerOrganisationUID = organisationUID;

                    dbMediTech.PatientVisit.Add(patientVisit);
                    dbMediTech.SaveChanges();

                    #region PatientServiceEvent
                    PatientServiceEvent serviceEvent = new PatientServiceEvent();
                    serviceEvent.PatientVisitUID = patientVisit.UID;
                    serviceEvent.EventStartDttm = now;
                    serviceEvent.VISTSUID = patientVisit.VISTSUID ?? 0;
                    serviceEvent.MUser = userUID;
                    serviceEvent.MWhen = now;
                    serviceEvent.CUser = userUID;
                    serviceEvent.CWhen = now;
                    serviceEvent.StatusFlag = "A";

                    dbMediTech.PatientServiceEvent.Add(serviceEvent);
                    #endregion

                    dbMediTech.SaveChanges();

                    PatientVisitPayor visitPayor = new PatientVisitPayor();
                    visitPayor.PatientUID = patientVisit.PatientUID;
                    visitPayor.PatientVisitUID = patientVisit.UID;
                    visitPayor.PayorDetailUID = payorDeail.PayorDetailUID;
                    visitPayor.PayorAgreementUID = payorDeail.PayorAgreementUID;
                    visitPayor.CUser = userUID;
                    visitPayor.CWhen = now;
                    visitPayor.MUser = userUID;
                    visitPayor.MWhen = now;
                    visitPayor.StatusFlag = "A";

                    dbMediTech.PatientVisitPayor.Add(visitPayor);
                    dbMediTech.SaveChanges();


                    var requestItem = dbMediTech.RequestItem.FirstOrDefault(p => p.UID == billableItem.ItemUID);

                    PatientOrder patientOrder = new PatientOrder();
                    patientOrder.CUser = userUID;
                    patientOrder.CWhen = now;

                    int seqPatientOrderID;
                    string patientOrderID = GetSEQIDFormat("SEQPatientOrder", out seqPatientOrderID);

                    if (string.IsNullOrEmpty(patientOrderID))
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No SEQPatientOrder in SEQCONFIGURATION");
                    }

                    if (seqPatientOrderID == 0)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Insert SEQPatientOrder is Fail");
                    }

                    patientOrder.OrderNumber = patientOrderID;
                    patientOrder.StartDttm = now;

                    patientOrder.OrderRaisedBy = userUID;
                    patientOrder.MUser = userUID;
                    patientOrder.MWhen = now;
                    patientOrder.StatusFlag = "A";
                    patientOrder.PatientUID = patientVisit.PatientUID;
                    patientOrder.PatientVisitUID = patientVisit.UID;

                    patientOrder.OwnerOrganisationUID = organisationUID;
                    dbMediTech.PatientOrder.Add(patientOrder);

                    dbMediTech.SaveChanges();


                    Request request = new Request();
                    request.CUser = userUID;
                    request.CWhen = now;
                    request.StatusFlag = "A";
                    int outrequestUID;
                    string seqRequestID = GetSEQIDFormat("SEQRequest", out outrequestUID);

                    if (string.IsNullOrEmpty(seqRequestID))
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No SEQRequest in SEQCONFIGURATION");
                    }

                    if (outrequestUID == 0)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Insert SEQRequest is Fail");
                    }


                    request.RequestNumber = seqRequestID;
                    request.RequestedDttm = now;
                    request.RequestedUserUID = userUID;
                    request.BSMDDUID = BSMDD_RADIO;
                    request.RQPRTUID = 440; //Priority Normal
                    request.PatientUID = patientVisit.PatientUID;
                    request.PatientVisitUID = patientVisit.UID;
                    request.PatientOrderUID = patientOrder.UID;
                    request.MUser = userUID;
                    request.MWhen = now;
                    request.OwnerOrganisationUID = organisationUID;


                    dbMediTech.Request.Add(request);
                    dbMediTech.SaveChanges();


                    dbMediTech.PatientOrder.Attach(patientOrder);
                    patientOrder.IdentifyingType = "REQUEST";
                    patientOrder.IdentifyingUID = request.UID;
                    dbMediTech.SaveChanges();

                    PatientOrderDetail orderDetail = new PatientOrderDetail();
                    orderDetail.CUser = userUID;
                    orderDetail.CWhen = now;
                    orderDetail.StartDttm = now;

                    orderDetail.ORDSTUID = refValueORDST.FirstOrDefault(p => p.ValueCode == "EXCUT" && p.StatusFlag == "A").UID;


                    orderDetail.PatientOrderUID = patientOrder.UID;
                    orderDetail.MUser = userUID;
                    orderDetail.MWhen = now;
                    orderDetail.StatusFlag = "A";
                    orderDetail.OwnerOrganisationUID = organisationUID;
                    orderDetail.ItemCode = billableItem.Code;
                    orderDetail.ItemName = billableItem.ItemName;
                    orderDetail.BillableItemUID = billableItem.UID;
                    orderDetail.Quantity = 1;

                    if (inboundData.NetAmount != 0)
                    {
                        orderDetail.UnitPrice = inboundData.NetAmount;
                    }
                    else
                    {
                        List<BillableItemDetailModel> billableItemDetail = dbMediTech.BillableItemDetail
                        .Where(p => p.StatusFlag == "A" && p.BillableItemUID == billableItem.UID)
                        .Select(p => new BillableItemDetailModel
                        {
                            BillableItemDetailUID = p.UID,
                            BillableItemUID = p.BillableItemUID,
                            ActiveFrom = p.ActiveFrom,
                            ActiveTo = p.ActiveTo,
                            Price = p.Price,
                            StatusFlag = p.StatusFlag,
                            OwnerOrganisationUID = p.OwnerOrganisationUID,
                            OwnerOrganisationName = p.OwnerOrganisationUID != 0 ? SqlFunction.fGetHealthOrganisationName(p.OwnerOrganisationUID) : "ราคามาตรฐานส่วนกลาง",
                            CURNCUID = p.CURNCUID,
                            Unit = SqlFunction.fGetRfValDescription(p.CURNCUID)
                        }).ToList();

                        var selectBillableItemPrice = GetBillableItemPrice(billableItemDetail, organisationUID);
                        if (selectBillableItemPrice != null)
                        {
                            orderDetail.UnitPrice = selectBillableItemPrice.Price;
                        }
                    }

                    orderDetail.DoctorFee = billableItem.DoctorFee;
                    orderDetail.NetAmount = orderDetail.UnitPrice;
                    dbMediTech.PatientOrderDetail.Add(orderDetail);
                    dbMediTech.SaveChanges();


                    #region SavePatinetOrderDetailHistory

                    PatientOrderDetailHistory patientOrderDetailHistory = new PatientOrderDetailHistory();
                    patientOrderDetailHistory.PatientOrderDetailUID = orderDetail.UID;
                    patientOrderDetailHistory.ORDSTUID = orderDetail.ORDSTUID;
                    patientOrderDetailHistory.EditedDttm = now;
                    patientOrderDetailHistory.EditByUserID = userUID;
                    patientOrderDetailHistory.CUser = userUID;
                    patientOrderDetailHistory.CWhen = now;
                    patientOrderDetailHistory.MUser = userUID;
                    patientOrderDetailHistory.MWhen = now;
                    patientOrderDetailHistory.StatusFlag = "A";
                    dbMediTech.PatientOrderDetailHistory.Add(patientOrderDetailHistory);
                    dbMediTech.SaveChanges();

                    #endregion


                    RequestDetail requestDetail = new RequestDetail();
                    requestDetail.CUser = userUID;
                    requestDetail.CWhen = now;
                    requestDetail.StatusFlag = "A";

                    requestDetail.AccessionNumber = GetAccessionNumber(organisationUID);
                    requestDetail.RIMTYPUID = requestItem.RIMTYPUID;




                    requestDetail.RequestUID = patientOrder.IdentifyingUID.Value;
                    requestDetail.RequestitemUID = requestItem.UID;
                    requestDetail.RequestedDttm = now;
                    requestDetail.ResultRequiredDttm = now;
                    requestDetail.PatientOrderDetailUID = orderDetail.UID;



                    requestDetail.ORDSTUID = refValueORDST.FirstOrDefault(p => p.ValueCode == "EXCUT" && p.StatusFlag == "A").UID;
                    requestDetail.RQPRTUID = 440; //Priority Normal
                    requestDetail.ORDSTUID = orderDetail.ORDSTUID;
                    requestDetail.Comments = orderDetail.Comments;
                    requestDetail.RequestedUserUID = userUID;
                    requestDetail.RequestItemCode = requestItem.Code;
                    requestDetail.RequestItemName = requestItem.ItemName;
                    requestDetail.ProcessingNote = inboundData.RequestNote;
                    requestDetail.MUser = userUID;
                    requestDetail.MWhen = now;

                    requestDetail.OwnerOrganisationUID = organisationUID;
                    requestDetail.NetworkPartnerID = inboundData.NetworkPartnerID;

                    DataTable dtSession = SqlStatement.GetRadiologistSession(requestDetail.UID);
                    if (dtSession != null && dtSession.Rows.Count > 0)
                    {
                        List<SessionDefinitionModel> sessionRadio = dtSession.ToList<SessionDefinitionModel>();
                        int radiologistUID = sessionRadio.OrderBy(p => p.RequestNumber).FirstOrDefault().CareproviderUID;
                        requestDetail.RadiologistUID = radiologistUID;
                    }

                    dbMediTech.RequestDetail.Add(requestDetail);
                    dbMediTech.SaveChanges();

                    dbMediTech.PatientOrderDetail.Attach(orderDetail);
                    orderDetail.IdentifyingType = "REQUESTDETAIL";
                    orderDetail.IdentifyingUID = requestDetail.UID;
                    dbMediTech.SaveChanges();

                    if (inboundData.DocumentContent != null)
                    {
                        RequestDetailDocument document = new RequestDetailDocument();
                        document.RequestDeailUID = requestDetail.UID;
                        document.DocumentName = inboundData.DocumentName;
                        document.DocumentContent = inboundData.DocumentContent;
                        document.CUser = userUID;
                        document.CWhen = now;
                        document.MUser = userUID;
                        document.MWhen = now;
                        document.StatusFlag = "A";
                        dbMediTech.RequestDetailDocument.Add(document);
                        dbMediTech.SaveChanges();

                    }

                    tran.Complete();
                }

                using (var tran = new TransactionScope())
                {
                    int rows;
                    int columns;
                    MemoryStream msDicom = new MemoryStream(inboundData.DicomFile);
                    DicomFile dicomFile = Dicom.DicomFile.Open(msDicom);

                    string studyInstanceUID = dicomFile.Dataset.GetSingleValue<string>(DicomTag.StudyInstanceUID);
                    string seriesInstanceUID = dicomFile.Dataset.GetSingleValue<string>(DicomTag.SeriesInstanceUID);
                    string sopInstanceUID = dicomFile.Dataset.GetSingleValue<string>(DicomTag.SOPInstanceUID);
                    rows = dicomFile.Dataset.GetSingleValue<int>(DicomTag.Rows);
                    columns = dicomFile.Dataset.GetSingleValue<int>(DicomTag.Columns);
                    //var pixelData = dicomFile.Dataset.GetValue<FileByteBuffer>(DicomTag.PixelData, 0);
                    long pixelPosition = 2500;
                    IByteBuffer buffer = null;
                    var dataset = dicomFile.Dataset;
                    var pixelData = dataset.Get<DicomItem>(DicomTag.PixelData);
                    if (pixelData is DicomOtherByte)
                    {
                        buffer = (pixelData as DicomOtherByte).Buffer;
                        if (buffer is FileByteBuffer)
                        {
                            pixelPosition = (buffer as FileByteBuffer).Position;
                        }
                        else if (buffer is StreamByteBuffer)
                        {
                            pixelPosition = (buffer as StreamByteBuffer).Position;
                        }
                    }
                    else if (pixelData is DicomOtherWord)
                    {
                        buffer = (pixelData as DicomOtherWord).Buffer;
                        if (buffer is FileByteBuffer)
                        {
                            pixelPosition = (buffer as StreamByteBuffer).Position;
                        }
                        else if (buffer is StreamByteBuffer)
                        {
                            pixelPosition = (buffer as StreamByteBuffer).Position;
                        }
                    }
                    else // pixel data is fragmented.
                    {
                        buffer = null;
                    }
                    if (buffer != null)
                    {
                        ushort[] pixels = Dicom.IO.ByteConverter.ToArray<ushort>(buffer);
                    }

                    SaveDicomData(dicomFile.Dataset, msDicom.Length, columns, rows, pixelPosition);
                    SaveDicomFile(studyInstanceUID, seriesInstanceUID, sopInstanceUID, dicomFile);

                    tran.Complete();
                }



                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message, ex);
            }
        }


        public void SaveDicomFile(string studyInstanceUID, string seriesInstanceUID, string sopInstanceUID, DicomFile dicomFile)
        {
            try
            {
                PACSFunctions.SaveDicomFile(studyInstanceUID, seriesInstanceUID, sopInstanceUID, dicomFile);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void SaveDicomData(DicomDataset dicomDataSet, long fileSize, int width, int height, long pixelData)
        {
            try
            {
                DateTime now = DateTime.Now;
                Patients pat = dbPACS.Patients.Find(dicomDataSet.GetSingleValue<string>(DicomTag.PatientID));
                if (pat == null)
                {
                    pat = new Patients();
                    pat.PatientID = dicomDataSet.GetSingleValue<string>(DicomTag.PatientID);
                }

                pat.PatientName = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.PatientName, null);
                pat.PatientComments = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.PatientComments, null);
                pat.PatientBirthDate = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.PatientBirthDate, null);
                pat.PatientSex = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.PatientSex, null);
                pat.NumberOfPatientRelatedStudies = dicomDataSet.GetSingleValueOrDefault<int>(DicomTag.NumberOfPatientRelatedStudies, 1);
                pat.NumberOfPatientRelatedSeries = dicomDataSet.GetSingleValueOrDefault<int>(DicomTag.NumberOfPatientRelatedSeries, 1);
                pat.NumberOfPatientRelatedInstances = dicomDataSet.GetSingleValueOrDefault<int>(DicomTag.NumberOfPatientRelatedInstances, 1);
                pat.SpecificCharacterSet = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.SpecificCharacterSet, null);
                pat.Edited = now;

                dbPACS.Patients.AddOrUpdate(pat);
                dbPACS.SaveChanges();

                Studies studie = dbPACS.Studies.Find(dicomDataSet.GetSingleValue<string>(DicomTag.StudyInstanceUID));

                if (studie == null)
                {
                    studie = new Studies();
                    studie.StudyInstanceUID = dicomDataSet.GetSingleValue<string>(DicomTag.StudyInstanceUID);
                }
                studie.StudyInstanceUID = dicomDataSet.GetSingleValue<string>(DicomTag.StudyInstanceUID);
                studie.PatientID = dicomDataSet.GetSingleValue<string>(DicomTag.PatientID);
                studie.PatientName = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.PatientName, null);
                //studie.PhotoMatricInterpretation = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.PerformingPhysicianName, null);
                //studie.ReferringPhysicianName = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.ReferringPhysicianName, null);
                studie.PatientComments = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.PatientComments, null);
                studie.PatientBirthDate = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.PatientBirthDate, null);
                studie.PatientSex = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.PatientSex, null);
                studie.StudyDescription = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.StudyDescription, null);
                studie.StudyID = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.StudyID, null);
                studie.StudyDate = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.StudyDate, null);
                studie.StudyTime = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.StudyTime, null);
                studie.AccessionNumber = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.AccessionNumber, null);
                studie.InstitutionName = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.InstitutionName, null);
                studie.ModalitiesInStudy = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.Modality, null);
                studie.BodyPartsInStudy = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.BodyPartExamined, null);
                studie.NumberOfStudyRelatedSeries = dicomDataSet.GetSingleValueOrDefault<int>(DicomTag.NumberOfStudyRelatedSeries, 1);
                studie.NumberOfStudyRelatedInstances = dicomDataSet.GetSingleValueOrDefault<int>(DicomTag.NumberOfStudyRelatedInstances, 1);
                studie.SpecificCharacterSet = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.SpecificCharacterSet, null);
                studie.PatientAge = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.PatientAge, null);
                studie.ReportStatus = 0;
                studie.Edited = now;

                dbPACS.Studies.AddOrUpdate(studie);
                dbPACS.SaveChanges();

                Series series = dbPACS.Series.Find(dicomDataSet.GetSingleValue<string>(DicomTag.SeriesInstanceUID));

                if (series == null)
                {
                    series = new Series();
                    series.SeriesInstanceUID = dicomDataSet.GetSingleValue<string>(DicomTag.SeriesInstanceUID);
                }
                series.SeriesInstanceUID = dicomDataSet.GetSingleValue<string>(DicomTag.SeriesInstanceUID);
                series.StudyInstanceUID = dicomDataSet.GetSingleValue<string>(DicomTag.StudyInstanceUID);
                series.PatientID = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.PatientID, null);
                series.SeriesNumber = dicomDataSet.GetSingleValueOrDefault<int?>(DicomTag.SeriesNumber, null);
                series.SeriesDate = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.SeriesDate, null);
                series.SeriesTime = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.SeriesTime, null);
                series.SeriesDescription = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.SeriesDescription, null);
                series.Modality = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.Modality, null);
                series.BodypartExamined = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.BodyPartExamined, null);
                series.NumberOfSeriesRelatedInstances = dicomDataSet.GetSingleValueOrDefault<int>(DicomTag.NumberOfSeriesRelatedInstances, 1);
                series.SpecificCharacterSet = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.SpecificCharacterSet, null);
                series.Edited = now;

                dbPACS.Series.AddOrUpdate(series);
                dbPACS.SaveChanges();

                Instances instances = dbPACS.Instances.Find(dicomDataSet.GetSingleValue<string>(DicomTag.SOPInstanceUID));
                if (instances == null)
                {
                    instances = new Instances();
                    instances.SOPInstanceUID = dicomDataSet.GetSingleValue<string>(DicomTag.SOPInstanceUID);
                }
                instances.SOPInstanceUID = dicomDataSet.GetSingleValue<string>(DicomTag.SOPInstanceUID);
                instances.SeriesInstanceUID = dicomDataSet.GetSingleValue<string>(DicomTag.SeriesInstanceUID);
                instances.StudyInstanceUID = dicomDataSet.GetSingleValue<string>(DicomTag.StudyInstanceUID);
                instances.SOPClassUID = dicomDataSet.GetSingleValue<string>(DicomTag.SOPClassUID);
                instances.TransferSyntaxUID = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.TransferSyntaxUID, DicomUID.ExplicitVRLittleEndian.UID);
                instances.PatientID = dicomDataSet.GetSingleValue<string>(DicomTag.PatientID);
                instances.PatientName = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.PatientName, null);
                //instances.ReferringPhysicianName = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.ReferringPhysicianName, null);
                instances.PhotoMatricInterpretation = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.PerformingPhysicianName, null);
                instances.PatientComments = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.PatientComments, null);

                instances.PatientBirthDate = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.PatientBirthDate, null);
                instances.PatientSex = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.PatientSex, null);
                instances.StudyDescription = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.StudyDescription, null);
                instances.StudyID = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.StudyID, null);
                instances.StudyDate = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.StudyDate, null);
                instances.StudyTime = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.StudyTime, null);
                instances.AccessionNumber = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.AccessionNumber, null);
                instances.InstitutionName = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.InstitutionName, null);
                instances.SeriesNumber = dicomDataSet.GetSingleValueOrDefault<int?>(DicomTag.SeriesNumber, null);


                instances.SeriesDate = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.SeriesDate, null);
                instances.SeriesTime = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.SeriesTime, null);
                instances.SeriesDescription = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.SeriesDescription, null);
                instances.Modality = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.Modality, null);
                instances.BodypartExamined = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.BodyPartExamined, null);
                instances.InstanceNumber = dicomDataSet.GetSingleValueOrDefault<int?>(DicomTag.InstanceNumber, null);
                instances.NumberOfFrames = dicomDataSet.GetSingleValueOrDefault<int?>(DicomTag.NumberOfFrames, null);


                string pixelSpacing = "";
                dicomDataSet.TryGetString(DicomTag.PixelSpacing, out pixelSpacing);
                instances.PixelSpacing = pixelSpacing;

                instances.PixelAspectRatio = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.PixelAspectRatio, null);
                instances.SliceThickness = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.SliceThickness, null);
                instances.SpacingBetweenSlices = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.SpacingBetweenSlices, null);
                instances.SliceLocation = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.SliceLocation, null);
                instances.PatientOrientation = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.PatientOrientation, null);
                instances.ImagePositionPatient = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.ImagePositionPatient, null);
                instances.ImageOrientationPatient = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.ImageOrientationPatient, null);
                instances.Width = width;
                instances.Height = height;
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
                instances.PixelData = dicomDataSet.GetSingleValueOrDefault<int?>(DicomTag.PixelData, null);
                instances.FileSize = fileSize;
                instances.Created = now;
                instances.SpecificCharacterSet = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.SpecificCharacterSet, null);
                instances.ImageType = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.ImageType, "ORIGINAL\\PRIMARY");
                instances.PhotoMatricInterpretation = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.PhotometricInterpretation, null);


                instances.PatientAge = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.PatientAge, null);
                instances.LossyImageCompression = dicomDataSet.GetSingleValueOrDefault<string>(DicomTag.LossyImageCompression, null);
                instances.PixelData = Convert.ToInt32(pixelData);
                instances.Method = dicomDataSet.GetSingleValueOrDefault<int>(DicomTag.LossyImageCompressionMethod, 32);

                dbPACS.Instances.AddOrUpdate(instances);
                dbPACS.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public BillableItemDetailModel GetBillableItemPrice(List<BillableItemDetailModel> billItmDetail, int ownerOrganisationUID)
        {
            BillableItemDetailModel selectBillItemDetail = null;

            if (billItmDetail.Count(p => p.OwnerOrganisationUID == ownerOrganisationUID) > 0)
            {
                selectBillItemDetail = billItmDetail
                    .FirstOrDefault(p => p.StatusFlag == "A" && p.OwnerOrganisationUID == ownerOrganisationUID
                    && (p.ActiveFrom == null || (p.ActiveFrom.HasValue && p.ActiveFrom.Value.Date >= DateTime.Now.Date))
                    && (p.ActiveTo == null || (p.ActiveTo.HasValue && p.ActiveTo.Value.Date <= DateTime.Now.Date))
                    );
            }
            else
            {
                selectBillItemDetail = billItmDetail
    .FirstOrDefault(p => p.StatusFlag == "A" && p.OwnerOrganisationUID == 0
    && (p.ActiveFrom == null || (p.ActiveFrom.HasValue && p.ActiveFrom.Value.Date <= DateTime.Now.Date))
    && (p.ActiveTo == null || (p.ActiveTo.HasValue && p.ActiveTo.Value.Date >= DateTime.Now.Date))
    );
            }

            return selectBillItemDetail;
        }


        public string GetAccessionNumber(int OwnerOrganisationUID)
        {
            DateTime now = DateTime.Now;
            string accessionNumber = string.Empty;
            accessionNumber = now.ToString("yyyyMMddHHmm");

            HealthOrganisation healthOrganisationCode = dbMediTech.HealthOrganisation.FirstOrDefault(p => p.UID == OwnerOrganisationUID);

            accessionNumber = (healthOrganisationCode != null ? healthOrganisationCode.Code : "00") + "-" + accessionNumber;

            return accessionNumber;
        }
        public static string GetSEQIDFormat(string seqTableName, out int outSeqUID)
        {
            MediTechEntities db = new MediTechEntities();
            string seqFormatID = string.Empty;
            string seqPreFix = string.Empty;

            DateTime now = DateTime.Now;

            SEQConfiguration sqlConfig = db.SEQConfiguration.FirstOrDefault(p => p.SEQTableName == seqTableName && p.StatusFlag == "A");
            seqFormatID = sqlConfig.IDFormat;
            seqPreFix = sqlConfig.SEQPrefix;

            int seqUID = SqlDirectStore.pGetSEQID(seqTableName) ?? 0;

            if (sqlConfig == null || seqUID == 0)
            {
                outSeqUID = 0;
                return seqFormatID;
            }


            //HealthOrganisation healthOrganisationCode = db.HealthOrganisation.FirstOrDefault(p => p.UID == ownerUID);

            outSeqUID = seqUID;

            if (!String.IsNullOrEmpty(seqFormatID))
            {
                if (seqFormatID.Contains("[YY]"))
                {
                    seqFormatID = seqFormatID.Replace("[YY]", now.Year.ToString().Substring(2, 2));
                }

                if (seqFormatID.Contains("[MM]"))
                {
                    seqFormatID = seqFormatID.Replace("[MM]", now.ToString("MM"));
                }

                if (seqFormatID.Contains("[DD]"))
                {
                    seqFormatID = seqFormatID.Replace("[DD]", now.ToString("dd"));
                }

                //if (seqFormatID.Contains("[Code]"))
                //{
                //    seqFormatID = seqFormatID.Replace("[Code]", (healthOrganisationCode != null && healthOrganisationCode.PrefixID.HasValue) ? healthOrganisationCode.PrefixID.Value.ToString("00") : "00");
                //}

                if (seqFormatID.Contains("[VisitID]"))
                {
                    seqFormatID = seqFormatID.Replace("[VisitID]", seqUID.ToString().PadLeft(sqlConfig.IDLength ?? 0, '0'));
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(seqPreFix))
                {
                    seqFormatID = seqPreFix + seqUID.ToString().PadLeft(sqlConfig.IDLength ?? 0, '0');
                }
                else
                {
                    seqFormatID = seqUID.ToString().PadLeft(sqlConfig.IDLength ?? 0, '0');
                }
            }



            return seqFormatID;
        }

        public static string GetSEQPayorBillNumber(string idFormat, int idLength, int numberValue)
        {
            MediTechEntities db = new MediTechEntities();
            string seqFormatID = idFormat;
            int length = idLength;
            int value = numberValue;

            DateTime now = DateTime.Now;

            if (seqFormatID.Contains("[YYYY]"))
            {
                seqFormatID = seqFormatID.Replace("[YYYY]", now.Year.ToString());
            }

            if (seqFormatID.Contains("[YY]"))
            {
                seqFormatID = seqFormatID.Replace("[YY]", now.Year.ToString().Substring(2, 2));
            }

            if (seqFormatID.Contains("[MM]"))
            {
                seqFormatID = seqFormatID.Replace("[MM]", now.ToString("MM"));
            }

            if (seqFormatID.Contains("[DD]"))
            {
                seqFormatID = seqFormatID.Replace("[DD]", now.ToString("dd"));
            }

            if (seqFormatID.Contains("[Number]"))
            {
                seqFormatID = seqFormatID.Replace("[Number]", numberValue.ToString().PadLeft(idLength, '0'));
            }

            return seqFormatID;
        }




        private T ByteArrayToObject<T>(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            T obj = (T)binForm.Deserialize(memStream);

            return obj;
        }
    }
}
