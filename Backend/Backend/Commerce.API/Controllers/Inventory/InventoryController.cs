using Microsoft.AspNetCore.Mvc;
using Mod.Inventory.Application.Dtos;
using Mod.Inventory.Application.UseCase;

namespace Commerce.API.Controllers.Inventory
{
    [Route("api/inventory")]
    public class InventoryController : BaseApiController
    {
        private readonly CreateStockMovementUseCase _createUseCase;
        private readonly GetAllStockMovementsUseCase _getAllUseCase;
        private readonly GetStockMovementsByProductUseCase _getByProductUseCase;

        public InventoryController(
            CreateStockMovementUseCase createUseCase,
            GetAllStockMovementsUseCase getAllUseCase,
            GetStockMovementsByProductUseCase getByProductUseCase)
        {
            _createUseCase = createUseCase ?? throw new ArgumentNullException(nameof(createUseCase));
            _getAllUseCase = getAllUseCase ?? throw new ArgumentNullException(nameof(getAllUseCase));
            _getByProductUseCase = getByProductUseCase ?? throw new ArgumentNullException(nameof(getByProductUseCase));
        }

        [HttpPost("movements")]
        public async Task<IActionResult> RegisterMovement([FromBody] CreateStockMovementDto command)
        {
            try
            {
                await _createUseCase.ExecuteAsync(command);
                return StatusCode(201, new { success = true, message = "Stock movement registered successfully." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
            catch (Exception ex) when (ex.Message.Contains("Stock insuficiente") || ex.Message.Contains("no inicializado"))
            {
                return Conflict(new { success = false, error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMovements()
        {
            try
            {
                var list = await _getAllUseCase.ExecuteAsync();
                return Ok(new { success = true, data = list });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "An error occurred while retrieving the global kárdex.", details = ex.Message });
            }
        }

        [HttpGet("{productId:int}")]
        public async Task<IActionResult> GetByProduct([FromRoute] int productId)
        {
            try
            {
                var list = await _getByProductUseCase.ExecuteAsync(productId);
                return Ok(new { success = true, data = list });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "An error occurred while retrieving the product kárdex.", details = ex.Message });
            }
        }
    }
}
