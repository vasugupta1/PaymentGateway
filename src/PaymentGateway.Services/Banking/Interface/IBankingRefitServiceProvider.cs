using PaymentGateway.Common.Models.Payment;
using PaymentGateway.Services.Banking.Models;
using Refit;
using System.Threading.Tasks;

namespace PaymentGateway.Services.Banking.Interface
{
    public interface IBankingRefitServiceProvider
    {
        [Post("/ProcessUsersPayment")]
        Task<BankingResponse> ProcessPayment([Body]PaymentProcessingRequest paymentRequest);
    }
}