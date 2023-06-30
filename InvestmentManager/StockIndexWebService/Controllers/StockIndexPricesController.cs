using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockIndexWebService.ViewModels;
using System;
using System.Linq;
using System.Threading;

namespace StockIndexWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockIndexPricesController : ControllerBase
    {
        private readonly StockIndexDbContext dataContext;

        public StockIndexPricesController(StockIndexDbContext dbContext)
        {
            this.dataContext = dbContext;
        }

        // GET api/values
        [HttpGet("{indexCode}")]
        public ActionResult<StockIndexPriceViewModel> Get(String indexCode, DateTime tradeDate)
        {
            // Introduce some delay to simulate the delay you have in calling a service over the wire
            Thread.Sleep(new Random().Next(250, 750));

            Domain.StockIndexPrice? stockIndexPrice = this.dataContext.StockIndexPrices!
            .Include(prop => prop.Index)
            .Where(p => p.IndexCode == indexCode && p.TradeDate == tradeDate)
            .FirstOrDefault();
                        
            if (stockIndexPrice == null)
                return NotFound();

            var model = new StockIndexPriceViewModel()
            {
                IndexCode = stockIndexPrice.IndexCode!,
                IndexName = stockIndexPrice.Index!.Name!,
                IndexShortDisplayName = stockIndexPrice.Index.ShortDisplayName!,
                TradeDate = stockIndexPrice.TradeDate,
                ClosePrice = stockIndexPrice.ClosePrice,
                OpenPrice = stockIndexPrice.OpenPrice,
                HighPrice = stockIndexPrice.HighPrice,
                LowPrice = stockIndexPrice.LowPrice,
                AdjustedClosePrice = stockIndexPrice.AdjustedClosePrice,
                Volume = stockIndexPrice.Volume,
                Change = stockIndexPrice.Change,
                ChangePercent = stockIndexPrice.ChangePercent
            };

            return Ok(model);
        }

    }
}
