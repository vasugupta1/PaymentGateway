using System;
using PaymentGateway.Common.Models.Storage;
using PaymentGateway.Services.Storage.Interface;
using System.Threading.Tasks;
using PaymentGateway.Services.Storage.Exceptions;
using OneOf;
using StackExchange.Redis;
using System.Text.Json;

namespace PaymentGateway.Services.Storage
{
    public class StorageService<T> : IStorageService<T>
    {   
        private readonly IDatabase _database;
        public StorageService(IDatabase database)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
        }
        public async Task<OneOf<T, NotFoundResponse>> Get(string key)
        {
            if(string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));
            try
            {
                var response = await _database.StringGetAsync(key);
                if (!string.IsNullOrEmpty(response))
                {
                    return JsonSerializer.Deserialize<T>(response);
                }

                return new NotFoundResponse()
                {
                    ErrorMessage = $"Payment record was not found for given key"
                };
            }
            catch(Exception ex)
            {
                throw new StorageException("Something went wrong when trying to get payment audit from database, " +
                    "please check inner exception", ex);
            }
        }

        public async Task Upsert(string key, T dataObject)
        {
            if(dataObject is null)
                throw new ArgumentNullException(nameof(dataObject));
            if(string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));
            try
            {
                await _database.StringSetAsync(key, JsonSerializer.Serialize<T>(dataObject));
            }
            catch(Exception ex)
            {
                throw new StorageException("Something went wrong when trying to upsert payment audit from database, please check inner exception", ex);
            }
        }
    }
}