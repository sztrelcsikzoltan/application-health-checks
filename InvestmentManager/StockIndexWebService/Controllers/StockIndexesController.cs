using Microsoft.AspNetCore.Mvc;
using StockIndexWebService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace StockIndexWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockIndexesController : ControllerBase
    {
        public StockIndexesController(StockIndexDbContext dbContext)
        {
            this.dataContext = dbContext;
        }

        private readonly StockIndexDbContext dataContext;

        // GET api/values
        [HttpGet]
        public ActionResult<List<StockIndexViewModel>> Get()
        {
            // Introduce some delay to simulate the delay you have in calling a service over the wire
            Thread.Sleep(new Random().Next(250, 750));

                var models = this.dataContext.StockIndexes!.Select(ix => new StockIndexViewModel()
                {
                    Code = ix.Code,
                    Name = ix.Name,
                    ShortDisplayName = ix.ShortDisplayName
                }).ToList();
            
            if (models == null)
            {
                return NotFound();
            }
            
            return Ok(models);
        }
     
    }
}
