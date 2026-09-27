using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Confing
{
    public class PrescripDetalleConfigType: IEntityTypeConfiguration<PrescripcionDetalle>
    {
        public void Configure(EntityTypeBuilder<PrescripcionDetalle> builder)
        {
            builder.HasKey(x => x.IdPrescripcionDetalle);
        }
    }
}
