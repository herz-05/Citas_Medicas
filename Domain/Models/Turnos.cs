using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Turnos
    {
        public int IdTurno { get; set; }
        public int IdHorario { get; set; }
        public int IdPaciente { get; set; }
        public int NumeroTurno { get; set; }
        public DateOnly FechaTurno { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
