using Core.Interface.Repositories;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.Paciente.Queries
{
    public class GetPacienteByIdQuery: IRequest<Pacientes>
    {
        public int IdPaciente { get; set; }
    }

    public class GetPacienteByIdQueryHandler
       : IRequestHandler<GetPacienteByIdQuery, Pacientes>
    {
        private readonly IGenericRepository<Pacientes> _repository;

        public GetPacienteByIdQueryHandler(
            IGenericRepository<Pacientes> repository)
        {
            _repository = repository;
        }

        public async Task<Pacientes> Handle(
            GetPacienteByIdQuery request,
            CancellationToken cancellationToken)
        {

            return await _repository.GetByIdAsync(
               request.IdPaciente);
        }
    }
}
