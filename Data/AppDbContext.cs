using Microsoft.EntityFrameworkCore;
using MinimalApi.Agendas;
using MinimalApi.Barbeiros;
using MinimalApi.Clientes;

namespace MinimalApi.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Barbeiro> Barbeiros { get; set; }
        public DbSet<Cliente> Clientes {get; set; }

        public DbSet<Agenda> Agenda { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Banco.databarber");

            //var con = "Server=localhost;Database=databarber;Uid=root;Pwd=positivo;",
            //optionsBuilder.UseMySQL(con);

            base.OnConfiguring(optionsBuilder);
        }
    }
}