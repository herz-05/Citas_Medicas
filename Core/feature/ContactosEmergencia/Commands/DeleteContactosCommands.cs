using Core.Interface.Repositories;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.ContactosEmergencia.Commands
{
    public class DeleteContactosCommands: IRequest<bool>
    {
        public int IdContacto { get; set; }
    }

    public class DeleteContactosCommandHandler
       : IRequestHandler<DeleteContactosCommands, bool>
    {
        private readonly IGenericRepository<ContactoEmergencia> _repository;

        public DeleteContactosCommandHandler(
            IGenericRepository<ContactoEmergencia> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            DeleteContactosCommands request,
            CancellationToken cancellationToken)
        {
            var contacto = await _repository.GetByIdAsync(
                request.IdContacto);

            if (contacto == null)
            {

                return false;
            }

            await _repository.AddAsync(contacto);

            return true;
        }
    }
}
