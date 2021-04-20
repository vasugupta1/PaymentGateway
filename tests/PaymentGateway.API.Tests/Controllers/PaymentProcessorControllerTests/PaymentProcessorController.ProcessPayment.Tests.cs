using System;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PaymentGateway.API.Controllers.v1;
using PaymentGateway.Common.Models.Payment;
using PaymentGateway.Services.PaymentProcessor.Interface;

namespace PaymentGateway.API.Tests.Controllers
{
    public partial class PaymentProcessorControllerTests
    {
        [TestCaseSource("BadRequestSource")]
        public void GivenBadRequest_WhenICallProcessPayment_ThenBadRequestResponseIsReturned(PaymentProcessingRequest request)
        {


            var output = _sut.TryValidateModel(request, null);

        
        }
    }
}
