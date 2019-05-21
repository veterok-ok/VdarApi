using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Contracts.Repository;
using Entities.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VdarApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private IRepositoryWrapper _repo;
        private ILoggerManager _logger;

        public StockController(IRepositoryWrapper wrapperRepository,
                                 ILoggerManager logger)
        {
            this._repo = wrapperRepository;
            this._logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<StockResult>> GetActiveStocks()
        {

            return null;
        }

        [HttpGet]
        public async Task<ActionResult<StockResult>> GetCompletedStocks()
        {

            return null;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "BearerLT")]
        public async Task<ActionResult<StockResult>> GetStockDetail()
        {

            return null;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "BearerLT")]
        public async Task<ActionResult<StockResult>> StartStockView()
        {

            return null;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "BearerLT")]
        public async Task<ActionResult<StockResult>> FixateStockTimeline()
        {

            return null;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "BearerLT")]
        public async Task<ActionResult<StockResult>> CompleteStockView()
        {

            return null;
        }

    }
}