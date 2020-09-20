using System;
using System.ComponentModel;
using System.Reflection;
using Starkit.Models;

namespace Starkit.Services
{
    public class GetEnumDescription
    {
        public static string GetBookingStateDescription(string enumElement)
        {
            Type type = typeof(BookingStatus);

            MemberInfo[] memInfo = type.GetMember(enumElement);
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }

            return enumElement;
        }
        
        public static string GetTableLocationDescription(string enumElement)
        {
            Type type = typeof(Location);

            MemberInfo[] memInfo = type.GetMember(enumElement);
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }

            return enumElement;
        }
    }
}