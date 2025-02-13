using Microsoft.AspNetCore.Mvc;
using MvcNetCoreProceduresEF.Models;
using MvcNetCoreProceduresEF.Repositories;

namespace MvcNetCoreProceduresEF.Controllers
{
    public class DoctoresController : Controller
    {
        private RepositoryDoctores repo;
        public DoctoresController(RepositoryDoctores repo)
        {
            this.repo = repo;
        }
        public async Task<IActionResult> Index()
        {
            ViewData["ESPECIALIDADES"] = await this.repo.GetEspecialidades();
            List<Doctor> doctores = await this.repo.GetDoctores();
            return View(doctores);
        }
        [HttpPost]
        public async Task<IActionResult> Index
            (strin)
        {
            ViewData["ESPECIALIDADES"] = await this.repo.GetEspecialidades();
            List<Doctor> doctores = await this.repo.IncrementarSalarioEspecialidad();
            return View(doctores);
        }
    }
}
