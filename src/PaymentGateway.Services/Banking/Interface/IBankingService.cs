using System.Threading.Tasks;
using PaymentGateway.Common.Models.Payment;
using PaymentGateway.Services.Banking.Models;

namespace PaymentGateway.Services.Banking.Interface
{
    public interface IBankingService
    {
        Task<BankingResponse> ProcessPayment(PaymentProcessingRequest request);
    }
}