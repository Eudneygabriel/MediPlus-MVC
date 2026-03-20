using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MediPlusApp.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace MediPlusApp.Controllers
{
    public class MedicosController : Controller
    {
        private readonly MediPlusContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MedicosController(MediPlusContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // 1. LISTAGEM: Corpo Clínico
        public async Task<IActionResult> Index()
        {
            var medicos = await _context.Medico.Include(m => m.Especialidade).ToListAsync();
            return View(medicos);
        }

        // 2. DETALHES: Carrega histórico e estatísticas para a View
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var medico = await _context.Medico
                .Include(m => m.Especialidade)
                .Include(m => m.Marcacoes)
                    .ThenInclude(mar => mar.Paciente)
                .FirstOrDefaultAsync(m => m.MedicoId == id);

            if (medico == null) return NotFound();

            // Passamos as contagens para as badges de "Resumo de Atividade"
            ViewBag.TotalRealizadas = medico.Marcacoes.Count(m => m.Estado == "Realizada");
            ViewBag.TotalFaltas = medico.Marcacoes.Count(m => m.Estado == "Faltou");

            return View(medico);
        }

        // 3. CREATE (POST): Com tratamento de imagem profissional
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MedicoId,Nome,Cedula,Email,EspecialidadeId,Bio")] Medico medico, IFormFile? FotoFile)
        {
            if (ModelState.IsValid)
            {
                if (FotoFile != null)
                {
                    medico.FotoPath = await SalvarFoto(FotoFile);
                }

                _context.Add(medico);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.EspecialidadeId = new SelectList(_context.Especialidade, "EspecialidadeId", "Nome", medico.EspecialidadeId);
            return View(medico);
        }

        // 4. EDIT (POST): Substitui a foto antiga se uma nova for enviada
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MedicoId,Nome,Cedula,Email,EspecialidadeId,Bio,FotoPath")] Medico medico, IFormFile? FotoFile)
        {
            if (id != medico.MedicoId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (FotoFile != null)
                    {
                        // Se já existia uma foto, removemos o ficheiro antigo para poupar espaço
                        if (!string.IsNullOrEmpty(medico.FotoPath))
                        {
                            EliminarFotoAntiga(medico.FotoPath);
                        }
                        medico.FotoPath = await SalvarFoto(FotoFile);
                    }

                    _context.Update(medico);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Medico.Any(e => e.MedicoId == id)) return NotFound();
                    else throw;
                }
            }
            ViewBag.EspecialidadeId = new SelectList(_context.Especialidade, "EspecialidadeId", "Nome", medico.EspecialidadeId);
            return View(medico);
        }

        // 5. DELETE: Remove o registo e o ficheiro de imagem do servidor
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medico = await _context.Medico.FindAsync(id);
            if (medico != null)
            {
                if (!string.IsNullOrEmpty(medico.FotoPath))
                {
                    EliminarFotoAntiga(medico.FotoPath);
                }
                _context.Medico.Remove(medico);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // MÉTODOS AUXILIARES (Private) para manter o código limpo
        private async Task<string> SalvarFoto(IFormFile file)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
            string path = Path.Combine(wwwRootPath, "images", "medicos", fileName);

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await fileStream.CopyToAsync(fileStream);
            }
            return fileName;
        }

        private void EliminarFotoAntiga(string fileName)
        {
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "images", "medicos", fileName);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }
    }
}