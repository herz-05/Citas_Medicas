using Generics.Interfaces;
using Domain.Models;
using MediatR;

namespace Core.feature.ContactosEmergencia.Commands
{
    public class UpdateContactosCommand : IRequest<bool>
    {
        public int IdContacto { get; set; }
        public int IdPaciente { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string? Parentesco { get; set; }
        public string Telefono { get; set; } = string.Empty;
        public string? Correo { get; set; }
        public string? Direccion { get; set; }
    }

    public class UpdateContactosCommandHandler
        : IRequestHandler<UpdateContactosCommand, bool>
    {
        private readonly IGenericRepository<ContactoEmergencia> _repository;

        public UpdateContactosCommandHandler(
            IGenericRepository<ContactoEmergencia> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            UpdateContactosCommand request,
            CancellationToken cancellationToken)
        {
            var contacto = await _repository.GetByIdAsync(
                request.IdContacto);

            if (contacto == null)
            {
                return false;
            }

            contacto.IdPaciente = request.IdPaciente;
            contacto.NombreCompleto = request.NombreCompleto;
            contacto.Parentesco = request.Parentesco;
            contacto.Telefono = request.Telefono;
            contacto.Correo = request.Correo;
            contacto.Direccion = request.Direccion;

            await _repository.UpdateAsync(contacto);

            return true;
        }
    }
}