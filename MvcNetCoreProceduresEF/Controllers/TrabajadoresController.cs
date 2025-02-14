using Microsoft.AspNetCore.Mvc;
using MvcNetCoreProceduresEF.Models;
using MvcNetCoreProceduresEF.Repositories;

namespace MvcNetCoreProceduresEF.Controllers
{
    public class TrabajadoresController : Controller
    {
        RepositoryTrabajadores repo;
        public TrabajadoresController(RepositoryTrabajadores repo)
        {
            this.repo = repo;
        }
        public async Task<IActionResult> Index()
        {
            TrabajadoresModel model = await this.repo.GetTrabajadoresModelAsync();
            ViewData["OFICIOS"] = await this.repo.GetOficiosAsync();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Index
            (string oficio)
        {
            TrabajadoresModel model = await this.repo.GetTrabajadoresOficioAsync(oficio);
            ViewData["OFICIOS"] = await this.repo.GetOficiosAsync();
            return View(model);
        }
    }
}
