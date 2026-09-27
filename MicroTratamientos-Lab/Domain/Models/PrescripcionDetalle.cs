using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class PrescripcionDetalle
    {
        public int IdPrescripcionDetalle { get; set; }
        public int IdPrescripcion { get; set; }
        public int IdMedicamento { get; set; }
        public decimal Dosis { get; set; }
        public string UnidadMedida { get; set; }
        public string Frecuencia { get; set; }
        public int DuracionDias { get; set; }
        public string IndicacionesEspeciales { get; set; }

        // Propiedades de navegación
        public Prescripcion Prescripcion { get; set; }
        public Medicamento Medicamento { get; set; }
    }
}
