using Microsoft.AspNetCore.Mvc;
using Mod.Customers.Application.DTOs;
using Mod.Customers.Application.UseCases;

namespace Commerce.API.Controllers.Customers
{
    [Route("api/customers/")]
    public class CustomerController : BaseApiController
    {
        private readonly CreateCustomerUseCase _createUseCase;
        private readonly GetByIdCustomerUseCase _getByIdUseCase;
        private readonly GetAllCustomerUseCase _getAllUseCase;
        private readonly UpdateCustomerUseCase _updateUseCase;
        private readonly DeleteCustomerUseCase _deleteUseCase;

        public CustomerController(
            CreateCustomerUseCase createUseCase,
            GetByIdCustomerUseCase getByIdUseCase,
            GetAllCustomerUseCase getAllUseCase,
            UpdateCustomerUseCase updateUseCase,
            DeleteCustomerUseCase deleteUseCase)
        {
            _createUseCase = createUseCase;
            _getByIdUseCase = getByIdUseCase;
            _getAllUseCase = getAllUseCase;
            _updateUseCase = updateUseCase;
            _deleteUseCase = deleteUseCase;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCustomerDto command)
        {
            try
            {
                await _createUseCase.ExecuteAsync(command);
                return StatusCode(201, new { success = true, message = "Customer successfully created." });
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
                return StatusCode(500, new { success = false, error = "An error occurred while retrieving the list.", details = ex.Message });
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var customer = await _getByIdUseCase.ExecuteAsync(id);
                if (customer == null)
                {
                    return NotFound(new { success = false, message = "Customer not found." });
                }
                return Ok(new { success = true, data = customer });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "An error occurred while retrieving the customer.", details = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCustomerDto command)
        {
            try
            {
                await _updateUseCase.ExecuteAsync(id, command);
                return Ok(new { success = true, message = "Customer updated successfully." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
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
                return Ok(new { success = true, message = "Customer successfully deactivated (Soft Delete)." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "An error occurred while deactivating the customer.", details = ex.Message });
            }
        }
    }
}
