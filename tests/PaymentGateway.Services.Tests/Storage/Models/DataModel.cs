using System;
using System.Text.Json.Serialization;

namespace PaymentGateway.Services.Tests.Storage.Models
{
    public class DataModel
    {
        [JsonPropertyName("Data1")]
        public string Data1 => Guid.NewGuid().ToString();
    }
}
