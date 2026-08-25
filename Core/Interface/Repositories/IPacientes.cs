using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Repositories
{
    public interface IPacientes
    {
        Task AddPaciente(Pacientes pacientes);
        Task<List<Pacientes>> GetPacientesAsync();

        Task<List<Pacientes>> GetPacientesAsync(int totalRegistros = 1000);
    }
}
