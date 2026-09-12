using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Config
{
    public class TurnosConfigType
        : IEntityTypeConfiguration<Turno>
    {
        public void Configure(
            EntityTypeBuilder<Turno> builder)
        {
            builder.HasKey(x => x.IdTurno);
        }
    }
}