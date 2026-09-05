namespace Domain.Models
{
    public class SesionUsuario
    {
        public int IdSesion { get; set; }

        public int IdUsuario { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaExpiracion { get; set; }

        public bool Estado { get; set; }
    }
}
