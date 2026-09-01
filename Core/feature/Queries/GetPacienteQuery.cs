using Core.Interface.Repositories;
using Domain.Models;
using MediatR;

namespace Core.feature.Queries
{
    public class GetPacienteQuery : IRequest<List<Pacientes>>
    {
        public int TotalRegistros { get; set; }
    }

    public class GetPacienteQueryHandler
        : IRequestHandler<GetPacienteQuery, List<Pacientes>>
    {
        private readonly IGenericRepository<Pacientes> _repository;

        public GetPacienteQueryHandler(
            IGenericRepository<Pacientes> repository)
        {
            _repository = repository;
        }

        public async Task<List<Pacientes>> Handle(
            GetPacienteQuery request,
            CancellationToken cancellationToken)
        {
            var pacientes = await _repository.GetAllAsync();

            if (request.TotalRegistros > 0)
            {
                return pacientes
                    .Take(request.TotalRegistros)
                    .ToList();
            }

            return pacientes;
        }
    }
}