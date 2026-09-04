using Core.Interface.Repositories;
using Domain.Models;
using MediatR;

namespace Core.feature.Consultorios.Queries
{
    public class GetConsultorioQuery : IRequest<List<Consultorio>>
    {
        public int TotalRegistros { get; set; }
    }

    public class GetConsultorioQueryHandler
        : IRequestHandler<GetConsultorioQuery, List<Consultorio>>
    {
        private readonly IGenericRepository<Consultorio> _repository;

        public GetConsultorioQueryHandler(
            IGenericRepository<Consultorio> repository)
        {
            _repository = repository;
        }

        public async Task<List<Consultorio>> Handle(
            GetConsultorioQuery request,
            CancellationToken cancellationToken)
        {
            var consultorios = await _repository.GetAllAsync();

            if (request.TotalRegistros > 0)
            {
                return consultorios
                    .Take(request.TotalRegistros)
                    .ToList();
            }

            return consultorios;
        }
    }
}