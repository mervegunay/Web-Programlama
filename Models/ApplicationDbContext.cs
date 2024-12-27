using Kuafor1.Models;
using Microsoft.EntityFrameworkCore;

namespace Kuafor1.Models
{
    public class ApplicationDbContext : DbContext // DbContext'ten türetildi
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Calisan> Calisanlar { get; set; }
        public DbSet<Hizmet> Hizmetler { get; set; }
        public DbSet<Randevu> Randevular { get; set; }
        public DbSet<Uye> Uyeler { get; set; }
    }
}

