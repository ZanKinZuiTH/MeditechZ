using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Helpers
{
    public class TranslateResult
    {
        public static string TranslateResultXray(string xrayResultText, string resultStatus, List<XrayTranslateMappingModel> dtResultMapping, ref List<string> listNoMapResult)
        {

            string thaiResult = string.Empty;

            List<string> NoMapResult = new List<string>();

            //if (resultStatus == "Normal")
            //{
            //    thaiResult = "ปกติ";
            //    return thaiResult;
            //}

            List<string> plainTextList;
            if (xrayResultText.ToUpper().Contains("IMPRESSION") || xrayResultText.ToUpper().Contains("CONCLUSION"))
            {
                int startIndex;
                if (xrayResultText.ToUpper().Contains("IMPRESSION"))
                {
                    startIndex = xrayResultText.ToUpper().IndexOf("IMPRESSION");
                }
                else
                {
                    startIndex = xrayResultText.ToUpper().IndexOf("CONCLUSION");
                }
                int endIndex = xrayResultText.Length - startIndex;
                string subString = xrayResultText.Substring(startIndex, endIndex);
                //plainTextList = subString.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Replace(".", "").Replace("-", "").Trim()).ToList();
                plainTextList = subString.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).Where(p => !string.IsNullOrEmpty(p)).ToList();
            }
            else
            {
                //plainTextList = xrayResultText.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Replace(".", "").Replace("-", "").Trim()).ToList();
                plainTextList = xrayResultText.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).Where(p => !string.IsNullOrEmpty(p)).ToList();
            }

            string tempResult = string.Empty;
            bool isSet = false;
            int numKeyword = 0;
            foreach (string textResult in plainTextList)
            {
                string formatText = textResult;

                if (formatText.StartsWith("-"))
                {
                    formatText = formatText.Remove(0, 1);
                }

                if (formatText.StartsWith("."))
                {
                    formatText = formatText.Remove(0, 1);
                }

                if (formatText.EndsWith("."))
                {
                    formatText = formatText.Remove(formatText.Length - 1, 1);
                }

                if (formatText.EndsWith(":"))
                {
                    formatText = formatText.Remove(formatText.IndexOf(":"), 1);
                }

                if (formatText.EndsWith("\\"))
                {
                    formatText = formatText.Remove(formatText.IndexOf("\\"), 1);
                }

                formatText = formatText.Trim();

                //            if (formatText.ToLower() == "negative study"
                //|| formatText.ToLower() == "normal study"
                //|| formatText.ToLower() == "negative chest")
                //            {
                //                thaiResult = "ปกติ";
                //                return thaiResult;
                //            }

                bool isConvert = false;
                bool isKeyword = false;

                XrayTranslateMappingModel row = dtResultMapping.FirstOrDefault(p => p.EngResult.ToString().ToLower() == formatText.ToLower());
                if (row != null)
                {
                    if (Convert.ToBoolean(row.IsKeyword) == true || row.ThaiResult.ToString().Trim() == "ปกติ")
                    {
                        isKeyword = true;
                        numKeyword++;
                    }
                    else if (row.ThaiResult.ToString().Trim() != "ปกติ")
                    {
                        if (tempResult == "")
                        {
                            tempResult = row.ThaiResult.ToString();
                        }
                        else
                        {
                            tempResult += "," + row.ThaiResult.ToString();
                        }

                    }

                    isConvert = true;
                }
                //foreach (DataRow row in dt.Rows)
                //{
                //    if (formatText.ToLower().Trim() == row["EngResult"].ToString().ToLower().Trim())
                //    {
                //        if (Convert.ToBoolean(row["IsKeyword"]) == true)
                //        {
                //            isKeyword = true;
                //            numKeyword++;
                //            break;
                //        }
                //        if (row["ThaiResult"].ToString() != "ปกติ")
                //        {
                //            if (tempResult == "")
                //            {
                //                tempResult = row["ThaiResult"].ToString();
                //            }
                //            else
                //            {
                //                tempResult += "," + row["ThaiResult"].ToString();
                //            }

                //        }

                //        isConvert = true;
                //    }
                //}

                if (isKeyword == false)
                {
                    if (isConvert == false)
                    {
                        if (!System.Text.RegularExpressions.Regex.IsMatch(formatText, "[ก-๙]+"))
                        {
                            NoMapResult.Add(formatText);
                            isSet = true;
                        }
                    }
                }


            }

            if (tempResult != "" && isSet == false)
            {
                thaiResult = tempResult;
            }
            else if ((tempResult == "" && isSet == false) && (numKeyword == plainTextList.Count) && (resultStatus.ToLower() == "normal"))
            {
                thaiResult = "ปกติ";
            }

            listNoMapResult = NoMapResult.Distinct().Where(p => p != string.Empty).ToList<string>();

            return thaiResult;
        }
    }
}
