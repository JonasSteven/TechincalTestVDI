using Microsoft.EntityFrameworkCore;
using TechnicalTestVDI.Models;

namespace TechnicalTestVDI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<DiscountTransaction> DiscountTransactions { get; set; }
    }
}
