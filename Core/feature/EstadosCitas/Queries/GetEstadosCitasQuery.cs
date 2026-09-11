using Domain.Models;
using MediatR;

namespace Core.feature.EstadosCitas.Queires
{
    public class GetEstadosCitasQuery : IRequest<List<EstadoCitas>>
    {
        public int TotalRegistros { get; set; }
    }
}