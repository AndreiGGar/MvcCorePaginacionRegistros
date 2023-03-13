using MvcCorePaginacionRegistros.Context;
using MvcCorePaginacionRegistros.Models;

namespace MvcCorePaginacionRegistros.Repositories
{
    public class RepositoryDepartamentos
    {
        private DataContext context;

        public RepositoryDepartamentos(DataContext context)
        {
            this.context = context;
        }

        public List<Departamento> GetDepartamentos()
        {
            return this.context.Departamentos.ToList();
        }

        public Departamento FindDepartamento(int iddepartamento)
        {
            return this.context.Departamentos.FirstOrDefault(x => x.DeptNo == iddepartamento);
        }
    }
}
