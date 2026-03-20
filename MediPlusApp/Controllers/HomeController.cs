using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MediPlusApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MediPlusApp.Controllers;

public class HomeController : Controller
{
    private readonly MediPlusContext _context;

    public HomeController(MediPlusContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var hoje = DateTime.Today;

        // 1. Estatísticas para os Cards
        ViewBag.TotalPacientes = await _context.Paciente.CountAsync();
        ViewBag.TotalMedicos = await _context.Medico.CountAsync();
        ViewBag.ConsultasHoje = await _context.Marcacao
            .CountAsync(m => m.DataHora.Date == hoje);

        // 2. Dados para o Gráfico (Estados)
        ViewBag.Realizadas = await _context.Marcacao.CountAsync(m => m.Estado == "Realizada" || m.Estado == "Concluída");
        ViewBag.Faltas = await _context.Marcacao.CountAsync(m => m.Estado == "Faltou");
        ViewBag.Pendentes = await _context.Marcacao.CountAsync(m => m.Estado == "Confirmada" || m.Estado == "Pendente");

        // 3. Lista de Próximos Atendimentos (CORREÇÃO AQUI)
        // Mostra todas as consultas de HOJE que ainda não foram finalizadas (Realizada/Faltou),
        // mesmo que o horário já tenha passado um pouco.
        ViewBag.ProximasConsultas = await _context.Marcacao
            .Include(m => m.Paciente)
            .Include(m => m.Medico)
            .Where(m => m.DataHora.Date == hoje && (m.Estado == "Confirmada" || m.Estado == "Pendente"))
            .OrderBy(m => m.DataHora)
            .Take(5)
            .ToListAsync();

        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}