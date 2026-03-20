using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediPlusApp.Models;

namespace MediPlusApp.Controllers
{
    public class PacientesController : Controller
    {
        private readonly MediPlusContext _context;

        public PacientesController(MediPlusContext context)
        {
            _context = context;
        }

        // 1. LISTAR: Ordenado por nome
        public async Task<IActionResult> Index()
        {
            var pacientes = await _context.Paciente.OrderBy(p => p.Nome).ToListAsync();
            return View(pacientes);
        }

        // 2. DETALHES: Carrega o histórico de consultas e o nome do médico
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var paciente = await _context.Paciente
                .Include(p => p.Marcacoes) 
                    .ThenInclude(m => m.Medico)
                .FirstOrDefaultAsync(m => m.PacienteId == id);
            
            if (paciente == null) return NotFound();

            return View(paciente);
        }

        // 3. CRIAR (GET)
        public IActionResult Create()
        {
            return View();
        }

        // 4. CRIAR (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PacienteId,Nome,Email,Telemovel,SNS")] Paciente paciente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(paciente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(paciente);
        }

        // 5. EDITAR (GET)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var paciente = await _context.Paciente.FindAsync(id);
            if (paciente == null) return NotFound();
            
            return View(paciente);
        }

        // 6. EDITAR (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PacienteId,Nome,Email,Telemovel,SNS")] Paciente paciente)
        {
            if (id != paciente.PacienteId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(paciente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Paciente.Any(e => e.PacienteId == paciente.PacienteId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(paciente);
        }

        // 7. ELIMINAR (GET)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var paciente = await _context.Paciente
                .FirstOrDefaultAsync(m => m.PacienteId == id);
            
            if (paciente == null) return NotFound();

            return View(paciente);
        }

        // 8. ELIMINAR (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var paciente = await _context.Paciente.FindAsync(id);
            if (paciente != null)
            {
                _context.Paciente.Remove(paciente);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}