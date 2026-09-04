using Core.Interface.Repositories;
using Domain.Models;
using MediatR;

namespace Core.feature.HorariosMedicos.Queries
{
    public class GetHorarioMedicoQuery : IRequest<List<HorarioMedico>>
    {
        public int TotalRegistros { get; set; }
    }

    public class GetHorarioMedicoQueryHandler
        : IRequestHandler<GetHorarioMedicoQuery, List<HorarioMedico>>
    {
        private readonly IGenericRepository<HorarioMedico> _repository;

        public GetHorarioMedicoQueryHandler(
            IGenericRepository<HorarioMedico> repository)
        {
            _repository = repository;
        }

        public async Task<List<HorarioMedico>> Handle(
            GetHorarioMedicoQuery request,
            CancellationToken cancellationToken)
        {
            var horarios = await _repository.GetAllAsync();

            if (request.TotalRegistros > 0)
                return horarios.Take(request.TotalRegistros).ToList();

            return horarios;
        }
    }
}