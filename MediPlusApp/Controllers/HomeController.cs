using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MediPlusApp.Models;

namespace MediPlusApp.Controllers;

public class HomeController : Controller
{
    private readonly MediPlusContext _context;

    // Injetamos o contexto da base de dados aqui
    public HomeController(MediPlusContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        // Contamos os registos reais
        ViewBag.TotalPacientes = _context.Paciente.Count();
        ViewBag.TotalMedicos = _context.Medico.Count();
        
        // Contamos apenas as marcações de hoje
        ViewBag.ConsultasHoje = _context.Marcacao
            .Count(m => m.DataHora.Date == DateTime.Today);

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}