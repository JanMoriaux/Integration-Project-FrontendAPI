using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEndAPI.Utility
{
    public class DateUtility
    {
        public static string ConvertToStringWithZoneOffset(DateTime dt)
        {
            try
            {
                return dt.ToString("yyyy-MM-ddTHH:mm:sszzz");
            }
            catch (FormatException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return null;
            }
            catch (ArgumentNullException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return null;
            }
        }

        public static DateTime? ConvertFromStringWithZoneOffset(string dt)
        {
            try
            {
                return DateTime.ParseExact(dt, "yyyy-MM-ddTHH:mm:sszzz", null);
            }
            catch (FormatException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return null;
            }
            catch (ArgumentNullException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return null;
            }
        }
    }
}
