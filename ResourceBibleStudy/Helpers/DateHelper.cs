using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceBibleStudy.Helpers
{
    public class DateHelper
    {
        public static DateTime GetCurrentDate()
        {
            return DateTime.UtcNow;
        }
    }
}