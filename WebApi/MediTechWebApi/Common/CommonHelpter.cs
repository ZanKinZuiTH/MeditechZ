using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MediTechWebApi.Common
{
    public static class CommonHelpter
    {
        public static string SetItemNameSearch(string itemName)
        {
            itemName = itemName.Replace(",", "");
            itemName = itemName.Replace(".", "");
            itemName = itemName.Replace("(", "");
            itemName = itemName.Replace(")", "");
            itemName = itemName.Replace(";", "");
            itemName = itemName.Replace("#", "");
            itemName = itemName.Replace("<", "");
            itemName = itemName.Replace(">", "");
            itemName = itemName.Replace("?", "");
            itemName = itemName.Replace("\"", "");
            itemName = itemName.Replace("*", "");
            itemName = itemName.Replace("&", "");
            itemName = itemName.Replace("^", "");
            itemName = itemName.Replace("$", "");
            itemName = itemName.Replace("@", "");
            itemName = itemName.Replace("!", "");
            itemName = itemName.Replace("|", "");
            itemName = itemName.Replace("}", "");
            itemName = itemName.Replace("{", "");
            itemName = itemName.Replace(":", "");
            itemName = itemName.Replace("\\", "");
            itemName = itemName.Replace("/", "");
            itemName = itemName.Replace(" ", "");
            itemName = itemName.Replace("	", "");
            itemName = itemName.Replace("-", "");
            itemName = itemName.Replace("+", "");
            itemName = itemName.Replace("=", "");
            itemName = itemName.Replace("_", "");

            return itemName;
        }
    }
}