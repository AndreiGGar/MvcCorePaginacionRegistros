using Microsoft.EntityFrameworkCore;
using MvcCorePaginacionRegistros.Models;

namespace MvcCorePaginacionRegistros.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Departamento> Departamentos { get; set; }
    }
}
