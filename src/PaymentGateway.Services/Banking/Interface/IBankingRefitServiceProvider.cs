using PaymentGateway.Common.Models.Payment;
using Refit;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PaymentGateway.Services.Banking.Interface
{
    public interface IBankingRefitServiceProvider
    {
        [Post("/bankA/process-payment")]
        Task<HttpResponseMessage> ProcessPayment(PaymentProcessingRequest paymentRequest);
    }
}