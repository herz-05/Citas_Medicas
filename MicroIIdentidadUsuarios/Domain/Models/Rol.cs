namespace Domain.Models
{
    public class Rol
    {
        public int IdRol { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        public bool Estado { get; set; }

        public DateTime FechaRegistro { get; set; }
    }
}