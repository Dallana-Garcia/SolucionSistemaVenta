using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SistemaVenta.BLL.Interfaces;
using SistemaVenta.DAL.Interfaces;
using SistemaVenta.Entity;

namespace SistemaVenta.BLL.Implementacion
{
    //A esta clase va a implementar el tipo de documento ITipoDocumentoVentaService
    public class TipoDocumentoVentaService : ITipoDocumentoVentaService
    {
        //Declarar la variable la cual es un repositorio
        private readonly IGenericRepository<TipoDocumentoVenta> _repositorio;

        public TipoDocumentoVentaService(IGenericRepository<TipoDocumentoVenta> repositorio)
        {
            //Y se recibe el valor del constructor
            _repositorio = repositorio;
        }

        public async Task<List<TipoDocumentoVenta>> Lista()
        {

            IQueryable<TipoDocumentoVenta> query = await _repositorio.Consultar();
            return query.ToList();
        }
    }
}
