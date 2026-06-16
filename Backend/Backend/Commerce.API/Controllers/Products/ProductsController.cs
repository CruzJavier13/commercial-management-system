using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mod.Products.Application.DTOs;
using Mod.Products.Application.UseCases;

namespace Commerce.API.Controllers.Products;

[Route("api/products/")]
public class ProductsController : BaseApiController
{
    private readonly CreateProductUseCase _createUseCase;
    private readonly GetAllProductUseCase _getAllUseCase;
    private readonly GetByIdProductUseCase _getByIdUseCase;
    private readonly UpdateProductUseCase _updateUseCase;
    private readonly DeleteProductUseCase _deleteUseCase;

    // El contenedor de inversión de control (IoC) inyectará automáticamente las dependencias
    public ProductsController(
        CreateProductUseCase createUseCase,
        GetAllProductUseCase getAllUseCase,
        GetByIdProductUseCase getByIdUseCase,
        UpdateProductUseCase updateUseCase,
        DeleteProductUseCase deleteUseCase)
    {
        _createUseCase = createUseCase ?? throw new ArgumentNullException(nameof(createUseCase));
        _getAllUseCase = getAllUseCase ?? throw new ArgumentNullException(nameof(getAllUseCase));
        _getByIdUseCase = getByIdUseCase ?? throw new ArgumentNullException(nameof(getByIdUseCase));
        _updateUseCase = updateUseCase ?? throw new ArgumentNullException(nameof(updateUseCase));
        _deleteUseCase = deleteUseCase ?? throw new ArgumentNullException(nameof(deleteUseCase));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductDto command)
    {
        try
        {
            await _createUseCase.ExecuteAsync(command);
            return StatusCode(201, new { success = true, message = "Product successfully created." });
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
            return StatusCode(500, new { success = false, error = "An error occurred while retrieving the catalog.", details = ex.Message });
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        try
        {
            var product = await _getByIdUseCase.ExecuteAsync(id);
            if (product == null)
            {
                return NotFound(new { success = false, message = "Product not found." });
            }
            return Ok(new { success = true, data = product });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { success = false, error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, error = "An error occurred while retrieving the product.", details = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateProductDto command)
    {
        try
        {
            await _updateUseCase.ExecuteAsync(id, command);
            return Ok(new { success = true, message = "Product updated successfully." });
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
            return Ok(new { success = true, message = "Product successfully deactivated (Soft Delete)." });
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
            return StatusCode(500, new { success = false, error = "An error occurred while deactivating the product.", details = ex.Message });
        }
    }
}
