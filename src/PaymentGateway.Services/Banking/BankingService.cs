using PaymentGateway.Common.Exceptions;
using PaymentGateway.Common.Models.Payment;
using PaymentGateway.Services.Banking.Exceptions;
using PaymentGateway.Services.Banking.Interface;
using PaymentGateway.Services.Banking.Models;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace PaymentGateway.Services.Banking
{
    public class BankingService : IBankingService
    {
        private readonly IBankingRefitServiceProvider _bankingService;
        public BankingService(IBankingRefitServiceProvider bankingService)
        {
            _bankingService = bankingService ?? throw new ArgumentNullException(nameof(bankingService));
        }
        public async Task<BankingResponse> ProcessPayment(PaymentProcessingRequest paymentRequest)
        {
            if(paymentRequest is null)
                throw new ArgumentNullException(nameof(paymentRequest));

            try
            {
                return await _bankingService.ProcessPayment(paymentRequest);
            }
            catch(Exception ex)
            {
                throw new BankingServiceException("Error occured when calling bank rest api, please check inner exception", ex);
            }
        }
    }
}