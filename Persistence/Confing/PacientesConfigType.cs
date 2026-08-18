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
    internal class PacientesConfigType : IEntityTypeConfiguration<Pacientes>
    {
        public void Configure(EntityTypeBuilder<Pacientes> builder)
        {
            builder.HasKey(x => x.IdPaciente);
        }
    }
}
