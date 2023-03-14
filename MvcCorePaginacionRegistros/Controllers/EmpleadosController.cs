using Microsoft.AspNetCore.Mvc;
using MvcCorePaginacionRegistros.Models;
using MvcCorePaginacionRegistros.Repositories;

namespace MvcCorePaginacionRegistros.Controllers
{
    public class EmpleadosController : Controller
    {
        private RepositoryEmpleados repo;
        private RepositoryHospital repoH;

        public EmpleadosController(RepositoryEmpleados repo, RepositoryHospital repoH)
        {
            this.repo = repo;
            this.repoH = repoH;
        }
        /*public IActionResult Index(int deptno, int? posicion)
        {
            List<Empleado> empleados = this.repo.FindEmpleados(deptno);
            return View(empleados);
        }*/

        public async Task<IActionResult> Index(int deptno, int? posicion)
        {
            if (posicion == null)
            {
                posicion = 1;
            }
            int numregistros = this.repoH.GetNumeroRegistrosVistaEmpleadoDept(deptno);
            ViewData["REGISTROS"] = numregistros;
            ViewData["DEPTNO"] = deptno;
            List<Empleado> empleados = await this.repoH.GetEmpleadoAsync(deptno, posicion.Value);
            return View(empleados);
        }
    }
}
