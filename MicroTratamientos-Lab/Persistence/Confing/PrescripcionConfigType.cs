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
    public class PrescripcionConfigType: IEntityTypeConfiguration<Prescripcion>
    {
        public void Configure(EntityTypeBuilder<Prescripcion> builder)
        {
            builder.HasKey(x => x.IdPrescripcion);
        }
    }
}
