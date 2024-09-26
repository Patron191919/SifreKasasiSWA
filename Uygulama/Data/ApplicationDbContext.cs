using Microsoft.EntityFrameworkCore;
using Uygulama.Models;

namespace Uygulama.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<UserInput> UserInputs { get; set; }
        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<SiteBilgileri> SiteBilgileri { get; set; }

    }
}
