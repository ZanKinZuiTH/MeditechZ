using MediTech.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MediTechWebApi
{
    public static class SEQHelper
    {
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

        public static string GetSEQBillNumber(string idFormat, int idLength, int numberValue)
        {
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
    }
}