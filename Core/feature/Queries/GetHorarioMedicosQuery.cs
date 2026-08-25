using Core.Interface.Repositories;
using Domain.Models;
using MediatR;

namespace Core.feature.Queries
{
    public class GetHorarioMedicoQuery
        : IRequest<List<HorarioMedico>>
    {
        public int TotalRegistros { get; set; }
    }

    public class GetHorarioMedicoQueryHandler
        : IRequestHandler<
            GetHorarioMedicoQuery,
            List<HorarioMedico>>
    {
        private readonly IHorariosMedicos _repository;

        public GetHorarioMedicoQueryHandler(
            IHorariosMedicos repository)
        {
            _repository = repository;
        }

        public async Task<List<HorarioMedico>> Handle(
            GetHorarioMedicoQuery request,
            CancellationToken cancellationToken)
        {
            return request.TotalRegistros > 0
                ? await _repository.GetHorariosMedicosAsync(
                    request.TotalRegistros)
                : await _repository.GetHorariosMedicosAsync();
        }
    }
}