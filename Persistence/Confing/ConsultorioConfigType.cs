using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Config
{
    public class ConsultorioConfigType
        : IEntityTypeConfiguration<Consultorio>
    {
        public void Configure(
            EntityTypeBuilder<Consultorio> builder)
        {
            builder.HasKey(x => x.IdConsultorio);
        }
    }
}