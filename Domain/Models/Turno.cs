
namespace Domain.Models
{
    public class Turno
    {
        public int IdTurno { get; set; }

        // Relación lógica con la cita.
        public int? IdCita { get; set; }

        public int IdHorario { get; set; }
        public int IdPaciente { get; set; }

        public int NumeroTurno { get; set; }

        public DateOnly FechaTurno { get; set; }

        // En espera, Llamado, Atendiendo,
        // Atendido, Ausente o Cancelado.
        public string Estado { get; set; } = "En espera";

        public DateTime FechaRegistro { get; set; }
    }
}
