using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class ContactosEmergencia
    {
        public int IdContacto { get; set; }
        public int IdPaciente { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string? Parentesco { get; set; }
        public string Telefono { get; set; } = string.Empty;
        public string? Correo { get; set; }
        public string? Direccion { get; set; }
    }
}
