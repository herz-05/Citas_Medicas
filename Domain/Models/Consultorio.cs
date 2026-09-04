using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class HorarioMedico
    {
        [Key]
        public int IdConsultorio { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string? NumeroConsultorio { get; set; }

        public string? Piso { get; set; }

        public string? Ubicacion { get; set; }
        public string? Descripcion { get; set; }

        public bool Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}