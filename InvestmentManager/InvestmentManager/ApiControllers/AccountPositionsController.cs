﻿using InvestmentManager.Core.DataAccess;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace InvestmentManager.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountPositionsController : ControllerBase
    {
        public AccountPositionsController(ITradeDateRepository tradeDateRepository, IAccountPositionsRepository positionsRepository)
        {
            this.tradeDateRepository = tradeDateRepository;
            this.positionsRepository = positionsRepository;
        }

        private readonly ITradeDateRepository tradeDateRepository;
        private readonly IAccountPositionsRepository positionsRepository;

        // GET: api/AccountPositions/5
        [HttpGet("{accountNumber}", Name = "Get")]
        public IActionResult Get(String accountNumber, [FromQuery]DateTime date)
        {
            var tradeDates = this.tradeDateRepository.LoadTradeDates();
            var tradeDate = tradeDates.FirstOrDefault(td => td.Date == date.Date);
            if (tradeDate == null)
                return BadRequest($"The date {date:yyyy-mm-DD} is not a valid trade date");

            var positions = this.positionsRepository.LoadAccountPositions(accountNumber, tradeDate);

            return Ok(positions);
        }

    }
}
