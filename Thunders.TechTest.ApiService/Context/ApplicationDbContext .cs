using Microsoft.EntityFrameworkCore;
using Thunders.TechTest.ApiService.Entities;

namespace Thunders.TechTest.ApiService.Context
{
    public class ThundersTechTestDbContext : DbContext
    {
        public ThundersTechTestDbContext(DbContextOptions<ThundersTechTestDbContext> options)
            : base(options)
        {
        }

        public DbSet<Pedagio> Pedagios { get; set; }
        public DbSet<Relatorio> Relatorios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
