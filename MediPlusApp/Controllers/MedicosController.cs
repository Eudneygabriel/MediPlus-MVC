using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MediPlusApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MediPlusApp.Controllers
{
    public class MedicosController : Controller
    {
        private readonly MediPlusContext _context;

        public MedicosController(MediPlusContext context)
        {
            _context = context;
        }

        // Listagem de médicos
        public async Task<IActionResult> Index()
        {
            var medicos = await _context.Medico.Include(m => m.Especialidade).ToListAsync();
            return View(medicos);
        }

        // GET: Medicos/Create (Abre o formulário)
        public IActionResult Create()
        {
            // Carrega as especialidades existentes para a lista de seleção
            ViewBag.EspecialidadeId = new SelectList(_context.Especialidade, "EspecialidadeId", "Nome");
            return View();
        }

        // POST: Medicos/Create (Guarda o médico)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MedicoId,Nome,Cedula,Email,EspecialidadeId,Bio")] Medico medico)
        {
            if (ModelState.IsValid)
            {
                _context.Add(medico);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.EspecialidadeId = new SelectList(_context.Especialidade, "EspecialidadeId", "Nome", medico.EspecialidadeId);
            return View(medico);
        }
    }
}