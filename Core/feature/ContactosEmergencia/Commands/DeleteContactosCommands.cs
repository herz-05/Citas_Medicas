using Generics.Interfaces;
using Domain.Models;
using MediatR;

namespace Core.feature.ContactosEmergencia.Commands
{
    public class DeleteContactosCommand : IRequest<bool>
    {
        public int IdContacto { get; set; }
    }

    public class DeleteContactosCommandHandler
        : IRequestHandler<DeleteContactosCommand, bool>
    {
        private readonly IGenericRepository<ContactoEmergencia> _repository;

        public DeleteContactosCommandHandler(
            IGenericRepository<ContactoEmergencia> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            DeleteContactosCommand request,
            CancellationToken cancellationToken)
        {
            var contacto = await _repository.GetByIdAsync(
                request.IdContacto);

            if (contacto == null)
            {
                return false;
            }

            await _repository.DeleteAsync(contacto);

            return true;
        }
    }
}