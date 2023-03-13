using Microsoft.AspNetCore.Mvc;
using MvcCorePaginacionRegistros.Repositories;
using MvcCorePaginacionRegistros.Models;

namespace MvcCorePaginacionRegistros.ViewComponents
{
    public class MenuDepartamentosViewComponent: ViewComponent
    {
        private RepositoryDepartamentos repo;

        public MenuDepartamentosViewComponent(RepositoryDepartamentos repo)
        {
            this.repo = repo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Departamento> departamentos = this.repo.GetDepartamentos();
            return View(departamentos);
        }
    }
}
