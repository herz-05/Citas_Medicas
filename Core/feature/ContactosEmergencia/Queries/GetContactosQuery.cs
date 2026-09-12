using Core.Interface.Repositories;
using Domain.Models;
using MediatR;

namespace Core.feature.ContactosEmergencia.Queries
{
    public class GetContactosQuery
        : IRequest<List<ContactoEmergencia>>
    {
        public int TotalRegistros { get; set; }
    }

    public class GetContactosQueryHandler
        : IRequestHandler<GetContactosQuery, List<ContactoEmergencia>>
    {
        private readonly IGenericRepository<ContactoEmergencia> _repository;

        public GetContactosQueryHandler(
            IGenericRepository<ContactoEmergencia> repository)
        {
            _repository = repository;
        }

        public async Task<List<ContactoEmergencia>> Handle(
            GetContactosQuery request,
            CancellationToken cancellationToken)
        {
            var contactos = await _repository.GetAllAsync();

            return contactos.ToList();
        }
    }
}