using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Bank.Models.Response;
using Bank.Models.Request;

namespace Bank.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProcessUsersPaymentController : ControllerBase
    {
        private readonly ILogger<ProcessUsersPaymentController> _logger;

        public ProcessUsersPaymentController(ILogger<ProcessUsersPaymentController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        public async Task<IActionResult> ProcessUsersPayment([FromBody] PaymentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return new OkObjectResult(new Response()
            {
                Id = Guid.NewGuid().ToString(),
                Successful = new Random().Next() % 2 == 0 ? true : false
            });
        }
    }
}
