using Microsoft.AspNetCore.Mvc;

using AutoMapper;

using SistemaVenta.AplicacionWeb.Models.ViewModels;
using SistemaVenta.BLL.Interfaces;
using SistemaVenta.Entity;

namespace SistemaVenta.AplicacionWeb.Controllers
{
    public class PlantillaController : Controller
    {
        //Variables para los servicios del PDF
        private readonly IMapper _mapper;
        private readonly INegocioService _negocioServicio;
        private readonly IVentaService _ventaServicio;

        public PlantillaController(IMapper mapper,
        INegocioService negocioServicio, 
        IVentaService ventaServicio)
        {
            //Se asigna el valor que tendrá cada uno
            _mapper = mapper;
            _negocioServicio = negocioServicio; 
            _ventaServicio = ventaServicio;
        }

        public IActionResult EnviarClave(string correo, string clave)
        {
            ViewData["Correo"] = correo; //Permitirá compartir info con la vista
            ViewData["Clave"] = clave;
            ViewData["Url"] = $"{this.Request.Scheme}://{this.Request.Host}";

            return View();
        }

        //Crear el método que generará la vista
        public async Task<IActionResult> PDFVenta(string numeroVenta)
        {

            VMVenta vmVenta = _mapper.Map<VMVenta>(await _ventaServicio.Detalle(numeroVenta)); 
            VMNegocio vmNegocio = _mapper.Map<VMNegocio>(await _negocioServicio.Obtener());

            VMPDFVenta modelo = new VMPDFVenta();

            modelo.negocio = vmNegocio;
            modelo.venta = vmVenta;

            return View(modelo);
        }

        public IActionResult RestablecerClave(string clave)
        {
            ViewData["Clave"] = clave;
            return View();
        }
    }
}
