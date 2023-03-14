using Microsoft.AspNetCore.Mvc;
using MvcCorePaginacionRegistros.Models;
using MvcCorePaginacionRegistros.Repositories;

namespace MvcCorePaginacionRegistros.Controllers
{
    public class PaginacionController : Controller
    {
        private RepositoryHospital repo;
        public PaginacionController (RepositoryHospital repo)
        {
            this.repo = repo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> PaginarGrupoDepartamentos(int? posicion)
        {
            if (posicion == null)
            {
                posicion = 1;
            }
            int numregistros = this.repo.GetNumeroRegistrosVistaDepartamento();
            ViewData["REGISTROS"] = numregistros;
            List<Departamento> departamentos = await this.repo.GetDepartamentoAsync(posicion.Value);
            return View(departamentos);
        }

        public async Task<IActionResult> PaginarRegistroVistaDepartamento(int? posicion)
        {
            if (posicion == null)
            {
                posicion = 1;
            }
            int numregistros = this.repo.GetNumeroRegistrosVistaDepartamento();
            int siguiente = posicion.Value + 1;
            if (siguiente > numregistros)
            {
                siguiente = numregistros;
            }
            int anterior = posicion.Value - 1;
            if (anterior < 1)
            {
                anterior = 1;
            }
            VistaDepartamento vistaDepartamento = await this.repo.GetVistaDepartamentoAsync(posicion.Value);
            ViewData["ULTIMO"] = numregistros;
            ViewData["SIGUIENTE"] = siguiente;
            ViewData["ANTERIOR"] = anterior;
            return View(vistaDepartamento);
        }

        public async Task<IActionResult> PaginarGrupoVistaDepartamentos(int? posicion)
        {
            if (posicion == null)
            {
                posicion = 1;
            }
            int numregistros = this.repo.GetNumeroRegistrosVistaDepartamento();
            int numeroPagina = 1;
            string html = "<div>";
            for (int i = 1; i <= numregistros; i += 2)
            {
                html += "<a href='PaginarGrupoVistaDepartamentos?posicion=" + i + "'>Página " + numeroPagina + "</a> | ";
                numeroPagina += 1;
            }
            html += "</div>";
            ViewData["LINKS"] = html;
            ViewData["REGISTROS"] = numregistros;
            List<VistaDepartamento> departamentos = await this.repo.GetGrupoVistaDepartamentoAsync(posicion.Value);
            return View(departamentos);
        }
    }
}
