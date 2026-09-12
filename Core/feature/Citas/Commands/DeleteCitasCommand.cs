using Generics.Interfaces;
using Domain.Models;
using MediatR;

namespace Core.feature.Citas.Commands
{
    public class DeleteCitasCommand : IRequest<bool>
    {
        public int IdCita { get; set; }
    }

    public class DeleteCitasCommandHandler
        : IRequestHandler<DeleteCitasCommand, bool>
    {
        private readonly IGenericRepository<Cita> _repository;

        public DeleteCitasCommandHandler(
            IGenericRepository<Cita> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            DeleteCitasCommand request,
            CancellationToken cancellationToken)
        {
            var cita = await _repository.GetByIdAsync(request.IdCita);

            if (cita == null)
            {
                return false;
            }

            await _repository.DeleteAsync(cita);

            return true;
        }
    }
}