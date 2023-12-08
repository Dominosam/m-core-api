using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mover.API.Exceptions.InventoryItem;
using Mover.API.Validation;
using Mover.Core.Inventory.Interfaces.Services;
using Mover.Core.Inventory.Models.DTOs;
using Mover.Core.Inventory.Models.Requests;
using Mover.Data.Repositories.Inventory.Models.Enums;
using Serilog;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Mover.API.Controllers.InventoryItem
{
    [ApiController]
    [Route("api/Inventory/[controller]")]
    public class InventoryItemController : ControllerBase
    {
        private readonly IInventoryItemService _inventoryItemService;

        public InventoryItemController(IInventoryItemService inventoryItemService)
        {
            _inventoryItemService = inventoryItemService ?? throw new ArgumentNullException(nameof(inventoryItemService));
        }

        /// <summary>
        /// Creates or updates an inventory item.
        /// </summary>
        /// <param name="requestModel">The inventory item details.</param>
        /// <returns>A string indicating the success of the operation.</returns>
        [HttpPost]
        [SwaggerOperation("CreateOrUpdateInventoryItem")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> CreateOrUpdate([FromBody] InventoryItemDto requestModel)
        {
            try
            {
                RequestModelValidator.Validate(requestModel);
                
                var action = await _inventoryItemService.UpsertInventoryItem(requestModel);
                var resultMessage = string.Empty;
                switch (action)
                {
                    case InventoryItemAction.AddedQuantity:
                        resultMessage = $"Inventory item updated successfully: SKU - {requestModel.SKU}, added quantity by: {requestModel.Quantity}";
                        Log.Information(resultMessage);
                        return Ok(resultMessage);
                    case InventoryItemAction.Inserted:
                    default:
                        resultMessage = $"Inventory item created successfully: SKU - {requestModel.SKU}";
                        Log.Information(resultMessage);
                        return Ok(resultMessage);
                }

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

        /// <summary>
        /// Removes a specified quantity from an inventory item.
        /// </summary>
        /// <param name="sku">The SKU of the inventory item.</param>
        /// <param name="requestModel">The request model specifying the quantity to remove.</param>
        /// <returns>A string indicating the success of the operation.</returns>
        [HttpPut("remove-quantity")]
        [SwaggerOperation("RemoveQuantity")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(InsufficientQuantityException), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> RemoveQuantity([FromBody] RemoveQuantityRequestModel requestModel)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(requestModel.SKU))
                {
                    throw new ValidationException("SKU is required.");
                }

                if (requestModel.Quantity <= 0)
                {
                    throw new ValidationException("Quantity must be greater than zero.");
                }

                var action = await _inventoryItemService.RemoveInventoryItemQuantity(requestModel.SKU, requestModel.Quantity);
                var resultMessage = string.Empty;
                switch (action)
                {
                    case InventoryItemAction.RemovedQuantity:
                        resultMessage = $"Removed quantity - {requestModel.Quantity} - from inventory item: SKU - {requestModel.SKU}";
                        Log.Information(resultMessage);
                        return Ok(resultMessage);

                    case InventoryItemAction.Failed:
                    default:
                        resultMessage = $"Failed to remove quantity - {requestModel.Quantity} - from inventory item: SKU - {requestModel.SKU}";
                        throw new ValidationException(resultMessage);
                }
            }
            catch (InsufficientQuantityException ex)
            {
                Log.Error(ex, $"Insufficient quantity to remove: SKU - {requestModel.SKU}");

                return BadRequest(ex.Message);
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

        /// <summary>
        /// Retrieves details of an inventory item.
        /// </summary>
        /// <param name="sku">The SKU of the inventory item.</param>
        /// <returns>Details of the inventory item.</returns>
        [HttpGet("{sku}")]
        [SwaggerOperation("GetInventoryItemDetails")]
        [ProducesResponseType(typeof(InventoryItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public ActionResult<InventoryItemDto> GetInventoryItemDetails(string sku)
        {
            try
            {
                if(string.IsNullOrEmpty(sku))
                {
                    var errorMessage = $"SKU is required.";
                    Log.Error(errorMessage);

                    throw new ValidationException("SKU is required.");
                }

                var itemDetails = _inventoryItemService.GetInventoryItemBySKU(sku);
                if(itemDetails == null)
                {
                    var notFoundMessage = $"Inventory item not found: SKU - {sku}";
                    Log.Information(notFoundMessage);

                    return Ok(notFoundMessage);
                }

                Log.Information($"Retrieved details for inventory item: SKU - {sku}");

                return Ok(itemDetails);
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

        /// <summary>
        /// Retrieves details of all inventory items.
        /// </summary>
        /// <returns>Details of all inventory items.</returns>
        [HttpGet]
        [SwaggerOperation("GetAllInventory")]
        [ProducesResponseType(typeof(List<InventoryItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public ActionResult<List<InventoryItemDto>> GetAllInventory()
        {
            try
            {
                var items = _inventoryItemService.GetAllInventoryItems();

                Log.Information($"Retrieved all inventory items");

                return Ok(items);
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
