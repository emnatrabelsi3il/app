using Microsoft.EntityFrameworkCore;

namespace PR3_SecureAPI.Models
{
    public class PosteContext : DbContext
    {
        public PosteContext(DbContextOptions<PosteContext> options) : base(options)
        {
        }
        public DbSet<Salle> Salle { get; set; }
        public DbSet<Etablissement> Etablissement { get; set; }
        public DbSet<CommandePoste> CommandesPostes { get; set; }

        public DbSet<Poste> Poste { get; set; } = null!;
    }
}
