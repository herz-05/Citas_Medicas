using Generics.Interfaces;
using Domain.Models;
using MediatR;

namespace Core.feature.HorarioMedico.Queries
{
    public class GetHorarioMedicoQuery
        : IRequest<List<HorariosMedico>>
    {
        public int TotalRegistros { get; set; }
    }

    public class GetHorarioMedicoQueryHandler
        : IRequestHandler<GetHorarioMedicoQuery, List<HorariosMedico>>
    {
        private readonly IGenericRepository<HorariosMedico> _repository;

        public GetHorarioMedicoQueryHandler(
            IGenericRepository<HorariosMedico> repository)
        {
            _repository = repository;
        }

        public async Task<List<HorariosMedico>> Handle(
            GetHorarioMedicoQuery request,
            CancellationToken cancellationToken)
        {
            var horarios = await _repository.GetAllAsync();

            if (request.TotalRegistros > 0)
            {
                return horarios
                    .Take(request.TotalRegistros)
                    .ToList();
            }

            return horarios.ToList();
        }
    }
}