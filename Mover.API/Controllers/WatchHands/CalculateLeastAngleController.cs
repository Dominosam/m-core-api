﻿using Microsoft.AspNetCore.Mvc;
using Mover.API.Exceptions.Watch;
using Mover.API.Validation;
using Mover.Core.Inventory.CustomExceptions;
using Mover.Core.Watch.Interfaces.Services;
using Mover.Core.Watch.Models.Request;
using Serilog;
using System.ComponentModel.DataAnnotations;

namespace Mover.API.Controllers.WatchHands
{
    [ApiController]
    [Route("api/WatchHands/[controller]")]
    public class CalculateLeastAngleController : ControllerBase
    {
        private readonly IWatchHandsAngleService _watchHandsAngleService;

        public CalculateLeastAngleController(IWatchHandsAngleService watchHandsAngleService)
        {
            _watchHandsAngleService = watchHandsAngleService ?? throw new ArgumentNullException(nameof(watchHandsAngleService));
        }

        [HttpGet]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationException), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Exception), StatusCodes.Status500InternalServerError)]
        public ActionResult<string> Get([FromQuery] CalculateLeastAngleRequest requestModel)
        {
            try
            {
                RequestModelValidator.Validate(requestModel);
                double leastAngle = _watchHandsAngleService.CalculateLeastAngleFromTime(requestModel.TimeStamp);

                var responseMessage = $"Least Angle: {leastAngle}";
                Log.Information(responseMessage);

                return Ok(responseMessage);
            }
            catch (ValidationException ex)
            {
                Log.Error(ex, "Request model validation failed.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                var errorMessage = $"An unexpected error occurred.";
                Log.Error(ex, errorMessage);
                return StatusCode(500, errorMessage);
            }
        }

    }
}