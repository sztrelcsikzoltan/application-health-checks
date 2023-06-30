using InvestmentManager.Core.Domain;
using System;
using System.Collections.Generic;

namespace InvestmentManager.Core.DataAccess
{
    public interface ISecurityRepository
    {
        Security LoadSecurity(String ticker);

        List<Security> LoadSecurities();
    }
}
