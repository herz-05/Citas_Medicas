using Generics.Interfaces;
using Domain.Models;
using MediatR;

namespace Core.feature.EstadosCitas.Queires
{
    public class GetEstadosCitasByIdQuery
        : IRequest<EstadoCitas?>
    {
        public int IdEstadoCitas { get; set; }
    }

    public class GetEstadosCitasByIdQueryHandler
        : IRequestHandler<GetEstadosCitasByIdQuery, EstadoCitas?>
    {
        private readonly IGenericRepository<EstadoCitas> _repository;

        public GetEstadosCitasByIdQueryHandler(
            IGenericRepository<EstadoCitas> repository)
        {
            _repository = repository;
        }

        public async Task<EstadoCitas?> Handle(
            GetEstadosCitasByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(
                request.IdEstadoCitas);
        }
    }
}