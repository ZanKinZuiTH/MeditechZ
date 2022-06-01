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
    public class TechnicalService
    {
        #region Location
        public LocationModel GetLocationByUID(int locationUID)
        {
            string requestApi = string.Format("Api/Technical/GetLocationByUID?locationUID={0}", locationUID);
            LocationModel data = MeditechApiHelper.Get<LocationModel>(requestApi);
            return data;
        }

        public List<LocationModel> GetLocationByTypeUID(int locationTypeUID)
        {
            string requestApi = string.Format("Api/Technical/GetLocationByTypeUID?locationTypeUID={0}", locationTypeUID);
            List<LocationModel> data = MeditechApiHelper.Get<List<LocationModel>>(requestApi);
            return data;
        }

        public List<LocationModel> GetLocation()
        {
            List<LocationModel> data = MeditechApiHelper.Get<List<LocationModel>>("Api/Technical/GetLocation");
            return data;
        }

        public bool DeleteLocation(int LocationUID, int userID)
        {
            bool flag = false;

            try
            {
                string requestApi = string.Format("Api/Technical/DeleteLocation?LocationUID={0}&userID={1}", LocationUID, userID);
                MeditechApiHelper.Delete(requestApi);
                flag = true;
            }
            catch (Exception)
            {
                throw;
            }
            return flag;
        }

        public bool ManageLocation(LocationModel locationModel, int userID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/Technical/ManageLocation?userID={0}", userID);
                MeditechApiHelper.Post<LocationModel>(requestApi, locationModel);
                flag = true;
            }
            catch (Exception)
            {
                throw;
            }
            return flag;
        }

        #endregion

        public List<ReferenceDomainModel> SearchReferenceDomain(string domainCode, string description, string valueDescription)
        {
            string requestApi = string.Format("Api/Technical/SearchReferenceDomain?domainCode={0}&description={1}&valueDescription={2}", domainCode, description, valueDescription);
            List<ReferenceDomainModel> data = MeditechApiHelper.Get<List<ReferenceDomainModel>>(requestApi);
            return data;
        }
        public List<ReferenceDomainModel> GetReferenceDomain()
        {
            List<ReferenceDomainModel> data = MeditechApiHelper.Get<List<ReferenceDomainModel>>("Api/Technical/GetReferenceDomain");
            return data;
        }
        public ReferenceDomainModel GetReferenceDomainByUID(int referenceDomainUID)
        {
            string requestApi = string.Format("Api/Technical/GetReferenceDomainByUID?ReferenceDomainUID={0}", referenceDomainUID);
            ReferenceDomainModel data = MeditechApiHelper.Get<ReferenceDomainModel>(requestApi);
            return data;
        }
        public List<LookupReferenceValueModel> GetReferenceValueMany(string domainCode)
        {
            List<LookupReferenceValueModel> data = MeditechApiHelper.Get<List<LookupReferenceValueModel>>("Api/Technical/GetReferenceValueMany?domainCode=" + domainCode);
            return data;
        }

        public List<LookupReferenceValueModel> GetReferenceValueList(string domainCodeList)
        {
            string requestApi = string.Format("Api/Technical/GetReferenceValueList?domainCodeList={0}", domainCodeList);
            List<LookupReferenceValueModel> datalist = MeditechApiHelper.Get<List<LookupReferenceValueModel>>(requestApi);

            return datalist;
        }

        public LookupReferenceValueModel GetReferenceValueByCode(string domainCode, string valueCode)
        {
            string requestApi = string.Format("Api/Technical/GetReferenceValueByCode?domainCode={0}&valueCode={1}", domainCode, valueCode);
            LookupReferenceValueModel data = MeditechApiHelper.Get<LookupReferenceValueModel>(requestApi);
            return data;
        }

        public LookupReferenceValueModel GetReferenceValueByDescription(string domainCode, string description)
        {
            string requestApi = string.Format("Api/Technical/GetReferenceValueByDescription?domainCode={0}&description={1}", domainCode, description);
            LookupReferenceValueModel data = MeditechApiHelper.Get<LookupReferenceValueModel>(requestApi);
            return data;
        }

        public LookupReferenceValueModel GetReferenceValue(int referencevalueUID)
        {
            string requestApi = string.Format("Api/Technical/GetReferenceValue?referencevalueUID={0}", referencevalueUID);
            LookupReferenceValueModel data = MeditechApiHelper.Get<LookupReferenceValueModel>(requestApi);
            return data;
        }

        public List<ReferenceRelationShipModel> GetReferenceRealationShip(string sourceReferenceDomainCode, string targetReferenceDomainCode)
        {
            string requestApi = string.Format("Api/Technical/GetReferenceRealationShip?sourceReferenceDomainCode={0}&targetReferenceDomainCode={1}", sourceReferenceDomainCode, targetReferenceDomainCode);
            List<ReferenceRelationShipModel> data = MeditechApiHelper.Get<List<ReferenceRelationShipModel>>(requestApi);
            return data;
        }

        public bool SaveReferenceDomain(ReferenceDomainModel referenceDomain, int userID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/Technical/SaveReferenceDomain?userID={0}", userID);
                MeditechApiHelper.Post<ReferenceDomainModel>(requestApi, referenceDomain);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }
            return flag;
        }

        public bool DeleteRefereceDomain(int referenceDomainUID, int userID)
        {
            bool flag = false;
            try
            {
                string requestApi = string.Format("Api/Technical/DeleteReferenceDomain?referenceDomainUID={0}&userID={1}", referenceDomainUID, userID);
                MeditechApiHelper.Delete(requestApi);
                flag = true;
            }
            catch (Exception)
            {

                throw;
            }
            return flag;
        }

        public List<LookupItemModel> GetProvince()
        {
            string requestApi = "Api/Technical/GetProvine";
            List<LookupItemModel> data = MeditechApiHelper.Get<List<LookupItemModel>>(requestApi);

            return data;
        }

        public List<LookupItemModel> GetAmphurByPronvince(int provinceUID)
        {
            string requestApi = string.Format("Api/Technical/GetAmphurByPronvince?provinceUID={0}", provinceUID);
            List<LookupItemModel> data = MeditechApiHelper.Get<List<LookupItemModel>>(requestApi);

            return data;
        }

        public List<LookupReferenceValueModel> GetDistrictByAmphur(int amphurUID)
        {
            string requestApi = string.Format("Api/Technical/GetDistrictByAmphur?amphurUID={0}", amphurUID);
            List<LookupReferenceValueModel> data = MeditechApiHelper.Get<List<LookupReferenceValueModel>>(requestApi);

            return data;
        }

        public List<MediTech.Model.PostalCode> GetPostalCode(string postalCode)
        {
            string requestApi = string.Format("Api/Technical/GetPostalCode?postalCode={0}", postalCode);
            List<MediTech.Model.PostalCode> data = MeditechApiHelper.Get<List<MediTech.Model.PostalCode>>(requestApi);

            return data;
        }

        public string GetAccessionNumber(int ownerOrganisationUID)
        {
            string requestApi = string.Format("Api/Technical/GetAccessionNumber?OwnerOrganisationUID={0}", ownerOrganisationUID);

            string data = MeditechApiHelper.Get<string>(requestApi);

            return data;
        }

        public string CheckAccessionNumberInPacs(string accessionNumber)
        {
            string requestApi = string.Format("Api/Technical/CheckAccessionNumberInPacs?accessionNumber={0}", accessionNumber);
            string data = MeditechApiHelper.Get<string>(requestApi);

            return data;
        }

        public List<MediTechInterfechModel> GetMediTechInterfaceByCode(string code)
        {
            string requestApi = string.Format("Api/Technical/GetMediTechInterfaceByCode?code={0}", code);
            List<MediTechInterfechModel> data = MeditechApiHelper.Get<List<MediTechInterfechModel>>(requestApi);

            return data;
        }
    }
}
