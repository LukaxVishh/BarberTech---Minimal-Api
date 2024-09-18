using Microsoft.EntityFrameworkCore;
using MinimalApi.Barbeiros;

namespace MinimalApi.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Barbeiro> Barbeiros { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Banco.databarber");
            base.OnConfiguring(optionsBuilder);
        }
    }
}