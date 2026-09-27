using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class SolicitudEstudioDetalle
    {
        public int IdSolicitudEstudioDetalle { get; set; }
        public int IdSolicitudEstudio { get; set; }
        public int IdCatalogoEstudio { get; set; }
        public string Indicaciones { get; set; }
        public string Estado { get; set; }

        // Propiedades de navegación
        public SolicitudEstudio SolicitudEstudio { get; set; }
        public CatalogoEstudio CatalogoEstudio { get; set; }
        public ResultadoEstudio ResultadoEstudio { get; set; }
    }
}
