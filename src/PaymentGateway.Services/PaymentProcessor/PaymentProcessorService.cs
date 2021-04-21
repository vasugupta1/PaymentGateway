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
using PaymentGateway.Services.Storage.Exceptions;
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
                    await UpsertPaymentAudit(Status.Failed, bankingResponse.Id, request);
                    return new UnsuccessfulPaymentProcessing()
                    {
                        Message = $"Bank has denied the payment, please contact the bank and refer to this id : {bankingResponse.Id}",
                        Success = false
                    };
                }

                await UpsertPaymentAudit(Status.Completed, bankingResponse.Id, request);

                return new SuccessfulPaymentProcessing()
                {
                    PaymentId = bankingResponse.Id,
                    Message = "The payment has been accepted by the bank",
                    Success = true
                };
            }
            catch(BankingServiceException bsex)
            {
                throw new PaymentProcessorServiceException("Something went wrong when calling the banking service, please check inner exception", bsex);
            }
            catch(StorageException ssex)
            {
                throw new PaymentProcessorServiceException("Something went wrong when trying to save, please check inner exception", ssex);
            }
            catch(Exception ex)
            {
                throw new PaymentProcessorServiceException("Something went wrong, please check inner exception", ex);
            }
        }

        private async Task UpsertPaymentAudit(Status status, string id, PaymentProcessingRequest request)
        {
            await _storageService.Upsert(id, new PaymentAudit()
                    {
                        CurrencyCode = request.CurrencyCode, 
                        ExpiryMonth = request.ExpiryMonth, 
                        ExpiryYear =  request.ExpiryYear, 
                        CVV = request.CVV,
                        CardNumber = request.CardNumber, 
                        Amount = request.Amount, 
                        Status = status.ToString(),
                        TranscationId = id
                    });
        }
    }
}