using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway.API.Services.Auth;
using PaymentGateway.Common.Models.Storage;
using PaymentGateway.Services.Storage.Exceptions;
using PaymentGateway.Services.Storage.Interface;

namespace PaymentGateway.API.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PaymentRetrievalController : ControllerBase
    {
        private readonly ILogger<PaymentRetrievalController> _logger;
        private readonly IStorageService<PaymentAudit> _storageService;

        public PaymentRetrievalController(ILogger<PaymentRetrievalController> logger, IStorageService<PaymentAudit> storageService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
        }

        [HttpGet("payment-retrieval/{id}")]
        [BasicAuth("PaymentGateway-Authentication")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaymentAudit))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RetrievePayment(string id)
        {
            if(string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));
            try
            {
                var oneOfResponse = await _storageService.Get(id);

                return oneOfResponse.Match<IActionResult>(
                    PaymentAudit => new OkObjectResult(PaymentAudit),
                    NotFoundResponse => new NotFoundObjectResult(NotFoundResponse));
            }
            catch(StorageException stex)
            {
                _logger.LogError("Something went wrong when trying to get from database, please check error", stex);
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
