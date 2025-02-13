using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using MvcNetCoreProceduresEF.Models;
using MvcNetCoreProceduresEF.Repositories;

namespace MvcNetCoreProceduresEF.Controllers
{
    public class EnfermosController : Controller
    {
        private RepositoryEnfermos repo;
        public EnfermosController(RepositoryEnfermos repo)
        {
            this.repo = repo;
        }
        public async Task<IActionResult> Index()
        {
            List<Enfermo> enfermos = await this.repo.GetEnfermos();
            return View(enfermos);
        }
        public async Task<IActionResult> Details
            (string inscripcion)
        {
            Enfermo enfermo = await this.repo.FindEnfermoAsync(inscripcion);
            return View(enfermo);
        }
        public async Task<IActionResult> Delete
            (string inscripcion)
        {
            await this.repo.DeleteEnfermoRawAsync(inscripcion);
            return RedirectToAction("Index");
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create
            (Enfermo enfermo)
        {
            await this.repo.InsertEnfermo(enfermo.Apellido, enfermo.Direccion, enfermo.FechaNacimiento, enfermo.Genero);
            return RedirectToAction("Index");
        }
    }
}
