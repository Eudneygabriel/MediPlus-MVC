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
        // Estatísticas para os Cards
        ViewBag.TotalPacientes = await _context.Paciente.CountAsync();
        ViewBag.TotalMedicos = await _context.Medico.CountAsync();
        
        // Consultas de Hoje
        var hoje = DateTime.Today;
        ViewBag.ConsultasHoje = await _context.Marcacao
            .CountAsync(m => m.DataHora.Date == hoje);

        // Dados para o Gráfico (Estados)
        ViewBag.Realizadas = await _context.Marcacao.CountAsync(m => m.Estado == "Realizada");
        ViewBag.Faltas = await _context.Marcacao.CountAsync(m => m.Estado == "Faltou");
        ViewBag.Pendentes = await _context.Marcacao.CountAsync(m => m.Estado == "Confirmada");

        // Lista de Próximas Consultas (Top 5)
        ViewBag.ProximasConsultas = await _context.Marcacao
            .Include(m => m.Paciente)
            .Include(m => m.Medico)
            .Where(m => m.DataHora >= DateTime.Now && m.Estado == "Confirmada")
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