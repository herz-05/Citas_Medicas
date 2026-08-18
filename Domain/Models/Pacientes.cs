using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Pacientes
    {
        public int IdPaciente { get; set; }
        public string? Nombres { get; set; } 
        public string? Apellidos { get; set; } 
        public DateTime FechaNacimiento { get; set; }
        public string? Sexo { get; set; } 
        public string? DUI { get; set; } 
        public string? Telefono { get; set; }
        public string? Correo { get; set; } 
        public string? Direccion { get; set; } 
        public DateTime FechaRegistro { get; set; }
    }
}
