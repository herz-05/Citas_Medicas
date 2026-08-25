using Core.Interface.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Persistence.Repositories
{
    public class ConsultoriosRepository : IConsultorios
    {
        private readonly ApplicationDbContext _context;

        public ConsultoriosRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Consultorio>> GetConsultoriosAsync()
        {
            return await _context.Consultorios.ToListAsync();
        }
    }
}