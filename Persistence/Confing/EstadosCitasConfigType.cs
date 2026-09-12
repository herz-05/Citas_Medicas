using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Config
{
    public class EstadosCitasConfigType
        : IEntityTypeConfiguration<EstadoCitas>
    {
        public void Configure(
            EntityTypeBuilder<EstadoCitas> builder)
        {
            builder.HasKey(x => x.IdEstadoCita);
        }
    }
}