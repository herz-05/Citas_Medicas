using Core.Interface.Repositories;
using Domain.Models;
using MediatR;

namespace Core.feature.Citas.Queries
{
    public class GetCitasQuery : IRequest<List<Cita>>
    {
        public int TotalRegistros { get; set; }
    }

    public class GetCitasQueryHandler
        : IRequestHandler<GetCitasQuery, List<Cita>>
    {
        private readonly IGenericRepository<Cita> _repository;

        public GetCitasQueryHandler(
            IGenericRepository<Cita> repository)
        {
            _repository = repository;
        }

        public async Task<List<Cita>> Handle(
            GetCitasQuery request,
            CancellationToken cancellationToken)
        {
            var citas = await _repository.GetAllAsync();

            return citas.ToList();
        }
    }
}