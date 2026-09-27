using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class ResultadoEstudio
    {
        public int IdResultadoEstudio { get; set; }
        public int IdSolicitudEstudioDetalle { get; set; }
        public int IdTecnicoOMedico { get; set; }
        public DateTime FechaResultado { get; set; }
        public string Hallazgos { get; set; }
        public string Conclusion { get; set; }
        public string ValoresReferencia { get; set; }

        // Propiedades de navegación
        public SolicitudEstudioDetalle SolicitudEstudioDetalle { get; set; }
        public List<ArchivoClinico> ArchivosClinicos { get; set; } = new();
    }
}
