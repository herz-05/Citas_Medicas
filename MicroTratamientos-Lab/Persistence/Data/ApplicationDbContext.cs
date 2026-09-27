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
    public class ApplicationDbContext: DbContext
    {
        public DbSet<ArchivoClinico> ArchivoClinicos { get; set; }
        public DbSet<CatalogoEstudio> CatalogoEstudios { get; set; }
        public DbSet<Medicamento> Medicamentos { get; set; }
        public DbSet<Prescripcion> Prescripciones { get; set; }
        public DbSet<PrescripcionDetalle> PrescripcionDetalles { get; set; }
        public DbSet<ResultadoEstudio> ResultadoEstudios { get; set; }
        public DbSet<SolicitudEstudio> SolicitudEstudios { get; set; }
        public DbSet<SolicitudEstudioDetalle> SolicitudEstudioDetalles { get; set; }

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
