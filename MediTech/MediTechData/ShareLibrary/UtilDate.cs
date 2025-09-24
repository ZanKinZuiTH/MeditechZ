using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareLibrary
{
    public static class UtilDate
    {
        public static String calAgeFromBirthDate(DateTime bdte)
        {
            string _xage = "0";
            int now = int.Parse(DateTime.Today.ToString("yyyyMMdd"));
            int dob = int.Parse(bdte.ToString("yyyyMMdd"));
            string dif = (now - dob).ToString();
            if (dif.Length > 4)
                _xage = dif.Substring(0, dif.Length - 4);
            return _xage;
        }

        public static String calAgeStringFromBirthDate(DateTime bdte)
        {
            string ageString = "";
            DateTime tempDate;
            int year = 0;
            int mounth = 0;
            int day;

            tempDate = bdte;

            year = DateTime.Now.Year - tempDate.Year;
            if (bdte.Month > DateTime.Now.Month)
            {
                year = year - 1;
            }
            else if (bdte.Month == DateTime.Now.Month && bdte.Day > DateTime.Now.Day)
            {
                year = year - 1;
            }

            tempDate = tempDate.AddYears(year);

            mounth = ((DateTime.Now.Year - tempDate.Year) * 12) + DateTime.Now.Month - tempDate.Month;

            if (bdte.Day > DateTime.Now.Day)
            {
                mounth = mounth - 1;
            }

            tempDate = tempDate.AddMonths(mounth);
            day = (DateTime.Now - tempDate).Days;

            ageString = year + "  Y  " + mounth + "  M  " + day + "  D  ";

            return ageString;
        }
        public static String getLastDayOfMonth(int mm, int yyyy)
        {
            DateTime xdte = new DateTime(yyyy, mm, 1).AddMonths(1);
            DateTime ldte = new DateTime(xdte.Year, xdte.Month, 1).AddDays(-1);
            return ldte.ToString("dd/MM/yyyy");
        }
        public static String getFirstDayOfMonth(int mm, int yyyy)
        {
            DateTime xdte = new DateTime(yyyy, mm, 1);
            return xdte.ToString("dd/MM/yyyy");
        }
        public static string ConverStringToTime(string pTime)
        {
            string xtime = pTime.Trim();
            if (xtime.Length > 0)
            {

                xtime = xtime.Replace('.', ':').Replace(',', ':');
                if (xtime.IndexOf(':') < 0) xtime = xtime + ":00";
                if (xtime.IndexOf(':') >= 0 && xtime.IndexOf(':') <= 1)
                {
                    xtime = xtime.Substring(0, xtime.IndexOf(':')).PadLeft(2, '0') + xtime.Substring(xtime.IndexOf(':')).PadRight(3, '0');
                }

            }
            else
            {
                return "";
            }
            if (xtime.Length >= 4 && xtime.Substring(2, 1) != ":")
            {
                xtime = xtime.Substring(0, 2) + ":" + xtime.Substring(2).Trim().PadLeft(2, '0');
            }
            else if (xtime.Length < 5)
            {
                xtime = xtime.PadRight(5, '0');
            }
            return xtime;
        }
        public static DateTime? ConvertStringToDate(string value, string format)
        {
            string strValue = value.ToString();
            if (strValue.Length > 10) strValue = strValue.Substring(0, 10);
            DateTime resultDateTime;
            //new CultureInfo("en-GB"),
            CultureInfo culture = CultureInfo.CurrentCulture;
            bool isDateTime;
            switch ((String)format)
            {
                case "DMY":
                    isDateTime = DateTime.TryParseExact(strValue, "dd/MM/yyyy", culture,
                            DateTimeStyles.None, out resultDateTime);
                    break;
                case "YMD":
                    isDateTime = DateTime.TryParseExact(strValue, "yyyy-MM-dd", culture,
                            DateTimeStyles.None, out resultDateTime);
                    break;
                default:
                    isDateTime = DateTime.TryParseExact(strValue, "dd/MM/yyyy", culture,
                            DateTimeStyles.None, out resultDateTime);
                    break;
            }
            if (isDateTime)
            {
                return resultDateTime;
            }
            else
            {
                return null;
            }

        }
    }

}
