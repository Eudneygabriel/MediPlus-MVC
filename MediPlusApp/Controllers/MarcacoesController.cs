using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediPlusApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList; 
using X.PagedList.Extensions; 

namespace MediPlusApp.Controllers
{
    public class MarcacoesController : Controller
    {
        private readonly MediPlusContext _context;

        public MarcacoesController(MediPlusContext context)
        {
            _context = context;
        }

        // 1. LISTAGEM PRINCIPAL COM FILTROS E PAGINAÇÃO
        public IActionResult Index(string buscaMedico, DateTime? buscaData, int? pagina)
        {
            int numeroPagina = pagina ?? 1;
            int tamanhoPagina = 5;

            var query = _context.Marcacao
                .Include(m => m.Medico)
                    .ThenInclude(med => med!.Especialidade)
                .Include(m => m.Paciente)
                .AsNoTracking()
                .AsQueryable();

            // Filtros com verificação de nulidade para evitar avisos CS8602
            if (!string.IsNullOrWhiteSpace(buscaMedico))
            {
                query = query.Where(m => m.Medico != null && m.Medico.Nome.Contains(buscaMedico));
            }

            if (buscaData.HasValue)
            {
                query = query.Where(m => m.DataHora.Date == buscaData.Value.Date);
            }

            // Conversão para lista paginada (Versão Síncrona para estabilidade)
            var marcacoesPaginadas = query
                .OrderByDescending(m => m.DataHora)
                .ToPagedList(numeroPagina, tamanhoPagina);
            
            return View(marcacoesPaginadas);
        }

        // 2. MUDAR ESTADO PARA REALIZADA
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

        // 5. GRAVAR AGENDAMENTO
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Marcacao marcacao)
        {
            ModelState.Remove("Paciente");
            ModelState.Remove("Medico");

            if (ModelState.IsValid)
            {
                if (marcacao.DataHora < DateTime.Now.AddMinutes(-5))
                {
                    ModelState.AddModelError("DataHora", "Não pode agendar consultas no passado.");
                }
                else 
                {
                    var existeConflito = await _context.Marcacao
                        .AnyAsync(m => m.MedicoId == marcacao.MedicoId 
                                      && m.DataHora == marcacao.DataHora 
                                      && m.Estado != "Faltou");

                    if (existeConflito)
                    {
                        ModelState.AddModelError("DataHora", "Este médico já possui uma consulta agendada para este horário.");
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

            ViewBag.EspecialidadeId = new SelectList(_context.Especialidade, "EspecialidadeId", "Nome");
            ViewBag.PacienteId = new SelectList(_context.Paciente, "PacienteId", "Nome", marcacao.PacienteId);
            
            var medicoTemp = await _context.Medico.FindAsync(marcacao.MedicoId);
            if (medicoTemp != null) {
                ViewBag.MedicoId = new SelectList(_context.Medico.Where(m => m.EspecialidadeId == medicoTemp.EspecialidadeId), "MedicoId", "Nome", marcacao.MedicoId);
            } else {
                ViewBag.MedicoId = new SelectList(Enumerable.Empty<SelectListItem>());
            }
            
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