using System.Threading.Tasks;
using OneOf;
using PaymentGateway.Common.Models.Storage;

namespace PaymentGateway.Services.Storage.Interface
{
    public interface IStorageService<T>
    {
        Task<OneOf<T, NotFoundResponse>> Get(string key);
        Task Upsert(string key, T dataObject);
    }
}