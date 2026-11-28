
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Persistence.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Pacientes> Pacientes { get; set; }
        public DbSet<Consultorio> Consultorios { get; set; }
        public DbSet<HorariosMedico> HorariosMedicos { get; set; }
        public DbSet<Cita> Citas { get; set; }
        public DbSet<ContactoEmergencia> ContactosEmergencias { get; set; }
        public DbSet<EstadoCitas> EstadosCitas { get; set; }
        public DbSet<Turno> Turnos { get; set; }

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(
                Assembly.GetExecutingAssembly());

            // Índice para búsquedas de citas por fecha y médico.
            modelBuilder.Entity<Cita>()
                .HasIndex(c => new
                {
                    c.FechaCita,
                    c.IdMedico
                })
                .HasDatabaseName("IX_Citas_Fecha_Medico");

            // Índice para búsquedas de citas por fecha y consultorio.
            modelBuilder.Entity<Cita>()
                .HasIndex(c => new
                {
                    c.FechaCita,
                    c.IdConsultorio
                })
                .HasDatabaseName("IX_Citas_Fecha_Consultorio");

            // Índice para búsquedas de horarios por médico y día.
            modelBuilder.Entity<HorariosMedico>()
                .HasIndex(h => new
                {
                    h.IdMedico,
                    h.DiaSemana,
                    h.Estado
                })
                .HasDatabaseName("IX_HorariosMedicos_Medico_Dia_Estado");
        }
    }
}
