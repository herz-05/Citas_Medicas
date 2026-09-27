using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class CatalogoEstudio
    {
        public int IdCatalogoEstudio { get; set; }
        public string Codigo { get; set; }
        public string NombreEstudio { get; set; }
        public string Categoria { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }

        // Propiedades de navegación
        public List<SolicitudEstudioDetalle> SolicitudEstudioDetalles { get; set; } = new();
    }
}
