using Domain.Models;
using MediatR;

namespace Core.feature.EstadosCitas.Queires
{
    public class GetEstadosCitasByIdQuery : IRequest<EstadoCitas?>
    {
        public int IdEstadoCitas { get; set; }
    }
}