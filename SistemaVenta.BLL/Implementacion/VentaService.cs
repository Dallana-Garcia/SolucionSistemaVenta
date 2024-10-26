using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SistemaVenta.BLL.Interfaces;
using SistemaVenta.DAL.Interfaces;
using SistemaVenta.Entity;

namespace SistemaVenta.BLL.Implementacion
{
    public class VentaService : IVentaService
    {
            //Se usará los reporitorios 
            private readonly IGenericRepository<Producto> _repositorioProducto;
            private readonly IVentaRepository _repositorioVenta;
        

        public VentaService(IGenericRepository<Producto> repositorioProducto,
                            IVentaRepository repositorioVenta
            )
        {
            _repositorioProducto = repositorioProducto;
            _repositorioVenta = repositorioVenta;
        }


        //Todos los productos relacionados con esa búsqueda...
        public async Task<List<Producto>> ObtenerProductos(string busqueda)
        {
            IQueryable<Producto> query = await _repositorioProducto.Consultar(p => 
                p.EsActivo == true &&
                p.Stock > 0 &&
                string.Concat(p.CodigoBarra,p.Marca,p.Descripcion).Contains(busqueda)
                );

            return query.Include(c => c.IdCategoriaNavigation).ToList();
        }

        public async Task<Venta> Registrar(Venta entidad)
        {
            try
            {
                return await _repositorioVenta.Registrar(entidad);
            }
            catch //(Exception)
            {

                throw;
            }
        }


        public async Task<List<Venta>> Historial(string numeroVenta, string fechaInicio, string fechaFin)
        {
            IQueryable<Venta> query = await _repositorioVenta.Consultar();
            fechaInicio = fechaInicio is null ? "":fechaInicio;
            fechaFin = fechaFin is null ? "" : fechaFin;

            if(fechaInicio != "" && fechaFin != "")
            {
                DateTime fech_inicio = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy",new CultureInfo("es-GT"));
                DateTime fech_Fin = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", new CultureInfo("es-GT"));


                // Filtrar todas aquellas fechas que están dentro del rango de inicio y el rango de fin
                return query.Where(v =>
                v.FechaRegistro.Value.Date >= fech_inicio.Date &&
                v.FechaRegistro.Value.Date <= fech_Fin.Date
                )
                    //Se llaman a las tablas
                    .Include(tdv => tdv.IdTipoDocumentoVentaNavigation)
                    .Include(u => u.IdUsuarioNavigation)
                    .Include(dv => dv.DetalleVenta)
                    .ToList();
            }
            //Si en caso se realiza la busqueda por tipo de venta se usará otro tipo de variable
            else
            {
                return query.Where(v => v.NumeroVenta == numeroVenta
                )
                    //Se llaman a las tablas
                    .Include(tdv => tdv.IdTipoDocumentoVentaNavigation) //Tipo de documento realizada en la venta
                    .Include(u => u.IdUsuarioNavigation) //El usuario que ha realizado esa venta
                    .Include(dv => dv.DetalleVenta) //Todo el detalle de la venta
                    .ToList();
            }
        }


        public async Task<Venta> Detalle(string numeroVenta)
        {
            //OBTENDREMOS EL DETALLE DE CADA VENTA, se ele coloca un alias y se verifica si el número de venta es igual al que se está recibiendo como parametro
            IQueryable<Venta> query = await _repositorioVenta.Consultar(v => v.NumeroVenta == numeroVenta);

            return query
                   //Se llaman a las tablas
                   .Include(tdv => tdv.IdTipoDocumentoVentaNavigation) //Tipo de documento realizada en la venta
                    .Include(u => u.IdUsuarioNavigation) //El usuario que ha realizado esa venta
                    .Include(dv => dv.DetalleVenta) //Todo el detalle de la venta
                    .First(); //Solo mostrar el primero que encuentre al buscar

            
        }


        public async Task<List<DetalleVenta>> Reporte(string fechaInicio, string fechaFin)
        {
            DateTime fech_inicio = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", new CultureInfo("es-GT"));
            DateTime fech_Fin = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", new CultureInfo("es-GT"));

            //Crear una lista de detalle de venta
            List<DetalleVenta> lista = await _repositorioVenta.Reporte(fech_inicio, fech_Fin);

            return lista;

        }
    }
}
