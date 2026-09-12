using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Config
{
    public class HorarioMedicoConfigType
        : IEntityTypeConfiguration<HorariosMedico>
    {
        public void Configure(
            EntityTypeBuilder<HorariosMedico> builder)
        {
            builder.HasKey(x => x.IdHorario);
        }
    }
}