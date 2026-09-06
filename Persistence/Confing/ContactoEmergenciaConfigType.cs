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
    public class ContactoEmergenciaConfigType : IEntityTypeConfiguration<ContactoEmergencia>
    {
        public void Configure(EntityTypeBuilder<ContactoEmergencia> builder)
        {
            throw new NotImplementedException();
        }
    }
}
