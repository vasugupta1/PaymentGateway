using System.Collections.Generic;

namespace PaymentGateway.API.Models.ApiConfiguration
{
    public class CustomConfiguration
    {
        public Bank Bank { get; set; }
        public Dictionary<string, string> Authentication { get; set; }
        public RedisSettings RedisSettings { get; set; }
    }

    public class RedisSettings
    {
        public string ConnectionString { get; set; }
    }
    public class Bank
    {
        public string Url { get; set; }
    }
}