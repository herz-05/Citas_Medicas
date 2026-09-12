using Generics.Interfaces;
using Domain.Models;
using MediatR;

namespace Core.feature.EstadosCitas.Commands
{
    public class DeleteEstadoCitasCommand : IRequest<bool>
    {
        public int IdEstadoCita { get; set; }
    }

    public class DeleteEstadoCitasCommandHandler
        : IRequestHandler<DeleteEstadoCitasCommand, bool>
    {
        private readonly IGenericRepository<EstadoCitas> _repository;

        public DeleteEstadoCitasCommandHandler(
            IGenericRepository<EstadoCitas> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            DeleteEstadoCitasCommand request,
            CancellationToken cancellationToken)
        {
            var estado = await _repository.GetByIdAsync(
                request.IdEstadoCita);

            if (estado == null)
            {
                return false;
            }

            await _repository.DeleteAsync(estado);

            return true;
        }
    }
}