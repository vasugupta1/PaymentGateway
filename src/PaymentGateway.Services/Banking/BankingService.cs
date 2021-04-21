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
                throw new ArgumentException($"Parameter {nameof(paymentRequest)} cannot be null");

            try
            {
                return await _bankingService.ProcessPayment(paymentRequest);
            }
            catch(HttpException httex)
            {
                throw new BankingServiceException("Error occured when trying to call bank, please check inner exception", httex);
            }
            catch(Exception ex)
            {
                throw new BankingServiceException("Error occured when trying to handle payment, please check inner exception", ex);
            }
        }
    }
}