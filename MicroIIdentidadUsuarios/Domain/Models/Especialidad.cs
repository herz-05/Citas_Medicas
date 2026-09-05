namespace Domain.Models
{
    public class Especialidad
    {
        public int IdEspecialidad { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        public bool Estado { get; set; }

        public DateTime FechaRegistro { get; set; }
    }
}