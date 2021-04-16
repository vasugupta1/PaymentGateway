using System;
using System.ComponentModel.DataAnnotations;
using PaymentGateway.Common.Models.Enums;
using PaymentGateway.Common.Models.Payment;

namespace PaymentGateway.Common.Models.Storage
{
    public class PaymentAudit
    {
        [Key]
        public string BankTranscationId {get; set;}
        public PaymentProcessingRequest PaymentProcessingRequest;
        public Status Status;
        public PaymentAudit(string bankTranscationId, PaymentProcessingRequest paymentProcessingRequest, Status status)
        {
            BankTranscationId = !string.IsNullOrEmpty(bankTranscationId) ? bankTranscationId : throw new ArgumentNullException(nameof(bankTranscationId));
            PaymentProcessingRequest = paymentProcessingRequest ?? throw new ArgumentNullException(nameof(paymentProcessingRequest));
            Status = status;
        }

        public PaymentAudit()
        {

        }
    }
}