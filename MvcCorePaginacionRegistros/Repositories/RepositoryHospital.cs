using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcCorePaginacionRegistros.Context;
using MvcCorePaginacionRegistros.Models;
using System.Data;

namespace MvcCorePaginacionRegistros.Repositories
{
    #region PROCEDURES
    /*CREATE VIEW V_DEPARTAMENTOS_INDIVIDUAL
        AS
        SELECT CAST(ROW_NUMBER() OVER (ORDER BY DEPT_NO) AS INT) AS POSICION, ISNULL(DEPT_NO, 0) AS DEPT_NO, DNOMBRE, LOC FROM DEPT
        GO

        INSERT INTO DEPT VALUES (60, 'INFORMATICA', 'GIJON')

        CREATE PROCEDURE SP_GRUPO_DEPARTAMENTOS (@POSICION INT) 
        AS 
            SELECT DEPT_NO, DNOMBRE, LOC FROM V_DEPARTAMENTOS_INDIVIDUAL WHERE POSICION >= @POSICION AND POSICION < (@POSICION + 2) 
        GO

        CREATE VIEW V_EMPLEADOS_INDIVIDUAL
        AS
        SELECT CAST(ROW_NUMBER() OVER (ORDER BY DEPT_NO) AS INT) AS POSICION, ISNULL(EMP_NO, 0) AS EMP_NO, APELLIDO, OFICIO, SALARIO, DEPT_NO FROM EMP
        GO

        ALTER PROCEDURE SP_GRUPO_EMPLEADOS (@DEPT_NO INT, @POSICION INT) 
        AS 
            SELECT * FROM (SELECT CAST(ROW_NUMBER() OVER (ORDER BY APELLIDO) AS INT) AS POSICION, ISNULL(EMP_NO, 0) AS EMP_NO, APELLIDO, OFICIO, SALARIO, DEPT_NO FROM EMP WHERE DEPT_NO = @DEPT_NO) AS QUERY WHERE QUERY.POSICION >= @POSICION AND QUERY.POSICION < (@POSICION + 3)
        GO
        CREATE PROCEDURE SP_GRUPO_EMPLEADOS_OFICIO (@POSICION INT, @OFICIO NVARCHAR(50))
        AS
	        SELECT EMP_NO, APELLIDO, OFICIO, SALARIO, DEPT_NO FROM
	        (SELECT CAST(ROW_NUMBER() OVER (ORDER BY APELLIDO) AS INT) AS POSICION, EMP_NO, APELLIDO, OFICIO, SALARIO, DEPT_NO FROM EMP WHERE OFICIO = @OFICIO) AS QUERY
	        WHERE QUERY.POSICION >= @POSICION AND QUERY.POSICION < (@POSICION + 3)
        GO
     */
    #endregion
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

        public int GetNumeroEmpleadosOficio(string oficio)
        {
            return this.context.VistaEmpleados.Count(x => x.Oficio == oficio);
        }

        public async Task<PaginarEmpleados> GetEmpleadosOficioAsync(int posicion, int cantidad, string oficio)
        {
            string sql = "SP_GRUPO_EMPLEADOS_OFICIO @POSICION, @CANTIDAD, @OFICIO, @NUMEROREGISTROS OUT";
            SqlParameter pamposicion = new SqlParameter("@POSICION", posicion);
            SqlParameter pamcantidad = new SqlParameter("@CANTIDAD", cantidad);
            SqlParameter pamoficio = new SqlParameter("@OFICIO", oficio);
            SqlParameter pamregistros = new SqlParameter("@NUMEROREGISTROS", -1);
            pamregistros.Direction = ParameterDirection.Output;
            var consulta = this.context.Empleados.FromSqlRaw(sql, pamposicion, pamcantidad, pamoficio, pamregistros);
            List<Empleado> empleados = await consulta.ToListAsync();
            int registros = (int)pamregistros.Value;
            return new PaginarEmpleados
            {
                NumeroRegistros = registros,
                Empleados = empleados,
            };
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
