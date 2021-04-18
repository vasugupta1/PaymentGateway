using System;
using System.Threading.Tasks;
using OneOf;
using PaymentGateway.Common.Models.Enums;
using PaymentGateway.Common.Models.Payment;
using PaymentGateway.Common.Models.Storage;
using PaymentGateway.Services.Banking.Exceptions;
using PaymentGateway.Services.Banking.Interface;
using PaymentGateway.Services.PaymentProcessor.Exceptions;
using PaymentGateway.Services.PaymentProcessor.Interface;
using PaymentGateway.Services.PaymentProcessor.Models;
using PaymentGateway.Services.Storage.Interface;

namespace PaymentGateway.Services.PaymentProcessor
{
    public class PaymentProcessorService : IPaymentProcessorService
    {
        private readonly IBankingService _bankingService;
        private readonly IStorageService<PaymentAudit> _storageService;

        public PaymentProcessorService(IBankingService bankingService, IStorageService<PaymentAudit> storageService)
        {
            _bankingService = bankingService ?? throw new ArgumentNullException(nameof(bankingService));
            _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
        }

        public async Task<OneOf<SuccessfulPaymentProcessing, UnsuccessfulPaymentProcessing>> ProcessPayment(PaymentProcessingRequest request)
        {
            if(request is null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                var bankingResponse = await _bankingService.ProcessPayment(request);

                if(!bankingResponse.Successful)
                {
                    await _storageService.Upsert(bankingResponse.Id, new PaymentAudit(request, Status.Failed));

                    return new UnsuccessfulPaymentProcessing()
                    {
                        Message = $"Bank has denied the payment, please contact the bank and refer to this id : {bankingResponse.Id}"
                    };
                }

                await _storageService.Upsert(bankingResponse.Id, new PaymentAudit(request, Status.Completed));

                return new SuccessfulPaymentProcessing()
                {
                    PaymentId = bankingResponse.Id,
                    Message = "The payment has been accepted by the bank"
                };
            }
            catch(BankingServiceException bsex)
            {
                throw new PaymentProcessorServiceException("Something went wrong when calling the banking service, please check inner exception", bsex);
            }
            catch(Exception ex)
            {
                throw new PaymentProcessorServiceException("Something went wrong, please check inner exception", ex);
            }
        }
    }
}