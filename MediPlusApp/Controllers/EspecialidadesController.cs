using Microsoft.AspNetCore.Mvc;

namespace MediPlusApp.Controllers
{
    public class EspecialidadesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}