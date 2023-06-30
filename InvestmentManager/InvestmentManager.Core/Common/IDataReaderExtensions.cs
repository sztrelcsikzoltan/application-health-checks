using System;
using System.Data;

namespace InvestmentManager.Core.Common
{
    public static class IDataReaderExtensions
    {


        public static String? GetNullableString(this IDataReader reader, int index)
        {
            return (reader.IsDBNull(index)) ? null : reader.GetString(index);
        }


        public static DateTime? GetNullableDateTime(this IDataReader reader, int index)
        {
            return (reader.IsDBNull(index)) ? (DateTime?)null : reader.GetDateTime(index);
        }
    }
}
