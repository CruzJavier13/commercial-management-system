using Microsoft.AspNetCore.Mvc;
using Mod.Billing.Application.Dtos;
using Mod.Billing.Application.UseCase;

namespace Commerce.API.Controllers.Billing
{
    [Route("api/billing/")]
    public class InvoicesController: BaseApiController
    {
        private readonly CreateBillingUseCase _createBillingUseCase;
        private readonly GetInvoiceByIdUseCase _getBillingUseCase;
        public InvoicesController(CreateBillingUseCase createBillingUseCase, GetInvoiceByIdUseCase getInvoiceByIdUseCase)
        {
            _createBillingUseCase = createBillingUseCase;
            _getBillingUseCase = getInvoiceByIdUseCase;
        }

        [HttpPost("invoices")]
        public async Task<IActionResult> Create([FromBody] CreateInvoiceDto command)
        {
            await _createBillingUseCase.ExecuteAsync(command);
            return Ok(new { success = true, message = "Invoice successfully created." });
        }

        [HttpGet("invoices/{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var invoice = await _getBillingUseCase.ExecuteAsync(id);
            if (invoice == null)
            {
                return NotFound(new { success = false, message = "Invoice not found." });
            }
            return Ok(new { success = true, data = invoice });
        }
}
}
