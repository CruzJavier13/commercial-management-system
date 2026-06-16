using Microsoft.AspNetCore.Mvc;
using Mod.Products.Application.DTOs;
using Mod.Products.Application.UseCases;

namespace Commerce.API.Controllers.Products
{
    [Route("api/categories")]
    public class CategoryController : BaseApiController
    {
        private readonly CreateCategoryUseCase _createUseCase;
        private readonly GetAllCategoryUseCase _getAllUseCase;
        private readonly GetByIdCategoryUseCase _getByIdUseCase;
        private readonly UpdateCategoryUseCase _updateUseCase;
        private readonly DeleteCategoryUseCase _deleteUseCase;

        public CategoryController(
            CreateCategoryUseCase createUseCase,
            GetAllCategoryUseCase getAllUseCase,
            GetByIdCategoryUseCase getByIdUseCase,
            UpdateCategoryUseCase updateUseCase,
            DeleteCategoryUseCase deleteUseCase)
        {
            _createUseCase = createUseCase ?? throw new ArgumentNullException(nameof(createUseCase));
            _getAllUseCase = getAllUseCase ?? throw new ArgumentNullException(nameof(getAllUseCase));
            _getByIdUseCase = getByIdUseCase ?? throw new ArgumentNullException(nameof(getByIdUseCase));
            _updateUseCase = updateUseCase ?? throw new ArgumentNullException(nameof(updateUseCase));
            _deleteUseCase = deleteUseCase ?? throw new ArgumentNullException(nameof(deleteUseCase));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDto command)
        {
            try
            {
                await _createUseCase.ExecuteAsync(command);
                return StatusCode(201, new { success = true, message = "Category successfully created." });
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
                return StatusCode(500, new { success = false, error = "An error occurred while retrieving categories.", details = ex.Message });
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var category = await _getByIdUseCase.ExecuteAsync(id);
                if (category == null)
                {
                    return NotFound(new { success = false, message = "Category not found." });
                }
                return Ok(new { success = true, data = category });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "An error occurred while retrieving the category.", details = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCategoryDto command)
        {
            try
            {
                await _updateUseCase.ExecuteAsync(id, command);
                return Ok(new { success = true, message = "Category updated successfully." });
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
                return Ok(new { success = true, message = "Category successfully deactivated (Soft Delete)." });
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
                return StatusCode(500, new { success = false, error = "An error occurred while deactivating the category.", details = ex.Message });
            }
        }
    }
}
