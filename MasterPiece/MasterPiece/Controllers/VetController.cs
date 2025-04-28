using Microsoft.AspNetCore.Mvc;

namespace MasterPiece.Controllers
{
    public class VetController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
