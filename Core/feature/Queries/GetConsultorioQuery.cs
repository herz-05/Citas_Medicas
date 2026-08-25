using Core.Interface.Repositories;
using Domain.Models;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.feature.Queries
{
    public class GetConsultorioQuery : IRequest<List<Consultorio>>
    {
        public int TotalRegistros { get; set; }
    }

    public class GetConsultorioQueryHandler
        : IRequestHandler<GetConsultorioQuery, List<Consultorio>>
    {
        private readonly IConsultorios _consultoriosRepository;

        public GetConsultorioQueryHandler(
            IConsultorios consultoriosRepository)
        {
            _consultoriosRepository = consultoriosRepository;
        }

        public async Task<List<Consultorio>> Handle(
            GetConsultorioQuery request,
            CancellationToken cancellationToken)
        {
            return request.TotalRegistros > 0
                ? await _consultoriosRepository.GetConsultoriosAsync(request.TotalRegistros)
                : await _consultoriosRepository.GetConsultoriosAsync();
        }
    }
}