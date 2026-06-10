using Microsoft.AspNetCore.Mvc;
using Mod.Billing.Application.Dtos;
using Mod.Billing.Application.UseCase;

namespace Commerce.API.Controllers.Billing
{
    [Route("api/billing/invoices/")]
    public class InvoicesController: BaseApiController
    {
        private readonly CreateInvoiceUseCase _createInvoiceUseCase;
        private readonly GetInvoiceByIdUseCase _getInvoiceUseCase;
        private readonly GetAllInvoiceUseCase _getAllInvoiceUseCase;
        private readonly DeleteInvoiceUseCase _deleteInvoiceUseCase;
        private readonly UpdateInvoiceUseCase _updateInvoiceUseCase;
        public InvoicesController(CreateInvoiceUseCase createInvoiceUseCase, GetInvoiceByIdUseCase getInvoiceByIdUseCase, GetAllInvoiceUseCase getAllInvoiceUseCase, DeleteInvoiceUseCase deleteInvoiceUseCase, UpdateInvoiceUseCase updateInvoiceUseCase)
        {
            _createInvoiceUseCase = createInvoiceUseCase;
            _getInvoiceUseCase = getInvoiceByIdUseCase;
            _getAllInvoiceUseCase = getAllInvoiceUseCase;
            _deleteInvoiceUseCase = deleteInvoiceUseCase;
            _updateInvoiceUseCase = updateInvoiceUseCase;
        }

        [HttpGet] 
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var list = await _getAllInvoiceUseCase.ExecuteAsync();
                return Ok(new { success = true, data = list });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while retrieving the list.", details = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateInvoiceDto command)
        {
            await _createInvoiceUseCase.ExecuteAsync(command);
            return Ok(new { success = true, message = "Invoice successfully created." });
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var invoice = await _getInvoiceUseCase.ExecuteAsync(id);
            if (invoice == null)
            {
                return NotFound(new { success = false, message = "Invoice not found." });
            }
            return Ok(new { success = true, data = invoice });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                await _deleteInvoiceUseCase.ExecuteAsync(id);

                return Ok(new { success = true, message = "Invoice successfully cancelled (Soft Delete)." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Ocurrio un error durante la anulación de la factura.", details = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateInvoiceDto command)
        {
            try
            {
                await _updateInvoiceUseCase.ExecuteAsync(id, command);
                return Ok(new { success = true, message = "Invoice and details updated successfully." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "A ocurrido un error durante la actualización de la factura.", details = ex.Message });
            }
        }
    }
}
