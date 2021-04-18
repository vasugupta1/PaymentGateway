using System;
using PaymentGateway.Common.Models.Enums;
using PaymentGateway.Common.Models.Payment;

namespace PaymentGateway.Common.Models.Storage
{
    public class PaymentAudit
    {
        public PaymentProcessingRequest PaymentProcessingRequest;
        public Status Status;
        public PaymentAudit(PaymentProcessingRequest paymentProcessingRequest, Status status)
        {
            PaymentProcessingRequest = paymentProcessingRequest ?? throw new ArgumentNullException(nameof(paymentProcessingRequest));
            Status = status;
        }

        public PaymentAudit()
        {
        }
    }
}