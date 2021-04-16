using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway.API.Auth;
using PaymentGateway.Common.Models.Storage;
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
            _logger = logger;
            _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
        }

        [HttpGet("payment-retrieval/{id}")]
        [BasicAuth("PaymentGateway-Authentication")]
        public async Task<IActionResult> RetrievePayment(string id)
        {
            if(string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));
           return Ok();
        }
    }
}
