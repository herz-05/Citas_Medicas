using Domain.Models;

namespace Core.Interface.Repositories
{
    public interface IHorariosMedicos
    {
        Task<List<HorarioMedico>> GetHorariosMedicosAsync();

        Task<List<HorarioMedico>> GetHorariosMedicosAsync(int totalRegistros);

        Task AddHorarioMedico(HorarioMedico horario);
    }
}