using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediPlusApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MediPlusApp.Controllers
{
    public class MarcacoesController : Controller
    {
        private readonly MediPlusContext _context;

        public MarcacoesController(MediPlusContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var marcacoes = await _context.Marcacao
                .Include(m => m.Medico)
                .Include(m => m.Paciente)
                .ToListAsync();
            return View(marcacoes);
        }

        // --- NOVO MÉTODO: Mudar estado para Realizada ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Concluir(int id)
        {
            var marcacao = await _context.Marcacao.FindAsync(id);
            
            if (marcacao != null)
            {
                marcacao.Estado = "Realizada";
                _context.Update(marcacao);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Create()
        {
            ViewBag.EspecialidadeId = new SelectList(_context.Medico.Select(m => m.Especialidade).Distinct(), "EspecialidadeId", "Nome");
            ViewBag.PacienteId = new SelectList(_context.Paciente, "PacienteId", "Nome");
            ViewBag.MedicoId = new SelectList(Enumerable.Empty<SelectListItem>());
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Marcacao marcacao)
        {
            if (ModelState.IsValid)
            {
                if (marcacao.DataHora < DateTime.Now)
                {
                    ModelState.AddModelError("DataHora", "Não agende no passado.");
                }
                else 
                {
                    // Por segurança, garantimos que nasce como Confirmada
                    marcacao.Estado = "Confirmada"; 
                    _context.Add(marcacao);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewBag.EspecialidadeId = new SelectList(_context.Medico.Select(m => m.Especialidade).Distinct(), "EspecialidadeId", "Nome");
            ViewBag.MedicoId = new SelectList(_context.Medico, "MedicoId", "Nome", marcacao.MedicoId);
            ViewBag.PacienteId = new SelectList(_context.Paciente, "PacienteId", "Nome", marcacao.PacienteId);
            return View(marcacao);
        }

        [HttpGet]
        public JsonResult GetMedicosPorEspecialidade(int especialidadeId)
        {
            var medicos = _context.Medico
                .Where(m => m.EspecialidadeId == especialidadeId)
                .Select(m => new { id = m.MedicoId, nome = m.Nome })
                .ToList();
                
            return Json(medicos);
        }
    }
}