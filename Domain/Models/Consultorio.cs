using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Consultorio
    {
        public int IdConsultorio { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string? NumeroConsultorio { get; set; }

        public string? Piso { get; set; }

        public string? Ubicacion { get; set; }

        public bool Estado { get; set; }
    }
}
