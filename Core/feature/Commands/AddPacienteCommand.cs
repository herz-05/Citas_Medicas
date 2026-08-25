using Core.Interface.Repositories;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.Commands
{
    public class AddPacienteCommand: IRequest<bool>
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


    public class AddPacienteCommandHandler : IRequestHandler<AddPacienteCommand, bool>
    {
        IPacientes _pacientesRepository;
        public AddPacienteCommandHandler(IPacientes pacientesRepository)
        {
            _pacientesRepository = pacientesRepository;
            
        }
        public async Task<bool> Handle(AddPacienteCommand request, CancellationToken cancellationToken)
        {
            Pacientes pacientes = new Pacientes();
            pacientes.IdPaciente = request.IdPaciente;
            pacientes.Nombres = request.Nombres;
            pacientes.Apellidos = request.Apellidos;
            pacientes.FechaNacimiento = request.FechaNacimiento;
            pacientes.Sexo = request.Sexo;
            pacientes.DUI = request.DUI;
            pacientes.Telefono = request.Telefono;
            pacientes.Correo = request.Correo;
            pacientes.Direccion = request.Direccion;
            pacientes.FechaNacimiento = request.FechaNacimiento;

            await _pacientesRepository.AddPaciente(pacientes);
            return true;

        }
    }
}
