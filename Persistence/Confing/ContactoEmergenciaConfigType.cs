using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Config
{
    public class ContactoEmergenciaConfigType
        : IEntityTypeConfiguration<ContactoEmergencia>
    {
        public void Configure(
            EntityTypeBuilder<ContactoEmergencia> builder)
        {
            builder.HasKey(x => x.IdContacto);
        }
    }
}