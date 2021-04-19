using System;
using System.Text.Json.Serialization;

namespace PaymentGateway.Services.Tests.Storage.Models
{
    public class DataModel
    {
        [JsonPropertyName("Data")]
        public string Data => new string("fake_data");
    }
}
