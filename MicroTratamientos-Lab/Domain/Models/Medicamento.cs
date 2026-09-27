using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Medicamento
    {
        public int IdMedicamento { get; set; }
        public string NombreComercial { get; set; }
        public string NombreGenerico { get; set; }
        public string Presentacion { get; set; }
        public string Concentracion { get; set; }
        public string ViaAdministracion { get; set; }
        public bool Activo { get; set; }

        // Propiedades de navegación
        public List<PrescripcionDetalle> PrescripcionDetalles { get; set; } = new()
    }
}
