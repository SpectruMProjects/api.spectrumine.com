using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SpectruMineAPI.Services.Payment;

namespace SpectruMineAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentService PaymentService;
        public PaymentController(PaymentService service) => PaymentService = service;
        [HttpGet("Recive")]
        public async Task<IActionResult> RecivePayment(PaymentDTO.PaymentReciveQuery query)
        {

            var result = await PaymentService.RegisterPayment(Request.Headers["x-api-sha256-signature"]!, query.code, query.custom_fields.userId, query.custom_fields.productId);
            switch (result)
            {
                case PaymentService.Errors.ProductNotFound:
                    return NotFound(new Models.Error(result.ToString(), "ProductIsIncorrect"));
                case PaymentService.Errors.Failed:
                    return BadRequest(new Models.Error(result.ToString(), "UnsuccessCode"));
                case PaymentService.Errors.UserNotFound:
                    return NotFound(new Models.Error(result.ToString(), "UserDoesNotExist"));
            }
            return Ok();
        }
    }
}
