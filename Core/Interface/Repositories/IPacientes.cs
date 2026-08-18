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
        Task<List<Pacientes>> GetPacientesAsync();
    }
}
