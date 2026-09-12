using Generics.Interfaces;
using Domain.Models;
using MediatR;

namespace Core.feature.EstadosCitas.Queires
{
    public class GetEstadosCitasQuery : IRequest<List<EstadoCitas>>
    {
        public int TotalRegistros { get; set; }
    }

    public class GetEstadosCitasQueryHandler
        : IRequestHandler<GetEstadosCitasQuery, List<EstadoCitas>>
    {
        private readonly IGenericRepository<EstadoCitas> _repository;

        public GetEstadosCitasQueryHandler(
            IGenericRepository<EstadoCitas> repository)
        {
            _repository = repository;
        }

        public async Task<List<EstadoCitas>> Handle(
            GetEstadosCitasQuery request,
            CancellationToken cancellationToken)
        {
            var estados = await _repository.GetAllAsync();

            if (request.TotalRegistros > 0)
            {
                return estados
                    .Take(request.TotalRegistros)
                    .ToList();
            }

            return estados.ToList();
        }
    }
}