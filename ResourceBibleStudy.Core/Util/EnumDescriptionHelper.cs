using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;

namespace ResourceBibleStudy.Core.Util
{
    /// <summary>
    /// 
    /// </summary>
    public class EnumDescriptionHelper
    {
        /// <summary>
        /// Enum Value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());

            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes.First().Description : value.ToString();
        } 
         
    }
}