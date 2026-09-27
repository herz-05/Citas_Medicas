using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class ArchivoClinico
    {
        public int IdArchivoClinico { get; set; }
        public int? IdResultadoEstudio { get; set; }
        public int? IdPaciente { get; set; }
        public string NombreArchivo { get; set; }
        public string Extension { get; set; }
        public string TipoMime { get; set; }
        public string RutaAlmacenamiento { get; set; }
        public long TamanoBytes { get; set; }
        public DateTime FechaSubida { get; set; }

        // Propiedades de navegación
        public ResultadoEstudio ResultadoEstudio { get; set; }
    }
}
