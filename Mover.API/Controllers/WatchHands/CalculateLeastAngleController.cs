using Microsoft.AspNetCore.Mvc;
using Mover.API.Exceptions.Watch;
using Mover.API.Validation;
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
        public ActionResult<string> Get([FromQuery] CalculateLeastAngleRequest requestModel)
        {
            try
            {
                RequestModelValidator.Validate(requestModel);

                double leastAngle = _watchHandsAngleService.CalculateLeastAngleFromTime(requestModel.TimeStamp);

                Log.Information($"Calculated least angle: {leastAngle}");

                return Ok($"Least Angle: {leastAngle}");
            }
            catch (ValidationException ex)
            {
                Log.Error(ex, "Request model validation failed.");

                return BadRequest(ex.Message);
            }
            catch (InvalidTimestampException ex)
            {
                var errorMessage = $"Invalid timestamp provided.";
                Log.Error(ex, errorMessage);

                return BadRequest(errorMessage);
            }
            catch (CalculateLeastAngleException ex)
            {
                Log.Error(ex, $"Error calculating least angle: {ex.Message}");

                return StatusCode(500, $"An error occurred while calculating the least angle: {ex.Message}");
            }
            catch (Exception ex)
            {
                var errorMessage = $"An unexpected error occurred..";
                Log.Error(ex, errorMessage);

                return StatusCode(500, errorMessage);
            }
        }

    }
}