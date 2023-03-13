using MvcCorePaginacionRegistros.Context;
using MvcCorePaginacionRegistros.Models;

namespace MvcCorePaginacionRegistros.Repositories
{
    public class RepositoryEmpleados
    {
        private DataContext context;

        public RepositoryEmpleados(DataContext context)
        {
            this.context = context;
        }

        public List<Empleado> GetEmpleados()
        {
            return this.context.Empleados.ToList();
        }

        public Empleado FindEmpleado(int idempleado)
        {
            return this.context.Empleados.FirstOrDefault(x => x.IdEmpleado == idempleado);
        }

        public List<Empleado> FindEmpleados(int deptno)
        {
            var consulta = from datos in this.context.Empleados
                           where datos.Dept_no == deptno
                           select datos;
            return consulta.ToList();
        }
    }
}
