using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mod.Products.Application.DTOs;
using Mod.Products.Application.UseCases;

namespace Commerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly CreateProductUseCase _createProductUseCase;

        public ProductsController(CreateProductUseCase createProductUseCase)
        {
            _createProductUseCase = createProductUseCase;
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateProductDto command)
        {
            _createProductUseCase.Execute(command);
            return Ok(new { success = true, message = "Product successfully created." });
        }
    }
}
