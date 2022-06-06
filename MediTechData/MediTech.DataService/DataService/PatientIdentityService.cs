using MediTech.Model;
using ShareLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.DataService
{
    public class PatientIdentityService
    {
        #region Patient

        public PatientInformationModel GetPatientByHN(string HN)
        {
            string requestApi = string.Format("Api/PatientIdentity/GetPatientByHN?HN={0}", HN);
            PatientInformationModel data = MeditechApiHelper.Get<PatientInformationModel>(requestApi);

            return data;
        }

        public PatientInformationModel GetPatientByEmployeeID(string EmployeeID)
        {
            string requestApi = string.Format("Api/PatientIdentity/GetPatientByEmployeeId?EmployeeID={0}", EmployeeID);
            PatientInformationModel data = MeditechApiHelper.Get<PatientInformationModel>(requestApi);

            return data;
        }

        public List<PatientInformationModel> GetPatientByName(string firstName, string lastName)
        {
            string requestApi = string.Format("Api/PatientIdentity/GetPatientByName?firstName={0}&lastName={1}", firstName, lastName);
            List<PatientInformationModel> data = MeditechApiHelper.Get<List<PatientInformationModel>>(requestApi);

            return data;
        }

        public PatientInformationModel GetPatientByIDCard(string idCard)
        {
            string requestApi = string.Format("Api/PatientIdentity/GetPatientByIDCard?idCard={0}", idCard);
            PatientInformationModel data = MeditechApiHelper.Get<PatientInformationModel>(requestApi);

            return data;
        }

        public PatientInformationModel GetPatientByUID(long patientUID)
        {
            string requestApi = string.Format("Api/PatientIdentity/GetPatientByUID?patientUID={0}", patientUID);
            PatientInformationModel data = MeditechApiHelper.Get<PatientInformationModel>(requestApi);

            return data;
        }

        public List<PatientInformationModel> SearchPatient(string patientID, string firstName, string middleName, string lastName, string nickName, DateTime? birthDate, int? SEXXXUID, string idCard, DateTime? lastVisitDate, string mobilePhone)
        {
            if (patientID == string.Empty && firstName == string.Empty && middleName == string.Empty && lastName == string.Empty &&
                nickName == string.Empty && birthDate == null && SEXXXUID == null && idCard == string.Empty && lastVisitDate == null && mobilePhone == string.Empty)
            {
                return null;

            }
            string requestApi = string.Format("Api/PatientIdentity/SearchPatient?patientID={0}&firstName={1}&middleName={2}&lastName={3}&nickName={4}&birthDate={5:MM/dd/yyyy}&SEXXXUID={6}&idCard={7}&lastVisitDate={8:MM/dd/yyyy}&mobilePhone={9}", patientID, firstName, middleName, lastName, nickName, birthDate, SEXXXUID, idCard, lastVisitDate,mobilePhone);
            List<PatientInformationModel> data = MeditechApiHelper.Get<List<PatientInformationModel>>(requestApi);

            return data;
        }

        public List<PatientInformationModel> SearchPatientEmergency(string patientID, string firstName, string middleName, string lastName, string nickName, DateTime? birthDate, int? SEXXXUID, string idCard, DateTime? lastVisitDate, string mobilePhone)
        {
            if (patientID == string.Empty && firstName == string.Empty && middleName == string.Empty && lastName == string.Empty &&
                nickName == string.Empty && birthDate == null && SEXXXUID == null && idCard == string.Empty && lastVisitDate == null && mobilePhone == string.Empty)
            {
                return null;

            }
            string requestApi = string.Format("Api/PatientIdentity/SearchPatientEmergency?patientID={0}&firstName={1}&middleName={2}&lastName={3}&nickName={4}&birthDate={5:MM/dd/yyyy}&SEXXXUID={6}&idCard={7}&lastVisitDate={8:MM/dd/yyyy}&mobilePhone={9}", patientID, firstName, middleName, lastName, nickName, birthDate, SEXXXUID, idCard, lastVisitDate, mobilePhone);
            List<PatientInformationModel> data = MeditechApiHelper.Get<List<PatientInformationModel>>(requestApi);

            return data;
        }

        public PatientInformationModel RegisterPatient(PatientInformationModel patientInfo, int userID, int OwernID)
        {
            string requestApi = string.Format("Api/PatientIdentity/RegisterPatient?userID={0}&OwnerOrganisationUID={1}", userID, OwernID);
            PatientInformationModel result = MeditechApiHelper.Post<PatientInformationModel, PatientInformationModel>(requestApi, patientInfo);

            return result;
        }

        public PatientInformationModel RegisterPatientEmergency(PatientInformationModel patientInfo, int userID, int OwernID)
        {
            string requestApi = string.Format("Api/PatientIdentity/RegisterPatientEmergency?userID={0}&OwnerOrganisationUID={1}", userID, OwernID);
            PatientInformationModel result = MeditechApiHelper.Post<PatientInformationModel, PatientInformationModel>(requestApi, patientInfo);

            return result;
        }

        public PatientInformationModel GetPatientByPassportNo(string passportNo)
        {
            string requestApi = string.Format("Api/PatientIdentity/GetPatientByPassportNo?passportNo={0}", passportNo);
            PatientInformationModel data = MeditechApiHelper.Get<PatientInformationModel>(requestApi);

            return data;
        }

        public PatientInformationModel CheckDupicatePatient(string firstName, string lastName, DateTime? birthDate, int SEXXXUID)
        {
            string requestApi = string.Format("Api/PatientIdentity/CheckDupicatePatient?firstName={0}&lastName={1}&birthDate={2:MM/dd/yyyy}&SEXXXUID={3}", firstName, lastName, birthDate, SEXXXUID);
            PatientInformationModel result = MeditechApiHelper.Get<PatientInformationModel>(requestApi);

            return result;
        }

        public List<PatientInsuranceDetailModel> GetPatientInsuranceDetail(long patientUID)
        {
            string requestApi = string.Format("Api/PatientIdentity/GetPatientInsuranceDetail?patientUID={0}", patientUID);
            List<PatientInsuranceDetailModel> result = MeditechApiHelper.Get<List<PatientInsuranceDetailModel>>(requestApi);

            return result;
        }

        public bool ManagePatientInsuranceDetail(List<PatientVisitPayorModel> visitPayorList)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/PatientIdentity/ManagePatientInsuranceDetail");
                MeditechApiHelper.Post<List<PatientVisitPayorModel>>(requestApi, visitPayorList);
                flag = true;
            }
            catch (Exception)
            {
                throw;
            }
            return flag;
        }

        #endregion

        #region PatientVisit

        public List<PatientVisitModel> SearchPatientVisit(string hn, string firstName, string lastName, int? careproviderUID
                  , string statusList, DateTime? dateFrom, DateTime? dateTo, DateTime? arrivedDttm, int? ownerOrganisationUID
            , int? payorDetailUID,int? checkupJobUID)
        {
            string requestApi = string.Format("Api/PatientIdentity/SearchPatientVisit?hn={0}&firstName={1}&lastName={2}&careproviderUID={3}&statusList={4}&dateFrom={5:MM/dd/yyyy}&dateTo={6:MM/dd/yyyy}&arrivedDttm={7:MM/dd/yyyy}&ownerOrganisationUID={8}&payorDetailUID={9}&checkupJobUID={10}", hn, firstName, lastName, careproviderUID, statusList, dateFrom, dateTo, arrivedDttm, ownerOrganisationUID, payorDetailUID,checkupJobUID);
            List<PatientVisitModel> data = MeditechApiHelper.Get<List<PatientVisitModel>>(requestApi);

            return data;
        }

        public List<PatientVisitModel> SearchERPatientVisit(string hn, string firstName, string lastName, int? careproviderUID
                  , string statusList, DateTime? dateFrom, DateTime? dateTo, DateTime? arrivedDttm, int? ownerOrganisationUID
            , int? payorDetailUID, int? checkupJobUID)
        {
            string requestApi = string.Format("Api/PatientIdentity/SearchERPatientVisit?hn={0}&firstName={1}&lastName={2}&careproviderUID={3}&statusList={4}&dateFrom={5:MM/dd/yyyy}&dateTo={6:MM/dd/yyyy}&arrivedDttm={7:MM/dd/yyyy}&ownerOrganisationUID={8}&payorDetailUID={9}&checkupJobUID={10}", hn, firstName, lastName, careproviderUID, statusList, dateFrom, dateTo, arrivedDttm, ownerOrganisationUID, payorDetailUID, checkupJobUID);
            List<PatientVisitModel> data = MeditechApiHelper.Get<List<PatientVisitModel>>(requestApi);

            return data;
        }

        public PatientAEAdmissionModel ManageEmergencyAE(PatientAEAdmissionModel model, int userUID)
        {
            PatientAEAdmissionModel returnData = null;
            try
            {
                string requestApi = string.Format("Api/PatientIdentity/ManageEmergencyAE?userUID={0}", userUID);
                returnData = MeditechApiHelper.Post<PatientAEAdmissionModel, PatientAEAdmissionModel>(requestApi, model);
            }
            catch (Exception)
            {
                throw;
            }
            return returnData;
        }

        public AEDischargeEventModel SaveAEDischargeEvent(AEDischargeEventModel model, int userUID)
        {
            AEDischargeEventModel returnData = null;
            try
            {
                string requestApi = string.Format("Api/PatientIdentity/SaveAEDischargeEvent?userUID={0}", userUID);
                returnData = MeditechApiHelper.Post<AEDischargeEventModel, AEDischargeEventModel>(requestApi, model);
            }
            catch (Exception)
            {
                throw;
            }
            return returnData;
        }

        public IPBookingModel SaveIPBooking(IPBookingModel model, int userUID)
        {
            IPBookingModel returnData = null;
            try
            {
                string requestApi = string.Format("Api/PatientIdentity/SaveIPBooking?userUID={0}", userUID);
                returnData = MeditechApiHelper.Post<IPBookingModel, IPBookingModel>(requestApi, model);
            }
            catch (Exception)
            {
                throw;
            }
            return returnData;
        }

        public List<PatientVisitModel> SearchPatientMedicalDischarge(string hn, string firstName, string lastName, int? careproviderUID,
            DateTime? dateFrom, DateTime? dateTo, int? ownerOrganisationUID, int? payorDetailUID)
        {
            string requestApi = string.Format("Api/PatientIdentity/SearchPatientMedicalDischarge?hn={0}&firstName={1}&lastName={2}&careproviderUID={3}&dateFrom={4:MM/dd/yyyy}&dateTo={5:MM/dd/yyyy}&ownerOrganisationUID={6}&payorDetailUID={7}", hn, firstName, lastName, careproviderUID, dateFrom, dateTo, ownerOrganisationUID, payorDetailUID);
            List<PatientVisitModel> data = MeditechApiHelper.Get<List<PatientVisitModel>>(requestApi);

            return data;
        }

        public PatientVisitModel SavePatientVisit(PatientVisitModel patientVisitInfo, int userID)
        {
            PatientVisitModel returnData;
            try
            {
                string requestApi = string.Format("Api/PatientIdentity/SavePatientVisit?userID={0}", userID);
                returnData = MeditechApiHelper.Post<PatientVisitModel, PatientVisitModel>(requestApi, patientVisitInfo);
            }
            catch (Exception)
            {
                throw;
            }
            return returnData;
        }

        public PatientVisitModel SaveERPatientVisit(PatientVisitModel patientVisitInfo, int userID)
        {
            PatientVisitModel returnData;
            try
            {
                string requestApi = string.Format("Api/PatientIdentity/SaveERPatientVisit?userID={0}", userID);
                returnData = MeditechApiHelper.Post<PatientVisitModel, PatientVisitModel>(requestApi, patientVisitInfo);
            }
            catch (Exception)
            {
                throw;
            }
            return returnData;
        }

        public void ModifyPatientVisit(PatientVisitModel patientVisitInfo, int userID)
        {
            try
            {
                string requestApi = string.Format("Api/PatientIdentity/ModifyPatientVisit?userID={0}", userID);
                MeditechApiHelper.Put<PatientVisitModel>(requestApi, patientVisitInfo);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public void CancelVisit(long patientVisitUID, int userID)
        {
            try
            {
                string requestApi = string.Format("Api/PatientIdentity/CancelVisit?patientVisitUID={0}&userID={1}", patientVisitUID, userID);
                MeditechApiHelper.Put(requestApi);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PatientVisitModel GetPatientVisitByUID(long patientVisitUID)
        {
            string requestApi = string.Format("Api/PatientIdentity/GetPatientVisitByUID?patientVisitUID={0}", patientVisitUID);
            PatientVisitModel data = MeditechApiHelper.Get<PatientVisitModel>(requestApi);

            return data;
        }

        public PatientAEAdmissionModel GetPatientAEAdmissionByUID(long patientVisitUID)
        {
            string requestApi = string.Format("Api/PatientIdentity/GetPatientAEAdmissionByUID?patientVisitUID={0}", patientVisitUID);
            PatientAEAdmissionModel data = MeditechApiHelper.Get<PatientAEAdmissionModel>(requestApi);

            return data;
        }

        public List<PatientVisitModel> GetPatientVisitByPatientUID(long patientUID)
        {
            string requestApi = string.Format("Api/PatientIdentity/GetPatientVisitByPatientUID?patientUID={0}", patientUID);
            List<PatientVisitModel> data = MeditechApiHelper.Get<List<PatientVisitModel>>(requestApi);

            return data;
        }


        public List<PatientVisitModel> GetPatientVisitByVisitType(long patientUID, string visitType)
        {
            string requestApi = string.Format("Api/PatientIdentity/GetPatientVisitByVisitType?patientUID={0}&visitType={1}", patientUID, visitType);
            List<PatientVisitModel> data = MeditechApiHelper.Get<List<PatientVisitModel>>(requestApi);

            return data;
        }

        public PatientVisitModel GetLatestPatientVisit(long patientUID)
        {
            string requestApi = string.Format("Api/PatientIdentity/GetLatestPatientVisit?patientUID={0}", patientUID);
            PatientVisitModel data = MeditechApiHelper.Get<PatientVisitModel>(requestApi);

            return data;
        }

        public bool ChangeVisitStatus(long patientVisitUID, int VISTSUID, int? careProviderUID, DateTime? editDttm, int userID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/PatientIdentity/ChangeVisitStatus?patientVisitUID={0}&VISTSUID={1}&careProviderUID={2}&editDttm={3:MM/dd/yyyy HH:mm:ss}&userID={4}", patientVisitUID, VISTSUID, careProviderUID, editDttm, userID);
                MeditechApiHelper.Put(requestApi);
                flag = true;
            }
            catch (Exception)
            {
                throw;
            }
            return flag;
        }

        public List<LocationModel> GetBedByPatientVisit(int parentLocationUID)
        {
            string requestApi = string.Format("Api/PatientIdentity/GetBedByPatientVisit?parentLocationUID={0}", parentLocationUID);
            List<LocationModel> data = MeditechApiHelper.Get<List<LocationModel>>(requestApi);
            return data;
        }

        public List<LocationModel> GetBedWardView(int parentLocationUID)
        {
            string requestApi = string.Format("Api/PatientIdentity/GetBedWardView?parentLocationUID={0}", parentLocationUID);
            List<LocationModel> data = MeditechApiHelper.Get<List<LocationModel>>(requestApi);
            return data;
        }

        public List<LocationModel> GetBedLocation(int parentLocationUID, int? entypUID)
        {
            string requestApi = string.Format("Api/PatientIdentity/GetBedLocation?parentLocationUID={0}&entypUID={1}", parentLocationUID, entypUID);
            List<LocationModel> data = MeditechApiHelper.Get<List<LocationModel>>(requestApi);
            return data;
        }

        #endregion

        #region PatientBanner
        public PatientBannerModel GetPatientDataForBanner(long patientUID, long patientVisitUID)
        {
            string requestApi = string.Format("Api/PatientIdentity/GetPatientDataForBanner?patientUID={0}&patientVisitUID={1}", patientUID, patientVisitUID);
            PatientBannerModel data = MeditechApiHelper.Get<PatientBannerModel>(requestApi);

            return data;
        }
        #endregion

        #region PatientAllergy

        public List<PatientAllergyModel> GetPatientAllergyByPatientUID(long patientUID)
        {
            string requestApi = string.Format("Api/PatientIdentity/GetPatientAllergyByPatientUID?patientUID={0}", patientUID);
            List<PatientAllergyModel> data = MeditechApiHelper.Get<List<PatientAllergyModel>>(requestApi);

            return data;
        }

        public bool ManagePatientAllergy(long patientUID, List<PatientAllergyModel> model, int userUID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/PatientIdentity/ManagePatientAllergy?patientUID={0}&userUID={1}", patientUID, userUID);
                MeditechApiHelper.Post<List<PatientAllergyModel>>(requestApi, model);
                flag = true;
            }
            catch (Exception)
            {
                throw;
            }
            return flag;
        }

        public bool DeletePatientAllergy(int patientAllergyUID, int userUID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/PatientIdentity/DeletePatientAllergy?patientAllergyUID={0}&userUID={1}", patientAllergyUID, userUID);
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

        #region Booking
        public List<BookingModel> SearchBooking(DateTime? dateFrom, DateTime? dateTo, int? careproviderUID, long? patientUID, int? bookStatus, int? PATMSGUID, int? ownerOrganisationUID)
        {
            string requestApi = string.Format("Api/PatientIdentity/SearchBooking?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&careproviderUID={2}&patientUID={3}&bookStatus={4}&PATMSGUID={5}&ownerOrganisationUID={6}", dateFrom, dateTo, careproviderUID, patientUID, bookStatus, PATMSGUID, ownerOrganisationUID);
            List<BookingModel> data = MeditechApiHelper.Get<List<BookingModel>>(requestApi);

            return data;
        }

        public List<BookingModel> SearchBookingNotExistsVisit(DateTime? dateFrom, DateTime? dateTo, int? careproviderUID, long? patientUID, int? bookStatus, int? PATMSGUID, int? ownerOrganisationUID)
        {
            string requestApi = string.Format("Api/PatientIdentity/SearchBookingNotExistsVisit?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&careproviderUID={2}&patientUID={3}&bookStatus={4}&PATMSGUID={5}&ownerOrganisationUID={6}", dateFrom, dateTo, careproviderUID, patientUID, bookStatus, PATMSGUID, ownerOrganisationUID);
            List<BookingModel> data = MeditechApiHelper.Get<List<BookingModel>>(requestApi);

            return data;
        }

        public BookingModel GetBookingByUID(int bookingUID)
        {
            string requestApi = string.Format("Api/PatientIdentity/GetBookingByUID?bookingUID={0}", bookingUID);
            BookingModel data = MeditechApiHelper.Get<BookingModel>(requestApi);

            return data;
        }

        public BookingModel ManageBooking(BookingModel model)
        {
            BookingModel returnData = null;
            try
            {
                string requestApi = string.Format("Api/PatientIdentity/ManageBooking");
                returnData = MeditechApiHelper.Post<BookingModel, BookingModel>(requestApi, model);
            }
            catch (Exception)
            {
                throw;
            }
            return returnData;
        }
        public bool UpdateBookingArrive(int bookingUID, int userUID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/PatientIdentity/UpdateBookingArrive?bookingUID={0}&userUID={1}", bookingUID, userUID);
                MeditechApiHelper.Put(requestApi);
                flag = true;
            }
            catch (Exception)
            {
                throw;
            }
            return flag;
        }



        public bool CancelBooking(int bookingUID, int userUID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/PatientIdentity/CancelBooking?bookingUID={0}&userUID={1}", bookingUID, userUID);
                MeditechApiHelper.Put(requestApi);
                flag = true;
            }
            catch (Exception)
            {
                throw;
            }
            return flag;
        }

        #endregion

        #region PatientScannedDocument

        public void UploadedScannedDocument(PatientScannedDocumentModel patScanDoc, int userUID)
        {
            try
            {
                string requestApi = string.Format("Api/PatientIdentity/UploadedScannedDocument?userUID={0}", userUID);
                MeditechApiHelper.Post<PatientScannedDocumentModel>(requestApi, patScanDoc);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<PatientScannedDocumentModel> GetPatientScannedDocumentByPatientUID(long patientUID)
        {
            string requestApi = string.Format("Api/PatientIdentity/GetPatientScannedDocumentByPatientUID?patientUID={0}", patientUID);
            List<PatientScannedDocumentModel> data = MeditechApiHelper.Get<List<PatientScannedDocumentModel>>(requestApi);

            return data;
        }

        public byte[] GetPatientScannedDocumentContent(int patScanDocumentUID)
        {
            string requestApi = string.Format("Api/PatientIdentity/GetPatientScannedDocumentContent?patScanDocumentUID={0}", patScanDocumentUID);
            byte[] data = MeditechApiHelper.Get<byte[]>(requestApi);

            return data;
        }

        public void DeletePatientScannedDocument(int patientScannedUID, int userUID)
        {
            try
            {
                string requestApi = string.Format("Api/PatientIdentity/DeletePatientScannedDocument?patientScannedUID={0}&userUID={1}", patientScannedUID, userUID);
                MeditechApiHelper.Delete(requestApi);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region PatientMerge

        public List<PatientMergeModel> SearchPatientMerge(DateTime? dateFrom, DateTime? dateTo, long? patientUID, string isUnMerge)
        {
            try
            {
                string requestApi = string.Format("Api/PatientIdentity/SearchPatientMerge?dateFrom={0:MM/dd/yyyy}&dateTo={1:MM/dd/yyyy}&patientUID={2}&isUnMerge={3}", dateFrom, dateTo, patientUID, isUnMerge);
                List<PatientMergeModel> data = MeditechApiHelper.Get<List<PatientMergeModel>>(requestApi);

                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void MergePatient(long majorPatientUID, long minorPatientUID, char address, char gender, char phone, char photo, char blood, int userUID)
        {
            try
            {
                string requestApi = string.Format("Api/PatientIdentity/MergePatient?majorPatientUID={0}&minorPatientUID={1}&address={2}&gender={3}&phone={4}&photo={5}&blood={6}&userUID={7}"
                    , majorPatientUID, minorPatientUID, address, gender, phone, photo, blood, userUID);
                MeditechApiHelper.Put(requestApi);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UnMergePatient(int patientMergeUID, int userUID)
        {
            try
            {
                string requestApi = string.Format("Api/PatientIdentity/UnMergePatient?patientMergeUID={0}&userUID={1}"
                    , patientMergeUID, userUID);
                MeditechApiHelper.Put(requestApi);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void EncounterMergePatient(long majorPatientUID, long minorPatientUID, string minorVisitUIDS, int userUID)
        {
            try
            {
                string requestApi = string.Format("Api/PatientIdentity/EncounterMergePatient?majorPatientUID={0}&minorPatientUID={1}&minorVisitUIDS={2}&userUID={3}"
                    , majorPatientUID, minorPatientUID, minorVisitUIDS, userUID);
                MeditechApiHelper.Put(requestApi);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region IPD

        public List<IPBookingModel> SearchIPBooking(string patientID, DateTime? dateFrom, DateTime? dateTo, int? bktypUID, int? wardUID)
        {
            string requestApi = string.Format("Api/PatientIdentity/SearchIPBooking?patientID={0}&dateFrom={1:MM/dd/yyyy}&dateTo={2:MM/dd/yyyy}&bktypUID={3}&wardUID={4}", patientID, dateFrom, dateTo, bktypUID, wardUID);
            List<IPBookingModel> data = MeditechApiHelper.Get<List<IPBookingModel>>(requestApi);

            return data;
        }

        public IPBookingModel GetIPBookingByVisitUID(long patientVisitUID)
        {
            string requestApi = string.Format("Api/PatientIdentity/GetIPBookingByVisitUID?patientVisitUID={0}", patientVisitUID);
            IPBookingModel data = MeditechApiHelper.Get<IPBookingModel>(requestApi);
            return data;
        }

        public void DropIPBooking(long ipBookingUID, int userUID)
        {
            try
            {
                string requestApi = string.Format("Api/PatientIdentity/DropIPBooking?ipBookingUID={0}&userUID={1}"
                    , ipBookingUID, userUID);
                MeditechApiHelper.Put(requestApi);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public AdmissionEventModel ManageAdmissionEvent(AdmissionEventModel model, int userUID)
        {
            AdmissionEventModel returnData = null;
            try
            {
                string requestApi = string.Format("Api/PatientIdentity/ManageAdmissionEvent?userUID={0}", userUID);
                returnData = MeditechApiHelper.Post<AdmissionEventModel, AdmissionEventModel>(requestApi, model);
            }
            catch (Exception)
            {
                throw;
            }
            return returnData;
        }

        #endregion

        #region Patient DemographicLog
        public List<PatientDemographicLogModel> GetPatientDemographicLogByUID(long patientUID)
        {
            string requestApi = string.Format("Api/PatientIdentity/GetPatientDemographicLogByUID?patientUID={0}", patientUID);
            List<PatientDemographicLogModel> data = MeditechApiHelper.Get<List<PatientDemographicLogModel>>(requestApi);
            return data;
        }

        #endregion
    }
}
