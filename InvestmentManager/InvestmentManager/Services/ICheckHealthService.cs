using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InvestmentManager.Services
{
    public interface ICheckHealthService
    {
        public Task<IActionResult> CheckHealth();
        public IActionResult VersionInfo();

    }
}
