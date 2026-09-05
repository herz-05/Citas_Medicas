namespace Domain.Models
{
    public class Medico
    {
        public int IdMedico { get; set; }

        public int IdUsuario { get; set; }

        public string NumeroJVPM { get; set; } = string.Empty;

        public bool Estado { get; set; }

        public DateTime FechaRegistro { get; set; }

        public DateTime? FechaSalida { get; set; }
    }
}