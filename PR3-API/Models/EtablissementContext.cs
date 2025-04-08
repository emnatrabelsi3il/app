using Microsoft.EntityFrameworkCore;

namespace PR3_API.Models
{
    public class EtablissementContext : DbContext
    {

        public EtablissementContext(DbContextOptions<EtablissementContext> options) : base(options)
        {
        }

        public DbSet<Etablissement> Etablissement { get; set; } = null!;
    }
}
