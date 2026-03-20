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

        // 1. LISTAGEM PRINCIPAL (AGENDA)
        public async Task<IActionResult> Index()
        {
            var marcacoes = await _context.Marcacao
                .Include(m => m.Medico)
                    .ThenInclude(med => med.Especialidade) // Para mostrar a especialidade na agenda
                .Include(m => m.Paciente)
                .OrderByDescending(m => m.DataHora) 
                .ToListAsync();
            return View(marcacoes);
        }

        // 2. MUDAR ESTADO PARA REALIZADA (CONCLUIR)
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

        // 3. MUDAR ESTADO PARA FALTOU
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarcarFalta(int id)
        {
            var marcacao = await _context.Marcacao.FindAsync(id);
            if (marcacao != null)
            {
                marcacao.Estado = "Faltou";
                _context.Update(marcacao);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // 4. ABRIR FORMULÁRIO DE CRIAÇÃO
        public IActionResult Create()
        {
            ViewBag.EspecialidadeId = new SelectList(_context.Especialidade, "EspecialidadeId", "Nome");
            ViewBag.PacienteId = new SelectList(_context.Paciente, "PacienteId", "Nome");
            ViewBag.MedicoId = new SelectList(Enumerable.Empty<SelectListItem>());
            
            return View();
        }

        // 5. GRAVAR AGENDAMENTO COM VALIDAÇÃO DE CONFLITO
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Marcacao marcacao)
        {
            ModelState.Remove("Paciente");
            ModelState.Remove("Medico");

            if (ModelState.IsValid)
            {
                // Validação 1: Data Passada
                if (marcacao.DataHora < DateTime.Now.AddMinutes(-5))
                {
                    ModelState.AddModelError("DataHora", "Não pode agendar consultas no passado.");
                }
                else 
                {
                    // Validação 2: CONFLITO DE HORÁRIO (O ponto crítico!)
                    // Verifica se o mesmo médico já tem uma marcação (Confirmada ou Realizada) na mesma hora
                    var existeConflito = await _context.Marcacao
                        .AnyAsync(m => m.MedicoId == marcacao.MedicoId 
                                  && m.DataHora == marcacao.DataHora 
                                  && m.Estado != "Faltou");

                    if (existeConflito)
                    {
                        ModelState.AddModelError("DataHora", "Este médico já possui uma consulta agendada para este horário exato.");
                    }
                    else
                    {
                        marcacao.Estado = "Confirmada"; 
                        _context.Add(marcacao);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
            }

            // Se chegou aqui, algo falhou. Recarregamos as listas para a View.
            ViewBag.EspecialidadeId = new SelectList(_context.Especialidade, "EspecialidadeId", "Nome");
            ViewBag.PacienteId = new SelectList(_context.Paciente, "PacienteId", "Nome", marcacao.PacienteId);
            
            // Recarrega os médicos da especialidade selecionada para não perder a seleção no erro
            var medicoTemp = await _context.Medico.FindAsync(marcacao.MedicoId);
            if (medicoTemp != null) {
                ViewBag.MedicoId = new SelectList(_context.Medico.Where(m => m.EspecialidadeId == medicoTemp.EspecialidadeId), "MedicoId", "Nome", marcacao.MedicoId);
            } else {
                ViewBag.MedicoId = new SelectList(Enumerable.Empty<SelectListItem>());
            }
            
            return View(marcacao);
        }

        // 6. FILTRO AJAX DE MÉDICOS
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