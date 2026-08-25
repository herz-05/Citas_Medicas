using Core.Interface.Repositories;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.Queries
{
    public class GetPacienteQuery : IRequest<List<Pacientes>>
    {
        public int TotalRegistros { get; set; }
    }

    public class GetPacienteQueryHandler : IRequestHandler<GetPacienteQuery, List<Pacientes>>
    {
        private readonly IPacientes _pacientesRepository;
        public GetPacienteQueryHandler(IPacientes pacientesRepository)
        {
            _pacientesRepository = pacientesRepository;
        }
        public async Task<List<Pacientes>> Handle(GetPacienteQuery request, CancellationToken cancellationToken)
        {
            return request.TotalRegistros > 0 ? await _pacientesRepository.GetPacientesAsync(request.TotalRegistros) : await _pacientesRepository.GetPacientesAsync();
        }
    }
}
