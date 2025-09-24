using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ShareLibrary
{
    public static class CheckValidate
    {
        public static bool IsNumber(string text)
        {
            Regex regex = new Regex("^[0-9]+$");
            if (!string.IsNullOrEmpty(text))
            {
                return regex.IsMatch(text);
            }
            else
            {
                return false;
            }

        }
        public static bool IsMail(string text)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            return regex.IsMatch(text);

        }
        public static bool VarifyPeopleID(string PID)
        {
            string digit = string.Empty;
            int sumValue = 0;
            for (int i = 0; i < PID.Length - 1; i++)
                sumValue += int.Parse(PID[i].ToString()) * (13 - i);
            int v = 11 - (sumValue % 11);
            if (v.ToString().Length == 2)
            {


                digit = v.ToString().Substring(1, 1);
            }
            else
            {
                digit = v.ToString();
            }
            return PID[12].ToString() == digit;
        }
    }
}
