using Domain.Models;

namespace Core.Interface.Repositories
{
    public interface IConsultorios
    {
        Task<List<Consultorio>> GetConsultoriosAsync();
    }
}