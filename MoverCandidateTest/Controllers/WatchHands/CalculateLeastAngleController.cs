using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace MoverCandidateTest.Controllers.WatchHands
{
    [ApiController]
    [Route("api/WatchHands/[controller]")]
    public class CalculateLeastAngleController : ControllerBase
    {
        private readonly ILogger<CalculateLeastAngleController> _logger;

        public CalculateLeastAngleController(ILogger<CalculateLeastAngleController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get([FromQuery] CalculateLeastAngleRequestModel requestModel)
        {
            return $"Hi candidate, return your result here :) (Input reads: {requestModel.DateTime})";
        }
    }
}
