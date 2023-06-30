using System;

namespace InvestmentManager.Core.Domain
{
    public class CashFlowType
    {
        public String? Code { get; set; }

        public String? Name { get; set; }

        public bool IsExternal { get; set; }
    }
}
