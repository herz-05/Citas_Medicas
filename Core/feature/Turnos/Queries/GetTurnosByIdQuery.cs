using Generics.Interfaces;
using Domain.Models;
using MediatR;

namespace Core.feature.Turnos.Queries
{
    public class GetTurnosByIdQuery : IRequest<Turno?>
    {
        public int IdTurno { get; set; }
    }

    public class GetTurnosByIdQueryHandler
        : IRequestHandler<GetTurnosByIdQuery, Turno?>
    {
        private readonly IGenericRepository<Turno> _repository;

        public GetTurnosByIdQueryHandler(
            IGenericRepository<Turno> repository)
        {
            _repository = repository;
        }

        public async Task<Turno?> Handle(
            GetTurnosByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(
                request.IdTurno);
        }
    }
}