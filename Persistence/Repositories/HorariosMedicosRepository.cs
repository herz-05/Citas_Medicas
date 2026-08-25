using Core.Interface.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Persistence.Repositories
{
    public class HorariosMedicosRepository : IHorariosMedicos
    {
        private readonly ApplicationDbContext _context;

        public HorariosMedicosRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<HorarioMedico>> GetHorariosMedicosAsync()
        {
            return await _context.HorariosMedicos.ToListAsync();
        }

        public async Task<List<HorarioMedico>> GetHorariosMedicosAsync(
            int totalRegistros)
        {
            return await _context.HorariosMedicos
                .Take(totalRegistros)
                .ToListAsync();
        }

        public async Task AddHorarioMedico(
            HorarioMedico horario)
        {
            await _context.HorariosMedicos.AddAsync(horario);

            await _context.SaveChangesAsync();
        }
    }
}