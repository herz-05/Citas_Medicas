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

        public async Task AddPaciente(Pacientes pacientes)
        {
            _context.Add(pacientes);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Pacientes>> GetPacientesAsync()
        {
            return await _context.Pacientes.Take(1000).ToListAsync();
        }

        public async Task<List<Pacientes>> GetPacientesAsync(int TotalRegistros = 1000)
        {
            return await _context.Pacientes.Take(TotalRegistros).ToListAsync();
        }
    }
}
