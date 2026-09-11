using Core.Interface.Repositories;
using Domain.Models;
using MediatR;

namespace Core.feature.Citas.Queries
{
    public class GetCitasByIdQuery : IRequest<Cita?>
    {
        public int IdCitas { get; set; }
    }

    public class GetCitasByIdQueryHandler
        : IRequestHandler<GetCitasByIdQuery, Cita?>
    {
        private readonly IGenericRepository<Cita> _repository;

        public GetCitasByIdQueryHandler(
            IGenericRepository<Cita> repository)
        {
            _repository = repository;
        }

        public async Task<Cita?> Handle(
            GetCitasByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(request.IdCitas);
        }
    }
}