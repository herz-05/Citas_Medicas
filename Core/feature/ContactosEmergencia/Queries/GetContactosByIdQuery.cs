using Generics.Interfaces;
using Domain.Models;
using MediatR;

namespace Core.feature.ContactosEmergencia.Queries
{
    public class GetContactosByIdQuery
        : IRequest<ContactoEmergencia?>
    {
        public int IdContacto { get; set; }
    }

    public class GetContactosByIdQueryHandler
        : IRequestHandler<GetContactosByIdQuery, ContactoEmergencia?>
    {
        private readonly IGenericRepository<ContactoEmergencia> _repository;

        public GetContactosByIdQueryHandler(
            IGenericRepository<ContactoEmergencia> repository)
        {
            _repository = repository;
        }

        public async Task<ContactoEmergencia?> Handle(
            GetContactosByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(
                request.IdContacto);
        }
    }
}