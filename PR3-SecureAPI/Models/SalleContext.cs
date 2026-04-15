using Microsoft.EntityFrameworkCore;

namespace PR3_SecureAPI.Models
{
    public class SalleContext : DbContext
    {
        public SalleContext(DbContextOptions<SalleContext> options) : base(options)
        {
        }

        public DbSet<Salle> Salle { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Salle>()
                .HasIndex(s => new { s.Numero, s.EtablissementId })
                .IsUnique();
        }
    }
}