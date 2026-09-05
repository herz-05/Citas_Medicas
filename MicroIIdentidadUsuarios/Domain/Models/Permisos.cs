namespace Domain.Models
{
    public class Permisos
    {
        public int IdPermiso { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        public bool Estado { get; set; }

    }
}