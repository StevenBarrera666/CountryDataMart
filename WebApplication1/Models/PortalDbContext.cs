using System.Data.Entity;
using WebApplication1.Models.Entities;
namespace WebApplication1.Models
{
    public class PortalDbContext : DbContext
    {
        public PortalDbContext() : base("name=PortalDbContext")
        {
            // Evita la sobrecarga de proxy si serializas a JSON
            this.Configuration.LazyLoadingEnabled = true;
        }

        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Cargo> Cargos { get; set; }
        public DbSet<EpsPrevision> EpsPrevisiones { get; set; }
    }
}