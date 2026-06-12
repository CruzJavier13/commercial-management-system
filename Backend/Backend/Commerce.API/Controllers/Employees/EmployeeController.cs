using Microsoft.AspNetCore.Mvc;
using Mod.Emp.Application.DTOs;
using Mod.Emp.Application.UseCases;

namespace Commerce.API.Controllers.Employees
{
    [Route("api/employees")]
    public class EmployeeController: BaseApiController
    {
        private readonly CreateEmployeeUseCase _createUseCase;
        private readonly GetByIdEmployeeUseCase _getByIdUseCase;
        private readonly GetAllEmployeeUseCase _getAllUseCase;
        private readonly UpdateEmployeeUseCase _updateUseCase;
        private readonly DeleteEmployeeUseCase _deleteUseCase;

        public EmployeeController(
            CreateEmployeeUseCase createUseCase,
            GetByIdEmployeeUseCase getByIdUseCase,
            GetAllEmployeeUseCase getAllUseCase,
            UpdateEmployeeUseCase updateUseCase,
            DeleteEmployeeUseCase deleteUseCase)
        {
            _createUseCase = createUseCase;
            _getByIdUseCase = getByIdUseCase;
            _getAllUseCase = getAllUseCase;
            _updateUseCase = updateUseCase;
            _deleteUseCase = deleteUseCase;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEmployeeDto command)
        {
            try
            {
                await _createUseCase.ExecuteAsync(command);
                return StatusCode(201, new { success = true, message = "Employee successfully created." });
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
                var employee = await _getByIdUseCase.ExecuteAsync(id);
                if (employee == null)
                {
                    return NotFound(new { success = false, message = "Employee not found." });
                }
                return Ok(new { success = true, data = employee });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "An error occurred while retrieving the employee.", details = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateEmployeeDto command)
        {
            try
            {
                await _updateUseCase.ExecuteAsync(id, command);
                return Ok(new { success = true, message = "Employee updated successfully." });
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
                return Ok(new { success = true, message = "Employee successfully deactivated (Soft Delete)." });
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
                return StatusCode(500, new { success = false, error = "An error occurred while deactivating the employee.", details = ex.Message });
            }
        }
    }
}
