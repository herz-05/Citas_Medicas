using Core.feature.Citas.Commands;
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
    public class AddContactosCommands: IRequest<bool>
    {
        public int IdContacto { get; set; }
        public int IdPaciente { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string? Parentesco { get; set; }
        public string Telefono { get; set; } = string.Empty;
        public string? Correo { get; set; }
        public string? Direccion { get; set; }
    }

    public class AddContactosCommandHandler
      : IRequestHandler<AddContactosCommands, bool>
    {
        private readonly IGenericRepository<ContactoEmergencia> _repository;

        public AddContactosCommandHandler(
            IGenericRepository<ContactoEmergencia> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            AddContactosCommands request,
            CancellationToken cancellationToken)
        {
            var contactos = new ContactoEmergencia
            {
                IdPaciente = request.IdPaciente,
                NombreCompleto = request.NombreCompleto,
                Parentesco = request.Parentesco,
                Telefono = request.Telefono,
                Correo = request.Correo,
                Direccion = request.Direccion
            };

            await _repository.AddAsync(contactos);

            return true;
        }
    }
}
