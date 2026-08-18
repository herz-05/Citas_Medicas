using Core.Interface.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class PacientesRepository : IPacientes
    {
        private readonly ApplicationDbContext _context;

        public PacientesRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<List<Pacientes>> GetPacientesAsync()
        {
            return await _context.Pacientes.ToListAsync();
        }
    }
}
