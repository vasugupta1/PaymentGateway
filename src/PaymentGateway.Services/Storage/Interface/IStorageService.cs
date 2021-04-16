using System.Threading.Tasks;

namespace PaymentGateway.Services.Storage.Interface
{
    public interface IStorageService<T>
    {
        Task<T> Get(string id);
        Task Upsert(T dataObject);
    }
}