using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SistemaVenta.AplicacionWeb.Controllers
{
    [Authorize]
    public class VentaController : Controller
    {
        public IActionResult NuevaVenta()
        {
            return View();
        }

        public IActionResult HistorialVenta()
        {
            return View();
        }
    }
}
