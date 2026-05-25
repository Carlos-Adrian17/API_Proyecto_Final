using Microsoft.EntityFrameworkCore;
using API_banco.Models;

namespace API_banco.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Cuenta> Cuentas { get; set; }
        public DbSet<Movimiento> Movimientos { get; set; }
        public DbSet<PagoServicio> Pagos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 🔥 NOMBRES ÚNICOS PARA NO CHOCAR
            modelBuilder.Entity<Cliente>().ToTable("clientes_banco_g3");
            modelBuilder.Entity<Cuenta>().ToTable("cuentas_banco_g3");
            modelBuilder.Entity<Movimiento>().ToTable("movimientos_banco_g3");
            modelBuilder.Entity<PagoServicio>().ToTable("pagos_banco_g3");
        }
    }
}