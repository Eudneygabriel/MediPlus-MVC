using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediPlusApp.Models;

namespace MediPlusApp.Controllers
{
    public class EspecialidadesController : Controller
    {
        private readonly MediPlusContext _context;

        // Injetamos o contexto para o Controller conseguir falar com a base de dados
        public EspecialidadesController(MediPlusContext context)
        {
            _context = context;
        }

        // 1. LISTAR: Vai à base de dados e envia a lista para a View Index
        public async Task<IActionResult> Index()
        {
            var especialidades = await _context.Especialidade.ToListAsync();
            return View(especialidades);
        }

        // 2. CRIAR (GET): Apenas abre a página com o formulário vazio
        public IActionResult Create()
        {
            return View();
        }

        // 3. CRIAR (POST): Recebe o que escreveste no formulário e guarda
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Especialidade especialidade)
        {
            if (ModelState.IsValid)
            {
                _context.Add(especialidade);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // Volta para a lista
            }
            return View(especialidade);
        }
    }
}