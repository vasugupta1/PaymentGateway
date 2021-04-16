using System;
using Microsoft.EntityFrameworkCore;
using PaymentGateway.Common.Models.Storage;
using PaymentGateway.Services.Storage.Interface;
using System.Threading.Tasks;
using PaymentGateway.Services.Storage.Exceptions;
using OneOf;

namespace PaymentGateway.Services.Storage
{
    public class StorageService : IStorageService<PaymentAudit>
    {   
        private readonly PaymentAuditDBContext _context;
        public StorageService(PaymentAuditDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<OneOf<PaymentAudit, NotFoundResponse>> Get(string id)
        {
            if(string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));
            try
            {
                var paymentRecord = await _context.PaymentAudits.FirstOrDefaultAsync(x => x.BankTranscationId == id);
                if(paymentRecord is null )
                {
                    return new NotFoundResponse(){
                        ErrorMessage = $"Payment record was not found for {id}"
                    };
                }
                return paymentRecord;
            }
            catch(Exception ex)
            {
                throw new StorageException("Something went wrong when trying to get payment audit from database, please check inner exception", ex);
            }
        }

        public async Task Upsert(PaymentAudit paymentObject)
        {
            if(paymentObject is null)
                throw new ArgumentNullException(nameof(paymentObject));
            try
            {
                if(await _context.PaymentAudits.FirstOrDefaultAsync(x => x.BankTranscationId == paymentObject.BankTranscationId) is null)
                {
                    _context.PaymentAudits.Add(paymentObject);
                }
                else
                {
                    _context.PaymentAudits.Update(paymentObject);
                }
                               
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw new StorageException("Something went wrong when trying to upsert payment audit from database, please check inner exception", ex);
            }
        }
    }
}