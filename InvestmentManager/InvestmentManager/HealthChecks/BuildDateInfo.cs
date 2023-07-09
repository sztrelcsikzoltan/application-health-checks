using System;
using System.Globalization;
using System.Reflection;

namespace InvestmentManager.HealthChecks
{
    public static class BuildDateInfo
    { 
        public static DateTime GetBuildDate(Assembly assembly)
        {
            var attribute = assembly.GetCustomAttribute<BuildDateAttribute>();
            return attribute != null ? attribute.DateTime : default(DateTime);
        }
    }

    [AttributeUsage(AttributeTargets.Assembly)]
    internal class BuildDateAttribute : Attribute
    {
        public BuildDateAttribute(string value)
        {
            DateTime = DateTime.ParseExact(value, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None);
        }

        public DateTime DateTime { get; }
    }
}
