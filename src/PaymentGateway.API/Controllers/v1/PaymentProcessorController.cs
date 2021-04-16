using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway.Common.Models.Payment;
using PaymentGateway.Services.PaymentProcessor.Interface;
using OneOf;
using PaymentGateway.Services.PaymentProcessor.Exceptions;
using PaymentGateway.API.Services.Auth;
using Microsoft.AspNetCore.Http;
using PaymentGateway.Services.PaymentProcessor.Models;

namespace PaymentGateway.API.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PaymentProcessorController : ControllerBase
    {
        private readonly ILogger<PaymentProcessorController> _logger;
        private readonly IPaymentProcessorService _paymentProcessorService;

        public PaymentProcessorController(ILogger<PaymentProcessorController> logger, IPaymentProcessorService paymentProcessorService)
        {
            _logger = logger;
            _paymentProcessorService = paymentProcessorService ?? throw new ArgumentNullException(nameof(paymentProcessorService));
        }

        [HttpPost]
        [BasicAuth("PaymentGateway-Authentication")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessfulPaymentProcessing))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(UnsuccessfulPaymentProcessing))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentProcessingRequest processingRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var oneOfProcessingResponse = await _paymentProcessorService.ProcessPayment(processingRequest);
                
                return oneOfProcessingResponse.Match<IActionResult>(
                    SuccessfulPaymentProcessing => Ok(SuccessfulPaymentProcessing),
                    UnsuccessfulPaymentProcessing => new BadRequestObjectResult(UnsuccessfulPaymentProcessing));
            }
            catch(PaymentProcessorServiceException ppsex)
            {
                _logger.LogError("Something went wrong with processing payment, please check error", ppsex);
                return StatusCode(500, "something went wrong, please contact technical team");
            }
            catch(Exception ex)
            {
                _logger.LogError("Something went wrong within controller, please check error", ex);
                return StatusCode(500, "something went wrong, please contact technical team");
            }
        }
    }
}
