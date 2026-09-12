using Core.Interface.Repositories;
using Domain.Models;
using MediatR;

namespace Core.feature.Turnos.Queries
{
    public class GetTurnosQuery : IRequest<List<Turno>>
    {
        public int TotalRegistros { get; set; }
    }

    public class GetTurnosQueryHandler
        : IRequestHandler<GetTurnosQuery, List<Turno>>
    {
        private readonly IGenericRepository<Turno> _repository;

        public GetTurnosQueryHandler(
            IGenericRepository<Turno> repository)
        {
            _repository = repository;
        }

        public async Task<List<Turno>> Handle(
            GetTurnosQuery request,
            CancellationToken cancellationToken)
        {
            var turnos = await _repository.GetAllAsync();

            return turnos.ToList();
        }
    }
}