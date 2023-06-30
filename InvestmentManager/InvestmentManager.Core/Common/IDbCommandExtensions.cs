using System;
using System.Data;

namespace InvestmentManager.Core.Common
{
    public static class IDbCommandExtensions
    {

        public static IDbDataParameter AddParameterWithValue(this IDbCommand command, String name, Object value)
        {
            IDbDataParameter parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;
            command.Parameters.Add(parameter);

            return parameter;
        }

    }
}
