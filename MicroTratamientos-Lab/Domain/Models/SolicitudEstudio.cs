using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class SolicitudEstudio
    {
        public int IdSolicitudEstudio { get; set; }
        public int IdPaciente { get; set; }
        public int IdMedicoSolicitante { get; set; }
        public int? IdCita { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public string Prioridad { get; set; }
        public string Estado { get; set; }
        public string DiagnosticoPresuntivo { get; set; }

        // Propiedades de navegación
        public List<SolicitudEstudioDetalle> Detalles { get; set; } = new();
    }
}
