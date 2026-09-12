using Generics.Interfaces;
using Domain.Models;
using MediatR;

namespace Core.feature.EstadosCitas.Commands
{
    public class UpdateEstadoCitasCommand : IRequest<bool>
    {
        public int IdEstadoCita { get; set; }
        public string NombreEstado { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
    }

    public class UpdateEstadoCitasCommandHandler
        : IRequestHandler<UpdateEstadoCitasCommand, bool>
    {
        private readonly IGenericRepository<EstadoCitas> _repository;

        public UpdateEstadoCitasCommandHandler(
            IGenericRepository<EstadoCitas> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            UpdateEstadoCitasCommand request,
            CancellationToken cancellationToken)
        {
            var estado = await _repository.GetByIdAsync(
                request.IdEstadoCita);

            if (estado == null)
            {
                return false;
            }

            estado.NombreEstado = request.NombreEstado;
            estado.Descripcion = request.Descripcion;

            await _repository.UpdateAsync(estado);

            return true;
        }
    }
}