using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
