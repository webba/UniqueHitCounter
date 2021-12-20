using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BlazorApp.Shared
{
    public static class TextUtil
    { 
        public static string WordToUpper(string str)
        {
            if (str == null)
            {
                return null;
            }
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            return textInfo.ToTitleCase(str);
        }
    }
}
