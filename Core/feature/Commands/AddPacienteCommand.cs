using Core.Interface.Repositories;
using Domain.Models;
using MediatR;

namespace Core.feature.Commands
{
    public class AddPacienteCommand : IRequest<bool>
    {
        public int IdPaciente { get; set; }
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string? Sexo { get; set; }
        public string? DUI { get; set; }
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
        public string? Direccion { get; set; }
        public DateTime FechaRegistro { get; set; }
    }

    public class AddPacienteCommandHandler
        : IRequestHandler<AddPacienteCommand, bool>
    {
        private readonly IGenericRepository<Pacientes> _repository;

        public AddPacienteCommandHandler(
            IGenericRepository<Pacientes> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            AddPacienteCommand request,
            CancellationToken cancellationToken)
        {
            var paciente = new Pacientes
            {
                IdPaciente = request.IdPaciente,
                Nombres = request.Nombres,
                Apellidos = request.Apellidos,
                FechaNacimiento = request.FechaNacimiento,
                Sexo = request.Sexo,
                DUI = request.DUI,
                Telefono = request.Telefono,
                Correo = request.Correo,
                Direccion = request.Direccion,
                FechaRegistro = request.FechaRegistro
            };

            await _repository.AddAsync(paciente);

            return true;
        }
    }
}