using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace EnterpriseManagementSystem.Application.Common.Localization
{
    public static class LocalizationHelper
    {
        public static string Localize(string? arValue, string? enValue)
        {
            var isArabic = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName
                .Equals("ar", StringComparison.OrdinalIgnoreCase);

            return isArabic
                ? arValue ?? enValue ?? string.Empty
                : enValue ?? arValue ?? string.Empty;
        }

       
    }

}
