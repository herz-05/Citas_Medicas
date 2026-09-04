using Core.Interface.Repositories;
using Domain.Models;
using MediatR;

namespace Core.feature.Consultorios.Queries
{
    public class GetConsultorioQuery : IRequest<List<HorarioMedico>>
    {
        public int TotalRegistros { get; set; }
    }

    public class GetConsultorioQueryHandler
        : IRequestHandler<GetConsultorioQuery, List<HorarioMedico>>
    {
        private readonly IGenericRepository<HorarioMedico> _repository;

        public GetConsultorioQueryHandler(
            IGenericRepository<HorarioMedico> repository)
        {
            _repository = repository;
        }

        public async Task<List<HorarioMedico>> Handle(
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