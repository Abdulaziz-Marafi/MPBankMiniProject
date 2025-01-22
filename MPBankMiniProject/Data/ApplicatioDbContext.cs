using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MPBankMiniProject.Models;

namespace MPBankMiniProject.Data
{
    public class ApplicatioDbContext : IdentityDbContext<ApplicationUser>
    {

        public ApplicatioDbContext(DbContextOptions<ApplicatioDbContext>options):base(options) { }

        
        public DbSet<Transaction> Transactions { get; set; }
    }
}
