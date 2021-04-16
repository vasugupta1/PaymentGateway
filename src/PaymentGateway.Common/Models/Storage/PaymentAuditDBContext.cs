using Microsoft.EntityFrameworkCore;

namespace PaymentGateway.Common.Models.Storage
{
    public class PaymentAuditDBContext : DbContext
    {
        public DbSet<PaymentAudit> PaymentAudits { get; set; }
        public PaymentAuditDBContext(DbContextOptions<PaymentAuditDBContext> options): base(options) 
        {

        }
        public PaymentAuditDBContext()
        {
            
        }
    }
}