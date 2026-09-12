using Core.Interface.Repositories;
using Domain.Models;
using MediatR;

namespace Core.feature.Turnos.Commands
{
    public class DeleteTurnosCommand : IRequest<bool>
    {
        public int IdTurno { get; set; }
    }

    public class DeleteTurnosCommandHandler
        : IRequestHandler<DeleteTurnosCommand, bool>
    {
        private readonly IGenericRepository<Turno> _repository;

        public DeleteTurnosCommandHandler(
            IGenericRepository<Turno> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            DeleteTurnosCommand request,
            CancellationToken cancellationToken)
        {
            var turno = await _repository.GetByIdAsync(
                request.IdTurno);

            if (turno == null)
            {
                return false;
            }

            await _repository.DeleteAsync(turno);

            return true;
        }
    }
}