using System;
using System.Configuration;

namespace Presentation.Helpers
{
    public static class AppSettings
    {
        public static string DefaultCountryName
        {
            get
            {
                return ConfigurationManager.AppSettings["DefaultCountryName"];
            }
        }

        public static int MinimumDrivingAge
        {
            get
            {
                string value = ConfigurationManager.AppSettings["MinimumDrivingAge"];
                return int.TryParse(value, out int result) ? result : 18;
            }
        }

        public static string PersonImagesDirectory
        {
            get
            {
                return ConfigurationManager.AppSettings["PersonImagesDirectory"]
                    ?? @"C:\DVLD-People-Images";
            }
        }
    }
}
