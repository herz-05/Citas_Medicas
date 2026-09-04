using Core.Interface.Repositories;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.Paciente.Commands
{
    public class UpdatePacienteCommand: IRequest<bool>
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

    public class UpdatePacienteCommandHandler
       : IRequestHandler<UpdatePacienteCommand, bool>
    {
        private readonly IGenericRepository<Pacientes> _repository;

        public UpdatePacienteCommandHandler(
            IGenericRepository<Pacientes> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            UpdatePacienteCommand request,
            CancellationToken cancellationToken)
        {
            var pacientes = await _repository.GetByIdAsync(
                request.IdPaciente);

            if (pacientes == null)
            {
                return false;
            }

            pacientes.Nombres = request.Nombres ?? string.Empty;
            pacientes.Apellidos = request.Apellidos ?? string.Empty;
            pacientes.FechaNacimiento = request.FechaNacimiento;
            pacientes.Sexo = request.Sexo;
            pacientes.DUI = request.DUI;
            pacientes.Telefono = request.Telefono;
            pacientes.Correo = request.Correo;
            pacientes.Direccion = request.Direccion;
            pacientes.FechaRegistro = request.FechaRegistro;

            await _repository.UpdateAsync(pacientes);

            return true;
        }
    }

}
