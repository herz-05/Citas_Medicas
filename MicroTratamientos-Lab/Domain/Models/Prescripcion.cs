using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Prescripcion
    {
        public int IdPrescripcion { get; set; }
        public int IdPaciente { get; set; }
        public int IdMedico { get; set; }
        public int? IdCita { get; set; }
        public DateTime FechaEmision { get; set; }
        public string Observaciones { get; set; }
        public string Estado { get; set; }

        // Propiedades de navegación
        public List<PrescripcionDetalle> Detalles { get; set; } = new();
    }
}
