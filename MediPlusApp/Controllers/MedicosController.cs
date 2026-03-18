using Microsoft.AspNetCore.Mvc;

namespace MediPlusApp.Controllers
{
    public class MedicosController : Controller
    {
        public IActionResult Index() => View();
    }
}