using Microsoft.AspNetCore.Mvc;

namespace MediPlusApp.Controllers
{
    public class MarcacoesController : Controller
    {
        // Esta função abre a lista de marcações
        public IActionResult Index() 
        {
            return View();
        }

        // Esta função abre a página de criação (Nova Marcação)
        public IActionResult Create()
        {
            return View();
        }
    }
}