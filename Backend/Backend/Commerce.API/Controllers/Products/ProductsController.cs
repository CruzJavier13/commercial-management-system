using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mod.Products.Application.DTOs;
using Mod.Products.Application.UseCases;

namespace Commerce.API.Controllers.Products
{
    [Route("api/products/")]
    public class ProductsController : BaseApiController
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
