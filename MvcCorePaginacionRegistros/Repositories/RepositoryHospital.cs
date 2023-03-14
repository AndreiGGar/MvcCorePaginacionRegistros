using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcCorePaginacionRegistros.Context;
using MvcCorePaginacionRegistros.Models;

namespace MvcCorePaginacionRegistros.Repositories
{
    public class RepositoryHospital
    {
        private DataContext context;

        public RepositoryHospital(DataContext context)
        {
            this.context = context;
        }

        public int GetNumeroRegistrosVistaDepartamento()
        {
            return this.context.VistaDepartamentos.Count();
        }

        public int GetNumeroRegistrosVistaEmpleado()
        {
            return this.context.VistaEmpleados.Count();
        }

        public int GetNumeroRegistrosVistaEmpleadoDept(int deptno)
        {
            return this.context.VistaEmpleados.Count(x => x.Dept_no == deptno);
        }

        public async Task<List<Departamento>> GetDepartamentoAsync(int posicion)
        {
            string sql = "SP_GRUPO_DEPARTAMENTOS @POSICION";
            SqlParameter pamposicion = new SqlParameter("@POSICION", posicion);
            var consulta = this.context.Departamentos.FromSqlRaw(sql, pamposicion);
            return await consulta.ToListAsync();
        }

        public async Task<VistaDepartamento> GetVistaDepartamentoAsync(int posicion)
        {
            VistaDepartamento vista = await this.context.VistaDepartamentos.FirstOrDefaultAsync(x => x.Posicion == posicion);
            return vista;
        }

        public async Task<List<VistaDepartamento>> GetGrupoVistaDepartamentoAsync(int posicion)
        {
            var consulta = from datos in this.context.VistaDepartamentos
                           where datos.Posicion >= posicion
                           && datos.Posicion < (posicion + 2)
                           select datos;
            return await consulta.ToListAsync();
        }

        public async Task<List<Empleado>> GetEmpleadoAsync(int deptno, int posicion)
        {
            string sql = "SP_GRUPO_EMPLEADOS @DEPT_NO, @POSICION";
            SqlParameter pamdeptno = new SqlParameter("@DEPT_NO", deptno);
            SqlParameter pamposicion = new SqlParameter("@POSICION", posicion);
            var consulta = this.context.Empleados.FromSqlRaw(sql, pamdeptno, pamposicion);
            return await consulta.ToListAsync();
        }
    }
}
