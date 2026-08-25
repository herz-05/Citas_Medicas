using Domain.Models;

namespace Core.Interface.Repositories
{
    public interface IConsultorios
    {
        Task<List<Consultorio>> GetConsultoriosAsync();

        Task<List<Consultorio>> GetConsultoriosAsync(int totalRegistros);

        Task AddConsultorio(Consultorio consultorio);
    }
}