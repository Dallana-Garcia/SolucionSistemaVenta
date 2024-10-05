using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaVenta.Entity;

namespace SistemaVenta.BLL.Interfaces
{
    public interface IVentaService
    {
        //Devuelve la lista de productos

        Task<List<Producto>> ObtenerProductos(string busqueda);

        Task<Venta> Registrar(Venta entidad);

        Task<List<Venta>> Historial(string numeroVenta, string fechaInicio, string fechaFin);

        Task<Venta> Detalle(string numeroVenta);

        //Tener un reporte de venta según un rango de fecha.
        Task<List<DetalleVenta>> Reporte(string fechaInicio, string fechaFin); 
    }
}
