using Microsoft.AspNetCore.Mvc;
using Mod.Products.Application.DTOs;
using Mod.Products.Application.UseCases;

namespace Commerce.API.Controllers.Products
{
    [Route("api/suppliers")]
    public class SupplierController : BaseApiController
    {
        private readonly CreateSupplierUseCase _createUseCase;
        private readonly GetAllSupplierUseCase _getAllUseCase;
        private readonly GetByIdSupplierUseCase _getByIdUseCase;
        private readonly UpdateSupplierUseCase _updateUseCase;
        private readonly DeleteSupplierUseCase _deleteUseCase;

        public SupplierController(
            CreateSupplierUseCase createUseCase,
            GetAllSupplierUseCase getAllUseCase,
            GetByIdSupplierUseCase getByIdUseCase,
            UpdateSupplierUseCase updateUseCase,
            DeleteSupplierUseCase deleteUseCase)
        {
            _createUseCase = createUseCase ?? throw new ArgumentNullException(nameof(createUseCase));
            _getAllUseCase = getAllUseCase ?? throw new ArgumentNullException(nameof(getAllUseCase));
            _getByIdUseCase = getByIdUseCase ?? throw new ArgumentNullException(nameof(getByIdUseCase));
            _updateUseCase = updateUseCase ?? throw new ArgumentNullException(nameof(updateUseCase));
            _deleteUseCase = deleteUseCase ?? throw new ArgumentNullException(nameof(deleteUseCase));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSupplierDto command)
        {
            try
            {
                await _createUseCase.ExecuteAsync(command);
                return StatusCode(201, new { success = true, message = "Supplier successfully created." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var list = await _getAllUseCase.ExecuteAsync();
                return Ok(new { success = true, data = list });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "An error occurred while retrieving suppliers.", details = ex.Message });
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var supplier = await _getByIdUseCase.ExecuteAsync(id);
                if (supplier == null)
                {
                    return NotFound(new { success = false, message = "Supplier not found." });
                }
                return Ok(new { success = true, data = supplier });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "An error occurred while retrieving the supplier.", details = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] CreateSupplierDto command)
        {
            try
            {
                await _updateUseCase.ExecuteAsync(id, command);
                return Ok(new { success = true, message = "Supplier updated successfully." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { success = false, error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "An error occurred during update.", details = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                await _deleteUseCase.ExecuteAsync(id);
                return Ok(new { success = true, message = "Supplier successfully deactivated (Soft Delete)." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { success = false, error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "An error occurred while deactivating the supplier.", details = ex.Message });
            }
        }
    }
}
